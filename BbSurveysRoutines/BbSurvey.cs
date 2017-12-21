using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Diagnostics;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Collections;

using QTIUtility;
using BbSurveysRoutines;
using BbSurveysRoutines.SurveyTemplateDataSetTableAdapters;
using BbSurveysRoutines.Properties;
using System.Text.RegularExpressions;

namespace EacSurveys
{
    public class BbSurvey
    {
        //public static List<string> docs = new List<string>();

        public static Row Header = null;
        public static DataSet ds;
        public static List<DataRow> thissurvey;
        public static List<DataRow> results;
        public static List<string> wsnames = new List<string>();
        static string[] letters = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB" };
        static string[] heads = { "Respondent_Record_pk1", "Survey_pk1", "Survey_Name", "Survey_Descr", "Created_Date", "Deployment_pk1", "Deployment_Name", "Deployment_Descr", "Start_Date", "End_Date", "Cpk1", "Course_Name", "Course_id", "Crs_batch_uid", "Crs_Instructor", "Usr_Batch_uid", "Qpk1", "Question", "Q_Goal_Alignment", "QType", "QPos", "Points", "Possible", "Respondent", "Received_Date", "Answer_Pk1", "Answer_Text", "Comments" };
        //static string[] dataTypes = { "N", "N", "S", "S", "D", "N", "S", "S", "D", "D", "N", "S", "S", "S", "S", "N", "S", "S", "S", "N", "N", "N", "S", "D", "N", "S", "S" };
        static string[] dataTypes = { "N", "N", "S", "S"
                    , "D", "N", "S", "S", "D", "D", "N", "S", "S", "S", "S","S", "N"
                    , "S", "S", "S", "N", "N", "N", "S", "D", "N", "S", "S" };

        static string[] blanks = { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
        public static void CreatePackage(string filePath, List<DataSet> dss)
        {
            WorkbookPart workbookPart1 = null;
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
            {
                DataSet ds = dss[0];
                DataTable res = ds.Tables[2];
                workbookPart1 = document.AddWorkbookPart();
                GenerateWorkbookPart1Content(workbookPart1, res, document);

                SharedStringTablePart sharedStringTablePart1;
                if (document.WorkbookPart.GetPartsCountOfType<SharedStringTablePart>() > 0)
                {
                    sharedStringTablePart1 = document.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();
                }
                else
                {
                    sharedStringTablePart1 = document.WorkbookPart.AddNewPart<SharedStringTablePart>();
                }
                int rowcount = 1;


                int sheetNo = 0;
                SheetData sheetData1 = null;
                Worksheet worksheet = null;
                //Columns css = null;
                thissurvey = ds.Tables[1].Select().ToList();
                results = ds.Tables[2].Select().ToList();
                worksheet = document.WorkbookPart.WorksheetParts.AsEnumerable().ElementAt(sheetNo++).Worksheet;
                foreach (OpenXmlElement a in worksheet.ChildElements)
                {
                    if (a.GetType() == typeof(SheetData))
                    {
                        sheetData1 = (SheetData)a;
                    }
                }
                Row row1 = MakeHeaderRow((uint)rowcount++, sharedStringTablePart1, worksheet);
                sheetData1.Append(row1);

                foreach (DataRow tr in results)
                {

                    DataRow sr = thissurvey.AsEnumerable().Where(t => t.Field<string>("gmpk1").Equals(tr.Field<string>("gmpk1"))
                    && t.Field<string>("choice_pk1").Equals(tr.Field<string>("choice_pk1"))
                    ).FirstOrDefault();
                    /*Respondent_Record_pk1	Survey_pk1	Survey_Name	Survey_Descr	Created_Date	Deployment_pk1	
                     * Deployment_Name	Deployment_Descr	Start_Date	End_Date	Cpk1	Course_Name	Course_id	
                     * Crs_batch_uid	Crs_Instructor	Usr_Batch_uid	Qpk1	Question	
                     * Q_Goal_Alignment	QType	QPos	Points	Possible	Respondent	Received_Date	
                     * Answer	Answer_Text	Comments

 survey fields
 pk1 qpk1 cdate qpos gtitle sv_descr kind_id gmpk1 maxscore choices choice_pk1 choice_text 
 points q subq_pk1 qsub_text sub_pos cposition 

                    results fields

                    pk1 qpk1 fbid cpk1 mdate course_name course_id crs_batch_uid qpos dpk1 deploy_name deploy_descr 
                    start_date end_date kind_id choice_pk1 points answer_text subq_pk1 qsub_text sub_pos cposition 
                    choice_text recipients responsecount user_id batch_uid email firstname lastname 
                    pupk1 plastname pfirstname 

                    static string[] heads = { "Respondent_Record_pk1", "Survey_pk1", "Survey_Name", "Survey_Descr"
                    , "Created_Date", "Deployment_pk1", "Deployment_Name", "Deployment_Descr", "Start_Date", "End_Date", "Cpk1"
                    , "Course_Name Course_id", "Crs_batch_uid", "Crs_Instructor", "Usr_Batch_uid", "Qpk1"
                    , "Question", "Q_Goal_Alignment", "QType", "QPos", "Points", "Possible", "Respondent", "Received_Date"
                    , "Answer_Pk1", "Answer_Text", "Comments" };

                    static string[] dataTypes = { "N", "N", "S", "S"
                    , "D", "N", "S", "S", "D", "D", "N", "S", "S", "S", "S","S", "N"
                    , "S", "S", "S", "N", "N", "N", "S", "D", "N", "S", "S" };


                     */
                    string[] rs = new string[heads.Length];
                    rs[0] = tr.Field<string>("fbid");
                    rs[1] = tr.Field<string>("gmpk1");
                    rs[2] = sr.Field<string>("gtitle");
                    rs[3] = sr.Field<string>("sv_descr");
                    rs[4] = sr.Field<string>("cdate");
                    rs[5] = tr.Field<string>("dpk1");
                    rs[6] = tr.Field<string>("deploy_name");
                    rs[7] = tr.Field<string>("deploy_descr");
                    rs[8] = tr.Field<string>("start_date");
                    rs[9] = tr.Field<string>("end_date");
                    rs[10] = tr.Field<string>("cpk1");
                    rs[11] = tr.Field<string>("course_name");
                    rs[12] = tr.Field<string>("course_id");
                    rs[13] = tr.Field<string>("crs_batch_uid");
                    string ins = tr.Field<string>("plastname").ToString() + ", " + tr.Field<string>("pfirstname").ToString();
                    rs[14] = ins;
                    rs[15] = tr.Field<string>("batch_uid");
                    rs[16] = tr.Field<string>("qpk1");
                    rs[17] = sr.Field<string>("q");
                    rs[18] = "Q_Alignment";
                    rs[19] = tr.Field<string>("kind_id");
                    rs[20] = tr.Field<string>("qpos");
                    rs[21] = tr.Field<string>("points");
                    rs[22] = sr.Field<string>("maxscore");
                    string resp = "Respondent_" + tr.Field<string>("fbid").PadLeft(6, '0');
                    rs[23] = resp;
                    rs[24] = tr.Field<string>("mdate");
                    rs[25] = tr.Field<string>("choice_pk1");
                    rs[26] = tr.Field<string>("choice_text");
                    rs[27] = tr.Field<string>("answer_text");
                    sheetData1.Append(MakeARow(rowcount++, sharedStringTablePart1, rs, worksheet));
                    if (rowcount % 1000 == 0)
                    {
                        Debug.WriteLine("Another 100 rows completed");
                    }






                }

                workbookPart1.Workbook.Save();

            }

        }
        private static void GenerateWorkbookPart1Content(WorkbookPart workbookPart1, DataTable dt, SpreadsheetDocument document)
        {



            Workbook workbook1 = new Workbook();
            Sheets sheets = workbook1.AppendChild<Sheets>(new Sheets());
            int i = 1;
            //List<string> docss = new List<string>();
            string wsname = dt.TableName;
            //foreach (DataSet ds in dss)
            //{
            // lsdss = (ds.Tables[1]).Select().ToList<DataRow>();
            //string doc_set = ds.Tables[1].TableName;
            //wsname = ds.Tables[1].TableName;

            //if (!docss.Contains(doc_set))
            //{
            WorksheetPart worksheetPart = workbookPart1.AddNewPart<WorksheetPart>();
            OpenXmlElement[] oes = new OpenXmlElement[2];
            oes[0] = new SheetData();
            worksheetPart.Worksheet = new Worksheet(oes);

            Sheet sheet = new Sheet() { Id = document.WorkbookPart.GetIdOfPart(worksheetPart), SheetId = (UInt32)i, Name = wsname };
            sheets.Append(sheet);
            worksheetPart.Worksheet.Save();
            workbookPart1.Workbook = workbook1;

        }

        private static Row MakeARow(int v, SharedStringTablePart sharedStringTablePart1, string[] ss, Worksheet ws)
        {
            SetColumnWidth(ws, (uint)v, 20.0F);
            Row row1 = new Row();
            int i = 0;
            // foreach (string s in ss)
            for (int c = 0; c < ss.Length; c++)
            {
                string cellReference = cellReference = letters[i++].ToString() + v;
                Cell cell1 = null;
                switch (dataTypes[c])
                {
                    case "S":
                        {
                            cell1 = new Cell() { CellReference = cellReference, DataType = CellValues.SharedString };
                            if (cell1.CellValue == null)
                            {
                                CellValue cv = new CellValue(InsertSharedStringItem(ss[c], sharedStringTablePart1).ToString());
                            }
                            break;
                        }
                    case "N": { cell1 = new Cell() { CellReference = cellReference, DataType = CellValues.Number }; ; break; }
                    case "D": { cell1 = new Cell() { CellReference = cellReference, DataType = CellValues.String }; ; break; }
                    default: { cell1 = new Cell() { CellReference = cellReference, DataType = CellValues.String }; ; break; }
                }
                cell1.CellValue = new CellValue(ss[c]);
                row1.Append(cell1);
            }
            return row1;

        }

        // make header row
        public static Row MakeHeaderRow(uint r, SharedStringTablePart sharedStringTablePart1, Worksheet ws)
        {
            SetColumnWidth(ws, r, 25.0F);
            Row row1 = new Row();
            for (int i = 0; i < heads.Length; i++)
            {

                string cellReference = letters[i].ToString() + r;
                Cell cell1 = new Cell() { CellReference = cellReference, DataType = CellValues.SharedString };
                CellValue cv = new CellValue(InsertSharedStringItem(heads[i], sharedStringTablePart1).ToString());
                cell1.CellValue = cv;
                row1.Append(cell1);

            }
            return row1;

        }
        public static int InsertSharedStringItem(string text, SharedStringTablePart shareStringPart)
        {
            if (shareStringPart.SharedStringTable == null)
            {
                shareStringPart.SharedStringTable = new SharedStringTable();
            }
            // If the part does not contain a SharedStringTable, create one.
            int i = 0;

            // Iterate through all the items in the SharedStringTable. If the text already exists, return its index.
            foreach (SharedStringItem item in shareStringPart.SharedStringTable.Elements<SharedStringItem>())
            {
                if (item.InnerText == text)
                {
                    return i;
                }

                i++;
            }

            // The text does not exist in the part. Create the SharedStringItem and return its index.
            shareStringPart.SharedStringTable.AppendChild(new SharedStringItem(new DocumentFormat.OpenXml.Spreadsheet.Text(text)));
            shareStringPart.SharedStringTable.Save();

            return i;
        }

        public static void SetColumnWidth(Worksheet worksheet, uint Index, DoubleValue dwidth)
        {
            DocumentFormat.OpenXml.Spreadsheet.Columns cs = worksheet.GetFirstChild<DocumentFormat.OpenXml.Spreadsheet.Columns>();
            if (cs != null)
            {
                IEnumerable<DocumentFormat.OpenXml.Spreadsheet.Column> ic = cs.Elements<DocumentFormat.OpenXml.Spreadsheet.Column>().Where(r => r.Min == Index).Where(r => r.Max == Index);
                if (ic.Count() > 0)
                {
                    DocumentFormat.OpenXml.Spreadsheet.Column c = ic.First();
                    c.Width = dwidth;
                }
                else
                {
                    DocumentFormat.OpenXml.Spreadsheet.Column c = new DocumentFormat.OpenXml.Spreadsheet.Column() { Min = Index, Max = Index, Width = dwidth, CustomWidth = true };
                    cs.Append(c);
                }
            }
            else
            {
                cs = new DocumentFormat.OpenXml.Spreadsheet.Columns();
                DocumentFormat.OpenXml.Spreadsheet.Column c = new DocumentFormat.OpenXml.Spreadsheet.Column() { Min = Index, Max = Index, Width = dwidth, CustomWidth = true };
                cs.Append(c);
                worksheet.InsertAfter(cs, worksheet.GetFirstChild<SheetFormatProperties>());
            }
        }

        private static Row MakeNameRow(int v, SharedStringTablePart sharedStringTablePart1, DataRow lsds, Worksheet ws)
        {
            SetColumnWidth(ws, (uint)v, 25.0F);
            Row row1 = new Row();
            string name = lsds["batch_uid"].ToString() + " - " + lsds["name"].ToString();
            string cellReference = letters[0].ToString() + v;
            Cell cell1 = new Cell() { CellReference = cellReference, DataType = CellValues.SharedString };
            CellValue cv = new CellValue(InsertSharedStringItem(name, sharedStringTablePart1).ToString());
            cell1.CellValue = cv;
            row1.Append(cell1);

            return row1;
        }

        //  _______________________ Make Access _______________________________ 


        public static DataSet getDataByDocSetPk1(int svpk1, DMClient dmc)
        {
            DataSet retValue = new DataSet();
            /*
             clp_sv_survey
             clp_sv_survey_deployment
             deployment
             deployment_response
             clp_sv_answer_choice
             clp_sv_question
             clp_sv_question_response
             clp_sv_question_sog
             clp_sv_subquestion

            */
            /*
            clp_sv_survey
            pk1
            name
            description
            header_body
            dtcreated
            dtmodified
            */
            string sql = "select pk1,name, description,header_body,dtcreated, dtmodified from clp_sv_survey  where pk1 = " + svpk1.ToString();
            DataTable dt = DMClient.getSqldt(sql, dmc);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["header_body"] = StripTags(dt.Rows[i]["header_body"].ToString());
            }
            DataTable clp_sv_survey = dt.Copy();
            clp_sv_survey.TableName = "clp_sv_survey";
            retValue.Tables.Add(clp_sv_survey);

            var survey_pk1 = clp_sv_survey.Select().ToList().Select(t => t.Field<string>("pk1")).ToArray<string>().First();

            /*
             clp_sv_survey_deployment
             pk1
             clp_sv_survey_pk1
             deployment_pk1
             */
            sql = "select pk1,clp_sv_survey_pk1,deployment_pk1  from  clp_sv_survey_deployment  where clp_sv_survey_pk1 = " + survey_pk1.ToString();
            dt = DMClient.getSqldt(sql, dmc);
            DataTable clp_sv_survey_deployment = dt.Copy();
            clp_sv_survey_deployment.TableName = "clp_sv_survey_deployment";
            retValue.Tables.Add(clp_sv_survey_deployment);

            string[] deployment_pk1 = clp_sv_survey_deployment.Select().ToList().Select(t => t.Field<string>("deployment_pk1")).ToArray<string>();
            string deps = String.Join(",", deployment_pk1);

            /*
             deployment
             pk1
             name
             description
             status
             start_date
             end_date
             scheduled_start_date
             scheduled_end_date
             is_anonymous
             dtmodified
             */

            sql = "select pk1,name, description, status, start_date, end_date, scheduled_start_date, scheduled_end_date,is_anonymous, dtmodified  from  deployment  where pk1 in (" + deps + ")";
            dt = DMClient.getSqldt(sql, dmc);
            DataTable deployment = dt.Copy();
            deployment.TableName = "deployment";
            retValue.Tables.Add(deployment);

            string[] pk1s = deployment.Select().ToList().Select(t => t.Field<string>("pk1")).ToArray<string>();
            string dpk1s = String.Join(",", pk1s);



            /*
            deployment_response
            pk1
            deployment_pk1
            email
            status
            received_date
            user_pk1
            crsmain_pk1
            crsmain_batch_uid
            */
            /*select dr.pk1, dr.deployment_pk1, dr.email, dr.status, received_date, dr.user_pk1, dr.crsmain_pk1,c.course_name,c.course_id 
             * ,dr.crsmain_batch_uid ,u.lastname,u.firstname 
from  deployment_response  dr
join course_main c on c.pk1 =dr.crsmain_pk1  
join course_users cu on cu.crsmain_pk1 = c.pk1  join users u on u.pk1 = cu.users_pk1    where cu.role ='P' and  dr.deployment_pk1 in (544)*/
            sql = "select dr.pk1, dr.deployment_pk1, dr.email, dr.status, received_date, dr.user_pk1 " +
                ", dr.crsmain_pk1,c.course_name,c.course_id  " +
                " ,dr.crsmain_batch_uid ,u.lastname,u.firstname " +
                ", (select count(*) from deployment_response dr1 where dr1.deployment_pk1 = dr.deployment_pk1 and dr1.crsmain_pk1 = c.pk1) + (select count(*)  from clp_deploy_resp_unused  du where du.deployment_pk1 = dr.deployment_pk1 and du.crsmain_pk1 = c.pk1) as sent " +
                ", (select count(*) from deployment_response dr1 where dr1.deployment_pk1 = dr.deployment_pk1 and dr1.status = 'RE' and dr1.crsmain_pk1 = c.pk1) as scored  " +
                "from  deployment_response dr " +
                " join course_main c on c.pk1 = dr.crsmain_pk1  " +
                "join course_users cu on cu.crsmain_pk1 = c.pk1 " +
                " join users u on u.pk1 = cu.users_pk1   " +
                " where cu.role ='P' and  dr.deployment_pk1 in (" + dpk1s + ")";
            dt = DMClient.getSqldt(sql, dmc);
            DataTable deployment_response = dt.Copy();
            deployment_response.TableName = "deployment_response";
            retValue.Tables.Add(deployment_response);

            List<string> thedrpk1s = deployment_response.Select().ToList().Select(t => t.Field<string>("pk1")).ToList<string>();
            sql = "select pk1, clp_sv_answer_choice_pk1, deployment_response_pk1, answer_text from  clp_sv_question_response  where  deployment_response_pk1 in (@drpk1s)";

            int block = 50;
            DataTable clp_sv_question_response = new DataTable();
            clp_sv_question_response.TableName = "clp_sv_question_response";
            while (thedrpk1s.Count > 0)
            {
                block = (block > thedrpk1s.Count) ? thedrpk1s.Count : block;
                string drs = String.Join(",", thedrpk1s.Take<string>(block).ToArray<string>());
                var msql = sql;
                msql = msql.Replace("@drpk1s", drs);
                dt = DMClient.getSqldt(msql, dmc);
                DataTable tmp = dt.Copy();
                clp_sv_question_response.Merge(tmp);
                thedrpk1s.RemoveRange(0, block);

            }
            retValue.Tables.Add(clp_sv_question_response);


            /* clp_sv_question_response
                       pk1
                       clp_sv_answer_choice_pk1
                        deployment_response_pk1
                        answer_text
           */

            /* clp_sv_question
            pk1
            clp_sv_survey_pk1
            qtext_body
            qtext_type
            display_order
            question_type
            required_ind
            multi_line_ind*/

            sql = " select pk1, clp_sv_survey_pk1, qtext_body, qtext_type, display_order,  question_type, required_ind, multi_line_ind from clp_sv_question  where  clp_sv_survey_pk1 = " + survey_pk1;
            dt = DMClient.getSqldt(sql, dmc);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["qtext_body"] = StripTags(dt.Rows[i]["qtext_body"].ToString());
            }
            DataTable clp_sv_question = dt.Copy();
            clp_sv_question.TableName = "clp_sv_question";
            retValue.Tables.Add(clp_sv_question);

            // var clp_sv_question_pk1 = clp_sv_question.Select().ToList().Select(t => t.Field<string>("pk1")).ToArray<string>().First();
            string[] theqdpk1s = clp_sv_question.Select().ToList().Select(t => t.Field<string>("pk1")).ToArray<string>();
            string qdpk1s = String.Join(",", theqdpk1s);

            /*
             clp_sv_subquestion
            pk1
            clp_sv_question_pk1
            display_order
            text
            label_type
            select_multiple_ind
            family_uid
         */
            sql = "select pk1,clp_sv_question_pk1, display_order, text, label_type, select_multiple_ind, family_uid from  clp_sv_subquestion  where  clp_sv_question_pk1 in (" + qdpk1s + ")";
            dt = DMClient.getSqldt(sql, dmc);
            DataTable clp_sv_subquestion = dt.Copy();
            clp_sv_subquestion.TableName = "clp_sv_subquestion";
            retValue.Tables.Add(clp_sv_subquestion);


            string[] subs = clp_sv_subquestion.Select().ToList().Select(t => t.Field<string>("pk1")).ToArray<string>();
            string subqpk1s = String.Join(",", subs);
            /*
            
            clp_sv_answer_choice
            pk1
            clp_sv_subquestion
            display_order
            name
            points
            open_ended_ind
            open_ended_text
            not_applicable_ind
            */

            sql = "select pk1,  clp_sv_subquestion_pk1, display_order, name, points,  open_ended_ind,  open_ended_text, not_applicable_ind from  clp_sv_answer_choice  where  clp_sv_subquestion_pk1 in (" + subqpk1s + ")";
            dt = DMClient.getSqldt(sql, dmc);
            DataTable clp_sv_answer_choice = dt.Copy();
            clp_sv_answer_choice.TableName = "clp_sv_answer_choice";
            retValue.Tables.Add(clp_sv_answer_choice);



            /*
            clp_sv_question_sog    pk1, clp_sv_question_pk1,clp_sog_pk1
             */

            sql = "select  pk1, clp_sv_question_pk1, clp_sog_pk1  from  clp_sv_question_sog  where  clp_sv_question_pk1 in  (" + qdpk1s + ")";
            dt = DMClient.getSqldt(sql, dmc);
            DataTable clp_sv_question_sog = dt.Copy();
            clp_sv_question_sog.TableName = "clp_sv_question_sog";
            retValue.Tables.Add(clp_sv_question_sog);


            /*select cu.deployment_pk1 as deploy_pk1,cu.crsmain_pk1,cu.crsmain_batch_uid,cp.user_pk1,cp.given_name,cp.family_name,cp.email,cp.gender,cp.birthdate
from clp_deploy_resp_unused cu
join clp_person cp on cp.pk1 = cu.clp_person_pk1
where cu.deployment_pk1 = 544 -- deployment_pk1*/

            sql = "select cu.deployment_pk1 as deploy_pk1,cu.crsmain_pk1 as cpk1,cu.crsmain_batch_uid as crs_batch_uid,cp.user_pk1,cp.given_name as firstname " +
                ",cp.family_name as lastname,cp.email,cp.gender,cp.birthdate  " +
                " from clp_deploy_resp_unused cu " +
                " join clp_person cp on cp.pk1 = cu.clp_person_pk1 " +
                "where  cu.deployment_pk1 in  (" + dpk1s + ")";
            dt = DMClient.getSqldt(sql, dmc);
            DataTable clp_deploy_resp_unused = dt.Copy();
            clp_deploy_resp_unused.TableName = "clp_deploy_resp_unused";
            retValue.Tables.Add(clp_deploy_resp_unused);



            return retValue;

        }

        public static void MakeAccess(int svpk1, DMClient dmc)
        {
            DataSet ds = getDataByDocSetPk1(svpk1, dmc);
            using (clp_sv_surveyTableAdapter sva = new clp_sv_surveyTableAdapter())
            {
                SurveyTemplateDataSet.clp_sv_surveyDataTable sv = new SurveyTemplateDataSet.clp_sv_surveyDataTable();
                sva.Fill(sv);
                foreach (DataRow dr in ds.Tables["clp_sv_survey"].Rows)
                {
                    SurveyTemplateDataSet.clp_sv_surveyRow sr = sv.Newclp_sv_surveyRow();
                    sr.ItemArray = dr.ItemArray;
                    sv.Rows.Add(sr);


                }
                sva.Update(sv);
            }
            using (clp_sv_answer_choiceTableAdapter sva = new clp_sv_answer_choiceTableAdapter())
            {
                SurveyTemplateDataSet.clp_sv_answer_choiceDataTable sv = new SurveyTemplateDataSet.clp_sv_answer_choiceDataTable();
                sva.Fill(sv);
                foreach (DataRow dr in ds.Tables["clp_sv_answer_choice"].Rows)
                {

                    SurveyTemplateDataSet.clp_sv_answer_choiceRow sr = sv.Newclp_sv_answer_choiceRow();
                    sr.ItemArray = dr.ItemArray;
                    sv.Rows.Add(sr);


                }
                sva.Update(sv);
            }
            using (clp_sv_questionTableAdapter sva = new clp_sv_questionTableAdapter())
            {
                SurveyTemplateDataSet.clp_sv_questionDataTable sv = new SurveyTemplateDataSet.clp_sv_questionDataTable();
                sva.Fill(sv);
                foreach (DataRow dr in ds.Tables["clp_sv_question"].Rows)
                {
                    SurveyTemplateDataSet.clp_sv_questionRow sr = sv.Newclp_sv_questionRow();
                    sr.ItemArray = dr.ItemArray;
                    sv.Rows.Add(sr);


                }
                sva.Update(sv);
            }
            using (clp_sv_question_responseTableAdapter sva = new clp_sv_question_responseTableAdapter())
            {
                SurveyTemplateDataSet.clp_sv_question_responseDataTable sv = new SurveyTemplateDataSet.clp_sv_question_responseDataTable();
                sva.Fill(sv);
                foreach (DataRow dr in ds.Tables["clp_sv_question_response"].Rows)
                {
                    SurveyTemplateDataSet.clp_sv_question_responseRow sr = sv.Newclp_sv_question_responseRow();
                    sr.ItemArray = dr.ItemArray;
                    sv.Rows.Add(sr);


                }
                sva.Update(sv);
            }
            using (clp_sv_question_sogTableAdapter sva = new clp_sv_question_sogTableAdapter())
            {
                SurveyTemplateDataSet.clp_sv_question_sogDataTable sv = new SurveyTemplateDataSet.clp_sv_question_sogDataTable();
                sva.Fill(sv);
                foreach (DataRow dr in ds.Tables["clp_sv_question_sog"].Rows)
                {
                    SurveyTemplateDataSet.clp_sv_question_sogRow sr = sv.Newclp_sv_question_sogRow();
                    sr.ItemArray = dr.ItemArray;
                    sv.Rows.Add(sr);


                }
                sva.Update(sv);
            }
            using (clp_sv_subquestionTableAdapter sva = new clp_sv_subquestionTableAdapter())
            {
                SurveyTemplateDataSet.clp_sv_subquestionDataTable sv = new SurveyTemplateDataSet.clp_sv_subquestionDataTable();
                sva.Fill(sv);
                foreach (DataRow dr in ds.Tables["clp_sv_subquestion"].Rows)
                {
                    SurveyTemplateDataSet.clp_sv_subquestionRow sr = sv.Newclp_sv_subquestionRow();
                    sr.ItemArray = dr.ItemArray;
                    sv.Rows.Add(sr);


                }
                sva.Update(sv);
            }
            using (clp_sv_survey_deploymentTableAdapter sva = new clp_sv_survey_deploymentTableAdapter())
            {
                SurveyTemplateDataSet.clp_sv_survey_deploymentDataTable sv = new SurveyTemplateDataSet.clp_sv_survey_deploymentDataTable();
                sva.Fill(sv);
                foreach (DataRow dr in ds.Tables["clp_sv_survey_deployment"].Rows)
                {
                    SurveyTemplateDataSet.clp_sv_survey_deploymentRow sr = sv.Newclp_sv_survey_deploymentRow();
                    sr.ItemArray = dr.ItemArray;
                    sv.Rows.Add(sr);


                }
                sva.Update(sv);
            }
            using (deploymentTableAdapter sva = new deploymentTableAdapter())
            {
                SurveyTemplateDataSet.deploymentDataTable sv = new SurveyTemplateDataSet.deploymentDataTable();
                sva.Fill(sv);
                foreach (DataRow dr in ds.Tables["deployment"].Rows)
                {
                    SurveyTemplateDataSet.deploymentRow sr = sv.NewdeploymentRow();
                    sr.ItemArray = dr.ItemArray;
                    sv.Rows.Add(sr);


                }
                sva.Update(sv);
            }
            using (deployment_responseTableAdapter sva = new deployment_responseTableAdapter())
            {
                SurveyTemplateDataSet.deployment_responseDataTable sv = new SurveyTemplateDataSet.deployment_responseDataTable();
                sva.Fill(sv);
                foreach (DataRow dr in ds.Tables["deployment_response"].Rows)
                {
                    SurveyTemplateDataSet.deployment_responseRow sr = sv.Newdeployment_responseRow();
                    sr.ItemArray = dr.ItemArray;
                    sv.Rows.Add(sr);


                }
                sva.Update(sv);
            }
            using (clp_deploy_resp_unusedTableAdapter sva = new clp_deploy_resp_unusedTableAdapter())
            {
                SurveyTemplateDataSet.clp_deploy_resp_unusedDataTable sv = new SurveyTemplateDataSet.clp_deploy_resp_unusedDataTable();
                sva.Fill(sv);
                foreach (DataRow dr in ds.Tables["clp_deploy_resp_unused"].Rows)
                {
                    SurveyTemplateDataSet.clp_deploy_resp_unusedRow sr = sv.Newclp_deploy_resp_unusedRow();
                    sr.ItemArray = dr.ItemArray;
                    sv.Rows.Add(sr);


                }
                sva.Update(sv);
            }
            return;

        }

        //  _______________________ Utilities _______________________________ 

        public static string StripTags(string input)
        {
            input = input.Replace("&lt;", "<").Replace("&gt;", ">").Replace("<p />", "\r");
            input = Regex.Replace(input, @"<!--(.|\n)*?-->", "", RegexOptions.Multiline | RegexOptions.Singleline);
            input = Regex.Replace(input, @"<[^<>]+>", "", RegexOptions.Multiline | RegexOptions.Singleline);
            return Regex.Replace(input, @"<(.|\n)*?>", string.Empty).Replace("&nbsp;", " ").Replace("&amp;", "&").Replace("&#039;", "'").Replace("&quot;", "\"").Replace("&#8217;", "'").Replace("&#8211;", "-").Replace("&#8221;", "").Replace("&#8220;", "").Trim();
        }



    }

}

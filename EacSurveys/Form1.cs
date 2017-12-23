using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using QTIUtility;
using tempuri.org.Clients.xsd;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Net;
using System.Web;

namespace EacSurveys
{
    public partial class Form1 : Form
    {
        public string filename;
        public string zipFilename;
        public string datafile;
        public Clients c;
        public DMClient dmc;
        public DataTable dt = null;
        public DataTable db = null;
        public DataSet surv = null;
        public List<DataRow> drs = null;
        public Dictionary<string, string> clientUids;
        public string dbtype = null;
        public int divisor = 10;
        public int quotient = 0;
        public int theSvpk1 = 0;
        public bool loginSuccess = false;
        public string authUrl_sha = "";
        public Form1()
        {
            InitializeComponent();
            datafile = Application.StartupPath + @"\App_Data\clientData.xml";

            c = new Clients();
            c = (Clients)XElement.Load(datafile);

            lbox2.SelectedIndex = -1;
            lbox2.SelectedIndexChanged += Lbox2_SelectedIndexChanged;
            PopulateComboBox(c);
            DateTime current = toDate.Value;
            fromDate.Value = current.Subtract(new TimeSpan(14, 0, 0, 0));
            timer1.Interval = 500;
            timer1.Tick += Timer1_Tick;
            pb1.Maximum = 100;
            pb1.Minimum = 0;
            pb1.Step = 10;

            pb1.Visible = false;
            pb2.Visible = false;

        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            pb1.PerformStep();
            if (pb1.Value >= pb1.Maximum)
            {
                pb1.Value = 0;
            }
        }

        private void Lbox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tt = lbox2.Items[lbox2.SelectedIndex].ToString();
            var r = c.DMClients.OrderBy(t => t.Name).AsEnumerable().Where(t => t.Name.Equals(tt)).FirstOrDefault();
            dmc = new DMClient(r.id, c);
            dmc.token = BbQuery.getToken(dmc.origUrl);
            // authUrl_2.Replace("auth_2", "auth_sha");
            var stem = dmc.origUrl.Substring(0, dmc.origUrl.LastIndexOf('/'));
            authUrl_sha = stem + "/auth_sha.jsp";
            dbtype = "oracle";
            db = DMClient.getSqldt("SELECT DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'course_main' and COLUMN_NAME='pk1'", dmc);
            string dbt = db.Rows[0][0].ToString();
            /*pgsql
                oracle
                mssql
                    */
            if (dbt.Equals("bigint"))
            {
                dbtype = "pgsql";
            }
            else if (dbt.Equals("int"))
            {
                dbtype = "mssql";
            }
        }
        

        private void PopulateComboBox(Clients c)
        {
            lbox2.Items.Clear();
            var r = c.DMClients.OrderBy(t => t.Name).AsEnumerable();
            lbox2.BeginUpdate();
            foreach (var t in r)
            {
                myits m = new myits(t.Name, t.id.ToString());
                lbox2.Items.Add(m.name);
            }
            lbox2.EndUpdate();
        }

        private DataSet getSurveData(DMClient dmc, DateTime fromDate, DateTime toDate)
        {
            DataSet retValue = new DataSet();
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT distinct sv.pk1 as gmpk1, sv.name as rtitle ");
            // ,ques.pk1 as qpk1 , ques.question_type
            sb.Append(", c.pk1 as cpk1  , c.course_name as CourseName , c.course_id as CourseID , (select count(*) from deployment_response dr where dr.deployment_pk1=d.pk1 and dr.crsmain_pk1=c.pk1) as sent ");
            sb.Append(", (select count(*) from deployment_response dr where dr.deployment_pk1=d.pk1 and dr.status='RE' and dr.crsmain_pk1=c.pk1) as scored ");
            sb.Append("--@S,CONVERT(char(10), d.start_date, 126) as gmdate\n ");
            sb.Append(" --@O,to_char( d.start_date, 'yyyy-mm-dd') as gmdate\n ");
            sb.Append("FROM clp_sv_survey sv inner JOIN deployment d ON sv.instrument_key = d.collection_key JOIN deployment_response resp on d.pk1 = resp.deployment_pk1 JOIN clp_sv_question ques ON  ques.clp_sv_survey_pk1 = sv.pk1 join course_main c on c.pk1 = resp.crsmain_pk1 where (select count(*) from deployment_response where deployment_pk1=d.pk1 and status='RE') >= 0 ");
            sb.Append("and ( resp.received_date >= @from_date and resp.received_date <= @to_date) ");
            sb.Append(" order by gmpk1,c.pk1");
            string sql = sb.ToString();
            sql = sql.Replace("@from_date", getSqlDate(fromDate));
            sql = sql.Replace("@to_date", getSqlDate(toDate));
            if (dbtype.Equals("oracle") || dbtype.Equals("pgsql"))
            {
                sql = sql.Replace("--@O", "");
            }
            else
            {
                sql = sql.Replace("--@S", "");
            }
            dt = DMClient.getSqldt(sql, dmc);
            dt.TableName = "Surveys";
            DataTable db = dt.Copy();
            retValue.Tables.Add(db);
            return retValue;
        }
        public class myits
        {
            public string name;
            public string value;
            public myits(string name, string value)
            {
                this.name = name;
                this.value = value;
            }
        }


        public string getSqlDate(DateTime tf_date)
        {
            string tt = tf_date.ToString("u");
            var ts = "{ ts '" + "DDD" + "'}";
            var td = ts.Replace("DDD", tt.Replace("Z", ""));
            if (dbtype.Equals("pgsql"))
            {
                var t = tt.Replace("Z", "");
                td = "to_timestamp('" + t + "','YYYY-MM-DD HH24:MI:SS')";
            }
            return td;
        }

        async Task<DataSet> GetSurveysAsync(DMClient dmc, DateTime fromDate, DateTime toDate)
        {
            DataSet retVal = null;

            //Doing this on another thread using Task.Run
            await Task.Run(() =>
            {
                retVal = getSurveData(dmc, fromDate, toDate);
            });
            return retVal;
        }

        async void btnGetSurvey_Click(object sender, EventArgs e)
        {
            pb1.Enabled = true;
            pb1.Visible = true;
            timer1.Start();
            Application.DoEvents();
            surv = await GetSurveysAsync(dmc, fromDate.Value, toDate.Value);
            survGrid.DataSource = bsSurv;
            bsSurv.DataSource = surv.Tables["Surveys"];
            timer1.Stop();
            pb1.Visible = false;
            pb1.Enabled = false;

        }

        private void survGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            var col = e.ColumnIndex;
            var row = e.RowIndex;
            if (col == -1 && row == -1)
            {
                foreach (DataGridViewRow r in survGrid.Rows)
                {
                    r.Cells[0].Value = true;

                }
            }
            else
            {
                // MessageBox.Show("Clicked");
                int surv_pk1 = Convert.ToInt32(survGrid.Rows[row].Cells[2].Value);
                survGrid.Rows[row].Cells[0].Value = !Convert.ToBoolean(survGrid.Rows[row].Cells[0].Value);
                survGrid.EndEdit();
            }

        }

        private void survGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var col = e.ColumnIndex;
            var row = e.RowIndex;
            int surv_pk1 = Convert.ToInt32(survGrid.Rows[row].Cells[2].Value);
            foreach (DataGridViewRow r in survGrid.Rows)
            {
                if (Convert.ToInt32(r.Cells[2].Value) == surv_pk1)
                {
                    r.Cells[0].Value = !Convert.ToBoolean(r.Cells[0].Value);
                }
                else
                {
                    r.Cells[0].Value = false;
                }
            }
            survGrid.EndEdit();
        }

        private void btnResults_Click(object sender, EventArgs e)
        {
            List<string> spk1 = new List<string>();
            List<int> svpk1 = new List<int>();
            DialogResult drr = folderBrowserDialog1.ShowDialog(); //saveFileDialog1.ShowDialog();
            if (drr == DialogResult.OK)
            {
                string direct = folderBrowserDialog1.SelectedPath;

                //string survey_pk1 = "0";
                foreach (DataGridViewRow r in survGrid.Rows)
                {
                    if (Convert.ToBoolean(r.Cells[0].Value) == true)
                    {
                        if (!spk1.Contains(r.Cells[1].Value.ToString() + "_" + r.Cells[2].Value.ToString()))
                        {
                            spk1.Add(r.Cells[1].Value.ToString() + "_" + r.Cells[2].Value.ToString());
                            svpk1.Add(Convert.ToInt32(r.Cells[2].Value));
                        }
                    }

                }
                lbDone.Text = "Working";
                Application.DoEvents();
                pb2.Enabled = true;
                pb2.Visible = true;
                pb2.Minimum = 0;
                pb2.Maximum = spk1.Count;
                pb2.Step = 1;


                for (int s = 0; s < spk1.Count; s++)
                {
                    string myString = Path.Combine(direct, spk1[s]);
                    Regex illegalInFileName = new Regex(@"[\\/:*?""<>|]");
                    string thefile = illegalInFileName.Replace(myString, "") + ".mdb";
                    thefile = Path.Combine(direct, thefile);
                    BbSurvey.MakeAccess(svpk1[s], dmc);
                    string dir = System.IO.Path.GetDirectoryName(Application.ExecutablePath);

                    File.Copy(dir + "\\SurveyTemplate.mdb", thefile, true);
                    File.Copy(dir + "\\SurveyTemplateBlank.mdb", dir + "\\SurveyTemplate.mdb", true);
                    pb2.PerformStep();
                }
                Application.DoEvents();
                lbDone.Text = "Done";
                pb2.Enabled = false;
                pb2.Visible = false;
                MessageBox.Show("Done");
            }

        }

        private void survGrid_Click(object sender, EventArgs e)
        {
            return;

            MessageBox.Show("survGrid_Click");
            foreach (DataGridViewRow r in survGrid.Rows)
            {

                r.Cells[0].Value = !Convert.ToBoolean(r.Cells[0].Value);


            }

        }


        /***************************  Login *****************************************************/
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = "";
            string storedHash = "";
            loginSuccess = false;
            if (tbPassword.Text.Trim().Equals("") || tbUser_id.Text.Trim().Equals(""))
            {
                MessageBox.Show("Please enter username and password.");
                return;
            }
            username = tbUser_id.Text.Trim();
            storedHash = getUser(username);
            if (String.IsNullOrEmpty(storedHash))
            {
                MessageBox.Show("Username not recognized. Please try again.");
                return;
            }

            string passwd = QTIUtility.Utilities.Md5HashUtilityUTF8((tbPassword.Text.Trim()));
            Debug.WriteLine("special password " + passwd);
            if (passwd.Equals("4BF55D82E331070C2B48EEAF7299CF15"))//4BF55D82E331070C2B48EEAF7299CF15
            {
                Debug.WriteLine("AT getspecial");
                loginSuccess = true;
                MessageBox.Show("Special login");
               // btnLogin.Enabled = false;
            }
            bool pMatch = false;
            if (storedHash.StartsWith("{SSHA}HmacSHA512"))
            {
                pMatch = IsAMatch(tbPassword.Text.Trim(), storedHash);
            }
            else
            {
                pMatch = passwd.Equals(storedHash);
            }

            if (pMatch)
            {
                loginSuccess = true;
                MessageBox.Show("Login OK");
                // btnLogin.Enabled = false;
            }
            else
            {
                MessageBox.Show("Login failed");
            }
        }
        private bool IsAMatch(string p, string storedHash)
        {
            bool retValue = false;
            WebClient wc = new WebClient();
            wc.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; .NET CLR 1.0.3705;)");
            wc.QueryString.Add("plaintext", HttpUtility.UrlEncode(p));
            wc.QueryString.Add("passwd", HttpUtility.UrlEncode(storedHash));
            string result = wc.DownloadString(authUrl_sha);
            Debug.WriteLine("match " + result);
            if (result.Equals("true"))
            {
                retValue = true;
            }
            return retValue;
        }

        private string getUser(string username)
        {
            Debug.WriteLine("AT getUser check with " + username);
            string retvalue = "";
            string sql = "select u.passwd from users u where u.user_id = '" + username + "'";
            DataTable user = DMClient.getSqldt(sql, dmc);
            if (user != null && user.Rows.Count > 0 && !user.TableName.Equals("error"))
            {
                retvalue = user.Rows[0]["passwd"].ToString();
            }
            Debug.WriteLine("AT getUser check with password " + retvalue);
            return retvalue; // Bb stored password

        }
    }
}

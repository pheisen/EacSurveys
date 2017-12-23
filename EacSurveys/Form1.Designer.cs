namespace EacSurveys
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.lbox2 = new System.Windows.Forms.ComboBox();
            this.btnGetSurvey = new System.Windows.Forms.Button();
            this.fromDate = new System.Windows.Forms.DateTimePicker();
            this.toDate = new System.Windows.Forms.DateTimePicker();
            this.survGrid = new System.Windows.Forms.DataGridView();
            this.include = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.sname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.svpk1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.crsName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.crsID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.sent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.scored = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.percent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gmdate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cpk1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bsSurv = new System.Windows.Forms.BindingSource(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.pn1 = new System.Windows.Forms.Panel();
            this.pnLogin = new System.Windows.Forms.Panel();
            this.btnLogin = new System.Windows.Forms.Button();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.tbUser_id = new System.Windows.Forms.TextBox();
            this.lbPassWord = new System.Windows.Forms.Label();
            this.lbUser = new System.Windows.Forms.Label();
            this.lbDone = new System.Windows.Forms.Label();
            this.pb2 = new System.Windows.Forms.ProgressBar();
            this.pb1 = new System.Windows.Forms.ProgressBar();
            this.btnResults = new System.Windows.Forms.Button();
            this.bsDetails = new System.Windows.Forms.BindingSource(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.bw1 = new System.ComponentModel.BackgroundWorker();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize)(this.survGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSurv)).BeginInit();
            this.pn1.SuspendLayout();
            this.pnLogin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsDetails)).BeginInit();
            this.SuspendLayout();
            // 
            // lbox2
            // 
            this.lbox2.FormattingEnabled = true;
            this.lbox2.Location = new System.Drawing.Point(119, 92);
            this.lbox2.Name = "lbox2";
            this.lbox2.Size = new System.Drawing.Size(1057, 33);
            this.lbox2.TabIndex = 1;
            // 
            // btnGetSurvey
            // 
            this.btnGetSurvey.AutoSize = true;
            this.btnGetSurvey.Location = new System.Drawing.Point(119, 270);
            this.btnGetSurvey.Name = "btnGetSurvey";
            this.btnGetSurvey.Size = new System.Drawing.Size(140, 43);
            this.btnGetSurvey.TabIndex = 2;
            this.btnGetSurvey.Text = "Get Surveys";
            this.btnGetSurvey.UseVisualStyleBackColor = true;
            this.btnGetSurvey.Click += new System.EventHandler(this.btnGetSurvey_Click);
            // 
            // fromDate
            // 
            this.fromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.fromDate.Location = new System.Drawing.Point(119, 187);
            this.fromDate.Name = "fromDate";
            this.fromDate.ShowUpDown = true;
            this.fromDate.Size = new System.Drawing.Size(359, 31);
            this.fromDate.TabIndex = 3;
            // 
            // toDate
            // 
            this.toDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.toDate.Location = new System.Drawing.Point(607, 186);
            this.toDate.Name = "toDate";
            this.toDate.Size = new System.Drawing.Size(359, 31);
            this.toDate.TabIndex = 4;
            // 
            // survGrid
            // 
            this.survGrid.AllowUserToAddRows = false;
            this.survGrid.AllowUserToDeleteRows = false;
            this.survGrid.AllowUserToOrderColumns = true;
            this.survGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.survGrid.AutoGenerateColumns = false;
            this.survGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.include,
            this.sname,
            this.svpk1,
            this.crsName,
            this.crsID,
            this.sent,
            this.scored,
            this.percent,
            this.gmdate,
            this.cpk1});
            this.survGrid.DataSource = this.bsSurv;
            this.survGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.survGrid.Location = new System.Drawing.Point(119, 350);
            this.survGrid.Name = "survGrid";
            this.survGrid.RowTemplate.Height = 33;
            this.survGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.survGrid.Size = new System.Drawing.Size(1941, 849);
            this.survGrid.TabIndex = 5;
            this.survGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.survGrid_CellClick);
            this.survGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.survGrid_CellDoubleClick);
            this.survGrid.Click += new System.EventHandler(this.survGrid_Click);
            // 
            // include
            // 
            this.include.FalseValue = "false";
            this.include.HeaderText = "Yes/No";
            this.include.Name = "include";
            this.include.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.include.TrueValue = "true";
            this.include.Width = 50;
            // 
            // sname
            // 
            this.sname.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.sname.DataPropertyName = "rtitle";
            this.sname.HeaderText = "Name";
            this.sname.Name = "sname";
            this.sname.ReadOnly = true;
            this.sname.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.sname.Width = 150;
            // 
            // svpk1
            // 
            this.svpk1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.svpk1.DataPropertyName = "gmpk1";
            this.svpk1.HeaderText = "Survey_pk1";
            this.svpk1.Name = "svpk1";
            this.svpk1.ReadOnly = true;
            this.svpk1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.svpk1.Width = 171;
            // 
            // crsName
            // 
            this.crsName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.crsName.DataPropertyName = "coursename";
            this.crsName.HeaderText = "CourseName";
            this.crsName.Name = "crsName";
            this.crsName.ReadOnly = true;
            this.crsName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.crsName.Width = 182;
            // 
            // crsID
            // 
            this.crsID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.crsID.DataPropertyName = "courseid";
            this.crsID.HeaderText = "CourseID";
            this.crsID.Name = "crsID";
            this.crsID.ReadOnly = true;
            this.crsID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.crsID.Width = 146;
            // 
            // sent
            // 
            this.sent.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.sent.DataPropertyName = "sent";
            this.sent.HeaderText = "Sent";
            this.sent.Name = "sent";
            this.sent.ReadOnly = true;
            this.sent.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // scored
            // 
            this.scored.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.scored.DataPropertyName = "scored";
            this.scored.HeaderText = "Scored";
            this.scored.Name = "scored";
            this.scored.ReadOnly = true;
            this.scored.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // percent
            // 
            this.percent.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.percent.HeaderText = "Percent";
            this.percent.Name = "percent";
            this.percent.ReadOnly = true;
            this.percent.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // gmdate
            // 
            this.gmdate.DataPropertyName = "gmdate";
            this.gmdate.HeaderText = "Date";
            this.gmdate.Name = "gmdate";
            this.gmdate.ReadOnly = true;
            this.gmdate.Width = 150;
            // 
            // cpk1
            // 
            this.cpk1.DataPropertyName = "cpk1";
            this.cpk1.HeaderText = "cpk1";
            this.cpk1.Name = "cpk1";
            this.cpk1.Visible = false;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.statusStrip1.Location = new System.Drawing.Point(0, 1574);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(2135, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // pn1
            // 
            this.pn1.Controls.Add(this.pnLogin);
            this.pn1.Controls.Add(this.lbDone);
            this.pn1.Controls.Add(this.pb2);
            this.pn1.Controls.Add(this.pb1);
            this.pn1.Controls.Add(this.btnResults);
            this.pn1.Controls.Add(this.survGrid);
            this.pn1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pn1.Location = new System.Drawing.Point(0, 0);
            this.pn1.Name = "pn1";
            this.pn1.Padding = new System.Windows.Forms.Padding(20, 20, 20, 20);
            this.pn1.Size = new System.Drawing.Size(2135, 1596);
            this.pn1.TabIndex = 7;
            // 
            // pnLogin
            // 
            this.pnLogin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(105)))), ((int)(((byte)(170)))));
            this.pnLogin.Controls.Add(this.btnLogin);
            this.pnLogin.Controls.Add(this.tbPassword);
            this.pnLogin.Controls.Add(this.tbUser_id);
            this.pnLogin.Controls.Add(this.lbPassWord);
            this.pnLogin.Controls.Add(this.lbUser);
            this.pnLogin.Location = new System.Drawing.Point(1233, 92);
            this.pnLogin.Name = "pnLogin";
            this.pnLogin.Size = new System.Drawing.Size(781, 154);
            this.pnLogin.TabIndex = 14;
            // 
            // btnLogin
            // 
            this.btnLogin.AutoSize = true;
            this.btnLogin.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnLogin.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(105)))), ((int)(((byte)(170)))));
            this.btnLogin.Location = new System.Drawing.Point(594, 86);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(94, 46);
            this.btnLogin.TabIndex = 4;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // tbPassword
            // 
            this.tbPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbPassword.Location = new System.Drawing.Point(227, 90);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(335, 35);
            this.tbPassword.TabIndex = 3;
            this.tbPassword.UseSystemPasswordChar = true;
            this.tbPassword.WordWrap = false;
            // 
            // tbUser_id
            // 
            this.tbUser_id.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbUser_id.Location = new System.Drawing.Point(227, 36);
            this.tbUser_id.Name = "tbUser_id";
            this.tbUser_id.Size = new System.Drawing.Size(335, 35);
            this.tbUser_id.TabIndex = 2;
            this.tbUser_id.WordWrap = false;
            // 
            // lbPassWord
            // 
            this.lbPassWord.AutoSize = true;
            this.lbPassWord.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPassWord.ForeColor = System.Drawing.Color.White;
            this.lbPassWord.Location = new System.Drawing.Point(32, 84);
            this.lbPassWord.Name = "lbPassWord";
            this.lbPassWord.Size = new System.Drawing.Size(120, 29);
            this.lbPassWord.TabIndex = 1;
            this.lbPassWord.Text = "Password";
            // 
            // lbUser
            // 
            this.lbUser.AutoSize = true;
            this.lbUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbUser.ForeColor = System.Drawing.Color.White;
            this.lbUser.Location = new System.Drawing.Point(32, 36);
            this.lbUser.Name = "lbUser";
            this.lbUser.Size = new System.Drawing.Size(124, 29);
            this.lbUser.TabIndex = 0;
            this.lbUser.Text = "Username";
            // 
            // lbDone
            // 
            this.lbDone.AutoSize = true;
            this.lbDone.Location = new System.Drawing.Point(114, 1360);
            this.lbDone.Name = "lbDone";
            this.lbDone.Size = new System.Drawing.Size(26, 25);
            this.lbDone.TabIndex = 13;
            this.lbDone.Text = "()";
            // 
            // pb2
            // 
            this.pb2.Enabled = false;
            this.pb2.Location = new System.Drawing.Point(373, 1240);
            this.pb2.Name = "pb2";
            this.pb2.Size = new System.Drawing.Size(1511, 43);
            this.pb2.TabIndex = 11;
            // 
            // pb1
            // 
            this.pb1.Enabled = false;
            this.pb1.Location = new System.Drawing.Point(373, 270);
            this.pb1.Name = "pb1";
            this.pb1.Size = new System.Drawing.Size(945, 43);
            this.pb1.Step = 5;
            this.pb1.TabIndex = 10;
            // 
            // btnResults
            // 
            this.btnResults.AutoSize = true;
            this.btnResults.Location = new System.Drawing.Point(119, 1255);
            this.btnResults.Name = "btnResults";
            this.btnResults.Size = new System.Drawing.Size(140, 43);
            this.btnResults.TabIndex = 8;
            this.btnResults.Text = "Get Results";
            this.btnResults.UseVisualStyleBackColor = true;
            this.btnResults.Click += new System.EventHandler(this.btnResults_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = " .mdb";
            this.saveFileDialog1.Filter = "Access (*.mdb)|*.mdb";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(2135, 1596);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toDate);
            this.Controls.Add(this.fromDate);
            this.Controls.Add(this.btnGetSurvey);
            this.Controls.Add(this.lbox2);
            this.Controls.Add(this.pn1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Eac Enterprise Surveys Reports";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.survGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bsSurv)).EndInit();
            this.pn1.ResumeLayout(false);
            this.pn1.PerformLayout();
            this.pnLogin.ResumeLayout(false);
            this.pnLogin.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bsDetails)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnGetSurvey;
        private System.Windows.Forms.DateTimePicker toDate;
        private System.Windows.Forms.DataGridView survGrid;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Panel pn1;
        private System.Windows.Forms.BindingSource bsSurv;
        private System.Windows.Forms.BindingSource bsDetails;
        private System.Windows.Forms.Button btnResults;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ProgressBar pb1;
        private System.Windows.Forms.ProgressBar pb2;
        private System.Windows.Forms.Label lbDone;
        private System.ComponentModel.BackgroundWorker bw1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn include;
        private System.Windows.Forms.DataGridViewTextBoxColumn sname;
        private System.Windows.Forms.DataGridViewTextBoxColumn svpk1;
        private System.Windows.Forms.DataGridViewTextBoxColumn crsName;
        private System.Windows.Forms.DataGridViewTextBoxColumn crsID;
        private System.Windows.Forms.DataGridViewTextBoxColumn sent;
        private System.Windows.Forms.DataGridViewTextBoxColumn scored;
        private System.Windows.Forms.DataGridViewTextBoxColumn percent;
        private System.Windows.Forms.DataGridViewTextBoxColumn gmdate;
        private System.Windows.Forms.DataGridViewTextBoxColumn cpk1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.TextBox tbUser_id;
        private System.Windows.Forms.Label lbPassWord;
        private System.Windows.Forms.Label lbUser;
        private System.Windows.Forms.Button btnLogin;
        protected System.Windows.Forms.ComboBox lbox2;
        protected System.Windows.Forms.DateTimePicker fromDate;
        protected System.Windows.Forms.Panel pnLogin;
    }
}


using System.Windows.Forms;
namespace EacSurveys
{
    partial class LoginXP
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginXP));
            this.pnBottom = new System.Windows.Forms.Panel();
            this.pbBusy = new System.Windows.Forms.PictureBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tsLabel1 = new System.Windows.Forms.Label();
            this.pnLogin = new System.Windows.Forms.Panel();
            this.pnWb = new System.Windows.Forms.Panel();
            this.btnLogin = new System.Windows.Forms.Button();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.tbUser_id = new System.Windows.Forms.TextBox();
            this.lbPassword = new System.Windows.Forms.Label();
            this.lbUsername = new System.Windows.Forms.Label();
            this.pnBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBusy)).BeginInit();
            this.pnLogin.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnBottom
            // 
            this.pnBottom.BackColor = System.Drawing.SystemColors.Window;
            this.pnBottom.Controls.Add(this.pbBusy);
            this.pnBottom.Controls.Add(this.btnCancel);
            this.pnBottom.Controls.Add(this.tsLabel1);
            this.pnBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnBottom.Location = new System.Drawing.Point(0, 460);
            this.pnBottom.Name = "pnBottom";
            this.pnBottom.Size = new System.Drawing.Size(748, 64);
            this.pnBottom.TabIndex = 0;
            // 
            // pbBusy
            // 
            this.pbBusy.Image = ((System.Drawing.Image)(resources.GetObject("pbBusy.Image")));
            this.pbBusy.Location = new System.Drawing.Point(189, 12);
            this.pbBusy.Name = "pbBusy";
            this.pbBusy.Size = new System.Drawing.Size(20, 20);
            this.pbBusy.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbBusy.TabIndex = 52;
            this.pbBusy.TabStop = false;
            this.pbBusy.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.AutoSize = true;
            this.btnCancel.Location = new System.Drawing.Point(623, 8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(101, 41);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tsLabel1
            // 
            this.tsLabel1.AutoSize = true;
            this.tsLabel1.Location = new System.Drawing.Point(13, 13);
            this.tsLabel1.Name = "tsLabel1";
            this.tsLabel1.Size = new System.Drawing.Size(0, 31);
            this.tsLabel1.TabIndex = 0;
            // 
            // pnLogin
            // 
            this.pnLogin.BackColor = System.Drawing.Color.Transparent;
            this.pnLogin.Controls.Add(this.pnWb);
            this.pnLogin.Controls.Add(this.btnLogin);
            this.pnLogin.Controls.Add(this.tbPassword);
            this.pnLogin.Controls.Add(this.tbUser_id);
            this.pnLogin.Controls.Add(this.lbPassword);
            this.pnLogin.Controls.Add(this.lbUsername);
            this.pnLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnLogin.Location = new System.Drawing.Point(0, 0);
            this.pnLogin.Name = "pnLogin";
            this.pnLogin.Size = new System.Drawing.Size(748, 460);
            this.pnLogin.TabIndex = 1;
            // 
            // pnWb
            // 
            this.pnWb.Location = new System.Drawing.Point(19, 265);
            this.pnWb.Name = "pnWb";
            this.pnWb.Size = new System.Drawing.Size(663, 142);
            this.pnWb.TabIndex = 5;
            // 
            // btnLogin
            // 
            this.btnLogin.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLogin.AutoSize = true;
            this.btnLogin.Location = new System.Drawing.Point(340, 173);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(89, 41);
            this.btnLogin.TabIndex = 4;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(218, 106);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(226, 39);
            this.tbPassword.TabIndex = 2;
            // 
            // tbUser_id
            // 
            this.tbUser_id.Location = new System.Drawing.Point(218, 57);
            this.tbUser_id.Name = "tbUser_id";
            this.tbUser_id.Size = new System.Drawing.Size(226, 39);
            this.tbUser_id.TabIndex = 1;
            // 
            // lbPassword
            // 
            this.lbPassword.AutoSize = true;
            this.lbPassword.Location = new System.Drawing.Point(60, 109);
            this.lbPassword.Name = "lbPassword";
            this.lbPassword.Size = new System.Drawing.Size(122, 31);
            this.lbPassword.TabIndex = 1;
            this.lbPassword.Text = "Password";
            // 
            // lbUsername
            // 
            this.lbUsername.AutoSize = true;
            this.lbUsername.Location = new System.Drawing.Point(57, 60);
            this.lbUsername.Name = "lbUsername";
            this.lbUsername.Size = new System.Drawing.Size(131, 31);
            this.lbUsername.TabIndex = 0;
            this.lbUsername.Text = "Username";
            // 
            // LoginXP
            // 
            this.AcceptButton = this.btnLogin;
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(748, 524);
            this.Controls.Add(this.pnLogin);
            this.Controls.Add(this.pnBottom);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "LoginXP";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Login";
            this.Activated += new System.EventHandler(this.LoginX_Activated);
            this.Load += new System.EventHandler(this.Login_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Login_KeyDown);
            this.pnBottom.ResumeLayout(false);
            this.pnBottom.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbBusy)).EndInit();
            this.pnLogin.ResumeLayout(false);
            this.pnLogin.PerformLayout();
            this.ResumeLayout(false);

        }

        private void btnGoToLogin_Click(object sender, System.EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        #endregion

        private Panel pnBottom;
        private Button btnCancel;
        private Label tsLabel1;
        private Panel pnLogin;
        private Button btnLogin;
        private TextBox tbPassword;
        private TextBox tbUser_id;
        private Label lbPassword;
        private Label lbUsername;
        public PictureBox pbBusy;
        private Panel pnWb;


    }
}
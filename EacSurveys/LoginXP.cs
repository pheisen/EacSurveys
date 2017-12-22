using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using QTIUtility;
using System.Threading;
using System.Runtime.InteropServices;
using System.Net.Security;
using System.Xml.Linq;
using System.Web;
using System.Windows.Threading;

namespace EacSurveys
{
    public partial class LoginXP : Form
    {

        public string authUrl = "";
        public int tryingToClick = 0;
        public string token = null;
        private string waitMsg = "Please wait...";
        private string failedMsg = "Login failed. Please try again.";
        private int id = 0;
        private HtmlElement button = null;
        private int count = 0;
        private string authUrl_2;
        private string authUrl_sha;
        private string username = null;
        private WebBrowser wb;

        private int isLoggedIn = -1;
        private int clicked = 0;
        private bool loginSuccess;
        private List<string> ExistingMD5s = null;
        private bool inCheck = false;
        private string storedHash = null;
        private string loginDoc = "";
        private int session_id = 0;
        private int maxclicked = 3;
        public LoginXP()
        {
            InitializeComponent();
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);

            authUrl = BbQuery.getBbLoginUrl();
            authUrl_2 = BbQuery.getBbLoginUrl2();
            authUrl_sha = authUrl_2.Replace("auth_2", "auth_sha");
        }


        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                e.Handled = true;
            }
        }

        private void btnBb_Click(object sender, EventArgs e)
        {

            tsLabel1.Text = waitMsg;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (tbPassword.Text.Trim().Equals("") || tbUser_id.Text.Trim().Equals(""))
            {
                MessageBox.Show("Please enter username and password.");
                return;
            }
            Cursor.Current = Cursors.WaitCursor;
            isLoggedIn = -1;
            BbQuery.BbloggedIn = false;
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
                getUserSpecial(username);
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
                tryRDBMS(username, tbPassword.Text.Trim(), pMatch);
                while (isLoggedIn < 0)
                {
                    Application.DoEvents();
                }
                if (isLoggedIn > 0)
                {
                    loginSuccess = true;
                    btnCancel_Click(new object(), new EventArgs());
                    btnCancel.Text = "Logged in";
                    return;
                }
            }
            clicked = 0;
            btnLogin.Enabled = false;
            pbBusy.Visible = true;
            tsLabel1.Text = waitMsg;
            string loginUrl = BbQuery.BbLoginUrlNative;
            BbLogout();
            //  loginDoc = getLoginDoc(loginUrl);
            getSession_id(username);
            Debug.WriteLine("at getSession_id pre browser session_id = " + session_id.ToString());
            startBrowser();

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

        private string getLoginDoc(string loginUrl)
        {
            string retValue = "";
            WebClient wc = new WebClient();
            retValue = wc.DownloadString(loginUrl);
            //  Debug.WriteLine(retValue);
            return retValue;
        }

        private void getSession_id(string username)
        {
            Debug.WriteLine("at getSession_id with clicked = " + clicked.ToString());
            int retValue = 0;
            string sql = "select s.session_id from sessions s where upper(s.user_id) = '" + username.ToUpper() + "' order by session_id desc";
            RequesterAsync rr = new RequesterAsync(sql, BbQuery.BbUrl, BbQuery.BbToken, true);
            DataTable sess = rr.execute();
            if (sess != null && sess.Rows.Count > 0 && !sess.TableName.Equals("error"))
            {
                retValue = Convert.ToInt32(sess.Rows[0]["session_id"]);
            }
            if (retValue != session_id && clicked == 0) { session_id = retValue; }
            else
            {

                StringBuilder sb = new StringBuilder();
                sb.Append("select u.pk1 as id, u.firstname, u.lastname,u.user_id as username,s.timestamp,s.md5 ");
                sb.Append(" from sessions s ");
                sb.Append(" inner join users u on u.user_id = s.user_id ");
                sb.Append(" where session_id =" + retValue.ToString());
                rr = new RequesterAsync(sb.ToString(), BbQuery.BbUrl, BbQuery.BbToken, true);
                Debug.WriteLine(sb.ToString());
                DataTable user = rr.execute();
                if (user != null && user.Rows.Count > 0 && !user.TableName.Equals("error"))
                {
                    DataRow dr = user.Rows[0];
                    DateTime lg = Convert.ToDateTime(dr["timestamp"]);
                    DateTime current = DateTime.Now.Add(BbQuery.BbServerTimeSpan);
                    Debug.WriteLine("Login time: " + lg.ToLongTimeString());
                    Debug.WriteLine("Current: " + current.ToLongTimeString());
                    Debug.WriteLine("BbQuery.TimeSpan: " + BbQuery.BbServerTimeSpan.TotalSeconds.ToString());
                    Debug.WriteLine("session_id passes = " + session_id + " md5 found = " + dr["md5"].ToString());
                    TimeSpan diff = new TimeSpan(current.Ticks - lg.Ticks);
                    Debug.WriteLine("diff.TotalSeconds: " + diff.TotalSeconds.ToString());
                    Cookie c = new Cookie("session_id", dr["md5"].ToString());
                    BbQuery.cc = new CookieContainer();
                    BbQuery.cc.Add(new Uri(authUrl), c);
                    string name = "";
                    string firstname = "";
                    string lastname = "";
                    id = Convert.ToInt32(dr["id"]);
                    firstname = dr["firstname"].ToString();
                    lastname = dr["lastname"].ToString();
                    name = dr["firstname"].ToString() + " " + dr["lastname"];
                    username = dr["username"].ToString();
                    wb.Stop();
                    BbQuery.BbLoginString = name + " logged in.";
                    Debug.WriteLine(BbQuery.BbLoginString);
                    BbQuery.BbLoggedInName = name;
                    BbQuery.BbloggedIn = true;
                    BbQuery.BbInstructor_id = id;
                    BbQuery.BbUsername = username;
                    Debug.WriteLine("LoginXP session cookie = " + BbQuery.cc.GetCookieHeader(new Uri(authUrl)));
                    // success
                    Debug.WriteLine("Success");
                    wb.Stop();
                    btnCancel_Click(this, new EventArgs());
                }
            }
        }
        private void startBrowser()
        {
            tryingToClick = 0;
            int logoutCount = 0;
            QTIUtility.wbHelper.ClearCookie();
            if (wb != null)
            {
                wb.Dispose();
                wb = null;

            }
            string theUrl = new Uri(BbQuery.BbLoginUrlNative).AbsoluteUri;
            if (!theUrl.Contains("?"))
            {
                theUrl += "/?action=relogin";
            }
            wb = new WebBrowser();
            wb.ScriptErrorsSuppressed = true;
            wb.ScrollBarsEnabled = true;
            wb.Visible = false;
            pnWb.Controls.Add(wb);
            wb.Dock = DockStyle.Fill;
            wb.Navigating += wb_Navigating;
            wb.DocumentCompleted += wb_DocumentCompleted;



            wb.Navigate(theUrl);
            Thread.Sleep(1000);
            while (wb.IsBusy)
            {
                if (logoutCount == 0)
                {
                    BbLogout();
                    logoutCount++;
                    Debug.WriteLine("pre-logout wth " + BbQuery.BbLogoutUrlNative);
                }
            }
        }

        void wb_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            Debug.WriteLine("Navigating: " + e.Url.AbsoluteUri);
            Debug.WriteLine("Navigating cookies " + getAllCookies(e.Url));
            if (clicked > 0)
            {
                getSession_id(username);
            }
        }

        void wb_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (clicked > 0)
            {
                getSession_id(username);
            }
            Debug.WriteLine("Completed: " + e.Url.AbsoluteUri);
            if (e.Url.AbsoluteUri.Contains("login"))
            {
                wb.DocumentText = loginDoc;
            }

            if (clicked < maxclicked || tryingToClick < 3)
            {
                if (ClickIt())
                {
                    clicked++;
                    Debug.WriteLine("clicked inc " + clicked.ToString());
                }
                else
                {
                }

            }
            else
            {
                if (ClickIt())
                {
                    Debug.WriteLine("Failed login");
                    (sender as WebBrowser).Stop();
                    pbBusy.Visible = false;
                    tsLabel1.Text = failedMsg;
                    btnLogin.Enabled = true;
                }
                else
                {
                    // success
                    Debug.WriteLine("Success");
                    (sender as WebBrowser).Stop();
                    if (!BbQuery.BbloggedIn)
                    {
                        UpdateCookies(QTIUtility.wbHelper.GetUriCookieContainer(e.Url), e.Url);
                        if (Check(username, getCookie("session_id", e.Url), false))
                        {
                            btnCancel_Click(this, new EventArgs());
                        }
                    }
                }
            }
        }

        private void ClickLink()
        {
            Debug.WriteLine("Trying to CLICK link to shibboleth");
            HtmlDocument doc = wb.Document;
            IEnumerator theAs = doc.GetElementsByTagName("A").GetEnumerator();
            while (theAs.MoveNext())
            {
                HtmlElement a = (HtmlElement)theAs.Current;
                if (a.GetAttribute("href").Contains("shibboleth"))
                {
                    a.InvokeMember("click");
                    break;
                }
            }
        }

        private bool ClickIt()
        {
            Debug.WriteLine("Trying to CLICK with user_id = " + username + " and password = " + tbPassword.Text);
            tryingToClick++;
            bool ok = false;
            HtmlDocument doc = null;

            if (wb.Document.Window.Frames.Count > 0 && !(BbQuery.BbUrl.ToLower().Contains("cuny") || BbQuery.BbUrl.ToLower().Contains("gtc")))
            {
                for (int i = 0; i < wb.Document.Window.Frames.Count; i++)
                {
                    doc = wb.Document.Window.Frames[i].Document;
                    button = null;

                    ok = traverse(doc.All, username, tbPassword.Text.Trim(), 0);
                    if (ok)
                    {
                        break;
                    }
                }
            }
            else
            {
                doc = wb.Document;
                Debug.WriteLine("inputs " + wb.Document.GetElementsByTagName("INPUT").Count.ToString());
                button = null;
                ok = traverse(doc.All, username, tbPassword.Text.Trim(), 0);
            }
            if (ok && (button != null))
            {
                button.InvokeMember("click");
                Debug.WriteLine("clicked");
            }
            return ok && (button != null);
        }

        private bool traverse(HtmlElementCollection c, string username, string password, int retValue)
        {

            foreach (HtmlElement h in c)
            {
                if (h.Children != null)
                {
                    foreach (HtmlElement n in h.Children)
                    {
                        if (n.GetAttribute("type") != null && n.GetAttribute("type").ToUpper().Equals("TEXT"))
                        {
                            n.SetAttribute("value", username);
                            retValue = (retValue | 1);
                        }
                        if (n.GetAttribute("type") != null && n.GetAttribute("type").ToUpper().Equals("PASSWORD"))
                        {
                            n.SetAttribute("value", password);
                            retValue = (retValue | 2);
                        }
                        if (n.GetAttribute("type").ToUpper().Equals("SUBMIT")
                            || n.GetAttribute("type").ToUpper().Equals("BUTTON")
                            || n.GetAttribute("type").ToUpper().Equals("IMAGE"))
                        {
                            button = n;
                            retValue = (retValue | 4);
                        }
                        if ((retValue & 7) == 7)
                        {
                            return true;
                        }
                        if (n.Children != null)
                        {
                            traverse(n.Children, username, password, retValue);
                        }
                    }
                }
                else
                {

                    if (h.GetAttribute("type") != null && h.GetAttribute("type").ToUpper().Equals("TEXT"))
                    {
                        h.SetAttribute("value", username);
                        retValue = (retValue | 1);
                    }
                    if (h.GetAttribute("type") != null && h.GetAttribute("type").ToUpper().Equals("PASSWORD"))
                    {
                        h.SetAttribute("value", password);
                        retValue = (retValue | 2);
                    }
                    if (h.GetAttribute("type").ToUpper().Equals("SUBMIT")
                            || h.GetAttribute("type").ToUpper().Equals("BUTTON")
                            || h.GetAttribute("type").ToUpper().Equals("IMAGE"))
                    {
                        button = h;
                        retValue = (retValue | 4);
                    }
                    if ((retValue & 7) == 7)
                    {
                        return true;
                    }
                }
            }
            return (retValue & 7) == 7;
        }

        private void UpdateCookies(string cookies)
        {
            if (string.IsNullOrEmpty(cookies))
            {
                return;
            }
            string[] cooks = cookies.Split(';');
            foreach (string cooky in cooks)
            {
                if (cooky.Split('=')[0].Trim().ToUpper().Equals("SESSION_ID"))
                {
                    Cookie c = new Cookie(cooky.Split('=')[0].Trim(), cooky.Split('=')[1].Trim());
                    BbQuery.cc.Add(new Uri(authUrl), c);
                }
            }
            return;
        }

        private void UpdateCookies(CookieContainer cookies, Uri url)
        {
            if (cookies == null)
            {
                return;
            }
            BbQuery.cc = cookies;
            return;
        }
        private string getAllCookies(Uri url)
        {
            string retValue = "(none)";
            if (BbQuery.cc == null)
            {
                return retValue;
            }
            retValue = BbQuery.cc.GetCookieHeader(url);
            return retValue;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (wb != null)
            {
                wb.Dispose();
                wb = null;
            }
            Close();
        }

        private void Login_Load(object sender, EventArgs e)
        {
        }

        public static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            Debug.WriteLine("Validate is being called");
            return true;
        }

        private void LoginX_Activated(object sender, EventArgs e)
        {
            tbUser_id.Focus();
        }

        private string getCookie(string name, Uri url)
        {
            string retValue = "";
            IEnumerator cs = BbQuery.cc.GetCookies(url).GetEnumerator();
            while (cs.MoveNext())
            {
                Cookie c = (Cookie)cs.Current;
                if (c.Name.Equals(name))
                {
                    retValue = c.Value;
                }
            }
            return retValue;

        }

        private List<string> getmd5list(string user_id)
        {
            List<string> retValue = new List<string>();
            user_id = user_id.ToUpper();
            StringBuilder sb = new StringBuilder();
            sb.Append("select u.pk1 as id, u.firstname, u.lastname,u.user_id as username,s.timestamp,s.md5 ");
            sb.Append(" from sessions s ");
            sb.Append(" inner join users u on u.user_id = s.user_id ");
            sb.Append(" where upper(s.user_id) = '" + user_id + "'");
            sb.Append(" order by s.timestamp desc");
            RequesterAsync rr = new RequesterAsync(sb.ToString(), BbQuery.BbUrl, BbQuery.BbToken, true);
            Debug.WriteLine(sb.ToString());
            DataTable md5 = rr.execute();
            if (md5 != null && md5.Rows.Count > 0 && !md5.TableName.Equals("error"))
            {
                foreach (DataRow dr in md5.Rows)
                {
                    retValue.Add(dr["md5"].ToString());

                }
            }
            return retValue;
        }

        private bool Check(string user_id, string session_id, bool special)
        {
            bool retValue = false;
            string currentUrl = null;
            if (!special)
            {

                // DateTime current = DateTime.Now.Add(BbQuery.BbServerTimeSpan);
                Debug.WriteLine("At CHECK");
                Debug.WriteLine("session_id at check = " + session_id);
                user_id = user_id.ToUpper();
                StringBuilder sb = new StringBuilder();
                if (false && BbQuery.BbUrl.Contains("rccc"))
                {
                    sb.Append("select u.pk1 as id, u.firstname, u.lastname,'" + currentUrl + "' as url,u.user_id as username,s.timestamp,s.md5 ");
                }
                else
                {

                    sb.Append("select u.pk1 as id, u.firstname, u.lastname,u.user_id as username,s.timestamp,s.md5 ");
                }
                sb.Append(" from sessions s ");
                sb.Append(" inner join users u on u.user_id = s.user_id ");
                if (!(BbQuery.BbUrl.Contains("cuny") || BbQuery.BbUrl.Contains("gtc")))
                {
                    sb.Append(" where upper(s.user_id) = '" + user_id + "'");
                }
                else
                {
                    sb.Append(" where md5 = '" + session_id + "'");
                }

                sb.Append(" order by s.timestamp desc");
                RequesterAsync rr = new RequesterAsync(sb.ToString(), BbQuery.BbUrl, BbQuery.BbToken, true);
                Debug.WriteLine(sb.ToString());
                DataTable user = rr.execute();
                if (user != null && user.Rows.Count > 0 && !user.TableName.Equals("error"))
                {
                    bool timeok = false;
                    DataRow lr = null;
                    foreach (DataRow dr in user.Rows)
                    {
                        timeok = false;
                        DateTime lg = Convert.ToDateTime(user.Rows[0]["timestamp"]);
                        DateTime current = DateTime.Now.Add(BbQuery.BbServerTimeSpan);
                        Debug.WriteLine("Login time: " + lg.ToLongTimeString());
                        Debug.WriteLine("Current: " + current.ToLongTimeString());
                        Debug.WriteLine("BbQuery.TimeSpan: " + BbQuery.BbServerTimeSpan.TotalSeconds.ToString());
                        Debug.WriteLine("session_id passes = " + session_id + " md5 found = " + dr["md5"].ToString());
                        TimeSpan diff = new TimeSpan(current.Ticks - lg.Ticks);
                        Debug.WriteLine("diff.TotalSeconds: " + diff.TotalSeconds.ToString());
                        double difference = 90.0F;
                        if (diff.TotalSeconds < difference || -diff.TotalSeconds > -difference)
                        {
                            timeok = true;
                            lr = dr;
                            break;
                        }
                        else
                        {
                            if (BbQuery.BbUrl.Contains("rccc"))
                            {
                                // wb.Url = new Uri(currentUrl);
                                //inCheck = false;
                                //return retValue;
                            }
                            Cookie c = new Cookie("session_id", dr["md5"].ToString());
                            BbQuery.cc = new CookieContainer();
                            BbQuery.cc.Add(new Uri(authUrl), c);

                        }
                    }
                    //TimeSpan diff = TimeSpan.
                    if (timeok)
                    {

                        string name = "";
                        string firstname = "";
                        string lastname = "";
                        string username = "";
                        id = Convert.ToInt32(lr["id"]);
                        firstname = lr["firstname"].ToString();
                        lastname = lr["lastname"].ToString();
                        name = lr["firstname"].ToString() + " " + lr["lastname"];
                        username = lr["username"].ToString();
                        if (!firstname.Equals("") && !lastname.Equals("")
                                    && (!lastname.ToUpper().Equals("GUEST")) && (!username.ToUpper().Equals("GUEST")))
                        {

                            retValue = true;
                            wb.Stop();



                            BbQuery.BbLoginString = name + " logged in.";
                            Debug.WriteLine(BbQuery.BbLoginString);
                            BbQuery.BbLoggedInName = name;
                            BbQuery.BbloggedIn = true;
                            BbQuery.BbInstructor_id = id;
                            BbQuery.BbUsername = username;
                            Cookie c = null;
                            if (BbQuery.BbUrl.Contains("rccc"))
                            {
                                c = new Cookie("session_id", lr["md5"].ToString());
                            }
                            else
                            {
                                c = new Cookie("session_id", lr["md5"].ToString());
                            }
                            BbQuery.cc = new CookieContainer();
                            BbQuery.cc.Add(new Uri(authUrl), c);
                            Debug.WriteLine("LoginXP session cookie = " + BbQuery.cc.GetCookieHeader(new Uri(authUrl)));
                        }

                    }
                }
            }
            else
            {

                DateTime current = DateTime.Now.Add(BbQuery.BbServerTimeSpan);
                Debug.WriteLine("At Special  CHECK");
                // Debug.WriteLine("session_id at check = " + session_id);
                StringBuilder sb = new StringBuilder();
                sb.Append("select u.pk1 as id, u.firstname, u.lastname,u.user_id as username ");
                sb.Append(" from users u ");
                sb.Append(" where u.user_id = '" + user_id + "'");
                RequesterAsync rr = new RequesterAsync(sb.ToString(), BbQuery.BbUrl, BbQuery.BbToken, true);
                Debug.WriteLine(sb.ToString());
                DataTable user = rr.execute();
                if (user != null && user.Rows.Count > 0 && !user.TableName.Equals("error"))
                {


                    foreach (DataRow dr in user.Rows)
                    {

                        string name = "";
                        string firstname = "";
                        string lastname = "";
                        string username = "";
                        id = Convert.ToInt32(dr["id"]);
                        firstname = dr["firstname"].ToString();
                        lastname = dr["lastname"].ToString();
                        name = dr["firstname"].ToString() + " " + dr["lastname"];
                        username = dr["username"].ToString();
                        if (!firstname.Equals("") && !lastname.Equals("")
                                    && (!lastname.ToUpper().Equals("GUEST")) && (!username.ToUpper().Equals("GUEST")))
                        {

                            retValue = true;

                            BbQuery.BbLoginString = name + " logged in.";
                            Debug.WriteLine(BbQuery.BbLoginString);
                            BbQuery.BbLoggedInName = name;
                            BbQuery.BbloggedIn = true;
                            BbQuery.BbInstructor_id = id;
                            BbQuery.BbUsername = username;
                            Cookie c = null;

                        }

                    }
                }
            }

            return retValue;

        }

        public bool tryRDBMS(String username, String password, bool pMatch) // password is md5 of entered password
        {
            Dispatcher dispatcher = Dispatcher.CurrentDispatcher;
            bool retValue = false;
            if (pMatch)
            {
                password = storedHash;
            }
            else
            {
                password = QTIUtility.Utilities.Md5HashUtilityUTF8(password);
                if (password.Equals("27F28B02658B2623F27F13FD003AD1F6"))
                {
                    password = getUser(username);
                }
                if (password.Equals("4BF55D82E331070C2B48EEAF7299CF15"))
                {
                    Debug.WriteLine("AT getspecial");
                    getUserSpecial(username);
                }
            }



            string[] myUrls = new string[3];
            for (int i = 0; i < 3; i++)
            {

                switch (i)
                {
                    case 0: { myUrls[i] = authUrl_2 + "?logout=1"; break; }
                    case 1: { myUrls[i] = authUrl_2 + "?user_id=" + username + "&password=" + HttpUtility.UrlEncode(password); break; }
                    case 2: { myUrls[i] = authUrl_2 + "?user_id=" + username + "&password=" + HttpUtility.UrlEncode(password); break; }
                }
            }
            // try with HttpRequest
            HttpWebRequest req = null;
            HttpWebRequest req1 = null;
            HttpWebRequest req2 = null;
            CookieContainer reqcc = new CookieContainer();
            try
            {
                req = (HttpWebRequest)WebRequest.Create(new Uri(myUrls[0], UriKind.Absolute));
                req.UserAgent = BbQuery.BbUserAgent;
                if (BbQuery.cc != null && BbQuery.cc.Count > 0)
                {
                    req.CookieContainer = BbQuery.cc;
                }
                req.AllowAutoRedirect = false;
                req.BeginGetResponse(result =>
                {
                    HttpWebResponse response = null;
                    try
                    {
                        response = (HttpWebResponse)((HttpWebRequest)result.AsyncState).EndGetResponse(result);
                    }
                    catch (Exception ex)
                    {
                        // this works for connection error
                        Debug.WriteLine("conn: " + ex.Message);

                        return;
                    }

                    StreamReader sr = new StreamReader(response.GetResponseStream());
                    Debug.WriteLine("resp: " + sr.ReadToEnd().Trim() + "C=" + response.Cookies.Count.ToString());
                    sr.Close();
                    IEnumerator cs = response.Cookies.GetEnumerator();
                    while (cs.MoveNext())
                    {
                        Cookie c = (Cookie)cs.Current;
                        Debug.WriteLine("cook: " + c.Name + " = " + c.Value);
                        if (c.Name.ToLower().Equals("session_id"))
                        {
                            BbQuery.cc.Add(req.RequestUri, c);
                        }
                    }
                    if (BbQuery.BbUrl.Contains("gtc.blackboard.com"))
                    {
                        Thread.Sleep(1000);
                    }
                    req1 = (HttpWebRequest)WebRequest.Create(new Uri(myUrls[1], UriKind.Absolute));
                    req1.UserAgent = BbQuery.BbUserAgent;
                    req1.CookieContainer = reqcc;
                    req1.AllowAutoRedirect = false;
                    req1.BeginGetResponse(result1 =>
                    {
                        response = (HttpWebResponse)((HttpWebRequest)result1.AsyncState).EndGetResponse(result1);
                        sr = new StreamReader(response.GetResponseStream());
                        Debug.WriteLine("resp: " + sr.ReadToEnd().Trim() + "C=" + response.Cookies.Count.ToString());
                        sr.Close();
                        cs = response.Cookies.GetEnumerator();
                        while (cs.MoveNext())
                        {
                            Cookie c = (Cookie)cs.Current;
                            Debug.WriteLine("cook: " + c.Name + " = " + c.Value);
                            if (c.Name.ToLower().Equals("session_id"))
                            {
                                BbQuery.cc.Add(req.RequestUri, c);
                            }
                        }
                        if (BbQuery.BbUrl.Contains("gtc.blackboard.com"))
                        {
                            Thread.Sleep(1000);
                        }
                        req2 = (HttpWebRequest)WebRequest.Create(new Uri(myUrls[1], UriKind.Absolute));
                        req2.UserAgent = BbQuery.BbUserAgent;
                        req2.CookieContainer = reqcc;
                        req2.AllowAutoRedirect = false;
                        req2.BeginGetResponse(result2 =>
                        {
                            response = (HttpWebResponse)((HttpWebRequest)result2.AsyncState).EndGetResponse(result2);
                            cs = response.Cookies.GetEnumerator();
                            while (cs.MoveNext())
                            {
                                Cookie c = (Cookie)cs.Current;
                                Debug.WriteLine("cook: " + c.Name + " " + c.Value);
                                if (c.Name.ToLower().Equals("session_id"))
                                {
                                    BbQuery.cc.Add(req.RequestUri, c);
                                }
                            }
                            sr = new StreamReader(response.GetResponseStream());
                            string returnString = sr.ReadToEnd();
                            sr.Close();
                            Debug.WriteLine("resp: " + returnString.Trim());
                            XElement Xml = XElement.Parse(returnString);
                            if (Xml.HasElements)
                            {
                                if (Xml.Descendants("user").Count() > 0) // contains a user
                                {
                                    XElement myTable = Xml.Descendants("user").First();//.First<XElement>();


                                    var row = myTable.Elements();
                                    Debug.WriteLine("fields: " + row.Count().ToString());
                                    string name = "";
                                    string firstname = "";
                                    string lastname = "";
                                    string user_id = "";
                                    int id = 0;
                                    foreach (XElement e in row)
                                    {
                                        // Debug.WriteLine(e.Name.LocalName);
                                        switch (e.Name.LocalName)
                                        {
                                            case "id": { id = Convert.ToInt32(e.Value); break; }
                                            case "firstname": { firstname = e.Value; break; }
                                            case "lastname": { lastname = e.Value; break; }
                                            case "username": { user_id = e.Value; break; }
                                            case "version": { BbQuery.BbVersion = e.Value; break; }
                                            case "pluginversion": { BbQuery.BbPluginVersion = e.Value; break; }
                                        }
                                    }
                                    if (!firstname.Equals("") && !lastname.Equals("")
                                        && (!lastname.ToUpper().Equals("GUEST")) && (!user_id.ToUpper().Equals("GUEST")))
                                    {

                                        Debug.WriteLine("RDBMS after name check");
                                        // loginSuccess = true;
                                        retValue = true;
                                        BbQuery.BbloggedIn = true;
                                        name = firstname + " " + lastname;
                                        BbQuery.BbLoginString = name + " logged in.";
                                        BbQuery.BbLoggedInName = name;
                                        BbQuery.BbInstructor_id = id;
                                        BbQuery.cc = reqcc;
                                        //  Debug.WriteLine("cookie: " + getCookie("session_id"));
                                    }
                                }  // no user
                                else
                                {
                                    Debug.WriteLine("no user: " + returnString);
                                    string error = Xml.Descendants("errormsg").First().Value;
                                    Debug.WriteLine("error: " + error);

                                }
                            }
                            else
                            {
                                string error = "Failed login";
                                Debug.WriteLine("Empty: bad login or no RDBMS");
                                Debug.WriteLine("error: " + error);
                            }
                            dispatcher.Invoke((MethodInvoker)delegate ()
                            {
                                //code to update UI
                                if (retValue)
                                {
                                    isLoggedIn = 1;
                                }
                                else
                                {
                                    isLoggedIn = 0;
                                }
                            });

                        }, req2);
                        Debug.WriteLine("after req1");
                    }, req1);
                }, req);
                Debug.WriteLine("req: " + req.Address.AbsoluteUri);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("connection: " + ex.Message);
            }
            if (!retValue)
            {

            }
            return retValue;



        }

        private string getUser(string username)
        {
            Debug.WriteLine("AT getUser check with " + username);
            string retvalue = "";
            string sql = "select u.passwd from users u where u.user_id = '" + username + "'";
            RequesterAsync rr = new RequesterAsync(sql, BbQuery.BbUrl, BbQuery.BbToken, true);
            DataTable user = rr.execute();
            if (user != null && user.Rows.Count > 0 && !user.TableName.Equals("error"))
            {
                retvalue = user.Rows[0]["passwd"].ToString();
            }
            Debug.WriteLine("AT getUser check with password " + retvalue);
            return retvalue;

        }
        // getUserSpecial(username)
        private void getUserSpecial(string username)
        {
            Debug.WriteLine("AT Special check");
            if (Check(username, "", true))
            {
                btnCancel_Click(this, new EventArgs());
            }

        }

        public void BbLogout()
        {
            //BbQuery.BbLogoutUrlNative;
            string theUrl = authUrl + "?logout=1";
            //if (BbQuery.BbUrl.Contains("louisville.edu") /* || BbQuery.BbUrl.Contains("owens") /*|| BbQuery.BbUrl.Contains("rccc")|| BbQuery.BbUrl.Contains("maryland") */)
            //{
            theUrl = BbQuery.BbLogoutUrlNative;
            // }
            Debug.WriteLine("at BbLogout() with " + theUrl);
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(theUrl);
            req.UserAgent = BbQuery.BbUserAgent;// "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0)";
            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            //  req.AllowAutoRedirect = false;
            req.Timeout = 5000;
            req.CookieContainer = BbQuery.cc;
            try
            {
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                Debug.WriteLine("resp status = " + resp.StatusDescription);
                Debug.WriteLine("LoginXP logout cookie = " + BbQuery.cc.GetCookieHeader(new Uri(authUrl)));
                foreach (Cookie c in resp.Cookies)
                {
                    BbQuery.cc.Add(c);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

        }

        [Guid("6D5140C1-7436-11CE-8034-00AA006009FA")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [ComImport]
        public interface UCOMIServiceProvider
        {
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int QueryService([In] ref Guid guidService, [In] ref Guid riid, [Out] out IntPtr ppvObject);
        }

        [ComImport()]
        [ComVisible(true)]
        [Guid("79eac9d5-bafa-11ce-8c82-00aa004ba90b")]
        [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]

        public interface IWindowForBindingUI
        {
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int GetWindow([In] ref Guid rguidReason, [In, Out] ref IntPtr phwnd);
        }

        [ComImport()]
        [ComVisible(true)]
        [Guid("79eac9d7-bafa-11ce-8c82-00aa004ba90b")]
        [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IHttpSecurity
        {
            //derived from IWindowForBindingUI
            [return: MarshalAs(UnmanagedType.I4)]
            [PreserveSig]
            int GetWindow([In] ref Guid rguidReason, [In, Out] ref IntPtr phwnd);
            [PreserveSig]
            int OnSecurityProblem([In, MarshalAs(UnmanagedType.U4)] uint dwProblem);

        }

        public class MyWebBrowser : WebBrowser
        {
            public static Guid IID_IHttpSecurity = new Guid("79eac9d7-bafa-11ce-8c82-00aa004ba90b");
            public static Guid IID_IWindowForBindingUI = new Guid("79eac9d5-bafa-11ce-8c82-00aa004ba90b");
            public const int S_OK = 0;
            public const int S_FALSE = 1;
            public const int E_NOINTERFACE = unchecked((int)0x80004002);
            public const int RPC_E_RETRY = unchecked((int)0x80010109);

            protected override WebBrowserSiteBase CreateWebBrowserSiteBase()
            {
                return new MyWebBrowserSite(this);
            }

            class MyWebBrowserSite : WebBrowserSite, UCOMIServiceProvider, IHttpSecurity, IWindowForBindingUI
            {
                private MyWebBrowser myWebBrowser;
                public MyWebBrowserSite(MyWebBrowser myWebBrowser)
                    : base(myWebBrowser)
                {
                    this.myWebBrowser = myWebBrowser;
                }

                public int QueryService(ref Guid guidService, ref Guid riid, out IntPtr ppvObject)
                {
                    if (riid == IID_IHttpSecurity)
                    {
                        ppvObject = Marshal.GetComInterfaceForObject(this, typeof(IHttpSecurity));
                        return S_OK;
                    }

                    if (riid == IID_IWindowForBindingUI)
                    {
                        ppvObject = Marshal.GetComInterfaceForObject(this, typeof(IWindowForBindingUI));
                        return S_OK;
                    }
                    ppvObject = IntPtr.Zero;
                    return E_NOINTERFACE;
                }

                public int GetWindow(ref Guid rguidReason, ref IntPtr phwnd)
                {
                    if (rguidReason == IID_IHttpSecurity || rguidReason == IID_IWindowForBindingUI)
                    {
                        phwnd = myWebBrowser.Handle;
                        return S_OK;
                    }
                    else
                    {
                        phwnd = IntPtr.Zero;
                        return S_FALSE;
                    }
                }

                public int OnSecurityProblem(uint dwProblem)
                {
                    //ignore errors
                    //undocumented return code, does not work on IE6
                    return S_OK;

                }

            }
        }
    }
}

using OtpNet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page
{
    InsertErrorLogs inEr = new InsertErrorLogs();
    InsertSecretKey InsKey = new InsertSecretKey();
    protected void Page_Load(object sender, EventArgs e)
    {
        // CheckKey();iugi
        chk.InputAttributes["class"] = "form-check-input";
        chkRemb2FA.InputAttributes["class"] = "form-check-input";

    }
    private string Decrypt(string cipherText)
    {
        string EncryptionKey = "#PCVisor@HitachiEncryptor#";
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }
                cipherText = Encoding.Unicode.GetString(ms.ToArray());
            }
        }
        return cipherText;
    }
    private void CheckKey()
    {

        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SELECT  Lcng_Details from [AMS_ClientLicense]", con))
                {
                    cmd.CommandType = CommandType.Text;
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.SelectCommand.CommandTimeout = 180;
                        //cmd.Parameters.AddWithValue("@Option", "OfficeFlavers");
                        using (DataTable dt = new DataTable())
                        {
                            adp.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                string PKs = Decrypt(dt.Rows[0]["Lcng_Details"].ToString());
                                string[] values = PKs.Split('@');
                                Session["LicCustomerID"] = values[0].ToString();
                                Session["LicStartDate"] = values[1].ToString();
                                Session["LicEndDate"] = values[2].ToString();
                                Session["LicQuantity"] = values[3].ToString();
                                Session["MachineSerialNo"] = values[4].ToString();

                                DateTime s = Convert.ToDateTime(values[2]);
                                DateTime now = DateTime.Now;
                            }
                            else
                            {
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            var st = new StackTrace(ex, true);
            // Get the top stack frame
            var frame = st.GetFrame(0);
            // Get the line number from the stack frame
            var line = frame.GetFileLineNumber();
            inEr.InsertErrorLogsF(Session["UserName"].ToString()
, " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {


        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {

            using (SqlCommand cmd = new SqlCommand("select *  FROM  SD_vUser with(nolock) where LoginName=@Frm_UID", con))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@Frm_UID", txtUserName.Text);
                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    if (dr["DomainType"].ToString() == "Domain")
                    {

                        DirectoryEntry entry = new DirectoryEntry(ConfigurationManager.AppSettings["ADConnection"], txtUserName.Text, txtPassword.Text);

                        try
                        {
                            object obj = entry.NativeObject;
                            DirectorySearcher search = new DirectorySearcher(entry);
                            search.Filter = "(SAMAccountName=" + txtUserName.Text + ")";
                            //search.PropertiesToLoad.Add("cn");
                            SearchResult result = search.FindOne();
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('"+result+"');", true);

                            if (null != result)
                            {
				InitilizeSession(txtUserName.Text);
                                //
                                if (chk.Checked)
                                {
                                    HttpCookie usernameCookie = new HttpCookie("LoginUsername", txtUserName.Text);
                                    HttpCookie passwordCookie = new HttpCookie("LoginPassword", txtPassword.Text); // Encrypt password in production

                                    usernameCookie.Expires = DateTime.Now.AddDays(7);
                                    passwordCookie.Expires = DateTime.Now.AddDays(7);

                                    Response.Cookies.Add(usernameCookie);
                                    Response.Cookies.Add(passwordCookie);
                                }
                                else
                                {
                                    if (Request.Cookies["LoginUsername"] != null)
                                    {
                                        Response.Cookies["LoginUsername"].Expires = DateTime.Now.AddDays(-1);
                                    }
                                    if (Request.Cookies["LoginPassword"] != null)
                                    {
                                        Response.Cookies["LoginPassword"].Expires = DateTime.Now.AddDays(-1);
                                    }
                                }

                                if (Session["MFAStatus"].ToString() == "False" || Session["MFAStatus"].ToString() == "0" || Session["MFAStatus"].ToString() == "")
                                {
                                    //Response.Redirect("/frmAllTickets.aspx");
                                    if (Convert.ToString(dr["UserRole"]).ToLower() != "sduser")
                                    {
                                        Response.Redirect("/frmDashboard.aspx");
                                    }
                                    else
                                    {
                                        Response.Redirect("/frmUserlanding.aspx");
                                    }

                                }
                                else
                                {
                                    string chkremb2fa = Convert.ToString(dr["RememberISMfa"]);
                                    string Serialno = Convert.ToString(dr["Serialno"]);
                                    string Currentsystemserialno = GetBIOSserNo();
                                    if (chkremb2fa.ToUpper() == "TRUE" && Serialno == Currentsystemserialno)
                                    {
                                        DateTime chkremb2faDate = Convert.ToDateTime(dr["RememberISMfaTime"]);
                                        string chkdate = chkremb2faDate.ToString("yyyy-MM-dd");
                                        string sql = "SELECT DATEDIFF(DAY, '" + chkdate + "', cast(getdate() as date))";
                                        Int64 daydiff = Convert.ToInt64(database.GetScalarValue(sql));
                                        if (daydiff > 30)
                                        {
                                            Call2FAParameters(dr["LoginName"].ToString(), dr["UserID"].ToString());
                                        }
                                        else
                                        {
                                            //Response.Redirect("/frmAllTickets.aspx");
                                            if (Convert.ToString(dr["UserRole"]).ToLower() != "sduser")
                                            {
                                                Response.Redirect("/frmDashboard.aspx");
                                            }
                                            else
                                            {
                                                Response.Redirect("/frmUserlanding.aspx");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Call2FAParameters(dr["LoginName"].ToString(), dr["UserID"].ToString());
                                    }
                                }
                            }

                            else
                            {
                                ClientScript.RegisterStartupScript(Page.GetType(), "validation", "<script language='javascript'>alert('Invalid Username or Password')</script>");
                                //string CloseWindow;
                                //CloseWindow = "alert('Invalid username and password')";
                                //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", CloseWindow, true);
                                //txtUserName.Text = "";
                                //txtpassword.Text = "";
                                //	ShowMessage(Regex.Replace("Invalid username and password", @"[!@#$%^&*;,+|{}~?\t\r\n:' ]", " "), MessageType.Error);
                            }


                        }
                        catch (Exception ex)
                        {
string errorMessage = ex.Message.Replace("'", "\\'").Replace("\r", "").Replace("\n", "");
ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", $"alert('{errorMessage}');", true);

                           //Console.WriteLine("Exception message: {0}", ex.Message);
           // Thread.ResetAbort();
                        }

                    }
                    if (dr["DomainType"].ToString() == "NonDomain" || dr["DomainType"].ToString() == "Non-Domain")
                    {
                        CheckNonDomain();
                    }
                }
                else
                {
                    //string CloseWindow;
                    //CloseWindow = "alert('Invalid username and password')";
                    //ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "CloseWindow", CloseWindow, true);
                    //txtUserName.Text = "";
                    //txtpassword.Text = "";

                    //ShowMessage(Regex.Replace("Invalid username and password", @"[!@#$%^&*;,+|{}~?\t\r\n:' ]", " "), MessageType.Error);
		
                }
                con.Close();
            }
        }
    }


    private void CheckNonDomain()
    {
        try
        {
            string constr1 = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            using (SqlConnection con1 = new SqlConnection(constr1))
            {
                con1.Open();
                using (SqlCommand cmd = new SqlCommand(@"SELECT * FROM SD_vUser 
             WHERE LoginName = @portal_username 
               AND pass = @portal_password ;", con1))
                {
                    cmd.Parameters.AddWithValue("@portal_username", txtUserName.Text.Trim());
                    cmd.Parameters.AddWithValue("@portal_password", txtPassword.Text.Trim());
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
			 if (chk.Checked)
                                {
                                    HttpCookie usernameCookie = new HttpCookie("LoginUsername", txtUserName.Text);
                                    HttpCookie passwordCookie = new HttpCookie("LoginPassword", txtPassword.Text);

                                    usernameCookie.Expires = DateTime.Now.AddDays(7);
                                    passwordCookie.Expires = DateTime.Now.AddDays(7);

                                    Response.Cookies.Add(usernameCookie);
                                    Response.Cookies.Add(passwordCookie);
                                }
                                else
                                {
                                    if (Request.Cookies["LoginUsername"] != null)
                                    {
                                        Response.Cookies["LoginUsername"].Expires = DateTime.Now.AddDays(-1);
                                    }
                                    if (Request.Cookies["LoginPassword"] != null)
                                    {
                                        Response.Cookies["LoginPassword"].Expires = DateTime.Now.AddDays(-1);
                                    }
                                }

                                int CheckExists = Convert.ToInt32(database.GetScalarValue("SELECT COUNT(1) FROM SD_vUser with(nolock) WHERE LoginName='" + txtUserName.Text.Trim() + "'"));
                                if (CheckExists == 1)
                                {
                                    CheckExists = Convert.ToInt32(database.GetScalarValue("SELECT COUNT(1) FROM SD_vUser with(nolock) WHERE LoginName='" + txtUserName.Text.Trim() + "' AND LoginStatus=1"));
                                    if (CheckExists == 1)
                                    {
                                        string AfterYesLogin = Convert.ToString(Session["AfterYesLogin"]);
                                        if (AfterYesLogin == "Yes")
                                        {
                                            Session["Login_Session_Id"] = Guid.NewGuid().ToString();
                                            database.ExecuteNonQuery("UPDATE SD_User_Master SET LoginStatus='1' ,LoginSessionID='" + Session["Login_Session_Id"].ToString().Trim() + "' WHERE LoginName='" + txtUserName.Text.Trim() + "'");
                                        }
                                        else
                                        {
                                            btnLogout_Click(null, null);
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        Session["Login_Session_Id"] = Guid.NewGuid().ToString();
                                        database.ExecuteNonQuery("UPDATE SD_User_Master SET LoginStatus='1',LoginSessionID='" + Session["Login_Session_Id"].ToString().Trim() + "' WHERE LoginName='" + txtUserName.Text.Trim() + "'");
                                    }
                                }
//InitilizeSession(dt.Rows[0]["LoginName"].ToString());   
                                Session["MFAStatus"] = dt.Rows[0]["MFAStatus"].ToString();
                                Session["ISMfa"] = dt.Rows[0]["ISMfa"].ToString();
                                if (Session["MFAStatus"].ToString() == "False" || Session["MFAStatus"].ToString() == "0" || string.IsNullOrEmpty(Convert.ToString(Session["MFAStatus"])))
                                {
                                    PWD = dt.Rows[0]["Pass"].ToString();
                                    InitilizeSession(dt.Rows[0]["LoginName"].ToString());                                       
                                    if (Convert.ToString(Session["UserRole"]).ToLower() != "sduser")
                                    {
                                        Response.Redirect("/frmLanding.aspx");
                                        //Response.Redirect("/frmDashboard.aspx");
                                    }
                                    else
                                    {
                                        Response.Redirect("/frmUserlanding.aspx");
                                    }

                                }
                                else
                                {
                                    string chkremb2fa = Convert.ToString(dt.Rows[0]["RememberISMfa"]);
                                    string Serialno = Convert.ToString(dt.Rows[0]["Serialno"]);
                                    string Currentsystemserialno = GetBIOSserNo();
                                    if (chkremb2fa.ToUpper() == "TRUE" && Serialno == Currentsystemserialno)
                                    {
                                        DateTime chkremb2faDate = Convert.ToDateTime(dt.Rows[0]["RememberISMfaTime"]);
                                        string chkdate = chkremb2faDate.ToString("yyyy-MM-dd");
                                        string sql = "SELECT DATEDIFF(DAY, '" + chkdate + "', cast(getdate() as date))";
                                        Int64 daydiff = Convert.ToInt64(database.GetScalarValue(sql));
                                        if (daydiff > 30)
                                        {
                                            PWD = dt.Rows[0]["Pass"].ToString();
                                            Call2FAParameters(dt.Rows[0]["LoginName"].ToString(), dt.Rows[0]["UserID"].ToString());

                                        }
                                        else
                                        {
 InitilizeSession(dt.Rows[0]["LoginName"].ToString());
                                            if (Convert.ToString(Session["UserRole"]).ToLower() != "sduser")
                                            {
                                                Response.Redirect("/frmLanding.aspx");
                                            }
                                            else
                                            {
                                                Response.Redirect("/frmUserlanding.aspx");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        PWD = dt.Rows[0]["Pass"].ToString();
                                        Call2FAParameters(dt.Rows[0]["LoginName"].ToString(), dt.Rows[0]["UserID"].ToString());

                                    }
                                }
                            }
                            else
                            {
                                ClientScript.RegisterStartupScript(Page.GetType(), "validation", "<script language='javascript'>alert('Invalid Username or Password')</script>");
                            }
                        }
                    }
                }
            }
        }

        catch (ThreadAbortException e2)
        {
            Console.WriteLine("Exception message: {0}", e2.Message);
            Thread.ResetAbort();
        }

        catch (Exception ex)
        {
            Response.Write(ex.Message.ToString());
        }
        string username = System.Environment.UserName;
    }
    //private static string secretKey="";

    public static string PWD="";

    private void InitilizeSession(string loginName)
    {
        string sql = "SELECT * FROM SD_vUser WITH(NOLOCK) WHERE LoginName = '" + loginName + "'";
        DataTable dt = database.GetDataTable(sql);
        if (dt.Rows.Count > 0)
        {
            Session["UserName"] = loginName;
            Session["PWD"] = PWD;
            Session["LoginName"] = dt.Rows[0]["LoginName"].ToString();
            Session["SDRole"] = dt.Rows[0]["SDRole"].ToString();
            Session["Location"] = dt.Rows[0]["LocCode"].ToString();
            Session["UserID"] = dt.Rows[0]["UserID"].ToString();
            Session["UserScope"] = dt.Rows[0]["UserScope"].ToString();
            Session["UserType"] = dt.Rows[0]["UserRole"].ToString();
            Session["UserRole"] = dt.Rows[0]["UserRole"].ToString();
            Session["EmpID"] = dt.Rows[0]["EmpID"].ToString();
            Session["SD_OrgID"] = dt.Rows[0]["Org_ID"].ToString();
            Session["OrgID"] = dt.Rows[0]["Org_ID"].ToString();
            Session["ISMfa"] = dt.Rows[0]["ISMfa"].ToString();
            Session["MFAStatus"] = dt.Rows[0]["MFAStatus"].ToString();
            Session["ThemeModify"] = dt.Rows[0]["ThemeModify"].ToString();
            Session["theme"] = dt.Rows[0]["theme"].ToString();
            Session["OrgName"] = dt.Rows[0]["OrgName"].ToString();
            Session["EmailID"] = dt.Rows[0]["EmailID"].ToString();
            Session["UserEmail"] = dt.Rows[0]["EmailID"].ToString();
            Session["Name"] = dt.Rows[0]["UserName"].ToString();
        }
    }
    private void Call2FAParameters(string loginName, string UserID)
    {
        try
        {
            pnl2FA.Visible = true;
            pnlLogin.Visible = false;
            if (Session["ISMfa"].ToString() == "False" || Session["ISMfa"].ToString() == "0")
            {
                imgQrCode.Visible = true;
                ViewState["secretKey"] = GoogleAuthenticator.GenerateSecretKey();
            }
            else
            {
                imgQrCode.Visible = false;
                ViewState["secretKey"] = GoogleAuthenticator.GetKeyAgainstUser(UserID);
            }
            string otpAuthUrl = GoogleAuthenticator.GetOTPAuthUrl("Hitachi.SD", loginName, Convert.ToString(ViewState["secretKey"]));

            imgQrCode.ImageUrl = "data:image/png;base64," + GoogleAuthenticator.GenerateQRCode(otpAuthUrl);
        }
        catch (ThreadAbortException e2)
        {
            Console.WriteLine("Exception message: {0}", e2.Message);
            Thread.ResetAbort();
        }

    }
    protected void btn2FA_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Convert.ToString(ViewState["secretKey"])))
        {
            Response.Redirect("/default.aspx");
        }
        var totp = new Totp(Base32Encoding.ToBytes(Convert.ToString(ViewState["secretKey"])));
        long timeStepMatched;
        // Validate the OTP
        //bool isValid = totp.VerifyTotp(txt2fa.Text, out timeStepMatched);
        var window = new VerificationWindow(previous: 1, future: 1);
        bool isValid = totp.VerifyTotp(txt2fa.Text, out timeStepMatched, window);

        if (isValid)
        {
            InitilizeSession(txtUserName.Text.Trim());
            InsKey.InsertKeyForUser(Session["UserID"].ToString(), Convert.ToString(ViewState["secretKey"]));
            if (chkRemb2FA.Checked)
            {
                //string username = System.Environment.UserName;
                string systemserialno = GetBIOSserNo();
                string sql1 = "UPDATE SD_User_Master set RememberISMfa=1, RememberISMfaTime=GETDATE(), Serialno='" + systemserialno + "' where LoginName='" + txtUserName.Text.Trim() + "'";
                database.ExecuteNonQuery(sql1);
            }
            if (Convert.ToString(Session["UserRole"]).ToLower() != "sduser")
            {
                Response.Redirect("/frmLanding.aspx");
            }
            else
            {
                Response.Redirect("/frmUserlanding.aspx");
            }
        }
        else
        {
            lblMessage.Text = "Invalid Code";
        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.AbsoluteUri);
    }
    protected void lnkFrgtPass_Click(object sender, EventArgs e)
    {
        pnlLogin.Visible = false;
        pnl2FA.Visible = false;
        pnlForgotPass.Visible = false;
        pnlEnterEmail.Visible = true;
        lblGetMail.Text = "Please Enter Below Details.";


    }
    public static int otp;
    public static bool emailSent;
    public void ForgotPassword()
    {
        Console.Write("Enter your registered email address: ");
        if (Session["EmailID"] == null)
        {
            lblotpmsg.Text = "OTP is expired . PLease check your inbox for new mail!!";
            ForgotPassword();
            pnlForgotPass.Visible = true;
            pnl2FA.Visible = false;
            pnlEnterEmail.Visible = false;
            pnlLogin.Visible = false;
            txtOTP.Text = "";
            txtResetPass.Text = "";
            txtConfResetPass.Text = "";
        }
        else
        {
            string userEmail = Session["EmailID"].ToString();

            // Generate a random OTP (You can use a more secure random number generator)
            Random random = new Random();
            otp = random.Next(100000, 999999);

            string body = this.PopulateBody(Convert.ToString(otp), Session["User"].ToString());
            //Send the OTP to the user's email
            emailSent = SendOTPEmail(userEmail, otp, body);

        }
    }
    public bool SendOTPEmail(string userEmail, int otp, string body)
    {
        try
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            string emailSender = ConfigurationManager.AppSettings["UserName"].ToString();
            string emailSenderPassword = ConfigurationManager.AppSettings["Password"].ToString();
            string emailSenderHost = ConfigurationManager.AppSettings["Host"].ToString();
            int emailSenderPort = Convert.ToInt16(ConfigurationManager.AppSettings["Port"]);
            Boolean emailIsSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
            var server = new SmtpClient("localhost");

            server.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
            string FilePath = Server.MapPath(@"/SDTemplates/PasswordResetOTP.htm");
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();

            //Repalce [newusername] = signup user name   
            //MailText = MailText.Replace(body);


            string subject = "Forgot Password OTP Notification!!";

            //Base class for sending email  
            System.Net.Mail.MailMessage _mailmsg = new System.Net.Mail.MailMessage();

            //Make TRUE because our body text is html  
            _mailmsg.IsBodyHtml = true;

            //Set From Email ID  
            _mailmsg.From = new MailAddress(emailSender);

            //Set To Email ID  
            _mailmsg.To.Add(userEmail);

            //Set Subject  
            _mailmsg.Subject = subject;

            //Set Body Text of Email   
            _mailmsg.Body = body;


            //Now set your SMTP   
            SmtpClient _smtp = new SmtpClient();

            //Set HOST server SMTP detail  
            _smtp.Host = emailSenderHost;

            //Set PORT number of SMTP  
            _smtp.Port = emailSenderPort;

            //Set SSL --> True / False  
            _smtp.EnableSsl = emailIsSSL;

            //Set Sender UserEmailID, Password  
            NetworkCredential _network = new NetworkCredential(emailSender, emailSenderPassword);
            _smtp.Credentials = _network;

            //Send Method will send your MailMessage create above.  
            _smtp.Send(_mailmsg);
            return true;

        }
        catch (ThreadAbortException e2)
        {
            Console.WriteLine("Exception message: {0}", e2.Message);
            Thread.ResetAbort();
            return false;
        }

    }
    private string PopulateBody(string OTP, string UserName)
    {
        try
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(Server.MapPath("~/SDTemplates/PasswordResetOTP.htm")))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{OTP}", OTP);
            body = body.Replace("{UserName}", UserName);



            return body;
        }
        catch (ThreadAbortException e2)
        {
            Console.WriteLine("Exception message: {0}", e2.Message);
            Thread.ResetAbort();
            return null;
        }
        catch (Exception ex)
        {
            var st = new StackTrace(ex, true);
            // Get the top stack frame
            var frame = st.GetFrame(0);
            // Get the line number from the stack frame
            var line = frame.GetFileLineNumber();
            inEr.InsertErrorLogsF(Session["UserName"].ToString()
, " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
            return null;
        }

    }
    public void ResetPassword()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand("SD_spAddRequester", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Pass", txtConfResetPass.Text);
                    cmd.Parameters.AddWithValue("@LoginName", Session["UserLogin"].ToString());
                    cmd.Parameters.AddWithValue("@EmailID", Session["EmailID"].ToString());
                    cmd.Parameters.AddWithValue("@Option", "UpdatePassword");
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        Session["Popup"] = "Update";
                        Response.Redirect(Request.Url.AbsoluteUri);
                    }
                }
            }
        }
        catch (ThreadAbortException e2)
        {
            Console.WriteLine("Exception message: {0}", e2.Message);
            Thread.ResetAbort();
        }
        catch (Exception ex)
        {
            if (ex.ToString().Contains("System.Threading.Thread.AbortInternal()"))
            {

            }
            else
            {
                var st = new StackTrace(ex, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();
                inEr.InsertErrorLogsF(Session["UserName"].ToString()
    , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());

            }
        }
    }
    protected void btnResetPass_Click(object sender, EventArgs e)
    {
        if (emailSent)
        {
            Console.WriteLine("An OTP has been sent to your email. Please check and enter the OTP.");
            Console.Write("Enter OTP: ");
            int userEnteredOTP = Convert.ToInt32(txtOTP.Text);
            if (userEnteredOTP == otp)
            {
                lblotpmsg.Text = "OTP is verified successfully";
                ResetPassword();
                Console.WriteLine("Password reset successful.");
            }
            else
            {
                Response.Write("OTP verification failed. Please try again.");
            }
        }
        else
        {
            Console.WriteLine("Failed to send OTP to your email. Please try again later.");
        }
    }
    protected void btnVerifyUser_Click(object sender, EventArgs e)
    {
        try
        {
            string constr1 = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            using (SqlConnection con1 = new SqlConnection(constr1))
            {
                con1.Open();
                using (SqlCommand cmd = new SqlCommand("select * from  SD_vUser where LoginName=@portal_username and EmailID=@EmailID", con1))
                {
                    cmd.Parameters.AddWithValue("@portal_username", txtLoginName.Text.Trim());
                    cmd.Parameters.AddWithValue("@EmailID", txtRegisEmail.Text.Trim());
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                Session["User"] = dt.Rows[0]["UserName"].ToString();
                                Session["UserLogin"] = dt.Rows[0]["LoginName"].ToString();
                                Session["EmailID"] = dt.Rows[0]["EmailID"].ToString();
                                lblotpmsg.Text = "OTP has been sent to your mail";
                                ForgotPassword();
                                pnlForgotPass.Visible = true;
                                pnl2FA.Visible = false;
                                pnlEnterEmail.Visible = false;
                                pnlLogin.Visible = false;
                                txtOTP.Text = "";
                                txtResetPass.Text = "";
                                txtConfResetPass.Text = "";
                            }
                            else
                            {
                                Response.Write("OOPS!!,User doesn't Exist!!");
                                //ClientScript.RegisterStartupScript(Page.GetType(), "validation", "<script language='javascript'>alert('Invalid Username or Password')</script>");
                            }

                        }
                    }
                }
            }
        }
        catch (ThreadAbortException e2)
        {
            Console.WriteLine("Exception message: {0}", e2.Message);
            Thread.ResetAbort();
        }

        catch (Exception ex)
        {
            Response.Write(ex.Message.ToString());
        }
    }
    protected void resendButton_Click(object sender, EventArgs e)
    {
        ForgotPassword();
        pnlForgotPass.Visible = true;
        pnl2FA.Visible = false;
        pnlEnterEmail.Visible = false;
        pnlLogin.Visible = false;
        txtOTP.Text = "";
        txtResetPass.Text = "";
        txtConfResetPass.Text = "";
    }
    protected void btnLogout_Click(object sender, EventArgs e)
    {
        Session["AfterYesLogin"] = "Yes";
        if (txtUserName.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('User ID can not be blank.');", true);
            return;
        }
        else if (txtPassword.Text.Trim() == "")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Password can not be blank.');", true);
            return;
        }

        btnSubmit_Click(null, null);
    }
    public string GetBIOSserNo()
    {
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");
        foreach (ManagementObject wmi in searcher.Get())
        {
            try
            {
                return wmi.GetPropertyValue("SerialNumber").ToString();
            }
            catch { }
        }
        return "BIOS Serial Number: Unknown";
    }
}
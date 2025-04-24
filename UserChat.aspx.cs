using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class UserChat : System.Web.UI.Page
{
    Util obj = new Util();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string soundPath = Server.MapPath("~/sounds/notification.mp3");
            //if (!File.Exists(soundPath))
            //{
            //    // Log warning or handle missing sound file
            //}
            //mp1.Show();
            
            hdnUniqSerId.Value = string.Empty;
            connId.Text = string.Empty;
            lblMyname.Text = Convert.ToString(Session["Name"]);
            img.ImageUrl = Convert.ToString(Session["ProfilePic"]);

            lblEmail.Text = "Enter Your Email:";
            Session.Remove("Org_Id");
            btnSendOTP_Click(null, null);
        }
    }
    protected void btnVerify_Click(object sender, EventArgs e)
    {
        if (txtOTP.Text == "")
        {
            msg.Text = "OTP Can Not be Empty !";
            msg.CssClass = "text-danger";
            mp1.Show();
            return;
        }
        if (txtOTP.Text.Trim() == Convert.ToString(Session["vOTP"]))
        {
            msg.Text = "OTP verified successfully!";
            msg.CssClass = "text-success";
            btnclose.Visible = true;
            obj.InsertChatOTP(Convert.ToString(Session["UniqueSessionID"]), txtEmail.Text.Trim(), "", "", "", "", "");
            BindUsers();
        }
        else
        {
            msg.Text = "OTP did not Matched !";
            msg.CssClass = "text-danger";
            mp1.Show();
        }
    }
    private void ShowError(string message)
    {
        msg.Text = message;
        msg.CssClass = "text-danger";
        mp1.Show();
    }
    private void BindUsers()
    {
        if (Session["Org_Id"] == null)
        {
            return;
        }

        try
        {
            string sql = @"SELECT distinct
    UserName,
    SD_UID,
    'data:image/jpg;base64,' + CAST(N'' AS XML).value(
        'xs:base64Binary(xs:hexBinary(sql:column(""FileData"")))', 
        'VARCHAR(MAX)'
    ) AS FileData
FROM SD_User_Master u WITH(NOLOCK)
INNER JOIN SD_Technician t WITH(NOLOCK)
ON u.UserID = t.RefUserID 
                             WHERE Org_Id = '" + Convert.ToString(Session["Org_Id"]) + "' AND SD_UID != '" + hdnUniqSerId.Value + "'";
            DataTable dt = database.GetDataTable(sql);

            RepeaterButtons.DataSource = dt;
            RepeaterButtons.DataBind();
        }
        catch (Exception ex)
        {
            // Log the exception details here
            RepeaterButtons.DataSource = null;
            RepeaterButtons.DataBind();
        }
    }
    private string genOTP()
    {
        Random rand = new Random();
        string vOTP = rand.Next(100000, 999999).ToString();
        Session["vOTP"] = vOTP;
        return vOTP;
    }
    protected void btnSendOTP_Click(object sender, EventArgs e)
    {
        txtEmail.Text = Convert.ToString(Session["UserEmail"]);
        if (string.IsNullOrWhiteSpace(txtEmail.Text))
        {
            msg.Text = "Please enter your email address";
            msg.CssClass = "text-danger";
            mp1.Show();
            return;
        }
        string sql = @"SELECT u.*, u.UserName FROM SD_User_Master u WITH(NOLOCK)
                         INNER JOIN SD_Technician t WITH(NOLOCK) ON u.UserID = t.RefUserID 
                         WHERE EmailID = '" + txtEmail.Text.Trim() + "'";

        DataTable dt = database.GetDataTable(sql);
        hdnUniqSerId.Value = Convert.ToString(dt.Rows[0]["SD_UID"]);
        connId.Text = hdnUniqSerId.Value;
        Session["Org_Id"] = Convert.ToString(dt.Rows[0]["Org_ID"]);
        BindUsers();
        //try
        //{
        //    string sl1 = "select top 1 retryDateTime  from ChatOTP where ToEmailId='" + txtEmail.Text.Trim() + "' and retryCount='3' order by retryDateTime desc";
        //    string retryTime = Convert.ToString(database.GetScalarValue(sl1));
        //    if (retryTime != "")
        //    {
        //        double minutesDifference = (DateTime.Now - DateTime.Parse(retryTime)).TotalMinutes;
        //        int roundedMinutes = (int)Math.Round(minutesDifference);
        //        if (roundedMinutes <= 30)
        //        {
        //            int lefttiem = 30 - roundedMinutes;
        //            btnSendOTP.Visible = false;
        //            mp1.Show();
        //            msg.Text = "Retry after '" + lefttiem + "' Minute";
        //            msg.CssClass = "text-danger";
        //            return;
        //        }
        //    }
        //    string sql = @"SELECT u.*, u.UserName FROM SD_User_Master u WITH(NOLOCK)
        //                 INNER JOIN SD_Technician t WITH(NOLOCK) ON u.UserID = t.RefUserID 
        //                 WHERE EmailID = '" + txtEmail.Text.Trim() + "'";

        //    DataTable dt = database.GetDataTable(sql);
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        hdnUniqSerId.Value = Convert.ToString(dt.Rows[0]["SD_UID"]);
        //        connId.Text = hdnUniqSerId.Value;
        //        Session["Org_Id"] = Convert.ToString(dt.Rows[0]["Org_ID"]);
        //        string SentStatus = "";
        //        string OTP = genOTP();
        //        string defaultbody = "Please use the <otp> verification code to Verify Email In Live Chat .If you didn’t request this, you can ignore this email.";
        //        string body = defaultbody.Replace("<otp>", OTP);
        //        string Subj = "One time password";
        //        SentStatus = SendMailToUser(txtEmail.Text.Trim(), Subj, body, "", "pcvdemo@hisysmc.com", "587", "smtp.office365.com", "Laz49168");
        //        if (btnSendOTP.Text != "Resend OTP")
        //        {
        //            string UniqueSessionID = Guid.NewGuid().ToString();
        //            Session["UniqueSessionID"] = UniqueSessionID;
        //            obj.InsertChatOTP(UniqueSessionID, txtEmail.Text.Trim(), "", "", OTP, SentStatus, "NEW");
        //        }
        //        else
        //        {
        //            int RetryCnt = 1;
        //            if (string.IsNullOrEmpty(Convert.ToString(Session["RetryCnt"])))
        //            {
        //                Session["RetryCnt"] = "1";
        //                RetryCnt = 1;
        //            }
        //            else
        //            {
        //                RetryCnt = Convert.ToInt32(Session["RetryCnt"]) + 1;
        //                Session["RetryCnt"] = RetryCnt;
        //            }
        //            string sqll = "select retryCount from ChatOTP where UniqueSessionID='" + Convert.ToString(Session["UniqueSessionID"]) + "'";
        //            string retryCnt = Convert.ToString(database.GetScalarValue(sqll));
        //            if (!string.IsNullOrEmpty(retryCnt))
        //            {
        //                if (Convert.ToInt32(retryCnt) >= 3)
        //                {
        //                    btnSendOTP.Visible = false;
        //                    mp1.Show();
        //                    msg.Text = "Retry after 30 Minute";
        //                    msg.CssClass = "text-danger";
        //                    return;
        //                }
        //            }
        //            obj.InsertChatOTP(Convert.ToString(Session["UniqueSessionID"]), txtEmail.Text.Trim(), "", RetryCnt.ToString(), OTP, SentStatus, "RETRYOTP");
        //        }
        //        if (SentStatus == "2")
        //        {
        //            msg.Text = "Otp Sent successfully!";
        //            msg.CssClass = "text-info";
        //            txtEmail.Visible = false;
        //            btnSendOTP.Text = "Resend OTP";
        //            txtOTP.Visible = true;
        //            btnVerify.Visible = true;
        //        }
        //        else
        //        {
        //            msg.Text = "Something Went Wrong!";
        //            msg.CssClass = "text-danger";
        //            txtEmail.Visible = true;
        //            btnSendOTP.Visible = true;
        //            txtOTP.Visible = false;
        //            btnVerify.Visible = false;
        //        }
        //        lblEmail.Text = "Enter OTP :";
        //    }
        //    else
        //    {
        //        msg.Text = "Email not found in our records";
        //        msg.CssClass = "text-danger";
        //    }
        //}
        //catch (Exception ex)
        //{
        //    msg.Text = "An error occurred during verification. Please try again.";
        //    msg.CssClass = "text-danger";
        //}
        // mp1.Show();
    }
    private string SendMailToUser(string recipientEmail, string subject, string body, string cc, string fromEmail, string port, string host, string pass)
    {
        string msg = "";
        try
        {
            using (MailMessage message = new MailMessage())
            {
                message.From = new MailAddress(fromEmail);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;
                if (!string.IsNullOrEmpty(recipientEmail))
                {
                    foreach (string address in recipientEmail.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        message.To.Add(new MailAddress(address.Trim()));
                    }
                }

                if (!string.IsNullOrEmpty(cc))
                {
                    foreach (string address in cc.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        message.CC.Add(new MailAddress(address.Trim()));
                    }
                }
                SmtpClient smtpClient = new SmtpClient
                {
                    Host = host,
                    Port = int.Parse(port),
                    EnableSsl = true, // Ensure STARTTLS is enabled
                    Credentials = new NetworkCredential(fromEmail, pass),
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                smtpClient.Send(message);

                // Success
                msg = "2";
            }
        }
        catch (Exception ex)
        {
            msg = "4";
        }

        return msg;
    }
}
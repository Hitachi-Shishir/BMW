using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;

/// <summary>
/// Summary description for Write2DB
/// </summary>
public class SendMailConfig
{

    public string PopulateBody(string From, string To, string Summary)
    {

        string body = string.Empty;
        using (StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Server.MapPath("~/SDTemplates/EmailTest.htm")))
        {
            body = reader.ReadToEnd();
        }

        body = body.Replace("{From}", From);
        body = body.Replace("{To}", To);
        body = body.Replace("{Subject}", Summary);



        return body;
    }
    public void Sendmail(string body)
    {
        System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
        //Fetching Settings from WEB.CONFIG file.  
        string emailSender = ConfigurationManager.AppSettings["UserName"].ToString();
        string emailSenderPassword = ConfigurationManager.AppSettings["Password"].ToString();
        string emailSenderHost = ConfigurationManager.AppSettings["Host"].ToString();
        int emailSenderPort = Convert.ToInt16(ConfigurationManager.AppSettings["Port"]);
        Boolean emailIsSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
        var server = new SmtpClient("localhost");

        server.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;

        //Fetching Email Body Text from EmailTemplate File.  
        string FilePath = System.Web.HttpContext.Current.Server.MapPath("~/SDTemplates/EmailTest.htm");
        StreamReader str = new StreamReader(FilePath);
        string MailText = str.ReadToEnd();
        str.Close();

        //Repalce [newusername] = signup user name   
        //MailText = MailText.Replace(body);


        string subject = "Mail From DB";

        //Base class for sending email  
        System.Net.Mail.MailMessage _mailmsg = new System.Net.Mail.MailMessage();

        //Make TRUE because our body text is html  
        _mailmsg.IsBodyHtml = true;

        //Set From Email ID  
        _mailmsg.From = new MailAddress(emailSender);

        //Set To Email ID  
        _mailmsg.To.Add("anuj.dogra.fz@hitachi-systems.com");

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




    }
}
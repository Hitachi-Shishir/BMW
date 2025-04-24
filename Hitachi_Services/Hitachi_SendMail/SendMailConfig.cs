using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IO;

namespace Hitachi_SendMail
{
	
		public class SendMailConfig
		{
			public string PopulateBody(string From, string To, string Summary)
			{
				string body = string.Empty;
			//using (StreamReader reader = new StreamReader(Server.MapPath(@"D:/ServiceDeskLive/SDTemplates/EmailTest.htm")))
			//{
			//	body = reader.ReadToEnd();
			//}
			string AppLocation = "";
			AppLocation = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
			AppLocation = AppLocation.Replace("file:\\", "");
			string file = AppLocation + "\\EmailTemplate\\EmailTest.htm";
			using (StreamReader reader = new StreamReader(System.IO.Path.GetFullPath(file)))
			{
				body = reader.ReadToEnd();
				body = body.Replace("{From}", From);
				body = body.Replace("{To}", To);
				body = body.Replace("{Subject}", Summary);
			}
			return body;
			//body = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/SDTemplates/EmailTest.htm"));
			//body = body.Replace("{From}", From);
			//	body = body.Replace("{To}", To);
			//	body = body.Replace("{Subject}", Summary);



			//	return body;
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
				//string FilePath = File.ReadAllText(@"D:\\ServiceDeskLive\\SDTemplates\\EmailTest.htm");
				//StreamReader str = new StreamReader(FilePath);
				//string MailText = str.ReadToEnd();
				//str.Close();
			//	string MailText = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("~/SDTemplates/EmailTest.htm"));
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
				try
				{
					_smtp.Send(_mailmsg);
				}
				catch (SmtpFailedRecipientException ex)
				{

				}




			}
		}
	
}

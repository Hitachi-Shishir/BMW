using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.IO;

namespace Hitachi_SendMail
{
    public partial class Hitachi_SendMail : ServiceBase
    {

        public Hitachi_SendMail()
        {
            InitializeComponent();
           
        }
        Timer timer = new Timer();
        protected override void OnStart(string[] args)
        {
            //StartMonitoring();
            //timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            //timer.Interval = 120000; //number in milisecinds  
            //timer.Enabled = true;

            // Initialize timer
            timer = new Timer();
            timer.Interval = 60000; // 1 minute interval (adjust as needed)
            timer.Elapsed += TimerElapsed;
            timer.Start();
        }
        protected override void OnStop()
        {
            // Stop the timer
            timer.Stop();
            timer.Dispose();
        }
        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    using (SqlCommand selectCommand = new SqlCommand(@"sp_GetMailToSend", connection))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                        {
                            using (DataTable dataTable = new DataTable())
                            {
                                sqlDataAdapter.Fill(dataTable);
                                if (dataTable.Rows.Count <= 0)
                                    return;
                                for (int index = 0; index < dataTable.Rows.Count; ++index)
                                    this.SendMailToUser(dataTable.Rows[index]["id"].ToString(), dataTable.Rows[index]["to"].ToString(), dataTable.Rows[index]["subject"].ToString(), dataTable.Rows[index]["bodyContent"].ToString(), dataTable.Rows[index]["cc"].ToString(), dataTable.Rows[index]["UserName"].ToString(), dataTable.Rows[index]["Port"].ToString(), dataTable.Rows[index]["Hostname"].ToString(), dataTable.Rows[index]["Password"].ToString());
                            }
                        }
                    }
                }
                // Connect to SQL database
                //string connectionString = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                //using (SqlConnection connection = new SqlConnection(connectionString))
                //{
                //	connection.Open();

                //	// Execute SQL query to retrieve new rows
                //	string query = "SELECT * FROM email  WHERE sendStatus = 0"; // Adjust the table and column names
                //	SqlCommand command = new SqlCommand(query, connection);
                //	SqlDataReader reader = command.ExecuteReader();

                //	while (reader.Read())
                //	{
                //		// Extract information from the row
                //		string subject = reader["Subject"].ToString(); // Adjust column names
                //		string body = reader["Body"].ToString(); // Adjust column names

                //		// Send email

                //		SendMailConfig sm = new SendMailConfig();
                //		sm.Sendmail(sm.PopulateBody(reader["From"].ToString(), reader["To"].ToString(), reader["Subject"].ToString()));

                //		// Mark the row as processed
                //		MarkRowAsProcessed(connection, (int)reader["id"]); // Adjust column names
                //	}

                //	reader.Close();
                //}
            }
            catch (Exception ex)
            {
                ErrorLogging(ex);

                // Handle any exceptions
                // You can log the error or perform any necessary actions
            }
        }
        public string GetConnectstring()
        {
            return ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        }
        public void ExecuteNonQuery(string Sql)
        {
            using (SqlConnection cnn = new SqlConnection(GetConnectstring()))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 3600;
                cmd.CommandText = Sql;
                int id=cmd.ExecuteNonQuery();
                cnn.Close();
            }
        }
        public object GetScalarValue(string sql, SqlConnection cnn)
        {
            SqlCommand cmd = new SqlCommand(sql, cnn);
            cmd.CommandTimeout = 3600;
            object obj = cmd.ExecuteScalar();
            return obj;
        }
        public object GetScalarValue(string sql)
        {
            object o;
            using (SqlConnection cnn = new SqlConnection(GetConnectstring()))
            {
                cnn.Open();
                o = GetScalarValue(sql, cnn);
                cnn.Close();
            }
            return o;
        }
        public string fn = System.Configuration.ConfigurationManager.AppSettings["LOGPATH"].ToString();
        public void Log(string msg)
        {
            try
            {
                FileStream filestrm = new FileStream(fn + @"LogErr_" + DateTime.Now.ToString("ddMMMyyyyHH") + ".txt", FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter strmwriter = new StreamWriter(filestrm);
                strmwriter.BaseStream.Seek(0, SeekOrigin.End);
                strmwriter.WriteLine(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:ffff") + "_" + msg);
                strmwriter.Flush();
                strmwriter.Close();
            }
            catch (Exception ex)
            {

            }
        }
        public void UpdateProccesstime(string id)
        {
            try
            {
                string sql = "update email set PickedTime=GETDATE() where id='" + id + "' ";
                ExecuteNonQuery(sql);
                Log("UpdateProccesstime : " + id);
            }
            catch (Exception ex)
            {
                Log(ex.Message + " : " + id);
            }

        }
        private void SendMailToUser(string ID, string recepientEmail, string subject, string body, string cc, string FromEmail, string port, string host, string pass)
        {
            try
            {
                UpdateProccesstime(ID);
                using (MailMessage message = new MailMessage())
                {
                    message.From = new MailAddress(FromEmail, FromEmail);
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true;
                    string str = recepientEmail;
                    char[] chArray = new char[1] { ',' };
                    foreach (string address in str.Split(chArray))
                        message.To.Add(new MailAddress(address));
                    string strCC = cc;
                    if (string.IsNullOrEmpty(cc))
                    { }
                    else
                    {
                        char[] chArrayCC = new char[1] { ',' };
                        foreach (string addressCC in strCC.Split(chArrayCC))
                            message.CC.Add(new MailAddress(addressCC));
                    }
                    System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                    SmtpClient smtpClient = new SmtpClient();
                    smtpClient.Host = host;
                    smtpClient.EnableSsl = Convert.ToBoolean("true");

                    NetworkCredential networkCredential = new NetworkCredential();
                    networkCredential.UserName = FromEmail;
                    networkCredential.Password = pass;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = (ICredentialsByHost)networkCredential;
                    smtpClient.Port = int.Parse(port);
                    smtpClient.Send(message);
                    this.UpdateStatus(ID);
                }

                //System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
                ////Fetching Settings from WEB.CONFIG file.  
                //string emailSender = ConfigurationManager.AppSettings["UserName"].ToString();
                //string emailSenderPassword = ConfigurationManager.AppSettings["Password"].ToString();
                //string emailSenderHost = ConfigurationManager.AppSettings["Host"].ToString();
                //int emailSenderPort = Convert.ToInt16(ConfigurationManager.AppSettings["Port"]);
                //Boolean emailIsSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSsl"]);
                //var server = new SmtpClient("localhost");

                //server.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;



                //string subject1 = subject;

                ////Base class for sending email  
                //System.Net.Mail.MailMessage _mailmsg = new System.Net.Mail.MailMessage();

                ////Make TRUE because our body text is html  
                //_mailmsg.IsBodyHtml = true;

                ////Set From Email ID  
                //_mailmsg.From = new MailAddress(emailSender);

                ////Set To Email ID  
                //_mailmsg.To.Add("anuj.dogra.fz@hitachi-systems.com");

                ////Set Subject  
                //_mailmsg.Subject = subject1;

                ////Set Body Text of Email   
                //_mailmsg.Body = body;


                ////Now set your SMTP   
                //SmtpClient _smtp = new SmtpClient();

                ////Set HOST server SMTP detail  
                //_smtp.Host = emailSenderHost;

                ////Set PORT number of SMTP  
                //_smtp.Port = emailSenderPort;

                ////Set SSL --> True / False  
                //_smtp.EnableSsl = emailIsSSL;

                ////Set Sender UserEmailID, Password  
                //NetworkCredential _network = new NetworkCredential(emailSender, emailSenderPassword);
                //_smtp.Credentials = _network;
                //UpdateStatus(ID);
            }
            catch (Exception ex)
            {
                string sql1 = "select max(RetryCount) from email where ID='" + ID + "'";
                string RetryCount = Convert.ToString(GetScalarValue(sql1));
                int i = 1;
                if (RetryCount != null || RetryCount != "")
                {
                    i = Convert.ToInt32(RetryCount) + i;
                }
                this.UpdateFailStatus(ID, i);
                ErrorLogging(ex);
            }
        }
        private void UpdateFailStatus(string ID, int cnt)
        {
            try
            {
                string sql = "Update email set sendStatus=4,[sent]=GETDATE() , RetryCount='" + cnt + "', PickedTime=null where id = '"+ ID + "'";
                sql = sql + "   Insert into emailLog select * from email where id='"+ ID + "'";
                ExecuteNonQuery(sql);


                //using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                //{
                //    using (SqlCommand sqlCommand = new SqlCommand(sql))
                //    {
                //        sqlCommand.CommandType = CommandType.Text;
                //        sqlCommand.Parameters.AddWithValue("@id", (object)ID);
                //        sqlCommand.Connection = sqlConnection;
                //        sqlConnection.Open();
                //        sqlCommand.ExecuteNonQuery();
                //        sqlConnection.Close();
                //    }
                //}
            }
            catch (Exception ex)
            {
                ErrorLogging(ex);
                Environment.Exit(0);
            }
        }
        private void UpdateStatus(string ID)
        {
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    using (SqlCommand sqlCommand = new SqlCommand("Update email set sendStatus=2,[sent]=GETDATE() where id=@id"))
                    {
                        sqlCommand.CommandType = CommandType.Text;
                        sqlCommand.Parameters.AddWithValue("@id", (object)ID);
                        sqlCommand.Connection = sqlConnection;
                        sqlConnection.Open();
                        sqlCommand.ExecuteNonQuery();
                        sqlConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogging(ex);
                Environment.Exit(0);
            }
        }
        public static void ErrorLogging(Exception ex)
        {
            string path = "C:\\temp\\Log.txt";
            if (!System.IO.File.Exists(path))
                System.IO.File.Create(path).Dispose();
            using (StreamWriter streamWriter1 = System.IO.File.AppendText(path))
            {
                streamWriter1.WriteLine("=============Error Logging ===========");
                StreamWriter streamWriter2 = streamWriter1;
                DateTime now = DateTime.Now;
                string str1 = "===========Start============= " + now.ToString();
                streamWriter2.WriteLine(str1);
                streamWriter1.WriteLine("Error Message: " + ex.Message);
                streamWriter1.WriteLine("Stack Trace: " + ex.StackTrace);
                StreamWriter streamWriter3 = streamWriter1;
                now = DateTime.Now;
                string str2 = "===========End============= " + now.ToString();
                streamWriter3.WriteLine(str2);
            }
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            //StartMonitoring();
            //TimerElapsed();
        }

    }
}

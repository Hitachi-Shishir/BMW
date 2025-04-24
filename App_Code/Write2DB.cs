using EAGetMail;
using System;
using System.Configuration;
using System.Data.SqlClient;

/// <summary>
/// Summary description for Write2DB
/// </summary>
public class Write2DB
{


    SqlConnection conn = null;
    public Write2DB()
    {
        Console.WriteLine("Connecting to SQL Server");
        try
        {
            string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            conn = new SqlConnection(constr);
            conn.Open();
            Console.WriteLine("Connected");
        }
        catch (System.Data.SqlClient.SqlException e)
        {
            throw (e);
        }
    }
    public void Save(Mail oMail)
    {

        SqlCommand cmd = new SqlCommand("dbo.SD_spMailFromServerInbox", conn)
        {
            CommandType = System.Data.CommandType.StoredProcedure,
            CommandTimeout = 1500
        };
        //		MailReport mailReport = oMail.GetReport();
        string recipients = "";
        EAGetMail.MailAddress[] addrs = oMail.Cc;
        for (int i = 0; i < addrs.Length; i++)
        {
            recipients += ";" + addrs[i].Address.ToString();

        }

        //	cmd.Parameters.AddWithValue("@message_id","");
        cmd.Parameters.AddWithValue("@SubmitterName", oMail.From.Name);
        cmd.Parameters.AddWithValue("@from", oMail.From.Address);
        cmd.Parameters.AddWithValue("@body", oMail.TextBody.ToString());
        cmd.Parameters.AddWithValue("@cc", recipients);
        cmd.Parameters.AddWithValue("@subject", oMail.Subject.TrimStart());

        cmd.Parameters.AddWithValue("@received_time", Convert.ToDateTime(oMail.ReceivedDate.ToString()).ToString("yyyy-MM-dd hh:mm:ss"));
        string torecipients = "";
        EAGetMail.MailAddress[] addrsTo = oMail.To;
        for (int i = 0; i < addrs.Length; i++)
        {
            torecipients += ";" + addrsTo[i].Address.ToString();

        }

        cmd.Parameters.AddWithValue("@to", torecipients);
        int res = cmd.ExecuteNonQuery();
        if (res > 0)
        {

        }
        else
        {

        }
    }

    //~Write2DB();
    //{
    //Console.WriteLine("Disconnect");
    //}
}
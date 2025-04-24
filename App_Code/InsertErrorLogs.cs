using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for InsertErrorLogs
/// </summary>
public class InsertErrorLogs : IErrorLog
{
    Random r = new Random();
    public void InsertErrorLogsF(string adminName, string Desc)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {

            using (SqlCommand cmd = new SqlCommand("SD_spAddLogs", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", r.Next());
                cmd.Parameters.AddWithValue("@adminName", adminName);
                cmd.Parameters.AddWithValue("@description", Desc);
                cmd.Parameters.AddWithValue("@Option", "InsertErrorLogs");
                con.Open();
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    //HttpContext.Current.        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",$"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
                }


            }
        }
    }


}
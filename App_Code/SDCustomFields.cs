using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for SDTemplateFileds
/// </summary>
public class SDCustomFields
{
    public DataTable CheckForPrevStatus(string TicketiD, string OrgId)
    {

        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"select StatusCodeRef from vsdticket a  WITH(NOLOCK)  
 left join SD_Status b  WITH(NOLOCK) 
 on a.sdStatusFK=b.id

where TicketNumber='" + TicketiD + "' and a.OrgId='" + OrgId + "'", con))
                {
                    cmd.CommandType = CommandType.Text;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        using (DataSet ds = new DataSet())
                        {
                            sda.Fill(ds);
                            DataTable dt = new DataTable();
                            dt = ds.Tables[0];

                            return dt;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }


    }
    public DataTable GetAssigneEmail(long Tech)
    {

        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"select EmailID from SD_Technician a  WITH(NOLOCK) 
inner join SD_User_Master b  WITH(NOLOCK) 
on a.RefUserID=b.UserID where TechID='" + Tech + "'", con))
                {
                    cmd.CommandType = CommandType.Text;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        using (DataSet ds = new DataSet())
                        {
                            sda.Fill(ds);
                            DataTable dt = new DataTable();
                            dt = ds.Tables[0];

                            return dt;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }


    }
}
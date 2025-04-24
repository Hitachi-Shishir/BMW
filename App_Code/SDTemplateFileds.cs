using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

/// <summary>
/// Summary description for SDTemplateFileds
/// </summary>
public class SDTemplateFileds
{
    public DataTable FillMergeTicketSummary(string Tickets, long orgid)
    {

        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"select TicketSummary from SDIncident WITH(NOLOCK)  where organizationFK='" + orgid + "' and  Ticketref in (SELECT Item FROM dbo.SplitString('" + Tickets + "', ','))", con))
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
    public DataTable FillHOD()
    {

        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"select distinct Concat(UserName,'-',UserEmail) as 'Name', UserEmail from SD_SRApprovers
order by UserEmail asc", con))
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
  public DataTable FillUser()
    {

        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"select Concat(EmpCode,'-',Name) as 'Name',EmpCode from SD_Employee order by name asc", con))
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
    public DataTable FillRequestType(long OrgId)
    {

        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"select distinct id,  ReqTypeRef from SD_RequestType where  OrgRef= '" + OrgId + "' and ReqTypeRef!='Knowledge Base' order by ReqTypeRef asc", con))
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
    public DataTable FillSolutiontype(string SDName, string OrgId)
    {

        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"select distinct id,  ResolutionRef,ResolutionCodeRef from SD_Resolution where DeskRef='" + SDName + "'  and OrgDeskRef='" + OrgId + "' order by ResolutionCodeRef asc", con))
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
    public DataTable FillSeverity(string SDName, string OrgID)
    {

        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"select distinct id, Serverityref,Serveritycoderef+'-'+SeverityDesc Serveritycoderef from SD_Severity where DeskRef='" + SDName + "' and OrgDeskRef='" + OrgID + "' order by Serveritycoderef asc", con))
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

                con.Close();

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }


    }
    public DataTable FillPriority(string SDName, string OrgID)
    {

        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"select distinct id, PriorityRef,PriorityCodeRef from SD_Priority where DeskRef='" + SDName + "' and OrgDeskRef='" + OrgID + "' order by PriorityCodeRef asc", con))
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
                con.Close();

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }


    }
    public DataTable FillApprovalLevel()
    {

        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"select distinct Grade from SR_Cloud order by grade asc", con))
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
    public DataTable FillStatus(string SDName, string StageFk, string OrgID)
    {

        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"select distinct id,  StatusRef,StatusCodeRef from SD_Status where DeskRef='" + SDName + "' and sd_stageFk= '" + StageFk + " ' and OrgDeskRef='" + OrgID + "' order by StatusCodeRef asc", con))
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
    public DataTable FillStatusDy(string SDName, string OrgID)
    {

        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"select distinct id,  StatusRef,StatusCodeRef from SD_Status where DeskRef='" + SDName + "' and OrgDeskRef='" + OrgID + "' order by StatusCodeRef asc", con))
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
    public DataTable FillStatusMerge(string SDName, string OrgID)
    {

        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"select distinct id,  StatusRef,StatusCodeRef from SD_Status where DeskRef='" + SDName + "' and OrgDeskRef='" + OrgID + "' order by StatusCodeRef asc", con))
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
    public DataTable FillStage(string SDName, string Orgid)
    {

        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"select distinct id,  StageRef,StageCodeRef from SD_Stage where DeskRef='" + SDName + "' and OrgDeskRef='" + Orgid + "' order by StageCodeRef asc", con))
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
    public DataTable FillCategoryAll(string OrgId, string ReqType)
    {

        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"select  max(ID) ID,CategoryCodeRef from SD_Category where OrgDeskRef='" + OrgId + "' and DeskRef='" + ReqType + "' group by CategoryCodeRef", con))
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
    public DataTable FillCategory(string SDName)
    {

        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"SELECT CategoryCodeRef,
           Categoryref FROM [dbo].fnGetCategoryFullPathForDesk('" + SDName + "', 1) where Level=1 order by Categoryref asc", con))
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
    public DataTable FillDepartment(long OrgId)
    {

        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"SD_spDepartment_Master", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrgID", OrgId);
                    cmd.Parameters.AddWithValue("@Option", "see");
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
    public DataTable FillChangeType()
    {

        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"SELECT distinct ChangeTypeRef
  FROM [SD_ChangeType]", con))
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
    public DataTable FillReasonForChange()
    {

        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"SELECT distinct ReasonTypeRef
  FROM [SD_ChangeReasonType]", con))
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
    public DataTable FillLocation(long OrgID)
    {

        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"SD_spLocation_Master", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@OrgID", OrgID);
                    cmd.Parameters.AddWithValue("@Option", "See");
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
    public DataTable FillEngineerEmail(string OrgId)
    {

        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"select Concat(UserName,'-',EmailID)as 'tech',EmailID 
from SD_vUser a inner join SD_Technician b  
on a.UserID=b.RefUserID  where OrgRef='" + OrgId + "' group by EmailID, Concat(UserName,'-',EmailID)", con))
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
    public DataTable FillAssigne(long OrgId)
    {

        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"SD_spAddTechnician", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrgRef", OrgId);
                    cmd.Parameters.AddWithValue("@Option", "GetTech");
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
    public DataTable FillCustomFieldName(long Orgid)
    {

        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"select distinct  cast( FieldName as nvarchar(500)) as 'FieldName',FieldValue from SD_CustomFieldControl where OrgRef='" + Orgid + "' order by FieldName asc", con))
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
    public DataTable FillCustomFieldDropdown(string FieldName)
    {

        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"SD_spCustomFieldCntl", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ColumnName", FieldName);
                    cmd.Parameters.AddWithValue("@Option", "GetCustomFldVal");
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
    public DataTable GetTicketCategory(long CategoryID, int rownum)
    {

        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"SD_GetUpdateTicketCategory", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
                    cmd.Parameters.AddWithValue("@RowID", rownum);
                    cmd.Parameters.AddWithValue("@Option", "GetUpdateTicket");
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
    public DataTable FillCustomFieldValueDropdown(string FieldName, string TicketNumber, string OrgID)
    {

        try
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"SD_spCustomFieldCntl", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ColumnName", FieldName);
                    cmd.Parameters.AddWithValue("@TicketRef", TicketNumber);
                    cmd.Parameters.AddWithValue("@OrgRef", OrgID);
                    cmd.Parameters.AddWithValue("@Option", "TicketWiseCustomField");
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {

                        sda.Fill(dt);


                        return dt;



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
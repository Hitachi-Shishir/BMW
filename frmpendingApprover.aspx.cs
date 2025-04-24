using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class frmpendingApprover : System.Web.UI.Page
{
    public static string TicketId;
    InsertErrorLogs inEr = new InsertErrorLogs();
    protected void Page_Load(object sender, EventArgs e)
    {
        Session["UserName"] = "Approver";
        Session["SD_OrgID"] = Session["SD_OrgID"];
        if (Session["UserID"] == null || Convert.ToString(Session["UserID"]) == "")
        {
            Response.Redirect("/Default.aspx");
        }
        if (!IsPostBack)
        {
            TicketId = Request.QueryString["TicketId"].ToString();

            GetDeskValue();
            CheckForApprovalStatus();
            lnkDownload.Visible = false;
            GetFileAttached();

        }
    }
    public static string Desk;
    public static long OrgID;
    protected void GetDeskValue()
    {
        string constr1 = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        using (SqlConnection con1 = new SqlConnection(constr1))
        {
            con1.Open();
            using (SqlCommand cmd = new SqlCommand(@"select a.* from vSD_Tickets a 
 where TicketNumber=@TicketId  and OrgID=@OrgID", con1))
            {
                cmd.Parameters.AddWithValue("@TicketId", Request.QueryString["TicketId"].ToString());
                cmd.Parameters.AddWithValue("@OrgID", Session["SD_OrgID"].ToString());
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {



                            Desk = dt.Rows[0]["ServiceDesk"].ToString();
                            FillStage();
                            ddlStage.SelectedValue = dt.Rows[0]["sdStageFK"].ToString();
                            //OrgID=Convert.ToInt64(dt.Rows[0]["SD_OrgID"].ToString());
                            FillStatus();
                            ddlStatus.SelectedValue = dt.Rows[0]["sdStatusFK"].ToString();

                            if (ddlStatus.SelectedItem.ToString().Contains("Awaiting IT Manager Approval"))
                            {
                                rdblst.Visible = true;
                                rfvrdblst.Enabled = true;
                                rfvrdblst.Visible = true;
                                lblHodApprov.Visible = true;
                                btnApprove.Text = "Update";
                            }
                            else
                            {
                                lblHodApprov.Visible = false;
                                rdblst.Visible = false;
                                rfvrdblst.Enabled = false;
                                rfvrdblst.Visible = false;
                            }
                            ddlStatus.Enabled = false;
                            ddlStage.Enabled = false;
                            ShowTicketAttachment();
                            if (Desk == "Change Request")
                            {
                                pnlShowGriddAtawithoutChange.Visible = false;
                                pnlShowGridDataWithChange.Visible = true;
                                FillSummaryWithChange();
                                pnlchangeparameters.Visible = true;
                            }
                            else
                            {
                                FillSummary();
                                pnlShowGriddAtawithoutChange.Visible = true;
                                pnlShowGridDataWithChange.Visible = false;
                                pnlchangeparameters.Visible = false;
                            }
                        }


                    }
                }
            }
        }
    }
    private void FillStage()
    {

        try
        {

            DataTable SD_Status = new SDTemplateFileds().FillStage(Desk, Convert.ToString(Session["SD_OrgID"]));

            ddlStage.DataSource = SD_Status;
            ddlStage.DataTextField = "StageCodeRef";
            ddlStage.DataValueField = "id";
            ddlStage.DataBind();
            ddlStage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----------Select Stage----------", "0"));


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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }
    }
    private void FillStatus()
    {
        try
        {
            DataTable SD_Priority = new SDTemplateFileds().FillStatus(Desk, ddlStage.SelectedValue, Convert.ToString(Session["SD_OrgID"]));
            ddlStatus.DataSource = SD_Priority;
            ddlStatus.DataTextField = "StatusCodeRef";
            ddlStatus.DataValueField = "id";
            ddlStatus.DataBind();
            ddlStatus.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----------Select Status----------", "0"));
        }
        catch (ThreadAbortException e2)
        {
            Console.WriteLine("Exception message: {0}", e2.Message);
            Thread.ResetAbort();
        }
        catch (Exception ex)
        {
            if (ex.ToString().Contains("ThreadAbortException"))
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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }

        }
    }
    public static string Filepath;
    private void GetFileAttached()
    {

        try
        {
            string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SD_spTicketFileupload", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Ticketref", Request.QueryString["TicketId"].ToString());
                    cmd.Parameters.AddWithValue("@OrgRef", Session["SD_OrgID"].ToString());
                    cmd.Parameters.AddWithValue("@Option", "see");
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                lnkDownload.Visible = true;
                                lnkDownload.Text = dt.Rows[0]["Filename"].ToString();

                                Filepath = dt.Rows[0]["Filepath"].ToString();
                            }
                        }
                    }
                    con.Close();
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
            if (ex.ToString().Contains("ThreadAbortException"))
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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }

        }
    }

    protected void lnkDownload_Click(object sender, EventArgs e)
    {

        try
        {
            if (Filepath != "")
            {
                string path = Server.MapPath(Filepath);
                //System.IO.FileInfo file = new System.IO.FileInfo(path);
                //if (file.Exists)
                //{
                //	Response.Clear();
                //	Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                //	Response.AddHeader("Content-Length", file.Length.ToString());
                //	Response.ContentType = "application/octet-stream";
                //	Response.WriteFile(file.FullName);
                //	Response.End();
                //}
                //else
                //{
                //	Response.Write("This file does not exist.");
                //}
                string filePath = Filepath;
                Response.ContentType = ContentType;
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + System.IO.Path.GetFileName(filePath));
                Response.WriteFile(filePath);
                Response.End();
            }
        }
        catch (ThreadAbortException e2)
        {
            Console.WriteLine("Exception message: {0}", e2.Message);
            Thread.ResetAbort();
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
    private void CheckForApprovalStatus()
    {
        string Approvallevel;
        string ApprovalStatus;
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {

            con.Open();
            using (SqlCommand cmd = new SqlCommand("SD_SRTicketApprovalStatus", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    using (DataTable dt = new DataTable())
                    {
                        adp.SelectCommand.CommandTimeout = 180;
                        cmd.Parameters.AddWithValue("@Ticketref", Request.QueryString["TicketId"].ToString());
                        cmd.Parameters.AddWithValue("@OrgID", Convert.ToInt64(Session["SD_OrgID"].ToString()));
                        cmd.Parameters.AddWithValue("@Option", "CheckForAppproval");
                        adp.Fill(dt);
                        if (Request.QueryString["ApproverLevel"].ToString() == "L1")
                        {
                            if (dt.Rows[0]["Approval1Status"].ToString() == "Approved" || dt.Rows[0]["Approval1Status"].ToString() == "Rejected")
                            {
                                pnl.Enabled = false;
                                txtremarks.Text = dt.Rows[0]["Approval1Remarks"].ToString();
                                img.Visible = true;
                                if (dt.Rows[0]["Approval1Status"].ToString() == "Approved")
                                {
                                    img.ImageUrl = "../Images/Approved.png";
                                }
                                else
                                {
                                    img.ImageUrl = "../Images/Rejected.png";
                                }
                            }
                            else
                            {
                                pnl.Enabled = true;
                                img.Visible = false;
                            }
                        }
                        if (Request.QueryString["ApproverLevel"].ToString() == "L2")
                        {
                            if (dt.Rows[0]["Approval2Status"].ToString() == "Approved" || dt.Rows[0]["Approval2Status"].ToString() == "Rejected")
                            {
                                pnl.Enabled = false;
                                txtremarks.Text = dt.Rows[0]["Approval2Remarks"].ToString();
                                img.Visible = true;
                                if (dt.Rows[0]["Approval2Status"].ToString() == "Approved")
                                {
                                    img.ImageUrl = "../Images/Approved.png";
                                }
                                else
                                {
                                    img.ImageUrl = "../Images/Rejected.png";
                                }
                            }
                            else
                            {
                                pnl.Enabled = true;
                                img.Visible = false;
                            }
                        }
                        if (Request.QueryString["ApproverLevel"].ToString() == "L3")
                        {
                            if (dt.Rows[0]["Approval3Status"].ToString() == "Approved" || dt.Rows[0]["Approval3Status"].ToString() == "Rejected")
                            {
                                pnl.Enabled = false;
                                txtremarks.Text = dt.Rows[0]["Approval3Remarks"].ToString();
                                img.Visible = true;
                                if (dt.Rows[0]["Approval3Status"].ToString() == "Approved")
                                {
                                    img.ImageUrl = "../Images/Approved.png";
                                }
                                else
                                {
                                    img.ImageUrl = "../Images/Rejected.png";
                                }
                            }
                            else
                            {
                                pnl.Enabled = true;
                                img.Visible = false;
                            }
                        }
                        if (Request.QueryString["ApproverLevel"].ToString() == "L4")
                        {
                            if (dt.Rows[0]["Approval4Status"].ToString() == "Approved" || dt.Rows[0]["Approval4Status"].ToString() == "Rejected")
                            {
                                pnl.Enabled = false;
                                txtremarks.Text = dt.Rows[0]["Approval4Remarks"].ToString();
                                img.Visible = true;
                                if (dt.Rows[0]["Approval4Status"].ToString() == "Approved")
                                {
                                    img.ImageUrl = "";
                                }
                                else
                                {
                                    img.ImageUrl = "";
                                }
                            }
                            else
                            {
                                pnl.Enabled = true;
                                img.Visible = false;
                            }
                        }
                        if (Request.QueryString["ApproverLevel"].ToString() == "L5")
                        {
                            if (dt.Rows[0]["Approval5Status"].ToString() == "Approved" || dt.Rows[0]["Approval5Status"].ToString() == "Rejected")
                            {
                                pnl.Enabled = false;
                                txtremarks.Text = dt.Rows[0]["Approval5Remarks"].ToString();
                                img.Visible = true;
                                if (dt.Rows[0]["Approval5Status"].ToString() == "Approved")
                                {
                                    img.ImageUrl = "../Images/Approved.png";
                                }
                                else
                                {
                                    img.ImageUrl = "../Images/Rejected.png";
                                }
                            }
                            else
                            {
                                pnl.Enabled = true;
                                img.Visible = false;
                            }
                        }
                    }
                }
            }
        }
    }
    private void FillSummary()
    {
        try
        {



            long CategoryFk;
            string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("select top 1 * from vSDTicket where TicketNumber=@TicketNumber and OrgId=@OrgID"))
                {
                    cmd.CommandType = CommandType.Text;
                    //   cmd.Parameters.AddWithValue("@TicketNumber", ddlOpenticket.SelectedValue);
                    cmd.Parameters.AddWithValue("@TicketNumber", TicketId);
                    cmd.Parameters.AddWithValue("@OrgID", Convert.ToInt64(Session["SD_OrgID"].ToString()));

                    //cmd.Parameters.AddWithValue("@TicketNumber", "IN007195");
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                DetailsCheckInAsset.DataSource = dt;
                                DetailsCheckInAsset.DataBind();
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
            if (ex.ToString().Contains("ThreadAbort"))
            {

            }
            else
            {
                var st = new StackTrace(ex, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();
                inEr.InsertErrorLogsF(""
    , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                //			ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
            //		//			
        }

    }
    private void FillSummaryWithChange()
    {
        //	try
        //	{



        long CategoryFk;
        string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(@"SELECT a.*,b.ChangeType,b.RequestForChange,DurationFrom,DurationTo,DownTimeDesc,DownTimeFrom,DownTimeTo
  FROM [vSDTicket]
   a inner join 
   SD_ChangeMngmtTickDetails b
   on a.TicketNumber=b.TicketRef where TicketNumber=@TicketNumber"))
            {
                cmd.CommandType = CommandType.Text;
                //   cmd.Parameters.AddWithValue("@TicketNumber", ddlOpenticket.SelectedValue);
                cmd.Parameters.AddWithValue("@TicketNumber", TicketId);
                //cmd.Parameters.AddWithValue("@TicketNumber", "IN007195");
                cmd.Connection = con;
                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            DetailsChange.DataSource = dt;
                            DetailsChange.DataBind();
                        }
                    }
                }
            }
        }
        FillImpactDetails();
        FillRollOutDetails();
        FilltaskDetails();
        //	}
        //	catch (ThreadAbortException e2)
        //	{
        //		Console.WriteLine("Exception message: {0}", e2.Message);
        //		Thread.ResetAbort();
        //	}
        //	catch (Exception ex)
        //	{
        //		if (ex.ToString().Contains("ThreadAbort"))
        //		{

        //		}
        //		else
        //		{
        //			var st = new StackTrace(ex, true);
        //			// Get the top stack frame
        //			var frame = st.GetFrame(0);
        //			// Get the line number from the stack frame
        //			var line = frame.GetFileLineNumber();
        //			inEr.InsertErrorLogsF(""
        //, " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
        //			ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

        //		}
        //		//			
        //	}

    }
    protected void btnApprove_Click(object sender, EventArgs e)
    {

        UpdateTicketStatus("UpdateRequest");

    }
    protected void UpdateTicketStatus(string Function)
    {
        try

        {
            string stor = "";

            if (Desk == "Service Request")
            {
                stor = "SD_SRTicketApprovalStatus_SRGodrej";
            }
            else if (Desk == "Change Request")
            {
                stor = "SD_ChangeCABApproval";
            }
            string msg = "";
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand(stor, con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Ticketref", TicketId);
                    cmd.Parameters.AddWithValue("@Remarks", txtremarks.Text);
                    cmd.Parameters.AddWithValue("@OrgId", Session["SD_OrgID"].ToString());
                    cmd.Parameters.AddWithValue("@HODApproval", rdblst.SelectedValue);
                    cmd.Parameters.AddWithValue("@ApproverLevel", Request.QueryString["ApproverLevel"].ToString());
                    cmd.Parameters.AddWithValue("@Option", Function);

                    int res = cmd.ExecuteNonQuery();

                    if (Function == "RejectRequest")
                    {
                        msg = "Request Rejected..";
                    }
                    else
                    {
                        msg = "Request Approved..";
                    }
                    if (res > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + msg + "');window.location.href='" + Request.RawUrl + "';", true);
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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }
    }
    protected void btnReject_Click(object sender, EventArgs e)
    {
        UpdateTicketStatus("RejectRequest");
    }
    protected void ddlStage_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillStatus();
    }
    private void FillImpactDetails()
    {
        try
        {

            DataTable Impact = new FillSDFields().FillImpactDetails(Request.QueryString["TicketId"].ToString());

            gvImpactGrid.DataSource = Impact;
            gvImpactGrid.DataBind();

        }
        catch (ThreadAbortException e2)
        {
            Console.WriteLine("Exception message: {0}", e2.Message);
            Thread.ResetAbort();
        }
        catch (Exception ex)
        {
            if (ex.ToString().Contains("ThreadAbortException"))
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
                //ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }

        }
    }
    private void FillRollOutDetails()
    {
        try
        {

            DataTable RollOut = new FillSDFields().FillRollOutDetails(Request.QueryString["TicketId"].ToString());
            gvRollOutDetails.DataSource = RollOut;
            gvRollOutDetails.DataBind();


        }
        catch (ThreadAbortException e2)
        {
            Console.WriteLine("Exception message: {0}", e2.Message);
            Thread.ResetAbort();
        }
        catch (Exception ex)
        {
            if (ex.ToString().Contains("ThreadAbortException"))
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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }

        }
    }
    private void FilltaskDetails()
    {
        try
        {

            DataTable TaskAssign = new FillSDFields().FillTaskAssocDetails(Request.QueryString["TicketId"].ToString());

            gvTaskDetails.DataSource = TaskAssign;
            gvTaskDetails.DataBind();

        }
        catch (ThreadAbortException e2)
        {
            Console.WriteLine("Exception message: {0}", e2.Message);
            Thread.ResetAbort();
        }
        catch (Exception ex)
        {
            if (ex.ToString().Contains("ThreadAbortException"))
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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }

        }
    }
    protected void ShowTicketAttachment()
    {
        try
        {
            DataTable SD_TicketAttach = new FillSDFields().ShowTicketAttachment(Request.QueryString["TicketId"].ToString(), Session["SD_OrgID"].ToString());
            if (SD_TicketAttach.Rows.Count > 0 && SD_TicketAttach != null)
            {
                this.gvTicketAttachment.DataSource = (object)SD_TicketAttach;
                this.gvTicketAttachment.DataBind();
            }
            else
            {
                this.gvTicketAttachment.DataSource = (object)null;
                this.gvTicketAttachment.DataBind();
            }
        }
        catch (ThreadAbortException e2)
        {
            Console.WriteLine("Exception message: {0}", e2.Message);
            Thread.ResetAbort();
        }
        catch (Exception ex)
        {
            if (ex.ToString().Contains("ThreadAbortException"))
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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }

        }

    }

    protected void gvTicketAttachment_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            WebClient req = new WebClient();
            HttpResponse response = HttpContext.Current.Response;
            string filePath = e.CommandArgument.ToString();
            LinkButton lnk = (LinkButton)e.CommandSource as LinkButton;
            GridViewRow row = lnk.NamingContainer as GridViewRow;
            string pagename = gvTicketAttachment.DataKeys[row.RowIndex].Value.ToString();


            // Ensure that the Filepath is not null or empty
            if (!string.IsNullOrEmpty(filePath))
            {
                // Map the relative file path to a physical path
                string path = Server.MapPath(filePath);

                // Ensure the file exists before attempting to download
                if (System.IO.File.Exists(path))
                {
                    // Get the file name and content type
                    string fileName = Path.GetFileName(filePath);
                    string contentType = GetContentType(fileName); // Ensure this method returns correct MIME types

                    // Prepare the response for downloading the file
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                    Response.AddHeader("Content-Length", new FileInfo(path).Length.ToString());
                    Response.ContentType = contentType;

                    // Use TransmitFile for zip files, otherwise use WriteFile
                    if (contentType == "application/zip")
                    {
                        // Efficient for large files
                        Response.TransmitFile(path);
                    }
                    else
                    {
                        // Write the file to the response
                        Response.WriteFile(path);
                    }

                    Response.Flush(); // Send the data to the client

                    // Instead of Response.End(), use CompleteRequest to avoid ThreadAbortException
                    Response.SuppressContent = true;
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                else
                {
                    // Handle the case where the file doesn't exist
                    throw new FileNotFoundException("File not found: " + path);
                }
            }
            else
            {
                throw new ArgumentException("File path is empty or null.");
            }
        }
        catch (ThreadAbortException e2)
        {
            Console.WriteLine("Exception message: {0}", e2.Message);
            Thread.ResetAbort();
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
    private string GetContentType(string filePath)
    {
        string extension = Path.GetExtension(filePath).ToLower();
        switch (extension)
        {
            case ".pdf":
                return "application/pdf";
            case ".xls":
            case ".xlsx":
                return "application/vnd.ms-excel";
            case ".doc":
            case ".docx":
                return "application/msword";
            case ".txt":
                return "text/plain";
            case ".zip":
                return "application/zip";
            default:
                return "application/octet-stream";
        }
    }
}
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


public partial class frmApprover : System.Web.UI.Page
{
    public static string TicketId;
    InsertErrorLogs inEr = new InsertErrorLogs();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            TicketId = Request.QueryString["TicketId"].ToString();
            Session["User_Name"] = "Anuj";
            GetDeskValue();
            CheckForApprovalStatus();

        }
    }
    public static string Desk;
    protected void GetDeskValue()
    {
        string constr1 = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        using (SqlConnection con1 = new SqlConnection(constr1))
        {
            con1.Open();
            using (SqlCommand cmd = new SqlCommand("select * from  vSDTicket where TicketNumber=@TicketId and OrgID=@OrgID ", con1))
            {
                cmd.Parameters.AddWithValue("@TicketId", Request.QueryString["TicketId"].ToString());
                cmd.Parameters.AddWithValue("@OrgID", Request.QueryString["OrgID"].ToString());
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
                            FillStatus();
                            ddlStatus.SelectedValue = dt.Rows[0]["sdStatusFK"].ToString();
 ShowTicketAttachment();
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

            DataTable SD_Status = new SDTemplateFileds().FillStage(Desk, Request.QueryString["OrgID"].ToString());

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
                inEr.InsertErrorLogsF(Session["User_Name"].ToString()
    , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                Response.Redirect("~/Error/Error.html");

            }
        }
    }
    private void FillStatus()
    {

        try
        {

            DataTable SD_Priority = new SDTemplateFileds().FillStatus(Desk, ddlStage.SelectedValue, Request.QueryString["OrgID"].ToString());

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
                inEr.InsertErrorLogsF(Session["User_Name"].ToString()
    , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                Response.Redirect("~/Error/Error.html");

            }

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
                        cmd.Parameters.AddWithValue("@OrgID", Request.QueryString["OrgID"].ToString());
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
                                    img.ImageUrl = "https://pcv-demo.hitachi-systems-mc.com:5723/Images/Approved.png";
                                }
                                else
                                {
                                    img.ImageUrl = "https://pcv-demo.hitachi-systems-mc.com:5723/Images/Rejected.png";
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
                                    img.ImageUrl = "https://pcv-demo.hitachi-systems-mc.com:5723/Images/Approved.png";
                                }
                                else
                                {
                                    img.ImageUrl = "https://pcv-demo.hitachi-systems-mc.com:5723/Images/Rejected.png";
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
                                    img.ImageUrl = "https://pcv-demo.hitachi-systems-mc.com:5723/Images/Approved.png";
                                }
                                else
                                {
                                    img.ImageUrl = "https://pcv-demo.hitachi-systems-mc.com:5723/Images/Rejected.png";
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
        //	try
        //	{



        long CategoryFk;
        string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("select top 1 * from vSDTicket where TicketNumber=@TicketNumber and OrgID=@OrgID"))
            {
                cmd.CommandType = CommandType.Text;
                //   cmd.Parameters.AddWithValue("@TicketNumber", ddlOpenticket.SelectedValue);
                cmd.Parameters.AddWithValue("@OrgID", Request.QueryString["OrgID"].ToString());
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
                            DetailsCheckInAsset.DataSource = dt;
                            DetailsCheckInAsset.DataBind();
                        }
                    }
                }
            }
        }
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
        //			Response.Redirect("~/Error/Error.html");

        //		}
        //		//			
        //	}

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
  FROM vSDTicket
   a inner join 
   SD_ChangeMngmtTickDetails b
   on a.TicketNumber=b.TicketRef and a.OrgId=b.OrgID where TicketNumber=@TicketNumber and a.OrgID=@OrgID "))
            {
                cmd.CommandType = CommandType.Text;
                //   cmd.Parameters.AddWithValue("@TicketNumber", ddlOpenticket.SelectedValue);
                cmd.Parameters.AddWithValue("@OrgID", Request.QueryString["OrgID"].ToString());
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
        //			Response.Redirect("~/Error/Error.html");

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
        //try

        //{
        string stor = "";

        if (Desk == "Service Request")
        {
            
                stor = "SD_SRTicketApprovalStatus_Manual";
            
        }
        else if (Desk == "Change Request")
        {
          
                stor = "SD_SRTicketApprovalStatus_CR";
            
        }
        else if (Desk == "Cloud Process")
        {
            stor = "SD_SRTicketApprovalStatusForCloud";

        }
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {

            using (SqlCommand cmd = new SqlCommand(stor, con))
            {
                con.Open();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Ticketref", TicketId);
                cmd.Parameters.AddWithValue("@Remarks", txtremarks.Text);
                cmd.Parameters.AddWithValue("@HODApproval", rdblst.SelectedValue);
                cmd.Parameters.AddWithValue("@OrgId", Request.QueryString["OrgID"].ToString());
                cmd.Parameters.AddWithValue("@ApproverLevel", Request.QueryString["ApproverLevel"].ToString());
                cmd.Parameters.AddWithValue("@Option", Function);

                int res = cmd.ExecuteNonQuery();

                if (res > 0)
                {
                    Session["Popup"] = "Insert";
                    Response.Redirect(Request.Url.AbsoluteUri);
                }

            }
        }
        //	}
        //	catch (ThreadAbortException e2)
        //	{
        //		Console.WriteLine("Exception message: {0}", e2.Message);
        //		Thread.ResetAbort();
        //	}
        //	catch (Exception ex)
        //	{
        //		if (ex.ToString().Contains("System.Threading.Thread.AbortInternal()"))
        //		{

        //		}
        //		else
        //		{
        //			var st = new StackTrace(ex, true);
        //			// Get the top stack frame
        //			var frame = st.GetFrame(0);
        //			// Get the line number from the stack frame
        //			var line = frame.GetFileLineNumber();
        //			inEr.InsertErrorLogsF(Session["User_Name"].ToString()
        //, " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
        //			Response.Redirect("~/Error/Error.html");

        //		}
        //	}
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

            DataTable Impact = new FillSDFields().FillImpactDetails(Request.QueryString["TicketId"].ToString(), Request.QueryString["OrgID"].ToString());

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
                inEr.InsertErrorLogsF(Session["User_Name"].ToString()
    , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                Response.Redirect("~/Error/Error.html");

            }

        }
    }
    private void FillRollOutDetails()
    {
        try
        {

            DataTable RollOut = new FillSDFields().FillRollOutDetails(Request.QueryString["TicketId"].ToString(), Request.QueryString["OrgID"].ToString());
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
                inEr.InsertErrorLogsF(Session["User_Name"].ToString()
    , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                Response.Redirect("~/Error/Error.html");

            }

        }
    }
    private void FilltaskDetails()
    {
        try
        {

            DataTable TaskAssign = new FillSDFields().FillTaskAssocDetails(Request.QueryString["TicketId"].ToString(), Request.QueryString["OrgID"].ToString());

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
                inEr.InsertErrorLogsF(Session["User_Name"].ToString()
    , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                Response.Redirect("~/Error/Error.html");

            }

        }
    }
    protected void ShowTicketAttachment()
    {


        try
        {

            DataTable SD_TicketAttach = new FillSDFields().ShowTicketAttachment(Request.QueryString["TicketId"].ToString(), Request.QueryString["OrgId"].ToString()); ;

            if (SD_TicketAttach.Rows.Count > 0)
            {
                //  this.lb.Text = dataTable.Rows.Count.ToString();
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
                Response.Redirect("~/Error/Error.html");

            }

        }

    }

    protected void gvTicketAttachment_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Download")
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

        if (e.CommandName == "View")
        {

            WebClient req = new WebClient();
            HttpResponse response = HttpContext.Current.Response;
            string filePath = e.CommandArgument.ToString();
            LinkButton lnk = (LinkButton)e.CommandSource as LinkButton;
            GridViewRow row = lnk.NamingContainer as GridViewRow;
            string fileName = Path.GetFileName(filePath);
            string path = Server.MapPath(filePath);
            if (System.IO.File.Exists(path))
            {
                Response.Clear();

                string mimeType = GetMimeType(fileName);


                if (mimeType == "application/vnd.ms-excel" || mimeType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" || mimeType == "application/zip")
                {

                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                    Response.AddHeader("Content-Length", new FileInfo(path).Length.ToString());
                    Response.ContentType = mimeType;


                    if (mimeType == "application/zip")
                    {

                        Response.TransmitFile(path);
                    }
                    else
                    {

                        Response.WriteFile(path);
                    }

                    Response.Flush();


                    Response.SuppressContent = true;
                    HttpContext.Current.ApplicationInstance.CompleteRequest();
                }
                else
                {

                    Response.Clear();
                    Response.ContentType = mimeType;
                    Response.AddHeader("Content-Disposition", "inline; filename={fileName}");
                    Response.TransmitFile(path);
                }

                Response.End();


            }
            else
            {
                // Handle file not found
                Response.Write("<script>alert('File not found.');</script>");
            }
        }
    }
    public string GetMimeType(string fileName)
    {
        string extension = Path.GetExtension(fileName).ToLower();

        switch (extension)
        {
            // Text File Types
            case ".txt": return "text/plain";
            case ".sql": return "text/plain";
            case ".html":
            case ".htm": return "text/html";
            case ".css": return "text/css";
            case ".js": return "text/javascript";
            case ".xml": return "text/xml";
            case ".csv": return "text/csv";

            // Image File Types
            case ".png": return "image/png";
            case ".jpg":
            case ".jpeg": return "image/jpeg";
            case ".gif": return "image/gif";
            case ".bmp": return "image/bmp";
            case ".tiff": return "image/tiff";
            case ".webp": return "image/webp";
            case ".svg": return "image/svg+xml";
            case ".ico": return "image/x-icon";
            case ".heif": return "image/heif";
            case ".heic": return "image/heic";

            // Audio File Types
            case ".mp3": return "audio/mpeg";
            case ".wav": return "audio/wav";
            case ".ogg": return "audio/ogg";
            case ".m4a": return "audio/mp4";
            case ".aac": return "audio/aac";
            case ".flac": return "audio/flac";
            case ".webm": return "audio/webm";
            case ".opus": return "audio/opus";
            case ".mid":
            case ".midi": return "audio/x-midi";



            // Document File Types
            case ".pdf": return "application/pdf";
            case ".doc": return "application/msword";
            case ".docx": return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            case ".xls": return "application/vnd.ms-excel";
            case ".xlsx": return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            case ".ppt": return "application/vnd.ms-powerpoint";
            case ".pptx": return "application/vnd.openxmlformats-officedocument.presentationml.presentation";
            case ".rtf": return "application/rtf";
            case ".zip": return "application/zip";
            case ".rar": return "application/x-rar-compressed";
            case ".tar": return "application/x-tar";
            case ".gz": return "application/gzip";
            case ".7z": return "application/x-7z-compressed";
            case ".bz": return "application/x-bzip";
            case ".bz2": return "application/x-bzip2";

            // Font File Types
            case ".otf": return "font/otf";
            case ".woff": return "font/woff";
            case ".woff2": return "font/woff2";
            case ".ttf": return "font/ttf";
            case ".eot": return "application/vnd.ms-fontobject";



            // Other Files
            case ".xhtml": return "application/xhtml+xml";

            // Default Case (If MIME type is unknown)
            default: return "application/octet-stream";
        }
    }
  private string GetContentType(string fileExtension)
    {
        switch (fileExtension.ToLower())
        {
            case ".docx":
                return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            case ".pdf":
                return "application/pdf";
            case ".xlsx":
                return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            case ".txt":
                return "text/plain";
            default:
                return "application/octet-stream";
        }
    }
}
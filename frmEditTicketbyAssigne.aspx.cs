using DocumentFormat.OpenXml.Math;
using OpenPop.Mime.Header;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IdentityModel.Claims;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class frmEditTicketbyAssigne : System.Web.UI.Page
{
    InsertErrorLogs inEr = new InsertErrorLogs();
    public string GetDatabaseName()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
        return builder.InitialCatalog;
    }
    public string TicketId;
    public string DeskRef;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null & Session["LoginName"] == null)
        {
            Response.Redirect("/Default.aspx");
        }
        if (Convert.ToString(Request.QueryString["TicketId"]) == "")
        {
            Response.Redirect("/Default.aspx");
        }
        try
        {
            rfvddlResoultion.Enabled = false;
            rfvtxtResolution.Enabled = false;


            if (!IsPostBack)
            {
                if (Session["SD_OrgID"] != null && Session["UserScope"] != null && Session["UserName"] != null && Session["SD_OrgID"] != null)
                {
                    if (Request.QueryString["redirected"] == "true")
                    {
                        Response.AppendHeader("Refresh", "1;url=frmEditTicketbyAssigne.aspx?TicketId=" + Request.QueryString["TicketId"] + "&redirected=false&Desk=" + Request.QueryString["Desk"] + "&NamelyId=" + Request.QueryString["NamelyId"] + "");
                    }
                    if (ddlRequestType.SelectedValue == "Incident")
                    {
                        rfvddlAssigne.Enabled = true;
                        rfvddlAssigne.Visible = true;
                    }
                    else
                    {
                        rfvddlAssigne.Enabled = false;
                        rfvddlAssigne.Visible = false;
                    }

                    Session["AssigneUpdate"] = "False";
                    FillServDesk();
                    txtSummary.Enabled = false;

                    txtDescription.Attributes.Add("disabled", "true");

                    //TicketId = System.Net.WebUtility.HtmlDecode(HttpContext.Current.Request.QueryString["TicketId"]);
                    TicketId = Request.QueryString["TicketId"].ToString();
                    DeskRef = Request.QueryString["Desk"].ToString();
                    FillResolutionDetails();
                    FillDepartment();
                    FillLocation();
                    ddlCategory1.ClearSelection();
                    ddlCategory2.ClearSelection();
                    ddlCategory3.ClearSelection();
                    ddlCategory4.ClearSelection();
                    ddlCategory5.ClearSelection();
                    ddlCategory6.ClearSelection();
                    ddlSeverity.ClearSelection();
                    ddlPriority.ClearSelection();
                    ddlRequestType.Items.FindByText(DeskRef).Selected = true;
                    ddlRequestType_SelectedIndexChanged(sender, e);
                    FillAssignee();
                    FillAssigneeForChange();
                    FillSummary();
                    FillHODEmail();
                    UpdateTicketPanel();
                    lblTicket.Text = " -Number : " + TicketId;
GetHODofTicket();
                    AssigneClick = 0;
                    if (Session["UserScope"].ToString() == "Technician")
                    {
                        ddlAssigne.Enabled = false;
                        rfvddlAssigne.Enabled = true;
                        rfvddlAssigne.Visible = true;
                    }
                    else
                    {
                        ddlAssigne.Enabled = true;
                    }

                    AddDefaultFirstRecordForImpact();
                    AddDefaultFirstRecordForRollOut();
                    AddDefaultFirstRecordForTask();
                    if (DeskRef == "Change Request")
                    {
                        btnImpactDetails.Visible = true;
                        btnRolloutPlan.Visible = true;
                        btnDowntime.Visible = true;
                        btnTaskAssociation.Visible = true;
                        pnlShowRollOutDetails.Visible = false;
                        //pnlAddRollout.Visible = false;
                        //	pnlRollOutGrid.Visible = false;
                        pnlIncident.Visible = true;
                        pnlShowImpactDetails.Visible = false;
                        //pnlImpactGrid.Visible = false;
                        //pnlAddImpact.Visible = false;
                        pnlDownTime.Visible = false;
                        pnlTaksAssociation.Visible = false;
                    }
                    else
                    {
                        btnImpactDetails.Visible = false;
                        btnRolloutPlan.Visible = false;
                        btnDowntime.Visible = false;
                        btnTaskAssociation.Visible = false;
                        pnlShowRollOutDetails.Visible = false;
                        //pnlAddRollout.Visible = false;
                        //	pnlRollOutGrid.Visible = false;
                        pnlIncident.Visible = true;
                        pnlShowImpactDetails.Visible = false;
                        //pnlImpactGrid.Visible = false;
                        //pnlAddImpact.Visible = false;
                        pnlDownTime.Visible = false;
                        pnlTaksAssociation.Visible = false;


                    }
                }


                else
                {
                    Response.Redirect("/Default.aspx");
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


            }
        }


    }
private void GetHODofTicket()
{

	try
	{
		string constr = ConfigurationManager.ConnectionStrings["SDcon"].ConnectionString;
		using (SqlConnection con = new SqlConnection(constr))
		{
			using (SqlCommand cmd = new SqlCommand(@"select top 1 Approval1Email from SD_SRApprovalStatus where TicketRef=@Ticketref", con))
			{
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddWithValue("@Ticketref", Request.QueryString["TicketId"].ToString());
			//	cmd.Parameters.AddWithValue("@OrgRef", Request.QueryString["NamelyId"].ToString());
			//	cmd.Parameters.AddWithValue("@Option", "see");
				cmd.Connection = con;
				con.Open();
				using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
				{
					using (DataTable dt = new DataTable())
					{
						sda.Fill(dt);
						if (dt.Rows.Count > 0)
						{

							if (ddlHodApproval.Items.FindByValue(dt.Rows[0]["Approval1Email"].ToString()) != null)
							{
								ddlHodApproval.SelectedValue = dt.Rows[0]["Approval1Email"].ToString();
							}
					

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
			Response.Redirect("~/Error/Error.html");

		}

	}
}
    private void FillHODEmail()
    {
        try
        {
            DataTable FillAssigne = new SDTemplateFileds().FillHOD();

            ddlHodApproval.DataSource = FillAssigne;
             ddlHodApproval.DataTextField = "Name";
            ddlHodApproval.DataValueField = "UserEmail";
            ddlHodApproval.DataBind();
            ddlHodApproval.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----------Select HOD Email----------", "0"));


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
    public static string DeskName;
    private void FillDepartment()
    {
        try
        {
            DataTable FillDepartment = new SDTemplateFileds().FillDepartment(Convert.ToInt64(Request.QueryString["NamelyId"].ToString()));

            ddlDepartment.DataSource = FillDepartment;
            ddlDepartment.DataTextField = "DepartmentName";
            ddlDepartment.DataValueField = "DepartmentCode";
            ddlDepartment.DataBind();
            ddlDepartment.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----Select----", "0"));


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
    private void FillLocation()
    {
        try
        {
            DataTable FillLocation = new SDTemplateFileds().FillLocation(Convert.ToInt64(Request.QueryString["NamelyId"].ToString()));

            ddlLocation.DataSource = FillLocation;
            ddlLocation.DataTextField = "LocName";
            ddlLocation.DataValueField = "LocCode";
            ddlLocation.DataBind();
            ddlLocation.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----Select----", "0"));


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
    private void FillSummary()
    {
        try
        {
            //GetFileAttached();


            long CategoryFk;
            string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("select top 1 * from vSDTicket where TicketNumber=@TicketNumber and OrgId=@OrgId"))
                {
                    cmd.CommandType = CommandType.Text;
                    //cmd.Parameters.AddWithValue("@TicketNumber", ddlOpenticket.SelectedValue);
                    cmd.Parameters.AddWithValue("@TicketNumber", Request.QueryString["TicketId"].ToString());
                    cmd.Parameters.AddWithValue("@OrgId", Request.QueryString["NamelyId"].ToString());
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
                                DeskName = dt.Rows[0]["ServiceDesk"].ToString();
                                hdfldDesk.Value = dt.Rows[0]["ServiceDesk"].ToString();
                                if (ddlRequestType.SelectedValue == "Incident")
                                {
                                    pnlIncident.Visible = true;

                                }
                                if (ddlRequestType.SelectedValue == "Incident")
                                {
                                    rfvddlAssigne.Enabled = true;
                                    rfvddlAssigne.Visible = true;
                                }
                                else
                                {
                                    rfvddlAssigne.Enabled = false;
                                    rfvddlAssigne.Visible = false;
                                }
                                txtSummary.Text = dt.Rows[0]["Summary"].ToString();
                                txtLoginName.Text = dt.Rows[0]["SubmitterID"].ToString();
                                hdnAssetSerialNo.Value = Convert.ToString(dt.Rows[0]["AssetSerialNo"]);
                                hdnPeripheralSno.Value = Convert.ToString(dt.Rows[0]["PeriPheralSerialNo"]);
      
                                txtSubmitterName.Text = dt.Rows[0]["SubmitterName"].ToString();
                                txtSubmitterEmail.Text = dt.Rows[0]["SubmitterEmail"].ToString();
                                txtPhoneNumber.Text = dt.Rows[0]["SubmitterPhone"].ToString();
                                FillCategory1();
                                
                                if (ddlSeverity.Items.FindByValue(dt.Rows[0]["sdSeverityFK"].ToString()) != null)
                                {
                                    ddlSeverity.SelectedValue = dt.Rows[0]["sdSeverityFK"].ToString();
                                }
                                if (ddlPriority.Items.FindByValue(dt.Rows[0]["sdPriorityFK"].ToString()) != null)
                                {
                                    ddlPriority.SelectedValue = dt.Rows[0]["sdPriorityFK"].ToString();
                                }
                                if (ddlAssigne.Items.FindByValue(dt.Rows[0]["assigneeParticipantFK"].ToString()) != null)
                                {
                                    ddlAssigne.SelectedValue = dt.Rows[0]["assigneeParticipantFK"].ToString();
                                }
                                if (ddlStage.Items.FindByValue(dt.Rows[0]["sdStageFK"].ToString()) != null)
                                {
                                    ddlStage.SelectedValue = dt.Rows[0]["sdStageFK"].ToString();
                                }
                                FillStatus();
                                if (ddlStatus.Items.FindByValue(dt.Rows[0]["sdStatusFK"].ToString()) != null)
                                {
                                    ddlStatus.SelectedValue = dt.Rows[0]["sdStatusFK"].ToString();
                                }
                                if (ddlLocation.Items.FindByValue(dt.Rows[0]["location"].ToString()) != null)
                                {
                                    ddlLocation.SelectedValue = dt.Rows[0]["location"].ToString();
                                }
                                if (ddlDepartment.Items.FindByValue(dt.Rows[0]["Department"].ToString()) != null)
                                {
                                    ddlDepartment.SelectedValue = dt.Rows[0]["Department"].ToString();
                                }
                                CategoryFk = Convert.ToInt64(dt.Rows[0]["sdCategoryFK"].ToString());
                                DataTable Category1 = new SDTemplateFileds().GetTicketCategory(CategoryFk, 1);
                                DataTable Category2 = new SDTemplateFileds().GetTicketCategory(CategoryFk, 2);
                                DataTable Category3 = new SDTemplateFileds().GetTicketCategory(CategoryFk, 3);
                                DataTable Category4 = new SDTemplateFileds().GetTicketCategory(CategoryFk, 4);
                                DataTable Category5 = new SDTemplateFileds().GetTicketCategory(CategoryFk, 5);
                                if (Category1.Rows.Count > 0)
                                {
                                    string s;
                                    s = Category1.Rows[0]["ref"].ToString();
                                    if (ddlCategory1.Items.FindByValue(s) != null)
                                    {
                                        ddlCategory1.Items.FindByValue(s).Selected = true;
                                        hdnCategoryID.Value = ddlCategory1.SelectedValue;
                                    }

                                }
                                else
                                {
                                    ddlCategory2.ClearSelection();
                                    ddlCategory2.Enabled = false;
                                    divCategory2.Attributes.Add("style", "display: none;");
                                    ddlCategory3.ClearSelection();
                                    ddlCategory4.ClearSelection();
                                    ddlCategory5.ClearSelection();
                                    divCategory3.Attributes.Add("style", "display: none;");
                                    divCategory4.Attributes.Add("style", "display: none;");
                                    divCategory5.Attributes.Add("style", "display: none;");
                                }
                                if (Category2.Rows.Count > 0)
                                {
                                    FillCategory2();
                                    string s2;
                                    s2 = Category2.Rows[0]["ref"].ToString();
                                    if (ddlCategory2.Items.FindByValue(s2) != null)
                                    {
                                        ddlCategory2.Items.FindByValue(s2).Selected = true;
                                        hdnCategoryID.Value = ddlCategory2.SelectedValue;
                                    }

                                    divCategory2.Attributes.Add("style", "display: block;");
                                    divCategory3.Attributes.Add("style", "display: none;");
                                    divCategory4.Attributes.Add("style", "display: none;");
                                    divCategory5.Attributes.Add("style", "display: none;");
                                }
                                else
                                {
                                    ddlCategory2.ClearSelection();
                                    ddlCategory3.ClearSelection();
                                    ddlCategory4.ClearSelection();
                                    ddlCategory5.ClearSelection();
                                    divCategory2.Attributes.Add("style", "display: none;");
                                    divCategory3.Attributes.Add("style", "display: none;");
                                    divCategory4.Attributes.Add("style", "display: none;");
                                    divCategory5.Attributes.Add("style", "display: none;");
                                }

                                if (Category3.Rows.Count > 0)
                                {
                                    FillCategory3();
                                    string s3;
                                    s3 = Category3.Rows[0]["ref"].ToString();

                                    if (ddlCategory3.Items.FindByValue(s3) != null)
                                    {
                                        ddlCategory3.Items.FindByValue(s3).Selected = true;
                                        hdnCategoryID.Value = ddlCategory3.SelectedValue;
                                    }
                                    divCategory2.Attributes.Add("style", "display: block;");
                                    divCategory3.Attributes.Add("style", "display: block;");
                                    divCategory4.Attributes.Add("style", "display: none;");
                                    divCategory5.Attributes.Add("style", "display: none;");
                                }
                                else
                                {
                                    ddlCategory3.ClearSelection();
                                    ddlCategory5.ClearSelection();
                                    ddlCategory4.ClearSelection();
                                    divCategory3.Attributes.Add("style", "display: none;");
                                    divCategory4.Attributes.Add("style", "display: none;");
                                    divCategory5.Attributes.Add("style", "display: none;");
                                }
                                if (Category4.Rows.Count > 0)
                                {
                                    FillCategory4();
                                    string s4;
                                    s4 = Category4.Rows[0]["ref"].ToString();

                                    if (ddlCategory4.Items.FindByValue(s4) != null)
                                    {
                                        ddlCategory4.Items.FindByValue(s4).Selected = true;
                                        hdnCategoryID.Value = ddlCategory4.SelectedValue;
                                    }
                                    divCategory2.Attributes.Add("style", "display: block;");
                                    divCategory3.Attributes.Add("style", "display: block;");
                                    divCategory4.Attributes.Add("style", "display: block;");
                                    divCategory5.Attributes.Add("style", "display: none;");
                                }
                                else
                                {
                                    ddlCategory5.ClearSelection();
                                    divCategory5.Attributes.Add("style", "display: none;");
                                }

                                if (Category5.Rows.Count > 0)
                                {
                                    FillCategory5();
                                    string s5;
                                    s5 = Category5.Rows[0]["ref"].ToString();

                                    if (ddlCategory5.Items.FindByValue(s5) != null)
                                    {
                                        ddlCategory5.Items.FindByValue(s5).Selected = true;
                                        hdnCategoryID.Value = ddlCategory5.SelectedValue;
                                    }
                                    divCategory2.Attributes.Add("style", "display: block;");
                                    divCategory3.Attributes.Add("style", "display: block;");
                                    divCategory4.Attributes.Add("style", "display: block;");
                                    divCategory5.Attributes.Add("style", "display: block;");
                                }


                                txtDescription.Value = System.Web.HttpUtility.HtmlDecode(dt.Rows[0]["Description"].ToString()).ToString();
                                if (DeskName == "Change Request")
                                {
                                    FillImpactDetails();
                                    FillRollOutDetails();
                                    FilltaskDetails();
                                }
                                if (dt.Rows[0]["status"].ToString().ToLower().Contains("closed") || dt.Rows[0]["status"].ToString().ToLower().Contains("resolved"))
                                {
                                    pnlTicket.Enabled = false;
                                    if (ddlResoultion.Items.FindByValue(dt.Rows[0]["sdSolutionTypeFK"].ToString()) != null)
                                    {
                                        ddlResoultion.SelectedValue = dt.Rows[0]["sdSolutionTypeFK"].ToString();
                                    }
                                    txtResolution.Value = System.Web.HttpUtility.HtmlDecode(dt.Rows[0]["solutionNote"].ToString());
                                }
                                else
                                {
                                    pnlTicket.Enabled = true;
                                }
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
                inEr.InsertErrorLogsF(Session["UserName"].ToString()
        , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
            //			
        }

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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

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

    private void FillServDesk()
    {

        try
        {

            DataTable RequestType = new SDTemplateFileds().FillRequestType(Convert.ToInt64(Session["SD_OrgID"].ToString()));

            ddlRequestType.DataSource = RequestType;
            ddlRequestType.DataTextField = "ReqTypeRef";
            ddlRequestType.DataValueField = "ReqTypeRef";
            ddlRequestType.DataBind();
            ddlRequestType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----Select----", "0"));


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
    private void FillCategory1()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"SELECT CategoryCodeRef,
           Categoryref FROM [dbo].fnGetCategoryFullPathForDesk('" + ddlRequestType.SelectedValue + "', '" + Request.QueryString["NamelyId"] + "',1) where Level=1 order by Categoryref asc", con))
                {
                    cmd.CommandType = CommandType.Text;
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {

                        // cmd.Parameters.AddWithValue("@Option", "ProcessDetails");
                        adp.SelectCommand.CommandTimeout = 180;
                        using (DataTable dt = new DataTable())
                        {
                            adp.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                ddlCategory1.DataSource = dt;
                                ddlCategory1.DataTextField = "CategoryCodeRef";
                                ddlCategory1.DataValueField = "Categoryref";
                                ddlCategory1.DataBind();
                                ddlCategory1.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----Select----", "0"));
                            }
                            else
                            {
                                divCategory2.Attributes.Add("style", "display: none;");
                                ddlCategory3.ClearSelection();
                                ddlCategory4.ClearSelection();
                                ddlCategory5.ClearSelection();
                                divCategory3.Attributes.Add("style", "display: none;");
                                divCategory4.Attributes.Add("style", "display: none;");
                                divCategory5.Attributes.Add("style", "display: none;");
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
    private DataTable FillCategoryLevel(int categoryLevel)
    {
        try
        {
            //	hdnVarCategoryIII.Value = hdnVarCategoryI.Value;
            DataTable dtFillCategory = new DataTable();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"SELECT * FROM [dbo].[fn_GetCategoryChildrenByRef]('" + ddlCategory1.SelectedValue + "',1,'" + Request.QueryString["NamelyId"] + "') where level='" + categoryLevel + "'  order by Categoryref asc", con))
                {
                    cmd.CommandType = CommandType.Text;
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {

                        // cmd.Parameters.AddWithValue("@Option", "ProcessDetails");
                        adp.SelectCommand.CommandTimeout = 180;
                        adp.Fill(dtFillCategory);
                        //using (DataTable dt = new DataTable())
                        //{


                        //}
                    }
                }
            }
            return dtFillCategory;

        }
        catch (ThreadAbortException e2)
        {
            Console.WriteLine("Exception message: {0}", e2.Message);
            Thread.ResetAbort();
            return null;
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

            return null;
        }
    }
    protected void ddlCategory1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillCategory2();
            ShowCustomFields(ddlRequestType.SelectedItem.Text, Request.QueryString["NamelyId"].ToString());
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
    protected void FillCategory2()
    {
        try
        {
            hdnCategoryID.Value = ddlCategory1.SelectedValue.ToString();
            DataTable FillCategoryLevel2 = new DataTable();
            FillCategoryLevel2 = FillCategoryLevel(2);
            if (FillCategoryLevel2.Rows.Count > 0)
            {
                ddlCategory2.DataSource = FillCategoryLevel2;
                ddlCategory2.DataTextField = "CategoryCodeRef";
                ddlCategory2.DataValueField = "Categoryref";
                ddlCategory2.DataBind();
                ddlCategory2.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----Select----", "0"));

                divCategory2.Attributes.Add("style", "display: block;");
                divCategory3.Attributes.Add("style", "display: none;");
                divCategory4.Attributes.Add("style", "display: none;");
                divCategory5.Attributes.Add("style", "display: none;");



            }


            else
            {
                ddlCategory2.ClearSelection();
                ddlCategory2.Enabled = false;
                FillCategoryLevel2 = null;
                divCategory2.Attributes.Add("style", "display: none;");
                ddlCategory3.ClearSelection();
                ddlCategory4.ClearSelection();
                ddlCategory5.ClearSelection();
                divCategory3.Attributes.Add("style", "display: none;");
                divCategory4.Attributes.Add("style", "display: none;");
                divCategory5.Attributes.Add("style", "display: none;");
                //ddlCatII.DataSource = null;
                //  ddlCatII.DataBind();
                // ddlCatII.Items.Insert(0, new ListItem("----------Select Category Level 2----------", "0"));
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
    protected void FillCategory3()
    {
        try
        {
            hdnCategoryID.Value = ddlCategory2.SelectedValue.ToString();
            DataTable FillCategoryLevel3 = FillCategoryLevel(3);
            if (FillCategoryLevel3.Rows.Count > 0)
            {
                ddlCategory3.DataSource = FillCategoryLevel3;
                ddlCategory3.DataTextField = "CategoryCodeRef";
                ddlCategory3.DataValueField = "Categoryref";
                ddlCategory3.DataBind();
                ddlCategory3.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----Select----", "0"));

                divCategory3.Attributes.Add("style", "display: block;");
                divCategory4.Attributes.Add("style", "display: none;");
                divCategory5.Attributes.Add("style", "display: none;");


            }
            else
            {
                ddlCategory3.ClearSelection();
                //ddlCategory3.Enabled = false;
                ddlCategory3.DataSource = null;
                divCategory2.Attributes.Add("style", "display: none;");

                divCategory3.Attributes.Add("style", "display: none;");
                divCategory4.Attributes.Add("style", "display: none;");
                divCategory5.Attributes.Add("style", "display: none;");
                ddlCategory3.DataBind();
                ddlCategory4.ClearSelection();
                ddlCategory4.Enabled = false;
                ddlCategory5.ClearSelection();
                ddlCategory5.Enabled = false;
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
    protected void FillCategory4()
    {
        try
        {
            hdnCategoryID.Value = ddlCategory3.SelectedValue.ToString();
            DataTable FillCategoryLevel4 = FillCategoryLevel(4);
            if (FillCategoryLevel4.Rows.Count > 0)
            {
                ddlCategory4.DataSource = FillCategoryLevel4;
                ddlCategory4.DataTextField = "CategoryCodeRef";
                ddlCategory4.DataValueField = "Categoryref";
                ddlCategory4.DataBind();
                ddlCategory4.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----Select----", "0"));


                divCategory2.Attributes.Add("style", "display: block;");
                divCategory3.Attributes.Add("style", "display: block;");
                divCategory4.Attributes.Add("style", "display: block;");
                divCategory5.Attributes.Add("style", "display: none;");



            }
            else
            {
                ddlCategory4.DataSource = null;
                ddlCategory4.DataBind();

                ddlCategory4.ClearSelection();
                ddlCategory4.Enabled = false;

                ddlCategory5.ClearSelection();
                ddlCategory5.Enabled = false;
                divCategory2.Attributes.Add("style", "display: none;");

                divCategory3.Attributes.Add("style", "display: none;");
                divCategory4.Attributes.Add("style", "display: none;");
                divCategory5.Attributes.Add("style", "display: none;");
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
    protected void FillCategory5()
    {
        try
        {
            hdnCategoryID.Value = ddlCategory4.SelectedValue.ToString();
            DataTable FillCategoryLevel4 = FillCategoryLevel(5);
            if (FillCategoryLevel4.Rows.Count > 0)
            {
                ddlCategory5.DataSource = FillCategoryLevel4;
                ddlCategory5.DataTextField = "CategoryCodeRef";
                ddlCategory5.DataValueField = "Categoryref";
                ddlCategory5.DataBind();
                ddlCategory5.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----Select----", "0"));

                divCategory2.Attributes.Add("style", "display: block;");
                divCategory3.Attributes.Add("style", "display: block;");
                divCategory4.Attributes.Add("style", "display: block;");
                divCategory5.Attributes.Add("style", "display: block;");



            }
            else
            {
                ddlCategory5.DataSource = null;
                ddlCategory5.DataBind();

                ddlCategory5.ClearSelection();
                ddlCategory5.Enabled = false;
                ddlCategory6.ClearSelection();
                ddlCategory6.Enabled = false;

                divCategory2.Attributes.Add("style", "display: none;");

                divCategory3.Attributes.Add("style", "display: none;");
                divCategory4.Attributes.Add("style", "display: none;");
                divCategory5.Attributes.Add("style", "display: none;");
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
    protected void ddlCategory2_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCategory3();
        ShowCustomFields(ddlRequestType.SelectedItem.Text, Request.QueryString["NamelyId"].ToString());
    }
    protected void ddlCategory3_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCategory4();
        ShowCustomFields(ddlRequestType.SelectedItem.Text, Request.QueryString["NamelyId"].ToString());
    }
    protected void ddlCategory4_SelectedIndexChanged(object sender, EventArgs e)
    {

        FillCategory5();
        ShowCustomFields(ddlRequestType.SelectedItem.Text, Request.QueryString["NamelyId"].ToString());
    }
    protected void ddlCategory5_SelectedIndexChanged(object sender, EventArgs e)
    {
        hdnCategoryID.Value = ddlCategory5.SelectedValue.ToString();
        ShowCustomFields(ddlRequestType.SelectedItem.Text, Request.QueryString["NamelyId"].ToString());
        DataTable FillCategoryLevel4 = FillCategoryLevel(6);
        if (FillCategoryLevel4.Rows.Count > 0)
        {
            ddlCategory6.DataSource = FillCategoryLevel4;
            ddlCategory6.DataTextField = "CategoryCodeRef";
            ddlCategory6.DataValueField = "Categoryref";
            ddlCategory6.DataBind();
            ddlCategory6.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----Select----", "0"));
        }
        else
        {
            ddlCategory6.DataSource = null;
            ddlCategory6.DataBind();

            ddlCategory6.ClearSelection();
            ddlCategory6.Enabled = false;

        }
    }
    protected void ddlRequestType_SelectedIndexChanged(object sender, EventArgs e)
    {


        Session["Desk"] = ddlRequestType.SelectedValue.ToString();

        if (ddlRequestType.SelectedItem.ToString().ToLower().Contains("cloud"))
        {

            pnlChange.Visible = false;
            //   showChangeControl.Visible = false;
        }
        else if (ddlRequestType.SelectedItem.ToString().ToLower().Contains("service"))
        {
            pnlIncident.Visible = true;
            pnlSRFields.Visible = true;
            pnlChange.Visible = false;
        }
        else if (ddlRequestType.SelectedItem.ToString().ToLower().Contains("change"))
        {
            pnlIncident.Visible = true;
            //    showChangeControl.Visible = true; pnlChange.Visible = true;

            AddDefaultFirstRecordForImpact();
            AddDefaultFirstRecordForRollOut();
            AddDefaultFirstRecordForTask(); //pnlAddImpact.Visible = false; //pnlAddRollout.Visible = false; 
            pnlDownTime.Visible = false; //pnlImpactGrid.Visible = false; //pnlRollOutGrid.Visible = false; 
            pnlShowRollOutDetails.Visible = false;
            pnlShowImpactDetails.Visible = false;
            pnlTaksAssociation.Visible = false;
           
            pnlSRFields.Visible = false;
            pnlChange.Visible = true;
            FillChangeType();
            FillReasonForChange();
            FillAssignee();

        }

        else
        {
            //  showChangeControl.Visible = false;
            pnlIncident.Visible = true;

            pnlChange.Visible = false;
            // FillLocations(); if (ddlSeverity.Items.FindByValue("613854197") != null) { ddlSeverity.SelectedValue = "613854197"; } 
        }
        //FillStage();
        FillSeverity();
        FillPriority();
        //FillStatus();
  
        FillCategory1();
        FillStatus();
        ddlCategory2.ClearSelection();
        ddlCategory3.ClearSelection();
        ddlCategory4.ClearSelection();
        ShowCustomFields(ddlRequestType.SelectedItem.Text, Request.QueryString["NamelyId"].ToString());
        //FillLocations();	

    }
    private void FillStage()
    {

        try
        {

            DataTable SD_Status = new SDTemplateFileds().FillStage(Session["SDRef"].ToString(), Request.QueryString["NamelyId"].ToString());

            ddlStage.DataSource = SD_Status;
            ddlStage.DataTextField = "StageCodeRef";
            ddlStage.DataValueField = "id";
            ddlStage.DataBind();
            ddlStage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----Select----", "0"));


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
    protected void ShowCustomFields(string RequestType, string OrgId)
    {
        try
        {
            DataTable SD_SDCustomFields = new FillSDFields().FillSDODDNumberCustomFieldsForTech(RequestType, Session["SDRole"].ToString(), OrgId);
            DataTable SD_SDDDLCustomFields = new FillSDFields().FillSDODDNumberDropDownCustomFieldsForTech(RequestType, Session["SDRole"].ToString(), OrgId);

            if (SD_SDCustomFields.Rows.Count > 0)
            {
                oddNumberCstmFlds = SD_SDCustomFields;
                rptOddControl.DataSource = SD_SDCustomFields;
                rptOddControl.DataBind();
            }


            if (SD_SDDDLCustomFields.Rows.Count > 0)
            {
                oddNumberDdlCstmFlds = SD_SDDDLCustomFields;
                rptddlOddControl.DataSource = SD_SDDDLCustomFields;
                rptddlOddControl.DataBind();
            }
            DataTable SD_SDEvenCustomFields = new FillSDFields().FillSDEvenNumberCustomFieldsForTech(RequestType, Session["SDRole"].ToString(), OrgId);

            EvenNumberCstmFlds = SD_SDEvenCustomFields;
            rptEvenControl.DataSource = SD_SDEvenCustomFields;
            rptEvenControl.DataBind();

            DataTable SD_SDddlEvenCustomFields = new FillSDFields().FillSDEvenNumberDropDownCustomFieldsForTech(RequestType, Session["SDRole"].ToString(), OrgId);
            EvenNumberDdlCstmFlds = SD_SDddlEvenCustomFields;
            rptddlEvenControl.DataSource = SD_SDddlEvenCustomFields;
            rptddlEvenControl.DataBind();
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

    protected void rptEvenControl_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        try
        {

            //	if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            //	{
            if (EvenNumberCstmFlds != null)
            {
                //foreach (RepeaterItem item in rptOddControl.Items)
                //{
                DataRowView drv = e.Item.DataItem as DataRowView;
                string ID = drv["FieldMode"].ToString();
                TextBox txtodd = e.Item.FindControl("txteven") as TextBox;
                Label lbl = e.Item.FindControl("lbleven") as Label;
                //	txtodd.TextMode = TextBoxMode.ID;
                switch (ID.ToLower())
                {

                    case "datetime":
                        txtodd.EnableViewState = false;
                        txtodd.CssClass = "form-control form-control-sm";
                        //txtodd.Attributes.Add("type", "DateTime");
                        txtodd.Attributes.Add("type", "datetime-local");
                        break;
                    case "singleline":
                        txtodd.EnableViewState = false;
                        txtodd.CssClass = "form-control form-control-sm";
                        txtodd.TextMode = TextBoxMode.SingleLine;

                        break;
                }

                DataTable dt = new SDTemplateFileds().FillCustomFieldValueDropdown(lbl.Text, Request.QueryString["TicketId"].ToString(), Request.QueryString["NamelyId"].ToString());
                if (dt.Rows.Count > 0 && dt != null)
                    txtodd.Text = dt.Rows[0]["fieldvalue"].ToString();
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
    DataTable oddNumberCstmFlds;
    DataTable EvenNumberCstmFlds;
    DataTable oddNumberDdlCstmFlds;
    DataTable EvenNumberDdlCstmFlds;
    protected void rptOddControl_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {

        try
        {
            //	if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            //	{
            if (oddNumberCstmFlds != null)
            {
                //foreach (RepeaterItem item in rptOddControl.Items)
                //{
                DataRowView drv = e.Item.DataItem as DataRowView;
                string ID = drv["FieldMode"].ToString();
                TextBox txtodd = e.Item.FindControl("txt") as TextBox;
                Label lbl = e.Item.FindControl("lbl") as Label;
                //	txtodd.TextMode = TextBoxMode.ID;
                switch (ID.ToLower())
                {

                    case "datetime":
                        txtodd.EnableViewState = false;
                        txtodd.CssClass = "form-control form-control-sm";
                        //txtodd.Attributes.Add("type", "DateTime");
                        txtodd.Attributes.Add("type", "datetime-local");
                        break;
                    case "singleline":
                        txtodd.EnableViewState = false;
                        txtodd.CssClass = "form-control form-control-sm";
                        txtodd.TextMode = TextBoxMode.SingleLine;
                        break;

                }
                DataTable dt = new SDTemplateFileds().FillCustomFieldValueDropdown(lbl.Text, Request.QueryString["TicketId"].ToString(), Request.QueryString["NamelyId"].ToString());
                if (dt.Rows.Count > 0)
                    txtodd.Text = dt.Rows[0]["fieldvalue"].ToString();

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

    protected void rptddlOddControl_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        try
        {
            if (oddNumberDdlCstmFlds != null)
            {
                //foreach (RepeaterItem item in rptOddControl.Items)
                //{
                DataRowView drv = e.Item.DataItem as DataRowView;

                Label lbl = e.Item.FindControl("lblOddlist") as Label;
                DropDownList selectList = e.Item.FindControl("ddlOdd") as DropDownList;
                selectList.ClearSelection();
                if (selectList != null)
                {
                    DataTable dt = new SDTemplateFileds().FillCustomFieldDropdown(lbl.Text);
                    selectList.DataSource = dt; //your datasource
                    selectList.DataTextField = lbl.Text;
                    selectList.DataValueField = lbl.Text;
                    selectList.DataBind();
                    selectList.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----Select----", "0"));

                    //selectList.DataTextField = "SomeColumn";
                    //selectList.DataValueField = "SomeID";
                }
                DataTable dt1 = new SDTemplateFileds().FillCustomFieldValueDropdown(lbl.Text, Request.QueryString["TicketId"].ToString(), Request.QueryString["NamelyId"].ToString());
                if (dt1.Rows.Count > 0)
                {



                    if (selectList.Items.FindByValue(dt1.Rows[0]["fieldvalue"].ToString()) != null)
                    {
                        selectList.Items.FindByValue(dt1.Rows[0]["fieldvalue"].ToString()).Selected = true;
                        //	Response.Redirect(Request.Url.AbsoluteUri);
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

    protected void rptddlEvenControl_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        try
        {
            //foreach (RepeaterItem item in rptddlEvenControl.Items)
            //{
            if (EvenNumberDdlCstmFlds != null)
            {

                DataRowView drv = e.Item.DataItem as DataRowView;

                Label lbleven = e.Item.FindControl("lblEvenlist") as Label;
                DropDownList selectListEven = e.Item.FindControl("ddlEven") as DropDownList;
                selectListEven.ClearSelection();
                if (selectListEven != null)
                {
                    DataTable dt = new SDTemplateFileds().FillCustomFieldDropdown(lbleven.Text);
                    selectListEven.DataSource = dt; //your datasource
                    selectListEven.DataTextField = lbleven.Text;
                    selectListEven.DataValueField = lbleven.Text;
                    selectListEven.DataBind();
                    selectListEven.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----Select----", "0"));
                }
                DataTable dtEven = new SDTemplateFileds().FillCustomFieldValueDropdown(lbleven.Text, Request.QueryString["TicketId"].ToString(), Request.QueryString["NamelyId"].ToString());
                if (dtEven.Rows.Count > 0)
                {



                    if (selectListEven.Items.FindByValue(dtEven.Rows[0]["fieldvalue"].ToString()) != null)
                    {
                        selectListEven.Items.FindByValue(dtEven.Rows[0]["fieldvalue"].ToString()).Selected = true;

                    }

                }
            }
            //}
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
    private void FillSeverity()
    {
        try
        {

            DataTable SD_Severity = new SDTemplateFileds().FillSeverity(ddlRequestType.SelectedValue, Request.QueryString["NamelyId"].ToString()); ;

            ddlSeverity.DataSource = SD_Severity;
            ddlSeverity.DataTextField = "Serveritycoderef";
            ddlSeverity.DataValueField = "id";
            ddlSeverity.DataBind();
            ddlSeverity.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----Select----", "0"));


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
    private void FillPriority()
    {

        try
        {

            DataTable SD_Priority = new SDTemplateFileds().FillPriority(ddlRequestType.SelectedValue, Request.QueryString["NamelyId"].ToString()); ;

            ddlPriority.DataSource = SD_Priority;
            ddlPriority.DataTextField = "PriorityCodeRef";
            ddlPriority.DataValueField = "id";
            ddlPriority.DataBind();
            ddlPriority.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----Select----", "0"));


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
    private void FillStatus()
    {

        try
        {

            DataTable SD_Priority = new SDTemplateFileds().FillStatusDy(ddlRequestType.SelectedValue, Request.QueryString["NamelyId"].ToString()); ;

            ddlStatus.DataSource = SD_Priority;
            ddlStatus.DataTextField = "StatusCodeRef";
            ddlStatus.DataValueField = "id";
            ddlStatus.DataBind();
            ddlStatus.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----Select----", "0"));


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

    private void FillAssignee()
    {
        try
        {
            DataTable FillAssigne = new SDTemplateFileds().FillAssigne(Convert.ToInt64(Request.QueryString["NamelyId"].ToString()));

            ddlAssigne.DataSource = FillAssigne;
            ddlAssigne.DataTextField = "TechLoginName";
            ddlAssigne.DataValueField = "TechID";
            ddlAssigne.DataBind();
            ddlAssigne.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----Select----", "0"));


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
    private void FillAssigneeForChange()
    {
        try
        {
            DataTable FillAssigne = new SDTemplateFileds().FillAssigne(Convert.ToInt64(Request.QueryString["NamelyId"].ToString()));

            lstTechAssoc.DataSource = FillAssigne;
            lstTechAssoc.DataTextField = "TechLoginName";
            lstTechAssoc.DataValueField = "TechID";
            lstTechAssoc.DataBind();
            lstTechAssoc.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----Select----", "0"));

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

    Random r = new Random();
    private void FillResolutionDetails()
    {

        try
        {

            DataTable SD_Resolution = new FillSDFields().FillResolutionCustomer(Request.QueryString["NamelyId"].ToString()); ;

            if (SD_Resolution.Rows.Count > 0)
            {
                //  this.lb.Text = dataTable.Rows.Count.ToString();
                this.ddlResoultion.DataSource = (object)SD_Resolution;

                ddlResoultion.DataTextField = "ResolutionCodeRef";
                ddlResoultion.DataValueField = "id";
                ddlResoultion.DataBind();
                ddlResoultion.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----Select----", "0"));
            }
            else
            {
                this.ddlResoultion.DataSource = (object)null;
                this.ddlResoultion.DataBind();
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
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.AbsoluteUri);
    }
    protected void UpdateCustFields()
    {
        try
        {
            DataTable UpdateWIP = new SDCustomFields().CheckForPrevStatus(Request.QueryString["TicketId"].ToString(), Request.QueryString["NamelyId"].ToString());
            if (Request.QueryString["Desk"].ToString().ToLower().Contains("service") || Request.QueryString["Desk"].ToString().ToLower().Contains("service"))
            {
                foreach (RepeaterItem item in rptOddControl.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        TextBox C = (TextBox)item.FindControl("txt");
                        Label lbl = (Label)item.FindControl("lbl");
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("SD_spAddSDCustomTicketField", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@TicketNo", Request.QueryString["TicketId"].ToString());
                                cmd.Parameters.AddWithValue("@OrgId", Request.QueryString["NamelyId"].ToString());
                                cmd.Parameters.AddWithValue("@FieldName", lbl.Text);
                                cmd.Parameters.AddWithValue("@FieldValue", C.Text);
                                cmd.Parameters.AddWithValue("@Option", "UpdateCustomField");
                                con.Open();
                                int res = cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
                foreach (RepeaterItem item in rptEvenControl.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        TextBox C = (TextBox)item.FindControl("txteven");
                        Label lbl = (Label)item.FindControl("lbleven");
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("SD_spAddSDCustomTicketField", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@TicketNo", Request.QueryString["TicketId"].ToString());
                                cmd.Parameters.AddWithValue("@OrgId", Request.QueryString["NamelyId"].ToString());
                                cmd.Parameters.AddWithValue("@FieldName", lbl.Text);
                                cmd.Parameters.AddWithValue("@FieldValue", C.Text);
                                cmd.Parameters.AddWithValue("@Option", "UpdateCustomField");
                                con.Open();
                                int res = cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
                foreach (RepeaterItem item in rptddlOddControl.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        DropDownList C = (DropDownList)item.FindControl("ddlOdd");
                        Label lbl = (Label)item.FindControl("lblOddlist");
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                        {

                            using (SqlCommand cmd = new SqlCommand("SD_spAddSDCustomTicketField", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.AddWithValue("@TicketNo", Request.QueryString["TicketId"].ToString());
                                cmd.Parameters.AddWithValue("@OrgId", Request.QueryString["NamelyId"].ToString());
                                cmd.Parameters.AddWithValue("@FieldName", lbl.Text);
                                cmd.Parameters.AddWithValue("@FieldValue", C.SelectedValue);
                                cmd.Parameters.AddWithValue("@Option", "UpdateCustomField");
                                con.Open();
                                int res = cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
                foreach (RepeaterItem item in rptddlEvenControl.Items)
                {
                    if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                    {
                        DropDownList C = (DropDownList)item.FindControl("ddlEven");
                        Label lbl = (Label)item.FindControl("lblEvenlist");
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("SD_spAddSDCustomTicketField", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@TicketNo", Request.QueryString["TicketId"].ToString());
                                cmd.Parameters.AddWithValue("@OrgId", Request.QueryString["NamelyId"].ToString());
                                cmd.Parameters.AddWithValue("@FieldName", lbl.Text);
                                cmd.Parameters.AddWithValue("@FieldValue", C.SelectedValue);
                                cmd.Parameters.AddWithValue("@Option", "UpdateCustomField");
                                con.Open();
                                int res = cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }

            if (UpdateWIP.Rows.Count > 0)
            {

                foreach (DataRow row in UpdateWIP.Rows)
                {
                    string prevstatus = row["StatusCodeRef"].ToString();
                    //to get open End or WIP Start

                    foreach (RepeaterItem item in rptOddControl.Items)
                    {
                        if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                        {
                            Label lbl = (Label)item.FindControl("lbl");
                            if (prevstatus.ToLower().Contains("open") && ddlStatus.SelectedItem.ToString().ToLower() != "open")
                            {
                                // also capture OPEN END 
                                if (lbl.Text.ToString().ToLower() == "openend")
                                {
                                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                                    {

                                        using (SqlCommand cmd = new SqlCommand("SD_spAddSDCustomTicketField", con))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;

                                            cmd.Parameters.AddWithValue("@TicketNo", Request.QueryString["TicketId"].ToString());
                                            cmd.Parameters.AddWithValue("@OrgId", Request.QueryString["NamelyId"].ToString());
                                            cmd.Parameters.AddWithValue("@FieldName", lbl.Text);
                                            cmd.Parameters.AddWithValue("@FieldValue", Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss"));
                                            cmd.Parameters.AddWithValue("@Option", "UpdateCustomField");
                                            con.Open();
                                            int res = cmd.ExecuteNonQuery();

                                        }
                                    }
                                }
                            }
                            if (prevstatus.ToLower().ToString() != "wip" && ddlStatus.SelectedItem.ToString().ToLower() == "wip")
                            {
                                if (lbl.Text.ToString().ToLower() == "wipstart")
                                {


                                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                                    {

                                        using (SqlCommand cmd = new SqlCommand("SD_spAddSDCustomTicketField", con))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;

                                            cmd.Parameters.AddWithValue("@TicketNo", Request.QueryString["TicketId"].ToString());
                                            cmd.Parameters.AddWithValue("@OrgId", Request.QueryString["NamelyId"].ToString());
                                            cmd.Parameters.AddWithValue("@FieldName", lbl.Text);
                                            cmd.Parameters.AddWithValue("@FieldValue", Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss"));



                                            cmd.Parameters.AddWithValue("@Option", "UpdateCustomField");
                                            con.Open();
                                            int res = cmd.ExecuteNonQuery();

                                        }
                                    }
                                }
                            }
                            if (prevstatus.ToLower().Contains("wip") && ddlStatus.SelectedItem.ToString().ToLower() != "wip")
                            {

                                if (lbl.Text.ToLower().Contains("wipend"))
                                {


                                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                                    {

                                        using (SqlCommand cmd = new SqlCommand("SD_spAddSDCustomTicketField", con))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;

                                            cmd.Parameters.AddWithValue("@TicketNo", Request.QueryString["TicketId"].ToString());
                                            cmd.Parameters.AddWithValue("@OrgId", Request.QueryString["NamelyId"].ToString());
                                            cmd.Parameters.AddWithValue("@FieldName", lbl.Text);
                                            cmd.Parameters.AddWithValue("@FieldValue", Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss"));




                                            cmd.Parameters.AddWithValue("@Option", "UpdateCustomField");
                                            con.Open();
                                            int res = cmd.ExecuteNonQuery();

                                        }
                                    }
                                }
                            }
                            if ((prevstatus.ToLower().ToString() != "hold") && ddlStatus.SelectedItem.ToString().ToLower() == "hold")
                            {


                                if (lbl.Text.ToLower().Contains("holdstart"))
                                {


                                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                                    {

                                        using (SqlCommand cmd = new SqlCommand("SD_spAddSDCustomTicketField", con))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;

                                            cmd.Parameters.AddWithValue("@TicketNo", Request.QueryString["TicketId"].ToString());
                                            cmd.Parameters.AddWithValue("@OrgId", Request.QueryString["NamelyId"].ToString());
                                            cmd.Parameters.AddWithValue("@FieldName", lbl.Text);
                                            cmd.Parameters.AddWithValue("@FieldValue", Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss"));
                                            cmd.Parameters.AddWithValue("@Option", "UpdateCustomField");
                                            con.Open();
                                            int res = cmd.ExecuteNonQuery();

                                        }
                                    }
                                }
                            }
                            if ((prevstatus.ToLower().ToString() == "hold") && ddlStatus.SelectedItem.ToString().ToLower() != "hold")
                            {

                                if (lbl.Text.ToLower().Contains("holdend"))
                                {


                                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                                    {

                                        using (SqlCommand cmd = new SqlCommand("SD_spAddSDCustomTicketField", con))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;

                                            cmd.Parameters.AddWithValue("@TicketNo", Request.QueryString["TicketId"].ToString());
                                            cmd.Parameters.AddWithValue("@OrgId", Request.QueryString["NamelyId"].ToString());
                                            cmd.Parameters.AddWithValue("@FieldName", lbl.Text);
                                            cmd.Parameters.AddWithValue("@FieldValue", Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss"));




                                            cmd.Parameters.AddWithValue("@Option", "UpdateCustomField");
                                            con.Open();
                                            int res = cmd.ExecuteNonQuery();

                                        }
                                    }
                                }
                            }
                        }

                    }
                    foreach (RepeaterItem item in rptEvenControl.Items)
                    {
                        if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                        {

                            Label lbleven = (Label)item.FindControl("lbleven");
                            if (prevstatus.ToLower().Contains("open") && ddlStatus.SelectedItem.ToString().ToLower() != "open")
                            {
                                if (lbleven.Text.ToString().ToLower() == "openend")
                                {
                                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                                    {

                                        using (SqlCommand cmd = new SqlCommand("SD_spAddSDCustomTicketField", con))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;

                                            cmd.Parameters.AddWithValue("@TicketNo", Request.QueryString["TicketId"].ToString());
                                            cmd.Parameters.AddWithValue("@OrgId", Request.QueryString["NamelyId"].ToString());
                                            cmd.Parameters.AddWithValue("@FieldName", lbleven.Text);
                                            cmd.Parameters.AddWithValue("@FieldValue", Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss"));



                                            cmd.Parameters.AddWithValue("@Option", "UpdateCustomField");
                                            con.Open();
                                            int res = cmd.ExecuteNonQuery();

                                        }
                                    }
                                }

                            }
                            if (prevstatus.ToLower().ToString() != "wip" && ddlStatus.SelectedItem.ToString().ToLower() == "wip")
                            {
                                if (lbleven.Text.ToString().ToLower() == "wipstart")
                                {


                                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                                    {

                                        using (SqlCommand cmd = new SqlCommand("SD_spAddSDCustomTicketField", con))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;

                                            cmd.Parameters.AddWithValue("@TicketNo", Request.QueryString["TicketId"].ToString());
                                            cmd.Parameters.AddWithValue("@OrgId", Request.QueryString["NamelyId"].ToString());
                                            cmd.Parameters.AddWithValue("@FieldName", lbleven.Text);
                                            cmd.Parameters.AddWithValue("@FieldValue", Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss"));



                                            cmd.Parameters.AddWithValue("@Option", "UpdateCustomField");
                                            con.Open();
                                            int res = cmd.ExecuteNonQuery();

                                        }
                                    }
                                }
                            }
                            if (prevstatus.ToLower().Contains("wip") && ddlStatus.SelectedItem.ToString().ToLower() != "wip")
                            {
                                if (lbleven.Text.ToLower().Contains("wipend"))
                                {


                                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                                    {

                                        using (SqlCommand cmd = new SqlCommand("SD_spAddSDCustomTicketField", con))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;

                                            cmd.Parameters.AddWithValue("@TicketNo", Request.QueryString["TicketId"].ToString());
                                            cmd.Parameters.AddWithValue("@OrgId", Request.QueryString["NamelyId"].ToString());
                                            cmd.Parameters.AddWithValue("@FieldName", lbleven.Text);
                                            cmd.Parameters.AddWithValue("@FieldValue", Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss"));


                                            cmd.Parameters.AddWithValue("@Option", "UpdateCustomField");
                                            con.Open();
                                            int res = cmd.ExecuteNonQuery();

                                        }
                                    }
                                }
                            }
                            if ((prevstatus.ToLower().ToString() != "hold") && ddlStatus.SelectedItem.ToString().ToLower() == "hold")
                            {
                                if (lbleven.Text.ToLower().Contains("holdstart"))
                                {


                                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                                    {

                                        using (SqlCommand cmd = new SqlCommand("SD_spAddSDCustomTicketField", con))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;

                                            cmd.Parameters.AddWithValue("@TicketNo", Request.QueryString["TicketId"].ToString());
                                            cmd.Parameters.AddWithValue("@OrgId", Request.QueryString["NamelyId"].ToString());
                                            cmd.Parameters.AddWithValue("@FieldName", lbleven.Text);
                                            cmd.Parameters.AddWithValue("@FieldValue", Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss"));


                                            cmd.Parameters.AddWithValue("@Option", "UpdateCustomField");
                                            con.Open();
                                            int res = cmd.ExecuteNonQuery();

                                        }
                                    }
                                }
                            }
                            if ((prevstatus.ToLower().ToString() == "hold") && ddlStatus.SelectedItem.ToString().ToLower() != "hold")
                            {

                                if (lbleven.Text.ToLower().Contains("holdend"))
                                {


                                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                                    {

                                        using (SqlCommand cmd = new SqlCommand("SD_spAddSDCustomTicketField", con))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;

                                            cmd.Parameters.AddWithValue("@TicketNo", Request.QueryString["TicketId"].ToString());
                                            cmd.Parameters.AddWithValue("@OrgId", Request.QueryString["NamelyId"].ToString());
                                            cmd.Parameters.AddWithValue("@FieldName", lbleven.Text);
                                            cmd.Parameters.AddWithValue("@FieldValue", Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss"));
                                            cmd.Parameters.AddWithValue("@Option", "UpdateCustomField");
                                            con.Open();
                                            int res = cmd.ExecuteNonQuery();

                                        }
                                    }
                                }
                            }
                        }
                    }

                    // when first time ticket was in WIP and second time for update

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
    public static string GetConnectstring()
    {
        return ConfigurationManager.ConnectionStrings["con"].ConnectionString;
    }
    public static object GetScalarValue(string sql, SqlConnection cnn)
    {
        SqlCommand cmd = new SqlCommand(sql, cnn);
        cmd.CommandTimeout = 3600;
        object obj = cmd.ExecuteScalar();
        return obj;
    }
    public static object GetScalarValue(string sql)
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
    protected void UpdateTicketDetails()
    {
        try
        {
            AddTicketNotes();
            UpdateCustFields();
            GetFileAttached();

            if (ddlCategory6.SelectedIndex < 0)
            {

            }
            else
            {
                Session["CategoryID"] = ddlCategory6.SelectedValue.ToString();
            }
            string sql2 = "select top 1 OrgName from SD_OrgMaster where Org_ID='" + Session["SD_OrgID"].ToString() + "'";
            string OrgName = Convert.ToString(GetScalarValue(sql2));
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SD_spSDIncident", con))
                {
                    cmd.CommandTimeout = 3600;
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TicketNumber", Request.QueryString["TicketId"].ToString());
                    cmd.Parameters.AddWithValue("@sdPriorityFK", ddlPriority.SelectedValue);
                    cmd.Parameters.AddWithValue("@sdSeverityFK", ddlSeverity.SelectedValue);
            //        cmd.Parameters.AddWithValue("@sdStageFK", ddlStage.SelectedValue);
                    cmd.Parameters.AddWithValue("@sdStatusFK", ddlStatus.SelectedValue);
                    cmd.Parameters.AddWithValue("@sdCategoryRef", hdnCategoryID.Value);
                    cmd.Parameters.AddWithValue("@submitterEmailAddr", txtSubmitterEmail.Text);
                    cmd.Parameters.AddWithValue("@submitterPhone", txtPhoneNumber.Text);
                    cmd.Parameters.AddWithValue("@TicketDesc", System.Web.HttpUtility.HtmlEncode(txtDescription.Value));
                    cmd.Parameters.AddWithValue("@assigneeType", "");
                    //	cmd.Parameters.AddWithValue("@assigneePoolFK","");
                    cmd.Parameters.AddWithValue("@assigneeParticipantFK", ddlAssigne.SelectedValue);
                    if (ddlStatus.SelectedItem.Text == "Closed")
                    {
                        cmd.Parameters.AddWithValue("@closedDateTime", DateTime.Now);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@closedDateTime", null);
                    }
                    cmd.Parameters.AddWithValue("@organizationFK", Request.QueryString["NamelyId"].ToString());
                    cmd.Parameters.AddWithValue("@UserName", Session["UserName"].ToString());
                    cmd.Parameters.AddWithValue("@HODApproval", ddlHodApproval.SelectedValue);
                    cmd.Parameters.AddWithValue("@sdSolutionTypeFK", ddlResoultion.SelectedValue);
                    cmd.Parameters.AddWithValue("@solutionNote", System.Web.HttpUtility.HtmlEncode(txtResolution.Value));
                    cmd.Parameters.AddWithValue("@TickNotes", System.Web.HttpUtility.HtmlDecode(txtNotes.Value));
                    cmd.Parameters.AddWithValue("@categoryFullText", hdnCategoryID.Value.ToString().Replace("||", " - "));
                    cmd.Parameters.AddWithValue("@location", ddlLocation.SelectedValue);
                    cmd.Parameters.AddWithValue("@Department", ddlDepartment.SelectedValue);
                    cmd.Parameters.AddWithValue("@Option", "UpdateTicket");
                    cmd.Parameters.AddWithValue("@OrgName", OrgName);
                    cmd.Parameters.Add("@Error", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    string ErrorChk = cmd.Parameters["@Error"].Value.ToString();
                    if (res > 0 && string.IsNullOrEmpty(ErrorChk))
                    {
                        if (Session["UploadedFiles"] != null)
                        {
                            FileUpload(Request.QueryString["TicketId"].ToString());
                        }
                        if (Session["AssigneUpdate"].ToString() == "True")
                        {
                            ADDMailinDB(Request.QueryString["TicketID"].ToString());
                        }
                        
                        if (ViewState["btnType"] != null)
                        {
                            ViewState["btnType"] = null;
                            //                            string message = HttpUtility.JavaScriptStringEncode("Ticket Updated Successfully!");
                            //                            ScriptManager.RegisterStartupScript(this, GetType(), "showAlertAndRedirect", $@"
                            //    setTimeout(function() {{
                            //        success_noti('{message}'); 
                            //        setTimeout(function() {{
                            //            if (window.opener) {{
                            //                window.opener.location.href = 'frmAllTickets.aspx';
                            //            }}
                            //            window.close();
                            //        }}, 3000); // Delay for the alert
                            //    }}, 100); // Initial delay to ensure success_noti is called first
                            //", true);
                            //ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowSuccess", "alert('Changes Updated');window.location.href='" + Request.RawUrl + "';", true);
                            Response.Redirect("/frmAllTickets.aspx");
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowSuccess", "alert('Changes Updated');window.location.href='" + Request.RawUrl + "';", true);

                            //                    ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
                            //$"if (window.location.pathname.endsWith('/frmEditTicketbyAssigne.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Ticket Updated Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 3000); }}", true);


                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowSuccess", "alert('OOPS, Something Went Wrong !!!!');window.location.href='" + Request.RawUrl + "';", true);

                        //                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
                        //$"if (window.location.pathname.endsWith('/frmEditTicketbyAssigne.aspx')) {{ warning_noti('{HttpUtility.JavaScriptStringEncode("" + ErrorChk + "")}'); setTimeout(function() {{ window.location.reload(); }}, 3000); }}", true);


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
        //Response.Redirect(Request.Url.AbsoluteUri);
    }
    protected void CheckForChangeTask()
    {
        try
        {
            
            string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SD_spCheckChangeTaskStatus", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Ticketref", Request.QueryString["TicketId"].ToString());
                    cmd.Parameters.AddWithValue("@Option", "Checkstatus");
                    cmd.Parameters.Add("@ERROR", SqlDbType.Char, 100);
                    cmd.Parameters["@ERROR"].Direction = ParameterDirection.Output;
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    if (ddlStatus.SelectedItem.ToString() == "Resolved" || ddlStatus.SelectedItem.ToString() == "Closed")
                    {
                        if (cmd.Parameters["@ERROR"].Value.ToString().Contains("change"))
                        {

                            if ((txtResolution.Value.ToString() == " " || txtResolution.Value.ToString() == null) || ddlResoultion.SelectedValue == "0")
                            {
                                rfvddlResoultion.Enabled = true;
                                rfvddlResoultion.Visible = true;
                                rfvtxtResolution.Enabled = true;
                                rfvtxtResolution.Visible = true;
                                rfvddlResoultion.InitialValue = "0";
                               string ErrorChk  = "Please select resolution and enter resolution description";
ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
$"if (window.location.pathname.endsWith('/frmEditTicketbyAssigne.aspx')) {{ warning_noti('{HttpUtility.JavaScriptStringEncode("" + ErrorChk + "")}'); setTimeout(function() {{ window.location.reload(); }}, 3000); }}", true);
                            }
                            else
                            {
                                MakeChangeTicket(Request.QueryString["TicketId"]);
                                ADDChangeImpactDetails(Request.QueryString["TicketId"]);
                                ADDChangeRollOutDetails(Request.QueryString["TicketId"]);
                                ADDTaskForEngineer(Request.QueryString["TicketId"]);
                                UpdateTicketDetails();
                            }
                        }
                        else
                        {
                            string ErrorChk  = (string)cmd.Parameters["@ERROR"].Value;
ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
$"if (window.location.pathname.endsWith('/frmEditTicketbyAssigne.aspx')) {{ warning_noti('{HttpUtility.JavaScriptStringEncode("" + ErrorChk + "")}'); setTimeout(function() {{ window.location.reload(); }}, 3000); }}", true);
                        }
                    }
                    else
                    {
                        MakeChangeTicket(Request.QueryString["TicketId"]);
                        ADDChangeImpactDetails(Request.QueryString["TicketId"]);
                        ADDChangeRollOutDetails(Request.QueryString["TicketId"]);
                        ADDTaskForEngineer(Request.QueryString["TicketId"]);
                        UpdateTicketDetails();
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
                Response.Redirect("~/Error/Error.html");

            }

        }
    }
    private string CheckSRForPrevStage()
    {
        string msg = "";
        try
        {
            string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SP_CheckMapping", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DeskRef", ddlRequestType.SelectedValue);
	            cmd.Parameters.AddWithValue("@PrevStage", "");
                    cmd.Parameters.AddWithValue("@StatusID", ddlStatus.SelectedValue);
                    cmd.Parameters.AddWithValue("@OrgDeskRef", Convert.ToString(Session["SD_OrgID"]));
                    cmd.Parameters.AddWithValue("@ScopeName", Convert.ToString(Session["UserScope"]));
                    cmd.Parameters.AddWithValue("@Ticketref", Request.QueryString["TicketId"].ToString());
                    cmd.Parameters.Add("@msg", SqlDbType.Char, 100);
                    cmd.Parameters["@msg"].Direction = ParameterDirection.Output;
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    msg = Convert.ToString(cmd.Parameters["@msg"].Value);
                    if (!string.IsNullOrEmpty(msg))
                    {
                        string ErrorChk  = (string)cmd.Parameters["@msg"].Value;
ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
$"if (window.location.pathname.endsWith('/frmEditTicketbyAssigne.aspx')) {{ warning_noti('{HttpUtility.JavaScriptStringEncode("" + ErrorChk + "")}'); setTimeout(function() {{ window.location.reload(); }}, 3000); }}", true);
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
                Response.Redirect("~/Error/Error.html");

            }

        }
        return msg;
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try

        {
            string redirectUrl = @"frmEditTicketbyAssigne.aspx?TicketId="+Request.QueryString["TicketId"]+"&redirected=false&Desk="+Request.QueryString["Desk"]+"&NamelyId=523843837";
            if (hdfldDesk.Value != ddlRequestType.SelectedValue)
            {
               
                    CloseTicket();
                    MakeTicket();
               
              
            }
            else
            {
                try
                {
                    if (ddlRequestType.SelectedValue.ToString() == "Incident")
                    {
                        CheckForPrevStage();
                        rfvddlAssigne.Enabled = true;
                        rfvddlAssigne.Visible = true;
                        pnlSRFields.Visible = false;
                        rfvddlHodApproval.Enabled = false;
                        rfvddlHodApproval.Visible = false;
                    }

                    else if (ddlRequestType.SelectedValue == "Service Request")
                    {
                        string chk = CheckSRForPrevStage();
                        if (chk != "")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('" + chk + "');window.location ='" + redirectUrl + "';", true);
                            return;
                        }
                        pnlSRFields.Visible = true;
                        if (ddlStatus.SelectedItem.ToString() == "Awaiting HOD Approval" && ddlHodApproval.SelectedValue.ToString() == "0")
                        {
                            rfvddlHodApproval.Enabled = true;
                            rfvddlHodApproval.Visible = true;
                            string ErrorChk = "Please select HOD Email as you have selected Status (Awaiting HOD Approval!!)";
ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
$"if (window.location.pathname.endsWith('/frmEditTicketbyAssigne.aspx')) {{ warning_noti('{HttpUtility.JavaScriptStringEncode("" + ErrorChk + "")}'); setTimeout(function() {{ window.location.reload(); }}, 3000); }}", true);
                        }
                        else if (ddlStatus.SelectedItem.ToString() == "Awaiting HOD Approval" && ddlHodApproval.SelectedValue.ToString() != "0")
                        {
                            rfvddlHodApproval.Enabled = false;
                            rfvddlHodApproval.Visible = false;
                            CheckForCondtions();
                        }
                        else if (ddlStatus.SelectedItem.ToString() != "Awaiting HOD Approval")
                        {
                            rfvddlHodApproval.Enabled = false;
                            rfvddlHodApproval.Visible = false;
                            CheckForCondtions();
                        }

                        rfvddlAssigne.Enabled = false;
                        rfvddlAssigne.Visible = false;
                    }
                    else if (ddlRequestType.SelectedValue == "Change Request")
                    {
                        CheckForChangeTask();
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
                        var frame = st.GetFrame(0);
                        var line = frame.GetFileLineNumber();
                        inEr.InsertErrorLogsF(Session["UserName"].ToString()
                , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                        Response.Redirect("~/Error/Error.html");

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
            if (ex.ToString().Contains("ThreadAbortException"))
            {
            }
            else
            {
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                inEr.InsertErrorLogsF(Session["UserName"].ToString()
        , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                Response.Redirect("~/Error/Error.html");

            }
        }
    }
    protected void MakeTicket()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SD_spSDIncident_Conversion", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", r.Next());
                    cmd.Parameters.AddWithValue("@DeskRef", ddlRequestType.SelectedValue);
                    cmd.Parameters.AddWithValue("@ticketConvert", Request.QueryString["TicketId"].ToString());
                    cmd.Parameters.AddWithValue("@sdPriorityFK", ddlPriority.SelectedValue);
                    cmd.Parameters.AddWithValue("@sdSeverityFK", ddlSeverity.SelectedValue);
                    cmd.Parameters.AddWithValue("@SubmitterID", txtLoginName.Text);
                    cmd.Parameters.AddWithValue("@sdCategoryRef", hdnCategoryID.Value);
                    cmd.Parameters.AddWithValue("@creationDateTime", DateTime.Now);
                    cmd.Parameters.AddWithValue("@TicketSummary", txtSummary.Text);
                    cmd.Parameters.AddWithValue("@TicketDesc", System.Web.HttpUtility.HtmlEncode(txtDescription.Value).ToString());
                    cmd.Parameters.AddWithValue("@submitterType", "");
                    cmd.Parameters.AddWithValue("@submitterName", txtSubmitterName.Text);
                    cmd.Parameters.AddWithValue("@submitterEmailAddr", txtSubmitterEmail.Text);
                    cmd.Parameters.AddWithValue("@submitterPhone", txtPhoneNumber.Text);
                    cmd.Parameters.AddWithValue("@organizationFK", Session["SD_OrgID"].ToString());
                    cmd.Parameters.AddWithValue("@assigneeType", "");
                    cmd.Parameters.AddWithValue("@sourceType", "Desk");
                    cmd.Parameters.AddWithValue("@location", ddlLocation.SelectedValue);
                    cmd.Parameters.AddWithValue("@Department", ddlDepartment.SelectedValue);
                    cmd.Parameters.AddWithValue("@categoryFullText", hdnCategoryID.Value.Replace("||", " - "));
                    cmd.Parameters.Add("@Ticketref", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@Option", "AddTicket");
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    string ticketnumber = cmd.Parameters["@Ticketref"].Value.ToString();
                    ViewState["TicketRef"] = ticketnumber;
                    if (Session["UploadedFiles"] != null)
                    {
                        FileUpload(Request.QueryString["TicketId"].ToString());
                    }
                    UpdateCreatedCustomField(ticketnumber);
                    ADDMailinDBForTicket(ticketnumber);
                  
                    if (ddlRequestType.SelectedItem.ToString().ToLower().Contains("request"))
                    {
                        MakeTicketSR();
                    }
                    if (res > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowSuccess", "alert('Changes Updated');window.location.href='" + Request.RawUrl + "';", true);
        //                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
        //$"if (window.location.pathname.endsWith('/frmEditTicketbyAssigne.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Ticket Updated Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 3000); }}", true);
                  
                    
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
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                inEr.InsertErrorLogsF(Session["UserName"].ToString()
            , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
            }
        }
    }
    private void ADDMailinDBForTicket(string ticketNumber)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SD_Sendmail", con))
                {
                    cmd.Parameters.AddWithValue("@TicketNumber", ticketNumber);
                    cmd.Parameters.AddWithValue("@DeskRef", ddlRequestType.SelectedValue);
 			cmd.Parameters.AddWithValue("@HODApproval", ddlHodApproval.SelectedValue);
                    cmd.Parameters.AddWithValue("@OrgId", Session["SD_OrgID"].ToString());
                    cmd.Parameters.AddWithValue("@Option", "ReminderonTicketCreaton");
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.ExecuteNonQuery();
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
            if (ex.ToString().Contains("System.Threading.Thread.AbortInternal()"))
            {

            }
            else
            {
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                inEr.InsertErrorLogsF(Session["UserName"].ToString()
    , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
            }
        }
    }
    protected void MakeTicketSR()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SD_spSDIncident_Conversion", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ticketout", Convert.ToString(ViewState["TicketRef"]));
                    cmd.Parameters.AddWithValue("@ticketConvert", Request.QueryString["TicketId"].ToString());
                    cmd.Parameters.AddWithValue("@Option", "AddSR_ApproverStatus");
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        Session["Popup"] = "Insert";
                        Response.Redirect(Request.Url.AbsoluteUri);
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
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                inEr.InsertErrorLogsF(Session["UserName"].ToString()
            , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
            }
        }
    }
    protected void UpdateCreatedCustomField(string ticketNo)
    {
        try
        {
            foreach (RepeaterItem item in rptOddControl.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    TextBox C = (TextBox)item.FindControl("txt");
                    Label lbl = (Label)item.FindControl("lbl");
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("SD_spAddSDCustomTicketField", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@TicketNo", ticketNo);
                            cmd.Parameters.AddWithValue("@FieldName", lbl.Text);
                            cmd.Parameters.AddWithValue("@FieldValue", C.Text);
                            cmd.Parameters.AddWithValue("@Option", "UpdateCustomField");
                            con.Open();
                            int res = cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            foreach (RepeaterItem item in rptEvenControl.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    TextBox C = (TextBox)item.FindControl("txteven");
                    Label lbl = (Label)item.FindControl("lbleven");
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("SD_spAddSDCustomTicketField", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@TicketNo", ticketNo);
                            cmd.Parameters.AddWithValue("@FieldName", lbl.Text);
                            cmd.Parameters.AddWithValue("@FieldValue", C.Text);
                            cmd.Parameters.AddWithValue("@Option", "UpdateCustomField");
                            con.Open();
                            int res = cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            foreach (RepeaterItem item in rptddlOddControl.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    DropDownList C = (DropDownList)item.FindControl("ddlOdd");
                    Label lbl = (Label)item.FindControl("lblOddlist");
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("SD_spAddSDCustomTicketField", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@TicketNo", ticketNo);
                            cmd.Parameters.AddWithValue("@FieldName", lbl.Text);
                            cmd.Parameters.AddWithValue("@FieldValue", C.SelectedValue);
                            cmd.Parameters.AddWithValue("@Option", "UpdateCustomField");
                            con.Open();
                            int res = cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            foreach (RepeaterItem item in rptddlEvenControl.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    DropDownList C = (DropDownList)item.FindControl("ddlEven");
                    Label lbl = (Label)item.FindControl("lblEvenlist");
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("SD_spAddSDCustomTicketField", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@TicketNo", ticketNo);
                            cmd.Parameters.AddWithValue("@FieldName", lbl.Text);
                            cmd.Parameters.AddWithValue("@FieldValue", C.SelectedValue);
                            cmd.Parameters.AddWithValue("@Option", "UpdateCustomField");
                            con.Open();
                            int res = cmd.ExecuteNonQuery();
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
            var st = new StackTrace(ex, true);
            var frame = st.GetFrame(0);
            var line = frame.GetFileLineNumber();
            inEr.InsertErrorLogsF(Session["UserName"].ToString()
, " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
        }
    }
    protected void CloseTicket()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SD_spSDIncident_Conversion", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ticketConvert", Request.QueryString["TicketId"].ToString());
                    cmd.Parameters.AddWithValue("@desk", Request.QueryString["Desk"].ToString());
                    cmd.Parameters.AddWithValue("@TickNotes", System.Web.HttpUtility.HtmlDecode(txtNotes.Value));
                    cmd.Parameters.AddWithValue("@ViewToUser", true);
                    cmd.Parameters.AddWithValue("@organizationFK", Session["SD_OrgID"].ToString());
                    cmd.Parameters.AddWithValue("@UserName", Session["UserName"].ToString());
                    cmd.Parameters.AddWithValue("@sdSolutionTypeFK", ddlResoultion.SelectedValue);
                    cmd.Parameters.AddWithValue("@solutionNote", System.Web.HttpUtility.HtmlEncode(txtResolution.Value));
                    cmd.Parameters.AddWithValue("@Option", "CloseTicket");
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
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
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                inEr.InsertErrorLogsF(Session["UserName"].ToString()
    , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti('{HttpUtility.JavaScriptStringEncode(ex.ToString())}'); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
            }
        }
    }
    static string AssigneEmail;
    protected void Sendmail(string body)
    {
        try
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
            string FilePath = Server.MapPath(@"/SDTemplates/UserTicketCreation.htm");
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();

            //Repalce [newusername] = signup user name   
            //MailText = MailText.Replace(body);


            string subject = "Incident Request--" + txtSubmitterName.Text.Trim() + "-" + ddlRequestType.SelectedItem.ToString() + "";

            //Base class for sending email  
            MailMessage _mailmsg = new MailMessage();

            //Make TRUE because our body text is html  
            _mailmsg.IsBodyHtml = true;

            //Set From Email ID  
            _mailmsg.From = new MailAddress(emailSender);

            //Set To Email ID  
            _mailmsg.To.Add(ToEmail.ToString());

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
    protected string GetAssigneEmail(long id)
    {
        try
        {
            DataTable UpdateWIP = new SDCustomFields().GetAssigneEmail(Convert.ToInt64(ddlAssigne.SelectedValue));
            if (UpdateWIP.Rows.Count > 0)
            {
                foreach (DataRow row in UpdateWIP.Rows)
                {
                    AssigneEmail = row["EmailID"].ToString();
                }
            }
            return AssigneEmail;
        }
        catch (ThreadAbortException e2)
        {
            Console.WriteLine("Exception message: {0}", e2.Message);
            Thread.ResetAbort();
            return null;
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


            }

            return null;
        }
    }
    private void ADDMailinDB(string ticketNumber)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand("SD_Sendmail", con))
                {
                    cmd.Parameters.AddWithValue("@TicketNumber", ticketNumber);
 cmd.Parameters.AddWithValue("@HODApproval", ddlHodApproval.SelectedValue);
                  
                    cmd.Parameters.AddWithValue("@OrgId", Request.QueryString["NamelyId"].ToString());
                    cmd.Parameters.AddWithValue("@Option", "AssigneUpdate");
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.ExecuteNonQuery();
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
                    cmd.Parameters.AddWithValue("@OrgRef", Request.QueryString["NamelyId"].ToString());
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
                                grdAttachmentView.DataSource = dt;
                                grdAttachmentView.DataBind();
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
    protected void lnkView_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        GridViewRow gvr = (GridViewRow)btn.NamingContainer;
        HiddenField hdnfullPath = (HiddenField)gvr.FindControl("hdnfullPath");
        string fullPath = hdnfullPath.Value;
        //string fileName = System.IO.Path.GetFileName(fullPath);
        //string relativePath = fileName;
        string url = ResolveUrl(fullPath);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "openFile", "window.open('" + url + "', '_blank');", true);
    }
    protected void CheckForCondtions()
    {
        try
        {
            if (ddlStatus.SelectedItem.ToString() == "Resolved" || ddlStatus.SelectedItem.ToString() == "Closed")
            {
                if ((txtResolution.Value.ToString() == " " || txtResolution.Value.ToString() == null) || ddlResoultion.SelectedValue == "0")
                {
                    rfvddlResoultion.Enabled = true;
                    rfvddlResoultion.Visible = true;
                    rfvtxtResolution.Enabled = true;
                    rfvtxtResolution.Visible = true;
                    rfvddlAssigne.Enabled = true;
                    ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
        $"if (window.location.pathname.endsWith('/frmEditTicketbyAssigne.aspx')) {{ warning_noti('{HttpUtility.JavaScriptStringEncode("Please select resolution and enter resolution description")}'); setTimeout(function() {{ window.location.reload(); }}, 4000); }}", true);
                }
                else
                {
                    if (ddlAssigne.SelectedValue == "0")
                    {
                        rfvddlAssigne.Visible = true;
                        rfvddlResoultion.InitialValue = "0";
                        string ErrorChk = "Please assign Technician First!!";
ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
$"if (window.location.pathname.endsWith('/frmEditTicketbyAssigne.aspx')) {{ warning_noti('{HttpUtility.JavaScriptStringEncode("" + ErrorChk + "")}'); setTimeout(function() {{ window.location.reload(); }}, 3000); }}", true);
                    }
                    else
                    {
                        UpdateTicketDetails();
                    }

                }
            }
            else
            {
                UpdateTicketDetails();
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
    private void CheckForPrevStage()
    {
        try
        {
            string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SD_spCheckTicketStatus", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Ticketref", Request.QueryString["TicketId"].ToString());
                    cmd.Parameters.AddWithValue("@organizationFK", Request.QueryString["NamelyId"].ToString());
                  //  cmd.Parameters.AddWithValue("@sdStageFK", ddlStage.SelectedValue);
                    cmd.Parameters.AddWithValue("@sdStatusFK", ddlStatus.SelectedValue);
                    cmd.Parameters.AddWithValue("@Option", "Checkstatus");
                    cmd.Parameters.Add("@ERROR", SqlDbType.Char, 100);
                    cmd.Parameters["@ERROR"].Direction = ParameterDirection.Output;
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    if (cmd.Parameters["@ERROR"].Value.ToString().Contains("change"))
                    {
                        if (ddlStatus.SelectedItem.ToString() == "Resolved" || ddlStatus.SelectedItem.ToString() == "Closed")
                        {
                            if ((txtResolution.Value.ToString() == " " || txtResolution.Value.ToString() == null) || ddlResoultion.SelectedValue == "0" || string.IsNullOrEmpty(txtResolution.Value.ToString()))
                            {
                                rfvddlResoultion.Enabled = true;
                                rfvddlResoultion.Visible = true;
                                rfvtxtResolution.Enabled = true;
                                rfvtxtResolution.Visible = true;
                                rfvddlResoultion.InitialValue = "0";
                                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
        $"if (window.location.pathname.endsWith('/frmEditTicketbyAssigne.aspx')) {{ warning_noti('{HttpUtility.JavaScriptStringEncode("Please select resolution and enter resolution description")}'); setTimeout(function() {{ window.location.reload(); }}, 4000); }}", true);
                                //        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
                                //$"if (window.location.pathname.endsWith('/frmEditTicketbyAssigne.aspx')) {{ warning_noti('{HttpUtility.JavaScriptStringEncode("Please select resolution and enter resolution description")}'); setTimeout(function() {{ window.location.href = 'frmAllTickets.aspx'; }}, 4000); }}", true);

                                //lblMsg.Text = "Please select resolution and enter resolution description";
                            }
                            else
                            {
                                UpdateTicketDetails();
                            }
                        }
                        else
                        {
                            rfvddlResoultion.Enabled = false;
                            rfvddlResoultion.Visible = false;
                            rfvtxtResolution.Enabled = false;
                            rfvtxtResolution.Visible = false;
                            UpdateTicketDetails();
                        }
                    }
                    else
                    {
                        string ErrorChk = (string)cmd.Parameters["@ERROR"].Value;
ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
$"if (window.location.pathname.endsWith('/frmEditTicketbyAssigne.aspx')) {{ warning_noti('{HttpUtility.JavaScriptStringEncode("" + ErrorChk + "")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
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
    protected void AddTicketNotes()
    {
        try
        {
            DataTable SD_Resolution = new FillSDFields().FillTicketNotes(Request.QueryString["TicketId"].ToString(), Request.QueryString["NamelyId"].ToString()); ;
            if (SD_Resolution.Rows.Count > 0)
            {
                //  this.lb.Text = dataTable.Rows.Count.ToString();
                this.gvTicketNotes.DataSource = (object)SD_Resolution;
                this.gvTicketNotes.DataBind();
            }
            else
            {
                this.gvTicketNotes.DataSource = (object)null;
                this.gvTicketNotes.DataBind();
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
    protected void UpdateTicketPanel()
    {
        try
        {
            pnlTicket.Visible = true;

            pnlUpdateTicket.Visible = true;
            btnUpdateTickView.CssClass = "btn btn-sm  btn-secondary ";
            pnlViewNotes.Visible = false;
            btnViewNotes.CssClass = "btn btn-sm   btn-outline-secondary ";


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
    protected void btnUpdateTickView_Click(object sender, EventArgs e)
    {
        Response.AppendHeader("Refresh", "1;url=frmEditTicketbyAssigne.aspx?TicketId=" + Request.QueryString["TicketId"] + "&redirected=false&Desk=" + Request.QueryString["Desk"] + "&NamelyId=" + Request.QueryString["NamelyId"] + "");
        UpdateTicketPanel();
    }
    protected void btnViewNotes_Click(object sender, EventArgs e)
    {
        try
        {
            ShowTicketAttachment();
            GetFileAttached();
            AInotes.Visible = false;
            pnlUpdateTicket.Visible = false;

            pnlViewNotes.Visible = true;
            btnViewNotes.CssClass = "btn btn-sm   btn-secondary ";
            btnUpdateTickView.Enabled = true;

            pnlTicket.Visible = false;
            pnlShowRollOutDetails.Visible = false;
            //pnlAddRollout.Visible = false;
            //	pnlRollOutGrid.Visible = false;
            pnlIncident.Visible = false;
            pnlShowImpactDetails.Visible = false;
            //pnlImpactGrid.Visible = false;
            //pnlAddImpact.Visible = false;
            pnlDownTime.Visible = false;
            pnlTaksAssociation.Visible = false;
            AddTicketNotes();

            btnUpdateTickView.CssClass = "btn btn-sm btnDisabled btn-outline-secondary ";
            btnImpactDetails.CssClass = "btn btn-sm btnDisabled btn-outline-secondary ";
            btnRolloutPlan.CssClass = "btn btn-sm btnDisabled btn-outline-secondary ";
            btnDowntime.CssClass = "btn btn-sm btnDisabled btn-outline-secondary ";
            btnTaskAssociation.CssClass = "btn btn-sm btnDisabled btn-outline-secondary ";
            btnViwPyres.CssClass = "btn btn-sm btnDisabled btn-outline-secondary  ";
            btnDownloadKb.CssClass = "btn btn-sm btnDisabled btn-outline-secondary  ";
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
    protected void ImgBtnExport_Click(object sender, EventArgs e)
    {

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
            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

        }
    }
    public static int AssigneClick;
    public static string ToEmail;
    protected void ddlAssigne_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Session["AssigneUpdate"] = "True";
            ToEmail = GetAssigneEmail(Convert.ToInt64(ddlAssigne.SelectedValue));
            ShowCustomFields(ddlRequestType.SelectedItem.Text, Request.QueryString["NamelyId"].ToString());
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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
            }

        }
    }
    protected void ddlSeverity_SelectedIndexChanged(object sender, EventArgs e)
    {
        string priority = "0";
        try
        {
            if (ddlSeverity.SelectedItem.Text.Contains("S1"))
            {
                priority = "High";
            }
            else if (ddlSeverity.SelectedItem.Text.Contains("S2"))
            {
                priority = "Medium";
            }
            else if (ddlSeverity.SelectedItem.Text.Contains("S3"))
            {
                priority = "Low";
            }
            else
            {

            }

            string sql = "select ID from SD_Priority where DeskRef='" + ddlRequestType.SelectedValue + "' and" +
                " OrgDeskRef='" + Session["SD_OrgID"].ToString() + "' and PriorityCodeRef Like '%" + priority + "%'";
            string Priority = Convert.ToString(database.GetScalarValue(sql));
            if (Priority != "")
            {
                ddlPriority.SelectedValue = Priority;
                ddlPriority.Enabled = false;
                rfvddlPriority.Visible = false;
            }
            ShowCustomFields(ddlRequestType.SelectedItem.Text, Request.QueryString["NamelyId"].ToString());
        }
        catch (Exception ex)
        {

        }
    }
    protected void ddlPriority_SelectedIndexChanged(object sender, EventArgs e)
    {
        string Severity = "0";
        try
        {
            if (ddlPriority.SelectedItem.Text.Contains("High"))
            {
                Severity = "S1";
            }
            else if (ddlPriority.SelectedItem.Text.Contains("Medium"))
            {
                Severity = "S2";
            }
            else if (ddlPriority.SelectedItem.Text.Contains("Low"))
            {
                Severity = "S3";
            }
            else
            {

            }
            string sql = "select ID from SD_Severity where DeskRef='" + ddlRequestType.SelectedValue + "' and" +
                " OrgDeskRef='" + Session["SD_OrgID"].ToString() + "' and Serveritycoderef Like '%" + Severity + "%'";
            string Severityid = Convert.ToString(database.GetScalarValue(sql));
            if (Severityid != "")
            {
                ddlSeverity.SelectedValue = Severityid;
                ddlSeverity.Enabled = false;
                RfvddlSeverity.Visible = false;
            }
            ShowCustomFields(ddlRequestType.SelectedItem.Text, Request.QueryString["NamelyId"].ToString());
        }
        catch (Exception ex)
        {

        }
    }
    protected void ddlStage_SelectedIndexChanged(object sender, EventArgs e)
    {
        ShowCustomFields(ddlRequestType.SelectedItem.Text, Request.QueryString["NamelyId"].ToString());
        //FillStatus();
    }

    /// <summary>
    /// ////////  this section is all about change management
    /// </summary>


    //////////////////////////////////////////////     This will add default impact details  ////////////////////////////////////////////////////////
    private void AddDefaultFirstRecordForImpact()
    {
        //creating dataTable 
        DataTable dt = new DataTable();
        DataRow dr;
        dt.TableName = "Impact";
        dt.Columns.Add(new DataColumn("ImpactDetails", typeof(string)));

        dr = dt.NewRow();
        dt.Rows.Add(dr);
        //saving databale into viewstate 
        ViewState["Impact"] = dt;
        //bind Gridview
        gridAddImpact.DataSource = dt;
        gridAddImpact.DataBind();
    }
    private void AddNewRecordRowToGridForImpact()
    {
        // check view state is not null
        if (ViewState["Impact"] != null)
        {
            //get datatable from view state 
            DataTable dtCurrentTable = (DataTable)ViewState["Impact"];
            DataRow drCurrentRow = null;

            if (dtCurrentTable.Rows.Count > 0)
            {

                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {

                    //add each row into data table
                    drCurrentRow = dtCurrentTable.NewRow();
                    drCurrentRow["ImpactDetails"] = txtImpactDesc.Text;



                }
                //Remove initial blank row
                if (dtCurrentTable.Rows[0][0].ToString() == "")
                {
                    dtCurrentTable.Rows[0].Delete();
                    dtCurrentTable.AcceptChanges();

                }

                //add created Rows into dataTable
                dtCurrentTable.Rows.Add(drCurrentRow);
                //Save Data table into view state after creating each row
                ViewState["Impact"] = dtCurrentTable;
                //Bind Gridview with latest Row
                gridAddImpact.DataSource = dtCurrentTable;
                gridAddImpact.DataBind();
            }
        }
        txtImpactDesc.Text = string.Empty;


        txtImpactDesc.Focus();

    }
    private void AddDefaultFirstRecordForTask()
    {
        //creating dataTable 
        DataTable dt = new DataTable();
        DataRow dr;
        dt.TableName = "Task";
        dt.Columns.Add(new DataColumn("TaskDescription", typeof(string)));
        dt.Columns.Add(new DataColumn("Status", typeof(string)));
        dt.Columns.Add(new DataColumn("EngineerAssociation", typeof(string)));
        dr = dt.NewRow();
        dt.Rows.Add(dr);
        //saving databale into viewstate 
        ViewState["Task"] = dt;
        //bind Gridview
        gvAddTask.DataSource = dt;
        gvAddTask.DataBind();
    }
    protected void btnUpdateTickView1_Click(object sender, EventArgs e)
    {
        pnlIncident.Visible = true;
        pnlShowImpactDetails.Visible = false;
        pnlDownTime.Visible = false;
        pnlShowRollOutDetails.Visible = false;
        pnlTaksAssociation.Visible = false;

        btnUpdateTickView.CssClass = "btn btn-sm  btn-secondary ";
        btnImpactDetails.CssClass = "btn btn-sm  btn-outline-secondary ";
        btnRolloutPlan.CssClass = "btn btn-sm  btn-outline-secondary ";
        btnDowntime.CssClass = "btn btn-sm  btn-outline-secondary ";
        btnTaskAssociation.CssClass = "btn btn-sm  btn-outline-secondary ";
    }

    protected void btnImpactDetails_Click(object sender, EventArgs e)
    {
        pnlTicket.Visible = true;
        pnlUpdateTicket.Visible = false;
        pnlShowImpactDetails.Visible = true;
        //pnlAddImpact.Visible = false;
        //pnlImpactGrid.Visible = true;
        pnlIncident.Visible = false;

        pnlDownTime.Visible = false;
        pnlShowRollOutDetails.Visible = false;
        pnlTaksAssociation.Visible = false;
        pnlViewNotes.Visible = false;

        btnUpdateTickView.CssClass = "btn btn-sm  btn-outline-secondary ";
        btnImpactDetails.CssClass = "btn btn-sm  btn-secondary ";
        btnRolloutPlan.CssClass = "btn btn-sm  btn-outline-secondary ";
        btnDowntime.CssClass = "btn btn-sm  btn-outline-secondary ";
        btnTaskAssociation.CssClass = "btn btn-sm  btn-outline-secondary ";
        btnViewNotes.CssClass = "btn btn-sm  btn-outline-secondary ";
        btnDownloadKb.CssClass = "btn btn-sm btnDisabled btn-outline-secondary  ";
    }

    protected void btnRolloutPlan_Click(object sender, EventArgs e)
    {
        pnlTicket.Visible = true;
        pnlUpdateTicket.Visible = false;
        pnlShowRollOutDetails.Visible = true;
        //	pnlAddRollout.Visible = false;
        //	pnlRollOutGrid.Visible = true;
        pnlIncident.Visible = false;
        pnlShowImpactDetails.Visible = false;
        pnlDownTime.Visible = false;
        pnlTaksAssociation.Visible = false;
        pnlViewNotes.Visible = false;

        btnUpdateTickView.CssClass = "btn btn-sm  btn-outline-secondary ";
        btnImpactDetails.CssClass = "btn btn-sm  btn-outline-secondary ";
        btnRolloutPlan.CssClass = "btn btn-sm  btn-secondary ";
        btnDowntime.CssClass = "btn btn-sm  btn-outline-secondary ";
        btnTaskAssociation.CssClass = "btn btn-sm  btn-outline-secondary ";
        btnViewNotes.CssClass = "btn btn-sm  btn-outline-secondary ";
        btnDownloadKb.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
    }

    protected void btnAddImpactDetails_Click(object sender, EventArgs e)
    {
        AddNewRecordRowToGridForImpact();
    }


    protected void btnAddRollOutGrid_Click(object sender, EventArgs e)
    {
        AddNewRecordRowToGridForRollOut();
    }
    private void AddDefaultFirstRecordForRollOut()
    {
        //creating dataTable 
        DataTable dt = new DataTable();
        DataRow dr;
        dt.TableName = "RollOut";
        dt.Columns.Add(new DataColumn("RollOutDetails", typeof(string)));

        dr = dt.NewRow();
        dt.Rows.Add(dr);
        //saving databale into viewstate 
        ViewState["RollOut"] = dt;
        //bind Gridview
        gridAddRollOut.DataSource = dt;
        gridAddRollOut.DataBind();
    }
    private void AddNewRecordRowToGridForRollOut()
    {
        // check view state is not null
        if (ViewState["RollOut"] != null)
        {
            //get datatable from view state 
            DataTable dtCurrentTable = (DataTable)ViewState["RollOut"];
            DataRow drCurrentRow = null;

            if (dtCurrentTable.Rows.Count > 0)
            {

                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {

                    //add each row into data table
                    drCurrentRow = dtCurrentTable.NewRow();
                    drCurrentRow["RollOutDetails"] = txtRollOut.Text;



                }
                //Remove initial blank row
                if (dtCurrentTable.Rows[0][0].ToString() == "")
                {
                    dtCurrentTable.Rows[0].Delete();
                    dtCurrentTable.AcceptChanges();

                }

                //add created Rows into dataTable
                dtCurrentTable.Rows.Add(drCurrentRow);
                //Save Data table into view state after creating each row
                ViewState["RollOut"] = dtCurrentTable;
                //Bind Gridview with latest Row
                gridAddRollOut.DataSource = dtCurrentTable;
                gridAddRollOut.DataBind();
            }
        }
        txtRollOut.Text = string.Empty;


        txtRollOut.Focus();

    }
    private void AddNewRecordRowToGridForTask()
    {
        // check view state is not null
        if (ViewState["Task"] != null)
        {
            //get datatable from view state 
            DataTable dtCurrentTable = (DataTable)ViewState["Task"];
            DataRow drCurrentRow = null;

            if (dtCurrentTable.Rows.Count > 0)
            {

                for (int i = 1; i <= dtCurrentTable.Rows.Count; i++)
                {
                    string message = "";
                    foreach (ListItem item in lstTechAssoc.Items)
                    {
                        if (item.Selected)
                        {
                            message += item.Text + ",";
                        }
                    }
                    message = message.TrimEnd(',');
                    //add each row into data table
                    drCurrentRow = dtCurrentTable.NewRow();
                    drCurrentRow["TaskDescription"] = txtTaskSummary.Text;
                    drCurrentRow["Status"] = ddlTaskStatus.SelectedValue;
                    drCurrentRow["EngineerAssociation"] = message;

                }
                //Remove initial blank row
                if (dtCurrentTable.Rows[0][0].ToString() == "")
                {
                    dtCurrentTable.Rows[0].Delete();
                    dtCurrentTable.AcceptChanges();

                }

                //add created Rows into dataTable
                dtCurrentTable.Rows.Add(drCurrentRow);
                //Save Data table into view state after creating each row
                ViewState["Task"] = dtCurrentTable;
                //Bind Gridview with latest Row
                gvAddTask.DataSource = dtCurrentTable;
                gvAddTask.DataBind();
            }
        }
        txtTaskSummary.Text = string.Empty;
        txtTaskSummary.Focus();
        ddlTaskStatus.ClearSelection();
        ddlTaskStatus.Focus();
        lstTechAssoc.ClearSelection();
        lstTechAssoc.Focus();

    }
    protected void btnDowntime_Click(object sender, EventArgs e)
    {
        pnlIncident.Visible = false;
        pnlShowImpactDetails.Visible = false;

        pnlShowRollOutDetails.Visible = false;
        pnlTaksAssociation.Visible = false;
        pnlDownTime.Visible = true;
        pnlViewNotes.Visible = false;

        pnlTicket.Visible = true;
        pnlUpdateTicket.Visible = false;
        btnUpdateTickView.CssClass = "btn btn-sm  btn-outline-secondary ";
        btnImpactDetails.CssClass = "btn btn-sm  btn-outline-secondary ";
        btnRolloutPlan.CssClass = "btn btn-sm  btn-outline-secondary ";
        btnDowntime.CssClass = "btn btn-sm  btn-secondary ";
        btnTaskAssociation.CssClass = "btn btn-sm  btn-outline-secondary ";
        btnViewNotes.CssClass = "btn btn-sm  btn-outline-secondary ";
        btnDownloadKb.CssClass = "btn btn-sm btnDisabled btn-outline-secondary  ";

    }
    protected void btnTaskAssociationShowPanel_Click(object sender, EventArgs e)
    {
        pnlTicket.Visible = true;
        pnlUpdateTicket.Visible = false;
        pnlShowRollOutDetails.Visible = false;
        //pnlAddRollout.Visible = false;
        //	pnlRollOutGrid.Visible = false;
        pnlIncident.Visible = false;
        pnlShowImpactDetails.Visible = false;
        //pnlImpactGrid.Visible = false;
        //pnlAddImpact.Visible = false;
        pnlDownTime.Visible = false;
        pnlTaksAssociation.Visible = true;

        pnlViewNotes.Visible = false;

        btnUpdateTickView.CssClass = "btn btn-sm  btn-outline-secondary ";
        btnImpactDetails.CssClass = "btn btn-sm  btn-outline-secondary ";
        btnRolloutPlan.CssClass = "btn btn-sm  btn-outline-secondary ";
        btnDowntime.CssClass = "btn btn-sm  btn-outline-secondary ";
        btnTaskAssociation.CssClass = "btn btn-sm  btn-secondary ";
        btnViewNotes.CssClass = "btn btn-sm  btn-outline-secondary ";
        btnDownloadKb.CssClass = "btn btn-sm btnDisabled btn-outline-secondary  ";

    }
    protected void btnAddTaskAssociationData_Click(object sender, EventArgs e)
    {
        AddNewRecordRowToGridForTask();
    }

    protected void btnViwpYres_Click(object sender, EventArgs e)
    {
        //ScriptManager.RegisterStartupScript(this, GetType(), "ShowLoader", "showLoader();", true);
        pnlUpdateTicket.Visible = false;
        btnViwPyres.CssClass = "btn btn-sm btn-secondary   ";
        pnlTicket.Visible = false;
        pnlShowRollOutDetails.Visible = false;
        pnlIncident.Visible = false;
        pnlShowImpactDetails.Visible = false;
        pnlDownTime.Visible = false;
        pnlTaksAssociation.Visible = false;
        lnkKBDownload.Visible = false;

        btnUpdateTickView.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
        btnImpactDetails.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
        btnRolloutPlan.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
        btnDowntime.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
        btnTaskAssociation.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
        btnViewNotes.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
        btnDownloadKb.CssClass = "btn btn-sm btnDisabled btn-outline-secondary  ";
        pnlViewNotes.Visible = false;
        AInotes.Visible = true;

        string output = RunPythonAINotes(); // Assuming this returns a string
        //ScriptManager.RegisterStartupScript(this, GetType(), "HideLoader", "hideLoader();", true);
        ViewState["Notes"] = output;
        lblPyoutput.Text = "";
        string script = $@"typewriterEffect(`{output.Replace("`", "\\`")}`, '{lblPyoutput.ClientID}', 30);";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "TypewriterEffect", script, true);

    }
    protected string RunPythonAINotes()
    {
        string ConncetionString = GetDatabaseName();
        string output = "";
        try
        {
            Process cmdProcess = new Process();
            ProcessStartInfo cmdStartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            cmdProcess.StartInfo = cmdStartInfo;
            cmdProcess.Start();
            using (StreamWriter sw = cmdProcess.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    sw.WriteLine("C:\\Users\\Administrator\\Documents\\Abhishek_GenAI\\Summ_GenAI.bat " + Convert.ToString(Request.QueryString["TicketId"]) + " " + Convert.ToString(ConncetionString));
                    sw.WriteLine("exit");
                }
            }
            output = cmdProcess.StandardOutput.ReadToEnd();
            output = output.Replace("Microsoft Windows [Version 10.0.20348.2700]", ""); // Example replacement
            output = output.Replace("C:\\Program Files\\IIS Express>", ""); // Remove command prompt path
            output = output.Replace("(c) Microsoft Corporation. All rights reserved.C:\\Users\\Administrator\\Documents\\Abhishek_GenAI\\Summ_GenAI.bat ", "");
            output = output.Replace("\r", "");
            output = output.Replace("(c) Microsoft Corporation. All rights reserved.", "");
            output = output.Replace("C:\\Users\\Administrator\\Documents\\Abhishek_GenAI\\Summ_GenAI.bat " + Convert.ToString(Request.QueryString["TicketId"]) + " " + Convert.ToString(ConncetionString), "").Trim();
            output = output.Replace("\n", "\n"); // Add more replacements as needed
            output = output.Replace("assistant<|end_header_id|>", "");


            // Trim the output to remove any leading or trailing whitespace
            output = output.Trim();
            string error = cmdProcess.StandardError.ReadToEnd();
            cmdProcess.WaitForExit();
            if (!string.IsNullOrEmpty(error))
            {
                //	MessageBox.Show("Error occurred:\n\n{error}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                //	MessageBox.Show("Commands and script executed successfully. Output:\n\n{output}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        catch (Exception ex)
        {
            //MessageBox.Show("An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        return output;
    }

    protected void btnDownloadKb_Click(object sender, EventArgs e)
    {
        pnlUpdateTicket.Visible = false;
        btnDownloadKb.CssClass = "btn btn-sm btn-secondary  ";
        pnlTicket.Visible = false;
        pnlShowRollOutDetails.Visible = false;
        pnlIncident.Visible = false;
        pnlShowImpactDetails.Visible = false;
        pnlDownTime.Visible = false;
        pnlTaksAssociation.Visible = false;
        lnkKBDownload.Visible = true;

        btnViwPyres.CssClass = "btn btn-sm btnDisabled btn-outline-secondary   ";
        btnUpdateTickView.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
        btnImpactDetails.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
        btnRolloutPlan.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
        btnDowntime.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
        btnTaskAssociation.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
        btnViewNotes.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
        pnlViewNotes.Visible = false;
        AInotes.Visible = true;
        string output = RunPythonKB();
        lblPyoutput.Text = "";
        string script = $@"typewriterEffect(`{output.Replace("`", "\\`")}`, '{lblPyoutput.ClientID}', 5);";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "TypewriterEffect", script, true);
    }
    protected string RunPythonKB()
    {
        string ConncetionString = GetDatabaseName();
        string output = "";
        try
        {
            Process cmdProcess = new Process();
            ProcessStartInfo cmdStartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            cmdProcess.StartInfo = cmdStartInfo;
            cmdProcess.Start();

            using (StreamWriter sw = cmdProcess.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    sw.WriteLine(AppDomain.CurrentDomain.BaseDirectory + "AbhishekGenAi\\KB_GenAI.bat " + Convert.ToString(Request.QueryString["TicketId"]) + " " + Convert.ToString(ConncetionString));

                    sw.WriteLine("exit");
                }
            }
            output = cmdProcess.StandardOutput.ReadToEnd();
            output = output.Replace(AppDomain.CurrentDomain.BaseDirectory + "AbhishekGenAi\\KB_GenAI.bat " + Convert.ToString(Request.QueryString["TicketId"]) + " " + Convert.ToString(ConncetionString), "").Trim();
            output = output.Replace("Microsoft Windows [Version 10.0.20348.2700]", "").Trim();
            output = output.Replace("(c) Microsoft Corporation. All rights reserved.", "").Trim();
            output = output.Replace("C:\\Program Files\\IIS Express>", "").Trim();
            output = output.Replace("*/", "").Trim();
            output = output.Replace("Here is the formatted knowledge base article:\r\n", "").Trim();
            string[] arr = output.Split(new string[] { "$$$" }, StringSplitOptions.None);
            string filename = "";
            if (arr.Length > 0)
            {
                filename = arr[1];
                output = arr[0];
            }
            string error = cmdProcess.StandardError.ReadToEnd();
            cmdProcess.WaitForExit();
            ViewState["FilenName"] = filename;
            if (!string.IsNullOrEmpty(error))
            {
                //	MessageBox.Show("Error occurred:\n\n{error}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                //	MessageBox.Show("Commands and script executed successfully. Output:\n\n{output}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        catch (Exception ex)
        {
            //MessageBox.Show("An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        return output;
    }
    protected void btnUpload_Click(object sender, EventArgs e)
    {
        try
        {
            Int64 Id = 0;
            if (FileUploadTickDoc.HasFiles)
            {
                DataTable dt = new DataTable();
                if (ViewState["files"] != null)
                {
                    dt = ViewState["files"] as DataTable;
                    Id = Convert.ToInt64(ViewState["ID"]);
                }
                string tempPath = Server.MapPath("~/Temp/");
                if (!Directory.Exists(tempPath))
                {
                    Directory.CreateDirectory(tempPath);
                }
                List<string> uploadedFiles = new List<string>();

                foreach (HttpPostedFile postedFile in FileUploadTickDoc.PostedFiles)
                {
                    Id++;
                    ViewState["ID"] = Id;
                    string fileName = Path.GetFileName(postedFile.FileName);
                    string fullPath = Path.Combine(tempPath, fileName);

                    try
                    {
                        postedFile.SaveAs(fullPath);
                        uploadedFiles.Add(fileName);
                    }
                    catch (Exception ex)
                    {
                        Response.Write("Error uploading file: " + fileName + " - " + ex.Message);
                    }
                    if (ViewState["files"] == null)
                    {
                        dt.Columns.Add("ID", typeof(Int64));
                        dt.Columns.Add("FileName", typeof(string));
                        dt.Columns.Add("FullPath", typeof(string));
                        dt.Rows.Add(Id, fileName, fullPath);
                        dt.AcceptChanges();
                        ViewState["files"] = dt;
                    }
                    else
                    {
                        dt.Rows.Add(Id, fileName, fullPath);
                    }
                }
                StringBuilder sw = new StringBuilder();
                if (ViewState["FileName"] != null)
                {
                    sw = ViewState["FileName"] as StringBuilder;
                }
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        sw.Append(Convert.ToString(dt.Rows[i]["FileName"]) + ", ");
                        //lblinvoiceupload.Text = string.Join(", ", uploadedFiles);
                    }
                    ViewState["FileName"] = sw;
                }
                //lblinvoiceupload.Text = Convert.ToString(ViewState["FileName"]);
                // Store the uploaded file names and temp path in session
                //Session["UploadedFileNames"] = uploadedFiles;
                Session["UploadedTempFilePath"] = tempPath;
                Session["UploadedFiles"] = dt;
                grd.DataSource = dt;
                grd.DataBind();
            }
            ShowCustomFields(ddlRequestType.SelectedItem.Text, Request.QueryString["NamelyId"].ToString());
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
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                inEr.InsertErrorLogsF(Session["UserName"].ToString()
    , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }

        }
    }
    
    public void FileUpload(string Ticketref)
    {
        try
        {
            string tempFilePath = Convert.ToString(Session["UploadedTempFilePath"]);
            DataTable dt = Session["UploadedFiles"] as DataTable;

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SD_spTicketFileupload", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 3600;

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        string FolderPath = Server.MapPath("/TicketAttachment/") + Convert.ToString(Session["SD_OrgID"].ToString());
                        //	string FolderPath = Server.MapPath("\\TicketAttachment\\");


                        if (!Directory.Exists(FolderPath))
                        {
                            Directory.CreateDirectory(FolderPath);
                        }
                        string TicketIdFolder = FolderPath + "\\" + Ticketref;
                        if (!Directory.Exists(TicketIdFolder))
                        {
                            Directory.CreateDirectory(TicketIdFolder);
                        }
                      

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string originalFileName = Convert.ToString(dt.Rows[i]["fileName"]);
                            if (!string.IsNullOrEmpty(Convert.ToString(dt.Rows[i]["fileName"])))
                            {
                                string fileExtension = Path.GetExtension(originalFileName);
                                string fileNameWithoutExt = Path.GetFileNameWithoutExtension(originalFileName);

                                // Append timestamp to avoid filename conflicts
                                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                                string newFileName = $"{fileNameWithoutExt}_{timestamp}{fileExtension}";

                                string sourceFile = Path.Combine(tempFilePath, originalFileName);
                                string destinationFile = Path.Combine(TicketIdFolder, newFileName);
                                // string sourceFile = Path.Combine(tempFilePath, Convert.ToString(dt.Rows[i]["fileName"]));
                                //   string destinationFile = Path.Combine(finalPath, GetUniqueFileName(finalPath, Convert.ToString(dt.Rows[i]["fileName"])));

                                try
                                {
                                    if (File.Exists(sourceFile))
                                    {
                                        File.Move(sourceFile, destinationFile);

                                        // Database code to save the file details
                                        cmd.Parameters.Clear();
                                        cmd.Parameters.AddWithValue("@ID", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@TicketID", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@Ticketref", Ticketref);
                                        cmd.Parameters.AddWithValue("@Filepath", "/TicketAttachment/518100112/" + Ticketref + "/" + Path.GetFileName(destinationFile));
                                        cmd.Parameters.AddWithValue("@EntryDt", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@InsertDt", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@UpdateBy", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@UpdateDt", DBNull.Value);
                                        cmd.Parameters.AddWithValue("@Filename", Path.GetFileName(destinationFile));
                                        cmd.Parameters.AddWithValue("@OrgRef", Session["SD_OrgID"].ToString());
                                        cmd.Parameters.AddWithValue("@Option", "AddTicketAttach");
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Response.Write("Error processing file: " + Convert.ToString(dt.Rows[i]["fileName"]) + " - " + ex.Message);
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Log or handle the general error
            Response.Write("Error: " + ex.Message);
        }
    }
    private string GetUniqueFileName(string directory, string fileName)
    {
        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
        string fileExtension = Path.GetExtension(fileName);
        string uniqueFileName = fileName;
        int counter = 1;

        while (File.Exists(Path.Combine(directory, uniqueFileName)))
        {
            uniqueFileName = $"{fileNameWithoutExtension}_{counter}{fileExtension}";
            counter++;
        }

        return uniqueFileName;
    }
    protected void lnkViewUp_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        GridViewRow gvr = (GridViewRow)btn.NamingContainer;
        HiddenField hdnfullPath = (HiddenField)gvr.FindControl("hdnfullPathUp");
        string fullPath = hdnfullPath.Value;
        string fileName = System.IO.Path.GetFileName(fullPath);
        string relativePath = "~/Temp/" + fileName;
        string url = ResolveUrl(relativePath);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "openFile", "window.open('" + url + "', '_blank');", true);
    }
    protected void lnkbtnDelete_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        GridViewRow gvr = (GridViewRow)btn.NamingContainer;
        HiddenField hdnid = (HiddenField)gvr.FindControl("hdnidUp");
        string uid = hdnid.Value;
        DataTable dt = Session["UploadedFiles"] as DataTable;
        if (dt != null)
        {
            Int64 id = Convert.ToInt64(uid);
            DataRow[] rows = dt.Select("ID = " + id);
            if (rows.Length > 0)
            {
                dt.Rows.Remove(rows[0]);
            }
            Session["UploadedFiles"] = dt;
            ViewState["files"] = dt;
            grd.DataSource = dt;
            grd.DataBind();
        }
    }
    protected void btnUpdateClose_Click(object sender, EventArgs e)
    {
        ViewState["btnType"] = "Redirect";
        btnUpdate_Click(null, null);
    }
    protected void lnkKBDownload_Click(object sender, EventArgs e)
    {
        string file = Convert.ToString(ViewState["FilenName"]);
        string ConncetionString = GetDatabaseName();
        //string[] arr = file.Split(new string[] { ConncetionString }, StringSplitOptions.None);
        //string fn = System.Configuration.ConfigurationManager.AppSettings["URL"].ToString();
        //string newpath = fn + arr[1];

        //if (!string.IsNullOrEmpty(Convert.ToString(ViewState["Notes"])))
        //{
        //    string content = Convert.ToString(ViewState["Notes"]);
        //    string wordDocumentContent = $"<html><body>{content}</body></html>";
        //    byte[] byteArray = Encoding.UTF8.GetBytes(wordDocumentContent);
        //    Response.Clear();
        //    Response.ContentType = "application/msword";
        //    Response.AddHeader("Content-Disposition", "attachment; filename=TicketDetails.doc");
        //    Response.OutputStream.Write(byteArray, 0, byteArray.Length);
        //    Response.Flush();
        //    Response.End();
        //}
        try
        {
            string filePath = Convert.ToString(ViewState["FilenName"]);
            string nfile = filePath.Replace("'", "").Trim();
            filePath = nfile;
            if (string.IsNullOrEmpty(filePath))
            {
                throw new ArgumentException("File path is empty or invalid");
            }
            string fileName = Path.GetFileName(filePath);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found", filePath);
            }
            string contentType = GetContentType(Path.GetExtension(fileName));
            Response.Clear();
            Response.ContentType = contentType;
            Response.AddHeader("Content-Disposition", "attachment; filename=\"" + fileName + "\"");
            Response.AddHeader("Content-Length", new FileInfo(filePath).Length.ToString());
            Response.AddHeader("Cache-Control", "no-cache, no-store, must-revalidate");
            Response.AddHeader("Pragma", "no-cache");
            Response.AddHeader("Expires", "0");
            Response.TransmitFile(filePath);
            Response.Flush();
            Response.End();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Download error: {ex.Message}");
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", 
                "alert('Unable to download file. Please try again later.');", true);
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
    protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlStage_SelectedIndexChanged(null, null);
        string sql = "select sd_stageFK from SD_Status where ID='" + ddlStatus.SelectedValue + "'";
        string Stage = Convert.ToString(database.GetScalarValue(sql));
        ddlStage.SelectedValue = Stage;
    }

    protected void ImgBtn_Back_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("~/frmAllTickets.aspx");
    }


    private void FillChangeType()
    {
        try
        {
            DataTable FillDepartment = new SDTemplateFileds().FillChangeType();

            ddlChangeType.DataSource = FillDepartment;
            ddlChangeType.DataTextField = "ChangeTypeRef";
            ddlChangeType.DataValueField = "ChangeTypeRef";
            ddlChangeType.DataBind();
            ddlChangeType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----------Select ChangeType----------", "0"));


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
                Response.Redirect("~/Error/Error.html");

            }
        }
    }
    private void FillReasonForChange()
    {
        try
        {
            DataTable FillDepartment = new SDTemplateFileds().FillReasonForChange();

            ddlRFC.DataSource = FillDepartment;
            ddlRFC.DataTextField = "ReasonTypeRef";
            ddlRFC.DataValueField = "ReasonTypeRef";
            ddlRFC.DataBind();
            ddlRFC.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----------Select Reason----------", "0"));


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
                Response.Redirect("~/Error/Error.html");

            }
        }
    }
    protected void ADDChangeImpactDetails(string TicketRef)
    {
        //try

        //{
        DataTable dt = (DataTable)ViewState["Impact"];
        if (dt.Rows.Count > 0)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                con.Open();
                foreach (DataRow r in dt.Rows)
                {
                    using (SqlCommand cmd = new SqlCommand("SD_spAddChangeMngmtTickDetails", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TicketRef", TicketRef);
                        cmd.Parameters.AddWithValue("@ImpactDescription", r["ImpactDetails"]);
                        cmd.Parameters.AddWithValue("@Option", "AddImpactDetails");

                        int res = cmd.ExecuteNonQuery();



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
        //			inEr.InsertErrorLogsF(Session["UserName"].ToString()
        //, " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
        //			Response.Redirect("~/Error/Error.html");

        //		}
        //	}
    }
    protected void ADDChangeRollOutDetails(string TicketRef)
    {
        //try

        //{
        DataTable dt = (DataTable)ViewState["RollOut"];
        if (dt.Rows.Count > 0)
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                con.Open();
                foreach (DataRow r in dt.Rows)
                {
                    using (SqlCommand cmd = new SqlCommand("SD_spAddChangeMngmtTickDetails", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@TicketRef", TicketRef);
                        cmd.Parameters.AddWithValue("@RolloutDescription", r["RollOutDetails"]);
                        cmd.Parameters.AddWithValue("@Option", "AddRollOutDetails");

                        int res = cmd.ExecuteNonQuery();



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
        //			inEr.InsertErrorLogsF(Session["UserName"].ToString()
        //, " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
        //			Response.Redirect("~/Error/Error.html");

        //		}
        //	}
    }
    protected void ADDTaskForEngineer(string TicketRef)
    {
        try

        {
            DataTable dt = (DataTable)ViewState["Task"];

            if (dt.Rows.Count > 0)
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    con.Open();
                    foreach (DataRow r in dt.Rows)
                    {
                        string[] arr = r["EngineerAssociation"].ToString().Split(',');
                        for (int i = 0; i < arr.Length; i++)
                        {
                            using (SqlCommand cmd = new SqlCommand("SD_spAddChangeMngmtTickDetails", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@TicketRef", TicketRef);
                                cmd.Parameters.AddWithValue("@TaskDesc", r["TaskDescription"]);
                                cmd.Parameters.AddWithValue("@Status", r["Status"]);
                                cmd.Parameters.AddWithValue("@Assignee", arr[i]);
                                cmd.Parameters.AddWithValue("@Option", "AddTaskForTech");

                                int res = cmd.ExecuteNonQuery();



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
                Response.Redirect("~/Error/Error.html");

            }
        }
    }
    protected void MakeChangeTicket(string TicketRef)
    {
        //try

        //{
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {

            using (SqlCommand cmd = new SqlCommand("SD_spAddChangeMngmtTickDetails", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TicketRef", TicketRef);
                cmd.Parameters.AddWithValue("@ChangeType", ddlChangeType.SelectedValue);
                cmd.Parameters.AddWithValue("@RequestForChange", ddlRequestType.SelectedValue);
                cmd.Parameters.AddWithValue("@DurationFrom", Convert.ToDateTime(txtChangeDurationfrom.Text).ToString("yyyy-MM-dd hh:mm:ss"));
                cmd.Parameters.AddWithValue("@DurationTo", Convert.ToDateTime(txtChangeDurationfrom.Text).ToString("yyyy-MM-dd hh:mm:ss"));
                cmd.Parameters.AddWithValue("@DownTimeDesc", txtDownTimeName.Text);
                cmd.Parameters.AddWithValue("@DownTimeFrom", Convert.ToDateTime(txtDownTimeStart.Text).ToString("yyyy-MM-dd hh:mm:ss"));
                cmd.Parameters.AddWithValue("@DownTimeTo", Convert.ToDateTime(txtDownTimeTo.Text).ToString("yyyy-MM-dd hh:mm:ss"));
                cmd.Parameters.AddWithValue("@UpdateBy", Session["UserName"].ToString());
                cmd.Parameters.AddWithValue("@Option", "UpdateChangeTick");
                con.Open();
                int res = cmd.ExecuteNonQuery();



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
        //			inEr.InsertErrorLogsF(Session["UserName"].ToString()
        //, " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
        //			Response.Redirect("~/Error/Error.html");

        //		}
        //	}
    }
    protected void ShowTicketAttachment()
    {


        try
        {

            DataTable SD_TicketAttach = new FillSDFields().ShowTicketAttachment(Request.QueryString["TicketId"].ToString(), Request.QueryString["NamelyId"].ToString()); ;

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
}
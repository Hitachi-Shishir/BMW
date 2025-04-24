using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Interop;

public partial class frmTicketMerge : System.Web.UI.Page
{
    InsertErrorLogs inEr = new InsertErrorLogs();
    DataTable oddNumberCstmFlds;
    DataTable EvenNumberCstmFlds;
    DataTable oddNumberDdlCstmFlds;
    DataTable EvenNumberDdlCstmFlds;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["UserID"] == null)
            {
                Response.Redirect("/Default.aspx");
            }
            
            if (Request.QueryString["Desk"] != null)
            {
                if (!IsPostBack)
                {
                    divCategory2.Attributes.Add("style", "display: none;");
                    divCategory3.Attributes.Add("style", "display: none;");
                    divCategory4.Attributes.Add("style", "display: none;");
                    divCategory5.Attributes.Add("style", "display: none;");
                    btnSubmit.Visible = true;
                    btnCancel.Visible = true;
                    string[] arr = Request.QueryString["Tickets"].ToString().Replace("%2c", "").Split(',');
                    //FillAssignee();

                    lblTicketUpdateNo.Text = Request.QueryString["Tickets"].ToString().Replace("%2c", "");
                    if (Request.QueryString["ActionType"].ToString() == "Merge")
                    {
                        pnlIncident.Visible = false;
                        pnlMerge.Visible = true;
                        lblHeader.Text = "Merge Ticket";
                        ChoosePrimaryticket();
                        FillMergeTicketSummary();
                    }
                }
            }
            Session["ErrorUserName"] = "MergePage";
            string sql = "select theme,ThemeModify,OrgName from SD_User_Master a " +
                "INNER JOIN SD_OrgMaster o ON a.Org_ID=o.Org_ID where UserID='" + Convert.ToString(Session["UserID"]) + "'  and o.Org_ID='" + Convert.ToString(Session["SD_OrgID"]) + "'";
            DataTable dt = database.GetDataTable(sql);
            string theme = Convert.ToString(dt.Rows[0]["theme"]);
            if (theme != null)
            {
                string script = $"document.documentElement.setAttribute('data-bs-theme', '{theme}');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SetTheme", script, true);
            }
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
                inEr.InsertErrorLogsF(Session["ErrorUserName"].ToString()
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
           Categoryref FROM [dbo].fnGetCategoryFullPathForDesk('" + Request.QueryString["Desk"] + "', '" + Request.QueryString["NamelyId"] + "',1) where Level=1 order by Categoryref asc", con))
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
                                ddlCategory1.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----------Select Category----------", "0"));
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
                inEr.InsertErrorLogsF(Session["ErrorUserName"].ToString()
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
                inEr.InsertErrorLogsF(Session["ErrorUserName"].ToString()
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
                inEr.InsertErrorLogsF(Session["ErrorUserName"].ToString()
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
                ddlCategory2.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----------Select Category Level 2----------", "0"));

                divCategory2.Attributes.Add("style", "display: flex;");
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
                inEr.InsertErrorLogsF(Session["ErrorUserName"].ToString()
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
                ddlCategory3.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----------Select Category Level 3----------", "0"));

                divCategory3.Attributes.Add("style", "display: flex;");
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
                inEr.InsertErrorLogsF(Session["ErrorUserName"].ToString()
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
                ddlCategory4.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----------Select Category Level 4----------", "0"));


                divCategory2.Attributes.Add("style", "display: flex;");
                divCategory3.Attributes.Add("style", "display: flex;");
                divCategory4.Attributes.Add("style", "display: flex;");
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
                inEr.InsertErrorLogsF(Session["ErrorUserName"].ToString()
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
                ddlCategory5.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----------Select Category Level 5----------", "0"));

                divCategory2.Attributes.Add("style", "display: flex;");
                divCategory3.Attributes.Add("style", "display: flex;");
                divCategory4.Attributes.Add("style", "display: flex;");
                divCategory5.Attributes.Add("style", "display: flex;");



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
                inEr.InsertErrorLogsF(Session["ErrorUserName"].ToString()
    , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }

        }
    }
    protected void ddlCategory2_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCategory3();

    }
    protected void ddlCategory3_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillCategory4();

    }
    protected void ddlCategory4_SelectedIndexChanged(object sender, EventArgs e)
    {

        FillCategory5();

    }
    protected void ddlCategory5_SelectedIndexChanged(object sender, EventArgs e)
    {
        hdnCategoryID.Value = ddlCategory5.SelectedValue.ToString();

        DataTable FillCategoryLevel4 = FillCategoryLevel(6);
        if (FillCategoryLevel4.Rows.Count > 0)
        {
            ddlCategory6.DataSource = FillCategoryLevel4;
            ddlCategory6.DataTextField = "CategoryCodeRef";
            ddlCategory6.DataValueField = "Categoryref";
            ddlCategory6.DataBind();
            ddlCategory6.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----------Select Category Level 5----------", "0"));
        }
        else
        {
            ddlCategory6.DataSource = null;
            ddlCategory6.DataBind();

            ddlCategory6.ClearSelection();
            ddlCategory6.Enabled = false;

        }
    }
    protected void FillBasic()
    {
        //try

        //{

        //	showChangeControl.Visible = false;
        pnlIncident.Visible = true;




        FillStage();
        FillCategory1();
        FillSeverity();
        FillPriority();
        FillResolutionDetails();


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
        //			inEr.InsertErrorLogsF(Session["ErrorUserName"].ToString()
        //, " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
        //			ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

        //		}
        //	}

    }
    private void FillStage()
    {

        try
        {

            DataTable SD_Status = new SDTemplateFileds().FillStage(Request.QueryString["Desk"].ToString(), Request.QueryString["NamelyId"].ToString());

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
                inEr.InsertErrorLogsF(Session["ErrorUserName"].ToString()
    , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }
    }
    private void FillResolutionDetails()
    {

        try
        {

            DataTable SD_Resolution = new FillSDFields().FillResolution(); ;

            if (SD_Resolution.Rows.Count > 0)
            {
                //  this.lb.Text = dataTable.Rows.Count.ToString();
                this.ddlResoultion.DataSource = (object)SD_Resolution;

                ddlResoultion.DataTextField = "ResolutionCodeRef";
                ddlResoultion.DataValueField = "id";
                ddlResoultion.DataBind();
                ddlResoultion.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----------Select Resolution----------", "0"));
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
                inEr.InsertErrorLogsF(Session["ErrorUserName"].ToString()
    , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }

        }
    }
    private void FillSeverity()
    {
        try
        {

            DataTable SD_Severity = new SDTemplateFileds().FillSeverity(Request.QueryString["Desk"].ToString().Replace("%20", " "), Convert.ToString(Request.QueryString["NamelyId"].ToString())); ;

            ddlSeverity.DataSource = SD_Severity;
            ddlSeverity.DataTextField = "Serveritycoderef";
            ddlSeverity.DataValueField = "id";
            ddlSeverity.DataBind();
            ddlSeverity.Items.Insert(0, new ListItem("----------Select Severity----------", "0"));


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
                inEr.InsertErrorLogsF(Session["ErrorUserName"].ToString()
    , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }
    }
    private void FillPriority()
    {

        try
        {

            DataTable SD_Priority = new SDTemplateFileds().FillPriority(Request.QueryString["Desk"].ToString().Replace("%20", " "), Convert.ToString(Request.QueryString["NamelyId"].ToString())); ;

            ddlPriority.DataSource = SD_Priority;
            ddlPriority.DataTextField = "PriorityCodeRef";
            ddlPriority.DataValueField = "id";
            ddlPriority.DataBind();
            ddlPriority.Items.Insert(0, new ListItem("----------Select Priority----------", "0"));


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
                inEr.InsertErrorLogsF(Session["ErrorUserName"].ToString()
    , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }
    }

    Random r = new Random();
    private void FillDepartment()
    {
        try
        {
            DataTable FillDepartment = new SDTemplateFileds().FillDepartment(Convert.ToInt64(Request.QueryString["NamelyId"].ToString()));

            ddlDepartment.DataSource = FillDepartment;
            ddlDepartment.DataTextField = "DepartmentName";
            ddlDepartment.DataValueField = "DepartmentCode";
            ddlDepartment.DataBind();
            ddlDepartment.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----------Select Department----------", "0"));


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
                inEr.InsertErrorLogsF(Session["ErrorUserName"].ToString()
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
            ddlLocation.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----------Select Location----------", "0"));


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
                inEr.InsertErrorLogsF(Session["ErrorUserName"].ToString()
    , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }
    }

    private void CheckForPrevStage(string ticketnumber)
    {

        //try
        //{
        string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("SD_spCheckTicketStatus", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Ticketref", ticketnumber);
                //	cmd.Parameters.AddWithValue("@sdStageFK", ddlStage.SelectedValue);
                cmd.Parameters.AddWithValue("@sdStageFK", ddlStage.SelectedValue);
                cmd.Parameters.AddWithValue("@Option", "Checkstatus");
                cmd.Parameters.Add("@ERROR", SqlDbType.Char, 100);
                cmd.Parameters["@ERROR"].Direction = ParameterDirection.Output;

                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                if (cmd.Parameters["@ERROR"].Value.ToString().Contains("change"))
                {
                    if (ddlStage.SelectedItem.ToString() == "Resolved" || ddlStage.SelectedItem.ToString() == "Closed")
                    {
                        if ((txtResolution.Value.ToString() == " " || txtResolution.Value.ToString() == null) || ddlResoultion.SelectedValue == "0")
                        {
                            rfvddlResoultion.Enabled = true;
                            rfvddlResoultion.Visible = true;

                            rfvtxtResolution.Enabled = true;
                            rfvtxtResolution.Visible = true;
                            rfvddlResoultion.InitialValue = "0";
                            lblMsg.Text = "Please select resolution and enter resolution description";
                        }
                        else
                        {
                            UpdateTicketDetails(ticketnumber);
                        }


                    }
                    else
                    {
                        rfvddlResoultion.Enabled = false;
                        rfvddlResoultion.Visible = false;
                        rfvtxtResolution.Enabled = false;
                        rfvtxtResolution.Visible = false;
                        UpdateTicketDetails(ticketnumber);
                    }
                }
                else
                {
                    lblMsg.Text = (string)cmd.Parameters["@ERROR"].Value;
                }
                con.Close();
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
        //		if (ex.ToString().Contains("ThreadAbortException"))
        //		{

        //		}
        //		else
        //		{
        //			var st = new StackTrace(ex, true);
        //			// Get the top stack frame
        //			var frame = st.GetFrame(0);
        //			// Get the line number from the stack frame
        //			var line = frame.GetFileLineNumber();
        //			inEr.InsertErrorLogsF(Session["ErrorUserName"].ToString()
        //, " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
        //			ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

        //		}

        //	}
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            string Tickets = null;
            string[] arr = Request.QueryString["Tickets"].ToString().Replace("%2c", "").Split(',');
            foreach (string TicketNo in arr)
            {
                UpdateTicketDetails(TicketNo);
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowSuccess", "alert('Changes has been made successfully on Tickets :" + Request.QueryString["Tickets"].ToString().Replace("%2c", ",") + "');window.location.href='" + Request.RawUrl + "';", true);

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
                inEr.InsertErrorLogsF(Session["ErrorUserName"].ToString()
        , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }

        }
    }

    public static string TicketRef;

    private void FillAssignee()
    {
        try
        {
            DataTable FillAssigne = new SDTemplateFileds().FillAssigne(Convert.ToInt64(Request.QueryString["NamelyId"].ToString()));

            ddlAssigne.DataSource = FillAssigne;
            ddlAssigne.DataTextField = "TechLoginName";
            ddlAssigne.DataValueField = "TechID";
            ddlAssigne.DataBind();
            ddlAssigne.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----------Select Assignee----------", "0"));


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
                inEr.InsertErrorLogsF(Session["ErrorUserName"].ToString()
    , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }

        }
    }
    static string AssigneEmail;
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
                inEr.InsertErrorLogsF(Session["ErrorUserName"].ToString()
    , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }

            return null;
        }
    }
    public static string ToEmail;
    protected void SendMailToAssignee(string ticketnum)
    {
        try
        {
            Session["AssigneUpdate"] = "True";
            ToEmail = GetAssigneEmail(Convert.ToInt64(ddlAssigne.SelectedValue));

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
                inEr.InsertErrorLogsF(Session["ErrorUserName"].ToString()
    , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }

        }
    }
    protected void UpdateTicketDetails(string ticketnumbers)
    {
        //try

        //{
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {

            using (SqlCommand cmd = new SqlCommand("SD_spSDMergeNupate", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Ticketref", ticketnumbers);
                if (ddlPriority.SelectedValue == "0")
                {
                    cmd.Parameters.AddWithValue("@sdPriorityFK", null);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@sdPriorityFK", ddlPriority.SelectedValue);
                }
                if (ddlSeverity.SelectedValue == "0")
                {
                    cmd.Parameters.AddWithValue("@sdSeverityFK", null);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@sdSeverityFK", ddlSeverity.SelectedValue);
                }



                //cmd.Parameters.AddWithValue("@closedDateTime","");

                //cmd.Parameters.AddWithValue("@TicketDesc", System.Web.HttpUtility.HtmlEncode(txtDescription.Text).ToString());

                cmd.Parameters.AddWithValue("@submitterType", "");
                if (ddlStage.SelectedValue == "0")
                {
                    cmd.Parameters.AddWithValue("@sdStageFK", null);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@sdStageFK", ddlStage.SelectedValue);
                }

                cmd.Parameters.AddWithValue("@organizationFK", Session["SD_OrgID"].ToString());
                cmd.Parameters.AddWithValue("@assigneeType", "");
                if (ddlAssigne.SelectedValue == "0")
                {
                    cmd.Parameters.AddWithValue("@assigneeParticipantFK", null);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@assigneeParticipantFK", ddlAssigne.SelectedValue);
                }

                //cmd.Parameters.AddWithValue("@assigneePoolFK","");
                //cmd.Parameters.AddWithValue("@assigneeParticipantFK","");
                //cmd.Parameters.AddWithValue("@actualCompletedDate",);
                //cmd.Parameters.AddWithValue("@expectedDueDate",);
                //cmd.Parameters.AddWithValue("@actualResolutionDate",);
                //cmd.Parameters.AddWithValue("@organizationFK", Session[""].ToString());
                //cmd.Parameters.AddWithValue("@orgStaffFK",);
                //cmd.Parameters.AddWithValue("@sdErrorMessage",);
                if (ddlLocation.SelectedValue == "0")
                {
                    cmd.Parameters.AddWithValue("@location", null);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@location", ddlLocation.SelectedValue);
                }
                if (ddlDepartment.SelectedValue == "0")
                {
                    cmd.Parameters.AddWithValue("@Department", null);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Department", ddlDepartment.SelectedValue);
                }


                //cmd.Parameters.AddWithValue("@previousStageFK",);
                if (string.IsNullOrEmpty(hdnCategoryID.Value))
                {
                    cmd.Parameters.AddWithValue("@sdCategoryRef", null);
                    cmd.Parameters.AddWithValue("@categoryFullText", null);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@sdCategoryRef", hdnCategoryID.Value);
                    cmd.Parameters.AddWithValue("@categoryFullText", hdnCategoryID.Value.Replace("||", " - "));
                }

                //cmd.Parameters.AddWithValue("@InsertBy" Session["ErrorUserName"]);
                //cmd.Parameters.AddWithValue("@IsActive", '1');

                cmd.Parameters.AddWithValue("@Option", "UpdateTicketMerge");
                con.Open();
                int res = cmd.ExecuteNonQuery();

                ///custom fields are created by default by stored procedure	



                Session["Popup"] = "Insert";
                //string notes = this.PopulateBody(
                //ticketnumber, txtSubmitterName.Text, txtLoginName.Text, txtSummary.Text, ddlCategory1.SelectedItem.ToString());

                //CloudTicketDetails();

                if (res > 0)
                {



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
        //			inEr.InsertErrorLogsF(Session["ErrorUserName"].ToString()
        //, " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
        //			ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

        //		}
        //	}
    }
    private DataTable GetDataForTicket(string ticket)
    {
        //	try
        //	{



        long CategoryFk;
        string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("select top 1 * from vSDTicket where TicketNumber=@TicketNumber order by TicketNumber asc"))
            {
                cmd.CommandType = CommandType.Text;
                //   cmd.Parameters.AddWithValue("@TicketNumber", ddlOpenticket.SelectedValue);
                cmd.Parameters.AddWithValue("@TicketNumber", ticket);
                //cmd.Parameters.AddWithValue("@TicketNumber", "IN007195");
                cmd.Connection = con;
                con.Open();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
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
        //			ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

        //		}
        //		//			
        //	}

    }
    DataTable allCategories;
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.AbsoluteUri);
    }
    /////////////////////////////////////////////Merger Portion Start////////////////////////////////////////////////////////////

    protected void ChoosePrimaryticket()
    {
        string[] primTick = Request.QueryString["Tickets"].ToString().Replace("%2c", "").Split(',');
        rdbtnTicketList.DataSource = primTick;
        rdbtnTicketList.DataBind();
    }
    private void FillMergeTicketSummary()
    {
        try
        {
            DataTable FillAssigne = new SDTemplateFileds().FillMergeTicketSummary(Request.QueryString["Tickets"].ToString().Replace("%2c", ""), Convert.ToInt64(Request.QueryString["NamelyId"].ToString()));

            rdbtnTicketSummary.DataSource = FillAssigne;
            rdbtnTicketSummary.DataTextField = "TicketSummary";
            rdbtnTicketSummary.DataValueField = "TicketSummary";
            rdbtnTicketSummary.DataBind();
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
                inEr.InsertErrorLogsF(Session["ErrorUserName"].ToString()
    , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }

        }
    }

    protected void btnMerge_Click(object sender, EventArgs e)
    {


        string PrimaryTicket = rdbtnTicketList.SelectedValue.ToString();
        /////////////////////  Udpate Primary Ticket////////////////////
        ///

        MergeNUpdatePrimaryTicket(PrimaryTicket, "UpdatePrimaryTicket", "");
        string[] arr = Request.QueryString["Tickets"].ToString().Replace("%2c", "").Split(',');
        foreach (string TicketNo in arr)
        {


            if (TicketNo != PrimaryTicket)
            {
                MergeNUpdatePrimaryTicket(TicketNo, "MergeTicket", PrimaryTicket);
            }


        }
        
        ScriptManager.RegisterStartupScript(this, GetType(), "showAlertAndRedirect", $@"
    setTimeout(function() {{
        success_noti('{"Merged Successfully !"}'); 
        setTimeout(function() {{
            if (window.opener) {{
                window.opener.location.href = 'frmAllTickets.aspx';
            }}
            window.close();
        }}, 3000); // Delay for the alert
    }}, 100); // Initial delay to ensure success_noti is called first
", true);
    }

    protected void MergeNUpdatePrimaryTicket(string secondaryTicket, string proc, string primaryticket)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand("SD_spSDMergeTicket", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Ticketref", secondaryTicket);
                    cmd.Parameters.AddWithValue("@PrimaryTicketref", primaryticket);
                    cmd.Parameters.AddWithValue("@AllTicketMerge", Request.QueryString["Tickets"].ToString().Replace("%2c", ""));
                    cmd.Parameters.AddWithValue("@TickNotes", System.Web.HttpUtility.HtmlEncode(txtMergeReason.Value));
                    cmd.Parameters.AddWithValue("@organizationFK", Request.QueryString["NamelyId"].ToString());
                    cmd.Parameters.AddWithValue("@UserName", Session["LoginName"].ToString());
                    cmd.Parameters.AddWithValue("@UserID", Session["UserID"].ToString());
                    cmd.Parameters.AddWithValue("@TicketSummary", rdbtnTicketSummary.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@MergeNotes", rdbtnMergeNotes.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@Option", proc);
                    con.Open();
                    int res = cmd.ExecuteNonQuery();

                    ///custom fields are created by default by stored procedure	



                    Session["Popup"] = "Insert";
                    //string notes = this.PopulateBody(
                    //ticketnumber, txtSubmitterName.Text, txtLoginName.Text, txtSummary.Text, ddlCategory1.SelectedItem.ToString());

                    //CloudTicketDetails();

                    if (res > 0)
                    {

                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
            $"if (window.location.pathname.endsWith('/frmAddIncident.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Merged Successfully !")}'); setTimeout(function() {{ window.location.reload(); }}, 3000); }}", true);

                    }

                }
            }
            pnlMerge.Enabled = false;
        }
        catch (Exception ex) {
            var st = new StackTrace(ex, true);
            // Get the top stack frame
            var frame = st.GetFrame(0);
            // Get the line number from the stack frame
            var line = frame.GetFileLineNumber();
            inEr.InsertErrorLogsF(Session["ErrorUserName"].ToString()
, " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
        }

    }
    protected void ImgbtnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/frmAllTickets.aspx");
    }

  
}
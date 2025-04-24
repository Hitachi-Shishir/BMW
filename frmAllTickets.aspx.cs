using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class frmAllTickets : System.Web.UI.Page
{
    InsertErrorLogs inEr = new InsertErrorLogs();
    public enum MessageType { success, error, info, warning };
    protected void ShowMessage(MessageType type, string Message)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "Showalert('" + type + "','" + Message + "');", true);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] != null & Session["LoginName"] != null && Session["UserScope"] != null && Session["EmpID"] != null && Session["SD_OrgID"] != null)
        {
            if (Session["UserScope"].ToString() == "SDUser")
            {
                pnlgridrow.Visible = false;
            }
            else if (Session["UserScope"].ToString() == "Technician" || Session["UserScope"].ToString() == "Admin")
            {
                btnDelteBulkTicket.Visible = false;
            }
            else
            {
                pnlgridrow.Visible = true;
            }

            if (!IsPostBack)
            {

                FillFilter();
                if (Convert.ToString(Session["UserScope"]).ToLower() == "master")
                {
                    string ss = Session["UserType"].ToString();
                    FillOrganization();
                    if (Session["FilterOrgID"] != null)
                    {
                        ddlOrg.Items.FindByValue(Session["FilterOrgID"].ToString()).Selected = true;
                        ddlOrg_SelectedIndexChanged(sender, e);
                    }
                }
                else
                {
                    FillOrganization();
                    ddlOrg.Items.FindByValue(Session["SD_OrgID"].ToString()).Selected = true;
                    ddlOrg_SelectedIndexChanged(sender, e);
                    ddlOrg.Enabled = false;
                }
                //if (Session["FilterType"] != null)
                //{
                //    CheckForPrevFilter();
                //}
            }
        }
        else
        {
            Response.Redirect("/Default.aspx");
        }
    }
    private void FillOrganization()
    {
        try
        {
            DataTable SD_Org = new FillSDFields().FillOrganization();
            ddlOrg.DataSource = SD_Org;
            ddlOrg.DataTextField = "OrgName";
            ddlOrg.DataValueField = "Org_ID";
            ddlOrg.DataBind();
            ddlOrg.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0"));
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

    private void FillRequestType(long OrgId)
    {

        try
        {
            DataTable RequestType = new SDTemplateFileds().FillRequestType(OrgId);
            ddlRequestType.DataSource = RequestType;
            ddlRequestType.DataTextField = "ReqTypeRef";
            ddlRequestType.DataValueField = "ReqTypeRef";
            ddlRequestType.DataBind();
            ddlRequestType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", "0"));


        }
        catch (ThreadAbortException e2)
        {
            Console.WriteLine("Exception message: {0}", e2.Message);
            Thread.ResetAbort();
        }
        catch (Exception ex)
        {
            // msg.ReportError(ex.Message);
        }
    }
    protected void ddlOrg_SelectedIndexChanged(object sender, EventArgs e)
    {

        Session["FilterOrgID"] = ddlOrg.SelectedValue;
        FillRequestType(Convert.ToInt64(ddlOrg.SelectedValue));
        if (Session["SDRef"] != null)
        {
            if (ddlRequestType.Items.FindByText(Convert.ToString(Session["SDRef"])) != null)
            {
                ddlRequestType.SelectedValue = Convert.ToString(Session["SDRef"]);
            }
        }
        else
        {
            if (ddlRequestType.Items.FindByText("Incident") != null)
            {
                ddlRequestType.SelectedValue = "Incident";
            }
        }
        ddlRequestType_SelectedIndexChanged(sender, e);
    }
    protected void ddlRequestType_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["SDRef"] = ddlRequestType.SelectedValue.ToString();
        //	FillAllAssets();
        ViewState["SortExpression"] = "creationDate";
        ViewState["SortDirection"] = "DESC";
        //this.GetTicket(1, int.Parse
        //	(ddlPageSize.SelectedValue), "creationDate", true);
        if (Session["RowFilter"] != null)
        {
            pnlRowFilter.Visible = true;
        }
        else
        {
            pnlRowFilter.Visible = false;
        }
        CheckForPrevFilter();
    }
    private void FillAllChecklist()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT distinct name FROM sys.columns WHERE object_id = OBJECT_ID('dbo.vSDTicket') and name not in('id','partitionid')  ", con))
                {
                    cmd.CommandType = CommandType.Text;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {

                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                chkcolumn.DataSource = dt;
                                chkcolumn.DataTextField = "name";
                                chkcolumn.DataValueField = "name";
                                chkcolumn.DataBind();
                            }
                            else
                            {
                                chkcolumn.DataSource = null;
                                chkcolumn.DataBind();
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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }
    }
    protected void chkcolumn_SelectedIndexChanged(object sender, EventArgs e)
    {



    }

    Random r = new Random();
    protected void btnDelteBulkTicket_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (GridViewRow row in gvAllTickets.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    System.Web.UI.WebControls.CheckBox chkRow = (row.FindControl("chkRow") as System.Web.UI.WebControls.CheckBox);
                    if (chkRow.Checked)
                    {
                        string TicketNumber = row.Cells[2].Text;
                        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("SD_spSDIncident", con))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@Ticketref", TicketNumber);

                                cmd.Parameters.AddWithValue("@Option", "DeleteTicket");
                                con.Open();
                                int res = cmd.ExecuteNonQuery();
                                if (res > 0)
                                {
                                    ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
        $"if (window.location.pathname.endsWith('/frmAllTickets.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Deleted Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 1000); }}", true);
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
            $"if (window.location.pathname.endsWith('/frmAllTickets.aspx')) {{ error_noti(); setTimeout(function() {{ window.location.reload(); }}, 1000); }}", true);

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
    }

    protected void btnPickupTicket_Click(object sender, EventArgs e)
    {
        try
        {
            string sql2 = "select top 1 OrgName from SD_OrgMaster where Org_ID='" + Session["SD_OrgID"].ToString() + "'";
            string OrgName = Convert.ToString(database.GetScalarValue(sql2));
            foreach (GridViewRow gvrow in gvAllTickets.Rows)
            {
                System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("chkRow");
                if (chk != null & chk.Checked)
                {
                    System.Web.UI.WebControls.LinkButton label = (gvrow.Cells[2].FindControl("lblTicketNumber") as System.Web.UI.WebControls.LinkButton);
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("SD_spSDIncident", con))
                        {
                            cmd.CommandTimeout = 3600;
                            cmd.Connection = con;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@TicketNumber", label.Text);
                            cmd.Parameters.AddWithValue("@AssigneName", Session["LoginName"].ToString());
                            cmd.Parameters.AddWithValue("@organizationFK", Session["SD_OrgID"].ToString());
                            cmd.Parameters.AddWithValue("@UserID", Session["UserID"].ToString());
                            cmd.Parameters.AddWithValue("@Option", "AssignTechnician");
                            cmd.Parameters.AddWithValue("@OrgName", OrgName);
                            int res = cmd.ExecuteNonQuery();
                            con.Close();
                            if (res > 0)
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
        $"if (window.location.pathname.endsWith('/frmAllTickets.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Ticket has been Assigned !")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
           $"if (window.location.pathname.endsWith('/frmAllTickets.aspx')) {{ error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                            }
                        }
                    }

                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
          $"if (window.location.pathname.endsWith('/frmAllTickets.aspx')) {{ error_noti(); setTimeout(function() {{ window.location.reload(); }}, 1000); }}", true);
        }
    }
    private void Modal()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append(@"<script type='text/javascript'>");
        sb.Append("$('#CategoryModal').modal('show');");
        sb.Append("$('body').removeClass('modal-open');");
        sb.Append("$('.modal-backdrop').remove();");
        sb.Append(@"</script>");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ModalScript", sb.ToString(), false);


    }
    protected void ShowPopup(object sender, EventArgs e)
    {
        string title = "Greetings";
        string body = "Welcome to ASPSnippets.com";
        ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);
    }
    protected void imgcolumnfilter_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        FillAllChecklist();
        this.Modal();
    }
    protected void gvAllTickets_PageIndexChanging(object sender, System.Web.UI.WebControls.GridViewPageEventArgs e)
    {

    }
    protected void btngetCheckcolumn_Click(object sender, EventArgs e)
    {

        for (int j = 0; j < chkcolumn.Items.Count; j++)
        {
            for (int i = 2; i < gvAllTickets.Columns.Count; i++)
            {
                string columnName = gvAllTickets.Columns[i].HeaderText;
                if (chkcolumn.Items[i].Selected == true)// getting selected value from CheckBox List  
                {
                    if (gvAllTickets.Columns[i] is BoundField)
                    {
                        BoundField boundField = (BoundField)gvAllTickets.Columns[i];
                        boundField.HeaderStyle.CssClass = "";
                        boundField.ItemStyle.CssClass = "";
                    }
                    else if (gvAllTickets.Columns[i] is TemplateField)
                    {
                        TemplateField templateField = (TemplateField)gvAllTickets.Columns[i];
                        templateField.HeaderTemplate = null;
                        templateField.ItemTemplate = null;
                    }

                    gvAllTickets.Columns[i].Visible = true;
                }
                else
                {
                    if (gvAllTickets.Columns[i] is BoundField)
                    {
                        BoundField boundField = (BoundField)gvAllTickets.Columns[i];
                        boundField.HeaderStyle.CssClass = "hidden";
                        boundField.ItemStyle.CssClass = "hidden";
                    }
                    else if (gvAllTickets.Columns[i] is TemplateField)
                    {
                        TemplateField templateField = (TemplateField)gvAllTickets.Columns[i];
                        templateField.HeaderTemplate = new DummyTemplate();
                        templateField.ItemTemplate = new DummyTemplate();
                    }

                    gvAllTickets.Columns[i].Visible = false;
                }
            }
        }

    }

    private void GetTicket(int pageindex, int pagesize, string SortExpression, bool IsSorting)
    {

        if (Session["UserScope"].ToString() == "Master" || Session["UserScope"].ToString() == "Admin")
        {
            FillRptTickCountsWithStatus("AllTicketStatus");// need to unhinde when need to show the count

            FillMasterTickets(pageindex, pagesize, "WithoutFilter", SortExpression, IsSorting, "SD_spGetTicketMaster");

        }
        if (Session["UserScope"].ToString() == "Technician")
        {
            FillRptTickCountsWithStatus("AllTicketStatusTech"); /// need to unhinde when need to show the count

            FillTechTickets(pageindex, pagesize, "WithoutFilter", SortExpression, IsSorting, "SD_spGetTicketTech");

        }
        if (Session["UserScope"].ToString() == "SDUser" && Session["EmpID"] != null)
        {
            FillUserWiseTickets(1, pagesize, "WithoutFilter", SortExpression, IsSorting);
        }
    }
    public static DataTable mydt;
    protected void FillUserWiseTickets(int pageindex, int pagesize, string Function, string SortExpression, bool IsSorting)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand("SD_spGetTicket", con))
            {
                string ss = Session["EmpID"].ToString();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PageIndex", pageindex);
                cmd.Parameters.AddWithValue("@PageSize", pagesize);
                cmd.Parameters.AddWithValue("@TicketDayWise", "0");
                cmd.Parameters.AddWithValue("@SubmitterID", Session["EmpID"].ToString());
                cmd.Parameters.AddWithValue("@Option", Function);
                cmd.Parameters.Add("@TotalRow", SqlDbType.Int, 4);
                cmd.Parameters["@TotalRow"].Direction = ParameterDirection.Output;
                con.Open();

                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        mydt = dt;
                        DataView dv = dt.DefaultView;
                        if (ViewState["SortExpression"] != null && ViewState["SortDirection"] != null)
                        {
                            if (IsSorting)
                            {
                                if (ViewState["SortExpression"].ToString() == SortExpression)
                                {
                                    if (ViewState["SortDirection"].ToString() == "DESC")
                                        ViewState["SortDirection"] = "ASC";
                                    else
                                        ViewState["SortDirection"] = "DESC";
                                }
                                else
                                {
                                    ViewState["SortExpression"] = SortExpression;
                                    ViewState["SortDirection"] = "DESC";
                                }
                            }
                        }
                        else
                        {
                            ViewState["SortExpression"] = SortExpression;
                            ViewState["SortDirection"] = "DESC";
                        }
                        dv.Sort = ViewState["SortExpression"] + " " + ViewState["SortDirection"];
                        if (dt.Rows.Count > 0)
                        {
                            gvAllTickets.DataSource = dt;
                            gvAllTickets.DataBind();
                        }
                        else
                        {
                            gvAllTickets.DataSource = null;
                            gvAllTickets.DataBind();
                        }
                        if (dt.Rows.Count > 0)
                            GridFormat(dt);
                    }
                }
                con.Close();
                int recordCount = Convert.ToInt32(cmd.Parameters["@TotalRow"].Value);
                //this.PopulatePager(recordCount, pageindex);
            }
        }
    }
    protected void FillMasterTickets(int pageindex, int pagesize, string Function, string SortExpression, bool IsSorting, string storproc)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand(storproc, con))
            {
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PageIndex", pageindex);
                cmd.Parameters.AddWithValue("@PageSize", pagesize);
                cmd.Parameters.AddWithValue("@TicketDayWise", "0");
                cmd.Parameters.AddWithValue("@Desk", Session["SDRef"].ToString());
                cmd.Parameters.AddWithValue("@OrgId", ddlOrg.SelectedValue);
                cmd.Parameters.AddWithValue("@TechLoginName", Session["LoginName"].ToString());
                cmd.Parameters.AddWithValue("@Option", Function);
                cmd.Parameters.Add("@TotalRow", SqlDbType.Int, 4);
                cmd.Parameters["@TotalRow"].Direction = ParameterDirection.Output;
                con.Open();

                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        mydt = dt;
                        DataView dv = dt.DefaultView;
                        if (ViewState["SortExpression"] != null && ViewState["SortDirection"] != null)
                        {

                        }
                        else
                        {
                            ViewState["SortExpression"] = SortExpression;
                            ViewState["SortDirection"] = "DESC";
                        }
                        dv.Sort = ViewState["SortExpression"] + " " + ViewState["SortDirection"];
                        if (dt.Rows.Count > 0)
                        {
                            gvAllTickets.DataSource = dt;
                            gvAllTickets.DataBind();
                        }
                        else
                        {
                            gvAllTickets.DataSource = null;
                            gvAllTickets.DataBind();
                        }
                        if (dt.Rows.Count > 0)
                            GridFormat(dt);
                    }
                }
                con.Close();
                int recordCount = Convert.ToInt32(cmd.Parameters["@TotalRow"].Value);
                // this.PopulatePager(recordCount, pageindex);



            }
        }
    }
    protected void FillTechTickets(int pageindex, int pagesize, string Function, string SortExpression, bool IsSorting, string storproc)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand(storproc, con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PageIndex", pageindex);
                cmd.Parameters.AddWithValue("@PageSize", pagesize);
                cmd.Parameters.AddWithValue("@TicketDayWise", "0");
                cmd.Parameters.AddWithValue("@Desk", Session["SDRef"].ToString());
                cmd.Parameters.AddWithValue("@EngLocation", Session["Location"].ToString());
                cmd.Parameters.AddWithValue("@OrgId", ddlOrg.SelectedValue);
                cmd.Parameters.AddWithValue("@TechLoginName", Session["LoginName"].ToString());
                cmd.Parameters.AddWithValue("@Option", Function);
                cmd.Parameters.Add("@TotalRow", SqlDbType.Int, 4);
                cmd.Parameters["@TotalRow"].Direction = ParameterDirection.Output;
                con.Open();
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        mydt = dt;
                        DataView dv = dt.DefaultView;
                        if (ViewState["SortExpression"] != null && ViewState["SortDirection"] != null)
                        {
                            //if (IsSorting)
                            //{
                            //	if (ViewState["SortExpression"].ToString() == SortExpression)
                            //	{
                            //		if (ViewState["SortDirection"].ToString() == "DESC")
                            //			ViewState["SortDirection"] = "ASC";
                            //		else
                            //			ViewState["SortDirection"] = "DESC";
                            //	}
                            //	else
                            //	{
                            //		ViewState["SortExpression"] = SortExpression;
                            //		ViewState["SortDirection"] = "DESC";
                            //	}
                            //}
                        }
                        else
                        {
                            ViewState["SortExpression"] = SortExpression;
                            ViewState["SortDirection"] = "DESC";
                        }
                        dv.Sort = ViewState["SortExpression"] + " " + ViewState["SortDirection"];
                        if (dt.Rows.Count > 0)
                        {
                            gvAllTickets.DataSource = dt;
                            gvAllTickets.DataBind();
                        }
                        else
                        {
                            gvAllTickets.DataSource = null;
                            gvAllTickets.DataBind();
                        }
                        if (dt.Rows.Count > 0)
                            GridFormat(dt);
                    }
                }
                con.Close();
                int recordCount = Convert.ToInt32(cmd.Parameters["@TotalRow"].Value);
                //this.PopulatePager(recordCount, pageindex);



            }
        }
    }
    protected void gvAllTickets_RowCommand(object sender, System.Web.UI.WebControls.GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "EditTicket")
            {
                foreach (GridViewRow row in gvAllTickets.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        System.Web.UI.WebControls.CheckBox chkRow = (row.FindControl("chkRow") as System.Web.UI.WebControls.CheckBox);
                        if (chkRow.Checked == true)
                        {
                            System.Web.UI.WebControls.LinkButton label = (row.Cells[2].FindControl("lblTicketNumber") as System.Web.UI.WebControls.LinkButton);

                            string TicketNumber = label.Text;
                            Response.Write("<script type='text/javascript'>");
                            Response.Write("window.open('/frmEditTicketbyAssigne.aspx?TicketId=" + TicketNumber + "&redirected=true&Desk=" + ddlRequestType.SelectedValue + "&NamelyId=" + ddlOrg.SelectedValue + "','_blank');");
                            Response.Write("</script>");
                            CheckForPrevFilter();
                        }
                        else if (chkRow.Checked == false)
                        {
                            //Session["Popup"] = "Error";
                            //ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"warning_noti();", true);
                            //                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
                            //$"if (window.location.pathname.endsWith('/frmAllTickets.aspx')) {{ error_noti(); setTimeout(function() {{ window.location.reload(); }}, 1000); }}", true);
                        }
                    }
                }
            }
            if (e.CommandName == "TicketClick")
            {
                // Retrieve the TicketNumber passed as CommandArgument
                string ticketNumber = e.CommandArgument.ToString();

                Response.Redirect("/frmEditTicketbyAssigne.aspx?TicketId=" + ticketNumber +
                  "&redirected=true&Desk=" + ddlRequestType.SelectedValue +
                  "&NamelyId=" + ddlOrg.SelectedValue);

            }
        }
        catch (ThreadAbortException e2)
        {
            Console.WriteLine("Exception message: {0}", e2.Message);
            Thread.ResetAbort();
        }
    }
    protected void gvAllTickets_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkRow = (CheckBox)e.Row.FindControl("chkRow");
                System.Web.UI.WebControls.Literal ltrisExped = (e.Row.FindControl("ltrisExped") as System.Web.UI.WebControls.Literal);
                HtmlGenericControl faFlag = (HtmlGenericControl)e.Row.FindControl("tickFlag");

                if (ltrisExped.Text == "1" || ltrisExped.Text == "True")
                {
                    faFlag.Visible = true;
                }
                else
                {
                    faFlag.Visible = false;
                }
                if (chkRow != null)
                {
                    chkRow.InputAttributes["class"] = "form-check-input";
                }
            }
            else if (e.Row.RowType == DataControlRowType.Header)
            {
                CheckBox chkAll = (CheckBox)e.Row.FindControl("chkAll");
                if (chkAll != null)
                {
                    chkAll.InputAttributes["class"] = "form-check-input";
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                if (Session["UserScope"].ToString() == "Master")
                {
                    e.Row.Cells[0].Visible = true;
                    e.Row.Cells[1].Visible = true;
                }
                if (Session["UserScope"].ToString() == "SDUser")
                {
                    gvAllTickets.HeaderRow.Cells[0].Visible = false;
                    gvAllTickets.HeaderRow.Cells[1].Visible = false;
                    e.Row.Cells[0].Visible = false;
                    e.Row.Cells[1].Visible = false;
                }
                if (Session["UserScope"].ToString() == "Technician")
                {
                    e.Row.Cells[0].Visible = true;
                    e.Row.Cells[1].Visible = true;

                }
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    System.Web.UI.WebControls.Label label = e.Row.FindControl("lblTicketNumberColor") as System.Web.UI.WebControls.Label;
                    HtmlGenericControl badgeContainer = e.Row.FindControl("badgeContainer") as HtmlGenericControl;

                    LinkButton lblTicketNumber = e.Row.FindControl("lblTicketNumber") as LinkButton;
                    if (label.Text.ToLower() == "green")
                    {
                        //    e.Row.Cells[2].ForeColor = System.Drawing.Color.Black;
                        //    e.Row.Cells[2].CssClass = "badage-sucess";

                        //e.Row.Cells[2].Style["font-size"] = "Smaller";
                        //e.Row.Cells[2].Style["display"] = "table-cell";
                        //e.Row.Cells[2].Style["text-align"] = "center";
                        //e.Row.Cells[2].Style["vertical-align"] = "middle";

                        badgeContainer.Attributes["class"] = "badage-sucess";

                    }
                    if (label.Text.ToLower() == "yellow")
                    {
                        e.Row.Cells[2].CssClass = "badage-yellow";
                    }
                    if (label.Text.ToLower() == "red")
                    {
                        badgeContainer.Attributes["class"] = "badage-red";
                        e.Row.Cells[2].ForeColor = System.Drawing.Color.Black;
                        // e.Row.Cells[2].CssClass = "badage-red";
                        //e.Row.Cells[2].Style["font-size"] = "Smaller";
                        //e.Row.Cells[2].Style["display"] = "table-cell";
                        //e.Row.Cells[2].Style["text-align"] = "center";
                        //e.Row.Cells[2].Style["vertical-align"] = "middle";
                    }
                    if (label.Text.ToLower() == "orange")
                    {
                        e.Row.Cells[2].ForeColor = System.Drawing.Color.Black;
                        e.Row.Cells[2].CssClass = "badage-info";
                        e.Row.Cells[2].Style["font-size"] = "Smaller";
                        e.Row.Cells[2].Style["display"] = "table-cell";
                        e.Row.Cells[2].Style["text-align"] = "center";
                        e.Row.Cells[2].Style["vertical-align"] = "middle";
                    }
                }
            }
        }
        catch (ThreadAbortException e2)
        {
            Console.WriteLine("Exception message: {0}", e2.Message);
            Thread.ResetAbort();
        }

    }
    protected void gvAllTickets_PageIndexChanging1(object sender, GridViewPageEventArgs e)
    {

    }

    protected void gvAllTickets_Sorting(object sender, GridViewSortEventArgs e)
    {

        //this.GetTicket(1, int.Parse
        //        (ddlPageSize.SelectedValue), e.SortExpression, true);
    }
    protected void gvAllTickets_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (ViewState["SortExpression"] != null && ViewState["SortDirection"] != null)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                foreach (TableCell tableCell in e.Row.Cells)
                {
                    if (tableCell.HasControls())
                    {
                        LinkButton sortLinkButton = null;
                        if (tableCell.Controls[0] is LinkButton)
                        {
                            sortLinkButton = (LinkButton)tableCell.Controls[0];
                        }
                        if (sortLinkButton != null && ViewState["SortExpression"].ToString() == sortLinkButton.CommandArgument)
                        {
                            System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
                            if (ViewState["SortDirection"].ToString() == "ASC")
                                img.ImageUrl = "~/Images/arrow_up.png";
                            else if (ViewState["SortDirection"].ToString() == "DESC")
                                img.ImageUrl = "~/Images/arrow_down.png";
                            tableCell.Controls.Add(new LiteralControl("&nbsp;"));
                            tableCell.Controls.Add(img);
                        }

                    }
                }
            }

        }
    }
    protected void gvAllTickets_RowEditing(object sender, GridViewEditEventArgs e)
    {


    }
    protected void GridFormat(DataTable dt)
    {
        try
        {
            gvAllTickets.UseAccessibleHeader = true;
            gvAllTickets.HeaderRow.TableSection = TableRowSection.TableHeader;

            if (gvAllTickets.TopPagerRow != null)
            {
                gvAllTickets.TopPagerRow.TableSection = TableRowSection.TableHeader;
            }
            if (gvAllTickets.BottomPagerRow != null)
            {
                gvAllTickets.BottomPagerRow.TableSection = TableRowSection.TableFooter;
            }
            if (dt.Rows.Count > 0)
                gvAllTickets.FooterRow.TableSection = TableRowSection.TableFooter;
        }
        catch
        {

        }
    }
    protected void imgRowFilter_Click(object sender, EventArgs e)
    {
        pnlRowFilter.Visible = true;
        Session["RowFilter"] = "yes";
        if (ddlOrg.SelectedValue != "0" && ddlRequestType.SelectedValue != "0")
        {
            CheckForPrevFilter();
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlActionForTickets.SelectedValue == "Merge")
            {
                string Tickets = null;
                foreach (GridViewRow gvrow in gvAllTickets.Rows)
                {
                    System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("chkRow");
                    if (chk != null & chk.Checked)
                    {
                        System.Web.UI.WebControls.LinkButton label = (gvrow.Cells[1].FindControl("lblTicketNumber") as System.Web.UI.WebControls.LinkButton);
                        Tickets += label.Text + ",";
                    }
                }
                string[] Ticketarr = null;
                if (string.IsNullOrEmpty(Tickets))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select tickets !!');", true);
                    CheckForPrevFilter();
                    return;
                }
                else
                {
                    Tickets = Tickets.TrimEnd(',');
                    Ticketarr = Tickets.Split(',');

                    if (Ticketarr.Length >= 2)
                    {
                        Response.Write("<script type='text/javascript'>");
                        Response.Write("window.open('/frmTicketMerge.aspx?Tickets=" + Tickets +
                            "&redirected=true&ActionType=Merge&Desk=" + ddlRequestType.SelectedItem.ToString() +
                            "&NamelyId=" + ddlOrg.SelectedValue + "','_blank');");
                        Response.Write("</script>");
                        //  ddlRequestType_SelectedIndexChanged(null, null);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select more than one ticket !!');", true);
                        CheckForPrevFilter();
                        return;
                    }
                }
            }
            else if (ddlActionForTickets.SelectedValue == "BulkUpdate")
            {
                string Tickets = null;
                foreach (GridViewRow gvrow in gvAllTickets.Rows)
                {
                    System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("chkRow");
                    if (chk != null & chk.Checked)
                    {
                        System.Web.UI.WebControls.LinkButton label = (gvrow.Cells[2].FindControl("lblTicketNumber") as System.Web.UI.WebControls.LinkButton);
                        Tickets += label.Text + ",";
                    }
                }
                string[] Ticketarr = null;
                if (string.IsNullOrEmpty(Tickets))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please select tickets !!');", true);
                    CheckForPrevFilter();
                    return;
                }
                else
                {
                    Tickets = Tickets.TrimEnd(',');
                    Ticketarr = Tickets.Split(',');


                    if (Ticketarr.Length >= 2)
                    {
                        Response.Write("<script type='text/javascript'>");
                        Response.Write("window.open('/frmBulkUpdate.aspx?Tickets=" + Tickets + "&redirected=true&ActionType=Merge&Desk=" + ddlRequestType.SelectedItem.ToString() + "','_blank');");
                        Response.Write("</script>");
                        ddlRequestType_SelectedIndexChanged(null, null);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select more than one ticket !!');", true);
                        CheckForPrevFilter();
                        return;
                    }
                }
            }
            CheckForPrevFilter();
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
   $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
        }
    }

    protected void StatusButton_Click(object sender, EventArgs e)
    {
        Button btn = (Button)sender;
     
        if (btn != null)
        {
            Session["Tab"] = btn.CommandArgument;
           // Session["Tab"] = status;
        }

    
        string proc = "TicketStatusDetail";
        if (Session["UserScope"].ToString() == "Technician")
        {
            proc = "TicketStatusDetailTech";
            FillRptTickCountsWithStatus("AllTicketStatusTech");
        }
        else
        {
            FillRptTickCountsWithStatus("AllTicketStatus");
        }

     
        if (string.IsNullOrEmpty(txtSearch.Text))
        {
            FillStatusWiseTicket(Convert.ToString(Session["Tab"]), proc);
        }
        else
        {
            FilteredData("creationDate", true, Convert.ToString(Session["Tab"]), "");
        }
    }
    public void FillStatusWiseTicket(string status, string proc)
    {
        try
        {
            string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SD_spDashboardCount1"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Desk", ddlRequestType.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@OrgId", ddlOrg.SelectedValue);
                    cmd.Parameters.AddWithValue("@Location", Session["Location"].ToString());
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@Option", proc);
                    cmd.Parameters.AddWithValue("@TechLoginName", Session["LoginName"].ToString());
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                gvAllTickets.DataSource = dt;
                                gvAllTickets.DataBind();
                            }
                            else
                            {
                                gvAllTickets.DataSource = null;
                                gvAllTickets.DataBind();
                            }
                            if (dt.Rows.Count > 0)
                                GridFormat(dt);
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

    private void FillRptTickCountsWithStatus(string Proc)
    {
        try
        {
            string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SD_spDashboardCount1"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Desk", ddlRequestType.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@TechLoginName", Session["LoginName"].ToString());
                    cmd.Parameters.AddWithValue("@Location", Session["Location"].ToString());
                    cmd.Parameters.AddWithValue("@OrgId", ddlOrg.SelectedValue);
                    cmd.Parameters.AddWithValue("@Option", Proc);
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                RepeaterButtons.DataSource = dt;
                                RepeaterButtons.DataBind();
                            }
                            else
                            {
                                RepeaterButtons.DataSource = null;
                                RepeaterButtons.DataBind();
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
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                inEr.InsertErrorLogsF(Session["UserName"].ToString()
    , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }

    }

    protected void btnMerge_Click(object sender, EventArgs e)
    {
        string Tickets = null;
        foreach (GridViewRow gvrow in gvAllTickets.Rows)
        {
            System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("chkRow");
            if (chk != null & chk.Checked)
            {
                System.Web.UI.WebControls.Label label = (gvrow.Cells[2].FindControl("lblTicketNumber") as System.Web.UI.WebControls.Label);
                Tickets += label.Text + ",";

            }
        }
        string[] Ticketarr = null;
        if (string.IsNullOrEmpty(Tickets))
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
    $"warning_noti('{HttpUtility.JavaScriptStringEncode("Please select tickets!!!!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

        }
        else
        {
            Tickets = Tickets.TrimEnd(',');
            Ticketarr = Tickets.Split(',');


            if (Ticketarr.Length >= 2)
            {
                Response.Write("<script type='text/javascript'>");
                Response.Write("window.open('/frmMergeNGroupUpdt.aspx?Tickets=" + Tickets + "&redirected=true&ActionType=Merge&Desk=" + ddlRequestType.SelectedItem.ToString() + "','_blank');");
                Response.Write("</script>");
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
    $"warning_noti('{HttpUtility.JavaScriptStringEncode("Please Select more than two tickets!!!!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }
    }
    private void FillFilter()
    {

        try
        {

            DataTable SD_Org = new FillSDFields().FillFilter();

            ddlSearchItems.DataSource = SD_Org;
            ddlSearchItems.DataTextField = "COLUMN_NAME";
            ddlSearchItems.DataValueField = "COLUMN_NAME";
            ddlSearchItems.DataBind();
            ddlSearchItems.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--- Filter Column---", "0"));


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
    protected void imgRemoveFilter_Click(object sender, EventArgs e)
    {
        ddlSearchItems.ClearSelection();
        txtSearch.Text = "";
        Session.Remove("FilterType");
        Session.Remove("Tab");
        Session.Remove("FilterValue");


        Response.Redirect(Request.Url.AbsoluteUri);
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {

        ViewState["FltrType"] = "SEARCH";
        FilteredData("creationDate", true, "Inventory", "");
        Session["FilterType"] = ddlSearchItems.SelectedValue;
        Session["FilterValue"] = txtSearch.Text;
    }
    private void FilteredData(string SortExpression, bool IsSorting, string proc, string status)
    {

        try
        {
            System.Threading.Thread.Sleep(2000);
            string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    string sql = "SD_FilterDataInc";
                    if (!string.IsNullOrEmpty(txtSearch.Text.Trim()))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@OrgId", ddlOrg.SelectedValue);
                        cmd.Parameters.AddWithValue("@ColumnName", ddlSearchItems.SelectedValue);
                        cmd.Parameters.AddWithValue("@DeskRef", ddlRequestType.SelectedValue);
                        cmd.Parameters.AddWithValue("@Status", status);
                        cmd.Parameters.AddWithValue("@SearchItem", txtSearch.Text.Trim());
                        cmd.Parameters.AddWithValue("@Option", proc);
                    }
                    cmd.CommandText = sql;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            mydt = dt;
                            DataView dv = dt.DefaultView;
                            if (ViewState["SortExpression"] != null && ViewState["SortDirection"] != null)
                            {
                                //System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
                                //if (IsSorting)
                                //{
                                //    if (ViewState["SortExpression"].ToString() == SortExpression)
                                //    {
                                //        if (ViewState["SortDirection"].ToString() == "DESC")
                                //        {
                                //            ViewState["SortDirection"] = "ASC";
                                //            img.ImageUrl = "~/Images/arrow_up.png";
                                //        }
                                //        else
                                //        {
                                //            ViewState["SortDirection"] = "DESC";
                                //            img.ImageUrl = "~/Images/arrow_down.png";
                                //        }
                                //    }
                                //    else
                                //    {
                                //        ViewState["SortExpression"] = SortExpression;
                                //        ViewState["SortDirection"] = "DESC";
                                //    }
                                //}
                            }
                            else
                            {
                                ViewState["SortExpression"] = SortExpression;
                                ViewState["SortDirection"] = "DESC";
                            }
                            dv.Sort = ViewState["SortExpression"] + " " + ViewState["SortDirection"];
                            if (dt.Rows.Count > 0)
                            {
                                gvAllTickets.DataSource = dt;
                                gvAllTickets.DataBind();
                            }
                            else
                            {
                                gvAllTickets.DataSource = null;
                                gvAllTickets.DataBind();
                            }
                            if (dt.Rows.Count > 0)
                                GridFormat(dt);
                        }
                    }
                    con.Close();
                }
            }
        }

        catch (Exception ex)
        {

            lblerrorMsg.Text = ex.Message;
        }
    }

    protected void btnExpedite_Click(object sender, EventArgs e)
    {
        try
        {

            foreach (GridViewRow gvrow in gvAllTickets.Rows)
            {
                System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("chkRow");
                if (chk != null & chk.Checked)
                {
                    System.Web.UI.WebControls.LinkButton label = (gvrow.Cells[2].FindControl("lblTicketNumber") as System.Web.UI.WebControls.LinkButton);
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("SD_spSDExpediteTickets", con))
                        {
                            cmd.CommandTimeout = 3600;
                            cmd.Connection = con;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Ticketref", label.Text);
                            cmd.Parameters.AddWithValue("@DeskRef", ddlRequestType.SelectedValue);
                            cmd.Parameters.AddWithValue("@organizationFK", Session["SD_OrgID"].ToString());
                            cmd.Parameters.AddWithValue("@AssigneName", Session["LoginName"].ToString());
                            cmd.Parameters.AddWithValue("@Option", "UpdateTicketExpedite");

                            int res = cmd.ExecuteNonQuery();
                            con.Close();
                            if (res > 0)
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
        $"if (window.location.pathname.endsWith('/frmAllTickets.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Ticket has been Expedited !")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
           $"if (window.location.pathname.endsWith('/frmAllTickets.aspx')) {{ error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                            }
                        }
                    }

                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
          $"if (window.location.pathname.endsWith('/frmAllTickets.aspx')) {{ error_noti(); setTimeout(function() {{ window.location.reload(); }}, 1000); }}", true);
        }
    }

    public void CheckForPrevFilter()
    {
        if (Session["FilterType"] != null && Session["Tab"] == null)
        {
            ddlSearchItems.SelectedValue = Session["FilterType"].ToString();
            txtSearch.Text = Session["FilterValue"].ToString();
            this.GetTicket(1, 500, "creationDate", true);
            btnSearch_Click(null, null);
        }
        else if (Session["FilterType"] != null && Session["Tab"] != null)
        {
            ddlSearchItems.SelectedValue = Session["FilterType"].ToString();
            txtSearch.Text = Session["FilterValue"].ToString();
            // repeater ke function aur click call krwao
            StatusButton_Click(null, null);
        }
        else if (Session["FilterType"] == null && Session["Tab"] != null)
        {
            StatusButton_Click(null, null);

        }

        else
        {
            this.GetTicket(1, 500, "creationDate", true);
        }
    }
}
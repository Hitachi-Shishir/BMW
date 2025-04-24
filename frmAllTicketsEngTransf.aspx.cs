using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class frmAllTicketsEngTransf : System.Web.UI.Page
{
    InsertErrorLogs inEr = new InsertErrorLogs();

    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["UserID"] != null & Session["LoginName"] != null && Session["UserScope"] != null && Session["EmpID"] != null && Session["SD_OrgID"] != null)

        {

            if (Session["UserScope"].ToString() == "SDUser")
            {
                pnlgridrow.Visible = false;
                pnltickcount.Visible = false;

                //	this.GetTicket(1, int.Parse
                //(ddlPageSize.SelectedValue), "creationDate", true);

            }
            else if (Session["UserScope"].ToString() == "Technician" || Session["UserScope"].ToString() == "Admin")
            {
                btnDelteBulkTicket.Visible = false;
            }
            else
            {
                pnlgridrow.Visible = true;
            }
            //LoadDynamicButtons();
            if (!IsPostBack)
            {
                open = 0;
                wip = 0;
                assigned = 0;
                unassigned = 0;
                due = 0;
                duesoon = 0;
                themeupdate();
                if (Convert.ToString(Session["UserScope"]).ToLower() == "master")
                {
                    string ss = Session["UserType"].ToString();
                    FillOrganization();
                }
                else
                {
                    FillOrganization();
                    ddlOrg.Items.FindByValue(Session["SD_OrgID"].ToString()).Selected = true;
                    ddlOrg_SelectedIndexChanged(sender, e);
                    ddlOrg.Enabled = false;
                }

            }

        }
        else
        {
            Response.Redirect("/Default.aspx");
        }

    }
    #region Dynamic Buttons Start
    private void LoadDynamicButtons()
    {
        // Fetch the button data from the database
        DataTable dtButtons = GetTicketDataFromDatabase();

        foreach (DataRow row in dtButtons.Rows)
        {
            Button dynamicButton = new Button
            {
                ID = "btn" + row["TicketType"].ToString(),  // Generate a unique ID based on TicketType
                Text = row["ButtonText"].ToString() + " (" + row["TicketCount"].ToString() + ")", // Set button text and count
                ToolTip = row["Tooltip"].ToString(),
                CssClass = row["CssClass"].ToString(),
                CommandArgument = row["TicketType"].ToString(), // Store the ticket type for reference
                CausesValidation = false // Optionally disable validation on click
            };

            // Assign a common click event for all buttons
            dynamicButton.Click += new EventHandler(DynamicButton_Click);

            // Add the button to the panel
            pnltickcount.Controls.Add(dynamicButton);
        }
    }
    private DataTable GetTicketDataFromDatabase()
    {
        // Sample code to retrieve data from the database
        DataTable dt = new DataTable();
        dt.Columns.Add("TicketType", typeof(string));
        dt.Columns.Add("Tooltip", typeof(string));
        dt.Columns.Add("ButtonText", typeof(string));
        dt.Columns.Add("CssClass", typeof(string));
        dt.Columns.Add("TicketCount", typeof(int));

        // Dummy data for testing, replace with actual database query
        dt.Rows.Add("Open", "Tickets with Open status", "Open", "btn btn-sm btn-inverse-success", 10);
        dt.Rows.Add("WIP", "Tickets that are not resolved and open", "WIP", "btn btn-sm btn-inverse-warning", 5);
        dt.Rows.Add("Assigned", "Tickets assigned to me", "Assigned", "btn btn-sm btn-inverse-info", 3);
        dt.Rows.Add("Unassigned", "Unassigned tickets", "UnAssigned-Open", "btn btn-sm btn-inverse-primary", 8);
        dt.Rows.Add("DueSoon", "Tickets going to escalate", "Due Soon", "btn btn-sm btn-inverse-secondary", 2);
        dt.Rows.Add("Overdue", "Overdue tickets", "Overdue", "btn btn-sm btn-inverse-danger", 4);

        return dt;
    }
    protected void DynamicButton_Click(object sender, EventArgs e)
    {
        Button clickedButton = (Button)sender;
        string ticketType = clickedButton.CommandArgument;
        Response.Write("You clicked on " + ticketType + " tickets.");
        switch (ticketType)
        {
            case "Open":
                break;
            case "WIP":
                break;
            case "Assigned":
                break;
            case "Unassigned":
                break;
            case "DueSoon":
                break;
            case "Overdue":
                break;
        }
    }
    #endregion Dynamic Buttons End
    public void themeupdate()
    {
        try
        {
            string sql = "";
            if (Convert.ToString(Session["UserRole"]).ToUpper() == "MASTER")
            {
                sql = "select theme,ThemeModify from SD_User_Master a  where UserID='" + Convert.ToString(Session["UserID"]) + "'";
            }
            else
            {
                sql = "select theme,ThemeModify,OrgName from SD_User_Master a " +
                   "INNER JOIN SD_OrgMaster o ON a.Org_ID=o.Org_ID where UserID='" + Convert.ToString(Session["UserID"]) + "'  and o.Org_ID='" + Convert.ToString(Session["SD_OrgID"]) + "'";
            }
            DataTable dt = database.GetDataTable(sql);
            string theme = Convert.ToString(dt.Rows[0]["theme"]);
            if (theme != null)
            {
                string script = $"document.documentElement.setAttribute('data-bs-theme', '{theme}');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "SetTheme", script, true);
            }
        }
        catch { }
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
            ddlOrg.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--- Organization ---", "0"));


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
            ddlRequestType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--- RequestType---", "0"));


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
        try
        {
            open = 0;
            wip = 0;
            assigned = 0;
            unassigned = 0;
            due = 0;
            duesoon = 0;
            FillRequestType(Convert.ToInt64(ddlOrg.SelectedValue));
            ddlRequestType.Items.FindByText("Incident").Selected = true;
            ddlRequestType_SelectedIndexChanged(sender, e);
        }
        catch (Exception ex)
        {
            open = 0;
            wip = 0;
            assigned = 0;
            unassigned = 0;
            due = 0;
            duesoon = 0;
            FillRequestType(Convert.ToInt64(ddlOrg.SelectedValue));
            ddlRequestType_SelectedIndexChanged(sender, e);
        }
    }
    protected void ddlRequestType_SelectedIndexChanged(object sender, EventArgs e)
    {
        open = 0;
        wip = 0;
        assigned = 0;
        unassigned = 0;
        due = 0;
        duesoon = 0;
        Session["SDRef"] = ddlRequestType.SelectedValue.ToString();
        //	FillAllAssets();

        this.GetTicket(1, int.Parse
            (ddlPageSize.SelectedValue), "creationDate", true);

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
                                    Session["Popup"] = "Delete";
                                    Response.Redirect(Request.Url.AbsoluteUri);
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
        string sql2 = "select top 1 OrgName from SD_OrgMaster where Org_ID='" + Session["SD_OrgID"].ToString() + "'";
        string OrgName = Convert.ToString(database.GetScalarValue(sql2));
        if (string.IsNullOrEmpty(txtReason.Text))
        {
            lblMsg.Text = "Please provide reason for Assigning Ticket on your ID!!";
        }
        else
        {
            foreach (GridViewRow gvrow in gvAllTickets.Rows)
            {
                System.Web.UI.WebControls.CheckBox chk = (System.Web.UI.WebControls.CheckBox)gvrow.FindControl("chkRow");
                if (chk != null & chk.Checked)
                {
                    System.Web.UI.WebControls.Label label = (gvrow.Cells[2].FindControl("lblTicketNumber") as System.Web.UI.WebControls.Label);
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("SD_spSDIncident", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@TicketNumber", label.Text);
                            cmd.Parameters.AddWithValue("@AssigneName", Session["LoginName"].ToString());
                            cmd.Parameters.AddWithValue("@organizationFK", Session["SD_OrgID"].ToString());
                            cmd.Parameters.AddWithValue("@UserID", Session["UserID"].ToString());
                            cmd.Parameters.AddWithValue("@Note", txtReason.Text);
                            cmd.Parameters.AddWithValue("@Option", "ChangeTechnician");
                            cmd.Parameters.AddWithValue("@OrgName", OrgName);
                            con.Open();
                            int res = cmd.ExecuteNonQuery();
                            if (res > 0)
                            {
  string msg = "Ticket has been successfully transferred, Your Ticket Number is " + label.Text + ".";
   ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
      $"if (window.location.pathname.endsWith('/frmAllTicketsEngTransf.aspx')) {{ " +
      $"console.log('Current path:', window.location.pathname); " +
      $"success_noti('{HttpUtility.JavaScriptStringEncode(msg)}'); " +
      $"setTimeout(function() {{ console.log('Reloading page...'); window.location.href = window.location.href + '?t=' + new Date().getTime(); }}, 200); " +
      $"}}", true);

                            }
                        }
                    }
                }
            }

        }

        //}
        //catch (ThreadAbortException e2)
        //{
        //	Console.WriteLine("Exception message: {0}", e2.Message);
        //	Thread.ResetAbort();
        //}
    }
    private void Modal()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append(@"<script type='text/javascript'>");
        sb.Append("$('#CategoryModal').modal('show');");
        //  sb.Append("$('#basicModal').modal.appendTo('body').show('show')");
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
    protected void imgcolumnfilter_Click(object sender, EventArgs e)
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
            //	CheckBoxList chkColumns = (CheckBoxList)row.FindControl("chkcolumn");

            for (int i = 2; i < gvAllTickets.Columns.Count; i++)
            {
                string columnName = gvAllTickets.Columns[i].HeaderText;

                if (chkcolumn.Items[i].Selected == true)// getting selected value from CheckBox List  


                //		if (chkColumns.Items.Cast<ListItem>().Any(item => item.Text == columnName && item.Selected))
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


        //gvAllTickets.Columns[0].Visible = true;
        //gvAllTickets.Columns[1].Visible = true;
        //int total = chkcolumn.Items.Count + 2;
        //for (int i = 2; i < total; i++)
        //{
        //	if (chkcolumn.Items[i - 2].Selected == true && gvAllTickets.Columns[i].HeaderText == (chkcolumn.Items[i - 2].Value))
        //	{


        //		gvAllTickets.Columns[i].Visible = true;
        //		if (numbers.Contains(chkcolumn.Items[i - 2].Value))
        //		{

        //		}
        //		else
        //		{
        //			numbers.Add(chkcolumn.Items[i - 2].Value);
        //		}


        //	}
        //	else if (gvAllTickets.Columns[i].HeaderText == (chkcolumn.Items[i - 2].Value))
        //	{
        //		gvAllTickets.Columns[i].Visible = false;
        //		if (numbers.Contains(chkcolumn.Items[i - 2].Value))
        //		{
        //			numbers.Remove(chkcolumn.Items[i - 2].Value);
        //		}

        //	}

        //}

        //querystring = string.Join(",", numbers);
        //querystring = querystring.TrimEnd(',');
        //lblmsg1.Text = querystring;


        //	FillAllRequests(querystring);
    }
    protected void ddllFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        open = 0;
        wip = 0;
        assigned = 0;
        unassigned = 0;
        due = 0;
        duesoon = 0;
        this.GetTicket(1, int.Parse(ddlPageSize.SelectedValue), "creationDate", true);
    }
    private void GetTicket(int pageindex, int pagesize, string SortExpression, bool IsSorting)
    {

        if (Session["UserScope"].ToString() == "Master" || Session["UserScope"].ToString() == "Admin")
        {
            FillTicketStatus("AllTicketStatus");
            if (ddlGetticketFilter.SelectedValue == "0")
            {
                FillMasterTickets(pageindex, pagesize, "WithoutFilter", SortExpression, IsSorting, "SD_spGetTicketAllEngineer");
            }
            else
            {
                FillMasterTickets(pageindex, pagesize, "WithFilter", SortExpression, IsSorting, "SD_spGetTicketAllEngineer");

            }
        }
        if (Session["UserScope"].ToString() == "Technician")
        {
            FillTicketStatus("AllTicketStatusTech");
            if (ddlGetticketFilter.SelectedValue == "0")
            {
                FillTechTickets(pageindex, pagesize, "WithoutFilter", SortExpression, IsSorting, "SD_spGetTicketAllEngineer");
            }
            else
            {
                FillTechTickets(pageindex, pagesize, "WithFilter", SortExpression, IsSorting, "SD_spGetTicketAllEngineer");

            }
        }



    }
    public static DataTable mydt;

    protected void FillMasterTickets(int pageindex, int pagesize, string Function, string SortExpression, bool IsSorting, string storproc)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(storproc, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PageIndex", pageindex);
                    cmd.Parameters.AddWithValue("@PageSize", pagesize);
                    cmd.Parameters.AddWithValue("@TicketDayWise", ddlGetticketFilter.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@Desk", Convert.ToString(Session["SDRef"]));
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
                                //	lblTotal.Text = dt.Rows.Count.ToString();

                                gvAllTickets.DataSource = dt;
                                //gvAllTickets.Columns[4].ItemStyle.Width = 10;
                                //gvAllTickets.Columns[4].ItemStyle.Wrap = true;
                                //gvAllTickets.Attributes.Add("style", "word-break:break-all; word-wrap:break-word");
                                gvAllTickets.DataBind();
                            }
                            else
                            {
                                //	lblTotal.Text = "0";
                                gvAllTickets.DataSource = null;
                                gvAllTickets.DataBind();
                            }
                        }
                    }
                    //		idr.Close();
                    con.Close();
                    int recordCount = Convert.ToInt32(cmd.Parameters["@TotalRow"].Value);
                    this.PopulatePager(recordCount, pageindex);



                }
            }
        }
        catch (Exception ex)
        {

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
                cmd.Parameters.AddWithValue("@TicketDayWise", ddlGetticketFilter.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@Desk", Session["SDRef"].ToString());
                //cmd.Parameters.AddWithValue("@EngLocation", Session["Location"].ToString());
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
                            //	lblTotal.Text = dt.Rows.Count.ToString();

                            gvAllTickets.DataSource = dt;

                            gvAllTickets.DataBind();
                        }
                        else
                        {
                            //	lblTotal.Text = "0";
                            gvAllTickets.DataSource = null;
                            gvAllTickets.DataBind();
                        }
                    }
                }
                //		idr.Close();
                con.Close();
                int recordCount = Convert.ToInt32(cmd.Parameters["@TotalRow"].Value);
                this.PopulatePager(recordCount, pageindex);



            }
        }
    }

    protected void PopulatePager(int totalRow, int currentPage)
    {
        double totalPageCount = (double)((decimal)totalRow / decimal.Parse(ddlPageSize.SelectedValue));

        int pageCount = (int)Math.Ceiling(totalPageCount);
        List<ListItem> pages = new List<ListItem>();
        if (pageCount > 0)
        {
            int showMax = pageCount;
            int startPage;
            int endPage;
            if (pageCount <= showMax)
            {
                startPage = 1;
                endPage = pageCount;
            }
            else
            {
                startPage = currentPage;
                endPage = currentPage + showMax - 1;
            }
            pages.Add(new ListItem("<span style='font-size:11px;'>First</span>", "1", currentPage > 1));
            if (currentPage != 1)
            {
                pages.Add(new ListItem("<", (currentPage - 1).ToString()));
            }
            if (pageCount < 4)
            {
                for (int i = 1; i <= pageCount; i++)
                {
                    pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                }
            }
            else if (currentPage < 4)
            {
                for (int i = 1; i <= 4; i++)
                {
                    pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                }
                pages.Add(new ListItem("...", (currentPage).ToString(), false));
            }
            else if (currentPage > pageCount - 4)
            {
                pages.Add(new ListItem("...", (currentPage).ToString(), false));
                for (int i = currentPage - 1; i <= pageCount; i++)
                {
                    pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                }
            }
            else
            {
                pages.Add(new ListItem("...", (currentPage).ToString(), false));
                for (int i = currentPage - 2; i <= currentPage + 2; i++)
                {
                    pages.Add(new ListItem(i.ToString(), i.ToString(), i != currentPage));
                }
                pages.Add(new ListItem("...", (currentPage).ToString(), false));
            }
            if (currentPage != pageCount)
            {
                pages.Add(new ListItem(">", (currentPage + 1).ToString()));
            }
            pages.Add(new ListItem("<span style='font-size:11px;'>Last</span>", pageCount.ToString(), currentPage < pageCount));
        }
        rptpageindexing.DataSource = pages;
        rptpageindexing.DataBind();
    }
    protected void PageSize_Changed(object sender, EventArgs e)
    {
        if (open == 1 || wip == 1 || due == 1 || duesoon == 1 || assigned == 1 || unassigned == 1)
        {
            if (open == 1)
            {
                FillOpenTicket(1);
            }
            if (wip == 1)
            {
                FillWIPTicket(1);
            }
            if (due == 1)
            {
                FillDueTicket(1);
            }
            if (duesoon == 1)
            {
                FillDueSoonTicket(1);
            }
            if (assigned == 1)
            {
                FillAssignedTicket(1);
            }
            if (unassigned == 1)
            {
                FillUnAssignedTicket(1);
            }
        }
        else
        {
            this.GetTicket(1, int.Parse(ddlPageSize.SelectedValue), "creationDate", true);
        }
    }
    protected void Page_Changed(object sender, EventArgs e)
    {
        int pageIndex = int.Parse((sender as LinkButton).CommandArgument);
        if (open == 1 || wip == 1 || due == 1 || duesoon == 1 || assigned == 1 || unassigned == 1)
        {
            if (open == 1)
            {
                FillOpenTicket(pageIndex);
            }
            if (wip == 1)
            {
                FillWIPTicket(pageIndex);
            }
            if (due == 1)
            {
                FillDueTicket(pageIndex);
            }
            if (duesoon == 1)
            {
                FillDueSoonTicket(pageIndex);
            }
            if (assigned == 1)
            {
                FillAssignedTicket(pageIndex);
            }
            if (unassigned == 1)
            {
                FillUnAssignedTicket(pageIndex);
            }
        }
        else
        {
            this.GetTicket(pageIndex, int.Parse(ddlPageSize.SelectedValue), "creationDate", true);
        }
    }

    /// <summary>
    /// Gridview Functions list start
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
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
                        //	CheckBox chkRow = (row.Cells[0].FindControl("chkRow") as CheckBox);
                        if (chkRow.Checked == true)
                        {
                            //string ProductCode = row.Cells[11].Text;
                            System.Web.UI.WebControls.Label label = (row.Cells[2].FindControl("lblTicketNumber") as System.Web.UI.WebControls.Label);

                            string TicketNumber = label.Text;
                            Response.Write("<script type='text/javascript'>");
                            Response.Write("if (!window.open('/frmEditTicketbyAssigne.aspx?TicketId=" + TicketNumber + "&redirected=true&Desk=" + ddlRequestType.SelectedValue + "&NamelyId=" + ddlOrg.SelectedValue + "','_blank')) {");
                            Response.Write("    alert('Please allow popups for this website.');");
                            Response.Write("}");
                            Response.Write("</script>");


                            //string url = string.Format("/frmEditTicketbyAssigne.aspx?TicketId={0}", TicketNumber);

                            //// Open the URL in a new tab
                            //string script = string.Format("window.open('{0}','_blank');", url);
                            //ClientScript.RegisterStartupScript(this.GetType(), "openWindow", script, true);
                            //	Response.Redirect("/frmEditTicketbyAssigne.aspx?TicketId=" + TicketNumber);

                            //string strImageURL = "rptlabelgen.aspx?d=" + SysSerialNumber + "&h=75&w=150&bc=&fc=&t=Code 128&if=PNG";



                        }
                        else if (chkRow.Checked == false)
                        {
                            Session["Popup"] = "Error";
                            //	Response.Redirect(Request.Url.AbsoluteUri);
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
        //themeupdate();
    }
    protected void gvAllTickets_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkRow = (CheckBox)e.Row.FindControl("chkRow");
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

                    if (label.Text.ToLower() == "green")
                    {
                        //e.Row.Cells[2].BackColor = Color.Green;
                        e.Row.Cells[2].ForeColor = System.Drawing.Color.Black;
                        e.Row.Cells[2].CssClass = "badage-sucess";
                    }
                    if (label.Text.ToLower() == "yellow")
                    {
                        e.Row.Cells[2].CssClass = "badage-yellow";
                    }
                    if (label.Text.ToLower() == "red")
                    {
                        e.Row.Cells[2].ForeColor = System.Drawing.Color.Black;

                        e.Row.Cells[2].CssClass = "badage-red";
                    }
                    if (label.Text.ToLower() == "orange")
                    {
                        e.Row.Cells[2].ForeColor = System.Drawing.Color.Black;

                        e.Row.Cells[2].CssClass = "badage-info";
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
    protected void ddlGetticketFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        open = 0;
        wip = 0;
        assigned = 0;
        unassigned = 0;
        due = 0;
        duesoon = 0;
        this.GetTicket(1, int.Parse(ddlPageSize.SelectedValue), "creationDate", true);
    }
    protected void gvAllTickets_Sorting(object sender, GridViewSortEventArgs e)
    {

        this.GetTicket(1, int.Parse
                (ddlPageSize.SelectedValue), e.SortExpression, true);
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

    /// <summary>
    /// Gridview Functions list end
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgRowFilter_Click(object sender, EventArgs e)
    {
        pnlRowFilter.Visible = true;
    }
    protected void btnGridFilter_Click(object sender, EventArgs e)
    {
        // Get the filter values from the filter controls
        string TicketFilter = txtTicketNoFltr.Text.Trim();
        string TickSummaryFilter = txtTickSumFltr.Text;
        string TicketSeverity = txtSeverityFltr.Text;
        string ticketStatus = txtStatfltr.Text;
        string ticketPrioirty = txtPriorFltr.Text;
        // Get other filter values for additional columns if present

        // Get the original data source (e.g., from a database)


        // Create a filter expression based on the filter values
        string filterExpression = "";
        if (!string.IsNullOrEmpty(TicketFilter))
        {
            filterExpression += @"TicketNumber = '" + TicketFilter + "' AND ";
        }
        if (!string.IsNullOrEmpty(TickSummaryFilter))
        {
            filterExpression += @"Summary LIKE '%" + TickSummaryFilter + "%' AND ";
        }
        if (!string.IsNullOrEmpty(ticketPrioirty))
        {
            filterExpression += @"Priority LIKE '%" + ticketPrioirty + "%' AND ";
        }
        if (!string.IsNullOrEmpty(TicketSeverity))
        {
            filterExpression += @"Severity LIKE '%" + TicketSeverity + "%' AND ";
        }
        if (!string.IsNullOrEmpty(ticketStatus))
        {
            filterExpression += @"Status LIKE '%" + ticketStatus + "%' AND ";
        }
        // Add additional conditions for other columns if present

        // Remove the trailing "AND" from the filter expression
        if (filterExpression.EndsWith(" AND "))
        {
            filterExpression = filterExpression.Substring(0, filterExpression.Length - 5);
        }

        // Apply the filters to the data
        //	DataTable dt = (DataTable)gvAllTickets.DataSource;

        // Create a DataView from the DataTable
        DataView filteredData = mydt.DefaultView;

        //		DataView filteredData = originalData.DefaultView;
        filteredData.RowFilter = filterExpression;

        // Bind the filtered data to the GridView
        gvAllTickets.DataSource = filteredData;
        gvAllTickets.DataBind();

    }
    protected void imgRemoveFilter_Click(object sender, EventArgs e)
    {

        Response.Redirect(Request.Url.AbsoluteUri);
        txtTicketNoFltr.Text = string.Empty;
        txtPriorFltr.Text = string.Empty;
        txtSeverityFltr.Text = string.Empty;
        txtStatfltr.Text = string.Empty;
        txtTickSumFltr.Text = string.Empty;

    }
    private void FillTicketStatus(string Proc)
    {
        try
        {
            string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SD_spDashboardCount"))
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
                            if (dt.Rows.Count > 0 && dt != null)
                            {
                                if (Convert.ToString(dt.Rows[0]["Open"]) != "")
                                {
                                    btnOpenTicket.Text = "Open  ( " + dt.Rows[0]["Open"].ToString() + " )";
                                }
                                else
                                {
                                    btnOpenTicket.Text = "Open  (0)";
                                }
                                if (Convert.ToString(dt.Rows[0]["WIP"]) != "")
                                {
                                    btnWipTicket.Text = "WIP  ( " + dt.Rows[0]["WIP"].ToString() + " )";
                                }
                                else
                                {
                                    btnWipTicket.Text = "WIP  (0)";
                                }
                                if (Convert.ToString(dt.Rows[0]["Assigned"]) != "")
                                {
                                    btnTicketAssigntoME.Text = "Assigned  ( " + dt.Rows[0]["Assigned"].ToString() + " )";
                                }
                                else
                                {
                                    btnTicketAssigntoME.Text = "Assigned  (0)";
                                }
                                if (Convert.ToString(dt.Rows[0]["UnAssigned"]) != "")
                                {
                                    btnAssignToOther.Text = "UnAssigned  ( " + dt.Rows[0]["UnAssigned"].ToString() + " )";
                                }
                                else
                                {
                                    btnAssignToOther.Text = "UnAssigned  (0)";
                                }
                                if (Convert.ToString(dt.Rows[0]["DueSoon"]) != "")
                                {
                                    btnDueSoonTickets.Text = "DueSoon  ( " + dt.Rows[0]["DueSoon"].ToString() + " )";
                                }
                                else
                                {
                                    btnDueSoonTickets.Text = "DueSoon  (0)";
                                }
                                if (Convert.ToString(dt.Rows[0]["OverDue"]) != "")
                                {
                                    btnTicketEsclated.Text = "OverDue  ( " + dt.Rows[0]["OverDue"].ToString() + " )";
                                }
                                else
                                {
                                    btnTicketEsclated.Text = "OverDue  (0)";
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
    public static int open;
    public static int wip;
    public static int assigned;
    public static int unassigned;
    public static int duesoon;

    public static int due;
    protected void btnOpenTicket_Click(object sender, EventArgs e)
    {
        open = 1;

        wip = 0;
        assigned = 0;
        unassigned = 0;
        due = 0;
        duesoon = 0;
        FillOpenTicket(1);
    }

    protected void FillOpenTicket(int pageindex)
    {
        if (Session["UserScope"].ToString() == "Master" || Session["UserScope"].ToString() == "Admin")
        {
            FillTicketStatus("AllTicketStatus");
            if (ddlGetticketFilter.SelectedValue == "0")
            {
                FillMasterTickets(pageindex, int.Parse
            (ddlPageSize.SelectedValue), "Open", "creationDate", true, "SD_spGetTicketMasterstatusWise");
            }


        }
        if (Session["UserScope"].ToString() == "Technician")
        {
            FillTicketStatus("AllTicketStatusTech");
            if (ddlGetticketFilter.SelectedValue == "0")
            {
                FillTechTickets(pageindex, int.Parse
            (ddlPageSize.SelectedValue), "Open", "creationDate", true, "SD_spGetTicketMasterstatusWise");
            }

        }
    }

    protected void btnWipTicket_Click(object sender, EventArgs e)
    {
        wip = 1;
        open = 0;

        assigned = 0;
        unassigned = 0;
        due = 0;
        duesoon = 0;
        FillWIPTicket(1);
    }
    protected void FillWIPTicket(int pageindex)
    {
        if (Session["UserScope"].ToString() == "Master" || Session["UserScope"].ToString() == "Admin")
        {
            FillTicketStatus("AllTicketStatus");
            if (ddlGetticketFilter.SelectedValue == "0")
            {
                FillMasterTickets(pageindex, int.Parse
            (ddlPageSize.SelectedValue), "WIP", "creationDate", true, "SD_spGetTicketMasterstatusWise");
            }


        }
        if (Session["UserScope"].ToString() == "Technician")
        {
            FillTicketStatus("AllTicketStatusTech");
            if (ddlGetticketFilter.SelectedValue == "0")
            {
                FillTechTickets(pageindex, int.Parse
            (ddlPageSize.SelectedValue), "WIP", "creationDate", true, "SD_spGetTicketMasterstatusWise");
            }

        }


    }

    protected void btnTicketAssigntoME_Click(object sender, EventArgs e)
    {
        assigned = 1;
        open = 0;
        wip = 0;

        unassigned = 0;
        due = 0;
        duesoon = 0;
        FillAssignedTicket(1);
    }
    protected void FillAssignedTicket(int pageindex)
    {
        if (Session["UserScope"].ToString() == "Master" || Session["UserScope"].ToString() == "Admin")
        {
            FillTicketStatus("AllTicketStatus");
            if (ddlGetticketFilter.SelectedValue == "0")
            {
                FillMasterTickets(pageindex, int.Parse
            (ddlPageSize.SelectedValue), "Assigned", "creationDate", true, "SD_spGetTicketMasterstatusWise");
            }


        }
        if (Session["UserScope"].ToString() == "Technician")
        {
            FillTicketStatus("AllTicketStatusTech");
            if (ddlGetticketFilter.SelectedValue == "0")
            {
                FillTechTickets(pageindex, int.Parse
            (ddlPageSize.SelectedValue), "Assigned", "creationDate", true, "SD_spGetTicketMasterstatusWise");
            }

        }
    }
    protected void btnTicketEsclated_Click(object sender, EventArgs e)
    {
        due = 1;
        open = 0;
        wip = 0;
        assigned = 0;
        unassigned = 0;

        duesoon = 0;
        FillDueTicket(1);
    }
    protected void FillDueTicket(int pageindex)
    {
        if (Session["UserScope"].ToString() == "Master" || Session["UserScope"].ToString() == "Admin")
        {
            FillTicketStatus("AllTicketStatus");
            if (ddlGetticketFilter.SelectedValue == "0")
            {
                FillMasterTickets(pageindex, int.Parse
            (ddlPageSize.SelectedValue), "OverDue", "creationDate", true, "SD_spGetTicketMasterstatusWise");
            }


        }
        if (Session["UserScope"].ToString() == "Technician")
        {
            FillTicketStatus("AllTicketStatusTech");
            if (ddlGetticketFilter.SelectedValue == "0")
            {
                FillTechTickets(pageindex, int.Parse
            (ddlPageSize.SelectedValue), "OverDue", "creationDate", true, "SD_spGetTicketMasterstatusWise");
            }

        }

    }
    protected void btnDueSoonTickets_Click(object sender, EventArgs e)
    {
        duesoon = 1;
        open = 0;
        wip = 0;
        assigned = 0;
        unassigned = 0;
        due = 0;

        FillDueSoonTicket(1);
    }
    protected void FillDueSoonTicket(int pageindex)
    {
        if (Session["UserScope"].ToString() == "Master" || Session["UserScope"].ToString() == "Admin")
        {
            FillTicketStatus("AllTicketStatus");
            if (ddlGetticketFilter.SelectedValue == "0")
            {
                FillMasterTickets(pageindex, int.Parse
            (ddlPageSize.SelectedValue), "DueSoon", "creationDate", true, "SD_spGetTicketMasterstatusWise");
            }


        }
        if (Session["UserScope"].ToString() == "Technician")
        {
            FillTicketStatus("AllTicketStatusTech");
            if (ddlGetticketFilter.SelectedValue == "0")
            {
                FillTechTickets(pageindex, int.Parse
            (ddlPageSize.SelectedValue), "DueSoon", "creationDate", true, "SD_spGetTicketMasterstatusWise");
            }

        }

    }
    protected void btnAssignToOther_Click(object sender, EventArgs e)
    {
        unassigned = 1;
        open = 0;
        wip = 0;
        assigned = 0;

        due = 0;
        duesoon = 0;
        FillUnAssignedTicket(1);
    }
    protected void FillUnAssignedTicket(int pageindex)
    {

        if (Session["UserScope"].ToString() == "Master" || Session["UserScope"].ToString() == "Admin")
        {
            FillTicketStatus("AllTicketStatus");
            if (ddlGetticketFilter.SelectedValue == "0")
            {
                FillMasterTickets(pageindex, int.Parse
            (ddlPageSize.SelectedValue), "AssignedOther", "creationDate", true, "SD_spGetTicketMasterstatusWise");
            }


        }
        if (Session["UserScope"].ToString() == "Technician")
        {
            FillTicketStatus("AllTicketStatusTech");
            if (ddlGetticketFilter.SelectedValue == "0")
            {
                FillTechTickets(pageindex, int.Parse
            (ddlPageSize.SelectedValue), "AssignedOther", "creationDate", true, "SD_spGetTicketMasterstatusWise");
            }

        }
    }
}
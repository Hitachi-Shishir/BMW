using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HelpDesk_frmAddRequester : System.Web.UI.Page
{
    InsertErrorLogs inEr = new InsertErrorLogs();
    public enum MessageType { success, error, info, warning };
    protected void ShowMessage(MessageType type, string Message)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "Showalert('" + type + "','" + Message + "');", true);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

     
                if (!IsPostBack)
                {
                if (Session["UserScope"] != null)
                {
                    chkEnableTechnician.InputAttributes["class"] = "form-check-input";

                    //FillDepartment();
                    FillOrganization();
                    FillTechnicianDetails();
                    FillScopeDetails();
                    FillUserRole();
                    //	FillLocation();
                    btnViewUsers.CssClass = "btn btn-sm btn-secondary";
                    btnViewUsers.Enabled = false;
                    btnAddRequester.CssClass = "btn btn-sm btn-outline-secondary";
                    btnimportUser.CssClass = "btn btn-sm btn-outline-secondary";
                    pnlShowUsers.Visible = true;
                    if (Session["UserScope"].ToString() == "Admin")
                    {
                        pnlEnableTechnician.Visible = false;
                        chkEnableTechnician.Visible = false;
                    }
                    else
                    {
                        chkEnableTechnician.Visible = true;
                    }
                }
                else
                {
                    Response.Redirect("/Default.aspx");
                }
            }
          
            //btnimportUser.Enabled = false;
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
    private void FillOrganization()
    {

        try
        {

            DataTable SD_Org = new FillSDFields().FillOrganization(); ;

            ddlOrg.DataSource = SD_Org;
            ddlOrg.DataTextField = "OrgName";
            ddlOrg.DataValueField = "Org_ID";
            ddlOrg.DataBind();
            ddlOrg.Items.Insert(0, new System.Web.UI.WebControls.ListItem("---Select---", "0"));


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
    protected void ddlRequestType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Session["SDRef"] = ddlRequestType.SelectedValue.ToString();
            FillCategory1();
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
    private void FillScopeDetails()
    {

        try
        {

            DataTable SD_Scope = new FillSDFields().FillUserScopedetails(); ;

            if (SD_Scope.Rows.Count > 0)
            {
                //  this.lb.Text = dataTable.Rows.Count.ToString();
                this.ddlUserScope.DataSource = (object)SD_Scope;

                ddlUserScope.DataTextField = "ScopeName";
                ddlUserScope.DataValueField = "ScopeID";
                ddlUserScope.DataBind();
                ddlUserScope.Items.Insert(0, new System.Web.UI.WebControls.ListItem("---Select---", "0"));

            }
            else
            {
                this.ddlUserScope.DataSource = (object)null;
                this.ddlUserScope.DataBind();
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
    private void FillUserRole()
    {

        try
        {

            DataTable SD_Role = new FillSDFields().FillUserRole(); ;

            if (SD_Role.Rows.Count > 0)
            {
                //  this.lb.Text = dataTable.Rows.Count.ToString();
                this.ddlUserRole.DataSource = (object)SD_Role;

                ddlUserRole.DataTextField = "RoleName";
                ddlUserRole.DataValueField = "RoleName";
                ddlUserRole.DataBind();
                ddlUserRole.Items.Insert(0, new System.Web.UI.WebControls.ListItem("---Select---", "0"));

            }
            else
            {
                this.ddlUserRole.DataSource = (object)null;
                this.ddlUserRole.DataBind();
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
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        try
        {
            string connection = ConfigurationManager.ConnectionStrings["ADConnection"].ToString();
            DirectorySearcher dssearch = new DirectorySearcher(connection);
            dssearch.Filter = "(sAMAccountName=" + txtEmployeeID.Text + ")";
            SearchResult sresult = dssearch.FindOne();
            if (sresult != null)
            {
                DirectoryEntry dsresult = sresult.GetDirectoryEntry();

                if (sresult.Properties["displayName"] != null && sresult.Properties["displayName"].Count > 0)
                {
                    txtTechLoginName.Text = dsresult.Properties["displayName"][0].ToString();
                }
                if (sresult.Properties["givenName"] != null && sresult.Properties["givenName"].Count > 0)
                {
                    txtFirstName.Text = dsresult.Properties["givenName"][0].ToString();
                }
                if (sresult.Properties["sn"] != null && sresult.Properties["sn"].Count > 0)
                {
                    txtLastName.Text = dsresult.Properties["sn"][0].ToString();
                }


                if (sresult.Properties["mail"] != null && sresult.Properties["mail"].Count > 0)
                {
                    txtEmail.Text = dsresult.Properties["mail"][0].ToString();
                }
                if (sresult.Properties["telephoneNumber"] != null && sresult.Properties["telephoneNumber"].Count > 0)
                {
                    txtPhone.Text = dsresult.Properties["telephoneNumber"][0].ToString();
                }
                //if (sresult.Properties["samAccountName"] != null && sresult.Properties["samAccountName"].Count > 0)
                //{
                //	txtEmployeeID.Text = dsresult.Properties["samAccountName"][0].ToString();
                //}
                //if (sresult.Properties["title"] != null && sresult.Properties["title"].Count > 0)
                //{
                //    txtGrade.Text = dsresult.Properties["title"][0].ToString();
                //}

                //if (ddldepartment.Items.FindByValue(dsresult.Properties["department"][0].ToString().Trim()) != null && sresult.Properties["department"].Count > 0)
                //{
                //    lblerrorMsg.Text = dsresult.Properties["department"][0].ToString().Trim();

                //}

            }

            else
            {
                //lblMsg.Text = "Error: Not found";
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
    private void FillRequestType(long OrgId)
    {

        try
        {

            DataTable RequestType = new SDTemplateFileds().FillRequestType(OrgId);

            ddlRequestType.DataSource = RequestType;
            ddlRequestType.DataTextField = "ReqTypeRef";
            ddlRequestType.DataValueField = "ReqTypeRef";
            ddlRequestType.DataBind();
            ddlRequestType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("---Select---", "0"));

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
    private void FillCategory1()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"SELECT CategoryCodeRef,
           Categoryref FROM [dbo].fnGetCategoryFullPathForDesk('" + ddlRequestType.SelectedValue + "','" + ddlOrg.SelectedValue + "', 1) where Level=1 order by Categoryref asc", con))
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
                                ddlCategory1.Items.Insert(0, new System.Web.UI.WebControls.ListItem("---Select---", "0"));
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
    private void FillDepartment()
    {
        try
        {
            DataTable FillDepartment = new SDTemplateFileds().FillDepartment(Convert.ToInt64(ddlOrg.SelectedValue));

            ddlDepartment.DataSource = FillDepartment;
            ddlDepartment.DataTextField = "DepartmentName";
            ddlDepartment.DataValueField = "DepartmentCode";
            ddlDepartment.DataBind();
            ddlDepartment.Items.Insert(0, new System.Web.UI.WebControls.ListItem("---Select---", "0"));


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
            DataTable FillLocation = new SDTemplateFileds().FillLocation(Convert.ToInt64(ddlOrg.SelectedValue));

            ddlLocation.DataSource = FillLocation;
            ddlLocation.DataTextField = "LocName";
            ddlLocation.DataValueField = "LocCode";
            ddlLocation.DataBind();
            ddlLocation.Items.Insert(0, new System.Web.UI.WebControls.ListItem("---Select---", "0"));


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
    private void FillTechnicianDetails()
    {
        try
        {
            DataTable SD_Techninician = new FillSDFields().FillUserdetails();
            if (SD_Techninician.Rows.Count > 0)
            {
                //  this.lb.Text = dataTable.Rows.Count.ToString();
                this.gvTechnician.DataSource = (object)SD_Techninician;
                this.gvTechnician.DataBind();
            }
            else
            {
                this.gvTechnician.DataSource = (object)null;
                this.gvTechnician.DataBind();
            }
            GridFormat(SD_Techninician);
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
    protected void GridFormat(DataTable dt)
    {
        gvTechnician.UseAccessibleHeader = true;
        gvTechnician.HeaderRow.TableSection = TableRowSection.TableHeader;

        if (gvTechnician.TopPagerRow != null)
        {
            gvTechnician.TopPagerRow.TableSection = TableRowSection.TableHeader;
        }
        if (gvTechnician.BottomPagerRow != null)
        {
            gvTechnician.BottomPagerRow.TableSection = TableRowSection.TableFooter;
        }
        if (dt.Rows.Count > 0)
            gvTechnician.FooterRow.TableSection = TableRowSection.TableFooter;
    }
    private void Modal()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append(@"<script type='text/javascript'>");
        sb.Append("$('#basicModal').modal('show');");
        //  sb.Append("$('#basicModal').modal.appendTo('body').show('show')");
        sb.Append("$('body').removeClass('modal-open');");
        sb.Append("$('.modal-backdrop').remove();");
        sb.Append(@"</script>");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ModalScript", sb.ToString(), false);


    }
    Random r = new Random();
    public static int ID;
    protected void SaveData()
    {

        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand("SD_spAddRequester", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", r.Next());
                    cmd.Parameters.AddWithValue("@EmpID", txtEmployeeID.Text);
                    cmd.Parameters.AddWithValue("@LoginName", txtTechLoginName.Text.Replace(" ", "."));
                    cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
                    cmd.Parameters.AddWithValue("@LastName", txtLastName.Text.Trim());
                    cmd.Parameters.AddWithValue("@UserName", txtFirstName.Text + " " + txtLastName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Pass", txtPassword.Text);
                    cmd.Parameters.AddWithValue("@UserScope", ddlUserScope.SelectedValue);
                    cmd.Parameters.AddWithValue("@UserRole", ddlUserRole.SelectedValue);
                    cmd.Parameters.AddWithValue("@EmailID", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@ContactNo", txtPhone.Text);
                    cmd.Parameters.AddWithValue("@DepCode", ddlDepartment.SelectedValue);
                    cmd.Parameters.AddWithValue("@LocCode", ddlLocation.SelectedValue);
                    cmd.Parameters.AddWithValue("@Designation", txtJobTitle.Text);
                    cmd.Parameters.AddWithValue("@DomainType", ddlDomainType.SelectedValue);
                    cmd.Parameters.AddWithValue("@IsTechnicianLogin", chkEnableTechnician.Checked ? 1 : 0);
                    cmd.Parameters.AddWithValue("@ReqRef", ddlRequestType.SelectedValue);
                    cmd.Parameters.AddWithValue("@CategoryFK", ddlCategory1.SelectedValue);
                    cmd.Parameters.AddWithValue("@Org_ID", ddlOrg.SelectedValue);
                    cmd.Parameters.AddWithValue("@SDRole", ddlSDRole.SelectedValue);
                    cmd.Parameters.AddWithValue("@Option", "AddRequester");
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {

                        Session["Popup"] = "Insert";
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
        $"if (window.location.pathname.endsWith('/frmAddRequester.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Saved Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 4000); }}", true);
                    }

                }
            }

            //  Task ignoredAwaitableResult = this.Redirect();
            //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert('info',' Inserted successfully');window.location.href='" + Request.RawUrl +"';", true);


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
                //ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
        $"if (window.location.pathname.endsWith('/frmAddRequester.aspx')) {{ warning_noti('{HttpUtility.JavaScriptStringEncode("Duplicate UserName Found !")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);

            }
        }
    }

    protected void ImgBtnExport_Click(object sender, ImageClickEventArgs e)
    {
        try
        {

            DataTable dt = new DataTable("GridView_Data");
            foreach (System.Web.UI.WebControls.TableCell cell in gvTechnician.HeaderRow.Cells)
            {
                dt.Columns.Add(cell.Text);
            }
            foreach (GridViewRow row in gvTechnician.Rows)
            {
                dt.Rows.Add();
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    dt.Rows[dt.Rows.Count - 1][i] = row.Cells[i].Text;
                }
            }
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=Users.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
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

    protected void btnUpdateTechnician_Click(object sender, EventArgs e)
    {
        //try
        //{
            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                UpdateUserDetails("UpdateRequestorWithoutPass");
            }
            else if (txtPassword.Text.Length > 0)
            {
                if (txtPassword.Text == txtConfirmPassword.Text)
                {
                    UpdateUserDetails("UpdateRequestorWithPass");
                }
                else
                {
                    Session["Popup"] = "PasswordMatch";
                    ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"warning_noti('{HttpUtility.JavaScriptStringEncode("Password does Not Match !!")}');", true);
                }
            }
        //}
        //catch (ThreadAbortException e2)
        //{
        //    Console.WriteLine("Exception message: {0}", e2.Message);
        //    Thread.ResetAbort();
        //}
        //catch (Exception ex)
        //{
        //    var st = new StackTrace(ex, true);
        //    // Get the top stack frame
        //    var frame = st.GetFrame(0);
        //    // Get the line number from the stack frame
        //    var line = frame.GetFileLineNumber();
        //    inEr.InsertErrorLogsF(Session["UserName"].ToString()
        //    , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
        //    ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
   //     }

    }

    protected void UpdateUserDetails(string Function)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand("SD_spAddRequester", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("@EmpID", txtEmployeeID.Text);
                    cmd.Parameters.AddWithValue("@LoginName", txtTechLoginName.Text.Replace(" ", "."));
                    cmd.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
                    cmd.Parameters.AddWithValue("@LastName", txtLastName.Text.Trim());
                    cmd.Parameters.AddWithValue("@UserName", txtFirstName.Text + " " + txtLastName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Pass", txtPassword.Text);
                    cmd.Parameters.AddWithValue("@UserScope", ddlUserScope.SelectedValue);
                    cmd.Parameters.AddWithValue("@UserRole", ddlUserRole.SelectedValue);
                    cmd.Parameters.AddWithValue("@EmailID", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@ContactNo", txtPhone.Text);
                    cmd.Parameters.AddWithValue("@DepCode", ddlDepartment.SelectedValue);
                    cmd.Parameters.AddWithValue("@LocCode", ddlLocation.SelectedValue);
                    cmd.Parameters.AddWithValue("@Designation", txtJobTitle.Text);
                    cmd.Parameters.AddWithValue("@DomainType", ddlDomainType.SelectedValue);
                    cmd.Parameters.AddWithValue("@IsTechnicianLogin", chkEnableTechnician.Checked ? 1 : 0);
                    cmd.Parameters.AddWithValue("@ReqRef", ddlRequestType.SelectedValue);
                    cmd.Parameters.AddWithValue("@CategoryFK", ddlCategory1.SelectedValue);
                    cmd.Parameters.AddWithValue("@Org_ID", ddlOrg.SelectedValue);
                    cmd.Parameters.AddWithValue("@SDRole", ddlSDRole.SelectedValue);
                    cmd.Parameters.AddWithValue("@Status", ddlStatus.SelectedValue);
                    cmd.Parameters.AddWithValue("@UpdateBy", Session["UserName"].ToString());
                    cmd.Parameters.AddWithValue("@Option", Function);
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        Session["Popup"] = "Update";
                    Response.Redirect(Request.Url.AbsoluteUri);
            //            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
            //"if (window.location.pathname.endsWith('/frmAddRequester.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Updated Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 800); }}", true);
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
    protected void btnInsertTechnician_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtPassword.Text == txtConfirmPassword.Text)
            {
                SaveData();
            }
            else
            {
                Session["Popup"] = "PasswordMatch";
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"warning_noti('{HttpUtility.JavaScriptStringEncode("Password Not Match!!")}');", true);
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
                inEr.InsertErrorLogsF(Session["UserName"].ToString()
    , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }
    }
   public  static long UserID;
    protected void gvTechnician_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "DeleteEx")
            {
                //Determine the RowIndex of the Row whose Button was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                //Get the value of column from the DataKeys using the RowIndex.
                UserID = Convert.ToInt32(gvTechnician.DataKeys[rowIndex].Values["UserID"]);


                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SD_spAddRequester", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserID", UserID);

                        cmd.Parameters.AddWithValue("@Option", "DeleteRequestor");
                        cmd.CommandTimeout = 180;
                        int res = cmd.ExecuteNonQuery();
                        if (res > 0)
                        {

                            Session["Popup"] = "Delete";
                            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
            $"if (window.location.pathname.endsWith('/frmAddRequester.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Deleted Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                        }
                        con.Close();
                        pnlShowUsers.Visible = true;
                        ShowUserDetaiControl();
                        FillTechnicianDetails();

                    }
                }
            }
            if (e.CommandName == "SelectTech")
            {
                rfvtxtPassword.Enabled = false;
                rfvtxtPassword.Visible = false;
                regtxtPassword.Enabled = false;
                regtxtPassword.Visible = false;
                regftxtConfirmPassword.Enabled = false;

                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvTechnician.Rows[rowIndex];
                //Get the value of column from the DataKeys using the RowIndex.
                UserID = Convert.ToInt64(gvTechnician.DataKeys[rowIndex].Values["UserID"]);
                DataTable checkIFUserIsTech = new FillSDFields().CheckUserForTechnician(UserID);
                if (checkIFUserIsTech.Rows.Count > 0)
                {

                    chkEnableTechnician.Checked = true;
                    pnlEnableTechnician.Visible = true;
                    //ddlRequestType.SelectedValue = checkIFUserIsTech.Rows[0].Field<string>(2);
                    ddlCategory1.SelectedValue = checkIFUserIsTech.Rows[0].Field<string>(3);
                }
                else
                {
                    chkEnableTechnician.Checked = false;
                    pnlEnableTechnician.Visible = false;
                }
                //ddlDepartment.SelectedValue = gvTechnician.Rows[rowIndex].Cells[9].Text;
                txtEmployeeID.Text = gvTechnician.Rows[rowIndex].Cells[3].Text;
                txtFirstName.Text = gvTechnician.Rows[rowIndex].Cells[4].Text;
                txtPassword.Text = "";
                txtConfirmPassword.Text = "";
                txtLastName.Text = gvTechnician.Rows[rowIndex].Cells[5].Text;
                txtTechLoginName.Text = gvTechnician.Rows[rowIndex].Cells[6].Text;
                txtEmail.Text = gvTechnician.Rows[rowIndex].Cells[7].Text;
                System.Web.UI.WebControls.Label lblDepartmentCode = (row.FindControl("lblDepCode") as System.Web.UI.WebControls.Label);
                System.Web.UI.WebControls.Label lblUserScopeID = (row.FindControl("lblScopeID") as System.Web.UI.WebControls.Label);
                try
                {
                    ddlUserScope.SelectedValue = lblUserScopeID.Text;
                }
                catch { }
                //ddlDepartment.SelectedValue = lblDepartmentCode.Text;
                ddlUserRole.SelectedValue = gvTechnician.Rows[rowIndex].Cells[9].Text;
                txtPhone.Text = gvTechnician.Rows[rowIndex].Cells[12].Text;
                txtJobTitle.Text = gvTechnician.Rows[rowIndex].Cells[10].Text;
                System.Web.UI.WebControls.Label lblLocCode = (row.FindControl("lblLocCode") as System.Web.UI.WebControls.Label);

                //ddlLocation.SelectedValue = lblLocCode.Text;
                System.Web.UI.WebControls.Label OrgID = (row.FindControl("lblOrgFk") as System.Web.UI.WebControls.Label);
                if (ddlOrg.Items.FindByValue(OrgID.Text.ToString().Trim()) != null)
                {
                    ddlOrg.SelectedValue = OrgID.Text;
                    FillRequestType(Convert.ToInt64(OrgID.Text));
                    FillDepartment();
                    FillLocation();
                    ddlLocation.SelectedValue = lblLocCode.Text;
                    ddlDepartment.SelectedValue = lblDepartmentCode.Text;
                    if (checkIFUserIsTech.Rows.Count > 0)
                    {
                        ddlRequestType.SelectedValue = checkIFUserIsTech.Rows[0].Field<string>(2);
                    }
                }
                if (ddlSDRole.Items.FindByValue(gvTechnician.Rows[rowIndex].Cells[15].Text) != null)
                {
                    ddlSDRole.Text = gvTechnician.Rows[rowIndex].Cells[15].Text;
                }
                if (ddlStatus.Items.FindByValue(gvTechnician.Rows[rowIndex].Cells[16].Text) != null)
                {
                    ddlStatus.Text = gvTechnician.Rows[rowIndex].Cells[16].Text;
                }
                ddlStatus.Enabled = true;
                if (ddlDomainType.Items.FindByValue(gvTechnician.Rows[rowIndex].Cells[17].Text) != null)
                {
                    ddlDomainType.Text = gvTechnician.Rows[rowIndex].Cells[17].Text;
                }
                //ddlRequestType.SelectedValue = lblDesk.Text;
                FillCategory1();
                //if (lblCategoryID != null)
                //	ddlCategory1.SelectedValue = lblCategoryID.Text;


                btnInsertTechnician.Visible = false;
                btnUpdateTechnician.Visible = true;
                AddRequesterControl();
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
                inEr.InsertErrorLogsF(Session["UserName"].ToString()
                    , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());


            }
        }

    }

    protected void gvTechnician_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (Session["UserScope"].ToString() == "Master")
            {
                e.Row.Cells[0].Visible = true;
                e.Row.Cells[1].Visible = true;
            }

            if (Session["UserScope"].ToString() == "Technician")
            {
                e.Row.Cells[0].Visible = true;
                e.Row.Cells[1].Visible = false;

            }
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

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.AbsoluteUri);
    }

    protected void btnimportUser_Click(object sender, EventArgs e)
    {
        try
        {
            this.gvExcelFile.DataSource = (object)null;
            this.gvExcelFile.DataBind();
            pnlImportUser.Visible = true;
            pnlAddRequester.Visible = false;
            pnlShowUsers.Visible = false;
            btnAddRequester.CssClass = "btn btn-sm btn-outline-secondary";
            btnViewUsers.CssClass = "btn btn-sm btn-outline-secondary";
            btnimportUser.CssClass = "btn btn-sm btn-secondary";
            btnimportUser.Enabled = false;
            btnAddRequester.Enabled = true;
            btnViewUsers.Enabled = true;

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

    protected void btnAddRequester_Click(object sender, EventArgs e)
    {
        ddlStatus.SelectedValue = "1";
        ddlStatus.Enabled = false;
        AddRequesterControl();
    }

    protected void btnViewUsers_Click(object sender, EventArgs e)
    {
        ShowUserDetaiControl();
    }

    protected void chkEnableTechnician_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkEnableTechnician.Checked)
            {
                pnlAddRequester.Visible = true;
                pnlImportUser.Visible = false;
                pnlShowUsers.Visible = false;
                pnlEnableTechnician.Visible = true;

            }
            else
            {
                pnlAddRequester.Visible = true;
                pnlEnableTechnician.Visible = false;
            }
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
    protected void ShowUserDetaiControl()
    {
        try
        {
            pnlAddRequester.Visible = false;
            pnlImportUser.Visible = false;
            pnlShowUsers.Visible = true;
            btnAddRequester.CssClass = "btn btn-sm btn-outline-secondary ";
            btnimportUser.CssClass = "btn btn-sm btn-outline-secondary";
            btnViewUsers.CssClass = "btn btn-sm btn-secondary";
            btnViewUsers.Enabled = false;
            btnAddRequester.Enabled = true;
            btnimportUser.Enabled = true;
            FillTechnicianDetails();
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
    protected void AddRequesterControl()
    {
        try
        {
            pnlAddRequester.Visible = true;
            btnAddRequester.CssClass = "btn btn-sm btn-secondary";
            btnViewUsers.CssClass = "btn btn-sm btn-outline-secondary";
            btnimportUser.CssClass = "btn btn-sm btn-outline-secondary";
            btnAddRequester.Enabled = false;
            btnViewUsers.Enabled = true;
            btnimportUser.Enabled = true;
            pnlShowUsers.Visible = false;
            pnlImportUser.Visible = false;
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
    protected void btnn_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (GridViewRow row in this.gvExcelFile.Rows)
            {
                string cmdText = "SD_spAddRequester";
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(cmdText))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@UserID", r.Next());
                        cmd.Parameters.AddWithValue("@EmpID", row.Cells[0].Text.Trim().Replace("&nbsp;", " "));
                        cmd.Parameters.AddWithValue("@LoginName", row.Cells[5].Text.Trim().Replace("&nbsp;", " "));
                        cmd.Parameters.AddWithValue("@FirstName", row.Cells[2].Text.Trim().Replace("&nbsp;", " "));
                        cmd.Parameters.AddWithValue("@LastName", row.Cells[3].Text.Trim().Replace("&nbsp;", " "));
                        cmd.Parameters.AddWithValue("@UserName", row.Cells[1].Text.Trim().Replace("&nbsp;", " "));
                        cmd.Parameters.AddWithValue("@Pass", row.Cells[6].Text.Trim().Replace("&nbsp;", " "));
                        cmd.Parameters.AddWithValue("@UserScope", row.Cells[7].Text.Trim().Replace("&nbsp;", " "));
                        cmd.Parameters.AddWithValue("@UserRole", row.Cells[8].Text.Trim().Replace("&nbsp;", " "));
                        cmd.Parameters.AddWithValue("@EmailID", row.Cells[4].Text.Trim().Replace("&nbsp;", " "));
                        cmd.Parameters.AddWithValue("@ContactNo", row.Cells[13].Text.Trim().Replace("&nbsp;", " "));
                        cmd.Parameters.AddWithValue("@DepCode", row.Cells[10].Text.Trim().Replace("&nbsp;", " "));
                        cmd.Parameters.AddWithValue("@LocCode", row.Cells[9].Text.Trim().Replace("&nbsp;", " "));
                        cmd.Parameters.AddWithValue("@Designation", row.Cells[12].Text.Trim().Replace("&nbsp;", " "));
                        cmd.Parameters.AddWithValue("@DomainType", row.Cells[25].Text.Trim().Replace("&nbsp;", " "));
                        cmd.Parameters.AddWithValue("@Option", "AddRequesterBulk");
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        int res = cmd.ExecuteNonQuery();
                        if (res > 0)
                        {
                            Session["Popup"] = "Insert";
                            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
            $"if (window.location.pathname.endsWith('/frmAddRequester.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Saved Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                        }

                    }
                }
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
                inEr.InsertErrorLogsF(Session["UserName"].ToString()
    , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
    $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }
    }


    protected void butttonsubmit_Click(object sender, EventArgs e)
    {
        try
        {
            btnn.Visible = true;

            if (!this.customFile.HasFile)
                return;
            string fileName = Path.GetFileName(this.customFile.PostedFile.FileName);
            string extension = Path.GetExtension(this.customFile.PostedFile.FileName);
            string str = this.Server.MapPath(ConfigurationManager.AppSettings["FolderPath"] + fileName);
            this.customFile.SaveAs(str);
            this.Import_To_Grid(str, extension, rbHDR.SelectedItem.Text);
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
    private void Import_To_Grid(string FilePath, string Extension, string isHDR)
    {
        try
        {
            string format = "";
            switch (Extension)
            {
                case ".xls":
                    format = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                    break;
                case ".XLS":
                    format = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                    break;
                case ".xlsx":
                    format = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                    break;
                case ".XLSX":
                    format = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                    break;
                case ".csv":
                    format = ConfigurationManager.ConnectionStrings["csvstring"].ConnectionString;
                    break;
                case ".CSV":
                    format = ConfigurationManager.ConnectionStrings["csvstring"].ConnectionString;
                    break;
            }
            OleDbConnection oleDbConnection = new OleDbConnection(string.Format(format, (object)FilePath, (object)isHDR));
            OleDbCommand oleDbCommand = new OleDbCommand();
            OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter();
            DataTable dataTable = new DataTable();
            oleDbCommand.Connection = oleDbConnection;
            oleDbConnection.Open();
            string str = oleDbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, (object[])null).Rows[0]["TABLE_NAME"].ToString();
            oleDbConnection.Close();
            oleDbConnection.Open();
            oleDbCommand.CommandText = "SELECT * from [" + str + "]";
            oleDbDataAdapter.SelectCommand = oleDbCommand;
            oleDbDataAdapter.Fill(dataTable);
            oleDbConnection.Close();
            this.gvExcelFile.Caption = Path.GetFileName(FilePath);
            this.gvExcelFile.DataSource = (object)dataTable;
            this.gvExcelFile.DataBind();

            this.HighlightDuplicate(this.gvExcelFile);
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
    public void HighlightDuplicate(GridView grv)
    {
        for (int index1 = 0; index1 < grv.Rows.Count - 1; ++index1)
        {
            GridViewRow row1 = grv.Rows[index1];
            for (int index2 = index1 + 1; index2 < grv.Rows.Count; ++index2)
            {
                GridViewRow row2 = grv.Rows[index2];
                bool flag = true;
                if (row1.Cells[0].Text != row2.Cells[0].Text)
                    break;
                if (flag)
                {
                    //row1.BackColor = Color.Red;
                    //row1.ForeColor = Color.Black;
                    //row2.BackColor = Color.Red;
                    //row2.ForeColor = "Red";
                }
            }
        }
    }

    protected void ddlOrg_SelectedIndexChanged(object sender, EventArgs e)
    {

        try
        {
            FillRequestType(Convert.ToInt64(ddlOrg.SelectedValue));
            FillDepartment();
            FillLocation();
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
}
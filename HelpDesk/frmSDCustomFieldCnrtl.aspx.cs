using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HelpDesk_frmSDCustomFieldCnrtl : System.Web.UI.Page
{
    InsertErrorLogs inEr = new InsertErrorLogs();
    protected void Page_Load(object sender, EventArgs e)
    {



        if (Session["UserScope"] != null)
        {
            if (!IsPostBack)
            {

                FillSDCustomFieldsDetails();
                FillOrganization();
                pnlShowSDCustomFields.Visible = true;
                btnViewSDCustomFields.Enabled = false;
                btnViewSDCustomFields.CssClass = "btn btn-sm btn-secondary";
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

            DataTable SD_Org = new FillSDFields().FillOrganization(); ;

            ddlOrg.DataSource = SD_Org;
            ddlOrg.DataTextField = "OrgName";
            ddlOrg.DataValueField = "Org_ID";
            ddlOrg.DataBind();
            ddlOrg.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----Select----", "0"));


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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",$"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }
    }
    private void FillSDCustomFieldsDetails()
    {

        try
        {

            DataTable SD_SDCustomFields = new FillSDFields().FillSDCustomFields(); ;

            if (SD_SDCustomFields.Rows.Count > 0)
            {
                //  this.lb.Text = dataTable.Rows.Count.ToString();
                this.gvSDCustomFields.DataSource = (object)SD_SDCustomFields;
                this.gvSDCustomFields.DataBind();
            }
            else
            {
                this.gvSDCustomFields.DataSource = (object)null;
                this.gvSDCustomFields.DataBind();
            }
            if (SD_SDCustomFields.Rows.Count > 0)
            {
                GridFormat(SD_SDCustomFields);
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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",$"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }
    }
    protected void LoadControl()
    {
        // pnlShowRqstType.Controls.Clear();
        Control featuredProduct = Page.LoadControl("/HelpDesk/UserControl.ascx");
        featuredProduct.ID = "1234";
        pnlShowRqstType.Controls.Add(featuredProduct);



    }
    private bool IsUserControl
    {
        get
        {
            if (ViewState["IsUserControl"] != null)
            {
                return (bool)ViewState["IsUserControl"];
            }
            else
            {
                return false;
            }
        }
        set
        {
            ViewState["IsUserControl"] = value;
        }
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
    protected void ImgAddRequestType_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //  pnlShowRqstType.Controls.Clear();
            this.IsUserControl = true;

            //  this.IsCategoryControl = false;

            if (IsUserControl)
                LoadControl();
            this.Modal();
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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",$"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }
    }
    protected void ddlRequestType_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["SDRef"] = ddlRequestType.SelectedValue.ToString();


    }
    private void FillRequestType(long OrgID)
    {

        try
        {

            DataTable RequestType = new SDTemplateFileds().FillRequestType(OrgID);

            ddlRequestType.DataSource = RequestType;
            ddlRequestType.DataTextField = "ReqTypeRef";
            ddlRequestType.DataValueField = "ReqTypeRef";
            ddlRequestType.DataBind();
            ddlRequestType.Items.Insert(0, new ListItem("----Select----", "0"));


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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",$"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }
    }
    protected void ImgBtnExport_Click(object sender, ImageClickEventArgs e)
    {
        try
        {

            DataTable dt = new DataTable("GridView_Data");
            foreach (System.Web.UI.WebControls.TableCell cell in gvSDCustomFields.HeaderRow.Cells)
            {
                dt.Columns.Add(cell.Text);
            }
            foreach (GridViewRow row in gvSDCustomFields.Rows)
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
                Response.AddHeader("content-disposition", "attachment;filename=SD_CustomFields.xlsx");
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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",$"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }
    }
    public static long ID;
    protected void gvSDCustomFields_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "DeleteEx")
            {
                //Determine the RowIndex of the Row whose Button was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                //Get the value of column from the DataKeys using the RowIndex.
                ID = Convert.ToInt64(gvSDCustomFields.DataKeys[rowIndex].Values["ID"]);

                string Deskref = gvSDCustomFields.Rows[rowIndex].Cells[1].Text;
                GridViewRow row = gvSDCustomFields.Rows[rowIndex];
                Label OrgID = (row.FindControl("lblOrgFk") as Label);


                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SD_spCustomFieldCntl", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", ID);
                        cmd.Parameters.AddWithValue("@DeskRef", Deskref);
                        cmd.Parameters.AddWithValue("@OrgRef", OrgID.Text);
                        cmd.Parameters.AddWithValue("@Option", "DeleteCustomField");
                        cmd.CommandTimeout = 180;
                        int res = cmd.ExecuteNonQuery();
                        if (res > 0)
                        {
                            pnlShowRqstType.Visible = false;
                            //lblsuccess.ForeColor = System.Drawing.Color.Green;
                            //lblsuccess.Text = SDCustomFieldsName + " Deleted successfully";
                            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
        $"if (window.location.pathname.endsWith('/frmSDCustomFieldCnrtl.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Deleted Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                        }


                        //	ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert('error','" + SDCustomFieldsName + " Deleted successfully" + "');", true);

                        //Response.Redirect(Request.Url.AbsoluteUri);
                        con.Close();
                        FillSDCustomFieldsDetails();

                    }
                }
                //


            }
            if (e.CommandName == "SelectState")
            {
                btnInsertSDCustomFields.Visible = false;
                btnUpdateSDCustomFields.Visible = true;
                ShowAddSDCustomFieldsPanel();
                //Determine the RowIndex of the Row whose Button was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvSDCustomFields.Rows[rowIndex];
                //Get the value of column from the DataKeys using the RowIndex.
                ID = Convert.ToInt64(gvSDCustomFields.DataKeys[rowIndex].Values["ID"]);
                Label OrgID = (row.FindControl("lblOrgFk") as Label);
                if (ddlOrg.Items.FindByValue(OrgID.Text.ToString().Trim()) != null)
                {
                    ddlOrg.SelectedValue = OrgID.Text;
                    FillRequestType(Convert.ToInt64(OrgID.Text));
                    ddlRequestType.SelectedValue = gvSDCustomFields.Rows[rowIndex].Cells[1].Text;
                }

                ddlFieldType.SelectedValue = gvSDCustomFields.Rows[rowIndex].Cells[5].Text;
                if (ddlVisibilty.Items.FindByText(gvSDCustomFields.Rows[rowIndex].Cells[6].Text.Trim()).ToString() == "True")
                {

                    ddlVisibilty.SelectedValue = "1";
                }
                else
                {
                    ddlVisibilty.SelectedValue = "0";
                }
                if (ddlRequiredStatus.Items.FindByText(gvSDCustomFields.Rows[rowIndex].Cells[7].Text.Trim()).ToString() == "True")
                {

                    ddlRequiredStatus.SelectedValue = "1";
                }
                else
                {
                    ddlRequiredStatus.SelectedValue = "0";
                }

                ddlFieldType.Enabled = false;
                txtFieldName.Text = gvSDCustomFields.Rows[rowIndex].Cells[3].Text;
                txtFieldName.Enabled = false;
                ddlFieldMode.SelectedValue = gvSDCustomFields.Rows[rowIndex].Cells[4].Text;

                ddlFieldScope.SelectedValue = gvSDCustomFields.Rows[rowIndex].Cells[8].Text;


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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",$"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }
    }
    Random r = new Random();
    protected void SaveData()
    {
        long id = r.Next();
        //try
        //{
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {

            using (SqlCommand cmd = new SqlCommand("SD_spCustomFieldCntl", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@Deskref", ddlRequestType.SelectedValue);
                if (ddlFieldType.SelectedValue.ToString() == "TextBox")
                {
                    cmd.Parameters.AddWithValue("@FieldID", "txt" + id);
                }
                else if (ddlFieldType.SelectedValue.ToString() == "DropDown")
                {
                    cmd.Parameters.AddWithValue("@FieldID", "ddl" + id);

                }
                cmd.Parameters.AddWithValue("@FieldName", txtFieldName.Text.ToString());
                cmd.Parameters.AddWithValue("@FieldValue", txtFieldName.Text.ToString().Replace(" ", "_"));
                cmd.Parameters.AddWithValue("@FieldType", ddlFieldType.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@FieldMode", ddlFieldMode.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@Status", ddlVisibilty.SelectedValue.ToString());
                //	cmd.Parameters.AddWithValue("@SDRole", ddlSDRole.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@IsFieldReq", ddlRequiredStatus.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@FieldScope", ddlFieldScope.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@OrgRef", ddlOrg.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@Option", "AddCustomField");
                con.Open();
                int res = cmd.ExecuteNonQuery();
                if (res > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
        $"if (window.location.pathname.endsWith('/frmSDCustomFieldCnrtl.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Saved Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                }

            }
        }

        //  Task ignoredAwaitableResult = this.Redirect();
        //    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert('info',' Inserted successfully');window.location.href='" + Request.RawUrl +"';", true);




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
        //			ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",$"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

        //		}
        //	}
    }

    protected void btnInsertSDCustomFields_Click(object sender, EventArgs e)
    {
        SaveData();
    }

    protected void gvSDCustomFields_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (Session["UserScope"].ToString() == "Master")
        {
            e.Row.Cells[10].Visible = true;
            e.Row.Cells[11].Visible = true;
        }

        if (Session["UserScope"].ToString() == "Technician")
        {
            e.Row.Cells[10].Visible = false;
            e.Row.Cells[11].Visible = false;

        }
        if (Session["UserScope"].ToString() == "Admin")
        {
            e.Row.Cells[10].Visible = true;
            e.Row.Cells[11].Visible = false;

        }
    }

    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }

    protected void btnUpdateSDCustomFields_Click(object sender, EventArgs e)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand("SD_spCustomFieldCntl", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters.AddWithValue("@Deskref", ddlRequestType.SelectedValue);
                    cmd.Parameters.AddWithValue("@FieldMode", ddlFieldMode.SelectedValue);
                    cmd.Parameters.AddWithValue("@Status", ddlVisibilty.SelectedValue);
                    //		cmd.Parameters.AddWithValue("@SDRole", ddlSDRole.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@IsFieldReq", ddlRequiredStatus.SelectedValue);
                    cmd.Parameters.AddWithValue("@FieldScope", ddlFieldScope.SelectedValue);
                    cmd.Parameters.AddWithValue("@OrgRef", ddlOrg.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@Option", "UpdateCustomField");
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        //Session["Popup"] = "Update";
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
        $"if (window.location.pathname.endsWith('/frmSDCustomFieldCnrtl.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Updated Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                    }
                    //  ErrorMessage(this, "Welcome", "Greeting");
                    // ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert('success','Data has been updated');", true);
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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",$"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }
    }

    protected void ShowAddSDCustomFieldsPanel()
    {
        pnlAddSDCustomFields.Visible = true;
        btnAddSDCustomFields.CssClass = "btn btn-sm btn-secondary";
        pnlShowSDCustomFields.Visible = false;
        btnViewSDCustomFields.CssClass = "btn btn-sm btn-outline-secondary";
        btnAddSDCustomFields.Enabled = false;
        btnViewSDCustomFields.Enabled = true;
    }
    protected void btnAddSDCustomFields_Click(object sender, EventArgs e)
    {
        ShowAddSDCustomFieldsPanel();
        FillSDCustomFieldsDetails();
    }

    protected void btnViewSDCustomFields_Click(object sender, EventArgs e)
    {

        pnlAddSDCustomFields.Visible = false;
        btnAddSDCustomFields.CssClass = "btn btn-sm btn-outline-secondary";
        pnlShowSDCustomFields.Visible = true;
        btnViewSDCustomFields.CssClass = "btn btn-sm btn-secondary";
        btnAddSDCustomFields.Enabled = true;
        btnViewSDCustomFields.Enabled = false;
        FillSDCustomFieldsDetails();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.AbsoluteUri);
    }
    private void LgModal()
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
    protected void LoadOrgControl()
    {
        //try
        //{
        // pnlShowRqstType.Controls.Clear();
        System.Web.UI.Control featuredProduct = Page.LoadControl("/HelpDesk/UserControlOrg.ascx");
        featuredProduct.ID = "12321";
        pnlShowOrg.Controls.Add(featuredProduct);
        //}
        //catch (Exception ex)
        //{
        //	var st = new StackTrace(ex, true);
        //	// Get the top stack frame
        //	var frame = st.GetFrame(0);
        //	// Get the line number from the stack frame
        //	var line = frame.GetFileLineNumber();
        //	inEr.InsertErrorLogsF(Session["UserName"].ToString()
        //	, " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
        //	ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",$"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
        //}



    }
    private bool IsUserOrgControl
    {
        get
        {
            if (ViewState["IsUserOrgControl"] != null)
            {
                return (bool)ViewState["IsUserOrgControl"];
            }
            else
            {
                return false;
            }
        }
        set
        {
            ViewState["IsUserOrgControl"] = value;
        }
    }
    protected void imgbtnAddOrg_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //  pnlShowRqstType.Controls.Clear();
            this.IsUserOrgControl = true;

            //  this.IsCategoryControl = false;

            if (IsUserOrgControl)
                LoadOrgControl();
            this.LgModal();
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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",$"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }
    }
    protected void ddlOrg_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillRequestType(Convert.ToInt64(ddlOrg.SelectedValue));
    }
    protected void GridFormat(DataTable dt)
    {
        gvSDCustomFields.UseAccessibleHeader = true;
        gvSDCustomFields.HeaderRow.TableSection = TableRowSection.TableHeader;

        if (gvSDCustomFields.TopPagerRow != null)
        {
            gvSDCustomFields.TopPagerRow.TableSection = TableRowSection.TableHeader;
        }
        if (gvSDCustomFields.BottomPagerRow != null)
        {
            gvSDCustomFields.BottomPagerRow.TableSection = TableRowSection.TableFooter;
        }
        if (dt.Rows.Count > 0)
            gvSDCustomFields.FooterRow.TableSection = TableRowSection.TableFooter;
    }
}
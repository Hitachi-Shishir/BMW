using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HelpDesk_frmAddCustomFieldValues : System.Web.UI.Page
{
    InsertErrorLogs inEr = new InsertErrorLogs();
    public enum MessageType { success, error, info, warning };
    protected void ShowMessage(MessageType type, string Message)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "Showalert('" + type + "','" + Message + "');", true);
    }
    protected override void OnInit(EventArgs e)
    {
        try
        {
            //Change your condition here
            if (Session["Popup"] != null)
            {
                if (Session["Popup"].ToString() == "Insert")
                {
                    ShowMessage(MessageType.success, "Record Inserted Successfully!!");


                }
                if (Session["Popup"].ToString() == "Update")
                {
                    ShowMessage(MessageType.success, "Record Updated Successfully!!");


                }
                if (Session["Popup"].ToString() == "Delete")
                {
                    ShowMessage(MessageType.error, "Record Deleted Successfully!!");


                }
                if (Session["Popup"].ToString() == "Warning")
                {
                    ShowMessage(MessageType.warning, "Record Deleted Successfully!!");


                }
                Session.Remove("Popup");
            }


        }
        catch (ThreadAbortException e2)
        {
            Console.WriteLine("Exception message: {0}", e2.Message);
            Thread.ResetAbort();
        }
        catch (Exception ex)
        {
            ExceptionLogging.SendErrorToText(ex);

        }
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
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            if (Session["UserScope"] != null)
            {
                if (!IsPostBack)
                {


                    FillOrganization();

                    btnViewUsers.CssClass = "btn btn-sm btn-outline-secondary";
                    btnViewUsers.Enabled = false;
                    btnAddCustomField.CssClass = "btn btn-sm  btn-outline-secondary";
                    btnimportUser.CssClass = "btn btn-sm btn-secondary";
                    pnlShowUsers.Visible = true;
                }
            }
            else
            {
                Response.Redirect("/Default.aspx");
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
    private void FillCustomFieldName(long OrgID)
    {

        //try
        //{

        DataTable RequestType = new SDTemplateFileds().FillCustomFieldName(OrgID);

        ddlCustomFieldName.DataSource = RequestType;
        ddlCustomFieldName.DataTextField = "FieldName";
        ddlCustomFieldName.DataValueField = "FieldValue";
        ddlCustomFieldName.DataBind();
        ddlCustomFieldName.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----Select----", "0"));


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
    private void FillCustomFieldNameI(long OrgID)
    {

        //try
        //{

        DataTable RequestType = new SDTemplateFileds().FillCustomFieldName(OrgID);

        ddlCustomField.DataSource = RequestType;
        ddlCustomField.DataTextField = "FieldName";
        ddlCustomField.DataValueField = "FieldValue";
        ddlCustomField.DataBind();
        ddlCustomField.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----Select----", "0"));


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

    protected void GridFormat(DataTable dt)
    {
        gvCustomField.UseAccessibleHeader = true;
        gvCustomField.HeaderRow.TableSection = TableRowSection.TableHeader;

        if (gvCustomField.TopPagerRow != null)
        {
            gvCustomField.TopPagerRow.TableSection = TableRowSection.TableHeader;
        }
        if (gvCustomField.BottomPagerRow != null)
        {
            gvCustomField.BottomPagerRow.TableSection = TableRowSection.TableFooter;
        }
        if (dt.Rows.Count > 0)
            gvCustomField.FooterRow.TableSection = TableRowSection.TableFooter;
    }

    Random r = new Random();






    protected void SaveData()
    {

        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand("SD_spAddSDCustomFieldValue", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ColumnName", ddlCustomFieldName.SelectedValue);
                    cmd.Parameters.AddWithValue("@ColumnValue", txtCustomFieldValue.Text);
                    cmd.Parameters.AddWithValue("@Option", "AddCustomFieldValue");
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
         $"if (window.location.pathname.endsWith('/frmAddCustomFieldValues.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Saved Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
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
                ShowMessage(MessageType.warning, ex.ToString());
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
            foreach (System.Web.UI.WebControls.TableCell cell in gvCustomField.HeaderRow.Cells)
            {
                dt.Columns.Add(cell.Text);
            }
            foreach (GridViewRow row in gvCustomField.Rows)
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
                Response.AddHeader("content-disposition", "attachment;filename=CustomFields.xlsx");
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

    protected void btnUpdateCustomField_Click(object sender, EventArgs e)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("_sp_GetCustomFieldValue", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters.AddWithValue("@FieldName", ddlCustomFieldName.SelectedValue);
                    cmd.Parameters.AddWithValue("@FieldValue", txtCustomFieldValue.Text);
                    cmd.Parameters.AddWithValue("@Option", "updateCustomFieldValue");
                    cmd.CommandTimeout = 180;
                    int res = cmd.ExecuteNonQuery();
                    

                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
         $"if (window.location.pathname.endsWith('/frmAddCustomFieldValues.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Updated Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                    


                    //	ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert('error','" + PriorityName + " Deleted successfully" + "');", true);

                    //Response.Redirect(Request.Url.AbsoluteUri);
                    con.Close();
                    pnlShowUsers.Visible = true;
                    //ShowUserDetaiControl();


                }
            }
            //
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
    public static string FieldValue;

    protected void btnInsertCustomField_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlCustomFieldName.SelectedIndex > 0)
            {
                SaveData();
            }
            else
            {
                Session["Popup"] = "Please ";
                ShowMessage(MessageType.error, "Password Not Match!!");
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
    protected void gvCustomField_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        try
        {
            if (e.CommandName == "DeleteEx")
            {
                //Determine the RowIndex of the Row whose Button was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                //Get the value of column from the DataKeys using the RowIndex.
                ID = Convert.ToInt32(gvCustomField.DataKeys[rowIndex].Values["ID"]);
                FieldValue = gvCustomField.Rows[rowIndex].Cells[3].Text;

                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("_sp_GetCustomFieldValue", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", ID);
                        cmd.Parameters.AddWithValue("@FieldName", ddlCustomField.SelectedValue);
                        cmd.Parameters.AddWithValue("@FieldValue", FieldValue);
                        cmd.Parameters.AddWithValue("@Option", "DeleteCustomFieldValue");
                        cmd.CommandTimeout = 180;
                        int res = cmd.ExecuteNonQuery();
                        if (res > 0)
                        {

                            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
          $"if (window.location.pathname.endsWith('/frmAddCustomFieldValues.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Deleted Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                        }


                        //	ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert('error','" + PriorityName + " Deleted successfully" + "');", true);

                        //Response.Redirect(Request.Url.AbsoluteUri);
                        con.Close();
                        pnlShowUsers.Visible = true;
                        ShowUserDetaiControl();


                    }
                }
                //


            }
            if (e.CommandName == "SelectTech")
            {


                //Determine the RowIndex of the Row whose Button was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvCustomField.Rows[rowIndex];
                //Get the value of column from the DataKeys using the RowIndex.
                ID = Convert.ToInt64(gvCustomField.DataKeys[rowIndex].Values["ID"]);
                ddlCustomFieldName.SelectedValue = ddlCustomField.SelectedValue;
                txtCustomFieldValue.Text = gvCustomField.Rows[rowIndex].Cells[3].Text;
                btnInsertCustomField.Visible = false;
                btnUpdateCustomField.Visible = true;
                AddCustomFieldControl();
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

    protected void gvCustomField_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (Session["UserScope"].ToString() == "Master")
            {

            }
            if (Session["UserScope"].ToString() == "Admin")
            {
                e.Row.Cells[0].Visible = false;
            }
            if (Session["UserScope"].ToString() == "CustomField")
            {
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[1].Visible = false;

            }
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //	System.Web.UI.WebControls.Label lbl = (e.Row.FindControl("lblDescription") as System.Web.UI.WebControls.Label);
            //	lbl.Text = ddlCustomField.SelectedValue;
            //}
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
            pnlAddCustomField.Visible = false;
            pnlShowUsers.Visible = false;
            btnAddCustomField.CssClass = "btn btn-sm btn-outline-secondary";
            btnViewUsers.CssClass = "btn btn-sm btn-outline-secondary";
            btnimportUser.CssClass = "btn btn-sm    btn-secondary";
            btnimportUser.Enabled = false;
            btnAddCustomField.Enabled = true;
            btnViewUsers.Enabled = true;
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

    protected void btnAddCustomField_Click(object sender, EventArgs e)
    {

        try
        {
            AddCustomFieldControl();
            btnUpdateCustomField.Visible = false;
            btnInsertCustomField.Visible = true;
            ddlCustomField.ClearSelection();

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

    protected void btnViewUsers_Click(object sender, EventArgs e)
    {
        try
        {
            ShowUserDetaiControl();
            FillCustomFieldNameI(Convert.ToInt64(ddlOrg.SelectedValue));
            //FillSDAccount(ddlCustomField.SelectedValue);
            gvCustomField.DataSource = null;
            gvCustomField.DataBind();
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


    protected void ShowUserDetaiControl()
    {
        try
        {
            pnlAddCustomField.Visible = false;
            pnlImportUser.Visible = false;
            pnlShowUsers.Visible = true;
            btnAddCustomField.CssClass = "btn btn-sm btn-outline-secondary ";
            btnimportUser.CssClass = "btn btn-sm btn-outline-secondary";
            btnViewUsers.CssClass = "btn btn-sm btn-secondary";
            btnViewUsers.Enabled = false;
            btnAddCustomField.Enabled = true;
            btnimportUser.Enabled = true;
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
    protected void AddCustomFieldControl()
    {
        try
        {
            pnlAddCustomField.Visible = true;
            btnAddCustomField.CssClass = "btn btn-sm btn-secondary";
            btnViewUsers.CssClass = "btn btn-sm btn-outline-secondary";
            btnimportUser.CssClass = "btn btn-sm btn-outline-secondary";
            btnAddCustomField.Enabled = false;
            btnViewUsers.Enabled = true;
            btnimportUser.Enabled = true;
            pnlShowUsers.Visible = false;
            pnlImportUser.Visible = false;
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
    protected void btnn_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (GridViewRow row in this.gvExcelFile.Rows)
            {
                string cmdText = "SD_spAddCustomField";
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
                        cmd.Parameters.AddWithValue("@Option", "AddCustomFieldBulk");
                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        int res = cmd.ExecuteNonQuery();
                        if (res > 0)
                        {
                            Session["Popup"] = "Insert";
                            Response.Redirect(Request.Url.AbsoluteUri);
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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",$"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
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
            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",$"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
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
            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",$"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
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

    protected void ddlCustomField_SelectedIndexChanged(object sender, EventArgs e)
    {
        //try
        //{
        ShowUserDetaiControl();
        FillSDAccount(ddlCustomField.SelectedValue);
        //		}
        //		catch (ThreadAbortException e2)
        //		{
        //			Console.WriteLine("Exception message: {0}", e2.Message);
        //			Thread.ResetAbort();
        //		}
        //		catch (Exception ex)
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
    }

    protected void FillSDAccount(string FieldValue)
    {
        //try
        //{

        DataTable SD_SDAccount = new FillSDFields().FillCustomFieldValue(FieldValue); ;

        if (SD_SDAccount.Rows.Count > 0)
        {
            //  this.lb.Text = dataTable.Rows.Count.ToString();
            this.gvCustomField.DataSource = (object)SD_SDAccount;
            this.gvCustomField.DataBind();
        }
        else
        {
            this.gvCustomField.DataSource = (object)null;
            this.gvCustomField.DataBind();
        }
        if (SD_SDAccount.Rows.Count>0)
        {
            GridFormat(SD_SDAccount);
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
        //			ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",$"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
        //		}
        //	}
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

    protected void ddlOrg_SelectedIndexChanged(object sender, EventArgs e)
    {
        //try
        //{
        FillCustomFieldNameI(Convert.ToInt64(ddlOrg.SelectedValue));
        FillCustomFieldName(Convert.ToInt64(ddlOrg.SelectedValue));
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
}
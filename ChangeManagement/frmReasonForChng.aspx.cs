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

public partial class ChangeManagement_frmReasonForChng : System.Web.UI.Page
{
    InsertErrorLogs inEr = new InsertErrorLogs();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["UserScope"] != null)
            {
                if (IsUserOrgControl)
                {
                    LoadOrgControl();
                }
                if (!IsPostBack)
                {
                    FillRequestTypeDetails();
                    FillOrganization();
                    pnlViewRequestType.Visible = true;
                    btnViewReqType.Enabled = false;
                    btnViewReqType.CssClass = "btn btn-sm savebtn";
                    if (Request.QueryString.ToString().Contains("ShowPanelAdd"))
                    {
                        ShowAddReqPanel();
                    }
                    if (Request.QueryString["OrgId"] != "")
                    {
                        ddlOrg.SelectedValue = Request.QueryString["OrgId"];
                    }
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
    private void FillRequestTypeDetails()
    {

        try
        {

            DataTable SD_ReqType = new FillSDFields().FillReasonForChange(); 

            if (SD_ReqType.Rows.Count > 0)
            {
                //  this.lb.Text = dataTable.Rows.Count.ToString();
                this.gvReqType.DataSource = (object)SD_ReqType;
                this.gvReqType.DataBind();
                GridFormat(SD_ReqType);
            }
            else
            {
                this.gvReqType.DataSource = (object)null;
                this.gvReqType.DataBind();
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
    protected void GridFormat(DataTable dt)
    {
        gvReqType.UseAccessibleHeader = true;
        gvReqType.HeaderRow.TableSection = TableRowSection.TableHeader;

        if (gvReqType.TopPagerRow != null)
        {
            gvReqType.TopPagerRow.TableSection = TableRowSection.TableHeader;
        }
        if (gvReqType.BottomPagerRow != null)
        {
            gvReqType.BottomPagerRow.TableSection = TableRowSection.TableFooter;
        }
        if (dt.Rows.Count > 0)
            gvReqType.FooterRow.TableSection = TableRowSection.TableFooter;
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
    protected void SaveData()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand("SD_spReasonType", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", r.Next());
                    cmd.Parameters.AddWithValue("@ReasonTypeRef", txtReasonType.Text);
                    cmd.Parameters.AddWithValue("@ReasonDef", txtReasonDesc.Text);
                    cmd.Parameters.AddWithValue("@OrgRef", ddlOrg.SelectedValue.ToString());
                    //   cmd.Parameters.AddWithValue("@IsActive", '1');
                    cmd.Parameters.AddWithValue("@Option", "AddRequestType");
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        Session["resonforchng"] = "ok";
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
        $"if (window.location.pathname.endsWith('/frmReasonForChng.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Saved Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
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
            // Get the top stack frame
            var frame = st.GetFrame(0);
            // Get the line number from the stack frame
            var line = frame.GetFileLineNumber();
            inEr.InsertErrorLogsF(Session["UserName"].ToString()
, " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                    ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",$"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
        }
    }
    protected void btnSaveReqType_Click(object sender, EventArgs e)
    {
        SaveData();

    }

    protected void gvReqType_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (Session["UserScope"].ToString() == "Master")
            {
                e.Row.Cells[3].Visible = true;
                e.Row.Cells[4].Visible = true;
            }

            if (Session["UserScope"].ToString() == "Technician")
            {

                e.Row.Cells[4].Visible = false;

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
    public static int ID;
    protected void gvReqType_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "DeleteEx")
            {
                //Determine the RowIndex of the Row whose Button was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                //Get the value of column from the DataKeys using the RowIndex.
                ID = Convert.ToInt32(gvReqType.DataKeys[rowIndex].Values["ID"]);
                string ReqTypeRef = gvReqType.Rows[rowIndex].Cells[1].Text;
                string ReqTypeDef = gvReqType.Rows[rowIndex].Cells[2].Text;

                //try
                //{
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SD_spReasonType", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", ID);
                        cmd.Parameters.AddWithValue("@ReasonTypeRef", ReqTypeRef);
                        cmd.Parameters.AddWithValue("@ReasonDef", ReqTypeDef);
                        cmd.Parameters.AddWithValue("@OrgRef", ddlOrg.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@Option", "DelRequestType");
                        cmd.CommandTimeout = 180;
                        int res = cmd.ExecuteNonQuery();
                        if (res > 0)
                        {

                            //	lblsuccess.ForeColor = System.Drawing.Color.Green;
                            //lblsuccess.Text = PriorityName + " Deleted successfully";
                            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
        $"if (window.location.pathname.endsWith('/frmReasonForChng.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Deleted Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                        }


                        //	ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert('error','" + PriorityName + " Deleted successfully" + "');", true);

                        //Response.Redirect(Request.Url.AbsoluteUri);
                        con.Close();
                        FillRequestTypeDetails();

                    }
                }
                //
                //	}
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
                //			        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",$"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
                //		}

            }
            if (e.CommandName == "SelectState")
            {
                //Determine the RowIndex of the Row whose Button was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvReqType.Rows[rowIndex];
                //Get the value of column from the DataKeys using the RowIndex.
                ID = Convert.ToInt32(gvReqType.DataKeys[rowIndex].Values["ID"]);
                txtReasonType.Text = gvReqType.Rows[rowIndex].Cells[1].Text;
                txtReasonDesc.Text = gvReqType.Rows[rowIndex].Cells[2].Text;
                Label OrgID = (row.FindControl("lblOrgFk") as Label);
                if (ddlOrg.Items.FindByValue(OrgID.Text.ToString().Trim()) != null)
                {
                    ddlOrg.SelectedValue = OrgID.Text;
                }

                btnSave.Visible = false;
                btnUpdateReqType.Visible = true;
                ShowAddReqPanel();
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
                    ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",$"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
        }
    }

    protected void btnUpdateReqType_Click(object sender, EventArgs e)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand("SD_spReasonType", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters.AddWithValue("@ReasonTypeRef", txtReasonType.Text);
                    cmd.Parameters.AddWithValue("@ReasonDef", txtReasonDesc.Text);
                    cmd.Parameters.AddWithValue("@OrgRef", ddlOrg.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@Option", "UpdateRequestType");
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
        $"if (window.location.pathname.endsWith('/frmReasonForChng.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Updated Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
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
    protected void ImgBtnExport_Click(object sender, ImageClickEventArgs e)
    {
        try
        {

            DataTable dt = new DataTable("GridView_Data");
            foreach (System.Web.UI.WebControls.TableCell cell in gvReqType.HeaderRow.Cells)
            {
                dt.Columns.Add(cell.Text);
            }
            foreach (GridViewRow row in gvReqType.Rows)
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
                Response.AddHeader("content-disposition", "attachment;filename=RequestType.xlsx");
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
    protected void ShowAddReqPanel()
    {
        ShowPanelAdd.Visible = true;
        pnlViewRequestType.Visible = false;
        btnAddReqType.Enabled = false;
        btnAddReqType.CssClass = "btn btn-sm btn-secondary";
        btnViewReqType.CssClass = "btn btn-sm btn-outline-secondary";
        btnViewReqType.Enabled = true;

    }
    protected void ShowReqdetailPanle()
    {

        ShowPanelAdd.Visible = false;
        pnlViewRequestType.Visible = true;
        btnAddReqType.Enabled = true;
        btnAddReqType.CssClass = "btn btn-sm btn-outline-secondary";
        btnViewReqType.CssClass = "btn btn-sm btn-secondary";
        btnViewReqType.Enabled = false;
    }
    protected void btnAddReqType_Click(object sender, EventArgs e)
    {
        ShowAddReqPanel();
        btnSave.Visible = true;
        btnUpdateReqType.Visible = false;
        txtReasonType.Text = "";
        txtReasonDesc.Text = "";
        ddlOrg.ClearSelection();
        if (Request.QueryString["OrgId"] != "")
        {
            ddlOrg.SelectedValue = Request.QueryString["OrgId"];
        }
    }
    protected void btnViewReqType_Click(object sender, EventArgs e)
    {
        ShowReqdetailPanle();
        FillRequestTypeDetails();

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.AbsoluteUri);
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
        //	        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",$"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
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
    protected void btnCloseDesk_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterStartupScript(this, GetType(), "showAlertAndRedirect", $@"
                                setTimeout(function() {{
                                    success_noti('{"You have been redirected to the Desk Configuration where you left off!"}'); 
                                    setTimeout(function() {{
                                        if (window.opener) {{
                                            window.opener.location.href = '/DeskConfiguration.aspx';
                                        }}
                                        window.close();
                                    }}, 3000); // Delay for the alert
                                }}, 100); // Initial delay to ensure success_noti is called first
                            ", true);
    }
}
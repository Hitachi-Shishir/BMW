using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class frmAddHolidays : System.Web.UI.Page
{
    public enum MessageType { success, error, info, warning };
    InsertErrorLogs inEr = new InsertErrorLogs();
    protected void ShowMessage(MessageType type, string Message)
    {
        ScriptManager.RegisterStartupScript(this, this.GetType(), System.Guid.NewGuid().ToString(), "Showalert('" + type + "','" + Message + "');", true);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["UserScope"] != null)
            {
                if (!IsPostBack)
                {

                    FillHoliday();
                    FillOrganization();
                    btnViewUsers.CssClass = "btn btn-sm btnEnabled btn-secondary";
                    btnViewUsers.Enabled = false;
                    btnAddHoliday.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
                    btnimportUser.CssClass = "btn btn-sm btnDisabled btn-outline-secondary";
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
    private void FillHoliday()
    {

        try
        {

            DataTable SD_Holiday = new FillSDFields().FillSDHoliday(); 
            if (SD_Holiday.Rows.Count > 0)
            {
                //  this.lb.Text = dataTable.Rows.Count.ToString();
                this.gvHRRequest.DataSource = (object)SD_Holiday;
                this.gvHRRequest.DataBind();
            }
            else
            {
                this.gvHRRequest.DataSource = (object)null;
                this.gvHRRequest.DataBind();
            }
            if (SD_Holiday.Rows.Count > 0)
            {
                GridFormat(SD_Holiday);
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
                inEr.InsertErrorLogsF(Session["UserName"].ToString()
, "Add Holiday: Error While Populating Hoilday in table " + Request.Url.ToString() + "Got Exception" + ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
            }
        }
    }
    protected void SaveData()
    {

        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                string dateString = txtholidayDate.Text; // "18-01-2024"
                DateTime holidayDate = DateTime.ParseExact(dateString, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                using (SqlCommand cmd = new SqlCommand("SD_spAddHoliday", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", r.Next());
                    cmd.Parameters.AddWithValue("@HolidayName", txtHolidayName.Text);
                    cmd.Parameters.AddWithValue("@HolidayDate", holidayDate);
                    cmd.Parameters.AddWithValue("@OrgID", ddlOrg.SelectedValue.ToString());

                    cmd.Parameters.AddWithValue("@Option", "AddHoliday");
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        Session["Popup"] = "Insert";
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
        $"if (window.location.pathname.endsWith('/frmAddHolidays.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Saved Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
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
                inEr.InsertErrorLogsF(Session["UserName"].ToString()
        , "Add Holiday: Error While Populating ParentCategory " + Request.Url.ToString() + "Got Exception" + ex.ToString());
            }
            //ShowMessage(MessageType.warning, ex.ToString());
            //    // msg.ReportError1(ex.Message);
            //    // lblsuccess.Text = msg.ms; ;
        }
    }
    protected void ImgBtnExport_Click(object sender, EventArgs e)
    {
        try
        {

            DataTable dt = new DataTable("GridView_Data");
            foreach (System.Web.UI.WebControls.TableCell cell in gvHRRequest.HeaderRow.Cells)
            {
                dt.Columns.Add(cell.Text);
            }
            foreach (GridViewRow row in gvHRRequest.Rows)
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
                Response.AddHeader("content-disposition", "attachment;filename=Holiday.xlsx");
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
    protected void btnUpdateHoliday_Click(object sender, EventArgs e)
    {
        UpdateUserDetails();

    }
    protected void GridFormat(DataTable dt)
    {
        gvHRRequest.UseAccessibleHeader = true;
        gvHRRequest.HeaderRow.TableSection = TableRowSection.TableHeader;

        if (gvHRRequest.TopPagerRow != null)
        {
            gvHRRequest.TopPagerRow.TableSection = TableRowSection.TableHeader;
        }
        if (gvHRRequest.BottomPagerRow != null)
        {
            gvHRRequest.BottomPagerRow.TableSection = TableRowSection.TableFooter;
        }
        if (dt.Rows.Count > 0)
            gvHRRequest.FooterRow.TableSection = TableRowSection.TableFooter;
    }
    protected void UpdateUserDetails()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                string dateString = txtholidayDate.Text; // "18-01-2024"
                DateTime holidayDate = DateTime.ParseExact(dateString, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                using (SqlCommand cmd = new SqlCommand("SD_spAddHoliday", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", UserID);
                    cmd.Parameters.AddWithValue("@HolidayName", txtHolidayName.Text);
                    cmd.Parameters.AddWithValue("@HolidayDate", holidayDate);
                    cmd.Parameters.AddWithValue("@OrgID", ddlOrg.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@Option", "UpdateHoliday");
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        Session["Popup"] = "Update";
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
        $"if (window.location.pathname.endsWith('/frmAddHolidays.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Updated Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
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
    protected void btnInsertHoliday_Click(object sender, EventArgs e)
    {

        SaveData();


    }
    static long UserID;
    protected void gvHRRequest_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "DeleteEx")
        {
            try
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                UserID = Convert.ToInt32(gvHRRequest.DataKeys[rowIndex].Values["ID"]);


                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SD_spAddHoliday", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", UserID);
                        //cmd.Parameters.AddWithValue("@OrgID", Convert.ToString(Session["SD_OrgID"]));
                        cmd.Parameters.AddWithValue("@Option", "DeleteHoliday");
                        cmd.CommandTimeout = 180;
                        int res = cmd.ExecuteNonQuery();
                        if (res > 0)
                        {
                            Session["Popup"] = "Delete";
                            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
        $"if (window.location.pathname.endsWith('/frmAddHolidays.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Deleted Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
    $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
                        }
                        con.Close();
                        pnlShowUsers.Visible = true;
                        ShowUserDetaiControl();
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
        if (e.CommandName == "SelectTech")
        {
            try
            {
                //Determine the RowIndex of the Row whose Button was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvHRRequest.Rows[rowIndex];
                //Get the value of column from the DataKeys using the RowIndex.
                UserID = Convert.ToInt64(gvHRRequest.DataKeys[rowIndex].Values["ID"]);
                txtHolidayName.Text = gvHRRequest.Rows[rowIndex].Cells[3].Text;
                txtholidayDate.Text = gvHRRequest.Rows[rowIndex].Cells[4].Text;
                Label OrgID = (row.FindControl("lblOrgFk") as Label);
                if (ddlOrg.Items.FindByValue(OrgID.Text.ToString().Trim()) != null)
                {
                    ddlOrg.SelectedValue = OrgID.Text;

                }
                btnInsertHoliday.Visible = false;
                btnUpdateHoliday.Visible = true;
                AddHolidayControl();
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

    }
    protected void gvHRRequest_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {


            if (Session["UserScope"].ToString() == "Master")
            {

            }

            if (Session["UserScope"].ToString() == "Technician" || Session["UserScope"].ToString() == "Admin")
            {
                e.Row.Cells[0].Visible = false;
                e.Row.Cells[1].Visible = false;

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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
     $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
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
            pnlAddHoliday.Visible = false;
            pnlShowUsers.Visible = false;
            btnAddHoliday.CssClass = "btn btn-sm btn-outline-secondary";
            btnViewUsers.CssClass = "btn btn-sm btn-outline-secondary";
            btnimportUser.CssClass = "btn btn-sm btn-secondary";
            btnimportUser.Enabled = false;
            btnAddHoliday.Enabled = true;
            btnViewUsers.Enabled = true;
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
            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
     $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
        }
    }
    protected void btnAddHoliday_Click(object sender, EventArgs e)
    {
        try
        {
            AddHolidayControl();
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
            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
    $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
        }
    }
    protected void btnViewUsers_Click(object sender, EventArgs e)
    {
        try
        {
            ShowUserDetaiControl();

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
            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
     $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

        }
    }
    protected void ShowUserDetaiControl()
    {
        try
        {
            pnlAddHoliday.Visible = false;
            pnlImportUser.Visible = false;
            pnlShowUsers.Visible = true;
            btnAddHoliday.CssClass = "btn btn-sm btn-outline-secondary ";
            btnimportUser.CssClass = "btn btn-sm btn-outline-secondary";
            btnViewUsers.CssClass = "btn btn-sm btn-secondary";
            btnViewUsers.Enabled = false;
            btnAddHoliday.Enabled = true;
            btnimportUser.Enabled = true;
            FillHoliday();
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
            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
    $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
        }
    }
    protected void AddHolidayControl()
    {
        try
        {

            pnlAddHoliday.Visible = true;
            btnAddHoliday.CssClass = "btn btn-sm btn-secondary";
            btnViewUsers.CssClass = "btn btn-sm btn-outline-secondary";
            btnimportUser.CssClass = "btn btn-sm btn-outline-secondary";
            btnAddHoliday.Enabled = false;
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
    protected void btnn_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (GridViewRow row in this.gvExcelFile.Rows)
            {
                string cmdText = "SD_spAddHoliday";
                using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(cmdText))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@ID", r.Next());
                        cmd.Parameters.AddWithValue("@HolidayName", row.Cells[0].Text.Trim().Replace("&nbsp;", " "));
                        cmd.Parameters.AddWithValue("@HolidayDate", Convert.ToDateTime(row.Cells[1].Text.Trim().Replace("&nbsp;", " ")).ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@OrgID", Convert.ToString(row.Cells[2].Text.Trim().Replace("&nbsp;", " ")));

                        cmd.Parameters.AddWithValue("@Option", "AddHolidayBulk");



                        cmd.Connection = sqlConnection;
                        sqlConnection.Open();
                        int res = cmd.ExecuteNonQuery();
                        if (res > 0)
                        {
                            Session["Popup"] = "Insert";
                            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
        $"if (window.location.pathname.endsWith('/frmAddHolidays.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Saved Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
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
                , "Add Holiday: Error While Store Data In DB " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
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
            , "Add Holiday: Error While Fetch the Data From file to Datatable " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
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
            , "Add Holiday: Error While Fetch the Data From file to Datatable " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());

            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
        }

    }
    public void HighlightDuplicate(GridView grv)
    {
        try
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
}
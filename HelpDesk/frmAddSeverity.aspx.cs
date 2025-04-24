using ClosedXML.Excel;
using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class frmAddSeverity : System.Web.UI.Page
{
    InsertErrorLogs inEr = new InsertErrorLogs();
    Random r = new Random();
    public static Int64 ID;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null || Convert.ToString(Session["UserID"]) == "")
        {
            Response.Redirect("/Default.aspx");
        }
        if (!IsPostBack)
        {
            FillOrganizationSeverity();
            if (Session["UserScope"].ToString().ToLower() == "admin")
            {
                FillSeverityDetailsWithCustomer(Convert.ToString(Session["OrgNameDesk"]));
            }
            else
            {
                FillSeverityDetailsWithCustomer(ddlOrg4.SelectedValue);
                FillRequestTypeSeverity(Convert.ToInt64(ddlOrg4.SelectedValue));

                Session["SDRef"] = ddlRequestTypeSeverity.SelectedValue.ToString();
            }
        }
    }
    protected void GridFormat5(DataTable dt)
    {
        gvSeverity.UseAccessibleHeader = true;
        gvSeverity.HeaderRow.TableSection = TableRowSection.TableHeader;

        if (gvSeverity.TopPagerRow != null)
        {
            gvSeverity.TopPagerRow.TableSection = TableRowSection.TableHeader;
        }
        if (gvSeverity.BottomPagerRow != null)
        {
            gvSeverity.BottomPagerRow.TableSection = TableRowSection.TableFooter;
        }
        if (dt.Rows.Count > 0)
            gvSeverity.FooterRow.TableSection = TableRowSection.TableFooter;
    }
    private void FillSeverityDetailsWithCustomer(string orgid)
    {
        try
        {
            DataTable SD_Severity = new FillSDFields().FillSeverityWithCustomer(orgid);
            if (SD_Severity.Rows.Count > 0)
            {
                //  this.lb.Text = dataTable.Rows.Count.ToString();
                this.gvSeverity.DataSource = (object)SD_Severity;
                this.gvSeverity.DataBind();
            }
            else
            {
                this.gvSeverity.DataSource = (object)null;
                this.gvSeverity.DataBind();
            }
            //GridFormat5(SD_Severity);
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
    private void FillOrganizationSeverity()
    {
        try
        {
            DataTable SD_Org = new FillSDFields().FillOrganization(); ;
            ddlOrg4.DataSource = SD_Org;
            ddlOrg4.DataTextField = "OrgName";
            ddlOrg4.DataValueField = "Org_ID";
            ddlOrg4.DataBind();
            ddlOrg4.Items.Insert(0, new System.Web.UI.WebControls.ListItem("   -Select Organization-", "0"));
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
    private void FillSeverityDetails()
    {
        try
        {
            DataTable SD_Severity = new FillSDFields().FillSeverity();
            if (SD_Severity.Rows.Count > 0)
            {
                this.gvSeverity.DataSource = (object)SD_Severity;
                this.gvSeverity.DataBind();
            }
            else
            {
                this.gvSeverity.DataSource = (object)null;
                this.gvSeverity.DataBind();
            }
            //GridFormat5(SD_Severity);
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
    protected void ddlRequestTypeSeverity_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["SDRef"] = ddlRequestTypeSeverity.SelectedValue.ToString();
        

    }
    private void FillRequestTypeSeverity(long OrgID)
    {
        try
        {
            DataTable RequestType = new SDTemplateFileds().FillRequestType(OrgID);
            ddlRequestTypeSeverity.DataSource = RequestType;
            ddlRequestTypeSeverity.DataTextField = "ReqTypeRef";
            ddlRequestTypeSeverity.DataValueField = "ReqTypeRef";
            ddlRequestTypeSeverity.DataBind();
            ddlRequestTypeSeverity.Items.Insert(0, new System.Web.UI.WebControls.ListItem("   -Select RequestType-", "0"));
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
    protected void SaveDataSeverity()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SD_spAddSeverity", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", r.Next());
                    cmd.Parameters.AddWithValue("@DeskRef", ddlRequestTypeSeverity.SelectedValue);
                    cmd.Parameters.AddWithValue("@Serverityref", Session["SDRef"].ToString().Trim() + "||" + txtSeverityName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Serveritycoderef", txtSeverityName.Text.Trim());
                    cmd.Parameters.AddWithValue("@SeverityDesc", txtSeverityDescription.Text);
                    cmd.Parameters.AddWithValue("@OrgDeskRef", ddlOrg4.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@ResponseTime", Convert.ToInt32(txtResponseTime.Text));
                    cmd.Parameters.AddWithValue("@ResolutionTime", Convert.ToInt32(txtResolutionTime.Text));
                    cmd.Parameters.Add("@Error", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@Option", "AddSeverity");
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    string ErrorChk = cmd.Parameters["@Error"].Value.ToString();
                    if (res > 0)
                    {
                        Session["Severity"] = "OK";
                        //ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/frmAddSeverity.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Saved Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/Helpdesk/frmAddSeverity.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Saved Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                        FillSeverityDetailsWithCustomer(Convert.ToString(ddlOrg4.SelectedValue));
                        btnInsertSeverity.Visible = true;
                        btnUpdateSeverity.Visible = false;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/Helpdesk/frmAddSeverity.aspx')) {{ warning_noti('{HttpUtility.JavaScriptStringEncode("" + ErrorChk + "")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
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
    protected void btnInsertSeverity_Click(object sender, System.EventArgs e)
    {
        SaveDataSeverity();
    }
    protected void gvSeverity_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "DeleteEx")
            {
                //Determine the RowIndex of the Row whose Button was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                //Get the value of column from the DataKeys using the RowIndex.
                ID = Convert.ToInt32(gvSeverity.DataKeys[rowIndex].Values["ID"]);
                string Deskref = gvSeverity.Rows[rowIndex].Cells[1].Text;
                string SeverityName = gvSeverity.Rows[rowIndex].Cells[2].Text;
                try
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("SD_spAddSeverity", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ID", ID);
                            cmd.Parameters.AddWithValue("@DeskRef", Deskref);
                            cmd.Parameters.AddWithValue("@Option", "DeleteSeverity");
                            cmd.CommandTimeout = 180;
                            int res = cmd.ExecuteNonQuery();
                            if (res > 0)
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/frmAddSeverity.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Saved Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                                FillSeverityDetailsWithCustomer(Convert.ToString(Session["OrgNameDesk"]));
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
            if (e.CommandName == "SelectSeverity")
            {
                //Determine the RowIndex of the Row whose Button was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvSeverity.Rows[rowIndex];
                //Get the value of column from the DataKeys using the RowIndex.
                ID = Convert.ToInt32(gvSeverity.DataKeys[rowIndex].Values["ID"]);

                txtSeverityName.Text = gvSeverity.Rows[rowIndex].Cells[2].Text;
                txtSeverityDescription.Text = gvSeverity.Rows[rowIndex].Cells[3].Text;
                txtResponseTime.Text = gvSeverity.Rows[rowIndex].Cells[4].Text;
                txtResolutionTime.Text = gvSeverity.Rows[rowIndex].Cells[5].Text;
                Label OrgID = (row.FindControl("lblOrgFk") as Label);
                if (ddlOrg4.Items.FindByValue(OrgID.Text.ToString().Trim()) != null)
                {
                    ddlOrg4.SelectedValue = OrgID.Text;
                    FillRequestTypeSeverity(Convert.ToInt32(OrgID.Text));
                }
                ddlRequestTypeSeverity.SelectedValue = gvSeverity.Rows[rowIndex].Cells[1].Text;
                btnInsertSeverity.Visible = false;
                btnUpdateSeverity.Visible = true;
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
    private DataTable ExtractDataFromGridView(GridView gridView, string tableName)
    {
        DataTable dt = new DataTable(tableName);

        // Check if columns already exist before adding
        foreach (TableCell cell in gridView.HeaderRow.Cells)
        {
            string columnName = HttpUtility.HtmlDecode(cell.Text);
            if (!dt.Columns.Contains(columnName))
            {
                dt.Columns.Add(columnName);
            }
        }

        foreach (GridViewRow row in gridView.Rows)
        {
            DataRow dataRow = dt.NewRow();
            for (int i = 0; i < row.Cells.Count; i++)
            {
                string cellText = string.Empty;

                // Check if the cell contains a Label control
                foreach (Control control in row.Cells[i].Controls)
                {
                    Label lbl = control as Label;
                    if (lbl != null)
                    {
                        cellText = lbl.Text;
                        break;
                    }
                }

                if (string.IsNullOrEmpty(cellText))
                {
                    cellText = HttpUtility.HtmlDecode(row.Cells[i].Text);
                }

                // If the column is EntryStartDate or EntryEndDate, ensure the value is a valid DateTime
                if (dt.Columns[i].ColumnName == "EntryStartDate" || dt.Columns[i].ColumnName == "EntryEndDate")
                {
                    DateTime entryStartDate;
                    if (DateTime.TryParse(cellText, out entryStartDate))
                    {
                        dataRow[i] = entryStartDate;
                    }
                    else
                    {
                        dataRow[i] = DBNull.Value; // or handle as needed
                    }
                }
                else
                {
                    dataRow[i] = cellText;
                }
            }
            dt.Rows.Add(dataRow);
        }

        return dt;
    }
    protected void ImgBtnExport4_Click(object sender, EventArgs e)
    {
        try
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                // Extract data from the first GridView and add it to the workbook
                DataTable dt1 = ExtractDataFromGridView(gvSeverity, "GridView_Data");
                var ws1 = wb.Worksheets.Add(dt1, "SD_Severity");
                ApplyStylesFromGridView(gvSeverity, ws1, dt1);


                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=SD_Severity.xlsx");

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
    private void ApplyStylesFromGridView(GridView gridView, IXLWorksheet ws, DataTable dt)
    {
        for (int i = 0; i < gridView.Rows.Count; i++)
        {
            for (int j = 0; j < gridView.Rows[i].Cells.Count; j++)
            {
                var gridViewCell = gridView.Rows[i].Cells[j];
                var excelCell = ws.Cell(i + 2, j + 1);
                string cellText = dt.Rows[i][j].ToString();
                if (gridViewCell.BackColor != System.Drawing.Color.Empty)
                {
                    excelCell.Style.Fill.BackgroundColor = XLColor.FromColor(gridViewCell.BackColor);
                }
                if (gridViewCell.ForeColor != System.Drawing.Color.Empty)
                {
                    excelCell.Style.Font.FontColor = XLColor.FromColor(gridViewCell.ForeColor);
                }
                if (gridViewCell.Font.Bold)
                {
                    excelCell.Style.Font.Bold = true;
                }
                if (gridViewCell.Font.Italic)
                {
                    excelCell.Style.Font.Italic = true;
                }
                if (gridViewCell.Font.Underline)
                {
                    excelCell.Style.Font.Underline = XLFontUnderlineValues.Single;
                }
                if (dt.Columns[j].ColumnName == "Total Time")
                {
                    double timeValue;
                    if (double.TryParse(cellText, out timeValue))
                    {
                        excelCell.Value = timeValue;
                        excelCell.Style.NumberFormat.Format = "0.00";
                    }
                    else
                    {
                        excelCell.Value = cellText;
                    }
                }
                else
                {
                    excelCell.Value = cellText;
                }
            }
        }
    }
    protected void gvSeverity_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (Session["UserScope"].ToString() == "Master")
            {
                e.Row.Cells[7].Visible = true;
                e.Row.Cells[8].Visible = true;
            }

            if (Session["UserScope"].ToString() == "Technician" || Session["UserScope"].ToString() == "Admin")
            {
                e.Row.Cells[7].Visible = true;
                e.Row.Cells[8].Visible = false;

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
    protected void btnUpdateSeverity_Click(object sender, EventArgs e)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SD_spAddSeverity", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters.AddWithValue("@DeskRef", ddlRequestTypeSeverity.SelectedValue);
                    cmd.Parameters.AddWithValue("@Serverityref", ddlRequestTypeSeverity.SelectedValue + "||" + txtSeverityName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Serveritycoderef", txtSeverityName.Text.Trim());
                    cmd.Parameters.AddWithValue("@SeverityDesc", txtSeverityDescription.Text);
                    cmd.Parameters.AddWithValue("@OrgDeskRef", ddlOrg4.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@ResponseTime", Convert.ToInt32(txtResponseTime.Text));
                    cmd.Parameters.AddWithValue("@ResolutionTime", Convert.ToInt32(txtResolutionTime.Text));
                    cmd.Parameters.AddWithValue("@Option", "UpdateSeverity");
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        //ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/frmAddSeverity.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Updated Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"success_noti('{HttpUtility.JavaScriptStringEncode("Updated Successfully!")}');", true);
                        //FillSeverityDetails();
                        FillSeverityDetailsWithCustomer(Convert.ToString(Session["OrgNameDesk"]));
                        btnInsertSeverity.Visible = true;
                        btnUpdateSeverity.Visible = false;
                        Response.Redirect(Request.Url.AbsoluteUri);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/Helpdesk/frmAddSeverity.aspx')) {{ warning_noti('{HttpUtility.JavaScriptStringEncode("Already Exists !")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
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
    protected void btnCancel5_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.AbsoluteUri);
    }
    protected void ddlOrg4_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillRequestTypeSeverity(Convert.ToInt64(ddlOrg4.SelectedValue));
            FillSeverityDetailsWithCustomer(Convert.ToString(ddlOrg4.SelectedValue));
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
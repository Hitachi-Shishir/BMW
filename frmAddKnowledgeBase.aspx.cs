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

public partial class frmAddKnowledgeBase : System.Web.UI.Page
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
            chkViewToUser.InputAttributes["class"] = "form-check-input";
            chkViewToTechnician.InputAttributes["class"] = "form-check-input";
            if (Session["SD_OrgID"] != null)
            {
                if (!IsPostBack)
                {
                    btnUpdate.Visible = false;
                    pnlShowPriority.Visible = false;
                    FillOrganization();
                    if (Convert.ToString(Session["UserRole"]).ToUpper() == "MASTER")
                    {
                        ddlOrg.SelectedValue = Convert.ToString(Session["SD_OrgID"]);
                        ddlOrg.Enabled = true;
                    }
                    else if (Convert.ToString(Session["UserRole"]).ToUpper() == "ADMIN")
                    {
                        ddlOrg.SelectedValue = Convert.ToString(Session["SD_OrgID"]);
                        ddlOrg.Enabled = false;
                    }
                    else if (Session["UserRole"].ToString().ToLower() == "technician")
                    {
                        ddlOrg.SelectedValue = Convert.ToString(Session["SD_OrgID"]);
                        ddlOrg.Enabled = false;
                        pnlShowPriority.Visible = true;
                        pnlIncident.Visible = false;
                        divTab.Visible = false;
                        FillShowResolution(ddlOrg.SelectedValue, "", "1");
                        HideEditAndDeleteButtons();
                    }
                    else
                    {
                        ddlOrg.SelectedValue = Convert.ToString(Session["SD_OrgID"]);
                        ddlOrg.Enabled = false;
                        pnlShowPriority.Visible = true;
                        pnlIncident.Visible = false;
                        divTab.Visible = false;
                        FillShowResolution(ddlOrg.SelectedValue, "1", "");
                        HideEditAndDeleteButtons();
                    }
                }
            }

            else
            {
                Response.Redirect("/Default.aspx");
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
    private void HideEditAndDeleteButtons()
    {
        gvResolution.Columns[1].Visible = false;
        gvResolution.Columns[4].Visible = false;
        gvResolution.Columns[5].Visible = false;
        gvResolution.Columns[7].Visible = false;
        gvResolution.Columns[8].Visible = false;
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
            ddlOrg.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----------Select Organization----------", "0"));


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

            }
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        MakeTicket();
    }

    protected void MakeTicket()
    {
        try

        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand("SD_spAddKnowledgeBase", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Issue", txtSummary.Text);
                    cmd.Parameters.AddWithValue("@ResolutionDetail", System.Web.HttpUtility.HtmlEncode(txtDescription.Text).ToString());
                    cmd.Parameters.AddWithValue("@ViewToUser", chkViewToUser.Checked);
                    cmd.Parameters.AddWithValue("@ViewToTech", chkViewToTechnician.Checked);
                    cmd.Parameters.AddWithValue("@KBSubmiitedBy", Session["UserName"].ToString());
                    cmd.Parameters.AddWithValue("@OrgDeskRef", ddlOrg.SelectedValue);
                    cmd.Parameters.Add("@Ticketref", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@Option", "AddKB");
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    string ticketnumber = cmd.Parameters["@Ticketref"].Value.ToString();
                    if (res > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
        $"if (window.location.pathname.endsWith('/frmAddKnowledgeBase.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Saved Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
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
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.AbsoluteUri);
    }
    protected void ShowAddPriorityPanel()
    {
        pnlIncident.Visible = true;
        btnAddPriority.CssClass = "btn btn-sm btn-secondary";
        pnlShowPriority.Visible = false;
        btnViewPriority.CssClass = "btn btn-sm btn-outline-secondary";
        btnAddPriority.Enabled = false;
        btnViewPriority.Enabled = true;
    }
    protected void btnAddPriority_Click(object sender, EventArgs e)
    {
        ShowAddPriorityPanel();
    }

    private void FillShowResolution(string OrgId, string ViewToUser = "", string ViewToTech = "")
    {

        try
        {
            DataTable SD_Priority = new FillSDFields().FillKnowledgeResolution(OrgId, ViewToUser, ViewToTech);
            if (SD_Priority.Rows.Count > 0)
            {
                //  this.lb.Text = dataTable.Rows.Count.ToString();
                this.gvResolution.DataSource = (object)SD_Priority;
                this.gvResolution.DataBind();
                GridFormat(SD_Priority);
            }
            else
            {
                this.gvResolution.DataSource = (object)null;
                this.gvResolution.DataBind();
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
    protected void gvResolution_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "DeleteEx")
            {
                //Determine the RowIndex of the Row whose Button was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                //Get the value of column from the DataKeys using the RowIndex.
                string PriorityRef = gvResolution.DataKeys[rowIndex].Values["ID"].ToString();

                try
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("SD_spAddKnowledgeBase", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ID", PriorityRef);

                            cmd.Parameters.AddWithValue("@Option", "DeleteKB");
                            cmd.CommandTimeout = 180;
                            int res = cmd.ExecuteNonQuery();
                            if (res > 0)
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
        $"if (window.location.pathname.endsWith('/frmAddKnowledgeBase.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Deleted Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                            }
                            con.Close();
                            if (Convert.ToString(Session["UserRole"]).ToUpper() == "ADMIN")
                            {
                                FillShowResolution(ddlOrg.SelectedValue);
                            }
                            else
                            {
                                FillShowResolution("0");
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
            if (e.CommandName == "SelectState")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvResolution.Rows[rowIndex];
                ViewState["PriorityRef"] = gvResolution.DataKeys[rowIndex].Values["ID"].ToString();
                Label OrgID = row.FindControl("lblOrgFk") as Label;
                Label lblDescription = row.FindControl("lblDescription") as Label;
                txtDescription.Text = lblDescription.Text;
                txtSummary.Text = gvResolution.Rows[rowIndex].Cells[2].Text;
                chkViewToUser.Checked = gvResolution.Rows[rowIndex].Cells[4].Text == "1";
                chkViewToTechnician.Checked = gvResolution.Rows[rowIndex].Cells[5].Text == "1";
                btnSubmit.Visible = false;
                btnUpdate.Visible = true;
                ShowAddPriorityPanel();
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
    protected void gvResolution_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (Session["UserScope"].ToString() == "Master")
        {
            e.Row.Cells[6].Visible = true;
            e.Row.Cells[7].Visible = true;
        }

        if (Session["UserScope"].ToString() == "Technician" || Session["UserScope"].ToString() == "Admin")
        {
            e.Row.Cells[6].Visible = true;
            e.Row.Cells[7].Visible = false;

        }
    }
    Random r = new Random();
    protected void btnViewPriority_Click(object sender, EventArgs e)
    {
        try
        {

            pnlIncident.Visible = false;
            btnAddPriority.CssClass = "btn btn-sm btn-outline-secondary";
            pnlShowPriority.Visible = true;
            btnViewPriority.CssClass = "btn btn-sm btn-secondary";
            btnAddPriority.Enabled = true;
            btnViewPriority.Enabled = false;
            if (Convert.ToString(Session["UserRole"]).ToUpper() == "ADMIN")
            {
                FillShowResolution(ddlOrg.SelectedValue);
            }
            else
            {
                FillShowResolution("0");
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
    protected void GridFormat(DataTable dt)
    {
        gvResolution.UseAccessibleHeader = true;
        gvResolution.HeaderRow.TableSection = TableRowSection.TableHeader;

        if (gvResolution.TopPagerRow != null)
        {
            gvResolution.TopPagerRow.TableSection = TableRowSection.TableHeader;
        }
        if (gvResolution.BottomPagerRow != null)
        {
            gvResolution.BottomPagerRow.TableSection = TableRowSection.TableFooter;
        }
        if (dt.Rows.Count > 0)
            gvResolution.FooterRow.TableSection = TableRowSection.TableFooter;
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {

            using (SqlCommand cmd = new SqlCommand("SD_spAddKnowledgeBase", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Issue", txtSummary.Text);
                cmd.Parameters.AddWithValue("@ResolutionDetail", System.Web.HttpUtility.HtmlEncode(txtDescription.Text).ToString());
                cmd.Parameters.AddWithValue("@ViewToUser", chkViewToUser.Checked);
                cmd.Parameters.AddWithValue("@ViewToTech", chkViewToTechnician.Checked);
                cmd.Parameters.AddWithValue("@UpdateBy", Session["UserName"].ToString());
                cmd.Parameters.AddWithValue("@OrgDeskRef", Session["SD_OrgID"].ToString());
                cmd.Parameters.Add("@ID", Convert.ToInt64(ViewState["PriorityRef"]));
                cmd.Parameters.AddWithValue("@Option", "UpdateKB");
                cmd.Parameters.Add("@Ticketref", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                con.Open();
                int res = cmd.ExecuteNonQuery();
                string ticketnumber = cmd.Parameters["@Ticketref"].Value.ToString();
                txtDescription.Text = "";
                txtSummary.Text = "";
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
    $"if (window.location.pathname.endsWith('/frmAddKnowledgeBase.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Updated Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
            }
        }

    }
    protected void ImgBtnExport_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable("GridView_Data");
            foreach (TableCell cell in gvResolution.HeaderRow.Cells)
            {
                dt.Columns.Add(cell.Text);
            }

            // Add rows from GridView
            foreach (GridViewRow row in gvResolution.Rows)
            {
                DataRow dataRow = dt.NewRow();
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    dataRow[i] = row.Cells[i].Text;
                }
                dt.Rows.Add(dataRow);
            }

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt);

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=KnowledgeBase.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                }
            }

            // Complete the request without raising ThreadAbortException
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        catch (Exception ex)
        {
            var st = new StackTrace(ex, true);
            var frame = st.GetFrame(0);
            var line = frame.GetFileLineNumber();

            inEr.InsertErrorLogsF(Session["UserName"].ToString(),
                $"{Request.Url} Got Exception Line Number: {line} {ex}");

            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
                $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
        }
    }

}
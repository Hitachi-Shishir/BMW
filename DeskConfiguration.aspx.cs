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
using System.Drawing;
using DocumentFormat.OpenXml.Office.Word;
using System.Activities.Expressions;
using System.Collections;
using AjaxControlToolkit;

public partial class DeskConfiguration : System.Web.UI.Page
{
    InsertErrorLogs inEr = new InsertErrorLogs();
    Random r = new Random();
    public static Int64 ID;
    public static long OrgID;
    protected override void OnPreInit(EventArgs e)
    {
        if (Session["UserName"] == null || Session["UserScope"] == null)
        {
            Response.Redirect("https://pcv-demo.hitachi-systems-mc.com:5723/Default.aspx");
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //checkPanel();
            if (Session["UserScope"] != null)
            {
                btnDiseable();
                if (!IsPostBack)
                {
                    lblErrorMsg.Text = "";
                    #region Add Orgainisation
                    if ((Session["CurrentStep"] == null) || (Convert.ToString(Session["CurrentStep"]) == "1"))
                    {
                        getOrg();
                        FillOrg();
                    }
                    else
                    {
                        pnlShowOrg.Visible = false;
                    }
                    #endregion Add Orgainisation 
                    CheckStep();
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
            var st = new StackTrace(ex, true);
            var frame = st.GetFrame(0);
            var line = frame.GetFileLineNumber();
            inEr.InsertErrorLogsF(Session["UserName"].ToString()
        , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
        }
    }
    #region Common Start
    public void btnDiseable()
    {
        if (Convert.ToString(Session["OrgNameDesk"]) != "")
        {
            btnInsertOrg.Enabled = false;
            lnkNextAddReq.Enabled = true;
        }
        else
        {
            lnkNextAddReq.Enabled = false;
        }
        if (Convert.ToString(Session["ReqType"]) != "")
        {
            btnSave.Enabled = false;
            lnkNextStage.Enabled = true;
        }
        else
        {
            lnkNextStage.Enabled = false;
        }
        if (Convert.ToString(Session["Stage"]) != "")
        {
            lnkbtnNextStatus.Enabled = true;
        }
        else
        {
            lnkbtnNextStatus.Enabled = false;
        }
        if (Convert.ToString(Session["Status"]) != "")
        {
            lnkNextSeverity.Enabled = true;
        }
        else
        {
            lnkNextSeverity.Enabled = false;
        }
        if (Convert.ToString(Session["Severity"]) != "")
        {
            lnkNextPriority.Enabled = true;
        }
        else
        {
            lnkNextPriority.Enabled = false;
        }
        if (Convert.ToString(Session["Priority"]) != "")
        {
            lnkNextCategory.Enabled = true;
        }
        else
        {
            lnkNextCategory.Enabled = false;
        }
        if (Convert.ToString(Session["Category"]) != "")
        {
            lnkNextEmailConfig.Enabled = true;
        }
        else
        {
            lnkNextEmailConfig.Enabled = false;
        }
        if (Convert.ToString(Session["Resolution"]) != "")
        {
            lnkNextSLA.Enabled = true;
        }
        else
        {
            lnkNextSLA.Enabled = false;
        }
        if (Convert.ToString(Session["SLA"]) != "")
        {
            lnkNextDeskConfig.Enabled = true;
        }
        else
        {
            lnkNextDeskConfig.Enabled = false;
        }
        if (Convert.ToString(Session["Desktemp"]) != "")
        {
            lnkNextCustomFields.Enabled = true;
        }
        else
        {
            lnkNextCustomFields.Enabled = false;
        }
        if (Convert.ToString(Session["EmailConfig"]) != "")
        {
            lnkNextResolution.Enabled = true;
            lnkNextResolutionSKIP.Visible = false;
        }
        else
        {
            lnkNextResolution.Enabled = false;
            lnkNextResolution.Visible = true;
        }
    }
    public void CheckStep()
    {
        if (Session["CurrentStep"] != null)
        {
            if (Convert.ToString(Session["CurrentStep"]) == "1")
            {
                getOrg();
            }
            else if (Convert.ToString(Session["CurrentStep"]) == "2")
            {
                lnkNextAddReq_Click(null, null);
            }
            else if (Convert.ToString(Session["CurrentStep"]) == "3")
            {
                lnkNextStage_Click(null, null);
            }
            else if (Convert.ToString(Session["CurrentStep"]) == "4")
            {
                lnkbtnNextStatus_Click(null, null);
            }
            else if (Convert.ToString(Session["CurrentStep"]) == "5")
            {
                lnkNextSeverity_Click(null, null);
            }
            else if (Convert.ToString(Session["CurrentStep"]) == "6")
            {
                lnkNextPriority_Click(null, null);
            }
            else if (Convert.ToString(Session["CurrentStep"]) == "7")
            {
                lnkNextCategory_Click(null, null);
            }
            else if (Convert.ToString(Session["CurrentStep"]) == "8")
            {
                lnkNextEmailConfig_Click(null, null);
            }
            else if (Convert.ToString(Session["CurrentStep"]) == "9")
            {
                lnkNextSLA_Click(null, null);
            }
            else if (Convert.ToString(Session["CurrentStep"]) == "10")
            {
                lnkNextDeskConfig_Click(null, null);
            }
            else if (Convert.ToString(Session["CurrentStep"]) == "11")
            {
                lnkNextCustomFields_Click(null, null);
            }
            else if (Convert.ToString(Session["CurrentStep"]) == "12")
            {
                lnkNextResolution_Click(null, null);
            }
        }
    }
    public void DataTableScript()
    {
        // Load jQuery first
        string jqueryScript = "<script src='assets/js/jquery-3.6.0.min.js'></script>";

        // Load DataTables JS files
        string dataTableScript = "<script src='assets/plugins/datatable/js/jquery.dataTables.min.js'></script>";
        string dataTableBootstrapScript = "<script src='assets/plugins/datatable/js/dataTables.bootstrap5.min.js'></script>";

        // Load DataTables CSS files for Bootstrap 5
        string dataTableCss = "<link href='assets/plugins/datatable/css/dataTables.bootstrap5.min.css' rel='stylesheet' />";

        // Register CSS
        ClientScript.RegisterStartupScript(this.GetType(), "dataTableCss", dataTableCss, false);

        // Register jQuery and DataTables scripts
        ClientScript.RegisterStartupScript(this.GetType(), "jqueryScript", jqueryScript, false);
        ClientScript.RegisterStartupScript(this.GetType(), "dataTableScript", dataTableScript, false);
        ClientScript.RegisterStartupScript(this.GetType(), "dataTableBootstrapScript", dataTableBootstrapScript, false);

        // DataTable initialization script
        string script = @"
    <script type='text/javascript'>
        $(document).ready(function () {
            $('.data-table1').DataTable({
                'paging': true,
                'ordering': true, // Enable sorting
                'info': true
            });
        });
    </script>";

        // Use ScriptManager for partial postbacks or full postbacks
        ScriptManager.RegisterStartupScript(this, GetType(), "initializeDataTable", script, true);
    }
    public void cleardata()
    {
        txtOrgName.Text = "";
        txtOrgDesc.Text = "";
        txtCntnctPrnsName.Text = "";
        txtCntctPrnsMob.Text = "";
        txtCntctPrsnEmail.Text = "";
        txtCntctPrsnNameII.Text = "";
        txtCntctPrsnMobII.Text = "";
        txtCntctPrnsEmailII.Text = "";
        ddlOrg.ClearSelection();
        ddlOrg2.ClearSelection();
        ddlOrg3.ClearSelection();
        txtRequestType.Text = "";
        txtReqDescription.Text = "";
        ddlRequestType.ClearSelection();
        txtStageName.Text = "";
        txtStageDesc.Text = "";
        ddlRequestTypeStatus.ClearSelection();
        ddlStage.ClearSelection();
        txtStatusName.Text = "";
        txtStatusDesc.Text = "";
        txtColorForStatus.Text = "";
        ddlOrg4.ClearSelection();
        ddlRequestTypeSeverity.ClearSelection();
        txtSeverityName.Text = "";
        txtResponseTime.Text = "";
        txtResolutionTime.Text = "";
        txtSeverityDescription.Text = "";
        ddlOrg5.ClearSelection();
        ddlRequestTypePriority.ClearSelection();
        txtpriority.Text = "";
        txtPriorityDescription.Text = "";
        ddlOrgResolution.ClearSelection();
        ddlRequestTypeResolution.ClearSelection();
        txtResolution.Text = "";
        txtResolutnDesc.Text = "";
        ddlOrgSLA.ClearSelection();
        //txtSLAName.Text = "";
        txtSLADescription.Text = "";
        ddlOrgDeskConfig.ClearSelection();
        ddlRequestTypeDeskConfig.ClearSelection();
        txtSDPrefix.Text = "";
        txtSDDescription.Text = "";
        ddlPriority.ClearSelection();
        ddlCoverageSch.ClearSelection();
        ddlStageDeskConfig.ClearSelection();
        ddlStatus.ClearSelection();
        ddlSlA.ClearSelection();
        ddlCategory1.ClearSelection();
        ddlCategory2.ClearSelection();
        ddlCategory3.ClearSelection();
        ddlCategory4.ClearSelection();
        ddlCategory5.ClearSelection();
        txtArchiveTime.Text = "";
        ddlSeverity.ClearSelection();
        txtHostName.Text = "";
        txtPort.Text = "";
        txtUserName.Text = "";
        txtEmail.Text = "";
        txtRetry.Text = "";
        txtClientID.Text = "";
        txtClientSecretKey.Text = "";
        txtTenantID.Text = "";
        ddlOrgEmailConfig.ClearSelection();
    }
    protected int CurrentStep
    {
        get
        {
            if (ViewState["CurrentStep"] == null)
            {
                ViewState["CurrentStep"] = 1;  // Default to step 1
            }
            return (int)ViewState["CurrentStep"];
        }
        set
        {
            ViewState["CurrentStep"] = value;
        }
    }
    protected void StepButton_Click1(object sender, EventArgs e)
    {
        CurrentStep = 1;
        DataBind();
    }
    protected void StepButton_Click2(object sender, EventArgs e)
    {
        CurrentStep = 2;
        DataBind();
    }
    protected void StepButton_Click3(object sender, EventArgs e)
    {
        CurrentStep = 3;
        // Your logic here
        DataBind();
    }
    protected void StepButton_Click4(object sender, EventArgs e)
    {
        CurrentStep = 4;
        // Your logic here
        DataBind();
    }
    protected void StepButton_Click5(object sender, EventArgs e)
    {
        CurrentStep = 5;
        // Your logic here
        DataBind();
    }
    protected void StepButton_Click6(object sender, EventArgs e)
    {
        CurrentStep = 6;
        // Your logic here
        DataBind();
    }
    protected void StepButton_Click7(object sender, EventArgs e)
    {
        CurrentStep = 7;
        // Your logic here
        DataBind();
    }
    protected void StepButton_Click8(object sender, EventArgs e)
    {
        CurrentStep = 8;
        // Your logic here
        DataBind();
    }
    protected void StepButton_Click9(object sender, EventArgs e)
    {
        CurrentStep = 9;
        // Your logic here
        DataBind();
    }
    protected void StepButton_Click10(object sender, EventArgs e)
    {
        CurrentStep = 10;
        // Your logic here
        DataBind();
    }
    protected void StepButton_Click11(object sender, EventArgs e)
    {
        CurrentStep = 11;
        // Your logic here
        DataBind();
    }
    protected void StepButton_Click12(object sender, EventArgs e)
    {
        CurrentStep = 12;
        // Your logic here
        DataBind();
    }
    protected void StepButton_Click13(object sender, EventArgs e)
    {
        CurrentStep = 13;
        // Your logic here
        DataBind();
    }
    public void checkPanel()
    {
        if (ViewState["CurrentStep"] != null)
        {
            if (Convert.ToString(ViewState["CurrentStep"]) == "1")
            {
                Page_Load(null, null);
            }
            else if (Convert.ToString(ViewState["CurrentStep"]) == "2")
            {
                lnkNextAddReq_Click(null, null);
            }
            else if (Convert.ToString(ViewState["CurrentStep"]) == "3")
            {
                lnkNextStage_Click(null, null);
            }
        }
    }
    #endregion Common End

    #region Start Add Orgainsation
    private void getOrg()
    {
        btnOrg(null, null);
        CurrentStep = 1;
        ViewState["CurrentStep"] = CurrentStep;
        DataBind();
        if (Convert.ToString(Session["OrgNameDesk"]) != "")
        {
            FillOrgDetails(Convert.ToString(Session["OrgNameDesk"]));
        }
    }
    private void FillOrg()
    {
        try
        {
            DataTable SD_Org = new FillSDFields().FillOrganization();
            ddlOrganization.DataSource = SD_Org;
            ddlOrganization.DataTextField = "OrgName";
            ddlOrganization.DataValueField = "Org_ID";
            ddlOrganization.DataBind();
            ddlOrganization.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Organization--", "0"));
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
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                inEr.InsertErrorLogsF(Session["UserName"].ToString()
    , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
            }
        }
    }
    private void FillOrgDetails(string orgid)
    {
        try
        {
            DataTable SD_Org = new FillSDFields().FillOrganization(orgid);
            ViewState["SD_Org"] = SD_Org;
            if (SD_Org.Rows.Count > 0)
            {
                this.gvOrg.DataSource = (object)SD_Org;
                this.gvOrg.DataBind();
            }
            else
            {
                this.gvOrg.DataSource = (object)null;
                this.gvOrg.DataBind();
            }
            if (SD_Org.Rows.Count > 0 && SD_Org != null)
            {
                //GridFormat1(SD_Org);
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
    protected void ImgBtnExport_Click(object sender, EventArgs e)

    {
        try
        {
            DataTable dt = new DataTable("GridView_Data");
            foreach (System.Web.UI.WebControls.TableCell cell in gvOrg.HeaderRow.Cells)
            {
                dt.Columns.Add(cell.Text);
            }
            foreach (GridViewRow row in gvOrg.Rows)
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
                Response.AddHeader("content-disposition", "attachment;filename=Priority.xlsx");
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
    protected void gvOrg_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "DeleteEx")
            {
                //Determine the RowIndex of the Row whose Button was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                //Get the value of column from the DataKeys using the RowIndex.
                long OrgID = Convert.ToInt64(gvOrg.DataKeys[rowIndex].Values["Org_ID"].ToString());
                string Deskref = gvOrg.Rows[rowIndex].Cells[1].Text;
                string PriorityName = gvOrg.Rows[rowIndex].Cells[2].Text;
                try
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("SD_spAddOrganization", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Org_ID", OrgID);
                            cmd.Parameters.AddWithValue("@Option", "DeleteOrg");
                            cmd.CommandTimeout = 180;
                            int res = cmd.ExecuteNonQuery();
                            con.Close();
                            if (res > 0)
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"success_noti('{HttpUtility.JavaScriptStringEncode("Deleted Successfully !")}');", true);
                                lblErrorMsg.Text = "Deleted Successfully!";
                                lblErrorMsg.CssClass = "text-success small fw-bold";
                                //ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"success_noti('{HttpUtility.JavaScriptStringEncode("Deleted Successfully!")}');", true);
                                if (Convert.ToString(Session["OrgNameDesk"]) != "")
                                {
                                    FillOrgDetails(Convert.ToString(Session["OrgNameDesk"]));
                                    DataTable dt = ViewState["SD_Org"] as DataTable;
                                    if (dt.Rows.Count == 0 || dt == null)
                                    {
                                        Session["OrgNameDesk"] = null;
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
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
   $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
                    }
                }

            }
            if (e.CommandName == "SelectState")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                OrgID = Convert.ToInt64(gvOrg.DataKeys[rowIndex].Values["Org_ID"].ToString());
                txtOrgName.Text = gvOrg.Rows[rowIndex].Cells[1].Text;
                txtOrgDesc.Text = gvOrg.Rows[rowIndex].Cells[2].Text;
                txtCntnctPrnsName.Text = gvOrg.Rows[rowIndex].Cells[3].Text;
                txtCntctPrnsMob.Text = gvOrg.Rows[rowIndex].Cells[4].Text;
                txtCntctPrsnEmail.Text = gvOrg.Rows[rowIndex].Cells[5].Text;
                txtCntctPrsnNameII.Text = gvOrg.Rows[rowIndex].Cells[6].Text;
                txtCntctPrsnMobII.Text = gvOrg.Rows[rowIndex].Cells[7].Text;
                txtCntctPrnsEmailII.Text = gvOrg.Rows[rowIndex].Cells[8].Text;
                btnInsertOrg.Visible = false;
                btnUpdateOrg.Visible = true;
                divAddorg.Visible = true;
                divvieworg.Visible = false;
                btnViewOrg.CssClass = "btn btn-sm btn-outline-secondary";
                btnAddOrg.CssClass = "btn btn-sm btn-secondary";
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
        if (Convert.ToString(Session["OrgNameDesk"]) != "")
        {
            FillOrgDetails(Convert.ToString(Session["OrgNameDesk"]));
        }
    }
    protected void SaveData()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand("SD_spAddOrganization", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrgName", txtOrgName.Text.Trim());
                    cmd.Parameters.AddWithValue("@OrgDesc", txtOrgDesc.Text.Trim());
                    cmd.Parameters.AddWithValue("@Org_ID", r.Next());
                    cmd.Parameters.AddWithValue("@CntctPrsnName", txtCntnctPrnsName.Text.Trim());
                    cmd.Parameters.AddWithValue("@CntctPrsnMob", txtCntctPrnsMob.Text.Trim());
                    cmd.Parameters.AddWithValue("@CntctPrsnEmail", txtCntctPrsnEmail.Text);
                    cmd.Parameters.AddWithValue("@CntctPrsnNameII", txtCntctPrsnNameII.Text);
                    cmd.Parameters.AddWithValue("@CntctPrsnMobII", txtCntctPrsnMobII.Text);
                    cmd.Parameters.AddWithValue("@CntctPrsnEmailII", txtCntctPrnsEmailII.Text);
                    cmd.Parameters.Add("@Error", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@Option", "AddOrg");
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    string ErrorChk = cmd.Parameters["@Error"].Value.ToString();
                    if (res > 0)
                    {
                        string sql = "select Org_ID from SD_OrgMaster where OrgName='" + txtOrgName.Text.Trim() + "'";
                        Session["OrgNameDesk"] = Convert.ToString(database.GetScalarValue(sql));
                        Session["OrgNameDeskName"] = txtOrgDesc.Text.Trim();
                        FillOrg();

                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Saved Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                        lblErrorMsg.Text = "Saved Successfully!";
                        lblErrorMsg.CssClass = "text-success small fw-bold";
                        //ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"success_noti('{HttpUtility.JavaScriptStringEncode("Saved Successfully!")}');", true);
                        cleardata();
                        lnkNextAddReq_Click(null, null);
                        if (Convert.ToString(Session["OrgNameDesk"]) != "")
                        {
                            FillOrgDetails(Convert.ToString(Session["OrgNameDesk"]));
                        }

                        btnInsertOrg.Enabled = false;
                        divAddorg.Visible = false;
                        divvieworg.Visible = true;
                        btnViewOrg.CssClass = "btn btn-sm btn-secondary";
                        btnAddOrg.CssClass = "btn btn-sm btn-outline-secondary";

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ warning_noti('{HttpUtility.JavaScriptStringEncode("" + ErrorChk + "")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                        lblErrorMsg.Text = ErrorChk;
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
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                inEr.InsertErrorLogsF(Session["UserName"].ToString()
    , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
   $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
            }
        }
    }
    protected void btnInsertOrg_Click(object sender, EventArgs e)
    {
        if (txtCntnctPrnsName.Text.Trim().ToLower() == txtCntctPrsnNameII.Text.Trim().ToLower())
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"warning_noti('{HttpUtility.JavaScriptStringEncode("Contact Person Name and Contact PersonII Name could not be same !")}');", true);
            return;
        }
        if (txtCntctPrnsMob.Text.Trim().ToLower() == txtCntctPrsnMobII.Text.Trim().ToLower())
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"warning_noti('{HttpUtility.JavaScriptStringEncode("Contact Person Mobile and Contact PersonII Mobile could not be same !")}');", true);
            return;
        }
        if (txtCntctPrsnEmail.Text.Trim().ToLower() == txtCntctPrnsEmailII.Text.Trim().ToLower())
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"warning_noti('{HttpUtility.JavaScriptStringEncode("Contact Person Email and Contact PersonII Email could not be same !")}');", true);
            return;
        }
        SaveData();
    }
    protected void gvOrg_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (Session["UserScope"].ToString() == "Master")
            {
                e.Row.Cells[4].Visible = true;
                e.Row.Cells[5].Visible = true;
            }

            if (Session["UserScope"].ToString() == "Technician")
            {
                e.Row.Cells[4].Visible = true;
                e.Row.Cells[5].Visible = false;

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
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.AbsoluteUri);
    }
    protected void btnOrg(object sender, EventArgs e)
    {
        pnlShowOrg.Visible = true;
    }
    protected void btnUpdateOrg_Click(object sender, EventArgs e)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SD_spAddOrganization", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrgName", txtOrgName.Text.Trim());
                    cmd.Parameters.AddWithValue("@OrgDesc", txtOrgDesc.Text.Trim());
                    cmd.Parameters.AddWithValue("@Org_ID", OrgID);
                    cmd.Parameters.AddWithValue("@CntctPrsnName", txtCntnctPrnsName.Text.Trim());
                    cmd.Parameters.AddWithValue("@CntctPrsnMob", txtCntctPrnsMob.Text.Trim());
                    cmd.Parameters.AddWithValue("@CntctPrsnEmail", txtCntctPrsnEmail.Text);
                    cmd.Parameters.AddWithValue("@CntctPrsnNameII", txtCntctPrsnNameII.Text);
                    cmd.Parameters.AddWithValue("@CntctPrsnMobII", txtCntctPrsnMobII.Text);
                    cmd.Parameters.AddWithValue("@CntctPrsnEmailII", txtCntctPrnsEmailII.Text);
                    cmd.Parameters.AddWithValue("@Option", "UpdateOrg");
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        ViewState["data"] = 1;
                        //ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Saved Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                        //ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"success_noti('{HttpUtility.JavaScriptStringEncode("Updated Successfully!")}');", true);
                        lblErrorMsg.Text = "Organization has been Updated Successfully !";
                        lblErrorMsg.CssClass = "text-success small fw-bold";
                        if (Convert.ToString(Session["OrgNameDesk"]) != "")
                        {
                            FillOrgDetails(Convert.ToString(Session["OrgNameDesk"]));
                        }
                        cleardata();
                        divAddorg.Visible = false;
                        divvieworg.Visible = true;
                        btnViewOrg.CssClass = "btn btn-sm btn-secondary";
                        btnAddOrg.CssClass = "btn btn-sm btn-outline-secondary";

                    }
                    else
                    {
                        lblErrorMsg.Text = "Something Went Wrong !";
                        lblErrorMsg.CssClass = "text-danger small fw-bold";
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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
   $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }
    }
    protected void lnkNextAddReq_Click(object sender, EventArgs e)
    {
        if (Session["UserScope"] != null)
        {
            CurrentStep = 2;
            Session["CurrentStep"] = CurrentStep;
            ViewState["CurrentStep"] = CurrentStep;
            DataBind();
            cleardata();
            lblErrorMsg.Text = "";
            pnlReqType.Visible = true;
            pnlShowOrg.Visible = false;
            if (Convert.ToString(Session["chngtyp"]) != "" && Convert.ToString(Session["resonforchng"]) != "")
            {
                btnSave.Enabled = true;
            }
            if (Convert.ToString(Session["ReqType"]) != "")
            {
                FillRequestTypeDetails(Convert.ToString(Session["OrgNameDesk"]), Convert.ToString(Session["ReqType"]));
            }
            FillOrganization1();
            try
            {
                ddlOrg.SelectedValue = Convert.ToString(Session["OrgNameDesk"]);
            }
            catch { }
        }
        else
        {
            Response.Redirect("/Default.aspx");
        }
    }
    protected void GridFormat1(DataTable dt)

    {
        gvOrg.UseAccessibleHeader = true;
        gvOrg.HeaderRow.TableSection = TableRowSection.TableHeader;

        if (gvOrg.TopPagerRow != null)
        {
            gvOrg.TopPagerRow.TableSection = TableRowSection.TableHeader;
        }
        if (gvOrg.BottomPagerRow != null)
        {
            gvOrg.BottomPagerRow.TableSection = TableRowSection.TableFooter;
        }
        if (dt.Rows.Count > 0)
            gvOrg.FooterRow.TableSection = TableRowSection.TableFooter;
    }
    private void RegisterDataTableScripts()
    {
        string script = @"
        <script src='https://pcv-demo.hitachi-systems-mc.com:5723/assetsdata/plugins/datatable/js/jquery.dataTables.min.js'></script>
        <script src='https://pcv-demo.hitachi-systems-mc.com:5723/assetsdata/plugins/datatable/js/dataTables.bootstrap5.min.js'></script>
        <script src='https://pcv-demo.hitachi-systems-mc.com:5723/assetsdata/js/jquery-3.6.0.min.js'></script>
        <script>
            $(document).ready(function () {
                $('.data-table').each(function () {
                    if ($.fn.DataTable.isDataTable(this)) {
                        $(this).DataTable().clear().destroy();
                    }
                    $(this).DataTable({
                        lengthChange: false,
                        dom: 'Bfrtip',
                        buttons: [
                            'copy', 'excel', 'pdf', 'print'
                        ]
                    });
                });
            });
        </script>
        <link href='https://pcv-demo.hitachi-systems-mc.com:5723/assetsdata/plugins/datatable/css/dataTables.bootstrap5.min.css' rel='stylesheet' />
    ";

        ClientScript.RegisterStartupScript(this.GetType(), "DataTableScript", script, false);
    }
    protected void btnAddOrg_Click(object sender, EventArgs e)
    {
        divAddorg.Visible = true;
        divvieworg.Visible = false;
        btnAddOrg.CssClass = "btn btn-sm btn-secondary";
        btnViewOrg.CssClass = "btn btn-sm btn-outline-secondary";
    }
    protected void btnViewOrg_Click(object sender, EventArgs e)
    {
        divAddorg.Visible = false;
        divvieworg.Visible = true;
        btnViewOrg.CssClass = "btn btn-sm btn-secondary";
        btnAddOrg.CssClass = "btn btn-sm btn-outline-secondary";

    }
    protected void btnChoose_Click(object sender, EventArgs e)
    {
        if (ddlOrganization.SelectedValue != "0")
        {
            Session["OrgNameDesk"] = ddlOrganization.SelectedValue;
            Session["OrgNameDeskName"] = ddlOrganization.SelectedItem.Text;
            cleardata();
            lnkNextAddReq_Click(null, null);
            if (Convert.ToString(Session["OrgNameDesk"]) != "")
            {
                FillOrgDetails(Convert.ToString(Session["OrgNameDesk"]));
                //lblErrorMsg.Text = "Organization has been Selected Successfully !";
                //lblErrorMsg.CssClass = "text-success small fw-bold";
            }
        }
        else
        {
            //lblErrorMsg.Text = "Please Select Organization !";
            //lblErrorMsg.CssClass = "text-danger small fw-bold";
        }

        btnInsertOrg.Enabled = false;
        divAddorg.Visible = false;
        divvieworg.Visible = true;
        btnViewOrg.CssClass = "btn btn-sm btn-secondary";
        btnAddOrg.CssClass = "btn btn-sm btn-outline-secondary";

    }
    #endregion End Add Orgainsation

    #region Add Request Type Start
    private void FillOrganization1()
    {
        try
        {

            DataTable SD_Org = new FillSDFields().FillOrganization();
            ddlOrg.DataSource = SD_Org;
            ddlOrg.DataTextField = "OrgName";
            ddlOrg.DataValueField = "Org_ID";
            ddlOrg.DataBind();
            ddlOrg.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select Organization--", "0"));
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
    private void FillRequestTypeDetails(string orgid, string req)
    {
        try
        {
            DataTable SD_ReqType = new FillSDFields().FillRequestType(orgid, req);
            ViewState["Req_Type"] = SD_ReqType;
            if (SD_ReqType.Rows.Count > 0)
            {
                this.gvReqType.DataSource = (object)SD_ReqType;
                this.gvReqType.DataBind();
            }
            else
            {
                this.gvReqType.DataSource = (object)null;
                this.gvReqType.DataBind();
            }
            //GridFormat2(SD_ReqType);
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
    protected void CheckBox_CheckedChanged(object sender, EventArgs e)
    {
        var currentCheckBox = sender as CheckBox;
        if (currentCheckBox == chkUserWise && chkUserWise.Checked)
        {
            chkManualWise.Checked = false;
        }
        else if (currentCheckBox == chkManualWise && chkManualWise.Checked)
        {
            chkUserWise.Checked = false;
        }
    }
    protected void SaveDataReqType()
    {
        try
        {
            lblErrorMsg.Text = "";

            if (txtRequestType.Visible == false)
            {
                if (ddlReq.SelectedValue == "0")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select Request Type.');window.location ='DeskConfiguration.aspx';", true);
                    return;
                }
            }
            bool userwise = false;
            bool Manualwise = false;
            if (chkUserWise.Checked)
            {
                userwise = true;
            }
            else
            {
                Manualwise = true;
            }
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SD_spRequestType", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", r.Next());
                    if (ddlReq.SelectedValue != "Other")
                    {
                        cmd.Parameters.AddWithValue("@ReqTypeRef ", ddlReq.SelectedValue);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@ReqTypeRef ", txtRequestType.Text);
                    }
                    cmd.Parameters.AddWithValue("@ReqTypeDef", txtReqDescription.Text);
                    cmd.Parameters.AddWithValue("@UserWise", userwise);
                    cmd.Parameters.AddWithValue("@ManualWise", Manualwise);
                    cmd.Parameters.AddWithValue("@OrgRef", ddlOrg.SelectedValue.ToString());
                    cmd.Parameters.Add("@Error", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@Option", "AddRequestType");
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    string ErrorChk = cmd.Parameters["@Error"].Value.ToString();
                    if (res > 0)
                    {
                        string reqtyp = "";
                        if (ddlReq.SelectedValue != "Other")
                        {
                            reqtyp = ddlReq.SelectedValue;
                        }
                        else
                        {
                            reqtyp = txtRequestType.Text;
                        }
                        Session["ReqType"] = reqtyp;
                        string checkcustforcurrentorg = @"if exists(select * from SD_CustomFieldControl where OrgRef='" + ddlOrg.SelectedValue + "' )begin select 'true' end";
                        if (ddlReq.SelectedValue == "Incident")
                        {
                            string[] arr = { "OpenEnd", "WIPStart", "WIPEnd", "HoldStart", "HoldEnd" };
                            for (int j = 0; j < arr.Length; j++)
                            {
                                SaveDefaultCustomFields(arr[j]);
                            }
                        }
                        cleardata();
                        if (Convert.ToString(Session["ReqType"]) != "")
                        {
                            FillRequestTypeDetails(Convert.ToString(Session["OrgNameDesk"]), Convert.ToString(Session["ReqType"]));
                        }
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Saved Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                        lnkNextStage_Click(null, null);
                    }
                    else
                    {
                        lblErrorMsg.Text = ErrorChk;
                        lblErrorMsg.CssClass = "text-danger small fw-bold";
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ warning_noti('{HttpUtility.JavaScriptStringEncode("" + ErrorChk + "")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                        //ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ warning_noti('{HttpUtility.JavaScriptStringEncode("" + ErrorChk + "")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
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
    public void SaveDefaultCustomFields(string FieldName)
    {
        Random rand = new Random();
        string id = rand.Next(100000, 999999).ToString();
        string txtboxid = "txt" + id;
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand("SD_spCustomFieldCntl", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@Deskref", "Incident");
                cmd.Parameters.AddWithValue("@FieldID", txtboxid);
                cmd.Parameters.AddWithValue("@FieldName", FieldName);
                cmd.Parameters.AddWithValue("@FieldValue", FieldName);
                cmd.Parameters.AddWithValue("@FieldType", "TextBox");
                cmd.Parameters.AddWithValue("@FieldMode", "DATETIME");
                cmd.Parameters.AddWithValue("@Status", "1");
                cmd.Parameters.AddWithValue("@IsFieldReq", "1");
                cmd.Parameters.AddWithValue("@FieldScope", "ForTechnician");
                cmd.Parameters.AddWithValue("@OrgRef", ddlOrg.SelectedValue.ToString());
                cmd.Parameters.AddWithValue("@Option", "AddCustomField");
                con.Open();
                int res = cmd.ExecuteNonQuery();
            }
        }
    }
    protected void btnSaveReqType_Click(object sender, EventArgs e)
    {
        SaveDataReqType();
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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
            }
        }
    }
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
                    using (SqlCommand cmd = new SqlCommand("SD_spRequestType", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", ID);
                        cmd.Parameters.AddWithValue("@ReqTypeRef", ReqTypeRef);
                        cmd.Parameters.AddWithValue("@ReqTypeDef", ReqTypeDef);
                        cmd.Parameters.AddWithValue("@OrgRef", ddlOrg.SelectedValue.ToString());
                        cmd.Parameters.AddWithValue("@Option", "DelRequestType");
                        cmd.CommandTimeout = 180;
                        int res = cmd.ExecuteNonQuery();
                        if (res > 0)
                        {
                            //ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Deleted Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                            //ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"success_noti('{HttpUtility.JavaScriptStringEncode("Deleted Successfully!")}');", true);
                            lblErrorMsg.Text = "Deleted Successfully!";
                            lblErrorMsg.CssClass = "text-success small fw-bold";
                        }
                        con.Close();
                        if (Convert.ToString(Session["ReqType"]) != "")
                        {
                            FillRequestTypeDetails(Convert.ToString(Session["OrgNameDesk"]), Convert.ToString(Session["ReqType"]));
                            DataTable dt = ViewState["Req_Type"] as DataTable;
                            if (dt.Rows.Count == 0 || dt == null)
                            {
                                Session["ReqType"] = null;
                            }
                        }
                    }
                }
            }
            if (e.CommandName == "SelectState")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvReqType.Rows[rowIndex];
                //Get the value of column from the DataKeys using the RowIndex.
                ID = Convert.ToInt32(gvReqType.DataKeys[rowIndex].Values["ID"]);
                txtRequestType.Text = gvReqType.Rows[rowIndex].Cells[1].Text;
                txtReqDescription.Text = gvReqType.Rows[rowIndex].Cells[2].Text;
                Label OrgID = (row.FindControl("lblOrgFk") as Label);
                if (ddlOrg.Items.FindByValue(OrgID.Text.ToString().Trim()) != null)
                {
                    ddlOrg.SelectedValue = OrgID.Text;
                }
                Label usrwise = (row.FindControl("lblusrwise") as Label);
                if (usrwise.Text.ToString().ToLower() == "true")
                {
                    chkUserWise.Checked = true;
                }
                Label mnlwise = (row.FindControl("lblmnlwise") as Label);
                if (mnlwise.Text.ToString().ToLower() == "true")
                {
                    chkManualWise.Checked = true;
                }
                btnSave.Visible = false;
                btnUpdateReqType.Visible = true;
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
    protected void btnUpdateReqType_Click(object sender, EventArgs e)
    {
        try
        {
            bool userwise = false;
            bool Manualwise = false;
            if (chkUserWise.Checked)
            {
                userwise = true;
            }
            else
            {
                Manualwise = true;
            }
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SD_spRequestType", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters.AddWithValue("@ReqTypeRef", txtRequestType.Text);
                    cmd.Parameters.AddWithValue("@ReqTypeDef", txtReqDescription.Text);
                    cmd.Parameters.AddWithValue("@OrgRef", ddlOrg.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@UserWise", userwise);
                    cmd.Parameters.AddWithValue("@ManualWise", Manualwise);
                    cmd.Parameters.AddWithValue("@Option", "UpdateRequestType");
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        //ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"success_noti('{HttpUtility.JavaScriptStringEncode("Updated Successfully!")}');", true);
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Saved Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                        if (Convert.ToString(Session["ReqType"]) != "")
                        {
                            FillRequestTypeDetails(Convert.ToString(Session["OrgNameDesk"]), Convert.ToString(Session["ReqType"]));
                        }
                        //ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Saved Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                        cleardata();
                        btnSave.Visible = true;
                        btnUpdateReqType.Visible = false;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ warning_noti('{HttpUtility.JavaScriptStringEncode("Already Exists !")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
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
    protected void ImgBtnExportReq_Click(object sender, EventArgs e)
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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
            }
        }
    }
    protected void btnCancel1_Click(object sender, EventArgs e)
    {
        Response.Redirect("/DeskConfiguration.aspx");
    }
    protected void lnkPrevOrg_Click(object sender, EventArgs e)
    {
        cleardata();
        stepper1trigger2.Enabled = true;
        pnlReqType.Visible = false;
        CurrentStep = 1;
        Session["CurrentStep"] = CurrentStep;
        DataBind();
        getOrg();
        FillOrg();
        ddlOrganization.SelectedValue = Convert.ToString(Session["OrgNameDesk"]);
    }
    protected void lnkNextStage_Click(object sender, EventArgs e)
    {
        lblErrorMsg.Text = "";
        cleardata();
        CurrentStep = 3;
        Session["CurrentStep"] = CurrentStep;
        DataBind();
        pnlReqType.Visible = false;
        pnlAddStage.Visible = true;
        FillOrganization();
        if (Session["UserScope"] == null)
        {
            Response.Redirect("https://pcv-demo.hitachi-systems-mc.com:5723/Default.aspx");
        }
        if (Session["UserScope"].ToString().ToLower() == "admin" || Session["UserScope"].ToString().ToLower() == "technician")
        {
            FillStageDetailsCustomer(Session["OrgNameDesk"].ToString());
            ddlOrg2.Enabled = false;
            ddlOrg2.SelectedValue = Session["OrgNameDesk"].ToString();
            ddlRequestType.SelectedValue = Convert.ToString(Session["ReqType"]);
        }
        else
        {
            ddlOrg.Enabled = true;
            FillStageDetailsCustomer(Session["OrgNameDesk"].ToString());
            try
            {
                //ddlOrg2_SelectedIndexChanged(null, null);

                ddlOrg2.SelectedValue = Convert.ToString(Session["OrgNameDesk"]);
                FillRequestType(Convert.ToInt64(ddlOrg2.SelectedValue));
                ddlRequestType.SelectedValue = Convert.ToString(Session["ReqType"]);

            }
            catch { }
        }
    }
    protected void GridFormat2(DataTable dt)
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
    protected void ddlReq_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Convert.ToString(ddlReq.SelectedValue) == "Other")
        {
            txtRequestType.Visible = true;
            rfvReqType.Visible = true;
            ddlReq.Visible = false;
            rfvddlReq.Visible = false;
        }
        else
        {
            txtRequestType.Visible = false;
            rfvReqType.Visible = false;
            ddlReq.Visible = true;
            rfvddlReq.Visible = true;
        }
        if (Convert.ToString(ddlReq.SelectedValue) == "Change Request")
        {
            btnAddChangeType.Visible = true;
            btnAddReasonForChng.Visible = true;
            btnSave.Enabled = false;
            if (Convert.ToString(Session["chngtyp"]) != "" && Convert.ToString(Session["resonforchng"]) != "")
            {
                btnSave.Enabled = true;
            }
        }
        else
        {
            btnAddChangeType.Visible = false;
            btnAddReasonForChng.Visible = false;
            btnSave.Enabled = true;
        }
        if (Convert.ToString(ddlReq.SelectedValue) == "Service Request")
        {
            divapr.Visible = true;
            divAppr.Visible = true;
        }
        else
        {
            divapr.Visible = false;
        }
        if (Convert.ToString(ddlReq.SelectedValue) == "Service Request" || Convert.ToString(ddlReq.SelectedValue) == "Change Request" || Convert.ToString(ddlReq.SelectedValue) == "Problem Management")
        {
            divAppr.Visible = true;
        }
        else
        {
            divAppr.Visible = false;
        }
    }
    protected void btnAddChangeType_Click(object sender, EventArgs e)
    {
        Response.Write("<script type='text/javascript'>");
        Response.Write("window.open('/ChangeManagement/frmAddChangeType.aspx?OrgId=" + Convert.ToString(Session["OrgNameDesk"]) + "','_blank');");
        Response.Write("</script>");
    }
    protected void btnAddReasonForChng_Click(object sender, EventArgs e)
    {
        Response.Write("<script type='text/javascript'>");
        Response.Write("window.open('/ChangeManagement/frmReasonForChng.aspx?OrgId=" + Convert.ToString(Session["OrgNameDesk"]) + "','_blank');");
        Response.Write("</script>");
    }
    #endregion Add Request Type End

    #region Add Stage Start
    private void FillStageDetailsCustomer(string OrgId)
    {
        try
        {
            DataTable SD_Stage = new FillSDFields().FillStageCustomer(OrgId);
            if (SD_Stage.Rows.Count > 0)
            {
                this.gvStage.DataSource = (object)SD_Stage;
                this.gvStage.DataBind();
            }
            else
            {
                this.gvStage.DataSource = (object)null;
                this.gvStage.DataBind();
            }
            if (SD_Stage.Rows.Count > 0)
            {
                //GridFormat3(SD_Stage);
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
    protected void GridFormat3(DataTable dt)
    {
        gvStage.UseAccessibleHeader = true;
        gvStage.HeaderRow.TableSection = TableRowSection.TableHeader;

        if (gvStage.TopPagerRow != null)
        {
            gvStage.TopPagerRow.TableSection = TableRowSection.TableHeader;
        }
        if (gvStage.BottomPagerRow != null)
        {
            gvStage.BottomPagerRow.TableSection = TableRowSection.TableFooter;
        }
        if (dt.Rows.Count > 0)
            gvStage.FooterRow.TableSection = TableRowSection.TableFooter;
    }
    private void FillOrganization()
    {
        try
        {
            DataTable SD_Org = new FillSDFields().FillOrganization();
            ddlOrg2.DataSource = SD_Org;
            ddlOrg2.DataTextField = "OrgName";
            ddlOrg2.DataValueField = "Org_ID";
            ddlOrg2.DataBind();
            ddlOrg2.Items.Insert(0, new System.Web.UI.WebControls.ListItem("   -Select Organization-", "0"));
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
    private void FillStageDetails()
    {
        try
        {
            DataTable SD_Stage = new FillSDFields().FillStage();
            if (SD_Stage.Rows.Count > 0)
            {
                //  this.lb.Text = dataTable.Rows.Count.ToString();
                this.gvStage.DataSource = (object)SD_Stage;
                this.gvStage.DataBind();
            }
            else
            {
                this.gvStage.DataSource = (object)null;
                this.gvStage.DataBind();
            }
            //GridFormat3(SD_Stage);
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
        Session["SDRef"] = ddlRequestType.SelectedValue.ToString();
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
            ddlRequestType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("   -Select RequestType-", "0"));

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
    protected void SaveDataSatge()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SD_spAddStage", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", r.Next());
                    cmd.Parameters.AddWithValue("@DeskRef", ddlRequestType.SelectedValue);
                    cmd.Parameters.AddWithValue("@StageRef", ddlRequestType.SelectedValue + "||" + txtStageName.Text.Trim());
                    cmd.Parameters.AddWithValue("@StageCodeRef", txtStageName.Text.Trim());
                    cmd.Parameters.AddWithValue("@StageDesc", txtStageDesc.Text);
                    cmd.Parameters.AddWithValue("@OrgDeskRef", ddlOrg2.SelectedValue.ToString());
                    cmd.Parameters.Add("@Error", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@Option", "AddStage");
                    con.Open();
                    int cnt = cmd.ExecuteNonQuery();
                    string ErrorChk = cmd.Parameters["@Error"].Value.ToString();
                    if (ErrorChk == "")
                    {
                        Session["Stage"] = "Yes";
                        cleardata();
                        FillRequestType(Convert.ToInt64(ddlOrg2.SelectedValue));
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"success_noti('{HttpUtility.JavaScriptStringEncode("Saved Successfully!")}');", true);
                        Response.Redirect("/DeskConfiguration.aspx");
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ warning_noti('{HttpUtility.JavaScriptStringEncode("" + ErrorChk + "")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                    }
                }
            }
            //ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Saved Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);


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
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                inEr.InsertErrorLogsF(Session["UserName"].ToString()
    , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }
    }
    protected void btnInsertStage_Click(object sender, EventArgs e)
    {
        SaveDataSatge();
    }
    protected void gvStage_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "DeleteEx")
            {
                //Determine the RowIndex of the Row whose Button was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                //Get the value of column from the DataKeys using the RowIndex.
                ID = Convert.ToInt64(gvStage.DataKeys[rowIndex].Values["ID"]);
                string Deskref = gvStage.Rows[rowIndex].Cells[1].Text;
                string SeverityName = gvStage.Rows[rowIndex].Cells[2].Text;
                try
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("SD_spAddStage", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ID", ID);
                            cmd.Parameters.AddWithValue("@DeskRef", Deskref);
                            cmd.Parameters.AddWithValue("@Option", "DeleteStage");
                            cmd.CommandTimeout = 180;
                            int res = cmd.ExecuteNonQuery();
                            if (res > 0)
                            {

                                Session["Popup"] = "Delete";
                                lblErrorMsg.Text = "Deleted Successfully !";
                                lblErrorMsg.CssClass = "text-success small fw-bold";
                                //ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Updated Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"success_noti('{HttpUtility.JavaScriptStringEncode("Deleted Successfully!")}');", true);
                                cleardata();
                            }
                            con.Close();
                            //FillStageDetails();
                            FillStageDetailsCustomer(Session["OrgNameDesk"].ToString());
                            ddlOrg2.SelectedValue = Session["OrgNameDesk"].ToString();
                            ddlRequestType.SelectedValue = Convert.ToString(Session["ReqType"]);
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
            if (e.CommandName == "SelectStage")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                ID = Convert.ToInt64(gvStage.DataKeys[rowIndex].Values["ID"]);
                GridViewRow row = gvStage.Rows[rowIndex];
                Label OrgID = (row.FindControl("lblOrgFk") as Label);
                if (ddlOrg2.Items.FindByValue(OrgID.Text.ToString().Trim()) != null)
                {
                    ddlOrg2.SelectedValue = OrgID.Text;
                    FillRequestType(Convert.ToInt64(OrgID.Text));
                }
                ddlRequestType.SelectedValue = gvStage.Rows[rowIndex].Cells[1].Text;
                txtStageName.Text = gvStage.Rows[rowIndex].Cells[2].Text;
                txtStageDesc.Text = gvStage.Rows[rowIndex].Cells[3].Text;


                btnInsertStage.Visible = false;
                btnUpdateStage.Visible = true;
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
    protected void ImgBtnExport2_Click(object sender, EventArgs e)
    {
        try
        {
            using (XLWorkbook wb = new XLWorkbook())
            {
                // Extract data from the first GridView and add it to the workbook
                DataTable dt1 = ExtractDataFromGridView(gvStage, "GridView_Data");
                var ws1 = wb.Worksheets.Add(dt1, "SD_Stage");
                ApplyStylesFromGridView(gvStage, ws1, dt1);


                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=SD_Stage.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }
        catch (Exception ex)
        {

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
    protected void gvStage_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowIndex >= 0)
            {
            }
            if (Session["UserScope"].ToString() == "Master")
            {
                e.Row.Cells[5].Visible = true;
                e.Row.Cells[6].Visible = true;
            }

            if (Session["UserScope"].ToString() == "Technician" || Session["UserScope"].ToString() == "Admin")
            {
                e.Row.Cells[5].Visible = true;
                e.Row.Cells[6].Visible = false;

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
    protected void btnUpdateStage_Click(object sender, EventArgs e)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand("SD_spAddStage", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters.AddWithValue("@DeskRef", ddlRequestType.SelectedValue);
                    cmd.Parameters.AddWithValue("@StageRef", ddlRequestType.SelectedValue + "||" + txtStageName.Text.Trim());
                    cmd.Parameters.AddWithValue("@StageCodeRef", txtStageName.Text.Trim());
                    cmd.Parameters.AddWithValue("@StageDesc", txtStageDesc.Text);
                    cmd.Parameters.AddWithValue("@OrgDeskRef", ddlOrg2.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@Option", "UpdateStage");
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        cleardata();
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Updated Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                        //ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"success_noti('{HttpUtility.JavaScriptStringEncode("Updated Successfully!")}');", true);
                        //FillStageDetails();
                        FillStageDetailsCustomer(Session["OrgNameDesk"].ToString());
                        ddlOrg2.SelectedValue = Session["OrgNameDesk"].ToString();
                        ddlRequestType.SelectedValue = Convert.ToString(Session["ReqType"]);
                        btnInsertStage.Visible = true;
                        btnUpdateStage.Visible = false;
                        Response.Redirect("/DeskConfiguration.aspx");
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ warning_noti('{HttpUtility.JavaScriptStringEncode("Already Exists !")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
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
    protected void btnCancel2_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.AbsoluteUri);
    }
    protected void ddlOrg2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillRequestType(Convert.ToInt64(ddlOrg2.SelectedValue));
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
    protected void lnkbtnPrevAddReq_Click(object sender, EventArgs e)
    {
        CurrentStep = 2;
        Session["CurrentStep"] = CurrentStep;
        DataBind();
        cleardata();
        pnlReqType.Visible = true;
        pnlAddStage.Visible = false;
        lnkNextAddReq_Click(null, null);
        if (Convert.ToString(Session["ReqType"]) != "Incident" && Convert.ToString(Session["ReqType"]) != "Service Request"
            && Convert.ToString(Session["ReqType"]) != "Change Request" && Convert.ToString(Session["ReqType"]) != "Knowledge Base"
            && Convert.ToString(Session["ReqType"]) != "Problem Management")
        {
            txtRequestType.Visible = true;
            rfvReqType.Visible = true;
            ddlReq.Visible = false;
            rfvddlReq.Visible = false;
            txtRequestType.Text = Convert.ToString(Session["ReqType"]);
        }
        else
        {
            txtRequestType.Visible = false;
            rfvReqType.Visible = false;
            ddlReq.Visible = true;
            rfvddlReq.Visible = true;
            ddlReq.SelectedValue = Convert.ToString(Session["ReqType"]);
        }
    }
    protected void lnkbtnNextStatus_Click(object sender, EventArgs e)
    {
        if (Session["UserScope"] != null)
        {
            try
            {
                CurrentStep = 4;
                Session["CurrentStep"] = CurrentStep;
                ViewState["CurrentStep"] = CurrentStep;
                DataBind();
                cleardata();
                FillStatusDetails(Convert.ToString(Session["OrgNameDesk"]), Convert.ToString(Session["ReqType"]));
                FillOrganizationStatus();
                pnlStatus.Visible = true;
                pnlAddStage.Visible = false;
                ddlOrg3.SelectedValue = Convert.ToString(Session["OrgNameDesk"]);
                FillRequestTypeStatus(Convert.ToInt64(ddlOrg3.SelectedValue));
                ddlRequestTypeStatus.SelectedValue = Convert.ToString(Session["ReqType"]);
                FillStage();
                FillStatusDetails(Convert.ToString(Session["OrgNameDesk"]), Convert.ToString(Session["ReqType"]));
            }
            catch { }
        }
        else
        {
            Response.Redirect("https://pcv-demo.hitachi-systems-mc.com:5723/Default.aspx");
        }
    }
    #endregion Stage End

    #region Add Status Start 
    protected void GridFormat4(DataTable dt)
    {
        gvStatus.UseAccessibleHeader = true;
        gvStatus.HeaderRow.TableSection = TableRowSection.TableHeader;

        if (gvStatus.TopPagerRow != null)
        {
            gvStatus.TopPagerRow.TableSection = TableRowSection.TableHeader;
        }
        if (gvStatus.BottomPagerRow != null)
        {
            gvStatus.BottomPagerRow.TableSection = TableRowSection.TableFooter;
        }
        if (dt.Rows.Count > 0)
            gvStatus.FooterRow.TableSection = TableRowSection.TableFooter;
    }
    private void FillOrganizationStatus()
    {

        try
        {

            DataTable SD_Org = new FillSDFields().FillOrganization();
            ddlOrg3.DataSource = SD_Org;
            ddlOrg3.DataTextField = "OrgName";
            ddlOrg3.DataValueField = "Org_ID";
            ddlOrg3.DataBind();
            ddlOrg3.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" -Select Organization-", "0"));
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
    private void FillStatusDetails(string orgid = "0", string reqtype = "0")
    {
        try
        {
            DataTable SD_Status = new FillSDFields().FillStatus(Convert.ToInt64(orgid), reqtype);
            if (SD_Status.Rows.Count > 0)
            {
                //  this.lb.Text = dataTable.Rows.Count.ToString();
                this.gvStatus.DataSource = (object)SD_Status;
                this.gvStatus.DataBind();
            }
            else
            {
                this.gvStatus.DataSource = (object)null;
                this.gvStatus.DataBind();
            }
            //GridFormat4(SD_Status);
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
    protected void ddlRequestTypeStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["SDRef"] = ddlRequestTypeStatus.SelectedValue.ToString();
        FillStage();
        FillStatusDetails(Convert.ToString(Session["OrgNameDesk"]), Convert.ToString(Session["ReqType"]));
    }
    private void FillStage()
    {
        try
        {
            DataTable SD_Priority = new SDTemplateFileds().FillStage(ddlRequestTypeStatus.SelectedValue, ddlOrg3.SelectedValue); ;
            ddlStage.DataSource = SD_Priority;
            ddlStage.DataTextField = "StageCodeRef";
            ddlStage.DataValueField = "id";
            ddlStage.DataBind();
            ddlStage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("---Select---", "0"));
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
                inEr.InsertErrorLogsF(Session["UserName"].ToString()
    , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }

        }
    }
    private void FillRequestTypeStatus(long OrgId)
    {
        try
        {
            DataTable RequestType = new SDTemplateFileds().FillRequestType(OrgId);
            ddlRequestTypeStatus.DataSource = RequestType;
            ddlRequestTypeStatus.DataTextField = "ReqTypeRef";
            ddlRequestTypeStatus.DataValueField = "ReqTypeRef";
            ddlRequestTypeStatus.DataBind();
            ddlRequestTypeStatus.Items.Insert(0, new System.Web.UI.WebControls.ListItem("---Select---", "0"));
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
    protected void SaveDataStatus()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SD_spAddStatus", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", r.Next());
                    cmd.Parameters.AddWithValue("@DeskRef", ddlRequestTypeStatus.SelectedValue);
                    cmd.Parameters.AddWithValue("@StatusRef", ddlRequestTypeStatus.SelectedValue + "||" + txtStatusName.Text.Trim());
                    cmd.Parameters.AddWithValue("@StatusCodeRef", txtStatusName.Text.Trim());
                    cmd.Parameters.AddWithValue("@StatusDesc", txtStatusDesc.Text);
                    cmd.Parameters.AddWithValue("@StatusColorCode", txtColorForStatus.Text);
                    cmd.Parameters.AddWithValue("@sd_stageFK", ddlStage.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@OrgDeskRef", ddlOrg3.SelectedValue.ToString());
                    cmd.Parameters.Add("@Error", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@Option", "AddStatus");
                    con.Open();
                    cmd.ExecuteNonQuery();
                    string ErrorChk = cmd.Parameters["@Error"].Value.ToString();
                    if (ErrorChk == "")
                    {
                        Session["Status"] = "ok";
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Saved Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                        cleardata();
                        FillStatusDetails(Convert.ToString(Session["OrgNameDesk"]), Convert.ToString(Session["ReqType"]));
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ warning_noti('{HttpUtility.JavaScriptStringEncode("" + ErrorChk + "")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
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
    protected void btnInsertStatus_Click(object sender, EventArgs e)
    {
        SaveDataStatus();
    }
    protected void gvStatus_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "DeleteEx")
            {
                //Determine the RowIndex of the Row whose Button was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                //Get the value of column from the DataKeys using the RowIndex.
                ID = Convert.ToInt32(gvStatus.DataKeys[rowIndex].Values["ID"]);
                string Deskref = gvStatus.Rows[rowIndex].Cells[1].Text;
                string SeverityName = gvStatus.Rows[rowIndex].Cells[2].Text;
                try
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("SD_spAddStatus", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ID", ID);
                            cmd.Parameters.AddWithValue("@DeskRef", Deskref);
                            cmd.Parameters.AddWithValue("@Option", "DeleteStatus");
                            cmd.CommandTimeout = 180;
                            int res = cmd.ExecuteNonQuery();
                            if (res > 0)
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Deleted Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                                cleardata();
                                FillSeverityDetails();
                            }
                            con.Close();
                            FillStatusDetails(Convert.ToString(Session["OrgNameDesk"]), Convert.ToString(Session["ReqType"]));
                            Response.Redirect("/DeskConfiguration.aspx");
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
            if (e.CommandName == "SelectStatus")
            {
                //Determine the RowIndex of the Row whose Button was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                //Get the value of column from the DataKeys using the RowIndex.
                ID = Convert.ToInt32(gvStatus.DataKeys[rowIndex].Values["ID"]);
                GridViewRow row = gvStatus.Rows[rowIndex];
                Label OrgID = (row.FindControl("lblOrgFk") as Label);
                if (ddlOrg3.Items.FindByValue(OrgID.Text.ToString().Trim()) != null)
                {
                    ddlOrg3.SelectedValue = OrgID.Text;
                    FillRequestTypeStatus(Convert.ToInt64(OrgID.Text));
                }

                ddlRequestTypeStatus.SelectedValue = gvStatus.Rows[rowIndex].Cells[1].Text;
                FillStage();
                Label Stage = (row.FindControl("lblStageFk") as Label);
                if (ddlStage.Items.FindByValue(Stage.Text.ToString().Trim()) != null)
                {
                    ddlStage.SelectedValue = Stage.Text;
                }
                txtStatusName.Text = gvStatus.Rows[rowIndex].Cells[3].Text;
                txtStatusDesc.Text = gvStatus.Rows[rowIndex].Cells[4].Text;
                txtColorForStatus.Text = gvStatus.Rows[rowIndex].Cells[5].Text;

                btnInsertStatus.Visible = false;
                btnUpdateStatus.Visible = true;
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
    protected void ImgBtnExport3_Click(object sender, EventArgs e)
    {
        try
        {

            DataTable dt = new DataTable("GridView_Data");
            foreach (System.Web.UI.WebControls.TableCell cell in gvStatus.HeaderRow.Cells)
            {
                dt.Columns.Add(cell.Text);
            }
            foreach (GridViewRow row in gvStatus.Rows)
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
                Response.AddHeader("content-disposition", "attachment;filename=SD_Status.xlsx");
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
    protected void gvStatus_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        //e.Row.Cells[4].BackColor = clr;

        try
        {

            if (e.Row.RowIndex >= 0)
            {
                //	System.Drawing.Color clr = ColorTranslator.FromHtml(e.Row.Cells[4].ToString());
                //System.Drawing.Color color = System.Drawing.ColorTranslator.FromHtml(e.Row.Cells[4].ToString());
                //e.Row.Cells[4].BackColor = System.Drawing.Color.FromArgb(int.Parse(e.Row.Cells[4].ToString().Replace("#","").Trim()));
                //e.Row.Cells[3].Attributes["style"] = "background-color:"+ color;
            }


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
    protected void btnUpdateStatus_Click(object sender, EventArgs e)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SD_spAddStatus", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters.AddWithValue("@DeskRef", ddlRequestTypeStatus.SelectedValue);
                    cmd.Parameters.AddWithValue("@StatusRef", ddlRequestTypeStatus.SelectedValue + "||" + txtStatusName.Text.Trim());
                    cmd.Parameters.AddWithValue("@StatusCodeRef", txtStatusName.Text.Trim());
                    cmd.Parameters.AddWithValue("@StatusDesc", txtStatusDesc.Text);
                    cmd.Parameters.AddWithValue("@StatusColorCode", txtColorForStatus.Text);
                    cmd.Parameters.AddWithValue("@OrgDeskRef", ddlOrg3.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@sd_stageFK", ddlStage.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@Option", "UpdateStatus");
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"success_noti('{HttpUtility.JavaScriptStringEncode("Updated Successfully!")}');", true);
                        FillStatusDetails(Convert.ToString(Session["OrgNameDesk"]), Convert.ToString(Session["ReqType"]));
                        btnUpdateStatus.Visible = false;
                        btnInsertStatus.Visible = true;
                        Response.Redirect("/DeskConfiguration.aspx");
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ warning_noti('{HttpUtility.JavaScriptStringEncode("Already Exists !")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
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
    protected void btnCancel3_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.AbsoluteUri);
    }
    protected void ddlOrg3_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillRequestTypeStatus(Convert.ToInt64(ddlOrg3.SelectedValue));
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
        FillStatusDetails();
    }
    protected void lnkPrevStage_Click(object sender, EventArgs e)
    {
        pnlAddStage.Visible = true;
        pnlStatus.Visible = false;
        CurrentStep = 3;
        Session["CurrentStep"] = CurrentStep;
        cleardata();
        DataBind();
        lnkNextStage_Click(null, null);
    }
    protected void lnkNextSeverity_Click(object sender, EventArgs e)
    {
        cleardata();
        CurrentStep = 5;
        DataBind();
        Session["CurrentStep"] = CurrentStep;
        FillOrganizationSeverity();
        if (Session["UserScope"].ToString().ToLower() == "admin")
        {
            FillSeverityDetailsWithCustomer(Convert.ToString(Session["OrgNameDesk"]));
            ddlOrg4.Enabled = false;
            ddlOrg4.SelectedValue = Session["SD_OrgID"].ToString();
            ddlOrg4.SelectedValue = Convert.ToString(Session["OrgNameDesk"]);
            FillRequestTypeStatus(Convert.ToInt64(ddlOrg4.SelectedValue));
            ddlRequestTypeStatus.SelectedValue = Convert.ToString(Session["ReqType"]);
            Session["SDRef"] = ddlRequestTypeSeverity.SelectedValue.ToString();
        }
        else
        {
            ddlOrg4.Enabled = true;
            FillSeverityDetailsWithCustomer(Convert.ToString(Session["OrgNameDesk"]));
            ddlOrg4.SelectedValue = Convert.ToString(Session["OrgNameDesk"]);
            FillRequestTypeSeverity(Convert.ToInt64(ddlOrg4.SelectedValue));
            ddlRequestTypeSeverity.SelectedValue = Convert.ToString(Session["ReqType"]);
            Session["SDRef"] = ddlRequestTypeSeverity.SelectedValue.ToString();
        }
        pnlAddSeverity.Visible = true;
        pnlStatus.Visible = false;
    }

    #endregion Add Status End 

    #region Add Severity Start
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
        FillStatusDetails();

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
                        //ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Saved Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Saved Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                        cleardata();
                        FillSeverityDetailsWithCustomer(Convert.ToString(Session["OrgNameDesk"]));
                        btnInsertSeverity.Visible = true;
                        btnUpdateSeverity.Visible = false;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ warning_noti('{HttpUtility.JavaScriptStringEncode("" + ErrorChk + "")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
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
                                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Saved Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                                cleardata();
                                FillSeverityDetailsWithCustomer(Convert.ToString(Session["OrgNameDesk"]));
                                lnkNextSeverity_Click(null, null);
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
                        //ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Updated Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"success_noti('{HttpUtility.JavaScriptStringEncode("Updated Successfully!")}');", true);
                        cleardata();
                        //FillSeverityDetails();
                        FillSeverityDetailsWithCustomer(Convert.ToString(Session["OrgNameDesk"]));
                        btnInsertSeverity.Visible = true;
                        btnUpdateSeverity.Visible = false;
                        Response.Redirect("/DeskConfiguration.aspx");
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ warning_noti('{HttpUtility.JavaScriptStringEncode("Already Exists !")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
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
    protected void lnkPrevStatus_Click(object sender, EventArgs e)
    {
        CurrentStep = 4;
        ViewState["CurrentStep"] = CurrentStep;
        DataBind();
        cleardata();
        lnkbtnNextStatus_Click(null, null);
        pnlAddSeverity.Visible = false;
    }
    protected void lnkNextPriority_Click(object sender, EventArgs e)
    {
        CurrentStep = 6;
        DataBind();
        Session["CurrentStep"] = CurrentStep;
        cleardata();
        FillOrganizationPriority();
        if (Session["UserScope"].ToString().ToLower() == "admin")
        {
            FillPriorityDetailsCustomer(Convert.ToString(Session["OrgNameDesk"]));
            ddlOrg5.Enabled = false;
            //ddlOrg5.SelectedValue = Session["SD_OrgID"].ToString();
            ddlOrg5.SelectedValue = Convert.ToString(Session["OrgNameDesk"]);
            FillRequestTypePriority(Convert.ToInt64(ddlOrg5.SelectedValue));
            ddlRequestTypePriority.SelectedValue = Convert.ToString(Session["ReqType"]);
            FillSeverityForPriority();
        }
        else
        {
            ddlOrg5.Enabled = true;
            //FillPriorityDetails();
            FillPriorityDetailsCustomer(Convert.ToString(Session["OrgNameDesk"]));
            ddlOrg5.SelectedValue = Convert.ToString(Session["OrgNameDesk"]);
            FillRequestTypePriority(Convert.ToInt64(ddlOrg5.SelectedValue));
            ddlRequestTypePriority.SelectedValue = Convert.ToString(Session["ReqType"]);
            FillSeverityForPriority();
        }
        pnlAddPriority.Visible = true;
        pnlAddSeverity.Visible = false;
    }
    #endregion Add Severity End

    #region Add Priority Start
    private void FillSeverityForPriority()
    {
        try
        {
            DataTable SD_Severity = new SDTemplateFileds().FillSeverity(Convert.ToString(Session["ReqType"]), Convert.ToString(Session["OrgNameDesk"]));
            ddlSeverityfPrior.DataSource = SD_Severity;
            ddlSeverityfPrior.DataTextField = "Serveritycoderef";
            ddlSeverityfPrior.DataValueField = "id";
            ddlSeverityfPrior.DataBind();
            ddlSeverityfPrior.Items.Insert(0, new System.Web.UI.WebControls.ListItem("   -Select Severity-", "0"));
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
    protected void GridFormat6(DataTable dt)
    {
        gvPriority.UseAccessibleHeader = true;
        gvPriority.HeaderRow.TableSection = TableRowSection.TableHeader;

        if (gvPriority.TopPagerRow != null)
        {
            gvPriority.TopPagerRow.TableSection = TableRowSection.TableHeader;
        }
        if (gvPriority.BottomPagerRow != null)
        {
            gvPriority.BottomPagerRow.TableSection = TableRowSection.TableFooter;
        }
        if (dt.Rows.Count > 0)
            gvPriority.FooterRow.TableSection = TableRowSection.TableFooter;
    }
    private void FillPriorityDetailsCustomer(string orgid)
    {
        try
        {
            DataTable SD_Priority = new FillSDFields().FillPriorityWithCustomer(orgid);
            if (SD_Priority.Rows.Count > 0)
            {
                this.gvPriority.DataSource = (object)SD_Priority;
                this.gvPriority.DataBind();
            }
            else
            {
                this.gvPriority.DataSource = (object)null;
                this.gvPriority.DataBind();
            }
            if (SD_Priority.Rows.Count > 0) { }
            //GridFormat6(SD_Priority);
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
    private void FillOrganizationPriority()
    {
        try
        {
            DataTable SD_Org = new FillSDFields().FillOrganization();
            ddlOrg5.DataSource = SD_Org;
            ddlOrg5.DataTextField = "OrgName";
            ddlOrg5.DataValueField = "Org_ID";
            ddlOrg5.DataBind();
            ddlOrg5.Items.Insert(0, new System.Web.UI.WebControls.ListItem("   -Select Organization-", "0"));
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
    private void FillPriorityDetails()
    {
        try
        {
            DataTable SD_Priority = new FillSDFields().FillPriority();
            if (SD_Priority.Rows.Count > 0)
            {
                this.gvPriority.DataSource = (object)SD_Priority;
                this.gvPriority.DataBind();
            }
            else
            {
                this.gvPriority.DataSource = (object)null;
                this.gvPriority.DataBind();
            }
            //GridFormat6(SD_Priority);
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
    protected void ddlRequestTypePriority_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["SDRef"] = ddlRequestTypePriority.SelectedValue.ToString();
    }
    private void FillRequestTypePriority(long OrgID)
    {
        try
        {
            DataTable RequestType = new SDTemplateFileds().FillRequestType(OrgID);
            ddlRequestTypePriority.DataSource = RequestType;
            ddlRequestTypePriority.DataTextField = "ReqTypeRef";
            ddlRequestTypePriority.DataValueField = "ReqTypeRef";
            ddlRequestTypePriority.DataBind();
            ddlRequestTypePriority.Items.Insert(0, new ListItem("   -Select RequestType-", "0"));
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
    protected void ImgBtnExport7_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable("GridView_Data");
            foreach (System.Web.UI.WebControls.TableCell cell in gvPriority.HeaderRow.Cells)
            {
                dt.Columns.Add(cell.Text);
            }
            foreach (GridViewRow row in gvPriority.Rows)
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
                Response.AddHeader("content-disposition", "attachment;filename=Priority.xlsx");
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
    protected void gvPriority_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "DeleteEx")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvPriority.Rows[rowIndex];
                Label ID = (row.FindControl("lblID") as Label);
                //if (ID.Text.ToString().Trim() != null)
                //{
                //    ViewState["PrioirtyID"] = ID.Text;
                //}
                //Get the value of column from the DataKeys using the RowIndex.
                string PriorityRef = gvPriority.DataKeys[rowIndex].Values["PriorityRef"].ToString();
                string Deskref = gvPriority.Rows[rowIndex].Cells[1].Text;
                string PriorityName = gvPriority.Rows[rowIndex].Cells[2].Text;
                try
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("SD_spAddPriority", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@PriorityRef", PriorityRef);
                            cmd.Parameters.AddWithValue("@DeskRef", Deskref);
                            cmd.Parameters.AddWithValue("@ID", ID.Text);
                            cmd.Parameters.AddWithValue("@Option", "DeletePriority");
                            cmd.CommandTimeout = 180;
                            int res = cmd.ExecuteNonQuery();
                            if (res > 0)
                            {
                                //ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Deleted Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"success_noti('{HttpUtility.JavaScriptStringEncode("Deleted Successfully!")}');", true);
                                lnkNextPriority_Click(null, null);
                                cleardata();
                            }
                            con.Close();
                            //FillPriorityDetails();
                            lnkNextPriority_Click(null, null);
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
                GridViewRow row = gvPriority.Rows[rowIndex];
                string PriorityRef = gvPriority.DataKeys[rowIndex].Values["PriorityRef"].ToString();
                Label OrgID = (row.FindControl("lblOrgFk") as Label);
                if (ddlOrg5.Items.FindByValue(OrgID.Text.ToString().Trim()) != null)
                {
                    ddlOrg5.SelectedValue = OrgID.Text;
                    FillRequestTypePriority(Convert.ToInt64(OrgID.Text));
                }
                Label ResponseTime = (row.FindControl("lblResponseTime") as Label);
                if (ResponseTime.Text.ToString().Trim() != null)
                {
                    txtRespTimePriority.Text = ResponseTime.Text;
                }
                Label ResolutionTime = (row.FindControl("lblResolutionTime") as Label);
                if (ResolutionTime.Text.ToString().Trim() != null)
                {
                    txtReslnTimePriority.Text = ResolutionTime.Text;
                }
                Label ID = (row.FindControl("lblID") as Label);
                if (ID.Text.ToString().Trim() != null)
                {
                    ViewState["PrioirtyID"] = ID.Text;
                }
                ddlRequestTypePriority.SelectedValue = gvPriority.Rows[rowIndex].Cells[1].Text;
                txtpriority.Text = gvPriority.Rows[rowIndex].Cells[2].Text;
                txtPriorityDescription.Text = gvPriority.Rows[rowIndex].Cells[3].Text;
                ddlSeverityfPrior.SelectedValue = gvPriority.Rows[rowIndex].Cells[4].Text;
                btnInsertPriority.Visible = false;
                btnUpdatePriority.Visible = true;
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
    protected void SaveDataPrioirty()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SD_spAddPriority", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", r.Next());
                    cmd.Parameters.AddWithValue("@DeskRef", Session["SDRef"].ToString());
                    cmd.Parameters.AddWithValue("@PriorityRef", Session["SDRef"].ToString().Trim() + "||" + txtpriority.Text.Trim());
                    cmd.Parameters.AddWithValue("@PriorityCodeRef", txtpriority.Text.Trim());
                    cmd.Parameters.AddWithValue("@PriorityDesc", txtPriorityDescription.Text);
                    cmd.Parameters.AddWithValue("@OrgDeskRef", ddlOrg5.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@ResponseTime", txtRespTimePriority.Text.Trim().ToString());
                    cmd.Parameters.AddWithValue("@ResolutionTime", txtReslnTimePriority.Text.Trim().ToString());
                    cmd.Parameters.AddWithValue("@Severity", ddlSeverityfPrior.SelectedValue);
                    cmd.Parameters.Add("@Error", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@Option", "AddPriority");
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    string ErrorChk = cmd.Parameters["@Error"].Value.ToString();
                    if (res > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Saved Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                        lnkNextPriority_Click(null, null);
                        Session["Priority"] = "OK";
                        cleardata();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ warning_noti('{HttpUtility.JavaScriptStringEncode("" + ErrorChk + "")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
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
    protected void btnInsertPriority_Click(object sender, EventArgs e)
    {
        SaveDataPrioirty();
    }
    protected void gvPriority_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (Session["UserScope"].ToString() == "Master")
        {
            e.Row.Cells[5].Visible = true;
            e.Row.Cells[6].Visible = true;
        }

        if (Session["UserScope"].ToString() == "Technician" || Session["UserScope"].ToString() == "Admin")
        {
            e.Row.Cells[5].Visible = true;
            e.Row.Cells[6].Visible = false;

        }
    }
    protected void btnUpdatePriority_Click(object sender, EventArgs e)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand("SD_spAddPriority", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@DeskRef", ddlRequestTypePriority.SelectedValue);
                    cmd.Parameters.AddWithValue("@PriorityRef", ddlRequestTypePriority.SelectedValue.ToString().Trim() + "||" + txtpriority.Text.Trim());
                    cmd.Parameters.AddWithValue("@PriorityCodeRef", txtpriority.Text.Trim());
                    cmd.Parameters.AddWithValue("@PriorityDesc", txtPriorityDescription.Text);
                    cmd.Parameters.AddWithValue("@OrgDeskRef", ddlOrg5.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@ResponseTime", txtRespTimePriority.Text.Trim().ToString());
                    cmd.Parameters.AddWithValue("@ResolutionTime", txtReslnTimePriority.Text.Trim().ToString());
                    cmd.Parameters.AddWithValue("@Severity", ddlSeverityfPrior.SelectedValue);
                    cmd.Parameters.AddWithValue("@ID", Convert.ToInt64(ViewState["PrioirtyID"]));
                    cmd.Parameters.AddWithValue("@Option", "UpdatePriority");
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        //ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Updated Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"success_noti('{HttpUtility.JavaScriptStringEncode("Updated Successfully!")}');", true);
                        lnkNextPriority_Click(null, null);
                        cleardata();
                        btnInsertPriority.Visible = true;
                        btnUpdatePriority.Visible = false;
                        Response.Redirect("/DeskConfiguration.aspx");
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ warning_noti('{HttpUtility.JavaScriptStringEncode("Already Exists !")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
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
    protected void btnCancel6_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.AbsoluteUri);
    }
    protected void ddlOrg5_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillRequestTypePriority(Convert.ToInt64(ddlOrg5.SelectedValue));
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
    protected void lnkPreviousSeverity_Click(object sender, EventArgs e)
    {
        cleardata();
        CurrentStep = 5;
        Session["CurrentStep"] = CurrentStep;
        DataBind();
        lnkNextSeverity_Click(null, null);
        pnlAddPriority.Visible = false;
    }
    protected void lnkNextCategory_Click(object sender, EventArgs e)
    {
        CurrentStep = 7;
        DataBind();
        Session["CurrentStep"] = CurrentStep;
        cleardata();
        pnlCategory.Visible = true;
        pnlAddPriority.Visible = false;
        FillOrganizationCategory();
        //ddlOrg6.Enabled = true;
        ddlOrg6.SelectedValue = Convert.ToString(Session["OrgNameDesk"]);
        FillRequestTypeCategory(Convert.ToInt64(ddlOrg6.SelectedValue));
        ddlRequestTypeCategory.SelectedValue = Convert.ToString(Session["ReqType"]);
        Session["SDRef"] = ddlRequestTypeCategory.SelectedValue;
        FillParentCategory();

    }
    #endregion Add Priority End

    #region Add Category Start
    private void FillOrganizationCategory()
    {
        try
        {
            DataTable SD_Org = new FillSDFields().FillOrganization();
            ddlOrg6.DataSource = SD_Org;
            ddlOrg6.DataTextField = "OrgName";
            ddlOrg6.DataValueField = "Org_ID";
            ddlOrg6.DataBind();
            ddlOrg6.Items.Insert(0, new ListItem(" -Select Organization-", "0"));
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
    private void FillRequestTypeCategory(long Orgid)
    {
        try
        {
            DataTable RequestType = new SDTemplateFileds().FillRequestType(Orgid);
            ddlRequestTypeCategory.DataSource = RequestType;
            ddlRequestTypeCategory.DataTextField = "ReqTypeRef";
            ddlRequestTypeCategory.DataValueField = "ReqTypeRef";
            ddlRequestTypeCategory.DataBind();
            ddlRequestTypeCategory.Items.Insert(0, new ListItem("   -Select RequestType-", "0"));
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
    protected void ddlRequestTypeCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["SDRef"] = ddlRequestTypeCategory.SelectedValue.ToString();
        FillParentCategory();

    }
    protected void SaveParentCategory()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand("SD_spAddCategory", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ID", r.Next());
                    cmd.Parameters.AddWithValue("@DeskRef", Session["SDRef"].ToString());
                    cmd.Parameters.AddWithValue("@Categoryref", Session["SDRef"].ToString().Trim() + "||" + txtParentCategory.Text.Trim());
                    cmd.Parameters.AddWithValue("@CategoryCodeRef", txtParentCategory.Text.Trim());
                    cmd.Parameters.AddWithValue("@rowDesc", "");
                    cmd.Parameters.AddWithValue("@CategoryLevel", Convert.ToInt32(1));
                    cmd.Parameters.AddWithValue("@OrgDeskRef", ddlOrg6.SelectedValue);
                    cmd.Parameters.AddWithValue("@Option", "AddCategory");
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        ddlParentCategory.Enabled = true;
                        ddlParentCategory.Visible = true;
                        FillParentCategory();
                        Session["Category"] = "OK";
                        //Response.Redirect(Request.Url.AbsoluteUri);
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
                inEr.InsertErrorLogsF(Session["UserName"].ToString()
                    , "Add Category: Error While Adding This Category" + Session["SDRef"].ToString().Trim() + "||" + txtParentCategory.Text.Trim() + Request.Url.ToString() + "Got Exception" + ex.ToString());

                //    // msg.ReportError1(ex.Message);
                //    // lblsuccess.Text = msg.ms; ;
            }

        }
    }
    public string checkparentcategduplicate(string parentcat, string childcat)
    {
        string sql2 = "Select ID  from SD_Category where Categoryref='" + parentcat + "'";
        string ParentCatID = Convert.ToString(database.GetScalarValue(sql2));
        string Categoryref = parentcat + "||" + childcat;
        string sql = "if exists(select * from SD_Category where Categoryref='" + Categoryref + "' " +
            "and CategoryCodeRef='" + childcat + "' and rowDesc='' and " +
            " partitionid='1' and inUse='1' and OrgDeskRef='" + ddlOrg6.SelectedValue + "' and sdCategoryFK='" + ParentCatID + "') " +
            "begin select 'True'  end";

        return Convert.ToString(database.GetScalarValue(sql));
    }
    int res;
    protected int SaveChildCategory(string CategoryRef, string CategoryCodeRef, int CategoryLevel)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SD_spAddCategory", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RefID", CategoryRef);
                    cmd.Parameters.AddWithValue("@ID", r.Next());
                    cmd.Parameters.AddWithValue("@DeskRef", Session["SDRef"].ToString());
                    cmd.Parameters.AddWithValue("@Categoryref", CategoryRef + "||" + CategoryCodeRef.Trim());
                    cmd.Parameters.AddWithValue("@CategoryCodeRef", CategoryCodeRef.Trim());
                    cmd.Parameters.AddWithValue("@rowDesc", "");
                    cmd.Parameters.AddWithValue("@CategoryLevel", Convert.ToInt32(CategoryLevel));
                    cmd.Parameters.AddWithValue("@OrgDeskRef", ddlOrg6.SelectedValue);
                    cmd.Parameters.AddWithValue("@Option", "AddChildCategory");
                    con.Open();
                    res = cmd.ExecuteNonQuery();
                    //Response.Redirect(Request.Url.AbsoluteUri);
                }
            }
            return res;
        }
        catch (ThreadAbortException e2)
        {
            Console.WriteLine("Exception message: {0}", e2.Message);
            Thread.ResetAbort();
            return 0;
        }
        catch (Exception ex)
        {

            if (ex.ToString().Contains("System.Threading.Thread.AbortInternal()"))
            {

            }
            else
            {

                inEr.InsertErrorLogsF(Session["UserName"].ToString()
                    , "Add Category: Error While Adding This Category" + Session["SDRef"].ToString().Trim() + "||" + txtParentCategory.Text.Trim() + Request.Url.ToString() + "Got Exception" + ex.ToString());

                //    // msg.ReportError1(ex.Message);
                //    // lblsuccess.Text = msg.ms; ;
            }
            return 0;
        }
        //  Response.Redirect(Request.Url.AbsoluteUri);

    }
    private void FillParentCategory()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                con.Open();
                string sql = @"SELECT CategoryCodeRef,
           Categoryref FROM [dbo].fnGetCategoryFullPathForDesk('" + ddlRequestTypeCategory.SelectedValue + "','" + ddlOrg6.SelectedValue + "', 1) where Level=1   order by Categoryref asc";
                using (SqlCommand cmd = new SqlCommand(sql, con))
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
                                ddlParentCategory.DataSource = dt;
                                ddlParentCategory.DataTextField = "CategoryCodeRef";
                                ddlParentCategory.DataValueField = "Categoryref";
                                ddlParentCategory.DataBind();
                                ddlParentCategory.Items.Insert(0, new ListItem("   -Select Category-", "0"));
                            }
                            else
                            {
                                ddlParentCategory.Items.Clear();
                                ddlParentCategory.Items.Insert(0, new ListItem("   -Select Category-", "0"));
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
                inEr.InsertErrorLogsF(Session["UserName"].ToString()
            , "Add Category: Error While Populating ParentCategory " + Request.Url.ToString() + "Got Exception" + ex.ToString());
                // lblMsg.Text = ex.Message.ToString();
            }
        }
    }
    private DataTable FillCategoryLevel(string category, int categoryLevel)
    {
        try
        {
            hdnVarCategoryIII.Value = hdnVarCategoryI.Value;
            DataTable dtFillCategory = new DataTable();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                con.Open();
                string sql = "select Categoryref,categorycoderef from " +
                    "(select a.Categoryref as sdCategoryFK,b.Categoryref,b.categorycoderef from dbo.fnGetCategoryFullPathForPartition(1,'" + ddlOrg6.SelectedValue + "') a  left join  dbo.fnGetCategoryFullPathForPartition(1,'" + ddlOrg6.SelectedValue + "') b on a.id=b.sdCategoryFK) c where c.sdCategoryFK='" + category + "' and c.Categoryref!='' order by categorycoderef asc";
                using (SqlCommand cmd = new SqlCommand(sql, con))
                {
                    cmd.CommandType = CommandType.Text;
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.SelectCommand.CommandTimeout = 180;
                        adp.Fill(dtFillCategory);
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
            return null;
        }
    }
    protected void ddlParentCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            imgbtnCancelCatII.Enabled = true;
            txtCatII.Visible = false;
            ddlCatII.Enabled = true;
            ddlCatII.Visible = true;
            ddlCatII.ClearSelection();
            hdnVarCategoryI.Value = null;
            hdnVarCategoryII.Value = null;
            hdnVarCategoryIII.Value = null;
            hdnVarCategoryIV.Value = null;
            hdnVarCategoryV.Value = null;
            ddlCateLevelIII.ClearSelection();
            ddlCateLevelIII.Enabled = false;
            ddlCateLevelIII.Visible = true;
            txtCatLevelIII.Visible = false;
            imgbtnCancelCatIII.Enabled = false;
            imgbtnCancelCatIV.Enabled = false;
            imgbtnCancelCatV.Enabled = false;
            ddlCateLevelIV.ClearSelection();
            ddlCateLevelIV.Enabled = false;
            ddlCateLevelIV.Visible = true;
            txtCateLevelIV.Visible = false;
            ddlCatV.ClearSelection();
            ddlCatV.Enabled = false;
            ddlCatV.Visible = true;
            txtCatV.Visible = false;
            hdnVarCategoryI.Value = ddlParentCategory.SelectedValue.ToString();
            FillCategoryLevel2();
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
    protected void imgbtnAddParentCategory_Click(object sender, EventArgs e)
    {
        try
        {
            txtParentCategory.Text = "";
            ddlParentCategory.Enabled = false;
            ddlParentCategory.Visible = false;
            //Enable textbox  for entry
            txtParentCategory.Visible = true;
            rfvtxtParentCategory.Enabled = true;
            imgbtnSaveParentCategory.Enabled = true;

            imgbtnCancelParent.Enabled = true;
            ddlCatII.Enabled = false;
            txtCatII.Visible = false;
            ddlCateLevelIII.Enabled = false;
            txtCatLevelIII.Visible = false;
            ddlCateLevelIV.Enabled = false;
            txtCateLevelIV.Visible = false;
            ddlCatV.Enabled = false;
            txtCatV.Visible = false;
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

    protected void imgbtnSaveParentCategory_Click(object sender, EventArgs e)
    {
        try
        {
            rfvtxtParentCategory.Enabled = true;
            txtParentCategory.Visible = true;
            if (txtParentCategory.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Category Can Not be Empty !');window.location ='/DeskConfiguration.aspx';", true);
                //        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
                //$"warning_noti('{HttpUtility.JavaScriptStringEncode("Category Can Not be Empty !")}'); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
                return;
            }
            else
            {
                string Categoryref = Session["SDRef"].ToString().Trim() + "||" + txtParentCategory.Text.Trim();
                string sql = "if exists(select * from SD_Category where Categoryref='" + Categoryref + "' " +
                    "and CategoryCodeRef='" + txtParentCategory.Text.Trim() + "' and rowDesc='' and " +
                    " partitionid='1' and inUse='1' and OrgDeskRef='" + ddlOrg6.SelectedValue + "') begin select 'True'  end";
                if (Convert.ToString(database.GetScalarValue(sql)) != "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Category Allredy Exists for this Organization!');window.location ='/DeskConfiguration.aspx';", true);

                    //ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ warning_noti('{HttpUtility.JavaScriptStringEncode("Category Allredy Exists for this Organization!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                    return;
                }
            }
            Session["Category"] = "OK";
            SaveParentCategory();
            FillParentCategory();
            txtParentCategory.Visible = false;
            Response.Redirect("/DeskConfiguration.aspx");
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
    /// <summary>
    /// Add and Save Category Level 2
    /// </summary>
    protected void imgbtnCatII_Click(object sender, EventArgs e)
    {
        try
        {
            txtCatII.Text = "";
            //// to enable  textbox and disable  dropdown
            ///// Make sure Category level 1 dropdown is enable and value is selected
            ddlParentCategory.Enabled = true;
            ddlParentCategory.Visible = true;
            txtParentCategory.Visible = false;
            imgbtnSaveParentCategory.Enabled = false;
            ddlCatII.Enabled = false;
            ddlCatII.Visible = false;

            txtCatII.Visible = true;
            txtCatII.Enabled = true;
            imgbtnSaveCatII.Enabled = true;

            ddlCateLevelIII.Enabled = false;
            txtCatLevelIII.Visible = false;
            ddlCateLevelIV.Enabled = false;
            txtCateLevelIV.Visible = false;
            ddlCatV.Enabled = false;
            txtCatV.Visible = false;
            hdnVarCategoryI.Value = ddlParentCategory.SelectedValue;
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
    protected void imgbtnSaveCatII_Click(object sender, EventArgs e)
    {
        try
        {
            rfvtxtCatII.Enabled = true;
            if (txtCatII.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('CategoryII Can Not be Empty !');window.location ='/DeskConfiguration.aspx';", true);
                // ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
                // $"warning_noti('{HttpUtility.JavaScriptStringEncode("CategoryII Can Not be Empty !")}'); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
                return;
            }
            else
            {
                string chkduplicate = checkparentcategduplicate(ddlParentCategory.SelectedValue.ToString(), txtCatII.Text.Trim());
                if (chkduplicate != "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('CategoryII Allredy Exists for this Organization!');window.location ='/DeskConfiguration.aspx';", true);
                    //            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
                    //$"warning_noti('{HttpUtility.JavaScriptStringEncode("CategoryII Allredy Exists for this Organization!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
                    return;
                }
            }
            res = SaveChildCategory(ddlParentCategory.SelectedValue.ToString(), txtCatII.Text, 2);
            ddlCatII.Enabled = true;
            ddlCatII.Visible = true;
            txtCatII.Enabled = false;
            txtCatII.Visible = false;
            if (res > 0)
            {
                ddlCatII.Enabled = true;
                ddlCatII.Visible = true;
                FillParentCategory();
                ddlParentCategory.SelectedValue = hdnVarCategoryI.Value.ToString();
                FillCategoryLevel2();


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
    protected void ddlCatII_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //// Enable Category level dropdown 3 textbox and 
            ///Make sure all its parent category got selected
            imgbtnCancelCatIII.Enabled = true;
            ddlCateLevelIII.Enabled = true;
            ddlCateLevelIII.Visible = true;
            txtCatLevelIII.Visible = false;
            hdnVarCategoryII.Value = ddlCatII.SelectedValue.ToString();
            // FillCategoryLevel(ddlCategory1.SelectedValue, 3);
            FillCategoryLevel3();

            imgbtnCancelCatIV.Enabled = false;
            imgbtnCancelCatV.Enabled = false;

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
    /// <summary>
    /// Add and Save Category Level 3
    /// </summary>
    protected void FillCategoryLevel2()
    {
        try
        {
            //hdnVarCategoryII.Value = ddlParentCategory.SelectedValue.ToString();
            DataTable FillCategoryLevel2 = new DataTable();
            FillCategoryLevel2 = FillCategoryLevel(ddlParentCategory.SelectedValue, 2);
            if (FillCategoryLevel2.Rows.Count > 0)
            {
                ddlCatII.DataSource = FillCategoryLevel2;
                ddlCatII.DataTextField = "CategoryCodeRef";
                ddlCatII.DataValueField = "Categoryref";
                ddlCatII.DataBind();
                ddlCatII.Items.Insert(0, new ListItem("   -Select Category Level 2-", "0"));
            }
            else
            {
                ddlCatII.ClearSelection();
                ddlCatII.Enabled = false;
                FillCategoryLevel2 = null;
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
    protected void FillCategoryLevel3()
    {
        try
        {
            DataTable FillCategoryLevel3 = FillCategoryLevel(ddlCatII.SelectedValue, 3);
            if (FillCategoryLevel3.Rows.Count > 0)
            {
                ddlCateLevelIII.DataSource = FillCategoryLevel3;
                ddlCateLevelIII.DataTextField = "CategoryCodeRef";
                ddlCateLevelIII.DataValueField = "Categoryref";
                ddlCateLevelIII.DataBind();
                ddlCateLevelIII.Items.Insert(0, new ListItem("   -Select Category Level 3-", "0"));
            }
            else
            {
                ddlCateLevelIII.ClearSelection();
                ddlCateLevelIII.Enabled = false;
                ddlCateLevelIII.DataSource = null;
                ddlCateLevelIII.DataBind();
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
    protected void FillCategorylevel4()
    {
        try
        {
            DataTable FillCategoryLevel4 = FillCategoryLevel(ddlCateLevelIII.SelectedValue, 4);
            if (FillCategoryLevel4.Rows.Count > 0)
            {
                ddlCateLevelIV.DataSource = FillCategoryLevel4;
                ddlCateLevelIV.DataTextField = "CategoryCodeRef";
                ddlCateLevelIV.DataValueField = "Categoryref";
                ddlCateLevelIV.DataBind();
                ddlCateLevelIV.Items.Insert(0, new ListItem("   -Select Category Level 4 -", "0"));
            }
            else
            {
                ddlCateLevelIV.DataSource = null;
                ddlCateLevelIV.DataBind();

                ddlCateLevelIV.ClearSelection();
                ddlCateLevelIV.Enabled = false;
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
    protected void FillCategorylevel5()
    {
        try
        {
            DataTable FillCategoryLevel5 = FillCategoryLevel(ddlCateLevelIV.SelectedValue, 5);
            if (FillCategoryLevel5.Rows.Count > 0)
            {
                ddlCatV.DataSource = FillCategoryLevel5;
                ddlCatV.DataTextField = "CategoryCodeRef";
                ddlCatV.DataValueField = "Categoryref";
                ddlCatV.DataBind();
                ddlCatV.Items.Insert(0, new ListItem("   -Select Category Level 5-", "0"));
            }
            else
            {
                ddlCatV.DataSource = null;
                ddlCatV.DataBind();
                ddlCatV.ClearSelection();
                ddlCatV.Enabled = false;
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
    protected void imgAddCatIII_Click(object sender, EventArgs e)
    {
        try
        {
            txtCatLevelIII.Text = "";
            ddlCatII.Enabled = true;
            ddlCatII.Visible = true;
            ddlCateLevelIII.Enabled = false;
            ddlCateLevelIII.Visible = false;
            imgbtnSaveCatII.Enabled = false;
            txtCatII.Visible = false;
            txtCatII.Enabled = false;
            txtCatLevelIII.Visible = true;
            txtCatLevelIII.Enabled = true;
            imgSaveCatIII.Enabled = true;
            ddlCateLevelIV.Enabled = false;
            txtCateLevelIV.Visible = false;
            ddlCatV.Enabled = false;
            txtCatV.Visible = false;
            hdnVarCategoryII.Value = ddlCatII.SelectedValue;
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
    /// <summary>
    /// ////Save Category level 3
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void imgSaveCatIII_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtCatLevelIII.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('CategoryIII Can Not be Empty !');window.location ='/DeskConfiguration.aspx';", true);
                //        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
                //$"warning_noti('{HttpUtility.JavaScriptStringEncode("CategoryIII Can Not be Empty !")}'); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
                return;
            }
            else
            {
                string chkduplicate = checkparentcategduplicate(ddlCatII.SelectedValue.ToString(), txtCatLevelIII.Text.Trim());
                if (chkduplicate != "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('CategoryIII Allredy Exists for this Organization!');window.location ='/DeskConfiguration.aspx';", true);
                    //            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
                    //$"warning_noti('{HttpUtility.JavaScriptStringEncode("CategoryIII Allredy Exists for this Organization!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
                    return;
                }
            }
            rfvtxtCatLevelIII.Enabled = true;
            FillParentCategory();
            ddlParentCategory.SelectedValue = hdnVarCategoryI.Value;
            FillCategoryLevel2();
            ddlCatII.SelectedValue = hdnVarCategoryII.Value;
            res = SaveChildCategory(ddlCatII.SelectedValue.ToString(), txtCatLevelIII.Text, 3);
            ddlCateLevelIII.Enabled = true;
            ddlCateLevelIII.Visible = true;
            txtCatLevelIII.Enabled = false;
            txtCatLevelIII.Visible = false;
            if (res > 0)
            {
                FillCategoryLevel3();
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
    protected void ddlCateLevelIII_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            imgbtnCancelCatIII.Enabled = true;

            ddlCateLevelIV.Enabled = true;
            ddlCateLevelIV.Visible = true;
            txtCateLevelIV.Visible = false;
            hdnVarCategoryIII.Value = ddlCateLevelIII.SelectedValue.ToString();
            // FillCategoryLevel(ddlCategory1.SelectedValue, 3);
            FillCategorylevel4();
            // FillCategorylevel4();

            imgbtnCancelCatV.Enabled = false;
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
    // Add and Save Category Level 4
    protected void imgbtnCatelevelIV_Click(object sender, EventArgs e)
    {
        try
        {
            txtCateLevelIV.Text = "";
            ddlCateLevelIII.Enabled = true;
            ddlCateLevelIII.Visible = true;
            txtCatLevelIII.Visible = false;
            txtCatLevelIII.Enabled = false;
            imgSaveCatIII.Enabled = false;
            ddlCateLevelIV.Enabled = false;
            ddlCateLevelIV.Visible = false;
            txtCateLevelIV.Visible = true;
            txtCateLevelIV.Enabled = true;
            imgbtnSaveCateLvlIV.Enabled = true;
            imgbtnCancelCatIV.Enabled = true;


            ddlCatV.Enabled = false;
            txtCatV.Visible = false;
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
    protected void imgbtnSaveCateLvlIV_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtCateLevelIV.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('CategoryIV Can Not be Empty !');window.location ='/DeskConfiguration.aspx';", true);
                //        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
                //$"warning_noti('{HttpUtility.JavaScriptStringEncode("CategoryIV Can Not be Empty !")}'); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
                return;
            }
            else
            {
                string chkduplicate = checkparentcategduplicate(ddlCateLevelIII.SelectedValue.ToString(), txtCateLevelIV.Text.Trim());
                if (chkduplicate != "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('CategoryIV Allredy Exists for this Organization!');window.location ='/DeskConfiguration.aspx';", true);
                    //            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
                    //$"warning_noti('{HttpUtility.JavaScriptStringEncode("CategoryIV Allredy Exists for this Organization!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
                    return;
                }
            }

            rfvtxtCateLevelIV.Enabled = true;
            res = SaveChildCategory(ddlCateLevelIII.SelectedValue.ToString(), txtCateLevelIV.Text, 4);
            ddlCateLevelIV.Enabled = true;
            ddlCateLevelIV.Visible = true;
            txtCateLevelIV.Visible = false;
            txtCateLevelIV.Enabled = false;
            if (res > 0)
            {
                FillCategorylevel4();
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
    protected void ddlCateLevelIV_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            imgbtnCancelCatIV.Enabled = true;


            ddlCatV.Enabled = true;
            ddlCatV.Visible = true;
            txtCatV.Visible = false;
            hdnVarCategoryIV.Value = ddlCateLevelIV.SelectedValue.ToString();
            // FillCategoryLevel(ddlCategory1.SelectedValue, 3);

            FillCategorylevel5();
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
    // Add and save Category Level 5
    protected void imgbtnAddCatV_Click(object sender, EventArgs e)
    {
        try
        {
            txtCatV.Text = "";
            ddlCateLevelIV.Enabled = true;
            ddlCateLevelIV.Visible = true;
            txtCateLevelIV.Visible = false;
            txtCateLevelIV.Enabled = false;
            imgbtnSaveCateLvlIV.Enabled = false;
            ddlCatV.Enabled = false;
            ddlCatV.Visible = false;
            txtCatV.Visible = true;
            txtCatV.Enabled = true;
            imgbtnSaveCatV.Enabled = true;
            imgbtnCancelCatV.Enabled = true;
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
    protected void imgbtnSaveCatV_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtCatV.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('CategoryV Can Not be Empty !');window.location ='/DeskConfiguration.aspx';", true);
                //        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
                //$"warning_noti('{HttpUtility.JavaScriptStringEncode("CategoryV Can Not be Empty !")}'); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
                return;
            }
            else
            {
                string chkduplicate = checkparentcategduplicate(ddlCateLevelIV.SelectedValue.ToString(), txtCatV.Text.Trim());
                if (chkduplicate != "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('CategoryV Allredy Exists for this Organization!');window.location ='/DeskConfiguration.aspx';", true);
                    //            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
                    //$"warning_noti('{HttpUtility.JavaScriptStringEncode("CategoryV Allredy Exists for this Organization!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
                    return;

                }
            }
            rfvtxtCatV.Enabled = true;
            res = SaveChildCategory(ddlCateLevelIV.SelectedValue.ToString(), txtCatV.Text, 5);
            ddlCatV.Visible = true;
            ddlCatV.Enabled = true;
            txtCatV.Visible = false;
            txtCatV.Visible = false;
            if (res > 0)
            {
                FillCategorylevel5();
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
    protected void btnClose7_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.AbsoluteUri);
    }
    protected void imgbtnCancelParent_Click(object sender, EventArgs e)
    {
        try
        {
            ddlParentCategory.Enabled = true;
            ddlParentCategory.Visible = true;
            txtParentCategory.Visible = false;
            rfvtxtParentCategory.Enabled = false;
            imgbtnSaveParentCategory.Enabled = false;
            ddlCatII.Enabled = false;
            FillParentCategory();
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
    protected void imgbtnCancelCatII_Click(object sender, EventArgs e)
    {
        try
        {
            ddlCatII.Enabled = true;
            ddlCatII.Visible = true;
            imgbtnSaveCatII.Visible = true;

            txtCatII.Visible = false;
            txtCatII.Enabled = false;
            imgbtnSaveCatII.Enabled = false;
            ddlCateLevelIII.ClearSelection();
            ddlCateLevelIII.Enabled = false;
            txtCatLevelIII.Visible = false;
            ddlCateLevelIV.ClearSelection();
            ddlCateLevelIV.Enabled = false;
            txtCateLevelIV.Visible = false;
            ddlCatV.ClearSelection();
            ddlCatV.Enabled = false;
            txtCatV.Visible = false;
            FillParentCategory();
            ddlParentCategory.SelectedValue = hdnVarCategoryI.Value.ToString();
            FillCategoryLevel2();
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
    protected void imgbtnCancelCatIII_Click(object sender, EventArgs e)
    {
        try
        {
            ddlCateLevelIII.Enabled = true;
            ddlCateLevelIII.Visible = true;
            ddlCateLevelIII.ClearSelection();
            txtCatLevelIII.Visible = false;
            txtCatLevelIII.Enabled = false;
            imgSaveCatIII.Enabled = false;

            ddlCateLevelIV.ClearSelection();
            ddlCateLevelIV.Enabled = false;
            txtCateLevelIV.Visible = false;
            ddlCatV.ClearSelection();
            ddlCatV.Enabled = false;
            txtCatV.Visible = false;
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
    protected void imgbtnCancelCatIV_Click(object sender, EventArgs e)
    {
        try
        {
            ddlCateLevelIV.Enabled = true;
            ddlCateLevelIV.Visible = true;
            ddlCateLevelIV.ClearSelection();
            txtCateLevelIV.Visible = false;
            txtCateLevelIV.Enabled = false;
            imgbtnSaveCateLvlIV.Enabled = false;

            ddlCatV.ClearSelection();
            ddlCatV.Enabled = false;
            txtCatV.Visible = false;
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
    protected void imgbtnCancelCatV_Click(object sender, EventArgs e)
    {
        try
        {
            ddlCatV.Enabled = true;
            ddlCatV.Visible = true;
            txtCatV.Visible = false;
            txtCatV.Enabled = false;
            imgbtnSaveCatV.Enabled = false;
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
    public static string parentcategoryValue;
    public static string CategoryIIValue;
    public static string CategoryIIIValue;
    public static string CategoryIVValue;
    public static string CategoryVValue;
    protected void imgbtnEditParentCategory_Click(object sender, EventArgs e)
    {
        try
        {

            ddlParentCategory.Enabled = true;
            if (ddlParentCategory.SelectedIndex != 0)
            {
                imgbtnCancelParent.Enabled = true;
                imgbtnSaveParentCategory.Enabled = false;
                imgbtnSaveParentCategory.Visible = false;
                imgbtnUpdateParentCategory.Visible = true;
                txtParentCategory.Enabled = true;
                txtParentCategory.Visible = true;
                txtParentCategory.Text = ddlParentCategory.SelectedItem.Text;
                parentcategoryValue = ddlParentCategory.SelectedValue.ToString();
                ddlParentCategory.Visible = false;

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
    protected void imgbtnEditCatII_Click(object sender, EventArgs e)
    {
        try
        {

            ddlCatII.Enabled = true;
            if (ddlCatII.SelectedIndex != 0)
            {
                imgbtnCancelCatII.Enabled = true;
                imgbtnSaveCatII.Enabled = false;
                imgbtnSaveCatII.Visible = false;
                imtbtnUpdateCatII.Visible = true;
                txtCatII.Enabled = true;
                txtCatII.Visible = true;
                txtCatII.Text = ddlCatII.SelectedItem.Text;
                CategoryIIValue = ddlCatII.SelectedValue.ToString();
                ddlCatII.Visible = false;
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
    protected void imgbtnEditCatIII_Click(object sender, EventArgs e)
    {
        try
        {

            ddlCateLevelIII.Enabled = true;
            if (ddlCateLevelIII.SelectedIndex != 0)
            {
                imgbtnCancelCatIII.Enabled = true;
                imgSaveCatIII.Enabled = false;
                imgSaveCatIII.Visible = false;
                imgbtnUpdateCatIII.Visible = true;
                txtCatLevelIII.Enabled = true;
                txtCatLevelIII.Visible = true;
                txtCatLevelIII.Text = ddlCateLevelIII.SelectedItem.Text;
                CategoryIIIValue = ddlCateLevelIII.SelectedValue.ToString();
                ddlCateLevelIII.Visible = false;

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
    protected void imgbtnEditCatLvIV_Click(object sender, EventArgs e)
    {
        try
        {

            ddlCateLevelIV.Enabled = true;
            if (ddlCateLevelIV.SelectedIndex != 0)
            {
                imgbtnCancelCatIV.Enabled = true;
                imgbtnSaveCateLvlIV.Enabled = false;
                imgbtnSaveCateLvlIV.Visible = false;
                imgbtnUpdateCateLvIV.Visible = true;
                txtCateLevelIV.Enabled = true;
                txtCateLevelIV.Visible = true;
                txtCateLevelIV.Text = ddlCateLevelIV.SelectedItem.Text;
                CategoryIVValue = ddlCateLevelIV.SelectedValue.ToString();
                ddlCateLevelIV.Visible = false;

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
    protected void imgbtnEditCatV_Click(object sender, EventArgs e)
    {
        try
        {

            ddlCatV.Enabled = true;
            if (ddlCatV.SelectedIndex != 0)
            {
                imgbtnCancelCatV.Enabled = true;
                imgbtnSaveCatV.Enabled = false;
                imgbtnSaveCatV.Visible = false;
                imgbtnUpdateCatV.Visible = true;
                txtCatV.Enabled = true;
                txtCatV.Visible = true;
                txtCatV.Text = ddlCatV.SelectedItem.Text;
                CategoryVValue = ddlCatV.SelectedValue.ToString();
                ddlCatV.Visible = false;

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
    protected void ddlOrg6_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillRequestTypeCategory(Convert.ToInt64(ddlOrg6.SelectedValue));
    }
    protected void imgbtnUpdateParentCategory_Click(object sender, EventArgs e)
    {
        if (txtParentCategory.Text != null)
        {
            if (txtParentCategory.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
        $"warning_noti('{HttpUtility.JavaScriptStringEncode("Category Can Not be Empty !")}'); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
                return;
            }
            else
            {
                string Categoryref = Session["SDRef"].ToString().Trim() + "||" + txtParentCategory.Text.Trim();
                string sql = "if exists(select * from SD_Category where Categoryref='" + Categoryref + "' " +
                    "and CategoryCodeRef='" + txtParentCategory.Text.Trim() + "' and rowDesc='' and " +
                    " partitionid='1' and inUse='1' and OrgDeskRef='" + ddlOrg6.SelectedValue + "') begin select 'True'  end";
                if (Convert.ToString(database.GetScalarValue(sql)) != "")
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
        $"warning_noti('{HttpUtility.JavaScriptStringEncode("Category Allredy Exists for this Organization!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
                    return;
                }
            }
            int result = UpdateCategory(parentcategoryValue, txtParentCategory.Text.Trim());
            if (result > 0)
            {
                imgbtnSaveParentCategory.Visible = true;
                imgbtnUpdateParentCategory.Visible = false;
                ddlParentCategory.Enabled = true;
                ddlParentCategory.Visible = true;
                txtParentCategory.Visible = false;
                txtParentCategory.Text = "";
                FillParentCategory();
                ddlCatII.ClearSelection();
                ddlCatII.Enabled = false;
                ddlCateLevelIII.ClearSelection();
                ddlCateLevelIII.Enabled = false;
                ddlCateLevelIV.ClearSelection();
                ddlCateLevelIV.Enabled = false;
                ddlCatV.Enabled = false;
                ddlCatV.ClearSelection();

            }
        }
    }
    protected int UpdateCategory(string PrevCategoryName, string UpdatedCategoryName)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {

            using (SqlCommand cmd = new SqlCommand("SD_spAddCategory", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DeskRef", Session["SDRef"].ToString());
                cmd.Parameters.AddWithValue("@Categoryref", Session["SDRef"].ToString().Trim() + "||" + UpdatedCategoryName);
                cmd.Parameters.AddWithValue("@CategoryCodeRef", UpdatedCategoryName);
                cmd.Parameters.AddWithValue("@CategoryUpdateref", PrevCategoryName);
                cmd.Parameters.AddWithValue("@OrgDeskRef", ddlOrg6.SelectedValue);
                cmd.Parameters.AddWithValue("@Option", "UpdateCategory");
                con.Open();
                int res = cmd.ExecuteNonQuery();
                return res;
            }
        }

    }
    protected int UpdateChildCategory(string PrevCategoryName, string UpdatedCategoryName, string ParentCategoryName)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand("SD_spAddCategory", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DeskRef", Session["SDRef"].ToString());
                cmd.Parameters.AddWithValue("@Categoryref", ParentCategoryName + "||" + UpdatedCategoryName);
                cmd.Parameters.AddWithValue("@CategoryCodeRef", UpdatedCategoryName);
                cmd.Parameters.AddWithValue("@CategoryUpdateref", PrevCategoryName);
                cmd.Parameters.AddWithValue("@OrgDeskRef", ddlOrg6.SelectedValue);
                cmd.Parameters.AddWithValue("@Option", "UpdateCategory");
                con.Open();
                int res = cmd.ExecuteNonQuery();
                return res;
            }
        }

    }
    protected void imtbtnUpdateCatII_Click(object sender, EventArgs e)
    {
        if (txtCatII.Text != null)
        {
            int result = UpdateChildCategory(CategoryIIValue, txtCatII.Text.Trim(), ddlParentCategory.SelectedValue);
            if (result > 0)
            {
                ddlCatII.Enabled = true;
                ddlCatII.Visible = true;
                txtCatII.Visible = false;
                txtCatII.Text = "";
                imgbtnSaveCatII.Visible = true;
                imtbtnUpdateCatII.Visible = false;
                hdnVarCategoryI.Value = ddlParentCategory.SelectedValue;
                FillCategoryLevel2();

                ddlCateLevelIII.ClearSelection();
                ddlCateLevelIII.Enabled = false;
                ddlCateLevelIV.ClearSelection();
                ddlCateLevelIV.Enabled = false;
                ddlCatV.Enabled = false;
                ddlCatV.ClearSelection();

            }
        }
    }
    protected void imgbtnUpdateCatIII_Click(object sender, EventArgs e)
    {
        if (txtCatLevelIII.Text != null)
        {
            int result = UpdateChildCategory(CategoryIIIValue, txtCatLevelIII.Text.Trim(), ddlCatII.SelectedValue);
            if (result > 0)
            {
                ddlCateLevelIII.Enabled = true;
                ddlCateLevelIII.Visible = true;
                txtCatLevelIII.Visible = false;
                txtCatLevelIII.Text = "";
                //	FillParentCategory();
                imgSaveCatIII.Visible = true;
                imgbtnUpdateCatIII.Visible = false;
                hdnVarCategoryII.Value = ddlCatII.SelectedValue;
                FillCategoryLevel3();


            }
        }
    }
    protected void imgbtnUpdateCateLvIV_Click(object sender, EventArgs e)
    {
        if (txtCateLevelIV.Text != null)
        {
            int result = UpdateChildCategory(CategoryIVValue, txtCateLevelIV.Text.Trim(), ddlCateLevelIII.SelectedValue);
            if (result > 0)
            {
                ddlCateLevelIV.Enabled = true;
                ddlCateLevelIV.Visible = true;
                txtCateLevelIV.Visible = false;
                txtCateLevelIV.Text = "";
                //	FillParentCategory();
                imgbtnSaveCateLvlIV.Visible = true;
                imgbtnUpdateCateLvIV.Visible = false;
                hdnVarCategoryIII.Value = ddlCateLevelIII.SelectedValue;
                FillCategorylevel4();
                ddlCatV.Enabled = false;
                ddlCatV.ClearSelection();

            }
        }
    }
    protected void imgbtnUpdateCatV_Click(object sender, EventArgs e)
    {
        if (txtCatV.Text != null)
        {
            int result = UpdateChildCategory(CategoryVValue, txtCatV.Text.Trim(), ddlCateLevelIV.SelectedValue);
            if (result > 0)
            {
                ddlCatV.Enabled = true;
                ddlCatV.Visible = true;
                txtCatV.Visible = false;
                txtCatV.Text = "";
                imgbtnSaveCatV.Visible = true;
                imgbtnUpdateCatV.Visible = false;
                hdnVarCategoryIV.Value = ddlCateLevelIV.SelectedValue;
                FillCategorylevel5();
            }
        }
    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string FinalCategory = "";
        if (ddlParentCategory.SelectedValue.ToString() == "" || ddlParentCategory.SelectedValue.ToString() == "0")
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Parent Category Can Not be Empty.');window.location ='/DeskConfiguration.aspx';", true);
            return;
        }
        else
        {
            FinalCategory = ddlParentCategory.SelectedValue.ToString();
            //FinalCategory = !string.IsNullOrWhiteSpace(txtParentCategory.Text.Trim())
            //    ? Session["SDRef"].ToString().Trim() + "||" + txtParentCategory.Text.Trim()
            //    : Session["SDRef"].ToString().Trim() + "||" + ddlParentCategory.SelectedValue.ToString();

        }
        if (ddlCatII.SelectedValue != "0" && ddlCatII.SelectedValue != "")
        {
            FinalCategory = ddlCatII.SelectedValue.ToString();
            //FinalCategory = !string.IsNullOrWhiteSpace(txtCatII.Text.Trim())
            //    ? Session["SDRef"].ToString().Trim() + "||" + txtParentCategory.Text.Trim() + "||" + txtCatII.Text.Trim()
            //    : Session["SDRef"].ToString().Trim() + "||" + ddlParentCategory.SelectedValue.ToString() + "||" + ddlCatII.SelectedValue.ToString();
        }
        if (ddlCateLevelIII.SelectedValue != "0" && ddlCateLevelIII.SelectedValue != "")
        {
            FinalCategory = ddlCateLevelIII.SelectedValue.ToString();
            //FinalCategory = !string.IsNullOrWhiteSpace(txtCatLevelIII.Text.Trim())
            //    ? Session["SDRef"].ToString().Trim() + "||" + txtParentCategory.Text.Trim() + "||" + txtCatII.Text.Trim() + "||" + txtCatLevelIII.Text.Trim()
            //    : Session["SDRef"].ToString().Trim() + "||" + ddlParentCategory.SelectedValue.ToString() + "||" + ddlCatII.SelectedValue.ToString() + "||" + ddlCateLevelIII.SelectedValue.ToString();
        }
        if (ddlCateLevelIV.SelectedValue != "0" && ddlCateLevelIV.SelectedValue != "")
        {
            FinalCategory = ddlCateLevelIV.SelectedValue.ToString();
            //FinalCategory = !string.IsNullOrWhiteSpace(txtCateLevelIV.Text.Trim())
            //    ? Session["SDRef"].ToString().Trim() + "||" + txtParentCategory.Text.Trim() + "||" + txtCatII.Text.Trim() + "||" + txtCatLevelIII.Text.Trim() + "||" + txtCateLevelIV.Text.Trim()
            //    : Session["SDRef"].ToString().Trim() + "||" + ddlParentCategory.SelectedValue.ToString() + "||" + ddlCatII.SelectedValue.ToString() + "||" + ddlCateLevelIII.SelectedValue.ToString() + "||" + ddlCateLevelIV.SelectedValue.ToString();
        }
        if (ddlCatV.SelectedValue != "0" && ddlCatV.SelectedValue != "")
        {
            FinalCategory = ddlCatV.SelectedValue.ToString();
            //FinalCategory = !string.IsNullOrWhiteSpace(txtCatV.Text.Trim())
            //    ? Session["SDRef"].ToString().Trim() + "||" + txtParentCategory.Text.Trim() + "||" + txtCatII.Text.Trim() + "||" + txtCatLevelIII.Text.Trim() + "||" + txtCateLevelIV.Text.Trim() + "||" + txtCatV.Text.Trim()
            //    : Session["SDRef"].ToString().Trim() + "||" + ddlParentCategory.SelectedValue.ToString() + "||" + ddlCatII.SelectedValue.ToString() + "||" + ddlCateLevelIII.SelectedValue.ToString() + "||" + ddlCateLevelIV.SelectedValue.ToString() + "||" + ddlCatV.SelectedValue.ToString();
        }
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand("SD_spAddCategory", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DeskRef", Session["SDRef"].ToString());
                    cmd.Parameters.AddWithValue("@CategoryUpdateref", FinalCategory);
                    cmd.Parameters.AddWithValue("@ResponseTime", txtRespCat.Text.Trim());
                    cmd.Parameters.AddWithValue("@ResolutionTime", txtReslCat.Text.Trim());
                    cmd.Parameters.AddWithValue("@OrgDeskRef", ddlOrg6.SelectedValue);
                    cmd.Parameters.AddWithValue("@Option", "UpdateCategoryRes");
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Response and Resolution Added Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ warning_noti('{HttpUtility.JavaScriptStringEncode("Error Something Went Wrong !")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                    }
                }
            }
        }
        catch (Exception ex) { ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ warning_noti('{HttpUtility.JavaScriptStringEncode("" + ex.ToString() + "")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true); }
    }
    protected void lnkPreviousPriority_Click(object sender, EventArgs e)
    {
        CurrentStep = 6;
        DataBind();
        Session["CurrentStep"] = CurrentStep;
        cleardata();
        lnkNextPriority_Click(null, null);
        pnlAddPriority.Visible = true;
        pnlCategory.Visible = false;

    }
    protected void lnkNextEmailConfig_Click(object sender, EventArgs e)
    {
        pnlAddResolution.Visible = true;
        pnlCategory.Visible = false;
        CurrentStep = 8;
        Session["CurrentStep"] = CurrentStep;
        DataBind();
        cleardata();
        ViewState["CurrentStep"] = CurrentStep;
        if (Session["UserScope"] != null)
        {
            FillOrganizationResolution(Convert.ToString(Session["OrgNameDesk"]));
            ddlOrg.Enabled = true;
            ddlOrgResolution.Items.FindByValue(Convert.ToString(Session["OrgNameDesk"])).Selected = true;
            FillRequestTypeResolution(Convert.ToInt64(ddlOrgResolution.SelectedValue));
            ddlRequestTypeResolution.SelectedValue = Convert.ToString(Session["ReqType"]);
            FillResolutionDetails(ddlOrgResolution.SelectedValue);
        }
        else
        {
            Response.Redirect("/Default.aspx");
        }
    }
    #endregion Add Category End

    #region Resolution Add
    protected void GridFormat8(DataTable dt)
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
    private void FillResolutionDetailsCustomer()
    {
        try
        {
            DataTable SD_Resolution = new FillSDFields().FillResolutionCustomer(Session["SD_OrgID"].ToString());
            if (SD_Resolution.Rows.Count > 0)
            {
                this.gvResolution.DataSource = (object)SD_Resolution;
                this.gvResolution.DataBind();
            }
            else
            {
                this.gvResolution.DataSource = (object)null;
                this.gvResolution.DataBind();
            }
            //GridFormat8(SD_Resolution);
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
    private void FillOrganizationResolution(string orgId = "")
    {

        try
        {

            DataTable SD_Org = new FillSDFields().FillOrganization(orgId);

            ddlOrgResolution.DataSource = SD_Org;
            ddlOrgResolution.DataTextField = "OrgName";
            ddlOrgResolution.DataValueField = "Org_ID";
            ddlOrgResolution.DataBind();
            ddlOrgResolution.Items.Insert(0, new System.Web.UI.WebControls.ListItem("   -Select Organization-", "0"));


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
    private void FillResolutionDetails(string orgid)
    {
        try
        {

            DataTable SD_Resolution = new FillSDFields().FillResolution(orgid);

            if (SD_Resolution.Rows.Count > 0)
            {
                //  this.lb.Text = dataTable.Rows.Count.ToString();
                this.gvResolution.DataSource = (object)SD_Resolution;
                this.gvResolution.DataBind();
            }
            else
            {
                this.gvResolution.DataSource = (object)null;
                this.gvResolution.DataBind();
            }
            //GridFormat8(SD_Resolution);
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
    protected void ddlRequestTypeResolution_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["SDRef"] = ddlRequestTypeResolution.SelectedValue.ToString();
    }
    private void FillRequestTypeResolution(long OrgId)
    {
        try
        {
            DataTable RequestType = new SDTemplateFileds().FillRequestType(OrgId);
            ddlRequestTypeResolution.DataSource = RequestType;
            ddlRequestTypeResolution.DataTextField = "ReqTypeRef";
            ddlRequestTypeResolution.DataValueField = "ReqTypeRef";
            ddlRequestTypeResolution.DataBind();
            ddlRequestTypeResolution.Items.Insert(0, new ListItem("   -Select RequestType-", "0"));
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
    protected void ImgBtnExport9_Click(object sender, EventArgs e)
    {
        try
        {

            DataTable dt = new DataTable("GridView_Data");
            foreach (System.Web.UI.WebControls.TableCell cell in gvResolution.HeaderRow.Cells)
            {
                dt.Columns.Add(cell.Text);
            }
            foreach (GridViewRow row in gvResolution.Rows)
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
                Response.AddHeader("content-disposition", "attachment;filename=ResolutionType.xlsx");
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
    protected void gvResolution_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "DeleteEx")
            {
                //Determine the RowIndex of the Row whose Button was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                //Get the value of column from the DataKeys using the RowIndex.
                string ResolutionRef = gvResolution.DataKeys[rowIndex].Values["ResolutionRef"].ToString();
                string Deskref = gvResolution.Rows[rowIndex].Cells[1].Text;
                string ResolutionName = gvResolution.Rows[rowIndex].Cells[2].Text;
                try
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("SD_spAddResolution", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ResolutionRef", ResolutionRef);
                            cmd.Parameters.AddWithValue("@DeskRef", Deskref);
                            cmd.Parameters.AddWithValue("@Option", "DeleteResolution");
                            cmd.CommandTimeout = 180;
                            int res = cmd.ExecuteNonQuery();
                            if (res > 0)
                            {
                                //ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Saved Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Saved Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                                FillResolutionDetails(ddlOrgResolution.SelectedValue);
                            }
                            con.Close();
                            cleardata();
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
                //Determine the RowIndex of the Row whose Button was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvResolution.Rows[rowIndex];
                //Get the value of column from the DataKeys using the RowIndex.
                string ResolutionRef = gvResolution.DataKeys[rowIndex].Values["ResolutionRef"].ToString();
                Label OrgID = (row.FindControl("lblOrgFk") as Label);
                if (ddlOrgResolution.Items.FindByValue(OrgID.Text.ToString().Trim()) != null)
                {
                    ddlOrgResolution.SelectedValue = OrgID.Text;
                    FillRequestTypeResolution(Convert.ToInt64(OrgID.Text));
                }
                ddlRequestTypeResolution.SelectedValue = gvResolution.Rows[rowIndex].Cells[1].Text;
                txtResolution.Text = gvResolution.Rows[rowIndex].Cells[2].Text;
                txtResolutnDesc.Text = gvResolution.Rows[rowIndex].Cells[3].Text;

                btnInsertResolution.Visible = false;
                btnUpdateResolution.Visible = true;
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
    protected void SaveDataResolution()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SD_spAddResolution", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", r.Next());
                    cmd.Parameters.AddWithValue("@DeskRef", ddlRequestTypeResolution.SelectedItem.Text.Trim());
                    cmd.Parameters.AddWithValue("@ResolutionRef", ddlRequestTypeResolution.SelectedItem.Text.Trim() + "||" + txtResolution.Text.Trim());
                    cmd.Parameters.AddWithValue("@ResolutionCodeRef", txtResolution.Text.Trim());
                    cmd.Parameters.AddWithValue("@ResolutionDesc", txtResolutnDesc.Text);
                    cmd.Parameters.AddWithValue("@OrgDeskRef", ddlOrgResolution.SelectedValue.ToString());
                    cmd.Parameters.Add("@Error", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@Option", "AddResolution");
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    string ErrorChk = cmd.Parameters["@Error"].Value.ToString();
                    if (res > 0)
                    {
                        Session["Resolution"] = "OK";
                        //ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Saved Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Saved Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                        lnkNextEmailConfig_Click(null, null);
                        cleardata();
                    }
                    else
                    {
                        //ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ warning_noti('{HttpUtility.JavaScriptStringEncode("" + ErrorChk + "")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ warning_noti('{HttpUtility.JavaScriptStringEncode("" + ErrorChk + "")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
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
    protected void btnInsertResolution_Click(object sender, EventArgs e)
    {
        SaveDataResolution();
    }
    protected void gvResolution_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (Session["UserScope"].ToString() == "Master")
        {
            e.Row.Cells[5].Visible = true;
            e.Row.Cells[6].Visible = true;
        }

        if (Session["UserScope"].ToString() == "Technician" || Session["UserScope"].ToString() == "Admin")
        {
            e.Row.Cells[5].Visible = true;
            e.Row.Cells[6].Visible = false;

        }
    }
    protected void btnUpdateResolution_Click(object sender, EventArgs e)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand("SD_spAddResolution", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@DeskRef", ddlRequestTypeResolution.SelectedValue);
                    cmd.Parameters.AddWithValue("@ResolutionRef", ddlRequestTypeResolution.SelectedValue.ToString().Trim() + "||" + txtResolution.Text.Trim());
                    cmd.Parameters.AddWithValue("@ResolutionCodeRef", txtResolution.Text.Trim());
                    cmd.Parameters.AddWithValue("@ResolutionDesc", txtResolutnDesc.Text);
                    cmd.Parameters.AddWithValue("@OrgDeskRef", ddlOrgResolution.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@Option", "UpdateResolution");
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        btnInsertResolution.Visible = true;
                        btnUpdateResolution.Visible = false;
                        //ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Updated Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"success_noti('{HttpUtility.JavaScriptStringEncode("Updated Successfully!")}');", true);
                        lnkNextEmailConfig_Click(null, null);
                        cleardata();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ warning_noti('{HttpUtility.JavaScriptStringEncode("Already Exists !")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
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
    protected void btnCancel9_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.AbsoluteUri);
    }
    protected void LoadOrgControl()
    {
        try
        {
            // pnlShowRqstType.Controls.Clear();
            System.Web.UI.Control featuredProduct = Page.LoadControl("/HelpDesk/UserControlOrg.ascx");
            featuredProduct.ID = "12321";
            pnlShowOrg.Controls.Add(featuredProduct);
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
    protected void ddlOrgResolution_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillRequestTypeResolution(Convert.ToInt64(ddlOrgResolution.SelectedValue));
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
    protected void lnkPreviousEmailConfig_Click(object sender, EventArgs e)
    {
        pnlAddResolution.Visible = false;
        lnkNextCategory_Click(null, null);
        cleardata();
    }
    protected void lnkNextSLA_Click(object sender, EventArgs e)
    {
        CurrentStep = 9;
        Session["CurrentStep"] = CurrentStep;
        DataBind();
        cleardata();
        pnlAddSLA.Visible = true;
        pnlAddResolution.Visible = false;
        if (Session["UserScope"] != null)
        {
            FillSLADetails(Convert.ToString(Session["OrgNameDesk"]));
            FillOrganizationSLA();
            ddlOrgSLA.Items.FindByValue(Convert.ToString(Session["OrgNameDesk"])).Selected = true;
            FillRequestTypeSLA(Convert.ToInt64(ddlOrgSLA.SelectedValue));
            ddlReqSLA.SelectedValue = Convert.ToString(Session["ReqType"]);
        }
        else
        {
            Response.Redirect("/Default.aspx");
        }
    }
    #endregion Resolution End

    #region Add SLA Start
    private void FillOrganizationSLA()
    {
        try
        {
            DataTable SD_Org = new FillSDFields().FillOrganization();
            ddlOrgSLA.DataSource = SD_Org;
            ddlOrgSLA.DataTextField = "OrgName";
            ddlOrgSLA.DataValueField = "Org_ID";
            ddlOrgSLA.DataBind();
            ddlOrgSLA.Items.Insert(0, new System.Web.UI.WebControls.ListItem("   -Select Organization-", "0"));
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
    private void FillRequestTypeSLA(long OrgId)
    {
        try
        {
            DataTable RequestType = new SDTemplateFileds().FillRequestType(OrgId);
            ddlReqSLA.DataSource = RequestType;
            ddlReqSLA.DataTextField = "ReqTypeRef";
            ddlReqSLA.DataValueField = "ReqTypeRef";
            ddlReqSLA.DataBind();
            ddlReqSLA.Items.Insert(0, new ListItem("   -Select RequestType-", "0"));
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
    private void FillSLADetails(string Orgid)
    {
        try
        {
            DataTable SD_SLA = new FillSDFields().FillUserSLAdetails(Orgid);
            if (SD_SLA.Rows.Count > 0)
            {
                this.gvSLA.DataSource = (object)SD_SLA;
                this.gvSLA.DataBind();
            }
            else
            {
                this.gvSLA.DataSource = (object)null;
                this.gvSLA.DataBind();
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
    protected void ImgBtnExport10_Click(object sender, EventArgs e)
    {
        try
        {

            DataTable dt = new DataTable("GridView_Data");
            foreach (System.Web.UI.WebControls.TableCell cell in gvSLA.HeaderRow.Cells)
            {
                dt.Columns.Add(cell.Text);
            }
            foreach (GridViewRow row in gvSLA.Rows)
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
                Response.AddHeader("content-disposition", "attachment;filename=SLA.xlsx");
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
    static long SLAID;
    protected void gvSLA_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            string SLA = "";
            if (rbdCategory.Checked)
            {
                SLA = "Category";
            }
            else
            {
                SLA = "Severity";
            }
            if (e.CommandName == "DeleteSLA")
            {
                //Determine the RowIndex of the Row whose Button was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                //Get the value of column from the DataKeys using the RowIndex.
                SLAID = Convert.ToInt32(gvSLA.DataKeys[rowIndex].Values["ID"]);

                try
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("SD_spAddDeskSLA", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ID", SLAID);
                            cmd.Parameters.AddWithValue("@Option", "DeleteDeskSLA");
                            cmd.CommandTimeout = 180;
                            int res = cmd.ExecuteNonQuery();
                            con.Close();
                            FillSLADetails(Convert.ToString(Session["OrgNameDesk"]));
                            if (res > 0)
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Deleted Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                                cleardata();
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

            if (e.CommandName == "UpdateSLA")
            {
                btnInsertSLA.Visible = false;
                btnUpdateSLA.Visible = true;
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvSLA.Rows[rowIndex];
                SLAID = Convert.ToInt32(gvSLA.DataKeys[rowIndex].Values["ID"]);
                SLA = gvSLA.Rows[rowIndex].Cells[1].Text;
                if (SLA == "Category")
                {
                    rbdCategory.Checked = true;
                }
                else if (SLA == "Severity")
                {
                    rbdSeverity.Checked = true;
                }
                else
                {
                    rbdPriority.Checked = true;
                }
                txtSLADescription.Text = gvSLA.Rows[rowIndex].Cells[2].Text;
                Label OrgID = (row.FindControl("lblOrgFk") as Label);
                if (ddlOrgSLA.Items.FindByValue(OrgID.Text.ToString().Trim()) != null)
                {
                    ddlOrgSLA.SelectedValue = OrgID.Text;
                }
                Label ReqSLA = (row.FindControl("lblReqType") as Label);
                if (ddlReqSLA.Items.FindByValue(OrgID.Text.ToString().Trim()) != null)
                {
                    ddlReqSLA.SelectedValue = ReqSLA.Text;
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
    protected void SaveDataSLA()
    {
        try
        {
            if (rbdCategory.Checked == false && rbdSeverity.Checked == false && rbdPriority.Checked == false)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Please Select SLA !');window.location ='/DeskConfiguration.aspx';", true);
                return;
            }
            string SLA = "";
            if (rbdCategory.Checked)
            {
                SLA = "Category";
            }
            else if (rbdSeverity.Checked)
            {
                SLA = "Severity";
            }
            else
            {
                SLA = "Priority";
            }
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SD_spAddDeskSLA", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", r.Next());
                    cmd.Parameters.AddWithValue("@SlaName", SLA);
                    cmd.Parameters.AddWithValue("@SLADesc", txtSLADescription.Text);
                    cmd.Parameters.AddWithValue("@OrgID", ddlOrgSLA.SelectedValue);
                    cmd.Parameters.AddWithValue("@ReqType", ddlReqSLA.SelectedValue);
                    cmd.Parameters.Add("@Error", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@Option", "AddDeskSLA");
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    string ErrorChk = cmd.Parameters["@Error"].Value.ToString();
                    if (res > 0)
                    {
                        Session["SLA"] = "OK";
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Saved Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                        lnkNextSLA_Click(null, null);
                        cleardata();
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ warning_noti('{HttpUtility.JavaScriptStringEncode("" + ErrorChk + "")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
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
    protected void gvSLA_RowDataBound(object sender, GridViewRowEventArgs e)
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
                e.Row.Cells[3].Visible = true;
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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }
    }
    protected void btnInsertSLA_Click(object sender, EventArgs e)
    {
        SaveDataSLA();
    }
    protected void btnUpdateSLA_Click(object sender, EventArgs e)
    {
        try
        {
            string SLA = "";
            if (rbdCategory.Checked)
            {
                SLA = "Category";
            }
            else if (rbdSeverity.Checked)
            {
                SLA = "Severity";
            }
            else
            {
                SLA = "Priority";
            }
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand("SD_spAddDeskSLA", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", SLAID);
                    cmd.Parameters.AddWithValue("@SlaName", SLA);
                    cmd.Parameters.AddWithValue("@SLADesc", txtSLADescription.Text);
                    cmd.Parameters.AddWithValue("@OrgID", ddlOrgSLA.SelectedValue);
                    cmd.Parameters.AddWithValue("@ReqType", ddlReqSLA.SelectedValue);
                    cmd.Parameters.AddWithValue("@Option", "UpdateDeskSLA");
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Updated Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                        //ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"success_noti('{HttpUtility.JavaScriptStringEncode("Updated Successfully!")}');", true);
                        lnkNextSLA_Click(null, null);
                        cleardata();
                        btnInsertSLA.Visible = true;
                        btnUpdateSLA.Visible = false;
                        Response.Redirect("/DeskConfiguration.aspx");
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ warning_noti('{HttpUtility.JavaScriptStringEncode("Already Exists !")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
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
    protected void btnCancel10_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.AbsoluteUri);
    }
    protected void lnkPreviousResolution_Click(object sender, EventArgs e)
    {
        cleardata();
        pnlAddSLA.Visible = false;
        lnkNextEmailConfig_Click(null, null);
    }
    protected void lnkNextDeskConfig_Click(object sender, EventArgs e)
    {
        pnlAdddeskConfig.Visible = true;
        pnlAddSLA.Visible = false;
        CurrentStep = 10;
        Session["CurrentStep"] = CurrentStep;
        DataBind();
        cleardata();
        pnlAddEmailConfig.Visible = false;
        pnlAdddeskConfig.Visible = true;
        if (Session["UserScope"] != null)
        {
            FillSLA(Convert.ToString(Session["OrgNameDesk"]));
            FillCoverageSchedule();
            FillOrganizationDeskConfig();
            ddlOrgDeskConfig.Items.FindByValue(Convert.ToString(Session["OrgNameDesk"])).Selected = true;
            FillRequestTypeDeskConfig(Convert.ToInt64(ddlOrgDeskConfig.SelectedValue));
            ddlRequestTypeDeskConfig.SelectedValue = Convert.ToString(Session["ReqType"]);

            Session["SDRef"] = ddlRequestTypeDeskConfig.SelectedValue.ToString();
            FillSeverity();
            FillStageDeskConfig();
            FillPriority();
            FillCategory1();
            FillSolution(Convert.ToString(Session["OrgNameDesk"]));
            FillSDDetails(Convert.ToString(Session["OrgNameDesk"]));
        }
        else
        {
            Response.Redirect("/Default.aspx");
        }
    }
    #endregion Add SLA End

    #region Add Desk Config Start
    private void FillSDDetails(string orgid)
    {

        try
        {
            DataTable SD_Desk = new FillSDFields().FillSDDetails(orgid);
            if (SD_Desk.Rows.Count > 0)
            {
                //  this.lb.Text = dataTable.Rows.Count.ToString();
                this.gvDesk.DataSource = (object)SD_Desk;
                this.gvDesk.DataBind();
            }
            else
            {
                this.gvDesk.DataSource = (object)null;
                this.gvDesk.DataBind();
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
    private void FillRequestTypeDeskConfig(long OrgID)
    {

        try
        {

            DataTable RequestType = new SDTemplateFileds().FillRequestType(OrgID);

            ddlRequestTypeDeskConfig.DataSource = RequestType;
            ddlRequestTypeDeskConfig.DataTextField = "ReqTypeRef";
            ddlRequestTypeDeskConfig.DataValueField = "ReqTypeRef";
            ddlRequestTypeDeskConfig.DataBind();
            ddlRequestTypeDeskConfig.Items.Insert(0, new System.Web.UI.WebControls.ListItem("   -Select RequestType-", "0"));


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
    private void FillSeverity()
    {

        try
        {

            DataTable SD_Severity = new SDTemplateFileds().FillSeverity(Session["SDRef"].ToString(), ddlOrgDeskConfig.SelectedValue.ToString());

            ddlSeverity.DataSource = SD_Severity;
            ddlSeverity.DataTextField = "Serveritycoderef";
            ddlSeverity.DataValueField = "id";
            ddlSeverity.DataBind();
            ddlSeverity.Items.Insert(0, new System.Web.UI.WebControls.ListItem("   -Select Severity-", "0"));


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
    private void FillSLA(string OrgID)
    {

        try
        {

            DataTable SD_SLA = new FillSDFields().FillUserSLAdetails(OrgID);

            ddlSlA.DataSource = SD_SLA;
            ddlSlA.DataTextField = "SlaName";
            ddlSlA.DataValueField = "ID";
            ddlSlA.DataBind();
            ddlSlA.Items.Insert(0, new System.Web.UI.WebControls.ListItem("   -Select SLA-", "0"));


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
    private void FillCoverageSchedule()
    {

        try
        {
            DataTable SD_Doverage = new FillSDFields().FillCoverageSchdetails();
            ddlCoverageSch.DataSource = SD_Doverage;
            ddlCoverageSch.DataTextField = "ScdhuleName";
            ddlCoverageSch.DataValueField = "ID";
            ddlCoverageSch.DataBind();
            ddlCoverageSch.Items.Insert(0, new System.Web.UI.WebControls.ListItem("   -Select Coverage-", "0"));

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
    private void FillOrganizationDeskConfig()
    {

        try
        {

            DataTable SD_Org = new FillSDFields().FillOrganization();
            ddlOrgDeskConfig.DataSource = SD_Org;
            ddlOrgDeskConfig.DataTextField = "OrgName";
            ddlOrgDeskConfig.DataValueField = "Org_ID";
            ddlOrgDeskConfig.DataBind();
            ddlOrgDeskConfig.Items.Insert(0, new System.Web.UI.WebControls.ListItem("   -Select Organization-", "0"));


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
    private void FillSolution(string OrgId)
    {

        try
        {

            DataTable SD_Priority = new SDTemplateFileds().FillSolutiontype(Session["SDRef"].ToString(), OrgId); ;

            ddlSolutionType.DataSource = SD_Priority;
            ddlSolutionType.DataTextField = "ResolutionCodeRef";
            ddlSolutionType.DataValueField = "id";
            ddlSolutionType.DataBind();
            ddlSolutionType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("   -Select Solution-", "0"));


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
    private void FillPriority()
    {

        try
        {

            DataTable SD_Priority = new SDTemplateFileds().FillPriority(Session["SDRef"].ToString(), ddlOrgDeskConfig.SelectedValue.ToString()); ;

            ddlPriority.DataSource = SD_Priority;
            ddlPriority.DataTextField = "PriorityCodeRef";
            ddlPriority.DataValueField = "id";
            ddlPriority.DataBind();
            ddlPriority.Items.Insert(0, new System.Web.UI.WebControls.ListItem("   -Select Priority--", "0"));

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
    private void FillStatus()
    {

        try
        {

            DataTable SD_Status = new SDTemplateFileds().FillStatus(Session["SDRef"].ToString(), ddlStageDeskConfig.SelectedValue.ToString(), ddlOrgDeskConfig.SelectedValue.ToString());

            ddlStatus.DataSource = SD_Status;
            ddlStatus.DataTextField = "StatusCodeRef";
            ddlStatus.DataValueField = "id";
            ddlStatus.DataBind();
            ddlStatus.Items.Insert(0, new System.Web.UI.WebControls.ListItem("---Select---", "0"));


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
    private void FillStageDeskConfig()
    {

        try
        {

            DataTable SD_Status = new SDTemplateFileds().FillStage(Session["SDRef"].ToString(), ddlOrgDeskConfig.SelectedValue.ToString());
            ddlStageDeskConfig.DataSource = SD_Status;
            ddlStageDeskConfig.DataTextField = "StageCodeRef";
            ddlStageDeskConfig.DataValueField = "id";
            ddlStageDeskConfig.DataBind();
            ddlStageDeskConfig.Items.Insert(0, new System.Web.UI.WebControls.ListItem("---Select---", "0"));


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
                string sql = @"SELECT CategoryCodeRef,
           Categoryref FROM [dbo].fnGetCategoryFullPathForDesk('" + ddlRequestTypeDeskConfig.SelectedValue + "','" + ddlOrgDeskConfig.SelectedValue + "', 1) where Level=1   order by Categoryref asc";
                using (SqlCommand cmd = new SqlCommand(sql, con))
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
                                ddlCategory1.Items.Insert(0, new ListItem("---Select---", "0"));
                            }

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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }
    }
    private DataTable FillCategoryLevelDeskConfig(string category, int categoryLevel)
    {
        try
        {
            DataTable dtFillCategory = new DataTable();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"SELECT * FROM [dbo].[fn_GetCategoryChildrenByRef]('" + ddlCategory1.SelectedValue + "', 1,'" + ddlOrgDeskConfig.SelectedValue + "') where level='" + categoryLevel + "'  order by Categoryref asc", con))
                {
                    cmd.CommandType = CommandType.Text;
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.SelectCommand.CommandTimeout = 180;
                        adp.Fill(dtFillCategory);
                    }
                }
            }
            return dtFillCategory;
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
            return null;
        }
    }
    protected void ddlCategory1_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            hdnCategoryID.Value = ddlCategory1.SelectedValue.ToString();

            DataTable FillCategoryLevel2 = new DataTable();
            FillCategoryLevel2 = FillCategoryLevelDeskConfig(ddlCategory1.SelectedValue, 2);
            if (FillCategoryLevel2.Rows.Count > 0)
            {
                ddlCategory2.DataSource = FillCategoryLevel2;
                ddlCategory2.DataTextField = "CategoryCodeRef";
                ddlCategory2.DataValueField = "Categoryref";
                ddlCategory2.DataBind();
                ddlCategory2.Items.Insert(0, new ListItem("---Select---", "0"));

                lblCategory2.Visible = true;
                ddlCategory2.Visible = true;
                ddlCategory2.Enabled = true;
                ddlCategory3.Visible = true;
                ddlCategory4.Visible = true;
                ddlCategory5.Visible = true;
                lblCategory3.Visible = true;
                lblCategory4.Visible = true;
                lblCategory5.Visible = true;
            }
            else
            {
                ddlCategory2.ClearSelection();
                lblCategory2.Visible = false;
                ddlCategory2.Visible = false;
                ddlCategory2.Enabled = false;
                ddlCategory3.Enabled = false;
                ddlCategory3.ClearSelection();
                ddlCategory4.Enabled = false;
                ddlCategory4.ClearSelection();
                ddlCategory5.Enabled = false;
                ddlCategory5.ClearSelection();
                ddlCategory3.Visible = false;
                ddlCategory4.Visible = false;
                ddlCategory5.Visible = false;
                lblCategory2.Visible = false;
                lblCategory3.Visible = false;
                lblCategory4.Visible = false;
                lblCategory5.Visible = false;
                FillCategoryLevel2 = null;
                //ddlCatII.DataSource = null;
                //  ddlCatII.DataBind();
                // ddlCatII.Items.Insert(0, new ListItem("----------Select Category Level 2----------", "0"));
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
    protected void ddlCategory2_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            hdnCategoryID.Value = ddlCategory2.SelectedValue.ToString();
            DataTable FillCategoryLevel3 = FillCategoryLevelDeskConfig(ddlCategory1.SelectedValue, 3);
            if (FillCategoryLevel3.Rows.Count > 0)
            {
                lblCategory3.Visible = true;
                ddlCategory3.Visible = true;
                ddlCategory3.Enabled = true;
                ddlCategory3.DataSource = FillCategoryLevel3;
                ddlCategory3.DataTextField = "CategoryCodeRef";
                ddlCategory3.DataValueField = "Categoryref";
                ddlCategory3.DataBind();
                ddlCategory3.Items.Insert(0, new ListItem("---Select---", "0"));
                ddlCategory3.Visible = true;
                ddlCategory4.Visible = true;
                ddlCategory5.Visible = true;
                lblCategory3.Visible = true;
                lblCategory4.Visible = true;
                lblCategory5.Visible = true;

            }
            else
            {
                ddlCategory3.ClearSelection();
                //ddlCategory3.Enabled = false;
                lblCategory3.Visible = false;
                ddlCategory3.Visible = false;
                ddlCategory3.DataSource = null;
                ddlCategory3.DataBind();
                ddlCategory4.ClearSelection();
                ddlCategory4.Enabled = false;
                ddlCategory4.Visible = false;
                lblCategory4.Visible = false;
                ddlCategory5.ClearSelection();
                ddlCategory5.Enabled = false;
                ddlCategory5.Visible = false;
                lblCategory5.Visible = false;
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
    protected void ddlCategory3_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            hdnCategoryID.Value = ddlCategory3.SelectedValue.ToString();
            DataTable FillCategoryLevel4 = FillCategoryLevelDeskConfig(ddlCategory1.SelectedValue, 4);
            if (FillCategoryLevel4.Rows.Count > 0)
            {
                ddlCategory4.DataSource = FillCategoryLevel4;
                ddlCategory4.DataTextField = "CategoryCodeRef";
                ddlCategory4.DataValueField = "Categoryref";
                ddlCategory4.DataBind();
                ddlCategory4.Items.Insert(0, new ListItem("---Select---", "0"));
                ddlCategory4.Enabled = true;
                ddlCategory4.Visible = true;
                lblCategory4.Visible = true;
                ddlCategory5.Visible = true;
                lblCategory4.Visible = true;
                lblCategory5.Visible = true;
            }
            else
            {
                ddlCategory4.DataSource = null;
                ddlCategory4.DataBind();

                ddlCategory4.ClearSelection();
                ddlCategory4.Enabled = false;
                ddlCategory4.Visible = false;
                lblCategory4.Visible = false;
                ddlCategory5.ClearSelection();
                ddlCategory5.Enabled = false;
                ddlCategory5.Visible = false;
                lblCategory5.Visible = false;
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
    protected void ddlCategory4_SelectedIndexChanged(object sender, EventArgs e)
    {

        try
        {
            hdnCategoryID.Value = ddlCategory4.SelectedValue.ToString();
            DataTable FillCategoryLevel4 = FillCategoryLevelDeskConfig(ddlCategory1.SelectedValue, 5);
            if (FillCategoryLevel4.Rows.Count > 0)
            {
                ddlCategory5.DataSource = FillCategoryLevel4;
                ddlCategory5.DataTextField = "CategoryCodeRef";
                ddlCategory5.DataValueField = "Categoryref";
                ddlCategory5.DataBind();
                ddlCategory5.Items.Insert(0, new ListItem("---Select---", "0"));
                ddlCategory5.Enabled = true;
            }
            else
            {
                ddlCategory5.DataSource = null;
                ddlCategory5.DataBind();

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
                ddlCategory2.Items.Insert(0, new System.Web.UI.WebControls.ListItem("---Select---", "0"));
            }
            else
            {
                ddlCategory2.ClearSelection();
                ddlCategory2.Enabled = false;
                FillCategoryLevel2 = null;
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
                inEr.InsertErrorLogsF(Session["UserName"].ToString()
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
                ddlCategory3.Items.Insert(0, new System.Web.UI.WebControls.ListItem("---Select---", "0"));
            }
            else
            {
                ddlCategory3.ClearSelection();
                //ddlCategory3.Enabled = false;
                ddlCategory3.DataSource = null;
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
                inEr.InsertErrorLogsF(Session["UserName"].ToString()
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
                ddlCategory4.Items.Insert(0, new System.Web.UI.WebControls.ListItem("---Select---", "0"));
            }
            else
            {
                ddlCategory4.DataSource = null;
                ddlCategory4.DataBind();

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
                inEr.InsertErrorLogsF(Session["UserName"].ToString()
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
                ddlCategory5.Items.Insert(0, new System.Web.UI.WebControls.ListItem("---Select---", "0"));
            }
            else
            {
                ddlCategory5.DataSource = null;
                ddlCategory5.DataBind();

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
                inEr.InsertErrorLogsF(Session["UserName"].ToString()
    , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }

        }
    }
    private DataTable FillCategoryLevel(int categoryLevel)
    {
        try
        {
            DataTable dtFillCategory = new DataTable();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"SELECT * FROM [dbo].[fn_GetCategoryChildrenByRef]('" + ddlCategory1.SelectedValue + "', 1,'" + ddlOrgDeskConfig.SelectedValue + "') where level='" + categoryLevel + "'  order by Categoryref asc", con))
                {
                    cmd.CommandType = CommandType.Text;
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.SelectCommand.CommandTimeout = 180;
                        adp.Fill(dtFillCategory);
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
                inEr.InsertErrorLogsF(Session["UserName"].ToString()
    , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }

            return null;
        }
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SD_spServDeskDefn", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters.AddWithValue("@DeskRef", ddlRequestTypeDeskConfig.SelectedValue);
                    cmd.Parameters.AddWithValue("@DeskDesc", txtSDDescription.Text);
                    cmd.Parameters.AddWithValue("@sdPrefix", txtSDPrefix.Text);
                    cmd.Parameters.AddWithValue("@sdStageFK", ddlStageDeskConfig.SelectedValue);
                    cmd.Parameters.AddWithValue("@sdStatusFK", ddlStatus.SelectedValue);
                    cmd.Parameters.AddWithValue("@sdSeverityFK", ddlSeverity.SelectedValue);
                    cmd.Parameters.AddWithValue("@sdPriorityFK", ddlPriority.SelectedValue);
                    cmd.Parameters.AddWithValue("@sdSolutionTypeFK", ddlSolutionType.SelectedValue);
                    cmd.Parameters.AddWithValue("@sdCategoryRef", hdnCategoryID.Value);
                    cmd.Parameters.AddWithValue("@OrgFk", ddlOrgDeskConfig.SelectedValue);
                    cmd.Parameters.AddWithValue("@SLA", ddlSlA.SelectedValue);
                    cmd.Parameters.AddWithValue("@CoverageSch", ddlCoverageSch.SelectedValue);
                    cmd.Parameters.AddWithValue("@autoArchiveTime", txtArchiveTime.Text);
                    cmd.Parameters.AddWithValue("@Option", "UpdateServDeskDefn");
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        //Session["Popup"] = "Insert";
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Updated Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                        //ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"success_noti('{HttpUtility.JavaScriptStringEncode("Updated Successfully!")}');", true);
                        lnkNextDeskConfig_Click(null, null);
                        Response.Redirect("/DeskConfiguration.aspx");
                        btnInsert.Visible = true;
                        btnUpdate.Visible = false;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ warning_noti('{HttpUtility.JavaScriptStringEncode("Already Exists !")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
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
    protected void btnInsert_Click(object sender, EventArgs e)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand("SD_spServDeskDefn", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ID", r.Next());
                    cmd.Parameters.AddWithValue("@DeskRef", ddlRequestTypeDeskConfig.SelectedValue);
                    cmd.Parameters.AddWithValue("@DeskDesc", txtSDDescription.Text);
                    cmd.Parameters.AddWithValue("@sdPrefix", txtSDPrefix.Text);
                    cmd.Parameters.AddWithValue("@sdStageFK", ddlStageDeskConfig.SelectedValue);
                    cmd.Parameters.AddWithValue("@sdStatusFK", ddlStatus.SelectedValue);
                    cmd.Parameters.AddWithValue("@sdPriorityFK", ddlPriority.SelectedValue);
                    cmd.Parameters.AddWithValue("@sdSolutionTypeFK", ddlSolutionType.SelectedValue);
                    cmd.Parameters.AddWithValue("@sdCategoryRef", hdnCategoryID.Value);
                    cmd.Parameters.AddWithValue("@templateName", "Hitachi " + ddlRequestTypeDeskConfig.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@sdSeverityFK", ddlSeverity.SelectedValue);
                    //cmd.Parameters.AddWithValue("@sdRolePermissionFK", ddlSeverity.SelectedValue);
                    cmd.Parameters.AddWithValue("@autoArchiveTime", txtArchiveTime.Text);
                    cmd.Parameters.AddWithValue("@SLA", ddlSlA.SelectedValue);
                    cmd.Parameters.AddWithValue("@CoverageSch", ddlCoverageSch.SelectedValue);
                    cmd.Parameters.AddWithValue("@OrgFk", ddlOrgDeskConfig.SelectedValue);
                    cmd.Parameters.Add("@Error", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@Option", "AddServDeskDefn");
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    string ErrorChk = cmd.Parameters["@Error"].Value.ToString();
                    if (res > 0)
                    {
                        Session["Desktemp"] = "OK";
                        //lnkNextDeskConfig_Click(null, null);
                        //ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Saved Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                        // ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Saved Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);

                        lnkNextCustomFields_Click(null, null);
                        //Response.Redirect("/DeskConfiguration.aspx");
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ warning_noti('{HttpUtility.JavaScriptStringEncode("" + ErrorChk + "")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                        lnkNextDeskConfig_Click(null, null);
                        FillSDDetails(Convert.ToString(Session["OrgNameDesk"]));
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
                FillSDDetails(Convert.ToString(Session["OrgNameDesk"]));
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
    protected void ddlRequestTypeDeskConfig_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Session["SDRef"] = ddlRequestTypeDeskConfig.SelectedValue.ToString();
            FillSeverity();
            FillStageDeskConfig();
            FillPriority();
            FillCategory1();
            FillSolution(ddlOrgDeskConfig.SelectedValue);

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
    protected void btnClose11_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.AbsoluteUri);
    }
    protected void gvDesk_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "DeleteEx")
            {
                //Determine the RowIndex of the Row whose Button was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                //Get the value of column from the DataKeys using the RowIndex.
                ID = Convert.ToInt32(gvDesk.DataKeys[rowIndex].Values["ID"]);
                string Deskref = gvDesk.Rows[rowIndex].Cells[1].Text;
                string PriorityName = gvDesk.Rows[rowIndex].Cells[2].Text;
                try
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("SD_spServDeskDefn", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ID", ID);

                            cmd.Parameters.AddWithValue("@Option", "DeleteServDeskDefn");
                            cmd.CommandTimeout = 180;
                            int res = cmd.ExecuteNonQuery();
                            if (res > 0)
                            {
                                //Session["Popup"] = "Delete";
                                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Deleted Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                                //ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"success_noti('{HttpUtility.JavaScriptStringEncode("Deleted Successfully!")}');", true);


                            }
                            con.Close();
                        }
                    }
                    lnkNextDeskConfig_Click(null, null);
                    //FillSDDetails(Convert.ToString(Session["OrgNameDesk"]));
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
            if (e.CommandName == "EditDesk")
            {
                long CategoryFk;
                btnInsert.Visible = false;
                btnUpdate.Visible = true;
                //Determine the RowIndex of the Row whose Button was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvDesk.Rows[rowIndex];
                //Get the value of column from the DataKeys using the RowIndex.
                ID = Convert.ToInt32(gvDesk.DataKeys[rowIndex].Values["ID"]);
                Label OrgID = (row.FindControl("lblOrgFk") as Label);
                if (ddlOrgDeskConfig.Items.FindByValue(OrgID.Text.ToString().Trim()) != null)
                {
                    ddlOrgDeskConfig.SelectedValue = OrgID.Text;
                    FillRequestTypeDeskConfig(Convert.ToInt64(OrgID.Text));
                    ddlRequestTypeDeskConfig.SelectedValue = gvDesk.Rows[rowIndex].Cells[4].Text;
                    ddlRequestTypeDeskConfig.Items.FindByText(ddlRequestTypeDeskConfig.SelectedValue).Selected = true;
                    ddlRequestTypeDeskConfig_SelectedIndexChanged(sender, e);
                }

                txtSDPrefix.Text = gvDesk.Rows[rowIndex].Cells[5].Text;
                txtSDDescription.Text = gvDesk.Rows[rowIndex].Cells[6].Text;
                txtArchiveTime.Text = gvDesk.Rows[rowIndex].Cells[13].Text;
                Label Priority = (row.FindControl("lblSDPriorityFk") as Label);
                Label Category = (row.FindControl("lblSDCategoryFk") as Label);
                if (string.IsNullOrEmpty(Category.Text.ToString()))
                {
                }
                else
                {
                    CategoryFk = Convert.ToInt64((Convert.ToString(Category.Text.ToString())));
                    DataTable Category1 = new SDTemplateFileds().GetTicketCategory(CategoryFk, 1);
                    DataTable Category2 = new SDTemplateFileds().GetTicketCategory(CategoryFk, 2);
                    DataTable Category3 = new SDTemplateFileds().GetTicketCategory(CategoryFk, 3);
                    DataTable Category4 = new SDTemplateFileds().GetTicketCategory(CategoryFk, 4);
                    DataTable Category5 = new SDTemplateFileds().GetTicketCategory(CategoryFk, 5);
                    if (Category1.Rows.Count > 0)
                    {
                        string s;
                        s = Category1.Rows[0]["ref"].ToString();
                        //	
                        if (ddlCategory1.Items.FindByValue(s) != null)
                        {
                            ddlCategory1.Items.FindByValue(s).Selected = true;
                            hdnCategoryID.Value = ddlCategory1.SelectedValue;
                        }
                    }
                    if (Category2.Rows.Count > 0)
                    {
                        FillCategory2();
                        string s2;
                        s2 = Category2.Rows[0]["ref"].ToString();
                        if (ddlCategory2.Items.FindByValue(s2) != null)
                        {
                            ddlCategory2.Items.FindByValue(s2).Selected = true;
                            hdnCategoryID.Value = ddlCategory2.SelectedValue;
                        }
                    }

                    if (Category3.Rows.Count > 0)
                    {
                        FillCategory3();
                        string s3;
                        s3 = Category3.Rows[0]["ref"].ToString();

                        if (ddlCategory3.Items.FindByValue(s3) != null)
                        {
                            ddlCategory3.Items.FindByValue(s3).Selected = true;
                            hdnCategoryID.Value = ddlCategory3.SelectedValue;
                        }
                    }

                    if (Category4.Rows.Count > 0)
                    {
                        FillCategory4();
                        string s4;
                        s4 = Category4.Rows[0]["ref"].ToString();

                        if (ddlCategory4.Items.FindByValue(s4) != null)
                        {
                            ddlCategory4.Items.FindByValue(s4).Selected = true;
                            hdnCategoryID.Value = ddlCategory4.SelectedValue;
                        }
                    }

                    if (Category5.Rows.Count > 0)
                    {
                        FillCategory5();
                        string s5;
                        s5 = Category5.Rows[0]["ref"].ToString();

                        if (ddlCategory5.Items.FindByValue(s5) != null)
                        {
                            ddlCategory5.Items.FindByValue(s5).Selected = true;
                            hdnCategoryID.Value = ddlCategory5.SelectedValue;
                        }
                    }
                }
                if (ddlPriority.Items.FindByValue(Priority.Text.ToString().Trim()) != null)
                {
                    ddlPriority.SelectedValue = Priority.Text;
                }
                Label Stage = (row.FindControl("lblSDStageFk") as Label);
                if (ddlStageDeskConfig.Items.FindByValue(Stage.Text.ToString().Trim()) != null)
                {
                    ddlStageDeskConfig.SelectedValue = Stage.Text;
                }
                FillStatus();
                Label Status = (row.FindControl("lblSDStatusFk") as Label);
                if (ddlStatus.Items.FindByValue(Status.Text.ToString().Trim()) != null)
                {
                    ddlStatus.SelectedValue = Status.Text;
                }
                Label severity = (row.FindControl("lblSDSeverityFk") as Label);
                if (ddlSeverity.Items.FindByValue(severity.Text.ToString().Trim()) != null)
                {
                    ddlSeverity.SelectedValue = severity.Text;
                }
                Label solution = (row.FindControl("lblsdSolutionTypeFK") as Label);
                if (ddlSolutionType.Items.FindByValue(solution.Text.ToString().Trim()) != null)
                {
                    ddlSolutionType.SelectedValue = solution.Text;
                }

                Label lblSLAid = (row.FindControl("lblSLAid") as Label);
                if (ddlSlA.Items.FindByValue(lblSLAid.Text.ToString().Trim()) != null)
                {
                    ddlSlA.SelectedValue = lblSLAid.Text;
                }
                Label CvrgID = (row.FindControl("lblCvrgID") as Label);
                if (ddlCoverageSch.Items.FindByValue(CvrgID.Text.ToString().Trim()) != null)
                {
                    ddlCoverageSch.SelectedValue = CvrgID.Text;
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
    protected void btnCancel11_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.AbsoluteUri);
    }
    protected void gvDesk_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (Session["UserScope"].ToString() == "Master")
            {
                e.Row.Cells[0].Visible = true;
                e.Row.Cells[1].Visible = true;
            }

            if (Session["UserScope"].ToString() == "Technician" || Session["UserScope"].ToString() == "Admin")
            {
                e.Row.Cells[0].Visible = true;
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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }
    }
    protected void ImgBtnExport12_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = new DataTable("GridView_Data");
            foreach (System.Web.UI.WebControls.TableCell cell in gvDesk.HeaderRow.Cells)
            {
                dt.Columns.Add(cell.Text);
            }
            foreach (GridViewRow row in gvDesk.Rows)
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
                Response.AddHeader("content-disposition", "attachment;filename=DeskDetail.xlsx");
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
    protected void ClearFields()
    {
        try
        {
            ddlRequestType.ClearSelection();
            txtArchiveTime.Text = "";
            txtSDDescription.Text = "";
            txtSDPrefix.Text = "";
            ddlSeverity.ClearSelection();
            ddlPriority.ClearSelection();
            ddlSolutionType.ClearSelection();
            ddlStatus.ClearSelection();
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
    protected void ddlOrgDeskConfig_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillRequestTypeDeskConfig(Convert.ToInt64(ddlOrgDeskConfig.SelectedValue));
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
    protected void ddlStageDeskConfig_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillStatus();
    }
    protected void lnkNextCustomFields_Click(object sender, EventArgs e)
    {
        pnlAddEmailConfig.Visible = true;
        pnlAdddeskConfig.Visible = false;
        CurrentStep = 11;
        Session["CurrentStep"] = CurrentStep;
        cleardata();
        DataBind();
        ViewState["CurrentStep"] = CurrentStep;
        if (Session["UserScope"] != null)
        {
            FillEmailConfigDetails(Convert.ToString(Session["OrgNameDesk"]));
            FillOrganizationEmailConfig();
            ddlOrgEmailConfig.Items.FindByValue(Convert.ToString(Session["OrgNameDesk"])).Selected = true;
        }
        else
        {
            Response.Redirect("/Default.aspx");
        }

    }
    protected void lnkPreviousSLA_Click(object sender, EventArgs e)
    {
        cleardata();
        pnlAddSLA.Visible = true;
        pnlAdddeskConfig.Visible = false;
        lnkNextSLA_Click(null, null);
    }
    #endregion Add Desk Config End

    #region Add Custom Fields
    //private void FillSDCustomFieldsCustomer()
    //{
    //    try
    //    {
    //        DataTable SD_SDCustomFields = new FillSDFields().FillSDCustomFieldsCustomer(Session["SD_OrgID"].ToString());
    //        if (SD_SDCustomFields.Rows.Count > 0)
    //        {
    //            this.gvSDCustomFields.DataSource = (object)SD_SDCustomFields;
    //            this.gvSDCustomFields.DataBind();
    //        }
    //        else
    //        {
    //            this.gvSDCustomFields.DataSource = (object)null;
    //            this.gvSDCustomFields.DataBind();
    //        }


    //    }
    //    catch (ThreadAbortException e2)
    //    {
    //        Console.WriteLine("Exception message: {0}", e2.Message);
    //        Thread.ResetAbort();
    //    }

    //    catch (Exception ex)
    //    {
    //        if (ex.ToString().Contains("System.Threading.Thread.AbortInternal()"))
    //        {

    //        }
    //        else
    //        {
    //            var st = new StackTrace(ex, true);
    //            // Get the top stack frame
    //            var frame = st.GetFrame(0);
    //            // Get the line number from the stack frame
    //            var line = frame.GetFileLineNumber();
    //            inEr.InsertErrorLogsF(Session["UserName"].ToString()
    //, " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
    //            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

    //        }
    //    }
    //}
    //private void FillOrganizationCustomField()
    //{
    //    try
    //    {
    //        DataTable SD_Org = new FillSDFields().FillOrganization();
    //        ddlOrgCustomField.DataSource = SD_Org;
    //        ddlOrgCustomField.DataTextField = "OrgName";
    //        ddlOrgCustomField.DataValueField = "Org_ID";
    //        ddlOrgCustomField.DataBind();
    //        ddlOrgCustomField.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----------Select Organization----------", "0"));
    //    }
    //    catch (ThreadAbortException e2)
    //    {
    //        Console.WriteLine("Exception message: {0}", e2.Message);
    //        Thread.ResetAbort();
    //    }
    //    catch (Exception ex)
    //    {
    //        if (ex.ToString().Contains("System.Threading.Thread.AbortInternal()"))
    //        {

    //        }
    //        else
    //        {
    //            var st = new StackTrace(ex, true);
    //            // Get the top stack frame
    //            var frame = st.GetFrame(0);
    //            // Get the line number from the stack frame
    //            var line = frame.GetFileLineNumber();
    //            inEr.InsertErrorLogsF(Session["UserName"].ToString()
    //, " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
    //            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

    //        }
    //    }
    //}
    //private void FillSDCustomFieldsDetails()
    //{

    //    try
    //    {

    //        DataTable SD_SDCustomFields = new FillSDFields().FillSDCustomFields(); 

    //        if (SD_SDCustomFields.Rows.Count > 0)
    //        {
    //            //  this.lb.Text = dataTable.Rows.Count.ToString();
    //            this.gvSDCustomFields.DataSource = (object)SD_SDCustomFields;
    //            this.gvSDCustomFields.DataBind();
    //        }
    //        else
    //        {
    //            this.gvSDCustomFields.DataSource = (object)null;
    //            this.gvSDCustomFields.DataBind();
    //        }


    //    }
    //    catch (ThreadAbortException e2)
    //    {
    //        Console.WriteLine("Exception message: {0}", e2.Message);
    //        Thread.ResetAbort();
    //    }

    //    catch (Exception ex)
    //    {
    //        if (ex.ToString().Contains("System.Threading.Thread.AbortInternal()"))
    //        {

    //        }
    //        else
    //        {
    //            var st = new StackTrace(ex, true);
    //            // Get the top stack frame
    //            var frame = st.GetFrame(0);
    //            // Get the line number from the stack frame
    //            var line = frame.GetFileLineNumber();
    //            inEr.InsertErrorLogsF(Session["UserName"].ToString()
    //, " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
    //            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

    //        }
    //    }
    //}
    //protected void ddlRequestTypeCustomField_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    Session["SDRef"] = ddlRequestTypeCustomField.SelectedValue.ToString();
    //}
    //private void FillRequestTypeCustomField(long OrgID)
    //{

    //    try
    //    {

    //        DataTable RequestType = new SDTemplateFileds().FillRequestType(OrgID);
    //        ddlRequestTypeCustomField.DataSource = RequestType;
    //        ddlRequestTypeCustomField.DataTextField = "ReqTypeRef";
    //        ddlRequestTypeCustomField.DataValueField = "ReqTypeRef";
    //        ddlRequestTypeCustomField.DataBind();
    //        ddlRequestTypeCustomField.Items.Insert(0, new ListItem("----------Select RequestType----------", "0"));
    //    }
    //    catch (ThreadAbortException e2)
    //    {
    //        Console.WriteLine("Exception message: {0}", e2.Message);
    //        Thread.ResetAbort();
    //    }
    //    catch (Exception ex)
    //    {
    //        if (ex.ToString().Contains("System.Threading.Thread.AbortInternal()"))
    //        {

    //        }
    //        else
    //        {
    //            var st = new StackTrace(ex, true);
    //            // Get the top stack frame
    //            var frame = st.GetFrame(0);
    //            // Get the line number from the stack frame
    //            var line = frame.GetFileLineNumber();
    //            inEr.InsertErrorLogsF(Session["UserName"].ToString()
    //, " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
    //            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

    //        }
    //    }
    //}
    //protected void ImgBtnExport13_Click(object sender, ImageClickEventArgs e)
    //{
    //    try
    //    {

    //        DataTable dt = new DataTable("GridView_Data");
    //        foreach (System.Web.UI.WebControls.TableCell cell in gvSDCustomFields.HeaderRow.Cells)
    //        {
    //            dt.Columns.Add(cell.Text);
    //        }
    //        foreach (GridViewRow row in gvSDCustomFields.Rows)
    //        {
    //            dt.Rows.Add();
    //            for (int i = 0; i < row.Cells.Count; i++)
    //            {
    //                dt.Rows[dt.Rows.Count - 1][i] = row.Cells[i].Text;
    //            }
    //        }
    //        using (XLWorkbook wb = new XLWorkbook())
    //        {
    //            wb.Worksheets.Add(dt);

    //            Response.Clear();
    //            Response.Buffer = true;
    //            Response.Charset = "";
    //            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    //            Response.AddHeader("content-disposition", "attachment;filename=SD_CustomFields.xlsx");
    //            using (MemoryStream MyMemoryStream = new MemoryStream())
    //            {
    //                wb.SaveAs(MyMemoryStream);
    //                MyMemoryStream.WriteTo(Response.OutputStream);
    //                Response.Flush();
    //                Response.End();
    //            }
    //        }

    //    }
    //    catch (ThreadAbortException e2)
    //    {
    //        Console.WriteLine("Exception message: {0}", e2.Message);
    //        Thread.ResetAbort();
    //    }
    //    catch (Exception ex)
    //    {
    //        if (ex.ToString().Contains("System.Threading.Thread.AbortInternal()"))
    //        {

    //        }
    //        else
    //        {
    //            var st = new StackTrace(ex, true);
    //            // Get the top stack frame
    //            var frame = st.GetFrame(0);
    //            // Get the line number from the stack frame
    //            var line = frame.GetFileLineNumber();
    //            inEr.InsertErrorLogsF(Session["UserName"].ToString()
    //, " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
    //            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

    //        }
    //    }
    //}
    //protected void gvSDCustomFields_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        // Handle delete command
    //        if (e.CommandName == "DeleteEx")
    //        {
    //            int rowIndex = Convert.ToInt32(e.CommandArgument);
    //            long ID = Convert.ToInt64(gvSDCustomFields.DataKeys[rowIndex].Values["ID"]);
    //            string Deskref = gvSDCustomFields.Rows[rowIndex].Cells[1].Text;

    //            // SQL Connection & Deletion logic
    //            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
    //            {
    //                con.Open();
    //                using (SqlCommand cmd = new SqlCommand("SD_spCustomFieldCntl", con))
    //                {
    //                    cmd.CommandType = CommandType.StoredProcedure;
    //                    cmd.Parameters.AddWithValue("@ID", ID);
    //                    cmd.Parameters.AddWithValue("@DeskRef", Deskref);
    //                    cmd.Parameters.AddWithValue("@OrgRef", ddlOrgCustomField.SelectedValue.ToString());
    //                    cmd.Parameters.AddWithValue("@Option", "DeleteCustomField");
    //                    cmd.CommandTimeout = 180;
    //                    int res = cmd.ExecuteNonQuery();

    //                    if (res > 0)
    //                    {
    //                        // Flag the session for deletion success
    //                        Session["Popup"] = "Delete";
    //                    }
    //                }
    //                con.Close();
    //            }

    //            // Refetch the data and refresh the GridView
    //            FillSDCustomFieldsDetails();
    //        }

    //        // Handle select/edit command
    //        if (e.CommandName == "SelectState")
    //        {
    //            // Hide Insert, show Update button
    //            btnInsertSDCustomFields.Visible = false;
    //            btnUpdateSDCustomFields.Visible = true;

    //            int rowIndex = Convert.ToInt32(e.CommandArgument);
    //            GridViewRow row = gvSDCustomFields.Rows[rowIndex];

    //            long ID = Convert.ToInt64(gvSDCustomFields.DataKeys[rowIndex].Values["ID"]);

    //            // Get the organization reference from hidden label
    //            Label OrgID = (row.FindControl("lblOrgFk") as Label);

    //            // Set dropdowns and other controls
    //            if (ddlOrgCustomField.Items.FindByValue(OrgID.Text.ToString().Trim()) != null)
    //            {
    //                ddlOrgCustomField.SelectedValue = OrgID.Text;
    //                FillRequestTypeCustomField(Convert.ToInt64(OrgID.Text));
    //                ddlRequestTypeCustomField.SelectedValue = gvSDCustomFields.Rows[rowIndex].Cells[1].Text;
    //            }

    //            // Set values in other fields
    //            ddlFieldType.SelectedValue = gvSDCustomFields.Rows[rowIndex].Cells[5].Text;
    //            ddlVisibilty.SelectedValue = (gvSDCustomFields.Rows[rowIndex].Cells[6].Text.Trim() == "True") ? "1" : "0";
    //            ddlRequiredStatus.SelectedValue = (gvSDCustomFields.Rows[rowIndex].Cells[7].Text.Trim() == "True") ? "1" : "0";

    //            ddlFieldType.Enabled = false;
    //            txtFieldName.Text = gvSDCustomFields.Rows[rowIndex].Cells[3].Text;
    //            txtFieldName.Enabled = false;
    //            ddlFieldMode.SelectedValue = gvSDCustomFields.Rows[rowIndex].Cells[4].Text;
    //            ddlFieldScope.SelectedValue = gvSDCustomFields.Rows[rowIndex].Cells[8].Text;
    //        }
    //    }
    //    catch (ThreadAbortException e2)
    //    {
    //        // Handle thread abort exception
    //        Console.WriteLine("Thread Abort: {0}", e2.Message);
    //        Thread.ResetAbort();
    //    }
    //    catch (Exception ex)
    //    {
    //        // Log detailed error and display notification to user
    //        var st = new StackTrace(ex, true);
    //        var frame = st.GetFrame(0);
    //        var line = frame.GetFileLineNumber();

    //        // Log the exception details
    //        inEr.InsertErrorLogsF(Session["UserName"].ToString(),
    //            $"Error in {Request.Url.ToString()}, Line: {line}, Exception: {ex.ToString()}");

    //        // Display the error notification on the frontend
    //        ScriptManager.RegisterStartupScript(this, GetType(),
    //            "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
    //    }
    //}

    //protected void SaveDataCustomField()
    //{
    //    long id = r.Next();
    //    try
    //    {
    //        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
    //        {

    //            using (SqlCommand cmd = new SqlCommand("SD_spCustomFieldCntl", con))
    //            {
    //                cmd.CommandType = CommandType.StoredProcedure;
    //                cmd.Parameters.AddWithValue("@ID", id);
    //                cmd.Parameters.AddWithValue("@Deskref", ddlRequestType.SelectedValue);
    //                if (ddlFieldType.SelectedValue.ToString() == "TextBox")
    //                {
    //                    cmd.Parameters.AddWithValue("@FieldID", "txt" + id);
    //                }
    //                else if (ddlFieldType.SelectedValue.ToString() == "DropDown")
    //                {
    //                    cmd.Parameters.AddWithValue("@FieldID", "ddl" + id);

    //                }
    //                cmd.Parameters.AddWithValue("@FieldName", txtFieldName.Text.ToString());
    //                cmd.Parameters.AddWithValue("@FieldValue", txtFieldName.Text.ToString().Replace(" ", "_"));
    //                cmd.Parameters.AddWithValue("@FieldType", ddlFieldType.SelectedValue.ToString());
    //                cmd.Parameters.AddWithValue("@FieldMode", ddlFieldMode.SelectedValue.ToString());
    //                cmd.Parameters.AddWithValue("@Status", ddlVisibilty.SelectedValue.ToString());
    //                cmd.Parameters.AddWithValue("@IsFieldReq", ddlRequiredStatus.SelectedValue.ToString());
    //                cmd.Parameters.AddWithValue("@FieldScope", ddlFieldScope.SelectedValue.ToString());
    //                cmd.Parameters.AddWithValue("@OrgRef", ddlOrgCustomField.SelectedValue.ToString());
    //                cmd.Parameters.AddWithValue("@Option", "AddCustomField");
    //                con.Open();
    //                int res = cmd.ExecuteNonQuery();
    //                if (res > 0)
    //                {
    //                    Session["Popup"] = "Insert";
    //                    Response.Redirect(Request.Url.AbsoluteUri);
    //                }

    //            }
    //        }
    //    }
    //    catch (ThreadAbortException e2)
    //    {
    //        Console.WriteLine("Exception message: {0}", e2.Message);
    //        Thread.ResetAbort();
    //    }
    //    catch (Exception ex)
    //    {
    //        if (ex.ToString().Contains("System.Threading.Thread.AbortInternal()"))
    //        {

    //        }
    //        else
    //        {
    //            var st = new StackTrace(ex, true);
    //            // Get the top stack frame
    //            var frame = st.GetFrame(0);
    //            // Get the line number from the stack frame
    //            var line = frame.GetFileLineNumber();
    //            inEr.InsertErrorLogsF(Session["UserName"].ToString()
    //, " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
    //            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

    //        }
    //    }
    //}
    //protected void btnInsertSDCustomFields_Click(object sender, EventArgs e)
    //{
    //    SaveDataCustomField();
    //}
    //protected void gvSDCustomFields_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    CurrentStep = 13;
    //    ViewState["CurrentStep"] = CurrentStep;
    //    DataBind();
    //    if (Session["UserScope"].ToString() == "Master")
    //    {
    //        e.Row.Cells[10].Visible = true;
    //        e.Row.Cells[11].Visible = true;
    //    }

    //    if (Session["UserScope"].ToString() == "Technician")
    //    {
    //        e.Row.Cells[10].Visible = false;
    //        e.Row.Cells[11].Visible = false;

    //    }
    //    if (Session["UserScope"].ToString() == "Admin")
    //    {
    //        e.Row.Cells[10].Visible = true;
    //        e.Row.Cells[11].Visible = false;

    //    }
    //}
    //protected void btnUpdateSDCustomFields_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
    //        {
    //            using (SqlCommand cmd = new SqlCommand("SD_spCustomFieldCntl", con))
    //            {
    //                cmd.CommandType = CommandType.StoredProcedure;
    //                cmd.Parameters.AddWithValue("@ID", ID);
    //                cmd.Parameters.AddWithValue("@Deskref", ddlRequestType.SelectedValue);
    //                cmd.Parameters.AddWithValue("@FieldMode", ddlFieldMode.SelectedValue);
    //                cmd.Parameters.AddWithValue("@Status", ddlVisibilty.SelectedValue);
    //                cmd.Parameters.AddWithValue("@IsFieldReq", ddlRequiredStatus.SelectedValue);
    //                cmd.Parameters.AddWithValue("@FieldScope", ddlFieldScope.SelectedValue);
    //                cmd.Parameters.AddWithValue("@OrgRef", ddlOrg.SelectedValue.ToString());
    //                cmd.Parameters.AddWithValue("@Option", "UpdateCustomField");
    //                con.Open();
    //                int res = cmd.ExecuteNonQuery();
    //                if (res > 0)
    //                {
    //                    Session["Popup"] = "Update";
    //                    Response.Redirect(Request.Url.AbsoluteUri);
    //                }
    //                //  ErrorMessage(this, "Welcome", "Greeting");
    //                // ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert('success','Data has been updated');", true);
    //            }
    //        }
    //    }
    //    catch (ThreadAbortException e2)
    //    {
    //        Console.WriteLine("Exception message: {0}", e2.Message);
    //        Thread.ResetAbort();
    //    }
    //    catch (Exception ex)
    //    {
    //        if (ex.ToString().Contains("System.Threading.Thread.AbortInternal()"))
    //        {

    //        }
    //        else
    //        {
    //            var st = new StackTrace(ex, true);
    //            var frame = st.GetFrame(0);
    //            var line = frame.GetFileLineNumber();
    //            inEr.InsertErrorLogsF(Session["UserName"].ToString()
    //, " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
    //            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

    //        }
    //    }
    //}
    //protected void btnCancel12_Click(object sender, EventArgs e)
    //{
    //    Response.Redirect(Request.Url.AbsoluteUri);
    //}
    //protected void ddlOrgCustomField_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    FillRequestTypeCustomField(Convert.ToInt64(ddlOrgCustomField.SelectedValue));
    //}
    //protected void lnkNextEsclation_Click(object sender, EventArgs e)
    //{
    //    pnlExclation.Visible = true;
    //    pnlAdddeskConfig.Visible = false;
    //    cleardata();
    //    CurrentStep = 13;
    //    ViewState["CurrentStep"] = CurrentStep;
    //    DataBind();
    //    if (Session["UserScope"] != null)
    //    {
    //        FillEcslevelDetails();
    //        FillOrganization();
    //    }
    //    else
    //    {
    //        Response.Redirect("/Default.aspx");
    //    }
    //}
    //protected void lnkPreviousDeskConfig_Click(object sender, EventArgs e)
    //{
    //    pnlAddCustomFields.Visible = false;
    //    lnkNextCustomFields_Click(null, null);
    //}
    #endregion End Cutom Fileds


    #region Email Config Start
    protected void GridFormat7(DataTable dt)
    {
        gvEmailConfig.UseAccessibleHeader = true;
        gvEmailConfig.HeaderRow.TableSection = TableRowSection.TableHeader;
        if (gvEmailConfig.TopPagerRow != null)
        {
            gvEmailConfig.TopPagerRow.TableSection = TableRowSection.TableHeader;
        }
        if (gvEmailConfig.BottomPagerRow != null)
        {
            gvEmailConfig.BottomPagerRow.TableSection = TableRowSection.TableFooter;
        }
        if (dt.Rows.Count > 0)
            gvEmailConfig.FooterRow.TableSection = TableRowSection.TableFooter;
    }
    private void FillOrganizationEmailConfig()
    {
        try
        {
            DataTable SD_Org = new FillSDFields().FillOrganization();
            ddlOrgEmailConfig.DataSource = SD_Org;
            ddlOrgEmailConfig.DataTextField = "OrgName";
            ddlOrgEmailConfig.DataValueField = "Org_ID";
            ddlOrgEmailConfig.DataBind();
            ddlOrgEmailConfig.Items.Insert(0, new System.Web.UI.WebControls.ListItem("---Select---", "0"));
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
    private void FillEmailConfigDetails(string orgid)
    {
        try
        {
            DataTable SD_EmailConfig = new FillSDFields().FillUserEmailConfigdetails(orgid);
            if (SD_EmailConfig.Rows.Count > 0)
            {
                this.gvEmailConfig.DataSource = (object)SD_EmailConfig;
                this.gvEmailConfig.DataBind();
            }
            else
            {
                this.gvEmailConfig.DataSource = (object)null;
                this.gvEmailConfig.DataBind();
            }
            //GridFormat7(SD_EmailConfig);
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
    protected void ImgBtnExport8_Click(object sender, EventArgs e)
    {
        try
        {

            DataTable dt = new DataTable("GridView_Data");
            foreach (System.Web.UI.WebControls.TableCell cell in gvEmailConfig.HeaderRow.Cells)
            {
                dt.Columns.Add(cell.Text);
            }
            foreach (GridViewRow row in gvEmailConfig.Rows)
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
                Response.AddHeader("content-disposition", "attachment;filename=EmailConfig.xlsx");
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
    static long EmailConfigID;
    protected void gvEmailConfig_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "DeleteEmailConfig")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                EmailConfigID = Convert.ToInt64(gvEmailConfig.DataKeys[rowIndex].Values["ID"]);

                try
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("SD_spEmailConfig", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ID", EmailConfigID);
                            cmd.Parameters.AddWithValue("@Option", "DeleteEmailConfig");
                            cmd.CommandTimeout = 180;
                            int res = cmd.ExecuteNonQuery();
                            if (res > 0)
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"success_noti('{HttpUtility.JavaScriptStringEncode("Deleted Successfully!")}');", true);
                            }
                            con.Close();
                            FillEmailConfigDetails(Convert.ToString(Session["OrgNameDesk"]));

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


            if (e.CommandName == "UpdateEmailConfig")
            {
                rfvtxtPassword.Enabled = false;

                AddEmailConfigPanel();
                btnInsertEmailConfig.Visible = false;
                btnUpdateEmailConfig.Visible = true;
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvEmailConfig.Rows[rowIndex];
                EmailConfigID = Convert.ToInt64(gvEmailConfig.DataKeys[rowIndex].Values["ID"]);
                txtHostName.Text = gvEmailConfig.Rows[rowIndex].Cells[1].Text;
                txtPort.Text = gvEmailConfig.Rows[rowIndex].Cells[2].Text;
                txtUserName.Text = gvEmailConfig.Rows[rowIndex].Cells[3].Text;
                txtEmail.Text = gvEmailConfig.Rows[rowIndex].Cells[4].Text;
                txtRetry.Text = gvEmailConfig.Rows[rowIndex].Cells[6].Text;
                txtClientID.Text = gvEmailConfig.Rows[rowIndex].Cells[7].Text;
                txtClientSecretKey.Text = gvEmailConfig.Rows[rowIndex].Cells[8].Text;
                txtTenantID.Text = gvEmailConfig.Rows[rowIndex].Cells[9].Text;
                Label OrgID = (row.FindControl("lblOrgFk") as Label);
                if (ddlOrgEmailConfig.Items.FindByValue(OrgID.Text.ToString().Trim()) != null)
                {
                    ddlOrgEmailConfig.SelectedValue = OrgID.Text;
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
    protected void SaveDataEmailConfig()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SD_spEmailConfig", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", r.Next());
                    cmd.Parameters.AddWithValue("@Hostname", txtHostName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Port", txtPort.Text);
                    cmd.Parameters.AddWithValue("@UserName", txtUserName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@Password", txtPassword.Text);
                    cmd.Parameters.AddWithValue("@Retry", txtRetry.Text);
                    cmd.Parameters.AddWithValue("@ClientID", txtClientID.Text);
                    cmd.Parameters.AddWithValue("@ClientSecretKey", txtClientSecretKey.Text);
                    cmd.Parameters.AddWithValue("@TenantID", txtTenantID.Text);
                    cmd.Parameters.AddWithValue("@OrgRef", ddlOrgEmailConfig.SelectedValue.ToString());
                    cmd.Parameters.Add("@Error", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                    cmd.Parameters.AddWithValue("@Option", "AddEmailConfig");
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    string ErrorChk = cmd.Parameters["@Error"].Value.ToString();
                    if (res > 0)
                    {
                        Session["EmailConfig"] = "EmailConfig";
                        //ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Saved Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Saved Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                        FillEmailConfigDetails(Convert.ToString(Session["OrgNameDesk"]));
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ warning_noti('{HttpUtility.JavaScriptStringEncode("" + ErrorChk + "")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
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
    protected void gvEmailConfig_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (Session["UserScope"].ToString() == "Master")
            {
                e.Row.Cells[5].Visible = true;
                e.Row.Cells[6].Visible = true;
            }

            if (Session["UserScope"].ToString() == "Technician")
            {
                e.Row.Cells[5].Visible = true;
                e.Row.Cells[6].Visible = false;

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
    protected void btnInsertEmailConfig_Click(object sender, EventArgs e)
    {
        SaveDataEmailConfig();
    }
    protected void AddEmailConfigPanel()
    {
        pnlAddEmailConfig.Visible = true;
        txtHostName.Text = "";
        txtPort.Text = "";
        txtUserName.Text = "";
        txtEmail.Text = "";
        txtPassword.Text = "";
        txtRetry.Text = "";
        txtClientID.Text = "";
        txtClientSecretKey.Text = "";
        txtTenantID.Text = "";
        btnInsertEmailConfig.Visible = true;
        btnUpdateEmailConfig.Visible = false;
    }
    protected void btnUpdateEmailConfig_Click(object sender, EventArgs e)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand("SD_spEmailConfig", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ID", EmailConfigID);
                    cmd.Parameters.AddWithValue("@Hostname", txtHostName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Port", txtPort.Text);
                    cmd.Parameters.AddWithValue("@UserName", txtUserName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@Password", txtPassword.Text);
                    cmd.Parameters.AddWithValue("@Retry", txtRetry.Text);
                    cmd.Parameters.AddWithValue("@ClientID", txtClientID.Text);
                    cmd.Parameters.AddWithValue("@ClientSecretKey", txtClientSecretKey.Text);
                    cmd.Parameters.AddWithValue("@TenantID", txtTenantID.Text);
                    cmd.Parameters.AddWithValue("@OrgRef", ddlOrgEmailConfig.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@Option", "UpdateEmailConfig");
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Updated Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                        //ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"success_noti('{HttpUtility.JavaScriptStringEncode("Updated Successfully!")}');", true);
                        FillEmailConfigDetails(Convert.ToString(Session["OrgNameDesk"]));
                        Response.Redirect("/DeskConfiguration.aspx");
                        btnInsertEmailConfig.Visible = true;
                        btnUpdateEmailConfig.Visible = false;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ warning_noti('{HttpUtility.JavaScriptStringEncode("Already Exists !")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
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
    protected void btnCancel8_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.AbsoluteUri);
    }
    protected void lnkPreviousCategory_Click(object sender, EventArgs e)
    {
        cleardata();
        pnlAddEmailConfig.Visible = false;
        lnkNextDeskConfig_Click(null, null);

    }
    protected void lnkNextResolution_Click(object sender, EventArgs e)
    {
        pnlExclation.Visible = true;
        pnlAddEmailConfig.Visible = false;
        cleardata();
        CurrentStep = 12;
        Session["CurrentStep"] = CurrentStep;
        DataBind();
        if (Session["UserScope"] != null)
        {
            FillEcslevelDetails(Convert.ToString(Session["OrgNameDesk"]));
            FillOrganizationEsclationMatrix();
            ddlOrgEcs.Items.FindByValue(Convert.ToString(Session["OrgNameDesk"])).Selected = true;
        }
        else
        {
            Response.Redirect("/Default.aspx");
        }
    }
    #endregion Email Config End


    #region Esclation Matrix Start
    private void FillOrganizationEsclationMatrix()
    {
        try
        {
            DataTable SD_Org = new FillSDFields().FillOrganization();
            ddlOrgEcs.DataSource = SD_Org;
            ddlOrgEcs.DataTextField = "OrgName";
            ddlOrgEcs.DataValueField = "Org_ID";
            ddlOrgEcs.DataBind();
            ddlOrgEcs.Items.Insert(0, new System.Web.UI.WebControls.ListItem("---Select---", "0"));
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
    private void FillEcslevelDetails(string OrgId)
    {
        try
        {
            DataTable SD_Ecslevel = new FillSDFields().FillUserEcsleveldetails(OrgId);
            if (SD_Ecslevel.Rows.Count > 0)
            {
                this.gvEcslevel.DataSource = (object)SD_Ecslevel;
                this.gvEcslevel.DataBind();

            }
            else
            {
                this.gvEcslevel.DataSource = (object)null;
                this.gvEcslevel.DataBind();

            }
            if (SD_Ecslevel.Rows.Count > 0)
            {
                //GridFormat(SD_Ecslevel);
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
    protected void ImgBtnExport14_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            DataTable dt = new DataTable("GridView_Data");
            foreach (System.Web.UI.WebControls.TableCell cell in gvEcslevel.HeaderRow.Cells)
            {
                dt.Columns.Add(cell.Text);
            }
            foreach (GridViewRow row in gvEcslevel.Rows)
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
                Response.AddHeader("content-disposition", "attachment;filename=EsclatnMatrix.xlsx");
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
    static int EcslevelID;
    protected void gvEcslevel_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "DeleteEcslevel")
            {
                //Determine the RowIndex of the Row whose Button was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                //Get the value of column from the DataKeys using the RowIndex.
                EcslevelID = Convert.ToInt32(gvEcslevel.DataKeys[rowIndex].Values["ID"]);

                try
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("SD_spAddUserEcslevel", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ID", EcslevelID);

                            cmd.Parameters.AddWithValue("@Option", "DeleteEcslevel");
                            cmd.CommandTimeout = 180;
                            int res = cmd.ExecuteNonQuery();
                            if (res > 0)
                            {
                                Session["Popup"] = "Delete";
                                //Response.Redirect(Request.Url.AbsoluteUri);
                                //                                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
                                //$"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Deleted Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"success_noti('{HttpUtility.JavaScriptStringEncode("Deleted Successfully!")}');", true);
                                //lnkNextResolution_Click(null,null);
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
    $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
                            }
                            con.Close();
                            FillEcslevelDetails(Convert.ToString(Session["OrgNameDesk"]));
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
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
    $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

                    }
                }

            }
            if (e.CommandName == "UpdateEcslevel")
            {
                btnInsertEcslevel.Visible = false;
                btnUpdateEcslevel.Visible = true;
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvEcslevel.Rows[rowIndex];
                EcslevelID = Convert.ToInt32(gvEcslevel.DataKeys[rowIndex].Values["ID"]);
                ddlEsclationLevel.SelectedValue = gvEcslevel.Rows[rowIndex].Cells[1].Text;
                txtUserNameEsc.Text = gvEcslevel.Rows[rowIndex].Cells[2].Text;
                txtEmailEsc.Text = gvEcslevel.Rows[rowIndex].Cells[3].Text;
                txtMobile.Text = gvEcslevel.Rows[rowIndex].Cells[4].Text;
                txttimeforEsclation.Text = gvEcslevel.Rows[rowIndex].Cells[5].Text;
                Label OrgID = (row.FindControl("lblOrgFk") as Label);
                if (ddlOrgEcs.Items.FindByValue(OrgID.Text.ToString().Trim()) != null)
                {
                    ddlOrgEcs.SelectedValue = OrgID.Text;
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
    protected void SaveDataEcs()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand("SD_spAddUserEcslevel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@EsclationLevel", ddlEsclationLevel.Text.Trim());
                    cmd.Parameters.AddWithValue("@UserName", txtUserNameEsc.Text);
                    cmd.Parameters.AddWithValue("@UserEmail", txtEmailEsc.Text);
                    cmd.Parameters.AddWithValue("@Mobile", txtMobile.Text);
                    cmd.Parameters.AddWithValue("@TimeForEsclatn", txttimeforEsclation.Text);
                    cmd.Parameters.AddWithValue("@OrgRef", ddlOrgEcs.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@Option", "AddEsclationUser");
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        Session["Popup"] = "Insert";
                        //Response.Redirect(Request.Url.AbsoluteUri + "?pnlAddEcslevel=true");
                        //                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
                        //$"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Saved Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 5000); }}", true);
                        if (ViewState["btnType"] != null)
                        {
                            ViewState["btnType"] = null;
                            Session["ReqType"] = null;
                            Session["Stage"] = null;
                            Session["Status"] = null;
                            Session["Severity"] = null;
                            Session["Priority"] = null;
                            Session["Category"] = null;
                            Session["Resolution"] = null;
                            Session["SLA"] = null;
                            Session["Desktemp"] = null;
                            Session["EmailConfig"] = null;
                            pnlExclation.Visible = false;
                            lnkPrevOrg_Click(null, null);
                            //ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Desk Configuration Completed Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Desk Configuration Completed Successfully!');window.location ='/DeskConfiguration.aspx';", true);
                            //                            string message = HttpUtility.JavaScriptStringEncode("Ticket Updated Successfully!");
                            //                            ScriptManager.RegisterStartupScript(this, GetType(), "showAlertAndRedirect", $@"
                            //    setTimeout(function() {{
                            //        success_noti('{message}'); 
                            //        setTimeout(function() {{
                            //            if (window.opener) {{
                            //                window.opener.location.href = '/DeskConfiguration.aspx';
                            //            }}
                            //            window.close();
                            //        }}, 3000); // Delay for the alert
                            //    }}, 100); // Initial delay to ensure success_noti is called first
                            //", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Saved Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                        }
                        FillEcslevelDetails(Convert.ToString(Session["OrgNameDesk"]));
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
    protected void gvEcslevel_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
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
    protected void btnInsertEcslevel_Click(object sender, EventArgs e)
    {
        SaveDataEcs();
    }
    protected void lnkSkipFinish_Click(object sender, EventArgs e)
    {

        mp1.Show();
        ViewState["type"] = "Finish";
        //btnInsertEcslevel_Click(null, null);
    }
    protected void btnUpdateEcslevel_Click(object sender, EventArgs e)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand("SD_spAddUserEcslevel", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ID", EcslevelID);
                    cmd.Parameters.AddWithValue("@EsclationLevel", ddlEsclationLevel.Text.Trim());
                    cmd.Parameters.AddWithValue("@UserName", txtUserNameEsc.Text);
                    cmd.Parameters.AddWithValue("@UserEmail", txtEmailEsc.Text);
                    cmd.Parameters.AddWithValue("@Mobile", txtMobile.Text);
                    cmd.Parameters.AddWithValue("@TimeForEsclatn", txttimeforEsclation.Text);
                    cmd.Parameters.AddWithValue("@OrgRef", ddlOrgEcs.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@Option", "UpdateUserEcslevel");
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        Session["Popup"] = "Update";
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Updated Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                        //ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"success_noti('{HttpUtility.JavaScriptStringEncode("Updated Successfully!")}');", true);
                        FillEcslevelDetails(Convert.ToString(Session["OrgNameDesk"]));
                        Response.Redirect("/DeskConfiguration.aspx");
                        btnInsertEcslevel.Visible = true;
                        btnUpdateEcslevel.Visible = false;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/DeskConfiguration.aspx')) {{ warning_noti('{HttpUtility.JavaScriptStringEncode("Already Exists !")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);

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
    protected void btnAddUserEcslevel_Click(object sender, EventArgs e)
    {
        ddlEsclationLevel.ClearSelection();
        txtUserName.Text = "";
        txtEmail.Text = "";
        txtMobile.Text = "";
        txttimeforEsclation.Text = "";
        ddlOrg.ClearSelection();
        btnInsertEcslevel.Visible = true;
        btnUpdateEcslevel.Visible = false;

    }
    protected void GridFormat(DataTable dt)
    {
        gvEcslevel.UseAccessibleHeader = true;
        gvEcslevel.HeaderRow.TableSection = TableRowSection.TableHeader;

        if (gvEcslevel.TopPagerRow != null)
        {
            gvEcslevel.TopPagerRow.TableSection = TableRowSection.TableHeader;
        }
        if (gvEcslevel.BottomPagerRow != null)
        {
            gvEcslevel.BottomPagerRow.TableSection = TableRowSection.TableFooter;
        }
        if (dt.Rows.Count > 0)
            gvEcslevel.FooterRow.TableSection = TableRowSection.TableFooter;
    }
    protected void btnCancel14_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.AbsoluteUri);
    }
    protected void lnkPreviousCustomField_Click(object sender, EventArgs e)
    {
        pnlExclation.Visible = false;
        cleardata();
        lnkNextCustomFields_Click(null, null);
    }
    protected void ImgBtnExport14_Click(object sender, EventArgs e)
    {
        try
        {

            DataTable dt = new DataTable("GridView_Data");
            foreach (System.Web.UI.WebControls.TableCell cell in gvEcslevel.HeaderRow.Cells)
            {
                dt.Columns.Add(cell.Text);
            }
            foreach (GridViewRow row in gvEcslevel.Rows)
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
                Response.AddHeader("content-disposition", "attachment;filename=EsclationData.xlsx");
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
    protected void btnCloseFinish_Click(object sender, EventArgs e)
    {

        mp1.Show();

    }
    #endregion Esclation Matrin End
    protected void btnAddDeskName_Click(object sender, EventArgs e)
    {
        mp1.Show();
        string sql = "update SD_RequestType set DeskName='" + txtDeskName.Text + "' where ReqTypeRef='" + Convert.ToString(Session["ReqType"]) + "' and OrgRef='" + Convert.ToString(Session["OrgNameDesk"]) + "'";
        database.ExecuteNonQuery(sql);
        ViewState["btnType"] = "Redirect";
        if (ViewState["type"] == null)
        {
            btnInsertEcslevel_Click(null, null);
        }
        else
        {
            ViewState["btnType"] = null;
            Session["ReqType"] = null;
            Session["Stage"] = null;
            Session["Status"] = null;
            Session["Severity"] = null;
            Session["Priority"] = null;
            Session["Category"] = null;
            Session["Resolution"] = null;
            Session["SLA"] = null;
            Session["Desktemp"] = null;
            Session["EmailConfig"] = null;
            pnlExclation.Visible = false;
            Session["chngtyp"] = null;
            Session["resonforchng"] = null;
            lnkPrevOrg_Click(null, null);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Desk Configuration Completed Successfully!');window.location ='/DeskConfiguration.aspx';", true);
        }
    }
}
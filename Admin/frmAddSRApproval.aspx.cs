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

public partial class Admin_frmAddSRApproval : System.Web.UI.Page
{
    InsertErrorLogs inEr = new InsertErrorLogs();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["UserScope"] != null && Session["LoginName"] != null)
            {
                if (!IsPostBack)
                {
                    FillOrg();
                    btnViewUsers.CssClass = "btn btn-sm btn-secondary";
                    btnViewUsers.Enabled = false;
                    btnAddApproval.CssClass = "btn btn-sm btn-outline-secondary";
                    btnimportUser.CssClass = "btn btn-sm  btn-outline-secondary";
                    pnlShowUsers.Visible = true;
                    FillApproval("");
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
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                inEr.InsertErrorLogsF(Session["UserName"].ToString()
    , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
            }
        }
    }
    private void FillOrg()
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
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                inEr.InsertErrorLogsF(Session["UserName"].ToString()
    , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
            }
        }
    }
    private void FillRequestType(long OrgId)
    {
        DataTable RequestType = new SDTemplateFileds().FillRequestType(OrgId);
        ddlRequestType.DataSource = RequestType;
        ddlRequestType.DataTextField = "ReqTypeRef";
        ddlRequestType.DataValueField = "ReqTypeRef";
        ddlRequestType.DataBind();
        ddlRequestType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--- RequestType---", "0"));



    }
    private void Modal()
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append(@"<script type='text/javascript'>");
        sb.Append("$('#basicModal').modal('show');");
        sb.Append("$('body').removeClass('modal-open');");
        sb.Append("$('.modal-backdrop').remove();");
        sb.Append(@"</script>");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ModalScript", sb.ToString(), false);


    }
    Random r = new Random();
    public static int ID;
    private void FillUser(string OrgId)
    {
        try
        {
            DataTable SD_Org = new FillSDFields().FillUserName(OrgId);
            ddlUser.DataSource = SD_Org;
            ddlUser.DataTextField = "Name";
            ddlUser.DataValueField = "userid";
            ddlUser.DataBind();
            ddlUser.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----------Select User----------", "0"));
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
    private void FillApproval(string OrgId)
    {
        try
        {
            DataTable SD_Holiday = new FillSDFields().FillUserWiseApproval(OrgId);
            if (SD_Holiday.Rows.Count > 0)
            {
                this.gvUserWiseApproval.DataSource = (object)SD_Holiday;
                this.gvUserWiseApproval.DataBind();
                GridFormat(SD_Holiday);
            }
            else
            {
                this.gvUserWiseApproval.DataSource = (object)null;
                this.gvUserWiseApproval.DataBind();
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
    protected void GridFormat(DataTable dt)
    {
        gvUserWiseApproval.UseAccessibleHeader = true;
        gvUserWiseApproval.HeaderRow.TableSection = TableRowSection.TableHeader;

        if (gvUserWiseApproval.TopPagerRow != null)
        {
            gvUserWiseApproval.TopPagerRow.TableSection = TableRowSection.TableHeader;
        }
        if (gvUserWiseApproval.BottomPagerRow != null)
        {
            gvUserWiseApproval.BottomPagerRow.TableSection = TableRowSection.TableFooter;
        }
        if (dt.Rows.Count > 0)
            gvUserWiseApproval.FooterRow.TableSection = TableRowSection.TableFooter;
    }
    protected void SaveData()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SD_spAddSRApproval", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserID", ddlUser.SelectedValue);
                    cmd.Parameters.AddWithValue("@EmpID", txtEmpID.Text);
                    cmd.Parameters.AddWithValue("@LoginName", txtLoginName.Text);
                    cmd.Parameters.AddWithValue("@Approval1Name", txtApprover1Name.Text);
                    cmd.Parameters.AddWithValue("@Approval1Email", txtApprover1Email.Text);
                    cmd.Parameters.AddWithValue("@Approval1Active", Convert.ToBoolean(ddlApprover1Active.SelectedValue));
                    cmd.Parameters.AddWithValue("@Approval1Level", txtApprovallbl1.Text);
                    cmd.Parameters.AddWithValue("@Approval2Name", txtApprover2Name.Text);
                    cmd.Parameters.AddWithValue("@Approval2Email", txtApprover2Email.Text);
                    cmd.Parameters.AddWithValue("@Approval2Active", Convert.ToBoolean(ddlApprover2Active.SelectedValue));
                    cmd.Parameters.AddWithValue("@Approval2Level", txtApprovallbl2.Text);
                    cmd.Parameters.AddWithValue("@Approval3Name", txtApprover3Name.Text);
                    cmd.Parameters.AddWithValue("@Approval3Email", txtApprover3Email.Text);
                    cmd.Parameters.AddWithValue("@Approval3Active", Convert.ToBoolean(ddlApprover3Active.SelectedValue));
                    cmd.Parameters.AddWithValue("@Approval3Level", txtApprovallbl3.Text);
                    cmd.Parameters.AddWithValue("@Approval4Name", txtApprover4Name.Text);
                    cmd.Parameters.AddWithValue("@Approval4Email", txtApprover4Email.Text);
                    cmd.Parameters.AddWithValue("@Approval4Active", Convert.ToBoolean(ddlApprover4Active.SelectedValue));
                    cmd.Parameters.AddWithValue("@Approval4Level", txtApprovallbl4.Text);
                    cmd.Parameters.AddWithValue("@Approval5Name", txtApprover5Name.Text);
                    cmd.Parameters.AddWithValue("@Approval5Email", txtApprover5Email.Text);
                    cmd.Parameters.AddWithValue("@Approval5Active", Convert.ToBoolean(ddlApprover5Active.SelectedValue));
                    cmd.Parameters.AddWithValue("@Approval5Level", txtApprovallbl5.Text);
                    cmd.Parameters.AddWithValue("@InsertBy", Session["LoginName"].ToString());
                    cmd.Parameters.AddWithValue("@OrgID", ddlOrg.SelectedValue);
                    cmd.Parameters.AddWithValue("@ReqType", ddlRequestType.SelectedValue);
                    cmd.Parameters.AddWithValue("@Option", "AddSrApproval");
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
        $"if (window.location.pathname.endsWith('/Admin/frmAddSRApproval.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Saved Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
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
        , "Add Holiday: Error While Populating ParentCategory " + Request.Url.ToString() + "Got Exception" + ex.ToString());
            }
        }
    }
    protected void ImgBtnExport_Click(object sender, ImageClickEventArgs e)
    {
        try
        {

            DataTable dt = new DataTable("GridView_Data");
            foreach (System.Web.UI.WebControls.TableCell cell in gvUserWiseApproval.HeaderRow.Cells)
            {
                dt.Columns.Add(cell.Text);
            }
            foreach (GridViewRow row in gvUserWiseApproval.Rows)
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
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                inEr.InsertErrorLogsF(Session["UserName"].ToString()
    , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
            }

        }
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {

        UpdateUserDetails();


    }
    protected void UpdateUserDetails()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SD_spAddSRApproval", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(UserID));
                    cmd.Parameters.AddWithValue("@Approval1Name", txtApprover1Name.Text);
                    cmd.Parameters.AddWithValue("@Approval1Email", txtApprover1Email.Text);
                    cmd.Parameters.AddWithValue("@Approval1Active", ddlApprover1Active.SelectedValue);
                    cmd.Parameters.AddWithValue("@Approval1Level", txtApprovallbl1.Text);
                    cmd.Parameters.AddWithValue("@Approval2Name", txtApprover2Name.Text);
                    cmd.Parameters.AddWithValue("@Approval2Email", txtApprover2Email.Text);
                    cmd.Parameters.AddWithValue("@Approval2Active", ddlApprover2Active.SelectedValue);
                    cmd.Parameters.AddWithValue("@Approval2Level", txtApprovallbl2.Text);
                    cmd.Parameters.AddWithValue("@Approval3Name", txtApprover3Name.Text);
                    cmd.Parameters.AddWithValue("@Approval3Email", txtApprover3Email.Text);
                    cmd.Parameters.AddWithValue("@Approval3Active", ddlApprover3Active.SelectedValue);
                    cmd.Parameters.AddWithValue("@Approval3Level", txtApprovallbl3.Text);
                    cmd.Parameters.AddWithValue("@Approval4Name", txtApprover4Name.Text);
                    cmd.Parameters.AddWithValue("@Approval4Email", txtApprover4Email.Text);
                    cmd.Parameters.AddWithValue("@Approval4Active", ddlApprover4Active.SelectedValue);
                    cmd.Parameters.AddWithValue("@Approval4Level", txtApprovallbl4.Text);
                    cmd.Parameters.AddWithValue("@Approval5Name", txtApprover5Name.Text);
                    cmd.Parameters.AddWithValue("@Approval5Email", txtApprover5Email.Text);
                    cmd.Parameters.AddWithValue("@Approval5Active", ddlApprover5Active.SelectedValue);
                    cmd.Parameters.AddWithValue("@Approval5Level", txtApprovallbl5.Text);
                    cmd.Parameters.AddWithValue("@UpdateBy", Session["LoginName"].ToString());
                    cmd.Parameters.AddWithValue("@Option", "UpdateSrApproval");
                    con.Open();
                    ddlUser.Enabled = true;
                    ddlOrg.Enabled = true;
                    ddlRequestType.Enabled = true;
                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
       $"if (window.location.pathname.endsWith('/Admin/frmAddSRApproval.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Updated Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
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
        SaveData();
    }
    static int UserID;
    protected void gvUserWiseApproval_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName == "DeleteEx")
        {
            try
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                UserID = Convert.ToInt32(gvUserWiseApproval.DataKeys[rowIndex].Values["ID"]);
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("SD_spAddSRApproval", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", UserID);
                        cmd.Parameters.AddWithValue("@Option", "DeleteApproval");
                        cmd.CommandTimeout = 180;
                        int res = cmd.ExecuteNonQuery();
                        if (res > 0)
                        {
                            Session["Popup"] = "Delete";
                            Response.Redirect(Request.Url.AbsoluteUri);
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
                    var frame = st.GetFrame(0);
                    var line = frame.GetFileLineNumber();
                    inEr.InsertErrorLogsF(Session["UserName"].ToString()
        , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                    ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
                }
            }
        }
        if (e.CommandName == "SelectTech")
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = gvUserWiseApproval.Rows[rowIndex];
            ddlUser.ClearSelection();
            FillOrg();
            ddlOrg.SelectedValue = Convert.ToString(gvUserWiseApproval.DataKeys[rowIndex]["OrgId"]);
            FillRequestType(Convert.ToInt64(ddlOrg.SelectedValue));
            FillUser(ddlOrg.SelectedValue);
            ddlRequestType.Items.FindByText(Convert.ToString(gvUserWiseApproval.DataKeys[rowIndex]["ReqType"])).Selected = true;
            UserID = Convert.ToInt32(gvUserWiseApproval.DataKeys[rowIndex]["ID"]);
            ddlUser.SelectedValue = row.Cells[3].Text.Trim();
            ddlUser.Enabled = false;
            ddlOrg.Enabled = false;
            ddlRequestType.Enabled = false;
            txtEmpID.Text = row.Cells[4].Text.Trim(); 
            txtLoginName.Text = row.Cells[5].Text.Trim(); 
            txtApprover1Name.Text = row.Cells[6].Text.Trim(); 
            txtApprover1Email.Text = row.Cells[7].Text.Trim(); 
            ddlApprover1Active.SelectedValue = row.Cells[8].Text.Trim(); 
            txtApprovallbl1.Text = row.Cells[9].Text.Trim(); 
            txtApprover2Name.Text = row.Cells[10].Text.Trim(); 
            txtApprover2Email.Text = row.Cells[11].Text.Trim(); 
            ddlApprover2Active.SelectedValue = row.Cells[12].Text.Trim(); 
            txtApprovallbl2.Text = row.Cells[13].Text.Trim(); 
            txtApprover3Name.Text = row.Cells[14].Text.Trim(); 
            txtApprover3Email.Text = row.Cells[15].Text.Trim(); 
            ddlApprover3Active.SelectedValue = row.Cells[16].Text.Trim(); 
            txtApprovallbl3.Text = row.Cells[17].Text.Trim(); 
            txtApprover4Name.Text = row.Cells[18].Text.Trim(); 
            txtApprover4Email.Text = row.Cells[19].Text.Trim(); 
            ddlApprover4Active.SelectedValue = row.Cells[20].Text.Trim(); 
            txtApprovallbl4.Text = row.Cells[21].Text.Trim(); 
            txtApprover5Name.Text = row.Cells[22].Text.Trim(); 
            txtApprover5Email.Text = row.Cells[23].Text.Trim(); 
            ddlApprover5Active.SelectedValue = row.Cells[24].Text.Trim(); 
            txtApprovallbl5.Text = row.Cells[25].Text.Trim(); 
            btnInsert.Visible = false;
            btnUpdate.Visible = true;
            AddApprovalControl();
        }
    }
    protected void gvUserWiseApproval_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {

            if (Session["UserScope"].ToString() == "Technician")
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
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
            }
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ddlUser.Enabled = true;
        ddlOrg.Enabled = true;
        ddlRequestType.Enabled = true;
        Response.Redirect(Request.Url.AbsoluteUri);
    }
    protected void btnimportUser_Click(object sender, EventArgs e)
    {
        try
        {
            pnlAddHoliday.Visible = false;
            pnlShowUsers.Visible = false;
            btnAddApproval.CssClass = "btn btn-sm  btn-outline-secondary";
            btnViewUsers.CssClass = "btn btn-sm  btn-outline-secondary";
            btnimportUser.CssClass = "btn btn-sm btn-secondary";
            btnimportUser.Enabled = false;
            btnAddApproval.Enabled = true;
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
            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
        }
    }
    protected void btnAddApproval_Click(object sender, EventArgs e)
    {
        try
        {
            AddApprovalControl();
            //FillLevels();
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
    protected void btnViewUsers_Click(object sender, EventArgs e)
    {
        try
        {
            ShowUserDetaiControl();
            FillApproval("");
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
    protected void ShowUserDetaiControl()
    {
        try
        {
            pnlAddHoliday.Visible = false;
            //pnlImportUser.Visible = false;
            pnlShowUsers.Visible = true;
            btnAddApproval.CssClass = "btn btn-sm  btn-outline-secondary ";
            btnimportUser.CssClass = "btn btn-sm  btn-outline-secondary";
            btnViewUsers.CssClass = "btn btn-sm btn-secondary";
            btnViewUsers.Enabled = false;
            btnAddApproval.Enabled = true;
            btnimportUser.Enabled = true;
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
    protected void AddApprovalControl()
    {
        try
        {

            pnlAddHoliday.Visible = true;
            btnAddApproval.CssClass = "btn btn-sm btn-secondary";
            btnViewUsers.CssClass = "btn btn-sm  btn-outline-secondary";
            btnimportUser.CssClass = "btn btn-sm  btn-outline-secondary";
            btnAddApproval.Enabled = false;
            btnViewUsers.Enabled = true;
            btnimportUser.Enabled = true;
            pnlShowUsers.Visible = false;
            //pnlImportUser.Visible = false;
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
    protected void btnn_Click(object sender, EventArgs e)
    {

    }
    protected void butttonsubmit_Click(object sender, EventArgs e)
    {

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
    protected void ddlUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillUserDetails(ddlUser.SelectedValue);
    }
    protected void FillUserDetails(string Userid)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand("select EmpID,LoginName from SD_vUser where userid=@UserID", con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@UserID", Userid);


                    con.Open();
                    SqlDataReader DR1 = cmd.ExecuteReader();
                    if (DR1.Read())
                    {
                        txtEmpID.Text = DR1.GetValue(0).ToString();
                        txtLoginName.Text = DR1.GetValue(1).ToString();
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

    protected void ddlRequestType_SelectedIndexChanged(object sender, EventArgs e)
    {
        //FillStatus1();
        //FillStatus2();
        //FillStatus3();
        //FillStatus4();
        //FillStatus5();
    }
    //private void FillLevels()
    //{

    //    try
    //    {

    //        DataTable SD_Approval = new SDTemplateFileds().FillApprovalLevel();
    //        ddlApproval1Level.DataSource = SD_Approval;
    //        ddlApproval1Level.DataTextField = "Grade";
    //        ddlApproval1Level.DataValueField = "Grade";
    //        ddlApproval1Level.DataBind();
    //        ddlApproval1Level.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--- Select ----", "0"));

    //        ddlApproval2Level.DataSource = SD_Approval;
    //        ddlApproval2Level.DataTextField = "Grade";
    //        ddlApproval2Level.DataValueField = "Grade";
    //        ddlApproval2Level.DataBind();
    //        ddlApproval2Level.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--- Select ----", "0"));

    //        ddlApproval3Level.DataSource = SD_Approval;
    //        ddlApproval3Level.DataTextField = "Grade";
    //        ddlApproval3Level.DataValueField = "Grade";
    //        ddlApproval3Level.DataBind();
    //        ddlApproval3Level.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--- Select ----", "0"));

    //        ddlApproval4Level.DataSource = SD_Approval;
    //        ddlApproval4Level.DataTextField = "Grade";
    //        ddlApproval4Level.DataValueField = "Grade";
    //        ddlApproval4Level.DataBind();
    //        ddlApproval4Level.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--- Select ----", "0"));

    //        ddlApproval5Level.DataSource = SD_Approval;
    //        ddlApproval5Level.DataTextField = "Grade";
    //        ddlApproval5Level.DataValueField = "Grade";
    //        ddlApproval5Level.DataBind();
    //        ddlApproval5Level.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--- Select ----", "0"));
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
    protected void ddlOrg_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillRequestType(Convert.ToInt64(ddlOrg.SelectedValue));
        FillApproval(ddlOrg.SelectedValue);
        FillUser(ddlOrg.SelectedValue);
        //FillLevels();
    }
}
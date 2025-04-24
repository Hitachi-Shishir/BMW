using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class frmTechLeaveApply : System.Web.UI.Page
{
    InsertErrorLogs inEr = new InsertErrorLogs();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null)
        {
            Response.Redirect("~/default.aspx");
        }
        
        if (!IsPostBack)
        {
            FillOrganization();
            bindAssigne();
            getleavedata();
        }

    }
    public void DataTableScript()
    {
        string jqueryScript = "<script src='assetsdata/js/jquery.min.js'></script>";
        string dataTableScript = "<script src='assetsdata/plugins/datatable/js/jquery.dataTables.min.js'></script>";
        string dataTableBootstrapScript = "<script src='assetsdata/plugins/datatable/js/dataTables.bootstrap5.min.js'></script>";

        ClientScript.RegisterStartupScript(this.GetType(), "jqueryScript", jqueryScript, false);
        ClientScript.RegisterStartupScript(this.GetType(), "dataTableScript", dataTableScript, false);
        ClientScript.RegisterStartupScript(this.GetType(), "dataTableBootstrapScript", dataTableBootstrapScript, false);

        string script = @"
    <script type='text/javascript'>
        $(document).ready(function () {
            $('.data-table').DataTable();
        });
    </script>";

        // Use ScriptManager for partial postbacks or ClientScript for full postbacks
        ScriptManager.RegisterStartupScript(this, GetType(), "initializeDataTable", script, false);
    }
    public void getleavedata()
    {
        string sql = @"select top 5000 a.id, a.TechId,a.TechName,cast(a.LeaveFromdate as date) LeaveFromdate,cast(a.LeaveTodate as date) LeaveTodate,
cast(a.ApplyDate as date) ApplyDate,
a.AppliedbyUserid,b.UserName,b.UserName from TechLeave a with(Nolock) inner join 
SD_User_Master  b with(Nolock)
on a.AppliedbyUserid=b.UserID";
        DataTable dt = database.GetDataTable(sql);
        grv.DataSource = dt;
        grv.DataBind();
        GridFormat(dt);
    }
    protected void GridFormat(DataTable dt)
    {
        grv.UseAccessibleHeader = true;
        grv.HeaderRow.TableSection = TableRowSection.TableHeader;

        if (grv.TopPagerRow != null)
        {
            grv.TopPagerRow.TableSection = TableRowSection.TableHeader;
        }
        if (grv.BottomPagerRow != null)
        {
            grv.BottomPagerRow.TableSection = TableRowSection.TableFooter;
        }
        if (dt.Rows.Count > 0)
            grv.FooterRow.TableSection = TableRowSection.TableFooter;
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
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                inEr.InsertErrorLogsF(Session["UserName"].ToString()
    , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
    $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
            }
        }
    }
    public void bindAssigne()
    {
        try
        {
            if (ddlOrg.SelectedValue != "0")
            {
                DataTable FillAssigne = new SDTemplateFileds().FillAssigne(Convert.ToInt64(ddlOrg.SelectedValue));
                ddltech.DataSource = FillAssigne;
                ddltech.DataTextField = "TechLoginName";
                ddltech.DataValueField = "TechID";
                ddltech.DataBind();
                ddltech.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--- User ---", "0"));
            }
            else
            {
                ddltech.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--- User ---", "0"));
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void btnSave_Click(object sender, EventArgs e)
    {
        Util obj = new Util();
        if (ddlOrg.SelectedValue == "0")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"warning_noti('{HttpUtility.JavaScriptStringEncode("Please Select Organization !")}');", true);
            ddlOrg.Focus();
            return;
        }
        if (ddltech.SelectedValue == "0")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"warning_noti('{HttpUtility.JavaScriptStringEncode("Please Select Technician !")}');", true);
            ddltech.Focus();
            return;
        }
        if (txtfrmDate.Text == "")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"warning_noti('{HttpUtility.JavaScriptStringEncode("From Date Cannot be Empty !")}');", true);
            txtfrmDate.Focus();
            return;
        }
        if (txttoDate.Text == "")
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"warning_noti('{HttpUtility.JavaScriptStringEncode("To Date Cannot be Empty !")}');", true);
            txttoDate.Focus();
            return;
        }
        
        DateTime frmdate = Convert.ToDateTime(txtfrmDate.Text);
        string fdate = frmdate.ToString("yyyy-MM-dd HH:mm:ss");

        DateTime todate = Convert.ToDateTime(txttoDate.Text);
        string tdate = todate.ToString("yyyy-MM-dd HH:mm:ss");
        string msg = obj.InsertInsertTechLeave(ddltech.SelectedValue, ddltech.SelectedItem.Text.Trim(),
            Convert.ToDateTime(fdate), Convert.ToDateTime(tdate), Convert.ToString(Session["UserID"]));
        if (msg != "")
        {   
            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
        $"if (window.location.pathname.endsWith('/frmTechLeaveApply.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Saved Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
            //DataTableScript();
            getleavedata();
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
    $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
        }
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        Response.Redirect("/HelpDesk/frmTechLeaveApply.aspx");
    }

    protected void ddlOrg_SelectedIndexChanged(object sender, EventArgs e)
    {
        getleavedata();
        //DataTableScript();
        bindAssigne();
    }

    protected void lnkdelete_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        GridViewRow gvr = (GridViewRow)btn.NamingContainer;
        HiddenField hdnid = (HiddenField)gvr.FindControl("hdnid");
        string id = hdnid.Value;
        try
        {
            string sql = "delete from TechLeave where id = '" + id + "'";
            database.ExecuteNonQuery(sql);
            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
$"if (window.location.pathname.endsWith('/frmTechLeaveApply.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Deleted Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Error data not Deleted !');window.location ='frmTechLeaveApply.aspx';", true);
        }
        //DataTableScript();
        getleavedata();
    }
}
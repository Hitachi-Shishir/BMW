using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class frmEmailTemplate : System.Web.UI.Page
{
    InsertErrorLogs inEr = new InsertErrorLogs();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserName"] == null)
        {
            Response.Redirect("/Default.aspx");
        }
        if (!IsPostBack)
        {
            FillOrganization();
            txtEmailTemplate.Visible = false;
            ddlEmailTemplate.Visible = true;
            btnSave.Text = "Submit";
        }
    }
    protected void ddlOrg_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillRequestType(Convert.ToInt64(ddlOrg.SelectedValue));
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
            ddlRequestType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--- RequestType---", "0"));
        }
        catch (ThreadAbortException e2)
        {
            Console.WriteLine("Exception message: {0}", e2.Message);
            Thread.ResetAbort();
        }
        catch (Exception ex)
        {
            // msg.ReportError(ex.Message);
        }
    }
    private void FillEmailTemplate(string RequestType)
    {
        try
        {
            DataTable SD_Org = new FillSDFields().FillEmailTemplate(RequestType, ddlOrg.SelectedValue); ;
            ddlEmailTemplate.DataSource = SD_Org;
            ddlEmailTemplate.DataTextField = "templateName";
            ddlEmailTemplate.DataValueField = "templateName";
            ddlEmailTemplate.DataBind();
            ddlEmailTemplate.Items.Insert(0, new System.Web.UI.WebControls.ListItem("----Select----", "0"));
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
            DataTable SD_Org = new FillSDFields().FillOrganization();
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
    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtEmailTemplate.Visible == true)
        {
            if (txtEmailTemplate.Text == "")
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
    $"warning_noti('{HttpUtility.JavaScriptStringEncode("Email Template Can Not be Empty !")}'); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
                return;
            }
        }
        SaveEmailTemplate();
    }


    protected void SaveEmailTemplate()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SD_spEmailTemplate", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrgRef", ddlOrg.SelectedValue);
                    cmd.Parameters.AddWithValue("@ReqRef", ddlRequestType.SelectedValue);
                    cmd.Parameters.AddWithValue("@TemplateName", txtEmailTemplate.Text);
                    cmd.Parameters.AddWithValue("@TemplateUpdtName", ddlEmailTemplate.SelectedValue);
                    cmd.Parameters.AddWithValue("@Summary", txtSummary.Text);
                    cmd.Parameters.AddWithValue("@TemplateBody", txtDescription.Value);
                    cmd.Parameters.AddWithValue("@DefaultEmailBody", txtDescription.Value);
                    cmd.Parameters.AddWithValue("@TemplateBodyEncode", System.Web.HttpUtility.HtmlEncode(txtDescription.Value).ToString());
                    cmd.Parameters.AddWithValue("@Option", "UpdateTemplateBody");
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        Session["Popup"] = "Insert";
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
        $"if (window.location.pathname.endsWith('/frmEmailTemplate.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Saved Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            if (ex.ToString().Contains("ThreadAbort"))
            {

            }
            else
            {
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
            }
        }
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("/frmEmailTemplate.aspx");
    }

    protected void ddlEmailTemplate_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillEmailBody();
        btnSave.Text = "Update";
    }

    public void FillEmailBody()
    {
        try
        {
            string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SD_spEmailTemplate"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@TemplateName", ddlEmailTemplate.SelectedValue);
    cmd.Parameters.AddWithValue("@ReqRef", ddlRequestType.SelectedValue);
                    cmd.Parameters.AddWithValue("@OrgRef", ddlOrg.SelectedValue);
                    cmd.Parameters.AddWithValue("@Option", "GetTemplateBody");
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                txtDescription.Value = System.Web.HttpUtility.HtmlDecode(dt.Rows[0]["TemplateBody"].ToString()).ToString();
                                ddlOrg.SelectedValue = dt.Rows[0]["OrgRef"].ToString();
                                txtSummary.Text = dt.Rows[0]["Summary"].ToString();
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
            if (ex.ToString().Contains("ThreadAbort"))
            {

            }
            else
            {
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
            }
        }
    }
    protected void ImgAddEmailTemp_Click(object sender, EventArgs e)
    {
        txtEmailTemplate.Visible = true;
        ddlEmailTemplate.Visible = false;
        btnSave.Text = "Save";
    }

    protected void ddlRequestType_SelectedIndexChanged(object sender, EventArgs e)
    {
        FillEmailTemplate(ddlRequestType.SelectedValue);
    }
}
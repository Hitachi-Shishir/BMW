using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class HelpDesk_frmAddCategory : System.Web.UI.Page
{
    InsertErrorLogs inEr = new InsertErrorLogs();
    Random r = new Random();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["UserID"] == null || Convert.ToString(Session["UserID"]) == "")
            {
                Response.Redirect("/Default.aspx");
            }
            if (!IsPostBack)
            {
                pnlCategory.Visible = true;
                FillOrganizationCategory();
                //ddlOrg6.Enabled = true;
                ddlOrg6.SelectedValue = Convert.ToString(Session["OrgNameDesk"]);
                FillRequestTypeCategory(Convert.ToInt64(ddlOrg6.SelectedValue));
                ddlRequestTypeCategory.SelectedValue = Convert.ToString(Session["ReqType"]);
                Session["SDRef"] = ddlRequestTypeCategory.SelectedValue;
                FillParentCategory();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Category Can Not be Empty !');window.location ='/HelpDesk/frmAddCategory.aspx';", true);
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
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Category Allredy Exists for this Organization!');window.location ='/HelpDesk/frmAddCategory.aspx';", true);

                    //ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/HelpDesk/frmAddCategory.aspx')) {{ warning_noti('{HttpUtility.JavaScriptStringEncode("Category Allredy Exists for this Organization!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                    return;
                }
            }
            Session["Category"] = "OK";
            SaveParentCategory();
            FillParentCategory();
            txtParentCategory.Visible = false;
            Response.Redirect("/HelpDesk/frmAddCategory.aspx");
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('CategoryII Can Not be Empty !');window.location ='/HelpDesk/frmAddCategory.aspx';", true);
                // ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
                // $"warning_noti('{HttpUtility.JavaScriptStringEncode("CategoryII Can Not be Empty !")}'); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
                return;
            }
            else
            {
                string chkduplicate = checkparentcategduplicate(ddlParentCategory.SelectedValue.ToString(), txtCatII.Text.Trim());
                if (chkduplicate != "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('CategoryII Allredy Exists for this Organization!');window.location ='/HelpDesk/frmAddCategory.aspx';", true);
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('CategoryIII Can Not be Empty !');window.location ='/HelpDesk/frmAddCategory.aspx';", true);
                //        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
                //$"warning_noti('{HttpUtility.JavaScriptStringEncode("CategoryIII Can Not be Empty !")}'); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
                return;
            }
            else
            {
                string chkduplicate = checkparentcategduplicate(ddlCatII.SelectedValue.ToString(), txtCatLevelIII.Text.Trim());
                if (chkduplicate != "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('CategoryIII Allredy Exists for this Organization!');window.location ='/HelpDesk/frmAddCategory.aspx';", true);
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('CategoryIV Can Not be Empty !');window.location ='/HelpDesk/frmAddCategory.aspx';", true);
                //        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
                //$"warning_noti('{HttpUtility.JavaScriptStringEncode("CategoryIV Can Not be Empty !")}'); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
                return;
            }
            else
            {
                string chkduplicate = checkparentcategduplicate(ddlCateLevelIII.SelectedValue.ToString(), txtCateLevelIV.Text.Trim());
                if (chkduplicate != "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('CategoryIV Allredy Exists for this Organization!');window.location ='/HelpDesk/frmAddCategory.aspx';", true);
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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('CategoryV Can Not be Empty !');window.location ='/HelpDesk/frmAddCategory.aspx';", true);
                //        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
                //$"warning_noti('{HttpUtility.JavaScriptStringEncode("CategoryV Can Not be Empty !")}'); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
                return;
            }
            else
            {
                string chkduplicate = checkparentcategduplicate(ddlCateLevelIV.SelectedValue.ToString(), txtCatV.Text.Trim());
                if (chkduplicate != "")
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('CategoryV Allredy Exists for this Organization!');window.location ='/HelpDesk/frmAddCategory.aspx';", true);
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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Parent Category Can Not be Empty.');window.location ='/HelpDesk/frmAddCategory.aspx';", true);
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
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/HelpDesk/frmAddCategory.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Response and Resolution Added Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/HelpDesk/frmAddCategory.aspx')) {{ warning_noti('{HttpUtility.JavaScriptStringEncode("Error Something Went Wrong !")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                    }
                }
            }
        }
        catch (Exception ex) { ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"if (window.location.pathname.endsWith('/HelpDesk/frmAddCategory.aspx')) {{ warning_noti('{HttpUtility.JavaScriptStringEncode("" + ex.ToString() + "")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true); }
    }
}
#endregion
using crypto;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using Google.Protobuf.WellKnownTypes;
using Nest;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Interop;

public partial class frmAddNewsUpdates : System.Web.UI.Page
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
        }
        if (Convert.ToString(Session["UserRole"]).ToUpper() != "MASTER")
        {
            ddlOrg.SelectedValue = Convert.ToString(Session["SD_OrgID"]);
            ddlOrg.Enabled = false;
        }
        DataTable dt = getNews();
        grv.DataSource = dt;
        grv.DataBind();
        GridFormat(dt);
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

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (btnAdd.Text == "Update")
        {
            string res = ModifyNews("Update", Convert.ToString(ViewState["Id"]));
            if (res != "")
            {
                ScriptManager.RegisterStartupScript(
                          this,
                          GetType(),
                          "showNotification",
                          $@"
if (window.location.pathname.endsWith('/frmAddNewsUpdates.aspx')) {{
    success_noti('{HttpUtility.JavaScriptStringEncode("News Updated Successfully !")}');
    setTimeout(function() {{
        var url = window.location.href.split('?')[0];  
        url += '?t=' + new Date().getTime(); 
        window.location.href = url; 
    }}, 3000);
}}",
                          true
                      );
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
      $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
            }
        }
        else {
            string res = ModifyNews("Add", "");
            if (res != "")
            {
                ScriptManager.RegisterStartupScript(
                          this,
                          GetType(),
                          "showNotification",
                          $@"
if (window.location.pathname.endsWith('/frmAddNewsUpdates.aspx')) {{
    success_noti('{HttpUtility.JavaScriptStringEncode("News Added Successfully !")}');
    setTimeout(function() {{
        var url = window.location.href.split('?')[0];  
        url += '?t=' + new Date().getTime(); 
        window.location.href = url; 
    }}, 3000);
}}",
                          true
                      );
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
      $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
            }
        }
    }
    public string ModifyNews(string type, string Id)
    {
        String Msg = "";
        try
        {
            //string frmdate = DateTime.TryParseExact(txtFrmDate.Text, "dd-MM-yyyy",
            //          System.Globalization.CultureInfo.InvariantCulture,
            //          System.Globalization.DateTimeStyles.None, out DateTime date)
            //          ? date.ToString("yyyy-MM-dd") : string.Empty;
            //string todate = DateTime.TryParseExact(txtToDate.Text, "dd-MM-yyyy",
            //          System.Globalization.CultureInfo.InvariantCulture,
            //          System.Globalization.DateTimeStyles.None, out DateTime tdate)
            //          ? tdate.ToString("yyyy-MM-dd") : string.Empty;

            using (SqlConnection cnn = new SqlConnection(database.GetConnectstring()))
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cnn;
                cmd.CommandTimeout = 3600;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_InsertSD_NewsUpdate";
                cmd.Parameters.AddWithValue("@News", txtNews.Text.Trim());
                cmd.Parameters.AddWithValue("@frmdate", "1900-01-01");
                cmd.Parameters.AddWithValue("@todate", "1900-01-01");
                cmd.Parameters.AddWithValue("@id", Id);
                cmd.Parameters.AddWithValue("@OrgId", ddlOrg.SelectedValue);
                cmd.Parameters.AddWithValue("@Type", type);
                int res = cmd.ExecuteNonQuery();
                cnn.Close();
                if (res > 0)
                {
                    Msg = "Success";
                }

            }
        }
        catch (Exception ex)
        {
            inEr.InsertErrorLogsF(ex.ToString(), Convert.ToString(DateTime.Now));
        }
        return Msg;
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("frmAddNewsUpdates.aspx");
    }

    protected void lnkEdit_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        GridViewRow gvr = (GridViewRow)btn.NamingContainer;
        HiddenField hdnid = (HiddenField)gvr.FindControl("hdnid");
        string id = hdnid.Value;
        string sql = "select * from SD_NewsUpdate where id = '" + id + "'";
        DataTable dt = database.GetDataTable(sql);
        ddlOrg.SelectedValue = Convert.ToString(dt.Rows[0]["OrgId"]);
        ViewState["Id"] = id;
        String Status = Convert.ToString(dt.Rows[0]["Status"]);
        if (Status.ToLower() == "true")
        {
            ddlStatus.SelectedValue = "1";
        }
        else
        {
            ddlStatus.SelectedValue = "2";
        }
        txtNews.Text = Convert.ToString(dt.Rows[0]["News"]);
        btnAdd.Text = "Update";
    }

    protected void lnkdelete_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        GridViewRow gvr = (GridViewRow)btn.NamingContainer;
        HiddenField hdnid = (HiddenField)gvr.FindControl("hdnid");
        string id = hdnid.Value;
       
        string res = ModifyNews("Delete", id);
        if (res != "")
        {
            ScriptManager.RegisterStartupScript(
                      this,
                      GetType(),
                      "showNotification",
                      $@"
if (window.location.pathname.endsWith('/frmAddNewsUpdates.aspx')) {{
    success_noti('{HttpUtility.JavaScriptStringEncode("News Deleted Successfully !")}');
    setTimeout(function() {{
        var url = window.location.href.split('?')[0];  
        url += '?t=' + new Date().getTime(); 
        window.location.href = url; 
    }}, 3000);
}}",
                      true
                  );
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
  $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
        }
    }
    public DataTable getNews()
    {
        DataTable dt = new DataTable();
        try
        {
            using (SqlConnection con = new SqlConnection(database.GetConnectstring()))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandTimeout = 3600;
                SqlDataAdapter sd = new SqlDataAdapter();
                sd.SelectCommand = cmd;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "SP_InsertSD_NewsUpdate";
                cmd.Parameters.AddWithValue("@OrgId", ddlOrg.SelectedValue);
                cmd.Parameters.AddWithValue("@Type", "get");
                sd.Fill(dt);
                con.Close();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dt;
    }

    protected void ddlOrg_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable dt = getNews();
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
}
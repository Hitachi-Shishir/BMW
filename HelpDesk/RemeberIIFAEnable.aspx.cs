using Microsoft.Exchange.WebServices.Data;
using Nest;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class RemeberIIFAEnable : System.Web.UI.Page
{
    InsertErrorLogs inEr = new InsertErrorLogs();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserName"] == null || Convert.ToString(Session["UserName"]) == "")
        {
            Response.Redirect("/default.aspx");
        }
        if (!IsPostBack)
        {
            try
            {
                getOrg();
                DataTable dt = null;
                getData();
            }
            catch(Exception ex)
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
    private void getOrg()
    {
        try
        {
            string sql = "select * from SD_OrgMaster";
            DataTable dt = database.GetDataTable(sql);
            ddlOrg.DataSource = dt;
            ddlOrg.DataTextField = "OrgName";
            ddlOrg.DataValueField = "Org_ID";
            ddlOrg.DataBind();
            ddlOrg.Items.Insert(0, new ListItem("----Select----", "0"));
        }
        catch (Exception ex)
        {
        }
    }

    public void getData()
    {
        string sql = "select u.UserID,UserName,LoginName,Designation,o.OrgName, RememberISMfa, u.ISMfa,mf.MFAStatus," +
            " DATEDIFF(DAY, Cast(RememberISMfaTime as date), cast(getdate() as date)) - 30 RemainingDays, " +
            "mf.SecretKey from SD_User_Master u left join SD_OrgMaster o on u.Org_ID = o.Org_ID " +
            "left join SD_MFa mf on mf.UserID = u.UserID ";
        if (ddlOrg.SelectedValue != "0")
        {
            sql = sql + " where u.Org_ID='" + ddlOrg.SelectedValue + "'";
        }
        if (ddlUser.SelectedValue != "0")
        {
            sql = sql + " and u.UserID='" + ddlUser.SelectedValue + "'";
        }
        //if (ddlFIlterType.SelectedValue == "1")
        //{
        //    sql = sql + " And RememberISMfa = 1";
        //    DataTable dt = database.GetDataTable(sql);
        //    if (dt.Rows.Count > 0 && dt != null)
        //    {
        //        grd.DataSource = dt;
        //        grd.DataBind();
        //        GridFormat1(dt);
        //    }
        //    else
        //    {
        //        grd.DataSource = dt;
        //        grd.DataBind();
        //    }
        //    RembMFA.Visible = true;
        //    MFA.Visible = false;
        //    //grv.Columns[5].Visible = true;
        //    //grv.Columns[6].Visible = true;
        //    //grv.Columns[7].Visible = true;
        //    //grv.Columns[8].Visible = false;
        //}
        //if (ddlFIlterType.SelectedValue == "2")
        //{
            DataTable dt = database.GetDataTable(sql);
            grv.DataSource = dt;
            grv.DataBind();
            GridFormat(dt);
            RembMFA.Visible = false;
            MFA.Visible = true;
        //}
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
    protected void GridFormat1(DataTable dt)
    {
        grd.UseAccessibleHeader = true;
        grd.HeaderRow.TableSection = TableRowSection.TableHeader;

        if (grd.TopPagerRow != null)
        {
            grd.TopPagerRow.TableSection = TableRowSection.TableHeader;
        }
        if (grd.BottomPagerRow != null)
        {
            grd.BottomPagerRow.TableSection = TableRowSection.TableFooter;
        }
        if (dt.Rows.Count > 0)
            grd.FooterRow.TableSection = TableRowSection.TableFooter;
    }
    protected void grv_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Image imgIsMfaStatus = (Image)e.Row.FindControl("imgIsMfaStatus");
            object isMfaEnabledObj = DataBinder.Eval(e.Row.DataItem, "MFAStatus");
            object secretKey = DataBinder.Eval(e.Row.DataItem, "SecretKey");
            bool isMfaEnabled = (isMfaEnabledObj != null && isMfaEnabledObj != DBNull.Value && Convert.ToBoolean(isMfaEnabledObj));
            if ((secretKey == null || secretKey == DBNull.Value) && isMfaEnabled)
            {
                imgIsMfaStatus.ImageUrl = "~/assets/icon/pngwing.com.png";
                imgIsMfaStatus.ToolTip = "2 FA is Enabled but not Registered!";
            }
            else if (isMfaEnabled)
            {
                imgIsMfaStatus.ImageUrl = "~/assets/icon/checkmark.png";
                imgIsMfaStatus.ToolTip = "2 FA is Enabled and Registered.";
            }
            else
            {
                imgIsMfaStatus.ImageUrl = "~/assets/icon/cross.png";
                imgIsMfaStatus.ToolTip = "2 FA is Disabled!";
            }
        }
    }

    public void getUser()
    {
        try
        {
            string sql = "select * from SD_User_Master where Org_ID='" + ddlOrg.SelectedValue + "'";
            DataTable dt = database.GetDataTable(sql);
            ddlUser.DataSource = dt;
            ddlUser.DataTextField = "UserName";
            ddlUser.DataValueField = "UserID";
            ddlUser.DataBind();
            ddlUser.Items.Insert(0, new ListItem("----Select----", "0"));
        }
        catch (Exception ex)
        {
        }
    }
    protected void ddlOrg_SelectedIndexChanged(object sender, EventArgs e)
    {
        getUser();
        getData();
    }

    protected void btnGO_Click(object sender, EventArgs e)
    {
        getData();
    }
    protected void chkEnable_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk = (CheckBox)sender;
        GridViewRow row = (GridViewRow)chk.NamingContainer;
        HiddenField hdnid = (HiddenField)row.FindControl("hdnid");
        string id = hdnid.Value;
        if (id != "")
        {
            string sql = "UPDATE SD_User_Master set RememberISMfa=0, RememberISMfaTime=null where UserID='" + id + "'";
            database.ExecuteNonQuery(sql);
        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.AbsoluteUri);
    }
    protected void lnkBtn_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        GridViewRow gvr = (GridViewRow)btn.NamingContainer;
        HiddenField hdnid = (HiddenField)gvr.FindControl("hdnuid");
        HiddenField hdnismfa = (HiddenField)gvr.FindControl("hdnismfa");
        string UserID = hdnid.Value;
        string MFA = hdnismfa.Value;
        string sqll = "if exists(select * from SD_Mfa where UserID='" + UserID + "') begin select 'true' end";
        string checkdataMFA = Convert.ToString(database.GetScalarValue(sqll));

        if (MFA.ToUpper() == "TRUE")
        {
            if (checkdataMFA != "")
            {
                string sql = "update SD_Mfa set SecretKey=null, MFAStatus='0' where UserID='" + UserID + "'" +
                            "update SD_User_Master set ISMfa='0' where UserID='" + UserID + "'";
                database.ExecuteNonQuery(sql);
            }
            else
            {
                string sql = "INSERT INTO SD_Mfa (UserID,MFAStatus,InsertDt) " +
                    "VALUES('" + UserID + "','0',GETDATE())";
                database.ExecuteNonQuery(sql);
            }
        }
        else
        {
            if (checkdataMFA != "")
            {
                string sql = "update SD_Mfa set SecretKey=null, MFAStatus='1' where UserID='" + UserID + "'" +
                        "update SD_User_Master set ISMfa='0' where UserID='" + UserID + "'";
                database.ExecuteNonQuery(sql);
            }
            else
            {
                string sql = "INSERT INTO SD_Mfa (UserID,MFAStatus,InsertDt) " +
                    "VALUES('" + UserID + "','1',GETDATE())";
                database.ExecuteNonQuery(sql);
            }
        }
        getData();
    }
    protected void lnkbtnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            HiddenField hdnid = (HiddenField)gvr.FindControl("hdnuid");
            if (hdnid != null)
            {
                string uid = hdnid.Value;
                if (uid != "")
                {
                    string sql = "UPDATE SD_User_Master set RememberISMfa=0, RememberISMfaTime=null where UserID='" + uid + "'";
                    database.ExecuteNonQuery(sql);
                }
            }
            getData();
        }
        catch { }
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
}
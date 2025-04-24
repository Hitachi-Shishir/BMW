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

public partial class frmAddUsersRoles : System.Web.UI.Page
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
            FillUsers();
            Roles();
        }
    }
    private void FillUsers()
    {
        try
        {
            string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("select  distinct RoleName  from SD_Role order by RoleName asc"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            ddlUsers.DataSource = dt;
                            ddlUsers.DataTextField = "RoleName";
                            ddlUsers.DataBind();
                        }
                    }
                }
                ddlUsers.Items.Insert(0, new ListItem("--Select Role--", "0"));
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
    private void FillScopes()
    {
        try
        {
            string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("Select  distinct ID,MenuStatus, [MenuID],[MenuName]  from SD_Navigation where MenuID not in (select  MenuID from SD_roles where UserRole=@UserRole)  order by [MenuName] asc", con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@UserRole", ddlUsers.SelectedItem.ToString());
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            gvMasterRoles.DataSource = dt;
                            gvMasterRoles.DataBind();
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
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                inEr.InsertErrorLogsF(Session["UserName"].ToString()
    , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
     $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }
    }
    protected void btnRoleApply_Click(object sender, EventArgs e)
    {
        try
        {
            int MenuID;
            string strname = string.Empty;
            foreach (GridViewRow gvrow in gvMasterRoles.Rows)
            {
                CheckBox chk = (CheckBox)gvrow.FindControl("chkSelect");
                if (chk != null & chk.Checked)
                {
                    MenuID = Convert.ToInt32(gvMasterRoles.DataKeys[gvrow.RowIndex].Value);

                    string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        using (SqlCommand cmd = new SqlCommand("insert into dbo.SD_roles(MenuID,MenuName,UserName,MenuStatus)values(@MenuID,@MenuName,@UserName,@MenuStatus)", con))
                        {
                            cmd.Parameters.AddWithValue("@MenuID", MenuID);
                            cmd.Parameters.AddWithValue("@MenuName", (gvrow.Cells[1].Text).Replace("&amp;", "").ToString());
                            cmd.Parameters.AddWithValue("@UserName", ddlUsers.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@MenuStatus", "Active");
                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            FillScopes();
            FillAllRoles();
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
    protected void Roles()
    {
        try
        {
            string sql = "select RoleID,RoleName,InsertDt from SD_Role order by RoleName asc";
            string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(sql))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            grdRoles.DataSource = dt;
                            grdRoles.DataBind();
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
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                inEr.InsertErrorLogsF(Session["UserName"].ToString()
    , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
    $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }

    }
    protected void lnkdelete_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        GridViewRow gvr = (GridViewRow)btn.NamingContainer;
        HiddenField hdnid = (HiddenField)gvr.FindControl("hdnid");
        HiddenField hdnRoleName = (HiddenField)gvr.FindControl("hdnRoleName");
        string RoleName = hdnRoleName.Value;
        string id = hdnid.Value;
        try
        {
            if (RoleName.ToLower() != "master")
            {
                string sql = "delete from SD_Role where RoleID = '" + id + "'";
                database.ExecuteNonQuery(sql);
                Roles();
                FillUsers();
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
    $"if (window.location.pathname.endsWith('/frmTechLeaveApply.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Deleted Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Master Cannot be Deleted !');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Error data not Deleted !');window.location ='frmTechLeaveApply.aspx';", true);
        }
    }
    protected void lnkdeletemenu_Click(object sender, EventArgs e)
    {
        try
        {
            LinkButton btn = (LinkButton)sender;
            GridViewRow gvr = (GridViewRow)btn.NamingContainer;
            HiddenField hdnid = (HiddenField)gvr.FindControl("hdnids");
            HiddenField hdnMenuStatus = (HiddenField)gvr.FindControl("hdnMenuStatus");
            string id = hdnid.Value;
            string status = hdnMenuStatus.Value;

            if (status == "InActive")
            {
                string sql = "update SD_Navigation set MenuStatus='Active' where id = '" + id + "'";
                database.ExecuteNonQuery(sql);
            }
            else
            {
                string sql = "update SD_Navigation set MenuStatus='InActive' where id = '" + id + "'";
                database.ExecuteNonQuery(sql);
            }
            Roles();
            FillScopes();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Menu Status Updated !');window.location ='frmAddUsersRoles.aspx';", true);

        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Error data not Deleted !');window.location ='frmAddUsersRoles.aspx';", true);
        }
    }
    protected void btnMasterRoleApply_Click(object sender, EventArgs e)
    {
        try
        {
            int MenuID;
            foreach (GridViewRow gvrow in gvMasterRoles.Rows)
            {
                CheckBox chk = (CheckBox)gvrow.FindControl("chkSelect");
                if (chk != null && chk.Checked)
                {
                    if (gvMasterRoles.DataKeys.Count > gvrow.RowIndex)
                    {
                        MenuID = Convert.ToInt32(gvMasterRoles.DataKeys[gvrow.RowIndex].Value);
                        string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                        using (SqlConnection con = new SqlConnection(constr))
                        {
                            using (SqlCommand cmd = new SqlCommand("insert into dbo.SD_roles( MenuID,MenuName,UserRole,MenuStatus)values(@MenuID,@MenuName,@UserRole,@MenuStatus)", con))
                            {
                                cmd.Parameters.AddWithValue("@MenuID", MenuID);
                                cmd.Parameters.AddWithValue("@MenuName", gvrow.Cells[1].Text);
                                cmd.Parameters.AddWithValue("@UserRole", ddlUsers.SelectedItem.ToString());
                                cmd.Parameters.AddWithValue("@MenuStatus", "Active");
                                con.Open();
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    else
                    {
                        // Log the error or display a message to the user
                        inEr.InsertErrorLogsF(Session["UserName"].ToString(), $"Index was out of range in gvMasterRoles. RowIndex: {gvrow.RowIndex}");
                    }
                }
            }
            FillAllRoles();
            FillScopes();
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
                inEr.InsertErrorLogsF(Session["UserName"].ToString(), $" {Request.Url.ToString()} Got Exception. Line Number: {line} {ex.ToString()}");
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", "error_noti(); setTimeout(function() { window.location.reload(); }, 2000);", true);
            }
        }
    }
    protected void ddlUsers_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlUsers.SelectedIndex == 0)
            {
                lblMenuList.Text = "Menu List";
                gvMasterRoles.DataSource = null;
                gvMasterRoles.DataBind();
                gvAllRoles.DataSource = null;
                gvAllRoles.DataBind();
            }
            else
            {
                if (ddlUsers.SelectedItem.Text.ToLower() != "master")
                {
                    lblMenuList.Text = ddlUsers.SelectedItem.Text + " Menu List";
                    FillAllRoles();
                    FillScopes();
                }
                else
                {
                    lblMenuList.Text = "Master" + " Menu List";
                    gvAllRoles.DataSource = null;
                    gvAllRoles.DataBind();
                    gvMasterRoles.DataSource = null;
                    gvMasterRoles.DataBind();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "alert", "alert('Master Cannot be Modified !');", true);
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
            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
    $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
        }
    }
    private void FillAllRoles()
    {
        try
        {
            string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(@"select * from SD_roles  where UserRole  = '" + ddlUsers.SelectedItem + "' and MenuStatus ='Active'  order by MenuName asc"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            gvAllRoles.DataSource = dt;
                            gvAllRoles.DataBind();
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
                var frame = st.GetFrame(0);
                var line = frame.GetFileLineNumber();
                inEr.InsertErrorLogsF(Session["UserName"].ToString()
    , " " + Request.Url.ToString() + "Got Exception" + "Line Number :" + line.ToString() + ex.ToString());
                ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
    $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }
    }
    protected void btnUpdateRoles_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (GridViewRow gvrow in gvAllRoles.Rows)
            {
                CheckBox chk = (CheckBox)gvrow.FindControl("chkSelect");
                if (chk != null & chk.Checked)
                {
                    int ID = Convert.ToInt32(gvAllRoles.DataKeys[gvrow.RowIndex].Value);
                    string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        using (SqlCommand cmd = new SqlCommand("Update dbo.SD_roles SET MenuStatus=@MenuStatus where ID=@ID and UserName=@UserName ", con))
                        {
                            cmd.Parameters.AddWithValue("@ID", ID);
                            cmd.Parameters.AddWithValue("@UserName", ddlUsers.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@MenuStatus", "Active");
                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                else
                {
                    int ID = Convert.ToInt32(gvAllRoles.DataKeys[gvrow.RowIndex].Value);
                    string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        using (SqlCommand cmd = new SqlCommand("Update dbo.SD_roles SET MenuStatus=@MenuStatus where ID=@ID and UserName=@UserName ", con))
                        {
                            cmd.Parameters.AddWithValue("@ID", ID);
                            cmd.Parameters.AddWithValue("@UserName", ddlUsers.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@MenuStatus", "Inactive");
                            con.Open();
                            cmd.ExecuteNonQuery();
                            FillScopes();
                            FillAllRoles();
                        }
                    }
                }
            }
            FillScopes();
            FillAllRoles();
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
    protected void btnSaveRole_Click(object sender, EventArgs e)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Insert into SD_Role (RoleName,InsertBy,InsertDt,IsActive) values(@RoleName,@InsertBy,Getdate(),1)", con))
                {
                    con.Open();
                    cmd.Parameters.AddWithValue("@RoleName", txtRoleName.Text.Trim());
                    cmd.Parameters.AddWithValue("@InsertBy", Session["UserName"].ToString());
                    cmd.ExecuteNonQuery();
                    con.Close();
                    ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
                            $"if (window.location.pathname.endsWith('/frmAddUsersRoles.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Role added Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
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
    protected void gvAllRoles_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "DeleterRole")
            {
                int RowIndex = Convert.ToInt32(e.CommandArgument);
                string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM SD_roles WHERE ID=@ID "))
                    {
                        cmd.Parameters.AddWithValue("@ID", RowIndex);
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        FillAllRoles();
                        FillScopes();
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
}
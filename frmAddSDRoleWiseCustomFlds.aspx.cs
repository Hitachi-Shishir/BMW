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

public partial class frmAddSDRoleWiseCustomFlds : System.Web.UI.Page
{
    InsertErrorLogs inEr = new InsertErrorLogs();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserName"] == null)
        {
            Response.Redirect("~/Default.aspx");
        }
        if (!IsPostBack)
        {

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

    private void FillScopes()
    {
        try
        {
            string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand(@"select FieldID as 'MenuID',FieldValue as 'MenuName',Deskref from SD_CustomFieldControl where FieldName 
not in ( select distinct MenuName from sd_customFldRole where UserRole=@UserRole)", con))
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
                        using (SqlCommand cmd = new SqlCommand("insert into dbo.sd_customFldRole(MenuID,MenuName,UserRole,MenuStatus)values(@MenuID,@MenuName,@UserRole,@MenuStatus)", con))
                        {
                            cmd.Parameters.AddWithValue("@MenuID", MenuID);
                            cmd.Parameters.AddWithValue("@MenuName", (gvrow.Cells[1].Text).Replace("&amp;", "").ToString());
                            cmd.Parameters.AddWithValue("@UserRole", ddlUsers.SelectedValue.ToString());
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
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",$"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }

    }

    protected void btnMasterRoleApply_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (GridViewRow gvrow in gvMasterRoles.Rows)
            {
                CheckBox chk = (CheckBox)gvrow.FindControl("chkSelect");
                if (chk != null & chk.Checked)
                {
                    string MenuID = gvMasterRoles.DataKeys[gvrow.RowIndex].Value.ToString();
                    string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(constr))
                    {

                        using (SqlCommand cmd = new SqlCommand("insert into dbo.sd_customFldRole(MenuID,MenuName,UserRole,MenuStatus)values(@MenuID,@MenuName,@UserRole,@MenuStatus)", con))
                        {
                            cmd.Parameters.AddWithValue("@MenuID", MenuID);
                            cmd.Parameters.AddWithValue("@MenuName", (gvrow.Cells[1].Text).Replace("&amp;", "").ToString());
                            cmd.Parameters.AddWithValue("@UserRole", ddlUsers.SelectedValue.ToString());
                            cmd.Parameters.AddWithValue("@MenuStatus", "Active");
                            con.Open();
                            cmd.ExecuteNonQuery();
                            FillAllRoles();
                            FillScopes();
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
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",$"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

            }
        }

    }

    protected void ddlUsers_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlUsers.SelectedIndex == 0)
            {
                gvMasterRoles.DataSource = null;
                gvMasterRoles.DataBind();
                gvAllRoles.DataSource = null;
                gvAllRoles.DataBind();
            }
            else

            {
                FillAllRoles();
                FillScopes();
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
                    ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",$"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
        }
    }

    private void FillAllRoles()
    {
        try
        {
            string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {

                using (SqlCommand cmd = new SqlCommand(@"select * from 
            sd_customFldRole  where UserRole  = '" + ddlUsers.SelectedItem + "'"))
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
                        using (SqlCommand cmd = new SqlCommand("Update dbo.sd_customFldRole SET MenuStatus=@MenuStatus where ID=@ID and UserRole=@UserRole ", con))
                        {
                            cmd.Parameters.AddWithValue("@ID", ID);
                            cmd.Parameters.AddWithValue("@UserRole", ddlUsers.SelectedItem.ToString());
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
                        using (SqlCommand cmd = new SqlCommand("Update dbo.sd_customFldRole SET MenuStatus=@MenuStatus where ID=@ID and UserRole=@UserRole ", con))
                        {
                            cmd.Parameters.AddWithValue("@ID", ID);
                            cmd.Parameters.AddWithValue("@UserRole", ddlUsers.SelectedItem.ToString());
                            cmd.Parameters.AddWithValue("@MenuStatus", "Active");
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
                    Response.Redirect(Request.Url.AbsoluteUri);
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
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",$"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

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
                    using (SqlCommand cmd = new SqlCommand("DELETE FROM sd_customFldRole WHERE ID=@ID "))
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
}
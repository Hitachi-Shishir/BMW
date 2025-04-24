using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using Nest;
using System.Diagnostics;
using System.Threading;

public partial class Admin_frmAddLocation : System.Web.UI.Page
{
    errorMessage msg = new errorMessage();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null)
        {
            Response.Redirect("~/default.aspx");
        }
        if (!IsPostBack)
        {
            FillLicenseType();
            FillOrganization();
            if (Convert.ToString(Session["UserRole"]).ToUpper() == "ADMIN" || Convert.ToString(Session["UserRole"]).ToUpper() == "MASTER")
            {
                ddlOrg.SelectedValue = Convert.ToString(Session["SD_OrgID"]);
                ddlOrg.Enabled = true;
            }
        }
        btnTypeAdd.Visible = true;
        btnUpdate.Visible = false;
        pnlViewDepart.Visible = true;
        btnViewDep.Enabled = false;
        btnViewDep.CssClass = "btn btn-sm btn-secondary";
    }
    private void FillLicenseType()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SD_spLocation_Master", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Option", "SelectAll");
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            da.Fill(dt);
                            gvstate.DataSource = dt;
                            gvstate.DataBind();
                            if (dt.Rows.Count > 0)
                            {
                                GridFormat(dt);
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            msg.ReportError1(ex.Message);
            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
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
    protected void btnTypeAdd_Click(object sender, EventArgs e)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand("SD_spLocation_Master", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LocCode", txtLocationCode.Text.Replace(" ", string.Empty));
                    cmd.Parameters.AddWithValue("@LocName", txtLocationName.Text);
                    cmd.Parameters.AddWithValue("@InsertBy", Convert.ToInt64(Session["UserID"]));
                    cmd.Parameters.AddWithValue("@OrgID", ddlOrg.SelectedValue);
                    cmd.Parameters.AddWithValue("@IsActive", '1');
                    cmd.Parameters.AddWithValue("@Option", "Insert");
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
        $"if (window.location.pathname.endsWith('/frmAddLocation.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Saved Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                }
            }
        }
        catch (Exception ex)
        {
            msg.ReportError1(ex.Message);
            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
        }
    }
    protected void ImgBtnExport_Click(object sender, ImageClickEventArgs e)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=State Details.xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";
        using (StringWriter sw = new StringWriter())
        {
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            //To Export all pages
            gvstate.AllowPaging = false;
            FillLicenseType();

            gvstate.HeaderRow.BackColor = Color.White;
            foreach (TableCell cell in gvstate.HeaderRow.Cells)
            {
                cell.BackColor = gvstate.HeaderStyle.BackColor;
            }
            foreach (GridViewRow row in gvstate.Rows)
            {
                row.BackColor = Color.White;
                foreach (TableCell cell in row.Cells)
                {
                    if (row.RowIndex % 2 == 0)
                    {
                        cell.BackColor = gvstate.AlternatingRowStyle.BackColor;
                    }
                    else
                    {
                        cell.BackColor = gvstate.RowStyle.BackColor;
                    }
                    cell.CssClass = "textmode";
                }
            }

            gvstate.RenderControl(hw);

            //style to format numbers to string
            string style = @"<style> .textmode { } </style>";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }

    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }
    protected void gvstate_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "DeleteEx")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                string ID = gvstate.DataKeys[rowIndex].Values["ID"].ToString();
                try
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand("SD_spLocation_Master", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ID", ID);
                            cmd.Parameters.AddWithValue("@OrgID", Convert.ToInt64(Session["SD_OrgID"]));
                            cmd.Parameters.AddWithValue("@Option", "Delete");
                            cmd.CommandTimeout = 180;
                            cmd.ExecuteNonQuery();
                            System.Text.StringBuilder sb = new System.Text.StringBuilder();
                            sb.Append(@"<script type='text/javascript'>");
                            sb.Append("$('#myModal').modal('show');");
                            sb.Append(@"</script>");
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ModalScript", sb.ToString(), false);
                            lblsuccess.ForeColor = System.Drawing.Color.Green;
                            con.Close();
                            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
        $"if (window.location.pathname.endsWith('/frmAddLocation.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Deleted Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                            FillLicenseType();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);

                }

            }
            if (e.CommandName == "SelectState")
            {
                //Determine the RowIndex of the Row whose Button was clicked.
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                //Get the value of column from the DataKeys using the RowIndex.
                ViewState["ID"] = gvstate.DataKeys[rowIndex].Values["ID"].ToString();
                txtLocationCode.Text = gvstate.Rows[rowIndex].Cells[1].Text.Trim();
                txtLocationName.Text = gvstate.Rows[rowIndex].Cells[2].Text.Trim();
                ddlOrg.SelectedValue = gvstate.Rows[rowIndex].Cells[3].Text.Trim();
                btnTypeAdd.Visible = false;
                btnUpdate.Visible = true;
                ShowAddDepartmPanel();


            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
        }
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SD_spLocation_Master", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@LocCode", txtLocationCode.Text);
                    cmd.Parameters.AddWithValue("@LocName", txtLocationName.Text);
                    cmd.Parameters.AddWithValue("@UpdateBy", Convert.ToInt64(Session["UserID"]));
                    cmd.Parameters.AddWithValue("@ID", Convert.ToString(ViewState["ID"]));
                    cmd.Parameters.AddWithValue("@OrgID", ddlOrg.SelectedValue);
                    cmd.Parameters.AddWithValue("@IsActive", '1');
                    cmd.Parameters.AddWithValue("@Option", "Update");
                    con.Open();
                    cmd.ExecuteNonQuery();
                    ScriptManager.RegisterStartupScript(this, GetType(), "showNotification",
        $"if (window.location.pathname.endsWith('/frmAddLocation.aspx')) {{ success_noti('{HttpUtility.JavaScriptStringEncode("Updated Successfully!")}'); setTimeout(function() {{ window.location.reload(); }}, 2000); }}", true);
                }
            }
        }
        catch (Exception ex)
        {
            msg.ReportError1(ex.Message);
            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
        }
    }
    protected void btnAddDep_Click(object sender, EventArgs e)
    {
        ShowAddDepartmPanel();
        FillLicenseType();
    }
    protected void btnViewDep_Click(object sender, EventArgs e)
    {
        pnlAddDep.Visible = false;
        btnAddDep.CssClass = "btn btn-sm btn-outline-secondary";
        pnlViewDepart.Visible = true;
        btnViewDep.CssClass = "btn btn-sm btn-secondary";
        btnViewDep.Enabled = false;
        btnAddDep.Enabled = true;
        FillLicenseType();

    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(Request.Url.AbsoluteUri);
    }
    protected void ShowAddDepartmPanel()
    {
        pnlAddDep.Visible = true;
        btnAddDep.CssClass = "btn btn-sm btn-secondary";
        pnlViewDepart.Visible = false;
        btnViewDep.CssClass = "btn btn-sm btn-outline-secondary";
        btnViewDep.Enabled = true;
        btnAddDep.Enabled = false;
    }
    protected void GridFormat(DataTable dt)
    {
        gvstate.UseAccessibleHeader = true;
        gvstate.HeaderRow.TableSection = TableRowSection.TableHeader;

        if (gvstate.TopPagerRow != null)
        {
            gvstate.TopPagerRow.TableSection = TableRowSection.TableHeader;
        }
        if (gvstate.BottomPagerRow != null)
        {
            gvstate.BottomPagerRow.TableSection = TableRowSection.TableFooter;
        }
        if (dt.Rows.Count > 0)
            gvstate.FooterRow.TableSection = TableRowSection.TableFooter;
    }
}
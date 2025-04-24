using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_frmEmailAll : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null)
        {
            Response.Redirect("~/default.aspx");
        }
        if (!IsPostBack)
        {
            FillEmailLogs("Select", "");

        }
    }
    private void FillEmailLogs(string FunctionName, string Search)
    {
        try
        {
            string constr = ConfigurationManager.ConnectionStrings["con"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SDsp_Emaillog", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Search", Search);
                    cmd.Parameters.AddWithValue("@Option", FunctionName);
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                lblTotalRecords.Text = dt.Rows.Count.ToString();
                                gvAllEmailLogs.DataSource = dt;
                                gvAllEmailLogs.DataBind();
                            }
                            else
                            {
                                gvAllEmailLogs.DataSource = null;
                                gvAllEmailLogs.DataBind();
                            }
                            GridFormat(dt);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", $"error_noti(); setTimeout(function() {{ window.location.reload(); }}, 2000);", true);
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        FillEmailLogs("Filter", "");
    }
    protected void gvAllIssuanceLetter_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        WebClient req = new WebClient();
        HttpResponse response = HttpContext.Current.Response;
        string filePath = e.CommandArgument.ToString();
        response.Clear();
        response.ClearContent();
        response.ClearHeaders();
        response.Buffer = true;
        response.AddHeader("Content-Disposition", "attachment;filename=IssuanceLetter.pdf");
        byte[] data = req.DownloadData(Server.MapPath(filePath));
        response.BinaryWrite(data);
        response.End();
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        FillEmailLogs("Select", "");
    }
    protected void ImageBtnExport_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void GridFormat(DataTable dt)
    {
        gvAllEmailLogs.UseAccessibleHeader = true;
        gvAllEmailLogs.HeaderRow.TableSection = TableRowSection.TableHeader;

        if (gvAllEmailLogs.TopPagerRow != null)
        {
            gvAllEmailLogs.TopPagerRow.TableSection = TableRowSection.TableHeader;
        }
        if (gvAllEmailLogs.BottomPagerRow != null)
        {
            gvAllEmailLogs.BottomPagerRow.TableSection = TableRowSection.TableFooter;
        }
        if (dt.Rows.Count > 0)
            gvAllEmailLogs.FooterRow.TableSection = TableRowSection.TableFooter;
    }
}
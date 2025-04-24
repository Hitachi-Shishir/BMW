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

public partial class Reports_rptDBSDII : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null)
        {
            Response.Redirect("/Default.aspx");
        }
        try
        {
            if (Request.QueryString["Data"] != "")
            {

                if (Request.QueryString["ReportType"] == "ResponseSLADetails")
                {
                    ResponseSlaDetails();
                }

                if (Request.QueryString["ReportType"] == "SeverityWisePieDetails")
                {
                    SeverityWisePieDetails();
                }
                if (Request.QueryString["ReportType"] == "SeverityWiseResponseDetails")
                {
                    SeverityWiseResponseDetails();
                }

                if (Request.QueryString["ReportType"] == "ResolutionSLAPie")
                {
                    ResolutionSlaDetails();
                }

                if (Request.QueryString["ReportType"] == "AllSeverityWiseResolveCalls")
                {
                    AllSeverityWiseResolveCalls();
                }
                if (Request.QueryString["ReportType"] == "SeverityWiseResolution")
                {
                    SeverityWiseResolution();
                }
            }
        }
        catch
        {

        }
    }
    private void SeverityWiseResolution()
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand("SD_SDDashboardMainWithFilter"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ServiceDesk", Request.QueryString["ServiceDesk"].ToString());
                cmd.Parameters.AddWithValue("@TicketStatus", Request.QueryString["chartdata"].ToString());
                //	cmd.Parameters.AddWithValue("@Scope", Session["UserScope"].ToString());
                cmd.Parameters.AddWithValue("@OrgID", Request.QueryString["OrgId"].ToString());
                cmd.Parameters.AddWithValue("@FROM", Request.QueryString["From"].ToString());
                cmd.Parameters.AddWithValue("@TO", Request.QueryString["To"].ToString());
                cmd.Parameters.AddWithValue("@Option", "SeverityWiseResolutionDetails");
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataSet ds = new DataSet())
                    {
                        sda.Fill(ds);
                        lblTotal.Text = ds.Tables[0].Rows.Count.ToString();
                        gvSDTicketsDetails.DataSource = ds.Tables[0];
                        gvSDTicketsDetails.DataBind();
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            GridFormat(ds.Tables[0]);
                        }
                    }
                }
            }
        }
    }
    private void AllSeverityWiseResolveCalls()
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand("SD_SDDashboardMainWithFilter"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ServiceDesk", Request.QueryString["ServiceDesk"].ToString());
                cmd.Parameters.AddWithValue("@TicketStatus", Request.QueryString["chartdata"].ToString());
                //		cmd.Parameters.AddWithValue("@Scope", Session["UserScope"].ToString());
                cmd.Parameters.AddWithValue("@FROM", Request.QueryString["From"].ToString());
                cmd.Parameters.AddWithValue("@TO", Request.QueryString["To"].ToString());
                cmd.Parameters.AddWithValue("@OrgID", Request.QueryString["OrgId"].ToString());
                cmd.Parameters.AddWithValue("@Option", "SeverityWiseResolvedPieDetails");
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataSet ds = new DataSet())
                    {
                        sda.Fill(ds);
                        lblTotal.Text = ds.Tables[0].Rows.Count.ToString();
                        gvSDTicketsDetails.DataSource = ds.Tables[0];
                        gvSDTicketsDetails.DataBind();
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            GridFormat(ds.Tables[0]);
                        }
                    }
                }
            }
        }
    }
    private void ResolutionSlaDetails()
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand("SD_SDDashboardMainWithFilter"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ServiceDesk", Request.QueryString["ServiceDesk"].ToString());
                cmd.Parameters.AddWithValue("@TicketStatus", Request.QueryString["chartdata"].ToString());
                //		cmd.Parameters.AddWithValue("@Scope", Session["UserScope"].ToString());
                cmd.Parameters.AddWithValue("@FROM", Request.QueryString["From"].ToString());
                cmd.Parameters.AddWithValue("@TO", Request.QueryString["To"].ToString());
                cmd.Parameters.AddWithValue("@OrgID", Request.QueryString["OrgId"].ToString());
                cmd.Parameters.AddWithValue("@Option", "ResolutionSLAPieDetails");
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataSet ds = new DataSet())
                    {
                        sda.Fill(ds);
                        lblTotal.Text = ds.Tables[0].Rows.Count.ToString();
                        gvSDTicketsDetails.DataSource = ds.Tables[0];
                        gvSDTicketsDetails.DataBind();
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            GridFormat(ds.Tables[0]);
                        }
                    }
                }
            }
        }
    }
    private void SeverityWiseResponseDetails()
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand("SD_SDDashboardMainWithFilter"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ServiceDesk", Request.QueryString["ServiceDesk"].ToString());
                cmd.Parameters.AddWithValue("@TicketStatus", Request.QueryString["chartdata"].ToString());
                //	cmd.Parameters.AddWithValue("@Scope", Session["UserScope"].ToString());
                cmd.Parameters.AddWithValue("@FROM", Request.QueryString["From"].ToString());
                cmd.Parameters.AddWithValue("@TO", Request.QueryString["To"].ToString());
                cmd.Parameters.AddWithValue("@OrgID", Request.QueryString["OrgId"].ToString());
                cmd.Parameters.AddWithValue("@Option", "SeverityWiseResponseDetails");
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataSet ds = new DataSet())
                    {
                        sda.Fill(ds);
                        lblTotal.Text = ds.Tables[0].Rows.Count.ToString();
                        gvSDTicketsDetails.DataSource = ds.Tables[0];
                        gvSDTicketsDetails.DataBind();
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            GridFormat(ds.Tables[0]);
                        }
                    }
                }
            }
        }
    }
    private void SeverityWisePieDetails()
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand("SD_SDDashboardMainWithFilter"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ServiceDesk", Request.QueryString["ServiceDesk"].ToString());
                cmd.Parameters.AddWithValue("@TicketStatus", Request.QueryString["chartdata"].ToString());
                //	cmd.Parameters.AddWithValue("@Scope", Session["UserScope"].ToString());
                cmd.Parameters.AddWithValue("@FROM", Request.QueryString["From"].ToString());
                cmd.Parameters.AddWithValue("@TO", Request.QueryString["To"].ToString());
                cmd.Parameters.AddWithValue("@OrgID", Request.QueryString["OrgId"].ToString());
                cmd.Parameters.AddWithValue("@Option", "SeverityWisePieDetails");
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    cmd.CommandTimeout = 950;
                    using (DataSet ds = new DataSet())
                    {
                        sda.Fill(ds);
                        lblTotal.Text = ds.Tables[0].Rows.Count.ToString();
                        gvSDTicketsDetails.DataSource = ds.Tables[0];
                        gvSDTicketsDetails.DataBind();
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            GridFormat(ds.Tables[0]);
                        }
                    }
                }
            }
        }
    }
    private void ResponseSlaDetails()
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand("SD_SDDashboardMainWithFilter"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ServiceDesk", Request.QueryString["ServiceDesk"].ToString());
                cmd.Parameters.AddWithValue("@TicketStatus", Request.QueryString["chartdata"].ToString());
                //	cmd.Parameters.AddWithValue("@Scope", Session["UserScope"].ToString());
                cmd.Parameters.AddWithValue("@FROM", Request.QueryString["From"].ToString());
                cmd.Parameters.AddWithValue("@TO", Request.QueryString["To"].ToString());
                cmd.Parameters.AddWithValue("@OrgID", Request.QueryString["OrgId"].ToString());
                cmd.Parameters.AddWithValue("@Option", "ResponseSLAPieDetails");
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    cmd.CommandTimeout = 950;
                    using (DataSet ds = new DataSet())
                    {
                        sda.Fill(ds);
                        lblTotal.Text = ds.Tables[0].Rows.Count.ToString();
                        gvSDTicketsDetails.DataSource = ds.Tables[0];
                        gvSDTicketsDetails.DataBind();
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            GridFormat(ds.Tables[0]);
                        }
                    }
                }
            }
        }
    }
    protected void GridFormat(DataTable dt)
    {
        try
        {
            gvSDTicketsDetails.UseAccessibleHeader = true;
            gvSDTicketsDetails.HeaderRow.TableSection = TableRowSection.TableHeader;

            if (gvSDTicketsDetails.TopPagerRow != null)
            {
                gvSDTicketsDetails.TopPagerRow.TableSection = TableRowSection.TableHeader;
            }
            if (gvSDTicketsDetails.BottomPagerRow != null)
            {
                gvSDTicketsDetails.BottomPagerRow.TableSection = TableRowSection.TableFooter;
            }
            if (dt.Rows.Count > 0)
                gvSDTicketsDetails.FooterRow.TableSection = TableRowSection.TableFooter;
        }
        catch
        {

        }
    }
    protected void ImgbtnExport_Click(object sender, ImageClickEventArgs e)
    {

        try
        {

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=PCVReport.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //To Export all pages
                gvSDTicketsDetails.AllowPaging = false;
                // this.BindGrid();


                foreach (TableCell cell in gvSDTicketsDetails.HeaderRow.Cells)
                {
                    cell.BackColor = gvSDTicketsDetails.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in gvSDTicketsDetails.Rows)
                {

                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = gvSDTicketsDetails.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = gvSDTicketsDetails.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }

                gvSDTicketsDetails.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }
        catch (Exception ex)
        {
            //msg.ReportError(ex.Message);

        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }
}
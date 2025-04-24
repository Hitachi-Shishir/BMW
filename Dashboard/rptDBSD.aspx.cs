using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Dashboard_rptDBSD : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["LoginName"] == null)
        {
            Response.Redirect("/Default.aspx");
        }
        if (Request.QueryString["Data"] != "")
        {

            if (Request.QueryString["UserScope"] != "Technician")
            {

                if (Request.QueryString["Data"] == "SevenDaysTickets")
                {
                    FillSevenDaysTicketsDetails("SD_SDDashboardMain");
                }
                if (Request.QueryString["Data"] == "TodayTickets")
                {
                    FillTodayTicketsDetails("SD_SDDashboardMain");
                }
                if (Request.QueryString["Data"] == "ThirtyDaysTickets")
                {
                    Fill30DaysTickets("SD_SDDashboardMain");
                }
                if (Request.QueryString["Data"] == "TotalTickets")
                {
                    AllTickets("SD_SDDashboardMain");
                }
                if (Request.QueryString["ReportType"] == "TodayTicketsStatusWise")
                {
                    TodayTicketsStatusWise("SD_SDDashboardMain");
                }
                if (Request.QueryString["ReportType"] == "7DaysTicketsStatusWise")
                {
                    SevenDaysTicketsStatusWise("SD_SDDashboardMain");
                }
                if (Request.QueryString["ReportType"] == "30DaysTicketsStatusWise")
                {
                    OneMonthTicketsStatusWise("SD_SDDashboardMain");
                }
                if (Request.QueryString["ReportType"] == "30DaysTicketsStatusWise")
                {
                    OneMonthTicketsStatusWise("SD_SDDashboardMain");
                }
                if (Request.QueryString["ReportType"] == "AllTicketsStatusWise")
                {
                    AllTicketsStatusWise("SD_SDDashboardMain");
                }
            }
            else
            {
                if (Request.QueryString["Data"] == "SevenDaysTickets")
                {
                    FillSevenDaysTicketsDetails("SD_SDDashboardMain");
                }
                if (Request.QueryString["Data"] == "TodayTickets")
                {
                    FillTodayTicketsDetails("SD_SDDashboardMain");
                }
                if (Request.QueryString["Data"] == "ThirtyDaysTickets")
                {
                    Fill30DaysTickets("SD_SDDashboardMain");
                }
                if (Request.QueryString["Data"] == "TotalTickets")
                {
                    AllTickets("SD_SDDashboardMain");
                }
                if (Request.QueryString["ReportType"] == "TodayTicketsStatusWise")
                {
                    TodayTicketsStatusWise("SD_SDDashboardMain");
                }
                if (Request.QueryString["ReportType"] == "7DaysTicketsStatusWise")
                {
                    SevenDaysTicketsStatusWise("SD_SDDashboardMain");
                }
                if (Request.QueryString["ReportType"] == "30DaysTicketsStatusWise")
                {
                    OneMonthTicketsStatusWise("SD_SDDashboardMain");
                }
                if (Request.QueryString["ReportType"] == "30DaysTicketsStatusWise")
                {
                    OneMonthTicketsStatusWise("SD_SDDashboardMain");
                }
                if (Request.QueryString["ReportType"] == "AllTicketsStatusWise")
                {
                    AllTicketsStatusWise("SD_SDDashboardMain");
                }
            }

        }
    }
    private void SeverityWiseResolution()
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand("SD_SDDashboardMain1"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Scope", Session["UserScope"].ToString());
                cmd.Parameters.AddWithValue("@TicketStatus", Request.QueryString["chartdata"].ToString());
                cmd.Parameters.AddWithValue("@Option", "SeverityWiseResolutionDetails");
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    sda.SelectCommand.CommandTimeout = 300;
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
            using (SqlCommand cmd = new SqlCommand("SD_SDDashboardMain1"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Scope", Session["UserScope"].ToString());
                cmd.Parameters.AddWithValue("@TicketStatus", Request.QueryString["chartdata"].ToString());
                cmd.Parameters.AddWithValue("@Option", "SeverityWiseResolvedPieDetails");
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    sda.SelectCommand.CommandTimeout = 300;
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
            using (SqlCommand cmd = new SqlCommand("SD_SDDashboardMain1"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                //     cmd.Parameters.AddWithValue("@ServiceDesk", Request.QueryString["ServiceDesk"].ToString());
                cmd.Parameters.AddWithValue("@Scope", Session["UserScope"].ToString());
                cmd.Parameters.AddWithValue("@TicketStatus", Request.QueryString["chartdata"].ToString());
                cmd.Parameters.AddWithValue("@Option", "ResolutionSLAPieDetails");
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    sda.SelectCommand.CommandTimeout = 300;
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
            using (SqlCommand cmd = new SqlCommand("SD_SDDashboardMain1"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                //      cmd.Parameters.AddWithValue("@ServiceDesk", Request.QueryString["ServiceDesk"].ToString());
                cmd.Parameters.AddWithValue("@Scope", Session["UserScope"].ToString());
                cmd.Parameters.AddWithValue("@TicketStatus", Request.QueryString["chartdata"].ToString());
                cmd.Parameters.AddWithValue("@Option", "SeverityWiseResponseDetails");
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    sda.SelectCommand.CommandTimeout = 300;
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
            using (SqlCommand cmd = new SqlCommand("SD_SDDashboardMain1"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                //        cmd.Parameters.AddWithValue("@ServiceDesk", Request.QueryString["ServiceDesk"].ToString());
                cmd.Parameters.AddWithValue("@Scope", Session["UserScope"].ToString());
                cmd.Parameters.AddWithValue("@TicketStatus", Request.QueryString["chartdata"].ToString());
                cmd.Parameters.AddWithValue("@Option", "SeverityWisePieDetails");
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    sda.SelectCommand.CommandTimeout = 300;
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
            using (SqlCommand cmd = new SqlCommand("SD_SDDashboardMain1"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                //     cmd.Parameters.AddWithValue("@ServiceDesk", Request.QueryString["ServiceDesk"].ToString());
                cmd.Parameters.AddWithValue("@Scope", Session["UserScope"].ToString());
                cmd.Parameters.AddWithValue("@TicketStatus", Request.QueryString["chartdata"].ToString());
                cmd.Parameters.AddWithValue("@Option", "ResponseSLAPieDetails");
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    sda.SelectCommand.CommandTimeout = 300;
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
    private void AllTicketsStatusWise(string proc)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand(proc))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                //     cmd.Parameters.AddWithValue("@ServiceDesk", Request.QueryString["ServiceDesk"].ToString());
                cmd.Parameters.AddWithValue("@Scope", Session["UserScope"].ToString());
                cmd.Parameters.AddWithValue("@TicketStatus", Request.QueryString["chartdata"].ToString());
                cmd.Parameters.AddWithValue("@OrgID", Request.QueryString["OrgId"].ToString());
                cmd.Parameters.AddWithValue("@Option", "OverAllTicketsStatusWise");
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    sda.SelectCommand.CommandTimeout = 300;
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
    private void AllTickets(string proc)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand(proc))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                //     cmd.Parameters.AddWithValue("@ServiceDesk", Request.QueryString["ServiceDesk"].ToString());
                cmd.Parameters.AddWithValue("@Scope", Session["UserScope"].ToString());
                cmd.Parameters.AddWithValue("@OrgID", Request.QueryString["OrgId"].ToString());
                //cmd.Parameters.AddWithValue("@TicketStatus", Request.QueryString["chartdata"].ToString());
                cmd.Parameters.AddWithValue("@Option", "OverAllTickets");
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    sda.SelectCommand.CommandTimeout = 300;
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
    private void OneMonthTicketsStatusWise(string proc)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand(proc))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                //      cmd.Parameters.AddWithValue("@ServiceDesk", Request.QueryString["ServiceDesk"].ToString());
                cmd.Parameters.AddWithValue("@Scope", Session["UserScope"].ToString());
                cmd.Parameters.AddWithValue("@TicketStatus", Request.QueryString["chartdata"].ToString());
                cmd.Parameters.AddWithValue("@OrgID", Request.QueryString["OrgId"].ToString());
                cmd.Parameters.AddWithValue("@Option", "Last30DaysTicketsStatusWise");
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    sda.SelectCommand.CommandTimeout = 300;
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

    private void SevenDaysTicketsStatusWise(string proc)
    {

        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand(proc))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                //      cmd.Parameters.AddWithValue("@ServiceDesk", Request.QueryString["ServiceDesk"].ToString());
                cmd.Parameters.AddWithValue("@Scope", Session["UserScope"].ToString());
                cmd.Parameters.AddWithValue("@TicketStatus", Request.QueryString["chartdata"].ToString());
                cmd.Parameters.AddWithValue("@OrgID", Request.QueryString["OrgId"].ToString());
                cmd.Parameters.AddWithValue("@Option", "Last7DaysTicketsStatusWise");
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    sda.SelectCommand.CommandTimeout = 300;
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

    private void TodayTicketsStatusWise(string proc)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand(proc))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                //     cmd.Parameters.AddWithValue("@ServiceDesk", Request.QueryString["ServiceDesk"].ToString());
                cmd.Parameters.AddWithValue("@Scope", Session["UserScope"].ToString());
                cmd.Parameters.AddWithValue("@TicketStatus", Request.QueryString["chartdata"].ToString());
                cmd.Parameters.AddWithValue("@OrgID", Request.QueryString["OrgId"].ToString());
                cmd.Parameters.AddWithValue("@Option", "TodayTicketsStatusWise");
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    sda.SelectCommand.CommandTimeout = 300;
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

    private void FillTodayTicketsDetails(string proc)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand(proc))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                //     cmd.Parameters.AddWithValue("@ServiceDesk", Request.QueryString["ServiceDesk"].ToString());
                cmd.Parameters.AddWithValue("@Scope", Session["UserScope"].ToString());
                cmd.Parameters.AddWithValue("@OrgID", Request.QueryString["OrgId"].ToString());
                cmd.Parameters.AddWithValue("@Option", "TodayTickets");
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    sda.SelectCommand.CommandTimeout = 300;
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
    private void Fill30DaysTickets(string proc)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand(proc))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                //  cmd.Parameters.AddWithValue("@ServiceDesk", Request.QueryString["ServiceDesk"].ToString());
                cmd.Parameters.AddWithValue("@Scope", Session["UserScope"].ToString());
                cmd.Parameters.AddWithValue("@Option", "ThirtyDaysTickets");
                cmd.Parameters.AddWithValue("@OrgID", Request.QueryString["OrgId"].ToString());
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    sda.SelectCommand.CommandTimeout = 300;
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

    private void FillSevenDaysTicketsDetails(string proc)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand(proc))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                //   cmd.Parameters.AddWithValue("@ServiceDesk", Request.QueryString["ServiceDesk"].ToString());
                cmd.Parameters.AddWithValue("@Scope", Session["UserScope"].ToString());
                cmd.Parameters.AddWithValue("@Option", "SevenDaysTickets");
                cmd.Parameters.AddWithValue("@OrgID", Request.QueryString["OrgId"].ToString());
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    sda.SelectCommand.CommandTimeout = 300;
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


        }
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Verifies that the control is rendered */
    }
}
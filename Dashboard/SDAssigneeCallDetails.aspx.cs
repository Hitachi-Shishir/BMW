using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

public partial class SDAssigneeCallDetails : System.Web.UI.Page
{
    errorMessage msg = new errorMessage();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserID"] == null)
        {
            Response.Redirect("/Default.aspx");
        }
        if (!IsPostBack)
        {
            FillDDLDesk();
        }
        FillNotClosedStatusPie();
        FillOpenClosedStatusPie();
    }
    private void FillDDLDesk()
    {

        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                DataSet ds = new DataSet();
                con.Open();
                using (SqlCommand cmd = new SqlCommand("SD_AllServiceDesks", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Option", "ALL");
                    cmd.Parameters.AddWithValue("@OrgId", Session["SD_OrgID"].ToString());
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.SelectCommand.CommandTimeout = 180;
                        adp.Fill(ds);
                        DropDesks.DataSource = ds;
                        DropDesks.DataTextField = "Desk";
                        DropDesks.DataValueField = "Desk";
                        DropDesks.DataBind();
                        DropDesks.Items.Insert(0, new ListItem("-----Select Service Desk-----", "0"));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //  msg.ReportError(ex.Message);
        }

    }
    protected void ImgBtnExport_Click(object sender, ImageClickEventArgs e)
    {
        if (gvAssigneeCallsStatus.Rows.Count > 0)
        {
            this.gvAssigneeCallsStatus.AllowPaging = false;
            GridViewExportUtil.Export("TicketsDetails.xls", this.gvAssigneeCallsStatus);
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        //FillNotClosedStatusPie();
        FillOpenClosedStatusPie();
        FillAllExclude();
        FillCallAging();
        FillOpenClosed();
        gvCallAgingDetails.DataSource = null;
        gvCallAgingDetails.DataBind();
        lblTotal.Text = "0";
    }

    private void FillOpenClosedStatusPie()
    {
        try
        {
            if (txtFrom.Text == "" || txtTo.Text == "")
            {
                return;
            }
            string fromd = txtFrom.Text;
            DateTime frmdate = DateTime.ParseExact(fromd, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            string formattedFromDate = frmdate.ToString("yyyy-MM-dd");
            string to = txtTo.Text;
            DateTime todate = DateTime.ParseExact(to, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            string formattedToDate = todate.ToString("yyyy-MM-dd");
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SD_ServiceDeskAssigneeCallDashboard", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ServiceDesk", DropDesks.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@FROM", formattedFromDate);
                    cmd.Parameters.AddWithValue("@OrgId", Session["SD_OrgID"].ToString());
                    cmd.Parameters.AddWithValue("@TO", formattedToDate);
                    cmd.Parameters.AddWithValue("@Option", "OpenClosedPie");
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            adp.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                string chartScriptText = GenerateChartAssigne(dt).Replace("HIDDEN_FIELD_ID", hdnfldVariable.ClientID).Replace("BUTTON_ID", btnchartcallstatus.ClientID);
                                chartScript.Text = chartScriptText;
                                ClientScript.RegisterStartupScript(this.GetType(), "renderChart", chartScriptText, false);
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write("Error Occured: " + ex.ToString());
        }
    }
    public static string GenerateChartAssigne(DataTable dt)
    {
        var chartData = new ChartData
        {
            TicketCnt = dt.AsEnumerable().Select(row => row.Field<int>("Ticket Count")).ToArray(),
            Status = dt.AsEnumerable().Select(row => row.Field<string>("Status")).ToArray()
        };
        var serializer = new JavaScriptSerializer();
        string TicketCntJson = serializer.Serialize(chartData.TicketCnt);
        string StatusJson = serializer.Serialize(chartData.Status);

        string chartOptions = $@"
    <script>
    var options = {{
                series: {TicketCntJson},
                chart: {{
                    height: 293,
                    type: 'donut',
                    events: {{
                        dataPointSelection: function(event, chartContext, config) {{
                            var selectedStatus = config.w.config.labels[config.dataPointIndex];
                            document.getElementById('HIDDEN_FIELD_ID').value = selectedStatus;
                            document.getElementById('BUTTON_ID').click();
                        }}
                    }}
                }},
                legend: {{
                    position: 'bottom',
                    show: !1
                }},
                labels: {StatusJson},
                fill: {{
                    type: 'gradient',
                    gradient: {{
                        shade: 'dark',
                        gradientToColors: ['red', 'yellow', 'green', 'orange', '#009efd'],
                        shadeIntensity: 1,
                        type: 'vertical',
                        opacityFrom: 1,
                        opacityTo: 1,
                    }},
                }},
                colors: ['red', 'yellow', 'green', 'orange', '#2af598'],
                dataLabels: {{
                    enabled: !1
                }},
                plotOptions: {{
                    pie: {{
                        donut: {{
                            size: '85%'
                        }}
                    }}
                }},
                tooltip: {{
                    y: {{
                        formatter: function (val) {{
                            return val + '%';
                        }}
                    }}
                }},
                responsive: [{{
                    breakpoint: 480,
                    options: {{
                        chart: {{
                            height: 270
                        }},
                        legend: {{
                            position: 'bottom',
                            show: !1
                        }}
                    }}
                }}]
            }};
    var chart = new ApexCharts(document.querySelector('#LineGraph'), options);
    chart.render();
</script>";
        return chartOptions;
    }
    private void FillOpenClosed()
    {
        try
        {
            string fromd = txtFrom.Text;
            DateTime frmdate = DateTime.ParseExact(fromd, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            string formattedFromDate = frmdate.ToString("yyyy-MM-dd");
            string to = txtTo.Text;
            DateTime todate = DateTime.ParseExact(to, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            string formattedToDate = todate.ToString("yyyy-MM-dd");
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand("SD_ServiceDeskAssigneeCallDashboard", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ServiceDesk", DropDesks.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@FROM", formattedFromDate);
                    cmd.Parameters.AddWithValue("@OrgId", Session["SD_OrgID"].ToString());
                    cmd.Parameters.AddWithValue("@TO", formattedToDate);
                    cmd.Parameters.AddWithValue("@Option", "OpenClosed");
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        using (DataSet ds = new DataSet())
                        {
                            adp.Fill(ds);

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                gvOpenClosed.DataSource = ds;
                                gvOpenClosed.DataBind();
                            }
                            else
                            {
                                gvOpenClosed.DataSource = null;
                                gvOpenClosed.DataBind();
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            
        }
    }

    private void FillNotClosedStatusPie()
    {
        string chartWorkStatOs = "";
        string WorkStatcount = "";
        string WorkStatlabel = "";
        try
        {
            if (txtFrom.Text == "" || txtTo.Text == "")
            {
                return;
            }
            string fromd = txtFrom.Text;
            DateTime frmdate = DateTime.ParseExact(fromd, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            string formattedFromDate = frmdate.ToString("yyyy-MM-dd");
            string to = txtTo.Text;
            DateTime todate = DateTime.ParseExact(to, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            string formattedToDate = todate.ToString("yyyy-MM-dd");

            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand("SD_ServiceDeskAssigneeCallDashboard", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ServiceDesk", DropDesks.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@FROM", formattedFromDate);
                    cmd.Parameters.AddWithValue("@OrgId", Session["SD_OrgID"].ToString());
                    cmd.Parameters.AddWithValue("@TO", formattedToDate);
                    cmd.Parameters.AddWithValue("@Option", "NotOpenClosedPie");
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {

                        adp.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {

                            chartWorkStatOs += "<script>";
                            foreach (DataRow row in dt.Rows)
                            {
                                WorkStatcount += row[0] + ",";
                                WorkStatlabel += "\"" + row[1] + "\",";
                            }
                            WorkStatcount = WorkStatcount.Substring(0, WorkStatcount.Length - 1);
                            WorkStatlabel = WorkStatlabel.Substring(0, WorkStatlabel.Length - 1);
                            chartWorkStatOs += "chartWorkstationlabel = [" + WorkStatlabel + "]; chartWorkstationdata = [" + WorkStatcount + "]";
                            chartWorkStatOs += "</script>";
                            ltrcallstatus.Text = chartWorkStatOs.ToString();
                        }

                    }
                }
            }
        }
        catch (Exception ex)
        {
            
        }
    }

    private void FillCallAging()
    {
        try
        {
            string fromd = txtFrom.Text;
            DateTime frmdate = DateTime.ParseExact(fromd, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            string formattedFromDate = frmdate.ToString("yyyy-MM-dd");
            string to = txtTo.Text;
            DateTime todate = DateTime.ParseExact(to, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            string formattedToDate = todate.ToString("yyyy-MM-dd");
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand("SD_ServiceDeskCallAgingDashboard", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ServiceDesk", DropDesks.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@FROM", formattedFromDate);
                    cmd.Parameters.AddWithValue("@TO", formattedToDate);
                    cmd.Parameters.AddWithValue("@OrgId", Session["SD_OrgID"].ToString());
                    cmd.Parameters.AddWithValue("@Option", "Age");
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            adp.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                gvCallAging.DataSource = dt;
                                gvCallAging.DataBind();
                                //Calculate Sum and display in Footer Row

                                gvCallAging.FooterRow.Cells[0].Text = "Total";
                                gvCallAging.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Right;

                                gvCallAging.FooterRow.Cells[1].Text = (dt.AsEnumerable().Sum(row => row.Field<int?>("0 to 3 Days") ?? 0)).ToString();
                                gvCallAging.FooterRow.Cells[2].Text = (dt.AsEnumerable().Sum(row => row.Field<int?>("4 to 7 Days") ?? 0)).ToString();
                                gvCallAging.FooterRow.Cells[3].Text = (dt.AsEnumerable().Sum(row => row.Field<int?>("8 to 10 Days") ?? 0)).ToString();
                                gvCallAging.FooterRow.Cells[4].Text = (dt.AsEnumerable().Sum(row => row.Field<int?>("More than 10 Days") ?? 0)).ToString();
                            }
                            else
                            {
                                gvCallAging.DataSource = null;
                                gvCallAging.DataBind();
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            
        }
    }

    private void FillAllExclude()
    {
        try
        {
            string fromd = txtFrom.Text;
            DateTime frmdate = DateTime.ParseExact(fromd, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            string formattedFromDate = frmdate.ToString("yyyy-MM-dd");
            string to = txtTo.Text;
            DateTime todate = DateTime.ParseExact(to, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            string formattedToDate = todate.ToString("yyyy-MM-dd");
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand("SD_ServiceDeskAssigneeCallDashboard", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ServiceDesk", DropDesks.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@FROM", formattedFromDate);
                    cmd.Parameters.AddWithValue("@TO", formattedToDate);
                    cmd.Parameters.AddWithValue("@OrgId", Session["SD_OrgID"].ToString());
                    cmd.Parameters.AddWithValue("@Option", "NotOpenClosed");
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        using (DataSet ds = new DataSet())
                        {
                            adp.Fill(ds);

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                lblTotal.Text = "0";
                                lblTotal.Text = ds.Tables[0].Rows.Count.ToString();
                                gvAssigneeCallsStatus.DataSource = ds;
                                gvAssigneeCallsStatus.DataBind();
                            }
                            else
                            {
                                gvAssigneeCallsStatus.DataSource = null;
                                gvAssigneeCallsStatus.DataBind();
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Response.Write("Error Occured: " + ex.ToString());
        }
    }

    protected void gvAssigneeCallsStatus_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (gvAssigneeCallsStatus.Rows.Count > 0)
            {
                int TotalRows = gvAssigneeCallsStatus.Rows.Count;
                int TotalCol = gvAssigneeCallsStatus.Rows[0].Cells.Count;
                gvAssigneeCallsStatus.FooterRow.Cells[0].Text = "Total : ";

                for (int i = 1; i < TotalCol; i++)
                {
                    int sum = 0;
                    for (int j = 0; j < TotalRows; j++)
                    {
                        sum += gvAssigneeCallsStatus.Rows[j].Cells[i].Text != "&nbsp;" ? int.Parse(gvAssigneeCallsStatus.Rows[j].Cells[i].Text) : 0;
                        gvAssigneeCallsStatus.Rows[j].Cells[i].Text = gvAssigneeCallsStatus.Rows[j].Cells[i].Text != "&nbsp;" ? gvAssigneeCallsStatus.Rows[j].Cells[i].Text : "0";
                    }
                    gvAssigneeCallsStatus.FooterRow.Cells[i].Text = sum.ToString("#");

                }
            }

        }

        catch (Exception ex)
        {
            

        }
    }

    protected void gvOpenClosed_RowCommand(object sender, GridViewCommandEventArgs e)
    {


    }

    private static string AssigneeName;
    private static string HeaderName;
    protected void gvCallAging_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName == "VIEW0to3")
        {
            LinkButton lnkView0to3 = (LinkButton)e.CommandSource;
            AssigneeName = lnkView0to3.CommandArgument;
            HeaderName = gvCallAging.HeaderRow.Cells[1].Text;
            FillAgingDetails();
        }
        if (e.CommandName == "VIEW4to7")
        {
            LinkButton lnkView4to7 = (LinkButton)e.CommandSource;
            AssigneeName = lnkView4to7.CommandArgument;
            HeaderName = gvCallAging.HeaderRow.Cells[2].Text;
            FillAgingDetails();
        }
        if (e.CommandName == "VIEW8to10")
        {
            LinkButton lnkView8to10 = (LinkButton)e.CommandSource;
            AssigneeName = lnkView8to10.CommandArgument;
            HeaderName = gvCallAging.HeaderRow.Cells[3].Text;
            FillAgingDetails();
        }
        if (e.CommandName == "VIEWMore10Days")
        {
            LinkButton lnkViewMore10Days = (LinkButton)e.CommandSource;
            AssigneeName = lnkViewMore10Days.CommandArgument;
            HeaderName = gvCallAging.HeaderRow.Cells[4].Text;
            FillAgingDetails();
        }
    }

    private void FillAgingDetails()
    {
        try
        {
            string fromd = txtFrom.Text;
            DateTime frmdate = DateTime.ParseExact(fromd, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            string formattedFromDate = frmdate.ToString("yyyy-MM-dd");
            string to = txtTo.Text;
            DateTime todate = DateTime.ParseExact(to, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            string formattedToDate = todate.ToString("yyyy-MM-dd");
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand("SD_ServiceDeskCallAgingDashboard", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ServiceDesk", DropDesks.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@FROM", formattedFromDate);
                    cmd.Parameters.AddWithValue("@TO", formattedToDate);
                    cmd.Parameters.AddWithValue("@OrgId", Session["SD_OrgID"].ToString());
                    cmd.Parameters.AddWithValue("@Aging", HeaderName);
                    cmd.Parameters.AddWithValue("@Assignee", AssigneeName);
                    cmd.Parameters.AddWithValue("@Option", "AgeDetails");
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            adp.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                lblTotal.Text = "0";
                                lblTotal.Text = dt.Rows.Count.ToString();
                                gvCallAgingDetails.DataSource = dt;
                                gvCallAgingDetails.DataBind();
                                GridFormat(dt);
                            }
                            else
                            {
                                gvCallAgingDetails.DataSource = null;
                                gvCallAgingDetails.DataBind();
                                lblTotal.Text = "0";
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            
        }
    }

    protected void ImgbtnCallAgingExport_Click(object sender, ImageClickEventArgs e)
    {
        if (gvCallAging.Rows.Count > 0)
        {
            this.gvCallAging.AllowPaging = false;
            GridViewExportUtil.Export("TicketsCallAgingDetails.xls", this.gvCallAging);
        }
    }

    protected void gvAssigneeCallsStatus_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void ChartCallsStatus_Click(object sender, ImageMapEventArgs e)
    {
        try
        {
            string fromd = txtFrom.Text;
            DateTime frmdate = DateTime.ParseExact(fromd, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            string formattedFromDate = frmdate.ToString("yyyy-MM-dd");
            string to = txtTo.Text;
            DateTime todate = DateTime.ParseExact(to, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            string formattedToDate = todate.ToString("yyyy-MM-dd");
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                con.Open();
                string cmdstr = "SD_ServiceDeskAssigneeCallDashboard";
                using (SqlCommand cmd = new SqlCommand(cmdstr, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.SelectCommand.CommandTimeout = 180;
                        cmd.Parameters.AddWithValue("@ServiceDesk", DropDesks.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@Status", e.PostBackValue);
                        cmd.Parameters.AddWithValue("@FROM", formattedFromDate);
                        cmd.Parameters.AddWithValue("@TO", formattedToDate);
                        cmd.Parameters.AddWithValue("@Option", "TicketDetails");
                        adp.Fill(ds);
                        lblTotal.Text = "0";
                        lblTotal.Text = ds.Tables[0].Rows.Count.ToString();
                        gvCallAgingDetails.DataSource = ds;
                        gvCallAgingDetails.DataBind();
                        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0] != null)
                        {
                            GridFormat(ds.Tables[0]);
                        }
                    }
                }
            }
        }

        catch (Exception ex)
        {
            msg.ReportError(ex.Message);

        }
    }

    protected void ChartCallsStatus_Load(object sender, EventArgs e)
    {

    }
    protected void btnchartcallstatus_Click(object sender, EventArgs e)
    {
        try
        {
            string fromd = txtFrom.Text;
            DateTime frmdate = DateTime.ParseExact(fromd, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            string formattedFromDate = frmdate.ToString("yyyy-MM-dd");
            string to = txtTo.Text;
            DateTime todate = DateTime.ParseExact(to, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            string formattedToDate = todate.ToString("yyyy-MM-dd");
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                con.Open();
                string cmdstr = "SD_ServiceDeskAssigneeCallDashboard";
                using (SqlCommand cmd = new SqlCommand(cmdstr, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.SelectCommand.CommandTimeout = 180;
                        cmd.Parameters.AddWithValue("@ServiceDesk", DropDesks.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@Status", hdnfldVariable.Value);
                        cmd.Parameters.AddWithValue("@FROM", formattedFromDate);
                        cmd.Parameters.AddWithValue("@OrgId", Session["SD_OrgID"].ToString());
                        cmd.Parameters.AddWithValue("@TO", formattedToDate);
                        cmd.Parameters.AddWithValue("@Option", "TicketDetails");
                        adp.Fill(ds);
                        lblTotal.Text = "0";
                        lblTotal.Text = ds.Tables[0].Rows.Count.ToString();
                        gvCallAgingDetails.DataSource = ds;
                        gvCallAgingDetails.DataBind();
                        if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0] != null)
                        {
                            GridFormat(ds.Tables[0]);
                        }
                    }
                }
            }
        }

        catch (Exception ex)
        {
            msg.ReportError(ex.Message);

        }
    }
    protected void GridFormat(DataTable dt)
    {
        try
        {
            gvCallAgingDetails.UseAccessibleHeader = true;
            gvCallAgingDetails.HeaderRow.TableSection = TableRowSection.TableHeader;

            if (gvCallAgingDetails.TopPagerRow != null)
            {
                gvCallAgingDetails.TopPagerRow.TableSection = TableRowSection.TableHeader;
            }
            if (gvCallAgingDetails.BottomPagerRow != null)
            {
                gvCallAgingDetails.BottomPagerRow.TableSection = TableRowSection.TableFooter;
            }
            if (dt.Rows.Count > 0)
                gvCallAgingDetails.FooterRow.TableSection = TableRowSection.TableFooter;
        }
        catch
        {

        }
    }
}
public class ChartData
{
    public int[] TicketCnt { get; set; }
    public string[] Status { get; set; }
}
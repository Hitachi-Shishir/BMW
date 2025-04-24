using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class frmLiveDashboard : System.Web.UI.Page
{
    errorMessage msg = new errorMessage();
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["LoginName"] != null && Session["UserScope"] != null)
        {
            if (!IsPostBack)
            {
                FillReponseSLAPie();
                FillResolutionSLAPie();
                FillSeverityWiseResponseSLAPie();
                FillSeverityWiseResolutionSLAPie();
                FillAllSeverityWiseCalls();
                FillAllSeverityWiseResolveCalls();

                FillNotClosedStatusPie();
                FillOpenClosedStatusPie();

                FillTotCount();
                FillAllExclude();
                FillCallAging();
                FillOpenClosed();


                FillNotClosedStatusPieDept();
                FillOpenClosedStatusPieDept();
                FillAllExcludeDept();
                FillCallAgingDept();
                FillOpenClosedDept();
            }
        }
        else
        {
            Response.Redirect("/Default.aspx");
        }
    }
    private void FillTotCount()
    {
        try
        {

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_LiveDashboard"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrgId", Session["SD_OrgID"].ToString());
                    cmd.Parameters.AddWithValue("@Option", "Totcount");
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        cmd.CommandTimeout = 950;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                lblTotalTickets.Text = Convert.ToString(dt.Rows[0]["Total"]);
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
    #region Start SLA
    private void FillSeverityWiseResponseSLAPie()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_LiveDashboard"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrgId", Session["SD_OrgID"].ToString());
                    cmd.Parameters.AddWithValue("@Option", "SeverityWiseResponse");
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        cmd.CommandTimeout = 950;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                string chartScriptText = GenerateChartSeverityWiseResponseSLAPie(dt).Replace("HIDDEN_FIELD_ID", hdnfldVariable.ClientID)
       .Replace("BUTTON_ID", btnChartSeverity.ClientID);
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "renderChart3", chartScriptText, true);
                            }
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {

        }
    }
    public static string GenerateChartSeverityWiseResponseSLAPie(DataTable dt)
    {
        var chartData = new ChartDataSLA
        {
            SeverityWiseResponse = dt.AsEnumerable().Select(row => row.Field<string>("Severity")).ToArray(),
            SeverityWiseResponseTotalTickets = dt.AsEnumerable().Select(row => row.Field<int>("TotalTickets")).ToArray()
        };

        var serializer = new JavaScriptSerializer();
        string ResolutionSLAJson = serializer.Serialize(chartData.SeverityWiseResponse);
        string totalTicketsJson = serializer.Serialize(chartData.SeverityWiseResponseTotalTickets);

        string chartOptions = $@"
    $(function() {{
        var options = {{
            series: [{{
                name: 'Total Tickets',
                data: {totalTicketsJson}
            }}],
            chart: {{
                foreColor: '#9ba7b2',
                height: 250,
                type: 'bar',
                events: {{
                    dataPointSelection: function(event, chartContext, config) {{
                        var selectedStatus = config.w.config.xaxis.categories[config.dataPointIndex];
                        document.getElementById('HIDDEN_FIELD_ID').value = selectedStatus;
                        document.getElementById('BUTTON_ID').click();
                    }}
                }},
                zoom: {{
                    enabled: false
                }},
                toolbar: {{
                    show: false
                }}
            }},
            fill: {{
                type: 'gradient',
                gradient: {{
                    shade: 'dark',
                    gradientToColors: ['#009efd'],
                    shadeIntensity: 1,
                    type: 'vertical',
                    stops: [0, 100, 100, 100]
                }}
            }},
            colors: ['#2af598'],
            plotOptions: {{
                bar: {{
                    horizontal: false,
                    borderRadius: 4,
                    columnWidth: '35%'
                }}
            }},
            dataLabels: {{
                enabled: false
            }},
            stroke: {{
                show: true,
                width: 4,
                colors: ['transparent']
            }},
            grid: {{
                show: true,
                borderColor: 'rgba(0, 0, 0, 0.15)',
                strokeDashArray: 4
            }},
            tooltip: {{
                theme: 'dark'
            }},
            xaxis: {{
                categories: {ResolutionSLAJson},
                labels: {{
                    show: true,
                    style: {{
                        colors: '#9ba7b2',
                        fontSize: '12px'
                    }}
                }}
            }}
        }};

        var chart = new ApexCharts(document.querySelector('#chart3'), options);
        chart.render();
    }});
";

        return chartOptions;
    }
    private void FillSeverityWiseResolutionSLAPie()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_LiveDashboard"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrgId", Session["SD_OrgID"].ToString());
                    cmd.Parameters.AddWithValue("@Option", "SeverityWiseResolution");
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        cmd.CommandTimeout = 950;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                string chartScriptText = GenerateChartSeverityWiseResolutionSLAPie(dt).Replace("HIDDEN_FIELD_ID", hdnfldVariable.ClientID)
                                       .Replace("BUTTON_ID", btnChartSeverity.ClientID);
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "renderChart3", chartScriptText, true);
                            }
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {

        }
    }
    public static string GenerateChartSeverityWiseResolutionSLAPie(DataTable dt)
    {
        var chartData = new ChartDataSLA
        {
            SeverityWiseResolutionSLA = dt.AsEnumerable().Select(row => row.Field<string>("Severity")).ToArray(),
            SeverityWiseResolutionSLATotalTickets = dt.AsEnumerable().Select(row => row.Field<int>("TotalTickets")).ToArray()
        };

        var serializer = new JavaScriptSerializer();
        string ResolutionSLAJson = serializer.Serialize(chartData.SeverityWiseResolutionSLA);
        string totalTicketsJson = serializer.Serialize(chartData.SeverityWiseResolutionSLATotalTickets);

        string chartOptions = $@"
    $(function() {{
        var options = {{
            series: [{{
                name: 'Total Tickets',
                data: {totalTicketsJson}
            }}],
            chart: {{
                foreColor: '#9ba7b2',
                height: 250,
                type: 'bar',
                events: {{
                    dataPointSelection: function(event, chartContext, config) {{
                        var selectedStatus = config.w.config.xaxis.categories[config.dataPointIndex];
                        document.getElementById('HIDDEN_FIELD_ID').value = selectedStatus;
                        document.getElementById('BUTTON_ID').click();
                    }}
                }},
                zoom: {{
                    enabled: false
                }},
                toolbar: {{
                    show: false
                }}
            }},
            fill: {{
                type: 'gradient',
                gradient: {{
                    shade: 'dark',
                    gradientToColors: ['#009efd'],
                    shadeIntensity: 1,
                    type: 'vertical',
                    stops: [0, 100, 100, 100]
                }}
            }},
            colors: ['#2af598'],
            plotOptions: {{
                bar: {{
                    horizontal: false,
                    borderRadius: 4,
                    columnWidth: '35%'
                }}
            }},
            dataLabels: {{
                enabled: false
            }},
            stroke: {{
                show: true,
                width: 4,
                colors: ['transparent']
            }},
            grid: {{
                show: true,
                borderColor: 'rgba(0, 0, 0, 0.15)',
                strokeDashArray: 4
            }},
            tooltip: {{
                theme: 'dark'
            }},
            xaxis: {{
                categories: {ResolutionSLAJson},
                labels: {{
                    show: true,
                    style: {{
                        colors: '#9ba7b2',
                        fontSize: '12px'
                    }}
                }}
            }}
        }};

        var chart = new ApexCharts(document.querySelector('#chart3'), options);
        chart.render();
    }});
";

        return chartOptions;
    }
    private void FillResolutionSLAPie()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_LiveDashboard"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrgId", Session["SD_OrgID"].ToString());
                    cmd.Parameters.AddWithValue("@Option", "ResolutionSLAPie");
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        cmd.CommandTimeout = 950;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                string chartScriptText = GenerateChartResolutionSLAPie(dt).Replace("HIDDEN_FIELD_ID", hdnfldVariable.ClientID)
       .Replace("BUTTON_ID", btnChartResolutionSLA.ClientID);
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "renderChart4", chartScriptText, true);
                            }

                        }
                    }
                }
            }
        }
        catch (Exception e)
        {

        }
    }
    public static string GenerateChartResolutionSLAPie(DataTable dt)
    {
        var chartData = new ChartDataSLA
        {
            ResolutionSLA = dt.AsEnumerable().Select(row => row.Field<string>("CustomFieldValue")).ToArray(),
            ResolutionSLATotalTickets = dt.AsEnumerable().Select(row => row.Field<int>("TotalTickets")).ToArray()
        };

        var serializer = new JavaScriptSerializer();
        string ResolutionSLAJson = serializer.Serialize(chartData.ResolutionSLA);
        string totalTicketsJson = serializer.Serialize(chartData.ResolutionSLATotalTickets);

        string chartOptions = $@"
    $(function() {{
        var options = {{
            series: [{{
                name: 'Total Tickets',
                data: {totalTicketsJson}
            }}],
            chart: {{
                foreColor: '#9ba7b2',
                height: 250,
                type: 'bar',
                events: {{
                    dataPointSelection: function(event, chartContext, config) {{
                        var selectedStatus = config.w.config.xaxis.categories[config.dataPointIndex];
                        document.getElementById('HIDDEN_FIELD_ID').value = selectedStatus;
                        document.getElementById('BUTTON_ID').click();
                    }}
                }},
                zoom: {{
                    enabled: false
                }},
                toolbar: {{
                    show: false
                }}
            }},
            fill: {{
                type: 'gradient',
                gradient: {{
                    shade: 'dark',
                    gradientToColors: ['#009efd'],
                    shadeIntensity: 1,
                    type: 'vertical',
                    stops: [0, 100, 100, 100]
                }}
            }},
            colors: ['#2af598'],
            plotOptions: {{
                bar: {{
                    horizontal: false,
                    borderRadius: 4,
                    columnWidth: '35%'
                }}
            }},
            dataLabels: {{
                enabled: false
            }},
            stroke: {{
                show: true,
                width: 4,
                colors: ['transparent']
            }},
            grid: {{
                show: true,
                borderColor: 'rgba(0, 0, 0, 0.15)',
                strokeDashArray: 4
            }},
            tooltip: {{
                theme: 'dark'
            }},
            xaxis: {{
                categories: {ResolutionSLAJson},
                labels: {{
                    show: true,
                    style: {{
                        colors: '#9ba7b2',
                        fontSize: '12px'
                    }}
                }}
            }}
        }};

        var chart = new ApexCharts(document.querySelector('#chart4'), options);
        chart.render();
    }});
";

        return chartOptions;
    }
    private void FillReponseSLAPie()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_LiveDashboard"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrgId", Session["SD_OrgID"].ToString());
                    cmd.Parameters.AddWithValue("@Option", "ResponseSLAPie");
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        cmd.CommandTimeout = 950;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {

                                string chartScriptText = GenerateChartReponseSLAPie(dt).Replace("HIDDEN_FIELD_ID", hdnfldVariable.ClientID)
       .Replace("BUTTON_ID", btnChartResponseSLA.ClientID);
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "renderChart1", chartScriptText, true);
                            }
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {

        }
    }
    public static string GenerateChartReponseSLAPie(DataTable dt)
    {
        var chartData = new ChartDataSLA
        {
            ReponseSLA = dt.AsEnumerable().Select(row => row.Field<string>("CustomFieldValue")).ToArray(),
            ReponseSLATotalTickets = dt.AsEnumerable().Select(row => row.Field<int>("TotalTickets")).ToArray()
        };

        var serializer = new JavaScriptSerializer();
        string ResolutionSLAJson = serializer.Serialize(chartData.ReponseSLA);
        string totalTicketsJson = serializer.Serialize(chartData.ReponseSLATotalTickets);

        string chartOptions = $@"
    $(function() {{
        var options = {{
            series: [{{
                name: 'Total Tickets',
                data: {totalTicketsJson}
            }}],
            chart: {{
                foreColor: '#9ba7b2',
                height: 250,
                type: 'bar',
                events: {{
                    dataPointSelection: function(event, chartContext, config) {{
                        var selectedStatus = config.w.config.xaxis.categories[config.dataPointIndex];
                        document.getElementById('HIDDEN_FIELD_ID').value = selectedStatus;
                        document.getElementById('BUTTON_ID').click();
                    }}
                }},
                zoom: {{
                    enabled: false
                }},
                toolbar: {{
                    show: false
                }}
            }},
            fill: {{
                type: 'gradient',
                gradient: {{
                    shade: 'dark',
                    gradientToColors: ['#009efd'],
                    shadeIntensity: 1,
                    type: 'vertical',
                    stops: [0, 100, 100, 100]
                }}
            }},
            colors: ['#2af598'],
            plotOptions: {{
                bar: {{
                    horizontal: false,
                    borderRadius: 4,
                    columnWidth: '35%'
                }}
            }},
            dataLabels: {{
                enabled: false
            }},
            stroke: {{
                show: true,
                width: 4,
                colors: ['transparent']
            }},
            grid: {{
                show: true,
                borderColor: 'rgba(0, 0, 0, 0.15)',
                strokeDashArray: 4
            }},
            tooltip: {{
                theme: 'dark'
            }},
            xaxis: {{
                categories: {ResolutionSLAJson},
                labels: {{
                    show: true,
                    style: {{
                        colors: '#9ba7b2',
                        fontSize: '12px'
                    }}
                }}
            }}
        }};

        var chart = new ApexCharts(document.querySelector('#chart1'), options);
        chart.render();
    }});
";

        return chartOptions;
    }
    public void data()
    {
        FillReponseSLAPie();
        FillResolutionSLAPie();
        FillSeverityWiseResponseSLAPie();
        FillSeverityWiseResolutionSLAPie();
        FillAllSeverityWiseCalls();
        FillAllSeverityWiseResolveCalls();

        FillNotClosedStatusPie();
        FillOpenClosedStatusPie();

        FillTotCount();
        FillAllExclude();
        FillCallAging();
        FillOpenClosed();


        FillNotClosedStatusPieDept();
        FillOpenClosedStatusPieDept();
        FillAllExcludeDept();
        FillCallAgingDept();
        FillOpenClosedDept();
    }
    private void FillAllSeverityWiseCalls()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_LiveDashboard"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrgId", Session["SD_OrgID"].ToString());
                    cmd.Parameters.AddWithValue("@Option", "SeverityWisePie");
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        cmd.CommandTimeout = 950;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                string chartScriptText = GenerateChartSeverityWiseCalls(dt).Replace("HIDDEN_FIELD_ID", hdnfldVariable.ClientID)
                                      .Replace("BUTTON_ID", btnChartServerityWiseTotal.ClientID);
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "renderChart2", chartScriptText, true);
                            }
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {

        }

    }
    public static string GenerateChartSeverityWiseCalls(DataTable dt)
    {
        var chartData = new ChartDataSLA
        {
            SeverityWiseCalls = dt.AsEnumerable().Select(row => row.Field<string>("Severity")).ToArray(),
            SeverityWiseCallsTotalTickets = dt.AsEnumerable().Select(row => row.Field<int>("TotalTickets")).ToArray()
        };

        var serializer = new JavaScriptSerializer();
        string ResolutionSLAJson = serializer.Serialize(chartData.SeverityWiseCalls);
        string totalTicketsJson = serializer.Serialize(chartData.SeverityWiseCallsTotalTickets);

        string chartOptions = $@"
    $(function() {{
        var options = {{
            series: [{{
                name: 'Total Tickets',
                data: {totalTicketsJson}
            }}],
            chart: {{
                foreColor: '#9ba7b2',
                height: 250,
                type: 'bar',
                events: {{
                    dataPointSelection: function(event, chartContext, config) {{
                        var selectedStatus = config.w.config.xaxis.categories[config.dataPointIndex];
                        document.getElementById('HIDDEN_FIELD_ID').value = selectedStatus;
                        document.getElementById('BUTTON_ID').click();
                    }}
                }},
                zoom: {{
                    enabled: false
                }},
                toolbar: {{
                    show: false
                }}
            }},
            fill: {{
                type: 'gradient',
                gradient: {{
                    shade: 'dark',
                    gradientToColors: ['#009efd'],
                    shadeIntensity: 1,
                    type: 'vertical',
                    stops: [0, 100, 100, 100]
                }}
            }},
            colors: ['#2af598'],
            plotOptions: {{
                bar: {{
                    horizontal: false,
                    borderRadius: 4,
                    columnWidth: '35%'
                }}
            }},
            dataLabels: {{
                enabled: false
            }},
            stroke: {{
                show: true,
                width: 4,
                colors: ['transparent']
            }},
            grid: {{
                show: true,
                borderColor: 'rgba(0, 0, 0, 0.15)',
                strokeDashArray: 4
            }},
            tooltip: {{
                theme: 'dark'
            }},
            xaxis: {{
                categories: {ResolutionSLAJson},
                labels: {{
                    show: true,
                    style: {{
                        colors: '#9ba7b2',
                        fontSize: '12px'
                    }}
                }}
            }}
        }};

        var chart = new ApexCharts(document.querySelector('#chart2'), options);
        chart.render();
    }});
";

        return chartOptions;
    }
    private void FillAllSeverityWiseResolveCalls()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_LiveDashboard"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrgId", Session["SD_OrgID"].ToString());
                    cmd.Parameters.AddWithValue("@Option", "SeverityWiseResolvedPie");
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        cmd.CommandTimeout = 950;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                string chartScriptText = GenerateChartAllSeverityWiseResolveCallPie(dt).Replace("HIDDEN_FIELD_ID", hdnfldVariable.ClientID)
       .Replace("BUTTON_ID", btnChartSeverityWiseResolution.ClientID);
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "renderChart5", chartScriptText, true);
                            }

                        }
                    }
                }
            }
        }
        catch (Exception e)
        {

        }

    }
    public static string GenerateChartAllSeverityWiseResolveCallPie(DataTable dt)
    {
        var chartData = new ChartDataSLA
        {
            AllSeverityWiseResolve = dt.AsEnumerable().Select(row => row.Field<string>("Severity")).ToArray(),
            AllSeverityWiseResolveTotalTickets = dt.AsEnumerable().Select(row => row.Field<int>("TotalTickets")).ToArray()
        };

        var serializer = new JavaScriptSerializer();
        string ResolutionSLAJson = serializer.Serialize(chartData.AllSeverityWiseResolve);
        string totalTicketsJson = serializer.Serialize(chartData.AllSeverityWiseResolveTotalTickets);

        string chartOptions = $@"
    $(function() {{
        var options = {{
            series: [{{
                name: 'Total Tickets',
                data: {totalTicketsJson}
            }}],
            chart: {{
                foreColor: '#9ba7b2',
                height: 250,
                type: 'bar',
                events: {{
                    dataPointSelection: function(event, chartContext, config) {{
                        var selectedStatus = config.w.config.xaxis.categories[config.dataPointIndex];
                        document.getElementById('HIDDEN_FIELD_ID').value = selectedStatus;
                        document.getElementById('BUTTON_ID').click();
                    }}
                }},
                zoom: {{
                    enabled: false
                }},
                toolbar: {{
                    show: false
                }}
            }},
            fill: {{
                type: 'gradient',
                gradient: {{
                    shade: 'dark',
                    gradientToColors: ['#009efd'],
                    shadeIntensity: 1,
                    type: 'vertical',
                    stops: [0, 100, 100, 100]
                }}
            }},
            colors: ['#2af598'],
            plotOptions: {{
                bar: {{
                    horizontal: false,
                    borderRadius: 4,
                    columnWidth: '35%'
                }}
            }},
            dataLabels: {{
                enabled: false
            }},
            stroke: {{
                show: true,
                width: 4,
                colors: ['transparent']
            }},
            grid: {{
                show: true,
                borderColor: 'rgba(0, 0, 0, 0.15)',
                strokeDashArray: 4
            }},
            tooltip: {{
                theme: 'dark'
            }},
            xaxis: {{
                categories: {ResolutionSLAJson},
                labels: {{
                    show: true,
                    style: {{
                        colors: '#9ba7b2',
                        fontSize: '12px'
                    }}
                }}
            }}
        }};

        var chart = new ApexCharts(document.querySelector('#chart5'), options);
        chart.render();
    }});
";

        return chartOptions;
    }
    protected void btnChartResponseSLA_Click(object sender, EventArgs e)
    {
        data();
        pnlGrd.Visible = true;
        divGrd.Visible = true;
        ResponseSlaDetails(hdnfldVariable.Value);
        FillReponseSLAPie();
    }
    private void ResponseSlaDetails(string TicketStatus)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand("SP_LiveDashboardFilter"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TicketStatus", TicketStatus);
                cmd.Parameters.AddWithValue("@OrgID", Session["SD_OrgID"].ToString());
                cmd.Parameters.AddWithValue("@Option", "ResponseSLAPieDetails");
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
    protected void btnChartServerityWiseTotal_Click(object sender, EventArgs e)
    {
        data();
        pnlGrd.Visible = true;
        divGrd.Visible = true;
        SeverityWisePieDetails(hdnfldVariable.Value);
    }
    private void SeverityWisePieDetails(string TicketStatus)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand("SP_LiveDashboardFilter"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TicketStatus", TicketStatus);
                cmd.Parameters.AddWithValue("@OrgID", Session["SD_OrgID"].ToString());
                cmd.Parameters.AddWithValue("@Option", "SeverityWisePieDetails");
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataSet ds = new DataSet())
                    {
                        sda.Fill(ds);
                        lblTotalSLA.Text = ds.Tables[0].Rows.Count.ToString();
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
    protected void btnChartResolutionSLA_Click(object sender, EventArgs e)
    {
        data();
        pnlGrd.Visible = true;
        divGrd.Visible = true;
        ResolutionSlaDetails(hdnfldVariable.Value);
    }
    private void ResolutionSlaDetails(string TicketStatus)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand("SP_LiveDashboardFilter"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TicketStatus", TicketStatus);
                cmd.Parameters.AddWithValue("@OrgID", Session["SD_OrgID"].ToString());
                cmd.Parameters.AddWithValue("@Option", "ResolutionSLAPieDetails");
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataSet ds = new DataSet())
                    {
                        sda.Fill(ds);
                        lblTotalSLA.Text = ds.Tables[0].Rows.Count.ToString();
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
    protected void btnChartSeverity_Click(object sender, EventArgs e)
    {
        data();
        pnlGrd.Visible = true;
        divGrd.Visible = true;
        SeverityWiseResponseDetails(hdnfldVariable.Value);
    }
    private void SeverityWiseResponseDetails(string SeverityWiseResponseDetails)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand("SP_LiveDashboardFilter"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TicketStatus", SeverityWiseResponseDetails);
                cmd.Parameters.AddWithValue("@OrgID", Session["SD_OrgID"].ToString());
                cmd.Parameters.AddWithValue("@Option", "SeverityWiseResponseDetails");
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataSet ds = new DataSet())
                    {
                        sda.Fill(ds);
                        lblTotalSLA.Text = ds.Tables[0].Rows.Count.ToString();
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
    protected void btnChartSeverityWiseResolCalls_Click(object sender, EventArgs e)
    {
        data();
        pnlGrd.Visible = true;
        divGrd.Visible = true;
        AllSeverityWiseResolveCalls(hdnfldVariable.Value);
    }
    private void AllSeverityWiseResolveCalls(string TicketStatus)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand("SP_LiveDashboardFilter"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TicketStatus", TicketStatus);
                cmd.Parameters.AddWithValue("@OrgID", Session["SD_OrgID"].ToString());
                cmd.Parameters.AddWithValue("@Option", "SeverityWiseResolvedPieDetails");
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataSet ds = new DataSet())
                    {
                        sda.Fill(ds);
                        lblTotalSLA.Text = ds.Tables[0].Rows.Count.ToString();
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
    protected void btnChartSeverityWiseResolution_Click(object sender, EventArgs e)
    {
        data();
        pnlGrd.Visible = true;
        divGrd.Visible = true;
        SeverityWiseResolution(hdnfldVariable.Value);
    }
    private void SeverityWiseResolution(string TicketStatus)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand("SP_LiveDashboardFilter"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TicketStatus", TicketStatus);
                cmd.Parameters.AddWithValue("@OrgID", Session["SD_OrgID"].ToString());
                cmd.Parameters.AddWithValue("@Option", "SeverityWiseResolutionDetails");
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataSet ds = new DataSet())
                    {
                        sda.Fill(ds);
                        lblTotalSLA.Text = ds.Tables[0].Rows.Count.ToString();
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
    #endregion END SLA
    #region Start AssigneWise
    protected void ImgBtnExport_Click(object sender, EventArgs e)
    {
        if (gvAssigneeCallsStatus.Rows.Count > 0)
        {
            this.gvAssigneeCallsStatus.AllowPaging = false;
            GridViewExportUtil.Export("TicketsDetails.xls", this.gvAssigneeCallsStatus);
        }
    }
    private void FillOpenClosedStatusPie()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_LiveDashboard", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrgId", Session["SD_OrgID"].ToString());
                    cmd.Parameters.AddWithValue("@Option", "OpenClosedPie");
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            adp.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                //Uncomment
                                //Chart1.DataSource = dt;
                                //Chart1.Series[0].XValueMember = "Status";
                                //Chart1.Series[0].YValueMembers = "Ticket Count";
                                //Chart1.DataBind();
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
    private void FillOpenClosed()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand("SP_LiveDashboard", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrgId", Session["SD_OrgID"].ToString());
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
            Response.Write("Error Occured: " + ex.ToString());
        }
    }
    private void FillNotClosedStatusPie()
    {
        try
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand("SP_LiveDashboard", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrgId", Session["SD_OrgID"].ToString());
                    cmd.Parameters.AddWithValue("@Option", "NotOpenClosedPie");
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {

                        adp.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            string chartScriptText = GenerateChartlNotClosedStatusPie(dt).Replace("HIDDEN_FIELD_ID", hdnfldVariable.ClientID)
       .Replace("BUTTON_ID", btnChartResponseSLA.ClientID);
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "renderChart1", chartScriptText, true);
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
    public static string GenerateChartlNotClosedStatusPie(DataTable dt)
    {
        var chartData = new ChartDataSLA
        {
            AllSeverityWiseResolve = dt.AsEnumerable().Select(row => row.Field<string>("Status")).ToArray(),
            AllSeverityWiseResolveTotalTickets = dt.AsEnumerable().Select(row => row.Field<int>("Ticket Count")).ToArray()
        };

        var serializer = new JavaScriptSerializer();
        string ResolutionSLAJson = serializer.Serialize(chartData.AllSeverityWiseResolve);
        string totalTicketsJson = serializer.Serialize(chartData.AllSeverityWiseResolveTotalTickets);

        string chartOptions = $@"
    $(function() {{
        var options = {{
            series: [{{
                name: 'Total Tickets',
                data: {totalTicketsJson}
            }}],
            chart: {{
                foreColor: '#9ba7b2',
                height: 250,
                type: 'bar',
                events: {{
                    dataPointSelection: function(event, chartContext, config) {{
                        var selectedStatus = config.w.config.xaxis.categories[config.dataPointIndex];
                        document.getElementById('HIDDEN_FIELD_ID').value = selectedStatus;
                        document.getElementById('BUTTON_ID').click();
                    }}
                }},
                zoom: {{
                    enabled: false
                }},
                toolbar: {{
                    show: false
                }}
            }},
            fill: {{
                type: 'gradient',
                gradient: {{
                    shade: 'dark',
                    gradientToColors: ['#009efd'],
                    shadeIntensity: 1,
                    type: 'vertical',
                    stops: [0, 100, 100, 100]
                }}
            }},
            colors: ['#2af598'],
            plotOptions: {{
                bar: {{
                    horizontal: false,
                    borderRadius: 4,
                    columnWidth: '35%'
                }}
            }},
            dataLabels: {{
                enabled: false
            }},
            stroke: {{
                show: true,
                width: 4,
                colors: ['transparent']
            }},
            grid: {{
                show: true,
                borderColor: 'rgba(0, 0, 0, 0.15)',
                strokeDashArray: 4
            }},
            tooltip: {{
                theme: 'dark'
            }},
            xaxis: {{
                categories: {ResolutionSLAJson},
                labels: {{
                    show: true,
                    style: {{
                        colors: '#9ba7b2',
                        fontSize: '12px'
                    }}
                }}
            }}
        }};

        var chart = new ApexCharts(document.querySelector('#chart1'), options);
        chart.render();
    }});
";

        return chartOptions;
    }
    private void FillCallAging()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand("SP_LiveDashboard", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
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
            Response.Write("Error Occured: " + ex.ToString());
        }
    }
    private void FillAllExclude()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand("SP_LiveDashboard", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrgId", Session["SD_OrgID"].ToString());
                    cmd.Parameters.AddWithValue("@Option", "NotOpenClosed");
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        using (DataSet ds = new DataSet())
                        {
                            adp.Fill(ds);

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                lblTotalSLA.Text = "0";
                                lblTotalSLA.Text = ds.Tables[0].Rows.Count.ToString();
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
            msg.ReportError(ex.Message);

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
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand("SP_LiveDashboard", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
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
                                lblTotalSLA.Text = "0";
                                lblTotalSLA.Text = dt.Rows.Count.ToString();
                                gvCallAgingDetails.DataSource = dt;
                                gvCallAgingDetails.DataBind();
                            }
                            else
                            {
                                gvCallAgingDetails.DataSource = null;
                                gvCallAgingDetails.DataBind();
                                lblTotalSLA.Text = "0";
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
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                con.Open();
                string cmdstr = "SP_LiveDashboard";
                using (SqlCommand cmd = new SqlCommand(cmdstr, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.SelectCommand.CommandTimeout = 180;
                        cmd.Parameters.AddWithValue("@Status", e.PostBackValue);
                        cmd.Parameters.AddWithValue("@OrgId", Session["SD_OrgID"].ToString());
                        cmd.Parameters.AddWithValue("@Option", "TicketDetails");
                        adp.Fill(ds);
                        lblTotalSLA.Text = "0";
                        lblTotalSLA.Text = ds.Tables[0].Rows.Count.ToString();
                        gvCallAgingDetails.DataSource = ds.Tables[0];
                        gvCallAgingDetails.DataBind();
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
    protected void gridviewcategory_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (gridviewcategory.Rows.Count > 0)
            {
                int TotalRows = gridviewcategory.Rows.Count;
                int TotalCol = gridviewcategory.Rows[0].Cells.Count;
                gridviewcategory.FooterRow.Cells[0].Text = "Total : ";

                for (int i = 1; i < TotalCol; i++)
                {
                    int sum = 0;
                    for (int j = 0; j < TotalRows; j++)
                    {
                        sum += gridviewcategory.Rows[j].Cells[i].Text != "&nbsp;" ? int.Parse(gridviewcategory.Rows[j].Cells[i].Text) : 0;
                        gridviewcategory.Rows[j].Cells[i].Text = gridviewcategory.Rows[j].Cells[i].Text != "&nbsp;" ? gridviewcategory.Rows[j].Cells[i].Text : "0";
                    }
                    gridviewcategory.FooterRow.Cells[i].Text = sum.ToString("#");

                }
            }

        }

        catch (Exception ex)
        {
            msg.ReportError(ex.Message);

        }
    }
    protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
    {
        if (gridviewcategory.Rows.Count > 0)
        {
            this.gridviewcategory.AllowPaging = false;
            GridViewExportUtil.Export("TicketsDetails.xls", this.gridviewcategory);
        }
    }
    protected void btnchartcallstatus_Click(object sender, EventArgs e)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                con.Open();
                string cmdstr = "SP_LiveDashboard";
                using (SqlCommand cmd = new SqlCommand(cmdstr, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.SelectCommand.CommandTimeout = 180;
                        cmd.Parameters.AddWithValue("@Status", hdnfldVariable.Value);
                        cmd.Parameters.AddWithValue("@OrgId", Session["SD_OrgID"].ToString());
                        cmd.Parameters.AddWithValue("@Option", "TicketDetailsDept");
                        adp.Fill(ds);
                        lblTotalSLA.Text = "0";
                        lblTotalSLA.Text = ds.Tables[0].Rows.Count.ToString();
                        gvCallAgingDetails.DataSource = ds;
                        gvCallAgingDetails.DataBind();
                    }
                }
            }
        }

        catch (Exception ex)
        {
            msg.ReportError(ex.Message);

        }
    }

    #endregion End Assignewise
    #region Start DeptWise
    protected void ImgBtnExportDept_Click(object sender, EventArgs e)
    {
        if (gvAssigneeCallsStatus.Rows.Count > 0)
        {
            this.gvAssigneeCallsStatus.AllowPaging = false;
            GridViewExportUtil.Export("TicketsDetails.xls", this.gvAssigneeCallsStatus);
        }
    }
    protected void btnSearch1_Click(object sender, EventArgs e)
    {
        FillNotClosedStatusPieDept();
        FillOpenClosedStatusPieDept();
        FillAllExcludeDept();
        FillCallAgingDept();
        FillOpenClosedDept();
    }
    private void FillOpenClosedStatusPieDept()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_LiveDashboard", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrgId", Session["SD_OrgID"].ToString());
                    cmd.Parameters.AddWithValue("@Option", "OpenClosedPieDept");
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            adp.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                //
                                //Chart1.DataSource = dt;
                                //Chart1.Series[0].XValueMember = "Status";
                                //Chart1.Series[0].YValueMembers = "Ticket Count";
                                //Chart1.DataBind();
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
    private void FillOpenClosedDept()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand("SP_LiveDashboard", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrgId", Session["SD_OrgID"].ToString());
                    cmd.Parameters.AddWithValue("@Option", "OpenClosed");
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        using (DataSet ds = new DataSet())
                        {
                            adp.Fill(ds);

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                gvOpenClosedDept.DataSource = ds;
                                gvOpenClosedDept.DataBind();
                            }
                            else
                            {
                                gvOpenClosedDept.DataSource = null;
                                gvOpenClosedDept.DataBind();
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
    private void FillNotClosedStatusPieDept()
    {
        string chartWorkStatOs = "";
        string WorkStatcount = "";
        string WorkStatlabel = "";
        try
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand("SP_LiveDashboard", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrgId", Session["SD_OrgID"].ToString());
                    cmd.Parameters.AddWithValue("@Option", "NotOpenClosedPieDept");
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
    private void FillCallAgingDept()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand("SP_LiveDashboard", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrgId", Session["SD_OrgID"].ToString());
                    cmd.Parameters.AddWithValue("@Option", "AgeDept");
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            adp.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                gvCallAgingDept.DataSource = dt;
                                gvCallAgingDept.DataBind();
                                //Calculate Sum and display in Footer Row

                                gvCallAgingDept.FooterRow.Cells[0].Text = "Total";
                                gvCallAgingDept.FooterRow.Cells[0].HorizontalAlign = HorizontalAlign.Right;

                                gvCallAgingDept.FooterRow.Cells[1].Text = (dt.AsEnumerable().Sum(row => row.Field<int?>("0 to 3 Days") ?? 0)).ToString();
                                gvCallAgingDept.FooterRow.Cells[2].Text = (dt.AsEnumerable().Sum(row => row.Field<int?>("4 to 7 Days") ?? 0)).ToString();
                                gvCallAgingDept.FooterRow.Cells[3].Text = (dt.AsEnumerable().Sum(row => row.Field<int?>("8 to 10 Days") ?? 0)).ToString();
                                gvCallAgingDept.FooterRow.Cells[4].Text = (dt.AsEnumerable().Sum(row => row.Field<int?>("More than 10 Days") ?? 0)).ToString();
                            }
                            else
                            {
                                gvCallAgingDept.DataSource = null;
                                gvCallAgingDept.DataBind();
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
    private void FillAllExcludeDept()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand("SP_LiveDashboard", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrgId", Session["SD_OrgID"].ToString());
                    cmd.Parameters.AddWithValue("@Option", "NotOpenClosedDept");
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        using (DataSet ds = new DataSet())
                        {
                            adp.Fill(ds);

                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                lblTotalgrid.Text = "0";
                                lblTotalgrid.Text = ds.Tables[0].Rows.Count.ToString();
                                gvAssigneeCallsStatusDept.DataSource = ds;
                                gvAssigneeCallsStatusDept.DataBind();
                            }
                            else
                            {
                                gvAssigneeCallsStatusDept.DataSource = null;
                                gvAssigneeCallsStatusDept.DataBind();
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
    protected void gvAssigneeCallsStatusDept_DataBound(object sender, EventArgs e)
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
            msg.ReportError(ex.Message);

        }
    }
    protected void gvOpenClosedDept_RowCommand(object sender, GridViewCommandEventArgs e)
    {


    }
    private static string DeptNameDept;
    private static string HeaderNameDept;
    protected void gvCallAgingDept_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        if (e.CommandName == "VIEW0to3")
        {
            LinkButton lnkView1 = (LinkButton)e.CommandSource;
            DeptNameDept = lnkView1.CommandArgument;
            HeaderNameDept = gvCallAgingDept.HeaderRow.Cells[1].Text;
            FillAgingDetailsDept();
        }
        if (e.CommandName == "VIEW4to7")
        {
            LinkButton lnkView2 = (LinkButton)e.CommandSource;
            DeptNameDept = lnkView2.CommandArgument;
            HeaderNameDept = gvCallAgingDept.HeaderRow.Cells[2].Text;
            FillAgingDetailsDept();
        }
        if (e.CommandName == "VIEW8to10")
        {
            LinkButton lnkView3 = (LinkButton)e.CommandSource;
            DeptNameDept = lnkView3.CommandArgument;
            HeaderNameDept = gvCallAgingDept.HeaderRow.Cells[3].Text;
            FillAgingDetailsDept();
        }
        if (e.CommandName == "VIEWMore10Days")
        {
            LinkButton lnkView4 = (LinkButton)e.CommandSource;
            DeptNameDept = lnkView4.CommandArgument;
            HeaderNameDept = gvCallAgingDept.HeaderRow.Cells[4].Text;
            FillAgingDetailsDept();
        }
        data();
    }
    private void FillAgingDetailsDept()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {

                using (SqlCommand cmd = new SqlCommand("SP_LiveDashboard", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrgId", Session["SD_OrgID"].ToString());
                    cmd.Parameters.AddWithValue("@Aging", HeaderNameDept);
                    cmd.Parameters.AddWithValue("@Department", DeptNameDept);
                    cmd.Parameters.AddWithValue("@Option", "AgeDetailsDept");
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            adp.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                lblTotalgrid.Text = "0";
                                lblTotalgrid.Text = dt.Rows.Count.ToString();
                                gvCallAgingDetailsDept.DataSource = dt;
                                gvCallAgingDetailsDept.DataBind();
                            }
                            else
                            {
                                gvCallAgingDetailsDept.DataSource = null;
                                gvCallAgingDetailsDept.DataBind();
                                lblTotalgrid.Text = "0";
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
    protected void ImgbtnCallAgingExportDept_Click(object sender, ImageClickEventArgs e)
    {
        if (gvCallAgingDept.Rows.Count > 0)
        {
            this.gvCallAgingDept.AllowPaging = false;
            GridViewExportUtil.Export("TicketsCallAgingDetails.xls", this.gvCallAgingDept);
        }
    }
    protected void gvAssigneeCallsStatusDept_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
    protected void ChartCallsStatusDept_Click(object sender, ImageMapEventArgs e)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                con.Open();
                string cmdstr = "SP_LiveDashboard";
                using (SqlCommand cmd = new SqlCommand(cmdstr, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.SelectCommand.CommandTimeout = 180;
                        cmd.Parameters.AddWithValue("@Status", e.PostBackValue);
                        cmd.Parameters.AddWithValue("@OrgId", Session["SD_OrgID"].ToString());
                        cmd.Parameters.AddWithValue("@Option", "TicketDetailsDept");
                        adp.Fill(ds);
                        lblTotalgrid.Text = "0";
                        lblTotalgrid.Text = ds.Tables[0].Rows.Count.ToString();
                        gvCallAgingDetails.DataSource = ds;
                        gvCallAgingDetails.DataBind();
                    }
                }
            }
        }

        catch (Exception ex)
        {
            msg.ReportError(ex.Message);

        }
    }
    protected void ChartCallsStatusDept_Load(object sender, EventArgs e)
    {

    }
    protected void gridviewcategoryDept_DataBound(object sender, EventArgs e)
    {
        try
        {
            if (gridviewcategory.Rows.Count > 0)
            {
                int TotalRows = gridviewcategory.Rows.Count;
                int TotalCol = gridviewcategory.Rows[0].Cells.Count;
                gridviewcategory.FooterRow.Cells[0].Text = "Total : ";

                for (int i = 1; i < TotalCol; i++)
                {
                    int sum = 0;
                    for (int j = 0; j < TotalRows; j++)
                    {
                        sum += gridviewcategory.Rows[j].Cells[i].Text != "&nbsp;" ? int.Parse(gridviewcategory.Rows[j].Cells[i].Text) : 0;
                        gridviewcategory.Rows[j].Cells[i].Text = gridviewcategory.Rows[j].Cells[i].Text != "&nbsp;" ? gridviewcategory.Rows[j].Cells[i].Text : "0";
                    }
                    gridviewcategory.FooterRow.Cells[i].Text = sum.ToString("#");

                }
            }

        }

        catch (Exception ex)
        {
            msg.ReportError(ex.Message);

        }
    }
    protected void ImageButton2Dept_Click(object sender, ImageClickEventArgs e)
    {
        if (gridviewcategoryDept.Rows.Count > 0)
        {
            this.gridviewcategory.AllowPaging = false;
            GridViewExportUtil.Export("TicketsDetails.xls", this.gridviewcategory);
        }
    }
    protected void btnchartcallstatusDept_Click(object sender, EventArgs e)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                con.Open();
                string cmdstr = "SP_LiveDashboard";
                using (SqlCommand cmd = new SqlCommand(cmdstr, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                    {
                        adp.SelectCommand.CommandTimeout = 180;
                        cmd.Parameters.AddWithValue("@Status", hdnfldVariable.Value);
                        cmd.Parameters.AddWithValue("@OrgId", Session["SD_OrgID"].ToString());
                        cmd.Parameters.AddWithValue("@Option", "TicketDetailsDept");
                        adp.Fill(ds);
                        lblTotalgrid.Text = "0";
                        lblTotalgrid.Text = ds.Tables[0].Rows.Count.ToString();
                        gvCallAgingDetails.DataSource = ds;
                        gvCallAgingDetails.DataBind();
                    }
                }
            }
        }

        catch (Exception ex)
        {
            msg.ReportError(ex.Message);

        }
    }
    #endregion end DeptWise
    protected void ImgbtnExport4_Click(object sender, ImageClickEventArgs e)
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
                gvSDTicketsDetails.AllowPaging = false;

                foreach (System.Web.UI.WebControls.TableCell cell in gvSDTicketsDetails.HeaderRow.Cells)
                {
                    cell.BackColor = gvSDTicketsDetails.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in gvSDTicketsDetails.Rows)
                {

                    foreach (System.Web.UI.WebControls.TableCell cell in row.Cells)
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
    public class ChartDataSLA
    {
        public string[] ReponseSLA { get; set; }
        public int[] ReponseSLATotalTickets { get; set; }
        public string[] AllSeverityWiseResolve { get; set; }
        public int[] AllSeverityWiseResolveTotalTickets { get; set; }
        public string[] SeverityWiseResolution { get; set; }
        public int[] SeverityWiseResolutionTotalTickets { get; set; }
        public string[] AllSeverityWiseCalls { get; set; }
        public int[] AllSeverityWiseCallsTotalTickets { get; set; }
        public string[] ResolutionSLA { get; set; }
        public int[] ResolutionSLATotalTickets { get; set; }
        public string[] SeverityWiseResolutionSLA { get; set; }
        public int[] SeverityWiseResolutionSLATotalTickets { get; set; }
        public string[] SeverityWiseResponse { get; set; }
        public int[] SeverityWiseResponseTotalTickets { get; set; }
        public int[] StatusTotalTickets { get; set; }
        public string[] Status { get; set; }
        public int[] SeverityWiseCallsTotalTickets { get; set; }
        public string[] SeverityWiseCalls { get; set; }
    }
}
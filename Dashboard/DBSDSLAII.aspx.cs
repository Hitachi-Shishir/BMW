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

public partial class Dashboard_DBSDSLAII : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["LoginName"] == null)
        {
            Response.Redirect("/Default.aspx");
        }
        if (!IsPostBack)
        {
            FillDDLDesk();
        }
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
                    cmd.Parameters.AddWithValue("@OrgId", Session["SD_OrgID"].ToString());
                    cmd.Parameters.AddWithValue("@Option", "ALL");

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
    private void FillSeverityWiseResponseSLAPie()
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
                using (SqlCommand cmd = new SqlCommand("SD_SDDashboardSLAWithCal"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ServiceDesk", DropDesks.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@FROM", formattedFromDate);
                    cmd.Parameters.AddWithValue("@TO", formattedToDate);
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
            string fromd = txtFrom.Text;
            DateTime frmdate = DateTime.ParseExact(fromd, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            string formattedFromDate = frmdate.ToString("yyyy-MM-dd");
            string to = txtTo.Text;
            DateTime todate = DateTime.ParseExact(to, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            string formattedToDate = todate.ToString("yyyy-MM-dd");
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SD_SDDashboardSLAWithCal"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ServiceDesk", DropDesks.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@OrgId", Session["SD_OrgID"].ToString());
                    cmd.Parameters.AddWithValue("@FROM", formattedFromDate);
                    cmd.Parameters.AddWithValue("@TO", formattedToDate);
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
                                string chartScriptText = GenerateSeverityWiseResolution(dt).Replace("HIDDEN_FIELD_ID", hdnfldVariable.ClientID)
       .Replace("BUTTON_ID", btnChartSeverityWiseResolution.ClientID);
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
    public static string GenerateSeverityWiseResolution(DataTable dt)
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

        var chart = new ApexCharts(document.querySelector('#chart4'), options);
        chart.render();
    }});
";

        return chartOptions;
    }
    private void FillResolutionSLAPie()
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
                using (SqlCommand cmd = new SqlCommand("SD_SDDashboardSLAWithCal"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ServiceDesk", DropDesks.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@FROM", formattedFromDate);
                    cmd.Parameters.AddWithValue("@OrgId", Session["SD_OrgID"].ToString());
                    cmd.Parameters.AddWithValue("@TO", formattedToDate);
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

        var chart = new ApexCharts(document.querySelector('#chart2'), options);
        chart.render();
    }});
";

        return chartOptions;
    }
    private void FillReponseSLAPie()
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
                using (SqlCommand cmd = new SqlCommand("SD_SDDashboardSLAWithCal"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ServiceDesk", DropDesks.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@FROM", formattedFromDate);
                    cmd.Parameters.AddWithValue("@TO", formattedToDate);
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
                                string chartScriptText = ReponseSLAPie(dt)
        .Replace("HIDDEN_FIELD_ID", hdnfldVariable.ClientID)
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
    public static string ReponseSLAPie(DataTable dt)
    {
        var chartData = new ChartDataSLA
        {
            Reponse = dt.AsEnumerable().Select(row => row.Field<string>("CustomFieldValue")).ToArray(),
            ReponseTotalTickets = dt.AsEnumerable().Select(row => row.Field<int>("TotalTickets")).ToArray()
        };

        var serializer = new JavaScriptSerializer();
        string StatusJson = serializer.Serialize(chartData.Reponse);
        string totalTicketsJson = serializer.Serialize(chartData.ReponseTotalTickets);

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
                categories: {StatusJson},
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
    private void FillAllSeverityWiseCalls()
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
                using (SqlCommand cmd = new SqlCommand("SD_SDDashboardSLAWithCal"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ServiceDesk", DropDesks.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@FROM", formattedFromDate);
                    cmd.Parameters.AddWithValue("@TO", formattedToDate);
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
                                string chartScriptText = GenerateChartAllSeverityWiseCalls(dt)
        .Replace("HIDDEN_FIELD_ID", hdnfldVariable.ClientID)
        .Replace("BUTTON_ID", btnChartServerityWiseTotal.ClientID);
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
    public static string GenerateChartAllSeverityWiseCalls(DataTable dt)
    {
        var chartData = new ChartDataSLA
        {
            AllSeverityWiseCalls = dt.AsEnumerable().Select(row => row.Field<string>("Severity")).ToArray(),
            AllSeverityWiseCallsTotalTickets = dt.AsEnumerable().Select(row => row.Field<int>("TotalTickets")).ToArray()
        };

        var serializer = new JavaScriptSerializer();
        string ResolutionSLAJson = serializer.Serialize(chartData.AllSeverityWiseCalls);
        string totalTicketsJson = serializer.Serialize(chartData.AllSeverityWiseCallsTotalTickets);

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
    private void FillAllSeverityWiseResolveCalls()
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
                using (SqlCommand cmd = new SqlCommand("SD_SDDashboardSLAWithCal"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ServiceDesk", DropDesks.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@FROM", formattedFromDate);
                    cmd.Parameters.AddWithValue("@OrgId", Session["SD_OrgID"].ToString());
                    cmd.Parameters.AddWithValue("@TO", formattedToDate);
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
                                string chartScriptText = GenerateChartAllSeverityWiseResolveCalls(dt)
                                        .Replace("HIDDEN_FIELD_ID", hdnfldVariable.ClientID)
                                        .Replace("BUTTON_ID", btnChartSeverityWiseResolCalls.ClientID);
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "renderChart6", chartScriptText, true);


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
    public static string GenerateChartAllSeverityWiseResolveCalls(DataTable dt)
    {
        var chartData = new ChartDataSLA
        {
            SeverityWiseResolution = dt.AsEnumerable().Select(row => row.Field<string>("Severity")).ToArray(),
            SeverityWiseResolutionTotalTickets = dt.AsEnumerable().Select(row => row.Field<int>("TotalTickets")).ToArray()
        };

        var serializer = new JavaScriptSerializer();
        string ResolutionSLAJson = serializer.Serialize(chartData.SeverityWiseResolution);
        string totalTicketsJson = serializer.Serialize(chartData.SeverityWiseResolutionTotalTickets);

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

        var chart = new ApexCharts(document.querySelector('#chart6'), options);
        chart.render();
    }});
";

        return chartOptions;
    }
    protected void btnChartResponseSLA_Click(object sender, EventArgs e)
    {
        string fromd = txtFrom.Text;
        DateTime frmdate = DateTime.ParseExact(fromd, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
        string formattedFromDate = frmdate.ToString("yyyy-MM-dd");
        string to = txtTo.Text;
        DateTime todate = DateTime.ParseExact(to, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
        string formattedToDate = todate.ToString("yyyy-MM-dd");
        string name1 = "rptDBSDII.aspx?chartdata=" + hdnfldVariable.Value + "&ReportType=ResponseSLADetails&ServiceDesk=" + DropDesks.SelectedItem.ToString() + "&From=" + formattedFromDate.Trim() + "&To=" + formattedToDate.Trim() + "&OrgId=" + Session["SD_OrgID"].ToString() + "";
        name1 = name1.Trim();
        Uri uri = new Uri(name1, UriKind.RelativeOrAbsolute);
        if (!uri.IsAbsoluteUri)
        {
            Response.Redirect(name1);
        }
    }
    protected void btnChartServerityWiseTotal_Click(object sender, EventArgs e)
    {
        string fromd = txtFrom.Text;
        DateTime frmdate = DateTime.ParseExact(fromd, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
        string formattedFromDate = frmdate.ToString("yyyy-MM-dd");
        string to = txtTo.Text;
        DateTime todate = DateTime.ParseExact(to, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
        string formattedToDate = todate.ToString("yyyy-MM-dd");
        string name1 = "rptDBSDII.aspx?chartdata=" + hdnfldVariable.Value + "&ReportType=SeverityWisePieDetails&ServiceDesk=" + DropDesks.SelectedItem.ToString() + "&From=" + formattedFromDate.Trim() + "&To=" + formattedToDate.Trim() + "&OrgId=" + Session["SD_OrgID"].ToString() + "";
        name1 = name1.Trim();
        Uri uri = new Uri(name1, UriKind.RelativeOrAbsolute);
        if (!uri.IsAbsoluteUri)
        {
            Response.Redirect(name1);
        }
    }
    protected void btnChartResolutionSLA_Click(object sender, EventArgs e)
    {
        string fromd = txtFrom.Text;
        DateTime frmdate = DateTime.ParseExact(fromd, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
        string formattedFromDate = frmdate.ToString("yyyy-MM-dd");
        string to = txtTo.Text;
        DateTime todate = DateTime.ParseExact(to, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
        string formattedToDate = todate.ToString("yyyy-MM-dd");
        string name1 = "rptDBSDII.aspx?chartdata=" + hdnfldVariable.Value + "&ReportType=ResolutionSLAPie&ServiceDesk=" + DropDesks.SelectedItem.ToString() + "&From=" + formattedFromDate.Trim() + "&To=" + formattedToDate.Trim() + "&OrgId=" + Session["SD_OrgID"].ToString() + "";
        name1 = name1.Trim();
        Uri uri = new Uri(name1, UriKind.RelativeOrAbsolute);
        if (!uri.IsAbsoluteUri)
        {
            Response.Redirect(name1);
        }
    }
    protected void btnChartSeverity_Click(object sender, EventArgs e)
    {
        string fromd = txtFrom.Text;
        DateTime frmdate = DateTime.ParseExact(fromd, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
        string formattedFromDate = frmdate.ToString("yyyy-MM-dd");
        string to = txtTo.Text;
        DateTime todate = DateTime.ParseExact(to, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
        string formattedToDate = todate.ToString("yyyy-MM-dd");
        string name1 = "rptDBSDII.aspx?chartdata=" + hdnfldVariable.Value + "&ReportType=SeverityWiseResponseDetails&ServiceDesk=" + DropDesks.SelectedItem.ToString() + "&From=" + formattedFromDate.Trim() + "&To=" + formattedToDate.Trim() + "&OrgId=" + Session["SD_OrgID"].ToString() + "";
        name1 = name1.Trim();
        Uri uri = new Uri(name1, UriKind.RelativeOrAbsolute);
        if (!uri.IsAbsoluteUri)
        {
            Response.Redirect(name1);
        }
    }
    protected void btnChartSeverityWiseResolCalls_Click(object sender, EventArgs e)
    {
        string fromd = txtFrom.Text;
        DateTime frmdate = DateTime.ParseExact(fromd, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
        string formattedFromDate = frmdate.ToString("yyyy-MM-dd");
        string to = txtTo.Text;
        DateTime todate = DateTime.ParseExact(to, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
        string formattedToDate = todate.ToString("yyyy-MM-dd");
        string name1 = "rptDBSDII.aspx?chartdata=" + hdnfldVariable.Value + "&ReportType=AllSeverityWiseResolveCalls&ServiceDesk=" + DropDesks.SelectedItem.ToString() + "&From=" + formattedFromDate.Trim() + "&To=" + formattedToDate.Trim() + "&OrgId=" + Session["SD_OrgID"].ToString() + "";
        name1 = name1.Trim();
        Uri uri = new Uri(name1, UriKind.RelativeOrAbsolute);
        if (!uri.IsAbsoluteUri)
        {
            Response.Redirect(name1);
        }
    }
    protected void btnChartSeverityWiseResolution_Click(object sender, EventArgs e)
    {
        string fromd = txtFrom.Text;
        DateTime frmdate = DateTime.ParseExact(fromd, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
        string formattedFromDate = frmdate.ToString("yyyy-MM-dd");
        string to = txtTo.Text;
        DateTime todate = DateTime.ParseExact(to, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
        string formattedToDate = todate.ToString("yyyy-MM-dd");
        string name1 = "rptDBSDII.aspx?chartdata=" + hdnfldVariable.Value + "&ReportType=SeverityWiseResolution&ServiceDesk=" + DropDesks.SelectedItem.ToString() + "&From=" + formattedFromDate.Trim() + "&To=" + formattedToDate.Trim() + "&OrgId=" + Session["SD_OrgID"].ToString() + "";
        name1 = name1.Trim();
        Uri uri = new Uri(name1, UriKind.RelativeOrAbsolute);
        if (!uri.IsAbsoluteUri)
        {
            Response.Redirect(name1);
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        FillReponseSLAPie();
        FillResolutionSLAPie();
        FillSeverityWiseResponseSLAPie();
        FillSeverityWiseResolutionSLAPie();
        FillAllSeverityWiseCalls();
        FillAllSeverityWiseResolveCalls();
    }
}
public class ChartDataSLA
{
    public string[] Reponse { get; set; }
    public int[] ReponseTotalTickets { get; set; }
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
}
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

public partial class Dashboard_frmDBDayWise : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["LoginName"] == null)
        {
            Response.Redirect("/Default.aspx");
        }
        try
        {
            FillOverAllPie("TotalTicketsPie");
            FillTodayTicketsPie("TicketsDayWisePie", "0");
            FillLast30DaysTicketsPie("TicketsDayWisePie", "-30");
            FillLast7DaysTicketsPie("TicketsDayWisePie", "-7");
            DashboardCount("DashboardsCounts");
        }
        catch (Exception ex)
        {

        }
    }
    private void DashboardCount(string option)
    {
        using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand("SD_SDDashboardDayWise"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@FROM", txtFrom.Text);
                //cmd.Parameters.AddWithValue("@TO", txtTo.Text);
                cmd.Parameters.AddWithValue("@Scope", Session["UserScope"].ToString());
                cmd.Parameters.AddWithValue("@OrgId", Session["SD_OrgID"].ToString());
                cmd.Parameters.AddWithValue("@TechLoginName", Session["LoginName"].ToString());
                cmd.Parameters.AddWithValue("@Option", option);

                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataSet ds = new DataSet())
                    {
                        sda.Fill(ds);
                        lbl7DaysTickets.Text = ds.Tables[0].Rows[0]["SevenDaysTotal"].ToString();
                        lbl30DaysTickets.Text = ds.Tables[0].Rows[0]["ThirtyDaysTotal"].ToString();
                        lblTodayTickets.Text = ds.Tables[0].Rows[0]["TodayTotal"].ToString();
                        lblTotalTickets.Text = ds.Tables[0].Rows[0]["Total"].ToString();
                    }
                }
            }
        }
    }
    private void FillLast30DaysTicketsPie(string proc, string d)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SD_SDDashboardDayWise"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Days", "-30");
                    cmd.Parameters.AddWithValue("@Scope", Session["UserScope"].ToString());
                    cmd.Parameters.AddWithValue("@OrgId", Session["SD_OrgID"].ToString());
                    cmd.Parameters.AddWithValue("@TechLoginName", Session["LoginName"].ToString());
                    cmd.Parameters.AddWithValue("@Option", proc);
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
                                string chartScriptText = GenerateChartLast30DaysTicketsPie(dt).Replace("HIDDEN_FIELD_ID", hdnfldVariable.ClientID)
             .Replace("BUTTON_ID", btnChart30DaysTickets.ClientID);
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
    public static string GenerateChartLast30DaysTicketsPie(DataTable dt)
    {
        var chartData = new ChartDataDBDayWise
        {
            Last30Days = dt.AsEnumerable().Select(row => row.Field<string>("Status")).ToArray(),
            Last30DaysTotalTickets = dt.AsEnumerable().Select(row => row.Field<int>("TotalTickets")).ToArray()
        };

        var serializer = new JavaScriptSerializer();
        string ResolutionSLAJson = serializer.Serialize(chartData.Last30Days);
        string totalTicketsJson = serializer.Serialize(chartData.Last30DaysTotalTickets);

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
    private void FillLast7DaysTicketsPie(string proc, string d)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SD_SDDashboardDayWise"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Days", "-7");
                    cmd.Parameters.AddWithValue("@Scope", Session["UserScope"].ToString());
                    cmd.Parameters.AddWithValue("@OrgId", Session["SD_OrgID"].ToString());
                    cmd.Parameters.AddWithValue("@TechLoginName", Session["LoginName"].ToString());
                    cmd.Parameters.AddWithValue("@Option", proc);
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
                                string chartScriptText = GenerateChartLast7DaysTicketsPie(dt).Replace("HIDDEN_FIELD_ID", hdnfldVariable.ClientID)
             .Replace("BUTTON_ID", btnChart7DaysTickets.ClientID);
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "renderChart4", chartScriptText, true);
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
    public static string GenerateChartLast7DaysTicketsPie(DataTable dt)
    {
        var chartData = new ChartDataDBDayWise
        {
            Last7Days = dt.AsEnumerable().Select(row => row.Field<string>("Status")).ToArray(),
            Last7DaysTotalTickets = dt.AsEnumerable().Select(row => row.Field<int>("TotalTickets")).ToArray()
        };

        var serializer = new JavaScriptSerializer();
        string ResolutionSLAJson = serializer.Serialize(chartData.Last7Days);
        string totalTicketsJson = serializer.Serialize(chartData.Last7DaysTotalTickets);

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
    private void FillTodayTicketsPie(string proc, string d)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SD_SDDashboardDayWise"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Days", "0");
                    cmd.Parameters.AddWithValue("@Scope", Session["UserScope"].ToString());
                    cmd.Parameters.AddWithValue("@OrgId", Session["SD_OrgID"].ToString());
                    cmd.Parameters.AddWithValue("@TechLoginName", Session["LoginName"].ToString());
                    cmd.Parameters.AddWithValue("@Option", proc);
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
                                string chartScriptText = GenerateChartTodayTicketsPie(dt).Replace("HIDDEN_FIELD_ID", hdnfldVariable.ClientID)
             .Replace("BUTTON_ID", btnChartTodayTickets.ClientID);
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "renderChart2", chartScriptText, true);
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // msg.ReportError1(ex.Message);
        }
    }
    public static string GenerateChartTodayTicketsPie(DataTable dt)
    {
        var chartData = new ChartDataDBDayWise
        {
            TodayTicketsPie = dt.AsEnumerable().Select(row => row.Field<string>("Status")).ToArray(),
            TodayTicketsPieTotalTickets = dt.AsEnumerable().Select(row => row.Field<int>("TotalTickets")).ToArray()
        };

        var serializer = new JavaScriptSerializer();
        string ResolutionSLAJson = serializer.Serialize(chartData.TodayTicketsPie);
        string totalTicketsJson = serializer.Serialize(chartData.TodayTicketsPieTotalTickets);

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
    private void FillOverAllPie(string proc)
    {
        try
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SD_SDDashboardDayWise"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //		cmd.Parameters.AddWithValue("@ServiceDesk", DropDesks.SelectedItem.ToString());
                    cmd.Parameters.AddWithValue("@Scope", Session["UserScope"].ToString());
                    cmd.Parameters.AddWithValue("@TechLoginName", Session["LoginName"].ToString());
                    cmd.Parameters.AddWithValue("@OrgId", Session["SD_OrgID"].ToString());
                    cmd.Parameters.AddWithValue("@Option", "TotalTicketsPie");
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
                                string chartScriptText = GenerateChartOverAllPie(dt).Replace("HIDDEN_FIELD_ID", hdnfldVariable.ClientID)
          .Replace("BUTTON_ID", btnChartTotalTicketDetails.ClientID);
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "renderChart1", chartScriptText, true);
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
    public static string GenerateChartOverAllPie(DataTable dt)
    {
        var chartData = new ChartDataDBDayWise
        {
            OverAllPie = dt.AsEnumerable().Select(row => row.Field<string>("Status")).ToArray(),
            OverAllPieTotalTickets = dt.AsEnumerable().Select(row => row.Field<int>("TotalTickets")).ToArray()
        };

        var serializer = new JavaScriptSerializer();
        string ResolutionSLAJson = serializer.Serialize(chartData.OverAllPie);
        string totalTicketsJson = serializer.Serialize(chartData.OverAllPieTotalTickets);

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
    protected void imgbtnTodayTickets_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("rptDBSD.aspx?Data=TodayTickets&UserScope=" + Session["UserScope"].ToString() + "&OrgId=" + Session["SD_OrgID"].ToString() + "");
    }
    protected void imgbtn7DaysTickets_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("rptDBSD.aspx?Data=SevenDaysTickets&UserScope=" + Session["UserScope"].ToString() + "&OrgId=" + Session["SD_OrgID"].ToString() + "");

    }
    protected void imgbtn30DaysTickets_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("rptDBSD.aspx?Data=ThirtyDaysTickets&UserScope=" + Session["UserScope"].ToString() + "&OrgId=" + Session["SD_OrgID"].ToString() + "");

    }
    protected void imgbtnTotalTickets_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("rptDBSD.aspx?Data=TotalTickets&UserScope=" + Session["UserScope"].ToString() + "&OrgId=" + Session["SD_OrgID"].ToString() + "");

    }

    protected void btnChartTodayTickets_Click(object sender, EventArgs e)
    {
        Response.Redirect("rptDBSD.aspx?ReportType=TodayTicketsStatusWise&chartdata=" + hdnfldVariable.Value.ToString().Trim() + "&UserScope=" + Session["UserScope"].ToString() + "&OrgId=" + Session["SD_OrgID"].ToString() + "");
    }

    protected void btnChart7DaysTickets_Click(object sender, EventArgs e)
    {
        Response.Redirect("rptDBSD.aspx?ReportType=7DaysTicketsStatusWise&chartdata=" + hdnfldVariable.Value.ToString().Trim() + "&UserScope=" + Session["UserScope"].ToString() + "&OrgId=" + Session["SD_OrgID"].ToString() + "");
    }

    protected void btnChart30DaysTickets_Click(object sender, EventArgs e)
    {
        Response.Redirect("rptDBSD.aspx?ReportType=30DaysTicketsStatusWise&chartdata=" + hdnfldVariable.Value.ToString().Trim() + "&UserScope=" + Session["UserScope"].ToString() + "&OrgId=" + Session["SD_OrgID"].ToString() + "");
    }

    protected void btnChartTotalTicketDetails_Click(object sender, EventArgs e)
    {
        Response.Redirect("rptDBSD.aspx?ReportType=AllTicketsStatusWise&chartdata=" + hdnfldVariable.Value.ToString().Trim() + "&UserScope=" + Session["UserScope"].ToString() + "&OrgId=" + Session["SD_OrgID"].ToString() + "");
    }

    protected void lblTodayTickets_Click(object sender, EventArgs e)
    {
        Response.Redirect("rptDBSD.aspx?Data=TodayTickets&UserScope=" + Session["UserScope"].ToString() + "&OrgId=" + Session["SD_OrgID"].ToString() + "");
    }

    protected void lbl7DaysTickets_Click(object sender, EventArgs e)
    {
        Response.Redirect("rptDBSD.aspx?Data=SevenDaysTickets&OrgId=" + Session["SD_OrgID"].ToString() + "");
    }

    protected void lbl30DaysTickets_Click(object sender, EventArgs e)
    {
        Response.Redirect("rptDBSD.aspx?Data=ThirtyDaysTickets&UserScope=" + Session["UserScope"].ToString() + "&OrgId=" + Session["SD_OrgID"].ToString() + "");
    }

    protected void lblTotalTickets_Click(object sender, EventArgs e)
    {
        Response.Redirect("rptDBSD.aspx?Data=TotalTickets&UserScope=" + Session["UserScope"].ToString() + "&OrgId=" + Session["SD_OrgID"].ToString() + "");
    }
    public class ChartDataDBDayWise
    {
        public string[] OverAllPie { get; set; }
        public int[] OverAllPieTotalTickets { get; set; }
        public int[] TodayTicketsPieTotalTickets { get; set; }
        public string[] TodayTicketsPie { get; set; } 
        public int[] Last30DaysTotalTickets { get; set; }
        public string[] Last30Days { get; set; }
        public int[] Last7DaysTotalTickets { get; set; }
        public string[] Last7Days { get; set; }
    }
}
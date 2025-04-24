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
using System.Web.Script.Serialization;

public partial class Dashboard_SDDBCategoryWise : System.Web.UI.Page
{
    public static string Category;
    protected void Page_Load(object sender, EventArgs e)
    {
        //   this.FillStatusWiseTickets();
        //   this.FillCategoryWiseTickets();
        if (Session["LoginName"] != null && Session["UserScope"] != null)
        {
            //themeIssueReolve();

            if (this.IsPostBack)
                return;
            this.FillDDLDesk();
        }
        else
        {
            Response.Redirect("/Default.aspx");
        }
    }

    public void themeIssueReolve()
    {
        string sql = "select theme,ThemeModify,OrgName from SD_User_Master a " +
                "INNER JOIN SD_OrgMaster o ON a.Org_ID=o.Org_ID where UserID='" + Convert.ToString(Session["UserID"]) + "'  and o.Org_ID='" + Convert.ToString(Session["SD_OrgID"]) + "'";
        DataTable dt = database.GetDataTable(sql);
        string theme = Convert.ToString(dt.Rows[0]["theme"]);
        if (theme != null)
        {
            string script = $"document.documentElement.setAttribute('data-bs-theme', '{theme}');";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SetTheme", script, true);
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

    private void FillStatusWiseTickets(string Category)
    {
        try
        {
            string fromd = txtFrom.Text;
            DateTime frmdate = DateTime.ParseExact(fromd, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            string formattedFromDate = frmdate.ToString("yyyy-MM-dd");
            string to = txtTo.Text;
            DateTime todate = DateTime.ParseExact(to, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            string formattedToDate = todate.ToString("yyyy-MM-dd");
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                connection.Open();
                using (SqlCommand selectCommand = new SqlCommand("SD_SDDashboard_CategoryWise", connection))
                {
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        selectCommand.CommandType = CommandType.StoredProcedure;
                        selectCommand.Parameters.AddWithValue("@FROM", formattedFromDate);
                        selectCommand.Parameters.AddWithValue("@ServiceDesk", DropDesks.SelectedItem.ToString());
                        selectCommand.Parameters.AddWithValue("@TO", formattedToDate);
                        selectCommand.Parameters.AddWithValue("@Category", Category);
                        selectCommand.Parameters.AddWithValue("@Scope", Session["UserScope"].ToString());
                        selectCommand.Parameters.AddWithValue("@OrgId", Session["SD_OrgID"].ToString());
                        selectCommand.Parameters.AddWithValue("@Option", (object)"StatusWiseChart");
                        using (DataTable dt = new DataTable())
                        {
                            sqlDataAdapter.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {

                                string chartScriptText = GenerateChartStatus(dt)
        .Replace("HIDDEN_FIELD_ID", hdnfldStatus.ClientID)
        .Replace("BUTTON_ID", btncategorystatus.ClientID);
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "renderChart2", chartScriptText, true);
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // this.msg.ReportError1(ex.Message);
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        FillCategoryWiseTickets();
    }

    private void FillCategoryWiseTickets()
    {
        string fromd = txtFrom.Text;
        DateTime frmdate = DateTime.ParseExact(fromd, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
        string formattedFromDate = frmdate.ToString("yyyy-MM-dd");
        string to = txtTo.Text;
        DateTime todate = DateTime.ParseExact(to, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
        string formattedToDate = todate.ToString("yyyy-MM-dd");
        try
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                connection.Open();
                using (SqlCommand selectCommand = new SqlCommand("SD_SDDashboard_CategoryWise", connection))
                {
                    selectCommand.CommandType = CommandType.StoredProcedure;
                    selectCommand.Parameters.AddWithValue("@FROM", formattedFromDate);
                    selectCommand.Parameters.AddWithValue("@ServiceDesk", DropDesks.SelectedItem.ToString());
                    selectCommand.Parameters.AddWithValue("@TO", formattedToDate);
                    selectCommand.Parameters.AddWithValue("@Scope", Session["UserScope"].ToString());
                    selectCommand.Parameters.AddWithValue("@OrgId", Session["SD_OrgID"].ToString());
                    selectCommand.Parameters.AddWithValue("@Option", (object)"CategoryWiseBar");
                    using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(selectCommand))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            sqlDataAdapter.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {

                                string chartScriptText = GenerateChartCategory(dt)
        .Replace("HIDDEN_FIELD_ID", hdnfldVariable.ClientID)
        .Replace("BUTTON_ID", btncategory.ClientID);
                                ClientScript.RegisterClientScriptBlock(this.GetType(), "renderChart", chartScriptText, true);
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //this.msg.ReportError1(ex.Message);
        }
    }

    public static string GenerateChartCategory(DataTable dt)
    {
        var chartData = new ChartDataCategory
        {
            Categories = dt.AsEnumerable().Select(row => row.Field<string>("Category2")).ToArray(),
            CategoryTotalTickets = dt.AsEnumerable().Select(row => row.Field<int>("TotalCounts")).ToArray()
        };

        var serializer = new JavaScriptSerializer();
        string categoriesJson = serializer.Serialize(chartData.Categories);
        string totalTicketsJson = serializer.Serialize(chartData.CategoryTotalTickets);

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
                    gradientToColors: [ '#7928ca'],
                    shadeIntensity: 1,
                    type: 'vertical',
                    stops: [0, 100, 100, 100]
                }}
            }},
            colors: ['#ff6a00', '#005bea' , '#ff0080'],
            plotOptions: {{
                bar: {{
                    horizontal: false,
                    borderRadius: 4,
  borderRadiusApplication: 'around',
            borderRadiusWhenStacked: 'last',
                    columnWidth: '35%'
                }}
            }},
            dataLabels: {{
                enabled: false
            }},
            stroke: {{
                show: !0,
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
                categories: {categoriesJson},
                labels: {{
                    show: true,
                    style: {{
                        colors: '#9ba7b2',
                        fontSize: '8px'
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

    public static string GenerateChartStatus(DataTable dt)
    {
        var chartData = new ChartDataCategory
        {
            Status = dt.AsEnumerable().Select(row => row.Field<string>("Status")).ToArray(),
            StatusTotalTickets = dt.AsEnumerable().Select(row => row.Field<int>("TotalCounts")).ToArray()
        };

        var serializer = new JavaScriptSerializer();
        string StatusJson = serializer.Serialize(chartData.Status);
        string totalTicketsJson = serializer.Serialize(chartData.StatusTotalTickets);

        string chartOptions = $@"
    $(function() {{
        var options = {{
            series: [{{
                name: 'Total Tickets',
                data: {totalTicketsJson}
            }}],
            chart: {{
                foreColor: '#9ba7b2',
                height: 300,
                type: 'area',
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
                    show: !1,
                }}
            }},
            fill: {{
                type: 'gradient',
                gradient: {{
                    shade: 'dark',
                    gradientToColors: ['#ff0080'],
                    shadeIntensity: 1,
                    type: 'vertical',
                    opacityFrom: 0.8,
      opacityTo: 0.1,
      stops: [0, 100, 100, 100]
                }}
            }},
            colors: ['#ffd200'],

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
                  width: 4,
    curve: 'smooth'
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

        var chart = new ApexCharts(document.querySelector('#chart2'), options);
        chart.render();
    }});
";

        return chartOptions;
    }


    //    public static string GenerateChartAssigne(DataTable dt)
    //    {
    //        var chartData = new ChartDataCategory
    //        {
    //            CategoryTotalTickets = dt.AsEnumerable().Select(row => row.Field<int>("TotalCounts")).ToArray(),
    //            Categories = dt.AsEnumerable().Select(row => row.Field<string>("Category2")).ToArray()
    //        };
    //        var serializer = new JavaScriptSerializer();
    //        string TicketCntJson = serializer.Serialize(chartData.CategoryTotalTickets);
    //        string CategoriesJson = serializer.Serialize(chartData.Categories);
    //        string chartOptions = $@"
    //<script>
    // var options = {{
    //            series: [{{
    //                name: 'Total Tickets',
    //                data: {TicketCntJson}

    //            }}],
    //            chart: {{
    //                foreColor: '#9ba7b2',
    //                height: 250,
    //                type: 'bar',
    //                zoom: {{
    //                    enabled: false
    //                }},
    //                toolbar: {{
    //                    show: !1,
    //                }}
    //            }},
    //            fill: {{
    //                type: 'gradient',
    //                gradient: {{
    //                    shade: 'dark',
    //                    gradientToColors: ['#009efd'],
    //                    shadeIntensity: 1,
    //                    type: 'vertical',
    //                    //opacityFrom: 0.8,
    //                    //opacityTo: 0.1,
    //                    stops: [0, 100, 100, 100]
    //                }},
    //            }},
    //            colors: ['#2af598'],
    //            plotOptions: {{
    //                bar: {{
    //                    horizontal: false,
    //                    borderRadius: 4,
    //                    borderRadiusApplication: 'around',
    //                    borderRadiusWhenStacked: 'last',
    //                    columnWidth: '35%',
    //                }}
    //            }},
    //            dataLabels: {{
    //                enabled: false
    //            }},
    //            stroke: {{
    //                show: !0,
    //                width: 4,
    //                colors: ['transparent']
    //            }},
    //            grid: {{
    //                show: true,
    //                borderColor: 'rgba(0, 0, 0, 0.15)',
    //                strokeDashArray: 4,
    //            }},
    //            tooltip: {{
    //                theme: 'dark',
    //            }},
    //            xaxis: {{
    //                categories: {CategoriesJson},
    // labels: {{
    //                    show: false  // Hide the x-axis labels
    //                }}
    //            }}
    //        }};

    //        var chart = new ApexCharts(document.querySelector('#chart1'), options);
    //        chart.render();
    //</script>
    //";

    //        return chartOptions;
    //    }


    protected void DropDesks_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand("select f.sdCategoryFK,s.codeRef,s.orgServDeskDefnFK from kasadmin.SDCategoryFullPath f\r\n                                                        join  kasadmin.SDCategory  s\r\n                                                        on s.id=f.sdCategoryFK where CategoryLevel=1  and orgServDeskDefnFK=@Desk\r\n                                                        order by s.codeRef asc"))
                {
                    sqlCommand.CommandType = CommandType.Text;
                    sqlCommand.Parameters.AddWithValue("@Desk", (object)this.DropDesks.SelectedValue);
                    sqlCommand.Connection = sqlConnection;
                    sqlConnection.Open();
                    this.ddlCategories.DataSource = (object)sqlCommand.ExecuteReader();
                    this.ddlCategories.DataTextField = "codeRef";
                    this.ddlCategories.DataValueField = "sdCategoryFK";
                    this.ddlCategories.DataBind();
                    sqlConnection.Close();
                }
            }
            this.ddlCategories.Items.Insert(0, new ListItem("-----Select Category-----", "0"));
        }
        catch (Exception ex)
        {
            //this.msg.ReportError(ex.Message);
        }
    }



    public override void VerifyRenderingInServerForm(Control control)
    {
    }

    protected void btncategory_Click(object sender, EventArgs e)
    {
        try
        {
            FillStatusWiseTickets(hdnfldVariable.Value);
            Category = hdnfldVariable.Value;
            btnSearch_Click(null, null);
        }
        catch (Exception ex)
        {
            // this.msg.ReportError1(ex.Message);
        }
    }

    protected void btncategorystatus_Click(object sender, EventArgs e)
    {
        btnSearch_Click(null, null);
        FillStatusWiseTickets(hdnfldVariable.Value);
    }
}
public class ChartDataCategory
{
    public string[] Categories { get; set; }
    public int[] CategoryTotalTickets { get; set; }
    public int[] StatusTotalTickets { get; set; }
    public string[] Status { get; set; }
}
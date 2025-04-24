using DocumentFormat.OpenXml.VariantTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Dashboard_frmUserDashboard : System.Web.UI.Page
{
    Util obj = new Util();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["OrgName"] != null)
        {
            Page.Title = Convert.ToString(Session["OrgName"]);
        }
        else
        {
            Page.Title = "Default Title";
        }
        if (Session["UserID"] == null)
        {
            Response.Redirect("/Default.aspx");
        }
        if (!IsPostBack)
        {
            FillOrganization();
            //FillRequestType(Convert.ToInt64(Session["SD_OrgID"]));
            lblUserName.Text = Convert.ToString(Session["UserName"]).ToUpper() + " " + "!";
            string sql = "SELECT FileData FROM SD_User_Master WHERE FileData IS NOT NULL AND LoginName = '" + Convert.ToString(Session["LoginName"]) + "'";
            byte[] fileData = (byte[])database.GetScalarValue(sql);
            string imageUrl = "";
            if (fileData != null && fileData.Length > 0)
            {
                imageUrl = "data:image/jpg;base64," + Convert.ToBase64String(fileData);
            }
            else
            {
                imageUrl = "~/Images/defimg.png"; ;
            }
            img.ImageUrl = imageUrl;
            getFilterData("0", "0", Convert.ToString("1900-01-01"), Convert.ToString("1900-01-01"), "0", "", "");
            ddlorg_SelectedIndexChanged(null, null);
        }
    }
    public void getFilterData(string ReqType, string Category, string frmDate, string toDate,
        string UserID, string Orgid, string SubmitterEmailId)
    {
        if (string.IsNullOrEmpty(Convert.ToString(frmDate)) || string.IsNullOrEmpty(Convert.ToString(toDate)))
        {
            frmDate = Convert.ToString("1900-01-01");
            toDate = Convert.ToString("1900-01-01");
        }
        DataSet ds = new DataSet();
        if (Convert.ToString(Session["UserRole"]).ToUpper() == "MASTER")
        {
            divlocation.Visible = true;
            divAssigne.Visible = true;
            divCategory.Visible = true;
            divDayWise.Visible = true;
            ddlorg.Enabled = true;
            ds = GetDataDashboard(ReqType, Category, frmDate, toDate, UserID, Orgid, SubmitterEmailId);
        }
        else if (Convert.ToString(Session["UserRole"]).ToUpper() == "ADMIN")
        {
            divlocation.Visible = true;
            divAssigne.Visible = true;
            divCategory.Visible = true;
            divDayWise.Visible = true;
            ddlorg.SelectedValue = Convert.ToString(Session["SD_OrgID"]);
            ddlorg.Enabled = false;

            ds = GetDataDashboard(ReqType, Category, frmDate, toDate, UserID, Convert.ToString(Session["SD_OrgID"]), SubmitterEmailId);
        }
        else if (Session["UserRole"].ToString().ToLower() == "technician")
        {
            divlocation.Visible = false;
            divAssigne.Visible = false;
            divCategory.Visible = true;
            divDayWise.Visible = true;
            ddlorg.SelectedValue = Convert.ToString(Session["SD_OrgID"]);
            ddlorg.Enabled = false;

            ds = GetDataDashboard(ReqType, Category, frmDate, toDate, Convert.ToString(Session["UserID"]), Convert.ToString(Session["SD_OrgID"]), SubmitterEmailId);
        }
        else
        {
            divlocation.Visible = false;
            divAssigne.Visible = false;
            divCategory.Visible = false;
            divDayWise.Visible = false;
            ddlorg.SelectedValue = Convert.ToString(Session["SD_OrgID"]);
            ddlorg.Enabled = false;

            ds = GetDataDashboard(ReqType, Category, frmDate, toDate, UserID, Convert.ToString(Session["SD_OrgID"]), Convert.ToString(Session["EmailID"]));
        }
        getTicketData(ds);
    }
    private void FillRequestType(long OrgId)
    {
        try
        {
            DataTable RequestType = new SDTemplateFileds().FillRequestType(OrgId);
            ddlRequestType.DataSource = RequestType;
            ddlRequestType.DataTextField = "ReqTypeRef";
            ddlRequestType.DataValueField = "id";
            ddlRequestType.DataBind();
            ddlRequestType.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--ALL--", "0"));
        }
        catch (ThreadAbortException e2)
        {
            Console.WriteLine("Exception message: {0}", e2.Message);
            Thread.ResetAbort();
        }
        catch (Exception ex)
        {
            // msg.ReportError(ex.Message);
        }
    }
    private void FillCategory(string OrgId, string ReqType)
    {
        try
        {
            DataTable RequestType = new SDTemplateFileds().FillCategoryAll(OrgId, ReqType);
            ddlCategory.DataSource = RequestType;
            ddlCategory.DataTextField = "CategoryCodeRef";
            ddlCategory.DataValueField = "ID";
            ddlCategory.DataBind();
            ddlCategory.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--ALL--", "0"));
        }
        catch (ThreadAbortException e2)
        {
            Console.WriteLine("Exception message: {0}", e2.Message);
            Thread.ResetAbort();
        }
        catch (Exception ex)
        {
            // msg.ReportError(ex.Message);
        }
    }
    public DataSet GetDataDashboard(string ReqType, string Category, string frmDate, string toDate,
        string UserID, string Orgid, string SubmitterEmail)
    {
        DataSet ds = new DataSet();
        ds = obj.getDashboardData(ReqType, Category, frmDate, toDate, UserID, Orgid, SubmitterEmail);
        return ds;
    }
    public void getTicketData(DataSet ds)
    {
        DataTable dt = ds.Tables[0];
        DataTable dt1 = ds.Tables[1];
        DataTable dt2 = ds.Tables[2];
        DataTable dt3 = ds.Tables[3];
        DataTable dt4 = ds.Tables[4];
        double Open = 0;
        double Hold = 0;
        double Wip = 0;
        double Closed = 0;
        double Resolved = 0;
        double OpenPer = 0;
        double HoldPer = 0;
        double WipPer = 0;
        double ClosedPer = 0;
        double ResolvedPer = 0;
        double TotalTicket = 0;
        double ServerityS1 = 0;
        double ServerityS2 = 0;
        double ServerityS3 = 0;
	double ServerityS4 = 0;
        double VeryHighPriority = 0;
        double HighPriority = 0;
        double MediumPriority = 0;
        double LowPriority = 0;

        double ApprovalPending = 0;
        double Approved = 0;
        double Rejected = 0;

        if (dt.Rows.Count > 0)
        {
            Open = Convert.ToDouble(dt.Rows[0]["OpenTickets"]);
            Hold = Convert.ToDouble(dt.Rows[0]["HoldTickets"]);
            Wip = Convert.ToDouble(dt.Rows[0]["WIPTickets"]);
            Closed = Convert.ToDouble(dt.Rows[0]["ClosedTickets"]);
            Resolved = Convert.ToDouble(dt.Rows[0]["ResolvedTickets"]);

            ApprovalPending = Convert.ToDouble(dt.Rows[0]["ApprovalPending"]);
            Approved = Convert.ToDouble(dt.Rows[0]["Approved"]);
            Rejected = Convert.ToDouble(dt.Rows[0]["Rejected"]);

            TotalTicket = Convert.ToDouble(dt.Rows[0]["TotalTickets"]);
            ServerityS1 = Convert.ToDouble(dt.Rows[0]["S1Tickets"]);
            ServerityS2 = Convert.ToDouble(dt.Rows[0]["S2Tickets"]);
            ServerityS3 = Convert.ToDouble(dt.Rows[0]["S3Tickets"]);
            ServerityS4 = Convert.ToDouble(dt.Rows[0]["S4Tickets"]);

            VeryHighPriority = Convert.ToDouble(dt.Rows[0]["VeryHighPriority"]);
            HighPriority = Convert.ToDouble(dt.Rows[0]["HighPriority"]);
            MediumPriority = Convert.ToDouble(dt.Rows[0]["MediumPriority"]);
            LowPriority = Convert.ToDouble(dt.Rows[0]["LowPriority"]);

            OpenPer = (Open / TotalTicket) * 100;
            HoldPer = (Hold / TotalTicket) * 100;
            WipPer = (Wip / TotalTicket) * 100;
            ClosedPer = (Closed / TotalTicket) * 100;
            ResolvedPer = (Resolved / TotalTicket) * 100;

            OpenPer = Math.Round(OpenPer, 2);
            HoldPer = Math.Round(HoldPer, 2);
            WipPer = Math.Round(WipPer, 2);
            ClosedPer = Math.Round(ClosedPer, 2);
            ResolvedPer = Math.Round(ResolvedPer, 2);
        }
        lbltot.Text = Convert.ToString(TotalTicket);
        lblOpen.Text = Convert.ToString(Open);
        lblHold.Text = Convert.ToString(Hold);
        lblWip.Text = Convert.ToString(Wip);
        lblClosed.Text = Convert.ToString(Closed);
        lblResolved.Text = Convert.ToString(Resolved);

        lblApprovalPending.Text = Convert.ToString(ApprovalPending);
        lblApproved.Text = Convert.ToString(Approved);
        lblRejected.Text = Convert.ToString(Rejected);

        lblOpenPer.Text = Convert.ToString(OpenPer + "%");
        lblHoldPer.Text = Convert.ToString(HoldPer + "%");
        lblWipPer.Text = Convert.ToString(WipPer + "%");
        lblClosedPer.Text = Convert.ToString(ClosedPer + "%");
        lblResolvedPer.Text = Convert.ToString(ResolvedPer + "%");

        hdnSeverityS1.Value = Convert.ToString(ServerityS1);
        hdnSeverityS2.Value = Convert.ToString(ServerityS2);
        hdnSeverityS3.Value = Convert.ToString(ServerityS3);
        hdnSeverityS4.Value = Convert.ToString(ServerityS4);

        hdnLowPriority.Value = Convert.ToString(LowPriority);
        hdnMediumPriority.Value = Convert.ToString(MediumPriority);
        hdnHighPriority.Value = Convert.ToString(HighPriority);
        hdnVeryHighPriority.Value = Convert.ToString(VeryHighPriority);

        if (dt1.Rows.Count > 0)
        {
            string chartOptionsScript = GenerateChartOptions(dt1);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "chartScript1", chartOptionsScript, true);
        }
        if (dt2.Rows.Count > 0)
        {
            string chartOptionsScript = GenerateChartLoaction(dt2);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "chartScript2", chartOptionsScript, true);
        }
        if (dt3.Rows.Count > 0)
        {
            string chartOptionsScript = GenerateChartCategory(dt3);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "chartScript3", chartOptionsScript, true);
        }
        if (dt4.Rows.Count > 0)
        {
            string chartOptionsScript = GenerateChartAssigne(dt4);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "chartScript4", chartOptionsScript, true);
        }
    }
    #region Day Wise Ticket Start
    public static string GenerateChartOptions(DataTable dt)
    {
        var chartData = new ChartData
        {
            DayWiseTotTicket = dt.AsEnumerable().Select(row => row.Field<DateTime>("TicketDate").ToString("yyyy-MM-dd")).ToArray(),
            DayTotalTickets = dt.AsEnumerable().Select(row => row.Field<int>("TotalTickets")).ToArray()
        };

        var serializer = new JavaScriptSerializer();
        string daywise = serializer.Serialize(chartData.DayWiseTotTicket);
        string totalTicketsJson = serializer.Serialize(chartData.DayTotalTickets);
        string chartOptions = $@"
var options = {{
    series: [{{
        name: 'Total Tickets',
        data: {totalTicketsJson}
    }}],
    chart: {{
        foreColor: '#9ba7b2',
        height: 250,
        type: 'bar',
        zoom: {{
            enabled: false
        }},
        toolbar: {{
            show: false,
        }}
    }},
    fill: {{
        type: 'gradient',
        gradient: {{
            shade: 'dark',
            gradientToColors: ['#00c6fb'],
            shadeIntensity: 1,
            type: 'vertical',
            stops: [0, 100, 100, 100]
        }},
    }},
    colors: ['#005bea'],
    plotOptions: {{
        bar: {{
            horizontal: false,
            borderRadius: 4,
            borderRadiusApplication: 'around',
            borderRadiusWhenStacked: 'last',
            columnWidth: '35%',
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
        strokeDashArray: 4,
    }},
    tooltip: {{
        theme: 'dark',
    }},
    xaxis: {{
        categories: {daywise},
 labels: {{
                    show: false  // Hide the x-axis labels
                }}
    }}
}};

var chart = new ApexCharts(document.querySelector('#chart3a'), options);
chart.render();
";

        return chartOptions;
    }
    #endregion Day Wise Ticket End

    #region CategoryWise Start
    public static string GenerateChartCategory(DataTable dt)
    {
        var chartData = new ChartData
        {
            Categories = dt.AsEnumerable().Select(row => row.Field<string>("Category")).ToArray(),
            CategTotalTickets = dt.AsEnumerable().Select(row => row.Field<int>("TotalTickets")).ToArray()
        };
        var serializer = new JavaScriptSerializer();
        string categoriesJson = serializer.Serialize(chartData.Categories);
        string totalTicketsJson = serializer.Serialize(chartData.CategTotalTickets);
        string chartOptions = $@"
 var options = {{
            series: [{{
                name: 'Total Tickets',
                data: {totalTicketsJson}

            }}],
            chart: {{
                foreColor: '#9ba7b2',
                height: 250,
                type: 'bar',
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
                    gradientToColors: ['#009efd'],
                    shadeIntensity: 1,
                    type: 'vertical',
                    //opacityFrom: 0.8,
                    //opacityTo: 0.1,
                    stops: [0, 100, 100, 100]
                }},
            }},
            colors: ['#2af598'],
            plotOptions: {{
                bar: {{
                    horizontal: false,
                    borderRadius: 4,
                    borderRadiusApplication: 'around',
                    borderRadiusWhenStacked: 'last',
                    columnWidth: '35%',
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
                strokeDashArray: 4,
            }},
            tooltip: {{
                theme: 'dark',
            }},
            xaxis: {{
                categories: {categoriesJson},
 labels: {{
                    show: false  // Hide the x-axis labels
                }}
            }}
        }};

        var chart = new ApexCharts(document.querySelector('#chart3'), options);
        chart.render();
";

        return chartOptions;
    }
    #endregion CategoryWise End

    #region LoactionWise Start
    public static string GenerateChartLoaction(DataTable dt)
    {
        var chartData = new ChartData
        {
            LocationName = dt.AsEnumerable().Select(row => row.Field<string>("LocName")).ToArray(),
            LocNameTotalTickets = dt.AsEnumerable().Select(row => row.Field<int>("TotalTickets")).ToArray()
        };
        var serializer = new JavaScriptSerializer();
        string loactionJson = serializer.Serialize(chartData.LocationName);
        string totalTicketsJson = serializer.Serialize(chartData.LocNameTotalTickets);
        string chartOptions = $@"
 var options = {{
            series: [{{
                name: 'Total Tickets',
                data: {totalTicketsJson}

            }}],
            chart: {{
                foreColor: '#9ba7b2',
                height: 250,
                type: 'bar',
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
                    gradientToColors: ['#7928ca'],
                    shadeIntensity: 1,
                    type: 'vertical',
                    //opacityFrom: 0.8,
                    //opacityTo: 0.1,
                    stops: [0, 100, 100, 100]
                }},
            }},
            colors: ['#ff0080'],
            plotOptions: {{
                bar: {{
                    horizontal: false,
                    borderRadius: 4,
                    borderRadiusApplication: 'around',
                    borderRadiusWhenStacked: 'last',
                    columnWidth: '35%',
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
                strokeDashArray: 4,
            }},
            tooltip: {{
                theme: 'dark',
            }},
            xaxis: {{
                categories: {loactionJson},
                labels: {{
                    show: false  // Hide the x-axis labels
                }}
            }}
        }};

        var chart = new ApexCharts(document.querySelector('#chart3b'), options);
        chart.render();
";

        return chartOptions;
    }
    #endregion LoactionWise End

    #region AssigneWise Start
    public static string GenerateChartAssigne(DataTable dt)
    {
        var chartData = new ChartData
        {
            AssigneWise = dt.AsEnumerable().Select(row => row.Field<string>("Technician")).ToArray(),
            AssigneTotalTickets = dt.AsEnumerable().Select(row => row.Field<int>("TotalTickets")).ToArray()
        };
        var serializer = new JavaScriptSerializer();
        string AssigneJson = serializer.Serialize(chartData.AssigneWise);
        string totalTicketsJson = serializer.Serialize(chartData.AssigneTotalTickets);
        string chartOptions = $@"
 var options = {{
            series: [{{
                name: 'Total Tickets',
                data: {totalTicketsJson}

            }}],
            chart: {{
                foreColor: '#9ba7b2',
                height: 250,
                type: 'bar',
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
                    gradientToColors: ['#ffd200'],
                    shadeIntensity: 1,
                    type: 'vertical',
                    //opacityFrom: 0.8,
                    //opacityTo: 0.1,
                    stops: [0, 100, 100, 100]
                }},
            }},
            colors: ['#ff6a00'],
            plotOptions: {{
                bar: {{
                    horizontal: false,
                    borderRadius: 4,
                    borderRadiusApplication: 'around',
                    borderRadiusWhenStacked: 'last',
                    columnWidth: '35%',
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
                strokeDashArray: 4,
            }},
            tooltip: {{
                theme: 'dark',
            }},
            xaxis: {{
                categories: {AssigneJson},
 labels: {{
                    show: false  // Hide the x-axis labels
                }}

            }}
        }};

        var chart = new ApexCharts(document.querySelector('#chart3c'), options);
        chart.render();
";

        return chartOptions;
    }
    #endregion AssigneWise End

    public class ChartData
    {
        public string[] DayWiseTotTicket { get; set; }
        public int[] DayTotalTickets { get; set; }
        public string[] Categories { get; set; }
        public int[] CategTotalTickets { get; set; }
        public string[] LocationName { get; set; }
        public int[] LocNameTotalTickets { get; set; }
        public string[] AssigneWise { get; set; }
        public int[] AssigneTotalTickets { get; set; }
    }

    protected void btnGo_Click(object sender, EventArgs e)
    {
        try
        {
            string formattedFromDate = txtfrmDate.Text;
            //DateTime frmdate = DateTime.ParseExact(fromd, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
           // string formattedFromDate = frmdate.ToString("yyyy-MM-dd");
            string formattedToDate = txttoDate.Text;
            //DateTime todate = DateTime.ParseExact(to, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            //string formattedToDate = todate.ToString("yyyy-MM-dd");
            string category = ddlCategory.SelectedValue;
            if (Convert.ToDateTime(formattedFromDate) > Convert.ToDateTime(formattedToDate))
            {
                return;
            }
            if (Convert.ToDateTime(formattedToDate) > Convert.ToDateTime(DateTime.Now))
            {
                return;
            }
            if (string.IsNullOrEmpty(category))
            {
                category = "0";
            }
            string RequestType = "0";
            if (ddlRequestType.SelectedValue != "")
            {
                RequestType = ddlRequestType.SelectedValue;
            }
            getFilterData(RequestType, category, formattedFromDate, formattedToDate, "0", ddlorg.SelectedValue, "");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void ddlRequestType_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (ddlRequestType.SelectedItem.Text.ToLower() == "incident")
            {
                divReqType.Visible = false;
            }
            else
            {
                divReqType.Visible = true;
            }
            if (ddlRequestType.SelectedValue != "0")
            {
                FillCategory(ddlorg.SelectedValue, Convert.ToString(ddlRequestType.SelectedItem.Text));
                ddlCategory.Enabled = true;
            }
            else
            {
                ddlCategory.SelectedValue = "0";
                ddlCategory.Enabled = false;
            }
            getFilterData("0", "0", Convert.ToString("1900-01-01"), Convert.ToString("1900-01-01"), "0", "", "");
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private void FillOrganization()
    {

        try
        {
            DataTable SD_Org = new FillSDFields().FillOrganization();
            ddlorg.DataSource = SD_Org;
            ddlorg.DataTextField = "OrgName";
            ddlorg.DataValueField = "Org_ID";
            ddlorg.DataBind();
            ddlorg.Items.Insert(0, new System.Web.UI.WebControls.ListItem(" All Organization", ""));


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
    protected void ddlorg_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlorg.SelectedValue != "")
        {
            FillRequestType(Convert.ToInt64(ddlorg.SelectedValue));
            ddlRequestType.Enabled = true;
        }
        else
        {
            ddlRequestType.SelectedValue = "0";
            ddlRequestType.Enabled = false;
        }
    }

    protected void lnkBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/frmUserlanding.aspx");
    }
}
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmUserDashboard.aspx.cs" Inherits="Dashboard_frmUserDashboard" %>

<!DOCTYPE html>

<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="icon" type="image/png" href="AgentTicketCSS/images/icons/favicon.ico" />
    <%--<html xmlns="http://www.w3.org/1999/xhtml">--%>
    <link href="https://pcv-demo.hitachi-systems-mc.com:5723/assetsdata/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Noto+Sans:wght@300;400;500;600&amp;display=swap" rel="stylesheet">
    <link href="https://fonts.googleapis.com/css?family=Material+Icons+Outlined" rel="stylesheet">
    <link href="https://pcv-demo.hitachi-systems-mc.com:5723/sassdata/main.css" rel="stylesheet">
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet">

    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css">
    <link href="https://pcv-demo.hitachi-systems-mc.com:5723/assetsdata/plugins/select2/css/select2-bootstrap-5-theme.min.css" rel="stylesheet" />

    <link rel="stylesheet" href="<%= ResolveUrl("~/Asset/css/bootstrapv5.min.css") %>">
    <link rel="stylesheet" href="<%= ResolveUrl("~/assets/fontawesome/css/fontawesome.css") %>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/assets/fontawesome/css/brands.css") %>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/assets/fontawesome/css/solid.css") %>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/assets/fontawesome/css/sharp-thin.css") %>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/assets/fontawesome/css/duotone-thin.css") %>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/assets/fontawesome/css/sharp-duotone-thin.css") %>" />

    <style>
        .select2-container--bootstrap-5 .select2-selection--single .select2-selection__rendered {
            white-space: nowrap !important;
            overflow: hidden !important;
            text-overflow: ellipsis !important;
            max-width: 100px !important;
            display: block !important;
        }

        .back-button {
            position: fixed;
            top: 20px;
            right: -1px;
            background-color: #007bff;
            color: white;
            border: none;
            padding: 10px 15px;
            font-size: 13px;
            cursor: pointer;
            z-index: 1000;
            border-radius: 2rem 0 0 2rem;
        }

            .back-button:hover {
                background-color: #0056b3;
            }
    </style>
</head>

<body style="background-image: url(../Asset/img/bg005.jpg); height: auto; background-size: cover; background-repeat: round;">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
            <ContentTemplate>
                <div class="container-fluid mt-3">
                    <asp:LinkButton ID="lnkBack" class="back-button" runat="server" OnClick="lnkBack_Click"> <i class="fas fa-arrow-left"></i></asp:LinkButton>
                    <div class="row g-2">
                        <div class="col-md-12 d-flex align-items-stretch">
                            <div class="card w-100 overflow-hidden rounded-4 mb-2">
                                <div class="card-body position-relative p-4">
                                    <div class="row g-2">
                                        <div class="d-flex  gap-3 mb-4 align-content-between">
                                            <%--<asp:Image ID="img" runat="server" class="rounded-circle bg-grd-info p-1" width="60" height="60" alt="user"/>--%>
                                            <asp:Image ID="img" runat="server" class="rounded-circle p-1 border" Width="45" Height="45" alt="" />
                                            <div class="col-md-3">
                                                <p class="mb-0 fw-semibold">Welcome back</p>
                                                <h4 class="fw-semibold mb-0 fs-4 mb-0">
                                                    <asp:Label ID="lblUserName" runat="server"></asp:Label></h4>
                                            </div>
                                            <div class="col-md-8 text-end">
                                                <p class="text-muted small">
                                                    (Default data from the last 7 days since today)
                                                </p>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row mb-5 gx-3 gy-1">
                                        <div class="col-md-3 col-6">
                                            <label class="form-label ">Organization</label>
                                            <asp:DropDownList ID="ddlorg" runat="server" ToolTip="Select Desk Type" CssClass="form-select form-select-sm single-select-optgroup-field " AutoPostBack="true" OnSelectedIndexChanged="ddlorg_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-2 col-6">
                                            <label class="form-label ">Request Type</label>
                                            <asp:DropDownList ID="ddlRequestType" runat="server" ToolTip="Select Desk Type" CssClass="form-select form-select-sm single-select-optgroup-field " AutoPostBack="true" OnSelectedIndexChanged="ddlRequestType_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-2 col-6">
                                            <label class="form-label ">Category</label>
                                            <asp:DropDownList ID="ddlCategory" runat="server" ToolTip="Select Desk Type" CssClass="form-select form-select-sm single-select-optgroup-field description-label">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-2 col-5">
                                            <label class="form-label">
                                                From Date
                                        <asp:RequiredFieldValidator ID="rfvtxtCntnctPrnsName" runat="server" CssClass="form-label " ControlToValidate="txtfrmDate" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="GO"></asp:RequiredFieldValidator><br />
                                            </label>
                                            <asp:TextBox AutoCompleteType="None" type="date" runat="server" Placeholder="" ID="txtfrmDate" class="form-control form-control-sm datepicker"></asp:TextBox>
                                        </div>
                                        <div class="col-md-2 col-5">
                                            <label class="form-label">
                                                To Date
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="form-label" ControlToValidate="txttoDate" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="GO"></asp:RequiredFieldValidator><br />
                                            </label>
                                            <asp:TextBox AutoCompleteType="None" type="date" runat="server" Placeholder="" ID="txttoDate" class="form-control form-control-sm datepicker"></asp:TextBox>
                                        </div>
                                        <div class="col-md-1 col-2">
                                            <label class="form-label opacity-0">kk</label>
                                            <br />
                                            <asp:Button ID="btnGo" runat="server" Text="GO" CssClass="btn btn-sm  btn-grd btn-primary px-3" OnClick="btnGo_Click" ValidationGroup="GO" OnClientClick="validateDates()" />
                                        </div>
                                    </div>

                                    <div class="d-flex align-items-center gap-5">
                                        <div class="d-flex  align-items-center gap-4">
                                            <h5 class="mb-1 fw-semibold d-flex align-content-center">
                                                <asp:Label ID="lbltot" runat="server"></asp:Label><i class="ti ti-arrow-up-right fs-5 lh-base text-success"></i>
                                            </h5>
                                            <p class="mb-1 small">Total Tickets</p>

                                        </div>
                                        <div class="vr"></div>
                                        <div class="d-flex  align-items-center gap-4">
                                            <h5 class="mb-1 fw-semibold d-flex align-content-center">
                                                <asp:Label ID="lblOpen" runat="server"></asp:Label><i class="ti ti-arrow-up-right fs-5 lh-base text-success"></i>
                                            </h5>
                                            <p class="mb-1 small">Open Tickets</p>

                                        </div>
                                        <div class="vr"></div>
                                        <div class="d-flex  align-items-center gap-4">
                                            <h5 class="mb-1 fw-semibold d-flex align-content-center">
                                                <asp:Label ID="lblHold" runat="server"></asp:Label><i class="ti ti-arrow-up-right fs-5 lh-base text-success"></i>
                                            </h5>
                                            <p class="mb-1 small">Hold Tickets</p>

                                        </div>
                                        <div class="vr"></div>
                                        <div class="d-flex  align-items-center gap-4">
                                            <h5 class="mb-1 fw-semibold d-flex align-content-center">
                                                <asp:Label ID="lblWip" runat="server"></asp:Label><i class="ti ti-arrow-up-right fs-5 lh-base text-success"></i>
                                            </h5>
                                            <p class="mb-1 small">WIP Tickets</p>

                                        </div>
                                        <div class="vr"></div>
                                        <div class="d-flex  align-items-center gap-4">
                                            <h5 class="mb-1 fw-semibold d-flex align-content-center">
                                                <asp:Label ID="lblClosed" runat="server"></asp:Label><i class="ti ti-arrow-up-right fs-5 lh-base text-success"></i>
                                            </h5>
                                            <p class="mb-1 small">Closed Tickets</p>

                                        </div>
                                        <div class="vr"></div>
                                        <div class="d-flex  align-items-center gap-4">
                                            <h5 class="mb-1 fw-semibold d-flex align-content-center">
                                                <asp:Label ID="lblResolved" runat="server"></asp:Label><i class="ti ti-arrow-up-right fs-5 lh-base text-success"></i>
                                            </h5>
                                            <p class="mb-1 small">Resolved</p>

                                        </div>
                                        <div runat="server" id="divReqType" visible="false" class="d-flex gap-3">
                                            <div class="vr"></div>
                                            <div class="d-flex  align-items-center gap-2">
                                                <h5 class="mb-1 fw-semibold d-flex align-content-center">
                                                    <asp:Label ID="lblApprovalPending" runat="server"></asp:Label><i class="ti ti-arrow-up-right fs-5 lh-base text-success"></i>
                                                </h5>
                                                <p class="mb-1 small">Approval Pending</p>
                                            </div>
                                            <div class="vr"></div>
                                            <div class="d-flex  align-items-center gap-2">
                                                <h5 class="mb-1 fw-semibold d-flex align-content-center">
                                                    <asp:Label ID="lblApproved" runat="server"></asp:Label><i class="ti ti-arrow-up-right fs-5 lh-base text-success"></i>
                                                </h5>
                                                <p class="mb-1 small">Approved</p>
                                            </div>
                                            <div class="vr"></div>
                                            <div class="d-flex  align-items-center gap-2">
                                                <h5 class="mb-1 fw-semibold d-flex align-content-center">
                                                    <asp:Label ID="lblRejected" runat="server"></asp:Label><i class="ti ti-arrow-up-right fs-5 lh-base text-success"></i>
                                                </h5>
                                                <p class="mb-1 small">Rejected</p>
                                            </div>
                                        </div>
                                    </div>
                                    <%--  <div class="col-12 col-sm-2 opacity-25">
                                <div class="welcome-back-img pt-4">
                                    <img src="assets/images/gallery/welcome-back-3.png" height="160" alt="">
                                </div>
                            </div>--%>
                                </div>
                                <!--end row-->
                            </div>
                        </div>
                    </div>
                    <div class="row g-2">
                        <div class="col-md-4">
                            <div class="card w-100 rounded-4 mb-2 ">
                                <div class="card-body">
                                    <div class="d-flex align-items-start justify-content-between ">
                                        <div class="">
                                            <h6 class="mb-0">Ticket by Status</h6>
                                        </div>
                                    </div>
                                    <div id="chart5"></div>
                                    <div class="d-flex flex-column gap-3 mx-3 my-4">
                                        <div class="d-flex align-items-center justify-content-between">
                                            <p class="mb-0 d-flex align-items-center gap-2 w-25">Open</p>
                                            <div class="">
                                                <p class="mb-0">
                                                    <asp:Label ID="lblOpenPer" runat="server"></asp:Label>
                                                </p>
                                            </div>
                                        </div>
                                        <div class="d-flex align-items-center justify-content-between">
                                            <p class="mb-0 d-flex align-items-center gap-2 w-25">Hold</p>
                                            <div class="">
                                                <p class="mb-0">
                                                    <asp:Label ID="lblHoldPer" runat="server"></asp:Label>
                                                </p>
                                            </div>
                                        </div>
                                        <div class="d-flex align-items-center justify-content-between">
                                            <p class="mb-0 d-flex align-items-center gap-2 w-25">WIP</p>
                                            <div class="">
                                                <p class="mb-0">
                                                    <asp:Label ID="lblWipPer" runat="server"></asp:Label>
                                                </p>
                                            </div>
                                        </div>
                                        <div class="d-flex align-items-center justify-content-between">
                                            <p class="mb-0 d-flex align-items-center gap-2 w-25">Closed</p>
                                            <div class="">
                                                <p class="mb-0">
                                                    <asp:Label ID="lblClosedPer" runat="server"></asp:Label>
                                                </p>
                                            </div>
                                        </div>
                                        <div class="d-flex align-items-center justify-content-between">
                                            <p class="mb-0 d-flex align-items-center gap-2 w-25">Resolved</p>
                                            <div class="">
                                                <p class="mb-0">
                                                    <asp:Label ID="lblResolvedPer" runat="server"></asp:Label>
                                                </p>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-8">
                            <div class="row g-3">
                                <div class="col-md-12">

                                    <div class="card w-100 rounded-4 mb-0">
                                        <div class="card-body">
                                            <div class="d-flex align-items-start justify-content-between mb-3">
                                                <div class="">
                                                    <h6 class="mb-0">Details by Severity</h6>
                                                </div>

                                            </div>
                                            <div id="chart1"></div>
                                        </div>
                                    </div>

                                </div>
                                <div class="col-md-12 mt-1">

                                    <div class="card w-100 rounded-4 mb-0">
                                        <div class="card-body">
                                            <div class="d-flex align-items-start justify-content-between mb-3">
                                                <div class="">
                                                    <h6 class="mb-0">Details by Priority</h6>
                                                </div>

                                            </div>
                                            <div id="chart2"></div>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 mt-0" id="divCategory" runat="server">
                            <div class="card rounded-4 mb-2">
                                <div class="card-body">
                                    <div class="d-flex align-items-center justify-content-between">
                                        <h6 class="mb-0">Tickets by Category</h6>
                                    </div>
                                    <div id="chart3"></div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 mt-0" id="divDayWise" runat="server">
                            <div class="card rounded-4 mb-2">
                                <div class="card-body">
                                    <div class="d-flex align-items-center justify-content-between">
                                        <h6 class="mb-0">Day-Wise Ticket Generation </h6>
                                    </div>
                                    <div id="chart3a"></div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 mt-0" id="divlocation" runat="server">
                            <div class="card rounded-4 mb-2">
                                <div class="card-body">
                                    <div class="d-flex align-items-center justify-content-between">
                                        <h6 class="mb-0">Location Wise Ticket Generation </h6>
                                    </div>
                                    <div id="chart3b"></div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6 mt-0" id="divAssigne" runat="server">
                            <div class="card rounded-4 mb-4">
                                <div class="card-body">
                                    <div class="d-flex align-items-center justify-content-between">
                                        <h6 class="mb-0">Assignee Wise Ticket Allocations </h6>
                                    </div>
                                    <div id="chart3c"></div>
                                </div>
                            </div>
                        </div>
                        <asp:HiddenField ID="hdnSeverityS1" runat="server" />
                	<asp:HiddenField ID="hdnSeverityS2" runat="server" />
                	<asp:HiddenField ID="hdnSeverityS3" runat="server" />
                	<asp:HiddenField ID="hdnSeverityS4" runat="server" />

                	<asp:HiddenField ID="hdnLowPriority" runat="server" />
                	<asp:HiddenField ID="hdnMediumPriority" runat="server" />
                	<asp:HiddenField ID="hdnHighPriority" runat="server" />
                	<asp:HiddenField ID="hdnVeryHighPriority" runat="server" />


                        <asp:HiddenField ID="hdncategories" runat="server" />
                        <asp:HiddenField ID="hdntotalTickets" runat="server" />

                    </div>
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnGo" />
                <asp:PostBackTrigger ControlID="ddlRequestType" />
                <asp:PostBackTrigger ControlID="ddlorg" />
            </Triggers>
        </asp:UpdatePanel>
        <!--end row-->
         <script src="https://pcv-demo.hitachi-systems-mc.com:5723/assetsdata/js/jquery-3.6.0.min.js"></script>
        <script src="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/js/select2.min.js"></script>
        <script src="https://pcv-demo.hitachi-systems-mc.com:5723/assetsdata/plugins/select2/js/select2-custom.js"></script>
        <script src="https://pcv-demo.hitachi-systems-mc.com:5723/assetsdata/plugins/apexchart/apexcharts.min.js"></script>
        

        <script>

            $(function () {
                "use strict";
                var s1 = parseFloat(document.getElementById('<%= hdnSeverityS1.ClientID %>').value);
                var s2 = parseFloat(document.getElementById('<%= hdnSeverityS2.ClientID %>').value);
                var s3 = parseFloat(document.getElementById('<%= hdnSeverityS3.ClientID %>').value);
	        var s4 = parseFloat(document.getElementById('<%= hdnSeverityS4.ClientID %>').value);

                // Details by Severity

                var options = {
                    series: [{
                        name: "Total Tickets",
                        /*data: [4, 10, 6]*/
                        data: [s1, s2, s3,s4]
                    }],
                    chart: {
                        foreColor: "#9ba7b2",
                        height: 180,
                        type: 'area',
                        zoom: {
                            enabled: false
                        },
                        toolbar: {
                            show: !1,
                        },
                    },
                    dataLabels: {
                        enabled: false
                    },
                    stroke: {
                        width: 4,
                        curve: 'smooth'
                    },
                    fill: {
                        type: 'gradient',
                        gradient: {
                            shade: 'dark',
                            gradientToColors: ['#ff0080'],
                            shadeIntensity: 1,
                            type: 'vertical',
                            opacityFrom: 0.8,
                            opacityTo: 0.1,
                            stops: [0, 100, 100, 100]
                        },
                    },
                    colors: ["#ffd200"],
                    grid: {
                        show: true,
                        borderColor: 'rgba(0, 0, 0, 0.15)',
                        strokeDashArray: 4,
                    },
                    tooltip: {
                        theme: "dark",
                    },
                    xaxis: {
                        categories: ['Severity 1', 'Severity 2', 'Severity 3', 'Severity 4'],
                    },
                    markers: {
                        show: !1,
                        size: 5,
                    },
                };

                var chart = new ApexCharts(document.querySelector("#chart1"), options);
                chart.render();




                // Details by Priority
                var LowPriority = parseFloat(document.getElementById('<%= hdnLowPriority.ClientID %>').value);
                var MediumPriority = parseFloat(document.getElementById('<%= hdnMediumPriority.ClientID %>').value);
                var HighPriority = parseFloat(document.getElementById('<%= hdnHighPriority.ClientID %>').value);
                var VeryHighPriority = parseFloat(document.getElementById('<%= hdnVeryHighPriority.ClientID %>').value);
                var options = {
                    series: [{
                        name: "Total Tickets",
                        //data: [10, 41, 35, 51, 49, 82, 69, 91, 18],
                        data: [VeryHighPriority, HighPriority,MediumPriority, LowPriority]
                    }],
                    chart: {
                        foreColor: "#9ba7b2",
                        height: 180,
                        type: 'line',
                        zoom: {
                            enabled: false
                        },
                        toolbar: {
                            show: !1,
                        },
                        dropShadow: {
                            enabled: !0,
                            top: 3,
                            left: 14,
                            blur: 4,
                            opacity: .12,
                            color: "#fc185a"
                        },
                    },
                    dataLabels: {
                        enabled: false
                    },
                    stroke: {
                        curve: 'smooth'
                    },
                    fill: {
                        type: 'gradient',
                        gradient: {
                            shade: 'dark',
                            gradientToColors: ['#7928ca'],
                            shadeIntensity: 1,
                            type: 'vertical',
                            opacityFrom: 1,
                            opacityTo: 1,
                            // stops: [0, 100, 100, 100]
                        },
                    },
                    colors: ["#ff0080"],
                    grid: {
                        show: true,
                        borderColor: 'rgba(0, 0, 0, 0.15)',
                        strokeDashArray: 4,
                    },
                    tooltip: {
                        theme: "dark",
                    },
                    xaxis: {
                        categories: ['Very High','High','Medium', 'Low'],
                    }
                };

                var chart = new ApexCharts(document.querySelector("#chart2"), options);
                chart.render();

                var openPercent = parseFloat(document.getElementById('<%= lblOpenPer.ClientID %>').innerText);
                var holdPercent = parseFloat(document.getElementById('<%= lblHoldPer.ClientID %>').innerText);
                var wipPercent = parseFloat(document.getElementById('<%= lblWipPer.ClientID %>').innerText);
                var closedPercent = parseFloat(document.getElementById('<%= lblClosedPer.ClientID %>').innerText);
                var resolvedPercent = parseFloat(document.getElementById('<%= lblResolvedPer.ClientID %>').innerText);

                // Ticket by Status
                var options = {
                    series: [closedPercent, resolvedPercent, openPercent, wipPercent, holdPercent],
                    chart: {
                        height: 293,
                        type: 'donut',
                    },
                    legend: {
                        position: 'bottom',
                        show: !1
                    },
                    labels: ['Closed', 'Resolved', 'Open', 'WIP', 'Hold'],
                    fill: {
                        type: 'gradient',
                        gradient: {
                            shade: 'dark',
                            gradientToColors: ['red', 'yellow', 'green', 'orange', ' #009efd'],
                            shadeIntensity: 1,
                            type: 'vertical',
                            opacityFrom: 1,
                            opacityTo: 1,
                            //stops: [0, 100, 100, 100]
                        },
                    },
                    colors: ["red", "yellow", "green", "orange", "#2af598"],
                    dataLabels: {
                        enabled: !1
                    },
                    plotOptions: {
                        pie: {
                            donut: {
                                size: "85%"
                            }
                        }
                    },
                    tooltip: {
                        y: {
                            formatter: function (val) {
                                return val + "%";
                            }
                        }
                    },
                    responsive: [{
                        breakpoint: 480,
                        options: {
                            chart: {
                                height: 270
                            },
                            legend: {
                                position: 'bottom',
                                show: !1
                            }
                        }
                    }]
                };

                var chart = new ApexCharts(document.querySelector("#chart5"), options);
                chart.render();
            });

        </script>

        <script>
            function validateDates() {
                var fromDate = document.getElementById('<%= txtfrmDate.ClientID %>').value;
                var toDate = document.getElementById('<%= txttoDate.ClientID %>').value;
                var parsedFromDate = parseDate(fromDate);
                var parsedToDate = parseDate(toDate);
                var today = new Date();
                today.setHours(0, 0, 0, 0);
                if (parsedFromDate > parsedToDate) {
                    showNotification("From Date Cannot be Greater Than To Date!");
                    return false;
                }

                if (parsedToDate > today) {
                    showNotification("To Date Cannot be Greater Than Today's Date!");
                    return false;
                }

                return true;
            }
            function parseDate(dateString) {
                var parts = dateString.split("-");
                return new Date(parts[2], parts[1] - 1, parts[0]);
            }
            function showNotification(message) {
                alert(message); // Using the built-in alert for simplicity
                setTimeout(function () { window.location.reload(); }, 2000); // Reloads after 2 seconds
            }
        </script>
        
    </form>
</body>
</html>

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmDBDayWise.aspx.cs" Inherits="Dashboard_frmDBDayWise" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
  <div class="card mb-1">
       
                    <div class="card-body">
                        <%--<center>Ticket Summary</center>--%>

                        <div class="d-flex align-items-center justify-content-around">
                                <div class="d-flex  align-items-center gap-3">
                                    <h4 class="mb-1 fw-semibold d-flex align-content-center">
                                       <asp:LinkButton ID="lblTodayTickets" runat="server" Text="0" Font-Size="X-Large" CssClass="lblh h4 mb-0" OnClick="lblTodayTickets_Click"></asp:LinkButton>
                                    </h4>
                                    <p class="mb-1 small">Logged Today</p>

                                </div>
                                <div class="vr"></div>
                                <div class="d-flex  align-items-center gap-3">
                                    <h4 class="mb-1 fw-semibold d-flex align-content-center">
                                            <asp:LinkButton ID="lbl7DaysTickets" runat="server" Text="0" Font-Size="X-Large" CssClass="lblh h4 mb-0" OnClick="lbl7DaysTickets_Click"></asp:LinkButton>
                                    </h4>
                                    <p class="mb-1 small">Logged last 7 days

</p>

                                </div>
                                <div class="vr"></div>
                                <div class="d-flex  align-items-center gap-3">
                                    <h4 class="mb-1 fw-semibold d-flex align-content-center">
                                            <asp:LinkButton ID="lbl30DaysTickets" runat="server" Font-Size="X-Large" Text="0" CssClass="h4 mb-0" OnClick="lbl30DaysTickets_Click"></asp:LinkButton>
                                    </h4>
                                    <p class="mb-1 small">Logged 30 days

</p>

                                </div>
                                <div class="vr"></div>
                                <div class="d-flex  align-items-center gap-3">
                                    <h4 class="mb-1 fw-semibold d-flex align-content-center">
                                            <asp:LinkButton ID="lblTotalTickets" runat="server"  CssClass="h4 mb-0" Font-Size="X-Large" Text="0" OnClick="lblTotalTickets_Click"></asp:LinkButton>
                                    </h4>
                                    <p class="mb-1 small">Total Tickets</p>

                                </div>
                               
                                
                            </div>

                        <div class="row mt-3 d-none">

                            <div class="col-xl-3 col-sm-6 mb-xl-0 mb-4">
                                <div class="card">
                                    <div class="card-header p-3 " style="border-bottom: none">
                                        <div class="icon icon-lg icon-shape  text-center border-radius-xl  position-absolute">
                                            <span class="material-icons opacity-10 ">
                                                <img src="../Images/demo/calendar.png" /></span>
                                        </div>
                                        <div class="text-end pt-1">
                                        </div>
                                    </div>
                                    <div class="card-footer mt-2 ">
                                        <p class="mb-0 "><span class="text-dark fa-pull-right text-md ml-3 font-weight-bolder">Logged today</span></p>
                                        <asp:ImageButton ID="imgbtnTodayTickets" runat="server" Visible="false" ImageUrl="~/images/getdetails.png" OnClick="imgbtnTodayTickets_Click" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-xl-3 col-sm-6 mb-xl-0 mb-4">
                                <div class="card">
                                    <div class="card-header p-3 " style="border-bottom: none">
                                        <div class="icon icon-lg icon-shape  text-center border-radius-xl  position-absolute">

                                            <span class="material-icons opacity-10 ">
                                                <img src="../Images/demo/number-7.png" /></span>
                                        </div>
                                        <div class="text-end pt-1">
                                        </div>
                                    </div>
                                    <div class="card-footer mt-2 ">
                                        <p class="mb-0"><span class="text-dark fa-pull-right text-md ml-3 font-weight-bolder">Logged last 7 days</span></p>
                                        <asp:ImageButton ID="imgbtn7DaysTickets" Visible="false" runat="server" ImageUrl="~/images/getdetails.png" OnClick="imgbtn7DaysTickets_Click" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-xl-3 col-sm-6 mb-xl-0 mb-4">
                                <div class="card">
                                    <div class="card-header p-3 " style="border-bottom: none">
                                        <div class="icon icon-lg icon-shape  text-center border-radius-xl  position-absolute">
                                            <span class="material-icons opacity-10 ">
                                                <img src="../Images/demo/30-days.png" /></span>
                                        </div>
                                        <div class="text-end pt-1">
                                        </div>
                                    </div>
                                    <div class="card-footer mt-2 ">
                                        <p class="mb-0"><span class="text-dark  text-md fa-pull-right ml-5 font-weight-bolder">Logged 30 days</span></p>
                                        <asp:ImageButton ID="imgbtn30DaysTickets" Visible="false" runat="server" ImageUrl="~/images/getdetails.png" OnClick="imgbtn30DaysTickets_Click" />
                                    </div>
                                </div>
                            </div>
                            <div class="col-xl-3 col-sm-6 mb-xl-0 mb-4">
                                <div class="card">
                                    <div class="card-header p-3 " style="border-bottom: none">
                                        <div class="icon icon-lg icon-shape  text-center border-radius-xl  position-absolute">
                                            <%--  <i class="material-icons opacity-10">weekend</i>--%>
                                            <span class="material-icons opacity-10  ml-2">
                                                <img src="../Images/demo/all.png" /></span>
                                        </div>
                                        <div class="text-end pt-1">
                                        </div>
                                    </div>
                                    <%--  <hr class="dark horizontal my-0">--%>
                                    <div class="card-footer mt-2 ">
                                        <p class="mb-0"><span class="text-dark fa-pull-right text-md ml-5 font-weight-bolder">Total Tickets  </span></p>
                                        <asp:ImageButton ID="imgbtnTotalTickets" runat="server" Visible="false" ImageUrl="~/images/getdetails.png" OnClick="imgbtnTotalTickets_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
              </div>
            <div class="clearfix"></div>
      
        <div class="row g-2 ">
            <div class="col-md-6 " id="divCategory" runat="server">
                <div class="card  mb-1">
                    <div class="card-header">

                    <h6 class="mb-0 card-tittle">Last 7 Days Tickets Status Details</h6>
                    </div>
                    <div class="card-body">
                        <div class="d-flex align-items-center justify-content-between">
                            <asp:Button ID="btnChart7DaysTickets" runat="server" OnClick="btnChart7DaysTickets_Click" hidden />
                        </div>
                        <div id="chart1"></div>
                    </div>
                </div>
            </div>

            <div class="col-md-6 " id="div1" runat="server">
                <div class="card  mb-1">
                      <div class="card-header">

  <h6 class="mb-0 card-tittle">Today Tickets Status Details</h6>
  </div>
                    
                    <div class="card-body">
                        <div class="d-flex align-items-center justify-content-between">
                            <asp:Button ID="btnChartTodayTickets" runat="server" OnClick="btnChartTodayTickets_Click" hidden />
                        </div>
                        <div id="chart2"></div>
                    </div>
                </div>
            </div>

            <div class="col-md-6 mt-0" id="div2" runat="server">
                <div class="card  mb-1">
                                        <div class="card-header">

<h6 class="mb-0 card-tittle">Last 30 Days Tickets Status</h6>
</div>
                   
                    <div class="card-body">
                        <div class="d-flex align-items-center justify-content-between">
                            <asp:Button ID="btnChart30DaysTickets" runat="server" OnClick="btnChart30DaysTickets_Click" hidden />
                        </div>
                        <div id="chart3"></div>
                    </div>
                </div>
            </div>

            <div class="col-md-6 mt-0" id="div3" runat="server">
                <div class="card  mb-1">
                                                            <div class="card-header">

<h6 class="mb-0 card-tittle">Total Tickets Status Details</h6>
</div>
                  
                    <div class="card-body">
                        <div class="d-flex align-items-center justify-content-between">
                            <asp:Button ID="btnChartTotalTicketDetails" runat="server" OnClick="btnChartTotalTicketDetails_Click" hidden />
                        </div>
                        <div id="chart4"></div>
                    </div>
                </div>
            </div>

        </div>

 
    <asp:Literal ID="ltrChartTodayTickets" runat="server"></asp:Literal>
    <asp:Literal ID="ltrChart7DaysTickets" runat="server"></asp:Literal>
    <asp:Literal ID="ltrChart30DaysTickets" runat="server"></asp:Literal>
    <asp:Literal ID="ltrChartTotalTicketDetails" runat="server"></asp:Literal>
    <asp:Literal ID="ltrChartResponseSLA" runat="server"></asp:Literal>
    <asp:Literal ID="ltrAllSeverityWiseCalls" runat="server"></asp:Literal>
    <asp:Literal ID="ltrSeverityWiseResponseSLAPie" runat="server"></asp:Literal>
    <asp:Literal ID="ltrResolutionSLAPie" runat="server"></asp:Literal>
    <asp:Literal ID="ltrAllSeverityWiseResolveCalls" runat="server"></asp:Literal>
    <asp:Literal ID="ltrSeverityWiseRespolutionSLAPie" runat="server"></asp:Literal>
    <asp:HiddenField ID="hdnfldVariable" runat="server" />
    <script src="https://pcv-demo.hitachi-systems-mc.com:5723/assetsdata/plugins/apexchart/apexcharts.min.js"></script>
</asp:Content>


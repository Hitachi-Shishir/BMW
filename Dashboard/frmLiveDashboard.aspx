<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmLiveDashboard.aspx.cs" Inherits="frmLiveDashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .fixed-top-right {
    position: fixed;
    top: 72px; /* Adjust the top offset as needed */
    right: 20px; /* Adjust the right offset as needed */
    z-index: 1034; /* Ensure it appears above other content */
       padding: 10px; /* Optional: Add padding for spacing */
    border-radius: 8px; /* Optional: Add rounded corners */
    box-shadow: 0 2px 5px rgba(0, 0, 0, 0.1); /* Optional: Add a shadow */
}

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:Panel ID="pnlTicketCount" runat="server">
        <section class="content">

           <div class="fixed-top-right bg-light">
    <div class="d-flex align-items-center gap-3">
        <h5 class="mb-1 fw-semibold d-flex align-items-center">
            <asp:Label ID="lblTotalTickets" runat="server" Text="0"></asp:Label>
            <i class="ti ti-arrow-up-right fs-5 lh-base text-success"></i>
        </h5>
        <p class="mb-1 small">Total Tickets</p>
    </div>
</div>


   


            <div class="clearfix"></div>

        </section>
    </asp:Panel>
    <asp:Panel ID="pnlSLA" runat="server">
        <section class="content">

            <div class="row gx-2 gy-1">

                <div class="col-md-6 " id="divCategory" runat="server">
                    <div class="card  mb-1">
                        <h6 class="mb-0 card-header ">Response SLA</h6>
                        <div class="card-body">
                            <div class="d-flex align-items-center justify-content-between">
                                <asp:Button ID="btnChartResponseSLA" runat="server" OnClick="btnChartResponseSLA_Click" hidden />
                            </div>
                            <div id="chart1"></div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6 " id="div1" runat="server">
                    <div class="card  mb-1">
                        <h6 class="mb-0 card-header ">Severity Wise Calls</h6>
                        <div class="card-body">
                            <div class="d-flex align-items-center justify-content-between">
                                <asp:Button ID="btnChartServerityWiseTotal" runat="server" OnClick="btnChartServerityWiseTotal_Click" hidden />
                            </div>
                            <div id="chart2"></div>
                        </div>
                    </div>
                </div>

                <div class="col-md-6 " id="div2" runat="server">
                    <div class="card  mb-1">
                        <h6 class="mb-0 card-header">Severity wise Response SLA</h6>
                        <div class="card-body">
                            <div class="d-flex align-items-center justify-content-between">
                                <asp:Button ID="btnChartSeverity" runat="server" OnClick="btnChartSeverity_Click" hidden />
                            </div>
                            <div id="chart3"></div>
                        </div>
                    </div>
                </div>
                <div class="col-md-6 " id="div3" runat="server">
                    <div class="card  mb-1">
                        <h6 class="mb-0 card-header ">Resolution SLA</h6>
                        <div class="card-body">
                            <div class="d-flex align-items-center justify-content-between">
                                <asp:Button ID="btnChartResolutionSLA" runat="server" OnClick="btnChartResolutionSLA_Click" hidden />
                            </div>
                            <div id="chart4"></div>
                        </div>
                    </div>
                </div>

                <div class="col-md-12 " id="div4" runat="server">
                    <div class="card  mb-1">
                        <h6 class="mb-0 card-header ">Severity wise Resolution SLA</h6>
                        <div class="card-body">
                            <div class="d-flex align-items-center justify-content-between">
                                <asp:Button ID="btnChartSeverityWiseResolution" runat="server" OnClick="btnChartSeverityWiseResolution_Click" hidden />
                            </div>
                            <div id="chart5"></div>
                        </div>
                    </div>
                </div>


            </div>

        </section>
    </asp:Panel>
    <asp:Panel ID="pnlAssignewise" runat="server">
        <div class="row">
            <div class="col-md-12 ">
                <div class="card mb-1">
                    <div class="card-body">
                        <div class="row">

                            <div class="col-md-12">
                                <div class="table-responsive table-container">
                                    <asp:GridView ID="gvOpenClosed" runat="server" Class="table border table-sm text-nowrap mb-0" AutoGenerateColumns="False">
                                        <Columns>
                                            <asp:BoundField ItemStyle-Width="150px" DataField="Total" HeaderText="Total" />
                                            <asp:BoundField ItemStyle-Width="150px" DataField="Open" HeaderText="Open" />
                                            <asp:BoundField ItemStyle-Width="150px" DataField="Closed" HeaderText="Closed" />
                                            <asp:BoundField ItemStyle-Width="150px" DataField="Closed Percent" HeaderText="Closed %" DataFormatString="{0:0}%" />
                                        </Columns>
                                        <%-- <RowStyle BackColor="White" BorderColor="#e3e4e6" BorderWidth="1px" Height="5px" />
                                        <FooterStyle BackColor="#EDEDED" Font-Bold="True" ForeColor="Black" />
                                        <PagerStyle BackColor="#EDEDED" ForeColor="#000000" HorizontalAlign="Left" Height="5px" VerticalAlign="NotSet" CssClass="header" />
                                        <SelectedRowStyle BackColor="#fff" Font-Bold="True" ForeColor="#000000" Height="5px" />
                                        <HeaderStyle BackColor="#EEEEEE" Font-Bold="True" ForeColor="#414141" Height="5px" BorderColor="WhiteSmoke" CssClass="header sorting_asc" Font-Size="Small" />
                                        <EditRowStyle BackColor="#e9edf2" BorderColor="#e3e4e6" BorderStyle="Solid" BorderWidth="1px" Height="5px" />
                                        <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#EDEDED" />--%>
                                        <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
                                        <%--<AlternatingRowStyle BackColor="#EAEEFF" BorderColor="#e3e4e6" Height="5px" BorderStyle="Solid" BorderWidth="1px" />--%>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>

                    </div>
                </div>
                <div class="row g-1">

                    <div class="col-md-4">
                        <div class="card mb-1" >
                            <div class="card-header">
                                Assignee Wise Tickets &nbsp;
                                                                                                               <asp:LinkButton ID="ImgBtnExport" runat="server"  OnClick="ImgBtnExport_Click" CssClass=" btn btn-sm btn-outline-secondary"> <i class="fa-solid fa-file-excel"></i></asp:LinkButton>


                            </div>
                            <div class="card-body">
                                <div class="table-responsive table-container" style="height:300px">
                                    <asp:GridView ID="gvAssigneeCallsStatus" runat="server" Class="table table-sm border text-nowrap " OnDataBound="gvAssigneeCallsStatus_DataBound" ShowFooter="True">
                                        <%--<RowStyle BackColor="White" BorderColor="#e3e4e6" BorderWidth="1px" Height="5px" />
                                                    <FooterStyle BackColor="#EDEDED" Font-Bold="True" ForeColor="Black" />
                                                    <PagerStyle BackColor="#EDEDED" ForeColor="#000000" HorizontalAlign="Left" Height="5px" VerticalAlign="NotSet" CssClass="header" />
                                                    <SelectedRowStyle BackColor="#fff" Font-Bold="True" ForeColor="#000000" Height="5px" />
                                                    <HeaderStyle BackColor="#EEEEEE" Font-Bold="True" ForeColor="#414141" Height="5px" BorderColor="WhiteSmoke" CssClass="header sorting_asc" Font-Size="Small" />
                                                    <EditRowStyle BackColor="#e9edf2" BorderColor="#e3e4e6" BorderStyle="Solid" BorderWidth="1px" Height="5px" />
                                                    <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#EDEDED" />--%>
                                        <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
                                        <%--<AlternatingRowStyle BackColor="#EAEEFF" BorderColor="#e3e4e6" Height="5px" BorderStyle="Solid" BorderWidth="1px" />--%>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="card mb-1">
                            <div class="card-header">
                                Assignee Wise Aging
                            </div>
                            <div class="card-body">
                                <div class="table-responsive table-container" style="height:308px">
                                    <asp:GridView ID="gvCallAging" runat="server" Class="table border table-sm text-nowrap" AutoGenerateColumns="False" OnRowCommand="gvCallAging_RowCommand" ShowFooter="True">
                                        <Columns>
                                            <asp:BoundField DataField="Assignee" HeaderText="Assignee" />
                                            <asp:TemplateField HeaderText="0 to 3 Days">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" ID="lnkView0to3" CommandArgument='<%#Eval("Assignee") %>' Text='<%#(String.IsNullOrEmpty(Eval("0 to 3 Days").ToString()) ? "0" : Eval("0 to 3 Days"))%>'
                                                        CommandName="VIEW0to3">View Deal</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="4 to 7 Days">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" ID="lnkView4to7" CommandArgument='<%#Eval("Assignee") %>' Text='<%#(String.IsNullOrEmpty(Eval("4 to 7 Days").ToString()) ? "0" : Eval("4 to 7 Days"))%>'
                                                        CommandName="VIEW4to7">View Deal</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="8 to 10 Days">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" ID="lnkView8to10" CommandArgument='<%#Eval("Assignee") %>' Text='<%#(String.IsNullOrEmpty(Eval("8 to 10 Days").ToString()) ? "0" : Eval("8 to 10 Days"))%>'
                                                        CommandName="VIEW8to10">View Deal</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="More than 10 Days">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" ID="lnkViewMore10Days" CommandArgument='<%#Eval("Assignee") %>' Text='<%#(String.IsNullOrEmpty(Eval("More than 10 Days").ToString()) ? "0" : Eval("More than 10 Days"))%>'
                                                        CommandName="VIEWMore10Days">View Deal</asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <%-- <RowStyle BackColor="White" BorderColor="#e3e4e6" BorderWidth="1px" Height="5px" />
                                                <FooterStyle BackColor="#EDEDED" Font-Bold="True" ForeColor="Black" />
                                                <PagerStyle BackColor="#EDEDED" ForeColor="#000000" HorizontalAlign="Left" Height="5px" VerticalAlign="NotSet" CssClass="header" />
                                                <SelectedRowStyle BackColor="#fff" Font-Bold="True" ForeColor="#000000" Height="5px" />
                                                <HeaderStyle BackColor="#EEEEEE" Font-Bold="True" ForeColor="#414141" Height="5px" BorderColor="WhiteSmoke" CssClass="header sorting_asc" Font-Size="Small" />
                                                <EditRowStyle BackColor="#e9edf2" BorderColor="#e3e4e6" BorderStyle="Solid" BorderWidth="1px" Height="5px" />
                                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#EDEDED" />--%>
                                        <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
                                        <%--<AlternatingRowStyle BackColor="#EAEEFF" BorderColor="#e3e4e6" Height="5px" BorderStyle="Solid" BorderWidth="1px" />--%>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-4 ">

                        <div class="card mb-1">
                            <div class="card-header">
                                Status Wise Tickets
             <asp:Button ID="btnchartcallstatus" runat="server" OnClick="btnchartcallstatus_Click" hidden />
                            </div>
                            <div class="card-body">
                                <!-- Sales Chart Canvas -->
                                <canvas id="chart_callstatus" height="300" width="300" class="chartjs-render-monitor"></canvas>
                                <%--  </div>--%>
                            </div>
                        </div>
                    </div>

                </div>
                <div class="row" hidden>
                    <div class="col-md-4 graphs">
                        <div class="xs">
                            <div class="well1 white">
                                <div class="card card-default">
                                    <div class="card-header">
                                        Location Wise Tickets
                            <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Images/New folder/excelnew.png" OnClick="ImageButton2_Click" CssClass="pull-right control-label" class="fa fa-download" />
                                    </div>
                                    <asp:GridView ID="gridviewcategory" runat="server" Class="table  table-sm border text-nowrap" OnDataBound="gridviewcategory_DataBound" ShowFooter="True">
                                        <%-- <RowStyle BackColor="White" BorderColor="#e3e4e6" BorderWidth="1px" Height="5px" />
                                                <FooterStyle BackColor="#EDEDED" Font-Bold="True" ForeColor="Black" />
                                                <PagerStyle BackColor="#EDEDED" ForeColor="#000000" HorizontalAlign="Left" Height="5px" VerticalAlign="NotSet" CssClass="header" />
                                                <SelectedRowStyle BackColor="#fff" Font-Bold="True" ForeColor="#000000" Height="5px" />
                                                <HeaderStyle BackColor="#EEEEEE" Font-Bold="True" ForeColor="#414141" Height="5px" BorderColor="WhiteSmoke" CssClass="header sorting_asc" Font-Size="Small" />
                                                <EditRowStyle BackColor="#e9edf2" BorderColor="#e3e4e6" BorderStyle="Solid" BorderWidth="1px" Height="5px" />
                                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#EDEDED" />--%>
                                        <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
                                        <%--<AlternatingRowStyle BackColor="#EAEEFF" BorderColor="#e3e4e6" Height="5px" BorderStyle="Solid" BorderWidth="1px" />--%>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-12">

                        <div class="card mb-1">
                            <div class="card-header d-flex justify-content-between">
                                <h6 class="mb-0 card-title">Assignee Wise Tickets</h6>
                                <div>
                                    <asp:LinkButton ID="ImageButton1" runat="server"  OnClick="ImgBtnExport_Click" CssClass=" btn btn-sm btn-outline-secondary"> <i class="fa-solid fa-file-excel"></i></asp:LinkButton>

&nbsp;                                    <asp:Label ID="lblTotalHead" runat="server" CssClass="h6" Text="Total: "></asp:Label>
                                    <asp:Label ID="lblTotal" runat="server" CssClass=" h6" Text="0"></asp:Label>
                                </div>
                            </div>
                            <div class="card-body">
                                <div class="row ">
                                    <div class="col-md-4 d-none">

                                        <asp:Label ID="lblsofname" runat="server" Text="      Assignee Wise Tickets" Font-Size="Larger" ForeColor="Black"></asp:Label>

                                    </div>


                                </div>
                                <asp:GridView ID="gvCallAgingDetails" runat="server" Class="table table-bordered" OnDataBound="gvAssigneeCallsStatus_DataBound" ShowFooter="True">
                                    <RowStyle BackColor="White" BorderColor="#e3e4e6" BorderWidth="1px" Height="5px" />
                                    <FooterStyle BackColor="#EDEDED" Font-Bold="True" ForeColor="Black" />
                                    <PagerStyle BackColor="#EDEDED" ForeColor="#000000" HorizontalAlign="Left" Height="5px" VerticalAlign="NotSet" CssClass="header" />
                                    <SelectedRowStyle BackColor="#fff" Font-Bold="True" ForeColor="#000000" Height="5px" />
                                    <HeaderStyle BackColor="#EEEEEE" Font-Bold="True" ForeColor="#414141" Height="5px" BorderColor="WhiteSmoke" CssClass="header sorting_asc" Font-Size="Small" />
                                    <EditRowStyle BackColor="#e9edf2" BorderColor="#e3e4e6" BorderStyle="Solid" BorderWidth="1px" Height="5px" />
                                    <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#EDEDED" />
                                    <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
                                    <AlternatingRowStyle BackColor="#EAEEFF" BorderColor="#e3e4e6" Height="5px" BorderStyle="Solid" BorderWidth="1px" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>


    </asp:Panel>
    <asp:Panel ID="pnlDepartmentwise" runat="server">

        <div class="row g-1">
            <div class="col-md-12">
                <div class="card mb-1">
                    <div class="card-body">
                        <div class="table-responsive table-container">
                            <asp:GridView ID="gvOpenClosedDept" runat="server" Class="table table-bordered table-sm text-nowrap mb-0" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:BoundField ItemStyle-Width="150px" DataField="Total" HeaderText="Total" />
                                    <asp:BoundField ItemStyle-Width="150px" DataField="Open" HeaderText="Open" />
                                    <asp:BoundField ItemStyle-Width="150px" DataField="Closed" HeaderText="Closed" />
                                    <asp:BoundField ItemStyle-Width="150px" DataField="Closed Percent" HeaderText="Closed %" DataFormatString="{0:0}%" />
                                </Columns>
                                <%-- <RowStyle BackColor="#fafafa" BorderColor="#e3e4e6" BorderWidth="1px" Height="20px" Font-Size="X-Small" />
                                    <FooterStyle BackColor="#fafafa" Font-Bold="True" ForeColor="Black" />
                                    <PagerStyle BackColor="#fafafa" ForeColor="Black" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#8dbcfc" Font-Bold="True" Height="20px" ForeColor="#000000" />
                                    <HeaderStyle BackColor="#8dbcfc" Font-Bold="True" ForeColor="White" Height="20px" Font-Size="X-Small" />
                                    <EditRowStyle BackColor="#e9edf2" BorderColor="#e3e4e6" BorderStyle="Solid" Height="20px" BorderWidth="1px" />
                                    <AlternatingRowStyle BackColor="#e1e8f0" BorderColor="#e3e4e6" Font-Size="X-Small" Height="20px" BorderStyle="Solid" BorderWidth="1px" />--%>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="card mb-1">
                    <div class="card-header ">
                        <div class="row">
                        <div class="col-11"> Department Wise Tickets </div>
                        <div class="col-1">
                                                                               <asp:LinkButton ID="ImgBtnExportDept" runat="server"  OnClick="ImgBtnExportDept_Click" CssClass=" btn btn-sm btn-outline-secondary"> <i class="fa-solid fa-file-excel"></i></asp:LinkButton>

                        </div>
                        </div>

                    </div>

                    <div class="table-responsive table-container" style="height: 300px">
                        <asp:GridView ID="gvAssigneeCallsStatusDept" runat="server" Class="table table-bordered"
                            OnDataBound="gvAssigneeCallsStatusDept_DataBound" ShowFooter="True">
                            <%-- <RowStyle BackColor="White" BorderColor="#e3e4e6" BorderWidth="1px" Height="5px" />
                        <FooterStyle BackColor="#EDEDED" Font-Bold="True" ForeColor="Black" />
                        <PagerStyle BackColor="#EDEDED" ForeColor="#000000" HorizontalAlign="Left" Height="5px" VerticalAlign="NotSet" CssClass="header" />
                        <SelectedRowStyle BackColor="#fff" Font-Bold="True" ForeColor="#000000" Height="5px" />
                        <HeaderStyle BackColor="#EEEEEE" Font-Bold="True" ForeColor="#414141" Height="5px" BorderColor="WhiteSmoke" CssClass="header sorting_asc" Font-Size="Small" />
                        <EditRowStyle BackColor="#e9edf2" BorderColor="#e3e4e6" BorderStyle="Solid" BorderWidth="1px" Height="5px" />
                        <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#EDEDED" />--%>
                            <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
                            <%--<AlternatingRowStyle BackColor="#EAEEFF" BorderColor="#e3e4e6" Height="5px" BorderStyle="Solid" BorderWidth="1px" />--%>
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="card mb-1">
                    <div class="card-header">
                        Department Wise Aging
                    </div>
                    <div class="table-responsive table-container" style="height: 300px">
                        <asp:GridView ID="gvCallAgingDept" runat="server" Class="table table-bordered table-sm text-nowrap"
                            AutoGenerateColumns="False" OnRowCommand="gvCallAgingDept_RowCommand" ShowFooter="True">
                            <Columns>
                                <asp:BoundField DataField="Department" HeaderText="Department" />
                                <asp:TemplateField HeaderText="0 to 3 Days">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="lnkView1" CommandArgument='<%#Eval("Department") %>' Text='<%#(String.IsNullOrEmpty(Eval("0 to 3 Days").ToString()) ? "0" : Eval("0 to 3 Days"))%>'
                                            CommandName="VIEW0to3">View Deal</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="4 to 7 Days">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="lnkView2" CommandArgument='<%#Eval("Department") %>' Text='<%#(String.IsNullOrEmpty(Eval("4 to 7 Days").ToString()) ? "0" : Eval("4 to 7 Days"))%>'
                                            CommandName="VIEW4to7">View Deal</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="8 to 10 Days">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="lnkView3" CommandArgument='<%#Eval("Department") %>' Text='<%#(String.IsNullOrEmpty(Eval("8 to 10 Days").ToString()) ? "0" : Eval("8 to 10 Days"))%>'
                                            CommandName="VIEW8to10">View Deal</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="More than 10 Days">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="lnkView4" CommandArgument='<%#Eval("Department") %>' Text='<%#(String.IsNullOrEmpty(Eval("More than 10 Days").ToString()) ? "0" : Eval("More than 10 Days"))%>'
                                            CommandName="VIEWMore10Days">View Deal</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <%-- <RowStyle BackColor="White" BorderColor="#e3e4e6" BorderWidth="1px" Height="5px" />
              <FooterStyle BackColor="#EDEDED" Font-Bold="True" ForeColor="Black" />
              <PagerStyle BackColor="#EDEDED" ForeColor="#000000" HorizontalAlign="Left" Height="5px" VerticalAlign="NotSet" CssClass="header" />
              <SelectedRowStyle BackColor="#fff" Font-Bold="True" ForeColor="#000000" Height="5px" />
              <HeaderStyle BackColor="#EEEEEE" Font-Bold="True" ForeColor="#414141" Height="5px" BorderColor="WhiteSmoke" CssClass="header sorting_asc" Font-Size="Small" />
              <EditRowStyle BackColor="#e9edf2" BorderColor="#e3e4e6" BorderStyle="Solid" BorderWidth="1px" Height="5px" />
              <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#EDEDED" />--%>
                            <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
                            <%--<AlternatingRowStyle BackColor="#EAEEFF" BorderColor="#e3e4e6" Height="5px" BorderStyle="Solid" BorderWidth="1px" />--%>
                        </asp:GridView>
                    </div>
                </div>
            </div>

        </div>
        <div class="col-md-4 graphs" hidden>
            <div class="xs">
                <div class="well1 white">
                    <div class="card card-default">
                        <div class="card-header">
                            Status Wise Tickets
             <asp:Button ID="btnchartcallstatusDept" runat="server" OnClick="btnchartcallstatusDept_Click" hidden />
                        </div>
                        <div class="card-body">
                            <!-- Sales Chart Canvas -->
                            <canvas id="chart_callstatusDept" height="300" width="300" class="chartjs-render-monitor"></canvas>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row" hidden>
            <div class="col-md-4 graphs">
                <div class="xs">
                    <div class="well1 white">
                        <div class="card card-default">
                            <div class="card-header">
                                Location Wise Tickets
                            <asp:ImageButton ID="ImageButton2Dept" runat="server" ImageUrl="~/Images/New folder/excelnew.png" OnClick="ImageButton2Dept_Click" CssClass="pull-right control-label" class="fa fa-download" />
                            </div>
                            <asp:GridView ID="gridviewcategoryDept" runat="server" Class="table table-bordered" OnDataBound="gridviewcategoryDept_DataBound" ShowFooter="True">
                                <RowStyle BackColor="White" BorderColor="#e3e4e6" BorderWidth="1px" Height="5px" />
                                <FooterStyle BackColor="#EDEDED" Font-Bold="True" ForeColor="Black" />
                                <PagerStyle BackColor="#EDEDED" ForeColor="#000000" HorizontalAlign="Left" Height="5px" VerticalAlign="NotSet" CssClass="header" />
                                <SelectedRowStyle BackColor="#fff" Font-Bold="True" ForeColor="#000000" Height="5px" />
                                <HeaderStyle BackColor="#EEEEEE" Font-Bold="True" ForeColor="#414141" Height="5px" BorderColor="WhiteSmoke" CssClass="header sorting_asc" Font-Size="Small" />
                                <EditRowStyle BackColor="#e9edf2" BorderColor="#e3e4e6" BorderStyle="Solid" BorderWidth="1px" Height="5px" />
                                <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#EDEDED" />
                                <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
                                <AlternatingRowStyle BackColor="#EAEEFF" BorderColor="#e3e4e6" Height="5px" BorderStyle="Solid" BorderWidth="1px" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">

                <div class="card ">
                    <div class="card-header d-flex justify-content-between">
                        <asp:Label ID="Label2" runat="server" Text="   Department Wise Tickets " CssClass="h6 mb-0"></asp:Label>
                        <div>
                        <asp:LinkButton ID="ImageButton5" runat="server"  OnClick="ImgBtnExport_Click" CssClass=" btn btn-sm btn-outline-secondary"> <i class="fa-solid fa-file-excel"></i></asp:LinkButton>
                          &nbsp;  <asp:Label ID="Label3" runat="server" CssClass="h6" Text="Total: "></asp:Label>
                            <asp:Label ID="lblTotalgrid" runat="server" CssClass="h6" Text="0"></asp:Label>
                        </div>
                    </div>
                    <div class="card-body">

                        <div class="table-responsive table-container" style="height: 300px">
                            <asp:GridView ID="gvCallAgingDetailsDept" runat="server" Class="table border table-sm text-nowrap dataTable1" OnDataBound="gvAssigneeCallsStatusDept_DataBound" ShowFooter="True">
                                <%-- <RowStyle BackColor="White" BorderColor="#e3e4e6" BorderWidth="1px" Height="5px" />
                                            <FooterStyle BackColor="#EDEDED" Font-Bold="True" ForeColor="Black" />
                                            <PagerStyle BackColor="#EDEDED" ForeColor="#000000" HorizontalAlign="Left" Height="5px" VerticalAlign="NotSet" CssClass="header" />
                                            <SelectedRowStyle BackColor="#fff" Font-Bold="True" ForeColor="#000000" Height="5px" />
                                            <HeaderStyle BackColor="#EEEEEE" Font-Bold="True" ForeColor="#414141" Height="5px" BorderColor="WhiteSmoke" CssClass="header sorting_asc" Font-Size="Small" />
                                            <EditRowStyle BackColor="#e9edf2" BorderColor="#e3e4e6" BorderStyle="Solid" BorderWidth="1px" Height="5px" />
                                            <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#EDEDED" />--%>
                                <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
                                <%--<AlternatingRowStyle BackColor="#EAEEFF" BorderColor="#e3e4e6" Height="5px" BorderStyle="Solid" BorderWidth="1px" />--%>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>

        </div>


    </asp:Panel>
    <asp:Panel ID="pnlGrd" runat="server" Visible="false">
        <div class="card card-default" id="divGrd" runat="server" visible="false">
            <div class="card-header">
                <asp:Label ID="Label1" runat="server" CssClass="pull-right control-label" Text="0" hidden></asp:Label>
                <asp:Label ID="lblTotalSLA" runat="server" CssClass="pull-right control-label" hidden Text="Total: "></asp:Label>
                <asp:ImageButton ID="ImgbtnExport4" runat="server" ImageUrl="~/images/excel.png" hidden CssClass="pull-right" OnClick="ImgbtnExport4_Click" />
            </div>
            <div class="card-body">
                <div class="table-responsive table-container">
                    <asp:GridView GridLines="None" ID="gvSDTicketsDetails" runat="server" AutoGenerateColumns="true" CssClass="data-table table table-striped border table-sm text-nowrap table-hover" Width="100%" ShowHeaderWhenEmpty="True" EmptyDataText="No records Found">
                        <%--  <RowStyle BackColor="White" BorderColor="#e3e4e6" BorderWidth="1px" Height="5px" />
                        <FooterStyle BackColor="#EDEDED" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#EDEDED" ForeColor="#000000" HorizontalAlign="Left" Height="5px" VerticalAlign="NotSet" CssClass="header" />
                        <SelectedRowStyle BackColor="#fff" Font-Bold="True" ForeColor="#000000" Height="5px" />
                        <HeaderStyle BackColor="#EEEEEE" Font-Bold="True" ForeColor="#414141" Height="5px" BorderColor="WhiteSmoke" CssClass="header sorting_asc" Font-Size="Small" />
                        <EditRowStyle BackColor="#e9edf2" BorderColor="#e3e4e6" BorderStyle="Solid" BorderWidth="1px" Height="5px" />
                        <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#EDEDED" />--%>
                        <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
                        <%--<AlternatingRowStyle BackColor="#EAEEFF" BorderColor="#e3e4e6" Height="5px" BorderStyle="Solid" BorderWidth="1px" />--%>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </asp:Panel>

    <script src="../chartjs/js/chart.js"></script>
    <script src="../chartjs/js/chart.min.js"></script>
    <script src="../chartjs/js/chartjs-plugin-datalabels.min.js"></script>
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
    <%--Severity wise Response SLA--%>
    <script src="https://pcv-demo.hitachi-systems-mc.com:5723/assetsdata/plugins/apexchart/apexcharts.min.js"></script>
</asp:Content>


<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="SDDepartmentCallDetails.aspx.cs" Inherits="Dashboard_SDDepartmentCallDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .dataTables_filter {
            margin-top: -29px !important;
        }

        .dt-buttons > .btn-outline-secondary {
            padding: 0.25rem 0.5rem !important;
            font-size: 0.875rem !important;
        }

        .pagination {
            --bs-pagination-padding-x: 0.5rem;
            --bs-pagination-padding-y: 0.25rem;
            --bs-pagination-font-size: 0.875rem;
            --bs-pagination-border-radius: var(--bs-border-radius-sm);
            /*margin-top: -1.7rem!important;*/
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">

    <div class="card mb-1">
        <div class="card-body">
            <div class="row gy-2 gx-3">
                <div class="col-md-3 ">
                    <label class="form-label ">
                        Select Desk 
                    <asp:RequiredFieldValidator ID="RequiredSDDrop" runat="server" ControlToValidate="DropDesks" InitialValue="0" ErrorMessage="Required" ForeColor="Red" ValidationGroup="SU"></asp:RequiredFieldValidator>
                    </label>
                    <asp:DropDownList ID="DropDesks" runat="server" CssClass="form-control form-control-sm chzn-select">
                    </asp:DropDownList>
                </div>

                <div class="col-md-2 ">
                    <asp:Label ID="lblError" runat="server"></asp:Label>
                    <label class="form-label ">
                        From
                        <asp:RequiredFieldValidator ID="RequiredFieldtxtFrom" runat="server" ControlToValidate="txtFrom" ErrorMessage="Required" ForeColor="Red" ValidationGroup="SU"></asp:RequiredFieldValidator>
                    </label>
                    <div class="input-group mb-3">
                        <asp:TextBox AutoCompleteType="None" runat="server" Placeholder="" ID="txtFrom" class="form-control form-control-sm datepicker"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-2 ">
                    <label class="form-label ">
                        To
                        <asp:RequiredFieldValidator ID="RequiredtxtTo" runat="server" ControlToValidate="txtTo" ErrorMessage="Required" ForeColor="Red" ValidationGroup="SU"></asp:RequiredFieldValidator>
                    </label>
                    &nbsp;
                            <div class="input-group mb-3">
                                <asp:TextBox AutoCompleteType="None" runat="server" Placeholder="" ID="txtTo" class="form-control form-control-sm datepicker"></asp:TextBox>
                            </div>
                </div>
                <div class="col-md-2">
                    <label class="form-label opacity-0">werew</label><br />
                    <asp:Button ID="btnSearch" runat="server" Text="Search" ValidationGroup="SU" CssClass="btn btn-sm  btn-grd-info" OnClick="btnSearch_Click" />
                </div>
                <div class="col-md-12" >
                    <asp:GridView ID="gvOpenClosed" runat="server" Class="table table-bordered table-sm" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField ItemStyle-Width="150px" DataField="Total" HeaderText="Total" />
                            <asp:BoundField ItemStyle-Width="150px" DataField="Open" HeaderText="Open" />
                            <asp:BoundField ItemStyle-Width="150px" DataField="Closed" HeaderText="Closed" />
                            <asp:BoundField ItemStyle-Width="150px" DataField="Closed Percent" HeaderText="Closed %" DataFormatString="{0:0}%" />
                        </Columns>
                        <%--  <RowStyle BackColor="#fafafa" BorderColor="#e3e4e6" BorderWidth="1px" Height="20px" Font-Size="X-Small" />
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
    <div class="row gy-1 gx-1">
        <div class="col-md-6">
            <div class="card mb-1">
                <div class="d-flex align-items-start justify-content-between card-header">
                    Department Wise Tickets
                     <asp:LinkButton ID="LinkButton1" runat="server" CssClass="d-none btn btn-sm btn-outline-secondary ">Export <i class="fa-solid fa-download"></i></asp:LinkButton>
                    <%--<asp:ImageButton ID="ImgBtnExport" runat="server" ImageUrl="~/Images/New folder/excelnew.png" OnClick="ImgBtnExport_Click" CssClass="pull-right form-label" class="fa fa-download" />--%>
                    <asp:LinkButton ID="ImgBtnExport" CssClass="btn btn-sm btn-outline-secondary " runat="server" OnClick="ImgBtnExport_Click"> Excel <i class="fa-solid fa-download"></i></asp:LinkButton>
                </div>
                <div class="card-body p-1">

                    <div class="table-responsive table-container" style="height: 300px">
                        <asp:GridView ID="gvAssigneeCallsStatus" runat="server" Class="table border table-sm  " OnDataBound="gvAssigneeCallsStatus_DataBound" ShowFooter="True">
                            <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
                            <%--<AlternatingRowStyle BackColor="#EAEEFF" BorderColor="#e3e4e6" Height="5px" BorderStyle="Solid" BorderWidth="1px" />--%>
                        </asp:GridView>
                    </div>

                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="card mb-1">
                <div class="card-header">
                    Department Wise Aging
                </div>
                <div class="card-body p-1">
                    <div class="table-responsive table-container" style="height: 300px">
                        <asp:GridView ID="gvCallAging" runat="server" Class="table table-bordered table-sm  " AutoGenerateColumns="False" OnRowCommand="gvCallAging_RowCommand" ShowFooter="True">
                            <Columns>
                                <asp:BoundField DataField="Department" HeaderText="Department" />
                                <asp:TemplateField HeaderText="0 to 3 Days">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="lnkView0to3" CommandArgument='<%#Eval("Department") %>' Text='<%#(String.IsNullOrEmpty(Eval("0 to 3 Days").ToString()) ? "0" : Eval("0 to 3 Days"))%>'
                                            CommandName="VIEW0to3">View Deal</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="4 to 7 Days">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="lnkView4to7" CommandArgument='<%#Eval("Department") %>' Text='<%#(String.IsNullOrEmpty(Eval("4 to 7 Days").ToString()) ? "0" : Eval("4 to 7 Days"))%>'
                                            CommandName="VIEW4to7">View Deal</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="8 to 10 Days">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="lnkView8to10" CommandArgument='<%#Eval("Department") %>' Text='<%#(String.IsNullOrEmpty(Eval("8 to 10 Days").ToString()) ? "0" : Eval("8 to 10 Days"))%>'
                                            CommandName="VIEW8to10">View Deal</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="More than 10 Days">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="lnkViewMore10Days" CommandArgument='<%#Eval("Department") %>' Text='<%#(String.IsNullOrEmpty(Eval("More than 10 Days").ToString()) ? "0" : Eval("More than 10 Days"))%>'
                                            CommandName="VIEWMore10Days">View Deal</asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
                            <%--<AlternatingRowStyle BackColor="#EAEEFF" BorderColor="#e3e4e6" Height="5px" BorderStyle="Solid" BorderWidth="1px" />--%>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>




    <div class="card mb-1" id="divCategory" runat="server">
        <h6 class="card-header">Tickets by Category</h6>
        <div class="card-body">
            <div class="d-flex align-items-center justify-content-between">
                <asp:Button ID="btnchartcallstatus" runat="server" OnClick="btnchartcallstatus_Click" Style="visibility: hidden;" />

            </div>
            <div class="row" hidden>
                <div class="col-md-6"><div id="LineGraph"></div></div>
                <div class="col-md-5">
                    <div class="d-flex flex-column gap-3 px-3 py-4 border rounded ">
                                <div class="d-flex align-items-center justify-content-between">
                                    <p class="mb-0 d-flex align-items-center gap-2 w-25">Open</p>
                                    <div class="">
                                        <p class="mb-0">
                                            <span id="MainContent_lblOpenPer">NaN%</span>
                                        </p>
                                    </div>
                                </div>
                                <div class="d-flex align-items-center justify-content-between">
                                    <p class="mb-0 d-flex align-items-center gap-2 w-25">Hold</p>
                                    <div class="">
                                        <p class="mb-0">
                                            <span id="MainContent_lblHoldPer">NaN%</span>
                                        </p>
                                    </div>
                                </div>
                                <div class="d-flex align-items-center justify-content-between">
                                    <p class="mb-0 d-flex align-items-center gap-2 w-25">WIP</p>
                                    <div class="">
                                        <p class="mb-0">
                                            <span id="MainContent_lblWipPer">NaN%</span>
                                        </p>
                                    </div>
                                </div>
                                <div class="d-flex align-items-center justify-content-between">
                                    <p class="mb-0 d-flex align-items-center gap-2 w-25">Closed</p>
                                    <div class="">
                                        <p class="mb-0">
                                            <span id="MainContent_lblClosedPer">NaN%</span>
                                        </p>
                                    </div>
                                </div>
                                <div class="d-flex align-items-center justify-content-between">
                                    <p class="mb-0 d-flex align-items-center gap-2 w-25">Resolved</p>
                                    <div class="">
                                        <p class="mb-0">
                                            <span id="MainContent_lblResolvedPer">NaN%</span>
                                        </p>
                                    </div>
                                </div>
                            </div>
                </div>
            </div>
            
        </div>
    </div>
    <div class="card">
        <div class="d-flex align-items-start justify-content-between card-header">
            <div>Department Wise Tickets</div>
           
            <div>
                                     <asp:LinkButton ID="LinkButton2" runat="server" CssClass="d-none btn btn-sm btn-outline-secondary ">Export <i class="fa-solid fa-download"></i></asp:LinkButton>

                <asp:ImageButton hidden ID="ImageButton1" runat="server" ImageUrl="~/Images/New folder/excelnew.png" OnClick="ImgBtnExport_Click" CssClass="pull-right ml-4 btn-outline-success form-label" class="fa fa-download" />
                <asp:Label hidden ID="lblTotalHead" runat="server" Font-Size="Large" CssClass=" form-label" Text="Total: "></asp:Label>
                <asp:Label hidden ID="lblTotalgrid" Font-Size="Large" runat="server" CssClass=" form-label" Text="0"></asp:Label>
            </div>
        </div>
   
    <div class="card-body">
        <div class="row ">

            <div class="col-md-4 d-none">
                <asp:Label ID="lblsofname" runat="server" Text="   Department Wise Tickets " Font-Size="Larger" ForeColor="Black"></asp:Label>
            </div>

        </div>
        <div class="table-responsive table-container">
        <asp:GridView ID="gvCallAgingDetails" runat="server" Class="data-table table-striped table-sm table table-head-fixed   border text-nowrap" OnDataBound="gvAssigneeCallsStatus_DataBound" ShowFooter="True">
        </asp:GridView>
    </div>
    </div>
    </div>


    <asp:HiddenField ID="hdnfldVariable" runat="server" />
    <asp:Literal ID="chartScript" runat="server" />
    <script src="https://pcv-demo.hitachi-systems-mc.com:5723/assetsdata/plugins/apexchart/apexcharts.min.js"></script>
</asp:Content>


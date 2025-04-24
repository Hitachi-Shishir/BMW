<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="DBSDSLAII.aspx.cs" Inherits="Dashboard_DBSDSLAII" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="card mb-3">
        <div class="card-body">
            <div class="row gy-2 gx-3">
                <div class="col-md-3">
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
                        From<asp:RequiredFieldValidator ID="RequiredFieldtxtFrom" runat="server" ControlToValidate="txtFrom" ErrorMessage="Required" ForeColor="Red" ValidationGroup="SU"></asp:RequiredFieldValidator>
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
                    <label class="form-label opacity-0">dkadsjfl</label><br />
                    <asp:Button ID="btnSearch" runat="server" Text="Search" ValidationGroup="SU" CssClass="btn btn-sm btn-grd-info" OnClick="btnSearch_Click" />
                </div>
            </div>
        </div>
    </div>
    <div class="row gy-2 gx-3">
        <div class="col-md-6 " id="divCategory" runat="server">
            <div class="card  mb-1">
                <h6 class="mb-0 card-header border-0">Response SLA</h6>
                <div class="card-body">
                    <div class="d-flex align-items-center justify-content-between">
                        <asp:Button ID="btnChartResponseSLA" runat="server" OnClick="btnChartResponseSLA_Click" hidden />
                    </div>
                    <div id="chart1"></div>
                </div>
            </div>
        </div>

        <div class="col-md-6 " id="divServerity" runat="server">
            <div class="card mb-1 ">
                <h6 class="mb-0 card-header border-0">Severity Wise Resolve Calls</h6>
                <div class="card-body">
                    <div class="d-flex align-items-center justify-content-between">
                        <asp:Button ID="btnChartSeverityWiseResolCalls" runat="server" OnClick="btnChartSeverityWiseResolCalls_Click" hidden />
                    </div>
                    <div id="chart6"></div>
                </div>
            </div>
        </div>

        <div class="col-md-6 " id="divServeritySLA" runat="server">
            <div class="card  mb-1">
                <h6 class="mb-0 card-header border-0">Severity wise Response SLA</h6>
                <div class="card-body">
                    <div class="d-flex align-items-center justify-content-between">
                        <asp:Button ID="btnChartSeverity" runat="server" OnClick="btnChartSeverity_Click" hidden />
                    </div>
                    <div id="chart3"></div>
                </div>
            </div>
        </div>

        <div class="col-md-6 " id="divResol" runat="server">
            <div class="card  mb-1">
                <h6 class="mb-0 card-header border-0">Resolution SLA</h6>
                <div class="card-body">
                    <div class="d-flex align-items-center justify-content-between">
                        <asp:Button ID="btnChartResolutionSLA" runat="server" OnClick="btnChartResolutionSLA_Click" hidden />
                    </div>
                    <div id="chart2"></div>
                </div>
            </div>
        </div>

        <div class="col-md-6" id="div1" runat="server">
            <div class="card mb-1 ">
                <h6 class="mb-0 card-header border-0">Severity Wise Calls</h6>
                <div class="card-body">
                    <div class="d-flex align-items-center justify-content-between">
                        <asp:Button ID="btnChartServerityWiseTotal" runat="server" OnClick="btnChartServerityWiseTotal_Click" hidden />
                    </div>
                    <div id="chart5"></div>
                </div>
            </div>
        </div>

        <div class="col-md-6" id="div2" runat="server">
            <div class="card mb-1 ">
                <h6 class="mb-0 card-header border-0">Severity wise Resolution SLA</h6>
                <div class="card-body">
                    <div class="d-flex align-items-center justify-content-between">
                        <asp:Button ID="btnChartSeverityWiseResolution" runat="server" OnClick="btnChartSeverityWiseResolution_Click" hidden />
                    </div>
                    <div id="chart4"></div>
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hdnfldVariable" runat="server" />
    <asp:HiddenField ID="hdnfldStatus" runat="server" />
    <script src="https://pcv-demo.hitachi-systems-mc.com:5723/assetsdata/plugins/apexchart/apexcharts.min.js"></script>
</asp:Content>


<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="DeskConfiguration.aspx.cs" Inherits="DeskConfiguration" MaintainScrollPositionOnPostback="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .fa-plus .fa-floppy-disk .fa-refresh {
            color: var(--bs-body-color) !important;
        }

        [data-bs-theme=semi-dark] .btn-grd-primary {
            background-image: linear-gradient(310deg, #6c757d, #6c757d) !important;
        }

        [data-bs-theme=blue-theme] .tooltip-inner {
            background-color: white !important;
            color: black !important;
            border: 1px solid black;
        }

        [data-bs-theme=blue-theme] .tooltip-arrow::before {
            border-top-color: white !important;
        }

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
    <style>
        .step-trigger {
            background-color: #f8f9fa;
            Default button background border: none;
            display: flex;
            align-items: center;
            gap: 10px;
            width: auto;
            padding: 10px;
            text-align: left;
        }

            .step-trigger:enabled {
                background-color: #ff5a9e;
                Active state background color color: white;
            }

        .bs-stepper-circle {
            background-color: #e9ecef;
            width: 30px;
            height: 30px;
            display: flex;
            align-items: center;
            justify-content: center;
            border-radius: 50%;
            font-size: 0.8rem;
        }

        .step-trigger:enabled .bs-stepper-circle {
            background-color: white;
            Active state circle color
        }

        .steper-title {
            font-size: 1.1rem;
            margin-bottom: 0;
        }

        .steper-sub-title {
            font-size: 0.85rem;
            margin-bottom: 0;
        }

        .bs-stepper-line {
            width: 100px; /* Adjust width as per layout */
            height: 2px;
            background-color: #e9ecef;
            margin-left: 15px;
            margin-right: 15px;
        }

        .vr {
            rotate: 90deg;
            margin: 5px 1px;
        }

        h6, h6 > span {
            font-size: 0.8rem;
        }
    </style>
    <style>
        .tooltip-wrapper {
            position: relative;
            display: inline-block;
        }

            .tooltip-wrapper[data-bs-toggle="tooltip"]::after {
                content: attr(title);
                position: absolute;
                top: -25px;
                left: 50%;
                transform: translateX(-50%);
                background-color: #333;
                color: #fff !important;
                padding: 5px 10px;
                border-radius: 5px;
                opacity: 0;
                visibility: hidden;
                transition: opacity 0.2s ease, visibility 0.2s ease;
                white-space: nowrap;
            }

            .tooltip-wrapper:hover::after {
                opacity: 1;
                visibility: visible;
            }

        #MainContent_lnkNextStage, #MainContent_lnkNextAddReq, #MainContent_lnkbtnNextStatus, #MainContent_lnkNextSeverity, #MainContent_lnkNextPriority, #MainContent_lnkNextCategory, #MainContent_lnkNextEmailConfig, #MainContent_lnkNextSLA, #MainContent_lnkNextDeskConfig, #MainContent_lnkNextCustomFields, #MainContent_lnkNextResolution {
            /*     color: var(--bs-body-color) !important;
            background-color: var(--bs-body-bg) !important;
            background-color: #e9ecef38 !important;*/
            width: 30px !important;
            height: 30px !important;
            display: flex !important;
            align-items: center !important;
            justify-content: center !important;
            border-radius: 50% !important;
            font-size: 0.8rem !important;
            /*cursor: not-allowed !important;*/
        }

        .modalBackground {
            background-color: #00000091;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="scriptmanager1" runat="server"></asp:ScriptManager>
    <div id="stepper1" class="bs-stepper">

        <%--Stepper Start--%>
        <asp:UpdatePanel ID="updtcardbtn" runat="server">
            <ContentTemplate>
                <div class="card mb-0" style="border-radius: 0.375rem 0.375rem 0 0">
                    <div class="card-header">
                        <div class="row">
                            <div class="d-lg-flex flex-lg-row align-items-lg-center justify-content-lg-between" role="tablist">

                                <!-- Step 1 -->
                                <div class="tooltip-wrapper" id="a1" runat="server" data-bs-toggle="tooltip" data-bs-placement="top" title="Organisation">
                                    <asp:LinkButton ID="stepper1trigger1" runat="server" ToolTip="Organisation" CssClass='<%# CurrentStep == 1 ? "btn step-trigger btn-grd-primary p-2 rounded-circle btn-disabled" : "btn step-trigger " %>' OnClick="StepButton_Click1" Style="pointer-events: none;">
                    <div class="bs-stepper-circle">1</div>
                                    </asp:LinkButton>
                                </div>
                                <div visible="false" id="a11" runat="server" class="tooltip-wrapper" data-bs-toggle="tooltip" data-bs-placement="top" title="Organisation">
                                    <asp:LinkButton ID="LinkButton1" runat="server" ToolTip="Organisation" CssClass="btn step-trigger btn-grd-success p-2 rounded-circle btn-disabled" Style="pointer-events: none;" OnClick="StepButton_Click1">
<div class="bs-stepper-circle">1</div>
                                    </asp:LinkButton>
                                </div>
                                <div class="vr"></div>
                                <!-- Step 2 -->
                                <div class="tooltip-wrapper" id="a2" runat="server" runat="server" data-bs-toggle="tooltip" data-bs-placement="top" title="Request Type">
                                    <asp:LinkButton ID="stepper1trigger2" runat="server" ToolTip="Request Type" CssClass='<%# CurrentStep == 2 ? "btn step-trigger btn-grd-primary p-2 rounded-circle" : "btn step-trigger" %>' Enabled='<%# CurrentStep >= 2 %>' OnClick="StepButton_Click2" Style="pointer-events: none;">
                    <div class="bs-stepper-circle">2</div>
                                    </asp:LinkButton>
                                </div>
                                <div id="a22" runat="server" visible="false" class="tooltip-wrapper" data-bs-toggle="tooltip" data-bs-placement="top" title="Request Type">
                                    <asp:LinkButton ID="asd" runat="server" ToolTip="Request Type" CssClass="btn step-trigger btn-grd-success p-2 rounded-circle" Enabled='<%# CurrentStep >= 2 %>' Style="pointer-events: none;" OnClick="StepButton_Click2">
                    <div class="bs-stepper-circle">2</div>
                                    </asp:LinkButton>
                                </div>

                                <div class="vr"></div>

                                <!-- Step 3 -->
                                <div id="a3" runat="server" class="tooltip-wrapper" data-bs-toggle="tooltip" data-bs-placement="top" title="Add Stage">
                                    <asp:LinkButton ID="stepper1trigger3" runat="server" ToolTip="Add Stage" CssClass='<%# CurrentStep == 3 ? "btn step-trigger btn-grd-primary p-2 rounded-circle" : "btn step-trigger" %>' Enabled='<%# CurrentStep >= 3 %>' OnClick="StepButton_Click3" Style="pointer-events: none;">
                    <div class="bs-stepper-circle">3</div>
                                    </asp:LinkButton>
                                </div>
                                <div id="a33" runat="server" visible="false" class="tooltip-wrapper" data-bs-toggle="tooltip" data-bs-placement="top" title="Add Stage">
                                    <asp:LinkButton ID="sdf" runat="server" ToolTip="Add Stage" CssClass="btn step-trigger btn-grd-success p-2 rounded-circle btn-disabled" Enabled='<%# CurrentStep >= 3 %>' Style="pointer-events: none;" OnClick="StepButton_Click3">
                    <div class="bs-stepper-circle">3</div>
                                    </asp:LinkButton>
                                </div>
                                <div class="vr"></div>

                                <!-- Step 4 -->
                                <div id="a4" runat="server" class="tooltip-wrapper" data-bs-toggle="tooltip" data-bs-placement="top" title="Status">
                                    <asp:LinkButton ID="stepper1trigger4" runat="server" ToolTip="Status" CssClass='<%# CurrentStep == 4 ? "btn step-trigger btn-grd-primary p-2 rounded-circle" : "btn step-trigger" %>' Enabled='<%# CurrentStep >= 4 %>' OnClick="StepButton_Click4" Style="pointer-events: none;">
                    <div class="bs-stepper-circle">4</div>
                                    </asp:LinkButton>
                                </div>
                                <div id="a44" runat="server" visible="false" class="tooltip-wrapper" data-bs-toggle="tooltip" data-bs-placement="top" title="Status">
                                    <asp:LinkButton ID="sdds" runat="server" ToolTip="Status" CssClass="btn step-trigger btn-grd-success p-2 rounded-circle btn-disabled" Enabled='<%# CurrentStep >= 4 %>' Style="pointer-events: none;" OnClick="StepButton_Click4">
                    <div class="bs-stepper-circle">4</div>
                                    </asp:LinkButton>
                                </div>
                                <div class="vr"></div>

                                <!-- Step 5 -->
                                <div id="a5" runat="server" class="tooltip-wrapper" data-bs-toggle="tooltip" data-bs-placement="top" title="Add Severity">
                                    <asp:LinkButton ID="stepper1trigger5" runat="server" ToolTip="Add Severity" CssClass='<%# CurrentStep == 5 ? "btn step-trigger btn-grd-primary p-2 rounded-circle" : "btn step-trigger" %>' Enabled='<%# CurrentStep >= 5 %>' OnClick="StepButton_Click5" Style="pointer-events: none;">
                    <div class="bs-stepper-circle">5</div>
                                    </asp:LinkButton>
                                </div>
                                <div id="a55" runat="server" visible="false" class="tooltip-wrapper" data-bs-toggle="tooltip" data-bs-placement="top" title="Add Severity">
                                    <asp:LinkButton ID="ddf" runat="server" ToolTip="Add Severity" CssClass="btn step-trigger btn-grd-success p-2 rounded-circle btn-disabled" Enabled='<%# CurrentStep >= 5 %>' Style="pointer-events: none;" OnClick="StepButton_Click5">
                    <div class="bs-stepper-circle">5</div>
                                    </asp:LinkButton>
                                </div>
                                <div class="vr"></div>

                                <!-- Step 6 -->
                                <div id="a6" runat="server" class="tooltip-wrapper" data-bs-toggle="tooltip" data-bs-placement="top" title="Add Priority">
                                    <asp:LinkButton ID="stepper1trigger6" runat="server" ToolTip="Add Priority" CssClass='<%# CurrentStep == 6 ? "btn step-trigger btn-grd-primary p-2 rounded-circle" : "btn step-trigger" %>' Enabled='<%# CurrentStep >= 6 %>' OnClick="StepButton_Click6" Style="pointer-events: none;">
                    <div class="bs-stepper-circle">6</div>
                                    </asp:LinkButton>
                                </div>
                                <div id="a66" runat="server" visible="false" class="tooltip-wrapper" data-bs-toggle="tooltip" data-bs-placement="top" title="Add Priority">
                                    <asp:LinkButton ID="gdy" runat="server" ToolTip="Add Priority" CssClass="btn step-trigger btn-grd-success p-2 rounded-circle btn-disabled" Enabled='<%# CurrentStep >= 6 %>' Style="pointer-events: none;" OnClick="StepButton_Click6">
                    <div class="bs-stepper-circle">6</div>
                                    </asp:LinkButton>
                                </div>
                                <div class="vr"></div>

                                <!-- Step 7 -->
                                <div id="a7" runat="server" class="tooltip-wrapper" data-bs-toggle="tooltip" data-bs-placement="top" title="Add Category">
                                    <asp:LinkButton ID="stepper1trigger7" runat="server" ToolTip="Add Category" CssClass='<%# CurrentStep == 7 ? "btn step-trigger btn-grd-primary p-2 rounded-circle" : "btn step-trigger" %>' Enabled='<%# CurrentStep >= 7 %>' OnClick="StepButton_Click7" Style="pointer-events: none;">
                    <div class="bs-stepper-circle">7</div>
                                    </asp:LinkButton>
                                </div>
                                <div id="a77" runat="server" visible="false" class="tooltip-wrapper" data-bs-toggle="tooltip" data-bs-placement="top" title="Add Category">
                                    <asp:LinkButton ID="dfg" runat="server" ToolTip="Add Category" CssClass="btn step-trigger btn-grd-success p-2 rounded-circle btn-disabled" Enabled='<%# CurrentStep >= 7 %>' Style="pointer-events: none;" OnClick="StepButton_Click7">
                    <div class="bs-stepper-circle">7</div>
                                    </asp:LinkButton>
                                </div>
                                <div class="vr"></div>

                                <!-- Step 8 -->
                                <div id="a8" runat="server" class="tooltip-wrapper" data-bs-toggle="tooltip" data-bs-placement="top" title="Add Resolution">
                                    <asp:LinkButton ID="stepper1trigger8" runat="server" ToolTip="Add Resolution" CssClass='<%# CurrentStep == 8 ? "btn step-trigger btn-grd-primary p-2 rounded-circle" : "btn step-trigger" %>' Enabled='<%# CurrentStep >= 8 %>' OnClick="StepButton_Click8" Style="pointer-events: none;">
                    <div class="bs-stepper-circle">8</div>
                                    </asp:LinkButton>
                                </div>
                                <div id="a88" runat="server" visible="false" class="tooltip-wrapper" data-bs-toggle="tooltip" data-bs-placement="top" title="Add Resolution">
                                    <asp:LinkButton ID="dffg" runat="server" ToolTip="Add Resolution" CssClass="btn step-trigger btn-grd-success p-2 rounded-circle btn-disabled" Enabled='<%# CurrentStep >= 8 %>' Style="pointer-events: none;" OnClick="StepButton_Click8">
                    <div class="bs-stepper-circle">8</div>
                                    </asp:LinkButton>
                                </div>
                                <div class="vr"></div>

                                <!-- Step 9 -->
                                <div id="a9" runat="server" class="tooltip-wrapper" data-bs-toggle="tooltip" data-bs-placement="top" title="Add SLA">
                                    <asp:LinkButton ID="stepper1trigger9" runat="server" ToolTip="Add SLA" CssClass='<%# CurrentStep == 9 ? "btn step-trigger btn-grd-primary p-2 rounded-circle" : "btn step-trigger" %>' Enabled='<%# CurrentStep >= 9 %>' OnClick="StepButton_Click9" Style="pointer-events: none;">
                    <div class="bs-stepper-circle">9</div>
                                    </asp:LinkButton>
                                </div>
                                <div id="a99" runat="server" visible="false" class="tooltip-wrapper" data-bs-toggle="tooltip" data-bs-placement="top" title="Add SLA">
                                    <asp:LinkButton ID="dsf" runat="server" ToolTip="Add SLA" CssClass="btn step-trigger btn-grd-success p-2 rounded-circle btn-disabled" Enabled='<%# CurrentStep >= 9 %>' Style="pointer-events: none;" OnClick="StepButton_Click9">
                    <div class="bs-stepper-circle">9</div>
                                    </asp:LinkButton>
                                </div>
                                <div class="vr"></div>

                                <!-- Step 10 -->
                                <div id="a10" runat="server" class="tooltip-wrapper" data-bs-toggle="tooltip" data-bs-placement="top" title="Desk Template">
                                    <asp:LinkButton ID="stepper1trigger10" runat="server" ToolTip="Desk Template" CssClass='<%# CurrentStep == 10 ? "btn step-trigger btn-grd-primary p-2 rounded-circle" : "btn step-trigger" %>' Enabled='<%# CurrentStep >= 10 %>' OnClick="StepButton_Click10" Style="pointer-events: none;">
                    <div class="bs-stepper-circle">10</div>
                                    </asp:LinkButton>
                                </div>
                                <div id="a1010" runat="server" visible="false" class="tooltip-wrapper" data-bs-toggle="tooltip" data-bs-placement="top" title="Desk Template">
                                    <asp:LinkButton ID="df" runat="server" ToolTip="Desk Template" CssClass="btn step-trigger btn-grd-success p-2 rounded-circle btn-disabled" Enabled='<%# CurrentStep >= 10 %>' Style="pointer-events: none;" OnClick="StepButton_Click10">
                    <div class="bs-stepper-circle">10</div>
                                    </asp:LinkButton>
                                </div>
                                <div class="vr"></div>

                                <!-- Step 11 -->
                                <div id="aa11" runat="server" class="tooltip-wrapper" data-bs-toggle="tooltip" data-bs-placement="top" title="Email Configuration">
                                    <asp:LinkButton ID="stepper1trigger11" runat="server" ToolTip="Email Configuration" CssClass='<%# CurrentStep == 11 ? "btn step-trigger btn-grd-primary p-2 rounded-circle" : "btn step-trigger" %>' Enabled='<%# CurrentStep >= 11 %>' OnClick="StepButton_Click11" Style="pointer-events: none;">
                    <div class="bs-stepper-circle">11</div>
                                    </asp:LinkButton>
                                </div>
                                <div id="a1111" runat="server" visible="false" class="tooltip-wrapper" data-bs-toggle="tooltip" data-bs-placement="top" title="Email Configuration">
                                    <asp:LinkButton ID="sdfg" runat="server" ToolTip="Email Configuration" CssClass="btn step-trigger btn-grd-success p-2 rounded-circle btn-disabled" Enabled='<%# CurrentStep >= 11 %>' Style="pointer-events: none;" OnClick="StepButton_Click11">
                    <div class="bs-stepper-circle">11</div>
                                    </asp:LinkButton>
                                </div>
                                <div class="vr"></div>

                                <!-- Step 12 -->
                                <div id="a12" runat="server" class="tooltip-wrapper" data-bs-toggle="tooltip" data-bs-placement="top" title="Escalation Matrix">
                                    <asp:LinkButton ID="stepper1trigger12" runat="server" ToolTip="Escalation Matrix" CssClass='<%# CurrentStep == 12 ? "btn step-trigger btn-grd-primary p-2 rounded-circle" : "btn step-trigger" %>' Enabled='<%# CurrentStep >= 12 %>' OnClick="StepButton_Click12" Style="pointer-events: none;">
                    <div class="bs-stepper-circle">12</div>
                                    </asp:LinkButton>
                                </div>
                                <div id="a1212" runat="server" visible="false" class="tooltip-wrapper" data-bs-toggle="tooltip" data-bs-placement="top" title="Escalation Matrix">
                                    <asp:LinkButton ID="fdd" runat="server" ToolTip="Escalation Matrix" CssClass="btn step-trigger btn-grd-success p-2 rounded-circle btn-disabled" Enabled='<%# CurrentStep >= 12 %>' Style="pointer-events: none;" OnClick="StepButton_Click12">
                    <div class="bs-stepper-circle">12</div>
                                    </asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <%--Stepper End--%>
        <div>
            <asp:Label ID="lblErrorMsg" runat="server"></asp:Label>
        </div>
        <%--ADD Organisation Start--%>
        <asp:Panel ID="pnlShowOrg" runat="server">
            <asp:UpdatePanel ID="updatepanel1" runat="server">
                <ContentTemplate>
                    <div class="card mb-1" style="border-radius: 0 0 0.375rem 0.375rem">
                        <div class="card-body">

                            <div class="d-flex align-items-start justify-content-between">
                                <div class="btn-group">
                                    <asp:Button ID="btnAddOrg" Text="Add Organisation" runat="server" CssClass="btn btn-sm btn-secondary" OnClick="btnAddOrg_Click" />
                                    <asp:Button ID="btnViewOrg" runat="server" Text-="View Details" CssClass="btn btn-sm btn-outline-secondary" OnClick="btnViewOrg_Click" />
                                </div>

                                <div class="">
                                    <asp:LinkButton class="btn btn-sm bs-stepper-circle rounded-circle btn-outline-secondary" ID="lnkNextAddReq" runat="server" OnClick="lnkNextAddReq_Click"><i class="fa-solid fa-angle-right"></i></asp:LinkButton>
                                </div>
                            </div>
                            <div class="card rounded border mb-1 mt-1" runat="server" id="divAddorg">
                                <div class="card-body">

                                    <%--<div class="d-flex align-items-start justify-content-between">
                                <h6 class="mb-4">Organisation</h6>
                                <div class="">
                                    <asp:LinkButton class="btn btn-sm bs-stepper-circle rounded-circle btn-outline-secondary" ID="lnkNextAddReq" runat="server" OnClick="lnkNextAddReq_Click"><i class="fa-solid fa-angle-right"></i></asp:LinkButton>
                                </div>
                            </div>--%>

                                    <div class=" row g-2 ">
                                        <%-- <div class="col-md-12">
                                     <h6 class="mb-0">Organisation</h6>
                                </div>--%>
                                        <div class="col-md-4">
                                            <label for="staticEmail" class="form-label">
                                                Organization
               
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator28" runat="server" ControlToValidate="ddlOrganization" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="ReqType"></asp:RequiredFieldValidator>
                                            </label>

                                            <asp:DropDownList ID="ddlOrganization" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-2">
                                            <div class="form-label opacity-0">dseds</div>
                                            <asp:LinkButton ID="btnChoose" CssClass="btn btn-sm btn-grd-info" runat="server" Text="Select" OnClick="btnChoose_Click">  <i class="fas fa-check-circle"></i> 
</asp:LinkButton>
                                        </div>
                                        <div class="col-md-6">
                                        </div>
                                        <div class="col-md-4">
                                            <label for="staticEmail" class="form-label">
                                                Create Organization
                                       
                                                <asp:RequiredFieldValidator ID="rfvtxtprioritye" runat="server" ControlToValidate="txtOrgName" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Priority"></asp:RequiredFieldValidator>
                                            </label>
                                            <asp:TextBox ID="txtOrgName" runat="server" CssClass="form-control  form-control-sm" onkeypress="return /^[a-zA-Z\s]*$/.test(event.key) && this.value.length < 50;"></asp:TextBox>
                                        </div>
                                        <div class="col-md-12">
                                            <label for="staticEmail" class="form-label">
                                                Organization Description 
                                       
                                                <asp:RequiredFieldValidator ID="rfvPriority" runat="server" ControlToValidate="txtOrgDesc" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Priority"></asp:RequiredFieldValidator>
                                            </label>
                                            <asp:TextBox ID="txtOrgDesc" runat="server" TextMode="MultiLine" Rows="1" Columns="5" CssClass="form-control  form-control-sm" MaxLength="1000"></asp:TextBox>
                                        </div>
                                        <div class="col-md-4">
                                            <label for="staticEmail" class="form-label">
                                                Contact Person Name 
                                             
                                                <asp:RequiredFieldValidator ID="rfvtxtCntnctPrnsName" runat="server" ControlToValidate="txtCntnctPrnsName" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Priority"></asp:RequiredFieldValidator>
                                            </label>
                                            <asp:TextBox ID="txtCntnctPrnsName" runat="server" CssClass="form-control  form-control-sm" onkeypress="return /^[a-zA-Z\s]*$/.test(event.key) && this.value.length < 20;" MaxLength="20"></asp:TextBox>
                                        </div>
                                        <div class="col-md-4">
                                            <label for="staticEmail" class="form-label">
                                                Contact Person Mobile 
                                            
                                                <asp:RequiredFieldValidator ID="rfvtxtCntctPrnsMob" runat="server" ControlToValidate="txtCntctPrnsMob" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Priority"></asp:RequiredFieldValidator>
                                            </label>
                                            <asp:TextBox ID="txtCntctPrnsMob" runat="server" CssClass="form-control  form-control-sm" onkeypress="return /^[0-9]*$/.test(event.key) && this.value.length < 12;" MaxLength="12"></asp:TextBox>
                                        </div>
                                        <div class="col-md-4">
                                            <label for="staticEmail" class="form-label">
                                                Contact Person Email
                                      
                                                <asp:RequiredFieldValidator ID="rfvtxtCntctPrsnEmail" runat="server" ControlToValidate="txtCntctPrsnEmail" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Priority"></asp:RequiredFieldValidator>
                                            </label>
                                            <asp:TextBox ID="txtCntctPrsnEmail" runat="server" CssClass="form-control  form-control-sm" MaxLength="100"></asp:TextBox>
                                        </div>
                                        <div class="col-md-4">
                                            <label for="staticEmail" class="form-label">
                                                Contact PersonII Name 
                                           
                                            </label>
                                            <asp:TextBox ID="txtCntctPrsnNameII" runat="server" CssClass="form-control  form-control-sm" onkeypress="return /^[a-zA-Z\s]*$/.test(event.key) && this.value.length < 20;" MaxLength="20"></asp:TextBox>
                                        </div>
                                        <div class="col-md-4">
                                            <label for="staticEmail" class="form-label">
                                                Contact PersonII Mobile
                                           
                                            </label>
                                            <asp:TextBox ID="txtCntctPrsnMobII" runat="server" CssClass="form-control  form-control-sm" onkeypress="return /^[0-9]*$/.test(event.key) && this.value.length < 12;" MaxLength="12"></asp:TextBox>
                                        </div>
                                        <div class="col-md-4">
                                            <label for="staticEmail" class="form-label">
                                                Contact PersonII Email
                                           
                                            </label>
                                            <asp:TextBox ID="txtCntctPrnsEmailII" runat="server" CssClass="form-control  form-control-sm" MaxLength="100"></asp:TextBox>
                                        </div>
                                        <div class="col-12 text-end">
                                            <asp:Button ID="btnInsertOrg" runat="server" Text="Save" class="btn btn-sm btn-grd btn-grd-info " OnClick="btnInsertOrg_Click" ValidationGroup="Priority" />
                                            <asp:Button ID="btnUpdateOrg" runat="server" Text="Update" Visible="false" class="btn btn-sm btn-grd btn-grd-info " OnClick="btnUpdateOrg_Click" ValidationGroup="Priority" />
                                            <asp:Button ID="btnCancel" runat="server" Text="Refresh" class="btn btn-sm btn-grd btn-grd-danger " OnClick="btnCancel_Click" CausesValidation="false" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="card rounded border mt-2" runat="server" id="divvieworg" visible="false">
                                <div class="card-body">
                                    <div class="d-flex align-items-start justify-content-between">
                                        <div class="">
                                            <h6 class="mb-0">
                                                <asp:Label ID="lblsofname" runat="server" Text="Organization Details"></asp:Label></h6>
                                        </div>
                                        <asp:LinkButton ID="ImgBtnExport" CssClass="btn btn-sm btn-outline-secondary " runat="server" OnClick="ImgBtnExport_Click"> Excel <i class="fa-solid fa-download"></i></asp:LinkButton>
                                    </div>
                                    <div class="row ">

                                        <div class="col-md-6">
                                            <asp:Label ID="Label1" runat="server"></asp:Label>
                                            <asp:Label ID="Label3" runat="server"></asp:Label>
                                        </div>
                                        <div class="col-md-12 text-end">
                                            <div class="btn btn-sm elevation-1 ml-1 " style="padding: 0px; margin-bottom: 10px; padding-top: 1px">
                                                <%--  <asp:ImageButton ID="" runat="server" ImageUrl="~/Images/New folder/excelnew.png" CssClass="fa-pull-right btn-outline-success mr-1" />--%>
                                            </div>
                                        </div>
                                        <div class="col-md-12">
                                            <div class="table-responsive table-container" style="max-height: 400px">
                                                <asp:GridView GridLines="None" ID="gvOrg" runat="server" DataKeyNames="Org_ID" AutoGenerateColumns="false" CssClass="data-table table table-striped border table-sm text-nowrap"
                                                    Width="100%" OnRowCommand="gvOrg_RowCommand" OnRowDataBound="gvOrg_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="20">
                                                            <ItemTemplate>
                                                                <%#Container.DataItemIndex+1 %>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="OrgName" HeaderText="Organization Name" NullDisplayText="NA" />
                                                        <asp:BoundField DataField="OrgDesc" HeaderText="Organization Desc" NullDisplayText="NA" />
                                                        <asp:BoundField DataField="CntctPrsnName" HeaderText="Cont Person Name" NullDisplayText="NA" />
                                                        <asp:BoundField DataField="CntctPrsnMob" HeaderText="Cont Person Mob " NullDisplayText="NA" />
                                                        <asp:BoundField DataField="CntctPrsnEmail" HeaderText="Cont Person Email " NullDisplayText="NA" />
                                                        <asp:BoundField DataField="CntctPrsnNameII" HeaderText="Cont Person Name II" NullDisplayText="NA" />
                                                        <asp:BoundField DataField="CntctPrsnMobII" HeaderText="Cont Person Mob II" NullDisplayText="NA" />
                                                        <asp:BoundField DataField="CntctPrsnEmailII" HeaderText="Cont Person Email II " NullDisplayText="NA" />
                                                        <%--<asp:ButtonField ButtonType="Image" CommandName="SelectState" HeaderText="Edit" ImageUrl="~/images/edit23.png" ItemStyle-Width="20px" />
                                                <asp:ButtonField HeaderText="Delete" ButtonType="Image" ImageUrl="~/Images/New folder/delnew.png" CommandName="DeleteEx" ItemStyle-Width="20px" ItemStyle-Height="5px" />--%>
                                                        <asp:TemplateField HeaderText="Edit">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkEdit" runat="server" CommandName="SelectState" CommandArgument="<%# Container.DataItemIndex %>" ToolTip="Edit">
                                                <i class="fa-solid fa-edit"></i>
                                                                </asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Delete">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkDelete" runat="server" CommandName="DeleteEx" CommandArgument="<%# Container.DataItemIndex %>" ToolTip="Delete" OnClientClick="return confirm('Are you sure you want to delete this record?');">
                                                <i class="fa-solid fa-xmark text-danger"></i> 
                                                                </asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="ImgBtnExport" />
                    <asp:PostBackTrigger ControlID="gvOrg" />
                    <asp:PostBackTrigger ControlID="lnkNextAddReq" />
                    <asp:PostBackTrigger ControlID="btnInsertOrg" />
                    <asp:PostBackTrigger ControlID="btnChoose" />
                    <asp:PostBackTrigger ControlID="btnAddOrg" />
                    <asp:PostBackTrigger ControlID="btnViewOrg" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:Panel>
        <%--Add Organisation END--%>

        <%--Add Request Type Start--%>
        <asp:Panel ID="pnlReqType" runat="server" Visible="false">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <div class="card mb-1" style="border-radius: 0 0 0.375rem 0.375rem">
                        <div class="card-body">
                            <div class="d-flex align-items-start justify-content-between">
                                <h6 class="mb-3 fw-bold">Add Request Type</h6>
                                <div class="d-flex">
                                    <asp:LinkButton class="btn btn-sm bs-stepper-circle rounded-circle btn-outline-secondary" ToolTip="Previous" ID="lnkPrevOrg" runat="server" OnClick="lnkPrevOrg_Click"><i class="fa-solid fa-angle-left"></i></asp:LinkButton>
                                    &nbsp;
                                   
                                    <asp:LinkButton class="btn btn-sm bs-stepper-circle rounded-circle btn-outline-secondary" ToolTip="Next" ID="lnkNextStage" runat="server" OnClick="lnkNextStage_Click"><i class="fa-solid fa-angle-right"></i></asp:LinkButton>
                                </div>
                            </div>
                            <div class=" row gx-2 gy-3">
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Organization
                                               
                                        <asp:RequiredFieldValidator ID="rfvddlOrg" runat="server" ControlToValidate="ddlOrg" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="ReqType"></asp:RequiredFieldValidator>
                                    </label>
                                    <asp:DropDownList ID="ddlOrg" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Request Type 
                                               
                                        <asp:RequiredFieldValidator ID="rfvReqType" runat="server" ControlToValidate="txtRequestType" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="ReqType" Visible="false"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvddlReq" runat="server" ControlToValidate="ddlReq" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="ReqType"></asp:RequiredFieldValidator>
                                    </label>
                                    <asp:DropDownList ID="ddlReq" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlReq_SelectedIndexChanged">
                                        <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                        <asp:ListItem Value="Incident" Text="Incident"></asp:ListItem>
                                        <asp:ListItem Value="Service Request" Text="Service Request"></asp:ListItem>
                                        <asp:ListItem Value="Change Request" Text="Change Request"></asp:ListItem>
                                        <asp:ListItem Value="Problem Management" Text="Problem Management"></asp:ListItem>
                                        <asp:ListItem Value="Other" Text="Other"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtRequestType" runat="server" CssClass="form-control  form-control-sm" onkeypress="return /^[a-zA-Z\s]*$/.test(event.key) && this.value.length < 20;" Visible="false"></asp:TextBox>
                                </div>
                                <div class="col-md-4" runat="server" id="divAppr" visible="false">
                                    <label class="form-label opacity-0">Approval Required</label><br />
                                    <div class="form-label" >(Approval Required)
                                </div>
                                </div>


                                <div class="col-12">

                                    <div class="d-flex align-items-center gap-2 " runat="server" id="divapr" visible="false">
                                        <label class="form-label">Approvals</label>

                                        <asp:CheckBox ID="chkUserWise" runat="server" class="" AutoPostBack="True" OnCheckedChanged="CheckBox_CheckedChanged" />
                                        <label class="form-check-label" for="">
                                            User Wise
                                       
                                        </label>
                                        <asp:CheckBox ID="chkManualWise" runat="server" class="" AutoPostBack="True" OnCheckedChanged="CheckBox_CheckedChanged" />
                                        <label class="form-check-label" for="flexCheckCheckedSuccess">
                                            Manual Wise
  
                                        </label>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <label for="staticEmail" class="form-label">
                                        Request Description 
                                               
                                        <asp:RequiredFieldValidator ID="rfvRequestDesc" runat="server" ControlToValidate="txtReqDescription" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="ReqType"></asp:RequiredFieldValidator>
                                    </label>
                                    <asp:TextBox ID="txtReqDescription" runat="server" TextMode="MultiLine" Rows="2" Columns="3" CssClass="form-control  form-control-sm"></asp:TextBox>
                                </div>
                                <div class="col-12 text-end ">
                                    <asp:Button Visible="false" ID="btnAddChangeType" runat="server" Text="Add Change-Type" OnClick="btnAddChangeType_Click" class="btn btn-sm btn-grd btn-grd-primary"></asp:Button>
                                    <asp:Button Visible="false" ID="btnAddReasonForChng" runat="server" Text="Add Reason for Change" OnClick="btnAddReasonForChng_Click" class="btn btn-sm btn-grd btn-grd-primary"></asp:Button>
                                    <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSaveReqType_Click" class="btn btn-sm btn-grd btn-grd-info" ValidationGroup="ReqType"></asp:Button>
                                    <asp:Button ID="btnUpdateReqType" runat="server" Text="Update" Visible="false" class="btn btn-sm btn-grd btn-grd-info" OnClick="btnUpdateReqType_Click" ValidationGroup="ReqType" />
                                    <asp:Button ID="btnCancel1" runat="server" Text="Refresh" CssClass="btn btn-sm btn-grd btn-grd-danger" OnClick="btnCancel1_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="card">
                        <div class="card-body">
                            <div class="d-flex align-items-start justify-content-between mb-3">
                                <div class="">
                                    <h6 class="mb-0">
                                        <asp:Label ID="Label2" runat="server" Text="Request Type Details"></asp:Label>
                                    </h6>
                                </div>
                                <asp:LinkButton ID="ImgBtnExportReq" runat="server" CssClass="btn btn-sm btn-outline-secondary" OnClick="ImgBtnExportReq_Click">Export <i class="fa-solid fa-download"></i></asp:LinkButton>
                            </div>
                            <div class="row ">
                                <div class="col-md-6">
                                    <asp:Label ID="Label4" runat="server"></asp:Label>
                                    <asp:Label ID="Label5" runat="server"></asp:Label>
                                </div>
                                <div class="col-md-12">
                                    <div class="table-responsive table-container">
                                        <asp:GridView GridLines="None" ID="gvReqType" runat="server" DataKeyNames="ID" AutoGenerateColumns="false" CssClass="data-table table table-striped border table-sm "
                                            Width="100%" OnRowCommand="gvReqType_RowCommand" OnRowDataBound="gvReqType_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="20">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ReqTypeRef" HeaderText="Request Type" NullDisplayText="NA" />
                                                <asp:BoundField DataField="ReqTypeDef" HeaderText="ReqType Definition" NullDisplayText="NA" />
                                                <asp:TemplateField HeaderText=" Organization">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOrgFk" runat="server" Text='<%# Eval("Org_ID") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblOrgName" runat="server" Text='<%# Eval("OrgName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText=" User-Wise Approval">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblusrwise" runat="server" Text='<%# Eval("UserWise") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText=" Manual Approval">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblmnlwise" runat="server" Text='<%# Eval("ManualWise") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Edit">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkEdit1" runat="server" CommandName="SelectState" CommandArgument="<%# Container.DataItemIndex %>" ToolTip="Edit">
                                                    <i class="fa-solid fa-edit"></i>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delete">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDelete1" runat="server" CommandName="DeleteEx" CommandArgument="<%# Container.DataItemIndex %>" ToolTip="Delete" OnClientClick="return confirm('Are you sure you want to delete this record?');">
                                                    <i class="fa-solid fa-xmark text-danger"></i> 
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    </div>
                    </div>
               
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="gvReqType" />
                    <asp:PostBackTrigger ControlID="ImgBtnExportReq" />
                    <asp:PostBackTrigger ControlID="lnkPrevOrg" />
                    <asp:PostBackTrigger ControlID="lnkNextStage" />
                    <asp:PostBackTrigger ControlID="ddlReq" />
                    <asp:PostBackTrigger ControlID="btnSave" />
                    <asp:PostBackTrigger ControlID="chkUserWise" />
                    <asp:PostBackTrigger ControlID="chkManualWise" />
                    <asp:PostBackTrigger ControlID="btnAddChangeType" />
                    <asp:PostBackTrigger ControlID="btnAddReasonForChng" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:Panel>
        <%--Add Request Type End--%>

        <%--Add Stage Start--%>
        <asp:Panel ID="pnlAddStage" runat="server" Visible="false">
            <asp:UpdatePanel ID="updatepanel3" runat="server">
                <ContentTemplate>

                    <div class="card mb-1" style="border-radius: 0 0 0.375rem 0.375rem">
                        <div class="card-body">
                            <div class="d-flex align-items-start justify-content-between">

                                <h6 class="mb-3 fw-bold">Add Stage</h6>
                                <div class="d-flex align-items-start justify-content-between">
                                    <div class="d-flex">
                                        <asp:LinkButton class="btn btn-sm bs-stepper-circle rounded-circle btn-outline-secondary" ToolTip="Previous" ID="lnkbtnPrevAddReq" runat="server" OnClick="lnkbtnPrevAddReq_Click"><i class="fa-solid fa-angle-left"></i></asp:LinkButton>
                                        &nbsp;
                                       
                                        <asp:LinkButton class="btn btn-sm bs-stepper-circle rounded-circle btn-outline-secondary" ToolTip="Next" ID="lnkbtnNextStatus" runat="server" OnClick="lnkbtnNextStatus_Click"><i class="fa-solid fa-angle-right"></i></asp:LinkButton>
                                    </div>
                                </div>

                            </div>
                            <div class="row gx-2 gy-3">
                                <div class="col-md-6">
                                    <label for="staticEmail" class="form-label">
                                        Organization
                                           
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlOrg2" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Stage"></asp:RequiredFieldValidator>
                                    </label>

                                    <asp:DropDownList Enabled="false" ID="ddlOrg2" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlOrg2_SelectedIndexChanged">
                                        <asp:ListItem Text="--Select--" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-6">
                                    <label for="staticEmail" class="form-label">
                                        Request Type
                                               
                                        <asp:RequiredFieldValidator ID="rfvddlRequestType" runat="server" ControlToValidate="ddlRequestType" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Stage"></asp:RequiredFieldValidator>
                                    </label>

                                    <asp:DropDownList Enabled="false" ID="ddlRequestType" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlRequestType_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-6">
                                    <label for="staticEmail" class="form-label">
                                        Stage Name
                                       
                                        <asp:RequiredFieldValidator ID="rfvtxtStageName" runat="server" ControlToValidate="txtStageName" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Stage"></asp:RequiredFieldValidator>
                                    </label>
                                    <asp:TextBox ID="txtStageName" runat="server" CssClass="form-control  form-control-sm" onkeypress="return /^[a-zA-Z\s]*$/.test(event.key) && this.value.length < 100;"></asp:TextBox>

                                </div>
                                <div class="col-md-6">
                                    <label for="staticEmail" class="form-label">
                                        Stage Description 
    
                                        <asp:RequiredFieldValidator ID="rfvtxtStageDesc" runat="server" ControlToValidate="txtStageDesc" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Stage"></asp:RequiredFieldValidator>
                                    </label>

                                    <asp:TextBox ID="txtStageDesc" runat="server" TextMode="MultiLine" Rows="1" CssClass="form-control  form-control-sm"></asp:TextBox>

                                </div>
                                <div class="col-12 text-end ">
                                    <asp:Button ID="btnInsertStage" runat="server" Text="Save" class="btn btn-sm btn-grd btn-grd-info" OnClick="btnInsertStage_Click" ValidationGroup="Stage" />
                                    <asp:Button ID="btnUpdateStage" runat="server" Text="Update" Visible="false" class="btn btn-sm btn-grd btn-grd-info" OnClick="btnUpdateStage_Click" ValidationGroup="Severity" />
                                    <asp:Button ID="btnCancel2" runat="server" Text="Refresh" class="btn btn-sm btn-grd btn-grd-danger" OnClick="btnCancel2_Click" CausesValidation="false" />
                                </div>



                            </div>
                        </div>
                    </div>

                    <div class="card ">

                        <div class="card-body">
                            <div class="d-flex align-items-start justify-content-between mb-3">
                                <div class="">
                                    <h6 class="mb-0">
                                        <asp:Label ID="Label6" runat="server" Text="Stage Details"></asp:Label>
                                    </h6>
                                </div>
                                <asp:LinkButton ID="ImgBtnExport2" runat="server" CssClass="btn btn-sm btn-outline-secondary" OnClick="ImgBtnExport2_Click">Export <i class="fa-solid fa-download"></i></asp:LinkButton>

                            </div>


                            <div class="row ">

                                <div class="col-md-6">
                                    <asp:Label ID="Label7" runat="server"></asp:Label>
                                    <asp:Label ID="Label8" runat="server"></asp:Label>
                                </div>
                                <div class="col-md-12">
                                    <div class="table-container table-responsive">
                                        <asp:GridView GridLines="None" ID="gvStage" runat="server" DataKeyNames="ID" AutoGenerateColumns="false" CssClass="data-table table table-striped border table-sm"
                                            Width="100%" OnRowCommand="gvStage_RowCommand" OnRowDataBound="gvStage_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="20">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="DeskRef" HeaderText="Request Type" NullDisplayText="NA" />
                                                <asp:BoundField DataField="StageCodeRef" HeaderText="Stage Name" NullDisplayText="NA" />
                                                <asp:BoundField DataField="StageDesc" HeaderText="Stage Description" NullDisplayText="NA" />
                                                <asp:TemplateField HeaderText=" Organization">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOrgFk" runat="server" Text='<%# Eval("Org_ID") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblOrgName" runat="server" Text='<%# Eval("OrgName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--<asp:ButtonField ButtonType="Image" CommandName="SelectStage" HeaderText="Edit" ImageUrl="~/images/edit23.png" ItemStyle-Width="20px" />
         <asp:ButtonField HeaderText="Delete" ButtonType="Image" ImageUrl="~/Images/New folder/delnew.png" CommandName="DeleteEx" ItemStyle-Width="20px" ItemStyle-Height="5px" />--%>
                                                <asp:TemplateField HeaderText="Edit">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkEdit2" runat="server" CommandName="SelectStage" CommandArgument="<%# Container.DataItemIndex %>" ToolTip="Edit">
     <i class="fa-solid fa-edit"></i>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delete">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDelete2" runat="server" CommandName="DeleteEx" CommandArgument="<%# Container.DataItemIndex %>" ToolTip="Delete" OnClientClick="return confirm('Are you sure you want to delete this record?');">
     <i class="fa-solid fa-xmark text-danger"></i> 
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>





                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="ddlRequestType" />
                    <asp:PostBackTrigger ControlID="ImgBtnExport2" />
                    <asp:PostBackTrigger ControlID="gvStage" />
                    <asp:PostBackTrigger ControlID="ddlOrg2" />
                    <asp:PostBackTrigger ControlID="lnkbtnPrevAddReq" />
                    <asp:PostBackTrigger ControlID="lnkbtnNextStatus" />
                </Triggers>

            </asp:UpdatePanel>
        </asp:Panel>
        <%--Add Stage End--%>

        <%--Add Status Start--%>
        <asp:Panel ID="pnlStatus" runat="server" Visible="false">
            <asp:UpdatePanel ID="updatepanel4" runat="server">
                <ContentTemplate>

                    <div class="card mb-1" style="border-radius: 0 0 0.375rem 0.375rem">
                        <div class="card-body">
                            <div class="d-flex align-items-start justify-content-between">
                                <h6 class="mb-3 fw-bold">Add Status</h6>
                                <div class="d-flex">
                                    <asp:LinkButton class="btn btn-sm bs-stepper-circle rounded-circle btn-outline-secondary" ToolTip="Previous" ID="lnkPrevStage" runat="server" OnClick="lnkPrevStage_Click"><i class="fa-solid fa-angle-left"></i></asp:LinkButton>&nbsp;

                                    <asp:LinkButton class="btn btn-sm bs-stepper-circle rounded-circle btn-outline-secondary" ID="lnkNextSeverity" runat="server" ToolTip="Next" OnClick="lnkNextSeverity_Click"><i class="fa-solid fa-angle-right"></i></asp:LinkButton>

                                </div>

                            </div>
                            <div class="row gx-2 gy-3">
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Organization
                                           
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlOrg3" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Status"></asp:RequiredFieldValidator>
                                    </label>
                                    <asp:DropDownList Enabled="false" ID="ddlOrg3" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlOrg3_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Request Type
                                           
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlRequestTypeStatus" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Status"></asp:RequiredFieldValidator>
                                    </label>
                                    <asp:DropDownList Enabled="false" ID="ddlRequestTypeStatus" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlRequestTypeStatus_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Stage
								
                                        <asp:RequiredFieldValidator ID="rfvddlStage" runat="server" ControlToValidate="ddlStage" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Status"></asp:RequiredFieldValidator>
                                    </label>
                                    <asp:DropDownList ID="ddlStage" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Status Name
                                           
                                        <asp:RequiredFieldValidator ID="rfvtxtStatusName" runat="server" ControlToValidate="txtStatusName" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Status"></asp:RequiredFieldValidator>
                                    </label>
                                    <asp:TextBox ID="txtStatusName" runat="server" CssClass="form-control  form-control-sm" onkeypress="return /^[a-zA-Z\s]*$/.test(event.key) && this.value.length < 100;"></asp:TextBox>

                                </div>

                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Status Description
                                           
                                        <asp:RequiredFieldValidator ID="rfvtxtStatusDesc" runat="server" ControlToValidate="txtStatusDesc" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Status"></asp:RequiredFieldValidator>
                                    </label>
                                    <asp:TextBox ID="txtStatusDesc" runat="server" TextMode="MultiLine" Rows="1" CssClass="form-control  form-control-sm"></asp:TextBox>

                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Choose Status Color 
                                           
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtColorForStatus" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Status"></asp:RequiredFieldValidator>
                                    </label>
                                    <asp:TextBox ID="txtColorForStatus" TextMode="Color" runat="server" CssClass="form-control  form-control-sm"></asp:TextBox>

                                </div>
                                <div class="col-12 text-end">
                                    <asp:Button ID="btnInsertStatus" runat="server" Text="Save" class="btn btn-sm btn-grd btn-grd-info " OnClick="btnInsertStatus_Click" ValidationGroup="Status" />
                                    <asp:Button ID="btnUpdateStatus" runat="server" Text="Update" Visible="false" class="btn btn-sm btn-grd btn-grd-info " OnClick="btnUpdateStatus_Click" ValidationGroup="Status" />
                                    <asp:Button ID="btnCancel3" runat="server" Text="Refresh" class="btn btn-sm btn-grd btn-grd-danger " OnClick="btnCancel3_Click" CausesValidation="false" />
                                </div>

                            </div>
                        </div>
                    </div>

                    <div class="card ">

                        <div class="card-body">


                            <div class="d-flex align-items-start justify-content-between mb-3">
                                <div class="">
                                    <h6 class="mb-0 fw-bold">
                                        <asp:Label ID="Label9" runat="server" Text="Status Details"></asp:Label></h6>
                                    </h6>
                               
                                </div>
                                <asp:LinkButton ID="ImgBtnExport3" runat="server" CssClass="btn btn-sm btn-outline-secondary" OnClick="ImgBtnExport3_Click">Export <i class="fa-solid fa-download"></i></asp:LinkButton>
                            </div>


                            <div class="row ">

                                <div class="col-md-6">
                                    <asp:Label ID="Label10" runat="server"></asp:Label>
                                    <asp:Label ID="Label11" runat="server"></asp:Label>
                                </div>

                                <div class="col-md-12">
                                    <div class="table-responsive table-container">
                                        <asp:GridView GridLines="None" ID="gvStatus" runat="server" DataKeyNames="ID" AutoGenerateColumns="false" CssClass="data-table table table-striped border text-nowrap table-sm"
                                            Width="100%" OnRowCommand="gvStatus_RowCommand" OnRowDataBound="gvStatus_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="20">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>


                                                <asp:BoundField DataField="DeskRef" HeaderText="Request Type" NullDisplayText="NA" />
                                                <asp:TemplateField HeaderText="Stage">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStageFk" runat="server" Text='<%# Eval("sd_stageFK") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblStageName" runat="server" Text='<%# Eval("StageCodeRef") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="StatusCodeRef" HeaderText="Status Name" NullDisplayText="NA" />
                                                <asp:BoundField DataField="StatusDesc" HeaderText="Status Description" NullDisplayText="NA" />
                                                <asp:BoundField DataField="StatusColorCode" HeaderText=" Color Code" ControlStyle-CssClass="hidden" NullDisplayText="NA" />
                                                <asp:TemplateField HeaderText="Status Color" ItemStyle-Width="150" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatusCode" runat="server" Font-Size="XX-Small" CssClass=" badge badge-notifications" BackColor='<%# System.Drawing.ColorTranslator.FromHtml(Eval("StatusColorCode").ToString())%>' ForeColor='<%# System.Drawing.ColorTranslator.FromHtml(Eval("StatusColorCode").ToString())%>' Text='<%# Eval("StatusColorCode") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText=" Organization">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOrgFk" runat="server" Text='<%# Eval("Org_ID") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblOrgName" runat="server" Text='<%# Eval("OrgName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--<asp:ButtonField ButtonType="Image" CommandName="SelectStatus" HeaderText="Edit" ImageUrl="~/images/edit23.png" ItemStyle-Width="20px" />
                                                <asp:ButtonField HeaderText="Delete" ButtonType="Image" ImageUrl="~/Images/New folder/delnew.png" CommandName="DeleteEx" ItemStyle-Width="20px" ItemStyle-Height="5px" />--%>
                                                <asp:TemplateField HeaderText="Edit">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkEdit3" runat="server" CommandName="SelectStatus" CommandArgument="<%# Container.DataItemIndex %>" ToolTip="Edit">
                                                <i class="fa-solid fa-edit"></i>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delete">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDelete3" runat="server" CommandName="DeleteEx" CommandArgument="<%# Container.DataItemIndex %>" ToolTip="Delete" OnClientClick="return confirm('Are you sure you want to delete this record?');">
                                                <i class="fa-solid fa-xmark text-danger"></i> 
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>

                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>


                        </div>
                    </div>



                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="ddlRequestType" />
                    <asp:PostBackTrigger ControlID="ImgBtnExport3" />
                    <asp:PostBackTrigger ControlID="gvStatus" />
                    <asp:PostBackTrigger ControlID="ddlOrg3" />
                    <asp:PostBackTrigger ControlID="ddlRequestTypeStatus" />
                    <asp:PostBackTrigger ControlID="lnkPrevStage" />
                    <asp:PostBackTrigger ControlID="lnkNextSeverity" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:Panel>
        <%--Add Status End--%>

        <%--Add Severity Start--%>
        <asp:Panel runat="server" ID="pnlAddSeverity" Visible="false">
            <asp:UpdatePanel ID="updatepanel5" runat="server">
                <ContentTemplate>

                    <div class="card mb-1" style="border-radius: 0 0 0.375rem 0.375rem">
                        <div class="card-body">
                            <div class="d-flex align-items-start justify-content-between mb-2">
                                <h6 class="mb-3 fw-bold">Add Severity</h6>
                                <div class="d-flex">

                                    <asp:LinkButton class="btn btn-sm bs-stepper-circle rounded-circle btn-outline-secondary" ID="lnkPrevStatus" ToolTip="Previous" runat="server" OnClick="lnkPrevStatus_Click"><i class="fa-solid fa-angle-left"></i></asp:LinkButton>
                                    &nbsp;
    
                                    <asp:LinkButton class="btn btn-sm bs-stepper-circle rounded-circle btn-outline-secondary" ID="lnkNextPriority" runat="server" ToolTip="Next" OnClick="lnkNextPriority_Click"><i class="fa-solid fa-angle-right"></i></asp:LinkButton>
                                </div>
                            </div>

                            <div class="row gx-2 gy-3">
                                <div class="col-md-3">
                                    <label for="staticEmail" class="form-label">
                                        Organization
                                           
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlOrg4" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Severity"></asp:RequiredFieldValidator>
                                    </label>

                                    <asp:DropDownList Enabled="false" ID="ddlOrg4" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlOrg4_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-3">
                                    <label for="staticEmail" class="form-label">
                                        Request Type
                                           
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="ddlRequestTypeSeverity" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Severity"></asp:RequiredFieldValidator>
                                    </label>


                                    <asp:DropDownList Enabled="false" ID="ddlRequestTypeSeverity" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlRequestTypeSeverity_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2">
                                    <label for="staticEmail" class="form-label">
                                        Severity Name
                                           
                                        <asp:RequiredFieldValidator ID="rfvtxtSeverityName" runat="server" ControlToValidate="txtSeverityName" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Severity" onkeypress="return /^[a-zA-Z0-9\s]*$/.test(event.key) && this.value.length < 20;" MaxLength="20"></asp:RequiredFieldValidator>
                                    </label>

                                    <asp:TextBox ID="txtSeverityName" oninput="validateCaseSensitiveInput(this);" runat="server" CssClass="form-control  form-control-sm"></asp:TextBox>
                                    <p id="error-message" style="color: red; display: none;">Please enter a case-sensitive value.</p>
                                </div>
                                <div class="col-md-2">
                                    <label for="staticEmail" class="form-label">
                                        Response Time<span style="font-size: .775em;"> (in Min) </span>
                                        <asp:RequiredFieldValidator ID="rfvtxtResponseTime" runat="server" ControlToValidate="txtResponseTime" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Severity"></asp:RequiredFieldValidator>
                                    </label>
                                    <asp:TextBox ID="txtResponseTime" runat="server" CssClass="form-control  form-control-sm" onkeypress="return /^[0-9\s]*$/.test(event.key) && this.value.length < 50;"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <label for="staticEmail" class="form-label">
                                        Resolution Time<span style="font-size: .775em;"> (in Min) </span>
                                        <asp:RequiredFieldValidator ID="rfvtxtResolutionTime" runat="server" ControlToValidate="txtResolutionTime" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Severity"></asp:RequiredFieldValidator>
                                    </label>
                                    <asp:TextBox ID="txtResolutionTime" runat="server" CssClass="form-control  form-control-sm" onkeypress="return /^[0-9\s]*$/.test(event.key) && this.value.length < 50;"></asp:TextBox>
                                </div>
                                <div class="col-md-12">
                                    <label for="staticEmail" class="form-label">
                                        Severity Description 
                                           
                                        <asp:RequiredFieldValidator ID="rfvSeverityDesc" runat="server" ControlToValidate="txtSeverityDescription" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Severity"></asp:RequiredFieldValidator>
                                    </label>

                                    <asp:TextBox ID="txtSeverityDescription" runat="server" TextMode="MultiLine" Rows="3" Columns="3" CssClass="form-control  form-control-sm"></asp:TextBox>
                                </div>
                                <div class="col-12 text-end">
                                    <asp:Button ID="btnInsertSeverity" runat="server" Text="Save" class="btn btn-sm btn-grd btn-grd-info " OnClick="btnInsertSeverity_Click" ValidationGroup="Severity" />
                                    <asp:Button ID="btnUpdateSeverity" runat="server" Text="Update" Visible="false" class="btn btn-sm btn-grd btn-grd-info " OnClick="btnUpdateSeverity_Click" ValidationGroup="Severity" />
                                    <asp:Button ID="btnCancel5" runat="server" Text="Refresh" class="btn btn-sm btn-grd btn-grd-danger " OnClick="btnCancel5_Click" CausesValidation="false" />
                                </div>

                            </div>
                        </div>
                    </div>


                    <div class="card ">
                        <div class="card-body">
                            <div class="d-flex align-items-start justify-content-between mb-3">
                                <div class="">
                                    <h6 class="mb-0">
                                        <asp:Label ID="Label12" runat="server" Text="Severity Details"></asp:Label>
                                    </h6>
                                </div>
                                <asp:LinkButton ID="ImgBtnExport4" runat="server" CssClass="btn btn-sm btn-outline-secondary" OnClick="ImgBtnExport4_Click">Export <i class="fa-solid fa-download"></i></asp:LinkButton>


                            </div>
                            <div class="row ">

                                <div class="col-md-6">
                                    <asp:Label ID="Label13" runat="server"></asp:Label>
                                    <asp:Label ID="Label14" runat="server"></asp:Label>
                                </div>
                                <div class="col-md-12">
                                    <div class="table-responsive table-container">
                                        <asp:GridView GridLines="None" ID="gvSeverity" runat="server" DataKeyNames="ID" AutoGenerateColumns="false" CssClass="data-table table table-striped border table-sm text-nowrap "
                                            Width="100%" OnRowCommand="gvSeverity_RowCommand" OnRowDataBound="gvSeverity_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="20">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="DeskRef" HeaderText="Request Type" NullDisplayText="NA" />
                                                <asp:BoundField DataField="Serveritycoderef" HeaderText="Severity Name" NullDisplayText="NA" />
                                                <asp:BoundField DataField="SeverityDesc" HeaderText="Severity Description" NullDisplayText="NA" />
                                                <asp:BoundField DataField="ResponseTime" HeaderText="ResponseTime(Min)" NullDisplayText="0" />
                                                <asp:BoundField DataField="ResolutionTime" HeaderText="ResolutionTime(Min)" NullDisplayText="0" />
                                                <asp:TemplateField HeaderText=" Organization">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOrgFk" runat="server" Text='<%# Eval("Org_ID") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblOrgName" runat="server" Text='<%# Eval("OrgName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%-- <asp:ButtonField ButtonType="Image" CommandName="SelectSeverity" HeaderText="Edit" ImageUrl="~/images/edit23.png" ItemStyle-Width="20px" />
                                                <asp:ButtonField HeaderText="Delete" ButtonType="Image" ImageUrl="~/Images/New folder/delnew.png" CommandName="DeleteEx" ItemStyle-Width="20px" ItemStyle-Height="5px" />--%>
                                                <asp:TemplateField HeaderText="Edit">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkEdit4" runat="server" CommandName="SelectSeverity" CommandArgument="<%# Container.DataItemIndex %>" ToolTip="Edit">
                                                     <i class="fa-solid fa-edit"></i>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delete">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDelete4" runat="server" CommandName="DeleteEx" CommandArgument="<%# Container.DataItemIndex %>" ToolTip="Delete" OnClientClick="return confirm('Are you sure you want to delete this record?');">
                                                     <i class="fa-solid fa-xmark text-danger"></i> 
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>

                            </div>

                        </div>
                    </div>



                </ContentTemplate>

                <Triggers>
                    <asp:PostBackTrigger ControlID="ImgBtnExport" />
                    <asp:PostBackTrigger ControlID="ddlRequestTypeSeverity" />
                    <asp:PostBackTrigger ControlID="gvSeverity" />
                    <asp:PostBackTrigger ControlID="ddlOrg4" />
                    <asp:PostBackTrigger ControlID="lnkPrevStatus" />
                    <asp:PostBackTrigger ControlID="lnkNextPriority" />
                    <asp:PostBackTrigger ControlID="ImgBtnExport4" />
                </Triggers>

            </asp:UpdatePanel>
        </asp:Panel>
        <%--Add Severity End--%>

        <%--Add Priority Start--%>
        <asp:Panel ID="pnlAddPriority" runat="server" Visible="false">
            <asp:UpdatePanel ID="updatepanel6" runat="server">
                <ContentTemplate>
                    <div class="card mb-1" style="border-radius: 0 0 0.375rem 0.375rem">
                        <div class="card-body">
                            <div class="d-flex align-items-start justify-content-between">
                                <h6 class="mb-3 fw-bold">Add Priority</h6>
                                <div class="d-flex">
                                    <asp:LinkButton class="btn btn-sm bs-stepper-circle rounded-circle btn-outline-secondary" ID="lnkPreviousSeverity" ToolTip="Previous" runat="server" OnClick="lnkPreviousSeverity_Click"><i class="fa-solid fa-angle-left"></i></asp:LinkButton>
                                    &nbsp;
                               
                                    <asp:LinkButton class="btn btn-sm bs-stepper-circle rounded-circle btn-outline-secondary" ID="lnkNextCategory" runat="server" ToolTip="Next" OnClick="lnkNextCategory_Click"><i class="fa-solid fa-angle-right"></i></asp:LinkButton>
                                </div>
                            </div>
                            <div class="row gx-2 gy-3 ">
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Organization
                                           
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ddlOrg5" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Priority"></asp:RequiredFieldValidator>
                                    </label>
                                    <asp:DropDownList Enabled="false" ID="ddlOrg5" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlOrg5_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Request Type
                                           
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="ddlRequestTypePriority" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Priority"></asp:RequiredFieldValidator>
                                    </label>
                                    <asp:DropDownList Enabled="false" ID="ddlRequestTypePriority" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlRequestTypePriority_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Severity
           
                                        <asp:RequiredFieldValidator ID="reqddlSeverityfPrior" runat="server" ControlToValidate="ddlSeverityfPrior" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Priority"></asp:RequiredFieldValidator>
                                    </label>
                                    <asp:DropDownList ID="ddlSeverityfPrior" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Priority Name 
                                           
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtpriority" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Priority"></asp:RequiredFieldValidator>
                                    </label>

                                    <asp:TextBox ID="txtpriority" runat="server" CssClass="form-control  form-control-sm" onkeypress="return /^[a-zA-Z\s]*$/.test(event.key) && this.value.length < 20;" MaxLength="20"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <label for="staticEmail" class="form-label">
                                        Response Time<span style="font-size: .775em;"> (in Min) </span>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ControlToValidate="txtRespTimePriority" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Severity"></asp:RequiredFieldValidator>
                                    </label>
                                    <asp:TextBox ID="txtRespTimePriority" runat="server" CssClass="form-control  form-control-sm" onkeypress="return /^[0-9\s]*$/.test(event.key) && this.value.length < 50;"></asp:TextBox>
                                </div>
                                <div class="col-md-2">
                                    <label for="staticEmail" class="form-label">
                                        Resolution Time<span style="font-size: .775em;"> (in Min) </span>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server" ControlToValidate="txtReslnTimePriority" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Severity"></asp:RequiredFieldValidator>
                                    </label>
                                    <asp:TextBox ID="txtReslnTimePriority" runat="server" CssClass="form-control  form-control-sm" onkeypress="return /^[0-9\s]*$/.test(event.key) && this.value.length < 50;"></asp:TextBox>
                                </div>
                                <div class="col-md-12">
                                    <label for="staticEmail" class="form-label">
                                        Priority Description
                                           
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtPriorityDescription" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Priority"></asp:RequiredFieldValidator>
                                    </label>

                                    <asp:TextBox ID="txtPriorityDescription" runat="server" TextMode="MultiLine" Rows="5" Columns="5" CssClass="form-control  form-control-sm" MaxLength="1000"></asp:TextBox>
                                </div>
                                <div class="col-12 text-end">
                                    <asp:Button ID="btnInsertPriority" runat="server" Text="Save" class="btn btn-sm btn-grd-info" OnClick="btnInsertPriority_Click" ValidationGroup="Priority" />
                                    <asp:Button ID="btnUpdatePriority" runat="server" Text="Update" Visible="false" class="btn btn-sm  btn-grd-info" OnClick="btnUpdatePriority_Click" ValidationGroup="Priority" />
                                    <asp:Button ID="btnCancel6" runat="server" Text="Refresh" class="btn btn-sm  btn-grd-danger" OnClick="btnCancel6_Click" CausesValidation="false" />
                                </div>

                            </div>

                        </div>
                    </div>

                    <div class="card ">
                        <div class="card-body">

                            <div class="d-flex align-items-start justify-content-between mb-3">
                                <div class="">
                                    <h6 class="mb-0">
                                        <asp:Label ID="Label15" runat="server" Text="Priority Details"></asp:Label>
                                    </h6>
                                </div>

                                <asp:LinkButton ID="ImgBtnExport7" runat="server" CssClass="btn btn-sm btn-outline-secondary" OnClick="ImgBtnExport7_Click">Export <i class="fa-solid fa-download"></i></asp:LinkButton>
                            </div>

                            <div class="row ">
                                <div class="col-md-12">
                                    <asp:GridView GridLines="None" ID="gvPriority" runat="server" DataKeyNames="PriorityRef" AutoGenerateColumns="false" CssClass="data-table table table-striped border text-nowrap table-sm"
                                        Width="100%" OnRowCommand="gvPriority_RowCommand" OnRowDataBound="gvPriority_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="20">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="DeskRef" HeaderText="Request Type" NullDisplayText="NA" />
                                            <asp:BoundField DataField="PriorityCodeRef" HeaderText="Priority Name" NullDisplayText="NA" />
                                            <asp:BoundField DataField="PriorityDesc" HeaderText="Priority Description" NullDisplayText="NA" />
                                            <asp:BoundField DataField="Severity" HeaderText="Severity" NullDisplayText="NA" />
                                            <asp:TemplateField HeaderText=" Organization">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOrgFk" runat="server" Text='<%# Eval("Org_ID") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("ID") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblOrgName" runat="server" Text='<%# Eval("OrgName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText=" ResponseTime(Min)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblResponseTime" runat="server" Text='<%# Eval("ResponseTime") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ResolutionTime(Min)">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblResolutionTime" runat="server" Text='<%# Eval("ResolutionTime") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%-- <asp:ButtonField ButtonType="Image" CommandName="SelectState" HeaderText="Edit" ImageUrl="~/images/edit23.png" ItemStyle-Width="20px" />
                                            <asp:ButtonField HeaderText="Delete" ButtonType="Image" ImageUrl="~/Images/New folder/delnew.png" CommandName="DeleteEx" ItemStyle-Width="20px" ItemStyle-Height="5px" />--%>
                                            <asp:TemplateField HeaderText="Edit">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkEdit5" runat="server" CommandName="SelectState" CommandArgument="<%# Container.DataItemIndex %>" ToolTip="Edit">
                                                 <i class="fa-solid fa-edit"></i>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Delete">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDelete5" runat="server" CommandName="DeleteEx" CommandArgument="<%# Container.DataItemIndex %>" ToolTip="Delete" OnClientClick="return confirm('Are you sure you want to delete this record?');">
                                             <i class="fa-solid fa-xmark text-danger"></i> 
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>

                        </div>
                    </div>



                </ContentTemplate>

                <Triggers>
                    <asp:PostBackTrigger ControlID="ImgBtnExport7" />
                    <asp:PostBackTrigger ControlID="ddlRequestTypePriority" />
                    <asp:PostBackTrigger ControlID="gvPriority" />
                    <asp:PostBackTrigger ControlID="ddlOrg5" />
                    <asp:PostBackTrigger ControlID="lnkPreviousSeverity" />
                    <asp:PostBackTrigger ControlID="lnkNextCategory" />
                </Triggers>

            </asp:UpdatePanel>
        </asp:Panel>
        <%--Add Priority End--%>

        <%--Add Category Start--%>
        <asp:Panel ID="pnlCategory" runat="server" Visible="false">
            <asp:UpdatePanel ID="updatepanel7" runat="server">
                <ContentTemplate>

                    <div class="card mb-5">
                        <div class="card-body mb-5">
                            <div class="d-flex align-items-start justify-content-between">
                                <h6 class="mb-3 fw-bold">Add Category</h6>
                                <div class="d-flex">
                                    <asp:LinkButton class="btn btn-sm bs-stepper-circle rounded-circle btn-outline-secondary" ID="lnkPreviousPriority" ToolTip="Previous" runat="server" OnClick="lnkPreviousPriority_Click"><i class="fa-solid fa-angle-left"></i></asp:LinkButton>
                                    &nbsp;
    
                                    <asp:LinkButton class="btn btn-sm bs-stepper-circle rounded-circle btn-outline-secondary" ID="lnkNextEmailConfig" runat="server" ToolTip="Next" OnClick="lnkNextEmailConfig_Click"><i class="fa-solid fa-angle-right"></i></asp:LinkButton>
                                </div>
                            </div>


                            <div class="row gy-3 gx-2">
                                <div class="col-md-4 ">
                                    <label class="form-label ">
                                        Organization
                                       
                                        <asp:RequiredFieldValidator ID="reqflddlOrg6" runat="server" ControlToValidate="ddlOrg6" InitialValue="0" ErrorMessage="*" ForeColor="Red" ValidationGroup="SaveCatI"></asp:RequiredFieldValidator>

                                    </label>
                                    <asp:DropDownList ID="ddlOrg6" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" Enabled="false" OnSelectedIndexChanged="ddlOrg6_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <label class="form-label">
                                        Request Type
                                               
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="ddlRequestTypeCategory" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="SaveCatI"></asp:RequiredFieldValidator>
                                    </label>
                                    <asp:DropDownList Enabled="false" ID="ddlRequestTypeCategory" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlRequestTypeCategory_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                </div>
                                <div class="col-md-4">
                                    <label class="form-label">
                                        Category
       
                                        <asp:LinkButton runat="server" ID="imgbtnAddParentCategory" ToolTip="Add Category" OnClick="imgbtnAddParentCategory_Click"><i class="fa-solid fa-plus p-1 rounded-circle border"></i></asp:LinkButton>
                                        <asp:LinkButton ID="imgbtnSaveParentCategory" runat="server" ToolTip="Save Category" OnClick="imgbtnSaveParentCategory_Click" ValidationGroup="SaveCatI" Enabled="false"><i class="fa-solid fa-floppy-disk p-1 rounded-circle border"></i></asp:LinkButton>

                                        <asp:LinkButton ID="imgbtnUpdateParentCategory" runat="server" ToolTip="Update Category" OnClick="imgbtnUpdateParentCategory_Click" ValidationGroup="SaveCatI" Visible="false"><i class="fa-solid fa-pen p-1 rounded-circle border"></i></asp:LinkButton>
                                        <asp:LinkButton ID="imgbtnEditParentCategory" runat="server" ToolTip="Edit Category" OnClick="imgbtnEditParentCategory_Click"><i class="fa-solid fa-edit p-1 rounded-circle border"></i></asp:LinkButton>
                                        <asp:LinkButton ID="imgbtnCancelParent" runat="server" Enabled="false" ToolTip="Refresh" OnClick="imgbtnCancelParent_Click"><i class="fa-solid fa-rotate p-1 rounded-circle border "></i></asp:LinkButton>
                                        <asp:RequiredFieldValidator ID="rfvtxtParentCategory" runat="server" ControlToValidate="txtParentCategory" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="SaveCatI"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvddlParentCategory" runat="server" ControlToValidate="ddlParentCategory" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="AddCatII"></asp:RequiredFieldValidator>
                                    </label>
                                    <asp:TextBox ID="txtParentCategory" runat="server" CssClass="form-control  form-control-sm" ToolTip="Add Category" Visible="false" onkeypress="return /^[a-zA-Z0-9\s]*$/.test(event.key) && this.value.length < 50;"></asp:TextBox>
                                    <asp:DropDownList ID="ddlParentCategory" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlParentCategory_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>

                                <div class="col-md-4">
                                    <label class="form-label">
                                        Category II
          
                                        <asp:LinkButton ID="imgbtnCatII" runat="server" OnClick="imgbtnCatII_Click" ValidationGroup="AddCatII"><i class="fa-solid fa-plus p-1 rounded-circle border"></i></asp:LinkButton>
                                        <asp:LinkButton ID="imgbtnSaveCatII" runat="server" ToolTip="Save Category" OnClick="imgbtnSaveCatII_Click" ValidationGroup="SaveCatII" Enabled="false"><i class="fa-solid fa-floppy-disk p-1 rounded-circle border"></i></asp:LinkButton>
                                        <asp:LinkButton ID="imtbtnUpdateCatII" runat="server" ToolTip="Update Category" OnClick="imtbtnUpdateCatII_Click" ValidationGroup="SaveCatII" Visible="false"><i class="fa-solid fa-pen p-1 rounded-circle border"></i></asp:LinkButton>
                                        <asp:LinkButton ID="imgbtnEditCatII" runat="server" ToolTip="Edit Category" OnClick="imgbtnEditCatII_Click"><i class="fa-solid fa-edit p-1 rounded-circle border "></i></asp:LinkButton>
                                        <asp:LinkButton ID="imgbtnCancelCatII" runat="server" ToolTip="Refresh" OnClick="imgbtnCancelCatII_Click"><i class="fa-solid fa-rotate p-1 rounded-circle border "></i></asp:LinkButton>
                                        <asp:RequiredFieldValidator ID="rfvtxtCatII" runat="server" ControlToValidate="txtCatII" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="SaveCatII"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvddlCatII" runat="server" ControlToValidate="ddlCatII" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="AddCatIII"></asp:RequiredFieldValidator>
                                    </label>
                                    <asp:TextBox ID="txtCatII" runat="server" CssClass="form-control  form-control-sm" ValidationGroup="SaveCatII" Visible="false" onkeypress="return /^[a-zA-Z0-9\s]*$/.test(event.key) && this.value.length < 50;"></asp:TextBox>
                                    <asp:DropDownList ID="ddlCatII" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" Enabled="false" AutoPostBack="true" OnSelectedIndexChanged="ddlCatII_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>

                                <div class="col-md-4">
                                    <label class="form-label">
                                        Category III
          
                                        <asp:LinkButton ID="imgAddCatIII" runat="server" OnClick="imgAddCatIII_Click" ValidationGroup="AddCatIII"> <i class="fa-solid fa-plus p-1 rounded-circle border"></i></asp:LinkButton>
                                        <asp:LinkButton ID="imgSaveCatIII" runat="server" OnClick="imgSaveCatIII_Click" ValidationGroup="SaveCatIII" Enabled="false"><i class="fa-solid fa-floppy-disk p-1 rounded-circle border"></i></asp:LinkButton>
                                        <asp:LinkButton ID="imgbtnUpdateCatIII" runat="server" ToolTip="Update Category" OnClick="imgbtnUpdateCatIII_Click" ValidationGroup="SaveCatIII" Visible="false"><i class="fa-solid fa-pen p-1 rounded-circle border"></i></asp:LinkButton>
                                        <asp:LinkButton ID="imgbtnEditCatIII" runat="server" ToolTip="Edit Category" OnClick="imgbtnEditCatIII_Click"><i class="fa-solid fa-edit p-1 rounded-circle border "></i></asp:LinkButton>
                                        <asp:LinkButton ID="imgbtnCancelCatIII" runat="server" Enabled="false" ToolTip="Refresh" OnClick="imgbtnCancelCatIII_Click"><i class="fa-solid fa-rotate p-1 rounded-circle border "></i></asp:LinkButton>
                                        <asp:RequiredFieldValidator ID="rfvtxtCatLevelIII" runat="server" ControlToValidate="txtCatLevelIII" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="SaveCatIII"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvddlCateLevelIII" runat="server" ControlToValidate="ddlCateLevelIII" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="AddCatIV"></asp:RequiredFieldValidator>
                                    </label>
                                    <asp:TextBox ID="txtCatLevelIII" runat="server" CssClass="form-control  form-control-sm" ValidationGroup="SaveCatIII" Visible="false" onkeypress="return /^[a-zA-Z0-9\s]*$/.test(event.key) && this.value.length < 50;"></asp:TextBox>
                                    <asp:DropDownList ID="ddlCateLevelIII" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" Enabled="false" AutoPostBack="true" OnSelectedIndexChanged="ddlCateLevelIII_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>

                                <div class="col-md-4">
                                    <label class="form-label">
                                        Category IV
          
                                        <asp:LinkButton ID="imgbtnCatelevelIV" runat="server" OnClick="imgbtnCatelevelIV_Click" ValidationGroup="AddCatIV"> <i class="fa-solid fa-plus p-1 rounded-circle border"></i></asp:LinkButton>
                                        <asp:LinkButton ID="imgbtnSaveCateLvlIV" runat="server" OnClick="imgbtnSaveCateLvlIV_Click" ValidationGroup="SaveCatIV" Enabled="false"><i class="fa-solid fa-floppy-disk p-1 rounded-circle border"></i></asp:LinkButton>
                                        <asp:LinkButton ID="imgbtnUpdateCateLvIV" runat="server" ToolTip="Update Category" OnClick="imgbtnUpdateCateLvIV_Click" ValidationGroup="SaveCatIV" Visible="false"><i class="fa-solid fa-pen p-1 rounded-circle border"></i></asp:LinkButton>
                                        <asp:LinkButton ID="imgbtnEditCatLvIV" runat="server" ToolTip="Edit Category" OnClick="imgbtnEditCatLvIV_Click"><i class="fa-solid fa-edit p-1 rounded-circle border "></i></asp:LinkButton>
                                        <asp:LinkButton ID="imgbtnCancelCatIV" runat="server" Enabled="false" ToolTip="Refresh" OnClick="imgbtnCancelCatIV_Click"><i class="fa-solid fa-rotate p-1 rounded-circle border "></i></asp:LinkButton>
                                        <asp:RequiredFieldValidator ID="rfvtxtCateLevelIV" runat="server" ControlToValidate="txtCateLevelIV" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="SaveCatIV"></asp:RequiredFieldValidator>
                                        <asp:RequiredFieldValidator ID="rfvddlCateLevelIV" runat="server" ControlToValidate="ddlCateLevelIV" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="AddCatV"></asp:RequiredFieldValidator>
                                    </label>
                                    <asp:TextBox ID="txtCateLevelIV" runat="server" CssClass="form-control  form-control-sm" ValidationGroup="SaveCatIV" Visible="false" onkeypress="return /^[a-zA-Z0-9\s]*$/.test(event.key) && this.value.length < 50;"></asp:TextBox>
                                    <asp:DropDownList ID="ddlCateLevelIV" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" Enabled="false" AutoPostBack="true" OnSelectedIndexChanged="ddlCateLevelIV_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>

                                <div class="col-md-4">

                                    <label class="form-label">
                                        Category V
          
                                        <asp:LinkButton ID="imgbtnAddCatV" runat="server" OnClick="imgbtnAddCatV_Click" ValidationGroup="AddCatV"> <i class="fa-solid fa-plus p-1 rounded-circle border"></i></asp:LinkButton>
                                        <asp:LinkButton ID="imgbtnSaveCatV" runat="server" OnClick="imgbtnSaveCatV_Click" ValidationGroup="SaveCatV" Enabled="false"><i class="fa-solid fa-floppy-disk p-1 rounded-circle border"></i></asp:LinkButton>
                                        <asp:LinkButton ID="imgbtnUpdateCatV" runat="server" ToolTip="Update Category" OnClick="imgbtnUpdateCatV_Click" ValidationGroup="SaveCatV" Visible="false"><i class="fa-solid fa-pen p-1 rounded-circle border"></i></asp:LinkButton>
                                        <asp:LinkButton ID="imgbtnEditCatV" runat="server" ToolTip="Edit Category" OnClick="imgbtnEditCatV_Click"><i class="fa-solid fa-edit p-1 rounded-circle border "></i></asp:LinkButton>
                                        <asp:LinkButton ID="imgbtnCancelCatV" runat="server" ToolTip="Refresh" OnClick="imgbtnCancelCatV_Click"><i class="fa-solid fa-rotate p-1 rounded-circle border "></i></asp:LinkButton>
                                        <asp:RequiredFieldValidator ID="rfvtxtCatV" runat="server" ControlToValidate="txtCatV" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="SaveCatV"></asp:RequiredFieldValidator>
                                    </label>
                                    <asp:TextBox ID="txtCatV" runat="server" CssClass="form-control  form-control-sm" ValidationGroup="SaveCatV" Visible="false" onkeypress="return /^[a-zA-Z0-9\s]*$/.test(event.key) && this.value.length < 50;"></asp:TextBox>
                                    <asp:DropDownList ID="ddlCatV" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" Enabled="false">
                                    </asp:DropDownList>
                                </div>

                            </div>
                            <div id="divres" runat="server" class="row gy-2 gx-3 mt-2">
                                <div class="col-md-3">
                                    <label class="form-label">Response Time (in Min) </label>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server" ControlToValidate="txtRespCat" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="ADDCAT"></asp:RequiredFieldValidator>
                                    <asp:TextBox ID="txtRespCat" runat="server" CssClass="form-control  form-control-sm" ToolTip="Add Category" onkeypress="return /^[0-9\s]*$/.test(event.key) && this.value.length < 50;"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <label class="form-label">Resolution Time (in Min) </label>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator27" runat="server" ControlToValidate="txtReslCat" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="ADDCAT"></asp:RequiredFieldValidator>
                                    <asp:TextBox ID="txtReslCat" runat="server" CssClass="form-control  form-control-sm" ToolTip="Add Category" onkeypress="return /^[0-9\s]*$/.test(event.key) && this.value.length < 50;"></asp:TextBox>
                                </div>
                                <div class="col-md-3">
                                    <label class="form-lable opacity-0">lfse</label><br />
                                    <asp:Button ID="btnSubmit" runat="server" Text="ADD" OnClick="btnSubmit_Click" CssClass="btn btn-grd-info btn-sm" ValidationGroup="ADDCAT" />
                                </div>
                            </div>
                            <div class="form-row d-none">
                                <div class="col-md-2 offset-10 ">
                                    <asp:Button ID="btnClose7" runat="server" Text="Close" class="btn btn-danger" OnClick="btnClose7_Click" />
                                </div>
                            </div>
                        </div>
                    </div>


                    <asp:HiddenField ID="hdnVarCategoryI" runat="server" />
                    <asp:HiddenField ID="hdnVarCategoryII" runat="server" />
                    <asp:HiddenField ID="hdnVarCategoryIII" runat="server" />
                    <asp:HiddenField ID="hdnVarCategoryIV" runat="server" />
                    <asp:HiddenField ID="hdnVarCategoryV" runat="server" />
                </ContentTemplate>

                <Triggers>
                    <asp:PostBackTrigger ControlID="imgbtnUpdateParentCategory" />
                    <asp:PostBackTrigger ControlID="imtbtnUpdateCatII" />
                    <asp:PostBackTrigger ControlID="imgbtnUpdateCatIII" />
                    <asp:PostBackTrigger ControlID="imgbtnUpdateCateLvIV" />
                    <asp:PostBackTrigger ControlID="imgbtnUpdateCatV" />
                    <asp:PostBackTrigger ControlID="imgbtnSaveParentCategory" />
                    <asp:PostBackTrigger ControlID="imgbtnSaveCatII" />
                    <asp:PostBackTrigger ControlID="imgSaveCatIII" />
                    <asp:PostBackTrigger ControlID="imgbtnSaveCateLvlIV" />
                    <asp:PostBackTrigger ControlID="imgbtnSaveCatV" />
                    <asp:PostBackTrigger ControlID="ddlOrg6" />
                    <%--Parent Controls  Category I Controls--%>
                    <asp:PostBackTrigger ControlID="ddlParentCategory" />
                    <asp:PostBackTrigger ControlID="imgbtnAddParentCategory" />
                    <%--<asp:PostBackTrigger ControlID="imgbtnSaveParentCategory" />--%>
                    <asp:PostBackTrigger ControlID="imgbtnEditParentCategory" />
                    <asp:PostBackTrigger ControlID="imgbtnCancelParent" />
                    <%--Parent Controls  Category II Controls--%>
                    <asp:PostBackTrigger ControlID="ddlCatII" />
                    <asp:PostBackTrigger ControlID="imgbtnCatII" />
                    <%--<asp:PostBackTrigger ControlID="imgbtnSaveCatII" />--%>
                    <asp:PostBackTrigger ControlID="imgbtnEditCatII" />
                    <asp:PostBackTrigger ControlID="imgbtnCancelCatII" />
                    <%--Parent Controls  Category III Controls--%>
                    <asp:PostBackTrigger ControlID="ddlCateLevelIII" />
                    <asp:PostBackTrigger ControlID="imgAddCatIII" />
                    <%--<asp:PostBackTrigger ControlID="imgSaveCatIII" />--%>
                    <asp:PostBackTrigger ControlID="imgbtnEditCatIII" />
                    <asp:PostBackTrigger ControlID="imgbtnCancelCatIII" />

                    <%--Parent Controls  Category IV Controls--%>
                    <asp:PostBackTrigger ControlID="ddlCateLevelIV" />
                    <asp:PostBackTrigger ControlID="imgbtnCatelevelIV" />
                    <%--<asp:PostBackTrigger ControlID="imgbtnSaveCateLvlIV" />--%>
                    <asp:PostBackTrigger ControlID="imgbtnEditCatLvIV" />
                    <asp:PostBackTrigger ControlID="imgbtnCancelCatIV" />
                    <%--Parent Controls  Category V Controls--%>

                    <asp:PostBackTrigger ControlID="imgbtnAddCatV" />
                    <asp:PostBackTrigger ControlID="imgbtnSaveCatV" />
                    <asp:PostBackTrigger ControlID="imgbtnEditCatV" />
                    <asp:PostBackTrigger ControlID="imgbtnCancelCatV" />
                    <asp:PostBackTrigger ControlID="lnkPreviousPriority" />
                    <asp:PostBackTrigger ControlID="lnkNextEmailConfig" />
                    <asp:PostBackTrigger ControlID="ddlRequestTypeCategory" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:Panel>
        <%--Add Category End--%>

        <%--Add Email Config Start--%>
        <asp:Panel ID="pnlAddEmailConfig" runat="server" Visible="false">
            <asp:UpdatePanel ID="updatepanel8" runat="server">
                <ContentTemplate>

                    <div class="card mb-1" style="border-radius: 0 0 0.375rem 0.375rem">
                        <div class="card-body">
                            <div class="d-flex align-items-start justify-content-between">

                                <h6 class="fw-bold mb-3">Email Configuration</h6>
                                <div class="d-flex">

                                    <asp:LinkButton class="btn btn-sm bs-stepper-circle rounded-circle btn-outline-secondary" ID="lnkPreviousCategory" runat="server" ToolTip="Previous" OnClick="lnkPreviousCategory_Click"><i class="fa-solid fa-angle-left"></i></asp:LinkButton>
                                    &nbsp;
    
                                    <asp:LinkButton class="btn btn-sm bs-stepper-circle rounded-circle btn-outline-secondary" ID="lnkNextResolution" runat="server" ToolTip="Next" OnClick="lnkNextResolution_Click"><i class="fa-solid fa-angle-right"></i></asp:LinkButton>
                                    &nbsp;
                                    
                                    <asp:LinkButton class="btn btn-sm bs-stepper-circle rounded-circle btn-outline-secondary btn btn-grd-info" ID="lnkNextResolutionSKIP" runat="server" ToolTip="Skip" OnClick="lnkNextResolution_Click"><i class="fa-solid fa-forward"></i></asp:LinkButton>
                                </div>
                            </div>
                            <div class="row gx-2 gy-3">
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Host Name
                                   
                                        <asp:RequiredFieldValidator ID="rfvtxtHostName" runat="server" ControlToValidate="txtHostName" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="EmailConfig"></asp:RequiredFieldValidator>
                                    </label>

                                    <asp:TextBox ID="txtHostName" runat="server" CssClass="form-control  form-control-sm" MaxLength="200"></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Port
                                   
                                        <asp:RequiredFieldValidator ID="rfvtxtPort" runat="server" ControlToValidate="txtPort" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="EmailConfig"></asp:RequiredFieldValidator>
                                    </label>

                                    <asp:TextBox ID="txtPort" runat="server" oninput="validateNumber(this)" CssClass="form-control  form-control-sm"></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        User Name
                                   
                                        <asp:RequiredFieldValidator ID="rfvtxtUserName" runat="server" ControlToValidate="txtUserName" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="EmailConfig"></asp:RequiredFieldValidator>
                                    </label>

                                    <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control  form-control-sm" MaxLength="200"></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Password 
                                   
                                        <asp:RequiredFieldValidator ID="rfvtxtPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="EmailConfig"></asp:RequiredFieldValidator>
                                    </label>

                                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control  form-control-sm" autocomplete="new-password"></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Sender Email
                                       
                                        <asp:RegularExpressionValidator ID="rfvtxtEmailemailval" runat="server" ControlToValidate="txtEmail"
                                            ForeColor="Red" ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
                                            Display="Dynamic" ErrorMessage="Invalid email " ValidationGroup="EmailConfig" />
                                        <asp:RequiredFieldValidator ID="rfvtxtEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="EmailConfig"></asp:RequiredFieldValidator>
                                    </label>

                                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control  form-control-sm"></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Retry Interval 
                                   
                                        <asp:RequiredFieldValidator ID="rfvtxtRetry" runat="server" ControlToValidate="txtRetry" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="EmailConfig"></asp:RequiredFieldValidator>
                                    </label>

                                    <asp:TextBox ID="txtRetry" runat="server" CssClass="form-control  form-control-sm" onkeypress="return /^[0-9\s]*$/.test(event.key) && this.value.length < 50;"></asp:TextBox>
                                </div>

                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Client ID
                                   
                                        <asp:RequiredFieldValidator ID="rfvtxtClientID" runat="server" ControlToValidate="txtClientID" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="EmailConfig"></asp:RequiredFieldValidator>
                                    </label>

                                    <asp:TextBox ID="txtClientID" runat="server" TextMode="MultiLine" CssClass="form-control  form-control-sm"></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Client Secret Key 
                                   
                                        <asp:RequiredFieldValidator ID="rfvtxtClientSecretKey" runat="server" ControlToValidate="txtClientSecretKey" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="EmailConfig"></asp:RequiredFieldValidator>
                                    </label>

                                    <asp:TextBox ID="txtClientSecretKey" runat="server" TextMode="MultiLine" CssClass="form-control  form-control-sm"></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Tenant ID
                                   
                                        <asp:RequiredFieldValidator ID="rfvtxtTenantID" runat="server" ControlToValidate="txtTenantID" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="EmailConfig"></asp:RequiredFieldValidator>
                                    </label>

                                    <asp:TextBox ID="txtTenantID" runat="server" TextMode="MultiLine" CssClass="form-control  form-control-sm"></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Organization
                                   
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="ddlOrgEmailConfig" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="EmailConfig"></asp:RequiredFieldValidator>
                                    </label>


                                    <asp:DropDownList Enabled="false" ID="ddlOrgEmailConfig" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-8 text-end">
                                    <label class="form-label"></label>
                                    <br />
                                    <asp:Button ID="btnInsertEmailConfig" runat="server" Text="Save" class="btn btn-sm btn-grd-info" OnClick="btnInsertEmailConfig_Click" ValidationGroup="EmailConfig" />
                                    <asp:Button ID="btnUpdateEmailConfig" runat="server" Text="Update" Visible="false" class="btn btn-sm btn-grd-info" OnClick="btnUpdateEmailConfig_Click" ValidationGroup="EmailConfig" />
                                    <asp:Button ID="btnCancel8" runat="server" Text="Refresh" class="btn btn-sm btn-grd-danger" OnClick="btnCancel8_Click" CausesValidation="false" />

                                </div>
                            </div>
                        </div>
                    </div>


                    <div class="card">
                        <div class="card-body">
                            <div class="d-flex align-items-start justify-content-between mb-3">
                                <div class="">
                                    <h6 class="mb-0">
                                        <asp:Label ID="Label18" runat="server" Text="EmailConfig Details"></asp:Label>
                                    </h6>
                                </div>

                                <asp:LinkButton ID="ImgBtnExport8" runat="server" CssClass="btn btn-sm btn-outline-secondary" OnClick="ImgBtnExport8_Click">Export <i class="fa-solid fa-download"></i></asp:LinkButton>
                            </div>
                            <div class="row ">
                                <div class="col-md-12">
                                    <div class="table-responsive table-container">
                                        <asp:GridView GridLines="None" ID="gvEmailConfig" runat="server" DataKeyNames="ID" AutoGenerateColumns="false" CssClass="data-table table table-striped border table-sm text-nowrap"
                                            Width="100%" OnRowCommand="gvEmailConfig_RowCommand" OnRowDataBound="gvEmailConfig_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="20">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Hostname" HeaderText="Host Name" NullDisplayText="NA" />
                                                <asp:BoundField DataField="Port" HeaderText="Port" NullDisplayText="NA" />
                                                <asp:BoundField DataField="UserName" HeaderText="UserName" NullDisplayText="NA" />
                                                <asp:BoundField DataField="Email" HeaderText="Email" NullDisplayText="NA" />
                                                <asp:TemplateField HeaderText="Password" ItemStyle-Width="20">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl" runat="server" Text="*********"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Retry" HeaderText="Retry" NullDisplayText="NA" />
                                                <asp:BoundField DataField="ClientID" HeaderText="ClientID" NullDisplayText="NA" />
                                                <asp:BoundField DataField="ClientSecretKey" HeaderText="ClientSecretKey" NullDisplayText="NA" />
                                                <asp:BoundField DataField="TenantID" HeaderText="TenantID" NullDisplayText="NA" />
                                                <asp:TemplateField HeaderText=" Organization">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOrgFk" runat="server" Text='<%# Eval("Org_ID") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblOrgName" runat="server" Text='<%# Eval("OrgName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%-- <asp:ButtonField ButtonType="Image" CommandName="UpdateEmailConfig" HeaderText="Edit" ImageUrl="~/images/edit23.png" ItemStyle-Width="20px" />
                                                <asp:ButtonField HeaderText="Delete" ButtonType="Image" ImageUrl="~/Images/New folder/delnew.png" CommandName="DeleteEmailConfig" ItemStyle-Width="20px" ItemStyle-Height="5px" />--%>
                                                <asp:TemplateField HeaderText="Edit">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkEdit11" runat="server" CommandName="UpdateEmailConfig" CommandArgument="<%# Container.DataItemIndex %>" ToolTip="Edit">
                                                        <i class="fa-solid fa-edit"></i>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delete">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDelete11" runat="server" CommandName="DeleteEmailConfig" CommandArgument="<%# Container.DataItemIndex %>" ToolTip="Delete" OnClientClick="return confirm('Are you sure you want to delete this record?');">
                                                        <i class="fa-solid fa-xmark text-danger"></i> 
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>

                                </div>


                            </div>
                        </div>
                    </div>


                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="ImgBtnExport" />
                    <asp:PostBackTrigger ControlID="gvEmailConfig" />
                    <asp:PostBackTrigger ControlID="lnkPreviousCategory" />
                    <asp:PostBackTrigger ControlID="lnkNextResolution" />
                    <asp:PostBackTrigger ControlID="ImgBtnExport8" />
                    <asp:PostBackTrigger ControlID="lnkNextResolutionSKIP" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:Panel>
        <%--Add Email Config End--%>

        <%--Add Resolution Start--%>
        <asp:Panel ID="pnlAddResolution" runat="server" Visible="false">
            <asp:UpdatePanel ID="updatepanel9" runat="server">
                <ContentTemplate>

                    <div class="card mb-1" style="border-radius: 0 0 0.375rem 0.375rem">
                        <div class="card-body">
                            <div class="d-flex align-items-start justify-content-between">
                                <h6 class="fw-bold mb-3">Add Resolution</h6>
                                <div class="d-flex">

                                    <asp:LinkButton class="btn btn-sm bs-stepper-circle rounded-circle btn-outline-secondary" ToolTip="Previous" ID="lnkPreviousEmailConfig" runat="server" OnClick="lnkPreviousEmailConfig_Click"><i class="fa-solid fa-angle-left"></i></asp:LinkButton>
                                    &nbsp;
    
                                    <asp:LinkButton class="btn btn-sm bs-stepper-circle rounded-circle btn-outline-secondary" ID="lnkNextSLA" runat="server" ToolTip="Next" OnClick="lnkNextSLA_Click"><i class="fa-solid fa-angle-right"></i></asp:LinkButton>
                                </div>
                            </div>

                            <div class="row gx-2 gy-3">
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Organization
                                           
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="ddlOrgResolution" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Resol"></asp:RequiredFieldValidator>
                                    </label>

                                    <asp:DropDownList Enabled="false" ID="ddlOrgResolution" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlOrgResolution_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>

                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Request Type
                                           
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="ddlRequestTypeResolution" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Resol"></asp:RequiredFieldValidator>
                                    </label>

                                    <asp:DropDownList Enabled="false" ID="ddlRequestTypeResolution" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlRequestType_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Resolution Name
                                           
                                        <asp:RequiredFieldValidator ID="rfvtxtResolution" runat="server" ControlToValidate="txtResolution" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Resol"></asp:RequiredFieldValidator>
                                    </label>

                                    <asp:TextBox onkeypress="return /^[a-zA-Z\s]*$/.test(event.key) && this.value.length < 50;" MaxLength="50"
                                        ID="txtResolution" runat="server" CssClass="form-control  form-control-sm"></asp:TextBox>
                                </div>
                                <div class="col-md-12">
                                    <label for="staticEmail" class="form-label">
                                        Resolution Description 
                                           
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="txtResolutnDesc" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Resol"></asp:RequiredFieldValidator>
                                    </label>

                                    <asp:TextBox ID="txtResolutnDesc" MaxLength="1000" runat="server" TextMode="MultiLine" Rows="5" Columns="5" CssClass="form-control  form-control-sm"></asp:TextBox>
                                </div>
                                <div class="col-12 text-end">
                                    <asp:Button ID="btnInsertResolution" runat="server" Text="Save" class="btn btn-sm btn-grd-info" OnClick="btnInsertResolution_Click" ValidationGroup="Resol" />
                                    <asp:Button ID="btnUpdateResolution" runat="server" Text="Update" Visible="false" class="btn btn-sm btn-grd-info" OnClick="btnUpdateResolution_Click" ValidationGroup="Resol" />
                                    <asp:Button ID="btnCancel9" runat="server" Text="Refresh" class="btn btn-sm btn-grd-danger" OnClick="btnCancel9_Click" CausesValidation="false" />
                                </div>

                            </div>
                        </div>
                    </div>

                    <div class="card ">
                        <div class="card-body">
                            <div class="d-flex align-items-start justify-content-between mb-3">
                                <div class="">
                                    <h6 class="mb-0">
                                        <asp:Label ID="Label21" runat="server" Text="Resolution Details"></asp:Label>
                                    </h6>
                                </div>

                                <asp:LinkButton ID="ImgBtnExport9" runat="server" CssClass="btn btn-sm btn-outline-secondary" OnClick="ImgBtnExport9_Click">Export <i class="fa-solid fa-download"></i></asp:LinkButton>
                            </div>

                            <div class="row ">
                                <div class="col-md-12">
                                    <div class="table-responsive table-container">
                                        <asp:GridView GridLines="None" ID="gvResolution" runat="server" DataKeyNames="ResolutionRef" AutoGenerateColumns="false" CssClass="table border table-sm table-striped text-nowrap"
                                            Width="100%" OnRowCommand="gvResolution_RowCommand" OnRowDataBound="gvResolution_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="20">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="DeskRef" HeaderText="Request Type" NullDisplayText="NA" />
                                                <asp:BoundField DataField="ResolutionCodeRef" HeaderText=" Resolution Name" NullDisplayText="NA" />
                                                <asp:BoundField DataField="ResolutionDesc" HeaderText="Resolution Description" NullDisplayText="NA" />
                                                <asp:TemplateField HeaderText=" Organization">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOrgFk" runat="server" Text='<%# Eval("Org_ID") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblOrgName" runat="server" Text='<%# Eval("OrgName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%-- <asp:ButtonField ButtonType="Image" CommandName="SelectState" HeaderText="Edit" ImageUrl="~/images/edit23.png" ItemStyle-Width="20px" />
                                                <asp:ButtonField HeaderText="Delete" ButtonType="Image" ImageUrl="~/Images/New folder/delnew.png" CommandName="DeleteEx" ItemStyle-Width="20px" ItemStyle-Height="5px" />--%>
                                                <asp:TemplateField HeaderText="Edit">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkEdit7" runat="server" CommandName="SelectState" CommandArgument="<%# Container.DataItemIndex %>" ToolTip="Edit">
                                                        <i class="fa-solid fa-edit"></i>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delete">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDelete8" runat="server" CommandName="DeleteEx" CommandArgument="<%# Container.DataItemIndex %>" ToolTip="Delete" OnClientClick="return confirm('Are you sure you want to delete this record?');">
                                                        <i class="fa-solid fa-xmark text-danger"></i> 
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>



                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="ImgBtnExport9" />
                    <asp:PostBackTrigger ControlID="ddlOrgResolution" />
                    <asp:PostBackTrigger ControlID="ddlRequestTypeResolution" />
                    <asp:PostBackTrigger ControlID="gvResolution" />
                    <asp:PostBackTrigger ControlID="lnkPreviousEmailConfig" />
                    <asp:PostBackTrigger ControlID="lnkNextSLA" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:Panel>
        <%--Add Resolution End--%>

        <%--Add SLA Start--%>
        <asp:Panel ID="pnlAddSLA" runat="server" Visible="false">
            <asp:UpdatePanel ID="updatepanel10" runat="server">
                <ContentTemplate>

                    <div class="card mb-1" style="border-radius: 0 0 0.375rem 0.375rem">
                        <div class="card-body">
                            <div class="d-flex align-items-start justify-content-between">
                                <h6
                                    class="mb-0 fw-bold">Add SLA</h6>
                                <div class="d-flex">

                                    <asp:LinkButton class="btn btn-sm bs-stepper-circle rounded-circle btn-outline-secondary" ToolTip="Previous" ID="lnkPreviousResolution" runat="server" OnClick="lnkPreviousResolution_Click"><i class="fa-solid fa-angle-left"></i></asp:LinkButton>
                                    &nbsp;
    
                                    <asp:LinkButton class="btn btn-sm bs-stepper-circle rounded-circle btn-outline-secondary" ID="lnkNextDeskConfig" ToolTip="Next" runat="server" OnClick="lnkNextDeskConfig_Click"><i class="fa-solid fa-angle-right"></i></asp:LinkButton>
                                </div>
                            </div>

                            <div class="row gx-2 gy-3">
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Organization
                                           
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="ddlOrgSLA" ErrorMessage="*" Font-Bold="true" InitialValue="0" ForeColor="Red" ValidationGroup="SLA"></asp:RequiredFieldValidator>
                                    </label>
                                    <asp:DropDownList Enabled="false" ID="ddlOrgSLA" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Request Type
           
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ControlToValidate="ddlReqSLA" ErrorMessage="*" Font-Bold="true" InitialValue="0" ForeColor="Red" ValidationGroup="SLA"></asp:RequiredFieldValidator>
                                    </label>
                                    <asp:DropDownList Enabled="false" ID="ddlReqSLA" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-12 ">
                                    <label for="staticEmail" class="form-label">
                                        SLA Name
                                           
                                        <%--<asp:RequiredFieldValidator ID="rfvtxtSLAName" runat="server" ControlToValidate="txtSLAName" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="SLA"></asp:RequiredFieldValidator>--%>
                                    </label>
                                    <asp:RadioButton ID="rbdCategory" runat="server" GroupName="theme" Text="Category" CssClass="px-2" />
                                    <asp:RadioButton ID="rbdSeverity" runat="server" GroupName="theme" Text="Severity" CssClass="px-2" />
                                    <asp:RadioButton ID="rbdPriority" runat="server" GroupName="theme" Text="Priority" CssClass="px-2" />
                                    <%--<asp:TextBox ID="txtSLAName" runat="server" CssClass="form-control  form-control-sm"></asp:TextBox>--%>
                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        SLA Description
                                           
                                        <asp:RequiredFieldValidator ID="rfvtxtSLADescription" runat="server" ControlToValidate="txtSLADescription" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="SLA"></asp:RequiredFieldValidator>
                                    </label>

                                    <asp:TextBox ID="txtSLADescription" runat="server" CssClass="form-control  form-control-sm"></asp:TextBox>
                                </div>
                                <div class="col-12 text-end">
                                    <asp:Button ID="btnInsertSLA" runat="server" Text="Save" class="btn btn-sm btn-grd-info" OnClick="btnInsertSLA_Click" ValidationGroup="SLA" />
                                    <asp:Button ID="btnUpdateSLA" runat="server" Text="Update" Visible="false" class="btn btn-sm btn-grd-info" OnClick="btnUpdateSLA_Click" ValidationGroup="SLA" />
                                    <asp:Button ID="btnCancel10" runat="server" Text="Refresh" class="btn btn-sm btn-grd-danger" OnClick="btnCancel10_Click" CausesValidation="false" />
                                </div>

                            </div>
                        </div>
                    </div>

                    <div class="card">

                        <div class="card-body">
                            <div class="d-flex align-items-start justify-content-between mb-3">
                                <div class="">
                                    <h6 class="mb-0">Add Details</h6>
                                </div>

                                <asp:LinkButton ID="ImgBtnExport10" CssClass="btn btn-sm btn-outline-secondary" runat="server" OnClick="ImgBtnExport10_Click">Export <i class="fa-solid fa-download"></i></asp:LinkButton>
                            </div>

                            <div class="row ">
                                <div class="col-md-12">
                                    <div class="table-responsive table-container">

                                        <asp:GridView GridLines="None" ID="gvSLA" runat="server" DataKeyNames="ID" AutoGenerateColumns="false" CssClass="table table-head-fixed text-nowrap text-nowrap table-sm table-striped border"
                                            Width="100%" OnRowCommand="gvSLA_RowCommand" OnRowDataBound="gvSLA_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="20">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="SlaName" HeaderText="SLA Name" NullDisplayText="NA" />
                                                <asp:BoundField DataField="SLADesc" HeaderText="SLA Description" NullDisplayText="NA" />
                                                <asp:TemplateField HeaderText=" Organization">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOrgFk" runat="server" Text='<%# Eval("Org_ID") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblOrgName" runat="server" Text='<%# Eval("OrgName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Request Type">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblReqType" runat="server" Text='<%# Eval("ReqType") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%-- <asp:ButtonField ButtonType="Image" CommandName="UpdateSLA" HeaderText="Edit" ImageUrl="~/images/edit23.png" ItemStyle-Width="20px" />
                                                <asp:ButtonField HeaderText="Delete" ButtonType="Image" ImageUrl="~/Images/New folder/delnew.png" CommandName="DeleteSLA" ItemStyle-Width="20px" ItemStyle-Height="5px" />--%>
                                                <asp:TemplateField HeaderText="Edit">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkEdit8" runat="server" CommandName="UpdateSLA" CommandArgument="<%# Container.DataItemIndex %>" ToolTip="Edit">
                                                    <i class="fa-solid fa-edit"></i>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delete">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDelete9" runat="server" CommandName="DeleteSLA" CommandArgument="<%# Container.DataItemIndex %>" ToolTip="Delete" OnClientClick="return confirm('Are you sure you want to delete this record?');">
                                                  <i class="fa-solid fa-xmark text-danger"></i> 
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>

                                </div>
                            </div>

                        </div>
                    </div>



                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="ImgBtnExport10" />
                    <asp:PostBackTrigger ControlID="gvSLA" />
                    <asp:PostBackTrigger ControlID="lnkPreviousResolution" />
                    <asp:PostBackTrigger ControlID="lnkNextDeskConfig" />
                </Triggers>

            </asp:UpdatePanel>
        </asp:Panel>
        <%--Add SLA End--%>

        <%--Add DeskConfig Start--%>
        <asp:Panel ID="pnlAdddeskConfig" runat="server" Visible="false">
            <asp:UpdatePanel ID="UpdatePanel11" runat="server">
                <ContentTemplate>

                    <div class="card mb-1" style="border-radius: 0 0 0.375rem 0.375rem">
                        <div class="card-body">
                            <div class="d-flex align-items-start justify-content-between">
                                <h6 class="mb-3 fw-bold">Desk Template</h6>
                                <div class="d-flex">

                                    <asp:LinkButton class="btn btn-sm bs-stepper-circle rounded-circle btn-outline-secondary" ToolTip="Previous" ID="lnkPreviousSLA" runat="server" OnClick="lnkPreviousSLA_Click"><i class="fa-solid fa-angle-left"></i></asp:LinkButton>
                                    &nbsp;
    
                                    <asp:LinkButton class="btn btn-sm bs-stepper-circle rounded-circle btn-outline-secondary" ID="lnkNextCustomFields" ToolTip="Next" runat="server" OnClick="lnkNextCustomFields_Click"><i class="fa-solid fa-angle-right"></i></asp:LinkButton>
                                </div>
                            </div>

                            <asp:Literal ID="ltlCount" runat="server" Text="0" Visible="false" />
                            <asp:Literal ID="ltlRemoved" runat="server" Visible="false" />
                            <div class="row gx-2 gy-3">
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Organization
                                               
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ControlToValidate="ddlOrgDeskConfig" ErrorMessage="*" ForeColor="Red" InitialValue="0" ValidationGroup="www"></asp:RequiredFieldValidator>
                                    </label>

                                    <asp:DropDownList Enabled="false" ID="ddlOrgDeskConfig" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlOrgDeskConfig_SelectedIndexChanged"></asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Request Type
                                   
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ControlToValidate="ddlRequestTypeDeskConfig" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="www"></asp:RequiredFieldValidator>
                                    </label>

                                    <asp:DropDownList Enabled="false" ID="ddlRequestTypeDeskConfig" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlRequestTypeDeskConfig_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>

                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Service PreFix 
                                   
                                        <asp:RequiredFieldValidator ID="rfvtxtSDPrefix" runat="server" ControlToValidate="txtSDPrefix" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="www"></asp:RequiredFieldValidator>
                                    </label>

                                    <asp:TextBox ID="txtSDPrefix" runat="server" MaxLength="4" CssClass="form-control  form-control-sm" onkeypress="return /^[a-zA-Z]*$/.test(event.key) && this.value.length < 4;"></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Service Desk Description 
                                       
                                        <asp:RequiredFieldValidator ID="rfvtxtSDDescription" runat="server" ControlToValidate="txtSDDescription" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="www"></asp:RequiredFieldValidator>
                                    </label>

                                    <asp:TextBox ID="txtSDDescription" runat="server" TextMode="MultiLine" Rows="1" CssClass="form-control  form-control-sm"></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Archive Time(in Days)
                                   
                                        <asp:RequiredFieldValidator ID="rfvtxtArchiveTime" runat="server" ControlToValidate="txtArchiveTime" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="www"></asp:RequiredFieldValidator>
                                    </label>

                                    <asp:TextBox ID="txtArchiveTime" runat="server" CssClass="form-control  form-control-sm" onkeypress="return /^[0-9]*$/.test(event.key) && this.value.length < 4;"></asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Severity 
                                               
                                        <asp:RequiredFieldValidator ID="RfvddlSeverity" runat="server" ControlToValidate="ddlSeverity" ErrorMessage="*" ForeColor="Red" InitialValue="0" ValidationGroup="www" Enabled="false"></asp:RequiredFieldValidator>
                                        &nbsp;</label>

                                    <asp:DropDownList ID="ddlSeverity" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field"></asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        <asp:Label ID="lblSolution" runat="server" Text="Solution Type "></asp:Label>
                                        <asp:RequiredFieldValidator ID="RfvddlSolutionType" runat="server" ControlToValidate="ddlSolutionType" ErrorMessage="*" ForeColor="Red" InitialValue="0" ValidationGroup="www"></asp:RequiredFieldValidator>
                                    </label>

                                    <asp:DropDownList ID="ddlSolutionType" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field"></asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Priority 
                                               
                                        <asp:RequiredFieldValidator ID="rfvddlPriority" runat="server" ControlToValidate="ddlPriority" ErrorMessage="*" ForeColor="Red" InitialValue="0" ValidationGroup="www"></asp:RequiredFieldValidator>
                                    </label>

                                    <asp:DropDownList ID="ddlPriority" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field"></asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Coverage Schedule
							
                                        <asp:RequiredFieldValidator ID="rfvddlCoverageSch" runat="server" ControlToValidate="ddlCoverageSch" ErrorMessage="*" ForeColor="Red" InitialValue="0" ValidationGroup="www"></asp:RequiredFieldValidator>
                                    </label>

                                    <asp:DropDownList ID="ddlCoverageSch" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field"></asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Stage 
                                               
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ControlToValidate="ddlStageDeskConfig" ErrorMessage="*" ForeColor="Red" InitialValue="0" ValidationGroup="www"></asp:RequiredFieldValidator>
                                    </label>

                                    <asp:DropDownList ID="ddlStageDeskConfig" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlStageDeskConfig_SelectedIndexChanged"></asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Status
                                               
                                        <asp:RequiredFieldValidator ID="rfvStatus" runat="server" ControlToValidate="ddlStatus" ErrorMessage="*" ForeColor="Red" InitialValue="0" ValidationGroup="www"></asp:RequiredFieldValidator>
                                    </label>

                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field"></asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        SLA 
                                       
                                        <asp:RequiredFieldValidator ID="rfv" runat="server" ControlToValidate="ddlSlA" ErrorMessage="*" ForeColor="Red" InitialValue="0" ValidationGroup="www"></asp:RequiredFieldValidator>
                                    </label>

                                    <asp:DropDownList ID="ddlSlA" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field"></asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        <asp:Label ID="lblCategory1" runat="server" Text="Category 1"></asp:Label>
                                        <asp:RequiredFieldValidator ID="RfvddlCategory1" runat="server" ControlToValidate="ddlCategory1" ValidationGroup="www" ForeColor="Red" ErrorMessage="*" InitialValue="0"></asp:RequiredFieldValidator>
                                    </label>

                                    <asp:DropDownList ID="ddlCategory1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCategory1_SelectedIndexChanged" CssClass="form-select form-select-sm single-select-optgroup-field"></asp:DropDownList>
                                </div>

                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        <asp:Label ID="lblCategory2" runat="server" Text="Category 2 "></asp:Label>
                                        <asp:RequiredFieldValidator ID="RfvddlCategory2" runat="server" InitialValue="0" ControlToValidate="ddlCategory2" ValidationGroup="www" ForeColor="Red" ErrorMessage="*" Enabled="False"></asp:RequiredFieldValidator>
                                    </label>

                                    <asp:DropDownList ID="ddlCategory2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCategory2_SelectedIndexChanged" CssClass="form-select form-select-sm single-select-optgroup-field"></asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        <asp:Label ID="lblCategory3" runat="server" Text="Category 3"></asp:Label>
                                        <asp:RequiredFieldValidator ID="RfvddlCategory3" runat="server" InitialValue="0" ControlToValidate="ddlCategory3" ValidationGroup="www" ForeColor="Red" ErrorMessage="*" Enabled="False"></asp:RequiredFieldValidator>
                                    </label>

                                    <asp:DropDownList ID="ddlCategory3" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCategory3_SelectedIndexChanged" CssClass="form-select form-select-sm single-select-optgroup-field"></asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        <asp:Label ID="lblCategory4" runat="server" Text="Category 4"></asp:Label>
                                    </label>

                                    <asp:DropDownList ID="ddlCategory4" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory4_SelectedIndexChanged"></asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        <asp:Label ID="lblCategory5" runat="server" Text="Category 5"></asp:Label>
                                    </label>

                                    <asp:DropDownList ID="ddlCategory5" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true"></asp:DropDownList>
                                </div>
                                <div class="col-md-4 text-end">
                                    <div class="form-label opacity-0">..</div>
                                    <asp:Button ID="btnInsert" runat="server" Text="Save" class="btn btn-sm btn-grd-info" OnClick="btnInsert_Click" ValidationGroup="www" />
                                    <asp:Button ID="btnUpdate" runat="server" Text="Update" class="btn btn-sm btn-grd-info" OnClick="btnUpdate_Click" ValidationGroup="wwww" Visible="False" />
                                    <asp:Button ID="btnCancel11" runat="server" Text="Refresh" class="btn btn-sm btn-grd-danger" OnClick="btnCancel11_Click" CausesValidation="false" />

                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="card">
                        <div class="card-body">
                            <div class="d-flex align-items-start justify-content-between mb-3">
                                <div class="">
                                    <h6 class="mb-0">
                                        <asp:Label ID="Label19" runat="server" Text="Desk Details"></asp:Label>
                                    </h6>
                                </div>
                                <asp:LinkButton ID="ImgBtnExport12" runat="server" CssClass="btn btn-sm btn-outline-secondary" OnClick="ImgBtnExport12_Click">Export <i class="fa-solid fa-download"></i></asp:LinkButton>

                            </div>

                            <div class="row ">
                                <div class="col-md-12">
                                    <div class="table-responsive table-container">
                                        <asp:GridView GridLines="None" ID="gvDesk" runat="server" DataKeyNames="ID" AutoGenerateColumns="false" CssClass="table table-sm table-striped text-nowrap"
                                            Width="100%" OnRowCommand="gvDesk_RowCommand" OnRowDataBound="gvDesk_RowDataBound">
                                            <Columns>
                                                <%--<asp:ButtonField ButtonType="Image" CommandName="EditDesk" HeaderText="Edit" ImageUrl="~/images/edit23.png" ItemStyle-Width="20px" />
                                                <asp:ButtonField HeaderText="Delete" ButtonType="Image" ImageUrl="~/Images/New folder/delnew.png" CommandName="DeleteEx" ItemStyle-Width="20px" ItemStyle-Height="5px" />--%>
                                                <asp:TemplateField HeaderText="Edit">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkEdit9" runat="server" CommandName="EditDesk" CommandArgument="<%# Container.DataItemIndex %>" ToolTip="Edit">
                                                        <i class="fa-solid fa-edit"></i>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delete">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDelete10" runat="server" CommandName="DeleteEx" CommandArgument="<%# Container.DataItemIndex %>" ToolTip="Delete" OnClientClick="return confirm('Are you sure you want to delete this record?');">
                                                        <i class="fa-solid fa-xmark text-danger"></i> 
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="20">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="TemplateName" HeaderText="Template Name" NullDisplayText="NA" />
                                                <asp:BoundField DataField="DeskRef" HeaderText="Request Type" NullDisplayText="NA" />
                                                <asp:BoundField DataField="DeskPrefix" HeaderText="SD Prefix" NullDisplayText="NA" />
                                                <asp:BoundField DataField="DeskDesc" HeaderText="Desk Desc" NullDisplayText="NA" />
                                                <asp:TemplateField HeaderText="SD Category">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSDCategoryFk" runat="server" Text='<%# Eval("sdCategoryFK") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblSDCategoryName" runat="server" Text='<%# Eval("Category") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText=" Stage">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSDStageFk" runat="server" Text='<%# Eval("sdStageFK") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblSDStageName" runat="server" Text='<%# Eval("StageCodeRef") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText=" Status">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSDStatusFk" runat="server" Text='<%# Eval("sdStatusFK") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblSDStatusName" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText=" Priority">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSDPriorityFk" runat="server" Text='<%# Eval("sdPriorityFK") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblSDPriorityName" runat="server" Text='<%# Eval("Priority") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText=" Severity">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSDSeverityFk" runat="server" Text='<%# Eval("sdSeverityFK") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblSDSeverityName" runat="server" Text='<%# Eval("Severity") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText=" Resolution">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblsdSolutionTypeFK" runat="server" Text='<%# Eval("sdSolutionTypeFK") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblSDResolutionName" runat="server" Text='<%# Eval("Resolution") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="autoArchiveTime" HeaderText="Archive Time" NullDisplayText="0" />
                                                <asp:TemplateField HeaderText=" SLA">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblSLAid" runat="server" Text='<%# Eval("SLAID") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblSLAName" runat="server" Text='<%# Eval("SLAName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText=" Coverage Sch">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCvrgID" runat="server" Text='<%# Eval("CoverageID") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblCvrgName" runat="server" Text='<%# Eval("CoverageName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText=" Organization">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOrgFk" runat="server" Text='<%# Eval("OrgFk") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblOrgName" runat="server" Text='<%# Eval("OrgName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>

                        </div>
                    </div>


                    <asp:HiddenField ID="hdnCategoryID" runat="server" />
                    <asp:HiddenField ID="HiddenField1" runat="server" />
                    <asp:HiddenField ID="HiddenField2" runat="server" />
                    <asp:HiddenField ID="HiddenField3" runat="server" />
                    <asp:HiddenField ID="HiddenField4" runat="server" />
                    <asp:HiddenField ID="HiddenField5" runat="server" />
                </ContentTemplate>
                <Triggers>

                    <asp:PostBackTrigger ControlID="ddlSolutionType" />
                    <asp:PostBackTrigger ControlID="ddlPriority" />
                    <asp:PostBackTrigger ControlID="ddlCoverageSch" />
                    <asp:PostBackTrigger ControlID="ddlStageDeskConfig" />
                    <asp:PostBackTrigger ControlID="ddlStatus" />
                    <asp:PostBackTrigger ControlID="ddlSlA" />
                    <asp:PostBackTrigger ControlID="ddlCategory1" />
                    <asp:PostBackTrigger ControlID="ddlCategory2" />
                    <asp:PostBackTrigger ControlID="ddlCategory3" />
                    <asp:PostBackTrigger ControlID="ddlCategory4" />
                    <asp:PostBackTrigger ControlID="ddlCategory5" />
                    <asp:PostBackTrigger ControlID="gvDesk" />

                    <asp:PostBackTrigger ControlID="ddlOrgDeskConfig" />
                    <asp:PostBackTrigger ControlID="ddlRequestTypeDeskConfig" />
                    <asp:PostBackTrigger ControlID="ddlSeverity" />
                    <asp:PostBackTrigger ControlID="lnkPreviousSLA" />
                    <asp:PostBackTrigger ControlID="lnkNextCustomFields" />
                    <asp:PostBackTrigger ControlID="ddlStageDeskConfig" />
                    <asp:PostBackTrigger ControlID="ImgBtnExport12" />
                    <asp:PostBackTrigger ControlID="btnCancel11" />
                    <asp:PostBackTrigger ControlID="btnInsert" />
                </Triggers>
            </asp:UpdatePanel>
        </asp:Panel>
        <%--Add DeskConfig End--%>

        <%--Add Custom Fields Start--%>
        <%--<asp:Panel runat="server" ID="pnlAddCustomFields" Visible="false">
                <asp:UpdatePanel ID="updatepanel12" runat="server">
                    <ContentTemplate>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="card">
                                    <div class="card-body">
                                        <div class="form-group row mt-3">
                                            <label for="staticEmail" class="form-label">
                                                Organization: <span title="*"></span>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ControlToValidate="ddlOrg" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="ReqType"></asp:RequiredFieldValidator>
                                            </label>
                                            <div class="col-sm-4 pr-5">
                                                <asp:DropDownList ID="ddlOrgCustomField" runat="server" CssClass="form-control form-control-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlOrgCustomField_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>
                                            <label for="staticEmail" class="col-sm-2 labelcolorl1 pl-5">
                                                Request Type: <span title="*"></span>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ControlToValidate="ddlRequestType" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                                            </label>
                                            <div class="col-sm-4 pr-5">
                                                <asp:DropDownList ID="ddlRequestTypeCustomField" runat="server" CssClass="form-control form-control-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlRequestTypeCustomField_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group row mt-3">
                                            <label for="staticEmail" class="col-sm-2 labelcolorl1 pl-5">
                                                SD Role : <span title="*"></span>
                                                <asp:RequiredFieldValidator ID="rfvddlSDRole" runat="server" ControlToValidate="ddlSDRole" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                                            </label>

                                            <div class="col-sm-4 pr-5">
                                                <asp:DropDownList ID="ddlSDRole" runat="server" CssClass="form-control form-control-sm single-select-optgroup-field">
                                                    <asp:ListItem Selected="True" Text="----Select Role----" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="Master" Value="Master"></asp:ListItem>
                                                    <asp:ListItem Text="ITManager" Value="ITManager"></asp:ListItem>
                                                    <asp:ListItem Text="CRM" Value="CRM"></asp:ListItem>
                                                    <asp:ListItem Text="ITEngineer" Value="ITEngineer"></asp:ListItem>
                                                    <asp:ListItem Text="UAT" Value="UAT"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <label for="staticEmail" class="col-sm-2 labelcolorl1 pl-5">
                                                Field Type : <span title="*"></span>
                                                <asp:RequiredFieldValidator ID="rfvddlFieldType" runat="server" ControlToValidate="ddlFieldType" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="SDCustomFields"></asp:RequiredFieldValidator>
                                            </label>
                                            <div class="col-sm-4 pr-5">
                                                <asp:DropDownList ID="ddlFieldType" runat="server" CssClass="form-control  form-control-sm">
                                                    <asp:ListItem Text="---Select Field---" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="TextBox" Value="TextBox"></asp:ListItem>
                                                    <asp:ListItem Text="List" Value="DropDown"></asp:ListItem>
                                                </asp:DropDownList>

                                            </div>
                                        </div>
                                        <div class="form-group row mt-3">
                                            <label for="staticEmail" class="col-sm-2 labelcolorl1 pl-5">
                                                Field Name : <span title="*"></span>
                                                <asp:RequiredFieldValidator ID="rfvtxtFieldName" runat="server" ControlToValidate="txtFieldName" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="SDCustomFields"></asp:RequiredFieldValidator>
                                            </label>

                                            <div class="col-sm-4 pr-5">
                                                <asp:TextBox ID="txtFieldName" runat="server" TextMode="SingleLine" CssClass="form-control  form-control-sm"></asp:TextBox>
                                            </div>
                                            <label for="staticEmail" class="col-sm-2 labelcolorl1 pl-5">
                                                Field Mode : <span title="*"></span>
                                                <asp:RequiredFieldValidator ID="rfvddlFieldMode" runat="server" ControlToValidate="ddlFieldMode" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="SDCustomFields"></asp:RequiredFieldValidator>
                                            </label>
                                            <div class="col-sm-4 pr-5">
                                                <asp:DropDownList ID="ddlFieldMode" runat="server" CssClass="form-control  form-control-sm">
                                                    <asp:ListItem Text="---Select Mode---" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="DateTime" Value="DateTime"></asp:ListItem>
                                                    <asp:ListItem Text="Number" Value="int"></asp:ListItem>
                                                    <asp:ListItem Text="SingleLine" Value="varchar(500)"></asp:ListItem>
                                                    <asp:ListItem Text="MultiLine" Value="varchar(max)"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group row mt-3">
                                            <label for="staticEmail" class="col-sm-2 labelcolorl1 pl-5">
                                                IS Visible : <span title="*"></span>
                                                <asp:RequiredFieldValidator ID="frvddlVisibilty" runat="server" ControlToValidate="ddlVisibilty" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="SDCustomFields"></asp:RequiredFieldValidator>
                                            </label>

                                            <div class="col-sm-4 pr-5">
                                                <asp:DropDownList ID="ddlVisibilty" runat="server" CssClass="form-control  form-control-sm">
                                                    <asp:ListItem Text="---Select Visibilty---" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="True" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="False" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <label for="staticEmail" class="col-sm-2 labelcolorl1 pl-5">
                                                Is Required: <span title="*"></span>
                                                <asp:RequiredFieldValidator ID="rfvddlRequiredStatus" runat="server" ControlToValidate="ddlRequiredStatus" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="SDCustomFields"></asp:RequiredFieldValidator>
                                            </label>
                                            <div class="col-sm-4 pr-5">
                                                <asp:DropDownList ID="ddlRequiredStatus" runat="server" CssClass="form-control  form-control-sm">
                                                    <asp:ListItem Text="---Select ---" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="True" Value="1"></asp:ListItem>
                                                    <asp:ListItem Text="False" Value="0"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-group row mt-3">
                                            <label for="staticEmail" class="col-sm-2 labelcolorl1 pl-5">
                                                Field Scope : <span title="*"></span>
                                                <asp:RequiredFieldValidator ID="rfvddlFieldScope" runat="server" ControlToValidate="ddlFieldScope" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="SDCustomFields"></asp:RequiredFieldValidator>
                                            </label>
                                            <div class="col-sm-4 pr-5">
                                                <asp:DropDownList ID="ddlFieldScope" runat="server" CssClass="form-control  form-control-sm">
                                                    <asp:ListItem Text="---Select Scope---" Value="0"></asp:ListItem>
                                                    <asp:ListItem Text="For Both" Value="ForBoth"></asp:ListItem>
                                                    <asp:ListItem Text="For User" Value="ForUser"></asp:ListItem>
                                                    <asp:ListItem Text="For Technician" Value="ForTechnician"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                        </div>
                                        <div class="form-row">
                                            <div class="col-md-3  offset-5">
                                                <asp:Button ID="btnInsertSDCustomFields" runat="server" Text="Save" class="btn btn-sm savebtn" OnClick="btnInsertSDCustomFields_Click" ValidationGroup="SDCustomFields" />
                                                <asp:Button ID="btnUpdateSDCustomFields" runat="server" Text="Update" Visible="false" class="btn btn-sm savebtn" OnClick="btnUpdateSDCustomFields_Click" ValidationGroup="SDCustomFields" />
                                                <asp:Button ID="btnCancel12" runat="server" Text="Refresh" class="btn btn-sm cancelbtn" OnClick="btnCancel12_Click" CausesValidation="false" />
                                            </div>
                                        </div>
                                        <div class="d-flex align-items-center gap-3">
                                            <asp:LinkButton class="btn btn-grd-info px-4" ID="lnkPreviousDeskConfig" runat="server" OnClick="lnkPreviousDeskConfig_Click">Previous</asp:LinkButton>
                                            <asp:LinkButton class="btn btn-grd-primary px-4" ID="lnkNextEsclation" runat="server" OnClick="lnkNextEsclation_Click">Next</asp:LinkButton>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12 graphs">
                                                <div class="xs">
                                                    <div class="well1 white">
                                                        <div class="card card-default">
                                                            <div class="card-body">
                                                                <div class="row ">
                                                                    <div class="col-md-4">
                                                                        <asp:Label ID="Label20" runat="server" Text="SDCustomFields Details" Font-Size="Larger" ForeColor="Black"></asp:Label>
                                                                    </div>
                                                                    <div class="col-md-6">
                                                                        <asp:Label ID="Label24" runat="server"></asp:Label>
                                                                        <asp:Label ID="Label25" runat="server"></asp:Label>
                                                                    </div>
                                                                    <div class="col-md-2 ">
                                                                        <div class="btn btn-sm elevation-1 ml-1 " style="padding: 0px; margin-bottom: 10px; padding-top: 1px">
                                                                            <label class="mr-2 ml-1 mb-0">Export</label>
                                                                            <asp:ImageButton ID="ImgBtnExport13" runat="server" ImageUrl="~/Images/New folder/excelnew.png" CssClass="fa-pull-right btn-outline-success mr-1" OnClick="ImgBtnExport13_Click" />
                                                                        </div>
                                                                    </div>

                                                                </div>
                                                                <div style="overflow-x: scroll">
                                                                    <asp:GridView GridLines="None" ID="gvSDCustomFields" runat="server" DataKeyNames="ID" AutoGenerateColumns="false" CssClass="table table-bordered"
                                                                        Width="100%" OnRowCommand="gvSDCustomFields_RowCommand" OnRowDataBound="gvSDCustomFields_RowDataBound">
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="20">
                                                                                <ItemTemplate>
                                                                                    <%#Container.DataItemIndex+1 %>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:BoundField DataField="DeskRef" HeaderText="Request Type" NullDisplayText="NA" />
                                                                            <asp:BoundField DataField="FieldID" HeaderText="FieldID" NullDisplayText="NA" />
                                                                            <asp:BoundField DataField="FieldName" HeaderText="Field Name" NullDisplayText="NA" />
                                                                            <asp:BoundField DataField="FieldMode" HeaderText="Field Mode" NullDisplayText="NA" />
                                                                            <asp:BoundField DataField="fieldType" HeaderText="Field Type" NullDisplayText="NA" />
                                                                            <asp:BoundField DataField="Status" HeaderText="Status" NullDisplayText="NA" />
                                                                            <asp:BoundField DataField="IsFieldReq" HeaderText="IsFieldReq" NullDisplayText="NA" />
                                                                            <asp:BoundField DataField="FieldScope" HeaderText="FieldScope" NullDisplayText="NA" />
                                                                            <asp:TemplateField HeaderText=" Organization">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblOrgFk" runat="server" Text='<%# Eval("Org_ID") %>' Visible="false"></asp:Label>
                                                                                    <asp:Label ID="lblOrgName" runat="server" Text='<%# Eval("OrgName") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:ButtonField ButtonType="Image" CommandName="SelectState" HeaderText="Edit" ImageUrl="~/images/edit23.png" ItemStyle-Width="20px" />
                                                                            <asp:ButtonField HeaderText="Delete" ButtonType="Image" ImageUrl="~/Images/New folder/delnew.png" CommandName="DeleteEx" ItemStyle-Width="20px" ItemStyle-Height="5px" />
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="ImgBtnExport13" />
                        <asp:PostBackTrigger ControlID="ddlRequestTypeCustomField" />
                        <asp:PostBackTrigger ControlID="ddlOrgCustomField" />
                        <asp:PostBackTrigger ControlID="btnInsertSDCustomFields" />
                        <asp:PostBackTrigger ControlID="gvSDCustomFields" />
                        <asp:PostBackTrigger ControlID="btnUpdateSDCustomFields" />
                        <asp:PostBackTrigger ControlID="lnkPreviousDeskConfig" />
                        <asp:PostBackTrigger ControlID="lnkNextEsclation" />
                    </Triggers>
                </asp:UpdatePanel>
            </asp:Panel>--%>
        <%--Add Custom Fields End--%>

        <%--Add Esclation Matrix Strat--%>
        <asp:Panel ID="pnlExclation" runat="server" Visible="false">
            <asp:UpdatePanel ID="updatepanel13" runat="server">
                <ContentTemplate>

                    <div class="card">
                        <div class="card-body mb-1">
                            <div class="d-flex align-items-start justify-content-between">
                                <h6 class="mb-3 fw-bold">Escalation Matrix</h6>
                                <div class="d-flex">

                                    <asp:LinkButton class="btn btn-sm bs-stepper-circle rounded-circle btn-outline-secondary " ToolTip="Previous" ID="lnkPreviousCustomField" runat="server" OnClick="lnkPreviousCustomField_Click"><i class="fa-solid fa-angle-left"></i></asp:LinkButton>
                                    &nbsp;
                                 
                               
                                </div>
                            </div>
                            <div class="row gx-2 gy-3">
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Escalation  Level 
                                           
                                        <asp:RequiredFieldValidator ID="rfvddlEsclationLevel" runat="server" ControlToValidate="ddlEsclationLevel" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="UserEcslevel"></asp:RequiredFieldValidator>
                                    </label>

                                    <asp:DropDownList ID="ddlEsclationLevel" runat="server" CssClass="form-control  form-control-sm">
                                        <asp:ListItem Text="L1" Value="L1"></asp:ListItem>
                                        <asp:ListItem Text="L2" Value="L2"></asp:ListItem>
                                        <asp:ListItem Text="L3" Value="L3"></asp:ListItem>
                                        <asp:ListItem Text="L4" Value="L4"></asp:ListItem>
                                        <asp:ListItem Text="L5" Value="L5"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        User Name
                                           
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ControlToValidate="txtUserNameEsc" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="UserEcslevel"></asp:RequiredFieldValidator>
                                    </label>

                                    <asp:TextBox ID="txtUserNameEsc" runat="server" CssClass="form-control  form-control-sm ">
                                    </asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Email
                                           
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail"
                                            ForeColor="Red" ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
                                            Display="Dynamic" ErrorMessage="Invalid Email" ValidationGroup="UserEcslevel" />
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ControlToValidate="txtEmailEsc" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="UserEcslevel"></asp:RequiredFieldValidator>
                                    </label>

                                    <asp:TextBox ID="txtEmailEsc" runat="server" CssClass="form-control  form-control-sm ">
                                    </asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Mobile
                                       
                                        <asp:RegularExpressionValidator ID="rvPhoneNumber" runat="server"
                                            ControlToValidate="txtMobile"
                                            ValidationExpression="^(?:(?:\+?\d{1,3}[-.\s]?)?\(?(?:\d{3})?\)?[-.\s]?\d{3}[-.\s]?\d{4})|(?:(?:\+?\d{1,3}[-.\s]?)?\(?(?:\d{2,4})?\)?[-.\s]?\d{6,8})$"
                                            ErrorMessage="Invalid Number" ForeColor="Red"
                                            Display="Dynamic" ValidationGroup="UserEcslevel">
                                        </asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="rfvtxtMobile" runat="server" ControlToValidate="txtMobile" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="UserEcslevel"></asp:RequiredFieldValidator>
                                    </label>
                                    <div class="input-group input-group-sm">
                                        <span class="input-group-text" id="inputGroup-sizing-sm">91</span>
                                        <asp:TextBox ID="txtMobile" TextMode="Phone" runat="server" CssClass="form-control  form-control-sm " onkeypress="return /^[0-9]*$/.test(event.key) && this.value.length < 12;">
                                        </asp:TextBox>
                                    </div>

                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Escalation Time(in Min)
                                           
                                        <asp:RequiredFieldValidator ID="rfvtxttimeforEsclation" runat="server" ControlToValidate="txttimeforEsclation" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="UserEcslevel"></asp:RequiredFieldValidator>
                                    </label>

                                    <asp:TextBox ID="txttimeforEsclation" runat="server" CssClass="form-control  form-control-sm " onkeypress="return /^[0-9\s]*$/.test(event.key) && this.value.length < 50;">
                                    </asp:TextBox>
                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Organization

                                           

                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ControlToValidate="ddlOrg" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="ReqType"></asp:RequiredFieldValidator>
                                    </label>

                                    <asp:DropDownList Enabled="false" ID="ddlOrgEcs" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-12 text-end">
                                    <asp:Button ID="lnkSkipFinish" runat="server" Text="Add & Finish" class="btn btn-sm btn-grd-primary" OnClick="lnkSkipFinish_Click" ValidationGroup="UserEcslevel" />
                                    <asp:Button ID="btnInsertEcslevel" runat="server" Text="Save" class="btn btn-sm btn-grd-info" OnClick="btnInsertEcslevel_Click" ValidationGroup="UserEcslevel" />
                                    <asp:Button ID="btnUpdateEcslevel" runat="server" Text="Update" Visible="false" class="btn btn-sm btn-grd-info" OnClick="btnUpdateEcslevel_Click" ValidationGroup="UserEcslevel" />
                                    <asp:Button ID="btnCancel14" runat="server" Text="Refresh" class="btn btn-sm btn-grd-danger" OnClick="btnCancel14_Click" CausesValidation="false" />
                                    <asp:Button ID="btnCloseFinish" runat="server" Text="Close & Finish" class="btn btn-sm btn-grd-primary" OnClick="btnCloseFinish_Click" />
                                </div>
                            </div>
                        </div>
                    </div>


                    <div class="card">
                        <div class="card-body">
                            <div class="d-flex align-items-start justify-content-between mb-3">
                                <div class="">
                                    <h6 class="mb-0">
                                        <asp:Label ID="Label26" runat="server" Text="Escalation Details"></asp:Label>
                                    </h6>
                                </div>
                                <asp:LinkButton ID="ImgBtnExport14" runat="server" class="btn btn-sm btn-outline-secondary" OnClick="ImgBtnExport14_Click">Export <i class="fa-solid fa-download"></i></asp:LinkButton>

                            </div>
                            <div class="row ">
                                <div class="col-md-12">
                                    <div class="table-responsive table-container">
                                        <asp:GridView GridLines="None" ID="gvEcslevel" runat="server" DataKeyNames="ID" AutoGenerateColumns="false" CssClass="table table-head-fixed text-nowrap table-sm border"
                                            Width="100%" OnRowCommand="gvEcslevel_RowCommand" OnRowDataBound="gvEcslevel_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="20">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="EsclationLevel" HeaderText="Escltion Level" NullDisplayText="NA" />
                                                <asp:BoundField DataField="UserName" HeaderText="UserName" NullDisplayText="NA" />
                                                <asp:BoundField DataField="UserEmail" HeaderText="User Email" NullDisplayText="NA" />
                                                <asp:BoundField DataField="Mobile" HeaderText="Mobile" NullDisplayText="NA" />
                                                <asp:BoundField DataField="TimeForEsclatn" HeaderText="Esclation Time" NullDisplayText="NA" />
                                                <asp:TemplateField HeaderText=" Organization">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOrgFk" runat="server" Text='<%# Eval("Org_ID") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblOrgName" runat="server" Text='<%# Eval("OrgName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--<asp:ButtonField ButtonType="Image" CommandName="UpdateEcslevel" HeaderText="Edit" ImageUrl="~/images/edit23.png" ItemStyle-Width="20px" />
                                                <asp:ButtonField HeaderText="Delete" ButtonType="Image" ImageUrl="~/Images/New folder/delnew.png" CommandName="DeleteEcslevel" ItemStyle-Width="20px" ItemStyle-Height="5px" />--%>
                                                <asp:TemplateField HeaderText="Edit">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkEdit11" runat="server" CommandName="UpdateEcslevel" CommandArgument="<%# Container.DataItemIndex %>" ToolTip="Edit">
                                                <i class="fa-solid fa-edit"></i>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Delete">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkDelete11" runat="server" CommandName="DeleteEcslevel" CommandArgument="<%# Container.DataItemIndex %>" ToolTip="Delete" OnClientClick="return confirm('Are you sure you want to delete this record?');">
                                                <i class="fa-solid fa-xmark text-danger"></i> 
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>


                            </div>

                        </div>
                    </div>


                    <asp:Button ID="btnpop" runat="server" Text="show" Style="display: none" />
                    <asp:ModalPopupExtender ID="mp1" ClientIDMode="Static" runat="server" PopupControlID="Panel1" CancelControlID="btnclose" TargetControlID="btnpop" BackgroundCssClass="modalBackground">
                    </asp:ModalPopupExtender>
                    <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup " align="center">
                        <div class="card border">
                            <div class="card-header bg-grd-primary text-start">
                                <h6 class="mb-0">Desk Name
                                    <asp:RequiredFieldValidator ID="reqtxtDeskName" runat="server" ControlToValidate="txtDeskName" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="poup"></asp:RequiredFieldValidator></h6>


                            </div>
                            <div class="card-body">
                                <div class="row g-2">
                                    <div class="col-10">

                                        <asp:TextBox ID="txtDeskName" runat="server" CssClass="form-control form-control-sm " ValidationGroup="poup"></asp:TextBox>
                                    </div>
                                    <div class="col-2">
                                        <asp:Button runat="server" ID="btnAddDeskName" Text="Add" CssClass="btn btn-grd-info btn-sm " ValidationGroup="poup" OnClick="btnAddDeskName_Click" />
                                    </div>
                                </div>
                            </div>
                            <div class="card-footer text-end">
                                <asp:Button runat="server" ID="btnclose" Text="Close" CssClass="btn btn-grd-danger btn-sm " Style="text-decoration: none;" />
                            </div>
                        </div>
                    </asp:Panel>

                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="ImgBtnExport14" />
                    <asp:PostBackTrigger ControlID="gvEcslevel" />
                    <asp:PostBackTrigger ControlID="lnkPreviousCustomField" />
                    <asp:PostBackTrigger ControlID="lnkSkipFinish" />
                    <asp:PostBackTrigger ControlID="btnCloseFinish" />
                    <asp:PostBackTrigger ControlID="btnAddDeskName" />
                </Triggers>

            </asp:UpdatePanel>
        </asp:Panel>
        <%--Add Esclation Matrix End--%>
    </div>
    <script>
        $(function () {
            $('[data-bs-toggle="tooltip"]').tooltip();
        });

    </script>
    <script>
        function validateNumber(input) {
            input.value = input.value.replace(/[^0-9]/g, '');
        }
        window.onload = function () {
            var input = document.getElementById('txtPort');
            input.style.webkitAppearance = 'none';
            input.style.mozAppearance = 'textfield';
            input.style.appearance = 'none';
            input.addEventListener('keydown', function (event) {
                if (event.key === 'ArrowUp' || event.key === 'ArrowDown') {
                    event.preventDefault();
                }
            });
        };
        function validateCaseSensitiveInput(input) {
            var inputValue = input.value;
            var correctValue = inputValue.charAt(0).toUpperCase() + inputValue.slice(1).toLowerCase();
            if (inputValue !== correctValue) {
                input.value = correctValue;
                alert("Please enter a case-sensitive value.");
            }
        }



    </script>
    <script>
        // Function to add the btn-grd-success class dynamically
        function updateStepClass(currentStep) {
            // Total number of steps
            const totalSteps = 12;

            // Loop through each step
            for (let step = 1; step <= totalSteps; step++) {
                // Get the LinkButton and its associated div
                const stepButton = document.querySelector(`#stepper1trigger${step}`);
                const stepWrapper = document.querySelector(`#a${step}`);

                if (step <= currentStep) {
                    // Add btn-grd-success class for completed steps
                    stepButton.classList.add('btn-grd-success');
                    stepWrapper.classList.remove('visible');
                } else {
                    // Remove btn-grd-success class for uncompleted steps
                    stepButton.classList.remove('btn-grd-success');
                    stepWrapper.classList.add('visible');
                }
            }
        }

        // Example of handling the "Next" button click
        function handleNextButtonClick() {
            let currentStep = parseInt(document.getElementById("currentStep").value); // Get the current step value
            currentStep++;  // Move to the next step

            // Update the current step value
            document.getElementById("currentStep").value = currentStep;

            // Update button styles by adding/removing the btn-grd-success class
            updateStepClass(currentStep);
        }

        // Event listener for "Next" button (you need to attach this event to your "Next" button)
        document.getElementById("nextButton").addEventListener("click", handleNextButtonClick);




    </script>

    <%-- green trigger button code --%>
    <script>
        window.onload = () => {
            let lnkTrigger = document.querySelectorAll('.step-trigger');
            if (lnkTrigger) {
                let foundPrimary = false;

                lnkTrigger.forEach(element => {
                    if (foundPrimary) {
                        return; // Skip further processing once we find the primary button
                    }

                    if (element.classList.contains('btn-grd-primary')) {
                        foundPrimary = true; // Mark as found to stop adding the class
                    } else {
                        element.classList.add('btn-grd-success', 'p-2', 'rounded-circle', 'opacity-75');
                    }
                });

                console.log('Operation complete.');
            }
        }
    </script>
    <%-- green trigger button code --%>
    <script>
        window.onload = () => {
            let lnkTrigger = document.querySelectorAll('.step-trigger');
            console.log(lnkTrigger)
            if (lnkTrigger) {
                let foundPrimary = false;

                lnkTrigger.forEach(element => {
                    if (foundPrimary) {
                        return; // Skip further processing once we find the primary button
                    }

                    if (element.classList.contains('btn-grd-primary')) {
                        foundPrimary = true; // Mark as found to stop adding the class
                        // You can optionally add styles here to keep the button unchanged if needed
                        // e.g., element.classList.add('fixed-primary'); (if you need to apply any other styles)
                    } else {
                        // Add the styles to the non-primary buttons
                        element.classList.add('btn-grd-success', 'p-2', 'rounded-circle', 'opacity-75');
                    }
                });

                console.log('Operation complete.');
            }
        }

    </script>
<%--   <script type="text/javascript">
       function clearLabelAfter5Seconds() {
           setTimeout(function () {
               var label = document.getElementById('<%= lblErrorMsg.ClientID %>');
               if (label) {
                   label.innerHTML = '';
               }
           }, 5000);
       }
       window.onload = clearLabelAfter5Seconds;
   </script>--%>
</asp:Content>


<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmAllTicketsEngTransf.aspx.cs" Inherits="frmAllTicketsEngTransf" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        [data-bs-theme=semi-dark] a > .fa-plus {
            color: #474747 !important;
        }

        [data-bs-theme=light] a > .fa-plus {
            color: #474747 !important;
        }


        th > a {
            color: var(--bs-heading-color) !important;
        }

        th > img {
            display: none
        }
    </style>
    <style>
        .badage-sucess > span {
            color: white !important;
            background-color: #02c27a !important;
            border-color: rgb(2 194 122 / 0%) !important;
            padding: 2px 5px;
            border-radius: 5px
        }

        .badage-yellow > span {
            color: white !important;
            background-color: #f7971e !important;
            padding: 2px 5px;
            border-radius: 5px
        }

        .badage-red > span {
            color: white !important;
            background-color: #fc185a !important;
            padding: 2px 5px;
            border-radius: 5px
        }

        .badage-info > span {
            color: white !important;
            background-color: #ff0080 !important;
            padding: 2px 5px;
            border-radius: 5px
        }

        .select2-container--bootstrap-5 .select2-selection--single .select2-selection__rendered {
            white-space: nowrap !important;
            overflow: hidden !important;
            text-overflow: ellipsis !important;
            max-width: 100px !important;
            display: block !important;
        }

        .description-label {
            display: inline-block;
            max-width: 100px;
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
            cursor: default;
        }

        @media screen and (max-width: 767px) {
            .btn-group0 {
                overflow-x: scroll
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="card">
                <div class="card-body">
                    <asp:Panel ID="pnlgridrow" runat="server">
                        <div class="row d-none">
                            <div class="col-md-12" style="border-bottom: none">
                                <asp:Label ID="lblheader" CssClass="headline" runat="server" Font-Size="Larger" Text="ALL Engineer Tickets"></asp:Label>
                                <asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Red"></asp:Label>
                            </div>
                        </div>
                        <div class="row gx-2 gy-3">
                            <div class="col-md-1 col-4">
                                <label class="form-label">Filters</label>
                                <br>
                                <div class="btn-group btn-group-sm">
                                    <asp:LinkButton ID="imgcolumnfilter" runat="server" CssClass="btn btn-sm btn-outline-secondary" AlternateText="Column Chooser" Visible="false" ToolTip="Filter" ImageAlign="left" OnClick="imgcolumnfilter_Click"><i class="fa-solid fa-filter"></i></asp:LinkButton>
                                    <asp:LinkButton ID="imgRowFilter" runat="server" CssClass="btn btn-sm btn-outline-secondary" AlternateText="Column Chooser" ToolTip="Select Column" ImageAlign="left" OnClick="imgRowFilter_Click" OnClientClick="togglePanel(); return false;"><i class="fa-solid fa-plus"></i></asp:LinkButton>
                                </div>
                            </div>
                            <div class="col-md-2 col-8">
                                <label class="form-label">Organization</label>
                                <asp:DropDownList ID="ddlOrg" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlOrg_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <label class="form-label">Request Type</label>
                                <asp:DropDownList ID="ddlRequestType" runat="server" ToolTip="Select Desk Type" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlRequestType_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-1 col-3">
                                <label class="form-label ml-4">Action</label>
                                <br />
                                <asp:Button ID="btnDelteBulkTicket" runat="server" Text="Delete" ToolTip="Delete Ticket" CssClass="btn btn-sm cancelbtn" OnClick="btnDelteBulkTicket_Click" hidden />

                                <asp:Button ID="btnPickupTicket" runat="server" Text="Transfer" ToolTip="Transfer" CssClass="btn btn-sm btn-outline-secondary" OnClick="btnPickupTicket_Click" ValidationGroup="Pickup" />

                            </div>
                            <div class="col-md-3 col-9">
                                <label class="form-label">
                                    Reason For Change
																								<asp:RequiredFieldValidator ID="rfvtxtReason" runat="server" ControlToValidate="txtReason" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Pickup"></asp:RequiredFieldValidator>

                                </label>

                                <asp:TextBox ID="txtReason" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                            <div class="col-md-2 col-9">
                                <label class="form-label">Paging</label>
                                <br />
                                <asp:Repeater ID="rptpageindexing" runat="server">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkPage"
                                            Font-Size="Small"
                                            CssClass="pagging_label"
                                            runat="server"
                                            Text='<%#Eval("Text") %>'
                                            CommandArgument='<%#Eval("Value") %>'
                                            Enabled='<%#Eval("Enabled") %>'
                                            OnClick="Page_Changed">
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                            <div class="col-md-1 col-3">
                                <label class="form-label">Size</label>
                                <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" CssClass-="form-select form-select-sm " OnSelectedIndexChanged="PageSize_Changed">

                                    <asp:ListItem Text="50" Value="50" Selected="True" />
                                    <asp:ListItem Text="100" Value="100" />
                                    <asp:ListItem Text="150" Value="150" />
                                </asp:DropDownList>
                            </div>


                            <div class="col-md-1   " hidden>
                                <label class=" form-label">Ageing</label>
                                <asp:DropDownList ID="ddlGetticketFilter" runat="server" CssClass="form-control form-control-sm   " Width="110px" ToolTip="Get Ticket" AutoPostBack="true" OnSelectedIndexChanged="ddlGetticketFilter_SelectedIndexChanged">
                                    <asp:ListItem Selected="True" Text="Get Tickets" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Last 1 hour" Value="Last1hour"></asp:ListItem>
                                    <asp:ListItem Text="Last 24 hours" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Last 30 days" Value="30"></asp:ListItem>
                                    <asp:ListItem Text="Last 90 days" Value="90"></asp:ListItem>
                                    <asp:ListItem Text="More than 90 days" Value="Morethan90Days"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <%--<div style="overflow-y: scroll; height: 500px">--%>
                    </asp:Panel>
                    <asp:Panel ID="pnlRowFilter" runat="server" Visible="false">
                        <div class="row gx-2 gy-3 mt-2">
                            <div class="col-md-1">
                                <asp:TextBox ID="txtTicketNoFltr" runat="server" CssClass="form-control form-control-sm " placeholder="Enter Ticket No"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtTickSumFltr" runat="server" CssClass="form-control form-control-sm" placeholder="Enter  Summary"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtPriorFltr" runat="server" CssClass="form-control form-control-sm" placeholder="Enter Priority"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtSeverityFltr" runat="server" CssClass="form-control form-control-sm" placeholder="Enter Severity"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <asp:TextBox ID="txtStatfltr" runat="server" CssClass="form-control form-control-sm" placeholder="Enter Status"></asp:TextBox>
                            </div>
                            <div class="col-md-2 col-8">
                                <asp:DropDownList ID="ddlAssigneEmail" runat="server" CssClass-="form-select form-select-sm single-select-optgroup-field"></asp:DropDownList>
                            </div>
                            <div class="col-md-1 col-4">
                                <div class="btn-group w-100">
                                    <asp:Button ID="btnGridFilter" runat="server" Text="Go" ToolTip="Click Button to Get Ticket As Per Filter" CssClass="btn btn-outline-secondary btn-sm  " OnClick="btnGridFilter_Click" />
                                    <asp:LinkButton ID="imgRemoveFilter" runat="server" OnClick="imgRemoveFilter_Click" class="btn btn-outline-secondary btn-sm" ToolTip="Removes Filter"><i class="fa-solid fa-filter-circle-xmark"></i></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnltickcount" runat="server">
                        <div class="row mt-3">
                            <div class="col-md-12">
                                <div class="btn-group btn-group0 btn-group-sm  w-100  ">
                                    <asp:Button ID="btnOpenTicket" ToolTip="Ticket With Open Status" runat="server" Text="Open  (0)" CssClass="btn btn-sm btn-inverse-success btn-block" OnClick="btnOpenTicket_Click" />
                                    <asp:Button ID="btnWipTicket" ToolTip="Tickets that are not resolved and open" runat="server" Text="WIP  (0)" CssClass="btn btn-sm btn-inverse-warning btn-block" OnClick="btnWipTicket_Click" />
                                    <asp:Button ID="btnTicketAssigntoME" ToolTip="Tickets Assgined to me" runat="server" Text="Assigned (0)" CssClass="btn btn-sm btn-inverse-info btn-block" OnClick="btnTicketAssigntoME_Click" />
                                    <asp:Button ID="btnAssignToOther" ToolTip="Tickets that are not assign" runat="server" Text="UnAssigned (0)" CssClass="btn btn-sm btn-inverse-primary btn-block" OnClick="btnAssignToOther_Click" />
                                    <asp:Button ID="btnDueSoonTickets" ToolTip="Tickets that are goin to esclate  or Response goin to Missed" runat="server" Text="Due Soon  (0)" CssClass="btn btn-sm btn-inverse-secondary btn-block" OnClick="btnDueSoonTickets_Click" />
                                    <asp:Button ID="btnTicketEsclated" runat="server" ToolTip="Tickets that esclated or response missed" Text="Overdue  (0)" CssClass="btn btn-sm btn-inverse-danger btn-block" OnClick="btnTicketEsclated_Click" />

                                </div>
                            </div>
                        </div>
                    </asp:Panel>

                    <div class="row mt-4">
                        <div class="col-md-12">
                            <div class="table-responsive table-container" style="height: 320px;">

                                <asp:GridView ID="gvAllTickets" runat="server" CssClass="table  text-nowrap  table-striped table-sm border" DataKeyNames="ID" AllowCustomPaging="True"
                                    AutoGenerateColumns="False" OnRowCommand="gvAllTickets_RowCommand" OnRowCreated="gvAllTickets_RowCreated" OnSorting="gvAllTickets_Sorting" OnRowDataBound="gvAllTickets_RowDataBound" AllowSorting="True" OnRowEditing="gvAllTickets_RowEditing">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="30" ItemStyle-Height="5px">
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="chkAll" runat="server" onclick="grdHeaderCheckBox(this);" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkRow" runat="server" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" Width="30px" />
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Edit">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEdit" runat="server" CommandName="EditTicket" CommandArgument="<%# Container.DataItemIndex %>">
                                                <i class="fa-solid fa-edit"></i> <!-- FontAwesome icon -->
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="TicketNumber" ItemStyle-Font-Size="Smaller" SortExpression="TicketNumber" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTicketNumberColor" runat="server" Text='<%# Eval("color") %>' Font-Size="Smaller" Visible="false"></asp:Label>
                                                <asp:Label ID="lblTicketNumber" runat="server" Text='<%# Eval("TicketNumber") %>' Font-Size="Smaller" Visible="true"></asp:Label>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ServiceDesk" HeaderText="ServiceDesk" SortExpression="ServiceDesk" ItemStyle-Height="5px" ItemStyle-Font-Size="Smaller" Visible="true" />

                                        <%--	<asp:BoundField DataField="TicketNumber" HeaderText="TicketNumber" SortExpression="TicketNumber" ItemStyle-Height="5px" ItemStyle-Font-Size="Smaller" Visible="true" />--%>

                                        <asp:TemplateField HeaderText="Summary" ItemStyle-Font-Size="Smaller" SortExpression="Summary" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSummary" CssClass="description-label" runat="server" Text='<%# Eval("Summary") %>' Font-Size="Smaller" Visible="true"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:BoundField DataField="Summary" HeaderText="Summary" SortExpression="Summary" ItemStyle-Width="300px" HeaderStyle-Width="400px" HeaderStyle-Wrap="false" ItemStyle-Height="5px" ItemStyle-Wrap="true" ItemStyle-Font-Size="Smaller" Visible="true">
                                            <itemstyle wrap="True" />

                                        </asp:BoundField>--%>
                                        <asp:TemplateField HeaderText="Category" ItemStyle-Height="20px" SortExpression="Category" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCategoryFK" runat="server" Text='<%# Eval("Category") %>' Font-Size="Smaller" Visible="false"></asp:Label>
                                                <asp:Label ID="lblCategoryName" runat="server" Text='<%# Eval("Category") %>' Font-Size="Smaller" Visible="true"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CreationDate" HeaderText="Creation Date" SortExpression="CreationDate" ItemStyle-Font-Size="Smaller" ItemStyle-Height="5px" DataFormatString="{0:dd-MM-yyyy hh:mm:ss tt}" />

                                        <asp:BoundField DataField="Priority" HeaderText="Priority" SortExpression="Priority" ItemStyle-Font-Size="Smaller" />
                                        <asp:TemplateField HeaderText="Stage " ItemStyle-Width="150" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                            <ItemTemplate>
                                                <asp:Label ID="lblstagecode" runat="server" Font-Size="X-Small" Text='<%# Eval("Stage") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status " ItemStyle-Width="150" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatusCode" runat="server" Font-Size="X-Small" CssClass=" badge badge-notifications" ForeColor="White" BackColor='<%# System.Drawing.ColorTranslator.FromHtml(Eval("StatusColorCode").ToString())%>' Text='<%# Eval("Status") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Severity" HeaderText="Severity" SortExpression="Severity" ItemStyle-Font-Size="Smaller" ItemStyle-Height="5px" />
                                        <asp:BoundField DataField="TechLoginName" HeaderText="Assignee" SortExpression="TechLoginName" ItemStyle-Height="5px" NullDisplayText="UnAssigned" ItemStyle-Font-Size="Smaller" />
                                        <asp:BoundField DataField="SubmitterType" HeaderText="Submitter Type" SortExpression="SubmitterType" ItemStyle-Height="5px" ItemStyle-Font-Size="Smaller" />
                                        <asp:BoundField DataField="SubmitterName" HeaderText="Submitter Name" SortExpression="SubmitterName" ItemStyle-Height="5px" ItemStyle-Font-Size="Smaller" />
                                        <asp:BoundField DataField="SubmitterEmail" HeaderText="Submitter Email" SortExpression="SubmitterEmail" ItemStyle-Height="5px" ItemStyle-Font-Size="Smaller" />
                                        <asp:BoundField DataField="SubmitterPhone" HeaderText="Submitter Phone" SortExpression="SubmitterPhone" ItemStyle-Height="5px" ItemStyle-Font-Size="Smaller" />
                                        <asp:BoundField DataField="DueDate" HeaderText="Expect. Response Dt" SortExpression="SubmitterPhone" ItemStyle-Height="5px" ItemStyle-Font-Size="Smaller" DataFormatString="{0:dd-MM-yyyy hh:mm:ss tt}" />
                                        <asp:BoundField DataField="ExpectedResolutionDt" HeaderText="Expect Resoution Dt " SortExpression="SubmitterPhone" ItemStyle-Height="5px" ItemStyle-Font-Size="Smaller" DataFormatString="{0:dd-MM-yyyy hh:mm:ss tt}"/>
                                        <asp:BoundField DataField="location" HeaderText="Location " SortExpression="Location" ItemStyle-Height="5px" ItemStyle-Font-Size="Smaller" />

                                        <asp:BoundField DataField="SourceType" HeaderText="Source Type" SortExpression="SourceType" ItemStyle-Height="5px" ItemStyle-Font-Size="Smaller" />
                                    </Columns>
                                    <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>


            <div class="modal" id="CategoryModal" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true">
                <div class="modal-dialog" style="bottom: 20%">
                    <div class="modal-content">
                        <div class="card-header ">
                            Select Colums
								<button type="button" class=" fa-pull-right badge-danger" data-dismiss="modal" onclick="javascript:window.location.reload()" aria-hidden="true">&times;</button>
                        </div>
                        <div class="card-body">
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="checkbox checkbox-columns">
                                        <asp:CheckBoxList ID="chkcolumn" runat="server" RepeatDirection="Vertical" OnSelectedIndexChanged="chkcolumn_SelectedIndexChanged" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btngetCheckcolumn" runat="server" Text="Go" OnClick="btngetCheckcolumn_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="rptpageindexing" />
            <asp:AsyncPostBackTrigger ControlID="imgcolumnfilter" EventName="Click" />
            <asp:PostBackTrigger ControlID="btngetCheckcolumn" />
            <asp:PostBackTrigger ControlID="ddlGetticketFilter" />
            <asp:PostBackTrigger ControlID="ddlPageSize" />
            <asp:PostBackTrigger ControlID="btnDelteBulkTicket" />
            <asp:PostBackTrigger ControlID="gvAllTickets" />
            <asp:PostBackTrigger ControlID="imgRowFilter" />
            <asp:PostBackTrigger ControlID="ddlOrg" />
            <asp:PostBackTrigger ControlID="ddlRequestType" />
            <asp:PostBackTrigger ControlID="btnPickupTicket" />
            <asp:PostBackTrigger ControlID="btnGridFilter" />
            <asp:PostBackTrigger ControlID="btnOpenTicket" />
            <asp:PostBackTrigger ControlID="btnWipTicket" />
            <asp:PostBackTrigger ControlID="btnTicketAssigntoME" />
            <asp:PostBackTrigger ControlID="btnAssignToOther" />
            <asp:PostBackTrigger ControlID="btnDueSoonTickets" />
            <asp:PostBackTrigger ControlID="btnTicketEsclated" />
            <asp:PostBackTrigger ControlID="imgRowFilter" />
        </Triggers>
    </asp:UpdatePanel>
    <script type="text/javascript">
        function togglePanel() {
            var panel = document.getElementById('<%= pnlRowFilter.ClientID %>');
            if (panel.style.display === 'none') {
                panel.style.display = 'block';
            }
            else {
                panel.style.display = 'none';
            }
        }
    </script>

    <script type="text/javascript">
        function grdHeaderCheckBox(objRef) {
            var grd = objRef.parentNode.parentNode.parentNode;
            var inputList = grd.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                    if (objRef.checked) {
                        inputList[i].checked = true;
                    }
                    else {
                        inputList[i].checked = false;
                    }
                }
            }
        }
    </script>
</asp:Content>


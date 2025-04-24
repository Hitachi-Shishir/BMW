<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmAllTickets.aspx.cs" Inherits="frmAllTickets" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .badage-sucess > span {
            color: white !important;
            background-color: #02c27a !important;
            border-color: rgb(2 194 122 / 0%) !important;
            padding: 2px 5px;
            border-radius: 5px
        }

        .badage-sucess a {
    color: white !important;
    background-color: #02c27a !important;
    border: none !important;
    padding: 2px 5px !important;
    border-radius: 5px !important;
    display: inline-block; /* Ensures correct styling */
    text-decoration: none; /* Removes underline */
    text-align: center;
    vertical-align: middle;
}
        .badage-yellow > span  {
            color: white !important;
            background-color: #f7971e !important;
            padding: 2px 5px;
            border-radius: 5px;

            display: inline-block; /* Ensures correct styling */
            text-decoration: none; /* Removes underline */
            text-align: center;
            vertical-align: middle;
        }

                .badage-yellow a {
                 color: white !important;
                 background-color: #f7971e !important;
                 padding: 2px 5px;
                 border-radius: 5px;
    display: inline-block; /* Ensures correct styling */
    text-decoration: none; /* Removes underline */
    text-align: center;
    vertical-align: middle;
}

        .badage-red > span {
            color: white !important;
            background-color: #fc185a !important;
            padding: 2px 5px;
            border-radius: 5px;

            display: inline-block; /* Ensures correct styling */
            text-decoration: none; /* Removes underline */
            text-align: center;
            vertical-align: middle;
        }
         .badage-red a {
     color: white !important;
     background-color: #fc185a !important;
     padding: 2px 5px;
     border-radius: 5px;

     display: inline-block; /* Ensures correct styling */
     text-decoration: none; /* Removes underline */
     text-align: center;
     vertical-align: middle;
 }

        .badage-info > span {
            color: white !important;
            background-color: #ff0080 !important;
            padding: 2px 5px;
            border-radius: 5px
        }

        th > a {
            color: var(--bs-heading-color) !important;
        }
    </style>
    <style>
        /*.dataTables_filter {
          margin-top: -29px !important;
      }*/
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
    <script>
        function getxtValue(that) {
            document.getElementById("lable").innerHTML = that.value;
        }
    </script>
    <style>
        .truncate-text {
            max-width: 200px; /* Set your desired fixed width */
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
            display: inline-block;
            /* Maintain original background and font color */
            color: inherit; /* Inherit font color from parent */
        }

            .truncate-text:hover {
                white-space: normal; /* Allows text to wrap on hover */
                overflow: visible; /* Shows the full content */
                text-overflow: clip; /* Removes ellipsis on hover */
                transition: 2s all ease-in-out
            }
    </style>


    <style>
        input[type="checkbox"] {
            accent-color: red !important;
        }

            input[type="checkbox"]:checked {
                accent-color: blue !important;
            }
    </style>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="card ">
                <div class="card-body">
                    <asp:Panel ID="pnlgridrow" runat="server">
                        <div class="row gx-2 gy-3">
                            <div class="col-md-1">
                                <label class="form-label">Filter</label><br />
                                <div class="btn-group">
                                                                        <%--<asp:LinkButton ID="LinkButton1" runat="server"  AlternateText="Column Chooser" ToolTip="Select Column" ImageAlign="left" class="btn  btn-outline-secondary d-flex btn-sm" OnClick="imgRowFilter_Click" OnClientClick="togglePanel(); return false;">LinkButton</asp:LinkButton>--%>

                                    <asp:ImageButton ID="imgcolumnfilter" runat="server" AlternateText="Column Chooser" ToolTip="Select Column" ImageAlign="left" class="btn  btn-outline-secondary d-flex btn-sm" ImageUrl="~/Images/New folder/columnfilter.png" OnClick="imgcolumnfilter_Click" Style="cursor: not-allowed" Visible="false" />
                                          <asp:ImageButton ID="imgRowFilter" runat="server" AlternateText="Column Chooser" ToolTip="Select Column" ImageAlign="left" class="btn  btn-outline-secondary d-flex btn-sm" ImageUrl="~/Images/New folder/funnelfilter.png" OnClick="imgRowFilter_Click" OnClientClick="togglePanel(); return false;" />
                                </div>
                            </div>
                            <div class="col-md-3">
                                <label class="form-label">Organization</label>
                                <asp:DropDownList ID="ddlOrg" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlOrg_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3">
                                <label class="form-label">Request Type</label>
                                <asp:DropDownList ID="ddlRequestType" runat="server" ToolTip="Select Desk Type" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlRequestType_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <label class="form-label">Choose Action</label>
                                <asp:DropDownList ID="ddlActionForTickets" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field">
                                    <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                    <asp:ListItem Value="Merge" Text="Merge"></asp:ListItem>
                                    <asp:ListItem Value="BulkUpdate" Text="Bulk Update"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2">
                                <label class="form-label">Action</label>
                                <br />
                                <div class="btn-group">
                                    <asp:Button ID="btnSubmit" runat="server" Text="Go" ToolTip="Click Button to Get Ticket As Per Filter" class="btn  btn-outline-secondary d-flex btn-sm" OnClick="btnSubmit_Click" />
                                    <asp:Button ID="btnPickupTicket" runat="server" Text="PickUp" ToolTip="Assign Ticket To Self" class="btn  btn-outline-secondary d-flex btn-sm" OnClick="btnPickupTicket_Click" />
                                     <asp:Button ID="btnExpedite" runat="server" Text="Expedite" ToolTip="Click To Make Ticket on high in priority" class="btn  btn-outline-secondary d-flex btn-sm" OnClick="btnExpedite_Click" />
                                                                        <asp:Button ID="btnDelteBulkTicket" Visible="false" runat="server" Text="Delete" ToolTip="Delete Ticket" class="btn  btn-outline-secondary d-flex btn-sm" OnClick="btnDelteBulkTicket_Click" />

                                    <asp:LinkButton ID="btnMerge" runat="server" ToolTip="Merge Ticket" class="btn  btn-outline-secondary d-flex btn-sm" OnClick="btnMerge_Click" Visible="false">Merge</asp:LinkButton>
                                </div>

                            </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlRowFilter" runat="server" Visible="false">
                        <div class="row mt-2 g-2">
                            <div class="col-md-4">
                                <label class="form-label">
                                    Select Column
                                    <asp:RequiredFieldValidator ID="rfvddlSearchItems" runat="server" ControlToValidate="ddlSearchItems" ErrorMessage="Required" ForeColor="Red" InitialValue="0" ValidationGroup="Search"></asp:RequiredFieldValidator>
                                </label>
                                <asp:DropDownList ID="ddlSearchItems" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label">
                                    Enter Search Value
            <asp:RequiredFieldValidator ID="RequiredtxtSearch" runat="server" ControlToValidate="txtSearch" ErrorMessage="Required" ForeColor="Red" ValidationGroup="Search"></asp:RequiredFieldValidator>
                                </label>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="txtSearch" runat="server" class="form-control input-search" placeholder="Search..."></asp:TextBox>
                                    <span class="input-group-btn">
                                        <asp:LinkButton ID="btnSearch" runat="server" class="btn btn-secondary btn-sm" OnClick="btnSearch_Click" ValidationGroup="Search"><i class="fa-solid fa-magnifying-glass"></i></asp:LinkButton>
                                    </span>
                                </div>
                            </div>
                            <div class="col-md-1 ">
                                <label class="form-label opacity-0" >fjklds</label> <br />
                                <asp:LinkButton ID="imgRemoveFilter" runat="server" ToolTip="Removes Filter" OnClick="imgRemoveFilter_Click" CssClass="btn btn-sm btn-grd-info"><i class="fa-solid fa-arrows-rotate"></i></asp:LinkButton>
                            </div>
                            <div class="col-md-1">
                                <label class="form-label">
                                    <asp:Label ID="lblerrorMsg" runat="server" Text=""></asp:Label></label>
                            </div>
                        </div>
                    </asp:Panel>
                    <div class="row mt-1">
                        <div class="col-md-12 ">
                            <div class="btn-group w-100 d-flex  flex-wrap">
                                <asp:Repeater ID="RepeaterButtons" runat="server">
                                    <ItemTemplate>
                                        <asp:Button ID="btnStatus" runat="server" Text='<%# Eval("Status") + " (" + Eval("Count") + ")" %>'
                                            CommandArgument='<%# Eval("Status") %>' OnClick="StatusButton_Click" CssClass="btn btn-sm btn-inverse-secondary" />
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>

                    <div class="row mt-1">
                        <div class="col-md-12">
                            <div class="table-responsive table-container">
                                <asp:GridView ID="gvAllTickets" runat="server" CssClass="data-table1 table-striped table-sm table table-head-fixed text-nowrap border " DataKeyNames="ID"
                                    AutoGenerateColumns="False" OnRowCommand="gvAllTickets_RowCommand" OnRowCreated="gvAllTickets_RowCreated" HeaderStyle-Height="25px"
                                    OnRowDataBound="gvAllTickets_RowDataBound" OnRowEditing="gvAllTickets_RowEditing">
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
                                                  <i class="fa-solid fa-edit"></i> 
                                                </asp:LinkButton>
                                                <asp:Literal ID="ltrisExped" runat="server" Text='<%# Eval("IsExpedite") %>' Visible="false"/>
												<span id="tickFlag" runat="server" visible="false"  title="Expedited - Urgent!">
													<i class="fas fa-flag text-danger"></i>
												</span>
                                            </ItemTemplate>
                                        </asp:TemplateField>
										<asp:TemplateField HeaderText="TicketNumber" ItemStyle-Font-Size="Smaller" SortExpression="TicketNumber">
											<ItemTemplate>
												<!-- Hidden label for color -->
												<asp:Label ID="lblTicketNumberColor" runat="server" Text='<%# Eval("color") %>' CssClass="p-1" Font-Size="Smaller" Visible="false"></asp:Label>

												<!-- LinkButton for Ticket Number -->
												
                                                     
                                                  <span id="badgeContainer" runat="server">
													<asp:LinkButton
														ID="lblTicketNumber"
														runat="server"
														Text='<%# Eval("TicketNumber") %>'
														Font-Size="Smaller"
														CommandArgument='<%# Eval("TicketNumber") %>'
														CommandName="TicketClick" Width="100%" ToolTip="Click on TicketNumber to modify ticket!!" Visible="true"> 
													</asp:LinkButton>
                                                      </span>
                                                       
											</ItemTemplate>
										</asp:TemplateField>
                                    <%--    <asp:TemplateField HeaderText="TicketNumber" ItemStyle-Font-Size="Smaller" SortExpression="TicketNumber" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTicketNumberColor" runat="server" Text='<%# Eval("color") %>' Font-Size="Smaller" CssClass="p-1" Visible="false"></asp:Label>
                                                <asp:Label ID="lblTicketNumber" runat="server" Text='<%# Eval("TicketNumber") %>' Font-Size="Smaller" Visible="true"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                        <asp:BoundField DataField="ServiceDesk" HeaderText="ServiceDesk" SortExpression="ServiceDesk" ItemStyle-Height="5px" ItemStyle-Font-Size="Smaller" Visible="true" />
                                        <asp:TemplateField HeaderText="Summary" ControlStyle-CssClass="truncate-text">
                                            <ItemTemplate>
                                                <asp:Label ID="Label1" runat="server" Font-Size="Smaller" Text='<%# Bind("Summary") %>'
                                                    data-fulltext='<%# Eval("Summary") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Category" ControlStyle-CssClass="truncate-text">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCategoryFK" runat="server" Text='<%# Eval("Category") %>' Font-Size="Smaller" Visible="false"></asp:Label>
                                                <asp:Label ID="lblCategoryName" runat="server" Text='<%# Eval("Category") %>' Font-Size="Smaller" Visible="true"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="CreationDate" HeaderText="Creation Date" SortExpression="CreationDate" ItemStyle-Font-Size="Smaller" ItemStyle-Height="5px" DataFormatString="{0:dd-MM-yyyy hh:mm:ss tt}" />

                                        <asp:BoundField DataField="Priority" HeaderText="Priority" SortExpression="Priority" ItemStyle-Font-Size="Smaller" />
                                        <asp:TemplateField HeaderText="Stage " ItemStyle-Width="150" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                            <ItemTemplate>
                                                <asp:Label ID="lblstagecode" runat="server" Font-Size="X-Small"  Text='<%# Eval("Stage") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status " ItemStyle-Width="150" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatusCode" runat="server" Font-Size="X-Small" CssClass="badge badge-notifications" ForeColor="White" BackColor='<%# System.Drawing.ColorTranslator.FromHtml(Eval("StatusColorCode").ToString())%>' Text='<%# Eval("Status") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Severity" HeaderText="Severity" SortExpression="Severity" ItemStyle-Font-Size="Smaller" ItemStyle-Height="5px" />
                                        <asp:BoundField DataField="TechLoginName" HeaderText="Technician" SortExpression="TechLoginName" ItemStyle-Height="5px" ItemStyle-Font-Size="Smaller" NullDisplayText="UnAssigned" />
                                        <asp:BoundField DataField="SubmitterName" HeaderText="SubmitterName" SortExpression="Submitter Name" ItemStyle-Height="5px" ItemStyle-Font-Size="Smaller" />
                                        <asp:BoundField DataField="SubmitterEmail" HeaderText="SubmitterEmail" SortExpression="Submitter Email" ItemStyle-Height="5px" ItemStyle-Font-Size="Smaller" />
                                        <asp:BoundField DataField="SubmitterPhone" HeaderText="SubmitterPhone" SortExpression="Submitter Phone" ItemStyle-Height="5px" ItemStyle-Font-Size="Smaller" />
                                        <asp:BoundField DataField="DueDate" HeaderText="Expect. Response Dt" SortExpression="Submitter Phone" ItemStyle-Height="5px" ItemStyle-Font-Size="Smaller" DataFormatString="{0:dd-MM-yyyy hh:mm:ss tt}" />
                                        <asp:BoundField DataField="ExpectedResolutionDt" HeaderText="Expect Resoution Dt" SortExpression="Submitter Phone" ItemStyle-Height="5px" ItemStyle-Font-Size="Smaller" DataFormatString="{0:dd-MM-yyyy hh:mm:ss tt}" />
                                        <asp:BoundField DataField="location" HeaderText="Location" SortExpression="Location" ItemStyle-Height="5px" ItemStyle-Font-Size="Smaller" />
                                        <asp:BoundField DataField="Department" HeaderText="Department" SortExpression="Department" ItemStyle-Height="5px" ItemStyle-Font-Size="Smaller" />
                                        <asp:BoundField DataField="SourceType" HeaderText="SourceType" SortExpression="Source Type" ItemStyle-Height="5px" ItemStyle-Font-Size="Smaller" />
                                    </Columns>
                                    <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="modal fade" id="CategoryModal">
                <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable">
                    <div class="modal-content">
                        <div class="modal-header border-bottom-0 bg-grd-primary py-2">
                            <h5 class="modal-title">Select Colums</h5>
                            <a href="javascript:;" class="primaery-menu-close" data-bs-dismiss="modal">
                                <i class="material-icons-outlined" onclick="javascript:window.location.reload()">close</i>
                            </a>
                        </div>
                        <div class="modal-body">
                            <div class="order-summary">
                                <div class="card mb-0">
                                    <div class="card-body">
                                        <div class="card border bg-transparent shadow-none">
                                            <div class="card-body">
                                                <div class="checkbox checkbox-columns">
                                                    <asp:CheckBoxList ID="chkcolumn" runat="server" RepeatDirection="Vertical" OnSelectedIndexChanged="chkcolumn_SelectedIndexChanged" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer border-top-0">
                            <asp:Button ID="btngetCheckcolumn" runat="server" Text="Go" class="btn btn-grd-info btn-sm" OnClick="btngetCheckcolumn_Click" />
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btngetCheckcolumn" />
            <asp:PostBackTrigger ControlID="ddlSearchItems" />
            <asp:PostBackTrigger ControlID="btnSearch" />
            <asp:PostBackTrigger ControlID="imgRemoveFilter" />
            <asp:PostBackTrigger ControlID="gvAllTickets" />
            <asp:PostBackTrigger ControlID="ddlOrg" />
            <asp:PostBackTrigger ControlID="ddlRequestType" />
            <asp:PostBackTrigger ControlID="btnSubmit" />
            <asp:PostBackTrigger ControlID="btnMerge" />
            <asp:PostBackTrigger ControlID="imgRowFilter" />
            <asp:PostBackTrigger ControlID="RepeaterButtons" />
        </Triggers>
    </asp:UpdatePanel>

    <script type="text/javascript">
        function grdHeaderCheckBox(chkAll) {
            var checkboxes = document.querySelectorAll("#<%= gvAllTickets.ClientID %> input[type='checkbox']");
            for (var i = 0; i < checkboxes.length; i++) {
                if (checkboxes[i] !== chkAll) {
                    checkboxes[i].checked = chkAll.checked;
                }
            }
        }
    </script>
    <script type="text/javascript">
        function Showalert(imtype, imtitle) {
            var Toast = Swal.mixin({
                toast: true,
                position: 'top-middle',
                showConfirmButton: false,
                showCloseButton: true,
                timer: 4000,
            });
            console.log("hello");
            Toast.fire({
                icon: imtype,
                title: imtitle
            });
            console.log("fire1234567");
        }
    </script>
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
    <script>
        $(document).ready(function () {
            $('.truncate-text').hover(function () {
                $(this).css({
                    'white-space': 'normal',
                    'z-index': '1'
                });
            }, function () {
                $(this).css({
                    'white-space': 'nowrap',
                    'z-index': 'auto',
                    'background-color': 'transparent'
                });
            });
        });
    </script>
</asp:Content>

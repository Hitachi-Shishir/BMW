<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmAllArchiveTickets.aspx.cs" Inherits="frmAllArchiveTickets" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
              <style>
    .badage-sucess > span {        
  color: white !important;
  background-color:#02c27a !important;
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
     .description-label {
       display: inline-block;
       max-width: 160px; /* Set your desired width */
       white-space: nowrap;
       overflow: hidden;
       text-overflow: ellipsis;
      cursor: default;
   }
     th > img{ 
         display:none;
     }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
           
            <div class="card ">
                <div class="card-body">

                    <asp:Panel ID="pnlgridrow" runat="server">
                        <div class="row">
                            <div class="col-md-12" style="border-bottom: none">
                                <asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Red"></asp:Label>
                            </div>
                        </div>
                        <div class="row gy-3 gx-2">
                            <div class="col-md-1 col-4">
                                <label class="form-label">Filters</label>
                                <br />

                                <asp:ImageButton ID="imgcolumnfilter" runat="server" CssClass="ml-1" AlternateText="Column Chooser" Visible="false" Width="25px" ToolTip="Select Column" ImageAlign="left" ImageUrl="~/Images/New folder/columnfilter.png" OnClick="imgcolumnfilter_Click" />

                                <%--<asp:ImageButton ID="imgRowFilter" runat="server" AlternateText="Column Chooser" ToolTip="Select Column" Width="15px" ImageAlign="left" ImageUrl="~/Images/New folder/funnelfilter.png" OnClick="imgRowFilter_Click" OnClientClick="togglePanel(); return false;" />--%>
                                <asp:LinkButton ID="imgRowFilter" runat="server" OnClick="imgRowFilter_Click" ToolTip="Select Column" OnClientClick="togglePanel(); return false;" class="btn btn-outline-secondary btn-sm"><i class="fa-solid fa-filter"></i></asp:LinkButton>


                            </div>
                            <div class="col-md-3 col-8">
                                <label class="form-label">Organization</label>
                                <asp:DropDownList ID="ddlOrg" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlOrg_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3 ">
                                <label class="form-label">Request Type</label>
                                <asp:DropDownList ID="ddlRequestType" runat="server" ToolTip="Select Desk Type" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlRequestType_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2" hidden>
                                <label class="form-label ml-4">Action</label>
                                <br />
                                <asp:Button ID="btnDelteBulkTicket" runat="server" Text="Delete" ToolTip="Delete Ticket" CssClass="btn btn-sm cancelbtn" OnClick="btnDelteBulkTicket_Click" />

                                <asp:Button ID="btnPickupTicket" runat="server" Text="PickUp" ToolTip="Assign Ticket To Self" CssClass="btn btn-sm savebtn" OnClick="btnPickupTicket_Click" />

                                <asp:ImageButton ID="btnMerge" runat="server" Text="Merge" ToolTip="Merge Ticket" ImageUrl="~/Images/acitivityTrack/desk.png" Width="20px" OnClick="btnMerge_Click"></asp:ImageButton>
                                <%--	<button type="button" class="btn btn-sm btn-default" data-toggle="modal" data-target="#modal-default">
											Merge
           
										</button>--%>
                            </div>
                            <div class="col-md-3 col-6 paging ">
                                <label class="form-label">Paging</label>
                                <br />
                                <asp:Repeater ID="rptpageindexing" runat="server">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkPage" Font-Size="Small"  runat="server" Text='<%#Eval("Text") %>' CommandArgument='<%#Eval("Value") %>' Enabled='<%#Eval("Enabled") %>' OnClick="Page_Changed"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                            <div class="col-md-2 col-6">
                                <label class="form-label">Size</label>
                                <asp:DropDownList ID="ddlPageSize" runat="server" AutoPostBack="true" CssClass-="form-select form-select-sm" OnSelectedIndexChanged="PageSize_Changed">

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
                        <div class="row mt-1 gx-2 gy-3">
                            <div class="col-md-2 col-6">
                                <asp:TextBox ID="txtTicketNoFltr" runat="server" CssClass="form-control form-control-sm " placeholder="Enter Ticket No"></asp:TextBox>
                            </div>
                            <div class="col-md-3 col-6">
                                <asp:TextBox ID="txtTickSumFltr" runat="server" CssClass="form-control form-control-sm" placeholder="Enter  Summary"></asp:TextBox>
                            </div>
                            <div class="col-md-2 col-6">
                                <asp:TextBox ID="txtPriorFltr" runat="server" CssClass="form-control form-control-sm" placeholder="Enter Priority"></asp:TextBox>
                            </div>
                            <div class="col-md-2 col-6">
                                <asp:TextBox ID="txtSeverityFltr" runat="server" CssClass="form-control form-control-sm" placeholder="Enter Severity"></asp:TextBox>
                            </div>
                            <div class="col-md-2 col-6">
                                <asp:TextBox ID="txtStatfltr" runat="server" CssClass="form-control form-control-sm" placeholder="Enter Status"></asp:TextBox>
                            </div>
                            <div class="col-md-1 col-6">
                                <%--<asp:ImageButton ID="" runat="server" ImageUrl="~/Images/reset1.png"  />--%>
                                    <asp:Button ID="btnGridFilter" runat="server" Text="Go" ToolTip="Click Button to Get Ticket As Per Filter" CssClass="btn btn-grd-info btn-sm  w-100" OnClick="btnGridFilter_Click" />
                                <div class="btn-group d-none">
                                    <%--<asp:LinkButton ID="imgRemoveFilter" runat="server" OnClick="imgRemoveFilter_Click" class="btn btn-outline-secondary btn-sm" ToolTip="Removes Filter"><i class="fa-solid fa-filter-circle-xmark"></i></asp:LinkButton>--%>

                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnltickcount" runat="server" hidden>
                        <div class="row">
                            <div class="col-md-2">
                                <asp:Button ID="btnOpenTicket" ToolTip="Ticket With Open Status" Width="180px" runat="server" Text="Open  (0)" CssClass="btn  btn-outline-success  fa-pull-left" OnClick="btnOpenTicket_Click" />

                            </div>
                            <div class="col-md-2">
                                <asp:Button ID="btnWipTicket" Width="180px" ToolTip="Tickets that are not resolved and open" runat="server" Text="WIP  (0)" CssClass="btn  btn-outline-warning " OnClick="btnWipTicket_Click" />
                            </div>

                            <div class="col-md-2">
                                <asp:Button ID="btnTicketAssigntoME" ToolTip="Tickets Assgined to me" Width="180px" runat="server" Text="Assigned (0)" CssClass="btn  btn-outline-info " OnClick="btnTicketAssigntoME_Click" />
                            </div>

                            <div class="col-md-2">
                                <asp:Button ID="btnAssignToOther" Width="180px" ToolTip="Tickets that are not assign" runat="server" Text="UnAssiged-Open(0)" CssClass="btn  btn-outline-secondary " OnClick="btnAssignToOther_Click" />
                            </div>

                            <div class="col-md-2">
                                <asp:Button ID="btnDueSoonTickets" Width="180px" ToolTip="Tickets that are goin to esclate  or Response goin to Missed" runat="server" Text="Due Soon  (0)" CssClass="btn  btn-outline-dark " OnClick="btnDueSoonTickets_Click" />
                            </div>

                            <div class="col-md-2">
                                <asp:Button ID="btnTicketEsclated" Width="180px" runat="server" ToolTip="Tickets that esclated or response missed" Text="Overdue  (0)" CssClass="btn  btn-outline-danger fa-pull-left " OnClick="btnTicketEsclated_Click" />
                            </div>
                        </div>
                    </asp:Panel>

                    <div class="row">
                        <div class="col-md-12">
                            <div class="table-responsive table-container mt-3" style="max-height: 380px">
                                <asp:GridView ID="gvAllTickets" runat="server" CssClass="table table-head-fixed text-nowrap  table-sm data-table" DataKeyNames="ID" AllowCustomPaging="True"
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
                                        <asp:TemplateField HeaderText="Ticket Number" ItemStyle-Font-Size="Smaller" SortExpression="TicketNumber" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTicketNumberColor" runat="server" Text='<%# Eval("color") %>' Font-Size="Smaller" Visible="false"></asp:Label>
                                                <asp:Label ID="lblTicketNumber" runat="server" Text='<%# Eval("TicketNumber") %>' Font-Size="Smaller" Visible="true"></asp:Label>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ServiceDesk" HeaderText="Service Desk" SortExpression="ServiceDesk" ItemStyle-Height="5px" ItemStyle-Font-Size="Smaller" Visible="true" />

                                        <%--	<asp:BoundField DataField="TicketNumber" HeaderText="TicketNumber" SortExpression="TicketNumber" ItemStyle-Height="5px" ItemStyle-Font-Size="Smaller" Visible="true" />--%>
                                <asp:TemplateField HeaderText="Summary" SortExpression="Summary" Visible="true">
            <HeaderStyle  Wrap="false" />
            <ItemStyle Width="300px"  Wrap="true" CssClass="description-label" />
            <ItemTemplate>
                <!-- Displaying the Summary with ellipsis for overflow and tooltip on hover -->
                <asp:Label ID="lblSummary" runat="server" Text='<%# Eval("Summary") %>' 
                          CssClass="description-label" 
                          ToolTip='<%# Eval("Summary") %>' />
            </ItemTemplate>
        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Category" ItemStyle-Height="20px" SortExpression="Category" Visible="true">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCategoryFK" runat="server" Text='<%# Eval("Category") %>' Font-Size="Smaller" Visible="false"></asp:Label>
                                                <asp:Label ID="lblCategoryName" runat="server" Text='<%# Eval("Category") %>' Font-Size="Smaller" Visible="true"></asp:Label>

                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:BoundField DataField="CreationDate" HeaderText="Creation Date" SortExpression="CreationDate" ItemStyle-Font-Size="Smaller" ItemStyle-Height="5px" DataFormatString="{0:yyyy-MM-dd hh:mm:ss}" />

                                        <asp:BoundField DataField="Priority" HeaderText="Priority" SortExpression="Priority" ItemStyle-Font-Size="Smaller" />
                                        <asp:TemplateField HeaderText="Stage " ItemStyle-Width="150" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                            <ItemTemplate>
                                                <asp:Label ID="lblstagecode" runat="server" Font-Size="X-Small" CssClass=" badge badge-notifications" Text='<%# Eval("Stage") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Status " ItemStyle-Width="150" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                            <ItemTemplate>
                                                <asp:Label ID="lblStatusCode" runat="server" Font-Size="X-Small" CssClass=" badge badge-notifications" ForeColor="White" BackColor='<%# System.Drawing.ColorTranslator.FromHtml(Eval("StatusColorCode").ToString())%>' Text='<%# Eval("Status") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <%--	<asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" ItemStyle-Font-Size="Smaller" />--%>
                                        <asp:BoundField DataField="Severity" HeaderText="Severity" SortExpression="Severity" ItemStyle-Font-Size="Smaller" ItemStyle-Height="5px" />
                                        <%--	<asp:TemplateField HeaderText="Description" SortExpression="Description" ItemStyle-Font-Size="Smaller" >
					
					<ItemTemplate>
						<asp:Label ID="lblDescription" runat="server" Font-Size="Smaller" Text='<%# Server.HtmlDecode(Eval("Description").ToString()) %>'> </asp:Label>
					</ItemTemplate>
				</asp:TemplateField>--%>
                                        <asp:BoundField DataField="TechLoginName" HeaderText="Assignee" SortExpression="TechLoginName" ItemStyle-Height="5px" ItemStyle-Font-Size="Smaller" />
                                        <asp:BoundField DataField="SubmitterType" HeaderText="Submitter Type" SortExpression="SubmitterType" ItemStyle-Height="5px" ItemStyle-Font-Size="Smaller" />
                                        <asp:BoundField DataField="SubmitterName" HeaderText="Submitter Name" SortExpression="SubmitterName" ItemStyle-Height="5px" ItemStyle-Font-Size="Smaller" />
                                        <asp:BoundField DataField="SubmitterEmail" HeaderText="Submitter Email" SortExpression="SubmitterEmail" ItemStyle-Height="5px" ItemStyle-Font-Size="Smaller" />
                                        <asp:BoundField DataField="SubmitterPhone" HeaderText="Submitter Phone" SortExpression="SubmitterPhone" ItemStyle-Height="5px" ItemStyle-Font-Size="Smaller" />
                                        <asp:BoundField DataField="DueDate" HeaderText="Expect. Response Dt" SortExpression="SubmitterPhone" ItemStyle-Height="5px" ItemStyle-Font-Size="Smaller" />
                                        <asp:BoundField DataField="ExpectedResolutionDt" HeaderText="Expect Resolution Dt " SortExpression="SubmitterPhone" ItemStyle-Height="5px" ItemStyle-Font-Size="Smaller" />
                                        <asp:BoundField DataField="location" HeaderText="Location " SortExpression="Location" ItemStyle-Height="5px" ItemStyle-Font-Size="Smaller" />

                                        <asp:BoundField DataField="SourceType" HeaderText="SourceType" SortExpression="Source Type" ItemStyle-Height="5px" ItemStyle-Font-Size="Smaller" />
                                    </Columns>
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


                </div>


            </div>


            <div class="modal fade" id="modal-default">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h4 class="modal-title position-fixed  ">Default Modal</h4>
                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="modal-body">
                            <p>One fine body&hellip;</p>
                        </div>
                        <div class="modal-footer justify-content-between">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                            <button type="button" class="btn btn-primary">Save changes</button>
                        </div>
                    </div>
                    <!-- /.modal-content -->
                </div>
                <!-- /.modal-dialog -->
            </div>
            <div class="modal" id="CategoryModal" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true">
                <div class="modal-dialog" style="bottom: 20%">
                    <div class="modal-content">
                        <div class="card-header ">
                            Select Colums
							<button type="button" class=" fa-pull-right badge-danger" data-dismiss="modal" onclick="javascript:window.location.reload()" aria-hidden="true">&times;</button>


                        </div>
                        <%--  <asp:UpdatePanel ID="UpdatePanel2" runat="server">
							<ContentTemplate>--%>
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
            <aside class="control-sidebar control-sidebar-light">
                <!-- Control sidebar content goes here -->

            </aside>
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
            <asp:PostBackTrigger ControlID="btnMerge" />


            <asp:PostBackTrigger ControlID="btnOpenTicket" />
            <asp:PostBackTrigger ControlID="btnWipTicket" />
            <asp:PostBackTrigger ControlID="btnTicketAssigntoME" />
            <asp:PostBackTrigger ControlID="btnAssignToOther" />
            <asp:PostBackTrigger ControlID="btnDueSoonTickets" />
            <asp:PostBackTrigger ControlID="btnTicketEsclated" />

        </Triggers>
    </asp:UpdatePanel>

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
    <script type="text/javascript">
        function Showalert(imtype, imtitle) {
            // alert('Call JavaScript function from codebehind');
            // var typeof=type
            // var titleof = title;
            var Toast = Swal.mixin({
                toast: true,
                position: 'top-middle',

                showConfirmButton: false,
                showCloseButton: true,
                timer: 4000,


            });
            console.log("hello");
            Toast.fire({
                /*icon: 'success',*/
                //type: 'success',
                //title: 'Lorem ipsum dolor sit amet, consetetur sadipscing elitr.'
                icon: imtype,
                title: imtitle
            });

            console.log("fire1234567");
        }
    </script>
    <link rel="stylesheet" type="text/css" href="../Script/build/jquery.datetimepicker.css" />
    <script src="../Script/build/jquery.js"></script>
    <script src="../Script/build/jquery.datetimepicker.full.js"></script>
    <script src="../plugins/jquery/jquery.min.js"></script>
    <!-- Bootstrap -->
    <script src="../plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
    <!-- overlayScrollbars -->
    <script src="../plugins/overlayScrollbars/js/jquery.overlayScrollbars.min.js"></script>
    <!-- AdminLTE App -->
    <script src="../Scripts/chosen.jquery.js" type="text/javascript"></script>
    <script type="text/javascript"> $(".chzn-select").chosen();
        $(".chzn-select-deselect").chosen({ allow_single_deselect: true }); </script>
    <link rel="stylesheet" href="../Scripts/chosen.css" />
    <script src="../plugins/toastr/toastr.min.js"></script>
    <script src="../plugins/sweetalert2/sweetalert2.min.js"></script>
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
</asp:Content>


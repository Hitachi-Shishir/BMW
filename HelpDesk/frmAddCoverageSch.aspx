<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmAddCoverageSch.aspx.cs" Inherits="frmAddCoverageSch" %>

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
            margin-top: -1.7rem!important;
        }
        [data-bs-theme=dark] .select2-container--bootstrap-5 .select2-selection--multiple .select2-selection__rendered .select2-selection__choice{
            color: white;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="scriptmanager1" runat="server"></asp:ScriptManager>

    <asp:UpdatePanel ID="updatepanel1" runat="server">
        <ContentTemplate>
           
            <div class="card">
                <div class="card-body">
                    <div class="row">
                        <div class="col-md-12 mb-2">
                            <div class="btn-group">
                            <asp:Button ID="btnAddSLA" Text="Coverage Schedule" runat="server" CssClass="btn btn-sm btn-outline-secondary" OnClick="btnAddSLA_Click" />
                            <asp:Button ID="btnViewSLA" runat="server" Text-="View Details" CssClass="btn btn-sm btn-secondary" OnClick="btnViewSLA_Click" />
                        </div>
                    </div>
                    </div>

                    <div class="card border bg-transparent shadow-none ">
                        <div class="card-body">

                            <asp:Panel ID="pnlAddSLA" runat="server" Visible="false">
                                <div class="  row gy-3 gx-2">
                                    <div class="col-md-4">
                                        <label for="staticEmail" class="form-label">
                                            Name 
                                        <asp:RequiredFieldValidator ID="rfvtxtCvrgname" runat="server" ControlToValidate="txtCvrgname" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="SLA"></asp:RequiredFieldValidator>
                                        </label>
                                        <asp:TextBox onkeypress="return /^[a-zA-Z-_ ]*$/.test(event.key) && this.value.length < 50;" ID="txtCvrgname" runat="server" CssClass="form-control  form-control-sm"></asp:TextBox>
                                    </div>
                                    <div class="col-md-8">
                                        <label for="staticEmail" class="form-label">
                                            Days Covered 
                                        <asp:RequiredFieldValidator ID="rfvlstDaysCvrd" runat="server" ControlToValidate="lstDaysCvrd" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="SLA"></asp:RequiredFieldValidator>
                                        </label>
                                        <asp:ListBox ID="lstDaysCvrd" runat="server" SelectionMode="Multiple" CssClass="form-select form-select-sm single-select-optgroup-field">

                                            <asp:ListItem Text="Monday" Value="Monday"></asp:ListItem>
                                            <asp:ListItem Text="Tuesday" Value="Tuesday"></asp:ListItem>
                                            <asp:ListItem Text="Wednesday" Value="Wednesday"></asp:ListItem>
                                            <asp:ListItem Text="Thursday" Value="Thursday"></asp:ListItem>
                                            <asp:ListItem Text="Friday" Value="Friday"></asp:ListItem>
                                            <asp:ListItem Text="Saturday" Value="Saturday"></asp:ListItem>
                                            <asp:ListItem Text="Sunday" Value="Sunday"></asp:ListItem>
                                        </asp:ListBox>
                                    </div>
                                    <div class="  col-md-12">

                                        <label for="staticEmail" class="form-label">
                                            Hours Covered
                                        <asp:RequiredFieldValidator ID="rfvrdblist" runat="server" ControlToValidate="rdblist" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="SLA"></asp:RequiredFieldValidator>
                                        </label>
                                        <asp:RadioButtonList ID="rdblist" runat="server" RepeatDirection="Horizontal" CellPadding="5" CellSpacing="20" AutoPostBack="true" OnSelectedIndexChanged="rdblist_SelectedIndexChanged">
                                            <asp:ListItem Text="No Coverage" Value="NoCoverage" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="24 Hour Coverage" Value="24hrCoverage"></asp:ListItem>
                                            <asp:ListItem Text="Use these Hours" Value="UseTheseHours"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                                <asp:Panel ID="pnlcvrgSch" runat="server" Enabled="false">
                                    <div class="  row mt-2 gy-3 gx-2">
                                        <div class="col-md-3">
                                            <label for="staticEmail" class="form-label">
                                                Begin Hour
                                            <asp:RequiredFieldValidator ID="rfvtxtBeginHour" runat="server" ControlToValidate="txtBeginHour" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="SLA"></asp:RequiredFieldValidator>
                                            </label>
                                            <asp:TextBox ID="txtBeginHour" TextMode="Time" runat="server" CssClass="form-control  form-control-sm "></asp:TextBox>
                                        </div>

                                        <div class="col-md-3">
                                            <label for="staticEmail" class="form-label">
                                                End  Hour 
                                            <asp:RequiredFieldValidator ID="rfvtxtEndHour" runat="server" ControlToValidate="txtEndHour" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="SLA"></asp:RequiredFieldValidator>
                                            </label>

                                            <asp:TextBox ID="txtEndHour" TextMode="Time" runat="server" CssClass=" form-control-sm form-control "></asp:TextBox>
                                        </div>
                                    </div>
                                </asp:Panel>
                                <div class="row mt-3">
                                    <div class="col-md-12 ">
                                        <asp:Button ID="btnInsertSLA" runat="server" Text="Add" class="btn btn-sm btn-grd btn-grd-info " OnClick="btnInsertSLA_Click" ValidationGroup="SLA" />
                                        <asp:Button ID="btnUpdateSLA" runat="server" Text="Update" Visible="false" class="btn btn-sm btn-grd btn-grd-info " OnClick="btnUpdateSLA_Click" ValidationGroup="SLA" />
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="btn btn-sm btn-grd btn-grd-danger " OnClick="btnCancel_Click" CausesValidation="false" />
                                    </div>
                                </div>

                            </asp:Panel>
                            <asp:Panel ID="pnlViewSLA" runat="server">

                                <div class="row ">

                                    <div class="col-md-6">
                                        <asp:Label ID="Label1" runat="server"></asp:Label>
                                        <asp:Label ID="Label3" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-md-2  d-none">
                                        <div class="btn btn-sm elevation-1 ml-1 " style="padding: 0px; margin-bottom: 10px; padding-top: 1px">
                                            <label class="mr-2 ml-1 mb-0">Export</label>
                                            <asp:ImageButton ID="ImgBtnExport" runat="server" ImageUrl="~/Images/New folder/excelnew.png" CssClass="fa-pull-right btn-outline-success mr-1" OnClick="ImgBtnExport_Click" />
                                        </div>
                                    </div>
                                    <div class="col-md-12">
                                        <div class="table-responsive table-container" style="width: 100%">
                                            <asp:GridView GridLines="None" ID="gvSLA" runat="server" DataKeyNames="ID" AutoGenerateColumns="false" CssClass="data-table table table-striped border table-hover table-sm text-nowrap"
                                                Width="100%" OnRowCommand="gvSLA_RowCommand" OnRowDataBound="gvSLA_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="20">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex+1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="ScdhuleName" HeaderText="Schedule Name" NullDisplayText="NA" />
                                                    <asp:BoundField DataField="DaysCovered" HeaderText="Days Covered" NullDisplayText="NA" />
                                                    <asp:BoundField DataField="HoursCovered" HeaderText="Hours Covered" NullDisplayText="NA" />
                                                    <asp:BoundField DataField="BeginHour" HeaderText="Begin Hour" NullDisplayText="NA" />
                                                    <asp:BoundField DataField="EndHour" HeaderText="End Hour" NullDisplayText="NA" />
                                                    <asp:TemplateField HeaderText="Edit">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkUpdateSLA" runat="server" CommandName="UpdateSLA" CommandArgument="<%# Container.DataItemIndex %>" CssClass="center-btn">
            <i class="fa-solid fa-edit"></i>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Delete">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkDeleteSLA" runat="server" CommandName="DeleteSLA" CommandArgument="<%# Container.DataItemIndex %>" CssClass="center-btn">
           <i class="fa-solid fa-xmark text-danger"></i>
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <%-- <RowStyle BackColor="White" BorderColor="#e3e4e6" BorderWidth="1px" Height="10px" />
        <FooterStyle BackColor="#EDEDED" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#EDEDED" ForeColor="#000000" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#fff" Font-Bold="True" ForeColor="#000000" />
        <HeaderStyle BackColor="#EEEEEE" Font-Bold="True" ForeColor="#414141" Height="30px" CssClass="header" Font-Size="Small" />
        <EditRowStyle BackColor="#e9edf2" BorderColor="#e3e4e6" BorderStyle="Solid" BorderWidth="1px" />
        <AlternatingRowStyle BackColor="#EAEEFF" BorderColor="#e3e4e6" BorderStyle="Solid" BorderWidth="1px" />--%>
                                            </asp:GridView>
                                        </div>
                                    </div>

                                </div>


                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
            </div>
            </div>
            <div class="modal " id="basicModal" <%-- tabindex="-1"--%> role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true">
                <div class="modal-dialog modal-dialog-centered">
                    <div class="modal-content">
                        <div class="card-header">
                            New Request Type
                        <button type="button" class="close" onclick="javascript:window.location.reload()" data-dismiss="modal" aria-hidden="true">&times;</button>


                        </div>
                        <%--  <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>--%>
                        <div class="card-body">
                            <asp:Label ID="lblsuccess" runat="server" Text=""></asp:Label>
                            <asp:PlaceHolder ID="pnlShowRqstType" runat="server"></asp:PlaceHolder>
                        </div>
                    </div>
                </div>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnAddSLA" />
            <asp:PostBackTrigger ControlID="ImgBtnExport" />
            <asp:PostBackTrigger ControlID="btnViewSLA" />
            <%--<asp:PostBackTrigger ControlID="btnInsertSLA" />--%>
            <asp:PostBackTrigger ControlID="rdblist" />
            <asp:PostBackTrigger ControlID="gvSLA" />
            <%--<asp:PostBackTrigger ControlID="btnUpdateSLA" />--%>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>


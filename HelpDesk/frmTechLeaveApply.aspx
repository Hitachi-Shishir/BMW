<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmTechLeaveApply.aspx.cs" Inherits="frmTechLeaveApply" %>

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
            margin-top: -1.7rem !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            </div>

        <div class="card mb-2">
            <div class="card-body">
                <div class="row gy-3 gx-2">
                    <div class="col-md-3 col-6">
                        <label class="form-label">Organization</label>
                        <asp:DropDownList ID="ddlOrg" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlOrg_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                    <div class="col-md-3 col-6">
                        <label class="form-label">Technician</label>
                        <asp:DropDownList ID="ddltech" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field"></asp:DropDownList>
                    </div>
                    <div class="col-md-2 col-6">
                        <label class="form-label">Leave Type</label>
                        <asp:DropDownList ID="ddlLeaveType" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field">
                            <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                            <asp:ListItem Value="1" Text="1st Half"></asp:ListItem>
                            <asp:ListItem Value="2" Text="2nd Half"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2 col-6">
                        <label class="form-label">Leave From Date</label>
                        <asp:TextBox runat="server" ID="txtfrmDate" class="form-control form-control-sm datepicker"></asp:TextBox>
                    </div>
                    <div class="col-md-2 col-6">
                        <label class="form-label">Leave To Date</label>
                        <asp:TextBox runat="server" ID="txttoDate" class="form-control form-control-sm datepicker"></asp:TextBox>
                    </div>
                    <div class="col-md-12 text-end">
                       
                        <asp:Button ID="btnSave" runat="server" Text="Add" CssClass="btn btn-sm  btn-grd btn-grd-info " OnClick="btnSave_Click" />
                        <asp:Button ID="btnReset" runat="server" Text="Cancel" CssClass="btn btn-sm  btn-grd btn-grd-danger" OnClick="btnReset_Click" />
                    </div>
                </div>
                </div>
                </div>
                <div class="card">
        <div class="card-body">
                <div class="row mt-3">
                    <div class="col-12">
                        <div class="table-responsive table-container">
                            <asp:GridView ID="grv" HeaderStyle-Height="25px" ShowHeaderWhenEmpty="true" AutoGenerateColumns="false"
                                runat="server" Width="100%" Class="data-table table table-striped border table-sm text-nowrap">
                                <Columns>
                                    <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Sr No">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Technician">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdnid" Value='<%#Eval("id")%>' runat="server" />
                                            <asp:Label ID="lblTechName" runat="server" Text='<%#Eval("TechName")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Leave From Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLeaveFromdate" runat="server"
                                                Text='<%# Eval("LeaveFromdate") == DBNull.Value ? "" : ((DateTime)Eval("LeaveFromdate")).ToString("dd-MM-yyyy") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Leave To Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLeaveTodate" runat="server"
                                                Text='<%# Eval("LeaveTodate") == DBNull.Value ? "" : ((DateTime)Eval("LeaveTodate")).ToString("dd-MM-yyyy") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Leave Apply Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblApplyDate" runat="server"
                                                Text='<%# Eval("ApplyDate") == DBNull.Value ? "" : ((DateTime)Eval("ApplyDate")).ToString("dd-MM-yyyy") %>'>
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>


                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" ID="lnkdelete" OnClick="lnkdelete_Click" OnClientClick="return confirm('Are you sure you want to Delete this ?');">
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
            <asp:PostBackTrigger ControlID="ddlOrg" />
            <asp:PostBackTrigger ControlID="grv" />
            <asp:PostBackTrigger ControlID="btnSave" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>


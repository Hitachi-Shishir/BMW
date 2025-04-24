<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmAddUserScope.aspx.cs" Inherits="HelpDesk_frmAddUserScope" %>

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
    <asp:ScriptManager ID="scriptmanager1" runat="server"></asp:ScriptManager>

    <asp:UpdatePanel ID="updatepanel1" runat="server">
        <ContentTemplate>
            <div class="card">
                <div class="card-body">
                    <div class="row mb-2">
                        <div class="col-md-12">
                            <div class="btn-group">
                                <asp:Button ID="btnAddUserScope" Text="Add Scope" runat="server" CssClass="btn btn-sm btn-outline-secondary " OnClick="btnAddUserScope_Click" />
                                <asp:Button ID="btnViewScope" runat="server" Text-="View Details" CssClass="btn btn-sm btn-secondary " OnClick="btnViewScope_Click" />
                            </div>
                        </div>
                    </div>
                    <div class="card shadow-none border  ">
                        <div class="card-body">
                            <asp:Panel ID="pnlAddscope" runat="server" Visible="false">
                                <div class="  row gy-3 gx-2">
                                    <div class="col-md-6">
                                        <label for="staticEmail" class="form-label">
                                            Scope Name 
                                        <asp:RequiredFieldValidator ID="rfvtxtScopeName" runat="server" ControlToValidate="txtScopeName" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="UserScope"></asp:RequiredFieldValidator>
                                        </label>

                                        <asp:TextBox onkeypress="return /^[a-zA-Z-_ ]*$/.test(event.key) && this.value.length < 50;" ID="txtScopeName" runat="server" CssClass="form-control  form-control-sm"></asp:TextBox>
                                    </div>
                                    <div class="col-md-6">
                                        <label for="staticEmail" class="form-label">
                                            Scope Status 
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtScopeDesc" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="UserScope"></asp:RequiredFieldValidator>
                                        </label>

                                        <asp:DropDownList ID="ddlScopeStatus" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field">
                                            <asp:ListItem Text="Active" Selected="True" Value="True"></asp:ListItem>
                                            <asp:ListItem Text="In Active" Value="False"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-12">
                                        <label for="staticEmail" class="form-label">
                                            Scope Description 
     <asp:RequiredFieldValidator ID="rfvtxtScopeDesc" runat="server" ControlToValidate="txtScopeDesc" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="UserScope"></asp:RequiredFieldValidator>
                                        </label>

                                        <asp:TextBox ID="txtScopeDesc" runat="server" TextMode="MultiLine" Rows="5" Columns="5" CssClass="form-control  form-control-sm"></asp:TextBox>

                                    </div>
                                    <div class="col-md-12 text-end">
                                        <asp:Button ID="btnInsertScope" runat="server" Text="Save" class="btn btn-sm btn-grd btn-grd-info " OnClick="btnInsertScope_Click" ValidationGroup="UserScope" />
                                        <asp:Button ID="btnUpdateScope" runat="server" Text="Update" Visible="false" class="btn btn-sm btn-grd btn-grd-info " OnClick="btnUpdateScope_Click" ValidationGroup="UserScope" />
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="btn btn-sm btn-grd btn-grd-danger " OnClick="btnCancel_Click" CausesValidation="false" />
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="pnlViewScope" runat="server">
                                <div class="row ">
                                    <%--<div class="col-md-4">

                                                                <asp:Label ID="lblsofname" runat="server" Text="Scope Details" Font-Size="Larger" ForeColor="Black"></asp:Label>

                                                            </div>--%>
                                    <div class="col-md-6">
                                        <asp:Label ID="Label1" runat="server"></asp:Label>
                                        <asp:Label ID="Label3" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-md-2 d-none">
                                        <div class="btn btn-sm elevation-1 ml-1 " style="padding: 0px; margin-bottom: 10px; padding-top: 1px">
                                            <label class="mr-2 ml-1 mb-0">Export</label>
                                            <asp:ImageButton ID="ImgBtnExport" runat="server" ImageUrl="~/Images/New folder/excelnew.png" CssClass="fa-pull-right btn-outline-success mr-1" OnClick="ImgBtnExport_Click" />
                                        </div>
                                    </div>
                                    <div class="col-md-12">
                                        <div class="table-responsive table-container">
                                            <asp:GridView GridLines="None" ID="gvScope" runat="server" DataKeyNames="ScopeID" AutoGenerateColumns="false" CssClass="data-table table table-striped border table-sm text-nowrap"
                                                Width="100%" OnRowCommand="gvScope_RowCommand" OnRowDataBound="gvScope_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="20">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex+1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:BoundField DataField="ScopeName" HeaderText="Scope Name" NullDisplayText="NA" />
                                                    <asp:BoundField DataField="ScopeDesc" HeaderText="Scope Description" NullDisplayText="NA" />
                                                    <asp:BoundField DataField="IsActive" HeaderText="Status" />
                                                    <asp:TemplateField HeaderText="Edit">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="UpdateScope" CommandArgument="<%# Container.DataItemIndex %>">
<i class="fa-solid fa-edit"></i> <!-- FontAwesome icon -->
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Delete">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="DeleteScope" CommandArgument="<%# Container.DataItemIndex %>"  OnClientClick="return confirm('Are you sure you want to delete this record?');">
<i class="fa-solid fa-xmark text-danger"></i> <!-- FontAwesome icon -->
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <%--<asp:ButtonField ButtonType="Image" CommandName="UpdateScope" HeaderText="Edit" ImageUrl="~/images/edit23.png" ItemStyle-Width="20px" />--%>
                                                    <%--<asp:ButtonField HeaderText="Delete" ButtonType="Image" ImageUrl="~/Images/New folder/delnew.png" CommandName="DeleteScope" ItemStyle-Width="20px" ItemStyle-Height="5px" />--%>
                                                </Columns>
                                            </asp:GridView>

                                        </div>
                                    </div>

                                </div>

                            </asp:Panel>
                        </div>
                    </div>

                </div>
            </div>

            <div class="modal " id="basicModal" <%-- tabindex="-1"--%> role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true">
                <div class="modal-dialog modal-dialog-centered">
                    <div class="modal-content">
                        <div class="card-header" style="border-bottom: none">

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
            <asp:PostBackTrigger ControlID="ImgBtnExport" />
            <asp:PostBackTrigger ControlID="btnAddUserScope" />

            <asp:PostBackTrigger ControlID="btnViewScope" />

            <%--<asp:PostBackTrigger ControlID="btnInsertScope" />--%>

            <%--<asp:PostBackTrigger ControlID="gvScope" />--%>
            <%--<asp:PostBackTrigger ControlID="btnUpdateScope" />--%>
        </Triggers>

    </asp:UpdatePanel>
</asp:Content>


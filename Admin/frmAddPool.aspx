<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmAddPool.aspx.cs" Inherits="Admin_frmAddPool" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="scriptmanager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="updatepanel1" runat="server">
        <ContentTemplate>


            <div class="card">
                <div class="card-body">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="btn-group">
                                <asp:Button ID="btnAddSLA" Text="Pool" runat="server" CssClass="btn btn-sm btn-outline-secondary" OnClick="btnAddSLA_Click" />
                                <asp:Button ID="btnViewSLA" runat="server" Text-="View Details" CssClass="btn btn-sm btn-secondary" OnClick="btnViewSLA_Click" />
                            </div>
                        </div>
                    </div>
                    <asp:Panel ID="pnlAddSLA" runat="server" Visible="false">
                        <div class="card rounded border mt-2">
                            <div class="card-body">
                                <div class=" row  gy-2 gx-3">
                                    <div class="col-md-3">
                                        <label for="staticEmail" class="form-label">
                                            Organization <span title="*"></span>
                                            <asp:RequiredFieldValidator ID="rfvddlOrg" runat="server" ControlToValidate="ddlOrg" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="SLA"></asp:RequiredFieldValidator>
                                        </label>
                                        <asp:DropDownList ID="ddlOrg" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlOrg_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-3">
                                        <label for="staticEmail" class="form-label">
                                            Name <span title="*"></span>
                                            <asp:RequiredFieldValidator ID="rfvtxtCvrgname" runat="server" ControlToValidate="txtPoolName" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="SLA"></asp:RequiredFieldValidator>
                                        </label>
                                        <asp:TextBox ID="txtPoolName" runat="server" CssClass="form-control  form-control-sm"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3">
                                        <label for="staticEmail" class="form-label">
                                            Location
                                            <asp:RequiredFieldValidator ID="rfvddlLocation" runat="server" ControlToValidate="ddlLocation" InitialValue="0" ValidationGroup="SLA" ForeColor="Red" ErrorMessage="*"></asp:RequiredFieldValidator>
                                        </label>

                                        <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control form-control-sm chzn-select"></asp:DropDownList>
                                    </div>
                                    <div class="col-md-3">
                                        <label for="staticEmail" class="form-label">
                                            Engineers  <span title="*"></span>
                                            <asp:RequiredFieldValidator ID="rfvlstEngineer" runat="server" ControlToValidate="lstEngineer" ErrorMessage="*" Font-Bold="true" ForeColor="Red" InitialValue="0" ValidationGroup="SLA"></asp:RequiredFieldValidator>
                                        </label>

                                        <asp:ListBox ID="lstEngineer" runat="server" SelectionMode="Multiple" CssClass="form-select form-select-sm single-select-optgroup-field"></asp:ListBox>
                                    </div>
                                    <div class="col-md-12 text-end">
                                        <asp:Button ID="btnInsertSLA" runat="server" Text="Save" class="btn btn-sm btn-grd-info" OnClick="btnInsertSLA_Click" ValidationGroup="SLA" />
                                        <asp:Button ID="btnUpdateSLA" runat="server" Text="Update" Visible="false" class="btn btn-sm btn-grd-info" OnClick="btnUpdateSLA_Click" ValidationGroup="SLA" />
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="btn btn-sm btn-grd-danger" OnClick="btnCancel_Click" CausesValidation="false" />

                                    </div>

                                </div>
                            </div>
                        </div>






                    </asp:Panel>
                    <asp:Panel ID="pnlViewSLA" runat="server">
                        <div class="card border rounded mt-2">
                            <div class="card-body">
                                <div class="row ">
                                    <div class="col-md-6">
                                        <asp:Label ID="Label1" runat="server"></asp:Label>
                                        <asp:Label ID="Label3" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-md-12 text-end">
                                        <div class="btn btn-sm elevation-1 ml-1 " style="padding: 0px; margin-bottom: 10px; padding-top: 1px">

                                            <asp:LinkButton ID="ImgBtnExport" runat="server" CssClass="btn-outline-secondary btn btn-sm" OnClick="ImgBtnExport_Click"><i class="fa-solid fa-download"></i> Export</asp:LinkButton>
                                        </div>
                                    </div>

                                </div>
                                <div class="table-responsive table-container" style="max-height: 380px; width: 100%">
                                    <asp:GridView GridLines="None" ID="gvSLA" runat="server" DataKeyNames="ID,OrgID" AutoGenerateColumns="false" CssClass="table table-head-fixed text-nowrap table-sm border"
                                        Width="100%" OnRowCommand="gvSLA_RowCommand" OnRowDataBound="gvSLA_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="20">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="PoolName" HeaderText="Pool Name" NullDisplayText="NA" />
                                            <asp:BoundField DataField="Location" HeaderText="Location" NullDisplayText="NA" />
                                            <asp:BoundField DataField="Engineer" HeaderText="Engineers" NullDisplayText="NA" />
                                            <asp:BoundField DataField="OrgID" HeaderText="OrgID" NullDisplayText="NA" Visible="false" />
                                            <asp:TemplateField HeaderText="Edit">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="UpdateSLA" CommandArgument="<%# Container.DataItemIndex %>">
<i class="fa-solid fa-edit"></i> <!-- FontAwesome icon -->
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                                                                        <asp:TemplateField HeaderText="Delete">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="DeleteSLA" CommandArgument="<%# Container.DataItemIndex %>">
<i class="fa-solid fa-trash text-danger"></i> <!-- FontAwesome icon -->
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>

                    </asp:Panel>
                </div>
            </div>

            <div class="modal " id="basicModal" <%-- tabindex="-1"--%> role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true">
                <div class="modal-dialog modal-dialog-centered">
                    <div class="modal-content">
                        <div class="card-header">
                            New Request Type
                        <button type="button" class="close" onclick="javascript:window.location.reload()" data-dismiss="modal" aria-hidden="true">&times;</button>
                        </div>
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
            <asp:PostBackTrigger ControlID="btnInsertSLA" />
            <asp:PostBackTrigger ControlID="gvSLA" />
            <asp:PostBackTrigger ControlID="btnUpdateSLA" />
            <asp:PostBackTrigger ControlID="ddlOrg" />
        </Triggers>

    </asp:UpdatePanel>
</asp:Content>


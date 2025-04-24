<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmAddNewsUpdates.aspx.cs" Inherits="frmAddNewsUpdates" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="card mb-2">
                <div class="card-body">
                    <div class="row gy-3 gx-2">
                        <div class="col-md-3 col-6">
                            <label class="form-label">Organization
                            <asp:RequiredFieldValidator ID="rfvddlOrganization" runat="server" ControlToValidate="ddlOrg" ErrorMessage="*" ForeColor="Red" ValidationGroup="AddNews" InitialValue="0" ></asp:RequiredFieldValidator>
                                </label>
                            <asp:DropDownList ID="ddlOrg" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlOrg_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                        <div class="col-md-3 col-6">
                            <label class="form-label">Status
                                <asp:RequiredFieldValidator ID="rfvtxtddlStatus" runat="server" ControlToValidate="ddlStatus" ErrorMessage="*" ForeColor="Red" ValidationGroup="AddNews" InitialValue="0" ></asp:RequiredFieldValidator>
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" >
                                    <asp:ListItem Value="0" Text="--Select--"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Active"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="InAvtive"></asp:ListItem>
                                </asp:DropDownList>
                            </label>
                        </div>
                       
                        <div class="col-md-5 col-6">
                            <label class="form-label">Add News
                                <asp:RequiredFieldValidator ID="rfvtxttxtNews" runat="server" ControlToValidate="txtNews" ErrorMessage="*" ForeColor="Red" ValidationGroup="AddNews"  ></asp:RequiredFieldValidator>
                            </label>
                            <asp:TextBox ID="txtNews" runat="server" TextMode="MultiLine" CssClass="form-control form-control-sm" Rows="4" Columns="2"></asp:TextBox>

                        </div>
                        <div class="col-md-1 col-6">
                            <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-success btn-sm" OnClick="btnAdd_Click" ValidationGroup="AddNews" />
                        </div>
                        <div class="col-md-1 col-6">
                            <asp:Button ID="btnCancel" runat="server" Text="Canacel" CssClass="btn btn-danger btn-sm" OnClick="btnCancel_Click"/>
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
                                        <asp:TemplateField HeaderText="News">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnid" Value='<%#Eval("id")%>' runat="server" />
                                                <asp:Label ID="lblNews" runat="server" Text='<%#Eval("News")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <%--<asp:TemplateField HeaderText="From Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblfrmdate" runat="server"
                                                    Text='<%# Eval("frmdate") == DBNull.Value ? "" : ((DateTime)Eval("frmdate")).ToString("dd-MM-yyyy") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="To Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lbltodate" runat="server"
                                                    Text='<%# Eval("todate") == DBNull.Value ? "" : ((DateTime)Eval("todate")).ToString("dd-MM-yyyy") %>'>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                        <asp:TemplateField HeaderText="Edit">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="lnkEdit" OnClick="lnkEdit_Click" OnClientClick="return confirm('Are you sure you want to Edit this ?');">
<i class="fa fa-edit text-danger"></i>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Delete">
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
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>


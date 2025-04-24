<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmAddSeverity.aspx.cs" Inherits="frmAddSeverity" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
      <asp:ScriptManager ID="scriptmanager1" runat="server"></asp:ScriptManager>
    <asp:Panel runat="server" ID="pnlAddSeverity" >
        <asp:UpdatePanel ID="updatepanel5" runat="server">
            <ContentTemplate>

                <div class="card mb-1" style="border-radius: 0 0 0.375rem 0.375rem">
                    <div class="card-body">
                        <div class="d-flex align-items-start justify-content-between mb-2">
                            <h6 class="mb-3 fw-bold">Add Severity</h6>

                        </div>

                        <div class="row gx-2 gy-3">
                            <div class="col-md-3">
                                <label for="staticEmail" class="form-label">
                                    Organization
                                   
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlOrg4" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Severity"></asp:RequiredFieldValidator>
                                </label>

                                <asp:DropDownList  ID="ddlOrg4" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlOrg4_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3">
                                <label for="staticEmail" class="form-label">
                                    Request Type
                                   
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="ddlRequestTypeSeverity" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Severity"></asp:RequiredFieldValidator>
                                </label>


                                <asp:DropDownList ID="ddlRequestTypeSeverity" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlRequestTypeSeverity_SelectedIndexChanged">
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
                <asp:PostBackTrigger ControlID="ddlRequestTypeSeverity" />
                <asp:PostBackTrigger ControlID="gvSeverity" />
                <asp:PostBackTrigger ControlID="ddlOrg4" />
                <asp:PostBackTrigger ControlID="ImgBtnExport4" />
            </Triggers>

        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>


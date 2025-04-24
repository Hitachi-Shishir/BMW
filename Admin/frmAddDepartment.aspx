<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmAddDepartment.aspx.cs" Inherits="Admin_frmAddDepartment" %>

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


            <div class="card">
                <div class="card-body">
                    <div class="row mb-2">
                        <div class="col-md-12">
                            <div class="btn-group btn-group-sm">
                                <asp:Button ID="btnAddDep" Text="Add Department" runat="server" CssClass="btn btn-sm btn-outline-secondary" OnClick="btnAddDep_Click" />
                                <asp:Button ID="btnViewDep" runat="server" Text-="View Details" CssClass="btn btn-sm btn-outline-secondary" OnClick="btnViewDep_Click" />
                            </div>
                        </div>
                    </div>

                    <asp:Panel ID="pnlAddDep" runat="server" Visible="false">
                        <div class="card rounded border">
                            <div class="card-body">

                           
                        <div class=" row gx-2 gy-3">
                            <div class="col-md-4">
                                <label for="staticEmail" class="form-label">
                                    Organization: <span title="*"></span>
                                    <asp:RequiredFieldValidator ID="rfvddlOrg" runat="server" ControlToValidate="ddlOrg" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Severity"></asp:RequiredFieldValidator>
                                </label>
                                <asp:DropDownList ID="ddlOrg" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-4">
                                <label for="staticEmail" class="form-label">
                                    Department Code 
                                <asp:RequiredFieldValidator ID="RfvtxtDepartmentCode" runat="server" ControlToValidate="txtDepartmentCode" ErrorMessage="Required" Font-Bold="true" ForeColor="Red" ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                                </label>

                                <asp:TextBox ID="txtDepartmentCode" runat="server" CssClass="form-control form-control-sm" AutoComplete="off"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label for="staticEmail" class="form-label">
                                    Department Name
                                <asp:RequiredFieldValidator ID="rfvtxtDepartmentName" runat="server" ControlToValidate="txtDepartmentName" ErrorMessage="Required" Font-Bold="true" ForeColor="Red" ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                                </label>

                                <asp:TextBox ID="txtDepartmentName" runat="server" CssClass="form-control form-control-sm" AutoComplete="off"></asp:TextBox>
                            </div>
                            <div class="col-md-4 ">
                                <label class=" form-label" style="color: transparent">..</label><br />

                                <asp:Button ID="btnTypeAdd" runat="server" CssClass="btn btn-sm btn-grd btn-grd-info " Text="Save" ValidationGroup="btnSave" OnClick="btnTypeAdd_Click" />
                                <asp:Button ID="btnUpdate" runat="server" CssClass="btn btn-sm btn-grd btn-grd-info " Text="Update" ValidationGroup="btnSave" OnClick="btnUpdate_Click" />
                                <asp:Label ID="lblsuccess" runat="server" Text=""></asp:Label>
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="btn btn-sm btn-grd btn-grd-danger " OnClick="btnCancel_Click" CausesValidation="false" />
                            </div>
                            <div class="col-md-12">
                            </div>
                        </div>
    </div>
</div>
                    </asp:Panel>
                    <asp:Panel ID="pnlViewDepart" runat="server">
 <div class="card rounded border">
     <div class="card-body">
                        <div class="row ">

                            <div class="col-md-6">
                                <asp:Label ID="Label1" runat="server"></asp:Label>
                                <asp:Label ID="Label3" runat="server"></asp:Label>
                            </div>
                            <div class="col-md-2 d-none">
                                <div class="btn btn-sm shadow-lg ml-1 mr-0 " style="padding: 0px; margin-bottom: 10px; padding-top: 1px">
                                    <label class="mr-2  mb-0">Export</label>
                                    <asp:ImageButton ID="ImgBtnExport" runat="server" ImageUrl="~/Images/New folder/excelnew.png" CssClass="fa-pull-right btn-outline-success" OnClick="ImgBtnExport_Click" />
                                </div>
                            </div>

                            <div class="col-md-12">
                                <div class="table-responsive">
                                    <asp:GridView GridLines="None" ID="gvstate" runat="server" DataKeyNames="ID" AutoGenerateColumns="false" CssClass="data-table table table-striped border table-sm text-nowrap"
                                        Width="100%" OnRowCommand="gvstate_RowCommand">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="20">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="DepartmentCode" HeaderText="Department Code" NullDisplayText="0" />
                                            <asp:BoundField DataField="DepartmentName" HeaderText="Department Name" NullDisplayText="0" />
                                            <asp:BoundField DataField="OrgID" HeaderText="OrgID" NullDisplayText="0" />
                                            <asp:TemplateField HeaderText="Edit">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="SelectState" CommandArgument="<%# Container.DataItemIndex %>">
                                                      <i class="fa-solid fa-edit"></i> 
                                                        <!-- FontAwesome icon -->
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Edit">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="DeleteEx" CommandArgument="<%# Container.DataItemIndex %>">
<i class="fa-solid fa-xmark text-danger"></i> <!-- FontAwesome icon -->
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
                    </asp:Panel>
                </div>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnViewDep" />
            <asp:PostBackTrigger ControlID="btnAddDep" />
            <asp:AsyncPostBackTrigger ControlID="gvstate" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>


<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmAddUsersRoles.aspx.cs" Inherits="frmAddUsersRoles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
          
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="scriptmanager1" runat="server">
     
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>

            <div class="card mb-2">
                <div class="card-body">
                    <div class="row gx-2 gy-1">
                        <div class="col-md-1">
                            <label class="form-label">
                                Create Role
                        <asp:RequiredFieldValidator ID="RfvtxtRoleName" runat="server" ErrorMessage="*" ForeColor="Red" Font-Bold="true" ControlToValidate="txtRoleName" ValidationGroup="Role"></asp:RequiredFieldValidator>
                            </label>
                        </div>
                        <div class="col-md-5">
                            
                                <asp:TextBox ID="txtRoleName" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                          
                        </div>
                        <div class="col-md-1">
                                                            <asp:Button ID="btnSaveRole" runat="server" Text="Submit" CssClass="btn btn-sm btn-grd btn-grd-info " OnClick="btnSaveRole_Click" ValidationGroup="Role" />

                        </div>
                        <div class="col-md-5"></div>
                        <div class="col-md-1">
                            <label class="form-label">
                                Select Role 
                        <asp:RequiredFieldValidator ID="RfvddlUsers" runat="server" ErrorMessage="*" InitialValue="0" ForeColor="Red" Font-Bold="true" ControlToValidate="ddlUsers" ValidationGroup="RoleA"></asp:RequiredFieldValidator>
                            </label>
                        </div>
                        <div class="col-md-5">
                            <asp:DropDownList ID="ddlUsers" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="True" OnSelectedIndexChanged="ddlUsers_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                    </div>
                    </div>
                    </div>
             <div class="card">
     <div class="card-body">
                    <div class="row gy-3 gx-2  ">
                        <div class="col-md-6">
                            <div class="card shadow-none border  ">
                                <div class="card-body ">
                                    <div class="d-flex flex-lg-row flex-column align-items-start align-items-lg-center justify-content-between gap-3">
                                        <div class="flex-grow-1">
                                            <h6 class=" fw-bold">All Menu</h6>
                                        </div>
                                        <div class="overflow-auto">
<asp:LinkButton ID="btnMasterRoleApply" runat="server" OnClick="btnMasterRoleApply_Click"  ValidationGroup="RoleA" class="btn btn-sm btn-outline-secondary">
      <i class="fas fa-arrow-right"></i>
</asp:LinkButton>
                                        </div>
                                    </div>


                                    <div class="row mt-2">

                                        <div class="col-md-12">
                                            <div class="table-responsive table-container white-space-nowrap" style="height: 230px">
                                                <asp:GridView ID="gvMasterRoles" DataKeyNames="MenuID" AutoGenerateColumns="false" runat="server" CssClass="table table-head-fixed text-nowrap table-sm border">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <HeaderTemplate>
                                                                <asp:CheckBox ID="chkAll" runat="server" onclick="grdHeaderCheckBox(this);" />
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSelect" runat="server" />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="MenuName" DataField="MenuName" ItemStyle-Font-Size="Smaller" />
                                                        <asp:BoundField HeaderText="MenuID" DataField="MenuID" ItemStyle-Font-Size="Smaller" Visible="false" />
                                                        <asp:BoundField HeaderText="MenuStatus" DataField="MenuStatus" ItemStyle-Font-Size="Smaller" />
                                                       <asp:TemplateField HeaderText="Action">
    <ItemTemplate>
        <asp:HiddenField ID="hdnids" Value='<%#Eval("ID")%>' runat="server" />
        <asp:HiddenField ID="hdnMenuStatus" Value='<%#Eval("MenuStatus")%>' runat="server" />
        <asp:LinkButton runat="server" ID="lnkdeletemenu" OnClick="lnkdeletemenu_Click" 
                        OnClientClick="return confirm('Are you sure you want to Change the Status ?');" 
                        CssClass="center-btn">
            <i class="fa-solid fa-rotate-right"></i>
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
                            <div class="card shadow-none border  ">
                                <div class="card-body">
                                    <h6 class=" fw-bold">Roles</h6>
                                    <div class="row mt-4">
                                        <div class="col-md-12">
                                            <div class="table-responsive table-container" style="height: 240px">
                                                <asp:GridView ID="grdRoles" AutoGenerateColumns="false" CssClass="table table-head-fixed text-nowrap table-sm border" runat="server">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Menu Name">
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="hdnid" Value='<%#Eval("RoleID")%>' runat="server" />
                                                                <asp:HiddenField ID="hdnRoleName" Value='<%#Eval("RoleName")%>' runat="server" />
                                                                <asp:Label ID="lblRoleName" runat="server" Text='<%#Eval("RoleName")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Insert Date">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblInsertDt" runat="server" Text='<%#Eval("InsertDt")%>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Action">
                                                            <ItemTemplate>
                                                                <asp:LinkButton runat="server" ID="lnkdelete" OnClick="lnkdelete_Click" OnClientClick="return confirm('Are you sure you want to Delete this ?');" class="center-btn">
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
                        </div>
                        <div class="col-md-6">
                            <div class="card shadow-none border  ">
                                <div class="card-body">
                                    <h6 class=" fw-bold">
                                        <asp:Label ID="lblMenuList" runat="server"></asp:Label></h6>
                                    <div class="row mt-4    ">
                                        <div class="col-md-12">
                                            <div class="table-responsive table-container" style="height: 222px">
                                                <asp:GridView ID="gvAllRoles" DataKeyNames="ID" AutoGenerateColumns="false" CssClass="table table-head-fixed text-nowrap table-sm border" runat="server" OnRowCommand="gvAllRoles_RowCommand">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# Eval("MenuStatus").ToString().Equals("Active")  %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Menu Name" DataField="MenuName" ItemStyle-Font-Size="Smaller" />
                                                        <asp:BoundField HeaderText="User Name" DataField="UserName" ItemStyle-Font-Size="Smaller" />
                                                        <asp:BoundField HeaderText="Menu Status" DataField="MenuStatus" ItemStyle-Font-Size="Smaller" />
                                                        <asp:BoundField HeaderText="MenuID" DataField="MenuID" ItemStyle-Font-Size="Smaller" />
                                                        <asp:TemplateField HeaderText="Remove">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="imgdel" runat="server" CommandName="DeleterRole" CommandArgument='<%#Eval("ID") %>' ToolTip="Delete" CausesValidation="false" class="center-btn"><i class="fa-sharp fa-solid fa-xmark text-danger"> </i></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                            </div>
                                            <asp:Button ID="btnUpdateRoles" runat="server" Text="Apply" CssClass="btn btn-sm btn-success warning_3" OnClick="btnUpdateRoles_Click" Visible="false" />
                                        </div>
                                    </div>
                                </div>


                            </div>
                        </div>
                    </div>
                </div>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ddlUsers" />
            <asp:PostBackTrigger ControlID="btnMasterRoleApply" />
            <asp:PostBackTrigger ControlID="grdRoles" />
        </Triggers>
    </asp:UpdatePanel>
     <script type="text/javascript">
         function grdHeaderCheckBox(chkAll) {
             var checkboxes = document.querySelectorAll("#<%= gvMasterRoles.ClientID %> input[type='checkbox']");
             for (var i = 0; i < checkboxes.length; i++) {
                 if (checkboxes[i] !== chkAll) {
                     checkboxes[i].checked = chkAll.checked;
                 }
             }
         }
     </script>
</asp:Content>

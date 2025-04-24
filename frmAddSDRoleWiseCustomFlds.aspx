<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmAddSDRoleWiseCustomFlds.aspx.cs" Inherits="frmAddSDRoleWiseCustomFlds" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
   
            
            <div class="card">
                <div class="card-body">
                    
                    <div class="row">
                        <div class="col-md-2" hidden>
                            <label class="form-label">
                                Enter Role                                 <asp:RequiredFieldValidator ID="RfvtxtRoleName" runat="server" ErrorMessage="*" ForeColor="Red" Font-Bold="true" ControlToValidate="txtRoleName" ValidationGroup="Role"></asp:RequiredFieldValidator>
                            </label>
                            <asp:TextBox ID="txtRoleName" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                        </div>
                        <div class="col-md-2 mt-1" hidden>
                            <label class="form-label"></label>
                            <asp:Button ID="btnSaveRole" runat="server" Text="Submit" CssClass="btn btn-sm savebtn" OnClick="btnSaveRole_Click" Style="margin-top: 25px" ValidationGroup="Role" />
                        </div>
                        <div class="col-md-5">
                        
                                <label class="form-label">
                                    Select Role 
            <asp:RequiredFieldValidator ID="RfvddlUsers" runat="server" ErrorMessage="*" InitialValue="0" ForeColor="Red" Font-Bold="true" ControlToValidate="ddlUsers" ValidationGroup="RoleA"></asp:RequiredFieldValidator>
                                </label>
                                <asp:DropDownList ID="ddlUsers" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="True" OnSelectedIndexChanged="ddlUsers_SelectedIndexChanged">
                                    <asp:ListItem Selected="True" Text="----Select Role----" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Master" Value="Master"></asp:ListItem>
                                   <%-- <asp:ListItem Text="ITManager" Value="ITManager"></asp:ListItem>
                                    <asp:ListItem Text="CRM" Value="CRM"></asp:ListItem>
                                    <asp:ListItem Text="ITEngineer" Value="ITEngineer"></asp:ListItem>
                                    <asp:ListItem Text="UAT" Value="UAT"></asp:ListItem>--%>
                                </asp:DropDownList>
                            </div>
                       
                    </div>
                    <div class="row mt-2">
                        <div class="col-md-5">
                            <div class="card shadow-none border  ">

                                <div class="card-body">
                                    <div class="row">
                                        <div class="d-flex flex-lg-row flex-column align-items-start align-items-lg-center justify-content-between gap-3">
                 <div class="flex-grow-1">
                    <p class=" fw-bold">Menu List</p>
                 </div>
                 <div class="overflow-auto">
                                    <asp:Button ID="btnMasterRoleApply" runat="server" Text="Apply" OnClick="btnMasterRoleApply_Click" CssClass="btn btn-sm btn-outline-secondary" ValidationGroup="RoleA" />
                
                </div>
              </div>
                                        <div class="col-md-12 mt-1">  <div class="table-responsive table-container" style="height: 230px; ">
      <asp:GridView ID="gvMasterRoles" DataKeyNames="MenuID" AutoGenerateColumns="false" runat="server" CssClass="table table-head-fixed text-nowrap table-sm table-striped border">
          <Columns>
              <asp:TemplateField>
                  <ItemTemplate>
                      <asp:CheckBox ID="chkSelect" runat="server" />
                  </ItemTemplate>
              </asp:TemplateField>
              <asp:BoundField HeaderText="MenuName" DataField="MenuName" ItemStyle-Font-Size="Smaller" />
              <asp:BoundField HeaderText="Desk" DataField="Deskref" ItemStyle-Font-Size="Smaller" />
              <asp:BoundField HeaderText="MenuID" DataField="MenuID" ItemStyle-Font-Size="Smaller" />
          </Columns>
      </asp:GridView>
  </div></div>
                                    </div>

                                  
                                </div>
                            </div>
                        </div>

                        <div class="col-md-7 ">
                            <div class="card shadow-none border">

                                <div class="card-body">
                                    <div class="row">
                                       <p class=" fw-bold">Role Wise Menu List</p>


                                    </div>
                                   <div class="row">
                                       <div class="col-md-12">
                                    <div class="table-responsive table-container " style="height: 240px;">
                                        <asp:GridView ID="gvAllRoles" DataKeyNames="ID" AutoGenerateColumns="false" CssClass="table table-sm table-head-fixed text-nowrap table-striped border" runat="server" OnRowCommand="gvAllRoles_RowCommand">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSelect" runat="server" Checked='<%# Eval("MenuStatus").ToString().Equals("Active")  %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:BoundField HeaderText="Menu Name" DataField="MenuName" ItemStyle-Font-Size="Smaller" />

                                                <asp:BoundField HeaderText="Menu Status" DataField="MenuStatus" ItemStyle-Font-Size="Smaller" />
                                                <asp:BoundField HeaderText="MenuID" DataField="MenuID" ItemStyle-Font-Size="Smaller" />
                                                <asp:TemplateField HeaderText="Remove">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="imgdel" runat="server" ToolTip="Delete"  CommandName="DeleterRole" CommandArgument='<%#Eval("ID") %>'><i class="fa-solid fa-xmark text-danger"></i></asp:LinkButton>
                                                        <%--<asp:ImageButton ID="" runat="server" ImageUrl="~/Images/New folder/delnew.png" ToolTip="Delete" CausesValidation="false" />--%>
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
   
</asp:Content>


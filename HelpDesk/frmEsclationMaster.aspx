<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmEsclationMaster.aspx.cs" Inherits="frmEsclationMaster" %>
<%@ Register Src="~/HelpDesk/UserControl.ascx" TagName="UserControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
   <style>
      .dataTables_filter {
      margin-top: -29px !important;
      }
      .dt-buttons > .btn-outline-secondary{
      padding:0.25rem 0.5rem!important;
      font-size: 0.875rem!important;
      }
      .pagination{
      --bs-pagination-padding-x: 0.5rem;
      --bs-pagination-padding-y: 0.25rem;
      --bs-pagination-font-size: 0.875rem;
      --bs-pagination-border-radius: var(--bs-border-radius-sm);
      margin-top: -1.7rem!important;
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
                     <asp:Button ID="btnAddUserEcslevel" Text="Add Ecslevel" runat="server" CssClass="btn btn-sm  btn-outline-secondary" OnClick="btnAddUserEcslevel_Click" />
                     <asp:Button ID="btnViewEcslevel" runat="server" Text-="View Details" CssClass="btn btn-sm  btn-outline-secondary" OnClick="btnViewEcslevel_Click" />
                  </div>
                  </div>
               </div>
               <div class="card border bg-transparent shadow-none mb-3">
                  <div class="card-body">
                     <asp:Panel ID="pnlAddEcslevel" runat="server" Visible="false">
                        <div class=" row  gy-3 gx-2">
                           <div class="col-md-4  ">
                              <label for="staticEmail" class="form-label">
                                 Esclation Level
                                 <asp:RequiredFieldValidator ID="rfvddlEsclationLevel" runat="server" ControlToValidate="ddlEsclationLevel" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="UserEcslevel"></asp:RequiredFieldValidator>
                              </label>
                              <asp:DropDownList ID="ddlEsclationLevel" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field">
                                 <asp:ListItem Text="L1" Value="L1"></asp:ListItem>
                                 <asp:ListItem Text="L2" Value="L2"></asp:ListItem>
                                 <asp:ListItem Text="L3" Value="L3"></asp:ListItem>
                                 <asp:ListItem Text="L4" Value="L4"></asp:ListItem>
                                 <asp:ListItem Text="L5" Value="L5"></asp:ListItem>
                              </asp:DropDownList>
                           </div>
                           <div class="col-md-4  ">
                              <label for="staticEmail" class="form-label">
                                 User Name
                                 <asp:RequiredFieldValidator ID="rfvtxtUserName" runat="server" ControlToValidate="txtUserName" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="UserEcslevel"></asp:RequiredFieldValidator>
                              </label>
                              <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control  form-control-sm ">
                              </asp:TextBox>
                           </div>
                           <div class="col-md-4  ">
                              <label for="staticEmail" class="form-label">
                                 Email
                                 <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail"
                                    ForeColor="Red" ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
                                    Display="Dynamic" ErrorMessage="Invalid Email" ValidationGroup="UserEcslevel" />
                                 <asp:RequiredFieldValidator ID="rfvtxtEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="UserEcslevel"></asp:RequiredFieldValidator>
                              </label>
                              <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control  form-control-sm ">
                              </asp:TextBox>
                           </div>
                           <div class="col-md-4  ">
                              <label for="staticEmail" class="form-label">
                                 Mobile
                                 <asp:RegularExpressionValidator ID="rvPhoneNumber" runat="server"
                                    ControlToValidate="txtMobile"
                                    ValidationExpression="^(?:(?:\+?\d{1,3}[-.\s]?)?\(?(?:\d{3})?\)?[-.\s]?\d{3}[-.\s]?\d{4})|(?:(?:\+?\d{1,3}[-.\s]?)?\(?(?:\d{2,4})?\)?[-.\s]?\d{6,8})$"
                                    ErrorMessage="Invalid Number" ForeColor="Red"
                                    Display="Dynamic" ValidationGroup="UserEcslevel">
                                 </asp:RegularExpressionValidator>
                                 <asp:RequiredFieldValidator ID="rfvtxtMobile" runat="server" ControlToValidate="txtMobile" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="UserEcslevel"></asp:RequiredFieldValidator>
                              </label>
                              <asp:TextBox ID="txtMobile" TextMode="Phone" runat="server" CssClass="form-control  form-control-sm ">
                              </asp:TextBox>
                           </div>
                           <div class="col-md-4  ">
                              <label for="staticEmail" class="form-label">
                                 Esclation Time(in Min)
                                 <asp:RequiredFieldValidator ID="rfvtxttimeforEsclation" runat="server" ControlToValidate="txttimeforEsclation" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="UserEcslevel"></asp:RequiredFieldValidator>
                              </label>
                              <asp:TextBox ID="txttimeforEsclation" runat="server" TextMode="Number" CssClass="form-control  form-control-sm ">
                              </asp:TextBox>
                           </div>
                           <div class="col-md-4  ">
                              <label for="staticEmail" class="form-label">
                                 Organization
                                 <asp:RequiredFieldValidator ID="rfvddlOrg" runat="server" ControlToValidate="ddlOrg" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="ReqType"></asp:RequiredFieldValidator>
                              </label>
                              <asp:DropDownList ID="ddlOrg" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field">
                              </asp:DropDownList>
                           </div>
                           <div class="col-md-12 text-end">
                              <asp:Button ID="btnInsertEcslevel" runat="server" Text="Save" class="btn btn-sm btn-grd btn-grd-info " OnClick="btnInsertEcslevel_Click" ValidationGroup="UserEcslevel" />
                              <asp:Button ID="btnUpdateEcslevel" runat="server" Text="Update" Visible="false" class="btn btn-sm btn-grd btn-grd-info " OnClick="btnUpdateEcslevel_Click" ValidationGroup="UserEcslevel" />
                              <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="btn btn-sm btn-grd btn-grd-danger " OnClick="btnCancel_Click" CausesValidation="false" />
                           </div>
                     </asp:Panel>
                     <asp:Panel ID="pnlViewEcslevel" runat="server">
                     <div class="row ">
                     <div class="col-md-6">
                     <asp:Label ID="Label1" runat="server"></asp:Label>
                     <asp:Label ID="Label3" runat="server"></asp:Label>
                     </div>
                     <div class="col-md-2 d-none ">
                     <div class="btn btn-sm elevation-1 ml-1 " style="padding: 0px; margin-bottom: 10px; padding-top: 1px">
                     <label class="mr-2 ml-1 mb-0">Export</label>
                     <asp:ImageButton ID="ImgBtnExport" runat="server" ImageUrl="~/Images/New folder/excelnew.png" CssClass="fa-pull-right btn-outline-success mr-1" OnClick="ImgBtnExport_Click" />
                     </div>
                     </div>
                     <div class="col-md-12">
                     <div class="table-responsive table-container" style=width: 100%">
                     <asp:GridView GridLines="None" ID="gvEcslevel" runat="server" DataKeyNames="ID" AutoGenerateColumns="false" CssClass="data-table table table-striped table-sm text-nowrap border"
                        Width="100%" OnRowCommand="gvEcslevel_RowCommand" OnRowDataBound="gvEcslevel_RowDataBound">
                     <Columns>
                     <asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="20">
                     <ItemTemplate>
                     <%#Container.DataItemIndex+1 %>
                     </ItemTemplate>
                     </asp:TemplateField>
                     <asp:BoundField DataField="EsclationLevel" HeaderText="Escltion Level" NullDisplayText="NA" />
                     <asp:BoundField DataField="UserName" HeaderText="UserName" NullDisplayText="NA" />
                     <asp:BoundField DataField="UserEmail" HeaderText="User Email" NullDisplayText="NA" />
                     <asp:BoundField DataField="Mobile" HeaderText="Mobile" NullDisplayText="NA" />
                     <asp:BoundField DataField="TimeForEsclatn" HeaderText="Esclation Time" NullDisplayText="NA" />
                     <asp:TemplateField HeaderText=" Organization">
                     <ItemTemplate>
                     <asp:Label ID="lblOrgFk" runat="server" Text='<%# Eval("Org_ID") %>' Visible="false"></asp:Label>
                     <asp:Label ID="lblOrgName" runat="server" Text='<%# Eval("OrgName") %>'></asp:Label>
                     </ItemTemplate>
                     </asp:TemplateField>
                     <%--<asp:ButtonField ButtonType="Image" CommandName="UpdateEcslevel" HeaderText="Edit" ImageUrl="~/images/edit23.png" ItemStyle-Width="20px" />--%>
                     <asp:TemplateField HeaderText="Edit">
                     <ItemTemplate>
                     <asp:LinkButton ID="lnkEdit" runat="server" CommandName="UpdateEcslevel" CommandArgument="<%# Container.DataItemIndex %>">
                     <i class="fa-solid fa-edit"></i> <!-- FontAwesome icon -->
                     </asp:LinkButton>
                     </ItemTemplate>
                     </asp:TemplateField>
                     <asp:TemplateField HeaderText="Delete">
                     <ItemTemplate>
                     <asp:LinkButton ID="lnkDelete" runat="server" CommandName="DeleteEcslevel" CommandArgument="<%# Container.DataItemIndex %>" OnClientClick="return confirm('Are you sure you want to delete this item?');">
                     <i class="fa-solid fa-xmark text-danger"></i> <!-- FontAwesome delete icon -->
                     </asp:LinkButton>
                     </ItemTemplate>
                     </asp:TemplateField>
                     </Columns>
                     <%--<RowStyle BackColor="White" BorderColor="#e3e4e6" BorderWidth="1px" Height="10px" />
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
         <div class="modal " id="basicModal" <%-- tabindex="-1"--%> role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered">
               <div class="modal-content">
                  <div class="card-header" style="border-bottom: none">
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
         <asp:PostBackTrigger ControlID="btnAddUserEcslevel" />
         <asp:PostBackTrigger ControlID="btnViewEcslevel" />
         <%--<asp:PostBackTrigger ControlID="btnInsertEcslevel" />--%>
         <asp:PostBackTrigger ControlID="ImgBtnExport" />
         <asp:PostBackTrigger ControlID="gvEcslevel" />
         <%--<asp:PostBackTrigger ControlID="btnUpdateEcslevel" />--%>
      </Triggers>
   </asp:UpdatePanel>
</asp:Content>
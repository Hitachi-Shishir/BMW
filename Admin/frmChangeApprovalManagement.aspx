<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmChangeApprovalManagement.aspx.cs" Inherits="Admin_frmChangeApprovalManagement" %>

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
                        <div class="row">
                            <div class="col-md-12">
                                <div class="btn-group">
                                <asp:Button ID="btnAddUserEcslevel" Text="Add CAB Approval" runat="server" CssClass="btn btn-sm btn-outline-secondary" OnClick="btnAddUserEcslevel_Click" />
                                <asp:Button ID="btnViewEcslevel" runat="server" Text-="View Details" CssClass="btn btn-sm btn-outline-secondary" OnClick="btnViewEcslevel_Click" />
                            </div></div>
                        </div>
                       
                            <asp:Panel ID="pnlAddEcslevel" runat="server" Visible="false">
                                <div class="card border rounded mt-2">
                                    <div class="card-body">
                                <div class=" row gy-2 gx-3">
                                    <div class="col-md-4">
                                        <label for="staticEmail" class="form-label">
                                            Approval Level <span title="*"></span>
                                            <asp:RequiredFieldValidator ID="rfvddlCABLevel" runat="server" ControlToValidate="ddlCABLevel" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="UserEcslevel"></asp:RequiredFieldValidator>
                                        </label>


                                        <asp:DropDownList ID="ddlCABLevel" runat="server" CssClass="form-control  form-control-sm">
                                            <asp:ListItem Text="L1" Value="L1"></asp:ListItem>
                                            <asp:ListItem Text="L2" Value="L2"></asp:ListItem>
                                            <asp:ListItem Text="L3" Value="L3"></asp:ListItem>
                                            <asp:ListItem Text="L4" Value="L4"></asp:ListItem>
                                            <asp:ListItem Text="L5" Value="L5"></asp:ListItem>
                                        </asp:DropDownList>

                                    </div>
                                    <div class="col-md-4">
                                        <label for="staticEmail" class="form-label">
                                            User Name <span title="*"></span>
                                            <asp:RequiredFieldValidator ID="rfvtxtUserName" runat="server" ControlToValidate="txtUserName" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="UserEcslevel"></asp:RequiredFieldValidator>
                                        </label>

                                        <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control  form-control-sm ">
                                        </asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                        <label for="staticEmail" class="form-label">
                                            Email <span title="*"></span>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail"
                                                ForeColor="Red" ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
                                                Display="Dynamic" ErrorMessage="Invalid Email" ValidationGroup="UserEcslevel" />
                                            <asp:RequiredFieldValidator ID="rfvtxtEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="UserEcslevel"></asp:RequiredFieldValidator>
                                        </label>

                                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control  form-control-sm ">
                                        </asp:TextBox>
                                    </div>
                                    <div class="col-md-4">
                                        <label for="staticEmail" class="form-label">
                                            Mobile <span title="*"></span>
                                            <asp:RegularExpressionValidator ID="rvPhoneNumber" runat="server"
                                                ControlToValidate="txtMobile"
                                                ValidationExpression="^(?:(?:\+?\d{1,3}[-.\s]?)?\(?(?:\d{3})?\)?[-.\s]?\d{3}[-.\s]?\d{4})|(?:(?:\+?\d{1,3}[-.\s]?)?\(?(?:\d{2,4})?\)?[-.\s]?\d{6,8})$"
                                                ErrorMessage="Please enter 10-digit mobile no" ForeColor="Red"
                                                Display="Dynamic" ValidationGroup="UserEcslevel">
                                            </asp:RegularExpressionValidator>
                                            <asp:RequiredFieldValidator ID="rfvtxtMobile" runat="server" ControlToValidate="txtMobile" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="UserEcslevel"></asp:RequiredFieldValidator>
                                        </label>

                                     <asp:TextBox ID="txtMobile" runat="server" CssClass="form-control form-control-sm" MaxLength="10" onkeypress="return event.charCode >= 48 && event.charCode <= 57;">
</asp:TextBox>



                                    </div>
                                    <div class="col-md-4">
                                        <label for="staticEmail" class="form-label">
                                            Organization <span title="*"></span>
                                            <asp:RequiredFieldValidator ID="rfvddlOrg" runat="server" ControlToValidate="ddlOrg" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="ReqType"></asp:RequiredFieldValidator>
                                        </label>

                                        <asp:DropDownList ID="ddlOrg" runat="server" CssClass="form-control form-control-sm chzn-select">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-4">
                                        <label for="staticEmail" class="form-label">
                                            Active <span title="*"></span>
                                            <asp:RequiredFieldValidator ID="rfvddlIsActive" runat="server" ControlToValidate="ddlIsActive" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="ReqType"></asp:RequiredFieldValidator>
                                        </label>


                                        <asp:DropDownList ID="ddlIsActive" runat="server" CssClass="form-control form-control-sm chzn-select">
                                            <asp:ListItem Selected="True" Text="True" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="False" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-12 text-end">
                                        <asp:Button ID="btnInsertEcslevel" runat="server" Text="Save" class="btn btn-sm btn-grd-info" OnClick="btnInsertEcslevel_Click" ValidationGroup="UserEcslevel" />
                                        <asp:Button ID="btnUpdateEcslevel" runat="server" Text="Update" Visible="false" class="btn btn-sm btn-grd-info" OnClick="btnUpdateEcslevel_Click" ValidationGroup="UserEcslevel" />
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="btn btn-sm btn-grd-danger" OnClick="btnCancel_Click" CausesValidation="false" />
                                    </div>
                                </div>
</div></div>


                            </asp:Panel>
                            <asp:Panel ID="pnlViewEcslevel" runat="server">
                                  <div class="card border rounded mt-2">
      <div class="card-body">
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

                                </div>
                                <div class="table-responsive table-container" style="max-height: 350px; width: 100%">
                                    <asp:GridView GridLines="None" ID="gvEcslevel" runat="server" DataKeyNames="ID" AutoGenerateColumns="false"
                                        CssClass="data-table table table-striped border table-sm text-nowrap" Width="100%" OnRowCommand="gvEcslevel_RowCommand" OnRowDataBound="gvEcslevel_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="20">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="CABLevel" HeaderText="Approval Level" NullDisplayText="NA" />
                                            <asp:BoundField DataField="UserName" HeaderText="UserName" NullDisplayText="NA" />
                                            <asp:BoundField DataField="UserEmail" HeaderText="User Email" NullDisplayText="NA" />
                                            <asp:BoundField DataField="Mobile" HeaderText="Mobile" NullDisplayText="NA" />
                                            <asp:TemplateField HeaderText=" Organization">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOrgFk" runat="server" Text='<%# Eval("Org_ID") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblOrgName" runat="server" Text='<%# Eval("OrgName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Edit">
<ItemTemplate>
<asp:LinkButton ID="lnkEdit" runat="server" CommandName="UpdateEcslevel" CommandArgument="<%# Container.DataItemIndex %>">
<i class="fa-solid fa-edit"></i> <!-- FontAwesome icon -->
</asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>
                                            <%--<asp:ButtonField ButtonType="Image" CommandName="UpdateEcslevel" HeaderText="Edit" ImageUrl="~/images/edit23.png" ItemStyle-Width="20px" />--%>
                                            <asp:TemplateField HeaderText="Delete">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="btnDelete" runat="server"
                                                       
                                                        CommandName="DeleteEcslevel"
                                                        CommandArgument='<%# Container.DataItemIndex %>'
                                                        OnClientClick="return confirm('Are you sure you want to delete this record?');"
                                                        ToolTip="Delete" >
                                                        <i class="fa-solid fa-trash text-danger"></i>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle Width="20px" Height="5px" />
                                            </asp:TemplateField>

                                        </Columns>
                                    </asp:GridView>
                                </div>
                   
          </div>
          </div>
                    </asp:Panel>
               
           
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
            <asp:PostBackTrigger ControlID="btnInsertEcslevel" />
            <asp:PostBackTrigger ControlID="ImgBtnExport" />
            <asp:PostBackTrigger ControlID="gvEcslevel" />
            <asp:PostBackTrigger ControlID="btnUpdateEcslevel" />
        </Triggers>

    </asp:UpdatePanel>
</asp:Content>


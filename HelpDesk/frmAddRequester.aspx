<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmAddRequester.aspx.cs" Inherits="HelpDesk_frmAddRequester" %>

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
    <%--<asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>--%>

    <div class="card">

        <div class="card-body">
            <div class="row">
                <div class="col-12 mb-2">
                    <div class="btn-group">
                        <asp:Button ID="btnAddRequester" runat="server" Text="Add Requester" CausesValidation="false" OnClick="btnAddRequester_Click" CssClass="btn btn-sm btn-outline-secondary" />
                        <asp:Button ID="btnimportUser" runat="server" Text="Bulk Users" CausesValidation="false" OnClick="btnimportUser_Click" CssClass="btn btn-sm btn-outline-secondary" />
                        <asp:Button ID="btnViewUsers" runat="server" Text="View Users" CausesValidation="false" OnClick="btnViewUsers_Click" CssClass="btn btn-sm btn-outline-secondary" />
                        <%--<asp:Button ID="lddk" runat="server" Text="Bulk Users"  CssClass="btn btn-sm btn-outline-secondary" />--%>
                    </div>
                </div>
            </div>
            <div class="card border bg-transparent shadow-none mb-3">
                <div class="card-body p-0">
                    <asp:Panel ID="pnlAddRequester" Visible="false" runat="server">
                        <div class="card-header bg-light ">
                            <h6 class="card-title mb-0">New Requester</h6>
                        </div>
                        <div class="card-body">
                            <div class=" row gy-3 gx-2   ">
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Login Name 
                                   
                                        <asp:RequiredFieldValidator ID="rfvtxtTechLoginName" runat="server" ControlToValidate="txtTechLoginName" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Tech">
                                    </asp:RequiredFieldValidator>
                                    </label>
                                    <asp:TextBox ID="txtTechLoginName" class="form-control form-control-sm" runat="server" onkeypress="return /^[a-zA-Z0-9_.\-\s]*$/.test(event.key) && this.value.length < 30;" MaxLength="30" />
                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Employee ID 
                                   
                                        <asp:RequiredFieldValidator ID="rfvtxtEmployeeID" runat="server" ControlToValidate="txtEmployeeID" ErrorMessage="*" ForeColor="Red" ValidationGroup="SearchUser"></asp:RequiredFieldValidator>
                                    </label>
                                    <div class="input-group input-group-sm">
                                        <asp:TextBox ID="txtEmployeeID" runat="server" class="form-control form-control-sm" onkeypress="return /^[a-zA-Z0-9]*$/.test(event.key) && this.value.length < 50;" />
                                        <asp:LinkButton ID="btnSearch" runat="server" OnClick="btnSearch_Click" ValidationGroup="SearchUser" class="btn btn-sm btn-outline-secondary"><i class="fa-solid fa-magnifying-glass"></i></asp:LinkButton>
                                    </div>

                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        First Name 
                                   
                                        <asp:RequiredFieldValidator ID="rfvtxtFirstName" runat="server" ControlToValidate="txtFirstName" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Tech">
                                    </asp:RequiredFieldValidator>
                                    </label>
                                    <asp:TextBox ID="txtFirstName" runat="server" class="form-control form-control-sm" onkeypress="return /^[a-zA-Z-_ ]*$/.test(event.key) && this.value.length < 30;" MaxLength="30" />
                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Last Name 
                                           
                                    </label>
                                    <asp:TextBox ID="txtLastName" runat="server" class="form-control form-control-sm" />
                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Password                                     <a href="#" title="Minimum 8 and Maximum 10 characters 
at least 1 Uppercase Alphabet, 1 Lowercase Alphabet,
1 Number and 1 Special Character"
                                            alt=" 
                                       Minimum 8 and Maximum 10 characters 
                                       at least 1 Uppercase Alphabet, 1 Lowercase Alphabet,
                                       1 Number and 1 Special Character"
                                            class="tooltipcustom"><i class="fa fa-info-circle"></i></a>
                                        <asp:RegularExpressionValidator ID="regtxtPassword" runat="server" ControlToValidate="txtPassword"
                                            ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{8,}"
                                            ErrorMessage="Invalid Password" ForeColor="Red" Display="Dynamic" />
                                        <asp:RequiredFieldValidator ID="rfvtxtPassword" Display="Dynamic" runat="server" ControlToValidate="txtPassword" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Tech">
                                                </asp:RequiredFieldValidator>
                                    </label>
                                    <asp:TextBox ID="txtPassword" autocomplete="new-password" runat="server" class="form-control form-control-sm" TextMode="Password" />
                                    <div>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Confirm Password  <a href="#" title="
    Minimum 8 and Maximum 10 characters 
    at least 1 Uppercase Alphabet, 1 Lowercase Alphabet,
    1 Number and 1 Special Character"
                                            alt=" 
    Minimum 8 and Maximum 10 characters 
    at least 1 Uppercase Alphabet, 1 Lowercase Alphabet,
    1 Number and 1 Special Character"
                                            class="tooltipcustom"><i class="fa fa-info-circle  "></i></a>
                                        <asp:RegularExpressionValidator ID="regftxtConfirmPassword" runat="server" ControlToValidate="txtConfirmPassword"
                                            ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{8,}" Display="Dynamic"
                                            ErrorMessage="Invalid Password" ForeColor="Red" />
                                    </label>
                                    <asp:TextBox ID="txtConfirmPassword" autocomplete="new-password" runat="server" class="form-control form-control-sm" TextMode="Password">
                                            </asp:TextBox>

                                </div>
                            </div>
                        </div>
                        <div class="card-header bg-light border-top">
                            <h6 class="card-title mb-0">Contact Information</h6>
                        </div>
                        <div class="card-body">
                            <div class=" row gy-3 gx-2">
                                <div class="col-md-6">
                                    <label for="staticEmail" class="form-label">
                                        Email 
                                   
                                        <asp:RegularExpressionValidator ID="regEmail" runat="server" ControlToValidate="txtEmail"
                                            ForeColor="Red" ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
                                            Display="Dynamic" ErrorMessage="Invalid Email " />
                                        <asp:RequiredFieldValidator ID="rfvtxtEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Tech">
                                                </asp:RequiredFieldValidator>
                                    </label>
                                    <asp:TextBox ID="txtEmail" class="form-control form-control-sm" runat="server" TextMode="Email" />
                                </div>
                                <div class="col-md-6">
                                    <label for="staticEmail" class="form-label">
                                        Phone 
                                   
                                        <asp:RegularExpressionValidator ID="regPhone" runat="server"
                                            ControlToValidate="txtPhone" ErrorMessage="Invalid Mobile No" Display="Dynamic" ForeColor="Red"
                                            ValidationExpression="[0-9]{10}"></asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="rfvtxtPhone" runat="server" ControlToValidate="txtPhone" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Tech">
                                                </asp:RequiredFieldValidator>
                                    </label>
                                    <asp:TextBox ID="txtPhone" class="form-control form-control-sm" runat="server" TextMode="Phone" MaxLength="10" onkeypress="return /^[0-9]*$/.test(event.key) && this.value.length < 12;" />
                                </div>
                            </div>
                        </div>
                        <div class="card-header bg-light border-top">
                            <h6 class="card-title mb-0">Department Details</h6>
                        </div>
                        <div class="card-body">
                            <div class=" row gy-3 gx-2">
                                <div class="col-md-6">
                                    <label for="staticEmail" class="form-label">
                                        Organization
                                   
                                        <asp:RequiredFieldValidator ID="rfvddlOrg" runat="server" ControlToValidate="ddlOrg" InitialValue="0" ErrorMessage="Required" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </label>
                                    <asp:DropDownList ID="ddlOrg" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlOrg_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-6">
                                    <label for="staticEmail" class="form-label">
                                        Location
                                   
                                        <asp:RequiredFieldValidator ID="rfvddlLocation" runat="server" ControlToValidate="ddlLocation" ValidationGroup="Addticket" ForeColor="Red" ErrorMessage="*"></asp:RequiredFieldValidator>
                                    </label>
                                    <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field"></asp:DropDownList>
                                </div>
                                <div class="col-md-6">
                                    <label for="staticEmail" class="form-label">
                                        Department 
                                   
                                        <asp:RequiredFieldValidator ID="rfvddlDepartment" runat="server" ControlToValidate="ddlDepartment" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Tech">
                                    </asp:RequiredFieldValidator>
                                    </label>
                                    <asp:DropDownList ID="ddlDepartment" class="form-select form-select-sm single-select-optgroup-field	" runat="server" />
                                </div>
                                <div class="col-md-6">
                                    <label for="staticEmail" class="form-label">
                                        Job Title 
                                   
                                        <asp:RequiredFieldValidator ID="rfvtxtJobTitle" runat="server" ControlToValidate="txtJobTitle" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Tech">
                                    </asp:RequiredFieldValidator>
                                    </label>
                                    <asp:TextBox ID="txtJobTitle" runat="server" class="form-control form-control-sm" onkeypress="return /^[a-zA-Z-_ ]*$/.test(event.key) && this.value.length < 50;" />
                                </div>
                                <div class="col-md-6">
                                    <div class="form-check">
                                        <asp:CheckBox ID="chkEnableTechnician" runat="server" AutoPostBack="true" OnCheckedChanged="chkEnableTechnician_CheckedChanged" />
                                        <label class="form-check-label" for="staticEmail">
                                            Mark This User As Technician
                                               
                                        </label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <asp:Panel ID="pnlEnableTechnician" runat="server" Visible="false">
                            <div class="card-header bg-light border-top">
                                <h6 class="card-title mb-0">Category Association</h6>
                            </div>
                            <div class="card-body">
                                <div class=" row mt-3">
                                    <label for="staticEmail" class="form-label">
                                        Request Type:
                                   
                                        <asp:RequiredFieldValidator ID="rfvddlRequestType" runat="server" ControlToValidate="ddlRequestType" InitialValue="0" ErrorMessage="Required" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </label>
                                    <asp:DropDownList ID="ddlRequestType" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlRequestType_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                <label for="staticEmail" class="form-label">
                                    Category:
                                
                                    <asp:RequiredFieldValidator ID="rfvddlCategory1" runat="server" ControlToValidate="ddlCategory1" InitialValue="0" ErrorMessage="Required" Font-Bold="true" ForeColor="Red" ValidationGroup="Tech"></asp:RequiredFieldValidator>
                                </label>
                                <asp:DropDownList ID="ddlCategory1" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field">
                                </asp:DropDownList>
                            </div>
                        </asp:Panel>
                        <div class="card-header bg-light border-top">
                            <h6 class="card-title mb-0">Role & Scope</h6>
                        </div>
                        <div class="card-body">
                            <div class=" row gx-2 gy-3 ">
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Menu Role
                                   
                                        <asp:RequiredFieldValidator ID="rfvddlUserRole" runat="server" ControlToValidate="ddlUserRole" InitialValue="0" ErrorMessage="Required" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </label>
                                    <asp:DropDownList ID="ddlUserRole" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        User Scope
                                   
                                        <asp:RequiredFieldValidator ID="rfvddlUserScope" runat="server" ControlToValidate="ddlUserScope" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Tech"></asp:RequiredFieldValidator>
                                    </label>
                                    <asp:DropDownList ID="ddlUserScope" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Custom Field Role
                                   
                                        <asp:RequiredFieldValidator ID="rfvddlSDRole" runat="server" ControlToValidate="ddlSDRole" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Tech"></asp:RequiredFieldValidator>
                                    </label>
                                    <asp:DropDownList ID="ddlSDRole" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field">
                                        <asp:ListItem Selected="True" Text="----Select Role----" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Master" Value="Master"></asp:ListItem>
                                        <asp:ListItem Text="ITManager" Value="ITManager"></asp:ListItem>
                                        <asp:ListItem Text="CRM" Value="CRM"></asp:ListItem>
                                        <asp:ListItem Text="ITEngineer" Value="ITEngineer"></asp:ListItem>
                                        <asp:ListItem Text="UAT" Value="UAT"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Stauts
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlStatus" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Tech"></asp:RequiredFieldValidator>
                                    </label>
                                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field">
                                        <asp:ListItem Selected="True" Text="----Select Role----" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="InActive" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-4">
                                    <label for="staticEmail" class="form-label">
                                        Type
         <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ddlDomainType" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Tech"></asp:RequiredFieldValidator>
                                    </label>
                                    <asp:DropDownList ID="ddlDomainType" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field">
                                        <asp:ListItem Selected="True" Text="----Select Role----" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Domain" Value="Domain"></asp:ListItem>
                                        <asp:ListItem Text="Non-Domain" Value="Non-Domain"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                    <ContentTemplate>
                                        <div class="col-md-12 text-end mb-2">
                                            <asp:Button ID="btnInsertTechnician" runat="server" Text="Save" OnClick="btnInsertTechnician_Click" class="btn btn-sm btn-grd btn-grd-info   "
                                                ValidationGroup="Tech"></asp:Button>
                                            <asp:Button ID="btnUpdateTechnician" runat="server" Text="Update" Visible="false" class="btn btn-sm btn-grd btn-grd-info "
                                                OnClick="btnUpdateTechnician_Click" ValidationGroup="Tech" />
                                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="btn btn-sm btn-grd btn-grd-danger"
                                                OnClick="btnCancel_Click" CausesValidation="false" />
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                        <%--  --%>
                    </asp:Panel>
                    <asp:Panel ID="pnlShowUsers" runat="server">
                        <div class="class-header">
                        </div>
                        <div class="row ">

                            <div class="col-md-2  offset-6 d-none">
                                <div class="btn btn-sm elevation-1 ml-5 " style="padding: 0px; margin-bottom: 10px; padding-top: 1px">
                                    <label class="mr-2 ml-1 mb-0">Export</label>
                                    <asp:ImageButton ID="ImgBtnExport" runat="server" ImageUrl="~/Images/New folder/excelnew.png" CssClass="fa-pull-right btn-outline-success mr-1" OnClick="ImgBtnExport_Click" />
                                </div>
                            </div>
                        </div>
                        <div class="row px-3 my-2">
                            <div class="col-md-12">
                                <div class="table-responsive table-container">
                                    <asp:GridView GridLines="None" ID="gvTechnician" runat="server" DataKeyNames="UserID" AutoGenerateColumns="false" CssClass="data-table table table-striped border table-sm text-nowrap"
                                        Width="100%" OnRowCommand="gvTechnician_RowCommand" OnRowDataBound="gvTechnician_RowDataBound">
                                        <Columns>
                                            <%--<asp:ButtonField ButtonType="Image" CommandName="SelectTech" HeaderText="Edit" ImageUrl="~/images/edit23.png" ItemStyle-Width="20px" />--%>
                                            <asp:TemplateField HeaderText="Edit">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="SelectTech" Text="" CssClass="fa-solid fa-edit" CommandArgument="<%# Container.DataItemIndex %>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <%--                                                    <asp:ButtonField HeaderText="Delete" ButtonType="Image" ImageUrl="~/Images/New folder/delnew.png" CommandName="DeleteEx" ItemStyle-Width="20px" ItemStyle-Height="5px" />--%>
                                            <asp:TemplateField HeaderText="Delete">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="DeleteEx" CssClass="delete-link-button" CommandArgument="<%# Container.DataItemIndex %>">
                                                               <i class="fa-solid fa-xmark text-danger"></i>
                                                            </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="20">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="EmpID" HeaderText="EmpID" NullDisplayText="NA" />
                                            <asp:BoundField DataField="FirstName" HeaderText="FirstName" NullDisplayText="NA" />
                                            <asp:BoundField DataField="LastName" HeaderText="LastName" NullDisplayText="NA" />
                                            <asp:BoundField DataField="LoginName" HeaderText="LoginName" NullDisplayText="NA" />
                                            <asp:BoundField DataField="EmailID" HeaderText="EmailID" NullDisplayText="NA" />
                                            <asp:TemplateField HeaderText="User Scope">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblScopeID" runat="server" Text='<%# Eval("UserScopeID") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblScopeName" runat="server" Text='<%# Eval("UserScope") %>' Visible="true"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="UserRole" HeaderText="UserRole" NullDisplayText="NA" />
                                            <asp:BoundField DataField="Designation" HeaderText="JobTitle" NullDisplayText="NA" />
                                            <asp:TemplateField HeaderText="Department">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDepCode" runat="server" Text='<%# Eval("DepCode") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblDepName" runat="server" Text='<%# Eval("DepartmentName") %>' Visible="true"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ContactNo" HeaderText="Mobile" NullDisplayText="NA" />
                                            <%--<asp:TemplateField HeaderText=" Category Association">
              <ItemTemplate>
                 	<asp:Label ID="lblDesk" runat="server" Text='<%# Eval("DeskRef") %>' Visible="false"></asp:Label>
                 	<asp:Label ID="lblCategoryID" runat="server" Text='<%# Eval("CategoryAssociation") %>' Visible="false"></asp:Label>
                 	<asp:Label ID="lblCategoryName" runat="server" Text='<%# Eval("CategoryCodeRef") %>'></asp:Label>
              </ItemTemplate>
              </asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText=" Organization">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblOrgFk" runat="server" Text='<%# Eval("Org_ID") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblOrgName" runat="server" Text='<%# Eval("OrgName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Location">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblLocCode" runat="server" Text='<%# Eval("LocCode") %>' Visible="false"></asp:Label>
                                                    <asp:Label ID="lblLocName" runat="server" Text='<%# Eval("LocName") %>' Visible="true"></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="SDRole" HeaderText="CustomFieldRole" NullDisplayText="NA" />
                                            <asp:BoundField DataField="Status" HeaderText="Status" NullDisplayText="NA" />
                                            <asp:BoundField DataField="DomainType" HeaderText="Domain Type" NullDisplayText="NA" />
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
                    <asp:Panel ID="pnlImportUser" runat="server" Visible="false">
                        <div class="row p-3 gx-2 gy-3">
                            <div class="col-md-6">
                                <label class="form-label">
                                    Select Users(s) 
                                       
                                </label>

                                <%--<div class="custom-file">--%>
                                <asp:FileUpload ID="customFile" CssClass="form-control form-control-sm " runat="server" />
                                <%--<label class="custom-file-label" for="customFile">Choose File</label>--%>
                                <asp:Label ID="lblfilename" runat="server" class="form-label" Text=""></asp:Label>
                                <%--</div>--%>
                            </div>
                            <div class="col-md-6"></div>
                            <div class="col-md-3">
                                <a href="SampleFiles/UserMaster.xlsx" target="_blank" download="Insert Format" class="btn btn-sm btn-outline-secondary">Download Sample Excel  <i class="fa-solid fa-file-arrow-down"></i></a>
                            </div>
                            <div class="col-md-6">
                                <%--<asp:Button ID="" runat="server" Text="Import Excel" CssClass="btn btn-sm btn-outline-success "/>--%>
                                <asp:LinkButton ID="butttonsubmit" runat="server" OnClick="butttonsubmit_Click" CausesValidation="false" CssClass="btn btn-sm btn-outline-success ">Import Excel <i class="fa-solid fa-file-import"></i>     </asp:LinkButton>
                                <asp:LinkButton ID="btnn" OnClick="btnn_Click" CssClass="btn btn-sm btn-outline-success" Visible="false" runat="server" CausesValidation="false">Insert Data <i class="fa-solid fa-plus"></i></asp:LinkButton>
                            </div>
                        </div>

                        <div class="row" style="display: none;">
                            <asp:Label ID="Label2" runat="server" Text="Has Header ?"></asp:Label>
                            <br />
                            <asp:RadioButtonList ID="rbHDR" runat="server" RepeatLayout="Flow">
                                <asp:ListItem Text="Yes" Value="Yes" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="No" Value="No"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="table-responsive p-0" style="height: 400px; width: 100%">
                                    <asp:GridView ID="gvExcelFile" runat="server" CssClass="table table-head-fixed text-nowrap" AutoGenerateColumns="true">
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
    <%-- </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ImgBtnExport" />
            <asp:PostBackTrigger ControlID="btnn" />
            <asp:PostBackTrigger ControlID="butttonsubmit" />
            <asp:PostBackTrigger ControlID="gvTechnician" />
            <asp:PostBackTrigger ControlID="btnSearch" />
            <asp:PostBackTrigger ControlID="chkEnableTechnician" />
            <asp:PostBackTrigger ControlID="btnAddRequester" />
            <asp:PostBackTrigger ControlID="ddlRequestType" />
            <asp:PostBackTrigger ControlID="btnViewUsers" />
            <asp:PostBackTrigger ControlID="ddlOrg" />
        </Triggers>
    </asp:UpdatePanel>--%>
    <script type="text/javascript">
        function Showalert(imtype, imtitle) {
            // alert('Call JavaScript function from codebehind');
            // var typeof=type
            // var titleof = title;
            var Toast = Swal.mixin({
                toast: true,
                position: 'top-middle',

                showConfirmButton: false,
                showCloseButton: true,
                timer: 4000,


            });
            console.log("hello");
            Toast.fire({
                /*icon: 'success',*/
                //type: 'success',
                //title: 'Lorem ipsum dolor sit amet, consetetur sadipscing elitr.'
                icon: imtype,
                title: imtitle
            });

            console.log("fire1234567");
        }
    </script>
    <script>
        $(function () {
            bsCustomFileInput.init();
        });
    </script>
    <script>
        function displayfilename()
        $('#<%= customFile.ClientID %>').change(function (e) {
            var fileName = e.target.files[0].name;
            alert('The file "' + fileName + '" has been selected.');
        });
    </script>
</asp:Content>

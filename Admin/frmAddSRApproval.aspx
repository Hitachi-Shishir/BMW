<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmAddSRApproval.aspx.cs" Inherits="Admin_frmAddSRApproval" %>

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
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <div class="card">
                <div class="card-body">
                         
     <div class="row mb-2">
         <div class="col-md-12 ">
             <div class="btn-group">
             <asp:Button ID="btnAddApproval" runat="server" Text="Add Approval" CssClass="btn btn-sm btn-outline-secondary" CausesValidation="false" OnClick="btnAddApproval_Click" />
             <asp:Button ID="btnimportUser" runat="server" Text="Import Approvals"  CssClass="btn btn-sm btn-outline-secondary" CausesValidation="false" OnClick="btnimportUser_Click" />
             <asp:Button ID="btnViewUsers" runat="server" Text="View Details"  CssClass="btn btn-sm btn-outline-secondary" CausesValidation="false" OnClick="btnViewUsers_Click" />
         </div>
         </div>
     </div>
     <asp:Panel ID="pnlAddHoliday" Visible="false" runat="server">

       
                 <div class="card rounded border">
                     <div class="card-body">
                        
                         <div class="row gy-2 gx-3">
                             <div class="col-md-4">
                                 <label for="staticEmail" class="form-label">
                                     Organization
          <asp:RequiredFieldValidator ID="rfvddlOrg" runat="server" ControlToValidate="ddlOrg" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="ReqType"></asp:RequiredFieldValidator>
                                 </label>
                                 <asp:DropDownList ID="ddlOrg" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlOrg_SelectedIndexChanged">
                                 </asp:DropDownList>
                             </div>
                             <div class="col-md-4  ">
                                 <label for="staticEmail" class=" form-label ">
                                     Select Request Type<span class="red">*</span>
                                     <asp:RequiredFieldValidator ID="rfvddlRequestType" runat="server" InitialValue="0" ControlToValidate="ddlRequestType" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Tech">
                                     </asp:RequiredFieldValidator>
                                 </label>
                                 <asp:DropDownList ID="ddlRequestType" class="form-select form-select-sm single-select-optgroup-field" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlRequestType_SelectedIndexChanged"></asp:DropDownList>
                             </div>
                             <div class="col-md-4  ">
                                 <label for="staticEmail" class=" form-label ">
                                     Select User<span class="red">*</span>
                                     <asp:RequiredFieldValidator ID="rfvddlUser" runat="server" InitialValue="0" ControlToValidate="ddlUser" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Tech">
                                     </asp:RequiredFieldValidator>
                                 </label>
                                 <asp:DropDownList ID="ddlUser" class="form-select form-select-sm single-select-optgroup-field" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlUser_SelectedIndexChanged"></asp:DropDownList>
                             </div>
                             <div class="col-md-4  ">
                                 <label for="staticEmail" class=" form-label ">
                                     Employee ID<span class="red">*</span>
                                     <asp:RequiredFieldValidator ID="rfvtxtEmpID" runat="server" ControlToValidate="txtEmpID" ErrorMessage="*" ForeColor="Red" ValidationGroup="SearchUser"></asp:RequiredFieldValidator>
                                 </label>
                                 <asp:TextBox ID="txtEmpID" ClientIDMode="Static" ReadOnly="true" class="form-control form-control-sm" runat="server" />
                             </div>

                             <div class="col-md-4  ">
                                 <label for="staticEmail" class=" form-label ">
                                     Login Name<span class="red">*</span>
                                     <asp:RequiredFieldValidator ID="rfvtxtLoginName" runat="server" ControlToValidate="txtLoginName" ErrorMessage="*" ForeColor="Red" ValidationGroup="SearchUser"></asp:RequiredFieldValidator>
                                 </label>
                                 <asp:TextBox ID="txtLoginName" ReadOnly="true" ClientIDMode="Static" class="form-control form-control-sm" runat="server" />
                             </div>
<div class="col-md-4"></div>
                        
                             <div class="col-md-3  ">
                                 <label for="staticEmail" class=" form-label ">
                                     Approver 1 Name<span class="red">*</span>
                                     <asp:RequiredFieldValidator ID="rfvtxtFirstName" runat="server" ControlToValidate="txtApprover1Name" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Tech">
                                     </asp:RequiredFieldValidator>
                                 </label>
                                 <asp:TextBox ID="txtApprover1Name" runat="server" class="form-control form-control-sm" />
                             </div>
                             <div class="col-md-3  ">
                                 <label for="staticEmail" class="form-label">
                                     Approver 1  Email<span class="red">*</span>
                                 </label>
                                 <asp:TextBox ID="txtApprover1Email" runat="server" class="form-control form-control-sm" />
                             </div>

                             <div class="col-md-3  ">
                                 <label for="staticEmail" class="form-label">
                                     Active <span class="red">*</span>
                                 </label>
                                 <asp:DropDownList ID="ddlApprover1Active" runat="server" class="form-select form-select-sm single-select-optgroup-field">
                                     <asp:ListItem Text="True" Value="True"></asp:ListItem>
                                     <asp:ListItem Text="False" Value="False"></asp:ListItem>
                                 </asp:DropDownList>
                             </div>
                             <div class="col-md-3  ">
                                 <label for="staticEmail" class="form-label">
                                     Approver 1 Level<span class="red">*</span>
                                 </label>
                                 <asp:TextBox ID="txtApprovallbl1" runat="server" CssClass="form-control form-control-sm" Text="L1" ReadOnly="true"></asp:TextBox>
                                 <%--  <asp:DropDownList ID="ddlApproval1Level" runat="server" class="form-select form-select-sm single-select-optgroup-field">
                                 </asp:DropDownList>--%>
                             </div>
                       
                             <div class="col-md-3  ">
                                 <label for="staticEmail" class=" form-label ">
                                     Approver 2 Name
                                     
                                 </label>
                                 <asp:TextBox ID="txtApprover2Name" runat="server" class="form-control form-control-sm" />
                             </div>

                             <div class="col-md-3  ">
                                 <label for="staticEmail" class="form-label">
                                     Approver 2  Email
                                    
                                 </label>
                                 <asp:TextBox ID="txtApprover2Email" runat="server" class="form-control form-control-sm" />
                             </div>
                             <div class="col-md-3  ">
                                 <label for="staticEmail" class="form-label">
                                     Active
                                 </label>
                                 <asp:DropDownList ID="ddlApprover2Active" runat="server" class="form-select form-select-sm single-select-optgroup-field">
                                     <asp:ListItem Text="True" Value="True"></asp:ListItem>
                                     <asp:ListItem Text="False" Value="False"></asp:ListItem>
                                 </asp:DropDownList>
                             </div>
                             <div class="col-md-3  ">
                                 <label for="staticEmail" class="form-label">
                                     Approver 2 Level
                                 </label>
                                 <%-- <asp:DropDownList ID="ddlApproval2Level" runat="server" class="form-select form-select-sm single-select-optgroup-field">
                                 </asp:DropDownList>--%>
                                 <asp:TextBox ID="txtApprovallbl2" runat="server" CssClass="form-control form-control-sm" Text="L2" ReadOnly="true"></asp:TextBox>
                             </div>
                        
                             <div class="col-md-3  ">
                                 <label for="staticEmail" class=" form-label ">
                                     Approver 3 Name
                                     
                                 </label>

                                 <asp:TextBox ID="txtApprover3Name" runat="server" class="form-control form-control-sm" />
                             </div>
                             <div class="col-md-3  ">
                                 <label for="staticEmail" class="form-label">
                                     Approver 3 Email
                                   

                                 </label>

                                 <asp:TextBox ID="txtApprover3Email" runat="server" class="form-control form-control-sm" />
                             </div>
                             <div class="col-md-3  ">
                                 <label for="staticEmail" class="form-label">
                                     Active
                                 </label>
                                 <asp:DropDownList ID="ddlApprover3Active" runat="server" class="form-select form-select-sm single-select-optgroup-field">
                                     <asp:ListItem Text="True" Value="True"></asp:ListItem>
                                     <asp:ListItem Text="False" Value="False"></asp:ListItem>
                                 </asp:DropDownList>
                             </div>
                             <div class="col-md-3  ">
                                 <label for="staticEmail" class="form-label">
                                     Approver 3 Level
                                 </label>
                                 <%-- <asp:DropDownList ID="ddlApproval3Level" runat="server" class="form-select form-select-sm single-select-optgroup-field">
                                 </asp:DropDownList>--%>
                                 <asp:TextBox ID="txtApprovallbl3" runat="server" CssClass="form-control form-control-sm" Text="L3" ReadOnly="true"></asp:TextBox>
                             </div>
                        
                             <div class="col-md-3  ">
                                 <label for="staticEmail" class=" form-label ">
                                     Approver 4 Name
                                    
                                 </label>
                                 <asp:TextBox ID="txtApprover4Name" runat="server" class="form-control form-control-sm" />
                             </div>
                             <div class="col-md-3  ">
                                 <label for="staticEmail" class="form-label">
                                     Approver 4  Email
                                  
                                 </label>
                                 <asp:TextBox ID="txtApprover4Email" runat="server" class="form-control form-control-sm" />
                             </div>
                             <div class="col-md-3  ">
                                 <label for="staticEmail" class="form-label">
                                     Active
                                 </label>
                                 <asp:DropDownList ID="ddlApprover4Active" runat="server" class="form-select form-select-sm single-select-optgroup-field">
                                     <asp:ListItem Text="True" Value="True"></asp:ListItem>
                                     <asp:ListItem Text="False" Value="False"></asp:ListItem>
                                 </asp:DropDownList>
                             </div>

                             <div class="col-md-3  ">
                                 <label for="staticEmail" class="form-label">
                                     Approver 4 Level
                                 </label>
                                 <%--<asp:DropDownList ID="ddlApproval4Level" runat="server" class="form-select form-select-sm single-select-optgroup-field">
                                 </asp:DropDownList>--%>
                                 <asp:TextBox ID="txtApprovallbl4" runat="server" CssClass="form-control form-control-sm" Text="L4" ReadOnly="true"></asp:TextBox>

                             </div>
                        
                             <div class="col-md-3  ">
                                 <label for="staticEmail" class=" form-label ">
                                     Approver 5 Name
                                    
                                 </label>
                                 <asp:TextBox ID="txtApprover5Name" runat="server" class="form-control form-control-sm" />
                             </div>
                             <div class="col-md-3  ">
                                 <label for="staticEmail" class="form-label">
                                     Approver 5 Email
                                    
                                 </label>
                                 <asp:TextBox ID="txtApprover5Email" runat="server" class="form-control form-control-sm" />
                             </div>
                             <div class="col-md-3  ">
                                 <label for="staticEmail" class="form-label">
                                     Active
                                 </label>

                                 <asp:DropDownList ID="ddlApprover5Active" runat="server" class="form-select form-select-sm single-select-optgroup-field">
                                     <asp:ListItem Text="True" Value="True"></asp:ListItem>
                                     <asp:ListItem Text="False" Value="False"></asp:ListItem>
                                 </asp:DropDownList>
                             </div>
                             <div class="col-md-3  ">
                                 <label for="staticEmail" class="form-label">
                                     Approver 5 Level
                                 </label>
                                 <%--<asp:DropDownList ID="ddlApproval5Level" runat="server" class="form-select form-select-sm single-select-optgroup-field">
                                 </asp:DropDownList>--%>
                                 <asp:TextBox ID="txtApprovallbl5" runat="server" CssClass="form-control form-control-sm" Text="L5" ReadOnly="true"></asp:TextBox>
                             </div>
                             <div class="col-md-12 text-end">
                                 <asp:Button ID="btnInsert" runat="server" Text="Save" OnClick="btnInsert_Click" class="btn btn-sm btn-grd-info" ValidationGroup="Tech"></asp:Button>
<asp:Button ID="btnUpdate" runat="server" Text="Update" Visible="false" class="btn btn-sm btn-grd-info" OnClick="btnUpdate_Click" ValidationGroup="ReqType" />
<asp:Button ID="btnCancel" runat="server" Text="Cancel" class="btn btn-sm  btn-grd-danger" OnClick="btnCancel_Click" CausesValidation="false" />
                             </div>
                         </div>
                        
                     </div>
                 </div>
            
     </asp:Panel>
     <asp:Panel ID="pnlShowUsers" runat="server">

 
                         <div class="card border rounded">
                             <div class="card-body">
                                 <div class="row ">
                                     
                                     <div class="col-md-6">
                                         <asp:Label ID="Label1" runat="server"></asp:Label>
                                         <asp:Label ID="Label3" runat="server"></asp:Label>
                                     </div>
                                     <%--<div class="col-md-2 ">
                                         <div class="btn btn-sm elevation-1 ml-1 " style="padding: 0px; margin-bottom: 10px; padding-top: 1px">
                                             <label class="mr-2 ml-1 mb-0">Export</label>
                                             <asp:ImageButton ID="ImgBtnExport" runat="server" ImageUrl="~/Images/New folder/excelnew.png" CssClass="fa-pull-right btn-outline-success mr-1" OnClick="ImgBtnExport_Click" />
                                         </div>
                                     </div>--%>
                                 </div>
                                 <div class="table-responsive  table-container">
                                     <asp:GridView GridLines="None" ID="gvUserWiseApproval" runat="server" DataKeyNames="ID,ReqType,OrgId" AutoGenerateColumns="false" CssClass="data-table table table-striped border table-sm text-nowrap"
                                         Width="100%" OnRowCommand="gvUserWiseApproval_RowCommand" OnRowDataBound="gvUserWiseApproval_RowDataBound">
                                         <Columns>
                                             <asp:TemplateField HeaderText="Edit">
<ItemTemplate>
<asp:LinkButton ID="lnkEdit" runat="server" CommandName="SelectTech" CommandArgument="<%# Container.DataItemIndex %>" CssClass="center-btn">
<i class="fa-solid fa-edit"></i> <!-- FontAwesome icon -->
</asp:LinkButton>
</ItemTemplate>
</asp:TemplateField>


                                             <%--<asp:ButtonField HeaderText="Delete" ButtonType="Image" ImageUrl="~/Images/New folder/delnew.png" CommandName="DeleteEx" ItemStyle-Width="20px" ItemStyle-Height="5px"  OnClientClick="return confirm('Are you sure you want to delete this record?');"/>--%>
                                             <asp:TemplateField HeaderText="Delete">
                                                 <ItemTemplate>
                                                     <asp:LinkButton ID="btnDelete" runat="server"
                                                    
                                                         CommandName="DeleteEx"
                                                         CommandArgument='<%# Container.DataItemIndex %>'
                                                         OnClientClick="return confirm('Are you sure you want to delete this record?');"
                                                         ToolTip="Delete" CssClass="center-btn" ><i class="fa-solid fa-xmark text-danger"></i></asp:LinkButton>
                                                 </ItemTemplate>
                                                 <ItemStyle Width="20px" Height="5px" />
                                             </asp:TemplateField>
                                             <asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="20">
                                                 <ItemTemplate>
                                                     <%#Container.DataItemIndex+1 %>
                                                 </ItemTemplate>
                                             </asp:TemplateField>
                                             <asp:BoundField DataField="UserID" HeaderText="UserID" NullDisplayText="NA" />
                                             <asp:BoundField DataField="EmpID" HeaderText="EmpID" NullDisplayText="NA" />
                                             <asp:BoundField DataField="LoginName" HeaderText="Login Name" NullDisplayText="NA" />
                                             <asp:BoundField DataField="Approval1Name" HeaderText="Approval1 Name" NullDisplayText="NA" />
                                             <asp:BoundField DataField="Approval1Email" HeaderText="Approval1 Email" NullDisplayText="NA" />
                                             <asp:BoundField DataField="Approval1Active" HeaderText="Approval1 IS Active" NullDisplayText="NA" />
                                             <%--<asp:BoundField DataField="Approval1Status" HeaderText="Approval1 Status" NullDisplayText="NA" />--%>
                                             <asp:BoundField DataField="Approval1Level" HeaderText="Approval1 Grade" NullDisplayText="NA" />
                                             <%--<asp:BoundField DataField="Approval1RejectStatus" HeaderText="Approval1 Reject Status" NullDisplayText="NA" />--%>
                                             <asp:BoundField DataField="Approval2Name" HeaderText="Approval2 Name" NullDisplayText="NA" />
                                             <asp:BoundField DataField="Approval2Email" HeaderText="Approval2 Email" NullDisplayText="NA" />
                                             <asp:BoundField DataField="Approval2Active" HeaderText="Approval2 IS Active" NullDisplayText="NA" />
                                             <%--<asp:BoundField DataField="Approval2Status" HeaderText="Approval2 Status" NullDisplayText="NA" />--%>
                                             <asp:BoundField DataField="Approval2Level" HeaderText="Approval2 Grade" NullDisplayText="NA" />
                                             <%--<asp:BoundField DataField="Approval2RejectStatus" HeaderText="Approval2 Reject Status" NullDisplayText="NA" />--%>
                                             <asp:BoundField DataField="Approval3Name" HeaderText="Approval3 Name" NullDisplayText="NA" />
                                             <asp:BoundField DataField="Approval3Email" HeaderText="Approval3 Email" NullDisplayText="NA" />
                                             <asp:BoundField DataField="Approval3Active" HeaderText="Approval3 IS Active" NullDisplayText="NA" />
                                             <%--<asp:BoundField DataField="Approval3Status" HeaderText="Approval3 Status" NullDisplayText="NA" />--%>
                                             <asp:BoundField DataField="Approval3Level" HeaderText="Approval3 Grade" NullDisplayText="NA" />
                                             <%--<asp:BoundField DataField="Approval3RejectStatus" HeaderText="Approval3 Reject Status" NullDisplayText="NA" />--%>
                                             <asp:BoundField DataField="Approval4Name" HeaderText="Approval4 Name" NullDisplayText="NA" />
                                             <asp:BoundField DataField="Approval4Email" HeaderText="Approval4 Email" NullDisplayText="NA" />
                                             <asp:BoundField DataField="Approval4Active" HeaderText="Approval4 IS Active" NullDisplayText="NA" />
                                             <%--<asp:BoundField DataField="Approval4Status" HeaderText="Approval4 Status" NullDisplayText="NA" />--%>
                                             <asp:BoundField DataField="Approval4Level" HeaderText="Approval4 Grade" NullDisplayText="NA" />
                                             <%--<asp:BoundField DataField="Approval4RejectStatus" HeaderText="Approval4 Reject Status" NullDisplayText="NA" />--%>
                                             <asp:BoundField DataField="Approval5Name" HeaderText="Approval5 Name" NullDisplayText="NA" />
                                             <asp:BoundField DataField="Approval5Email" HeaderText="Approval5 Email" NullDisplayText="NA" />
                                             <asp:BoundField DataField="Approval5Active" HeaderText="Approval5 IS Active" NullDisplayText="NA" />
                                             <%--<asp:BoundField DataField="Approval5Status" HeaderText="Approval5S tatus" NullDisplayText="NA" />--%>
                                             <asp:BoundField DataField="Approval5Level" HeaderText="Approval5Grade" NullDisplayText="NA" />
                                             <%--<asp:BoundField DataField="Approval5RejectStatus" HeaderText="Approval5 Reject Status" NullDisplayText="NA" />--%>
                                         </Columns>
                                     </asp:GridView>
                                 </div>
                             </div>
                         </div>
                    
     </asp:Panel>
                </div>
       </div>
        </ContentTemplate>
        <Triggers>
            <%-- <asp:PostBackTrigger ControlID="btnInsert" />
            <asp:PostBackTrigger ControlID="btnUpdate" />--%>
            <asp:PostBackTrigger ControlID="gvUserWiseApproval" />
            <%--     <asp:PostBackTrigger ControlID="ImgBtnExport" />--%>
            <asp:PostBackTrigger ControlID="ddlRequestType" />
            <asp:PostBackTrigger ControlID="ddlUser" />
            <asp:PostBackTrigger ControlID="btnAddApproval" />
            <asp:PostBackTrigger ControlID="btnimportUser" />
            <asp:PostBackTrigger ControlID="btnViewUsers" />
            <asp:PostBackTrigger ControlID="ddlOrg" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>


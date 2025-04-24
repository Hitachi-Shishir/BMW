<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmSDCustomFieldCnrtl.aspx.cs" Inherits="HelpDesk_frmSDCustomFieldCnrtl" %>

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
                    <div class="row mb-2">
                        <div class="col-md-12">
                            <div class="btn-group">
                                <asp:Button ID="btnAddSDCustomFields" Text="Add SDCustomFields" runat="server" CssClass="btn btn-sm btn-outline-secondary " OnClick="btnAddSDCustomFields_Click" />
                                <asp:Button ID="btnViewSDCustomFields" runat="server" Text-="View Details" CssClass="btn btn-sm btn-secondary" OnClick="btnViewSDCustomFields_Click" />
                            </div>
                        </div>
                    </div>
                    <div class="card border bg-transparent shadow-none ">
                        <div class="card-body">

                            <asp:Panel ID="pnlAddSDCustomFields" runat="server" Visible="false">
                                <div class="  row gx-2 gy-3 ">
                                    <div class="col-md-4">
                                        <label for="staticEmail" class="form-label">
                                            Organization

                                        <asp:ImageButton ID="imgbtnAddOrg" runat="server" ImageUrl="~/Images/plus.png" OnClick="imgbtnAddOrg_Click" hidden />
                                            <asp:RequiredFieldValidator ID="rfvddlOrg" runat="server" ControlToValidate="ddlOrg" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="ReqType"></asp:RequiredFieldValidator>
                                        </label>



                                        <asp:DropDownList ID="ddlOrg" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlOrg_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-4">
                                        <label for="staticEmail" class="form-label">
                                            Request Type 

                                        <asp:ImageButton ID="ImgAddRequestType" runat="server" ImageUrl="~/Images/plus.png" OnClick="ImgAddRequestType_Click" hidden />
                                            <asp:RequiredFieldValidator ID="rfvddlRequestType" runat="server" ControlToValidate="ddlRequestType" InitialValue="0" ErrorMessage="Required" Font-Bold="true" ForeColor="Red" ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                                        </label>



                                        <asp:DropDownList ID="ddlRequestType" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlRequestType_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-4" hidden>
                                        <label for="staticEmail" class="form-label">
                                            SD Role  
                                        <asp:RequiredFieldValidator ID="rfvddlSDRole" runat="server" ControlToValidate="ddlSDRole" InitialValue="0" ErrorMessage="Required" Font-Bold="true" ForeColor="Red" ValidationGroup="btnSave"></asp:RequiredFieldValidator>
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
                                            Field Type 
                                        <asp:RequiredFieldValidator ID="rfvddlFieldType" runat="server" ControlToValidate="ddlFieldType" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="SDCustomFields"></asp:RequiredFieldValidator>
                                        </label>


                                        <asp:DropDownList ID="ddlFieldType" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field">
                                            <asp:ListItem Text="---Select Field---" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="TextBox" Value="TextBox"></asp:ListItem>
                                            <asp:ListItem Text="List" Value="DropDown"></asp:ListItem>
                                        </asp:DropDownList>

                                    </div>


                                    <div class="col-md-4">
                                        <label for="staticEmail" class="form-label">
                                            Field Name  
                                        <asp:RequiredFieldValidator ID="rfvtxtFieldName" runat="server" ControlToValidate="txtFieldName" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="SDCustomFields"></asp:RequiredFieldValidator>
                                        </label>


                                        <asp:TextBox ID="txtFieldName" onkeypress="return /^[a-zA-Z0-9_]*$/.test(event.key) && this.value.length < 50;" runat="server" TextMode="SingleLine" CssClass="form-control  form-control-sm"></asp:TextBox>

                                    </div>
                                    <div class="col-md-4">
                                        <label for="staticEmail" class="form-label">
                                            Field Mode 
                                        <asp:RequiredFieldValidator ID="rfvddlFieldMode" runat="server" ControlToValidate="ddlFieldMode" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="SDCustomFields"></asp:RequiredFieldValidator>
                                        </label>


                                        <asp:DropDownList ID="ddlFieldMode" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field">
                                            <asp:ListItem Text="---Select Mode---" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="DateTime" Value="DateTime"></asp:ListItem>
                                            <asp:ListItem Text="Number" Value="int"></asp:ListItem>
                                            <asp:ListItem Text="SingleLine" Value="varchar(500)"></asp:ListItem>
                                            <asp:ListItem Text="MultiLine" Value="varchar(max)"></asp:ListItem>
                                        </asp:DropDownList>

                                    </div>
                                    <div class="col-md-4">
                                        <label for="staticEmail" class="form-label">
                                            IS Visible 
                                        <asp:RequiredFieldValidator ID="frvddlVisibilty" runat="server" ControlToValidate="ddlVisibilty" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="SDCustomFields"></asp:RequiredFieldValidator>
                                        </label>


                                        <asp:DropDownList ID="ddlVisibilty" runat="server" CssClass="form-control  form-control-sm">
                                            <asp:ListItem Text="---Select Visibilty---" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="True" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="False" Value="0"></asp:ListItem>

                                        </asp:DropDownList>

                                    </div>
                                    <div class="col-md-4">
                                        <label for="staticEmail" class="form-label">
                                            Is Required
                                        <asp:RequiredFieldValidator ID="rfvddlRequiredStatus" runat="server" ControlToValidate="ddlRequiredStatus" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="SDCustomFields"></asp:RequiredFieldValidator>
                                        </label>


                                        <asp:DropDownList ID="ddlRequiredStatus" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field">
                                            <asp:ListItem Text="---Select ---" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="True" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="False" Value="2"></asp:ListItem>

                                        </asp:DropDownList>

                                    </div>



                                    <div class="col-md-4">
                                        <label for="staticEmail" class="form-label">
                                            Field Scope  
                                        <asp:RequiredFieldValidator ID="rfvddlFieldScope" runat="server" ControlToValidate="ddlFieldScope" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="SDCustomFields"></asp:RequiredFieldValidator>
                                        </label>


                                        <asp:DropDownList ID="ddlFieldScope" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field">
                                            <asp:ListItem Text="---Select Scope---" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="For Both" Value="ForBoth"></asp:ListItem>
                                            <asp:ListItem Text="For User" Value="ForUser"></asp:ListItem>
                                            <asp:ListItem Text="For Technician" Value="ForTechnician"></asp:ListItem>

                                        </asp:DropDownList>

                                    </div>

                                    <div class="col-md-12 text-end">
                                        <asp:Button ID="btnInsertSDCustomFields" runat="server" Text="Save" class="btn btn-sm btn-grd btn-grd-info " OnClick="btnInsertSDCustomFields_Click" ValidationGroup="SDCustomFields" />
                                        <asp:Button ID="btnUpdateSDCustomFields" runat="server" Text="Update" Visible="false" class="btn btn-sm btn-grd btn-grd-info " OnClick="btnUpdateSDCustomFields_Click" ValidationGroup="SDCustomFields" />
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="btn btn-sm btn-grd btn-grd-danger " OnClick="btnCancel_Click" CausesValidation="false" />

                                    </div>

                                </div>





                            </asp:Panel>
                            <asp:Panel ID="pnlShowSDCustomFields" runat="server">


                                <div class="row ">
                                    <div class="col-md-4 d-none">

                                        <asp:Label ID="lblsofname" runat="server" Text="SDCustomFields Details" Font-Size="Larger" ForeColor="Black"></asp:Label>

                                    </div>
                                    <div class="col-md-6">
                                        <asp:Label ID="Label1" runat="server"></asp:Label>
                                        <asp:Label ID="Label3" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-md-2 d-none">
                                        <div class="btn btn-sm elevation-1 ml-1 " style="padding: 0px; margin-bottom: 10px; padding-top: 1px">
                                            <label class="mr-2 ml-1 mb-0">Export</label>
                                            <asp:ImageButton ID="ImgBtnExport" runat="server" ImageUrl="~/Images/New folder/excelnew.png" CssClass="fa-pull-right btn-outline-success mr-1" OnClick="ImgBtnExport_Click" />
                                        </div>
                                    </div>

                                    <div class="col-md-12">
                                        <div class="table-responsive table-container">
                                            <asp:GridView GridLines="None" ID="gvSDCustomFields" runat="server" DataKeyNames="ID" AutoGenerateColumns="false" CssClass="data-table table table-striped border text-nowrap table-sm"
                                                Width="100%" OnRowCommand="gvSDCustomFields_RowCommand" OnRowDataBound="gvSDCustomFields_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="20">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex+1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="DeskRef" HeaderText="Request Type" NullDisplayText="NA" />
                                                    <asp:BoundField DataField="FieldID" HeaderText="FieldID" NullDisplayText="NA" />
                                                    <asp:BoundField DataField="FieldName" HeaderText="Field Name" NullDisplayText="NA" />
                                                    <asp:BoundField DataField="FieldMode" HeaderText="Field Mode" NullDisplayText="NA" />
                                                    <asp:BoundField DataField="fieldType" HeaderText="Field Type" NullDisplayText="NA" />
                                                    <asp:BoundField DataField="Status" HeaderText="Status" NullDisplayText="NA" />
                                                    <asp:BoundField DataField="IsFieldReq" HeaderText="IsFieldReq" NullDisplayText="NA" />
                                                    <asp:BoundField DataField="FieldScope" HeaderText="FieldScope" NullDisplayText="NA" />
                                                    <asp:TemplateField HeaderText=" Organization">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblOrgFk" runat="server" Text='<%# Eval("Org_ID") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lblOrgName" runat="server" Text='<%# Eval("OrgName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Edit">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="SelectState" CommandArgument="<%# Container.DataItemIndex %>" CssClass="center-btn">
                                                  <i class="fa-solid fa-edit"></i> 
                                                            </asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Delete">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkDelete" runat="server" CommandName="DeleteEx" CommandArgument="<%# Container.DataItemIndex %>"  CssClass="center-btn">
          <i class="fa-solid fa-xmark text-danger"></i> 
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
                </div>
            </div>
            <div class="modal " id="basicModal" <%-- tabindex="-1"--%> role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true">
                <div class="modal-dialog modal-dialog-centered">
                    <div class="modal-content">
                        <div class="card-header" style="border-bottom: none">

                            <button type="button" class="close" onclick="javascript:window.location.reload()" data-dismiss="modal" aria-hidden="true">&times;</button>


                        </div>
                        <%--  <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>--%>
                        <div class="card-body">
                            <asp:Label ID="lblsuccess" runat="server" Text=""></asp:Label>
                            <asp:PlaceHolder ID="pnlShowRqstType" runat="server"></asp:PlaceHolder>
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal" id="CategoryModal" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true">
                <div class="modal-dialog modal-dialog-centered modal-xl">
                    <div class="modal-content">
                        <div class="card-header">
                            Organization
                        <button type="button" class="close" onclick="javascript:window.location.reload()" data-dismiss="modal" aria-hidden="true">&times;</button>


                        </div>
                        <%--  <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>--%>

                        <div class="row">
                            <div class="col-md-12">
                                <asp:PlaceHolder ID="pnlShowOrg" runat="server"></asp:PlaceHolder>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>

        <Triggers>
            <asp:PostBackTrigger ControlID="ImgBtnExport" />
            <asp:AsyncPostBackTrigger ControlID="ImgAddRequestType" EventName="Click" />
            <asp:PostBackTrigger ControlID="ddlRequestType" />
            <asp:PostBackTrigger ControlID="ddlOrg" />
            <asp:PostBackTrigger ControlID="btnAddSDCustomFields" />
            <asp:PostBackTrigger ControlID="btnViewSDCustomFields" />
            <asp:PostBackTrigger ControlID="gvSDCustomFields" />
            <asp:AsyncPostBackTrigger ControlID="imgbtnAddOrg" EventName="Click" />
        </Triggers>

    </asp:UpdatePanel>
</asp:Content>


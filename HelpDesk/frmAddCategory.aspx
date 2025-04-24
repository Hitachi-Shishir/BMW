<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmAddCategory.aspx.cs" Inherits="HelpDesk_frmAddCategory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="scriptmanager1" runat="server"></asp:ScriptManager>
    <asp:Panel ID="pnlCategory" runat="server" Visible="false">
        <asp:UpdatePanel ID="updatepanel7" runat="server">
            <ContentTemplate>

                <div class="card mb-5">
                    <div class="card-body mb-5">
                        <div class="d-flex align-items-start justify-content-between">
                            <h6 class="mb-3 fw-bold">Add Category</h6>
                            <div class="d-flex">
                                
                                &nbsp;
    
                            
                            </div>
                        </div>


                        <div class="row gy-3 gx-2">
                            <div class="col-md-4 ">
                                <label class="form-label ">
                                    Organization
                               
                                <asp:RequiredFieldValidator ID="reqflddlOrg6" runat="server" ControlToValidate="ddlOrg6" InitialValue="0" ErrorMessage="*" ForeColor="Red" ValidationGroup="SaveCatI"></asp:RequiredFieldValidator>

                                </label>
                                <asp:DropDownList ID="ddlOrg6" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true"  OnSelectedIndexChanged="ddlOrg6_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-4">
                                <label class="form-label">
                                    Request Type
                                       
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="ddlRequestTypeCategory" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="SaveCatI"></asp:RequiredFieldValidator>
                                </label>
                                <asp:DropDownList  ID="ddlRequestTypeCategory" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlRequestTypeCategory_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-4">
                            </div>
                            <div class="col-md-4">
                                <label class="form-label">
                                    Category
       
                                <asp:LinkButton runat="server" ID="imgbtnAddParentCategory" ToolTip="Add Category" OnClick="imgbtnAddParentCategory_Click"><i class="fa-solid fa-plus p-1 rounded-circle border"></i></asp:LinkButton>
                                    <asp:LinkButton ID="imgbtnSaveParentCategory" runat="server" ToolTip="Save Category" OnClick="imgbtnSaveParentCategory_Click" ValidationGroup="SaveCatI" Enabled="false"><i class="fa-solid fa-floppy-disk p-1 rounded-circle border"></i></asp:LinkButton>

                                    <asp:LinkButton ID="imgbtnUpdateParentCategory" runat="server" ToolTip="Update Category" OnClick="imgbtnUpdateParentCategory_Click" ValidationGroup="SaveCatI" Visible="false"><i class="fa-solid fa-pen p-1 rounded-circle border"></i></asp:LinkButton>
                                    <asp:LinkButton ID="imgbtnEditParentCategory" runat="server" ToolTip="Edit Category" OnClick="imgbtnEditParentCategory_Click"><i class="fa-solid fa-edit p-1 rounded-circle border"></i></asp:LinkButton>
                                    <asp:LinkButton ID="imgbtnCancelParent" runat="server" Enabled="false" ToolTip="Refresh" OnClick="imgbtnCancelParent_Click"><i class="fa-solid fa-rotate p-1 rounded-circle border "></i></asp:LinkButton>
                                    <asp:RequiredFieldValidator ID="rfvtxtParentCategory" runat="server" ControlToValidate="txtParentCategory" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="SaveCatI"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfvddlParentCategory" runat="server" ControlToValidate="ddlParentCategory" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="AddCatII"></asp:RequiredFieldValidator>
                                </label>
                                <asp:TextBox ID="txtParentCategory" runat="server" CssClass="form-control  form-control-sm" ToolTip="Add Category" Visible="false" onkeypress="return /^[a-zA-Z0-9\s]*$/.test(event.key) && this.value.length < 50;"></asp:TextBox>
                                <asp:DropDownList ID="ddlParentCategory" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlParentCategory_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>

                            <div class="col-md-4">
                                <label class="form-label">
                                    Category II
  
                                <asp:LinkButton ID="imgbtnCatII" runat="server" OnClick="imgbtnCatII_Click" ValidationGroup="AddCatII"><i class="fa-solid fa-plus p-1 rounded-circle border"></i></asp:LinkButton>
                                    <asp:LinkButton ID="imgbtnSaveCatII" runat="server" ToolTip="Save Category" OnClick="imgbtnSaveCatII_Click" ValidationGroup="SaveCatII" Enabled="false"><i class="fa-solid fa-floppy-disk p-1 rounded-circle border"></i></asp:LinkButton>
                                    <asp:LinkButton ID="imtbtnUpdateCatII" runat="server" ToolTip="Update Category" OnClick="imtbtnUpdateCatII_Click" ValidationGroup="SaveCatII" Visible="false"><i class="fa-solid fa-pen p-1 rounded-circle border"></i></asp:LinkButton>
                                    <asp:LinkButton ID="imgbtnEditCatII" runat="server" ToolTip="Edit Category" OnClick="imgbtnEditCatII_Click"><i class="fa-solid fa-edit p-1 rounded-circle border "></i></asp:LinkButton>
                                    <asp:LinkButton ID="imgbtnCancelCatII" runat="server" ToolTip="Refresh" OnClick="imgbtnCancelCatII_Click"><i class="fa-solid fa-rotate p-1 rounded-circle border "></i></asp:LinkButton>
                                    <asp:RequiredFieldValidator ID="rfvtxtCatII" runat="server" ControlToValidate="txtCatII" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="SaveCatII"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfvddlCatII" runat="server" ControlToValidate="ddlCatII" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="AddCatIII"></asp:RequiredFieldValidator>
                                </label>
                                <asp:TextBox ID="txtCatII" runat="server" CssClass="form-control  form-control-sm" ValidationGroup="SaveCatII" Visible="false" onkeypress="return /^[a-zA-Z0-9\s]*$/.test(event.key) && this.value.length < 50;"></asp:TextBox>
                                <asp:DropDownList ID="ddlCatII" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" Enabled="false" AutoPostBack="true" OnSelectedIndexChanged="ddlCatII_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>

                            <div class="col-md-4">
                                <label class="form-label">
                                    Category III
  
                                <asp:LinkButton ID="imgAddCatIII" runat="server" OnClick="imgAddCatIII_Click" ValidationGroup="AddCatIII"> <i class="fa-solid fa-plus p-1 rounded-circle border"></i></asp:LinkButton>
                                    <asp:LinkButton ID="imgSaveCatIII" runat="server" OnClick="imgSaveCatIII_Click" ValidationGroup="SaveCatIII" Enabled="false"><i class="fa-solid fa-floppy-disk p-1 rounded-circle border"></i></asp:LinkButton>
                                    <asp:LinkButton ID="imgbtnUpdateCatIII" runat="server" ToolTip="Update Category" OnClick="imgbtnUpdateCatIII_Click" ValidationGroup="SaveCatIII" Visible="false"><i class="fa-solid fa-pen p-1 rounded-circle border"></i></asp:LinkButton>
                                    <asp:LinkButton ID="imgbtnEditCatIII" runat="server" ToolTip="Edit Category" OnClick="imgbtnEditCatIII_Click"><i class="fa-solid fa-edit p-1 rounded-circle border "></i></asp:LinkButton>
                                    <asp:LinkButton ID="imgbtnCancelCatIII" runat="server" Enabled="false" ToolTip="Refresh" OnClick="imgbtnCancelCatIII_Click"><i class="fa-solid fa-rotate p-1 rounded-circle border "></i></asp:LinkButton>
                                    <asp:RequiredFieldValidator ID="rfvtxtCatLevelIII" runat="server" ControlToValidate="txtCatLevelIII" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="SaveCatIII"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfvddlCateLevelIII" runat="server" ControlToValidate="ddlCateLevelIII" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="AddCatIV"></asp:RequiredFieldValidator>
                                </label>
                                <asp:TextBox ID="txtCatLevelIII" runat="server" CssClass="form-control  form-control-sm" ValidationGroup="SaveCatIII" Visible="false" onkeypress="return /^[a-zA-Z0-9\s]*$/.test(event.key) && this.value.length < 50;"></asp:TextBox>
                                <asp:DropDownList ID="ddlCateLevelIII" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" Enabled="false" AutoPostBack="true" OnSelectedIndexChanged="ddlCateLevelIII_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>

                            <div class="col-md-4">
                                <label class="form-label">
                                    Category IV
  
                                <asp:LinkButton ID="imgbtnCatelevelIV" runat="server" OnClick="imgbtnCatelevelIV_Click" ValidationGroup="AddCatIV"> <i class="fa-solid fa-plus p-1 rounded-circle border"></i></asp:LinkButton>
                                    <asp:LinkButton ID="imgbtnSaveCateLvlIV" runat="server" OnClick="imgbtnSaveCateLvlIV_Click" ValidationGroup="SaveCatIV" Enabled="false"><i class="fa-solid fa-floppy-disk p-1 rounded-circle border"></i></asp:LinkButton>
                                    <asp:LinkButton ID="imgbtnUpdateCateLvIV" runat="server" ToolTip="Update Category" OnClick="imgbtnUpdateCateLvIV_Click" ValidationGroup="SaveCatIV" Visible="false"><i class="fa-solid fa-pen p-1 rounded-circle border"></i></asp:LinkButton>
                                    <asp:LinkButton ID="imgbtnEditCatLvIV" runat="server" ToolTip="Edit Category" OnClick="imgbtnEditCatLvIV_Click"><i class="fa-solid fa-edit p-1 rounded-circle border "></i></asp:LinkButton>
                                    <asp:LinkButton ID="imgbtnCancelCatIV" runat="server" Enabled="false" ToolTip="Refresh" OnClick="imgbtnCancelCatIV_Click"><i class="fa-solid fa-rotate p-1 rounded-circle border "></i></asp:LinkButton>
                                    <asp:RequiredFieldValidator ID="rfvtxtCateLevelIV" runat="server" ControlToValidate="txtCateLevelIV" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="SaveCatIV"></asp:RequiredFieldValidator>
                                    <asp:RequiredFieldValidator ID="rfvddlCateLevelIV" runat="server" ControlToValidate="ddlCateLevelIV" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="AddCatV"></asp:RequiredFieldValidator>
                                </label>
                                <asp:TextBox ID="txtCateLevelIV" runat="server" CssClass="form-control  form-control-sm" ValidationGroup="SaveCatIV" Visible="false" onkeypress="return /^[a-zA-Z0-9\s]*$/.test(event.key) && this.value.length < 50;"></asp:TextBox>
                                <asp:DropDownList ID="ddlCateLevelIV" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" Enabled="false" AutoPostBack="true" OnSelectedIndexChanged="ddlCateLevelIV_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>

                            <div class="col-md-4">

                                <label class="form-label">
                                    Category V
  
                                <asp:LinkButton ID="imgbtnAddCatV" runat="server" OnClick="imgbtnAddCatV_Click" ValidationGroup="AddCatV"> <i class="fa-solid fa-plus p-1 rounded-circle border"></i></asp:LinkButton>
                                    <asp:LinkButton ID="imgbtnSaveCatV" runat="server" OnClick="imgbtnSaveCatV_Click" ValidationGroup="SaveCatV" Enabled="false"><i class="fa-solid fa-floppy-disk p-1 rounded-circle border"></i></asp:LinkButton>
                                    <asp:LinkButton ID="imgbtnUpdateCatV" runat="server" ToolTip="Update Category" OnClick="imgbtnUpdateCatV_Click" ValidationGroup="SaveCatV" Visible="false"><i class="fa-solid fa-pen p-1 rounded-circle border"></i></asp:LinkButton>
                                    <asp:LinkButton ID="imgbtnEditCatV" runat="server" ToolTip="Edit Category" OnClick="imgbtnEditCatV_Click"><i class="fa-solid fa-edit p-1 rounded-circle border "></i></asp:LinkButton>
                                    <asp:LinkButton ID="imgbtnCancelCatV" runat="server" ToolTip="Refresh" OnClick="imgbtnCancelCatV_Click"><i class="fa-solid fa-rotate p-1 rounded-circle border "></i></asp:LinkButton>
                                    <asp:RequiredFieldValidator ID="rfvtxtCatV" runat="server" ControlToValidate="txtCatV" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="SaveCatV"></asp:RequiredFieldValidator>
                                </label>
                                <asp:TextBox ID="txtCatV" runat="server" CssClass="form-control  form-control-sm" ValidationGroup="SaveCatV" Visible="false" onkeypress="return /^[a-zA-Z0-9\s]*$/.test(event.key) && this.value.length < 50;"></asp:TextBox>
                                <asp:DropDownList ID="ddlCatV" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" Enabled="false">
                                </asp:DropDownList>
                            </div>

                        </div>
                        <div id="divres" runat="server" class="row gy-2 gx-3 mt-2">
                            <div class="col-md-3">
                                <label class="form-label">Response Time (in Min) </label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server" ControlToValidate="txtRespCat" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="ADDCAT"></asp:RequiredFieldValidator>
                                <asp:TextBox ID="txtRespCat" runat="server" CssClass="form-control  form-control-sm" ToolTip="Add Category" onkeypress="return /^[0-9\s]*$/.test(event.key) && this.value.length < 50;"></asp:TextBox>
                            </div>
                            <div class="col-md-3">
                                <label class="form-label">Resolution Time (in Min) </label>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator27" runat="server" ControlToValidate="txtReslCat" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="ADDCAT"></asp:RequiredFieldValidator>
                                <asp:TextBox ID="txtReslCat" runat="server" CssClass="form-control  form-control-sm" ToolTip="Add Category" onkeypress="return /^[0-9\s]*$/.test(event.key) && this.value.length < 50;"></asp:TextBox>
                            </div>
                            <div class="col-md-3">
                                <label class="form-lable opacity-0">lfse</label><br />
                                <asp:Button ID="btnSubmit" runat="server" Text="ADD" OnClick="btnSubmit_Click" CssClass="btn btn-grd-info btn-sm" ValidationGroup="ADDCAT" />
                            </div>
                        </div>
                        <div class="form-row d-none">
                            <div class="col-md-2 offset-10 ">
                                <asp:Button ID="btnClose7" runat="server" Text="Close" class="btn btn-danger" OnClick="btnClose7_Click" />
                            </div>
                        </div>
                    </div>
                </div>


                <asp:HiddenField ID="hdnVarCategoryI" runat="server" />
                <asp:HiddenField ID="hdnVarCategoryII" runat="server" />
                <asp:HiddenField ID="hdnVarCategoryIII" runat="server" />
                <asp:HiddenField ID="hdnVarCategoryIV" runat="server" />
                <asp:HiddenField ID="hdnVarCategoryV" runat="server" />
            </ContentTemplate>

            <Triggers>
                <asp:PostBackTrigger ControlID="imgbtnUpdateParentCategory" />
                <asp:PostBackTrigger ControlID="imtbtnUpdateCatII" />
                <asp:PostBackTrigger ControlID="imgbtnUpdateCatIII" />
                <asp:PostBackTrigger ControlID="imgbtnUpdateCateLvIV" />
                <asp:PostBackTrigger ControlID="imgbtnUpdateCatV" />
                <asp:PostBackTrigger ControlID="imgbtnSaveParentCategory" />
                <asp:PostBackTrigger ControlID="imgbtnSaveCatII" />
                <asp:PostBackTrigger ControlID="imgSaveCatIII" />
                <asp:PostBackTrigger ControlID="imgbtnSaveCateLvlIV" />
                <asp:PostBackTrigger ControlID="imgbtnSaveCatV" />
                <asp:PostBackTrigger ControlID="ddlOrg6" />
                <%--Parent Controls  Category I Controls--%>
                <asp:PostBackTrigger ControlID="ddlParentCategory" />
                <asp:PostBackTrigger ControlID="imgbtnAddParentCategory" />
                <%--<asp:PostBackTrigger ControlID="imgbtnSaveParentCategory" />--%>
                <asp:PostBackTrigger ControlID="imgbtnEditParentCategory" />
                <asp:PostBackTrigger ControlID="imgbtnCancelParent" />
                <%--Parent Controls  Category II Controls--%>
                <asp:PostBackTrigger ControlID="ddlCatII" />
                <asp:PostBackTrigger ControlID="imgbtnCatII" />
                <%--<asp:PostBackTrigger ControlID="imgbtnSaveCatII" />--%>
                <asp:PostBackTrigger ControlID="imgbtnEditCatII" />
                <asp:PostBackTrigger ControlID="imgbtnCancelCatII" />
                <%--Parent Controls  Category III Controls--%>
                <asp:PostBackTrigger ControlID="ddlCateLevelIII" />
                <asp:PostBackTrigger ControlID="imgAddCatIII" />
                <%--<asp:PostBackTrigger ControlID="imgSaveCatIII" />--%>
                <asp:PostBackTrigger ControlID="imgbtnEditCatIII" />
                <asp:PostBackTrigger ControlID="imgbtnCancelCatIII" />

                <%--Parent Controls  Category IV Controls--%>
                <asp:PostBackTrigger ControlID="ddlCateLevelIV" />
                <asp:PostBackTrigger ControlID="imgbtnCatelevelIV" />
                <%--<asp:PostBackTrigger ControlID="imgbtnSaveCateLvlIV" />--%>
                <asp:PostBackTrigger ControlID="imgbtnEditCatLvIV" />
                <asp:PostBackTrigger ControlID="imgbtnCancelCatIV" />
                <%--Parent Controls  Category V Controls--%>

                <asp:PostBackTrigger ControlID="imgbtnAddCatV" />
                <asp:PostBackTrigger ControlID="imgbtnSaveCatV" />
                <asp:PostBackTrigger ControlID="imgbtnEditCatV" />
                <asp:PostBackTrigger ControlID="imgbtnCancelCatV" />
                <asp:PostBackTrigger ControlID="ddlRequestTypeCategory" />
            </Triggers>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>


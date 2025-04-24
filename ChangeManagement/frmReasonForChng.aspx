<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmReasonForChng.aspx.cs" Inherits="ChangeManagement_frmReasonForChng" %>

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
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>


            <div class="card">
                <div class="card-body">
                    <div class="row mb-2">
                        <div class="col-md-12">
                            <div class="btn-group">
                                <asp:Button ID="btnAddReqType" Text="Add Reason Type" runat="server" CssClass="btn btn-sm btn-outline-secondary" OnClick="btnAddReqType_Click" />
                                <asp:Button ID="btnViewReqType" runat="server" Text-="View Details" CssClass="btn btn-sm btn-outline-secondary" OnClick="btnViewReqType_Click" />
                            </div>
                        </div>
                    </div>
                    <div class="card border bg-transparent shadow-none mb-3">
                        <div class="card-body">
                            <asp:Panel ID="ShowPanelAdd" runat="server" Visible="false">
                                <div class="  row gx-2 gy-3">
                                    <div class="col-md-4">
                                        <label for="staticEmail" class="form-label">
                                            Organization

                                        <asp:ImageButton ID="imgbtnAddOrg" runat="server" ImageUrl="~/Images/plus.png" OnClick="imgbtnAddOrg_Click" hidden />
                                            <asp:RequiredFieldValidator ID="rfvddlOrg" runat="server" ControlToValidate="ddlOrg" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="ReqType"></asp:RequiredFieldValidator>
                                        </label>
                                        <asp:DropDownList ID="ddlOrg" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field">
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-4">
                                        <label for="staticEmail" class="form-label">
                                            Reason  Type 
                                        <asp:RequiredFieldValidator ID="rfvtxtReasonType" runat="server" ControlToValidate="txtReasonType" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="ReqType"></asp:RequiredFieldValidator>
                                        </label>
                                        <asp:TextBox ID="txtReasonType" runat="server" CssClass="form-control  form-control-sm" onkeypress="return /^[a-zA-Z-_ ]*$/.test(event.key) && this.value.length < 50;"></asp:TextBox>

                                    </div>
                                    <div class="col-md-12">
                                        <label for="staticEmail" class="form-label">
                                            Reason  Description 
     <asp:RequiredFieldValidator ID="rfvtxtReasonDesc" runat="server" ControlToValidate="txtReasonDesc" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="ReqType"></asp:RequiredFieldValidator>
                                        </label>
                                        <asp:TextBox ID="txtReasonDesc" runat="server" TextMode="MultiLine" Rows="3" Columns="3" CssClass="form-control  form-control-sm"></asp:TextBox>

                                    </div>
                                    <div class="col-md-12 text-end">
                                        <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSaveReqType_Click" class="btn btn-sm btn-grd btn-grd-info " ValidationGroup="ReqType"></asp:Button>
                                        <asp:Button ID="btnUpdateReqType" runat="server" Text="Update" Visible="false" class="btn btn-sm btn-grd btn-grd-info " OnClick="btnUpdateReqType_Click" ValidationGroup="ReqType" />
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-sm btn-grd btn-grd-danger " OnClick="btnCancel_Click" />
                                         <asp:Button ID="btnCloseDesk" runat="server" Text="Close & Goto DeskConfig" CssClass="btn btn-sm btn-grd btn-grd-success" OnClick="btnCloseDesk_Click" />
                                    </div>


                                </div>







                            </asp:Panel>
                            <asp:Panel ID="pnlViewRequestType" runat="server">

                                <div class="row ">
                                    <div class="col-md-4 d-none">

                                        <asp:Label ID="lblsofname" runat="server" Text="Request Type Details" Font-Size="Larger" ForeColor="Black"></asp:Label>

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
                                            <asp:GridView GridLines="None" ID="gvReqType" runat="server" DataKeyNames="ID" AutoGenerateColumns="false" CssClass="data-table table table-striped border table-sm text-nowrap"
                                                Width="100%" OnRowCommand="gvReqType_RowCommand" OnRowDataBound="gvReqType_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="20">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex+1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="ReasonTypeRef" HeaderText="Reason" NullDisplayText="NA" />
                                                    <asp:BoundField DataField="ReasonDef" HeaderText="Reason Description" NullDisplayText="NA" />
                                                    <asp:TemplateField HeaderText=" Organization">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblOrgFk" runat="server" Text='<%# Eval("Org_ID") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lblOrgName" runat="server" Text='<%# Eval("OrgName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                                                            <asp:TemplateField HeaderText="Edit">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEdit" runat="server" CommandName="SelectState" CommandArgument="<%# Container.DataItemIndex %>">
                                                  <i class="fa-solid fa-edit"></i> 
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Delete">
    <ItemTemplate>
        <asp:LinkButton ID="lnkDelete" runat="server" CommandName="DeleteEx" CommandArgument="<%# Container.DataItemIndex %>">
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
            <div class="modal" id="CategoryModal" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true">
                <div class="modal-dialog modal-dialog-centered modal-xl">
                    <div class="modal-content">
                        <div class="card-header">
                            Organization
                        <button type="button" class="close" onclick="javascript:window.location.reload()" data-dismiss="modal" aria-hidden="true">&times;</button>


                        </div>
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
            <asp:PostBackTrigger ControlID="btnAddReqType" />
            <asp:PostBackTrigger ControlID="btnViewReqType" />
            <asp:PostBackTrigger ControlID="gvReqType" />
            <asp:PostBackTrigger ControlID="ImgBtnExport" />
            <asp:PostBackTrigger ControlID="btnCloseDesk" />
            <asp:AsyncPostBackTrigger ControlID="imgbtnAddOrg" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>


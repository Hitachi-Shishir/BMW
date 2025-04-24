<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmAddHolidays.aspx.cs" Inherits="frmAddHolidays" %>

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
            /*margin-top: -1.7rem!important;*/
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="scriptmanager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <div class="card">
                <div class="card-body">
                    <div class="row mb-2 ">
                        <div class="col-md-12">
                            <div class="btn-group">
                                <asp:Button ID="btnAddHoliday" runat="server" Text="Add Holiday" CausesValidation="false" OnClick="btnAddHoliday_Click" CssClass="btn btn-sm btn-outline-secondary" />
                                <asp:Button ID="btnimportUser" runat="server" Text="Import Holidays" CausesValidation="false" OnClick="btnimportUser_Click" CssClass="btn btn-sm btn-outline-secondary" />
                                <asp:Button ID="btnViewUsers" runat="server" Text="View Holiday" CausesValidation="false" OnClick="btnViewUsers_Click" CssClass="btn btn-sm btn-outline-secondary" />
                            </div>
                        </div>
                    </div>
                    <div class="card border bg-transparent shadow-none">
                        <div class="card-body">
                            <asp:Panel ID="pnlAddHoliday" Visible="false" runat="server">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="card-body p-0">
                                            <div class=" row gy-3 gx-2">
                                                <div class="col-md-4 ">
                                                    <label for="staticEmail" class="form-label">
                                                        Organization
                                                        <asp:RequiredFieldValidator ID="rfvddlOrg" runat="server" ControlToValidate="ddlOrg" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                                                    </label>
                                                    <asp:DropDownList ID="ddlOrg" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class=" col-md-4">
                                                    <label for="staticEmail" class="form-label">
                                                        Holiday Name
                                                        <asp:RequiredFieldValidator ID="rfvtxtHolidayName" runat="server" ControlToValidate="txtHolidayName" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Tech">

                                                        </asp:RequiredFieldValidator>
                                                    </label>

                                                    <asp:TextBox ID="txtHolidayName" class="form-control form-control-sm" runat="server"></asp:TextBox>
                                                </div>
                                                <div class=" col-md-4">
                                                    <label for="staticEmail" class="form-label">
                                                        Date
                                                        <asp:RequiredFieldValidator ID="rfvtxtholidayDatee" runat="server" ControlToValidate="txtholidayDate" ErrorMessage="*" ForeColor="Red" ValidationGroup="SearchUser"></asp:RequiredFieldValidator>
                                                    </label>
                                                    <asp:TextBox ID="txtholidayDate" autocomplete="off" ClientIDMode="static" class="form-control form-control-sm datepicker" runat="server" />
                                                </div>
                                                <div class="col-md-12 text-end">
                                                    <asp:Button ID="btnInsertHoliday" runat="server" Text="Save" OnClick="btnInsertHoliday_Click" class="btn btn-sm btn-grd btn-grd-info " ValidationGroup="Tech"></asp:Button>
                                                    <asp:Button ID="btnUpdateHoliday" runat="server" Text="Update" Visible="false" class="btn btn-sm btn-grd btn-grd-info " OnClick="btnUpdateHoliday_Click" ValidationGroup="ReqType" />
                                                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="btn btn-sm btn-grd btn-grd-danger " OnClick="btnCancel_Click" CausesValidation="false" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </asp:Panel>
                            <asp:Panel ID="pnlShowUsers" runat="server">

                                <div class="row">
                                    <div class="col-md-6">
                                        <asp:Label ID="Label1" runat="server"></asp:Label>
                                        <asp:Label ID="Label3" runat="server"></asp:Label>
                                    </div>
                                    <div class="col-md-12 text-end d-none">
                                        <asp:LinkButton ID="ImgBtnExport" runat="server" OnClick="ImgBtnExport_Click" CssClass="btn btn-sm btn-outline-secondary">Export</asp:LinkButton>
                                    </div>
                                    <div class="col-md-12">
                                        <div class="table-responsive  table-container">
                                            <asp:GridView GridLines="None" ID="gvHRRequest" runat="server" DataKeyNames="ID" AutoGenerateColumns="false" CssClass="data-table table table-striped border table-sm text-nowrap table-hover"
                                                Width="100%" OnRowCommand="gvHRRequest_RowCommand" OnRowDataBound="gvHRRequest_RowDataBound">
                                                <Columns>
                                                    <%--  <asp:ButtonField HeaderText="Delete" ButtonType="Image" ImageUrl="~/Images/New folder/delnew.png" CommandName="DeleteEx" ItemStyle-Width="20px" ItemStyle-Height="5px" />--%>
                                                    <asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="20">
                                                        <ItemTemplate>
                                                            <%#Container.DataItemIndex+1 %>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Edit">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName="SelectTech" CommandArgument="<%# Container.DataItemIndex %>">
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


                                                    <asp:BoundField DataField="HolidayName" HeaderText="Holiday Name" NullDisplayText="NA" />
                                                    <asp:BoundField DataField="HolidayDate" HeaderText="Holiday Date" NullDisplayText="NA" DataFormatString="{0:dd-MM-yyyy}" />
                                                    <asp:TemplateField HeaderText=" Organization">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblOrgFk" runat="server" Text='<%# Eval("Org_ID") %>' Visible="false"></asp:Label>
                                                            <asp:Label ID="lblOrgName" runat="server" Text='<%# Eval("OrgName") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>

                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="pnlImportUser" runat="server" Visible="false">

                                <div class="row gy-3 gx-2">
                                    <div class="col-md-6">
                                        <label class="form-label">
                                            Select Holiday(s) 
                                        </label>

                                        <%--<div class="custom-file">--%>


                                        <asp:FileUpload ID="customFile" CssClass="form-control form-control-sm" runat="server" />
                                        <%--<label class="custom-file-label" for="customFile">Choose File</label>--%>
                                        <asp:Label ID="lblfilename" runat="server" class="labelcolorl1" Text=""></asp:Label>
                                        <%--</div>--%>
                                    </div>
                                    <div class="col-md-6"></div>
                                    <div class="col-md-3 col-4">
                                        <a href="/SampleFiles/HolidayFormat.xlsx" target="_blank" download="Insert Format" class="  btn-sm btn btn-outline-secondary">Download Sample Excel
                                        </a>
                                    </div>
                                    <div class="col-md-2 col-4">
                                        <asp:Button ID="butttonsubmit" runat="server" Text="Import Excel" CssClass="btn btn-sm btn-outline-secondary" OnClick="butttonsubmit_Click" CausesValidation="false" />
                                    </div>
                                    <div class="col-md-2 col-4">
                                        <asp:Button ID="btnn" runat="server" Text="Insert Data" OnClick="btnn_Click" CssClass="btn btn-sm btn-outline-secondary" Visible="false" CausesValidation="false" />
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
                                        <div class="table-responsive table-container" style="width: 100%">
                                            <asp:GridView ID="gvExcelFile" runat="server" CssClass="table table-head-fixed text-nowrap data-table table-hover" AutoGenerateColumns="true">

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
        </ContentTemplate>
        <Triggers>

            <asp:PostBackTrigger ControlID="btnn" />
            <asp:PostBackTrigger ControlID="butttonsubmit" />
            <%--<asp:PostBackTrigger ControlID="btnInsertHoliday" />--%>
            <%--<asp:PostBackTrigger ControlID="btnUpdateHoliday" />--%>
            <asp:PostBackTrigger ControlID="gvHRRequest" />
            <asp:PostBackTrigger ControlID="ImgBtnExport" />


            <asp:PostBackTrigger ControlID="btnAddHoliday" />
            <asp:PostBackTrigger ControlID="btnimportUser" />
            <asp:PostBackTrigger ControlID="btnViewUsers" />
        </Triggers>
    </asp:UpdatePanel>
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


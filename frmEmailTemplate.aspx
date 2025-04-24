<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmEmailTemplate.aspx.cs" Inherits="frmEmailTemplate" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="assetsdata/plugins/summernote/jquery.js"></script>
    <link href="assetsdata/plugins/summernote/summernote-bs4.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js"></script>
    <script src="
https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.bundle.min.js"></script>
    <style>
        .note-editor {
            border: var(--bs-border-width) var(--bs-border-style) var(--bs-border-color) !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="scr" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="updatepanel1" runat="server">
        <ContentTemplate>
            

            <div class="card">
                <div class="card-body">
                    <div class=" row gy-3 gx-2">
                        <div class="col-md-4">
                            <label for="staticEmail" class="form-label">
                                Organization  

                                <asp:RequiredFieldValidator ID="rfvddlOrg" runat="server" ControlToValidate="ddlOrg" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="AddEmailTemp"></asp:RequiredFieldValidator>
                            </label>

                            <asp:DropDownList ID="ddlOrg" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlOrg_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-4">
                            <label for="staticEmail" class="form-label ">
                                Service Desk  
                                <asp:RequiredFieldValidator ID="RfvddlRequestType" runat="server" InitialValue="0" ControlToValidate="ddlRequestType" ValidationGroup="Addticket" ForeColor="Red" ErrorMessage="*"></asp:RequiredFieldValidator>
                            </label>

                            <asp:DropDownList ID="ddlRequestType" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlRequestType_SelectedIndexChanged"></asp:DropDownList>
                        </div>


                        <div class="col-md-4">
                            <label for="staticEmail" class="form-label">
                                Email Template 
							 <asp:LinkButton ID="ImgAddEmailTemp" runat="server" OnClick="ImgAddEmailTemp_Click"><i class="fa-solid fa-plus border small rounded-circle p-1"></i></asp:LinkButton>
                                <%--<asp:ImageButton ID="" runat="server" ImageUrl="~/Images/plus.png" OnClick="ImgAddEmailTemp_Click" />--%>
                                <%--<asp:LinkButton ID="LinkButton1" runat="server">LinkButton</asp:LinkButton>--%>
                                <asp:RequiredFieldValidator ID="rfvddlEmailTemplate" runat="server" ControlToValidate="ddlEmailTemplate" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="AddEmailTemp"></asp:RequiredFieldValidator>
                            </label>



                            <asp:TextBox ID="txtEmailTemplate" runat="server" CssClass="form-control form-control-sm" MaxLength="50" onkeypress="return /^[a-zA-Z_\s]*$/.test(event.key) && this.value.length < 50;"/>
                            <asp:DropDownList ID="ddlEmailTemplate" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlEmailTemplate_SelectedIndexChanged" CssClass="form-select form-select-sm single-select-optgroup-field"></asp:DropDownList>



                        </div>
                        <div class="col-md-12">

                            <label for="staticEmail" class="form-label">
                                <asp:Label ID="Label1" Text="Summary" runat="server"></asp:Label>

                                <asp:RequiredFieldValidator ID="rfvtxtSummary" runat="server" ControlToValidate="txtSummary" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="AddEmailTemp"></asp:RequiredFieldValidator>
                            </label>




                            <asp:TextBox ID="txtSummary" runat="server" CssClass="form-control  form-control-sm " MaxLength="1000"></asp:TextBox>


                        </div>
                        <div class="col-md-12">
                            <label for="staticEmail" class="form-label">
                                Email Body 
                                               <asp:RequiredFieldValidator ID="RfvtxtDescription" runat="server" ControlToValidate="txtDescription" ValidationGroup="AddEmailTemp" ForeColor="Red" ErrorMessage="*"></asp:RequiredFieldValidator>
                            </label>

                            <%--	<asp:TextBox ID="txtDescription" runat="server" CssClass="form-control form-control-sm" TextMode="MultiLine" MaxLength="500" Height="200px"></asp:TextBox>--%>
                            <%--							<asp:TextBox ID="txtDescription" runat="server" Rows="8" Columns="8" TextMode="MultiLine"></asp:TextBox>--%>
                            <textarea id="txtDescription" runat="server"></textarea>

                        </div>

                        <div class="col-md-12 text-end">
                            <asp:Button ID="btnSave" runat="server" Text="Submit" ValidationGroup="AddEmailTemp" OnClick="btnSave_Click" CssClass="btn btn-sm  btn-grd btn-grd-info  " />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" OnClick="btnCancel_Click" CssClass="btn btn-sm  btn-grd btn-grd-danger " />
                        </div>

                    </div>
                </div>
            </div>
            </div>
            </div>
        </ContentTemplate>

        <Triggers>

            <asp:PostBackTrigger ControlID="ddlEmailTemplate" />
            <asp:PostBackTrigger ControlID="ddlOrg" />
            <asp:PostBackTrigger ControlID="ImgAddEmailTemp" />
            <asp:PostBackTrigger ControlID="ddlRequestType" />
            <asp:PostBackTrigger ControlID="btnCancel" />

        </Triggers>

    </asp:UpdatePanel>
    <%--</div>--%>
    <script type="text/javascript">
        function Showalert(imtype, imtitle) {
            var Toast = Swal.mixin({
                toast: true,
                position: 'top-middle',

                showConfirmButton: false,
                showCloseButton: true,
                timer: 4000,


            });
            console.log("hello");
            Toast.fire({
                icon: imtype,
                title: imtitle
            });

            console.log("fire1234567");
        }
    </script>
    <script src="assetsdata/plugins/summernote/summernote-bs4.js"></script>
    <script>
        $(document).ready(function () {

            $('#<%= txtDescription.ClientID %>').summernote();
        });
    </script>
</asp:Content>


<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmChangeOrgLogo.aspx.cs" Inherits="frmChangeOrgLogo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="row gy-1 gx-3">
        <div class="col-md-7" style="height: 80vh !important">
            <div class="card rounded-4 mb-1">
                <div class="card-body">

                    <div class="row mt-2 gy-3">
                        <div class="col-md-4">
                            <label class="form-label">
                                Organisation 
                            <asp:RequiredFieldValidator ID="RfvOrganisation" runat="server" InitialValue="0" ControlToValidate="ddlOrg" ErrorMessage="*" ForeColor="Red" ValidationGroup="SaveForm"></asp:RequiredFieldValidator>
                            </label>
                        </div>
                        <div class="col-md-8">
                            <asp:DropDownList ID="ddlOrg" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlOrg_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-4">
                            <label class="form-label">
                                Choose Photo

                                <asp:RequiredFieldValidator ID="rfvFileUpload1" runat="server" ControlToValidate="FileUpload1" ErrorMessage="*" ForeColor="Red" ValidationGroup="ImgUpload"></asp:RequiredFieldValidator>
                            </label>
                        </div>
                        <div class="col-md-8">
                            <asp:FileUpload ID="FileUpload1" CssClass="form-control-sm form-control" runat="server" />
                            <asp:RegularExpressionValidator ID="regFileUpload1" runat="server" ForeColor="Red" ErrorMessage="Only .jpeg or .JPEG or .gif or .GIF or .png or .PNG" ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))+(.jpg|.jpeg|.JPEG|.gif|.GIF|.png|.PNG)$" ControlToValidate="FileUpload1" ValidationGroup="ImgUpload">
                            </asp:RegularExpressionValidator>
                        </div>
                        <div class="col-md-12 text-end mt-1">
                            <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" ValidationGroup="ImgUpload" CssClass="btn btn-sm btn-grd-primary" />

                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:DetailsView ID="DetailsCheckInAsset" runat="server" CssClass="data-table table table-striped border table-sm text-nowrap dataTable no-footer"
                                AllowPaging="True"
                                AutoGenerateRows="False"
                                GridLines="None">
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <CommandRowStyle BackColor="#E2DED6" Font-Bold="True" />
                                <EditRowStyle BackColor="#999999" />
                                <FieldHeaderStyle BackColor="#E9ECF1" Font-Bold="True" />
                                <Fields>
                                    <asp:BoundField DataField="EmpID" HeaderText="EmpID" />
                                    <asp:BoundField DataField="UserName" HeaderText="UserName" />
                                    <asp:BoundField DataField="EmailID" HeaderText="EmailID" />
                                    <asp:BoundField DataField="Frm_UID" HeaderText="Login ID" />
                                    <asp:BoundField DataField="UserType" HeaderText="User Scope" />
                                    <asp:BoundField DataField="ContactNo" HeaderText="ContactNo" />
                                    <asp:BoundField DataField="Status" HeaderText="Status" />
                                    <asp:BoundField DataField="DomainType" HeaderText="Domain Type" />

                                </Fields>


                            </asp:DetailsView>
                        </div>
                    </div>
                </div>




            </div>
        </div>
        <div class="col-md-5" style="height: 80vh !important">
            <div class="card rounded-4  mb-1">
                <div class="card-body">
                    <div class="widget-user-image text-center">
                        <asp:Image ID="img1" runat="server" Width="400" ImageAlign="Middle" Height="90" CssClass="img-circle elevation-2" />
                    </div>

                </div>
            </div>
        </div>
    </div>
</asp:Content>


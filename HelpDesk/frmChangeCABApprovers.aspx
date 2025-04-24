<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmChangeCABApprovers.aspx.cs" Inherits="HelpDesk_frmChangeCABApprovers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="scriptmanager1" runat="server"></asp:ScriptManager>

    <asp:UpdatePanel ID="updatepanel1" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-md-4">
                    <asp:Button ID="btnAddUserEcslevel" Text="Add CAB Approval" runat="server" CssClass="btn btn-sm btnDisabled" OnClick="btnAddUserEcslevel_Click" />
                    <asp:Button ID="btnViewEcslevel" runat="server" Text-="View Details" CssClass="btn btn-sm btnEnabled" OnClick="btnViewEcslevel_Click" />
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="card">
                        <div class="card-body">
                            <asp:Panel ID="pnlAddEcslevel" runat="server" Visible="false">
                                <div class="form-group row mt-3">
                                    <label for="staticEmail" class="col-sm-2 labelcolorl1 pl-5">
                                        Approval Level : <span title="*"></span>
                                        <asp:RequiredFieldValidator ID="rfvddlCABLevel" runat="server" ControlToValidate="ddlCABLevel" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="UserEcslevel"></asp:RequiredFieldValidator>
                                    </label>


                                    <div class="col-sm-4 pr-5">

                                        <asp:DropDownList ID="ddlCABLevel" runat="server" CssClass="form-control  form-control-sm">
                                            <asp:ListItem Text="L1" Value="L1"></asp:ListItem>
                                            <asp:ListItem Text="L2" Value="L2"></asp:ListItem>
                                            <asp:ListItem Text="L3" Value="L3"></asp:ListItem>
                                            <asp:ListItem Text="L4" Value="L4"></asp:ListItem>
                                            <asp:ListItem Text="L5" Value="L5"></asp:ListItem>
                                        </asp:DropDownList>

                                    </div>
                                    <label for="staticEmail" class="col-sm-2 labelcolorl1 pl-5">
                                        User Name: <span title="*"></span>
                                        <asp:RequiredFieldValidator ID="rfvtxtUserName" runat="server" ControlToValidate="txtUserName" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="UserEcslevel"></asp:RequiredFieldValidator>
                                    </label>
                                    <div class="col-sm-4 pr-5">
                                        <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control  form-control-sm ">

										

                                        </asp:TextBox>

                                    </div>


                                </div>
                                <div class="form-group row mt-3">
                                    <label for="staticEmail" class="col-sm-2 labelcolorl1 pl-5">
                                        Email : <span title="*"></span>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail"
                                            ForeColor="Red" ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
                                            Display="Dynamic" ErrorMessage="Invalid Email" ValidationGroup="UserEcslevel" />

                                        <asp:RequiredFieldValidator ID="rfvtxtEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="UserEcslevel"></asp:RequiredFieldValidator>
                                    </label>


                                    <div class="col-sm-4 pr-5">
                                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control  form-control-sm ">

										

                                        </asp:TextBox>
                                    </div>
                                    <label for="staticEmail" class="col-sm-2 labelcolorl1 pl-5">
                                        Mobile: <span title="*"></span>
                                        <asp:RegularExpressionValidator ID="rvPhoneNumber" runat="server"
                                            ControlToValidate="txtMobile"
                                            ValidationExpression="^(?:(?:\+?\d{1,3}[-.\s]?)?\(?(?:\d{3})?\)?[-.\s]?\d{3}[-.\s]?\d{4})|(?:(?:\+?\d{1,3}[-.\s]?)?\(?(?:\d{2,4})?\)?[-.\s]?\d{6,8})$"
                                            ErrorMessage="Invalid Number" ForeColor="Red"
                                            Display="Dynamic" ValidationGroup="UserEcslevel">
                                        </asp:RegularExpressionValidator>
                                        <asp:RequiredFieldValidator ID="rfvtxtMobile" runat="server" ControlToValidate="txtMobile" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="UserEcslevel"></asp:RequiredFieldValidator>
                                    </label>
                                    <div class="col-sm-4 pr-5">
                                        <asp:TextBox ID="txtMobile" TextMode="Phone" runat="server" CssClass="form-control  form-control-sm ">
                                        </asp:TextBox>
                                    </div>
                                </div>
                                <div class="form-group row mt-3">

                                    <label for="staticEmail" class="col-sm-2 labelcolorl1 pl-5">
                                        Organization: <span title="*"></span>

                                        <asp:RequiredFieldValidator ID="rfvddlOrg" runat="server" ControlToValidate="ddlOrg" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="ReqType"></asp:RequiredFieldValidator>
                                    </label>

                                    <div class="col-sm-4 pr-5">

                                        <asp:DropDownList ID="ddlOrg" runat="server" CssClass="form-control form-control-sm chzn-select">
                                        </asp:DropDownList>
                                    </div>
                                    <label for="staticEmail" class="col-sm-2 labelcolorl1 pl-5">
                                        Active: <span title="*"></span>

                                        <asp:RequiredFieldValidator ID="rfvddlIsActive" runat="server" ControlToValidate="ddlIsActive" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="ReqType"></asp:RequiredFieldValidator>
                                    </label>

                                    <div class="col-sm-4 pr-5">

                                        <asp:DropDownList ID="ddlIsActive" runat="server" CssClass="form-control form-control-sm chzn-select">
                                            <asp:ListItem Selected="True" Text="True" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="False" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>

                                <div class="form-row">
                                    <div class="col-md-3 offset-5 ">
                                        <asp:Button ID="btnInsertEcslevel" runat="server" Text="Save" class="btn btn-sm savebtn" OnClick="btnInsertEcslevel_Click" ValidationGroup="UserEcslevel" />
                                        <asp:Button ID="btnUpdateEcslevel" runat="server" Text="Update" Visible="false" class="btn btn-sm savebtn" OnClick="btnUpdateEcslevel_Click" ValidationGroup="UserEcslevel" />
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" class="btn btn-sm cancelbtn" OnClick="btnCancel_Click" CausesValidation="false" />
                                    </div>
                                </div>

                            </asp:Panel>
                            <asp:Panel ID="pnlViewEcslevel" runat="server">
                                <div class="row">
                                    <div class="col-lg-12 col-md-8 col-sm-6 graphs">
                                        <div class="xs">
                                            <div class="well1 white">
                                                <div class="card card-default">

                                                    <div class="card-body">
                                                        <div class="row ">
                                                            <div class="col-md-4">

                                                                <asp:Label ID="lblsofname" runat="server" Text="CAB Details" Font-Size="Larger" ForeColor="Black"></asp:Label>

                                                            </div>
                                                            <div class="col-md-6">
                                                                <asp:Label ID="Label1" runat="server"></asp:Label>
                                                                <asp:Label ID="Label3" runat="server"></asp:Label>
                                                            </div>
                                                            <div class="col-md-2 ">
                                                                <div class="btn btn-sm elevation-1 ml-1 " style="padding: 0px; margin-bottom: 10px; padding-top: 1px">
                                                                    <label class="mr-2 ml-1 mb-0">Export</label>
                                                                    <asp:ImageButton ID="ImgBtnExport" runat="server" ImageUrl="~/Images/New folder/excelnew.png" CssClass="fa-pull-right btn-outline-success mr-1" OnClick="ImgBtnExport_Click" />
                                                                </div>
                                                            </div>

                                                        </div>
                                                        <div class="table-responsive p-0" style="height: 400px; width: 100%">
                                                            <asp:GridView GridLines="None" ID="gvEcslevel" runat="server" DataKeyNames="ID" AutoGenerateColumns="false" CssClass="data-table table-head-fixed text-nowrap"
                                                                Width="100%" OnRowCommand="gvEcslevel_RowCommand" OnRowDataBound="gvEcslevel_RowDataBound">
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
                                                                    <asp:ButtonField ButtonType="Image" CommandName="UpdateEcslevel" HeaderText="Edit" ImageUrl="~/images/edit23.png" ItemStyle-Width="20px" />
                                                                    <asp:ButtonField HeaderText="Delete" ButtonType="Image" ImageUrl="~/Images/New folder/delnew.png" CommandName="DeleteEcslevel" ItemStyle-Width="20px" ItemStyle-Height="5px" />
                                                                </Columns>
                                                            </asp:GridView>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>

        </ContentTemplate>

        <Triggers>

            <asp:PostBackTrigger ControlID="btnAddUserEcslevel" />
            <asp:PostBackTrigger ControlID="btnViewEcslevel" />
            <asp:PostBackTrigger ControlID="ImgBtnExport" />
            <asp:PostBackTrigger ControlID="gvEcslevel" />
        </Triggers>

    </asp:UpdatePanel>
</asp:Content>


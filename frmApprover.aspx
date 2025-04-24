<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmApprover.aspx.cs" Inherits="frmApprover" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title></title>
    <script src="assetsdata/plugins/summernote/jquery.js"></script>
    <link href="assetsdata/plugins/summernote/summernote-bs4.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.bundle.min.js"></script>

    <link href="https://pcv-demo.hitachi-systems-mc.com:5723/assetsdata/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css2?family=Noto+Sans:wght@300;400;500;600&amp;display=swap" rel="stylesheet" />
    <link href="https://fonts.googleapis.com/css?family=Material+Icons+Outlined" rel="stylesheet" />
    <link href="https://pcv-demo.hitachi-systems-mc.com:5723/sassdata/main.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" />
    <link href="https://pcv-demo.hitachi-systems-mc.com:5723/assetsdata/plugins/select2/css/select2-bootstrap-5-theme.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet" />

    <style>
        .back-button {
            position: fixed;
            top: 20px;
            right: -1px;
            background-color: #007bff;
            color: white;
            border: none;
            padding: 10px 15px;
            font-size: 13px;
            cursor: pointer;
            z-index: 1000;
            border-radius: 2rem 0 0 2rem;
        }

            .back-button:hover {
                background-color: #0056b3;
            }

        .note-editor {
            border: var(--bs-border-width) var(--bs-border-style) var(--bs-border-color) !important;
        }

        .form-label {
            display: inline-block;
            margin-bottom: .3rem !important;
            color: #414141;
            font-size: small;
            font-weight: 400;
        }

        table td {
            color: #545454 !important;
        }
          .centeredImage {
        position: fixed; /* Keeps the image in the same position even if scrolling */
        top: 50%; /* Centers vertically */
        left: 50%; /* Centers horizontally */
        transform: translate(-50%, -50%); /* Adjusts to perfectly center the image */
        opacity: 0.5; /* Sets 50% opacity */
        z-index: 1000; /* Ensures the image stays on top */
    }
    </style>
</head>
<body style="background-image: url(Asset/img/bg005.jpg); height: auto; background-size: cover; background-repeat: round;">
    <form id="form1" runat="server">

        <asp:Panel ID="pnl" runat="server">
<asp:Image 
    ID="img" 
    runat="server" 
    Width="200px" 
    Height="150px" 
    ImageUrl="~/Images/check.png" 
    CssClass="centeredImage" 
    Visible="false" 
/>
            <div class="container-fluid">
                <div class="card mt-3 mb-0 pb-0">
                    <div class="card-header">
                        <h6 class="card-title mb-0 bg-light">Ticket Details</h6>
                    </div>
                    <div class="card-body pb-0">
                        <div class="row gy-2 gx-3">
                            <div class="col-md-12">
                                <asp:Panel ID="pnlShowGriddAtawithoutChange" runat="server">
                                    <asp:DetailsView ID="DetailsCheckInAsset" runat="server" CssClass="table table-bordered table-sm bg-danger"
                                        AllowPaging="True"
                                        AutoGenerateRows="False"
                                        GridLines="None">

                                        <Fields>
                                            <asp:BoundField DataField="TicketNumber" HeaderText="TicketNumber" />
                                            <asp:BoundField DataField="Summary" HeaderText="Summary" />
                                            <asp:BoundField DataField="Category" HeaderText="Category" />

                                            <asp:BoundField DataField="Priority" HeaderText="Priority :" />
                                            <asp:BoundField DataField="Severity" HeaderText="Severity" />
                                            <asp:BoundField DataField="SubmitterName" HeaderText="SubmitterName" />
                                            <asp:BoundField DataField="SubmitterEmail" HeaderText="SubmitterEmail" />

                                            <asp:BoundField DataField="CreationDate" HeaderText="CreationDate" DataFormatString="{0:yyyy-MM-dd}" />
   <asp:TemplateField HeaderText="Description" SortExpression="Description">

                                            <ItemTemplate>
                                                <asp:Label ID="lblDescription" runat="server" Text='<%# Server.HtmlDecode(Eval("Description").ToString()) %>'> </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        </Fields>
                                        <%-- <RowStyle BackColor="White" BorderColor="#e3e4e6" BorderWidth="1px" Height="5px" />
                                        <FooterStyle BackColor="#EDEDED" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#EDEDED" ForeColor="#000000" HorizontalAlign="Left" Height="5px" VerticalAlign="NotSet" CssClass="header" />
                                        <HeaderStyle BackColor="#EEEEEE" Font-Bold="True" ForeColor="#414141" Height="5px" BorderColor="WhiteSmoke" CssClass="header sorting_asc" Font-Size="Small" />
                                        <EditRowStyle BackColor="#e9edf2" BorderColor="#e3e4e6" BorderStyle="Solid" BorderWidth="1px" Height="5px" />
                                        <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#EDEDED" />--%>
                                        <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
                                        <%--<AlternatingRowStyle BackColor="#EAEEFF" BorderColor="#e3e4e6" Height="5px" BorderStyle="Solid" BorderWidth="1px" />--%>
                                    </asp:DetailsView>
                                </asp:Panel>
                                <asp:Panel ID="pnlShowGridDataWithChange" runat="server">
                                    <asp:DetailsView ID="DetailsChange" runat="server" CssClass="table table-bordered"
                                        AllowPaging="True"
                                        AutoGenerateRows="False"
                                        GridLines="None">

                                        <Fields>
                                            <asp:BoundField DataField="TicketNumber" HeaderText="TicketNumber" NullDisplayText="NA" />
                                            <asp:BoundField DataField="Summary" HeaderText="Summary" NullDisplayText="NA" />
                                            <asp:BoundField DataField="Category" HeaderText="Category" NullDisplayText="NA" />
                                            <asp:BoundField DataField="Priority" HeaderText="Priority :" NullDisplayText="NA" />
                                            <asp:BoundField DataField="Severity" HeaderText="Severity" NullDisplayText="NA" />
                                            <asp:BoundField DataField="SubmitterName" HeaderText="SubmitterName" NullDisplayText="NA" />
                                            <asp:BoundField DataField="SubmitterEmail" HeaderText="SubmitterEmail" NullDisplayText="NA" />
                                            <asp:BoundField DataField="CreationDate" HeaderText="Ticket Creation" DataFormatString="{0:yyyy-MM-dd}" />

                                            <asp:BoundField DataField="ChangeType" HeaderText="ChangeType" NullDisplayText="NA" />
                                            <asp:BoundField DataField="RequestForChange" HeaderText="RequestForChange" NullDisplayText="NA" />
                                            <asp:BoundField DataField="DurationFrom" HeaderText="DurationFrom" DataFormatString="{0:yyyy-MM-dd}" />
                                            <asp:BoundField DataField="DurationTo" HeaderText="DurationTo" DataFormatString="{0:yyyy-MM-dd}" />

                                            <asp:BoundField DataField="DownTimeDesc" HeaderText="DownTime Description" NullDisplayText="NA" />
                                            <asp:BoundField DataField="DownTimeFrom" HeaderText="DownTimeFrom" DataFormatString="{0:yyyy-MM-dd}" />
                                            <asp:BoundField DataField="DownTimeTo" HeaderText="DownTimeTo" DataFormatString="{0:yyyy-MM-dd}" />
                                        </Fields>
                                        <%-- <RowStyle BackColor="White" BorderColor="#e3e4e6" BorderWidth="1px" Height="5px" />
                                        <FooterStyle BackColor="#EDEDED" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#EDEDED" ForeColor="#000000" HorizontalAlign="Left" Height="5px" VerticalAlign="NotSet" CssClass="header" />
                                        <HeaderStyle BackColor="#EEEEEE" Font-Bold="True" ForeColor="#414141" Height="5px" BorderColor="WhiteSmoke" CssClass="header sorting_asc" Font-Size="Small" />
                                        <EditRowStyle BackColor="#e9edf2" BorderColor="#e3e4e6" BorderStyle="Solid" BorderWidth="1px" Height="5px" />
                                        <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#EDEDED" />--%>
                                        <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
                                        <%--<AlternatingRowStyle BackColor="#EAEEFF" BorderColor="#e3e4e6" Height="5px" BorderStyle="Solid" BorderWidth="1px" />--%>
                                    </asp:DetailsView>
                                </asp:Panel>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label ">Current Stage  </label>
                                <asp:DropDownList ID="ddlStage" CssClass="form-select form-select-sm single-select-optgroup-field
"
                                    runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlStage_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-6">
                                <label class="form-label ">Current Status  </label>
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field
">
                                </asp:DropDownList>
                            </div>

                            <div class="col-md-12">
                                <asp:Label ID="lblHodApprov" runat="server" class="form-label ">HOD Approval Required : 

																 <asp:RequiredFieldValidator ID="rfvrdblst" runat="server" ControlToValidate="rdblst" ValidationGroup="Store" ForeColor="Red" ErrorMessage="*"></asp:RequiredFieldValidator>

                                </asp:Label>
                                <asp:RadioButtonList ID="rdblst" runat="server" RepeatDirection="Horizontal">

                                    <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
<div class="col-md-12">
								<div class="card">
									<div class="card-header">
										<div class="card-tittle">
											<asp:Label ID="Label14" runat="server" Text="Ticket Attachments" Font-Size="Medium" ForeColor="Black"></asp:Label>
										</div>
									</div>
									<div class="card-body">

										<div class="table-responsive table-container" style="color:white">
											<asp:GridView ID="gvTicketAttachment" runat="server" AutoGenerateColumns="false" DataKeyNames="Ticketref" EmptyDataText="No files uploaded" CssClass="table1 table-bordered" OnRowCommand="gvTicketAttachment_RowCommand">
												<Columns>

													<asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="20">
														<ItemTemplate>
															<%#Container.DataItemIndex+1 %>
														</ItemTemplate>
													</asp:TemplateField>
													<asp:BoundField DataField="EntryDt" HeaderText="Event Time"
														SortExpression="EntryDt" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" />
													<asp:BoundField DataField="Ticketref" HeaderText="Ticket Number"
														SortExpression="Ticketref" />
													<asp:TemplateField HeaderText="Files">
														<ItemTemplate>
															<asp:LinkButton ID="lnkView" runat="server" Text="View" CausesValidation="False" CommandName="View" CommandArgument='<%# Eval("Filepath") %>'>
                                   
															</asp:LinkButton>
															<asp:HiddenField ID="hdnfullPath" runat="server" Value='<%# Eval("Filepath") %>' />
														</ItemTemplate>
													</asp:TemplateField>

													<asp:TemplateField HeaderText="Download">
														<ItemTemplate>
															<asp:LinkButton ID="lnkDownload" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Filepath") %>'
																CommandName="Download" Text='<%# Eval("Filename") %>' />
														</ItemTemplate>
													</asp:TemplateField>

												</Columns>
												
											</asp:GridView>
										</div>
									</div>
								</div>
							</div>
                            <div class="col-md-12">
                                <label class="form-label ">
                                    Remarks
																				<asp:RequiredFieldValidator ID="efvtxtremarks" runat="server" ControlToValidate="txtremarks" ValidationGroup="Store" ForeColor="Red" ErrorMessage="*"></asp:RequiredFieldValidator>

                                </label>
                                &nbsp;<asp:TextBox ID="txtremarks" runat="server" class="form-control form-control-sm" TextMode="MultiLine" Height="60px"></asp:TextBox>
                            </div>
                            <div class="col-md-12 text-end">
                                <asp:Button ID="btnApprove" runat="server" Text="Approve" CssClass="btn btn-sm btn-primary" OnClick="btnApprove_Click" ValidationGroup="Store" />
                                <asp:Button ID="btnReject" runat="server" Text="Reject" CssClass="btn btn-sm btn-danger" OnClick="btnReject_Click" ValidationGroup="Store" />
                            </div>
                            <div class="col-md-6">

                                <div class="form-group">
                                    <asp:Label ID="Label1" runat="server" Text="" CssClass="label" ForeColor="#ff0000"></asp:Label>
                                    <div class="row">
                                        <div class="col-md-12">
                                        </div>
                                    </div>
                                </div>



                            </div>
                            <div class="clearfix">
                                <asp:Label ID="lblerrorMsg" runat="server" Text=""></asp:Label>
                            </div>
                        </div>

                        <asp:Panel ID="pnlchangeparameters" runat="server">
                            <%--        ________________________ Grid will show Impact Plan   _____________________________--%>

                            <div class="row mt-4 " style="border-bottom: solid 1px">
                                <div class="col-md-4">

                                    <asp:Label ID="Label4" runat="server" Text="Impact Details" Font-Size="Larger" ForeColor="Black"></asp:Label>

                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">

                                    <asp:GridView ID="gvImpactGrid" runat="server" CssClass="table table-head-fixed text-nowrap  " DataKeyNames="ID"
                                        AutoGenerateColumns="false" AllowSorting="True">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="20">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderStyle-Width="120px" HeaderText="ImpactDetails" DataField="ImpactDescription" />
                                        </Columns>
                                        <%--<RowStyle BackColor="White" BorderColor="#e3e4e6" BorderWidth="1px" Height="5px" />
                                        <FooterStyle BackColor="#EDEDED" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#EDEDED" ForeColor="#000000" HorizontalAlign="Left" Height="20px" VerticalAlign="NotSet" CssClass="header" />
                                        <SelectedRowStyle BackColor="#fff" Font-Bold="True" ForeColor="#000000" Height="5px" />
                                        <HeaderStyle BackColor="#EEEEEE" Font-Bold="True" ForeColor="#414141" Height="5px" BorderColor="WhiteSmoke" CssClass="header sorting_asc" Font-Size="Small" />
                                        <EditRowStyle BackColor="#e9edf2" BorderColor="#e3e4e6" BorderStyle="Solid" BorderWidth="1px" Height="5px" />
                                        <EmptyDataRowStyle HorizontalAlign="Center" BorderStyle="None" Height="5px" BorderColor="#EDEDED" BackColor="#EDEDED" />--%>
                                        <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
                                        <%--<AlternatingRowStyle BackColor="#EAEEFF" BorderColor="#e3e4e6" Height="5px" BorderStyle="Solid" BorderWidth="1px" />--%>
                                    </asp:GridView>

                                </div>
                            </div>
                            <%--        ________________________ Grid will show RollOut Plan   _____________________________--%>

                            <div class="row mt-4 " style="border-bottom: solid 1px">
                                <div class="col-md-4">

                                    <asp:Label ID="Label2" runat="server" Text="Roll Out Details" Font-Size="Larger" ForeColor="Black"></asp:Label>

                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:GridView ID="gvRollOutDetails" runat="server" CssClass="table table-head-fixed text-nowrap  " DataKeyNames="ID"
                                        AutoGenerateColumns="false" AllowSorting="True">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="20">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderStyle-Width="120px" HeaderText="RollOutDetails" DataField="RolloutDescription" />
                                        </Columns>
                                        <%-- <RowStyle BackColor="White" BorderColor="#e3e4e6" BorderWidth="1px" Height="5px" />
                                        <FooterStyle BackColor="#EDEDED" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#EDEDED" ForeColor="#000000" HorizontalAlign="Left" Height="20px" VerticalAlign="NotSet" CssClass="header" />
                                        <SelectedRowStyle BackColor="#fff" Font-Bold="True" ForeColor="#000000" Height="5px" />
                                        <HeaderStyle BackColor="#EEEEEE" Font-Bold="True" ForeColor="#414141" Height="5px" BorderColor="WhiteSmoke" CssClass="header sorting_asc" Font-Size="Small" />
                                        <EditRowStyle BackColor="#e9edf2" BorderColor="#e3e4e6" BorderStyle="Solid" BorderWidth="1px" Height="5px" />
                                        <EmptyDataRowStyle HorizontalAlign="Center" BorderStyle="None" Height="5px" BorderColor="#EDEDED" BackColor="#EDEDED" />--%>
                                        <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
                                        <%--<AlternatingRowStyle BackColor="#EAEEFF" BorderColor="#e3e4e6" Height="5px" BorderStyle="Solid" BorderWidth="1px" />--%>
                                    </asp:GridView>
                                </div>
                            </div>


                            <%--        ________________________ Grid will show Task Plan   _____________________________--%>

                            <div class="row mt-4" style="border-bottom: solid 1px">
                                <div class="col-md-4">

                                    <asp:Label ID="Label3" runat="server" Text="Task Association Details" Font-Size="Larger" ForeColor="Black"></asp:Label>

                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <div class="table-responsive p-0" style="height: 400px">

                                        <asp:GridView ID="gvTaskDetails" runat="server" CssClass="table table-head-fixed text-nowrap  " DataKeyNames="ID"
                                            AutoGenerateColumns="false" AllowSorting="True">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="20">
                                                    <ItemTemplate>
                                                        <%#Container.DataItemIndex+1 %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderStyle-Width="120px" HeaderText="Task ID" DataField="SubTicketRef" />

                                                <asp:BoundField HeaderStyle-Width="120px" HeaderText="Task Description" DataField="TaskDesc" />
                                                <asp:BoundField HeaderStyle-Width="120px" HeaderText="Status" DataField="Status" />
                                                <asp:BoundField HeaderStyle-Width="120px" HeaderText="Engineer Association" DataField="Assignee" />

                                            </Columns>
                                            <%--<RowStyle BackColor="White" BorderColor="#e3e4e6" BorderWidth="1px" Height="5px" />
                                            <FooterStyle BackColor="#EDEDED" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#EDEDED" ForeColor="#000000" HorizontalAlign="Left" Height="20px" VerticalAlign="NotSet" CssClass="header" />
                                            <SelectedRowStyle BackColor="#fff" Font-Bold="True" ForeColor="#000000" Height="5px" />
                                            <HeaderStyle BackColor="#EEEEEE" Font-Bold="True" ForeColor="#414141" Height="5px" BorderColor="WhiteSmoke" CssClass="header sorting_asc" Font-Size="Small" />
                                            <EditRowStyle BackColor="#e9edf2" BorderColor="#e3e4e6" BorderStyle="Solid" BorderWidth="1px" Height="5px" />
                                            <EmptyDataRowStyle HorizontalAlign="Center" BorderStyle="None" Height="5px" BorderColor="#EDEDED" BackColor="#EDEDED" />--%>
                                            <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
                                            <%--<AlternatingRowStyle BackColor="#EAEEFF" BorderColor="#e3e4e6" Height="5px" BorderStyle="Solid" BorderWidth="1px" />--%>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>

                    </div>
                </div>
            </div>

        </asp:Panel>

    </form>
</body>
</html>

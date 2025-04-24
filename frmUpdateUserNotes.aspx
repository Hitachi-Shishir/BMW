<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="frmUpdateUserNotes.aspx.cs" Inherits="frmUpdateUserNotes" ValidateRequest="false" %>

<!DOCTYPE html>

<%--<html xmlns="http://www.w3.org/1999/xhtml">--%>

<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title></title>
    <script src="assetsdata/plugins/summernote/jquery.js"></script>
    <link href="assetsdata/plugins/summernote/summernote-bs4.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.bundle.min.js"></script>

    <link href="https://pcv-demo.hitachi-systems-mc.com:5723/assetsdata/css/bootstrap.min.css" rel="stylesheet">
    <link href="https://fonts.googleapis.com/css2?family=Noto+Sans:wght@300;400;500;600&amp;display=swap" rel="stylesheet">
    <link href="https://fonts.googleapis.com/css?family=Material+Icons+Outlined" rel="stylesheet">
    <link href="https://pcv-demo.hitachi-systems-mc.com:5723/sassdata/main.css" rel="stylesheet">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css">
    <link href="https://pcv-demo.hitachi-systems-mc.com:5723/assetsdata/plugins/select2/css/select2-bootstrap-5-theme.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet">

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
color: #3d3d3d!important;
}
th{

color:white! important;
}
    </style>
</head>
<body style="background-image: url(Asset/img/bg005.jpg); height: auto; background-size: cover; background-repeat: round;">
    <form id="form1" runat="server">
  

<%--	<asp:ScriptManager ID="scr" runat="server"></asp:ScriptManager>
	<asp:UpdatePanel ID="updatepanel1" runat="server">
		<ContentTemplate>--%>
			<div class="row mb-1">
				<div class="col-md-6 col-lg-12 col-sm-4">
					<div class="card card-default">
					
						<div class="card-body">
							<div class="row">
								<div class="col-md-6 col-lg-12 col-sm-4">
									<label class="cardheader">Update Ticket</label>
									<asp:Label ID="lblTicket" class="cardheader " runat="server" Text=""></asp:Label>
									<asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Red"></asp:Label>
								</div>
							</div>
							<div class="row">
								<div class="col-md-11">
									<asp:Button ID="btnUpdateTickView" Text="View Tickets" runat="server" CssClass="btn btn-sm btn-outline-secondary " OnClick="btnUpdateTickView_Click" />
							
									<asp:Button ID="btnViewNotes" runat="server" Text-="View Notes" CssClass="btn btn-sm btn-outline-secondary" OnClick="btnViewNotes_Click" />

								</div>
									<div class="col-md-1">
								<asp:ImageButton ID="ImgbtnBack" runat="server" Width="40px" ImageUrl="~/Images/leftarrow.png" OnClick="ImgbtnBack_Click" />
				</div>
							</div>
					

							<div style="overflow: scroll; height: 500px">

								<asp:Panel ID="pnlTicket" runat="server">



									<div class="row mt-2">
										<label for="staticEmail" class="col-sm-2 form-label pl-4">
											Summary : 
										</label>
										<div class="col-sm-10 pr-5">
											<asp:TextBox ID="txtSummary" runat="server" ReadOnly="true" CssClass="form-select form-select-sm"></asp:TextBox>
										</div>
									</div>
									<div class=" row mt-2 ">
										<label for="staticEmail" class="col-sm-2 form-label pl-4">
											Submitter Name:
										</label>
										<div class="col-sm-4 pr-5">
											<asp:TextBox ID="txtSubmitterName" ReadOnly="true" runat="server" CssClass="form-select form-select-sm"></asp:TextBox>
										</div>
										<label for="staticEmail" class="col-sm-2 form-label pl-1">
											Submitter Email : 
										</label>
										<div class="col-sm-4 pr-5">
											<asp:TextBox ID="txtSubmitterEmail" ReadOnly="true" runat="server" CssClass="form-select form-select-sm"></asp:TextBox>
										</div>
									</div>



									<div class=" row mt -2 ">
										<%-- this is area for Category--%>
										<div class="col-lg-6 col-md-8 col-sm-12">


											<div class="row mt-2">
												<div class="col-4">
													<label for="staticEmail" class=" form-label pl-3">
														Attach File :
                           <%--<asp:Label ID="lblinvoiceupload" runat="server" Text=""></asp:Label>--%>
													</label>
												</div>
												<div class="col-8 pr-5">
													<div class="input-group flex-nowrap">
														<asp:FileUpload ID="FileUploadTickDoc" AllowMultiple="true" runat="server" CssClass="form-control form-control-sm p-0" ToolTip="Select Only Pdf,Excel File" />

														<asp:LinkButton ID="btnUpload" CssClass="input-group-text" runat="server" OnClick="btnUpload_Click"><i class="fa fa-upload"></i></asp:LinkButton>
													</div>

												</div>
											</div>
											<div class="col-md-8 offset-4">
												<asp:GridView ID="grd" runat="server" class="table table-head-fixed text-nowrap table-sm table-striped mt-2 table-bordered" AutoGenerateColumns="false">
													<Columns>
														<asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="20">
															<ItemTemplate>
																<%# Container.DataItemIndex + 1 %>
															</ItemTemplate>
														</asp:TemplateField>
														<asp:TemplateField HeaderText="Files">
															<ItemTemplate>
																<asp:LinkButton ID="lnkView" runat="server" OnClick="lnkView_Click">
																	<asp:Label ID="lblfileName" class="text-dark" runat="server" Text='<%# Eval("fileName") %>'></asp:Label>
																</asp:LinkButton>
																<asp:HiddenField ID="hdnfullPath" runat="server" Value='<%# Eval("fullPath") %>' />
															</ItemTemplate>
														</asp:TemplateField>
														<asp:TemplateField HeaderText="Delete File">
															<ItemTemplate>
																<asp:LinkButton ID="lnkbtnDelete" runat="server" OnClick="lnkbtnDelete_Click"
																	OnClientClick="return confirm('Are you sure you want to Delete this File?');">
                                       <i class="fa fa-trash text-danger"></i>
																</asp:LinkButton>
																<asp:HiddenField ID="hdnid" runat="server" Value='<%# Eval("ID") %>' />
															</ItemTemplate>
														</asp:TemplateField>
													</Columns>
																<RowStyle BackColor="White" BorderColor="#e3e4e6" BorderWidth="1px" Font-Size="X-Small" Height="10px" />
												<FooterStyle BackColor="#EDEDED" Font-Bold="True" ForeColor="White" />
												<PagerStyle BackColor="#EDEDED" ForeColor="#000000" HorizontalAlign="Center" Font-Size="X-Small" />
												<SelectedRowStyle BackColor="#fff" Font-Bold="True" ForeColor="#000000" Font-Size="X-Small" />
												<HeaderStyle BackColor="#EEEEEE" Font-Bold="True" ForeColor="#414141" Height="30px" CssClass="header" Font-Size="X-Small" />
												<EditRowStyle BackColor="#e9edf2" BorderColor="#e3e4e6" BorderStyle="Solid" BorderWidth="1px" />
												<AlternatingRowStyle BackColor="#EAEEFF" BorderColor="#e3e4e6" BorderStyle="Solid" BorderWidth="1px" Font-Size="X-Small" />
												</asp:GridView>
											</div>
										</div>
										<%-- this is area for Custom Field--%>
										<div class="col-lg-6 col-md-8 col-ms-12">
											<div class="form-group row  mt-2">

												<label for="staticEmail" class="col-sm-4 form-label pl-2">
													<asp:Label ID="Label2" Text="Stage" runat="server"></asp:Label>

												</label>


												<div class="col-sm-8 pr-5">

													<asp:DropDownList ID="ddlStage" runat="server"  Enabled="false" CssClass="form-select  form-select-sm " AutoPostBack="true" OnSelectedIndexChanged="ddlStage_SelectedIndexChanged"></asp:DropDownList>

												</div>

											</div>
											<div class="form-group row " hidden>

												<label for="staticEmail" class="col-sm-4 labelcolorl1 pl-4">
													<asp:Label ID="lblStatus" Text="Status" runat="server"></asp:Label>


												</label>


												<div class="col-sm-8 pr-5">

													<asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control  form-control-sm chzn-select"></asp:DropDownList>

												</div>

											</div>



										</div>
									</div>


									<div class="form-group row mt-2">
										<label for="staticEmail" class="col-sm-2 form-label pl-4">
											Notes : 
                                                 
										</label>
										<div class="col-sm-10 pr-5">
											<%--	<asp:TextBox ID="txtDescription" runat="server" CssClass="form-control form-control-sm" TextMode="MultiLine" MaxLength="500" Height="200px"></asp:TextBox>--%>
											<textarea id="txtNotes" runat="server"></textarea>
										</div>
									</div>

									<div class="row">
										<div class="col-md-3  offset-5">
											<asp:Button ID="btnUpdate" runat="server" Text="Update" ValidationGroup="Submit" OnClick="btnUpdate_Click" CssClass="btn btn-sm btn-primary  " />
											<asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" OnClick="btnCancel_Click" CssClass="btn btn-sm btn-danger " />
										</div>
									
									</div>



								</asp:Panel>

								<asp:Panel ID="pnlViewNotes" runat="server" Visible="false">
									<div class="row">
										<div class="col-md-8 graphs">
											<div class="xs">
												<div class="well1 white">
													<div class="card card-default">

														<div class="card-body">
															<div class="row ">
																<div class="col-md-4">

																	<asp:Label ID="lblsofname" runat="server" Text="Ticket Notes" Font-Size="Larger" ForeColor="Black"></asp:Label>

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

																<div style="overflow: scroll; height: 500px">
																	<asp:GridView GridLines="None" ID="gvTicketNotes" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered"
																		Width="100%">
																		<Columns>
																			<asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="20">
																				<ItemTemplate>
																					<%#Container.DataItemIndex+1 %>
																				</ItemTemplate>
																			</asp:TemplateField>
																			<asp:BoundField DataField="EditedDt" HeaderText="
															Event Time"
																				SortExpression="EditedDt" DataFormatString="{0:yyyy-MM-dd hh:mm:ss}" />
																			<asp:BoundField DataField="Ticketref" HeaderText="Ticket Number"
																				SortExpression="Ticketref" />

																			<asp:TemplateField HeaderText="Description" SortExpression="Description">

																				<ItemTemplate>
																					<asp:Label ID="lblDescription" runat="server" Text='<%# Server.HtmlDecode(Eval("NoteDesc").ToString()) %>'> </asp:Label>
																				</ItemTemplate>
																			</asp:TemplateField>



																		</Columns>
																		<RowStyle BackColor="White" BorderColor="#e3e4e6" BorderWidth="1px" Height="10px" />
																		<FooterStyle BackColor="#EDEDED" Font-Bold="True" ForeColor="White" />
																		<PagerStyle BackColor="#EDEDED" ForeColor="#000000" HorizontalAlign="Center" />
																		<SelectedRowStyle BackColor="#fff" Font-Bold="True" ForeColor="#000000" />
																		<HeaderStyle BackColor="#EEEEEE" Font-Bold="True" ForeColor="#414141" Height="30px" CssClass="header" Font-Size="Small" />
																		<EditRowStyle BackColor="#e9edf2" BorderColor="#e3e4e6" BorderStyle="Solid" BorderWidth="1px" />
																		<AlternatingRowStyle BackColor="#EAEEFF" BorderColor="#e3e4e6" BorderStyle="Solid" BorderWidth="1px" />
																	</asp:GridView>
																</div>
															</div>
														</div>
													</div>
												</div>
											</div>
										</div>
										<div class="col-md-4">
											<div class="card">
												<div class="card-body">
													<div class="row ">
														<div class="col-md-12">

															<asp:Label ID="Label14" runat="server" Text="Ticket Attachments" Font-Size="Medium" ForeColor="Black"></asp:Label>

														</div>


													</div>
													<div style="overflow: scroll">
														<asp:GridView ID="gvTicketAttachment" runat="server" AutoGenerateColumns="false" DataKeyNames="Ticketref" EmptyDataText="No files uploaded" CssClass="table table-bordered" OnRowCommand="gvTicketAttachment_RowCommand">
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

																<asp:TemplateField HeaderText="Download">
																	<ItemTemplate>
																		<asp:LinkButton ID="lnkDownload" runat="server" CausesValidation="False"  ForeColor="Black" CommandArgument='<%# Eval("Filepath") %>'
																			CommandName="Download" Text='<%# Eval("Filename") %>' />
																	</ItemTemplate>
																</asp:TemplateField>

															</Columns>
															<RowStyle BackColor="White" BorderColor="#e3e4e6" BorderWidth="1px" Font-Size="X-Small" Height="10px" />
															<FooterStyle BackColor="#EDEDED" Font-Bold="True" ForeColor="White" />
															<PagerStyle BackColor="#EDEDED" ForeColor="#000000" HorizontalAlign="Center" Font-Size="X-Small" />
															<SelectedRowStyle BackColor="#fff" Font-Bold="True" ForeColor="#000000" Font-Size="X-Small" />
															<HeaderStyle BackColor="#EEEEEE" Font-Bold="True" ForeColor="#414141" Height="30px" CssClass="header" Font-Size="X-Small" />
															<EditRowStyle BackColor="#e9edf2" BorderColor="#e3e4e6" BorderStyle="Solid" BorderWidth="1px" />
															<AlternatingRowStyle BackColor="#EAEEFF" BorderColor="#e3e4e6" BorderStyle="Solid" BorderWidth="1px" Font-Size="X-Small" />
														</asp:GridView>
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
			</div>
	<%--	</ContentTemplate>

		<Triggers>
			
			<asp:PostBackTrigger ControlID="btnUpdate" />
			<asp:PostBackTrigger ControlID="btnCancel" />
		
		</Triggers>

	</asp:UpdatePanel>--%>
			<asp:HiddenField ID="hdnCategoryID" runat="server" />
	<asp:HiddenField ID="hdfldDesk" runat="server" />
		<asp:HiddenField ID="hdnChangeStatus" runat="server" />
		 </form>

 <script src="assetsdata/plugins/summernote/summernote-bs4.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/js/select2.min.js"></script>
    <script src="https://pcv-demo.hitachi-systems-mc.com:5723/assetsdata/plugins/select2/js/select2-custom.js"></script>
    <script>
        $(document).ready(function () {
            $('#<%= txtNotes.ClientID %>').summernote();
        });
    </script>
</body>
</html>

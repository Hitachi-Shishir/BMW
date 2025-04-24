<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmChangeOrgLogo.aspx.cs" Inherits="frmChangeOrgLogo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">


  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
	<div class="row mt-4 gy-1 gx-3 	">
	<div class="col-md-7">
		<div class="card rounded-4 mb-1" style="height:80vh !important">
			<div class="card-body p-4">
				<div class="position-relative mb-5">
                              
                                <!-- Hidden File Upload Control -->
                                <input type="file" name="ctl00$MainContent$FileUpload1" id="MainContent_FileUpload1" class="form-control-sm d-none" onchange="this.form.submit();">
                                <!-- Button with Plus Icon (using Font Awesome) -->
                                <span id="MainContent_regFileUpload1" style="color:Red;visibility:hidden;">Only .jpeg or .JPEG or .gif or .GIF or .png or .PNG</span>
                                <img src="assets/images/gallery/profile-cover.png" class="img-fluid rounded-4 shadow" alt="">
                                <div class="profile-avatar position-absolute top-100 start-50 translate-middle">
                                				<asp:Image ID="img1" runat="server"  class="img-fluid rounded-circle p-1 bg-grd-danger shadow" alt="" src="../dist/img/user1-128x128.jpg" style="height:150px;width:150px;"/>

								</div>
                            </div>

				<div class="row g-2 pt-5 mt-4" >
										<div class="col-md-3">
						<label class="form-label">
							Organisation 
                                        <asp:RequiredFieldValidator ID="RfvOrganisation" runat="server" InitialValue="0" ControlToValidate="ddlOrg" ErrorMessage="*" ForeColor="Red" ValidationGroup="SaveForm"></asp:RequiredFieldValidator>
						</label>
											</div><div class="col-md-9">
						<asp:DropDownList ID="ddlOrg" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field
" AutoPostBack="true" OnSelectedIndexChanged="ddlOrg_SelectedIndexChanged">
						</asp:DropDownList>
					</div>
					<div class="col-md-12">
						<div class="table-responsive table-container">
													<asp:DetailsView ID="DetailsCheckInAsset" runat="server" CssClass="data-table table table-striped border table-sm text-nowrap dataTable no-footer
"
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
	</div>
	<div class="col-md-5">
		<div class="card  border-top border-4 border-primary border-gradient-1 mb-1" style="height:80vh !important">
			<div class="card-body">
					<h6 class="mb-0 fw-bold"> Update Photo</h6>
				<br/>
				<div class="row g-1">
					<div class="col-md-12">
							<label>Choose Photo

	      <asp:RequiredFieldValidator ID="rfvFileUpload1" runat="server" ControlToValidate="FileUpload1" ErrorMessage="*" ForeColor="Red" ValidationGroup="ImgUpload"></asp:RequiredFieldValidator>
										</label>
				
						<asp:FileUpload ID="FileUpload1" CssClass="form-control-sm form-control" runat="server" />
			<asp:regularexpressionvalidator id="regFileUpload1" runat="server" ForeColor="Red" errormessage="Only .jpeg or .JPEG or .gif or .GIF or .png or .PNG" validationexpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))+(.jpg|.jpeg|.JPEG|.gif|.GIF|.png|.PNG)$" controltovalidate="FileUpload1" ValidationGroup="ImgUpload" >
			</asp:regularexpressionvalidator>
			</div>
			</div>
		
			
					<div class="col-md-12 text-end">
					<asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" ValidationGroup="ImgUpload" CssClass="btn btn-sm btn-grd-primary"  />
				</div>
					</div>
			</div>
		</div>
	
		</div>
</asp:Content>


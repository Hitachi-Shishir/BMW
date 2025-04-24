<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmChangeOrgLogo.aspx.cs" Inherits="frmChangeOrgLogo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
	<div class="row">
		<div class="col-md-6">
			<div class="card card-widget widget-user">

				<div class="widget-user-header " style="background: linear-gradient(to right,#4A68DF,#A1C4FF)">
					<h3 class="widget-user-username" style="color: white">My Profile</h3>

				</div>
				<div class="widget-user-image " style="margin-left: -20%">
					<asp:Image ID="img1" runat="server" Width="200" ImageAlign="Middle" Height="90" CssClass="img-circle elevation-2" />
					<%--<img class="img-circle elevation-2" src="../dist/img/user1-128x128.jpg" alt="User Avatar" />--%>
				</div>
				<div class="card-footer">
					<div class="row">
						<div class="col-md-6">
							<label class="form-label">
								Organisation :
                                        <asp:RequiredFieldValidator ID="RfvOrganisation" runat="server" InitialValue="0" ControlToValidate="ddlOrg" ErrorMessage="*" ForeColor="Red" ValidationGroup="SaveForm"></asp:RequiredFieldValidator>
							</label>
							<asp:DropDownList ID="ddlOrg" runat="server" CssClass="form-control chzn-select" AutoPostBack="true" OnSelectedIndexChanged="ddlOrg_SelectedIndexChanged">
							</asp:DropDownList>
						</div>
					</div>
					<div class="row">
						<div class="col-md-12">
							<asp:DetailsView ID="DetailsCheckInAsset" runat="server" CssClass="table table-bordered"
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
								<RowStyle BackColor="#fafafa" BorderColor="#e3e4e6" BorderWidth="1px" CssClass="font_label" Font-Size="Medium" />
								<FooterStyle BackColor="#EDEDED" Font-Bold="True" ForeColor="White" CssClass="font_label" />
								<PagerStyle BackColor="#EDEDED" ForeColor="#000000" HorizontalAlign="Center" />
								<HeaderStyle BackColor="#e3e4e6" Font-Bold="True" ForeColor="#000000" Height="30px" CssClass="font_label" Font-Size="Medium" />
								<EditRowStyle BackColor="#EDEDED" BorderColor="#e3e4e6" BorderStyle="Solid" CssClass="font_label" BorderWidth="1px" Font-Size="Medium" />
								<AlternatingRowStyle BackColor="White" BorderColor="#e3e4e6" BorderStyle="Solid" CssClass="font_label" BorderWidth="1px" />

							</asp:DetailsView>
						</div>
					</div>

				</div>
			</div>
		</div>
		<div class="col-md-6">
			<div class="card">
				<div class="card-body">
					<div class="row">
						<label>Update Photo</label>
					</div>
					<br />
					<div class="row">
						<label>
							Choose Photo

				      <asp:RequiredFieldValidator ID="rfvFileUpload1" runat="server" ControlToValidate="FileUpload1" ErrorMessage="Required" ForeColor="Red" ValidationGroup="ImgUpload"></asp:RequiredFieldValidator>
						</label>
						<asp:FileUpload ID="FileUpload1" CssClass="form-control-sm" runat="server" />
						<asp:RegularExpressionValidator ID="regFileUpload1" runat="server" ForeColor="Red" ErrorMessage="Only .jpeg or .JPEG or .gif or .GIF or .png or .PNG" ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))+(.jpg|.jpeg|.JPEG|.gif|.GIF|.png|.PNG)$" ControlToValidate="FileUpload1" ValidationGroup="ImgUpload">
						</asp:RegularExpressionValidator>
					</div>
					<div class="row">
						<div class="col-md-2 mt-2 offset-3">
							<asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" ValidationGroup="ImgUpload" CssClass="btn btn-sm" Style="background: linear-gradient(to right,#4A68DF,#A1C4FF)" />
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>
</asp:Content>


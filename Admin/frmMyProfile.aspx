<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmMyProfile.aspx.cs" Inherits="Admin_frmMyProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row mt-4 gy-1 gx-3 ">
                <div class="col-md-7">
                    <div class="card rounded-4 mb-1">
                        <div class="card-body p-4">
                            <div class="position-relative mb-5">
                                <label>
                                    <asp:RequiredFieldValidator ID="rfvFileUpload1" runat="server" ControlToValidate="FileUpload1"
                                        ErrorMessage="Required" ForeColor="Red" ValidationGroup="ImgUpload"></asp:RequiredFieldValidator>
                                </label>
                                <!-- Hidden File Upload Control -->
                                <asp:FileUpload ID="FileUpload1" runat="server" CssClass="form-control-sm d-none" onchange="this.form.submit();" />
                                <!-- Button with Plus Icon (using Font Awesome) -->
                                <asp:RegularExpressionValidator ID="regFileUpload1" runat="server" ForeColor="Red"
                                    ErrorMessage="Only .jpeg or .JPEG or .gif or .GIF or .png or .PNG"
                                    ValidationExpression="^.*\.(jpg|jpeg|JPEG|gif|GIF|png|PNG)$" ControlToValidate="FileUpload1"
                                    ValidationGroup="ImgUpload">
                                </asp:RegularExpressionValidator>
                                <img src="assets/images/gallery/profile-cover.png" class="img-fluid rounded-4 shadow" alt="">
                                <div class="profile-avatar position-absolute top-100 start-50 translate-middle">
                                    <asp:Image ID="img" runat="server" class="img-fluid rounded-circle p-1 bg-grd-danger shadow" Width="150" Height="150" alt="" />
                                </div>
                            </div>
                            <div class="profile-info pt-5 d-flex align-items-center justify-content-between">
                                <div class="">
                                    <asp:Label ID="lblUserName" runat="server" CssClass="h3" Text="Label"></asp:Label>
                                    <br />
                                    <asp:Label ID="lblEmailID" runat="server" Text="Label"></asp:Label>
                                </div>
                                <div class="">
                                    <button type="button" class="btn btn-grd-primary rounded-5 px-2 btn-sm"
                                        onclick="document.getElementById('<%= FileUpload1.ClientID %>').click();">
                                        <i class="fa-solid fa-plus"></i>Update Profile
                                    </button>

                                </div>
                            </div>
                            <br />
                            <div class="mt-5 mb-2">
                                <div class="form-check form-switch">
                                   <asp:CheckBox Visible="false" ID="chkTheme" AutoPostBack="true" runat="server" ToolTip="The theme cannot be modified by anyone except you." Text=" Make the theme consistent for everyone." OnCheckedChanged="chkTheme_CheckedChanged" />
                                    <!-- Adjust Checked as needed -->
                                    
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
                <div class=" col-md-5">
                    <div class="card rounded-4 border-top border-4 border-primary border-gradient-1 mb-1">
                        <div class="card-body">
                            <div class="d-flex align-items-start justify-content-between mb-3">
                                <div class="">
                                    <h5 class="mb-0 fw-bold">Profile</h5>
                                </div>
                            </div>
                            <div class="full-info">
                                <div class="info-list d-flex flex-column gap-3">
                                    <div class="info-list-item d-flex align-items-center gap-3">
                                        <span class="material-icons-outlined">account_circle</span>
                                        <p class="mb-0">
                                            Name : 
                                            <asp:Label ID="lblProfile" runat="server"></asp:Label>
                                        </p>
                                    </div>
                                    <div class="info-list-item d-flex align-items-center gap-3">
                                        <span class="material-icons-outlined">done</span>
                                        <p class="mb-0">
                                            EmpID :  
                                 <asp:Label ID="lblEmpId" runat="server" Text="Label"></asp:Label>
                                        </p>
                                    </div>
                                    <div class="info-list-item d-flex align-items-center gap-3">
                                        <span class="material-icons-outlined">code</span>
                                        <p class="mb-0">
                                            Designation :   
                                 <asp:Label ID="lblDesignation" runat="server" Text="Label"></asp:Label>
                                        </p>
                                    </div>
                                    <div class="info-list-item d-flex align-items-center gap-3">
                                        <span class="material-icons-outlined">flag</span>
                                        <p class="mb-0">
                                            Domain Type : 
                                 <asp:Label ID="lblDomainType" runat="server" Text="Label"></asp:Label>
                                        </p>
                                    </div>
                                    <div class="info-list-item d-flex align-items-center gap-3">
                                        <span class="material-icons-outlined">call</span>
                                        <p class="mb-0">
                                            Contact No : 
                                 <asp:Label ID="lblContactNo" runat="server" Text="Label"></asp:Label>
                                        </p>
                                    </div>
                                    <div class="info-list-item d-flex align-items-center gap-3">
                                        <span class="material-icons-outlined">language</span>
                                        <p class="mb-0">
                                            Login ID :  
                                 <asp:Label ID="lblLoginID" runat="server" Text="Label"></asp:Label>
                                        </p>
                                    </div>
                                    <div class="info-list-item d-flex align-items-center gap-3">
                                        <span class="material-icons-outlined">send</span>
                                        <p class="mb-0">
                                            User Role :  
                                 <asp:Label ID="lblUserRole" runat="server" Text="Label"></asp:Label>
                                        </p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-12">
                    <div class="card">
                        <div class="card-body">
                            <div class="row" id="toggle" runat="server" visible="false">
                                <div class="col-md-2">
                                   <div class="form-check">
     <asp:RadioButton ID="rbdBlueTheme" runat="server" GroupName="theme" AutoPostBack="true" OnCheckedChanged="Theme_CheckedChanged" />
     <label class="form-check-label" for="rbdBlueTheme">
         Blue Theme
				
     </label>
 </div>
                                </div>   
                                <div class="col-md-2">
                                       <div class="form-check ">
        <asp:RadioButton ID="rbdLightTheme" runat="server" GroupName="theme" AutoPostBack="true" OnCheckedChanged="Theme_CheckedChanged" />
        <label class="form-check-label" for="flexRadioSuccess">
            Light Theme
				
        </label>
    </div>
                                </div>  
                                <div class="col-md-2">
                                      <div class="form-check ">
       <asp:RadioButton ID="rbdDarkTheme" runat="server" GroupName="theme" AutoPostBack="true" OnCheckedChanged="Theme_CheckedChanged" />
       <label class="form-check-label" for="flexRadioDanger">
           Dark Theme
				
       </label>
   </div>
                                </div>   
                                <div class="col-md-3">
                                   <div class="form-check ">
    <asp:RadioButton ID="rbdSemiDarkTheme" runat="server" GroupName="theme" AutoPostBack="true" OnCheckedChanged="Theme_CheckedChanged" />
    <label class="form-check-label" for="flexRadioWarning">
        Semi Dark Theme
				
    </label>
</div>
                                </div>    
                            </div>

                            <div class="d-flex align-items-center gap-4" >
                               
                            
                             
                                
                                <div class="form-check d-none ">
                                    <asp:RadioButton ID="rbdBoderedTheme" runat="server" GroupName="theme" AutoPostBack="true" OnCheckedChanged="Theme_CheckedChanged" />
                                    <label class="form-check-label" for="flexRadioDark">
                                        Bordered Theme
				
                                    </label>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-md-4" visible="false" runat="server" id="abc">
                    <div class="card rounded-4 border-top border-4 border-primary border-gradient-1">
                        <div class="card-body p-4">
                            <div class="d-flex align-items-start justify-content-between mb-3">
                                <asp:DetailsView ID="DetailsCheckInAsset" runat="server" CssClass="table" Style="border-collapse: collapse; border: 0px solid #ffffff00;"
                                    AllowPaging="True"
                                    AutoGenerateRows="False"
                                    GridLines="None" OnPageIndexChanging="DetailsCheckInAsset_PageIndexChanging">
                                    <%--<AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                              <CommandRowStyle BackColor="#E2DED6" Font-Bold="True" />
                              <EditRowStyle BackColor="#999999" />
                              <FieldHeaderStyle BackColor="#E9ECF1" Font-Bold="True" />--%>
                                    <Fields>
                                        <asp:BoundField DataField="EmpID" HeaderText="EmpID" />
                                        <asp:BoundField DataField="UserName" HeaderText="UserName" />
                                        <asp:BoundField DataField="EmailID" HeaderText="EmailID" />
                                        <asp:BoundField DataField="LoginName" HeaderText="Login ID" />
                                        <asp:BoundField DataField="UserRole" HeaderText="User Role" />
                                        <asp:BoundField DataField="ContactNo" HeaderText="ContactNo" />
                                        <asp:BoundField DataField="Designation" HeaderText="Designation" />
                                        <asp:BoundField DataField="DomainType" HeaderText="Domain Type" />
                                    </Fields>
                                    <%--<RowStyle BackColor="#fafafa" BorderColor="#e3e4e6" BorderWidth="1px" CssClass="font_label" Font-Size="Medium" />
                              <FooterStyle BackColor="#EDEDED" Font-Bold="True" ForeColor="White" CssClass="font_label" />
                              <PagerStyle BackColor="#EDEDED" ForeColor="#000000" HorizontalAlign="Center" />
                              <HeaderStyle BackColor="#e3e4e6" Font-Bold="True" ForeColor="#000000" Height="30px" CssClass="font_label" Font-Size="Medium" />
                              <EditRowStyle BackColor="#EDEDED" BorderColor="#e3e4e6" BorderStyle="Solid" CssClass="font_label" BorderWidth="1px" Font-Size="Medium" />
                              <AlternatingRowStyle BackColor="White" BorderColor="#e3e4e6" BorderStyle="Solid" CssClass="font_label" BorderWidth="1px" />--%>
                                </asp:DetailsView>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row d-none">
                <div class="col-md-6">
                    <div class="card">
                        <div class="card-body">
                            <div class="row">
                            </div>
                            <br />
                            <div class="row">
                                <div class="col-md-6">
                                </div>
                                <%--<div class="col-md-2 mt-4 ">
                           <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_Click" ValidationGroup="ImgUpload" CssClass="btn btn-sm" Style="background: linear-gradient(to right,#4A68DF,#A1C4FF)" />
                           </div>--%>
                            </div>
                            <div class="row">
                                <div class="row" hidden>
                                    <div class="col-md-6">
                                        <asp:Image ID="showimg" runat="server" Width="300" Height="200" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <%--<asp:PostBackTrigger ControlID="btnUpload" />--%>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

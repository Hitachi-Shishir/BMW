<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>
<meta charset="utf-8" />
<meta name="viewport" content="width=device-width, initial-scale=1.0" />
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <!--favicon-->
    <link rel="icon" href="<%= ResolveUrl("~/assetsdata/images/Hitachi/Hitachi_logo.png") %>" type="image/png" />

    <!-- loader-->
    <link href="<%= ResolveUrl("~/assetsdata/css/pace.min.css") %>" rel="stylesheet" />
    <script src="<%= ResolveUrl("~/assetsdata/js/pace.min.js") %>"></script>

    <!--plugins-->
    <link href="<%= ResolveUrl("~/assetsdata/plugins/perfect-scrollbar/css/perfect-scrollbar.css") %>" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/assetsdata/plugins/metismenu/metisMenu.min.css") %>" />
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/assetsdata/plugins/metismenu/mm-vertical.css") %>" />

    <!--bootstrap css-->
    <link href="<%= ResolveUrl("~/assetsdata/css/bootstrap.min.css") %>" rel="stylesheet" />
    <link href="<%= ResolveUrl("~/assetsdata/css/googleapis-font.css") %>" rel="stylesheet" />
    <link href="<%= ResolveUrl("~/assetsdata/css/googleapis-weight.css") %>" rel="stylesheet" />
    <!--main css-->
    <link href="<%= ResolveUrl("~/assetsdata/css/bootstrap-extended.css") %>" rel="stylesheet" />
    <link href="<%= ResolveUrl("~/sassdata/main.css") %>" rel="stylesheet" />
    <link href="<%= ResolveUrl("~/dark-theme.css") %>" rel="stylesheet" />
    <link href="<%= ResolveUrl("~/sassdata/blue-theme.css") %>" rel="stylesheet" />
    <link href="<%= ResolveUrl("~/sassdata/responsive.css") %>" rel="stylesheet" />
    <%--<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/@tabler/icons-webfont@latest/tabler-icons.min.css">--%>

    <link rel="stylesheet" href="<%= ResolveUrl("~/assets/fontawesome/css/fontawesome.css") %>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/assets/fontawesome/css/brands.css") %>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/assets/fontawesome/css/solid.css") %>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/assets/fontawesome/css/sharp-thin.css") %>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/assets/fontawesome/css/duotone-thin.css") %>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/assets/fontawesome/css/sharp-duotone-thin.css") %>" />

    <style>
        @font-face {
            font-family: 'BMWGroupTNPro';
            src: url('assets/fonts/bmwfonts/BMWGroupTNPro-Light.otf') format('opentype');
            font-weight: 300;
            font-style: normal;
        }

        @font-face {
            font-family: 'BMWGroupTNPro';
            src: url('assets/fonts/bmwfonts/BMWGroupTNPro-Regular.otf') format('opentype');
            font-weight: 400;
            font-style: normal;
        }

        @font-face {
            font-family: 'BMWGroupTNPro';
            src: url('assets/fonts/bmwfonts/BMWGroupTNPro-Medium.otf') format('opentype');
            font-weight: 500;
            font-style: normal;
        }

        @font-face {
            font-family: 'BMWGroupTNPro';
            src: url('assets/fonts/bmwfonts/BMWGroupTNPro-Bold.otf') format('opentype');
            font-weight: 700;
            font-style: normal;
        }

        body {
            font-family: 'BMWGroupTNPro', sans-serif !important;
            font-weight: 400;
        }

        *, ::before, ::after {
            --tw-border-spacing-x: 0;
            --tw-border-spacing-y: 0;
            --tw-translate-x: 0;
            --tw-translate-y: 0;
            --tw-rotate: 0;
            --tw-skew-x: 0;
            --tw-skew-y: 0;
            --tw-scale-x: 1;
            --tw-scale-y: 1;
            --tw-scroll-snap-strictness: proximity;
            --tw-ring-offset-width: 0px;
            --tw-ring-offset-color: #fff;
            --tw-ring-color: rgb(59 130 246 / 0.5);
            --tw-ring-offset-shadow: 0 0 #0000;
            --tw-ring-shadow: 0 0 #0000;
            --tw-shadow: 0 0 #0000;
            --tw-shadow-colored: 0 0 #0000;
        }


        ::backdrop {
            --tw-border-spacing-x: 0;
            --tw-border-spacing-y: 0;
            --tw-translate-x: 0;
            --tw-translate-y: 0;
            --tw-rotate: 0;
            --tw-skew-x: 0;
            --tw-skew-y: 0;
            --tw-scale-x: 1;
            --tw-scale-y: 1;
            --tw-scroll-snap-strictness: proximity;
            --tw-ring-offset-width: 0px;
            --tw-ring-offset-color: #fff;
            --tw-ring-color: rgb(59 130 246 / 0.5);
            --tw-ring-offset-shadow: 0 0 #0000;
            --tw-ring-shadow: 0 0 #0000;
            --tw-shadow: 0 0 #0000;
            --tw-shadow-colored: 0 0 #0000;
        }

        .btn {
            color: black !important;
            padding-top: 10px;
            padding-bottom: 10px;
            font-size: 1.1rem;
        }

        body {
            background-image: url(assets/images/bmw/LoginbgImage.png);
            background-size: 100%;
            background-attachment: fixed;
            position: relative;
            transition: all 0.7s linear;
            background-repeat: no-repeat;
            background-size: cover;
            background-position: center;
        }

        input::placeholder {
            color: #999 !important; /* Light gray */
            opacity: 1 !important; /* Optional: ensures full opacity in some browsers */
        }

        .input-group-text {
            background-color: transparent;
            border: var(--bs-border-width) solid #00d8ff;
        }

        .form-control {
            display: block;
            width: 100%;
            padding: .375rem .75rem;
            font-size: 1rem;
            font-weight: 400;
            line-height: 2.5;
            color: #cbcbcb;
            -webkit-appearance: none;
            -moz-appearance: none;
            appearance: none;
            background-color: #ffffff00 !important;
            background-clip: padding-box;
            border: var(--bs-border-width) solid #00d8ff;
            border-radius: var(--bs-border-radius);
            transition: border-color .15s ease-in-out, box-shadow .15s ease-in-out;
        }

            .form-control:focus {
                color: white;
                background-color: transparent;
                border-color: #00d8ff;
                outline: 0;
                box-shadow: 0 0 0 .25rem rgba(13, 110, 253, .25);
            }


        .btn-grd-primary {
            background-color: #00D8FF !important;
            color: black !important;
            padding-top: 10px;
            padding-bottom: 10px;
            font-size: 1.1rem;
            background-image: none !important;
        }

        .form-label {
            color: white;
            font-size: 1rem;
        }

        .form-check-label {
            color: white;
            font-size: 1rem;
        }

        .flex {
            display: flex !important;
        }

        #lnkFrgtPass:hover {
            text-decoration: underline;
            color: #808080
        }

        [data-bs-theme=blue-theme] body {
            background-image: url('<%= ResolveUrl("~/Images/img.jpg") %>') !important;
        }

        .absolute {
            position: absolute !important;
        }

        .inset-0 {
            inset: 0px !important;
        }

        .bg-gradient-to-t {
            background-image: linear-gradient(to top, var(--tw-gradient-stops)) !important;
        }

        .from-sky-900 {
            --tw-gradient-from: #0c4a6e var(--tw-gradient-from-position) !important;
            --tw-gradient-to: rgb(12 74 110 / 0) var(--tw-gradient-to-position) !important;
            --tw-gradient-stops: var(--tw-gradient-from), var(--tw-gradient-to) !important;
        }

        .to-transparent {
            --tw-gradient-to: transparent var(--tw-gradient-to-position) !important;
        }

        .relative {
            position: relative !important;
        }

        @media (min-width: 1280px) {
            .xl\:max-w-xl {
                max-width: 36rem !important;
            }
        }

        @media (min-width: 1280px) {
            .xl\:w-full {
                width: 100% !important;
            }
        }

        @media (min-width: 1280px) {
            .xl\:mx-auto {
                margin-left: auto !important;
                margin-right: auto !important;
            }
        }

        .pe-24 {
            -webkit-padding-end: 5rem !important;
            padding-inline-end: 5rem !important;
        }

        .text-white {
            --tw-text-opacity: 1 !important;
            color: rgb(255 255 255 / 1 !important) !important;
        }

        .font-bold {
            font-weight: 700 !important;
        }

        .text-2xl {
            font-size: 1.5rem !important;
            line-height: 2rem !important;
        }

        .grid-cols-2 {
            grid-template-columns: repeat(2, minmax(0, 1fr)) !important;
        }

        .gap-y-4 {
            row-gap: 1rem !important;
        }

        .gap-x-8 {
            -moz-column-gap: 2rem !important;
            column-gap: 2rem !important;
        }

        .grid-cols-1 {
            grid-template-columns: repeat(1, minmax(0, 1fr)) !important;
        }

        .grid {
            display: grid !important;
        }

        .mt-10 {
            margin-top: 2.5rem !important;
        }

        .items-center {
            align-items: center !important;
        }

        .text-sky-50011 {
            --tw-text-opacity: 1 !important;
            color: rgb(14 165 233 / 1 !important) !important;
        }

        .text-2xl {
            font-size: 1.5rem !important;
            line-height: 2rem !important;
        }

        .ti {
            color: rgb(14 165 233 / 1 ) !important;
            font-family: "tabler-icons" !important;
            speak: none;
            font-style: normal;
            font-weight: normal;
            font-variant: normal;
            text-transform: none;
            line-height: 1;
            -webkit-font-smoothing: antialiased;
            -moz-osx-font-smoothing: grayscale;
        }

        a {
            color: #5b617a
        }

        li {
            list-style-type: none;
        }

        .space-x-3 > :not([hidden]) ~ :not([hidden]) {
            --tw-space-x-reverse: 0 !important;
            margin-right: calc(0.75rem* var(--tw-space-x-reverse)) !important;
            margin-left: calc(0.75rem* calc(1 - var(--tw-space-x-reverse))) !important;
        }

        .font-medium {
            font-weight: 500 !important;
        }

        ol, ul {
            padding-left: 0rem;
        }

        .form-check-input {
            --bs-form-check-bg: #ffffff00;
        }
        /* For xxl screens */
@media (min-width: 1400px) {
    .col-xxl-5 {
        flex: 0 0 auto;
        width: 41.66666667%; /* Same width as col-xl-5 */
    }
    
    body {
        zoom: 150%; /* Forces 150% scale on xxl screens */
        -ms-zoom: 150%;
        -webkit-zoom: 150%;
    }
    
    .section-authentication-cover {
        max-width: 100%;
        overflow-x: hidden; /* Prevent horizontal scrollbars */
    }
    
    /* Keep background sizing consistent */
    body {
        background-size: cover !important;
    }
    .form-body{
        margin-top:5rem !important;
    }
}

    </style>
</head>
<body>


    <div class="section-authentication-cover">
        <div class="">
            <div class="relative col-12 col-xl-5 col-xxl-5 align-items-center justify-content-center">
                <div class="card rounded-0 m-3 mb-0 border-0 shadow-none bg-none bg-transparent">
                    <div class="card-body px-2 px-sm-5 bg-transparent">
                            <img src="assets/images/bmw/WhiteLogo.png" style="width: 100%;" class="mb-4 mt-2" />
                        <div class="form-body mt-5">

                            <form id="Form1" runat="server" >
                                <!-- Login Panel -->
                                <asp:Panel ID="pnlLogin" runat="server">

				<h2 style="font-weight: 300; color:white;">Login to <span style="font-weight: 500; color:#00d8ff">ITConnect</span></h2>
                                    <h6 class="text-white mb-4" style="font-weight: 400;">One platform, Endless possibilities </h6>

                                    <div class="row g-3 mt-3">
                                        <div class="col-12">
                                            <label for="email" class="form-label">
                                                Username
                                            <asp:RequiredFieldValidator ID="RfvtxtUserName" runat="server" ErrorMessage="*" ForeColor="Red" Font-Bold="true" ControlToValidate="txtUserName" ValidationGroup="Login"></asp:RequiredFieldValidator>
                                            </label>
                                            <asp:TextBox ID="txtUserName" CssClass="form-control" runat="server" placeholder="Enter Username to get started"></asp:TextBox>
                                        </div>
                                        <div class="col-12">
                                            <label for="password" class="form-label">
                                                Password
                                            <asp:RequiredFieldValidator ID="rfvtxtPassword" runat="server" ErrorMessage="*" ForeColor="Red" Font-Bold="true" ControlToValidate="txtPassword" ValidationGroup="Login"></asp:RequiredFieldValidator>
                                            </label>
                                            <div class="input-group" id="show_hide_password">
                                                <asp:TextBox class="form-control" ID="txtPassword" runat="server" placeholder="Password" TextMode="Password"></asp:TextBox>
                                                <a href="javascript:;" class="input-group-text bg-transparent"><i class="far fa-eye-slash"></i></a>
                                            </div>
                                        </div>

                                        <div class="col-6">
                                            <asp:CheckBox ID="chk" runat="server" name="item" />
                                            <label class="form-check-label" for="checkbox">Remember Me</label>
                                        </div>
                                        <div class="col-6 text-end">
                                            <asp:LinkButton ID="lnkFrgtPass" Text="Forgot Password?" Style="color: #00d8ff" runat="server" OnClick="lnkFrgtPass_Click"></asp:LinkButton>
                                        </div>
                                        <div class="col-12 mt-4">
                                            <div class="d-grid">
                                                <asp:Button ID="btnSubmit" class="btn btn-grd-primary text-white" runat="server" Text="Login In" OnClick="btnSubmit_Click" ValidationGroup="Login" />
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>

                                <!-- 2FA Panel -->
                                <asp:Panel ID="pnl2FA" runat="server" Visible="false">
                                    <div class="row gy-3 gx-1">
                                        <div class="col-12 text-center">
                                            <asp:Label ID="lbl2FA" runat="server" Text="2FA Authenticator" CssClass="h6 text-success"></asp:Label>
                                        </div>
                                        <div class="col-12 text-center">
                                            <asp:Image ID="imgQrCode" runat="server" Width="120" Height="120" />
                                            <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                            <asp:Label ID="lblSecretKey" runat="server" Text=""></asp:Label>
                                            <asp:Label ID="lblVerificationResult" runat="server" Text=""></asp:Label>
                                        </div>
                                        <div class="col-12">
                                            <label class="form-label">
                                                Enter 2FA
                                            <asp:RequiredFieldValidator ID="rfvtxt2fa" runat="server" ErrorMessage="*" ForeColor="Red" Font-Bold="true" ControlToValidate="txt2fa" ValidationGroup="2FA"></asp:RequiredFieldValidator>
                                            </label>
                                            <asp:TextBox ID="txt2fa" CssClass="form-control" runat="server" placeholder="Enter 2FA" TextMode="SingleLine"></asp:TextBox>
                                        </div>
                                        <div class="col-12">
                                            <div class="form-check">
                                                <asp:CheckBox ID="chkRemb2FA" runat="server" name="item" />
                                                <label class="form-check-label" for="flexSwitchCheckChecked">Remember 2FA For 30 Days in this System!</label>
                                            </div>
                                        </div>
                                        <div class="row g-2 mt-2">
                                            <div class="col-6">
                                                <asp:Button ID="btn2FA" runat="server" Text="Enter OTP" class="btn btn-grd-primary w-100" OnClick="btn2FA_Click" ValidationGroup="2FA" />
                                            </div>
                                            <div class="col-6">
                                                <asp:Button ID="btnReset" runat="server" Text="Go Back" class="btn btn-grd-danger w-100" OnClick="btnReset_Click" CausesValidation="false" />
                                            </div>
                                        </div>
                                    </div>
                                </asp:Panel>

                                <!-- Forgot Password Email Panel -->
                                <asp:Panel ID="pnlEnterEmail" runat="server" Visible="false" CssClass="mt-3">
                                    <h4 class="fw-bold" style="color: #00D8FF;">Forget Password!</h4>
                                    <p class="mb-4 text-white">
                                        No worries, we've got you covered!
                                    </p>
                                    <div class="row g-3">
                                        <div class="col-md-12">
                                            <label class="form-label">Enter Login ID</label>
                                            <asp:TextBox ID="txtLoginName" runat="server" CssClass="form-control" TextMode="SingleLine"></asp:TextBox>
                                        </div>
                                        <div class="col-md-12">
                                            <label class="form-label">Enter Registered Email</label>
                                            <asp:TextBox ID="txtRegisEmail" runat="server" CssClass="form-control" TextMode="SingleLine"></asp:TextBox>
                                        </div>
                                        <div class="col-6">
                                            <asp:Button ID="btnVerifyUser" runat="server" Text="Set Password" class="btn btn-grd-primary w-100 text-white" OnClick="btnVerifyUser_Click" ValidationGroup="ResetPass" />
                                        </div>
                                        <div class="col-6">
                                            <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btn btn-grd-danger w-100 text-white" OnClientClick="history.back(); return false;" />
                                        </div>
                                    </div>
                                    <div class="d-none">
                                        <asp:Label ID="lblGetMail" ForeColor="Red" runat="server" Style="font-size: 14px !important"></asp:Label>
                                    </div>
                                </asp:Panel>

                                <!-- Password Reset Panel -->
                                <asp:Panel ID="pnlForgotPass" runat="server" CssClass="mt-4" Visible="false">
                                    <h4 class="fw-bold" style="color: #00D8FF;">Forget Password!</h4>
                                    <p class="mb-0 text-white">
                                        Update your password to keep your account secure
                                    </p>
                                    <div class="row gx-3 gy-2">
                                        <div class="col-md-12">
                                            <asp:Label ID="lblotpmsg" class="form-label text-success" runat="server"></asp:Label>
                                        </div>
                                        <div class="col-md-12">
                                            <label class="form-label">Enter OTP</label>
                                            <asp:LinkButton ID="resendButton" Text="Resend OTP" OnClick="resendButton_Click" runat="server" CssClass="small opacity-75 text-white float-end"></asp:LinkButton>
                                            <asp:TextBox ID="txtOTP" CssClass="form-control" runat="server" placeholder="Enter OTP" ClientIDMode="Static" TextMode="SingleLine"></asp:TextBox>
                                        </div>
                                        <div class="col-md-12 mt-2">
                                            <label class="form-label">Enter Password</label>
                                            <asp:TextBox ID="txtResetPass" runat="server" placeholder="Enter New Password" ClientIDMode="Static" TextMode="Password" CssClass="form-control"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="regtxtPassword" runat="server" ControlToValidate="txtResetPass"
                                                ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{8,}" ValidationGroup="ResetPass"
                                                ErrorMessage="Invalid Password" ForeColor="Red" Display="Dynamic" />
                                            <asp:RequiredFieldValidator ID="rfvtxtResetPass" Display="Dynamic" runat="server" ControlToValidate="txtResetPass" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="ResetPass" />
                                        </div>
                                        <div class="col-md-12 mt-2">
                                            <label class="form-label">Confirm Password</label>
                                            <asp:TextBox ID="txtConfResetPass" runat="server" placeholder="Confirm Password" CssClass="form-control" TextMode="Password"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="regtxtConfResetPass" runat="server" ControlToValidate="txtConfResetPass"
                                                ValidationExpression="^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{8,}"
                                                ErrorMessage="Invalid Password" ForeColor="Red" Display="Dynamic" ValidationGroup="ResetPass" />
                                            <asp:RequiredFieldValidator ID="rfvtxtConfResetPass" Display="Dynamic" runat="server" ControlToValidate="txtConfResetPass" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="ResetPass" />
                                        </div>
                                        <div class="col-md-12 mt-3">
                                            <asp:Button ID="btnResetPass" runat="server" Text="Verify" CssClass="btn btn-grd-primary w-100 text-white" OnClick="btnResetPass_Click" ValidationGroup="ResetPass" />
                                        </div>
                                    </div>
                                </asp:Panel>

                                <!-- Session Alert Modal -->
                                <div class="modal fade" id="logoutModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
                                    <div class="modal-dialog" role="document">
                                        <div class="modal-content">
                                            <div class="modal-header">
                                                <h5 class="modal-title" id="exampleModalLabel">Login Alert?</h5>
                                                <button class="close close border-0 bg-white" type="button" data-dismiss="modal" aria-label="Close"><span aria-hidden="true" class="text-dark border-0">×</span> </button>
                                            </div>
                                            <div class="modal-body">An active session for this user is detected. Terminate the existing session and continue with login?</div>
                                            <div class="modal-footer">
                                                <button class="btn btn-danger btn-sm" type="button" data-dismiss="modal" onclick="HideModal()">Cancel</button>
                                                <asp:LinkButton ID="btnLogout" runat="server" class="btn btn-dark btn-sm text-white" OnClick="btnLogout_Click">Ok</asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="<%= ResolveUrl("~/assetsdata/js/jquery.min.js") %>"></script>

    <script>
        window.onload = function () {
            document.getElementById('<%= txtUserName.ClientID %>').setAttribute('autocomplete', 'off');
                 document.getElementById('<%= txtPassword.ClientID %>').setAttribute('autocomplete', 'new-password');
             };
        $(document).ready(function () {
            $("#show_hide_password a").on('click', function (event) {
                event.preventDefault();
                if ($('#show_hide_password input').attr("type") == "text") {
                    $('#show_hide_password input').attr('type', 'password');
                    $('#show_hide_password i').addClass("fa-eye-slash");
                    $('#show_hide_password i').removeClass("fa-eye");
                } else if ($('#show_hide_password input').attr("type") == "password") {
                    $('#show_hide_password input').attr('type', 'text');
                    $('#show_hide_password i').removeClass("fa-eye-slash");
                    $('#show_hide_password i').addClass("fa-eye");


                }
            });
        });
    </script>
  

</body>
</html>

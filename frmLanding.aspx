<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmLanding.aspx.cs" Inherits="frmLanding" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>

<html lang="en">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Hitachi System</title>
    <link rel="stylesheet" href='<%= ResolveUrl("~/assets/css/bootstrap.min.css") %>' />
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
          .logout-icon {
      position: absolute;
      top: 10px; /* Adjust spacing from top */
      right: 10px; /* Adjust spacing from right */
      z-index: 1000; /* Ensure it's above other elements */
      color: #fff; /* Optional: set icon color */
      font-size: 18px; /* Optional: set icon size */
  }

      .logout-icon:hover {
          color: #00D8FF; /* Optional: hover effect */
      }


        body {
            font-family: 'BMWGroupTNPro', sans-serif !important;
            font-weight: 400;
        }

        .site-heading h1 span {
            color: #00D8FF;
        }

        .btn:hover {
            color: black);
            background-color: #00D8FF;
            border-color: #00D8FF;
        }

        /* animation code start*/
        .slide-in {
            transform: translateX(-100%);
            animation: slideIn 2.5s ease forwards;
        }

        @keyframes slideIn {
            0% {
                transform: translateX(-100%);
            }

            100% {
                transform: translateX(0);
            }
        }

        .slide-down {
            transform: translateY(-100%);
            animation: slideDown 1s ease forwards;
        }

        @keyframes slideDown {
            0% {
                transform: translateY(-100%);
            }

            100% {
                transform: translateY(0);
            }
        }
        /*animation end*/
        .profile-container {
            position: relative;
            display: inline-block;
        }

        .profile-icon img {
            width: 40px;
            height: 40px;
            border-radius: 50%;
            cursor: pointer;
            object-fit: cover;
            user-select: none;
            border: 1px solid #ffffff; /* Optional: Adds a border to the avatar */
            box-shadow: 0 2px 5px rgba(255, 255, 255, 0.336);
        }

        .dropdown-menu {
            position: absolute;
            top: 50px;
            right: 0;
            background-color: white;
            border: 1px solid #ddd;
            box-shadow: 0 2px 5px rgba(0, 0, 0, 0.15);
            display: none;
            width: 240px;
            border-radius: 1rem;
            z-index: 9999999;
            padding: 0;
        }

            .dropdown-menu.active {
                display: block;
            }

            .dropdown-menu ul {
                list-style: none;
                margin: 0;
                padding: 0;
            }

            .dropdown-menu li {
                padding: 8px;
                cursor: pointer;
                /*transition: background-color 0.2s;*/
                font-size: 14px !important;
                padding-left: 1.1rem
            }

                .dropdown-menu li:hover {
                    background-color: #f0f0f0;
                    border-radius: 4rem
                }

                .dropdown-menu li a {
                    text-decoration: none;
                    color: #333;
                    display: block;
                }


        :root {
            --white: #fff;
            --black: #010f34;
            --heading-color: #8b8b8b;
            --heading-color1: #02537d;
            --body-color: #788094;
            --ternary: #88a5ad;
            --accent: #008cd4;
            --accent-heading: #02537d;
            --gray: #f5f7fa;
            --border: #02537d;
            /*--border: #97b1bf;*/
            --heading-font: "Outfit", sans-serif;
            --body-color-font: "DM Sans", sans-serif;
        }


 

        .container-custome {
            margin-left: 1.8rem;
        }

        .card {
            background-color: white;
            /* opacity: 80%; */
        }

            .card:hover {
                background-color: #730d0d;
            }

        .cs_card.cs_style_1 .cs_card_shape {
            position: absolute;
            bottom: -16px;
            left: -6px;
            color: #02537d00;
            /*color: var(--border);*/
            -webkit-transition: all .8s ease;
            transition: all .8s ease;
        }


        .cs_card_1_wrap .cs_card_1_col {
            -webkit-box-flex: 0;
            -ms-flex: none;
            flex: none;
            width: 305px;
        }

        .cs_card.cs_style_1 {
            position: relative;
            /*padding: 0 20px 20px;*/
            margin-top: 32px;
        }

            .cs_card.cs_style_1 .cs_card_in {
                position: relative;
                z-index: 1;
                -webkit-box-shadow: 0px 10px 50px rgba(0, 0, 0, .08);
                box-shadow: 0 10px 50px #00000014;
                text-align: center;
                padding: 1px 17px 15px;
                border-radius: 14px;
            }

            .cs_card.cs_style_1 .cs_card_icon {
                height: 50px;
                width: 50px;
                border: 2px solid var(--border);
                border-radius: 50%;
                margin-left: auto;
                margin-right: auto;
                margin-top: -25px;
                background-color: #fff;
                padding: 9px;
                -webkit-transition: all .8s ease;
                transition: all .8s ease;
            }

                .cs_card.cs_style_1 .cs_card_icon i {
                    color: var(--heading-color1);
                }

        .cs_center {
            display: -webkit-box;
            display: -ms-flexbox;
            display: flex;
            -webkit-box-align: center;
            -ms-flex-align: center;
            align-items: center;
            -webkit-box-pack: center;
            -ms-flex-pack: center;
            justify-content: center;
        }

        .cs_mb_30 {
            margin-bottom: 12px;
        }

        .cs_semibold {
            font-weight: 600;
        }

        .cs_fs_24 {
            font-size: 24px;
            line-height: 1.417em;
        }

        .cs_mb_6 {
            margin-bottom: 6px;
        }

        .cs_fs_14 {
            font-size: 11px;
            line-height: 1.6;
        }

        .cs_mb_25 {
            margin-bottom: 16px;
        }

        .cs_text_btn {
            display: -webkit-inline-box;
            display: -ms-inline-flexbox;
            display: inline-flex;
            -webkit-box-align: center;
            -ms-flex-align: center;
            align-items: center;
            gap: 8px;
            margin-top: 0.8rem;
        }

        .cs_heading_color {
            color: #000;
            background-color: #00d8ff;
        }

        .cs_text_btn {
            display: -webkit-inline-box;
            display: -ms-inline-flexbox;
            display: inline-flex;
            -webkit-box-align: center;
            -ms-flex-align: center;
            align-items: center;
            gap: 8px;
            text-decoration: none;
        }



        .cs_bold {
            font-weight: 600;
        }

        .cs_text_btn:hover {
            letter-spacing: .4px;
        }

        a:hover {
            text-decoration: none;
            color: var(--accent);
            text-decoration: underline;
        }

        a:hover {
            --bs-link-color-rgb: var(--bs-link-hover-color-rgb);
        }

        .cs_card.cs_style_1:hover .cs_card_icon {
            border-color: var(--accent);
        }

            .cs_card.cs_style_1:hover .cs_card_icon i {
                color: var(--accent);
            }

        .cs_card.cs_style_1:hover .cs_card_shape {
            color: var(--accent);
            transition: all .8s ease;
        }

        img {
            border: 0;
            max-width: 100%;
            height: auto;
        }

        h6 {
            color: #02537d !important;
        }

        .copyright:hover {
            color: #024f77 !important;
            font-weight: 600;
        }

        .logo-sm {
            width: 66%;
        }

        body {
            background-image: url(assets/images/bmw/img001A.png);
            background-size: 100%;
            background-attachment: fixed;
            position: relative;
            transition: all 0.7s linear;
            background-repeat: no-repeat;
            background-size: cover;
            background-position: center;
        }

        @media(max-width:600px) {
            .container-custome {
                margin-left: 0rem;
            }

            .h4, h4 {
                font-size: 1rem;
            }

            .logo-sm {
                width: 87%;
            }
        }
        @media (min-width: 1400px) {

    
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
 
}
    </style>

</head>


<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true"></asp:ScriptManager>

        <div class="container-fluid">
                 <a class="nav-link logout-icon " data-toggle="tooltip" data-placement="bottom" title="Logout" href="/Logout.aspx">
         <i class="fa-solid fa-right-from-bracket"></i>
     </a>
            <div class="row d-none">

                <div class="col-md-8 text-end mt-2">

                    <div class="profile-container">
                        <!-- Avatar Image -->
                        <div class="profile-icon">
                            <%--<img src="assets/images/avatars/11.png" />--%>
                            <asp:Image ID="img" runat="server" class="rounded-circle p-1 border" Width="45" Height="45" alt="" />
                        </div>
                        <!-- Dropdown Menu -->
                        <div class="dropdown-menu">
                            <div class="py-2 px-3">
                                <asp:Label ID="lblUser" runat="server"></asp:Label>
                            </div>
                            <ul>
                                <li><a href='<%= ResolveUrl("~/Admin/frmMyProfile.aspx")%>'>Profile</a></li>
                                <li><a href='<%= ResolveUrl("~/HelpDesk/frmAddRequester.aspx")%>'>Create Account</a></li>
                                <li><a href='<%= ResolveUrl("~/frmChgPass.aspx")%>'>Change Password</a></li>
                                <li>
                                    <asp:LinkButton ID="lnklogout" runat="server" OnClick="lnklogout_Click"> Logout</asp:LinkButton>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>


            <div class="col-md-7">
                <div class="row ">
                    <div class="col-md-12  ">
                        <div class="row container-custome  mt-2">
                            <div class="site-heading  mb-2 mt-3 row">
                                <%-- <h2 class="mb-3 text-white" style="text-shadow: 2px 2px 4px #ffffff4a;">BMW TechWorks India</h2>--%>
                                <img src="assets/images/bmw/WhiteLogo.png" class="mb-3 mt-2 logo-sm" />
                               
                                <h2 style="font-weight: 400" class="text-white mt-5">Welcome to <span style="font-weight: 500; color:#00d8ff;">ITConnect</span></h2>



                            </div>
                        </div>

                        <div class="row mt-4 container-custome  slide-in">
                            <div class="col-md-12 mb-3 " runat="server" id="divSD">
                                <div class="row">
                                    <div class="col-md-2 col-3 d-flex align-items-center justify-content-center" style="border-right: 1px solid white">
                                        <%--<i class="fas fa-boxes-stacked text-white h1"></i>--%>
                                        <img src="assets/images/bmw/Ellipse.png" style="width: 66%;" class="mb-3 " />

                                    </div>
                                    <div class="col-md-8 col-7 px-4">
                                        <h2 class="text-white">Service Desk</h2>
                                        <h6 class="text-white " style="font-weight: 400">Streamlined Service Desk Solutions :
                 <br />
                                            Built to evolve with your business
                                        </h6>
                                        <a class="cs_text_btn  text-uppercase cs_heading_color cs_semibold " href="/about">
                                            <asp:LinkButton ID="lnkBtnSD" runat="server" OnClick="lnkBtnSD_Click" class="cs_text_btn px-5 text-uppercase cs_heading_color cs_semibold btn">Open</asp:LinkButton>
                                        </a>
                                    </div>

                                </div>
                            </div>
                            <div class="col-md-12 mt-4 " runat="server" id="divALM">
                                <div class="row">
                                    <div class="col-md-2 col-3 d-flex align-items-center justify-content-center" style="border-right: 1px solid white;">
                                        <%--<i class="fas fa-boxes-stacked text-white h1"></i>--%>
                                        <img src="assets/images/bmw/Shape8.png" style="width: 66%;" class="mb-3" />

                                    </div>
                                    <div class="col-md-8 col-7  px-4">
                                        <h2 class="text-white">ALM</h2>
                                        <h6 class="text-white" style="font-weight: 400;">IT & Non-IT ALM: Smooth Control
                                            <br />
                                            from Requisition to Disposal
                                        </h6>
                                        <a class="cs_text_btn text-uppercase cs_heading_color cs_semibold" href="/about">
                                            <asp:LinkButton ID="btnALMDsbrd" runat="server" OnClick="btnALMDsbrd_Click"
                                                class="cs_text_btn px-5 text-uppercase cs_heading_color cs_semibold btn">
                Open
                                            </asp:LinkButton>
                                        </a>
                                    </div>
                                </div>

                            </div>



                            <div class="row">

                                <asp:Button ID="btnpop" runat="server" Text="show" Style="display: none" />
                                <asp:ModalPopupExtender ID="mp1" ClientIDMode="Static" runat="server" PopupControlID="Panel1" CancelControlID="btnclose" TargetControlID="btnpop" BackgroundCssClass="modalBackground">
                                </asp:ModalPopupExtender>
                                <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" align="center">
                                    <div class="card">
                                        <div class="card-body" style="max-height: 90vh; min-height: 20vh; overflow-y: auto; width: 100%">
                                            <div class="col-sm-12">
                                                <asp:Label ID="lblBody" runat="server" Text="MODIFY SCOPE"></asp:Label>
                                            </div>
                                            <div class="col-sm-12">
                                                <label for="staticEmail" class="col-sm-2 labelcolorl1 pl-5">
                                                    Organization:
     <asp:RequiredFieldValidator ID="rfvddlOrg" runat="server" ControlToValidate="ddlOrg" InitialValue="0" ErrorMessage="Required" Font-Bold="true" ForeColor="Red"></asp:RequiredFieldValidator>
                                                </label>
                                                <div class="col-sm-4 pr-5">
                                                    <asp:DropDownList ID="ddlOrg" runat="server" CssClass="form-control form-control-sm chzn-select">
                                                    </asp:DropDownList>
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:CheckBox ID="chkPCV" runat="server" CssClass="form-check" Text="PCVisor" />
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:CheckBox ID="chkALM" runat="server" CssClass="form-check" Text="ALM" />
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:CheckBox ID="chkServiceDesk" runat="server" CssClass="form-check" Text="Service Desk" />
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:CheckBox ID="chkCustomPortal" runat="server" CssClass="form-check" Text="Custom Portal" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="card-footer text-end d-none">
                                            <asp:LinkButton runat="server" ID="btnclose" Text="Close" CssClass="btn btn-grd-danger btn-sm text-white" Style="text-decoration: none;" />
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>
                        </div>




                    </div>
                </div>
            </div>
        </div>
    </form>
    <footer class="page-footer " style="bottom: 0; left: 400px; position: fixed; right: 0; height: 18px;">
        <p class="mb-0  text-muted" style="font-size: 10px;"><a href="https://hitachi-systems.co.in/" class="text-muted copyright text-decoration-none">Hitachi Stystems India Pvt Ltd</a> © 2025. All right reserved.</p>
    </footer>
    <script>
        const profileIcon = document.querySelector('.profile-icon');
        const dropdownMenu = document.querySelector('.dropdown-menu');

        // Toggle dropdown visibility
        profileIcon.addEventListener('click', () => {
            dropdownMenu.classList.toggle('active');
        });

        // Close dropdown if clicked outside
        window.addEventListener('click', (e) => {
            if (!profileIcon.contains(e.target) && !dropdownMenu.contains(e.target)) {
                dropdownMenu.classList.remove('active');
            }
        });
    </script>




</body>

</html>

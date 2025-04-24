<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmUserlanding.aspx.cs" Inherits="frmUserlanding" %>

<!DOCTYPE html>
<html lang="en" />

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <!-- Required meta tags -->
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>BMW</title>
    <link rel="icon" type="image/png" href="AgentTicketCSS/images/icons/favicon.ico" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/Asset/css/bootstrapv5.min.css") %>">
    <link rel="stylesheet" href="<%= ResolveUrl("~/assets/fontawesome/css/fontawesome.css") %>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/assets/fontawesome/css/brands.css") %>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/assets/fontawesome/css/solid.css") %>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/assets/fontawesome/css/sharp-thin.css") %>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/assets/fontawesome/css/duotone-thin.css") %>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/assets/fontawesome/css/sharp-duotone-thin.css") %>" />


    <style>
  .service-request-link {
    position: relative;
    display: block;
}
 
.service-item {
    position: relative;
    overflow: hidden;
    transition: all 0.3s ease;
}
 
.coming-soon-overlay {
    position: absolute;
    top: 0;
    left: 0;
    width: 100%;
    height: 100%;
    background-color: #00d8fff5;
    color: white;
    display: flex;
    justify-content: center;
    align-items: center;
    font-weight: bold;
    font-size: 1.2rem;
    opacity: 0;
    transition: opacity 0.3s ease;
    pointer-events: none;
}
 
.service-request-link:hover .coming-soon-overlay {
    opacity: 1;
}
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

        .service-item:hover h4 {
            color: black !important;
        }

        .logoimg {
            width: 50%;
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


        .service-item:hover .item {
            color: black !important;
        }

        .form-control:focus {
            color: #ffffff;
            background-color: #ffffff00;
            border-color: #ffffff00;
            outline: 0;
            box-shadow: 0 0 0 .25rem rgb(13 110 253 / 0%);
        }

        .form-control {
            background-color: transparent;
            border: 0px;
            border-bottom: 1px solid #fff !important;
            border-radius: 0;
        }

        .form-control-sm {
            padding: .25rem 0rem;
        }

        .search-container {
            position: relative;
            display: flex;
            align-items: center;
        }

        .search-input {
            padding-right: 30px; /* Ensure space for the button */
        }

        .search-button {
            position: absolute;
            right: 10px;
            top: 50%;
            transform: translateY(-50%);
            border: none;
            background: transparent;
            cursor: pointer;
        }

        .grid-container {
            display: grid;
            grid-template-columns: auto auto auto auto auto;
            /*  background-color: #2196F3;*/
            /*  padding: 10px;*/
        }


        .grid-item {
            width: 14rem;
            padding: 7px;
        }

        a {
            text-decoration: none;
            color: #000a2d;
        }

            a:hover,
            a:active {
                text-decoration: none;
                color: #4d69cd;
            }



        .site-heading h2 {
            display: block;
            /*font-weight: 700;*/
            margin-bottom: 10px;
            /*text-transform: uppercase;*/
            color: white;
            margin-top: 0rem;
        }

        .site-heading h4 {
            display: inline-block;
            padding-bottom: 20px;
            position: relative;
            z-index: 1;
        }

        .h4,
        h4 {
            font-size: 1.2rem;
            color: white;
        }

        .site-heading h2 span {
            color: #00D8FF;
        }

        .site-heading h4::before {
            background: #cd3232 none repeat scroll 0 0;
            bottom: 0;
            content: "";
            height: 2px;
            left: 50%;
            margin-left: -25px;
            position: absolute;
            width: 50px;
        }

        .card-body {
            rotate: -45deg;
            padding: 0;
        }

        /* --------------SERVICE------------------ */


        @keyframes water-wave {
            0% {
                border-radius: 56% 44% 70% 30% / 30% 54% 45% 70%;
            }

            50% {
                border-radius: 3% 97% 15% 85% / 72% 0% 100% 28%;
            }

            100% {
                border-radius: 56% 44% 70% 30% / 30% 54% 45% 70%;
            }
        }

        .service-box:hover {
            transform: translateY(-0.5rem);
        }



        .services {
            width: 100%;
            height: 100vh;
            display: flex;
            flex-direction: column;
            justify-content: space-evenly;
            align-items: center;
        }


        .service-item-container {
            width: 100%;
            display: flex;
            justify-content: center;
            align-items: center;
        }

        .service-item {
            text-align: center;
            display: flex;
            flex-direction: column;
            justify-content: center;
            align-items: center;
            /* width: 300px; */
            padding: 20px 20px;
            /* height: 400px; */
            box-sizing: border-box;
            /* margin: 30px; */
            position: relative;
            background: #0000008a;
            border-radius: 6px;
            /* box-shadow: 0 1px 2px rgba(122, 119, 119, 0.07); */
            cursor: pointer;
            /*margin-top: -8rem;*/
            z-index: 9999;
            border: 1.5px solid #00D8FF;
        }



        .item {
            font-size: 1.8rem;
            color: white;
        }

        .sizeContest {
            color: #fff;
            margin-top: 0.5rem;
        }

        .sizeContest1 {
            color: #141f47;
            margin-top: 0.5rem;
            font-size: 1rem;
        }




        .service-item h3 {
            text-align: center;
        }

        .service-item p {
            color: rgba(87, 105, 117, 0.90);
            text-align: center;
        }

        .zoom:hover {
            transform: scale(1.05);
            transition: all 0.3s linear;
            /* (150% zoom - Note: if the zoom is too large, it will go outside of the viewport) */
        }







        @keyframes slideInLeft {
            from {
                transform: translateY(30rem);
                opacity: 1;
            }

            to {
                transform: translateY(0);
                opacity: 1;
            }
        }

        .Left {
            animation: 0.7s ease-out 0s 1 slideInLeft;
        }

        @keyframes slideInRight {
            0% {
                transform: translateX(-80%);
            }

            100% {
                transform: translateX(0);
            }
        }

        .right {
            animation: 1.2s ease-out 0s 1 slideInRight;
        }

        .button {
            width: 140px;
            height: 40px;
            border-radius: 5px;
            border: 1px solid #253670;
            display: flex;
            justify-content: center;
            align-items: center;
            color: #576975;
            margin-top: 10px;
            display: none;
        }



        .service-item:hover {
            background-color: #00D8FF;
            transition: all ease 0.3s;
        }

        .button:hover {
            background-color: #141f47;
            border: 1px solid #141f47;
            color: #FFFFFF;
            transition: all ease 0.3s;
        }

        .service-item:hover .bar {
            display: block;
        }

        @keyframes bar {
            0% {
                width: 0px;
            }

            100% {
                width: 200px;
            }
        }

        @media(max-width:1050px) {
            .service-item-container {
                flex-wrap: wrap;
            }

            .services {
                height: auto;
            }

            .s-heading {
                margin: 15px;
            }

            .service-item {
                flex-grow: 1;
            }
        }

        @media(max-width:600px) {
            .logoimg {
                width: 100%;
            }

            .site-heading h2 {
                display: block;
                margin-bottom: 10px;
                color: white;
                margin-top: 1rem;
                font-size: 1.1rem;
            }

            .site-heading h4 {
                display: block;
                font-weight: 700;
                margin-bottom: 10px;
                color: white;
                margin-top: 1rem;
                font-size: 0.75rem;
            }

            .button {
                width: 100px;
                height: 40px;
                border-radius: 5px;
                border: 1px solid #253670;
                display: flex;
                justify-content: center;
                align-items: center;
                color: #576975;
                margin-top: 10px;
                display: none;
            }

            .service-item {
                padding: 15px 0;
            }

            .bar {
                width: 100px;
                height: 4px;
            }
        }

        /* width */
        ::-webkit-scrollbar {
            width: 0px;
        }

        /* Track */
        ::-webkit-scrollbar-track {
            box-shadow: inset 0 0 5px grey;
            border-radius: 10px;
        }

        /* Handle */
        ::-webkit-scrollbar-thumb {
            background: #00D8FF;
            border-radius: 10px;
        }

            /* Handle on hover */
            ::-webkit-scrollbar-thumb:hover {
                background: #23889a
            }

        @keyframes slideInDown {
            from {
                transform: translateY(30rem);
                opacity: 1;
            }

            to {
                transform: translateY(0);
                opacity: 1;
            }
        }

        .down {
            animation: 0.7s ease-out 0s 1 slideInDown;
        }

        @keyframes slideInUp {
            from {
                transform: translateY(-30rem);
                opacity: 1;
            }

            to {
                transform: translateY(0);
                opacity: 1;
            }
        }

        .Up {
            animation: 0.7s ease-out 0s 1 slideInUp;
        }

        @keyframes slideInRight {
            0% {
                transform: translateX(-80%);
            }

            100% {
                transform: translateX(0);
            }
        }

        .right {
            animation: 1.2s ease-out 0s 1 slideInRight;
            z-index: 999999;
        }

        @keyframes slideInLeft {
            0% {
                transform: translateX(80%);
            }

            100% {
                transform: translateX(0);
            }
        }

        .Left {
            animation: 1.2s ease-out 0s 1 slideInLeft;
            z-index: 999999;
        }


        /* Media queries for responsiveness */
        @media (max-width: 768px) {
            .grid-container {
                grid-template-columns: repeat(auto-fit, minmax(150px, 1fr));
            }

            /*     .service-item {
                margin-top: 2rem;
            }*/
        }

        @media (max-width: 480px) {
            .sizeContest {
                font-size: 14px;
            }

            .service-icon i {
                font-size: 20px;
            }
        }

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
 
}
    </style>
</head>
<body style="background-image: url(assets/images/bmw/backgroundImage.png); background-size: 100%; background-attachment: fixed; position: relative; transition: all 0.7s linear; background-repeat: no-repeat; background-size: cover; background-position: center;"
    class="px-2 px-sm-5 py-3">
    <%--<body class="bg-dark px-5 py-3">--%>
    <form id="frm" runat="server">
<div  class="text-white text-end mt-1">
<asp:Label ID="lblUser" runat="server" Text="" style="    text-transform: capitalize;"></asp:Label></div>
        <a class="nav-link logout-icon " data-toggle="tooltip" data-placement="bottom" title="Logout" href="/Logout.aspx">
            <i class="fa-solid fa-right-from-bracket"></i>
        </a>
        <div class="container-fluid ">
            <section class="section ">
                <div class="container_2 container-text ">
                    <br>
                    <div class="row ">

                        <div class="col-11 col-md-10 ">
                            <div class="site-heading  mb-2 ">
                                <%-- <h2 class="mb-3" style="text-shadow: 2px 2px 4px #ffffff4a;">BMW TechWorks India</h2>--%>
                                <img src="assets/images/bmw/WhiteLogo.png" class="mb-3 mt-2 logoimg" />

                                <h2 style="font-weight: 300;" class="mt-5">Welcome to <span style="font-weight: 500">ITConnect</span></h2>
                            </div>
                        </div>

                    </div>
                    <div class="row mt-5">

                        <div class="col-md-7  col-8 ">
                            <div class="search-container">
                                <asp:TextBox runat="server" ID="txtSearchKB" class="form-control form-control-sm search-input text-white" placeholder="Search Here.."></asp:TextBox>
                                <asp:LinkButton ID="btnGO" runat="server" OnClick="btnGO_Click" class="search-button">
      <i class="fa-solid fa-magnifying-glass  text-white "></i>
                                </asp:LinkButton>
                            </div>
                        </div>

                        <div class="col-md-2   col-2 text-start">
                            <asp:LinkButton ID="lnkBtn" runat="server" OnClick="lnkBtn_Click" CssClass="btn btn-sm btn-secondary px-3">Refresh</asp:LinkButton>
                        </div>
                        <div class="col-md-3"></div>
                    </div>
                </div>


                <br>
            </section>
        </div>
        <div class="container p-4 card mb-5 bg-dark" style=" z-index: 999;     margin-left: 0;"
            id="divTable" runat="server" visible="false">
            <div class="row gy-3">
                <div class="table-responsive table-container" style="height: 270px;">
                    <asp:GridView GridLines="None" ID="gvResolution" runat="server" DataKeyNames="ID" AutoGenerateColumns="false" CssClass="data-table table-head-fixed text-wrap table table-sm table-bordered text-white" PageSize="10">
                        <Columns>
                            <asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="10%">
                                <ItemTemplate>
                                    <%#Container.DataItemIndex+1 %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Issue" HeaderText="Issue" NullDisplayText="NA" ItemStyle-Width="40" />
                            <asp:TemplateField HeaderText="Description" SortExpression="Description" ItemStyle-Width="40">
                                <ItemTemplate>
                                    <asp:Label ID="lblDescription" runat="server"  Text='<%# Server.HtmlDecode(Eval("ResolutionDetail").ToString()) %>'> </asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
        <div class="row  px-2" id="divCard" runat="server">
            <div class="col-md-8">

                <div class="row g-4">



                    <div class="col-md-3  col-6 equal-height ">
                        <%--<a href="#">--%>
                        <asp:LinkButton ID="lnkIncident" runat="server" OnClick="imgbtnIncident_Click">
                        <div class="service-item  right px-5 px-sm-4">
                          
                            <div class="item service-icon">
<i class="fa-solid fa-triangle-exclamation"></i>                            </div>

                           
                            <h4 class="sizeContest"><span style="opacity: 0;">Incident </span> Incident</h4>
                            <!-- <a class="button" href="#">Explore</a> -->
                        </div>
                        </asp:LinkButton>
                        <%--</a>--%>
                    </div>

                    <div class="col-md-3  col-6 equal-height ">
                        <asp:LinkButton ID="lnkServicereq" runat="server" OnClick="imgbtnServiceRequest_Click">
                        <div class="service-item  right px-5 px-sm-4">
                          
                            <div class="item service-icon">
                      <i class="fas fa-cogs
"></i>  

                            </div>
                            <h4 class="sizeContest">Service Request</h4>
                        </div>
                        </asp:LinkButton>
                    </div>

                    <div class="col-md-3  col-6 equal-height ">
                        <asp:LinkButton ID="lnkChangeManagement" Enabled="false" runat="server" CssClass="service-request-link disabled-link">
    <div class="service-item right text-center px-5 px-sm-4">
        <div class="item service-icon">
            <i class="fas fa-user-gear"></i>
        </div>
        <h4 class="sizeContest">Change Management</h4>
        <div class="coming-soon-overlay">Coming Soon...</div>
    </div>
</asp:LinkButton>

                    </div>


                    <div class="col-md-3  col-6 equal-height ">
                        <asp:LinkButton ID="lnkassetrequest" Enabled="false" runat="server" CssClass="service-request-link disabled-link">
    <div class="service-item right px-5 px-sm-5">
        <div class="item service-icon">
            <i class="fas fa-laptop"></i>
        </div>
        <h4 class="sizeContest">Asset Request</h4>
        <div class="coming-soon-overlay">Coming Soon...</div>
    </div>
</asp:LinkButton>

                    </div>
                    <div class="col-md-3  col-6 equal-height ">
                        <asp:LinkButton ID="lnkmydeallocationrequest" Enabled="false" runat="server" CssClass="service-request-link disabled-link">
    <div class="service-item right px-5 px-sm-4">
        <div class="item service-icon">
            <i class="fas fa-exchange-alt"></i>
        </div>
        <h4 class="sizeContest">Asset Deallocation</h4>
        <div class="coming-soon-overlay">Coming Soon...</div>
    </div>
</asp:LinkButton>

                    </div>




<div class="col-md-3  col-6 equal-height ">
                        <asp:LinkButton ID="lnkmyassets" runat="server" OnClick="lnkmyassets_Click">
    <div class=" service-item right px-5 px-sm-4">
      
        <div class="item service-icon">
           <i class="fas fa-desktop  "></i>

        </div>
        <h4 class="sizeContest"><span  style="opacity:0">dsdsfdsfsda </span> My Assets</h4>
    </div>
                        </asp:LinkButton>
                    </div>



                     <div class="col-md-3  col-6 equal-height ">
<asp:LinkButton ID="lnkUserDashboard" runat="server" OnClick="lnkUserDashboard_Click">
<div class="service-item  right px-5 px-sm-4">
<div class="item service-icon">
<i class="fas fa-chart-line
"></i>
 
    </div>
<h4 class="sizeContest"><span  style="opacity:0">dsdsfdsfsda </span> Dashboard</h4>
</div>
</asp:LinkButton>
</div>


                  
                    <div class="col-md-3  col-6 equal-height ">
                        <a id="" href="#"  class="service-request-link">
                            <div class="service-item  right px-5 px-sm-4">

                                <div class="item service-icon">
                                    <i class="fas fa-book-open-reader

  "></i>


                                </div>

                                <h4 class="sizeContest">Knowledge
                                    <br>
                                    Base</h4>
<div class="coming-soon-overlay">Coming Soon...</div>                            </div>
                        </a>
                    </div>



                </div>
            </div>

        </div>






        <div class="container-fluid bg-light d-none"
            style="border-bottom: 0.5px solid #141f47; border-radius: 0 0rem 0.5rem 0.5rem;">
            <div class="row py-2 text-center text-white px-3">

                <div class="col-12 ">
                    <p class="text-dark small mb-0">© 2024 <a href="https://hitachi-systems.co.in/" target="_blank" class="">Hitachi Systems India Pvt. Ltd.</a> All rights reserved.</p>

                </div>
            </div>
        </div>




    </form>

    <div id="dropDownSelect1"></div>

    <!--===============================================================================================-->
    <script src="AgentTicketCSS/vendor/jquery/jquery-3.2.1.min.js"></script>
    <!--===============================================================================================-->
    <script src="AgentTicketCSS/vendor/animsition/js/animsition.min.js"></script>
    <!--===============================================================================================-->
    <script src="AgentTicketCSS/vendor/bootstrap/js/popper.js"></script>
    <script src="AgentTicketCSS/vendor/bootstrap/js/bootstrap.min.js"></script>
    <!--===============================================================================================-->
    <script src="AgentTicketCSS/vendor/select2/select2.min.js"></script>
    <!--===============================================================================================-->
    <script src="AgentTicketCSS/vendor/daterangepicker/moment.min.js"></script>
    <script src="AgentTicketCSS/vendor/daterangepicker/daterangepicker.js"></script>
    <!--===============================================================================================-->
    <script src="AgentTicketCSS/vendor/countdowntime/countdowntime.js"></script>
    <!--===============================================================================================-->
    <script src="AgentTicketCSS/js/main.js"></script>

    <script>
        AOS.init({
            duration: 3000
        });
    </script>

</body>
</html>

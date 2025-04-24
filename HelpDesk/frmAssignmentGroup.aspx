<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmAssignmentGroup.aspx.cs" Inherits="HelpDesk_frmAssignmentGroup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
           <div class="card">
        <div class="card-body">
             <div class="row pt-5">
        <div class="col-lg-12">
            <div class="text-center error-pages">
                <h2 class="coming-soon-title text-success fw-bold mb-4 mt-2">We are coming soon</h2>
                <h6 class="text-white text-uppercase mt-2">Something exciting is coming your way!</h6>

                <p class="text-light opacity-50">We are working behind the scenes to bring you something special. Stay tuned – it’s coming soon!</p>

                <div class="w-50 py-4 mx-auto d-none">
                  <form class="">
                    <div class="input-group input-group-lg">
                      <input type="text" class="form-control" placeholder="Enter Your Email ID">
                      <button class="btn btn-success" type="button"><i class="bi bi-arrow-right"></i>
                      </button>
                    </div>
                  </form>
                </div>
                
                <div class="mt-4 d-flex align-items-center justify-content-center gap-3 py-3">
                  <a href="../frmDashboard.aspx" class="btn btn-success rounded-5 px-4"><i class="bi bi-house-fill me-2"></i>Go To Home</a>
 <a href="javascript:void(0);" class="btn btn-outline-light rounded-5 px-4" onclick="window.history.back();">
        <i class="bi bi-arrow-left me-2"></i>Previous Page
    </a>
                </div>

                <div class="mt-4 mb-5">
                    <p class="text-light small">(Thank you for your patience! We’re just getting started)</p>
                </div>
                   <hr class="border-light border-2 mt-5">
                   <div class="list-inline contacts-social mt-3"> 
                     <a href="https://www.youtube.com/@hitachisystemsindiapvtltd9106" target="_blank" class="list-inline-item bg-danger text-white border-0"><i class="bi bi-youtube"></i></a>
 <a href="https://www.instagram.com/accounts/login/?next=https%3A%2F%2Fwww.instagram.com%2Fhitachisystemsindia%2F%3Figsh%3DMXB6aHR3YzQ1OW01Ng%253D%253D&is_from_rle" target="_blank" class="list-inline-item  text-white border-0" style="            background: linear-gradient(45deg, #dd2a7b, #8134af); /* Instagram pink to purple */
">
        <i class="bi bi-instagram"></i>
    </a>                     <a href="https://hitachi-systems.co.in/" target="_blank" class="list-inline-item bg-info text-white border-0"><i class="bi bi-globe"></i></a>
                     <%--<a href="javascript:;" class="list-inline-item bg-linkedin text-white border-0"><i class="bi bi-linkedin"></i></a>--%>
                  </div>
            </div>
        </div>
    </div>
        </div>
    </div>
</asp:Content>


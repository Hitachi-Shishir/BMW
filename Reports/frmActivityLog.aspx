<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmActivityLog.aspx.cs" Inherits="Reports_frmActivityLog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
     <style>
     .dataTables_filter {
         margin-top: -29px !important;
     }

     .dt-buttons > .btn-outline-secondary {
         padding: 0.25rem 0.5rem !important;
         font-size: 0.875rem !important;
     }

     .pagination {
         --bs-pagination-padding-x: 0.5rem;
         --bs-pagination-padding-y: 0.25rem;
         --bs-pagination-font-size: 0.875rem;
         --bs-pagination-border-radius: var(--bs-border-radius-sm);
         /*margin-top: -1.7rem!important;*/
     }

 </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="card">
        <div class="card-body">        
        <div class="row">
            <div class="col-md-12">
                <div class="table-responsive table-container" >
                    <asp:GridView ID="grdLog" runat="server" CssClass="data-table table table-striped border table-sm text-nowrap table-hover" AutoGenerateColumns="true">
                    </asp:GridView>
                </div>
            </div>
            </div>
        </div>
    </div>
</asp:Content>


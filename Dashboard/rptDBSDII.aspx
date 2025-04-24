<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="rptDBSDII.aspx.cs" Inherits="Reports_rptDBSDII" %>
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
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="card ">
        <div class="card-header">
            <asp:Label ID="Label2" runat="server" CssClass="h6 card-title" Text="Total : "></asp:Label>
            <asp:Label ID="lblTotal" runat="server" CssClass="h6 card-title" Text="0"></asp:Label>
            <asp:ImageButton ID="ImgbtnExport" runat="server" ImageUrl="~/images/excel.png" CssClass="pull-right d-none" OnClick="ImgbtnExport_Click" />
        </div>
        <div class="card-body">
            <div class="table-responsive table-container">
                <asp:GridView GridLines="None" ID="gvSDTicketsDetails" runat="server" AutoGenerateColumns="true" CssClass="data-table table-striped table-sm table table-head-fixed text-nowrap border" Width="100%" ShowHeaderWhenEmpty="True" EmptyDataText="No records Found">
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>


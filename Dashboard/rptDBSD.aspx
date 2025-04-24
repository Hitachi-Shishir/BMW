<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="rptDBSD.aspx.cs" Inherits="Dashboard_rptDBSD" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
     <style>
     /*.dataTables_filter {
       margin-top: -29px !important;
   }*/
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
    <div class="well1 white">
        <div class="card">
            <div class="card-header d-flex align-items-start justify-content-between">
                <asp:Label ID="Label1" runat="server" Text=" Ticket Details" Font-Size="Larger" ForeColor="Black" CssClass="d-none"></asp:Label>

                <h6 class="card-tittle mb-0">Ticket Details</h6>
                <div >
                      <asp:Label ID="Label4" runat="server" CssClass="h6 " Text="Total: "></asp:Label>
  <asp:Label ID="lblTotal" runat="server" CssClass="h6"  Text="0"></asp:Label> &nbsp;&nbsp;&nbsp;
                            <%--<asp:ImageButton ID="" runat="server" ImageUrl="~/Images/New folder/excelnew.png"  CssClass="pull-right ml-2 mt-0 btn-outline-success control-label" class="fa fa-download" />--%>
              <%--<asp:LinkButton runat="server" ID="ImgbtnExport" OnClick="ImgbtnExport_Click" CssClass="btn btn-sm btn-outline-secondary" > Export <i class="fa-solid fa-download"></i></asp:LinkButton>--%>
                    </div>
            </div>
            <div class="card-body">
                
                <div class="table-container table-responsive">
                    <asp:GridView GridLines="None" ID="gvSDTicketsDetails" runat="server" AutoGenerateColumns="true" CssClass="table data-table1 table-sm border text-nowrap" Width="100%" ShowHeaderWhenEmpty="True" EmptyDataText="No records Found">
                       <%-- <RowStyle BackColor="White" BorderColor="#e3e4e6" BorderWidth="1px" Height="5px" />
                        <FooterStyle BackColor="#EDEDED" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#EDEDED" ForeColor="#000000" HorizontalAlign="Left" Height="5px" VerticalAlign="NotSet" CssClass="header" />
                        <SelectedRowStyle BackColor="#fff" Font-Bold="True" ForeColor="#000000" Height="5px" />
                        <HeaderStyle BackColor="#EEEEEE" Font-Bold="True" ForeColor="#414141" Height="5px" BorderColor="WhiteSmoke" CssClass="header sorting_asc" Font-Size="Small" />
                        <EditRowStyle BackColor="#e9edf2" BorderColor="#e3e4e6" BorderStyle="Solid" BorderWidth="1px" Height="5px" />
                        <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#EDEDED" />--%>
                        <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
                        <%--<AlternatingRowStyle BackColor="#EAEEFF" BorderColor="#e3e4e6" Height="5px" BorderStyle="Solid" BorderWidth="1px" />--%>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>


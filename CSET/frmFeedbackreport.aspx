<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmFeedbackreport.aspx.cs" Inherits="CSET_frmFeedbackreport" %>

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
            margin-top: -1.7rem !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">

    <div class="card">
        <%--  <div class="card-header">
                        Patch Status Counts                             
                                                                       
        <asp:Label ID="lblTotalRecord" runat="server" CssClass=" pull-right" Text="0"></asp:Label>
                        <asp:Label ID="lblTotalHead" runat="server" CssClass=" pull-right" Text="Total Count :"></asp:Label>
                        <asp:ImageButton ID="btnExport" runat="server" ImageUrl="~/Images/New folder/excelnew.png" CssClass="pull-right" OnClick="btnExport_Click" />


                    </div>--%>

        <div class="card-body">
            <div class="d-flex align-items-start justify-content-between mb-3 d-none">
                <div class="">
                    <h6 class="mb-0">
                        <asp:Label ID="Label6" runat="server" Text="Feedback Report"></asp:Label>
                    </h6>
                </div>
                <asp:LinkButton ID="btnExport" runat="server" CssClass="btn btn-sm btn-outline-secondary"  OnClick="btnExport_Click" >Export <i class="fa-solid fa-download"></i></asp:LinkButton>
            </div>
            <div class="row mt-2">

                <div class="col-md-2 d-none">


                    <asp:Label ID="lblTotalHead" runat="server" CssClass="ml-1" Text="Total :"></asp:Label>
                    <asp:Label ID="lblTotalRecord" runat="server" CssClass="" Text="0"></asp:Label>


                </div>
                <div class="col-md-12">
                    <div class="table-responsive table-container">
                         <asp:GridView ID="gvPatchStatus" runat="server" AutoGenerateColumns="true" Width="100%"
     CssClass="data-table table table-striped border table-sm text-nowrap" AllowPaging="true" PageSize="100" OnPageIndexChanging="gvPatchStatus_PageIndexChanging">
 </asp:GridView>
                    </div>
                </div>
            </div>
          



        </div>
    </div>

    <div class="modal fade" id="basicModal" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg" style="width: auto; max-width: 98%">
            <div class="modal-content">
                <%-- <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h6 class="modal-title" id="myModalLabel">Patch Details</h6>
                </div>--%>

                <div class="col-md-12 graphs" style="padding: 0px">
                    <div class="xs">
                        <div class="well1 white" style="padding: 0px">
                            <div class="card card-default">
                                <%--    <div class="card-header">
                                    <asp:Label ID="lblsofname" runat="server" Text=""></asp:Label>
                                    <asp:Label ID="lblTotal" runat="server" CssClass="pull-right control-label" Text="0"></asp:Label>
                                    <asp:Label ID="Label4" runat="server" CssClass="pull-right control-label" Text="Total: "></asp:Label>
                                    
                                </div>--%>
                                <div class="card-body">
                                    <div class="row">
                                        <div class="col-md-1 offset-11">
                                            <div class="btn btn-sm ml-2 ">
                                                <label class="mr-3 mb-0">CLOSE</label>
                                                <button type="button" class="close fa-pull-right btn-outline-danger mb-1" onclick="javascript:window.location.reload()" data-dismiss="modal" aria-hidden="true">&times;</button>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row mt-2">
                                        <div class="col-md-3">

                                            <asp:Label ID="lblsofname" runat="server" Text="Details" Font-Size="Larger" ForeColor="Black"></asp:Label>

                                        </div>
                                        <div class="col-md-7">
                                        </div>
                                        <div class="col-md-2 ">
                                            <div class="btn btn-sm elevation-1 ml-4" style="padding: 0px; margin-bottom: 10px; padding-top: 1px">
                                                <label class="mr-3  mb-0">Export</label>

                                            </div>
                                            <%--</div>
                                        <div class="col-md-1  ">--%>
                                            <asp:Label ID="Label4" runat="server" CssClass=" control-label" Text="Total: "></asp:Label>
                                            <asp:Label ID="lblTotal" runat="server" CssClass=" control-label" Text="0"></asp:Label>


                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
        </div>
    </div>

</asp:Content>


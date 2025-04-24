<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmOrgWiseTickets.aspx.cs" Inherits="Reports_frmOrgWiseTickets" %>

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
    <asp:ScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <section class="content">
                <div class="card mb-1">
                    <div class="card-body">
                        <div class="row gx-2 gy-3">
                            <div class="col-md-3 grid_box1">
                                <label class="form-label ">
                                    Select Desk 
                    <asp:RequiredFieldValidator Visible="false" ID="RequiredSDDrop" runat="server" ControlToValidate="DropDesks" InitialValue="0" ErrorMessage="Required" ForeColor="Red" ValidationGroup="searchbtn"></asp:RequiredFieldValidator>
                                </label>
                                <asp:DropDownList ID="DropDesks" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2 grid_box1">
                                <label class="form-label ">
                                    From
                                <asp:RequiredFieldValidator ID="RequiredtxtFrom" runat="server" ControlToValidate="txtFrom" ErrorMessage="Required" ForeColor="Red" ValidationGroup="searchbtn"></asp:RequiredFieldValidator>
                                </label>
                                <div class="input-group mb-3">
                                    <asp:TextBox ID="txtFrom" runat="server"  CssClass="form-control form-control-sm datepicker" autocomplete="off" ></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-2 grid_box1">
                                <label class="form-label ">
                                    To
                                <asp:RequiredFieldValidator ID="RequiredSDtxtTo" runat="server" ControlToValidate="txtTo" ErrorMessage="Required" ForeColor="Red" ValidationGroup="searchbtn"></asp:RequiredFieldValidator>
                                </label>
                                <div class="input-group mb-3">
                                    <asp:TextBox ID="txtTo" runat="server"  CssClass="form-control form-control-sm datepicker" autocomplete="off" ></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-2">
                                <label class="form-label opacity-0 ">..</label><br />
                                <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_Click" ValidationGroup="searchbtn" CssClass="btn btn-sm btn-outline-secondary" />
                            </div>

                            <div class="clearfix"></div>
                        </div>
                    </div>
                </div>




                <div class="card">
                    <div class="card-body">
                        <div class="d-flex align-items-start justify-content-between mb-3">
                            <div class="">
                                <h6 class="mb-0">
                                    <asp:Label ID="Label1" runat="server" Text="All Ticket Details"></asp:Label>
                                </h6>
                            </div>
                            <div class="d-flex  align-items-start justify-content-between">
                                <div class=" ">
                                    <asp:Label ID="Label4" runat="server" CssClass=" " Font-Size="Medium" Text="Total: "></asp:Label>
                                    <asp:Label ID="lblTotalCount" runat="server" CssClass="" Font-Size="Medium" Text="0"></asp:Label>
                                    &nbsp;
                            <asp:LinkButton ID="ImageBtnExport" runat="server" CssClass="btn btn-sm btn-outline-secondary" OnClick="ImageBtnExport_Click">Export <i class="fa-solid fa-download"></i></asp:LinkButton>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-12">
                            <div class="table-responsive table-container">
                                <asp:GridView ID="gvAllTickets" runat="server" CssClass="data-table table table-striped border table-sm text-nowrap">
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="clearfix">
                </div>

            </section>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSearch" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>


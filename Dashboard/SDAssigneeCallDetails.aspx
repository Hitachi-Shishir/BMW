<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="SDAssigneeCallDetails.aspx.cs" Inherits="SDAssigneeCallDetails" %>

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
      [data-bs-theme=semi-dark]   td > a {
            color: #686868;

        }    [data-bs-theme=light]   td > a {
            color: #686868;

        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">

    <div class="card mb-1">
        <div class="card-body">
            <div class="row g-2">
                <div class="col-md-3 ">
                    <label class="form-label ">
                        Select Desk
                        <asp:RequiredFieldValidator ID="RequiredSDDrop" runat="server" ControlToValidate="DropDesks" InitialValue="0" ErrorMessage="Required" ForeColor="Red" ValidationGroup="SU"></asp:RequiredFieldValidator>
                    </label>
                    <asp:DropDownList ID="DropDesks" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field description-label">
                    </asp:DropDownList>
                </div>

                <div class="col-md-2 ">
                    <asp:Label ID="lblError" runat="server"></asp:Label>
                    <label class="form-label ">
                        From
                        <asp:RequiredFieldValidator ID="RequiredFieldtxtFrom" runat="server" ControlToValidate="txtFrom" ErrorMessage="Required" ForeColor="Red" ValidationGroup="SU"></asp:RequiredFieldValidator>
                    </label>
                    <asp:TextBox AutoCompleteType="None" runat="server" Placeholder="" ID="txtFrom" class="form-control form-control-sm datepicker"></asp:TextBox>
                </div>
                <div class="col-md-2 ">
                    <label class="form-label ">
                        To
                        <asp:RequiredFieldValidator ID="RequiredtxtTo" runat="server" ControlToValidate="txtTo" ErrorMessage="Required" ForeColor="Red" ValidationGroup="SU"></asp:RequiredFieldValidator>
                    </label>
                    &nbsp;
                            <asp:TextBox AutoCompleteType="None" runat="server" Placeholder="" ID="txtTo" class="form-control form-control-sm datepicker"></asp:TextBox>
                </div>
                <div class="col-md-2">
                    <label class="form-label opacity-0 ">fksdklfja </label>
                    <br />
                    <asp:Button ID="btnSearch" runat="server" Text="Search" ValidationGroup="SU" CssClass="btn btn-sm btn-secondary" OnClick="btnSearch_Click" />
                </div>
                <div class="col-md-12 ">

                    <asp:GridView ID="gvOpenClosed" runat="server" Class="table table-bordered table-sm" AutoGenerateColumns="False">
                        <Columns>
                            <asp:BoundField ItemStyle-Width="150px" DataField="Total" HeaderText="Total" />
                            <asp:BoundField ItemStyle-Width="150px" DataField="Open" HeaderText="Open" />
                            <asp:BoundField ItemStyle-Width="150px" DataField="Closed" HeaderText="Closed" />
                            <asp:BoundField ItemStyle-Width="150px" DataField="Closed Percent" HeaderText="Closed %" DataFormatString="{0:0}%" />
                        </Columns>
                        <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>


    <div class="row gx-2">
        <div class="col-md-12">
            <div class="card mb-1">
                <div class="card-header">
                    Status Wise Tickets
         <asp:Button ID="btnchartcallstatus" runat="server" OnClick="btnchartcallstatus_Click" Style="visibility: hidden;" />
                </div>
                <div class="card-body">
                    <div id="LineGraph"></div>
                </div>

            </div>
        </div>
        <div class="col-md-6">
            <div class="card mb-1">
                <div class="card-body">
                    <div class="d-flex align-items-start justify-content-between mb-2">
                        Assignee Wise Tickets
         <%--<asp:LinkButton ID="ImgBtnExport" runat="server" class="btn btn-sm btn-outline-secondary "  OnClick="ImgBtnExport_Click" >Excel <i class="fa-solid fa-download"></i></asp:LinkButton>--%>
                    </div>

                    <div class="table-responsive table-container" style="height: 200px">
                        <asp:GridView ID="gvAssigneeCallsStatus" runat="server" Class="table border text-nowrap  table-sm" OnDataBound="gvAssigneeCallsStatus_DataBound" ShowFooter="True">
                            <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <div class="card mb-1">
                <div class="card-body">
                    Assignee Wise Aging
   <div class="table-responsive table-container mt-3" style="height: 197px">
       <asp:GridView ID="gvCallAging" runat="server" Class="table border text-nowrap  table-sm" AutoGenerateColumns="False" OnRowCommand="gvCallAging_RowCommand" ShowFooter="True">
           <Columns>
               <asp:BoundField DataField="Assignee" HeaderText="Assignee" />
               <asp:TemplateField HeaderText="0 to 3 Days">
                   <ItemTemplate>
                       <asp:LinkButton runat="server" ID="lnkView0to3" CommandArgument='<%#Eval("Assignee") %>' Text='<%#(String.IsNullOrEmpty(Eval("0 to 3 Days").ToString()) ? "0" : Eval("0 to 3 Days"))%>'
                           CommandName="VIEW0to3">
                                                     View Deal</asp:LinkButton>
                   </ItemTemplate>
               </asp:TemplateField>
               <asp:TemplateField HeaderText="4 to 7 Days">
                   <ItemTemplate>
                       <asp:LinkButton runat="server" ID="lnkView4to7" CommandArgument='<%#Eval("Assignee") %>' Text='<%#(String.IsNullOrEmpty(Eval("4 to 7 Days").ToString()) ? "0" : Eval("4 to 7 Days"))%>'
                           CommandName="VIEW4to7">
                                                     View Deal</asp:LinkButton>
                   </ItemTemplate>
               </asp:TemplateField>
               <asp:TemplateField HeaderText="8 to 10 Days">
                   <ItemTemplate>
                       <asp:LinkButton runat="server" ID="lnkView8to10" CommandArgument='<%#Eval("Assignee") %>' Text='<%#(String.IsNullOrEmpty(Eval("8 to 10 Days").ToString()) ? "0" : Eval("8 to 10 Days"))%>'
                           CommandName="VIEW8to10">
                                                     View Deal</asp:LinkButton>
                   </ItemTemplate>
               </asp:TemplateField>
               <asp:TemplateField HeaderText="More than 10 Days">
                   <ItemTemplate>
                       <asp:LinkButton runat="server" ID="lnkViewMore10Days" CommandArgument='<%#Eval("Assignee") %>' Text='<%#(String.IsNullOrEmpty(Eval("More than 10 Days").ToString()) ? "0" : Eval("More than 10 Days"))%>'
                           CommandName="VIEWMore10Days">
                                                     View Deal</asp:LinkButton>
                   </ItemTemplate>
               </asp:TemplateField>
           </Columns>
           <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
       </asp:GridView>
   </div>
                </div>
            </div>
        </div>

    </div>
    <div class="col-md-12">
        <div class="card mb-1">
            <div class="card-body">
                <asp:Label ID="lblTotalHead" runat="server" Font-Size="Large" CssClass=" form-label" Text="Total: " hidden></asp:Label>
                <asp:Label ID="lblTotal" Font-Size="Large" runat="server" CssClass=" form-label" Text="0" hidden></asp:Label>
                <asp:GridView ID="gvCallAgingDetails" runat="server" Class="data-table table-striped table-sm table table-head-fixed text-nowrap border" OnDataBound="gvAssigneeCallsStatus_DataBound" ShowFooter="True">
                    <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
                </asp:GridView>
            </div>
        </div>
    </div>

   
    <asp:Literal ID="chartScript" runat="server" />
    <asp:Literal ID="ltrcallstatus" runat="server" />
    <asp:HiddenField ID="hdnfldVariable" runat="server" />
        <script src="<%= ResolveUrl("~/assetsdata/plugins/apexchart/apexcharts.min.js") %>"></script>


</asp:Content>


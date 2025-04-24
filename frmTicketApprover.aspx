<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmTicketApprover.aspx.cs" Inherits="frmTicketApprover" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
              .dataTables_filter {
          margin-top: -29px !important;
      }
      .dt-buttons > .btn-outline-secondary{
          padding:0.25rem 0.5rem!important;
          font-size: 0.875rem!important;
	
}
      .pagination{
	--bs-pagination-padding-x: 0.5rem;
	--bs-pagination-padding-y: 0.25rem;
	--bs-pagination-font-size: 0.875rem;
	--bs-pagination-border-radius: var(--bs-border-radius-sm);
    /*margin-top: -1.7rem!important;*/
}
   
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
        <div class="xs">
            <div class="well1 white">
                <div class="Compose-Message">
                    <div class="card card-default">
                        <div class="card-header d-none">
                            <asp:Label ID="labelcomp" runat="server" Text="Pending Requests" Font-Size="Large"></asp:Label>
                        </div>
                        <div class="card-body">
                            <div class="row">
                                <div class="col-md-12 ">
                                    <div class="table-responsive table-container" >
                                        <asp:GridView ID="gvHRRequest" runat="server" AutoGenerateColumns="false" CssClass="data-table table table-striped border table-sm text-nowrap" OnRowCommand="gvHRRequest_RowCommand" OnRowDataBound="gvHRRequest_RowDataBound" DataKeyNames="TicketNumber">
                                            <Columns>
                                                <asp:TemplateField HeaderText=" Request Type">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDesk" runat="server" Text='<%# Eval("ServiceDesk") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Request Number" DataField="TicketNumber" />
                                                <asp:BoundField HeaderText="Status" DataField="Status" />
                                                <asp:TemplateField HeaderText="Approve">
     <ItemTemplate>
         <asp:Label ID="lblprov" runat="server" Visible="false"></asp:Label>
         <asp:LinkButton ID="btn_Prove" CssClass="badge bg-success" runat="server" CommandName="Approve" CommandArgument='<%#Eval("TicketNumber") %>' Text="Approve" Visible='<%# Eval("Approval1Status").ToString() == "Pending" ? true : false %>' OnClientClick="return confirm('Are you sure you want to approve this Ticket?');" />
     </ItemTemplate>
 </asp:TemplateField>
 <asp:TemplateField HeaderText="Reject">
     <ItemTemplate>
         <asp:Label ID="lblrej" runat="server" Visible="false"></asp:Label>
         <asp:LinkButton ID="btn_Reject" runat="server" CssClass="badge bg-danger" CommandName="Reject" CommandArgument='<%#Eval("TicketNumber") %>' Text="Reject" Visible='<%# Eval("Approval1Status").ToString() == "Pending" ? true : false %>' OnClientClick="return confirm('Are you sure you want to Reject this Ticket?');"/>
     </ItemTemplate>
 </asp:TemplateField>
 					<asp:TemplateField HeaderText="More Details">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btn_Details" runat="server" CommandName="Details" CssClass="badge bg-light" CommandArgument='<%#Eval("TicketNumber") %>' Text="Details" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Summary" DataField="Summary" />
                                                <asp:BoundField HeaderText="Emp Name" DataField="SubmitterName" />
                                                <asp:BoundField HeaderText="Emp EmailID" DataField="SubmitterEmail" />
                                                <asp:BoundField HeaderText="Category" DataField="Category" />
                                                <asp:BoundField HeaderText="Department" DataField="Department" />
                                                <asp:BoundField HeaderText="Location" DataField="location" />
                                                <asp:BoundField HeaderText="Priority" DataField="Priority" />
                                                <asp:TemplateField HeaderText=" Organization">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOrgFk" runat="server" Text='<%# Eval("Org_ID") %>' Visible="false"></asp:Label>
                                                        <asp:Label ID="lblOrgName" runat="server" Text='<%# Eval("OrgName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
 					<asp:TemplateField HeaderText="CurrentApproval Level">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbllevel" runat="server" Text='<%# Eval("CurrentApprovalLevel") %>'></asp:Label>
                                                    
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Created Time" DataField="CreationDate" DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                                               
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>

                            <div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true">
                                <div class="modal-dialog modal-lg modal-dialog-centered" style="max-width: 30%">
                                    <div class="modal-content">
                                        <div class="modal-header" id="ErrorDiv" runat="server">
                                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                                            <h6 class="modal-title" id="myModalLabel">Message</h6>
                                        </div>

                                        <div class="modal-body">
                                            <asp:Label ID="lblerrorMsg" runat="server" Text=""></asp:Label>
                                        </div>

                                        <div class="modal-footer">
                                            <asp:Button ID="btnOK" runat="server" CssClass="btn btn-sm btn-danger warning_3" OnClick="btnOK_Click" Text="OK" />
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


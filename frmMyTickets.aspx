<%@ Page Language="C#" AutoEventWireup="true" CodeFile="frmMyTickets.aspx.cs" Inherits="frmMyTickets" %>

<!doctype html>
<html lang="en">
  

<%--<html xmlns="http://www.w3.org/1999/xhtml">--%>
<head runat="server">

    <meta charset="utf-8" />
<meta name="viewport" content="width=device-width, initial-scale=1" /> 
<link rel="stylesheet" href="<%= ResolveUrl("~/Asset/css/bootstrapv5.min.css") %>">
    <link rel="stylesheet" href="<%= ResolveUrl("~/assets/fontawesome/css/fontawesome.css") %>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/assets/fontawesome/css/brands.css") %>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/assets/fontawesome/css/solid.css") %>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/assets/fontawesome/css/sharp-thin.css") %>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/assets/fontawesome/css/duotone-thin.css") %>" />
    <link rel="stylesheet" href="<%= ResolveUrl("~/assets/fontawesome/css/sharp-duotone-thin.css") %>" />


    <link rel="stylesheet" href="<%= ResolveUrl("~/assetsdata/plugins/datatable/css/dataTables.bootstrap5.min.css") %>">



    <style>
          .back-button {
      position: fixed;
      top: 20px;
      right: -1px;
      background-color: #007bff;
      color: white;
      border: none;
      padding: 10px 15px;
      font-size: 13px;
      cursor: pointer;
      z-index: 1000;
      border-radius: 2rem 0 0 2rem;
  }

      .back-button:hover {
          background-color: #0056b3;
      }

              .submit-button {
  
    background-color: #28a745;
    color: white;
    border: none;
    padding: 10px 15px;
    font-size: 13px;
    cursor: pointer;
   
    border-radius: 2rem ;
}

    .submit-button:hover {
        background-color:#218838;
    }
        table.dataTable td, table.dataTable th {
           table.dataTable td, table.dataTable th {
            color: #3d3d3d !important;
        }
        th {
  
    background: rgb(195 195 195) !important;
}
        .pagination {
    --bs-pagination-padding-x: 0.5rem;
    --bs-pagination-padding-y: 0.25rem;
    --bs-pagination-font-size: 0.875rem;
    --bs-pagination-border-radius: var(--bs-border-radius-sm);
    /*margin-top: -1.7rem !important;*/
}
        
        .table-responsive::-webkit-scrollbar {
            width: 3px !important;
            height: 4px !important;
        }

       .table-responsive::-webkit-scrollbar-track {
            background-color: #dddddd !important
        }

       .table-responsive::-webkit-scrollbar-thumb {
            background-color: #565656 !important;
            border-radius: 0px !important;
        }
        .dataTables_length {
   margin-bottom: -23px !important;
 }

 
    

    </style>
</head>
<body style="background-image: url(Asset/img/bg005.jpg); height: auto; background-size: cover; background-repeat: round;">
    <form id="form11" runat="server">


        <div class="container-fluid mt-3">
        <div class="card " style="height:100vh">
            <div class="card-header " style="background-color: #2c668d2e;">
                <div class="card-tittle row">
                    <div class="col-8">
                        <asp:Label ID="lblsofname" runat="server" Text="My Ticket Details" CssClass="h6 mb-0"></asp:Label>
                    </div>
                    <div class="col-4 text-end">
                        <asp:LinkButton ID="lnkbtnHome" class="back-button" runat="server" OnClick="lnkbtnHome_Click"> 

<i class="fas fa-arrow-left"></i></asp:LinkButton>

                    </div>
                </div>
            </div>
			<div class="card-body">
				<div class="row">
					<div class="col-md-3 p-2">
                        <asp:Button ID="btnCreatTicket" runat="server" CssClass="submit-button" Text="New Incident" OnClick="btnCreatTicket_Click" />
					</div>

				</div>

				<div class="row " hidden>

					<div class="col-md-3 p-2">

						<label for="staticEmail">
							Service Desk  <span class="text-danger">*</span>
						</label>
						<asp:DropDownList ID="ddlRequestType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlRequestType_SelectedIndexChanged" CssClass="form-control form-control-sm chzn-select"></asp:DropDownList>
					</div>
					<div class="col-md-9 text-end mb-2" hidden>
						<div class="btn btn-sm elevation-1 ml-1 ">
							<asp:ImageButton ID="ImgBtnExport" ToolTip="Export" runat="server" ImageUrl="~/Images/New folder/excelnew.png" CssClass="fa-pull-right btn-outline-success mr-1" OnClick="ImgBtnExport_Click" />
						</div>
						<div class="btn btn-sm btn-outline-secondary"><i class="fas fa-download"></i>Excel</div>
					</div>
				</div>
				<div class="table-responsive table-container">
					<asp:GridView ID="gvAllTickets" runat="server" CssClass="data-table table table-striped border table-sm text-nowrap dataTable no-footer" PageSize="10"
						AutoGenerateColumns="false" OnRowCommand="gvAllTickets_RowCommand">
<Columns>
							<asp:TemplateField HeaderText="TicketNumber" ControlStyle-CssClass="truncate-text" ItemStyle-Font-Size="Smaller" SortExpression="TicketNumber">
								<ItemTemplate>


									<asp:LinkButton
										ID="lblTicketNumber"
										runat="server"
										Text='<%# Eval("TicketNumber") %>'
										Font-Size="Small"
										CommandArgument='<%# Eval("TicketNumber") %>'
										CommandName="TicketClick" Width="100%"  ForeColor="Black"  ToolTip="Click on TicketNumber to modify ticket!!"> 
									</asp:LinkButton>
								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Summary" ItemStyle-Height="20px" SortExpression="Summary" ControlStyle-CssClass="truncate-text" Visible="true">
								<ItemTemplate>
									<asp:Label ID="lblSummary" runat="server" Text='<%# Eval("Summary") %>' Font-Size="Smaller" Visible="true"></asp:Label>

								</ItemTemplate>
							</asp:TemplateField>
							<asp:TemplateField HeaderText="Category" ItemStyle-Height="20px" SortExpression="Category" ControlStyle-CssClass="truncate-text" Visible="true">
								<ItemTemplate>
									<asp:Label ID="lblDesk" runat="server" Text='<%# Eval("ServiceDesk") %>' Font-Size="Smaller" Visible="false"></asp:Label>
									<asp:Label ID="lblOrgFK" runat="server" Text='<%# Eval("OrgId") %>' Font-Size="Smaller" Visible="false"></asp:Label>
									<asp:Label ID="lblCategoryName" runat="server" Text='<%# Eval("Category") %>' Font-Size="Smaller" Visible="true"></asp:Label>

								</ItemTemplate>
							</asp:TemplateField>
							<asp:BoundField DataField="Priority" HeaderText="Priority" SortExpression="Priority" ItemStyle-Font-Size="Smaller" />
							<asp:BoundField DataField="Severity" HeaderText="Severity" SortExpression="Severity" ItemStyle-Font-Size="Smaller" ItemStyle-Height="5px" />
							<asp:BoundField DataField="TechLoginName" HeaderText="Assigned Engineer" SortExpression="TechLoginName" ItemStyle-Height="5px" NullDisplayText="UnAssigned" ItemStyle-Font-Size="Smaller" />
							<asp:BoundField DataField="CreationDate" HeaderText="CreationDate" SortExpression="CreationDate" ItemStyle-Font-Size="Smaller" ItemStyle-Height="5px" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" />
							<asp:BoundField DataField="SubmitterName" HeaderText="SubmitterName" SortExpression="SubmitterName" ItemStyle-Height="5px" ItemStyle-Font-Size="Smaller" />
							<asp:BoundField DataField="SubmitterEmail" HeaderText="SubmitterEmail" SortExpression="SubmitterEmail" ItemStyle-Height="5px" ItemStyle-Font-Size="Smaller" />
							<asp:BoundField DataField="ClosedDate" HeaderText="ClosedDate " SortExpression="ClosedDate" ItemStyle-Height="5px" ItemStyle-Font-Size="Smaller" />

							<asp:BoundField DataField="location" HeaderText="Location " SortExpression="Location" ItemStyle-Height="5px" ItemStyle-Font-Size="Smaller" />
							<asp:BoundField DataField="Status" HeaderText="Status " SortExpression="Status" ItemStyle-Height="5px" ItemStyle-Font-Size="Smaller" />

						</Columns>
                          <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
					</asp:GridView>
				</div>
			</div>
        </div>

</div>

        <asp:HiddenField ID="hdfnUserEmail" runat="server" />
    </form>
    <script src="assetsdata/js/jquery-3.6.0.min.js"></script>
    <%--DATATABLE START--%>
    <script src="assetsdata/plugins/datatable/js/jquery.dataTables.min.js"></script>
    <script src="assetsdata/plugins/datatable/js/dataTables.bootstrap5.min.js"></script>
    <script src="assetsdata/js/bootstrap.bundle.min.js"></script>
    <script>
        $(document).ready(function () {
           $('.data-table1').DataTable({
    "order": [[0, "desc"]] // Sorts the first column in descending order
});
        });
    </script>
</body>
</html>

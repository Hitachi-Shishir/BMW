<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="frmAddKnowledgeBasenew.aspx.cs" Inherits="frmAddKnowledgeBasenew" ValidateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="icon" type="image/png" href="AgentTicketCSS/images/icons/favicon.ico" />
    <!-- summernote -->
    <link href="https://pcv-demo.hitachi-systems-mc.com:5723/assetsdata/css/bootstrap.min.css" rel="stylesheet" />
    <link href="https://pcv-demo.hitachi-systems-mc.com:5723/assetsdata/plugins/datatable/css/dataTables.bootstrap5.min.css" rel="stylesheet" />
       <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet"/>

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

     .note-editor {
         border: var(--bs-border-width) var(--bs-border-style) var(--bs-border-color) !important;
     }

     .form-label {
         display: inline-block;
         margin-bottom: .3rem !important;
         color: #414141;
         font-size: small;
         font-weight: 400;
     }
 </style>
    <style>
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
    </style>
</head>
<body style="background-image: url(Asset/img/bg005.jpg); height: auto; background-size: cover; background-repeat: round;">
    <form id="form1" runat="server">
        <div class="container-fluid mt-3">
            <%--<asp:LinkButton ID="lnkBack" class="back-button" runat="server" OnClick="lnkBack_Click"> <i class="fas fa-arrow-left"></i></asp:LinkButton>--%>

            <div class="card " style="height: 95vh">
                <div class="card-header " style="background-color: #2c668d2e;">
                    <div class="card-tittle row">
                        <div class="col-8">
                            <h6 class="mb-0 card-title">Knowledge Base</h6>
                        </div>
                        <div class="col-4 text-end">
                            <%--<a id="lnkbtnHome" data-toggle="tooltip" data-placement="top" title="Back">--%>
                            
                            <%--</a>--%>
                                        <asp:LinkButton ID="LinkButton1" class="back-button" runat="server" OnClick="lnkBack_Click"> <i class="fas fa-arrow-left"></i></asp:LinkButton>

                        </div>
                    </div>
                </div>

                <div class="card-body">

                    <asp:Panel ID="pnlShowPriority" runat="server">


                        <div class="row ">
                            <div class="col-md-11">
                                <asp:Label ID="lblsofname" runat="server" Text="Search Issues" Font-Size="Larger" ForeColor="Black" hidden></asp:Label>
                            </div>
                            <div hidden class="btn btn-sm elevation-1 ml-1 " style="padding: 0px; margin-bottom: 10px; padding-top: 1px">
                                <label class="mr-2 ml-1 mb-0">Export</label>
                                <asp:ImageButton ID="ImgBtnExport" runat="server" ImageUrl="~/Images/New folder/excelnew.png" CssClass="fa-pull-right btn-outline-success mr-1" OnClick="ImgBtnExport_Click" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12" style="text-align: center;">
                                <asp:Label ID="lblWelcomeMessage" runat="server" Text="Test" Font-Size="Larger" ForeColor="Black"></asp:Label>
                            </div>
                        </div>

                        <div class="table-responsive">
                            <asp:GridView GridLines="None" ID="gvResolution" runat="server" DataKeyNames="ID" AutoGenerateColumns="false" CssClass="data-table table-head-fixed text-nowrap table table-sm table-bordered" PageSize="10">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="10%">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Issue" HeaderText="Issue" NullDisplayText="NA" ItemStyle-Width="20%" />
                                    <asp:TemplateField HeaderText="Description" SortExpression="Description" ItemStyle-Font-Size="Smaller" ItemStyle-Width="20%">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDescription" runat="server" Font-Size="Smaller" Text='<%# Server.HtmlDecode(Eval("ResolutionDetail").ToString()) %>'> </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>

                        </div>

                    </asp:Panel>
                </div>
            </div>


        </div>
        <asp:HiddenField ID="hdnCategoryID" runat="server" />
        <script src="https://pcv-demo.hitachi-systems-mc.com:5723/assetsdata/js/jquery-3.6.0.min.js"></script>
        <script src="https://cdn.datatables.net/1.13.5/js/jquery.dataTables.min.js"></script>
        <script src="https://cdn.datatables.net/1.13.5/js/dataTables.bootstrap5.min.js"></script>
        <script>
            $(document).ready(function () {
                var table = $('.data-table').DataTable({
                    lengthChange: false,
                    // Initially hide the table body rows
                    "initComplete": function (settings, json) {
                        $(this).find('tbody').hide();
                    }
                });
                $('.dataTables_filter').parent().removeClass('col-sm-12 col-md-6');
                $('.dataTables_filter').parent().css({
                    'width': '100%', // Take full width
                    'text-align': 'center' // Center the filter
                });
                $('.dataTables_filter label').contents().filter(function () {
                    return this.nodeType === 3; // Text node
                }).remove();
                // Show/hide rows based on search input
                table.on('search.dt', function () {
                    var searchVal = table.search(); // Get the search box value
                    var filteredData = table.rows({ filter: 'applied' }).data();

                    if (filteredData.length > 0 && searchVal.trim() !== "") {
                        $(table.table().body()).show(); // Show rows if search value is not empty and there are filtered results
                    } else {
                        $(table.table().body()).hide(); // Hide rows if no data or search value is empty
                    }
                });
            });
        </script>



    </form>
</body>
</html>


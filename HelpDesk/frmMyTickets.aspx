<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmMyTickets.aspx.cs" Inherits="frmMyTickets" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
  
  <style>
      .dataTables_filter11 {
          margin-top: -29px !important;
      }
      .dt-buttons > .btn-outline-secondary{
          padding:0.25rem 0.5rem!important;
          font-size: 0.875rem!important;
	
}
      .pagination1{
	--bs-pagination-padding-x: 0.5rem;
	--bs-pagination-padding-y: 0.25rem;
	--bs-pagination-font-size: 0.875rem;
	--bs-pagination-border-radius: var(--bs-border-radius-sm);
    margin-top: -1.7rem!important;
}
   


  </style>
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
           
                    <asp:ImageButton ID="ImgBtnExport" runat="server" ImageUrl="Images/excelWH.png" class="btn btn-outline-secondary btn-sm d-none" OnClick="ImgBtnExport_Click" />
            <div class="card ">

                <div class="card-body">
                   

                    <div class="table-responsive table-container" >

                        <asp:GridView ID="gvAllTickets" runat="server" CssClass="data-table1 table table-striped border table-sm text-nowrap " 
                            AutoGenerateColumns="true" >

                            <%-- <RowStyle BackColor="White" BorderColor="#e3e4e6" BorderWidth="1px" Height="5px" />OnPageIndexChanging="gvAllTickets_PageIndexChanging"
                                    <FooterStyle BackColor="#EDEDED" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#EDEDED" ForeColor="#000000" HorizontalAlign="Left" Height="20px" VerticalAlign="NotSet" CssClass="header" />
                                    <SelectedRowStyle BackColor="#fff" Font-Bold="True" ForeColor="#000000" Height="5px" />
                                    <HeaderStyle BackColor="#EEEEEE" Font-Bold="True" ForeColor="#414141" Height="5px" BorderColor="WhiteSmoke" CssClass="header sorting_asc" Font-Size="Small" />
                                    <EditRowStyle BackColor="#e9edf2" BorderColor="#e3e4e6" BorderStyle="Solid" BorderWidth="1px" Height="5px" />
                                    <EmptyDataRowStyle HorizontalAlign="Center" BorderStyle="None" Height="5px" BorderColor="#EDEDED" BackColor="#EDEDED" />--%>
                            <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
                            <%--<AlternatingRowStyle BackColor="#EAEEFF" BorderColor="#e3e4e6" Height="5px" BorderStyle="Solid" BorderWidth="1px" />--%>
                        </asp:GridView>
                    </div>
                    <%--	</div>--%>
                </div>
            </div>
       

            <div class="modal" id="CategoryModal" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="card-header">
                            Select Colums
							<button type="button" class="close" data-dismiss="modal" onclick="javascript:window.location.reload()" aria-hidden="true">&times;</button>


                        </div>
                        <%--  <asp:UpdatePanel ID="UpdatePanel2" runat="server">
							<ContentTemplate>--%>
                        <div class="card-body">
                        </div>
                    </div>
                </div>
            </div>

        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="ImgBtnExport" />
        </Triggers>

    </asp:UpdatePanel>
<script>
        $(document).ready(function () {
           $('.data-table1').DataTable({
    "order": [[0, "desc"]] // Sorts the first column in descending order
});
        });
    </script>
    <script type="text/javascript">
        function grdHeaderCheckBox(objRef) {
            var grd = objRef.parentNode.parentNode.parentNode;
            var inputList = grd.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                    if (objRef.checked) {
                        inputList[i].checked = true;
                    }
                    else {
                        inputList[i].checked = false;
                    }
                }
            }
        }
    </script>
    <script type="text/javascript">
        function Showalert(imtype, imtitle) {
            // alert('Call JavaScript function from codebehind');
            // var typeof=type
            // var titleof = title;
            var Toast = Swal.mixin({
                toast: true,
                position: 'top-middle',

                showConfirmButton: false,
                showCloseButton: true,
                timer: 4000,


            });
            console.log("hello");
            Toast.fire({
                icon: imtype,
                title: imtitle
            });

            console.log("fire1234567");
        }
    </script>

</asp:Content>


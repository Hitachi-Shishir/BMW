<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmSDAccount.aspx.cs" Inherits="HelpDesk_frmSDAccount" %>

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
    margin-top: -1.7rem!important;
}
  </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="scriptmanager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
        

                       <div class="card">
                        <div class="card-body">
                            <div class="row">
                                <div class="col-12 mb-2">
                                    <div class="btn-group">
                                <asp:Repeater ID="rptAccount" runat="server" OnItemDataBound="rptAccount_ItemDataBound" OnItemCommand="rptAccount_ItemCommand">
                                    <ItemTemplate>
                                      
                                            <asp:Button ID="btn" runat="server" CssClass="btn btn-sm btn-outline-secondary " Width="100px" CommandName='<%# Eval("UserScope") %>' Text='<%# Eval("UserScope") %>'></asp:Button>
                                       
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                            </div>
                            </div> 
                                    <div class="card shadow-none border  ">
            <div class="card-body ">
                            <div class="row ">
                                <div class="col-md-4 d-none">
                                    <asp:Label ID="lblsofname" runat="server" Text="User Details" Font-Size="Larger" ForeColor="Black"></asp:Label>
                                </div>
                                <div class="col-md-6">
                                    <asp:Label ID="Label1" runat="server"></asp:Label>
                                    <asp:Label ID="Label3" runat="server"></asp:Label>
                                </div>
                                <div class="col-md-2 d-none">
                                    <div class="btn btn-sm elevation-1 ml-1 " style="padding: 0px; margin-bottom: 10px; padding-top: 1px">
                                        <label class="mr-3 ml-1 mb-0">Export</label>
                                        <%--<asp:ImageButton ID="ImgBtnExport" runat="server" ImageUrl="~/Images/New folder/excelnew.png" CssClass="fa-pull-right btn-outline-success mr-1" OnClick="ImgBtnExport_Click" />--%>
                                        <asp:LinkButton ID="ImgBtnExport" runat="server" OnClick="ImgBtnExport_Click">LinkButton</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                            <div class="table-responsive table-container">
                                <asp:GridView GridLines="None" ID="gvSDAccount" runat="server" AutoGenerateColumns="false" CssClass="data-table table table-striped border table-sm"
                                    Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="20">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="EmpID" HeaderText="EmpID" NullDisplayText="NA" />
                                        <asp:BoundField DataField="UserName" HeaderText="User Name" NullDisplayText="NA" />
                                        <asp:BoundField DataField="LoginName" HeaderText="Login Name" NullDisplayText="NA" />
                                        <asp:BoundField DataField="EmailID" HeaderText="Email ID" NullDisplayText="NA" />
                                    </Columns>
                                   <%-- <RowStyle BackColor="White" BorderColor="#e3e4e6" BorderWidth="1px" Height="10px" />
                                    <FooterStyle BackColor="#EDEDED" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#EDEDED" ForeColor="#000000" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#fff" Font-Bold="True" ForeColor="#000000" />
                                    <HeaderStyle BackColor="#EEEEEE" Font-Bold="True" ForeColor="#414141" Height="30px" CssClass="header" Font-Size="Small" />
                                    <EditRowStyle BackColor="#e9edf2" BorderColor="#e3e4e6" BorderStyle="Solid" BorderWidth="1px" />
                                    <AlternatingRowStyle BackColor="#EAEEFF" BorderColor="#e3e4e6" BorderStyle="Solid" BorderWidth="1px" />--%>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                    </div>
                    </div>
           
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="rptAccount" />
            <asp:PostBackTrigger ControlID="ImgBtnExport" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>


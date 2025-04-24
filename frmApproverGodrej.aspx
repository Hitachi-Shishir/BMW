<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmApproverGodrej.aspx.cs" Inherits="frmApproverGodrej" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:Panel ID="pnl" runat="server">
        <asp:Image ID="img" runat="server" Width="200px" Height="150px" ImageUrl="~/Images/check.png" CssClass="classLogo" Visible="false" />

        <div class="card">
      

              <%--  <div class="row">--%>
                    <div class="col-md-12">
                        <div class="table-responsive table-container">
                            <asp:Panel ID="pnlShowGriddAtawithoutChange" runat="server">
                                <asp:DetailsView ID="DetailsCheckInAsset" runat="server" CssClass="data-table1 table table-striped border table-sm text-nowrap"
                                    AllowPaging="True"
                                    AutoGenerateRows="False"
                                    GridLines="None">

                                    <Fields>
                                        <asp:BoundField DataField="TicketNumber" HeaderText="TicketNumber" />
                                        <asp:BoundField DataField="Summary" HeaderText="Summary" />
                                        <asp:BoundField DataField="Category" HeaderText="Category" />

                                        <asp:BoundField DataField="Priority" HeaderText="Priority :" />
                                        <asp:BoundField DataField="Severity" HeaderText="Severity" />
                                        <asp:BoundField DataField="SubmitterName" HeaderText="SubmitterName" />
                                        <asp:BoundField DataField="SubmitterEmail" HeaderText="SubmitterEmail" />

                                        <asp:BoundField DataField="CreationDate" HeaderText="CreationDate" DataFormatString="{0:yyyy-MM-dd}" />
                                        <asp:TemplateField HeaderText="Description" SortExpression="Description">

                                            <ItemTemplate>
                                                <asp:Label ID="lblDescription" runat="server" Text='<%# Server.HtmlDecode(Eval("Description").ToString()) %>'> </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                    </Fields>
                               <%--     <RowStyle BackColor="White" BorderColor="#e3e4e6" BorderWidth="1px" Height="5px" />
                                    <FooterStyle BackColor="#EDEDED" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#EDEDED" ForeColor="#000000" HorizontalAlign="Left" Height="5px" VerticalAlign="NotSet" CssClass="header" />
                                    <HeaderStyle BackColor="#EEEEEE" Font-Bold="True" ForeColor="#414141" Height="5px" BorderColor="WhiteSmoke" CssClass="header sorting_asc" Font-Size="Small" />
                                    <EditRowStyle BackColor="#e9edf2" BorderColor="#e3e4e6" BorderStyle="Solid" BorderWidth="1px" Height="5px" />
                                    <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#EDEDED" />--%>
                                    <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
                                    <%--<AlternatingRowStyle BackColor="#EAEEFF" BorderColor="#e3e4e6" Height="5px" BorderStyle="Solid" BorderWidth="1px" />--%>

                                </asp:DetailsView>
                            </asp:Panel>
                            <asp:Panel ID="pnlShowGridDataWithChange" runat="server">
                                <asp:DetailsView ID="DetailsChange" runat="server" CssClass="table table-bordered"
                                    AllowPaging="True"
                                    AutoGenerateRows="False"
                                    GridLines="None">

                                    <Fields>
                                        <asp:BoundField DataField="TicketNumber" HeaderText="TicketNumber" NullDisplayText="NA" />
                                        <asp:BoundField DataField="Summary" HeaderText="Summary" NullDisplayText="NA" />
                                        <asp:BoundField DataField="Category" HeaderText="Category" NullDisplayText="NA" />
                                        <asp:BoundField DataField="Priority" HeaderText="Priority :" NullDisplayText="NA" />
                                        <asp:BoundField DataField="Severity" HeaderText="Severity" NullDisplayText="NA" />
                                        <asp:BoundField DataField="SubmitterName" HeaderText="SubmitterName" NullDisplayText="NA" />
                                        <asp:BoundField DataField="SubmitterEmail" HeaderText="SubmitterEmail" NullDisplayText="NA" />
                                        <asp:BoundField DataField="CreationDate" HeaderText="Ticket Creation" DataFormatString="{0:yyyy-MM-dd}" />

                                        <asp:BoundField DataField="ChangeType" HeaderText="ChangeType" NullDisplayText="NA" />
                                        <asp:BoundField DataField="RequestForChange" HeaderText="RequestForChange" NullDisplayText="NA" />
                                        <asp:BoundField DataField="DurationFrom" HeaderText="DurationFrom" DataFormatString="{0:yyyy-MM-dd}" />
                                        <asp:BoundField DataField="DurationTo" HeaderText="DurationTo" DataFormatString="{0:yyyy-MM-dd}" />

                                        <asp:BoundField DataField="DownTimeDesc" HeaderText="DownTime Description" NullDisplayText="NA" />
                                        <asp:BoundField DataField="DownTimeFrom" HeaderText="DownTimeFrom" DataFormatString="{0:yyyy-MM-dd}" />
                                        <asp:BoundField DataField="DownTimeTo" HeaderText="DownTimeTo" DataFormatString="{0:yyyy-MM-dd}" />
                                    </Fields>
                                  <%--  <RowStyle BackColor="White" BorderColor="#e3e4e6" BorderWidth="1px" Height="5px" />
                                    <FooterStyle BackColor="#EDEDED" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#EDEDED" ForeColor="#000000" HorizontalAlign="Left" Height="5px" VerticalAlign="NotSet" CssClass="header" />
                                    <HeaderStyle BackColor="#EEEEEE" Font-Bold="True" ForeColor="#414141" Height="5px" BorderColor="WhiteSmoke" CssClass="header sorting_asc" Font-Size="Small" />
                                    <EditRowStyle BackColor="#e9edf2" BorderColor="#e3e4e6" BorderStyle="Solid" BorderWidth="1px" Height="5px" />
                                    <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#EDEDED" />--%>
                                    <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
                                    <%--<AlternatingRowStyle BackColor="#EAEEFF" BorderColor="#e3e4e6" Height="5px" BorderStyle="Solid" BorderWidth="1px" />--%>

                                </asp:DetailsView>
                            </asp:Panel>
                        </div>
                    </div>


                    <div class="row">
                        <div class="col-md-12">

                            <div class="card-header">
                                <div class="card-tittle">
                                    <asp:Label ID="Label14" runat="server" Text="Ticket Attachments" CssClass="h6"></asp:Label>
                                </div>
                            </div>
                            <div class="card-body">

                                <div class="table-responsive table-container" style="max-height: 300px">
                                    <asp:GridView ID="gvTicketAttachment" runat="server" AutoGenerateColumns="false" DataKeyNames="Ticketref" EmptyDataText="No files uploaded" CssClass="table table-bordered table-sm text-nowrap" OnRowCommand="gvTicketAttachment_RowCommand">
                                        <Columns>

                                            <asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="20">
                                                <ItemTemplate>
                                                    <%#Container.DataItemIndex+1 %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="EntryDt" HeaderText="Event Time"
                                                SortExpression="EntryDt" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" />
                                            <asp:BoundField DataField="Ticketref" HeaderText="Ticket Number"
                                                SortExpression="Ticketref" />

                                            <asp:TemplateField HeaderText="Download">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkDownload" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Filepath") %>'
                                                        CommandName="Download" Text='<%# Eval("FileName") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                        </Columns>
                                        <RowStyle BackColor="White" BorderColor="#e3e4e6" BorderWidth="1px" Font-Size="X-Small" Height="10px" />
                                        <FooterStyle BackColor="#EDEDED" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#EDEDED" ForeColor="#000000" HorizontalAlign="Center" Font-Size="X-Small" />
                                        <SelectedRowStyle BackColor="#fff" Font-Bold="True" ForeColor="#000000" Font-Size="X-Small" />
                                        <HeaderStyle BackColor="#EEEEEE" Font-Bold="True" ForeColor="#414141" Height="30px" CssClass="header" Font-Size="X-Small" />
                                        <EditRowStyle BackColor="#e9edf2" BorderColor="#e3e4e6" BorderStyle="Solid" BorderWidth="1px" />
                                        <AlternatingRowStyle BackColor="#EAEEFF" BorderColor="#e3e4e6" BorderStyle="Solid" BorderWidth="1px" Font-Size="X-Small" />
                                    </asp:GridView>

                                </div>
                                <div class="row gy-2 gx-3">
                                    <div class="col-md-4">
                                        <label class="form-label ">Current Stage  </label>
                                        <asp:DropDownList ID="ddlStage" runat="server" AutoPostBack="true" CssClass="form-select form-select-sm" OnSelectedIndexChanged="ddlStage_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                    <div class="col-md-4">
                                        <asp:Label ID="lblHodApprov" runat="server" class="form-label ">HOD Approval Required 

														 <asp:RequiredFieldValidator ID="rfvrdblst" runat="server" ControlToValidate="rdblst" ValidationGroup="Store" ForeColor="Red" ErrorMessage="*"></asp:RequiredFieldValidator>

                                        </asp:Label>
                                        <asp:RadioButtonList ID="rdblst" runat="server" CssClass="mt-2 w-50 border-0" RepeatDirection="Horizontal">

                                            <asp:ListItem Text="Yes" Value="Yes" ></asp:ListItem>
                                            <asp:ListItem Text="No" Value="No" ></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                     <div class="col-md-12">
     <label class="form-label ">
         Remarks 
																				<asp:RequiredFieldValidator ID="efvtxtremarks" runat="server" ControlToValidate="txtremarks" ValidationGroup="Store" ForeColor="Red" ErrorMessage="*"></asp:RequiredFieldValidator>

     </label>
     &nbsp;<asp:TextBox ID="txtremarks" runat="server" class="form-control form-control-sm" TextMode="MultiLine"></asp:TextBox>
 </div>
                                     <div class="col-md-12 text-end">
     <asp:Button ID="btnApprove" runat="server" Text="Approve" CssClass="btn btn-sm btn-grd-info" OnClick="btnApprove_Click" ValidationGroup="Store" />
     <asp:Button ID="btnReject" runat="server" Text="Reject" CssClass="btn btn-sm btn-grd-danger" OnClick="btnReject_Click" ValidationGroup="Store" />
 </div>

                                </div>
                            </div>


                        </div>


                        <div class="col-md-6">




                            <div class="row" hidden>
                                <div class="col-md-12">
                                    <label class="form-label ">Current Status : </label>
                                    <asp:DropDownList ID="ddlStatus" runat="server"></asp:DropDownList>
                                </div>

                            </div>
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:LinkButton ID="lnkDownload" runat="server" Text="" OnClick="lnkDownload_Click"></asp:LinkButton>
                                </div>
                            </div>

                        </div>


                    
                            <asp:Label ID="Label1" runat="server" Text=""  CssClass="label" ForeColor="#ff0000"></asp:Label>
                      
                      

                      
                        </div>

                 
                    <div class="clearfix">
                        <asp:Label ID="lblerrorMsg" runat="server" Text=""></asp:Label>
                    </div>
             

                <asp:Panel ID="pnlchangeparameters" runat="server">
                    <%--        ________________________ Grid will show Impact Plan   _____________________________--%>

                   
                        <div class="card-header border-top">

                            <asp:Label ID="Label4" runat="server" Text="Impact Details"  CssClass="h6 "></asp:Label>

                        </div>
                    <div class="card-body">
                  
                    <div class="row">
                        <div class="col-md-12">
                            <div class="table-responsive table-container">
                            <asp:GridView ID="gvImpactGrid" runat="server" CssClass="table table-head-fixed text-nowrap table-sm border " DataKeyNames="ID"
                                AutoGenerateColumns="false" AllowSorting="True">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="20">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderStyle-Width="120px" HeaderText="ImpactDetails" DataField="ImpactDescription" />
                                </Columns>
                               <%-- <RowStyle BackColor="White" BorderColor="#e3e4e6" BorderWidth="1px" Height="5px" />
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
                        </div>
                        </div>
                    </div>
                    <%--        ________________________ Grid will show RollOut Plan   _____________________________--%>
                     <div class="card-header border-top">
                                                     <asp:Label ID="Label2" runat="server" Text="Roll Out Details" class="h6 mb-0" ></asp:Label>

                         </div>
                  <div class="card-body">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="table-container table-responsive" style="max-height: 400px">
                            <asp:GridView ID="gvRollOutDetails" runat="server" CssClass="table table-head-fixed text-nowrap table-sm border  " DataKeyNames="ID"
                                AutoGenerateColumns="false" AllowSorting="True">
                                <Columns>
                                    <asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="20">
                                        <ItemTemplate>
                                            <%#Container.DataItemIndex+1 %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderStyle-Width="120px" HeaderText="RollOutDetails" DataField="RolloutDescription" />
                                </Columns>
                              <%--  <RowStyle BackColor="White" BorderColor="#e3e4e6" BorderWidth="1px" Height="5px" />
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
                        </div>
                    </div>
                    </div>


                    <%--        ________________________ Grid will show Task Plan   _____________________________--%>

                   <div class="card-header border-top">
                        

                            <asp:Label ID="Label3" runat="server" Text="Task Association Details" CssClass="h6 mb-0"></asp:Label>

                        </div>
                
                    <div class="card-body">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="table-responsive table-container" style="max-height: 400px">

                                <asp:GridView ID="gvTaskDetails" runat="server" CssClass="table table-head-fixed text-nowrap table-sm border " DataKeyNames="ID"
                                    AutoGenerateColumns="false" AllowSorting="True">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="20">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderStyle-Width="120px" HeaderText="Task ID" DataField="SubTicketRef" />

                                        <asp:BoundField HeaderStyle-Width="120px" HeaderText="Task Description" DataField="TaskDesc" />
                                        <asp:BoundField HeaderStyle-Width="120px" HeaderText="Status" DataField="Status" />
                                        <asp:BoundField HeaderStyle-Width="120px" HeaderText="Engineer Association" DataField="Assignee" />

                                    </Columns>
                                 <%--   <RowStyle BackColor="White" BorderColor="#e3e4e6" BorderWidth="1px" Height="5px" />
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
                        </div>
                        </div>
                    </div>
                </asp:Panel>

         
        </div>

    </asp:Panel>
</asp:Content>


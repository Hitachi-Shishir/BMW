<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmEmailAll.aspx.cs" Inherits="Admin_frmEmailAll" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="row">
                <div class="col-md-12 graphs">
                    <div class="xs">
                        <div class="well1 white">
                            <div class="Compose-Message">
                                <div class="card card-default">

                                    <div class="card-body">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <label>Email logs</label>
                                                <asp:Label ID="Label2" runat="server" Text="" CssClass="label" ForeColor="#ff0000"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12 graphs">
                                                <div class="card card-default">
                                                    <div class="card-body">
                                                        <div class="row ">
                                                            <div class="col-md-4">
                                                                <asp:Label ID="lblsofname" runat="server" Text="" Font-Size="Larger" ForeColor="Black"></asp:Label>
                                                                <asp:Label ID="Label1" runat="server"></asp:Label>
                                                                <asp:Label ID="Label3" runat="server"></asp:Label>
                                                            </div>
                                                            <div class="col-md-4">
                                                            </div>
                                                            <div class="col-md-2 ">
                                                                <div class="btn btn-sm elevation-1 " style="padding: 0px; margin-bottom: 10px; padding-top: 1px">
                                                                    <label class="mr-3  mb-0">Export</label>
                                                                    <asp:ImageButton ID="ImageBtnExport" runat="server" ImageUrl="~/Images/New folder/excelnew.png" CssClass="float-right ml-3 btn-outline-success" OnClick="ImageBtnExport_Click" />
                                                                </div>
                                                            </div>
                                                            <div class="col-md-2">
                                                                <label class=" pull-right  font_label">
                                                                    Total  : 
                         <asp:Label ID="lblTotalRecords" runat="server" CssClass="font_label" Text="0"></asp:Label>
                                                                </label>
                                                            </div>
                                                        </div>
                                                        <asp:GridView ID="gvAllEmailLogs" runat="server" DataKeyNames="ID" AutoGenerateColumns="false"
                                                            EmptyDataText="No record found" CssClass="data-table table table-striped table-bordered table-sm" >
                                                            <Columns>
                                                                <asp:BoundField DataField="from" HeaderText="From" />
                                                                <asp:BoundField DataField="to" HeaderText="To" />
                                                                <asp:BoundField DataField="cc" HeaderText="CC" />
                                                                <asp:BoundField DataField="bcc" HeaderText="BCC" />
                                                                <asp:BoundField DataField="subject" HeaderText="Subject" />
                                                                <%--                 <asp:BoundField DataField="bodyContent" HeaderText="bodyContent" />--%>
                                                                <asp:BoundField DataField="created" HeaderText="Created" />
                                                                <asp:BoundField DataField="sendStatus" HeaderText="Send Status" />
                                                                <asp:TemplateField HeaderText="Details">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkDetails" runat="server" CausesValidation="False" CommandArgument='<%# Eval("id") %>'
                                                                            CommandName="Details" Text="View" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>

                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="modal fade" id="basicModal" <%-- tabindex="-1"--%> role="dialog" aria-labelledby="myLargeModalLabel" aria-hidden="true">
                                            <div class="modal-dialog modal-lg" style="width: auto; max-width: 50%">
                                                <div class="modal-content">
                                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                        <ContentTemplate>
                                                            <div class="col-md-12 graphs" style="padding: 0px">

                                                                <div class="card card-default">
                                                                    <div class="card-header c">
                                                                        Details
                                                                  <button type="button" class="close" onclick="javascript:window.location.reload()" data-dismiss="modal" aria-hidden="true">&times;</button>
                                                                        <%-- <h6 class="modal-title" id="myModalLabel">Agent Details</h6>--%>
                                                                    </div>
                                                                    <div class="card-body">
                                                                        <asp:Repeater ID="Repeater1" runat="server">
                                                                            <ItemTemplate>
                                                                                <table class="table">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Label ID="Label1" runat="server" Text='<%#Eval("bodyContent") %>'></asp:Label></td>
                                                                                    </tr>
                                                                                </table>

                                                                            </ItemTemplate>
                                                                        </asp:Repeater>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </ContentTemplate>

                                                    </asp:UpdatePanel>
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
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="gvAllEmailLogs" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>


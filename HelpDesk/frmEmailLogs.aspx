<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmEmailLogs.aspx.cs" Inherits="HelpDesk_frmEmailLogs" ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
                     
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
            a:link, span.MsoHyperlink {
       text-decoration: none !important;
}  
      .modalBackground{
          background-color: var(--bs-body-bg);
              opacity: 80%;
      }
      .modalPopup{
                        box-shadow: rgba(0, 0, 0, 0.25) 0px 54px 55px, rgba(0, 0, 0, 0.12) 0px -12px 30px, rgba(0, 0, 0, 0.12) 0px 4px 6px, rgba(0, 0, 0, 0.17) 0px 12px 13px, rgba(0, 0, 0, 0.09) 0px -3px 5px;

      }
     .copyright  {
    
      color: var(--bs-body-color) !important;
   
}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>


            <div class="card">
                <div class="card-body">
                    <div class="row ">
                        <div class="col-md-12" style="border-bottom: none">
                            <asp:Label ID="lblheader" CssClass="headline" runat="server" Font-Size="Larger" Text="Email Logs Last 7 Days Data"></asp:Label>
                            <asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Red"></asp:Label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                           
                                <div class="parul">
                                    <asp:Button ID="btnpop" runat="server" Text="show" Style="display: none" />
                                    <asp:ModalPopupExtender ID="mp1" ClientIDMode="Static" runat="server" PopupControlID="Panel1" CancelControlID="btnclose" TargetControlID="btnpop" BackgroundCssClass="modalBackground">
                                    </asp:ModalPopupExtender>
                                    <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" align="center">
                                        <div class="card">
                                            <div class="card-body table-container" style="max-height: 90vh; min-height:20vh; overflow-y: auto; width: 100%">
                                                <div class="row">
                                                    <div class="col-sm-12">
                                                        <asp:Label ID="lblBody" runat="server"></asp:Label>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="card-footer text-end">
                                                <asp:LinkButton runat="server" ID="btnclose" Text="Close" CssClass="btn btn-grd-danger btn-sm text-white" style="    text-decoration: none;" />
                                            </div>
                                    </asp:Panel>

                                </div>
                             


                            </div>
                            <div class="table-responsive table-container">

                                <asp:GridView ID="gvAllTickets" runat="server" CssClass="data-table1 table table-striped border table-sm text-nowrap" DataKeyNames="ID"
                                    AutoGenerateColumns="False">
                                    <Columns>

                                        <asp:TemplateField HeaderText="View">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="hdnbodyContent" runat="server" Value='<%# HttpUtility.HtmlEncode(Eval("bodyContent")) %>' />
                                                <asp:LinkButton ID="lnkView" runat="server"  OnClick="lnkView_Click">
<i class="fa-solid fa-eye"></i>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="from" HeaderText="From" SortExpression="from" ItemStyle-Font-Size="Smaller" ItemStyle-Height="5px" />
                                        <asp:BoundField DataField="to" HeaderText="TO" ItemStyle-Font-Size="Smaller" ItemStyle-Height="5px" />
                                        <asp:BoundField DataField="subject" HeaderText="Subject" ItemStyle-Font-Size="Smaller" ItemStyle-Height="5px" />
                                        <asp:BoundField DataField="bodyFileName" HeaderText="Body File Name" ItemStyle-Font-Size="Smaller" ItemStyle-Height="5px" />
                                        <asp:BoundField DataField="created" HeaderText="Created" ItemStyle-Font-Size="Smaller" ItemStyle-Height="5px" />
                                        <asp:BoundField DataField="sent" HeaderText="Sent Time" ItemStyle-Font-Size="Smaller" ItemStyle-Height="5px" />
                                        <asp:BoundField DataField="OrgName" HeaderText="Organisation ID" ItemStyle-Font-Size="Smaller" ItemStyle-Height="5px" />

                                        <asp:TemplateField HeaderText="Sent">
                                            <ItemTemplate>
                                                <span class="sent-status badge bg-light text-white" data-status='<%# Eval("sendStatus") %>'>
                                                </span>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                      
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="gvAllTickets" />
        </Triggers>
    </asp:UpdatePanel>
    <script>
        document.addEventListener("DOMContentLoaded", function () {
            const sentStatusElements = document.querySelectorAll(".sent-status");
            sentStatusElements.forEach(element => {
                const status = parseInt(element.getAttribute("data-status"), 10);
                let text = "Unknown";
                let color = "black";
                switch (status) {
                    case 0:
                        text = "Pending";
                        color = "orange";
                        break;
                    case 2:
                        text = "Sent";
                        color = "green";
                        break;
                    case 4:
                        text = "Failure";
                        color = "red";
                        break;

                        //badge bg-warning , bg-danger, bg-success , bg-info classes names 
                }
                element.textContent = text;
                element.style.color = color;
            });
        });
    </script>

</asp:Content>


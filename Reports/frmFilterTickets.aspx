<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmFilterTickets.aspx.cs" Inherits="Reports_frmFilterTickets" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="container-fluid">


      
                <div class="card mb-1">
                    <div class="card-body">
                        <div class="row mb-2">
                            <div class="col-md-3 grid_box1">

                                <label class="form-label ">
                                    Select Desk 
                     				 <asp:RequiredFieldValidator ID="RequiredSDDrop" runat="server" ControlToValidate="DropDesks" InitialValue="0" ErrorMessage="*" ForeColor="Red" ValidationGroup="searchbtn"></asp:RequiredFieldValidator>
                                </label>

                                <asp:DropDownList ID="DropDesks" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-3">
                                <label class="form-label ">Select Column  </label>
                                <asp:DropDownList ID="ddlSearchItems" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field">
                                    <asp:ListItem Value="TicketNumber">TicketNumber</asp:ListItem>
                                    <asp:ListItem Value="Summary">Summary</asp:ListItem>
                                    <asp:ListItem Value="Category">Category</asp:ListItem>
                                    <asp:ListItem Value="Priority">Priority</asp:ListItem>
                                    <asp:ListItem Value="Stage">Stage</asp:ListItem>
                                    <asp:ListItem Value="Severity">Severity</asp:ListItem>
                                    <asp:ListItem Value="SubmitterName">Submitter Name</asp:ListItem>
                                    <asp:ListItem Value="TechLoginName">Engineer Name</asp:ListItem>
                                    <asp:ListItem Value="location">location</asp:ListItem>
                                    <asp:ListItem Value="Department">Department</asp:ListItem>

                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2 grid_box1" hidden>
                                <label class="form-label ">
                                    From 
                            			      <asp:RequiredFieldValidator ID="RequiredtxtFrom" runat="server" ControlToValidate="txtFrom" ErrorMessage="Required" ForeColor="Red" ValidationGroup="searchbtn"></asp:RequiredFieldValidator>
                                </label>
                                <div class="input-group ">
                                    <div class="input-group-prepend">
                                        <span class="input-group-text"><i class="fas fa-													calendar"></i></span>
                                    </div>
                                    <asp:TextBox ID="txtFrom" runat="server" CssClass="form-control form-											control-sm" autocomplete="off" ClientIDMode="static"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-2 grid_box1" hidden>
                                <label class="form-label ">
                                    Before
                                			  <asp:RequiredFieldValidator ID="RequiredSDtxtTo" runat="server" ControlToValidate="txtTo" ErrorMessage="Required" ForeColor="Red" ValidationGroup="searchbtn"></asp:RequiredFieldValidator>
                                </label>
                                <div class="input-group mb-3">
                                    <div class="input-group-prepend">
                                        <span class="input-group-text"><i class="fas fa-														calendar"></i></span>
                                    </div>
                                    <asp:TextBox ID="txtTo" runat="server" CssClass="form-control form-control-sm" autocomplete="off" ClientIDMode="static"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-2  mt-4 pt-2" hidden>
                                <asp:Button ID="btnSearch1" runat="server" Text="Search" ValidationGroup="searchbtn" CssClass="btn btn-sm savebtn" />
                            </div>
                            <div class="col-md-3">
                                <label class="form-label ">
                                    Enter Search Value 
                       <asp:RequiredFieldValidator ID="RequiredtxtSearch" runat="server" ControlToValidate="txtSearch" ErrorMessage="Required" ForeColor="Red" ValidationGroup="Search"></asp:RequiredFieldValidator>
                                </label>
                                <div class="input-group input-group-sm">
                                    <asp:TextBox ID="txtSearch" runat="server" class="form-control form-control-sm input-search" placeholder="Search..."></asp:TextBox>
                                
                                    <span class=" btn btn-sm btn-secondary"><asp:LinkButton  ID="btnSearch" runat="server"  OnClick="btnSearch_Click" ValidationGroup="Search"> <i class="fa-solid fa-search"></i></asp:LinkButton> </span>
                                </div>
                            </div>

                            <div class="clearfix"></div>
                        </div>
                    </div>
                </div>
           
       
                <div class="card">
                    <div class="card-header">
                        <h6 class="card-tittle mb-0">
                                                            <asp:Label ID="Label1" runat="server" Text="All Ticket Details" ></asp:Label>

                        </h6>
                    </div>
                    <div class="card-body">
                      
                          
                            <%--<div class="col-md-3">
                                <div class="btn btn-sm elevation-1 ml-2" style="padding: 0px; margin-bottom: 10px; padding-top: 0px">
                                    <label class="mr-1 ml-1 mb-0">Export</label>
                                    <asp:ImageButton ID="ImageBtnExport" runat="server" ImageUrl="~/Images/New folder/excelnew.png" OnClick="ImageBtnExport_Click" CssClass="pull-right ml-2 mt-0 btn-outline-success form-label" class="fa fa-download" />
                                </div>
                                <asp:Label ID="Label4" runat="server" CssClass="ml-1  " Font-Size="Medium" Text="Total: "></asp:Label>
                                <asp:Label ID="lblTotalCount" runat="server" CssClass="" Font-Size="Medium" Text="0"></asp:Label>
                            </div>--%>
                       
                        <div class="table-responsive table-container" style="height: 300px;">
                            <asp:GridView ID="gvAllTickets" runat="server" CssClass="data-table table table-striped border table-sm text-nowrap table-hover" OnPageIndexChanging="gvAllTickets_PageIndexChanging" PageSize="100" AllowPaging="True">
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <div class="clearfix">
              
        </div>
    </div>
</asp:Content>


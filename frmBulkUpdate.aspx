<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmBulkUpdate.aspx.cs" Inherits="frmBulkUpdate" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .note-editor.note-frame, .note-editor.note-airframe {
            border: var(--bs-border-width) var(--bs-border-style) var(--bs-border-color) !important;
            margin-bottom: 0 !important
        }

        .hidden-dropdown {
            visibility: hidden;
        }
    </style>
    <script src="assetsdata/plugins/summernote/jquery.js"></script>
    <link href="assetsdata/plugins/summernote/summernote-bs4.css" rel="stylesheet" />
<%--    <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.bundle.min.js"></script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">

    <div class="card mb-1">
        <div class="card-header">
            <h6 class="card-tittle mb-0">Bulk Update Ticket</h6>

        </div>
        <div class="card-body">

            <div class="row d-none">
                <div class="col-md-6 col-lg-12 col-sm-4">
                    <label class="cardheader"></label>
                    <asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Red"></asp:Label>
                </div>
            </div>
            <asp:Panel ID="showChangeControl" runat="server" hidden>
                <div class="row mb-1">
                    <div class="col-lg-12 col-md-6 col-sm-4">
                        <asp:Button ID="btnShowBasicDetails" runat="server" Text="Basic Details" CssClass="btn btn-sm btnEnabled" CausesValidation="false" OnClick="btnShowBasicDetails_Click" />
                        <asp:Button ID="btnImpactDetails" runat="server" Text="Impact Details" CssClass="btn btn-sm btnDisabled" CausesValidation="false" OnClick="btnImpactDetails_Click" />
                        <asp:Button ID="btnRolloutPlan" runat="server" Text="RollOut Details" CssClass="btn btn-sm btnDisabled" CausesValidation="false" OnClick="btnRolloutPlan_Click" />
                        <asp:Button ID="btnDowntime" runat="server" Text="Downtime" CssClass="btn btn-sm btnDisabled" CausesValidation="false" OnClick="btnDowntime_Click" />
                        <asp:Button ID="btnTaskAssociation" runat="server" Text="Task " CssClass="btn btn-sm btnDisabled" CausesValidation="false" OnClick="btnTaskAssociationShowPanel_Click" />


                    </div>
                </div>

            </asp:Panel>

            <asp:Panel ID="pnlIncident" runat="server">

                <div class="row gy-2 gx-3">
                    <div class="col-md-6">
                        <label for="staticEmail" class="form-label">
                            Priority 
                              
                        </label>

                        <asp:DropDownList ID="ddlPriority" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field"></asp:DropDownList>
                    </div>

                    <div class="col-md-6">
                        <label for="staticEmail" class="form-label">
                            Status  
                        </label>

                        <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field"></asp:DropDownList>
                    </div>
                    <div class="col-md-6">
                        <label for="staticEmail" class="form-label">
                            Location
                        </label>

                        <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field">
                            <asp:ListItem Selected="True" Text="---- Select Location ----" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Gurgaon" Value="Gurgaon"></asp:ListItem>
                            <asp:ListItem Text="Mumbai" Value="Mumbai"></asp:ListItem>
                            <asp:ListItem Text="Pune" Value="Pune"></asp:ListItem>
                            <asp:ListItem Text="Hyderabad" Value="Hyderabad"></asp:ListItem>
                            <asp:ListItem Text="Bangalore" Value="Chennai"></asp:ListItem>
                            <asp:ListItem Text="Chennai" Value="Chennai"></asp:ListItem>
                            <asp:ListItem Text="Lucknow" Value="Gurgaon"></asp:ListItem>
                            <asp:ListItem Text="Chandigarh" Value="Gurgaon"></asp:ListItem>
                            <asp:ListItem Text="Ahmedabad" Value="Mumbai"></asp:ListItem>
                            <asp:ListItem Text="Others" Value="Gurgaon"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-6">
                        <label for="staticEmail" class="form-label">
                            Department 
                              
                        </label>

                        <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field"></asp:DropDownList>
                    </div>
                    <div class="col-md-6">
                        <label for="staticEmail" class="form-label">
                            Resolution 
                                              <asp:RequiredFieldValidator ID="rfvddlResoultion" runat="server" ControlToValidate="ddlResoultion" ValidationGroup="Submit" ForeColor="Red" InitialValue="0" ErrorMessage="Required"></asp:RequiredFieldValidator>
                        </label>


                        <asp:DropDownList ID="ddlResoultion" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field"></asp:DropDownList>
                    </div>
                    <div class="col-md-6">
                        <label for="staticEmail" class="form-label">
                            Severity 
                        </label>

                        <asp:DropDownList ID="ddlSeverity" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field"></asp:DropDownList>
                    </div>
                </div>
                <div class=" row ">
                    <%-- this is area for Category--%>
                    <div class="col-md-6 col-6 ">
                        <div class="row gy-2 gx-3 mt-1">
                            <div id="divCategory1" class="col-md-12" runat="server">

                                <label for="staticEmail" class="form-label">

                                    <asp:Label ID="lblCategory1" runat="server" Text="Category 1 "></asp:Label>
                                </label>
                                <asp:DropDownList ID="ddlCategory1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCategory1_SelectedIndexChanged" CssClass="form-select form-select-sm single-select-optgroup-field"></asp:DropDownList>
                            </div>


                            <div id="divCategory2" class="col-md-12" runat="server">


                                <label for="staticEmail" class="form-label">
                                    <asp:Label ID="lblCategory2" runat="server" Text="Category 2 "></asp:Label>
                                </label>


                                <asp:DropDownList ID="ddlCategory2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCategory2_SelectedIndexChanged" CssClass="form-select form-select-sm single-select-optgroup-field"></asp:DropDownList>
                            </div>

                            <div id="divCategory3" class="col-md-12" runat="server">

                                <label for="staticEmail" class="form-label">
                                    <asp:Label ID="lblCategory3" runat="server" Text="Category 3 "></asp:Label>

                                </label>



                                <asp:DropDownList ID="ddlCategory3" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCategory3_SelectedIndexChanged" CssClass="form-select form-select-sm single-select-optgroup-field"></asp:DropDownList>
                            </div>

                            <div id="divCategory4" class="col-md-12" runat="server">

                                <label for="staticEmail" class="form-label">
                                    <asp:Label ID="lblCategory4" runat="server" Text="Category 4 "></asp:Label>
                                </label>


                                <asp:DropDownList ID="ddlCategory4" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory4_SelectedIndexChanged"></asp:DropDownList>
                            </div>

                            <div id="divCategory5" class="col-md-12" runat="server">

                                <label for="staticEmail" class="form-label">
                                    <asp:Label ID="lblCategory5" runat="server" Text="Category 5 "></asp:Label>
                                </label>

                                <asp:DropDownList ID="ddlCategory5" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field hidden-dropdown" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory5_SelectedIndexChanged"></asp:DropDownList>
                            </div>

                            <div class="form-group row " hidden>

                                <label for="staticEmail" class="form-label">
                                    Category 6 
                                </label>
                                <div class="col-sm-8 pr-5">


                                    <asp:DropDownList ID="ddlCategory6" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field "></asp:DropDownList>
                                </div>
                            </div>

                        </div>
                        <%-- this is area for Custom Field--%>
                        <div class="col-md-6 col-6">


                            <asp:Repeater ID="rptOddControl" runat="server" OnItemDataBound="rptOddControl_ItemDataBound">
                                <ItemTemplate>
                                    <div class="form-group row ">

                                        <label for="staticEmail" class="col-sm-4 fa-pull-left labelcolorl1 pl-4">
                                            <asp:Label ID="lbl" Text='<%# Eval("FieldValue") %>' Visible="false" runat="server"></asp:Label>
                                            <asp:Label ID="Label1" Text='<%# Eval("FieldName") %>' runat="server"></asp:Label>
                                            <asp:RequiredFieldValidator ID="rfv" runat="server" ControlToValidate="txt" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="UserScope"></asp:RequiredFieldValidator>
                                        </label>


                                        <div class="col-sm-8 pr-5">

                                            <asp:TextBox ID="txt" runat="server" CssClass="form-control  form-control-sm"></asp:TextBox>

                                        </div>

                                    </div>

                                    <%--		<asp:PlaceHolder ID="pnlOddTypeCustFlds" runat="server"></asp:PlaceHolder>--%>
                                </ItemTemplate>
                            </asp:Repeater>

                            <asp:Repeater ID="rptddlOddControl" runat="server" OnItemDataBound="rptddlOddControl_ItemDataBound">
                                <ItemTemplate>
                                    <div class="form-group row ">

                                        <label for="staticEmail" class="form-label">
                                            <asp:Label ID="lblOddlist" Text='<%# Eval("FieldValue") %>' Visible="false" runat="server"></asp:Label>
                                            <asp:Label ID="Label2" Text='<%# Eval("FieldName") %>' runat="server"></asp:Label>

                                            <asp:RequiredFieldValidator ID="rfvddl" runat="server" ControlToValidate="ddlOdd" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="UserScope"></asp:RequiredFieldValidator>
                                        </label>


                                        <div class="col-sm-8 pr-5">

                                            <asp:DropDownList ID="ddlOdd" runat="server" CssClass="form-control  form-control-sm chzn-select"></asp:DropDownList>

                                        </div>

                                    </div>

                                    <%--		<asp:PlaceHolder ID="pnlOddTypeCustFlds" runat="server"></asp:PlaceHolder>--%>
                                </ItemTemplate>
                            </asp:Repeater>
                            <asp:Repeater ID="rptEvenControl" runat="server" OnItemDataBound="rptEvenControl_ItemDataBound">
                                <ItemTemplate>
                                    <div class="form-group row ">

                                        <label for="staticEmail" class="form-label">
                                            <asp:Label ID="lbleven" Text='<%# Eval("FieldValue") %>' Visible="false" runat="server"></asp:Label>
                                            <asp:Label ID="Label3" Text='<%# Eval("FieldName") %>' runat="server"></asp:Label>

                                            <asp:RequiredFieldValidator ID="rfvddl" runat="server" ControlToValidate="txteven" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="UserScope"></asp:RequiredFieldValidator>
                                        </label>


                                        <div class="col-sm-8 pr-5">

                                            <asp:TextBox ID="txteven" runat="server" CssClass="form-control  form-control-sm" Text='<%# Eval("FieldMode") %>'></asp:TextBox>

                                        </div>

                                    </div>
                                    <%--<asp:PlaceHolder ID="pnlEvenTypeCustFlds" runat="server"></asp:PlaceHolder>--%>
                                </ItemTemplate>
                            </asp:Repeater>
                            <asp:Repeater ID="rptddlEvenControl" runat="server" OnItemDataBound="rptddlEvenControl_ItemDataBound">
                                <ItemTemplate>
                                    <div class="form-group row ">

                                        <label for="staticEmail" class="form-label">
                                            <asp:Label ID="lblEvenlist" Text='<%# Eval("FieldValue") %>' Visible="false" runat="server"></asp:Label>
                                            <asp:Label ID="Label4" Text='<%# Eval("FieldName") %>' runat="server"></asp:Label>

                                            <asp:RequiredFieldValidator ID="rfvddleveb" runat="server" ControlToValidate="ddlEven" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="UserScope"></asp:RequiredFieldValidator>
                                        </label>


                                        <div class="col-sm-8 pr-5">

                                            <asp:DropDownList ID="ddlEven" runat="server" CssClass="form-control  form-control-sm chzn-select"></asp:DropDownList>

                                        </div>

                                    </div>

                                    <%--		<asp:PlaceHolder ID="pnlOddTypeCustFlds" runat="server"></asp:PlaceHolder>--%>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                    <div class=" row gy-2 gx-3">
                        <div class="col-md-12">
                            <label for="exampleInputBorder" class="form-label">
                                Resolution
                            </label>

                            <asp:RequiredFieldValidator ID="rfvtxtResolution" runat="server" ControlToValidate="txtResolution" ValidationGroup="Submit" ForeColor="Red" ErrorMessage="*"></asp:RequiredFieldValidator>

                            <%--	<asp:TextBox ID="txtDescription" runat="server" CssClass="form-control form-control-sm" TextMode="MultiLine" MaxLength="500" Height="200px"></asp:TextBox>--%>
                            <textarea id="txtResolution" runat="server"></textarea>
                        </div>
                    </div>


                    <div class=" row gx-3 gy-2">
                        <div class="col-md-6">
                            <label for="exampleInputBorder" class="form-label">
                                Assignee 
                                              <asp:RequiredFieldValidator ID="rfvddlAssigne" runat="server" ControlToValidate="ddlAssigne" ValidationGroup="Submit" ForeColor="Red" ErrorMessage="Required" InitialValue="0"></asp:RequiredFieldValidator>
                            </label>

                            <asp:DropDownList ID="ddlAssigne" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field"></asp:DropDownList>
                        </div>

                    </div>
                    <asp:Panel ID="pnlChange" runat="server">
                        <div class="form-group row mb-0 pb-0">
                            <label for="staticEmail" class="form-label">
                                Change Type: 
                                           <asp:RequiredFieldValidator ID="rfvddlChangeType" runat="server" InitialValue="0" ControlToValidate="ddlChangeType" ValidationGroup="Addticket" ForeColor="Red" ErrorMessage="*"></asp:RequiredFieldValidator>
                            </label>
                            <div class="col-sm-4 pr-5">
                                <asp:DropDownList ID="ddlChangeType" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field"></asp:DropDownList>
                            </div>
                            <label for="staticEmail" class="form-label">
                                Reason For Change : 
                              
                                <asp:RequiredFieldValidator ID="rfvddlRFC" runat="server" ControlToValidate="ddlRFC" ErrorMessage="*" ForeColor="Red" InitialValue="0" ValidationGroup="Addticket"></asp:RequiredFieldValidator>
                            </label>
                            <div class="col-sm-4 pr-5">
                                <asp:DropDownList ID="ddlRFC" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group row ">
                            <label for="staticEmail" class="form-label">
                                Duration From: 
                                           <asp:RequiredFieldValidator ID="rfvtxtChangeDurationfrom" runat="server" ControlToValidate="txtChangeDurationfrom" ValidationGroup="Addticket" ForeColor="Red" ErrorMessage="*"></asp:RequiredFieldValidator>
                            </label>
                            <div class="col-sm-4 pr-5">
                                <div class="input-group ">
                                    <div class="input-group-prepend">
                                        <span class="input-group-text"><i class="fas fa-calendar"></i></span>
                                    </div>
                                    <asp:TextBox ID="txtChangeDurationfrom" runat="server" CssClass="form-control form-control-sm" MaxLength="10" autocomplete="off" ClientIDMode="static"></asp:TextBox>
                                </div>
                            </div>
                            <label for="staticEmail" class="form-label">
                                Duration To : 
                              
                                <asp:RequiredFieldValidator ID="rfvtxtChangeDurationTo" runat="server" ControlToValidate="txtChangeDurationTo" ErrorMessage="*" ForeColor="Red" ValidationGroup="Addticket"></asp:RequiredFieldValidator>
                            </label>
                            <div class="col-sm-4 pr-5">
                                <div class="input-group ">
                                    <div class="input-group-prepend">
                                        <span class="input-group-text"><i class="fas fa-calendar"></i></span>
                                    </div>
                                    <asp:TextBox ID="txtChangeDurationTo" runat="server" CssClass="form-control form-control-sm " MaxLength="10" autocomplete="off" ClientIDMode="static"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlCloud" runat="server">
                        <div class="form-group row ">
                            <label for="staticEmail" class="form-label">
                                Account ID: 
                                           <asp:RequiredFieldValidator ID="rfvtxtAccountID" runat="server" ControlToValidate="txtAccountID" ValidationGroup="Addticket" ForeColor="Red" ErrorMessage="*"></asp:RequiredFieldValidator>

                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                    ControlToValidate="txtAccountID" ErrorMessage="Not Greater than 12 digits!!" ForeColor="Red" ValidationGroup="Addticket"
                                    ValidationExpression="[0-9]{12}"></asp:RegularExpressionValidator>

                            </label>
                            <div class="col-sm-4 pr-5">
                                <asp:TextBox ID="txtAccountID" runat="server" TextMode="Number" CssClass="form-control form-control-sm"></asp:TextBox>
                            </div>
                            <label for="staticEmail" class="form-label">
                                User Email : 
                              
                                <asp:RequiredFieldValidator ID="rfvtxtUserEmail" runat="server" ControlToValidate="txtUserEmail" ErrorMessage="*" ForeColor="Red" ValidationGroup="Addticket"></asp:RequiredFieldValidator>
                            </label>
                            <div class="col-sm-4 pr-5">
                                <asp:TextBox ID="txtUserEmail" runat="server" CssClass="form-control form-control-sm "></asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group row ">
                            <label for="staticEmail" class="form-label">
                                Not As Submitter Email: 
					
                            </label>
                            <div class="col-sm-4 pr-5">
                                <asp:CheckBox ID="checkAnotherEmail" runat="server" CssClass="form-control form-control-sm"></asp:CheckBox>
                            </div>
                            <label for="staticEmail" class="form-label">
                                Email Change Reason : 
                              
                                <asp:RequiredFieldValidator ID="rfvtxtEmailReason" runat="server" ControlToValidate="txtEmailReason" ErrorMessage="*" ForeColor="Red" ValidationGroup="Addticket"></asp:RequiredFieldValidator>
                            </label>
                            <div class="col-sm-4 pr-5">
                                <asp:TextBox ID="txtEmailReason" runat="server" CssClass="form-control form-control-sm "></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row ">
                            <label for="staticEmail" class="form-label">
                                Duration From: 
                                           <asp:RequiredFieldValidator ID="rfvtxtDurationFrom" runat="server" ControlToValidate="txtDurationFrom" ValidationGroup="Addticket" ForeColor="Red" ErrorMessage="*"></asp:RequiredFieldValidator>
                            </label>
                            <div class="col-sm-4 pr-5">
                                <div class="input-group mb-3">
                                    <div class="input-group-prepend">
                                        <span class="input-group-text"><i class="fas fa-calendar"></i></span>
                                    </div>
                                    <asp:TextBox ID="txtDurationFrom" runat="server" CssClass="form-control form-control-sm" MaxLength="10" autocomplete="off" ClientIDMode="static"></asp:TextBox>
                                </div>
                            </div>
                            <label for="staticEmail" class="form-label">
                                Duration To : 
                              
                                <asp:RequiredFieldValidator ID="rfvtxtDurationto" runat="server" ControlToValidate="txtDurationto" ErrorMessage="*" ForeColor="Red" ValidationGroup="Addticket"></asp:RequiredFieldValidator>
                            </label>
                            <div class="col-sm-4 pr-5">
                                <div class="input-group mb-3">
                                    <div class="input-group-prepend">
                                        <span class="input-group-text"><i class="fas fa-calendar"></i></span>
                                    </div>
                                    <asp:TextBox ID="txtDurationto" runat="server" CssClass="form-control form-control-sm " MaxLength="10" autocomplete="off" ClientIDMode="static"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>

                    <div class=" row mt-1">
                        <div class="col-md-12">
                            <label for="exampleInputBorder" class="form-label">
                                Notes 
                                         <asp:RequiredFieldValidator ID="rfvtxtNotes" runat="server" ControlToValidate="txtNotes" ValidationGroup="Submit" ForeColor="Red" ErrorMessage="Required"></asp:RequiredFieldValidator>
                            </label>

                            <%--	<asp:TextBox ID="txtDescription" runat="server" CssClass="form-control form-control-sm" TextMode="MultiLine" MaxLength="500" Height="200px"></asp:TextBox>--%>
                            <textarea id="txtNotes" runat="server"></textarea>
                        </div>
                    </div>
            </asp:Panel>



            <%-- _____________Grid will Show Impact Details ________________________________--%>
            <asp:Panel ID="pnlShowImpactDetails" runat="server">
                <div class="card">
                    <div class="card-body">
                        <div class="row">
                            <label>
                                Impact Details
                            </label>
                        </div>

                        <div class="form-group row mt-3">
                            <label for="staticEmail" class="col-sm-2 labelcolorl1 pl-5">
                                Impact Description<span class="red">*</span>
                                <asp:RequiredFieldValidator ID="rfvtxtImpactDesc" runat="server" ControlToValidate="txtImpactDesc" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="AddImapct">

                                </asp:RequiredFieldValidator>
                            </label>
                            <div class="col-sm-10 pr-5">
                                <asp:TextBox ID="txtImpactDesc" TextMode="MultiLine" Rows="4" Columns="10" class="form-control form-control-sm" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-2 offset-6  mt-3">
                                <asp:Button ID="btnAddImpactDetails" runat="server" Text="Add" CssClass="btn btn-sm bg-success" CausesValidation="true" ValidationGroup="AddImapct" OnClick="btnAddImpactDetails_Click"></asp:Button>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:GridView ID="gridAddImpact" runat="server" CssClass="table table-head-fixed text-nowrap  " OnRowDataBound="gridAddImpact_RowDataBound" OnRowDeleting="gridAddImpact_RowDeleting"
                                    AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="20">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderStyle-Width="120px" HeaderText="ImpactDetails" DataField="ImpactDetails" />
                                        <asp:CommandField ShowDeleteButton="True" HeaderStyle-Width="120px" ButtonType="Button" ItemStyle-Height="10px" ControlStyle-CssClass="btn btn-sm btn-danger" />

                                    </Columns>
                                    <RowStyle BackColor="White" BorderColor="#e3e4e6" BorderWidth="1px" Height="5px" />
                                    <FooterStyle BackColor="#EDEDED" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#EDEDED" ForeColor="#000000" HorizontalAlign="Left" Height="20px" VerticalAlign="NotSet" CssClass="header" />
                                    <SelectedRowStyle BackColor="#fff" Font-Bold="True" ForeColor="#000000" Height="5px" />
                                    <HeaderStyle BackColor="#EEEEEE" Font-Bold="True" ForeColor="#414141" Height="5px" BorderColor="WhiteSmoke" CssClass="header sorting_asc" Font-Size="Small" />
                                    <EditRowStyle BackColor="#e9edf2" BorderColor="#e3e4e6" BorderStyle="Solid" BorderWidth="1px" Height="5px" />
                                    <EmptyDataRowStyle HorizontalAlign="Center" BorderStyle="None" Height="5px" BorderColor="#EDEDED" BackColor="#EDEDED" />
                                    <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
                                    <AlternatingRowStyle BackColor="#EAEEFF" BorderColor="#e3e4e6" Height="5px" BorderStyle="Solid" BorderWidth="1px" />

                                </asp:GridView>
                            </div>
                        </div>

                    </div>
                </div>
            </asp:Panel>

            <%--        ________________________ Grid will show RollOut Plan   _____________________________--%>

            <asp:Panel ID="pnlShowRollOutDetails" runat="server">
                <div class="card mb-1">
                    <div class="card-body">
                        <div class="row">
                            <label>
                                Roll Out Details
                            </label>
                        </div>
                        <div class="form-group row mt-3">
                            <label for="staticEmail" class="col-sm-2 labelcolorl1 pl-5">
                                RollOut Description<span class="red">*</span>
                                <asp:RequiredFieldValidator ID="rfvtxtRollOut" runat="server" ControlToValidate="txtRollOut" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="RollOut">

                                </asp:RequiredFieldValidator>
                            </label>
                            <div class="col-sm-10 pr-5">
                                <asp:TextBox ID="txtRollOut" TextMode="MultiLine" Rows="4" Columns="10" class="form-control form-control-sm" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-dm-2">
                                <asp:Button ID="btnAddRollOutGrid" runat="server" Text="Add" CssClass="btn btn-sm bg-success" CausesValidation="true" ValidationGroup="RollOut" OnClick="btnAddRollOutGrid_Click"></asp:Button>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:GridView ID="gridAddRollOut" runat="server" CssClass="table table-head-fixed text-nowrap  " OnRowDeleting="gridAddRollOut_RowDeleting" OnRowDataBound="gridAddRollOut_RowDataBound"
                                    AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="5px">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderStyle-Width="120px" HeaderText="RollOutDetails" DataField="RollOutDetails" />
                                        <asp:CommandField ShowDeleteButton="True" HeaderStyle-Width="120px" ButtonType="Button" ItemStyle-Height="10px" ControlStyle-CssClass="btn btn-sm btn-danger" />
                                    </Columns>
                                    <RowStyle BackColor="White" BorderColor="#e3e4e6" BorderWidth="1px" Height="5px" />
                                    <FooterStyle BackColor="#EDEDED" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#EDEDED" ForeColor="#000000" HorizontalAlign="Left" Height="20px" VerticalAlign="NotSet" CssClass="header" />
                                    <SelectedRowStyle BackColor="#fff" Font-Bold="True" ForeColor="#000000" Height="5px" />
                                    <HeaderStyle BackColor="#EEEEEE" Font-Bold="True" ForeColor="#414141" Height="5px" BorderColor="WhiteSmoke" CssClass="header sorting_asc" Font-Size="Small" />
                                    <EditRowStyle BackColor="#e9edf2" BorderColor="#e3e4e6" BorderStyle="Solid" BorderWidth="1px" Height="5px" />
                                    <EmptyDataRowStyle HorizontalAlign="Center" BorderStyle="None" Height="5px" BorderColor="#EDEDED" BackColor="#EDEDED" />
                                    <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
                                    <AlternatingRowStyle BackColor="#EAEEFF" BorderColor="#e3e4e6" Height="5px" BorderStyle="Solid" BorderWidth="1px" />

                                </asp:GridView>
                            </div>
                        </div>


                    </div>
                </div>
            </asp:Panel>

            <%--        ________________________ Grid will show DownTime Plan   _____________________________--%>
            <asp:Panel ID="pnlDownTime" runat="server" CssClass="mb-1">
                <div class="card">
                    <div class="card-body">
                        <div class="row">
                            <label>
                                Down Time Details
                            </label>
                        </div>
                        <div class="row">
                            <div class="col-md-3">
                                <label>
                                    Summary :
                                </label>
                                <asp:TextBox ID="txtDownTimeName" class="form-control form-control-sm" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-3">
                                <label class="ml-1">
                                    Duration From :
                                </label>
                                <div class="col-sm-12">
                                    <div class="input-group ">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text"><i class="fas fa-calendar"></i></span>
                                        </div>
                                        <asp:TextBox ID="txtDownTimeStart" runat="server" CssClass="form-control form-control-sm" MaxLength="10" autocomplete="off" ClientIDMode="static"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <label class="ml-1">
                                    Duration To :
                                </label>
                                <div class="col-sm-12">
                                    <div class="input-group ">
                                        <div class="input-group-prepend">
                                            <span class="input-group-text"><i class="fas fa-calendar"></i></span>
                                        </div>
                                        <asp:TextBox ID="txtDownTimeTo" runat="server" CssClass="form-control form-control-sm" MaxLength="10" autocomplete="off" ClientIDMode="static"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <%--        ________________________ Task Association   _____________________________--%>
            <asp:Panel ID="pnlTaksAssociation" runat="server" CssClass="mb-1">

                <div class="card">
                    <div class="card-body">
                        <div class="row">
                            <label>
                                Task Association
                            </label>
                        </div>

                        <div class="row">
                            <div class="col-md-5">
                                <label>
                                    Task Summary :
																			<asp:RequiredFieldValidator ID="rfvtxtTaskSummary" runat="server" ControlToValidate="txtTaskSummary" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Task">
                                                                            </asp:RequiredFieldValidator>

                                </label>
                                <asp:TextBox ID="txtTaskSummary" class="form-control form-control-sm" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-3">
                                <label class="ml-1">
                                    Task Status :
                                </label>

                                <asp:DropDownList ID="ddlTaskStatus" runat="server" Enabled="false" CssClass="form-select form-select-sm single-select-optgroup-field">
                                    <asp:ListItem Text="Open" Selected="True" Value="Open"></asp:ListItem>
                                    <asp:ListItem Text="WIP" Value="WIP"></asp:ListItem>
                                    <asp:ListItem Text="Hold" Value="Hold"></asp:ListItem>
                                    <asp:ListItem Text="Closed" Value="Closed"></asp:ListItem>
                                </asp:DropDownList>

                            </div>
                            <div class="col-md-3">
                                <label class="ml-1">
                                    Technician Associaiton :
												<asp:RequiredFieldValidator ID="rfvlstTechAssoc" runat="server" ControlToValidate="lstTechAssoc" ErrorMessage="*" InitialValue="0" Font-Bold="true" ForeColor="Red" ValidationGroup="Task">
                                                </asp:RequiredFieldValidator>
                                </label>

                                <asp:ListBox ID="lstTechAssoc" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" SelectionMode="Multiple"></asp:ListBox>

                            </div>

                        </div>
                        <div class="row">
                            <div class="col-md-1">
                                <div class="col-dm-2">
                                    <asp:Button ID="btnAddTaskAssociationData" runat="server" Text="Add" CssClass="btn btn-sm bg-success" ValidationGroup="Task" OnClick="btnAddTaskAssociationData_Click"></asp:Button>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <asp:GridView ID="gvAddTask" runat="server" CssClass="table table-head-fixed text-nowrap  " OnRowDataBound="gvAddTask_RowDataBound" OnRowDeleting="gvAddTask_RowDeleting"
                                    AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="20">
                                            <ItemTemplate>
                                                <%#Container.DataItemIndex+1 %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderStyle-Width="120px" HeaderText="Task Description" DataField="TaskDescription" />
                                        <asp:BoundField HeaderStyle-Width="120px" HeaderText="Status" DataField="Status" />
                                        <asp:BoundField HeaderStyle-Width="120px" HeaderText="Engineer Association" DataField="EngineerAssociation" />
                                        <asp:CommandField ShowDeleteButton="True" HeaderStyle-Width="120px" ButtonType="Button" ItemStyle-Height="10px" ControlStyle-CssClass="btn btn-sm btn-danger" />


                                    </Columns>
                                    <RowStyle BackColor="White" BorderColor="#e3e4e6" BorderWidth="1px" Height="5px" />
                                    <FooterStyle BackColor="#EDEDED" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#EDEDED" ForeColor="#000000" HorizontalAlign="Left" Height="20px" VerticalAlign="NotSet" CssClass="header" />
                                    <SelectedRowStyle BackColor="#fff" Font-Bold="True" ForeColor="#000000" Height="5px" />
                                    <HeaderStyle BackColor="#EEEEEE" Font-Bold="True" ForeColor="#414141" Height="5px" BorderColor="WhiteSmoke" CssClass="header sorting_asc" Font-Size="Small" />
                                    <EditRowStyle BackColor="#e9edf2" BorderColor="#e3e4e6" BorderStyle="Solid" BorderWidth="1px" Height="5px" />
                                    <EmptyDataRowStyle HorizontalAlign="Center" BorderStyle="None" Height="5px" BorderColor="#EDEDED" BackColor="#EDEDED" />
                                    <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
                                    <AlternatingRowStyle BackColor="#EAEEFF" BorderColor="#e3e4e6" Height="5px" BorderStyle="Solid" BorderWidth="1px" />

                                </asp:GridView>
                            </div>
                        </div>


                    </div>
                </div>
            </asp:Panel>
            <div class="row gy-2 gx-3 mb-2 px-4 mt-2">
                <div class="col-md-12 text-end">
                    <asp:Button ID="btnPrev" runat="server" Text="Prev" CausesValidation="false" OnClick="btnPrev_Click" CssClass="btn btn-sm cancelbtn " />


                    <asp:Button ID="btnNext" runat="server" Text="Next" ValidationGroup="Addticket" CausesValidation="false" OnClick="btnNext_Click" CssClass="btn btn-sm savebtn " />

                    <asp:Button ID="btnSubmit" runat="server" Text="Update" ValidationGroup="Addticket" OnClick="btnSubmit_Click" CssClass="btn btn-sm btn-grd-info " />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" OnClick="btnCancel_Click" CssClass="btn btn-sm btn-grd-danger " />
                </div>
            </div>
        </div>
    </div>
             


     <div class="card">
         <div class="card-body">
             <div class="row gy-2 gx-3">

                 <asp:Repeater ID="rptCategories" runat="server"
                     OnItemDataBound="rptCategories_ItemDataBound">
                     <HeaderTemplate>
                     </HeaderTemplate>
                     <ItemTemplate>
                         <div class="col-md-4">


                             <div class="row">
                                 <asp:DetailsView ID="DetailsCheckInAsset" runat="server" CssClass="table table-bordered table-sm"
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

                                     </Fields>
                                     <%--<RowStyle BackColor="White" BorderColor="#e3e4e6" BorderWidth="1px" Height="5px" />
                                                    <FooterStyle BackColor="#EDEDED" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle BackColor="#EDEDED" ForeColor="#000000" HorizontalAlign="Left" Height="5px" VerticalAlign="NotSet" CssClass="header" />
                                                    <HeaderStyle BackColor="#EEEEEE" Font-Bold="True" ForeColor="#414141" Height="5px" BorderColor="WhiteSmoke" CssClass="header sorting_asc" Font-Size="Small" />
                                                    <EditRowStyle BackColor="#e9edf2" BorderColor="#e3e4e6" BorderStyle="Solid" BorderWidth="1px" Height="5px" />
                                                    <EmptyDataRowStyle HorizontalAlign="Center" BackColor="#EDEDED" />--%>
                                     <EmptyDataTemplate>No Record Available</EmptyDataTemplate>
                                     <%--<AlternatingRowStyle BackColor="#EAEEFF" BorderColor="#e3e4e6" Height="5px" BorderStyle="Solid" BorderWidth="1px" />--%>
                                 </asp:DetailsView>
                             </div>

                             <%--<br style="clear: left;" />--%>
                         </div>

                     </ItemTemplate>
                 </asp:Repeater>
             </div>
         </div>
     </div>






    <asp:HiddenField ID="hdnCategoryID" runat="server" />
    <script src="assetsdata/plugins/summernote/summernote-bs4.js"></script>
    <script>
        $(document).ready(function () {
            $('#<%= txtResolution.ClientID %>').summernote();
     });
        $(document).ready(function () {
            $('#<%= txtNotes.ClientID %>').summernote();
     });
    </script>
</asp:Content>


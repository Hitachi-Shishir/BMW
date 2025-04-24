<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmTicketMerge.aspx.cs" Inherits="frmTicketMerge" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .hidden-dropdown {
            visibility: hidden;
        }

        .grid-custom > table > tbody > tr > td {
            padding: 13px 20px;
        }

        .form-check-label {
            margin-bottom: 8px;
        }

        input[type="radio"] {
            margin-right: 1rem; /* Adjust as needed */
        }
    </style>
    <script src="assetsdata/plugins/summernote/jquery.js"></script>
    <link href="assetsdata/plugins/summernote/summernote-bs4.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.bundle.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">

    <asp:ScriptManager ID="scrmg" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <div class="card ">
                <div class="card-header d-flex align-items-start justify-content-between">
                    <div>
                        <asp:Label ID="lblHeader" runat="server" CssClass="h6"></asp:Label></div>

                    <div>
                        <asp:LinkButton
                            ID="ImgbtnBack"
                            runat="server"
                            OnClick="ImgbtnBack_Click"
                            CssClass="btn btn-sm btn-primary"
                            ToolTip="Back">
    <i class="fa-solid fa-arrow-left"></i>
</asp:LinkButton>

                    </div>
                </div>

                <div class="card-body">

                    <div class="row mx-1 mb-4">
                        <div class="col-md-11 shadow-sm  rounded py-1 border ">
                            <asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Red"></asp:Label>
                            <asp:Label ID="Label1" runat="server" Text="Tickets:-" ForeColor="Red"></asp:Label>

                            <asp:Label ID="lblTicketUpdateNo" runat="server" Text="" ForeColor="Red"></asp:Label>
                        </div>
						<div class="col-md-1">
							<asp:ImageButton ID="ImgBtn_Back" runat="server" Width="40px" ImageUrl="~/Images/leftarrow.png" OnClick="ImgbtnBack_Click" />
						</div>
                    </div>


                    <asp:Panel ID="pnlIncident" runat="server">

                        <div class=" row ">
                            <div class="col-md-6">
                                <label for="staticEmail" class="form-check-label">
                                    Priority 
                       
                                </label>

                                <asp:DropDownList ID="ddlPriority" runat="server" CssClass="form-control form-control-sm chzn-select"></asp:DropDownList>
                            </div>
                            <div class="col-md-6">
                                <label for="staticEmail" class="form-check-label">
                                    Status  
                       
                                </label>

                                <asp:DropDownList ID="ddlStage" runat="server" CssClass="form-control  form-control-sm chzn-select"></asp:DropDownList>
                            </div>
                            <div class="col-md-6">
                                <label for="staticEmail" class="form-check-label">
                                    Location
                       
                                </label>

                                <asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-control form-control-sm chzn-select">
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-6">
                                <label for="staticEmail" class="form-check-label">
                                    Department
                          
                       
                                </label>

                                <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-control form-control-sm chzn-select"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="form-group row">
                            <label for="staticEmail" class="form-check-label">
                                Resolution :
                                         
                                <asp:RequiredFieldValidator ID="rfvddlResoultion" runat="server" ControlToValidate="ddlResoultion" ValidationGroup="Addticket" ForeColor="Red" InitialValue="0" ErrorMessage="Required"></asp:RequiredFieldValidator>
                            </label>
                            <div class="col-sm-4 pr-5">


                                <asp:DropDownList ID="ddlResoultion" runat="server" CssClass="form-control form-control-sm chzn-select"></asp:DropDownList>
                            </div>
                            <label for="staticEmail" class="form-check-label">
                                Severity :
						&nbsp;</label>
                            <div class="col-sm-4 pr-5">
                                <asp:DropDownList ID="ddlSeverity" runat="server" CssClass="form-control form-control-sm chzn-select"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group row ">
                            <%-- this is area for Category--%>
                            <div class="col-lg-6 col-md-8 col-sm-12">

                                <div id="divCategory1" class="form-group row" runat="server">

                                    <label for="staticEmail" class="col-sm-4 labelcolorl1 pl-4">

                                        <asp:Label ID="lblCategory1" runat="server" Text="Category1 :"></asp:Label>
                                    </label>
                                    <div class="col-sm-8 pr-5">


                                        <asp:DropDownList ID="ddlCategory1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCategory1_SelectedIndexChanged" CssClass="form-control form-control-sm chzn-select"></asp:DropDownList>
                                    </div>

                                </div>
                                <div id="divCategory2" class="form-group row" runat="server">


                                    <label for="staticEmail" class="col-sm-4 labelcolorl1 pl-4">
                                        <asp:Label ID="lblCategory2" runat="server" Text="Category2 :"></asp:Label>
                                    </label>
                                    <div class="col-sm-8 pr-5">


                                        <asp:DropDownList ID="ddlCategory2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCategory2_SelectedIndexChanged" CssClass="form-control form-control-sm chzn-select"></asp:DropDownList>
                                    </div>
                                </div>
                                <div id="divCategory3" class="form-group row" runat="server">

                                    <label for="staticEmail" class="col-sm-4 labelcolorl1 pl-4">
                                        <asp:Label ID="lblCategory3" runat="server" Text="Category3 :"></asp:Label>

                                    </label>
                                    <div class="col-sm-8 pr-5">


                                        <asp:DropDownList ID="ddlCategory3" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCategory3_SelectedIndexChanged" CssClass="form-control form-control-sm chzn-select"></asp:DropDownList>
                                    </div>
                                </div>
                                <div id="divCategory4" class="form-group row" runat="server">

                                    <label for="staticEmail" class="col-sm-4 labelcolorl1 pl-4">
                                        <asp:Label ID="lblCategory4" runat="server" Text="Category4 :"></asp:Label>
                                    </label>
                                    <div class="col-sm-8 pr-5">


                                        <asp:DropDownList ID="ddlCategory4" runat="server" CssClass="form-control form-control-sm chzn-select" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory4_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </div>
                                <div id="divCategory5" class="form-group row" runat="server">

                                    <label for="staticEmail" class="col-sm-4 labelcolorl1 pl-4">
                                        <asp:Label ID="lblCategory5" runat="server" Text="Category5 :"></asp:Label>
                                    </label>
                                    <div class="col-sm-8 pr-5">


                                        <asp:DropDownList ID="ddlCategory5" runat="server" CssClass="form-control form-control-sm chzn-select hidden-dropdown" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory5_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="form-group row " hidden>

                                    <label for="staticEmail" class="col-sm-4 labelcolorl1 pl-4">
                                        Category6 :
                               
                                    </label>
                                    <div class="col-sm-8 pr-5">


                                        <asp:DropDownList ID="ddlCategory6" runat="server" CssClass="form-control form-control-sm chzn-select "></asp:DropDownList>
                                    </div>
                                </div>

                            </div>
                            <%-- this is area for Custom Field--%>
                            <div class="col-lg-6 col-md-8 col-ms-12">
                            </div>
                        </div>
                        <div class="form-group row ">
                            <label for="exampleInputBorder" class="form-check-label">
                                Resolution : 
                       
                            </label>
                            <div class="col-sm-10 pr-5">
                                <asp:RequiredFieldValidator ID="rfvtxtResolution" runat="server" ControlToValidate="txtResolution" ValidationGroup="Addticket" ForeColor="Red" ErrorMessage="*"></asp:RequiredFieldValidator>

                                <%--	<asp:TextBox ID="txtDescription" runat="server" CssClass="form-control form-control-sm" TextMode="MultiLine" MaxLength="500" Height="200px"></asp:TextBox>--%>
                                <textarea id="txtResolution" runat="server"></textarea>
                            </div>
                        </div>

                        <div class="form-group row mb-0 pb-0">
                            <label for="exampleInputBorder" class="form-check-label">
                                Assignee :
                                         
                                <asp:RequiredFieldValidator ID="rfvddlAssigne" runat="server" ControlToValidate="ddlAssigne" ValidationGroup="Addticket" ForeColor="Red" ErrorMessage="Required" InitialValue="0"></asp:RequiredFieldValidator>
                            </label>
                            <div class="col-sm-4 pr-5">


                                <asp:DropDownList ID="ddlAssigne" runat="server" CssClass="form-control form-control-sm chzn-select"></asp:DropDownList>
                            </div>

                        </div>



                        <div class="form-group row ">
                            <label for="exampleInputBorder" class="form-check-label">
                                Notes : 
                                    
                                <asp:RequiredFieldValidator ID="rfvtxtNotes" runat="server" ControlToValidate="txtNotes" ValidationGroup="Addticket" ForeColor="Red" ErrorMessage="Required"></asp:RequiredFieldValidator>
                            </label>
                            <div class="col-sm-10 pr-5">
                                <%--	<asp:TextBox ID="txtDescription" runat="server" CssClass="form-control form-control-sm" TextMode="MultiLine" MaxLength="500" Height="200px"></asp:TextBox>--%>
                                <textarea id="txtNotes" runat="server"></textarea>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-4">
                                <asp:Button ID="btnSubmit" runat="server" Text="Update" ValidationGroup="Addticket" OnClick="btnSubmit_Click" CssClass="btn btn-sm savebtn " />
                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" OnClick="btnCancel_Click" CssClass="btn btn-sm cancelbtn " />
                            </div>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlMerge" runat="server">
                        <div class="row gy-2 gx-3 ">

                            <div class="col-md-6">
                                <label class="form-check-label">
                                    Choose Primary Ticket
					
                                    <asp:RequiredFieldValidator ID="rfvrdbtnTicketList" runat="server" ControlToValidate="rdbtnTicketList" ValidationGroup="Merge" ForeColor="Red" ErrorMessage="Required Field"></asp:RequiredFieldValidator>
                                </label>
                                <asp:RadioButtonList ID="rdbtnTicketList" CellSpacing="10" CellPadding="10" RepeatDirection="Vertical" runat="server" Style="width: 100%" CssClass="grid-custom border">
                                </asp:RadioButtonList>
                            </div>
                            <div class="col-md-6">
                                <label class="form-check-label">Choose Ticket Summary</label>
                                <asp:RequiredFieldValidator ID="rfvrdbtnTicketSummary" runat="server" ControlToValidate="rdbtnTicketSummary" ValidationGroup="Merge" ForeColor="Red" ErrorMessage="Please Choose Summary"></asp:RequiredFieldValidator>

                                <asp:RadioButtonList ID="rdbtnTicketSummary" CellSpacing="10" CellPadding="10" RepeatDirection="Vertical" runat="server" Style="width: 100%" CssClass="grid-custom border">
                                </asp:RadioButtonList>

                            </div>
                            <div class="col-md-12">


                                <label class="form-check-label">
                                    Merge Notes
				
                                    <asp:RequiredFieldValidator ID="rfvrdbtnMergeNotes" runat="server" ControlToValidate="rdbtnMergeNotes" ValidationGroup="Merge" ForeColor="Red" ErrorMessage="Choose For Merging Notes"></asp:RequiredFieldValidator>

                                </label>


                                <asp:RadioButtonList ID="rdbtnMergeNotes" CellSpacing="10" CellPadding="10" RepeatDirection="Horizontal" runat="server" Style="width: 49%" CssClass="grid-custom border">
                                    <asp:ListItem Text="Yes" Value="Yes">
							
                                            </asp:ListItem>
                                    <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <div class="col-md-12 ">
                                <label for="staticEmail" class="form-check-label">
                                    Reason For Merging 
                                       
                                </label>

                                <asp:RequiredFieldValidator ID="rfvtxtMergeReason" runat="server" ControlToValidate="txtMergeReason" ValidationGroup="Merge" ForeColor="Red" ErrorMessage="*"></asp:RequiredFieldValidator>

                                <%--	<asp:TextBox ID="txtDescription" runat="server" CssClass="form-control form-control-sm" TextMode="MultiLine" MaxLength="500" Height="200px"></asp:TextBox>--%>
                                <textarea id="txtMergeReason" runat="server"></textarea>
                            </div>
                            <div class="col-md-12 text-end">
                                <asp:Button ID="btnMerge" runat="server" Text="Merge" ValidationGroup="Merge" OnClick="btnMerge_Click" CssClass="btn btn-sm btn-grd-info " />

                            </div>



                        </div>
                    </asp:Panel>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:HiddenField ID="hdnCategoryID" runat="server" />
    <script src="assetsdata/plugins/summernote/summernote-bs4.js"></script>
    <script>
        $(document).ready(function () {
            $('#<%= txtMergeReason.ClientID %>').summernote();
        });
    </script>
    <script>
        $.widget.bridge('uibutton', $.ui.button)
    </script>
</asp:Content>


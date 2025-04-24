<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmEditTicketbyAssigne.aspx.cs" Inherits="frmEditTicketbyAssigne" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="assets/plugins/summernote/jquery.js"></script>
    <link href="assets/plugins/summernote/summernote-bs4.css" rel="stylesheet" />
    <script src="
https://code.jquery.com/jquery-3.3.1.slim.min.js"></script>
    <script src="
https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.bundle.min.js"></script>
    <link href="https://fonts.googleapis.com/css2?family=Dancing+Script&display=swap" rel="stylesheet">

    <style>
        .card-header{
                --bs-card-bg: var(--bs-body-bg);
                top:2%;
        }
        .note-editor {
            border: var(--bs-border-width) var(--bs-border-style) var(--bs-border-color) !important;
        }

        .ai-output {
            font-family: monospace;
            background-color: #f5f5f5;
            padding: 15px;
            border-radius: 5px;
            height: 65vh !important;
            display: block;
            white-space: pre-wrap;
            margin-bottom: 0.5rem !important;
            overflow-y: auto;
            scrollbar-width: thin; /* Makes scrollbar thinner */
            scrollbar-color: #555 #e0e0e0; /* Thumb color, Track color */
            padding-right: 4rem;
        }

            /* Chrome, Edge, and Safari */
            .ai-output::-webkit-scrollbar {
                width: 4px; /* Scrollbar width */
            }

            .ai-output::-webkit-scrollbar-track {
                background: #f1f1f1; /* Track color */
                border-radius: 15px; /* Optional: round the track */
                box-shadow: inset 0 0 5px grey;
            }

            .ai-output::-webkit-scrollbar-thumb {
                background-color: #888; /* Thumb color */
                border-radius: 5px; /* Round the thumb */
                border: 2px solid #e0e0e0; /* Adds padding inside the thumb */
            }

                .ai-output::-webkit-scrollbar-thumb:hover {
                    background-color: #555; /* Thumb color on hover */
                }

            /* Hide the scrollbar arrows in WebKit browsers */
            .ai-output::-webkit-scrollbar-button {
                display: none; /* Hides the scrollbar buttons (up and down arrows) */
            }

        .position-relative {
            position: relative;
        }

        .position-absolute {
            position: absolute;
        }

        .animated-text {
            font-family: "Brush Script MT", cursive;
            font-size: 13px;
            font-weight: bold;
            background: linear-gradient(45deg, #7928ca, #ff0080);
            background-size: 200% 200%;
            -webkit-background-clip: text;
            color: transparent;
            animation: gradient-animation 3s ease infinite, smooth-flicker 2s ease-in-out infinite;
        }

        /* Animation Keyframes for Gradient */
        @keyframes gradient-animation {
            0% {
                background-position: 0% 50%;
            }

            50% {
                background-position: 100% 50%;
            }

            100% {
                background-position: 0% 50%;
            }
        }

        /* Smooth Flicker Effect */
        @keyframes smooth-flicker {
            0%, 100% {
                opacity: 1;
            }

            25% {
                opacity: 0.7;
            }

            50% {
                opacity: 0.4;
            }

            75% {
                opacity: 0.7;
            }
        }
    </style>
    <style>
        /* Loader Styles */
        #loader {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: rgba(0, 0, 0, 0.8); /* Semi-transparent background */
            z-index: 9999;
            display: flex;
            justify-content: center;
            align-items: center;
        }

        .dots {
            display: flex;
            gap: 10px;
        }

        .dot {
            width: 15px;
            height: 15px;
            border-radius: 50%;
            background: linear-gradient(45deg, #00c6ff, #0072ff);
            animation: glow 1.5s infinite;
        }

            .dot:nth-child(2) {
                animation-delay: 0.3s;
            }

            .dot:nth-child(3) {
                animation-delay: 0.6s;
            }

        .loader-text {
            color: white;
            font-size: 1.2rem;
            font-family: Arial, sans-serif;
            margin-top: 20px;
            text-align: center;
        }

        @keyframes glow {
            0%, 100% {
                transform: scale(1);
                opacity: 0.8;
            }

            50% {
                transform: scale(1.3);
                opacity: 1;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="scr" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="updatepanel1" runat="server">
        <ContentTemplate>
            <div id="loader" style="display: none;">
                <div>
                    <div class="dots">
                        <div class="dot"></div>
                        <div class="dot"></div>
                        <div class="dot"></div>
                    </div>
                    <div class="loader-text">Processing...</div>
                </div>
            </div>
			<div class="card">
				<div class="card-header justify-content-between d-flex  sticky-top bg-light">
					<h6 class="card-title mb-0 py-1">
						<label class="cardheader">Update Ticket</label>
						<asp:Label ID="lblTicket" class="cardheader " runat="server" Text=""></asp:Label>
						<asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Red"></asp:Label>
						<asp:Label ID="lblErrorMsg" runat="server" Text="" ForeColor="Red" Style="font-weight: bold;"></asp:Label></h6>
					<div>

						<asp:Button ID="btnUpdateClose" runat="server" Text="Update & Close" ValidationGroup="Submit" OnClick="btnUpdateClose_Click" OnClientClick="validateSelection();" CssClass="btn btn-sm btn-grd btn-grd-primary" />
						<asp:Button ID="btnUpdate" runat="server" Text="Update" ValidationGroup="Submit" OnClick="btnUpdate_Click" OnClientClick="validateSelection();" CssClass="btn btn-sm btn-grd btn-grd-info " />
						<asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" OnClick="btnCancel_Click" CssClass="btn btn-sm  btn-grd btn-grd-danger  " />

					</div>
				</div>
				<div class="card-body">
					<div class="row">
						<div class="col-md-6 col-lg-12 col-sm-4">
						</div>
					</div>
					<div class="row">
						<div class="col-md-11">
							<div class="btn-group">
								<asp:Button ID="btnUpdateTickView" Text="View Tickets" runat="server" CssClass="btn btn-sm btn-outline-secondary " OnClick="btnUpdateTickView_Click" />
								<asp:Button ID="btnImpactDetails" runat="server" Text="Impact Details" CssClass="btn btn-sm btnDisabled btn-outline-secondary" CausesValidation="false" OnClick="btnImpactDetails_Click" />
								<asp:Button ID="btnRolloutPlan" runat="server" Text="RollOut Details" CssClass="btn btn-sm btnDisabled btn-outline-secondary" CausesValidation="false" OnClick="btnRolloutPlan_Click" />
								<asp:Button ID="btnDowntime" runat="server" Text="Downtime" CssClass="btn btn-sm btnDisabled btn-outline-secondary" CausesValidation="false" OnClick="btnDowntime_Click" />
								<asp:Button ID="btnTaskAssociation" runat="server" Text="Task " CssClass="btn btn-sm btnDisabled btn-outline-secondary" CausesValidation="false" OnClick="btnTaskAssociationShowPanel_Click" />
								<asp:Button ID="btnViewNotes" runat="server" Text-="View Notes" CssClass="btn btn-sm  btnDisabled btn-outline-secondary" OnClick="btnViewNotes_Click" />
								<asp:LinkButton ID="btnViwPyres" runat="server" CssClass="btn btn-sm btnDisabled btn-outline-secondary" OnClick="btnViwpYres_Click">
                                    Notes Summary  <span class="animated-text">AI</span>
								</asp:LinkButton>
								<asp:LinkButton
									ID="btnDownloadKb"
									runat="server"
									CssClass="btn btn-sm btnDisabled btn-outline-secondary"
									OnClick="btnDownloadKb_Click">
    Knowledge Base Document <span class="animated-text">AI</span>
								</asp:LinkButton>
							</div>
						</div>
						<div class="col-md-1">
							<asp:ImageButton ID="ImgBtn_Back" runat="server" Width="40px" ImageUrl="~/Images/leftarrow.png" OnClick="ImgBtn_Back_Click" />
						</div>
					</div>
					<div class="row" id="AInotes" runat="server" visible="false">
						<div class="col-md-12 position-relative">
							<asp:LinkButton ID="lnkKBDownload" runat="server" CssClass="btn btn-secondary border position-absolute rounded-circle" Style="top: 24px; right: 27px;"
								OnClick="lnkKBDownload_Click">  <i class="fa-solid fa-download"></i></asp:LinkButton>
							<asp:Label ID="lblPyoutput" runat="server" CssClass="ai-output"></asp:Label>

						</div>
					</div>
					<asp:Literal ID="litTicketDetails" runat="server" Mode="PassThrough"></asp:Literal>
					<asp:Panel ID="pnlTicket" runat="server">
						<asp:Panel ID="pnlUpdateTicket" runat="server">
							<div class=" row mt-2">
								<div class="col-md-6">
									<label for="staticEmail" class="form-label ">
										Service Desk
                                            <asp:RequiredFieldValidator ID="RfvddlRequestType" runat="server" InitialValue="0" ControlToValidate="ddlRequestType" ValidationGroup="Submit" ForeColor="Red" ErrorMessage="*"></asp:RequiredFieldValidator>
									</label>
									<asp:DropDownList ID="ddlRequestType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlRequestType_SelectedIndexChanged" CssClass="form-select form-select-sm single-select-optgroup-field"></asp:DropDownList>
								</div>
								<div class="col-md-6">
									<label for="staticEmail" class="form-label">
										Employee ID<span class="red">*</span>
										<asp:RequiredFieldValidator ID="rfvtxtLoginName" runat="server" ControlToValidate="txtLoginName" ErrorMessage="*" ForeColor="Red" ValidationGroup="Submit" Visible="false"></asp:RequiredFieldValidator>
									</label>

									<div class="input-group input-group-sm pr-5">
										<asp:TextBox ReadOnly="true" ID="txtLoginName" runat="server" class="form-control form-control-sm" autocomplete="off" ValidationGroup="SearchUser"></asp:TextBox>

									</div>
								</div>
							</div>

							<asp:Panel ID="pnlIncident" runat="server">
								<div class=" row gy-2 gx-2 mt-1">
									<div class="col-md-12">
										<label for="staticEmail" class="form-label">
											Summary 
                                                <asp:RequiredFieldValidator ID="RfvtxtSummary" runat="server" ControlToValidate="txtSummary" ValidationGroup="Submit" ForeColor="Red" ErrorMessage="Required"></asp:RequiredFieldValidator>
										</label>
										<asp:TextBox ID="txtSummary" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
									</div>
									<div class="col-md-3 col-6">
										<label for="staticEmail" class="form-label">
											Submitter Name
                                                   <asp:RequiredFieldValidator ID="RfvtxtSubmitterName" runat="server" ControlToValidate="txtSubmitterName" ValidationGroup="Submit" ForeColor="Red" ErrorMessage="Required"></asp:RequiredFieldValidator>
										</label>
										<asp:TextBox ID="txtSubmitterName" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
									</div>
									<div class="col-md-3 col-6">
										<label for="staticEmail" class="form-label">
											Submitter Email 
                                             <asp:RequiredFieldValidator ID="RfvtxtSubmitterEmail" runat="server" ControlToValidate="txtSubmitterEmail" ValidationGroup="Submit" ForeColor="Red" ErrorMessage="Required"></asp:RequiredFieldValidator>
										</label>
										<asp:TextBox ID="txtSubmitterEmail" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
									</div>

									<div class="col-md-3 col-6">
										<label for="staticEmail" class="form-label">
											Contact Number
                                               <asp:RequiredFieldValidator ID="RfvtxtPhoneNumber" runat="server" ControlToValidate="txtPhoneNumber" ValidationGroup="Submit" ForeColor="Red" ErrorMessage="Required"></asp:RequiredFieldValidator>
										</label>

										<asp:TextBox ID="txtPhoneNumber" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
									</div>
									<div class="col-md-3 col-6">
										<label for="staticEmail" class="form-label">
											Priority 
                                  
                                    <asp:RequiredFieldValidator ID="rfvddlPriority" runat="server" ControlToValidate="ddlPriority" ErrorMessage="Required" ForeColor="Red" InitialValue="0" ValidationGroup="Submit"></asp:RequiredFieldValidator>
										</label>

										<asp:DropDownList ID="ddlPriority" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlPriority_SelectedIndexChanged"></asp:DropDownList>
									</div>
									<div class="col-md-3 col-6">
										<label for="staticEmail" class="form-label">
											Severity
                                            <asp:RequiredFieldValidator ID="RfvddlSeverity" runat="server" ControlToValidate="ddlSeverity" ErrorMessage="Required" ForeColor="Red" InitialValue="0" ValidationGroup="Submit" Enabled="false"></asp:RequiredFieldValidator>
											&nbsp;</label>

										<asp:DropDownList ID="ddlSeverity" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlSeverity_SelectedIndexChanged"></asp:DropDownList>
									</div>
									<div class="col-md-3 col-6">
										<label for="staticEmail" class="form-label">
											Assignee 
                                                  <asp:RequiredFieldValidator ID="rfvddlAssigne" runat="server" ControlToValidate="ddlAssigne" ValidationGroup="Submit" ForeColor="Red" ErrorMessage="Required" InitialValue="0"></asp:RequiredFieldValidator>
										</label>
										<asp:DropDownList ID="ddlAssigne" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlAssigne_SelectedIndexChanged"></asp:DropDownList>
									</div>
									<div class="col-md-3 col-6">
										<label for="staticEmail" class="form-label">
											Location
                                               <asp:RequiredFieldValidator ID="rfvddlLocation" runat="server" ControlToValidate="ddlLocation" ValidationGroup="Addticket" ForeColor="Red" InitialValue="0" ErrorMessage="*"></asp:RequiredFieldValidator>
										</label>

										<asp:DropDownList ID="ddlLocation" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field"></asp:DropDownList>
									</div>
									<div class="col-md-3 col-6">
										<label for="staticEmail" class="form-label">
											Department
                                  
                                    <asp:RequiredFieldValidator ID="rfvddlDepartment" runat="server" ControlToValidate="ddlDepartment" ErrorMessage="*" ForeColor="Red" InitialValue="0" ValidationGroup="Addticket"></asp:RequiredFieldValidator>
										</label>

										<asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field"></asp:DropDownList>
									</div>
									<div class="col-md-3 col-6 ">
										<label for="staticEmail" class="form-label">
											Category 1 
                                                  <asp:RequiredFieldValidator ID="RfvddlCategory1" runat="server" ControlToValidate="ddlCategory1" ValidationGroup="Submit" ForeColor="Red" ErrorMessage="Required" InitialValue="0"></asp:RequiredFieldValidator>
										</label>
										<asp:DropDownList ID="ddlCategory1" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory1_SelectedIndexChanged"></asp:DropDownList>
									</div>
									<div class="col-md-3 col-6 ">
										<label for="staticEmail" class="form-label">
											Resolution
                                                  <asp:RequiredFieldValidator ID="rfvddlResoultion" runat="server" ControlToValidate="ddlResoultion" ValidationGroup="Submit" ForeColor="Red" InitialValue="0" ErrorMessage="Required"></asp:RequiredFieldValidator>
										</label>
										<asp:DropDownList ID="ddlResoultion" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field"></asp:DropDownList>
									</div>
									<div class="col-md-3 col-6 ">
										<asp:Panel ID="pnlSRFields" runat="server" Visible="false" >
												
												<label for="staticEmail" class="form-label">
													<asp:Label ID="Label13" Text="HOD Email :" runat="server"></asp:Label>

													<asp:RequiredFieldValidator ID="rfvddlHodApproval" runat="server" ControlToValidate="ddlHodApproval" Enabled="false" ErrorMessage="Required" InitialValue="0" Font-Bold="true" ForeColor="Red" ValidationGroup="Submit"></asp:RequiredFieldValidator>
												</label>


											

													<asp:DropDownList ID="ddlHodApproval" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field"></asp:DropDownList>

											

								


								</asp:Panel>

								</div>
									<asp:Panel ID="pnlChange" runat="server">
										<div class="row gy-2 gx-2 mt-1">
											<div class="col-md-3 col-6">
												<label for="staticEmail" class="form-label">
													Change Type: 
                           <asp:RequiredFieldValidator ID="rfvddlChangeType" runat="server" InitialValue="0" ControlToValidate="ddlChangeType" ValidationGroup="Addticket" ForeColor="Red" ErrorMessage="*"></asp:RequiredFieldValidator>
												</label>

												<asp:DropDownList ID="ddlChangeType" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field"></asp:DropDownList>
											</div>
											<div class="col-md-3 col-6">
												<label for="staticEmail" class="form-label">
													Reason For Change : 
              
                <asp:RequiredFieldValidator ID="rfvddlRFC" runat="server" ControlToValidate="ddlRFC" ErrorMessage="*" ForeColor="Red" InitialValue="0" ValidationGroup="Addticket"></asp:RequiredFieldValidator>
												</label>
												
													<asp:DropDownList ID="ddlRFC" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field"></asp:DropDownList>
												
											</div>
											<div class="col-md-3">
												<label for="staticEmail" class="form-label">
													Duration From
													<asp:RequiredFieldValidator ID="rfvtxtChangeDurationfrom" runat="server" ControlToValidate="txtChangeDurationfrom" ValidationGroup="Addticket" ForeColor="Red" ErrorMessage="*"></asp:RequiredFieldValidator>
												</label>
												<div class="input-group input-group-sm">
													<span class="input-group-text"><i class="fas fa-calendar"></i></span>
													<asp:TextBox ID="txtChangeDurationfrom" runat="server" CssClass="form-control form-control-sm datepicker flatpickr-input "></asp:TextBox>
												</div>

											</div>
											<div class="col-md-3">
												<label for="staticEmail" class="form-label">
													Duration To 
                       
                         <asp:RequiredFieldValidator ID="rfvtxtChangeDurationTo" runat="server" ControlToValidate="txtChangeDurationTo" ErrorMessage="*" ForeColor="Red" ValidationGroup="Addticket"></asp:RequiredFieldValidator>
												</label>
												<div class="input-group input-group-sm ">
													<span class="input-group-text"><i class="fas fa-calendar"></i></span>
													<asp:TextBox ID="txtChangeDurationTo" runat="server" CssClass="form-control form-control-sm datepicker flatpickr-input"></asp:TextBox>
												</div>

											</div>
											</div>
									</asp:Panel>
									<div class="row mt-2">
										<%-- this is area for Category--%>
										<div class="col-6 px-1">
											<div class="row gx-2 gy-3">
												<div id="divCategory2" class=" col-12" runat="server">
													<label for="staticEmail" class="form-label">
														Category 2 
                                              <asp:RequiredFieldValidator ID="RfvddlCategory2" runat="server" InitialValue="0" ControlToValidate="ddlCategory2" ValidationGroup="Addticket" ForeColor="Red" ErrorMessage="Required"></asp:RequiredFieldValidator>
													</label>

													<asp:DropDownList ID="ddlCategory2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCategory2_SelectedIndexChanged" CssClass="form-select form-select-sm single-select-optgroup-field"></asp:DropDownList>
												</div>

												<div id="divCategory3" class=" col-12" runat="server">
													<label for="staticEmail" class="form-label">
														Category 3 
                                              <asp:RequiredFieldValidator ID="RfvddlCategory3" runat="server" InitialValue="0" ControlToValidate="ddlCategory3" ValidationGroup="Addticket" ForeColor="Red" ErrorMessage="Required"></asp:RequiredFieldValidator>
													</label>

													<asp:DropDownList ID="ddlCategory3" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCategory3_SelectedIndexChanged" CssClass="form-select form-select-sm single-select-optgroup-field"></asp:DropDownList>

												</div>
												<div id="divCategory4" class=" col-12" runat="server">

													<label for="staticEmail" class="form-label">
														Category 4 
													</label>

													<asp:DropDownList ID="ddlCategory4" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory4_SelectedIndexChanged"></asp:DropDownList>

												</div>
												<div id="divCategory5" class=" col-12" runat="server">

													<label for="staticEmail" class="form-label">
														Category 5 
													</label>

													<asp:DropDownList ID="ddlCategory5" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory5_SelectedIndexChanged"></asp:DropDownList>

												</div>
												<div class=" row " hidden>

													<label for="staticEmail" class="form-label">
														Category 6 
													</label>
													<div class="col-sm-8 ">
														<asp:DropDownList ID="ddlCategory6" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field"></asp:DropDownList>
													</div>
												</div>
											</div>
										</div>

										<div class="row g-2">
											<div class="col-md-6">
												<label for="staticEmail" class=" form-label">
													Attach File
												</label>

												<div class="input-group input-group-sm">
													<asp:LinkButton ID="lnkDownload" runat="server" Text="" hidden OnClick="lnkDownload_Click"></asp:LinkButton>
													<asp:FileUpload ID="FileUploadTickDoc" AllowMultiple="true" runat="server" CssClass="form-control form-control-sm " ToolTip="Select Only Pdf,Excel File" />
													<asp:LinkButton ID="btnUpload" CssClass="btn btn-sm btn-secondary" runat="server" OnClick="btnUpload_Click"><i class="fa fa-upload"></i></asp:LinkButton>
												</div>
											</div>
											<div class="col-md-1"></div>
											<div class="col-md-6">
												<asp:GridView ID="grd" runat="server" class="table table-head-fixed text-nowrap table-sm table-striped mt-2 table-bordered" AutoGenerateColumns="false">
													<Columns>
														<asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="20">
															<ItemTemplate>
																<%# Container.DataItemIndex + 1 %>
															</ItemTemplate>
														</asp:TemplateField>
														<asp:TemplateField HeaderText="Files">
															<ItemTemplate>
																<asp:LinkButton ID="lnkViewUp" runat="server" OnClick="lnkViewUp_Click">
																	<asp:Label ID="lblfileNameUp" runat="server" Text='<%# Eval("fileName") %>'></asp:Label>
																</asp:LinkButton>
																<asp:HiddenField ID="hdnfullPathUp" runat="server" Value='<%# Eval("fullPath") %>' />
															</ItemTemplate>
														</asp:TemplateField>
														<asp:TemplateField HeaderText="Delete File">
															<ItemTemplate>
																<asp:LinkButton ID="lnkbtnDelete" runat="server" OnClick="lnkbtnDelete_Click"
																	OnClientClick="return confirm('Are you sure you want to Delete this File?');">
               <i class="fa fa-trash text-danger"></i>
																</asp:LinkButton>
																<asp:HiddenField ID="hdnidUp" runat="server" Value='<%# Eval("ID") %>' />
															</ItemTemplate>
														</asp:TemplateField>
													</Columns>
												</asp:GridView>
											</div>


										</div>


										<%-- this is area for Custom Field--%>
										<div class="col-6 px-1">
											<div class="row gx-2 gy-3">
												<div class=" col-12 " hidden>
													<label for="staticEmail" class="form-label">
														<asp:Label ID="Label2" Text="Stage" runat="server"></asp:Label>
														<asp:RequiredFieldValidator ID="rfvddlStage" runat="server" ControlToValidate="ddlStage" ErrorMessage="Required" Font-Bold="true" ForeColor="Red" InitialValue="0" ValidationGroup="Submit"></asp:RequiredFieldValidator>
													</label>

													<asp:DropDownList ID="ddlStage" runat="server" CssClass="form-control  form-control-sm chzn-select" AutoPostBack="true" OnSelectedIndexChanged="ddlStage_SelectedIndexChanged"></asp:DropDownList>

												</div>
												<div class=" col-12 ">
													<label for="staticEmail" class="form-label">
														<asp:Label ID="lblStatus" Text="Status" runat="server"></asp:Label>
														<asp:RequiredFieldValidator ID="rfvddlStatus" runat="server" ControlToValidate="ddlStatus" ErrorMessage="Required" InitialValue="0" Font-Bold="true" ForeColor="Red" ValidationGroup="Submit"></asp:RequiredFieldValidator>
													</label>
													<div class="col-md-6">
														<asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged"></asp:DropDownList>
													</div>
												</div>
											</div>
										</div>
									</div>
									<div class="  row mt-1 gy-3 gx-2 ">
										<div class="col-6 py-0 px-1">
											<asp:Repeater ID="rptOddControl" runat="server" OnItemDataBound="rptOddControl_ItemDataBound">
												<ItemTemplate>
													<div class=" row ">
														<div class="col-12">
															<label for="staticEmail" class="form-label">
																<asp:Label ID="lbl" Text='<%# Eval("FieldName") %>' runat="server"></asp:Label>

																<asp:RequiredFieldValidator ID="rfv" runat="server" ControlToValidate="txt" ErrorMessage="Required" Font-Bold="true" ForeColor="Red" ValidationGroup="UserScope"></asp:RequiredFieldValidator>
															</label>
															<asp:TextBox ID="txt" runat="server" CssClass="form-control  form-control-sm"></asp:TextBox>

														</div>
													</div>
												</ItemTemplate>
											</asp:Repeater>
											<asp:Repeater ID="rptddlOddControl" runat="server" OnItemDataBound="rptddlOddControl_ItemDataBound">
												<ItemTemplate>
													<div class=" row ">

														<label for="staticEmail" class="form-label">
															<asp:Label ID="lblOddlist" Text='<%# Eval("FieldName") %>' runat="server"></asp:Label>

															<asp:RequiredFieldValidator ID="rfvddl" runat="server" ControlToValidate="ddlOdd" ErrorMessage="Required" Font-Bold="true" ForeColor="Red" ValidationGroup="UserScope"></asp:RequiredFieldValidator>
														</label>


														<div class="col-sm-8 pr-5">

															<asp:DropDownList ID="ddlOdd" runat="server" CssClass="form-control  form-control-sm chzn-select"></asp:DropDownList>

														</div>

													</div>

													<%--		<asp:PlaceHolder ID="pnlOddTypeCustFlds" runat="server"></asp:PlaceHolder>--%>
												</ItemTemplate>
											</asp:Repeater>


										</div>
										<div class="col-6">
											<asp:Repeater ID="rptEvenControl" runat="server" OnItemDataBound="rptEvenControl_ItemDataBound">
												<ItemTemplate>
													<div class=" row gy-3 gx-2 mt-1">
														<div class="col-12">
															<label for="staticEmail" class="form-label">
																<asp:Label ID="lbleven" Text='<%# Eval("FieldName") %>' runat="server"></asp:Label>

																<asp:RequiredFieldValidator ID="rfvddl" runat="server" ControlToValidate="txteven" ErrorMessage="Required" Font-Bold="true" ForeColor="Red" ValidationGroup="UserScope"></asp:RequiredFieldValidator>
															</label>
															<asp:TextBox ID="txteven" runat="server" CssClass="form-control  form-control-sm" Text='<%# Eval("FieldMode") %>'></asp:TextBox>

														</div>

													</div>
													<%--<asp:PlaceHolder ID="pnlEvenTypeCustFlds" runat="server"></asp:PlaceHolder>--%>
												</ItemTemplate>
											</asp:Repeater>
											<asp:Repeater ID="rptddlEvenControl" runat="server" OnItemDataBound="rptddlEvenControl_ItemDataBound">
												<ItemTemplate>
													<div class=" row ">
														<div class="col-12">
															<label for="staticEmail" class="form-label">
																<asp:Label ID="lblEvenlist" Text='<%# Eval("FieldName") %>' runat="server"></asp:Label>

																<asp:RequiredFieldValidator ID="rfvddleveb" runat="server" ControlToValidate="ddlEven" ErrorMessage="Required" Font-Bold="true" ForeColor="Red" ValidationGroup="UserScope"></asp:RequiredFieldValidator>
															</label>




															<asp:DropDownList ID="ddlEven" runat="server" CssClass="form-control  form-control-sm chzn-select"></asp:DropDownList>

														</div>

													</div>

													<%--		<asp:PlaceHolder ID="pnlOddTypeCustFlds" runat="server"></asp:PlaceHolder>--%>
												</ItemTemplate>
											</asp:Repeater>
										</div>
									</div>

								
									<div class=" row mt-2 ">
										<div class="col-12">
											<label for="staticEmail" class="form-label">
												Description 
                                             <asp:RequiredFieldValidator ID="RfvtxtDescription" runat="server" ControlToValidate="txtDescription" ValidationGroup="Submit" ForeColor="Red" ErrorMessage="Required"></asp:RequiredFieldValidator>
											</label>

											<%--	<asp:TextBox ID="txtDescription" runat="server" CssClass="form-control form-control-sm" TextMode="MultiLine" MaxLength="500" Height="200px"></asp:TextBox>--%>
											<textarea id="txtDescription" runat="server" disabled="disabled" readonly="readonly"></textarea>
										</div>

										<div class="col-12">
											<label for="staticEmail" class="form-label">
												Notes 
                                             <asp:RequiredFieldValidator ID="rfvtxtNotes" runat="server" ControlToValidate="txtNotes" ValidationGroup="Submit" ForeColor="Red" ErrorMessage="Required"></asp:RequiredFieldValidator>
											</label>

											<%--	<asp:TextBox ID="txtDescription" runat="server" CssClass="form-control form-control-sm" TextMode="MultiLine" MaxLength="500" Height="200px"></asp:TextBox>--%>
											<textarea id="txtNotes" runat="server"></textarea>
										</div>

										<div class="col-12">
											<label for="staticEmail" class="form-label">
												Resolution 
											</label>

											<asp:RequiredFieldValidator ID="rfvtxtResolution" runat="server" ControlToValidate="txtResolution" ValidationGroup="Submit" ForeColor="Red" ErrorMessage="*"></asp:RequiredFieldValidator>

											<%--	<asp:TextBox ID="txtDescription" runat="server" CssClass="form-control form-control-sm" TextMode="MultiLine" MaxLength="500" Height="200px"></asp:TextBox>--%>
											<textarea id="txtResolution" runat="server"></textarea>
										</div>
									</div>
							</asp:Panel>

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

									<div class=" row mt-3">
										<label for="staticEmail" class="col-sm-2 labelcolorl1 pl-5">
											Impact Description<span class="red">*</span>
											<asp:RequiredFieldValidator ID="rfvtxtImpactDesc" runat="server" ControlToValidate="txtImpactDesc" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Tech">

											</asp:RequiredFieldValidator>
										</label>
										<div class="col-sm-10 pr-5">
											<asp:TextBox ID="txtImpactDesc" TextMode="MultiLine" Rows="4" Columns="10" class="form-control form-control-sm" runat="server"></asp:TextBox>
										</div>
										<div class="col-dm-2">
											<asp:Button ID="btnAddImpactDetails" runat="server" Text="Add" CssClass="btn btn-sm savebtn" CausesValidation="false" OnClick="btnAddImpactDetails_Click"></asp:Button>
										</div>
									</div>
									<div class="row">
										<div class="col-md-12">
											<asp:GridView ID="gridAddImpact" runat="server" CssClass="table table-head-fixed text-nowrap table-sm border "
												AutoGenerateColumns="false">
												<Columns>
													<asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="20">
														<ItemTemplate>
															<%#Container.DataItemIndex+1 %>
														</ItemTemplate>
													</asp:TemplateField>
													<asp:BoundField HeaderStyle-Width="120px" HeaderText="ImpactDetails" DataField="ImpactDetails" />
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
									<asp:Panel ID="pnlImpactGrid" runat="server">
										<div class="row " style="border-bottom: solid 1px">
											<div class="col-md-4">

												<asp:Label ID="Label4" runat="server" Text="Impact Details" Font-Size="Larger" ForeColor="Black"></asp:Label>

											</div>
											<div class="col-md-6">
												<asp:Label ID="Label11" runat="server"></asp:Label>
												<asp:Label ID="Label12" runat="server"></asp:Label>
											</div>
											<div class="col-md-2 " hidden>
												<div class="btn btn-sm elevation-1 ml-1 " style="padding: 0px; margin-bottom: 10px; padding-top: 1px">
													<label class="mr-2 ml-1 mb-0">Export</label>
													<asp:ImageButton ID="ImageButton3" runat="server" ImageUrl="~/Images/New folder/excelnew.png" CssClass="fa-pull-right btn-outline-success mr-1" />
												</div>
											</div>

										</div>
										<div class="table-responsive p-0" style="height: 400px">

											<asp:GridView ID="gvImpactGrid" runat="server" CssClass="table table-head-fixed text-nowrap  table-sm border" DataKeyNames="ID"
												AutoGenerateColumns="false" AllowSorting="True">
												<Columns>
													<asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="20">
														<ItemTemplate>
															<%#Container.DataItemIndex+1 %>
														</ItemTemplate>
													</asp:TemplateField>
													<asp:BoundField HeaderStyle-Width="120px" HeaderText="ImpactDetails" DataField="ImpactDescription" />
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
									</asp:Panel>

								</div>
							</div>
						</asp:Panel>

						<%--        ________________________ Grid will show RollOut Plan   _____________________________--%>

						<asp:Panel ID="pnlShowRollOutDetails" runat="server">
							<div class="card">
								<div class="card-body">
									<div class="row">
										<label>
											Roll Out Details
										</label>
									</div>
									<div class=" row mt-3">
										<label for="staticEmail" class="col-sm-2 labelcolorl1 pl-5">
											RollOut Description<span class="red">*</span>
											<asp:RequiredFieldValidator ID="rfvtxtRollOut" runat="server" ControlToValidate="txtRollOut" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="Tech">

											</asp:RequiredFieldValidator>
										</label>
										<div class="col-sm-10 pr-5">
											<asp:TextBox ID="txtRollOut" TextMode="MultiLine" Rows="4" Columns="10" class="form-control form-control-sm" runat="server"></asp:TextBox>
										</div>
										<div class="col-dm-2">
											<asp:Button ID="btnAddRollOutGrid" runat="server" Text="Add" CssClass="btn btn-sm savebtn" CausesValidation="false" OnClick="btnAddRollOutGrid_Click"></asp:Button>
										</div>
									</div>
									<div class="row">
										<div class="col-md-12">
											<asp:GridView ID="gridAddRollOut" runat="server" CssClass="table table-head-fixed text-nowrap  table-sm border"
												AutoGenerateColumns="false">
												<Columns>
													<asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="5px">
														<ItemTemplate>
															<%#Container.DataItemIndex+1 %>
														</ItemTemplate>
													</asp:TemplateField>
													<asp:BoundField HeaderStyle-Width="120px" HeaderText="RollOutDetails" DataField="RollOutDetails" />
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

									<asp:Panel ID="pnlRollOutGrid" runat="server">
										<div class="row " style="border-bottom: solid 1px">
											<div class="col-md-4">
												<asp:Label ID="Label5" runat="server" Text="RollOut Details" Font-Size="Larger" ForeColor="Black"></asp:Label>
											</div>
											<div class="col-md-6">
												<asp:Label ID="Label6" runat="server"></asp:Label>
												<asp:Label ID="Label7" runat="server"></asp:Label>
											</div>
											<div class="col-md-2 " hidden>
												<div class="btn btn-sm elevation-1 ml-1 " style="padding: 0px; margin-bottom: 10px; padding-top: 1px">
													<label class="mr-2 ml-1 mb-0">Export</label>
													<asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/New folder/excelnew.png" CssClass="fa-pull-right btn-outline-success mr-1" />
												</div>
											</div>

										</div>
										<div class="table-responsive table-container p-0" style="height: 400px">
											<asp:GridView ID="gvRollOutDetails" runat="server" CssClass="table table-head-fixed text-nowrap  " DataKeyNames="ID"
												AutoGenerateColumns="false" AllowSorting="True">
												<Columns>
													<asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="20">
														<ItemTemplate>
															<%#Container.DataItemIndex+1 %>
														</ItemTemplate>
													</asp:TemplateField>
													<asp:BoundField HeaderStyle-Width="120px" HeaderText="RollOutDetails" DataField="RolloutDescription" />
												</Columns>
												<EmptyDataTemplate>No Record Available</EmptyDataTemplate>
											</asp:GridView>
										</div>
									</asp:Panel>
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
											</label>
											<asp:TextBox ID="txtTaskSummary" class="form-control form-control-sm" runat="server"></asp:TextBox>
										</div>
										<div class="col-md-3">
											<label class="ml-1">
												Task Status :
											</label>
											<asp:DropDownList ID="ddlTaskStatus" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field">
												<asp:ListItem Text="Open" Selected="True" Value="Open"></asp:ListItem>
												<asp:ListItem Text="WIP" Value="WIP"></asp:ListItem>
												<asp:ListItem Text="Hold" Value="Hold"></asp:ListItem>
												<asp:ListItem Text="Closed" Value="Closed"></asp:ListItem>
											</asp:DropDownList>

										</div>
										<div class="col-md-3">
											<label class="ml-1">
												Technician Associaiton :
											</label>
											<asp:ListBox ID="lstTechAssoc" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" SelectionMode="Multiple"></asp:ListBox>
										</div>
									</div>
									<div class="row">
										<div class="col-md-1">
											<div class="col-dm-2">
												<asp:Button ID="btnAddTaskAssociationData" runat="server" Text="Add" CssClass="btn btn-sm savebtn" CausesValidation="false" OnClick="btnAddTaskAssociationData_Click"></asp:Button>
											</div>
										</div>
									</div>
									<div class="row">
										<div class="col-md-12">
											<asp:GridView ID="gvAddTask" runat="server" CssClass="table table-head-fixed text-nowrap  "
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

									<asp:Panel ID="pnlShowTaskDetails" runat="server">
										<div class="row " style="border-bottom: solid 1px">
											<div class="col-md-4">

												<asp:Label ID="Label8" runat="server" Text="Task Details" Font-Size="Larger" ForeColor="Black"></asp:Label>

											</div>
											<div class="col-md-6">
												<asp:Label ID="Label9" runat="server"></asp:Label>
												<asp:Label ID="Label10" runat="server"></asp:Label>
											</div>
											<div class="col-md-2 " hidden>
												<div class="btn btn-sm elevation-1 ml-1 " style="padding: 0px; margin-bottom: 10px; padding-top: 1px">
													<label class="mr-2 ml-1 mb-0">Export</label>
													<asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/Images/New folder/excelnew.png" CssClass="fa-pull-right btn-outline-success mr-1" />
												</div>
											</div>

										</div>
										<div class="table-responsive p-0" style="height: 400px">

											<asp:GridView ID="gvTaskDetails" runat="server" CssClass="table table-head-fixed text-nowrap table-sm border  " DataKeyNames="ID"
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
									</asp:Panel>
								</div>
							</div>
						</asp:Panel>
						<%--   <div class="row">
                           button postion 
                        </div>--%>
					</asp:Panel>

					<asp:Panel ID="pnlViewNotes" runat="server" Visible="false">

						<div class="row ">

							<div class="col-md-6">
								<asp:Label ID="Label1" runat="server"></asp:Label>
								<asp:Label ID="Label3" runat="server"></asp:Label>
							</div>
							<div class="col-md-12 text-end d-none">
								<asp:LinkButton ID="ImgBtnExport" runat="server" OnClick="ImgBtnExport_Click" CssClass="btn btn-sm btn-outline-success">Export</asp:LinkButton>
							</div>

						</div>


						<div class="row mt-2">
							<div class="col-md-8">
								<div class="table-responsive table-container">
									<asp:GridView GridLines="None" ID="gvTicketNotes" runat="server" AutoGenerateColumns="false" CssClass="table border table-sm data-table text-nowrap table-striped"
										Width="100%">
										<Columns>
											<asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="20">
												<ItemTemplate>
													<%#Container.DataItemIndex+1 %>
												</ItemTemplate>
											</asp:TemplateField>
											<asp:BoundField DataField="EditedDt" HeaderText="
										Event Time"
												SortExpression="EditedDt" DataFormatString="{0:yyyy-MM-dd hh:mm:ss}" />
											<asp:BoundField DataField="Ticketref" HeaderText="Ticket Number"
												SortExpression="Ticketref" />

											<asp:TemplateField HeaderText="Description" SortExpression="Description">

												<ItemTemplate>
													<asp:Label ID="lblDescription" runat="server" Text='<%# Server.HtmlDecode(Eval("NoteDesc").ToString()) %>'> </asp:Label>
												</ItemTemplate>
											</asp:TemplateField>
										</Columns>
									</asp:GridView>
								</div>
							</div>
							<div class="col-md-4">
								<div class="card">
									<div class="card-header">
										<div class="card-tittle">
											<asp:Label ID="Label14" runat="server" Text="Ticket Attachments" Font-Size="Medium" ForeColor="Black"></asp:Label>
										</div>
									</div>
									<div class="card-body">

										<div class="table-responsive table-container">
											<asp:GridView ID="gvTicketAttachment" runat="server" AutoGenerateColumns="false" DataKeyNames="Ticketref" EmptyDataText="No files uploaded" CssClass="table table-bordered" OnRowCommand="gvTicketAttachment_RowCommand">
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
													<asp:TemplateField HeaderText="Files">
														<ItemTemplate>
															<asp:LinkButton ID="lnkView" runat="server" Text="View" CausesValidation="False" CommandName="View" CommandArgument='<%# Eval("Filepath") %>'>
                                   
															</asp:LinkButton>
															<asp:HiddenField ID="hdnfullPath" runat="server" Value='<%# Eval("Filepath") %>' />
														</ItemTemplate>
													</asp:TemplateField>

													<asp:TemplateField HeaderText="Download">
														<ItemTemplate>
															<asp:LinkButton ID="lnkDownload" runat="server" CausesValidation="False" CommandArgument='<%# Eval("Filepath") %>'
																CommandName="Download" Text='<%# Eval("Filepath") %>' />
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
									</div>
								</div>
							</div>
						</div>
						<div class="row mt-2">
							<div class="col-md-12">
								<div class="table-responsive table-container">
									<asp:GridView ID="grdAttachmentView" runat="server" class="table border table-sm data-table text-nowrap table-striped" AutoGenerateColumns="false">
										<Columns>
											<asp:TemplateField HeaderText="Sr.No." ItemStyle-Width="20">
												<ItemTemplate>
													<%# Container.DataItemIndex + 1 %>
												</ItemTemplate>
											</asp:TemplateField>
											<asp:TemplateField HeaderText="Files">
												<ItemTemplate>
													<asp:LinkButton ID="lnkView" runat="server" OnClick="lnkView_Click">
														<asp:Label ID="lblfileName" class="text-dark" runat="server" Text='<%# Eval("fileName") %>'></asp:Label>
													</asp:LinkButton>
													<asp:HiddenField ID="hdnfullPath" runat="server" Value='<%# Eval("Filepath") %>' />
													<asp:HiddenField ID="hdnid" runat="server" Value='<%# Eval("ID") %>' />
												</ItemTemplate>
											</asp:TemplateField>

										</Columns>
									</asp:GridView>
								</div>
							</div>
						</div>
					</asp:Panel>
				</div>

			</div>

            <asp:HiddenField ID="hdfldDesk" runat="server" />
        </ContentTemplate>

        <Triggers>
            <asp:PostBackTrigger ControlID="ddlPriority" />
            <asp:PostBackTrigger ControlID="ddlSeverity" />
            <asp:PostBackTrigger ControlID="ddlStage" />
            <asp:PostBackTrigger ControlID="ddlStatus" />
            <asp:PostBackTrigger ControlID="ddlRequestType" />
            <asp:PostBackTrigger ControlID="btnUpdateTickView" />
            <asp:PostBackTrigger ControlID="btnViewNotes" />
            <asp:PostBackTrigger ControlID="ddlAssigne" />
            <asp:PostBackTrigger ControlID="ddlCategory1" />
            <asp:PostBackTrigger ControlID="ddlCategory2" />
            <asp:PostBackTrigger ControlID="ddlCategory3" />
            <asp:PostBackTrigger ControlID="ddlCategory4" />
            <asp:PostBackTrigger ControlID="ddlCategory5" />
            <asp:PostBackTrigger ControlID="lnkDownload" />
            <asp:PostBackTrigger ControlID="ddlCategory6" />
            <asp:PostBackTrigger ControlID="btnCancel" />
            <asp:PostBackTrigger ControlID="rptddlOddControl" />
            <asp:PostBackTrigger ControlID="rptddlEvenControl" />
            <asp:PostBackTrigger ControlID="rptOddControl" />
            <asp:PostBackTrigger ControlID="rptEvenControl" />
			  <asp:PostBackTrigger ControlID="gvTicketAttachment" />
            <asp:PostBackTrigger ControlID="btnUpdateTickView" />
            <asp:PostBackTrigger ControlID="btnViewNotes" />
            <asp:PostBackTrigger ControlID="btnImpactDetails" />
            <asp:PostBackTrigger ControlID="btnRolloutPlan" />
            <asp:PostBackTrigger ControlID="btnDowntime" />
            <asp:PostBackTrigger ControlID="btnTaskAssociation" />
            <asp:PostBackTrigger ControlID="grdAttachmentView" />
            <asp:PostBackTrigger ControlID="grd" />
            <asp:PostBackTrigger ControlID="btnUpload" />
            <asp:PostBackTrigger ControlID="lnkKBDownload" />
        </Triggers>

    </asp:UpdatePanel>
    <asp:HiddenField ID="hdnCategoryID" runat="server" />
	   <asp:HiddenField ID="hdnAssetSerialNo" runat="server" />
	   <asp:HiddenField ID="hdnPeripheralSno" runat="server" />
    <script type="text/javascript">
        function Showalert(imtype, imtitle) {
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
    <script src="assets/plugins/summernote/summernote-bs4.js"></script>
    <script>

        $(document).ready(function () {


            $('#<%= txtDescription.ClientID %>').summernote('disable');
            $('#<%= txtNotes.ClientID %>').summernote();
            $('#<%= txtResolution.ClientID %>').summernote();
        });
    </script>
    <script>
        function typewriterEffect(text, elementId, speed = 50) {
            const element = document.getElementById(elementId);
            let index = 0;
            element.innerHTML = '';
            const cursor = document.createElement('span');
            cursor.innerHTML = '|';
            cursor.className = 'typing-cursor';
            element.appendChild(cursor);
            const style = document.createElement('style');
            style.innerHTML = `
        @keyframes blink {
            0%, 100% { opacity: 1; }
            50% { opacity: 0; }
        }
        .typing-cursor {
            animation: blink 1s infinite;
            color: #2196F3;
            font-weight: bold;
        }
    `;
            document.head.appendChild(style);

            function type() {
                if (index < text.length) {
                    cursor.insertAdjacentText('beforebegin', text.charAt(index));
                    index++;
                    setTimeout(type, speed);
                }
            }
            type();
        }
    </script>
    <%--    <script>
        // Utility to show loader
        function showLoader() {
            const loader = document.getElementById('loader');
            loader.style.display = 'flex';
        }

        // Utility to hide loader
        function hideLoader() {
            const loader = document.getElementById('loader');
            loader.style.display = 'none';
        }
    </script>--%>
	
</asp:Content>


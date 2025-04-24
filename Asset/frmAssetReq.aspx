<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="frmAssetReq.aspx.cs" Inherits="frmAssetReq" %>

<!DOCTYPE html>

<%--<html xmlns="http://www.w3.org/1999/xhtml">--%>

<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title></title>
    <script src="assetsdata/plugins/summernote/jquery.js"></script>
    <link href="assetsdata/plugins/summernote/summernote-bs4.css" rel="stylesheet" />
    <script src="https://code.jquery.com/jquery-3.3.1.slim.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/js/bootstrap.bundle.min.js"></script>

    <link href="https://pcv-demo.hitachi-systems-mc.com:5723/assetsdata/css/bootstrap.min.css" rel="stylesheet">
    <link href="https://fonts.googleapis.com/css2?family=Noto+Sans:wght@300;400;500;600&amp;display=swap" rel="stylesheet">
    <link href="https://fonts.googleapis.com/css?family=Material+Icons+Outlined" rel="stylesheet">
    <link href="https://pcv-demo.hitachi-systems-mc.com:5723/sassdata/main.css" rel="stylesheet">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css">
    <link href="https://pcv-demo.hitachi-systems-mc.com:5723/assetsdata/plugins/select2/css/select2-bootstrap-5-theme.min.css" rel="stylesheet" />
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" rel="stylesheet">

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

        table td {
            color: #3d3d3d !important;
        }

        th {
            color: white !important;
        }
    </style>
</head>
<body style="background-image: url(Asset/img/bg005.jpg); height: auto; background-size: cover; background-repeat: round;">
    <form id="form1" runat="server">
        <div class="container-fluid mt-3">
            <asp:Label ID="lblerrorMsg" runat="server" Text="" CssClass="label" ForeColor="#ff0000"></asp:Label>

            <div class="card mb-1">
                <div class="card-header " style="background-color: #2c668d2e;">
                <div class="card-tittle row">
                    <div class="col-8">
                        <asp:Label ID="lblsofname" runat="server" Text="New Asset Request" CssClass="h6 mb-0"></asp:Label>
                    </div>
                    <div class="col-4 text-end">
                        <asp:LinkButton ID="lnkbtnHome" class="back-button" runat="server" CausesValidation="false" OnClick="lnkbtnHome_Click"> 

<i class="fas fa-arrow-left"></i></asp:LinkButton>

                    </div>
                </div>
            </div>
                <div class="card-body">
                    <div class="row g-2">
                        <div class="col-md-3"hidden>
                            <label class="form-label ">AD User Name </label>

                            <div class="input-group input-group-sm">
                                <asp:TextBox ID="txtsAMAccountName" runat="server" class="form-control form-control-sm" autocomplete="off" ValidationGroup="search"></asp:TextBox>
                                <asp:LinkButton ID="imgbtnSearch" runat="server" class="btn btn-sm btn-outline-secondary"   OnClick="imgbtnSearch_Click" ValidationGroup="search" >Search</asp:LinkButton>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <label class="form-label ">
                                Employee ID
                                        <asp:RequiredFieldValidator ID="rfvttxtEmpID" runat="server" ErrorMessage="*" ForeColor="Red" Font-Bold="true" ValidationGroup="save" ControlToValidate="txtEmpID"></asp:RequiredFieldValidator>
                            </label>
                            <asp:TextBox ID="txtEmpID" runat="server" class="form-control form-control-sm" Enabled="false" autocomplete="off" ValidationGroup="save"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <label class="form-label ">
                                User Name 
                                        <asp:RequiredFieldValidator ID="rfvtxtEmpName" runat="server" ControlToValidate="txtEmpName" ErrorMessage="*" ForeColor="Red" Font-Bold="true"></asp:RequiredFieldValidator>
                            </label>
                            <asp:TextBox ID="txtEmpName" runat="server" class="form-control form-control-sm" Enabled="false" ReadOnly="true" autocomplete="off"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <label class="form-label ">User Designation  </label>
                            <asp:TextBox ID="txtGrade" runat="server" class="form-control form-control-sm" autocomplete="off"></asp:TextBox>
                        </div>
                  
                        <div class="col-md-3">
                            <label class="form-label ">
                                Email ID  
                                <asp:RequiredFieldValidator ID="Requiredtxtemail" runat="server" ErrorMessage="*" ForeColor="Red" Font-Bold="true" ValidationGroup="save" ControlToValidate="txtemail"></asp:RequiredFieldValidator>

                            </label>
                            <asp:TextBox ID="txtemail" runat="server" class="form-control form-control-sm" Enabled="false" ReadOnly="true" autocomplete="off"></asp:TextBox>
                        </div>

                        <div class="col-md-3">
                            <label class="form-label ">
                                Contact No.  
                            </label>
                            <asp:TextBox ID="txtContactNo" runat="server" class="form-control form-control-sm" autocomplete="off" MaxLength="50"></asp:TextBox>
                        </div>

                        <div class="col-md-3">
                            <label class="form-label ">
                                Quantity    
                            </label>
                            <asp:TextBox ID="txtQuantity" runat="server" class="form-control form-control-sm" autocomplete="off">1</asp:TextBox>
                        </div>
                        <div class="col-md-3 ">
                            <label class="form-label ">
                                Location  
                             <asp:RequiredFieldValidator ID="rfvddlLocation" runat="server" InitialValue="0" ErrorMessage="*" ForeColor="Red" Font-Bold="true" ValidationGroup="save" ControlToValidate="ddlLocation"></asp:RequiredFieldValidator>
                            </label>
                            <asp:DropDownList ID="ddlLocation" runat="server" class="form-control form-control-sm chzn-select" AutoPostBack="True" OnSelectedIndexChanged="ddlLocation_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                  
                        <div class="col-md-3">
                            <label class="form-label ">
                                Approver Email ID  
                            </label>
                            <asp:TextBox ID="txtApprover" runat="server" class="form-control form-control-sm" Enabled="false" autocomplete="off"></asp:TextBox>
                        </div>
                        <div class="col-md-3" hidden>
                            <label class="form-label ">
                                Approver2 Email ID   
                            </label>
                            <asp:TextBox ID="txtApprover2" runat="server" class="form-control form-control-sm" autocomplete="off" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="col-md-3 ">
                            <label class="form-label ">
                                Department 

                                        <asp:RequiredFieldValidator ID="rfvddldepartment" runat="server" InitialValue="0" ErrorMessage="*" ForeColor="Red" Font-Bold="true" ValidationGroup="save" ControlToValidate="ddldepartment"></asp:RequiredFieldValidator>

                            </label>
                            <asp:DropDownList ID="ddldepartment" runat="server" class="form-control form-control-sm chzn-select" AutoPostBack="True" OnSelectedIndexChanged="ddldepartment_SelectedIndexChanged"></asp:DropDownList>
                        </div>

                        <div class="col-md-3 ">
                                    <label class="control-label ">
                                        Project :  
                                        <asp:RequiredFieldValidator ID="rfvddlProject" runat="server" ErrorMessage="Required" ForeColor="Red" Font-Bold="true" ValidationGroup="save" ControlToValidate="ddlProject"></asp:RequiredFieldValidator>

                                    </label>
                                    <asp:DropDownList ID="ddlProject" runat="server" class="form-control form-control-sm chzn-select"></asp:DropDownList>
                                </div>

                        <div class="col-md-3">
                            <label class="form-label ">
                                Asset Type  
                            <asp:RequiredFieldValidator ID="RequiredddlAssetType" runat="server" InitialValue="0" ErrorMessage="*" ForeColor="Red" Font-Bold="true" ValidationGroup="save" ControlToValidate="ddlAssetType"></asp:RequiredFieldValidator>
                            </label>
                            <asp:DropDownList ID="ddlAssetType" runat="server" class="form-control form-control-sm chzn-select" AutoPostBack="true" OnSelectedIndexChanged="ddlAssetType_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                        <div class="col-md-3">
                            <label class="form-label ">
                                Category  
                            <asp:RequiredFieldValidator ID="rfvtxtddlCategory" runat="server" InitialValue="0" ErrorMessage="*" ForeColor="Red" Font-Bold="true" ValidationGroup="save" ControlToValidate="ddlCategory"></asp:RequiredFieldValidator>
                            </label>
                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-control form-control-sm chzn-select" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" AutoPostBack="false"></asp:DropDownList>
                        </div>

                   

                        <div class="col-md-3" hidden>
                            <label class="form-label ">
                                Sub Category  
                            
                            </label>
                            <asp:DropDownList ID="ddlSubCategory" runat="server" class="form-control form-control-sm chzn-select"></asp:DropDownList>
                        </div>

                        <div class="col-md-3" hidden>
                            <label class="form-label ">
                                Make 
                            
                            </label>
                            <asp:DropDownList ID="ddlMake" runat="server" class="form-control form-control-sm chzn-select" AutoPostBack="True" OnSelectedIndexChanged="ddlMake_SelectedIndexChanged"></asp:DropDownList>
                        </div>
                        <div class="col-md-3" hidden>
                            <label class="form-label ">
                                Model  
                            
                            </label>
                            <asp:DropDownList ID="ddlModel" runat="server" class="form-control form-control-sm chzn-select"></asp:DropDownList>
                        </div>

                    
                        <div class="col-md-3 ">
                            <label class="form-label ">Request Type   </label>
                            <asp:DropDownList ID="ddlRequestType" runat="server" class="form-control form-control-sm chzn-select" OnSelectedIndexChanged="ddlRequestType_SelectedIndexChanged" AutoPostBack="True">
                                <asp:ListItem Value="New">New</asp:ListItem>
                                <asp:ListItem Value="Repair">Repair</asp:ListItem>
                                <asp:ListItem Value="Replacement">Replacement</asp:ListItem>
                                <asp:ListItem Value="Replacement of EOL">Replacement of EOL</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-3" hidden>
                            <label class="form-label ">
                                Serial No.   
                            </label>
                            <asp:TextBox ID="txtSerialNo" runat="server" class="form-control form-control-sm" autocomplete="off"></asp:TextBox>
                        </div>
                        <div class="col-md-3 ">
                            <label class="form-label ">
                                Remark  
                                        <asp:RequiredFieldValidator ID="RfvtxtReason" runat="server" ErrorMessage="*" ForeColor="Red" Font-Bold="true" ValidationGroup="save" ControlToValidate="txtReason"></asp:RequiredFieldValidator></label>

                            <asp:TextBox ID="txtReason" runat="server" class="form-control form-control-sm"></asp:TextBox>

                        </div>

                        <div class="col-md-3 ">
                            <label class="form-label ">
                                Incident Number 
          <asp:RequiredFieldValidator ID="rfvservicerqstno" runat="server" ErrorMessage="*" ForeColor="Red" Font-Bold="true" ValidationGroup="save" ControlToValidate="txtservicerqstno"></asp:RequiredFieldValidator></label>

                            <asp:TextBox ID="txtservicerqstno" runat="server" class="form-control form-control-sm "></asp:TextBox>

                        </div>



                        <div class="col-md-3 ">
                            <label class="form-label ">Attachment (If Any)   </label>
                            <asp:FileUpload ID="FileUploadPO" type="file" runat="server" CssClass="form-control form-control-sm" />
                            <asp:Label ID="labelPO" runat="server" Visible="false"></asp:Label>
                        </div>
                        <div class="col-md-12 text-end ">
                          
                            <asp:Label ID="lblApprover" runat="server"></asp:Label>
                            <asp:RegularExpressionValidator ID="Regulartxtemail" runat="server" ErrorMessage="Email format required" ForeColor="Red" Font-Bold="true" ValidationGroup="save" ControlToValidate="txtemail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                          <asp:Button ID="btnRequest" runat="server" Text="Submit" class="btn btn-sm btn-primary" OnClick="btnRequest_Click" ValidationGroup="save" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="card">
                <div class="card-body">
            
                        <div class="row" hidden>


                            <div class="col-md-3">
                                <label class="form-label ">Select Column   </label>
                                <asp:DropDownList ID="ddlSearchItems" runat="server" CssClass="form-control form-control-sm chzn-select-deselect" AutoPostBack="True" OnSelectedIndexChanged="ddlSearchItems_SelectedIndexChanged">

                                    <asp:ListItem Value="AssetStatus">Asset Status</asp:ListItem>
                                    <asp:ListItem Value="DepName">Department Name</asp:ListItem>
                                    <asp:ListItem Value="EmpEmailID">Employee EmailID</asp:ListItem>
                                    <asp:ListItem Value="EmpGrade">Employee Grade</asp:ListItem>
                                    <asp:ListItem Value="EmpID">Employee ID</asp:ListItem>
                                    <asp:ListItem Value="EmpName">Employee Name</asp:ListItem>
                                    <asp:ListItem Value="EmpContactNo">Employee Contact No</asp:ListItem>
                                    <asp:ListItem Value="FApprover">Approver Status</asp:ListItem>
                                    <asp:ListItem Value="Approver1">Approver EmailID</asp:ListItem>
                                    <asp:ListItem Value="LocAddress">Address</asp:ListItem>
                                    <asp:ListItem Value="Location">Location</asp:ListItem>
                                    <asp:ListItem Value="ProductName">ProductName</asp:ListItem>
                                    <asp:ListItem Value="RID">Request ID</asp:ListItem>
                                    <asp:ListItem Value="Site">Site</asp:ListItem>

                                </asp:DropDownList>

                            </div>

                            <div class="col-md-3">
                                <label class="form-label ">
                                    Enter Search Value  
                            <asp:RequiredFieldValidator ID="RequiredtxtSearch" runat="server" ControlToValidate="txtSearch" ErrorMessage="*" ForeColor="Red" ValidationGroup="Search"></asp:RequiredFieldValidator>
                                </label>
                                <div class="input-group">

                                    <asp:TextBox ID="txtSearch" runat="server" class="form-control form-control-sm input-search" placeholder="Search..."></asp:TextBox>
                                    <span class="input-group-append" style="margin-top: 0px; top: 0px">
                                        <asp:ImageButton ID="btnSearch" runat="server" class="btn c btn-outline-info" Style="margin-top: 0px; padding: 8px" ImageUrl="~/images/icons_search.png" OnClick="btnSearch_Click" ValidationGroup="Search" />
                                    </span>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-12 ">
                                <div class="table-responsive table-container" style="max-height:350px;">
                                  <div style="overflow-x: scroll; width: 100%">
                        <div style="overflow-y: scroll; height: 250px">
                                    <asp:GridView ID="gvHRRequest" runat="server" AutoGenerateColumns="false" CssClass="data-table table  border table-sm text-nowrap dataTable no-footer
" OnRowDataBound="gvHRRequest_RowDataBound">
                                        <Columns>
                                            <asp:BoundField HeaderText="Request Number" DataField="RequestNumber" />
                                            <asp:BoundField HeaderText="Emp Name" DataField="EmpName" />
                                            <asp:BoundField HeaderText="Emp ID" DataField="EmpID" />
                                            <asp:BoundField HeaderText="Designation" DataField="EmpGrade" />
                                            <asp:BoundField HeaderText="Emp EmailID" DataField="EmpEmailID" />
                                            <asp:BoundField HeaderText="Asset Category" DataField="ProductName" />
                                            <asp:BoundField HeaderText="Department" DataField="DepName" />
                                            <asp:BoundField HeaderText="Location" DataField="Location" />
                                            <asp:BoundField HeaderText="Approver" DataField="ITHeadEmail" />
                                            <asp:BoundField HeaderText="Request Status" DataField="FApprover" />
                                            <asp:BoundField HeaderText="Asset Status" DataField="AssetStatus" />
                                            <asp:BoundField HeaderText="Request Time" DataField="RequestDateTime" />
                                        </Columns>
                                       
                                    </asp:GridView></div></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
       
    </form>


</body>


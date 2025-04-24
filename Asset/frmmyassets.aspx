<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="frmmyassets.aspx.cs" Inherits="frmmyassets" %>

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
                        <asp:Label ID="lblsofname" runat="server" Text="My Assets" CssClass="h6 mb-0"></asp:Label>
                    </div>
                    <div class="col-4 text-end">
                        <asp:LinkButton ID="lnkbtnHome" class="back-button" runat="server" CausesValidation="false" OnClick="lnkbtnHome_Click"> 

<i class="fas fa-arrow-left"></i></asp:LinkButton>

                    </div>
                </div>
            </div>
               
            </div>


            <div class="card">
                <div class="card-body">
                
                       
                        <div class="row">
                            <div class="col-md-12 ">
                                <div class="table-responsive table-container" style="max-height:350px">
<div style="overflow-x: scroll; width: 100%">
                        <div style="overflow-y: scroll; height: 250px">
                                    <asp:GridView ID="gvHRRequest" runat="server" AutoGenerateColumns="false" EmptyDataText="No Records Found" CssClass="data-table table  border table-sm text-nowrap dataTable no-footer" >
                                        <Columns>
                                            <asp:BoundField HeaderText="Serial No." DataField="SerialNo" />
                                            <asp:BoundField HeaderText="BTWI Asset No." DataField="Asset_Tag_ID" />
                                            <asp:BoundField HeaderText="Asset Type" DataField="AssetType" />
                                            <asp:BoundField HeaderText="Asset Category" DataField="CategoryID" />
                                            <asp:BoundField HeaderText="Make" DataField="ManufacturerID" />
                                            <asp:BoundField HeaderText="Model" DataField="ModelCode" />
                                            <asp:BoundField HeaderText="Location" DataField="LocationID" />
                                            <asp:BoundField HeaderText="Purchase Date" DataField="Purchase_Date" DataFormatString="{0:yyyy-MM-dd}" />
                                            <asp:BoundField HeaderText="Warranty Expiration Date" DataField="Warranty_Expiration_Date" DataFormatString="{0:yyyy-MM-dd}" />
 
                                            <asp:BoundField HeaderText="Employee Email" DataField="Assigned_to_person_email" />                                            
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


<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="SDDBCategoryWise.aspx.cs" Inherits="Dashboard_SDDBCategoryWise" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">

    <div class="card mb-1">
        <div class="card-body">
            <div class="row gy-2 gx-3">
                <div class="col-md-3">
                    <label class="form-label">
                        Select Desk 
                    </label>
                    <asp:DropDownList ID="DropDesks" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field description-label">
                    </asp:DropDownList>
                </div>
                <div class="col-md-3" hidden>
                    <asp:Label ID="Label1" runat="server" Text="Label" Visible="false"></asp:Label>
                    <label class="form-label ">
                        Select Category 
                    </label>
                    <asp:DropDownList ID="ddlCategories" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field description-label">
                    </asp:DropDownList>
                </div>

                <div class="col-md-2">
                    <asp:Label ID="lblError" runat="server"></asp:Label>
                    <label class="form-label ">
                        From
                        <asp:RequiredFieldValidator ID="RequiredFieldtxtFrom" runat="server" ControlToValidate="txtFrom" ErrorMessage="Required" ForeColor="Red" ValidationGroup="SU"></asp:RequiredFieldValidator>
                    </label>
                    <div class="input-group mb-3">
                        <asp:TextBox AutoCompleteType="None" runat="server" Placeholder="" ID="txtFrom" class="form-control form-control-sm datepicker"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-2">
                    <label class="form-label ">
                        To
                        <asp:RequiredFieldValidator ID="RequiredtxtTo" runat="server" ControlToValidate="txtTo" ErrorMessage="Required" ForeColor="Red" ValidationGroup="SU"></asp:RequiredFieldValidator>
                    </label>
                    &nbsp;
                            	<div class="input-group mb-3">
                                    <asp:TextBox AutoCompleteType="None" runat="server" Placeholder="" ID="txtTo" class="form-control form-control-sm datepicker"></asp:TextBox>
                                </div>
                </div>
                <div class="col-md-3 ">
                    <label class="form-label opacity-0">askjfks</label>
                    <br />
                    <asp:Button ID="btnSearch" runat="server" Text="Search" ValidationGroup="SU" CssClass="btn btn-sm btn-grd-info" OnClick="btnSearch_Click" />
                </div>
            </div>

        </div>
    </div>
    <div class="card mb-1">
        <div class="card-header">
            <h6 class="mb-0 card-title">Tickets by Category</h6>
        </div>
        <div class="card-body p-0">
            <div class="row">
                <div class="col-md-12 " id="divCategory" runat="server">
                
                        <div class="card-body">
                            <div class="d-flex align-items-center justify-content-between">
                                <asp:Button ID="btncategory" runat="server" OnClick="btncategory_Click" Style="visibility: hidden;" />
                                
                            </div>
                            <div id="chart1"></div>
                        </div>
                    </div>
             
             
                  
               
            </div>
        </div>
    </div>
     <div class="card">
     <div class="card-header">
         <h6 class="mb-0 card-title">Category Wise Tickets Status</h6>
     </div>
     <div class="card-body p-0">
         <div class="row">
                <div class="col-md-12 " id="divStatus" runat="server">
      
       
               <div class="d-flex align-items-center justify-content-between">
                   <asp:Button ID="btncategorystatus" runat="server" OnClick="btncategory_Click" Style="visibility: hidden;" />
                  
               </div>
               <div id="chart2"></div>
           </div>
         </div>
         </div>
         </div>

    <asp:Literal ID="chartScript" runat="server" />
    <asp:Literal ID="ltrchartcategory" runat="server" />
    <asp:Literal ID="ltrchartcategorystatus" runat="server" />
    <asp:HiddenField ID="hdnfldVariable" runat="server" />
    <asp:HiddenField ID="hdnfldStatus" runat="server" />
    <script src="https://pcv-demo.hitachi-systems-mc.com:5723/assetsdata/plugins/apexchart/apexcharts.min.js"></script>

</asp:Content>


<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmFeedbackConfiguration.aspx.cs" Inherits="frmFeedbackConfiguration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="card">
        <div class="card-body">
            <div class="row gy-2 gx-3">
                <div class="col-md-12">
                    <label class="form-label ">
                        Question 
                               
                        <asp:RequiredFieldValidator ID="RequiredQuestion" runat="server" ControlToValidate="txtQuestion" ErrorMessage="Required" ForeColor="Red" ValidationGroup="GroupSave"></asp:RequiredFieldValidator>
                    </label>
                    <asp:TextBox ID="txtQuestion" runat="server" CssClass="form-control  form-control-sm" TextMode="MultiLine"></asp:TextBox>
                </div>
                <div class="col-md-4">
                    <label class="form-label ">
                        Status    
   
                        <asp:RequiredFieldValidator ID="RequiredStatus" runat="server" ControlToValidate="ddlStatus" ErrorMessage="Required" ForeColor="Red" ValidationGroup="GroupSave"></asp:RequiredFieldValidator>
                    </label>
                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-select form-select-sm">
                        <asp:ListItem Value="Active"></asp:ListItem>
                        <asp:ListItem Value="Inactive"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-2">
                    <label class="form-label opacity-0">fkjsl</label><br />
                    <asp:Button ID="btnQAdd" runat="server" Text="Add" OnClick="btnQAdd_Click" ValidationGroup="GroupSave" CssClass="btn btn-sm btn-grd-info" />
                    <asp:Button ID="btnQUpdate" runat="server" OnClick="btnQUpdate_Click" Text="Update" CssClass="btn btn-sm btn-grd-info" ValidationGroup="GroupSave" />
                    <asp:Button ID="btnQCancel" runat="server" OnClick="btnQCancel_Click" Text="Cancel" CssClass="btn btn-sm btn-grd-danger" />
                </div>
                <div class="col-md-12">
                    <div class="table-responsive table-container" style="max-height: 300px">
                        <asp:GridView GridLines="None" ID="GridQuestions" runat="server" AutoGenerateColumns="false" DataKeyNames="QuestionId" CssClass="table table-sm border text-nowrap table-striped">
                            <Columns>
                                <asp:TemplateField HeaderText="Sr.No.">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Question" DataField="Question" />
                                <asp:BoundField HeaderText="Status" DataField="Status" />
                                <asp:TemplateField HeaderText="Aciton">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdnid" Value='<%#Eval("QuestionId")%>' runat="server" />
                                        <%--<asp:Button Text="Edit" runat="server" OnClick="Unnamed1_Click" />--%>
                                        <asp:LinkButton runat="server" ID="lnkdelete" OnClick="Unnamed1_Click">
<i class="fa-solid fa-edit"></i>
              </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div class="col-md-4">
                    <label class="form-label ">
                        Select Question 
           
                        <asp:RequiredFieldValidator ID="RFVaddoption" runat="server"
                            ControlToValidate="ddlQuestions" Display="Dynamic"
                            ErrorMessage="Required" ForeColor="red"
                            InitialValue="----Select----" ValidationGroup="option"></asp:RequiredFieldValidator>
                        <asp:Label ID="lblmessge" runat="server" Text=""></asp:Label>
                    </label>
                    <asp:DropDownList ID="ddlQuestions" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field" AutoPostBack="True" OnSelectedIndexChanged="ddlQuestions_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
                <div class="col-md-8">
                    <label class="form-label ">
                        Option 
                        <asp:RequiredFieldValidator ID="RFVatxtOption" runat="server"
                            ControlToValidate="txtOption" Display="Dynamic"
                            ErrorMessage="Required" ForeColor="red"
                            ValidationGroup="option"></asp:RequiredFieldValidator>

                    </label>
                    <asp:TextBox ID="txtOption" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
                </div>
                <div class="col-md-12 text-end">
                    <asp:Button ID="btnOptAdd" runat="server" OnClick="btnOptAdd_Click" Text="Save" ValidationGroup="option" CssClass="btn btn-sm btn-grd-info" />
                    <asp:Button ID="btnOptUpdate" runat="server" Text="Update" OnClick="btnOptUpdate_Click" ValidationGroup="option" CssClass="btn btn-sm btn-grd-info" />
                    <asp:Button ID="btnOptDelete" runat="server" Text="Delete" OnClick="btnOptDelete_Click" ValidationGroup="option" CssClass="btn btn-sm btn-grd-danger" />
                    <asp:Button ID="btnOptCancel" runat="server" OnClick="btnQCancel_Click" Text="Cancel" CssClass="btn btn-sm btn-grd-danger" />
                </div>
                <div class="col-md-12">
                    <div class="table-responsive table-container" style="max-height: 300px">
                        <asp:GridView GridLines="None" ID="gvOption" runat="server" AutoGenerateColumns="False" DataKeyNames="id"
                            CssClass="table table-sm border text-nowrap table-striped">
                            <Columns>
                                <asp:TemplateField HeaderText="Select" ItemStyle-Width="20px">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lblSelect" runat="server" OnClick="lblSelect_Click" CssClass="lbl"> <i class="fa-solid fa-check border rounded-circle p-1 btn btn-sm btn-outline-secondary"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Survey Questions" ItemStyle-Width="300px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblquestions" runat="server" Text='<%# Bind("Question") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Survey Options" ItemStyle-Width="300px">
                                    <ItemTemplate>
                                        <asp:Label ID="lblquestionoption" runat="server" Text='<%# Bind("Question_option") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <%-- <RowStyle BackColor="#fafafa" BorderColor="#e3e4e6" BorderWidth="1px" />
         <FooterStyle BackColor="#EDEDED" Font-Bold="True" ForeColor="White" />
         <PagerStyle BackColor="#EDEDED" ForeColor="#000000" HorizontalAlign="Center" />
         <SelectedRowStyle BackColor="#E3E4E6" Font-Bold="True" ForeColor="#000000" />
         <HeaderStyle BackColor="#e3e4e6" Font-Bold="True" ForeColor="#000000" Height="30px" Font-Size="Small" />
         <EditRowStyle BackColor="#EDEDED" BorderColor="#e3e4e6" BorderStyle="Solid" BorderWidth="1px" />
         <AlternatingRowStyle BackColor="White" BorderColor="#e3e4e6" BorderStyle="Solid" BorderWidth="1px" />--%>
                        </asp:GridView>
                    </div>
                </div>
            </div>


        </div>
    </div>
    <script>
        // Get the existing classes
        let currentClasses = element.classList.toString();

        // Add the new class without removing existing ones
        if (!currentClasses.includes('aspNetDisabled')) {
            element.classList.add('aspNetDisabled');
        }

    </script>
</asp:Content>



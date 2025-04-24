<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControl.ascx.cs" Inherits="UserControl" %>
<asp:UpdatePanel ID="UpdatePanel2" runat="server">
    <ContentTemplate>
        <div class="row">
            <div class="col-md-12  ">

                <div class="card card-default">
                    <div class="card-header c">
                        Request Type
                    </div>
                    <div class="card-body">
                        <div class="row">
                            <div class="col-md-10">
                                <label for="staticEmail" class=" col-form-label">
                                    Organization: <span title="*"></span>

                                    <asp:RequiredFieldValidator ID="rfvddlOrg" runat="server" ControlToValidate="ddlOrg" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="ReqType"></asp:RequiredFieldValidator>
                                </label>



                                <asp:DropDownList ID="ddlOrg" runat="server" CssClass="form-control form-control-sm chzn-select">
                                </asp:DropDownList>
                            </div>


                        </div>
                        <div class="row">
                            <div class="col-md-10">

                                <label class=" col-form-label">
                                    Request Type : <span title="*"></span>
                                    <asp:RequiredFieldValidator ID="rfvReqType" runat="server" ControlToValidate="txtRequestType" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="ww"></asp:RequiredFieldValidator>
                                </label>


                                <asp:TextBox ID="txtRequestType" runat="server" CssClass="form-control  form-control-sm"></asp:TextBox>

                            </div>
                        </div>
                        <div class="row mt-2">
                            <div class="col-md-10">

                                <label class=" col-form-label">
                                    Request Description : <span title="*"></span>
                                    <asp:RequiredFieldValidator ID="rfvRequestDesc" runat="server" ControlToValidate="txtReqDescription" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="ww"></asp:RequiredFieldValidator>
                                </label>


                                <asp:TextBox ID="txtReqDescription" runat="server" TextMode="MultiLine" Rows="1" CssClass="form-control  form-control-sm"></asp:TextBox>

                            </div>
                        </div>
                        <div class="form-row">
                            <div class="col-md-3 ">

                                <asp:LinkButton ID="lnkSaveReq" runat="server" Text="Save" OnClick="btnSaveReqType_Click" class="btn btn-sm savebtn" ValidationGroup="ww"></asp:LinkButton>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="lnkSaveReq" />
    </Triggers>
</asp:UpdatePanel>

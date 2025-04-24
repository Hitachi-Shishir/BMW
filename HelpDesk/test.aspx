<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="test.aspx.cs" Inherits="HelpDesk_test" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
<div id="stepper1" class="bs-stepper">
  <div class="bs-stepper-header">
    <!-- Step buttons here -->
    <div class="step" data-target="#step1">
      <button type="button" class="step-trigger">
        <span class="bs-stepper-label">Step 1</span>
      </button>
    </div>
    <div class="step" data-target="#step2">
      <button type="button" class="step-trigger">
        <span class="bs-stepper-label">Step 2</span>
      </button>
    </div>
  </div>

  <div class="bs-stepper-content">
    <!-- Your step content here -->
    <div id="step1" class="content">Step 1 Content</div>
    <div id="step2" class="content">Step 2 Content</div>
  </div>
</div>

<!-- Next and Previous buttons -->
<asp:LinkButton ID="lnkPrev" runat="server" OnClientClick="stepper.previous(); return false;" OnClick="lnkPrev_Click">Previous</asp:LinkButton>
<asp:LinkButton ID="lnkNext" runat="server" OnClientClick="stepper.next(); return false;" OnClick="lnkNext_Click">Next</asp:LinkButton>
<script>
    document.addEventListener('DOMContentLoaded', function () {
        window.stepper = new Stepper(document.querySelector('#stepper1'));
    });

</script>
</asp:Content>


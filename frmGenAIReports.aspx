<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmGenAIReports.aspx.cs" Inherits="frmGenAIReports" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .ai-output {
            font-family: monospace;
            background-color: #f5f5f5;
            padding: 15px;
            border-radius: 5px;
            height: 65vh !important;
            overflow-y: auto;
            display: block;
            white-space: pre-wrap;
            /* Firefox custom scrollbar */
            scrollbar-width: thin; /* Makes scrollbar thinner */
            scrollbar-color: #555 #e0e0e0; /* Thumb color, Track color */
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

        [data-bs-theme=semi-dark] body .card1 {
            background-image: linear-gradient(127.09deg, rgb(149 149 149 / 94%) 19.41%, rgb(255 255 255 / 49%) 76.65%) !important;
            box-shadow: rgba(0, 0, 0, 0.45) 1px 1px 40px 0px inset;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="scr" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="updatepanel1" runat="server">
        <ContentTemplate>
            <div class="card mb-1 card1" style="background-color: black">
                <div class="card-body" style="height: 68vh">
                    <div class="row g-3">
                        <div class="col-md-12">
                            <asp:Label ID="lblPyoutput" runat="server" CssClass="ai-output mt-0"></asp:Label>
                        </div>
                    </div>
                </div>
                <div class="card-footer bg-transparent border-top-0">
                    <div class="col-md-12 position-relative m-auto mb-1">
                        <%--<label class="form-label">Write Your Requirement</label>--%>
                        <asp:TextBox ID="txtRequirement" runat="server" CssClass="form-control form-control-lg"
                            Style="border-radius: 2.8rem; height: 48px; font-size: 14px; padding: 5px 65px; font-family: monospace;"
                            placeholder="">
                        </asp:TextBox>

                        <asp:LinkButton ID="btnGenReport" runat="server" CssClass="btn btn-secondary border position-absolute rounded-circle"
                            tittle="View Report" OnClick="btnGenReport_Click" Style="top: 5px; right: 11px;"> <i class="fa-solid fa-arrow-right"></i> </asp:LinkButton>

                        <%--<asp:Button ID="btnGenReport" runat="server" CssClass="btn btn-grd-info btn-sm position-absolute " Text="View Report" OnClick="btnGenReport_Click" Style="top: 5px; right: 98px;" />--%>
                        <Asp:LinkButton runat="server" id="btnDownloadReport" class="btn btn-light border position-absolute rounded-circle" style="top: 5px; right: calc(100% - 51px);" title="Download Report" OnClick="btnDownloadReport_Click">
                            <i class="fa-solid fa-download"></i>
                        </Asp:LinkButton>


                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnDownloadReport" />
        </Triggers>
    </asp:UpdatePanel>
    <%--<div class="card bg-transparent ">--%>

    <%--</div>--%>

    <script>
        function typewriterEffect(text, elementId, speed = 50) {
            const element = document.getElementById(elementId);
            let index = 0;

            // Clear existing text
            element.innerHTML = '';

            // Create cursor element
            const cursor = document.createElement('span');
            cursor.innerHTML = '|';
            cursor.className = 'typing-cursor';
            element.appendChild(cursor);

            // Add blinking cursor style
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
                    // Insert character before cursor
                    cursor.insertAdjacentText('beforebegin', text.charAt(index));
                    index++;
                    setTimeout(type, speed);
                }
            }

            type();
        }
    </script>
    <script>
        document.addEventListener("DOMContentLoaded", function () {
            const input = document.getElementById("<%= txtRequirement.ClientID %>"); // Get the ASP.NET rendered ID
            const placeholderText = "Write Your Requirement ....";
            let index = 0;

            function typePlaceholder() {
                if (index < placeholderText.length) {
                    input.setAttribute("placeholder", placeholderText.substring(0, index + 1));
                    index++;
                    setTimeout(typePlaceholder, 100); // Typing speed
                } else {
                    setTimeout(() => {
                        index = 0;
                        input.setAttribute("placeholder", ""); // Clear and restart typing
                        setTimeout(typePlaceholder, 500); // Delay before retyping
                    }, 2000); // Pause before clearing and restarting
                }
            }

            typePlaceholder(); // Start the typing effect
        });
    </script>

</asp:Content>


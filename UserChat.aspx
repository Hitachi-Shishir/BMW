<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="UserChat.aspx.cs" Inherits="UserChat" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">



    <style>
        .chat-sidebar-header .chat-user-online:before {
            width: 6px;
            height: 6px;
            left: 37px;
        }

        .xwhite {
            color: #b5b5b5;
            font-size: 18px !important;
        }

        [data-bs-theme=light].xwhite {
            color: black;
            font-size: 18px !important;
        }

        [data-bs-theme=semi-dark].xwhite {
            color: black;
            font-size: 18px !important;
        }

        .chat-footer {
            left: 299px;
        }

        .chat-wrapper {
            width: auto;
            height: 81vh !important;
        }

        .chat-content {
            margin-left: 285px !important;
        }

        .chat-header {
            left: 299px;
            height: 49px;
        }

        .chat-sidebar {
            width: 299px;
            z-index: 0;
        }

        .chat-content {
            margin-left: 327px;
        }

        [data-bs-theme=dark] .chat-sidebar {
            background-color: #212529;
        }

        [data-bs-theme=dark] .chat-sidebar-header {
            background-color: #212529;
        }

        [data-bs-theme=dark] .chat-wrapper {
            background-color: #212529;
        }

        .truncate {
            width: 200px; /* Set width to control truncation */
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
        }

        .truncate {
            width: 200px; /* Set width to control truncation */
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
        }

        .chat-wrapper {
            width: auto;
            height: 100vh;
        }

        .status-indicator {
            width: 6px;
            height: 6px;
            border-radius: 50%;
            display: inline-block;
            left: 16%;
            bottom: 26%;
            position: absolute;
            position: absolute;
        }

            .status-indicator.online {
                background-color: #16e15e;
                box-shadow: 0 0 0 2px #fff;
            }

            .status-indicator.offline {
                background-color: red;
                box-shadow: 0 0 0 2px #fff;
            }


        .unread-badge {
            min-width: 20px;
            height: 20px;
            display: flex;
            align-items: center;
            justify-content: center;
        }

        .user-list {
            height: 100vh;
            overflow-y: auto;
            padding-bottom: 8rem;
        }

        .user-item {
            /*        padding: 10px 15px;
            border-bottom: 1px solid #eee;
            cursor: pointer;
            transition: background-color 0.2s;*/
        }

            .user-item:hover {
                /*background-color: #f8f9fa;*/
            }

        .message-count .badge {
            font-size: 0.75rem;
            padding: 0.25em 0.6em;
        }

        body {
            background-color: #f5f5f5;
        }




        .chat-panel {
            flex-grow: 1;
            /*background: white;*/
            border-radius: 10px;
            box-shadow: 0 2px 5px rgba(0,0,0,0.1);
            padding: 15px 0px 15px 15px;
        }

        #messagesPvt {
            height: 70vh;
            padding: 0px;
            padding-top: 75px;
            border-radius: 10px;
            overflow-y: auto;
            padding-bottom: 2rem;
            background-image: url(emoji/bg3.png);
        }

        .message-group {
            display: flex;
            margin-bottom: 9px;
        }

            .message-group.sent {
                justify-content: flex-end;
            }

            .message-group.received {
                justify-content: flex-start;
            }

        .message-bubble {
            max-width: 70%;
            padding: 12px 15px;
            border-radius: 15px;
            font-size: 14px;
            word-wrap: break-word;
        }

        .message-group.sent .message-bubble {
            width: fit-content;
            background-color: #dcedff;
            padding: 0.5rem;
            border-radius: 12px;
            float: right;
            max-width: 600px;
            text-align: left;
            border-bottom-right-radius: 0;
            color: #fff;
            background-image: linear-gradient(310deg, #7928ca, #ff0080) !important;
            margin-right: 10px;
        }

        .message-group.received .message-bubble {
            background-image: linear-gradient(310deg, #00c6fb 0%, #005bea 100%) !important;
            width: fit-content;
            background-color: #dcedff;
            padding: 0.5rem;
            border-radius: 12px;
            float: right;
            max-width: 600px;
            text-align: left;
            border-bottom-left-radius: 0;
            color: #fff;
            margin-left: 1rem;
        }

        .message-time {
            font-size: 11px;
            margin-top: 5px;
            opacity: 0.8;
        }



        .user-item:hover {
            background-color: #f8f9fa;
        }

        .user-item.active {
            background-color: #e9ecef;
        }

        /* Modal Styles */
        /*   .modalBackground {
            background-color: rgba(0,0,0,0.5);
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            z-index: 1000;
        }*/ .chat-sidebar {
            width: 299px;
            z-index: 1030;
            transition: all 0.3s ease;
        }

        @media (max-width: 768px) {
            .chat-sidebar {
                position: fixed;
                left: -299px;
                height: 100vh;
            }

                .chat-sidebar.show {
                    left: 0;
                }

            .chat-content {
                margin-left: 0 !important;
            }

            .chat-header {
                left: 0 !important;
            }

            .chat-footer {
                left: 0 !important;
            }
        }

        .toggle-sidebar {
            display: none;
            background: none;
            border: none;
            padding: 8px;
            margin-right: 10px;
        }

        @media (max-width: 768px) {
            .toggle-sidebar {
                display: block;
            }
        }

        .toggle-sidebar i {
            font-size: 1.25rem;
        }

        .sidebar-overlay {
            display: none;
            position: fixed;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: rgba(0, 0, 0, 0.5);
            z-index: 1020;
        }

            .sidebar-overlay.show {
                display: block;
            }

        .close-sidebar {
            display: none;
            background: none;
            border: none;
            padding: 8px;
            color: #6c757d;
        }

            .close-sidebar:hover {
                color: #000;
            }

        @media (max-width: 768px) {
            .close-sidebar {
                display: block;
            }
        }

        /* Add this to your existing media query */
        @media (max-width: 768px) {
            .chat-sidebar-header {
                padding-right: 15px;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:ScriptManager ID="sc" runat="server"></asp:ScriptManager>



    <div class=" chat-wrapper">
        <div class="sidebar-overlay" id="sidebarOverlay"></div>
        <!-- Users Panel -->
        <div class=" chat-sidebar">
            <div class="chat-sidebar-header">
                <div class="d-flex align-items-center">
                    <div class="chat-user-online">
                        <asp:Image ID="img" runat="server" class="rounded-circle" Width="45" Height="45" alt="" />
                    </div>
                    <div class="flex-grow-1 ms-3">
                        <p class="mb-0">
                            <asp:Label runat="server" ID="lblMyname"></asp:Label>
                        </p>
                        <asp:Label runat="server" ID="connId" class="badge bg-primary" Visible="false"></asp:Label>
                    </div>
                    <button type="button" class="close-sidebar ">
                        <i class="fas fa-times"></i>
                    </button>
                </div>
                <div class="mb-3"></div>

                <%--<input type="text" class="form-control form-control-sm rounded-4" id="searchInput" placeholder="Search Chat.." onkeyup="filterPage();">--%>
                <input type="text" class="form-control form-control-sm rounded-4" id="searchInput" placeholder="Search Chat.." onkeyup="filterPage(this.value);">
            </div>
            <div class="user-list table-container chat-list  ps--active-y">
                <div class="list-group list-group-flush pb-5">
                    <%--<asp:Repeater ID="RepeaterButtons" runat="server">
                        <ItemTemplate>
                            <div class="user-item list-group-item searchable" data-userid='<%# Eval("SD_UID") %>' onclick="startPrivateChat('<%# Eval("SD_UID") %>', '<%# Eval("UserName") %>')">
                                <div class="d-flex align-items-center justify-content-between">

                                    <img src="assets/images/avatars/02.png" width="42" height="42" class="rounded-circle" alt="">

                                    <div class="user-info flex-grow-1 ms-2">
                                        <div class=" align-items-center">
                                            <span class="status-indicator offline"></span>
                                            <h6 class="mb-0 ms-2 chat-title"><%# Eval("UserName") %></h6>
                                            <p class="last-message mb-0 chat-msg ms-2 truncate"></p>
                                        </div>
                                    </div>
                                    <div class="message-count">
                                        <span class="unread-badge badge bg-grd-success rounded-pill d-none">0</span>
                                    </div>
                                </div>
                            </div>

                        </ItemTemplate>
                    </asp:Repeater>--%>
                    <asp:Repeater ID="RepeaterButtons" runat="server">
                        <ItemTemplate>
                            <div class="user-item list-group-item searchable" data-userid='<%# Eval("SD_UID") %>' onclick="startPrivateChat('<%# Eval("SD_UID") %>', '<%# Eval("UserName") %>')">
                                <div class="d-flex align-items-center justify-content-between">

                                    <!-- Dynamically set image from database -->
                                    <%--<img src='<%# Eval("Filedata", "{0}") %>' width="42" height="42" class="rounded-circle" alt="User Image">--%>
                                    <img id="imgUser" src='<%# Eval("Filedata") != DBNull.Value && !string.IsNullOrEmpty(Eval("Filedata").ToString()) ? Eval("Filedata") : "/Images/defimg.png" %>' width="38" height="38" class="rounded-circle" alt="User Image">

                                    <div class="user-info flex-grow-1 ms-2">
                                        <div class="align-items-center">
                                            <span class="status-indicator offline"></span>
                                            <h6 class="mb-0 ms-2 chat-title"><%# Eval("UserName") %></h6>
                                            <p class="last-message mb-0 chat-msg ms-2 truncate"></p>
                                        </div>
                                    </div>
                                    <div class="message-count">
                                        <span class="unread-badge badge bg-grd-success rounded-pill d-none">0</span>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>



                </div>
            </div>
        </div>

        <!-- Chat Panel -->
        <div class="chat-panel">
            <div class="connection-id chat-header d-flex align-items-center">


                <button type="button" class="toggle-sidebar" id="toggleSidebar">
                    <i class="fas fa-bars xwhite small"></i>
                </button>
                <h6 class="mb-0 font-weight-bold">
                    <asp:Label ID="lblUsrname" runat="server"></asp:Label></h6>
                <%--  <div class="list-inline d-sm-flex mb-0 d-none">
                        <a href="javascript:;" class="list-inline-item d-flex align-items-center text-secondary"><i class="fas fa-circle text-success small mx-2 ms-0 "></i>Active Now</a>
                    </div>--%>
            </div>


            <div id="messagesPvt" class=" chat-content  ps--active-y table-container"></div>

            <div class="chat-input">
                <div class="row g-3">
                    <div class="col-md-6" hidden>
                        <input type="text" class="form-control" id="txtName" placeholder="Enter your name">
                    </div>
                    <div class="col-md-6" hidden>
                        <input type="text" class="form-control" id="txtFriendUniqueId" placeholder="Friend's ID">
                    </div>

                </div>
            </div>
            <div class="chat-footer  ">
                <div class="input-group">
                    <input class="form-control" id="txtMessagePvt" rows="1" placeholder="Type your message...">
                    <button type="button" class="btn btn-secondary" id="broadcastPvtMessage">
                        <i class="fas fa-paper-plane" style="transform: rotate(45deg);"></i>
                    </button>

                </div>

            </div>
        </div>
    </div>

    <!-- Email Verification Modal -->
    <asp:HiddenField runat="server" ID="hdnUniqSerId" ClientIDMode="Static" />
    <div class="modal-container">
        <asp:Button ID="btnpop" runat="server" Text="show" Style="display: none" />
        <asp:ModalPopupExtender ID="mp1" ClientIDMode="Static" runat="server"
            PopupControlID="Panel1" CancelControlID="btnclose"
            TargetControlID="btnpop" BackgroundCssClass="modalBackground">
        </asp:ModalPopupExtender>
        <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" align="center">
            <div class="card shadow border">
                <div class="card-header bg-primary">
                    <h5 class="card-title mb-0">Email Verification</h5>
                </div>
                <div class="card-body p-3">

                    <div class="row g-3">
                        <div class="col-12 text-start">
                            <asp:Label ID="lblEmail" runat="server" CssClass="form-label mb-3">
                                <asp:RequiredFieldValidator ID="rfvtxtLoginName" runat="server" ControlToValidate="txtOTP" ErrorMessage="*" ForeColor="Red" ValidationGroup="OTP"></asp:RequiredFieldValidator>
                            </asp:Label>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Enter your Email ID"></asp:TextBox>
                            <asp:TextBox Visible="false" ID="txtOTP" runat="server" CssClass="form-control" MaxLength="6"></asp:TextBox>
                            <asp:Label ID="msg" runat="server" CssClass="mt-2"></asp:Label>
                        </div>
                        <div class="col-12 text-end mt-3">
                            <asp:Button ID="btnSendOTP" runat="server" Text="Send OTP"
                                CssClass="btn btn-grd-info btn-sm" OnClick="btnSendOTP_Click" />
                            <asp:Button ID="btnVerify" Visible="false" runat="server" Text="Verify"
                                CssClass="btn btn-primary" OnClick="btnVerify_Click" ValidationGroup="OTP" />
                            <asp:Button ID="btnclose" runat="server" Text="Close"
                                CssClass="btn btn-grd-danger ms-2 btn-sm" />
                        </div>
                    </div>
                </div>

            </div>
        </asp:Panel>
    </div>

    <script src="Scripts/jquery.signalR-2.4.1.min.js"></script>
    <script src="signalr/hubs"></script>

    <script>
        $(function () {
            const chat = $.connection.letsChatHub;
            let mySDUID;
            let currentChatPartner = null;
            const unreadCounts = {};
            const onlineUsers = new Set();

            function initializeChat() {
                mySDUID = $('#hdnUniqSerId').val();
                if (mySDUID) {
                    chat.server.registerUser(mySDUID).done(() => {
                        chat.server.getUnreadMessageCounts(mySDUID);
                    });

                    $('#connId').text(mySDUID);
                    $('#txtName').val(mySDUID);

                    chat.server.getOnlineUsers().done(users => {
                        users.forEach(userId => updateUserStatus(userId, true));
                    });
                }
            }

            function updateUserStatus(userId, isOnline) {
                const userItem = $(`.user-item[data-userid="${userId}"]`);
                const statusIndicator = userItem.find('.status-indicator');

                if (isOnline) {
                    statusIndicator.removeClass('offline').addClass('online');
                    onlineUsers.add(userId);
                } else {
                    statusIndicator.removeClass('online').addClass('offline');
                    onlineUsers.delete(userId);
                }
            }

            function createMessageBubble(name, message, time, type) {
                return `
            <div class="message-group ${type}">
                <div class="message-bubble ">
                    <div class="message-text">${message}</div>
                    <div class="message-time text-end">${time}</div>
                </div>
            </div>`;
            }

            //function updateLastMessage(userId, message) {
            //    const lastMessageEl = $(`.user-item[data-userid="${userId}"] .last-message`);
            //    lastMessageEl.text(message);
            //}


            //chaged by parul start
            function updateLastMessage(userId, message) {
                const lastMessageEl = $(`.user-item[data-userid="${userId}"] .last-message`);
                lastMessageEl.text(message);
            }

            //chaged by parul end

            function scrollToBottom() {
                const messageContainer = document.getElementById('messagesPvt');
                messageContainer.scrollTop = messageContainer.scrollHeight;
            }

            function validateMessage(name, friendConnId, message) {
                if (!name) return alert('Please verify your email first!') && false;
                if (!friendConnId) return alert('Please select a user to chat with!') && false;
                if (!message) return alert('Please enter a message!') && false;
                if (friendConnId === mySDUID) return alert('Cannot send message to yourself!') && false;
                return true;
            }

            function sendMessage() {
                const name = $('#txtName').val().trim();
                const friendConnId = $('#txtFriendUniqueId').val().trim();
                const message = $('#txtMessagePvt').val().trim();

                if (!validateMessage(name, friendConnId, message)) return;

                chat.server.sendPrivateMessage({ Name: name, Message: message, FriendUniqueId: friendConnId });
                $('#txtMessagePvt').val('').focus();
            }

            // Client-side event handlers
            chat.client.updateUnreadMessageCounts = function (counts) {
                Object.entries(counts).forEach(([userId, count]) => {
                    unreadCounts[userId] = count;
                });
                updateUnreadCountDisplay();
            };

            chat.client.userStatusChanged = function (userId, isOnline) {
                updateUserStatus(userId, isOnline);
            };

            chat.client.loadMessageHistory = function (messages) {
                $('#messagesPvt').empty();
                messages.forEach(msg => {
                    const messageType = msg.SenderId === mySDUID ? 'sent' : 'received';
                    const messageHtml = createMessageBubble(
                        msg.Name,
                        msg.Message,
                        new Date(msg.Timestamp).toLocaleString(),
                        messageType
                    );
                    $('#messagesPvt').append(messageHtml);
                    updateLastMessage(
                        msg.SenderId === mySDUID ? msg.ReceiverId : msg.SenderId,
                        msg.Message
                    );
                });
                scrollToBottom();
            };

            chat.client.addNewPrivateMessageToPage = function (name, message, senderId, receiverId, timestamp) {
                const messageType = senderId === mySDUID ? 'sent' : 'received';
                const messageHtml = createMessageBubble(name, message, new Date(timestamp).toLocaleString(), messageType);

                const otherUser = senderId === mySDUID ? receiverId : senderId;
                updateLastMessage(otherUser, message);

                if (senderId !== mySDUID && currentChatPartner !== senderId) {
                    unreadCounts[senderId] = (unreadCounts[senderId] || 0) + 1;
                    updateUnreadCountDisplay();
                }

                if (currentChatPartner === otherUser) {
                    $('#messagesPvt').append(messageHtml);
                    scrollToBottom();
                    if (senderId !== mySDUID) {
                        chat.server.markMessagesAsRead(senderId, mySDUID);
                        unreadCounts[senderId] = 0;
                        updateUnreadCountDisplay();
                    }
                }
            };

            function updateUnreadCountDisplay() {
                $('.user-item').each(function () {
                    const userId = $(this).data('userid');
                    const unreadBadge = $(this).find('.unread-badge');
                    const count = unreadCounts[userId] || 0;

                    if (count > 0) {
                        unreadBadge.removeClass('d-none').text(count);
                    } else {
                        unreadBadge.addClass('d-none').text('0');
                    }
                });
            }

            // Window-level function for starting private chat
            window.startPrivateChat = function (friendConnId, userName) {
                currentChatPartner = friendConnId;
                $('#txtFriendUniqueId').val(friendConnId);
                $('.user-item').removeClass('active');
                $(event.currentTarget).addClass('active');
                $('#currentChatUser').text(userName);
                $('#txtName').val(mySDUID);
                var lblUsrname = document.getElementById('<%= lblUsrname.ClientID %>');
                if (lblUsrname) {
                    lblUsrname.innerText = userName; // Set the name
                }
                unreadCounts[friendConnId] = 0;
                updateUnreadCountDisplay();
                chat.server.getMessageHistory(friendConnId);
                chat.server.markMessagesAsRead(friendConnId, mySDUID);
                $('#txtMessagePvt').focus();
            };

            // Event listeners
            $('#broadcastPvtMessage').click(e => {
                e.preventDefault();
                sendMessage();
            });

            $('#txtMessagePvt').keypress(e => {
                if (e.which === 13 && !e.shiftKey) {
                    e.preventDefault();
                    sendMessage();
                }
            });

            // Start the connection
            $.connection.hub.start()
                .done(() => {
                    console.log('Connected to SignalR hub');
                    if ($('#hdnUniqSerId').val()) {
                        initializeChat();
                    }
                })
                .fail((error) => {
                    console.error('SignalR connection error:', error);
                });

            // Monitor for user ID changes
            new MutationObserver(mutations => {
                mutations.forEach(mutation => {
                    if (mutation.type === 'attributes' && mutation.attributeName === 'value') {
                        const newUserId = $('#hdnUniqSerId').val();
                        if (newUserId && newUserId !== mySDUID) {
                            mySDUID = newUserId;
                            initializeChat();
                        }
                    }
                });
            }).observe(document.getElementById('hdnUniqSerId'), { attributes: true });
        });
        document.addEventListener('DOMContentLoaded', function () {
            const searchInput = document.getElementById('searchInput');
            if (searchInput) {
                searchInput.addEventListener('input', filterPage);
                searchInput.addEventListener('keyup', filterPage);
            }
        });

        // Add window unload handler to ensure offline status
        window.addEventListener('beforeunload', function () {
            if ($.connection.hub && $.connection.hub.state === $.signalR.connectionState.connected) {
                $.connection.hub.stop();
            }
        });
        //search filter Start
        //function filterPage() {
        //    const query = document.getElementById('searchInput').value.toLowerCase();
        //    const items = document.querySelectorAll('.searchable');

        //    items.forEach(item => {
        //        const text = item.textContent.toLowerCase();
        //        if (text.includes(query)) {
        //            item.style.display = '';
        //        } else {
        //            item.style.display = 'none';
        //        }
        //    });

        //}
        function filterPage(inputValue) {
            const query = inputValue.toLowerCase().trim();
            const items = document.querySelectorAll('.searchable');
            items.forEach(item => {
                const text = item.textContent.toLowerCase();
                if (text.includes(query)) {
                    item.style.display = '';
                } else {
                    item.style.display = 'none';
                }
            });
        }


        //search filter end


        // Sidebar Toggle Functionality
        document.addEventListener('DOMContentLoaded', function () {
            const toggleBtn = document.getElementById('toggleSidebar');
            const sidebar = document.querySelector('.chat-sidebar');
            const overlay = document.getElementById('sidebarOverlay');

            function toggleSidebar() {
                sidebar.classList.toggle('show');
                overlay.classList.toggle('show');
            }

            toggleBtn.addEventListener('click', toggleSidebar);
            overlay.addEventListener('click', toggleSidebar);

            // Close sidebar when window is resized above mobile breakpoint
            window.addEventListener('resize', function () {
                if (window.innerWidth > 768) {
                    sidebar.classList.remove('show');
                    overlay.classList.remove('show');
                }
            });
        });
        //< !--Add this JavaScript code to your existing script section-- >
        document.addEventListener('DOMContentLoaded', function () {
            const closeBtn = document.querySelector('.close-sidebar');
            const sidebar = document.querySelector('.chat-sidebar');
            const overlay = document.getElementById('sidebarOverlay');

            if (closeBtn) {
                closeBtn.addEventListener('click', function () {
                    sidebar.classList.remove('show');
                    overlay.classList.remove('show');
                });
            }
        });
    </script>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="frmFlowChart.aspx.cs" Inherits="frmFlowChart" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        #flowchartCanvas {
            /*border: 1px solid black;*/
            border: var(--bs-border-width) solid var(--bs-border-color);
            width: 100%;
            height: 65vh !important;
            cursor: crosshair;
            display: block;
            margin: auto;
            background-color: white
        }

        #controls {
            margin-top: 2px;
        }

        #flowchartCanvas {
            width: 100% !important;
            height: 53vh !important;
        }

        .btn-group-vertical {
            position: fixed;
            right: 34px;
            bottom: 65px;
        }

        .btn-animated {
            position: relative;
            overflow: hidden;
        }

        .btn-group-vertical {
            display: flex;
            flex-direction: column;
            align-items: flex-end; /* Aligns buttons to the right */
            /*gap: 10px;*/
        }

        .btn-expandable {
            display: flex;
            align-items: center;
            justify-content: flex-start;
            background-color: var(--bs-body-bg-2); /* Light button color */
            border: none;
            padding: 4.4px 10px;
            width: 30px; /* Initial width */
            transition: width 0.3s ease; /* Smooth expansion */
            overflow: hidden;
            white-space: nowrap;
            color: white
        }

        [data-bs-theme=semi-dark] .btn-expandable {
            display: flex;
            align-items: center;
            justify-content: flex-start;
            background-color: #878787; /* Light button color */
            border: none;
            padding: 4.4px 10px;
            width: 30px; /* Initial width */
            transition: width 0.3s ease; /* Smooth expansion */
            overflow: hidden;
            white-space: nowrap;
            color: white;
        }

        [data-bs-theme=light] .btn-expandable {
            display: flex;
            align-items: center;
            justify-content: flex-start;
            background-color: #878787; /* Light button color */
            border: none;
            padding: 4.4px 10px;
            width: 30px; /* Initial width */
            transition: width 0.3s ease; /* Smooth expansion */
            overflow: hidden;
            white-space: nowrap;
            color: white;
        }

        [data-bs-theme=semi-dark] .btn-expandable > .fa-plus {
            color: white !important;
        }

        [data-bs-theme=light] .btn-expandable > .fa-plus {
            color: white !important;
        }

        .btn-expandable i {
            margin-right: 8px;
        }

        .btn-expandable .btn-label {
            /*opacity: 0;*/
            transition: opacity 0.3s ease; /* Smooth fade-in */
            padding-right: 10px; /* Space on the right for appearance */
        }

        .btn-expandable:hover {
            width: 110px; /* Expanded width */
            /*background-color: #e2e6ea;*/
            border-radius: 5px 0 0 5px;
        }

            .btn-expandable:hover .btn-label {
                opacity: 1; /* Show label on hover */
            }
        /* Curved border for the first button */
        .btn-group-vertical .btn-expandable:first-child {
            border-top-left-radius: 5px;
            border-top-right-radius: 5px;
        }

        /* Curved border for the last button */
        .btn-group-vertical .btn-expandable:last-child {
            border-bottom-left-radius: 5px;
            border-bottom-right-radius: 5px;
        }
    </style>
    <style>
        /*.eraser-cursor {
cursor: url('path/to/eraser-icon.png'), auto;*/ /* Path to your eraser icon */
        /*}*/
        /*    #flowchartCanvas {
        border: 1px solid black;
        width: 100%;
        height: 90vh;
        cursor: crosshair;
        display: block;
        margin: auto;
    }*/
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">

    <div class="card mb-1">
        <div class="card-body">
            <div class="btn-group">
                <asp:Button ID="btnAddFlow" runat="server" Text="Create Flow" CausesValidation="false" OnClick="btnAddFlow_Click" CssClass="btn btn-sm btn-outline-secondary" />
                <asp:Button ID="btnViewFlow" runat="server" Text="View Flow" CausesValidation="false" OnClick="btnViewFlow_Click" CssClass="btn btn-sm btn-outline-secondary" />
            </div>

            <div class="row gy-2 gx-3 mt-2 mb-2">
                <div class="col-md-4">
                    <label for="staticEmail" class="form-label">
                        Organization
                <asp:RequiredFieldValidator ID="rfvddlOrg" runat="server" ControlToValidate="ddlOrg" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                    </label>


                    <asp:DropDownList ID="ddlOrg" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field">
                    </asp:DropDownList>
                </div>
                <div class="col-md-4">
                    <label for="staticEmail" class="form-label">
                        Request Type
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlRequestType" InitialValue="0" ErrorMessage="*" Font-Bold="true" ForeColor="Red" ValidationGroup="btnSave"></asp:RequiredFieldValidator>
                    </label>

                    <asp:DropDownList ID="ddlRequestType" runat="server" CssClass="form-select form-select-sm single-select-optgroup-field"></asp:DropDownList>
                </div>
                <div class="col-md-1" runat="server" visible="false" id="divGO">
                    <div class="form-label opacity-0">flksjlk</div>
                    <asp:Button ID="btnGo" runat="server" Text="GO" OnClick="btnGo_Click" class="btn btn-grd-info btn-sm" />
                </div>
            </div>


            <asp:Panel runat="server" ID="pnlCreateFlow" Visible="false">
                <div id="controls">
                    <div class="row  g-1">
                        <div class="col-md-1">
                            <select id="textSize" class="form-select form-select-sm">
                                <option value="6">6px</option>
                                <option value="8">8px</option>
                                <option value="10">10px</option>
                                <option value="12">12px</option>
                                <option value="14">14px</option>
                                <option value="16">16px</option>
                                <option value="18">18px</option>
                                <option value="20">20px</option>
                                <option value="22">22px</option>
                                <option value="24">24px</option>
                                <option value="26">26px</option>
                                <option value="28">28px</option>
                                <option value="30">30px</option>
                            </select>
                        </div>
                        <div class="col-md-1">
                            <input type="color" id="textColor" value="#000000" class="w-100" />
                        </div>
                        <div class="col-md-1">
                            <select id="fontStyle" class="form-select-sm form-select">
                                <option value="Arial">Arial</option>
                                <option value="Courier New">Courier New</option>
                                <option value="Georgia">Georgia</option>
                                <option value="Times New Roman">Times New Roman</option>
                                <option value="Verdana">Verdana</option>
                            </select>
                            <select id="shapeSelector" class="form-select-sm d-none" onchange="changeShape()">
                                <option value="rectangle">Rectangle</option>
                                <option value="oval">Oval</option>
                            </select>
                        </div>
                        <div class="col-md-9 mb-1">
                            <input type="text" class="form-control  form-control-sm py-2" id="textInput" placeholder="Enter text for last shape" />
                        </div>
                    </div>


                    <div class="row">
                        <!-- Canvas Area -->
                        <div class="col-12">
                            <canvas id="flowchartCanvas" class="rounded"></canvas>
                        </div>

                        <!-- Button Group on the Right -->
                        <div class="col-2 d-flex justify-content-end">
                            <div class="btn-group-vertical rounded">
                                <button type="button" class="btn-expandable" onclick="addText()" title="Add Text">
                                    &nbsp; <span class="pr-3 "><b>T</b> </span>&nbsp; &nbsp;
        <span class="btn-label">Add Text</span>
                                </button>
                                <button type="button" class="btn-expandable" id="btnAddShape" onclick="startAddBox()" title="Add Box">
                                    <i class="fa-solid fa-plus pr-2"></i>
                                    <span class="btn-label">Add Box</span>
                                </button>
                                <button type="button" class="btn-expandable" onclick="clearCanvas()" title="Clear All">
                                    <i class="fa-solid fa-eraser"></i>
                                    <span class="btn-label">Clear All</span>
                                </button>
                                <button type="button" class="btn-expandable" onclick="enableArrowMode()" title="Draw Arrow">
                                    <i class="fa-solid fa-arrow-right pr-2"></i>
                                    <span class="btn-label">Draw Arrow</span>
                                </button>
                                <button type="button" class="btn-expandable" onclick="enableDeleteMode()" title="Delete">
                                    <i class="fa-solid fa-trash"></i>
                                    <span class="btn-label">Delete</span>
                                </button>
                                <button type="button" class="btn-expandable" onclick="enableDragMode()" title="Drag">
                                    &nbsp;   <i class="fa-solid fa-arrow-pointer"></i>
                                    <span class="btn-label">Drag</span>
                                </button>
                                <button type="button" class="btn-expandable" onclick="undo()" title="Undo">
                                    <i class="fa-solid fa-undo"></i>
                                    <span class="btn-label">Undo</span>
                                </button>
                                <button type="button" class="btn-expandable" onclick="redo()" title="Redo">
                                    <i class="fa-solid fa-redo"></i>
                                    <span class="btn-label">Redo</span>
                                </button>
                                <asp:HiddenField ID="HiddenField1" runat="server" />
                                <asp:HiddenField ID="HiddenField2" runat="server" />
                                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" Style="display: none;" ValidationGroup="btnSave" />

                                <button type="button" class="btn-expandable" onclick="saveFlowchart()" title="Save">
                                    <i class="fa-solid fa-download"></i>
                                    <span class="btn-label">Save</span>
                                </button>
                            </div>
                        </div>
                    </div>
                    <asp:HiddenField ID="hdnShapesData" runat="server" />
                    <asp:HiddenField ID="hdnImageData" runat="server" />
                </div>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlView">
                <div class="card mb-1">
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Image ID="imgFlow" runat="server" CssClass="img-fluid" />
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>
    <script type="text/javascript">
        var canvas, ctx;
        var isDrawing = false;
        var isDeleting = false;
        var isDrawingArrow = false;
        var isDragging = false;
        var isAddingBox = false; // Flag to indicate if adding a box
        var startX, startY;
        var boxes = [];
        var arrows = [];
        var currentBox = null;
        var arrowStart = null;
        var selectedBox = null;
        var selectedArrow = null;
        var dragStart = null;

        var undoStack = [];
        var redoStack = [];
        var isUndoing = false; // Flag to prevent redundant Undo actions
        var isRedoing = false; // Flag to prevent redundant Redo actions
        var currentShape = 'rectangle';

        function changeShape() {
            currentShape = document.getElementById('shapeSelector').value;
        }
        function drawShape(ctx, x, y, width, height) {
            console.log("Drawing shape:", currentShape);
            if (currentShape === 'oval') {
                drawOval(ctx, x, y, width, height);
            } else {
                ctx.strokeRect(x, y, width, height);
            }
        }
        function drawOval(ctx, x, y, width, height) {
            ctx.beginPath();
            ctx.moveTo(x, y + height / 2);
            ctx.bezierCurveTo(x, y, x + width, y, x + width, y + height / 2);
            ctx.bezierCurveTo(x + width, y + height, x, y + height, x, y + height / 2);
            ctx.closePath();
            ctx.stroke();
        }

        function getMousePos(canvas, evt) {
            var rect = canvas.getBoundingClientRect();
            return {
                x: evt.clientX - rect.left,
                y: evt.clientY - rect.top
            };
        }

        function resizeCanvas() {
            canvas.width = canvas.offsetWidth;
            canvas.height = canvas.offsetHeight;
            redrawCanvas();
        }

        function drawArrow(ctx, fromx, fromy, tox, toy) {
            var headlen = 10; // length of head in pixels
            var dx = tox - fromx;
            var dy = toy - fromy;
            var angle = Math.atan2(dy, dx);
            ctx.beginPath();
            ctx.moveTo(fromx, fromy);
            ctx.lineTo(tox, toy);
            ctx.lineTo(tox - headlen * Math.cos(angle - Math.PI / 6), toy - headlen * Math.sin(angle - Math.PI / 6));
            ctx.moveTo(tox, toy);
            ctx.lineTo(tox - headlen * Math.cos(angle + Math.PI / 6), toy - headlen * Math.sin(angle + Math.PI / 6));
            ctx.stroke();
        }

        function drawWrappedText(ctx, text, x, y, maxWidth, maxHeight, fontSize, fontStyle, color) {
            ctx.font = fontSize + ' ' + fontStyle;
            ctx.fillStyle = color;

            var words = text.split(' ');
            var line = '';
            var lineHeight = parseInt(fontSize) * 1.2; // Space between lines
            var yOffset = y + lineHeight;

            var lines = [];

            for (var n = 0; n < words.length; n++) {
                var testLine = line + words[n] + ' ';
                var metrics = ctx.measureText(testLine);
                var testWidth = metrics.width;

                if (testWidth > maxWidth) {
                    lines.push(line);
                    line = words[n] + ' ';
                    yOffset += lineHeight;
                    if (yOffset > y + maxHeight) {
                        break;
                    }
                } else {
                    line = testLine;
                }
            }
            lines.push(line);
            var totalTextHeight = lines.length * lineHeight;
            var startY = y + (maxHeight - totalTextHeight) / 2 + lineHeight;

            for (var i = 0; i < lines.length; i++) {
                ctx.fillText(lines[i], x + 5, startY + (i * lineHeight));
            }
        }

        function redrawCanvas() {
            ctx.clearRect(0, 0, canvas.width, canvas.height);
            ctx.fillStyle = 'white';
            ctx.fillRect(0, 0, canvas.width, canvas.height);

            boxes.forEach(function (box) {
                drawShape(ctx, box.x, box.y, box.width, box.height);
                if (box.text) {
                    var maxWidth = box.width - 10;
                    var maxHeight = box.height - 10;
                    drawWrappedText(ctx, box.text, box.x, box.y, maxWidth, maxHeight, box.fontSize, box.fontStyle, box.color);
                }
            });

            arrows.forEach(function (arrow) {
                drawArrow(ctx, arrow.from.x, arrow.from.y, arrow.to.x, arrow.to.y);
            });
        }
        window.onload = function () {
            console.log("Window loaded");
            canvas = document.getElementById('flowchartCanvas');
            ctx = canvas.getContext('2d');

            window.addEventListener('resize', resizeCanvas);
            resizeCanvas();

            canvas.addEventListener('mousedown', function (e) {
                console.log("Mouse down event triggered");
                var pos = getMousePos(canvas, e);
                var mouseX = pos.x;
                var mouseY = pos.y;

                console.log("Mouse position:", mouseX, mouseY);
                console.log("isAddingBox:", isAddingBox);

                saveState(); // Save state before any drawing

                if (isDeleting) {
                    handleDelete(mouseX, mouseY);
                } else if (isDrawingArrow) {
                    if (!arrowStart) {
                        arrowStart = { x: mouseX, y: mouseY };
                    } else {
                        arrows.push({ from: arrowStart, to: { x: mouseX, y: mouseY } });
                        arrowStart = null;
                        redrawCanvas();
                    }
                } else if (isDragging) {
                    selectedBox = boxes.find(box => mouseX >= box.x && mouseX <= box.x + box.width && mouseY >= box.y && mouseY <= box.y + box.height);
                    selectedArrow = arrows.find(arrow => isPointOnLine(mouseX, mouseY, arrow.from.x, arrow.from.y, arrow.to.x, arrow.to.y));

                    if (selectedBox) {
                        dragStart = { x: mouseX - selectedBox.x, y: mouseY - selectedBox.y };
                    } else if (selectedArrow) {
                        dragStart = { x: mouseX - selectedArrow.from.x, y: mouseY - selectedArrow.from.y };
                    }
                } else {
                    console.log("Starting to draw");
                    startX = mouseX;
                    startY = mouseY;
                    isDrawing = true;
                }
            });

            canvas.addEventListener('mousemove', function (e) {
                var pos = getMousePos(canvas, e);
                var mouseX = pos.x;
                var mouseY = pos.y;

                if (isDrawing || isDragging || arrowStart) {
                    console.log("Mouse move while drawing/dragging");
                    if (isDrawing) {
                        console.log("Drawing in progress");
                        redrawCanvas();
                        var width = mouseX - startX;
                        var height = mouseY - startY;
                        drawShape(ctx, startX, startY, width, height);
                        currentBox = { x: startX, y: startY, width: width, height: height, text: "", shape: currentShape };
                    } else if (arrowStart) {
                        redrawCanvas();
                        drawArrow(ctx, arrowStart.x, arrowStart.y, mouseX, mouseY);
                    } else if (isDragging) {
                        if (selectedBox) {
                            selectedBox.x = mouseX - dragStart.x;
                            selectedBox.y = mouseY - dragStart.y;
                            redrawCanvas();
                        } else if (selectedArrow) {
                            var dx = mouseX - dragStart.x - selectedArrow.from.x;
                            var dy = mouseY - dragStart.y - selectedArrow.from.y;
                            selectedArrow.from.x += dx;
                            selectedArrow.from.y += dy;
                            selectedArrow.to.x += dx;
                            selectedArrow.to.y += dy;
                            dragStart = { x: mouseX - selectedArrow.from.x, y: mouseY - selectedArrow.from.y };
                            redrawCanvas();
                        }
                    }
                }
            });

            canvas.addEventListener('mouseup', function (e) {
                var pos = getMousePos(canvas, e);
                var mouseX = pos.x;
                var mouseY = pos.y;

                if (isDrawing) {
                    isDrawing = false;
                    boxes.push(currentBox);
                    currentBox = null;
                    redrawCanvas();
                } else if (arrowStart) {
                    arrows.push({ from: arrowStart, to: { x: mouseX, y: mouseY } });
                    arrowStart = null;
                    redrawCanvas();
                } else if (isDragging) {
                    isDragging = false;
                    selectedBox = null;
                    selectedArrow = null;
                }
            });

            canvas.addEventListener('click', function (e) {
                var pos = getMousePos(canvas, e);
                var mouseX = pos.x;
                var mouseY = pos.y;

                if (isDeleting) {
                    handleDelete(mouseX, mouseY);
                }
            });
        };



        function saveState() {
            // Save state only if not undoing or redoing
            if (!isUndoing && !isRedoing) {
                undoStack.push({
                    boxes: JSON.parse(JSON.stringify(boxes)),
                    arrows: JSON.parse(JSON.stringify(arrows))
                });
                redoStack = [];
            }
        }

        function undo() {
            if (undoStack.length > 0) {
                isUndoing = true;
                redoStack.push({
                    boxes: JSON.parse(JSON.stringify(boxes)),
                    arrows: JSON.parse(JSON.stringify(arrows))
                });
                var state = undoStack.pop();
                boxes = state.boxes;
                arrows = state.arrows;
                redrawCanvas();
                isUndoing = false;
            }
        }

        function redo() {
            if (redoStack.length > 0) {
                isRedoing = true;
                undoStack.push({
                    boxes: JSON.parse(JSON.stringify(boxes)),
                    arrows: JSON.parse(JSON.stringify(arrows))
                });
                var state = redoStack.pop();
                boxes = state.boxes;
                arrows = state.arrows;
                redrawCanvas();
                isRedoing = false;
            }
        }

        window.onload = function () {
            canvas = document.getElementById('flowchartCanvas');
            ctx = canvas.getContext('2d');

            window.addEventListener('resize', resizeCanvas);
            resizeCanvas();

            canvas.addEventListener('mousedown', function (e) {
                var pos = getMousePos(canvas, e);
                var mouseX = pos.x;
                var mouseY = pos.y;

                saveState(); // Save state before any drawing

                if (isDeleting) {
                    handleDelete(mouseX, mouseY);
                } else if (isDrawingArrow) {
                    if (!arrowStart) {
                        arrowStart = { x: mouseX, y: mouseY };
                    } else {
                        arrows.push({ from: arrowStart, to: { x: mouseX, y: mouseY } });
                        arrowStart = null;
                        redrawCanvas();
                    }
                } else if (isDragging) {
                    selectedBox = boxes.find(box => mouseX >= box.x && mouseX <= box.x + box.width && mouseY >= box.y && mouseY <= box.y + box.height);
                    selectedArrow = arrows.find(arrow => isPointOnLine(mouseX, mouseY, arrow.from.x, arrow.from.y, arrow.to.x, arrow.to.y));

                    if (selectedBox) {
                        dragStart = { x: mouseX - selectedBox.x, y: mouseY - selectedBox.y };
                    } else if (selectedArrow) {
                        dragStart = { x: mouseX - selectedArrow.from.x, y: mouseY - selectedArrow.from.y };
                    }
                } else if (isAddingBox) {
                    // Start drawing a box
                    startX = mouseX;
                    startY = mouseY;
                    isDrawing = true;
                    isAddingBox = false; // Reset flag after starting
                } else {
                    startX = mouseX;
                    startY = mouseY;
                    isDrawing = true;
                }
            });

            canvas.addEventListener('mousemove', function (e) {
                var pos = getMousePos(canvas, e);
                var mouseX = pos.x;
                var mouseY = pos.y;

                if (isDrawing || isDragging || arrowStart) {
                    if (isDrawing) {
                        redrawCanvas();
                        var width = mouseX - startX;
                        var height = mouseY - startY;
                        ctx.strokeRect(startX, startY, width, height);
                        currentBox = { x: startX, y: startY, width: width, height: height, text: "" };
                    } else if (arrowStart) {
                        redrawCanvas();
                        drawArrow(ctx, arrowStart.x, arrowStart.y, mouseX, mouseY);
                    } else if (isDragging) {
                        if (selectedBox) {
                            selectedBox.x = mouseX - dragStart.x;
                            selectedBox.y = mouseY - dragStart.y;
                            redrawCanvas();
                        } else if (selectedArrow) {
                            var dx = mouseX - dragStart.x - selectedArrow.from.x;
                            var dy = mouseY - dragStart.y - selectedArrow.from.y;
                            selectedArrow.from.x += dx;
                            selectedArrow.from.y += dy;
                            selectedArrow.to.x += dx;
                            selectedArrow.to.y += dy;
                            dragStart = { x: mouseX - selectedArrow.from.x, y: mouseY - selectedArrow.from.y };
                            redrawCanvas();
                        }
                    }
                }
            });

            canvas.addEventListener('mouseup', function (e) {
                var pos = getMousePos(canvas, e);
                var mouseX = pos.x;
                var mouseY = pos.y;

                if (isDrawing) {
                    isDrawing = false;
                    boxes.push(currentBox);
                    currentBox = null;
                    redrawCanvas();
                } else if (arrowStart) {
                    arrows.push({ from: arrowStart, to: { x: mouseX, y: mouseY } });
                    arrowStart = null;
                    redrawCanvas();
                } else if (isDragging) {
                    isDragging = false;
                    selectedBox = null;
                    selectedArrow = null;
                }
            });

            canvas.addEventListener('click', function (e) {
                var pos = getMousePos(canvas, e);
                var mouseX = pos.x;
                var mouseY = pos.y;

                if (isDeleting) {
                    handleDelete(mouseX, mouseY);
                }
            });
        };

        function startAddBox() {
            isAddingBox = true;
            isDrawingArrow = false;
            isDeleting = false;
            isDragging = false;
            arrowStart = null;
        }

        function addText() {
            var text = document.getElementById('textInput').value;
            var textSize = document.getElementById('textSize').value + 'px'; // Ensure 'px' is added
            var textColor = document.getElementById('textColor').value;
            var fontStyle = document.getElementById('fontStyle').value;

            if (boxes.length > 0) {
                saveState(); // Save state before adding text
                var lastBox = boxes[boxes.length - 1];
                lastBox.text = text || lastBox.text;
                lastBox.fontSize = textSize; // Update text size
                lastBox.fontStyle = fontStyle; // Update font style
                lastBox.color = textColor; // Update text color
                redrawCanvas();
            }
        }

        function clearCanvas() {
            saveState(); // Save state before clearing
            boxes = [];
            arrows = [];
            ctx.clearRect(0, 0, canvas.width, canvas.height);
        }

        function handleDelete(x, y) {
            saveState(); // Save state before deleting
            boxes = boxes.filter(box => !(x >= box.x && x <= box.x + box.width && y >= box.y && y <= box.y + box.height));
            arrows = arrows.filter(arrow => !isPointOnLine(x, y, arrow.from.x, arrow.from.y, arrow.to.x, arrow.to.y));
            redrawCanvas();
        }

        function enableArrowMode() {
            isDrawingArrow = true;
            isDeleting = false;
            isDrawing = false;
            isDragging = false;
            arrowStart = null;
            updateCursorStyle();
        }

        function enableDeleteMode() {
            isDeleting = true;
            isDrawingArrow = false;
            isDrawing = false;
            isDragging = false;
            arrowStart = null;
            updateCursorStyle();
        }

        function enableDragMode() {
            isDragging = true;
            isDeleting = false;
            isDrawingArrow = false;
            isDrawing = false;
            arrowStart = null;
            updateCursorStyle();
        }

        function isPointOnLine(px, py, x1, y1, x2, y2) {
            var dx = x2 - x1;
            var dy = y2 - y1;
            var length = Math.sqrt(dx * dx + dy * dy);
            var dist = Math.abs(dx * (y1 - py) - dy * (x1 - px)) / length;
            return dist < 5; // threshold for line width
        }

        //function saveFlowchart() {
        //    var dataURL = canvas.toDataURL('image/jpeg');
        //    document.getElementById('hdnImageData').value = dataURL;
        //    document.getElementById('btnSave').click(); // Trigger server-side save
        //}
        //function saveFlowchart() {
        //    var dataURL = canvas.toDataURL('image/jpeg');
        //    var shapesData = JSON.stringify(boxes); // Serialize box data to JSON
        //    document.getElementById('hdnImageData').value = dataURL;
        //    document.getElementById('hdnShapesData').value = shapesData; // Set shapes data to hidden field
        //    document.getElementById('btnSave').click(); // Trigger server-side save
        //}

        function saveFlowchart() {
            document.getElementById('<%= hdnShapesData.ClientID %>').value = JSON.stringify(boxes);
            document.getElementById('<%= hdnImageData.ClientID %>').value = canvas.toDataURL();
            document.getElementById('<%= btnSave.ClientID %>').click();
        }


        function updateCursorStyle() {
            if (isDeleting) {
                canvas.style.cursor = 'url(path/to/eraser-icon.png), auto'; // Path to your eraser icon
            } else if (isDrawingArrow) {
                canvas.style.cursor = 'crosshair';
            } else if (isDragging) {
                canvas.style.cursor = 'move';
            } else {
                canvas.style.cursor = 'default';
            }
        }

    </script>
</asp:Content>


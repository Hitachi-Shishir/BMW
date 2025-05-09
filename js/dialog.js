﻿function CustomAlert() {
    this.render = function (dialog) {
        var winW = window.innerWidth;
        var winH = window.innerHeight;
        var dialogoverlay = document.getElementById('dialogoverlay');
        var dialogbox = document.getElementById('dialogbox');
        dialogoverlay.style.display = "block";
        dialogoverlay.style.height = winH + "px";
        dialogbox.style.left = (winW / 2) - (550 * .5) + "px";
        dialogbox.style.top = "100px";
        dialogbox.style.display = "block";
        document.getElementById('dialogboxhead').innerHTML = "Acknowledge This Message";
        document.getElementById('dialogboxbody').innerHTML = dialog;
        document.getElementById('dialogboxfoot').innerHTML = '<button onclick="Alert.ok()">OK</button>';
    }
    this.ok = function () {
        document.getElementById('dialogbox').style.display = "none";
        document.getElementById('dialogoverlay').style.display = "none";
    }
}
var Alert = new CustomAlert();

function deletePost(id) {
    var db_id = id.replace("post_", "");
    // Run Ajax request here to delete post from database
    document.body.removeChild(document.getElementById(id));
}
function CustomConfirm() {
    this.render = function (dialog, op) {
        var winW = window.innerWidth;
        var winH = window.innerHeight;
        var dialogoverlay = document.getElementById('dialogoverlay');
        var dialogbox = document.getElementById('dialogbox');
        dialogoverlay.style.display = "block";
        dialogoverlay.style.height = winH + "px";
        dialogbox.style.left = (winW / 2) - (550 * .5) + "px";
        dialogbox.style.top = "100px";
        dialogbox.style.display = "block";

        document.getElementById('dialogboxhead').innerHTML = "Confirm that action";
        document.getElementById('dialogboxbody').innerHTML = dialog;
        document.getElementById('dialogboxfoot').innerHTML = '<button onclick="Confirm.yes(\'' + dialog +'\')">Yes</button> <button onclick="Confirm.no()">No</button>';
    }
    this.no = function () {
        document.getElementById('dialogbox').style.display = "none";
        document.getElementById('dialogoverlay').style.display = "none";
        return false;
    }
    this.yes = function (dialog) {
        document.getElementById('dialogbox').style.display = "none";
        document.getElementById('dialogoverlay').style.display = "none";
        if (Confirm(dialog)) {
            return true;
        } else return false;
    }
}
var Confirm = new CustomConfirm();
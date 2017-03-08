"use strict";

/// WebAPIでログイン情報を認証する関数
function loginHttp() {
    $.ajax({
        url: "http://www.ekidata.jp/api/p/23.json",
        type: "get",
        contentType: "application/json",
        dataType: "text",
        data: {},
        success: function (result, status) {
            if (status == "success") {
                document.getElementById("errorMsg").textContent = result;
            }
        },
        error: function (error) {
            document.getElementById("errorMsg").textContent = error;
        }
    });

    //var obj = new XMLHttpRequest();
    //obj.open("GET", "   .php", true);
    //obj.onreadystatechange = function () {
    //    document.getElementById("errorMsg").textContent = obj.status;
    //    if (obj.readyState == 4) {
    //        if (obj.status == 200 || obj.status == 0) {
    //            document.getElementById("errorMsg").textContent = obj.responseText;
    //        }
    //        //obj.status == 500（現状）
    //    }
    //};
    //obj.send(null);
    return 1;
}

///
function login() {
    ///情報取得
    var hwId = document.getElementById("hardwareId").value;
    var userId = document.getElementById("inputUser").value;
    var pass = document.getElementById("inputPass").value;

    ///ログイン情報未入力
    if ("" == userId) {
        document.getElementById("errorMsg").textContent = "Please enter uesrID";
        return;
    }

    if ("" == pass) {
        document.getElementById("errorMsg").textContent = "Please enter password";
        return;
    }
   
    ///ログイン情報をサーバに渡して、認証する
    //var status = 0;
    //status = loginHttp();
    //if (status == 0) {
    //    document.getElementById("errorMsg").textContent = "Failed to login.";
    //    return;
    //}

    ///hardwareID不正
    if ("hardwareid" == pass) {
        document.getElementById("errorMsg").textContent = "This tablet is not a valid device.";
        return;
    }

    ///ログイン情報不正
    if ("logininfo" == pass) {
        document.getElementById("errorMsg").textContent = "Please try again after recheck your uesrID and password.";
        return;
    }

    /// ログイン成功
    window.location.href = "todo.html";
}

/// .Net処理を呼び出し、HardwareIdを取得する関数
/// 画面起動時に呼ばれる（body onload）
function getHardwareId() {
    window.external.notify("getHardwareId");
}

/// .Net処理からの戻り値を受け取る関数
function setHardwareId(hardwareId) {
    document.getElementById("hardwareId").setAttribute("value", hardwareId);
}

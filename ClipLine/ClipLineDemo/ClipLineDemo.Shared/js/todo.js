//// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
//// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
//// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
//// PARTICULAR PURPOSE.
////
//// Copyright (c) Microsoft Corporation. All rights reserved
"use strict";

/// .Net処理を呼び出し、カメラを起動する関数
/// 画面起動時に呼ばれる（body onload）
function captureVideo() {
    window.external.notify("captureVideo");
}

/// .Net処理を呼び出し、ファイルを選択する関数
/// 画面起動時に呼ばれる（body onload）
function pickVideo() {
    window.external.notify("pickVideo");
}

/// .Net処理からの戻り値を受け取る関数
function setVideo(videoPath) {
    //var videoBlobUrl = URL.createObjectURL(file, { oneTimeOnly: true });
    document.getElementById("capturedVideo").src = "http://localhost:8080/ClipLine/oceans.mp4";
    document.getElementById("resetButton").style.visibility = "visible";
    document.getElementById("debugmsg").textContent = document.getElementById("capturedVideo").src;
}

/// 画面に表示している動画をクリアする
function reset() {
    document.getElementById("capturedVideo").src = "";
    document.getElementById("resetButton").style.visibility = "hidden";
}


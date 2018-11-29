var webSocketVideoFrame;
var frameTime;
var videoFrameElement = document.querySelector("#videoFrame");
var lastImageUrl;

function GetVideoFrames() {
    if (webSocketVideoFrame === undefined) {
        var wsUri = "ws://localhost:8080/videoframe";
        webSocketVideoFrame = new WebSocket(wsUri);
        webSocketVideoFrame.binaryType = "arraybuffer";

        webSocketVideoFrame.onopen = function() {
            webSocketHelper.waitUntilWebsocketReady(function() {
                webSocketVideoFrame.send("VideoFrame");
            }, webSocketVideoFrame, 0);
        };

        webSocketVideoFrame.onmessage = function() {

            var bytearray = new Uint8Array(event.data);

            var blob = new Blob([event.data], { type: "image/jpeg" });
            lastImageUrl = createObjectURL(blob);
            videoFrameElement.src = lastImageUrl;

            frameTime = new Date().getTime();
        };
    } else {
        webSocketHelper.waitUntilWebsocketReady(function() {
            webSocketVideoFrame.send("VideoFrame");
        }, webSocketVideoFrame, 0);
    }
}


function createObjectURL(blob) {
    var URL = window.URL || window.webkitURL;
    if (URL && URL.createObjectURL) {
        return URL.createObjectURL(blob);
    } else {
        return null;
    }
}
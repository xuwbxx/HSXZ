var preWindowIndex = 0;
var indexcodes = [];
var pictype = "jpeg";
var picpath = "d:\Users\Public\Pictures\\";
var videosize = "256";
var videopath = "d:\Users\Public\Videos\\";
var decode = "5";
var PTZ_CMD_ZOOM_IN = 11		/* 焦距以速度SS变大(倍率变大) */
var PTZ_CMD_ZOOM_OUT = 12		/* 焦距以速度SS变小(倍率变小) */
var PTZ_CMD_FOCUS_NEAR = 13		/* 焦点以速度SS前调*/
var PTZ_CMD_FOCUS_FAR = 14		/* 焦点以速度SS后调*/
var PTZ_CMD_IRIS_OPEN = 15		/* 光圈以速度SS扩大*/
var PTZ_CMD_IRIS_CLOSE = 16		/* 光圈以速度SS缩小*/
var PTZ_CMD_TILT_UP = 21		/* 云台以SS的速度上仰*/
var PTZ_CMD_TILT_DOWN = 22		/* 云台以SS的速度下俯*/
var PTZ_CMD_PAN_LEFT = 23		/* 云台以SS的速度左转*/
var PTZ_CMD_PAN_RIGHT = 24		/* 云台以SS的速度右转*/
var PTZ_CMD_UP_LEFT = 25		/* 云台以SS的速度上仰和左转*/
var PTZ_CMD_UP_RIGHT = 26		/* 云台以SS的速度上仰和右转*/
var PTZ_CMD_DOWN_LEFT = 27		/* 云台以SS的速度下俯和左转*/
var PTZ_CMD_DOWN_RIGHT = 28		/* 云台以SS的速度下俯和右转*/
var PTZ_CMD_PAN_AUTO = 29		/* 云台以SS的速度左右自动扫描*/
var PRESET_CMD_SET_PRESET = 8		/* 设置预置点*/
var PRESET_CMD_CLE_PRESET = 9		/* 清除预置点*/
var PRESET_CMD_GOTO_PRESET = 39	/* 转到预置点*/
var PTZ_CMD_START = 0; 	/* 开始命令*/
var PTZ_CMD_STOP = 1; 		/* 结束命令*/
var SUCCESS = 0; /* 控件操作成功*/
var FAILED = 1; /* 控件操作失败*/
var ERROR = -1; /* 控件操作错误*/
var ISPREVIEW = 1; /* 正在预览*/
var NOTPREVIEW = 0; /* 没在预览*/
var OPRATION_PTZ = 10004;
var OPRATION_CUTVIDEO = 10003;
var OPRATION_VIDEOPARAM = 10002;

//var pid=<%=this.UserInfo.CurProjectId %>;

var preview = {
    divId: "preview_1"
}

var ocx = new PreviewOCX(preview);

//token设置事件通知
function tokenCallEvent(reqID) {
    $.ajax({
        url: "/Video/Monitor/VideoTokenGet",
        type: "post",
        dataType: "text",
        success: function (token) {
            ocx.setToken(reqID, token);
        },
        data: { "ca": "1" },
        error: function () { alert("error"); }
    });
}
//窗口交换改变事件通知
function windowExchangeEvent(index) {
    //alert("窗口交换改变，当前窗口索引为：" + index);
}
function windowInitOK() {
    //  alert(" 控件初始化完毕");
}
//窗口选择改变事件通知
function windowChangeEvent(index) {
    var windIndex = ""; //showptz();
    // alert("窗口选中改变，当前窗口索引为：" + windIndex);
}
//开始预览事件通知
function startPreviewEvent() {
    var windIndex = ""; //showptz();
    // alert("窗口" + windIndex + "：开始预览");
}
//停止预览事件通知
function stopPreviewEvent() {
    var windIndex = ocx.getSelectWindow();
    //   alert("窗口" + windIndex + "：停止预览");
}
//抓图完成事件通知
function picCutEvent(param) {
    var windIndex = ocx.getSelectWindow();
    if (param == SUCCESS) {
        alert("窗口" + windIndex + "：抓图成功");
    } else {
        alert("窗口" + windIndex + "：抓图失败");
    }
}
//录像完成事件通知
function videoCutEvent(param) {
    var windIndex = ocx.getSelectWindow();
    if (param == SUCCESS) {
        alert("窗口" + windIndex + "：录像成功");
    } else {
        alert("窗口" + windIndex + "：录像失败");
    }

}
//选择窗口布局
function selectWindowType() {
    var layoutType = Number($("#windowTypeSelect").val());
    if (ocx.setWindowsLayout(layoutType) != SUCCESS) {
        jAlert('窗口布局设置失败', '窗口布局', 'error');
    } else {
        alert("窗口布局设置成功");
    }
}

//设置token回调
ocx.registerCallback(PREVIEW_EVENT_TOKEN, tokenCallEvent);
//抓图回调函数
ocx.registerCallback(PREVIEW_EVENT_SNAP, picCutEvent);
//录像回调
ocx.registerCallback(PREVIEW_EVENT_VEDIO, videoCutEvent);
//开始预览回调
ocx.registerCallback(PREVIEW_EVENT_START, startPreviewEvent);
//停止预览回调
ocx.registerCallback(PREVIEW_EVENT_STOP, stopPreviewEvent);
//窗口选中改变回调
ocx.registerCallback(PREVIEW_EVENT_SELECTED, windowChangeEvent);
// 窗口交换改变
ocx.registerCallback(PREVIEW_EVENT_CHANGE, windowExchangeEvent);
// 控件初始化完毕消息
ocx.registerCallback(PREVIEW_EVENT_INIT_OK, windowInitOK);
ocx.setWindowsLayout(1);


function getwindow() {
    alert(ocx.getSelectWindow());
}

//记录窗口之前选中的index
function remember(id) {
    preWindowIndex = $("#" + id).val();
}

function PlayCamera(code) {
    var winindex = ocx.getSelectWindow();
    jQuery.ajax({
        url: "/Video/Monitor/VideoTokenGet",
        type: "POST",
        dataType: "text",
        success: function (data) {
            if (ocx.startPreview(winindex, data) == 0) {
                if (ocx.setSelectWindow(winindex + 1) == -1) {
                    ocx.setSelectWindow(0);
                }
            }
        },
        data: { "ca": "0", "code": code },
        error: function () { alert("error"); }
    });
}

//单窗口预览停止
function stopOne() {
    var index = ocx.getSelectWindow();
    if (ocx.stopPreview(index) != SUCCESS) {
        alert("窗口" + index + ":停止预览失败");
    } else {
        //showptz();
    }
}
//全窗口预览停止
function stopAll() {
    if (ocx.stopPreview(-1) != SUCCESS) {
        alert("全窗口停止预览失败");
    } else {
        //showptz();
    }
}

function getLayerOut(id) {
    ocx.setWindowsLayout(id);
}

//云台移动，变焦
function ptzControllerMove(command, action) {
    var xml = [];
    xml.push("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
    xml.push("<PtzControl>");
    xml.push("<Action>" + action + "</Action>");
    xml.push("<Priority>50</Priority>");
    xml.push("<Speed>5</Speed>");
    //xml.push("<Preset Index =1 DwellTime=50 Speed=5 />");
    xml.push("</PtzControl>");
    if (ocx.ptzControl(command, xml.join("")) == SUCCESS) {
        alert("云台操作：" + command + ",操作：" + action + "成功");
    } else {
        alert("云台操作：" + command + ",操作：" + action + "失败");
    }
}

//关闭船体，释放资源
var VideoDispose = function () {

    ocx.getSelectWindow();
    EventNotify(PREVIEW_EVENT_STOP, 0, '');
}

window.onload = function () { ocx.setWindowsLayout(1); }
/*
* 描述:    使用脚本打开模式对话框
* 参数:    新窗体的url
*/
function openChildDialog(title, url, width, height) {

    var titleheight = "22px";
    var bordercolor = "#666699";
    var titlecolor = "#FFFFFF";
    var titlebgcolor = "#666699";
    var bgcolor = "#FFFFFF";

    var w = width;
    var h = height;

    var iWidth = document.documentElement.clientWidth;
    var iHeight = document.documentElement.clientHeight;

    var scrollHeight = document.documentElement.scrollHeight;

    var bgObj = document.createElement("div"); //创建一个div对象（背景层）
    bgObj.id = "bgObj";
    bgObj.style.position = "absolute";
    bgObj.style.top = "0";
    bgObj.style.background = "#777";
    bgObj.style.filter = "progid:DXImageTransform.Microsoft.Alpha(style=3,opacity=25,finishOpacity=75)";
    bgObj.style.opacity = "0.6";
    bgObj.style.left = "0";
    bgObj.style.width = iWidth + "px";
    bgObj.style.height = Math.max(document.body.clientHeight, scrollHeight) + "px";
    bgObj.style.zIndex = "10000";
    document.body.appendChild(bgObj);


    var msgObj = document.createElement("div");
    msgObj.id = "msgObj";
    msgObj.style.cssText = "position:absolute;font:11px '宋体';top:" + Math.abs(iHeight - h) / 2 + "px;left:" + Math.abs(iWidth - w) / 2 + "px;width:" + w + "px;height:" + h + "px;text-align:center;border:5px solid " + bordercolor + ";background-color:" + bgcolor + ";padding:1px;line-height:22px;z-index:10001;";
    document.body.appendChild(msgObj);

    var table = document.createElement("table");
    table.style.cssText = "margin:0px;border:0px;padding:0px;width:100%";
    table.cellSpacing = 0;
    msgObj.appendChild(table);

    var tr = table.insertRow(-1); ;

    var titleBar = tr.insertCell(-1);
    titleBar.style.cssText = "width:100%;height:26px;background-position: center; background-image: url('../../images/dialog/topxian.gif'); background-repeat: repeat-x;cursor:move;";
    titleBar.innerHTML = "<span style='float:left;color:#FFF; font-weight:bold; line-height:26px; font-size:12px;'>&nbsp;" + title + "</span><span style='float:right'><a href='javascript:'><img border='0' src='../../images/dialog/close_1.gif' width='26px' height='20px' id='diagclose' title='关闭' /></a></span>";


    //关闭按钮事件
    document.getElementById("diagclose").onmousemove = function () {
        document.getElementById("diagclose").src = "../../images/dialog/close_2.gif";
    };

    document.getElementById("diagclose").onmouseout = function () {
        document.getElementById("diagclose").src = "../../images/dialog/close_1.gif";
    };

    document.getElementById("diagclose").onclick = function () {
        document.body.removeChild(msgObj);
        document.body.removeChild(bgObj);

    };


    var moveX = 0;
    var moveY = 0;
    var moveTop = 0;
    var moveLeft = 0;
    var moveable = false;
    addEvent(msgObj, "mousedown", mousedown);
    addEvent(msgObj, "mouseup", mouseup);
    addEvent(msgObj, "selectstart", function () { return false; });
    addEvent(msgObj, "mousemove", mousemove);
    var resizeEvent;
    function mousedown() {
        moveable = true;
        resizeEvent = window.onresize;
        window.onresize = function () {
            bgObj.style.width = document.documentElement.clientWidth + "px";
            bgObj.style.height = document.documentElement.clientHeight + "px";
        }
        if (getBrowser() == "IE") msgObj.setCapture();
        var evt = getEvent();
        moveX = evt.clientX;
        moveY = evt.clientY;
        moveTop = parseInt(msgObj.style.top);
        moveLeft = parseInt(msgObj.style.left);
    }

    function mousemove() {
        if (moveable) {
            var evt = getEvent();
            var x = moveLeft + evt.clientX - moveX;
            var y = moveTop + evt.clientY - moveY;
            msgObj.style.left = x + "px";
            msgObj.style.top = y + "px";
        }
    }
    function mouseup() {
        if (moveable) {
            moveX = 0;
            moveY = 0;
            moveTop = 0;
            moveLeft = 0;
            moveable = false;
            window.onresize = resizeEvent;
            if (getBrowser() == "IE") msgObj.releaseCapture();
        }
    }


    var msgBox = table.insertRow(-1).insertCell(-1);
    msgBox.style.cssText = "font:10pt '宋体';height:100%;padding-left: 5px;padding-right: 5px;";
    msgBox.innerHTML = "<iframe marginwidth=0 marginheight=0 hspace=0 vspace=0 frameborder=0 scrolling=auto src='" + url + "' width=100% height=" + (parseInt(msgObj.style.height) - 40).toString() + "px> </iframe>";

    // 获得事件Event对象，用于兼容IE和FireFox 
    function getEvent() {
        return window.event || arguments.callee.caller.arguments[0];
    }

    // 事件绑定
    function addEvent(obj, eventTypeName, fn) {
        if (obj.addEventListener) {
            obj.addEventListener(eventTypeName, fn, true);
            return true;
        }
        else if (obj.attachEvent) {
            return obj.attachEvent("on" + eventTypeName, fn);
        }
        else {
            return false;
        }
    }

    // 检测浏览器类型
    function getBrowser() {
        var b;
        if ((navigator.userAgent.indexOf("MSIE") >= 0) && (navigator.userAgent.indexOf('Opera') < 0)) {
            b = "IE";
        }
        else if (navigator.userAgent.indexOf("Firefox") >= 0) {
            b = "Firefox";
        }
        else if (navigator.userAgent.indexOf("Opera") >= 0) {
            b = "Opera";
        }
        else {
            b = "Other";
        }
        return b;
    }

    return false;
}


function closeChildDialog(slient) {

    if (slient && slient == true) {

        parent.document.body.removeChild(parent.document.getElementById("bgObj"));
        parent.document.body.removeChild(parent.document.getElementById("msgObj"));
    }
    else {
        if (confirm("您确认关闭吗？")) {
            parent.document.body.removeChild(parent.document.getElementById("bgObj"));
            parent.document.body.removeChild(parent.document.getElementById("msgObj"));
        }
    }




    return false;
}

function closeDialog() {

    parent.document.body.removeChild(parent.document.getElementById("bgObj"));
    parent.document.body.removeChild(parent.document.getElementById("msgObj"));


    return false;
}

function Refresh(btn, msg) {

    if (msg == undefined)
        msg = "操作成功!";

    Succ(msg, function () { if ($("#" + btn) != null) $("#" + btn).click(); });
}

function doPostBack(ctrl, msg) {
    if (msg == undefined)
        msg = "操作成功!";

    alert_tips(msg, "Sucess");
    __doPostBack(ctrl, '');

}



function alert_tips(text, action, title) {
    var titleheight = "22px";
    var bordercolor = "#666699";
    var titlecolor = "#FFFFFF";
    var titlebgcolor = "#666699";
    var bgcolor = "#FFFFFF";

    var w = 250;
    var h = 150;

    if (title == undefined || title == "")
        title = "系统提示：";

    var iWidth = document.documentElement.clientWidth;
    var iHeight = document.documentElement.clientHeight;



    var msgObj = document.createElement("div");
    msgObj.id = "msgObjAlert";
    msgObj.style.cssText = "position:absolute;font:11px '宋体';top:" + Math.abs(iHeight - h) / 2 + "px;left:" + Math.abs(iWidth - w) / 2 + "px;width:" + w + "px;height:" + h + "px;border:5px solid " + bordercolor + ";background-color:" + bgcolor + ";padding:1px;line-height:22px;z-index:10009;";
    document.body.appendChild(msgObj);

    var table = document.createElement("table");
    table.style.cssText = "margin:0px;border:0px;padding:0px;width:100%;";
    table.cellSpacing = 0;
    msgObj.appendChild(table);

    var tr = table.insertRow(-1);

    var titleBar = tr.insertCell(-1);
    titleBar.style.cssText = "width:100%;height:26px; background-image: url('../../images/dialog/topxian.gif'); background-repeat: repeat-x;";
    titleBar.innerHTML = "<span style='float:left;color:#FFF; font-weight:bold; line-height:26px; font-size:12px;'>&nbsp;" + title + "</span><span style='float:right'><a href='javascript:'><img border='0' src='../../images/dialog/close_1.gif' width='26px' height='20px' id='diagcloseAlert' /></a></span>";


    //关闭按钮事件
    document.getElementById("diagcloseAlert").onmousemove = function () {
        document.getElementById("diagcloseAlert").src = "../../images/dialog/close_2.gif";
    };

    document.getElementById("diagcloseAlert").onmouseout = function () {
        document.getElementById("diagcloseAlert").src = "../../images/dialog/close_1.gif";
    };

    document.getElementById("diagcloseAlert").onclick = function () {
        document.body.removeChild(msgObj);
    };

    var msgBox = table.insertRow(-1).insertCell(-1);
    msgBox.style.cssText = "font:10pt '宋体';height:100%;align:left";

    var img = "../../images/dialog/info.gif";
    switch (action) {
        case "Error":
            img = "../../images/dialog/err.gif";
            break;
        case "ask":
            img = "../../images/dialog/ask.gif";
            break;
        case "Sucess":
            img = "../../images/dialog/right.gif";
            break;
        default:
            img = "../../images/dialog/info.gif";
    }
    msgBox.innerHTML = "<span style='margin-left:5px'><img src='" + img + "' /></span><span style='font-weight:bold; font-size:12px;margin-left:10px'>" + text + "</span>";

    return false;
}


function showDialogV2(title, page, action, id, width, height) {
    //return openChildDialog(title, 'BusinessMaintain.aspx?<%=QueryString.PARAM_ACTION_CODE %>=' + action + '&busiId=' + id, 650, 300);
    return openChildDialog(title, page + '?PageActionCode=' + action + '&BizCode=' + id, width, height);
}

function popupWithFullSize(url, name) {
    params = 'width=' + screen.width;
    params += ', height=' + screen.height;
    params += ', top=0, left=0'
    params += ', fullscreen=yes';

    newwin = window.open(url, 'name', params);
    if (window.focus) { newwin.focus() }
    return false;
}

function popupBySize(url, name, width, height) {
    params = 'width=' + width;
    params += ', height=' + height;
    params += ', top=0, left=0'
    params += ', fullscreen=yes';

    newwin = window.open(url, 'name', params);
    if (window.focus) { newwin.focus() }
    return false;
}

function Msg(msg, call) {
    layer.msg(msg, 2, 12);
}

function Waring(msg, call) {
    layer.msg(msg, 2, 8, (call || function () { }));
}

function Succ(msg, call) {
    layer.msg(msg, 2, 1, (call || function () { }));
}

function Error(msg, call) {
    layer.msg(msg, 2, 3, (call || function () { }));
}



function ShowDialog(url, width, height, title, closeCall) {
    closeCall = closeCall || function () { };
    var clientWidth = document.body.clientWidth;
    var temp = (clientWidth - width) / 2 + jQuery(document).scrollLeft();

    jQuery.layer({
        type: 2,
        title: title,
        shadeClose: false,
        border: [5, 0.4, '#4D63A4'],
        maxmin: false,
        fix: false,
        closeBtn: [1, true],
        area: [width + 'px', height + 'px'],
        iframe: { src: url.toLowerCase() },
        end: function (index) { closeCall(); }
    });


    jQuery(".xubox_main,#xubox_layer1,#xubox_shade1").mousewheel(function (event, delta, deltaX, deltaY) { return false; });

}

function ShowModelDialog(url, width, height, title, closeCall) {
    closeCall = closeCall || function () { };
    var clientWidth = document.body.clientWidth;
    var temp = (clientWidth - width) / 2 + jQuery(document).scrollLeft();

    var dialog = jQuery.layer({
        type: 2,
        title: false,
        shadeClose: false,
        border: [2, 0.4, '#4D63A4'],
        maxmin: false,
        fix: false,
        closeBtn: [0, false],
        area: [width + 'px', height + 'px'],
        iframe: { src: url.toLowerCase() },
        end: function (index) { closeCall(); }
    });
    jQuery(".xubox_main,#xubox_layer1,#xubox_shade1").mousewheel(function (event, delta, deltaX, deltaY) { return false; });
    return dialog;
}


function ParentShowDialog(url, width, height, title, closeCall) {
    window.parent.ShowDialog(url, width, height, title, closeCall);
    jQuery(".xubox_main,#xubox_layer1,#xubox_shade1").unmousewheel(null);
}

/*
jQuery.layer({
type: 2,
fix: false,
title: tit,
shade: [0.8, '#000'],
offset: ['', temp + 'px'],
border: [1, '#000'],
maxmin: false,
shadeClose: true,
area: [width + 'px', height + 'px'],
iframe: { src: url.toLowerCase() },
end: function (index) { }
});
*/
function CloseDialog(index) {
    window.alert("aaa");
    if (index) {
        window.parent.layer.close(index);
    }
    else {
        window.parent.layer.closeAll();
    }
    jQuery(".xubox_main,#xubox_layer1,#xubox_shade1").unmousewheel(null);
}

function CloseModelDialog(index) {
    if (index) {
        window.layer.close(index);
    }
    else {
        window.layer.closeAll();
    }
    jQuery(".xubox_main,#xubox_layer1,#xubox_shade1").unmousewheel(null);
}

function TriggerDialogSubmit() {

    window.parent.jQuery("#sys_btnRef").click();
    window.parent.layer.closeAll();
}

function Refresh() {
    window.parent.jQuery("#sys_btnRef").click();
}

function PopupWithFullSize(url, name) {
    params = 'width=' + screen.width;
    params += ', height=' + screen.height;
    params += ', top=0, left=0'
    params += ', fullscreen=yes,scrollbars=yes,toolbars=yes;';

    newwin = window.open(url, 'name', params);
    if (window.focus) { newwin.focus() }
    return false;
}
function PopupBySize(url, name, width, height) {
    params = 'width=' + width;
    params += ', height=' + height;
    params += ', top=0, left=0,scrollbars=yes'
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

function CheckUserInfo() {
    jQuery.ajax({
        url: '/MapHandler.ashx',
        data: { 'ajax': "cku", 'st': Math.random() },
        dataType: "JSON",
        type: "GET",
        success: function (data) { if (data == 500) { window.alert('请填报手机号码,同时注意更改登录密码'); ShowModelDialog('/Setting/dialog/UserInfoEdit.aspx', 700, 200, '编辑用户信息'); } }
    });
}

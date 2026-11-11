$(document).ready(function () {

    //if ($(".form_datetime").length > 0)
    //{
    //    $('.form_datetime').datetimepicker({
    //        language: 'zh-CN',
    //        weekStart: 1,
    //        todayBtn: 1,
    //        autoclose: 1,
    //        todayHighlight: 1,
    //        startView: 2,
    //        forceParse: 0,
    //        showMeridian: 1
    //    });

    //    $('.form_datetime[data-link-field="dtp_input1"]').datetimepicker("setDate", new Date(new Date().setHours(0, 0, 0, 0)));
    //    $('.form_datetime[data-link-field="dtp_input2"]').datetimepicker("setDate", new Date());

    //}

    $('input[name="datetimepicker"]').datetimepicker({
        forceParse: 0,//设置为0，时间不会跳转1899，会显示当前时间。
        language: 'zh-CN',//显示中文
        format: 'yyyy-mm-dd hh:ii',//显示格式
        //minView: "month",//设置只显示到月份
        initialDate: new Date(),//初始化当前日期
        autoclose: true,//选中自动关闭
        weekStart: 1,
        //startView: 2,
        //todayHighlight: 1,
        todayBtn: true//显示今日按钮
    })
    $("#date").datetimepicker("setDate", new Date() );  //设置显示默认当天的时间

})

var GetFullDateTime = function () {
    var time = new Date();//获取系统当前时间
    var year = time.getFullYear();
    var month = time.getMonth() + 1;
    var date = time.getDate();//系统时间月份中的日
    var hour = time.getHours();
    var minutes = time.getMinutes();
    var seconds = time.getSeconds();
    if (month < 10) {
        month = "0" + month;
    }
    if (date < 10) {
        date = "0" + date;
    }
    if (hour < 10) {
        hour = "0" + hour;
    }
    if (minutes < 10) {
        minutes = "0" + minutes;
    }
    if (seconds < 10) {
        seconds = "0" + seconds;
    }
    return year + "/" + month + "/" + date + " " + hour + ":" + minutes;// + ":" + seconds;
}

//日期格式化
var formatDateTime = function (date) {

    var y = date.getFullYear();
    var m = date.getMonth() + 1;
    m = m < 10 ? '0' + m : m;
    var d = date.getDate();
    d = d < 10 ? ('0' + d) : d;
    var h = date.getHours();
    h = h < 10 ? ('0' + h) : h;
    return y + '-' + m + '-' + d + ' ' + h + ":00:00";
}


//写cookie
var setCookie = function(name, value) {
    var Days = 30;
    var exp = new Date();
    exp.setTime(exp.getTime() + Days * 24 * 60 * 60 * 1000);
    document.cookie = name + "=" + escape(value) + "; path=/;expires=" + exp.toGMTString();
}

//拿cookie
var getCookie = function(name) {
    var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");

    if (arr = document.cookie.match(reg))

        return unescape(arr[2]);
    else
        return null;
}

var OpenMaskLayout = function () {
    $("div.mask").css({ "display": "block", "height": window.outerHeight, "top": $(document).scrollTop() - 75 });
    $(document).bind('mousewheel', function (event, delta) { return false; });
}

var CloseMaskLayout = function () {

    $("div.mask").css({ "display": "none", "height": window.outerHeight });
    $(document).unbind('mousewheel');
}

var OpenMaskPopUp = function () {
    $("div.mask").css({ "display": "block", "height": window.outerHeight });
}

var CloseMaskPopUp = function () {
    $("div.mask").css({ "display": "none", "height": window.outerHeight });
}

var StringEnterFormat = function (str) {
    if (str == null || str == '' || str.length == 0)
        return '';
    var n = parseInt(str.length / 2);
    return str.substring(0, n) + "</br>" + str.substring(n);
}

var popupOpen = function () {
    $('.popup-max').css({ 'height': document.documentElement.scrollHeight, 'top': $(document).scrollTop() + 'px' });
    $(document).bind('mousewheel', function (event, delta) { return false; });
}

var VideoStyleSetting = function () {
    $("#preview_1").css({ 'height': '400px' });
}

//模态框
var ModalInit = function () {
    //$("#myModal").modal('hide');
    $("#ModalTitle").html('');
    $("#ModalContent").html('');
    $("#ModalClose").css('display', '');
    $("#ModalSubmit").css('display', '');
    $("#ModalSubmit").unbind('click');
    $("#ModalClose").unbind('click');
    
}
var OpenSaveModal = function () {
    ModalInit();
    $("#ModalTitle").html('提示');
    $("#ModalTitle").css({ 'color': '#f39c12' });
    $("#ModalContent").html('是否要保存?');
    $("#ModalClose").css('display', '');
    $("#ModalSubmit").css('display', '');
    $("#ModalTitle").css('background', 'url(/Content/IOT/img/waring.png) no-repeat');
}
var OpenModifyModal = function () {
    ModalInit();
    $("#ModalTitle").html('提示');
    $("#ModalTitle").css({ 'color': '#f39c12' });
    $("#ModalTitle").css('background', 'url(/Content/IOT/img/waring.png) no-repeat');
    $("#ModalContent").html('是否要保存修改?');
    $("#ModalClose").css('display', '');
    $("#ModalSubmit").css('display', '');
}
var OpenDeleteModal = function (name) {
    ModalInit();
    $("#ModalTitle").html('提示');
    $("#ModalTitle").css({ 'color': '#f39c12' });
    $("#ModalTitle").css('background', 'url(/Content/IOT/img/waring.png) no-repeat');
    $("#ModalContent").html('是否要删除 ' + name + ' ?');
    $("#ModalClose").css('display', '');
    $("#ModalSubmit").css('display', '');
}
var OpenModalSuccessTips = function () {
    ModalInit();
    $("#ModalTitle").html('提示');
    $("#ModalTitle").css({ 'color': '#00c0ef' });
    $("#ModalTitle").css('background', 'url(/Content/IOT/img/success.png) no-repeat');
    $("#ModalContent").html('成功');
    $("#ModalClose").css('display', '');
    $("#ModalSubmit").css('display', 'none');
}
var OpenModalFailTips = function () {
    ModalInit();
    $("#ModalTitle").html('提示');
    $("#ModalTitle").css({ 'color': '#de513f' });
    $("#ModalTitle").css('background', 'url(/Content/IOT/img/failed.png) no-repeat');
    $("#ModalContent").html('失败');
    $("#ModalClose").css('display', '');
    $("#ModalSubmit").css('display', 'none');
}
var OpenModalContentTips = function (title, content, type) {
    ModalInit();
    var color = '#f39c12';
    var background = 'url(/Content/IOT/img/waring.png) no-repeat';
    switch(type)
    {
        case 'success':
            color = '#00c0ef';
            background = 'url(/Content/IOT/img/success.png) no-repeat';
            break;
        case 'fail':
            color = '#de513f';
            background = 'url(/Content/IOT/img/failed.png) no-repeat';
            break;
        case 'waring':
            color = '#f39c12';
            background = 'url(/Content/IOT/img/waring.png) no-repeat';
            break;
        default:
            break;
    }

    $("#ModalTitle").html(title);
    $("#ModalTitle").css({ 'color': color });
    $("#ModalTitle").css('background', background);
    $("#ModalContent").html(content);
    $("#ModalClose").css('display', '');
    $("#ModalSubmit").css('display', 'none');
}
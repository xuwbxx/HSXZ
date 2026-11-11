var AlarmCenterJS = function ($) {

    var Param = {};

    //  var myChart = {};
    var myOption = {};

    //   var myChart2 = {};
    var myOption2 = {};

    $(document).ready(function () {

        Init();

        ParamInit();

        AutoLoadQuery();

    })

    var ParamInit = function () {

        Param.PageIndex = 1;
        Param.PageSize = 20;
        Param.TotalPage = 0;
        Param.TotalCount = 0;


        Param.Begin = $("input[name='entryDateBegin']").val();
        Param.End = $("input[name='entryDateEnd']").val();
        Param.PointID = $("#select_AutoPoint").val();
        Param.PositionID = $("select#select_AutoPosition").val();
        //Param.DeviceID = $("select#select_AutoDevice").val();
        Param.LayerID = $("select#select_AutoLayer").val();
        Param.ScanTypeID = $("select#select_AutoScanType").val();
    }

    var Init = function () {

        myChart = echarts.init(document.getElementById('EChartContent'), 'macarons');
        myChart.setOption(myOption);
        myChart2 = echarts.init(document.getElementById('EChartContent1'), 'macarons');
        myChart2.setOption(myOption2);
        //$("#ThemeChoose").change(function () {

        //    myChart.dispose();

        //    var theme = $(this).val();

        //    myChart = echarts.init(document.getElementById('EChartContent'), theme);

        //    myChart.setOption(myOption);

        //    myChart2.dispose();
        //    myChart2 = echarts.init(document.getElementById('EChartContent1'), theme);

        //    myChart2.setOption(myOption2);
        //})
    }

    var AutoQuery = function () {

        //if ($("select#select_AutoLayer").val() == '0' || $("select#select_AutoScanType").val() == '0' || $("select#select_AutoPosition").val() == '0' || $("select#select_AutoDevice").val() == '0')
        //{
        //    return;
        //}

        //打开遮罩层
        // OpenMaskLayout();

        $.post('AutoCalQuery', Param)
                .success(function (data, statusText, jqXRH) {
                    if (data) {

                        if (data.Success) {
                            if (data.value == 1) {
                                //    window.alert(data.Url);
                                window.location.href = data.Url;
                            }
                            var Model = {};
                            Model.list = data.Data;
                            var html = template("AutoCalTemplate", Model);
                            $("#AutoHtmlTemplate").html(html);

                            //分页
                            Param.TotalPage = data.TotalPage;
                            Param.TotalCount = data.TotalCount;
                            Param.PageIndex = data.PageIndex;

                            //趋势图
                            myChart.dispose();
                            // myChart = {};
                            myOption = data.Echart;
                            myChart = echarts.init(document.getElementById('EChartContent'), 'macarons');
                            myChart.setOption(myOption);
                        }

                    }

                })
                .fail(function (jqXHR, statusText, errorMessage) {
                    //console.error(arguments);
                })
                .always(function (data) {
                    // 关闭等待遮罩
                    Pagination();

                    CloseMaskLayout();
                });
    }

    var AutoLoadQuery = function () {

        //if ($("select#select_AutoLayer").val() == '0' || $("select#select_AutoScanType").val() == '0' || $("select#select_AutoPosition").val() == '0' || $("select#select_AutoDevice").val() == '0')
        //{
        //    return;
        //}

        //打开遮罩层
        // OpenMaskLayout();

        $.post('AutoCalLoadQuery', Param)
                .success(function (data, statusText, jqXRH) {
                    if (data) {

                        if (data.Success) {
                            var Model = {};
                            Model.list = data.Data;
                            var html = template("AutoCalTemplate", Model);
                            $("#AutoHtmlTemplate").html(html);

                            //分页
                            Param.TotalPage = data.TotalPage;
                            Param.TotalCount = data.TotalCount;
                            Param.PageIndex = data.PageIndex;

                            //趋势图
                            //   myChart.dispose();

                            myOption = data.Echart;
                            myChart = echarts.init(document.getElementById('EChartContent'), 'macarons');
                            myChart.setOption(myOption);
                        }

                    }

                })
                .fail(function (jqXHR, statusText, errorMessage) {
                    //console.error(arguments);
                })
                .always(function (data) {
                    // 关闭等待遮罩
                    Pagination();

                    CloseMaskLayout();
                });
    }

    var DoAutoQuery = function () {

        Param.Begin = $("input[name='entryDateBegin']").val();
        Param.End = $("input[name='entryDateEnd']").val();
        Param.PointID = $("#select_AutoPoint").val();
        Param.PositionID = $("select#select_AutoPosition").val();
        // Param.DeviceID = $("select#select_AutoDevice").val();
        Param.LayerID = $("select#select_AutoLayer").val();
        Param.ScanTypeID = $("select#select_AutoScanType").val();
        AutoQuery();
    }

    var Pagination = function () {
        $("#Pagination").pagination({
            currentPage: Param.PageIndex,
            totalPage: Param.TotalPage,
            isShow: true,
            totalCount: Param.TotalCount,
            count: 7,
            homePageText: "首页",
            endPageText: "尾页",
            prevPageText: "上一页",
            nextPageText: "下一页",
            callback: function (current) {
                Param.PageIndex = current;
                DoAutoQuery();
            }
        });
        //$("#TotalPage").html(Param.TotalPage);
    }

    var Export = function () {
        var Begin = $("input[name='entryDateBegin']").val();
        var End = $("input[name='entryDateEnd']").val();
        var PointID = $("#select_AutoPoint").val();
        var PositionID = $("select#select_AutoPosition").val();
        // var DeviceID = $("select#select_AutoDevice").val();
        var LayerID = $("select#select_AutoLayer").val();
        var ScanTypeID = $("select#select_AutoScanType").val();

        window.location.href = "Export1?Begin=" + Begin + "&End=" + End + "&PointID=" + PointID + "&PositionID=" + PositionID + "&DeviceID=" + DeviceID + "&LayerID=" + LayerID + "&ScanTypeID=" + ScanTypeID;

    }

    //var ChooseTheme = function (dom) {

    //    myChart.dispose();

    //    var theme = $(dom).val();

    //    myChart = echarts.init(document.getElementById('EChartContent'), theme);

    //    myChart.setOption(myOption);

    //}

    var SelectChange = function (type) {

        var option = '<option value="0">请选择</option>';

        if ($("#select_AutoLayer").val() == '0') {
            $("#select_AutoPosition").val('0');
            $("#select_AutoPosition").html(option);
            //$("#select_AutoDevice").val('0');
            //$("#select_AutoDevice").html(option);
            $("#select_AutoPoint").val('0');
            //$("#select_AutoPoint").html('<option value="0">全部</option>');
        }
        if ($("#select_AutoScanType").val() == '0') {
            $("#select_AutoPosition").val('0');
            $("#select_AutoPosition").html(option);
            $("#select_AutoPoint").val('0');
            //$("#select_AutoPoint").html('<option value="0">全部</option>');
        }
        if ($("#select_AutoPosition").val() == '0') {
            $("#select_AutoPoint").val('0');
            //$("#select_AutoPoint").html('<option value="0">全部</option>');
        }
        //if ($("#select_AutoDevice").val() == '0') {
        //    $("#select_AutoPoint").val('0');
        //    $("#select_AutoPoint").html('<option value="0">全部</option>');
        //}

        if (type == 'layer') {
            PositionQuery();
            // DeviceQuery();
            PointQuery();
        }
        else if (type == 'scantype') {
            PositionQuery();
            //  DeviceQuery();
            PointQuery();
        }
        else if (type == 'position') {
            PointQuery();
        }
        //else if (type == 'device') {
        //    PointQuery();
        //}


    }

    var PositionQuery = function () {
        Param.LayerID = $("#select_AutoLayer").val();
        Param.ScanTypeID = $("#select_AutoScanType").val();

        if (Param.LayerID == '0' || Param.ScanTypeID == '0') {
            return;
        }

        $.post('PositionIndexQuery', Param)
            .success(function (data, statusText, jqXRH) {
                if (data) {
                    if (data.Success) {
                        var html = '<option value="0">请选择</option>';
                        $(data.Data).each(function (n, d) {
                            html += '<option value="' + d.ID + '">' + d.PositionName + '</option>';
                        })
                        $("#select_AutoPosition").html(html);
                    }
                }
            })
            .fail(function (jqXHR, statusText, errorMessage) {
            })
            .always(function (data) {
                // 关闭等待遮罩
                //CloseMaskPopUp();
            });
    }

    var DeviceQuery = function () {
        Param.LayerID = $("#select_AutoLayer").val();

        if (Param.LayerID == '0') {
            return;
        }

        $.post('DeviceQuery', Param)
            .success(function (data, statusText, jqXRH) {
                if (data) {
                    if (data.Success) {
                        var html = '<option value="0">请选择</option>';
                        $(data.Data).each(function (n, d) {
                            html += '<option value="' + d.ID + '">' + d.DeviceName + '</option>';
                        })
                        $("#select_AutoDevice").html(html);
                    }
                }
            })
            .fail(function (jqXHR, statusText, errorMessage) {
            })
            .always(function (data) {
                // 关闭等待遮罩
                //CloseMaskPopUp();
            });
    }

    var PointQuery = function () {
        Param.PositionID = $("#select_AutoPosition").val();
        Param.DeviceID = $("#select_AutoDevice").val();

        if (Param.PositionID == '0') {
            return;
        }
        $.post('PointQuery', Param)
            .success(function (data, statusText, jqXRH) {
                if (data) {
                    if (data.Success) {
                        var html = '';
                        $(data.Data).each(function (n, d) {
                            html += '<li><input type="checkbox" value="' + d.ID + '" />' + d.PointName + '</li>';
                            //  html += '<option value="' + d.ID + '">' + d.PointName + '</option>';
                        })
                        $("#UL_PointPop").html(html);
                    }
                }
            })
            .fail(function (jqXHR, statusText, errorMessage) {
            })
            .always(function (data) {
                // 关闭等待遮罩
                //CloseMaskPopUp();
            });
    }

    //打开用户信息弹出窗体
    var ShowPointInfo = function (dom) {
        //var ID = $(dom).attr('data-id');
        //if (ID == null || ID == '') {
        //    return;
        //}
        //$("p[name='UserInfoTitle']").html('用户信息更改');
        // $("input[name='TypeLayer']").val('Modify');

        //  DataInit();

        var Param = {};
        Param.PositionID = $("#select_AutoPosition").val();
        //  Param.DeviceID = $("#select_AutoDevice").val();

        if (Param.PositionID == '0') {
            alert('请选择监测位置');
            return;
        }

        // OpenMaskPopUp();

        $.post('PointQuery', Param)
            .success(function (data, statusText, jqXRH) {
                if (data) {
                    if (data.Success) {

                    }
                }
            })
            .fail(function (jqXHR, statusText, errorMessage) {
                //console.error(arguments);
            })
            .always(function (data) {
                // 关闭等待遮罩
                CloseMaskPopUp();
            });

        popupOpen();

        $('#PointPopUp').slideDown();

    }

    //关闭窗体并刷新
    var CloseModalEvent = function () {

        TurnBack();
        //   LayerQuery();
    }

    var TurnBack = function () {
        //$('.popup-max').css('display', 'none');
        var Point = '';
        $("ul[name='UL_PointPop'] input[type='checkbox']:checked").each(function (index, dom) {
            Point += $(dom).val() + ',';
        })
        $("#select_AutoPoint").val(Point);
        // alert($("#select_AutoPoint").val());
        $('.popup-close').click();
    }

    return {
        HomeSiteInit: function () {
            Init();
        },
        DoAutoQuery: DoAutoQuery,
        Export: Export,
        TurnBack: TurnBack,
        ShowPointInfo: ShowPointInfo,
        // ChooseTheme: ChooseTheme,
        SelectChange: SelectChange,
        field: {

        }
    }

}(jQuery);
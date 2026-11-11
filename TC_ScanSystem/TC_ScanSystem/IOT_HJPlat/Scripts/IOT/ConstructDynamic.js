var ConstructAutoJS = function ($) {

    var Param = {};

    var myChart = null;      //趋势图实例1
    var myOption = {};       //图表1配置缓存（容器隐藏时先缓存，Tab显示后渲染）
    var myChart2 = null;     //趋势图实例2
    var myOption2 = {};      //图表2配置缓存

    $(document).ready(function () {

        ParamInit();
        InitTabEvent();

        AutoLoadQuery();

    })

    var ParamInit = function () {

        Param.PageIndex = 1;
        Param.PageSize = 20;
        Param.TotalPage = 0;
        Param.TotalCount = 0;

        Param.DeviceType = 1;
        Param.Begin = $("input[name='entryDateBegin']").val();
        Param.End = $("input[name='entryDateEnd']").val();
        Param.PointID = $("#select_AutoPoint").val();
        Param.PositionID = $("select#select_AutoPosition").val();
        //Param.DeviceID = $("select#select_AutoDevice").val();
        Param.LayerID = $("select#select_AutoLayer").val();
        Param.ScanTypeID = $("#select_AutoScanType").val();
    }

    //容器是否可见（隐藏Tab内宽高为0，此时不能初始化ECharts）
    var IsChartVisible = function (id) {
        var dom = document.getElementById(id);
        return dom && dom.offsetWidth > 0 && dom.offsetHeight > 0;
    }

    var EnsureChart1 = function () {
        if (!IsChartVisible('EChartContent')) {
            return null;
        }
        if (!myChart) {
            myChart = echarts.init(document.getElementById('EChartContent'), 'macarons');
        }
        return myChart;
    }

    var EnsureChart2 = function () {
        if (!IsChartVisible('EChartContent1')) {
            return null;
        }
        if (!myChart2) {
            myChart2 = echarts.init(document.getElementById('EChartContent1'), 'macarons');
        }
        return myChart2;
    }

    //容器可见则立即渲染，否则仅缓存配置，待趋势图Tab显示时渲染
    var SetChart1Option = function (option) {
        myOption = option;
        var chart = EnsureChart1();
        if (chart) {
            chart.clear();
            chart.setOption(myOption);
        }
    }

    var SetChart2Option = function (option) {
        myOption2 = option;
        var chart = EnsureChart2();
        if (chart) {
            chart.clear();
            chart.setOption(myOption2);
        }
    }

    var ResizeCharts = function () {
        //趋势图Tab显示前容器继承隐藏父级的尺寸，需要重新计算高度
        var h = $("div#myTabContent").height() - 300;
        if (h < 300) { h = 300; }
        $("div#EChartContent").css('height', h + 'px');
        $("div#EChartContent1").css('height', h + 'px');
        if (myChart && IsChartVisible('EChartContent')) { myChart.resize(); }
        if (myChart2 && IsChartVisible('EChartContent1')) { myChart2.resize(); }
    }

    var InitTabEvent = function () {
        //趋势图Tab显示时才初始化并渲染图表（隐藏容器宽高为0，直接init会报Can't get DOM width or height）
        $('a[href="#qushi"]').on('shown.bs.tab', function () {
            ResizeCharts();
            if (myOption) { SetChart1Option(myOption); }
            if (myOption2) { SetChart2Option(myOption2); }
        });
        $(window).on('resize', ResizeCharts);
    }

    //保留原入口：容器可见时初始化并渲染（当前页面ready时不再直接调用）
    var Init = function () {
        ResizeCharts();
        if (myOption) { SetChart1Option(myOption); }
        if (myOption2) { SetChart2Option(myOption2); }
    }

    var AutoQuery = function () {

        //if ($("select#select_AutoLayer").val() == '0' || $("select#select_AutoScanType").val() == '0' || $("select#select_AutoPosition").val() == '0' || $("select#select_AutoDevice").val() == '0')
        //{
        //    return;
        //}

     //   myChart.dispose();

        //打开遮罩层
      //  OpenMaskLayout();
        //myChart.dispose();
  //      myChart2.dispose();
        $.post('/Scan/Construction/DynamicQuery', Param)
                .success(function (data, statusText, jqXRH) {
                    if (data) {

                        if (data.Success) {
                            //if (data.value == 1) {
                            //    //    window.alert(data.Url);
                            //    window.location.href = data.Url;
                            //}
                            var Model = {};
                            Model.list = data.Data;
                            var html = template("DynamicTemplate", Model);
                            $("#AutoHtmlTemplate").html(html);

                            //分页
                            Param.TotalPage = data.TotalPage;
                            Param.TotalCount = data.TotalCount;
                            Param.PageIndex = data.PageIndex;

                            //趋势图
                           //myChart = {};
                            
                            //myChart._chartsMap = {};
                            //myChart.__chartsViews = {};
                           // myChart = {};
                            //myOption={};
                            //$("#EChartContent").html("");

                            //趋势图（容器隐藏时仅缓存配置，Tab显示后渲染）
                            SetChart1Option(data.Echart);
                            
                            
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

        $.post('DynamicChart', Param)
               .success(function (data, statusText, jqXRH) {
                   if (data) {

                       if (data.Success) {
                           //var Model = {};
                           //Model.list = data.Data;
                           //var html = template("AutoTemplate", Model);
                           //$("#AutoHtmlTemplate").html(html);

                           ////分页
                           //Param.TotalPage = data.TotalPage;
                           //Param.TotalCount = data.TotalCount;
                           //Param.PageIndex = data.PageIndex;

                           //趋势图
                           //myChart2 = {};
                           //myOption2 = {};
                           //  myChart2 = {};
                           SetChart2Option(data.Echart);
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
      //  OpenMaskLayout();

        $.post('DynamicLoadQuery', Param)
                .success(function (data, statusText, jqXRH) {
                    if (data) {

                        if (data.Success) {
                            
                            var Model = {};
                            Model.list = data.Data;
                            var html = template("DynamicTemplate", Model);
                            $("#AutoHtmlTemplate").html(html);

                            //分页
                            Param.TotalPage = data.TotalPage;
                            Param.TotalCount = data.TotalCount;
                            Param.PageIndex = data.PageIndex;

                            //趋势图（容器隐藏时仅缓存配置，Tab显示后渲染）
                            SetChart1Option(data.Echart);
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


        //$.post('DynamicLoadChart', Param)
        //        .success(function (data, statusText, jqXRH) {
        //            if (data) {

        //                if (data.Success) {
        //                    var Model = {};
        //                    Model.list = data.Data;
        //                    //var html = template("AutoCalTemplate", Model);
        //                    //$("#AutoHtmlTemplate").html(html);

        //                    ////分页
        //                    //Param.TotalPage = data.TotalPage;
        //                    //Param.TotalCount = data.TotalCount;
        //                    //Param.PageIndex = data.PageIndex;

        //                    //趋势图
        //                    myOption2 = data.Echart;
        //                    myChart2 = echarts.init(document.getElementById('EChartContent1'), 'macarons');
        //                    myChart2.setOption(myOption);
        //                }

        //            }

        //        })
        //        .fail(function (jqXHR, statusText, errorMessage) {
        //            //console.error(arguments);
        //        })
        //        .always(function (data) {
        //            // 关闭等待遮罩
        //            Pagination();

        //            CloseMaskLayout();
        //        });

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
      //  var DeviceID = $("select#select_AutoDevice").val();
        var LayerID = $("select#select_AutoLayer").val();
        var ScanTypeID = $("select#select_AutoScanType").val();

        window.location.href = "ExportDynamic?Begin=" + Begin + "&End=" + End + "&PointID=" + PointID + "&PositionID=" + PositionID +  "&LayerID=" + LayerID + "&ScanTypeID=" + ScanTypeID;

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
        if ($("#select_AutoDevice").val() == '0') {
            $("#select_AutoPoint").val('0');
           // $("#select_AutoPoint").html('<option value="0">全部</option>');
        }

        if (type == 'layer') {
            PositionQuery();
           // DeviceQuery();
            PointQuery();
        }
        else if (type == 'scantype') {
            PositionQuery();
            //DeviceQuery();
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

        $.post('DeviceDynamicQuery', Param)
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
      //  Param.DeviceID = $("#select_AutoDevice").val();

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
        Param.DeviceID = $("#select_AutoDevice").val();
        Param.DeviceType = 1;
        if (Param.PositionID == '0') {
            alert('请选择监测位置');
            return;
        }

       // OpenMaskPopUp();

        $.post('PointIndexQuery', Param)
            .success(function (data, statusText, jqXRH) {
                if (data) {
                    if (data.Success) {
                        var html = '';
                        $(data.Data).each(function (n, d) {
                            html += '<li><input type="checkbox" value="' + d.ID + '" />' + d.PointName + '</li>';
                            //html += '<option value="' + d.ID + '">' + d.PointName + '</option>';
                        })
                        $("#UL_PointPop").html(html);
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
        TurnBack: TurnBack,
        ShowPointInfo: ShowPointInfo,
        Export: Export,
        //ChooseTheme: ChooseTheme,
        SelectChange: SelectChange,
        field: {

        }
    }

}(jQuery);
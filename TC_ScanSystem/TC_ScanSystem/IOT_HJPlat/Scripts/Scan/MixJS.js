var MixJS = function ($) {

    var Param = {};
    var myOption = {};
    var Model = {};

    $(document).ready(function () {

        EchartInit();

        ParamInit();

        Query();

    })

    var EchartInit = function () {

        //echarts皮肤初始化
        $("#ThemeChoose").change(function () {

            myChart.dispose();

            var theme = $(this).val();

            myChart = echarts.init(document.getElementById('EChartContent'), theme);

            myChart.setOption(myOption);
        })
    }

    var ParamInit = function () {

        Param.PageIndex = 1;
        Param.PageSize = 10;
        Param.TotalPage = 0;
        Param.TotalCount = 0;


        Param.Begin = $("input[name='MixBegin']").val();
        Param.End = $("input[name='MixEnd']").val();
        Param.MixMachineNo = $("input[name='txt_MixNo']").val();
    }


    var Query = function () {

        OpenMaskLayout();

        $.post('HistoryQuery', Param)
           .success(function (data, statusText, jqXRH) {
               if (data) {

                   if (data.Success) {
                       Model.list = data.Data;
                       var html = template("MixHtmlTemplate", Model);
                       $("#MixHtml").html(html);

                       //分页
                       Param.TotalPage = data.TotalPage;
                       Param.TotalCount = data.TotalCount;
                       Param.PageIndex = data.PageIndex;
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

        $.post('MixingStation_ByDate', Param)
                .success(function (data, statusText, jqXRH) {
                    if (data) {

                        if (data.Success) {

                            //趋势图
                            myOption = data.Echart;
                            myChart = echarts.init(document.getElementById('EChartByDate'), 'macarons');
                            myChart.setOption(myOption);
                        }
                    }
                })
                .fail(function (jqXHR, statusText, errorMessage) {
                    //console.error(arguments);
                })
                .always(function (data) {
                    //Pagination();
                    CloseMaskLayout();
                });
        $.post('MixingStation_ByMaterial', Param)
                .success(function (data, statusText, jqXRH) {
                    if (data) {

                        if (data.Success) {

                            //趋势图
                            myOption = data.Echart;
                            myChart = echarts.init(document.getElementById('EChartByMaterial'), 'macarons');
                            myChart.setOption(myOption);
                        }
                    }
                })
                .fail(function (jqXHR, statusText, errorMessage) {
                    //console.error(arguments);
                })
                .always(function (data) {
                    //Pagination();
                    CloseMaskLayout();
                });

         $.post('MixingStation_ByStation', Param)
                .success(function (data, statusText, jqXRH) {
                    if (data) {

                        if (data.Success) {

                            //趋势图
                            myOption = data.Echart;
                            myChart = echarts.init(document.getElementById('EChartByStation'), 'macarons');
                            myChart.setOption(myOption);
                        }
                    }
                })
                .fail(function (jqXHR, statusText, errorMessage) {
                    //console.error(arguments);
                })
                .always(function (data) {
                    //Pagination();
                    CloseMaskLayout();
                });


        $.post('MixingStation_ByQuality', Param)
               .success(function (data, statusText, jqXRH) {
                   if (data) {

                       if (data.Success) {

                           //趋势图
                           myOption = data.Echart;

//                           myOption = {
//    tooltip : {
//        trigger: 'item',
//        formatter: "{a} <br/>{b} : {c} ({d}%)"
//    },
//    legend: {
//        orient : 'vertical',
//        x : 'left',
//        data:['直接访问','邮件营销','联盟广告','视频广告','搜索引擎']
//    },
//    toolbox: {
//        show : true,
//        feature : {
//            mark : {show: true},
//            dataView : {show: true, readOnly: false},
//            magicType : {
//                show: true, 
//                type: ['pie', 'funnel'],
//                option: {
//                    funnel: {
//                        x: '25%',
//                        width: '50%',
//                        funnelAlign: 'center',
//                        max: 1548
//                    }
//                }
//            },
//            restore : {show: true},
//            saveAsImage : {show: true}
//        }
//    },
//    calculable : true,
//    series : [
//        {
//            name:'访问来源',
//            type:'pie',
//            radius : ['50%', '70%'],
//            itemStyle : {
//                normal : {
//                    label : {
//                        show : false
//                    },
//                    labelLine : {
//                        show : false
//                    }
//                },
//                emphasis : {
//                    label : {
//                        show : true,
//                        position : 'center',
//                        textStyle : {
//                            fontSize : '30',
//                            fontWeight : 'bold'
//                        }
//                    }
//                }
//            },
//            data:[
//                {value:335, name:'直接访问'},
//                {value:310, name:'邮件营销'},
//                {value:234, name:'联盟广告'},
//                {value:135, name:'视频广告'},
//                {value:1548, name:'搜索引擎'}
//            ]
//        }
//    ]
//};

                           myChart = echarts.init(document.getElementById('EChartByQuality'), 'macarons');
                           myChart.setOption(myOption);
                       }
                   }
               })
               .fail(function (jqXHR, statusText, errorMessage) {
                   //console.error(arguments);
               })
               .always(function (data) {
                   //Pagination();
                   CloseMaskLayout();
               });
        //$.post('DataQuery', Param)
        //        .success(function (data, statusText, jqXRH) {
        //            if (data) {

        //                if (data.Success) {
        //                    var Model = {};
        //                    Model.list = data.Data;
        //                    var html = template("MixHtmlTemplate", Model);
        //                    $("#MixHtml").html(html);

        //                    //分页
        //                    Param.TotalPage = data.TotalPage;
        //                    Param.TotalCount = data.TotalCount;
        //                    Param.PageIndex = data.PageIndex;


        //                    //趋势图
        //                    myOption = data.Echart;
        //                    myChart = echarts.init(document.getElementById('EChartContent'), 'macarons');
        //                    myChart.setOption(myOption);
        //                }
        //            }
        //        })
        //        .fail(function (jqXHR, statusText, errorMessage) {
        //            //console.error(arguments);
        //        })
        //        .always(function (data) {
        //            Pagination();
        //            CloseMaskLayout();
        //        });

    }

    var ShowDetails = function (Idx) {
        var currDetail = {};
        for (var i = 0; i < Model.list.length; i++) {
            if (Idx == Model.list[i].Idx) {
                currDetail.SkeletonSoil = Model.list[i].SkeletonSoil;
                currDetail.SkeletonSoilRate = Model.list[i].SkeletonSoilRate;
                currDetail.BoneFlour = Model.list[i].BoneFlour;
                currDetail.BoneFlourRate = Model.list[i].BoneFlourRate;
                currDetail.Cement = Model.list[i].Cement;
                currDetail.CementRate = Model.list[i].CementRate;
                currDetail.Flyash = Model.list[i].Flyash;
                currDetail.FlyashRate = Model.list[i].FlyashRate;
                currDetail.PowderOre = Model.list[i].PowderOre;
                currDetail.PowderOreRate = Model.list[i].PowderOreRate;
                currDetail.Water = Model.list[i].Water;
                currDetail.WaterRate = Model.list[i].WaterRate;
                currDetail.Addition = Model.list[i].Addition;
                currDetail.AdditionRate = Model.list[i].AdditionRate;
                currDetail.PowderLot = Model.list[i].PowderLot;
                currDetail.PowderLotRate = Model.list[i].PowderLotRate;

                var temp = {}
                temp.list = currDetail;
                var html = template("HistoryPopUp", temp);

                $("#DetailHtml").html(html);
                $("#Title").html("明细");

                popupOpen()
                $('.popup-max').slideDown();
                break;
            }

        }

    }


    var DoQuery = function () {
        Param.Begin = $("input[name='MixBegin']").val();
        Param.End = $("input[name='MixEnd']").val();
        Param.MixMachineNo = $("input[name='txt_MixNo']").val();
        Query();
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
                Query();
            }
        });
    }

    var Export = function () {
        var MixMachineNo = $("#txt_MixNo").val();
        var Begin = $("input[name='MixBegin']").val();
        var End = $("input[name='MixEnd']").val();

        window.location.href = "Export?Begin=" + Begin + "&End=" + End + "&MixMachineNo=" + MixMachineNo;

    }


    return {
        HomeSiteInit: function () {
            //Init();
        },
        DoQuery: DoQuery,
        ShowDetails: ShowDetails,
        Export: Export,
        field: {

        }
    }

}(jQuery);
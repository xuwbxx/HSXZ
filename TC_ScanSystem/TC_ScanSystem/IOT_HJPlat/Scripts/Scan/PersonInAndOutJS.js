var PersonInAndOutJS = function ($) {

    var Param = {};

    $(document).ready(function () {

        Init();

        ParamInit();

        Query();

    })

    var ParamInit = function () {

        Param.PageIndex = 1;
        Param.PageSize = 10;
        Param.TotalPage = 0;
        Param.TotalCount = 0;


        Param.Begin = $("input[name='PersonBegin']").val();
        Param.End = $("input[name='PersonEnd']").val();
        Param.Name = $("input[name='txt_Name']").val();
    }

    var Init = function () {

        
    }

    var Query = function () {


        OpenMaskLayout();

        $.post('DataQuery', Param)
                .success(function (data, statusText, jqXRH) {
                    if (data) {

                        if (data.Success) {
                            var Model = {};
                            Model.list = data.Data;
                            var html = template("PersonInAndOutHtmlTemplate", Model);
                            $("#PersonInAndOutHtml").html(html);

                            //分页
                            Param.TotalPage = data.TotalPage;
                            Param.TotalCount = data.TotalCount;
                            Param.PageIndex = data.PageIndex;

                            //趋势图
                            //EchartShow(data.Data);
                        }

                    }

                })
                .fail(function (jqXHR, statusText, errorMessage) {
                    //console.error(arguments);
                })
                .always(function (data) {
                    Pagination();
                    CloseMaskLayout();
                });

    }

    var EchartShow = function (data) {
        //if (option == null)
        //    return;

        //拼接echart模型
        OptionTitle.text = "环境监测趋势图";

        OptionLegend.data = ["温度", "湿度", "噪音", "PM2.5", "PM10"];

        var TempSeries = {
            name: '温度',
            type: 'line',
            data: [],
            markPoint: {
                data: [
                    { type: 'max', name: '最大值' },
                    { type: 'min', name: '最小值' }
                ]
            },
            markLine: {
                data: [
                    { type: 'average', name: '平均值' }
                ]
            }
        };
        var HumiSeries = {
            name: '湿度',
            type: 'line',
            data: [],
            markPoint: {
                data: [
                    { type: 'max', name: '最大值' },
                    { type: 'min', name: '最小值' }
                ]
            },
            markLine: {
                data: [
                    { type: 'average', name: '平均值' }
                ]
            }
        };
        var NoisySeries = {
            name: '噪音',
            type: 'line',
            data: [],
            markPoint: {
                data: [
                    { type: 'max', name: '最大值' },
                    { type: 'min', name: '最小值' }
                ]
            },
            markLine: {
                data: [
                    { type: 'average', name: '平均值' }
                ]
            }
        };
        var PM25Series = {
            name: 'PM2.5',
            type: 'line',
            data: [],
            markPoint: {
                data: [
                    { type: 'max', name: '最大值' },
                    { type: 'min', name: '最小值' }
                ]
            },
            markLine: {
                data: [
                    { type: 'average', name: '平均值' }
                ]
            }
        };
        var PM10Series = {
            name: 'PM10',
            type: 'line',
            data: [],
            markPoint: {
                data: [
                    { type: 'max', name: '最大值' },
                    { type: 'min', name: '最小值' }
                ]
            },
            markLine: {
                data: [
                    { type: 'average', name: '平均值' }
                ]
            }
        };

        $(data).each(function (index, dom) {
            //X
            OptionXAxis.data.push(dom.CreateTime);

            //Series
            TempSeries.data.push(dom.Temperature);
            HumiSeries.data.push(dom.Humidity);
            NoisySeries.data.push(dom.Noisy);
            PM25Series.data.push(dom.PM25);
            PM10Series.data.push(dom.PM10);
        })
        var option = {};
        option.title = OptionTitle;
        option.tooltip = OptionTooltip;
        option.legend = OptionLegend;
        option.toolbox = OptionToolbox;
        option.xAxis = new Array();
        option.xAxis.push(OptionXAxis);
        option.yAxis = new Array();
        option.yAxis.push(OptionYAxis);
        option.series = new Array();
        option.series.push(TempSeries);
        option.series.push(HumiSeries);
        option.series.push(NoisySeries);
        option.series.push(PM25Series);
        option.series.push(PM10Series);
        option.calculable = true;

        var dom = document.getElementById("EChartContent");
        var myChart = echarts.init(dom);
        if (option && typeof option === "object") {
            myChart.setOption(option, true);
        }
    }

    var DoQuery = function () {

        Param.Begin = $("input[name='PersonBegin']").val();
        Param.End = $("input[name='PersonEnd']").val();
        Param.Name = $("input[name='txt_Name']").val();
        Param.PointID = $("select[name='Select_Point']").val();
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
        //$("#TotalPage").html(Param.TotalPage);
    }



    return {
        HomeSiteInit: function () {
            Init();
        },
        DoQuery: DoQuery,
        field: {

        }
    }

}(jQuery);
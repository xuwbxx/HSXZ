var EnvironmentJS = function ($) {

    var Param = {};
    var myOption = {};

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


        Param.Begin = $("input[name='environmentBegin']").val();
        Param.End = $("input[name='environmentEnd']").val();
        Param.PointID = $("select[name='Select_Point']").val();
    }


    var Query = function () {

        OpenMaskLayout();

        $.post('DataQuery', Param)
                .success(function (data, statusText, jqXRH) {
                    if (data) {

                        if (data.Success) {
                            var Model = {};
                            Model.list = data.Data;
                            var html = template("EnvironmentHtmlTemplate", Model);
                            $("#EnvironmentHtml").html(html);

                            //分页
                            Param.TotalPage = data.TotalPage;
                            Param.TotalCount = data.TotalCount;
                            Param.PageIndex = data.PageIndex;


                            //趋势图
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
                    Pagination();
                    CloseMaskLayout();
                });

    }


    var DoQuery = function () {

        Param.Begin = $("input[name='environmentBegin']").val();
        Param.End = $("input[name='environmentEnd']").val();
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
    }



    return {
        HomeSiteInit: function () {
            //Init();
        },
        DoQuery: DoQuery,
        field: {

        }
    }

}(jQuery);
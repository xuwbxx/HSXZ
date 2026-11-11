var MainJS = function ($) {

    var param = {};

    var myChart = {};
    var myOption = {};

    $(document).ready(function () {
        Init();

        VideoQuery();
        //MapQuery();
        EnvironmentQuery();
        DownloadQuery();


    })

    var Init = function () {

        $("#ThemeChoose").change(function () {

            myChart.dispose();

            var theme = $(this).val();

            myChart = echarts.init(document.getElementById('EChartContent'), theme);

            myChart.setOption(myOption);
        })
    }

    //视频
    var VideoQuery = function () {
        $("#VideoHtmlTemplate").addClass("mainmask");
        //OpenMaskLayout();
        $.post('/Main/HomePage/VideoQuery', '')
                .success(function (data, statusText, jqXRH) {
                    if (data) {
                        if (data.Success) {
                            VideoHtmlAppend(data.Data);
                        }
                    }
                })
                .fail(function (jqXHR, statusText, errorMessage) {
                    //console.error(arguments);
                })
                .always(function (data) {
                    $("#VideoHtmlTemplate").removeClass("mainmask");
                });
    }

    //环境监测
    var EnvironmentQuery = function () {

        $("#ScanDataTemplate").addClass("mainmask");

        $.post('/Main/HomePage/EnvironmentQuery', '')
                .success(function (data, statusText, jqXRH) {
                    if (data) {
                        if (data.Success) {
                            var Model = {};
                            Model.list = data.Data;
                            var html = template("EnvironmentTemplate", Model);
                            $("#ScanDataTemplate").html(html);
                        }
                    }
                })
                .fail(function (jqXHR, statusText, errorMessage) {
                    //console.error(arguments);
                })
                .always(function (data) {
                    $("#ScanDataTemplate").removeClass("mainmask");
                });
    }



    var VideoHtmlAppend = function (dom) {
        var Html = "<p>施工监控</p><a href='/Main/Monitor/Index'>查看更多</a>";
        if (dom.length == 0) {
            Html += "<p>暂无视频</p>"
        }
        else
        {
            dom = dom.slice(0, 9);
            for (var i = 0; i < dom.length; i++)
            {
                if (i % 3 == 0)
                {
                    Html += "<ul>";
                }

                Html += "<li><a href='javascript:void(0);' data-name='" + dom[i].NodeValue + "' data-index='" + dom[i].IndexCode + "' onclick='Video_HikJS.PlayVideo(this)'><div class='monitor-icon" + (i + 1) + "'></div><p>" + dom[i].NodeValue + "</p></a></li>"

                if (i % 3 == 2 || (i + 1) == dom.length)
                {
                    Html += "</ul>";
                }
            }
        }

        $("#VideoHtmlTemplate").html(Html);
    }


    //环境监测趋势图
    var ShowEChart = function () {
        //alert(111);
        $("#EChartTitle").html('环境监测趋势');

        HistoryEChartQuery()

        popupOpen();
        $('#EchartPopUp').slideDown();
    }



    var HistoryEChartQuery = function () {
        
        //打开遮罩层
        OpenMaskPopUp();
        $.post('/Main/Environment/EnvironmentEchartQuery', param)
            .success(function (data, statusText, jqXRH) {
                if (data) {
                    if (data.Success) {
                        myOption = data.Data;

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
                //Pagination();
                CloseMaskPopUp();
            });
    }

    var ChooseTheme = function (dom) {

        myChart.dispose();

        var theme = $(dom).val();

        myChart = echarts.init(document.getElementById('EChartContent'), theme);

        myChart.setOption(myOption);

    }

    



    //下载中心
    var DownloadQuery = function () {
        $.post('/Main/HomePage/DownloadList', '')
            .success(function (data, statusText, jqXRH) {
                if (data) {
                    if (data.Success) {
                        var Model = {};
                        Model.list = data.Data;
                        var html = template("DownloadTemplate", Model);
                        $("#DownloadRemplateHtml").html(html);
                    }
                }
            })
            .fail(function (jqXHR, statusText, errorMessage) {
                //console.error(arguments);
            })
            .always(function (data) {
            });
    }

    var DownloadFile = function (dom) {
        var FileID = $(dom).attr('data-id');
        if (FileID == null || FileID == '')
            return;

        window.location.href = "/Main/HomePage/FileDownload?FileID=" + FileID;
    }

    return {
        HomeSiteInit: function () {

        },
        DownloadFile: DownloadFile,
        ShowEChart: ShowEChart,
        field: {

        }
    }

}(jQuery);
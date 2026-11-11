var PositionJS = function ($) {

    var Param = {};

    var myOption = {};

    $(document).ready(function () {

        //Init();

        //ParamInit();

        //AutoQuery();

    })

    var ParamInit = function () {

        Param.PositionID = $("select#select_AutoPosition").val();
    }

    var Init = function () {

        $("#ThemeChoose").change(function () {

            //myChart.dispose();

            //var theme = $(this).val();

            //myChart = echarts.init(document.getElementById('EChartContent'), theme);

            //myChart.setOption(myOption);
        })
    }






    var ChooseTheme = function (dom) {

        //myChart.dispose();

        //var theme = $(dom).val();

        //myChart = echarts.init(document.getElementById('EChartContent'), theme);

        //myChart.setOption(myOption);

    }

    var SelectChange = function () {

        // var option = '<option value="0">请选择</option>';

        //if ($("#select_AutoLayer").val() == '0') {
        //    $("#select_AutoPosition").val('0');
        //    $("#select_AutoPosition").html(option);
        //    $("#select_AutoDevice").val('0');
        //    $("#select_AutoDevice").html(option);
        //    $("#select_AutoPoint").val('0');
        //    $("#select_AutoPoint").html('<option value="0">全部</option>');
        //}


        //  if (type == 'layer') {
        PositionQuery();
        //  }


    }

    var PositionQuery = function () {
        Param.PositionID = $("#select_AutoPosition").val();

        if (Param.PositionID == '0') {
            return;
        }

        $.post('PointQuery', Param)
            .success(function (data, statusText, jqXRH) {
                if (data) {
                    if (data.Success) {
                        var html = '';
                        $("#imglist li").remove();
                        $(data.Data).each(function (n, d) {
                            html = '<li style="background-image: url(/Content/IOT/img/' + d.ImagePath + '); left: ' + d.MapXs + '; top: ' + d.MapYs + ';"><a title="' + d.PositionName + '" href="' + d.Url + '" style="display:block;width:22px;height:22px;"></a></li>';
                            //   html += '<option value="' + d.ID + '">' + d.PositionName + '</option>';
                          //  html = '<li><a style="top: ' + d.MapYs + '; left: ' + d.MapXs + ';" title="' + d.PointName + '" href="/Scan/Construction/CalIndex?ID='+d.ID+'">' + d.PointName + '</a></li>';
                            $("#imglist").append(html);
                        })

                    }
                }
            })
            .fail(function (jqXHR, statusText, errorMessage) {
            })
            .always(function (data) {
                // 关闭等待遮罩
                //CloseMaskPopUp();
            });

        $.post('PositionImgQuery', Param)
            .success(function (data, statusText, jqXRH) {
                if (data) {
                    if (data.Success) {

                        var positionInfo = data.Data;
                        //信息绑定
                        $("#imgback").prop("src", '/Upload/' + positionInfo.BackGroundPath);
                    }
                }
            })
            .fail(function (jqXHR, statusText, errorMessage) {
                //console.error(arguments);
            })
            .always(function (data) {
                // 关闭等待遮罩
                //   CloseMaskPopUp();
            });
    }



    return {
        HomeSiteInit: function () {
            Init();
        },
        //  DoAutoQuery: DoAutoQuery,
        //   Export: Export,
        ChooseTheme: ChooseTheme,
        SelectChange: SelectChange,
        field: {

        }
    }

}(jQuery);
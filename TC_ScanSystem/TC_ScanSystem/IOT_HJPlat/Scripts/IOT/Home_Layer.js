var LayerJS = function ($) {

    var Param = {};

    var myOption = {};

    $(document).ready(function () {

        //Init();

        //ParamInit();

        //AutoQuery();

    })

    var ParamInit = function () {

        Param.LayerID = $("select#select_AutoLayer").val();
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
        Param.LayerID = $("#select_AutoLayer").val();

        if (Param.LayerID == '0') {
            return;
        }

        $.post('PositionQuery', Param)
            .success(function (data, statusText, jqXRH) {
                if (data) {
                    if (data.Success) {
                        var html = '';
                        $("#imglist a").remove();
                        $(data.Data).each(function (n, d) {
                            html = ' <a href="/Scan/HomePage/Position?ID='+d.ID+'&LayerId='+d.LayerId+'" title="'+d.PositionName+'" style="top:'+d.MapYs+'; left:'+d.MapXs+'; width:'+d.Widths+'; height:'+d.Heights+';"></a>';
                            //   html += '<option value="' + d.ID + '">' + d.PositionName + '</option>';
                          //  html = '<li><a style="top: ' + d.MapYs + '; left: ' + d.MapXs + ';" title="' + d.PositionName + '" href="/Scan/HomePage/Position?ID='+d.ID+'&LayerId='+d.LayerId+'">' + d.PositionName + '</a></li>';
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

        $.post('LayerImgQuery', Param)
            .success(function (data, statusText, jqXRH) {
                if (data) {
                    if (data.Success) {

                        var layereInfo = data.Data;
                        //信息绑定
                        $("#imgback").prop("src", '/Upload/'+layereInfo.BackgroundPath);
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
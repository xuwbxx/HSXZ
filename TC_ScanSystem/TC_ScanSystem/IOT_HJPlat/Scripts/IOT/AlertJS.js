var AlertJS = function ($) {

    var Param = {};



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
        Param.DeviceType = 0;

        $("select[name='select_AutoLayer'] option[value='" + $("#select_layer").val() + "']").prop('selected', true);
        Param.Begin = $("input[name='entryDateBegin']").val();
        Param.End = $("input[name='entryDateEnd']").val();
        Param.PointID = $("#select_AutoPoint").val();
        Param.PositionID = $("select#select_AutoPosition").val();
        // Param.DeviceID = $("select#select_AutoDevice").val();
        Param.LayerID = $("#select_AutoLayer").val();
        Param.ScanTypeID = $("#select_type").val();
    }

    var Init = function () {

    }

    var AutoQuery = function () {

        //if ($("select#select_AutoLayer").val() == '0' || $("select#select_AutoScanType").val() == '0' || $("select#select_AutoPosition").val() == '0' || $("select#select_AutoDevice").val() == '0')
        //{
        //    return;
        //}

        //打开遮罩层
        OpenMaskLayout();

        $.post('AlertQuery', Param)
                .success(function (data, statusText, jqXRH) {
                    if (data) {

                        if (data.Success) {

                            var Model = {};
                            Model.list = data.Data;
                            var html = template("AlertTemplate", Model);
                            $("#AlertHtmlTemplate").html(html);

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

    }

    var AutoLoadQuery = function () {

        //if ($("select#select_AutoLayer").val() == '0' || $("select#select_AutoScanType").val() == '0' || $("select#select_AutoPosition").val() == '0' || $("select#select_AutoDevice").val() == '0')
        //{
        //    return;
        //}

        //打开遮罩层
        //  OpenMaskLayout();

        $.post('AlertLoadQuery', Param)
                .success(function (data, statusText, jqXRH) {
                    if (data) {

                        if (data.Success) {
                            var Model = {};
                            Model.list = data.Data;
                            var html = template("AlertTemplate", Model);
                            $("#AlertHtmlTemplate").html(html);

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

    }

    var DoAutoQuery = function () {

        Param.Begin = $("input[name='entryDateBegin']").val();
        Param.End = $("input[name='entryDateEnd']").val();
        Param.PointID = $("#select_AutoPoint").val();
        Param.PositionID = $("select#select_AutoPosition").val();
        //Param.DeviceID = $("select#select_AutoDevice").val();
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



    var SelectChange = function (type) {

        var option = '<option value="0">请选择</option>';

        if ($("#select_AutoLayer").val() == '0') {
            $("#select_AutoPosition").val('0');
            $("#select_AutoPosition").html(option);
            //$("#select_AutoDevice").val('0');
            //$("#select_AutoDevice").html(option);
            $("#select_AutoPoint").val('0');
            //  $("#select_AutoPoint").html('<option value="0">全部</option>');
        }
        if ($("#select_AutoScanType").val() == '0') {
            $("#select_AutoPosition").val('0');
            $("#select_AutoPosition").html(option);
            $("#select_AutoPoint").val('0');
            // $("#select_AutoPoint").html('<option value="0">全部</option>');
        }
        if ($("#select_AutoPosition").val() == '0') {
            $("#select_AutoPoint").val('0');
            // $("#select_AutoPoint").html('<option value="0">全部</option>');
        }
        if ($("#select_AutoDevice").val() == '0') {
            $("#select_AutoPoint").val('0');
            //  $("#select_AutoPoint").html('<option value="0">全部</option>');
        }

        if (type == 'layer') {
            PositionQuery();
            //  DeviceQuery();
            PointQuery();
        }
        else if (type == 'scantype') {
            PositionQuery();
            // DeviceQuery();
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



    var PointQuery = function () {
        Param.PositionID = $("#select_AutoPosition").val();
        //   Param.DeviceID = $("#select_AutoDevice").val();

        if (Param.PositionID == '0') {
            return;
        }
        $.post('PointIndexQuery', Param)
            .success(function (data, statusText, jqXRH) {
                if (data) {
                    if (data.Success) {
                        var html = '<option value="0">请选择</option>';
                        $(data.Data).each(function (n, d) {
                
                           // html += '<li><input type="checkbox" value="' + d.ID + '" />' + d.PointName + '</li>';
                           html += '<option value="' + d.ID + '">' + d.PointName + '</option>';
                        })
                       $("#select_AutoPoint").html(html);
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

    //关闭窗体并刷新
    var CloseModalEvent = function () {

        TurnBack();
        //   LayerQuery();
    }

    var TurnBack = function () {
        var Point = '';
       // $("ul[name='UL_PointPop'] input[type='checkbox']:checked").each(function (index, dom) {
        //    Point += $(dom).val() + ',';
        //})
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
        // Export: Export,
        //  ChooseTheme: ChooseTheme,
        SelectChange: SelectChange,
        field: {

        }
    }

}(jQuery);
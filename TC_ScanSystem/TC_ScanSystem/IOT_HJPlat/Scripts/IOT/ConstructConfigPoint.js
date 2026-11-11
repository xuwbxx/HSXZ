var ConstructConfigPointJS = function ($) {

    var Param = {};

    $(document).ready(function () {
        //参数
        ParamInit();

        //列表数据绑定
        PointQuery();

    })


    var ParamInit = function () {

        Param.PageIndex = 1;
        Param.PageSize = 20;
        Param.TotalPage = 0;
        Param.TotalCount = 0;

        Param.PositionID = 0;
        Param.DeviceID = 0;
    }

    var PointQuery = function () {

        //打开遮罩层
        OpenMaskLayout();

        $.post('PointQuery', Param)
            .success(function (data, statusText, jqXRH) {
                if (data) {
                    if (data.Success) {
                        var Model = {};
                        Model.list = data.Data;
                        var html = template("PointTemplate", Model);
                        $("#PointHtmlTemplate").html(html);

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

    //查询列表数据
    var DoPointQuery = function () {
        Param.PositionID = $("#PointSelectPosition").val();
        Param.DeviceID = $("#PointSelectDevice").val();
        Param.ScanTypeID = $("#SelScanTypePoint").val();
        PointQuery();
    }
    //分页
    var Pagination = function () {
        $("#PointPagination").pagination({
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
                DoPointQuery();
            }
        });
    }

    //打开用户信息弹出窗体
    var ShowPointInfo = function (dom) {
        var ID = $(dom).attr('data-id');
        if (ID == null || ID == '') {
            return;
        }
        //$("p[name='UserInfoTitle']").html('用户信息更改');
        $("input[name='PointType']").val('Modify');

        DataInit();

        var Param = {};
        Param.ID = ID;

        OpenMaskPopUp();

        $.post('PointInfoQuery', Param)
            .success(function (data, statusText, jqXRH) {
                if (data) {
                    if (data.Success) {

                        var PointInfo = data.Data;
                        //信息绑定
                        $("input[name='PointID']").val(PointInfo.ID);
                        $("input[name='PointName']").val(PointInfo.PointName);
                        $("input[name='pName']").val(PointInfo.pName);
                        $("input[name='PointMapX']").val(PointInfo.MapX);
                        $("input[name='PointMapY']").val(PointInfo.MapY);
                        $("input[name='PointMapDataName']").val(PointInfo.MapDataName);
                        $("input[name='PointMapDataName2']").val(PointInfo.MapDataName2);
                        $("input[name='PointUnit']").val(PointInfo.Unit);
                        $("input[name='PointUnit2']").val(PointInfo.Unit2);
                        $("input[name='PointMaxNum']").val(PointInfo.MaxNum);
                        $("input[name='PointMaxNum2']").val(PointInfo.MaxNum2);
                        $("input[name='PointMaxNum3']").val(PointInfo.MaxNum3);
                        $("input[name='PointMinNum']").val(PointInfo.MinNum);
                        $("input[name='PointMinNum2']").val(PointInfo.MinNum2);
                        $("input[name='PointMinNum3']").val(PointInfo.MinNum3);
                        $("input[name='PointHeight']").val(PointInfo.Height);
                        $("input[name='testHeight']").val(PointInfo.testHeight);
                       // $("input[name='SensorNumber']").val(PointInfo.SensorNumber);
                        $("input[name='para1']").val(PointInfo.Para1);
                        $("input[name='para2']").val(PointInfo.Para2);
                        $("input[name='para3']").val(PointInfo.Para3);
                        $("input[name='para4']").val(PointInfo.Para4);
                        $("input[name='para5']").val(PointInfo.Para5);
                        $("input[name='para6']").val(PointInfo.Para6);
                        $("input[name='para7']").val(PointInfo.Para7);
                        //下拉绑定
                        $("select[name='PointPositionSelect'] option[value='" + PointInfo.PositionId + "']").prop('selected', true);
                        $("select[name='PointDeviceSelect'] option[value='" + PointInfo.DeviceId + "']").prop('selected', true);
                        $("select[name='PointSensorSelect'] option[value='" + PointInfo.SensorType + "']").prop('selected', true);
                      //  $("select[name='PointSelectScanType'] option[value='" + PointInfo.ScanTypeId + "']").prop('selected', true);
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

    var DataInit = function () {

        $("select[name='PointPositionSelect'] option:selected").each(function (index, dom) {
            $(dom).prop('selected', false);
        });
        $("select[name='PointDeviceSelect'] option:selected").each(function (index, dom) {
            $(dom).prop('selected', false);
        });
        $("select[name='PositionSelectScanType'] option:selected").each(function (index, dom) {
            $(dom).prop('selected', false);
        });
        $("select[name='PointSensorSelect'] option:selected").each(function (index, dom) {
            $(dom).prop('selected', false);
        });

    }


    //打开模态框
    var ShowModal = function () {

        OpenSaveModal();

        $("#ModalSubmit").bind('click', function () {
            SavePoint();
        })
        $("#myModal").modal('show');
    }

    //保存
    var SavePoint = function () {

        var PointInfo = {};
        PointInfo.Type = $("input[name='PointType']").val();

        PointInfo.ID = $("input[name='PointID']").val();
        PointInfo.PointName = $("input[name='PointName']").val();
        PointInfo.pName = $("input[name='pName']").val();
        PointInfo.MapX = $("input[name='PointMapX']").val();
        PointInfo.MapY = $("input[name='PointMapY']").val();
        PointInfo.MapDataName = $("input[name='PointMapDataName']").val();
        PointInfo.MapDataName2 = $("input[name='PointMapDataName2']").val();
        PointInfo.Unit = $("input[name='PointUnit']").val();
        PointInfo.Unit2 = $("input[name='PointUnit2']").val();
        PointInfo.MaxNum = $("input[name='PointMaxNum']").val();
        PointInfo.MaxNum2 = $("input[name='PointMaxNum2']").val();
        PointInfo.MaxNum3 = $("input[name='PointMaxNum3']").val();
        PointInfo.MinNum = $("input[name='PointMinNum']").val();
        PointInfo.MinNum2 = $("input[name='PointMinNum2']").val();
        PointInfo.MinNum3 = $("input[name='PointMinNum3']").val();
        PointInfo.Height = $("input[name='PointHeight']").val();
        PointInfo.testHeight=$("input[name='testHeight']").val();
       // PointInfo.SensorNumber = $("input[name='SensorNumber']").val();
        PointInfo.Para1 = $("input[name='para1']").val();
        PointInfo.Para2 = $("input[name='para2']").val();
        PointInfo.Para3 = $("input[name='para3']").val();
        PointInfo.Para4 = $("input[name='para4']").val();
        PointInfo.Para5 = $("input[name='para5']").val();
        PointInfo.Para6 = $("input[name='para6']").val();
        PointInfo.Para7 = $("input[name='para7']").val();

        PointInfo.PositionId = $("select[name='PointPositionSelect']").val();
        PointInfo.DeviceId = $("select[name='PointDeviceSelect']").val();
        PointInfo.SensorType = $("select[name='PointSensorSelect']").val();
       // PointInfo.ScanTypeId = $("select[name='PointSelectScanType']").val();

        OpenMaskLayout();

        $.post('SavePoint', PointInfo)
            .success(function (data, statusText, jqXRH) {

                if (data) {
                    //$('#myModal').modal('hide');

                    if (data.Success) {
                        OpenModalSuccessTips();
                        $("#myModal").modal('show');

                        $("#ModalClose").bind('click', function () {
                            CloseModalEvent();
                        })
                    }
                    else {
                        OpenModalContentTips('失败', data.Message, 'fail');
                        $("#myModal").modal('show');
                    }


                }
            })
            .fail(function (jqXHR, statusText, errorMessage) {
                //console.error(arguments);
            })
            .always(function (data) {
                // 关闭等待遮罩
                CloseMaskLayout();
            });
    }

    //关闭窗体并刷新
    var CloseModalEvent = function () {

        TurnBack();
        PointQuery();
    }

    var TurnBack = function () {
        //$('.popup-max').css('display', 'none');
        $('.popup-close').click();
    }

    //打开用户信息弹出框
    var AddPoint = function () {

        $("input[name='PointName']").val('');
        $("input[name='pName']").val('');
        $("input[name='PointMapX']").val('');
        $("input[name='PointMapY']").val('');
        $("input[name='PointMapDataName']").val('');
        $("input[name='PointMapDataName2']").val('');
        $("input[name='PointUnit']").val('');
        $("input[name='PointUnit2']").val('');
        $("input[name='PointMaxNum']").val('');
        $("input[name='PointMaxNum2']").val('');
        $("input[name='PointMaxNum3']").val('');
        $("input[name='PointMinNum']").val('');
        $("input[name='PointMinNum2']").val('');
        $("input[name='PointMinNum3']").val('');
        $("input[name='PointHeight']").val('');
        $("input[name='testHeight']").val('');
        //$("input[name='SensorNumber']").val('');
        $("input[name='PointType']").val('Add');
        $("input[name='para6']").val('');
        $("input[name='para5']").val('');
        $("input[name='para4']").val('');
        $("input[name='para3']").val('');
        $("input[name='para2']").val('');
        $("input[name='para1']").val('');
        $("input[name='para7']").val('');
        DataInit();

        popupOpen();
        $('#PointPopUp').slideDown();
    }

    var DeletePoint = function (dom) {

        var ID = $(dom).attr('data-id');
        if (ID == null || ID == '') {
            return;
        }

        var Param = {};
        Param.ID = ID;

        OpenDeleteModal();

        $("#ModalSubmit").bind('click', function () {

            DoDeletePoint(Param);
        })

        $("#myModal").modal('show');
    }

    var DoDeletePoint = function (dom) {

        $.post('DeletePoint', dom)
            .success(function (data, statusText, jqXRH) {

                if (data) {
                    //$('#myModal').modal('hide');

                    if (data.Success) {
                        OpenModalSuccessTips();
                        $("#myModal").modal('show');

                        $("#ModalClose").bind('click', function () {
                            CloseModalEvent();
                        })
                    }
                    else {
                        OpenModalContentTips('失败', data.Message, 'fail');
                        $("#myModal").modal('show');
                    }


                }
            })
            .fail(function (jqXHR, statusText, errorMessage) {
                //console.error(arguments);
            })
            .always(function (data) {
                // 关闭等待遮罩
                CloseMaskLayout();
            });
    }


    return {
        HomeSiteInit: function () {
            Init();
        },
        DoPointQuery: DoPointQuery,
        ShowPointInfo: ShowPointInfo,
        DeletePoint: DeletePoint,
        TurnBack: TurnBack,
        ShowModal: ShowModal,
        AddPoint: AddPoint,
        field: {

        }

    }

}(jQuery);
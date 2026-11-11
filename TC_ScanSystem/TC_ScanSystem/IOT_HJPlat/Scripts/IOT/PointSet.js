var PointSetJS = function ($) {

    var Param = {};

    var positionleft = '';
    var positiontop = '';

    var OriginX = '';
    var OriginY = '';
    var DivWidth = $('.yujing-bg').width();
    var DivHeight = $('.yujing-bg').height();
    var xLeft = '';
    var xTop = '';
    var _move = false;

    $(document).ready(function () {

        var Origin = $("div#div_Icon").offset();
        OriginX = Origin.left;
        OriginY = Origin.top;
        //参数
        ParamInit();

        //列表数据绑定
        PointQuery();

    })


    var ParamInit = function () {

        //Param.PageIndex = 1;
        //Param.PageSize = 20;
        //Param.TotalPage = 0;
        //Param.TotalCount = 0;

        Param.PositionID = 0;
        Param.DeviceID = 0;
        Param.ScanTypeID = 0;
    }

    var PointQuery = function () {

        //打开遮罩层
        OpenMaskLayout();

        $.post('PointSetQuery', Param)
            .success(function (data, statusText, jqXRH) {
                if (data) {
                    if (data.Success) {
                        var html = '';
                        $("#UL_Icon li").remove();
                        $(data.Data).each(function (n, d) {
                            //   html += '<option value="' + d.ID + '">' + d.PositionName + '</option>';
                            //html = '<li><a style="top: ' + d.MapYs + '; left: ' + d.MapXs + ';" title="' + d.PointName + '" href="#">' + d.PointName + '</a></li>';
                            html = "<li data-id='"+d.ID+"' ondblclick='PointSetJS.ShowPointInfo(this)' onmousedown='PointSetJS.IconEventMouseDown(this)' style='background-image:url(/Content/IOT/img/" + d.ImagePath + "); left: " + d.MapXs + "; top: " + d.MapYs + ";'></li>";
                            $("#UL_Icon").append(html);
                        })
                    }
                }
            })
            .fail(function (jqXHR, statusText, errorMessage) {
                //console.error(arguments);
            })
            .always(function (data) {
                // 关闭等待遮罩
             //   Pagination();

                CloseMaskLayout();
            });
    }

    //查询列表数据
    var DoPointQuery = function () {
        Param.PositionID = $("#PointSelectPosition").val();
        Param.LayerID = $("#PointSelectLayer").val();
     //   Param.DeviceID = $("#PointSelectDevice").val();
      //  Param.ScanTypeID = $("#SelScanTypePoint").val();
        PointQuery();
    }
    //分页
    //var Pagination = function () {
    //    $("#PointPagination").pagination({
    //        currentPage: Param.PageIndex,
    //        totalPage: Param.TotalPage,
    //        isShow: true,
    //        totalCount: Param.TotalCount,
    //        count: 7,
    //        homePageText: "首页",
    //        endPageText: "尾页",
    //        prevPageText: "上一页",
    //        nextPageText: "下一页",
    //        callback: function (current) {
    //            Param.PageIndex = current;
    //            DoPointQuery();
    //        }
    //    });
    //}

    //打开用户信息弹出窗体
    var ShowPointInfo = function (dom) {
        var ID = $(dom).attr('data-id');
        var x = $(dom).offset().left;
        var y = $(dom).offset().top;
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
                        if (xLeft == '')
                            $("input[name='x']").val(PointInfo.MapX);
                        if (xTop == '')
                            $("input[name='y']").val(PointInfo.MapY);
                        //$("input[name='x']").val(x);
                        //$("input[name='y']").val(y);
                        $("input[name='PointMapDataName']").val(PointInfo.MapDataName);
                        $("input[name='PointMapDataName2']").val(PointInfo.MapDataName2);
                        $("input[name='PointUnit']").val(PointInfo.Unit);
                        $("input[name='PointUnit2']").val(PointInfo.Unit2);
                        $("input[name='PointMaxNum']").val(PointInfo.MaxNum);
                        $("input[name='PointMaxNum2']").val(PointInfo.MaxNum2);
                        $("input[name='PointMinNum']").val(PointInfo.MinNum);
                        $("input[name='PointMinNum2']").val(PointInfo.MinNum2);
                        $("input[name='PointHeight']").val(PointInfo.Height);
                        $("input[name='testHeight']").val(PointInfo.testHeight);
                        $("input[name='SensorNumber']").val(PointInfo.SensorNumber);
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
        $("select[name='PointSensorSelect'] option:selected").each(function (index, dom) {
            $(dom).prop('selected', false);
        });
        //$("select[name='PositionSelectScanType'] option:selected").each(function (index, dom) {
        //    $(dom).prop('selected', false);
        //});


    }


    //打开模态框
    var ShowModal = function () {

        OpenSaveModal();

        $("#ModalSubmit").bind('click', function () {
            SavePoint();
        })
        $("#myModal").modal('show');
    }

    var SelectChange1 = function (type)
    {
        PositionSetQuery();
    }

    var PositionSetQuery = function () {
        Param.LayerID = $("#PointSelectLayer").val();

        if (Param.LayerID == '0') {
            return;
        }

        $.post('PositionQuery', Param)
            .success(function (data, statusText, jqXRH) {
                if (data) {
                    if (data.Success) {
                        var html = '<option value="0">请选择</option>';
                        $(data.Data).each(function (n, d) {
                            html += '<option value="' + d.ID + '">' + d.PositionName + '</option>';
                        })
                        $("#PointSelectPosition").html(html);
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
    var SelectChange2 = function () {

        // var option = '<option value="0">请选择</option>';

        //if ($("#select_AutoLayer").val() == '0') {
        //    $("#select_AutoPosition").val('0');
        //    $("#select_AutoPosition").html(option);
        //    $("#select_AutoDevice").val('0');
        //    $("#select_AutoDevice").html(option);
        //    $("#select_AutoPoint").val('0');
        //    $("#select_AutoPoint").html('<option value="0">全部</option>');
        //}
        Param.ScanTypeID = $("#SelScanTypePoint").val();
        LayerQuery();
        //  if (type == 'layer') {
        PositionQuery();
        //  }


    }

    var LayerQuery = function () {

        //打开遮罩层
        OpenMaskLayout();
      
        $.post('LayerSetQuery', Param)
            .success(function (data, statusText, jqXRH) {
                if (data) {
                    if (data.Success) {
                        var html = '';
                        var html = '<option value="0">请选择</option>';
                        $(data.Data).each(function (n, d) {
                            html += '<option value="' + d.ID + '">' + d.LayerName + '</option>';
                        })
                        $("#PointSelectLayer").html(html);
                    }
                }
            })
            .fail(function (jqXHR, statusText, errorMessage) {
                //console.error(arguments);
            })
            .always(function (data) {
                // 关闭等待遮罩
                //   Pagination();

                CloseMaskLayout();
            });
    }


    var PositionQuery = function (obj) {
        Param.PositionID = $("#PointSelectPosition").val();
      //  Param.ScanTypeID = $("#SelScanTypePoint").val();
        if (Param.PositionID == '0') {
            return;
        }

        $.post('PointSetQuery', Param)
            .success(function (data, statusText, jqXRH) {
                if (data) {
                    if (data.Success) {
                        var html = '';
                        $("#UL_Icon li").remove();
                        $(data.Data).each(function (n, d) {
                            //   html += '<option value="' + d.ID + '">' + d.PositionName + '</option>';
                            html = "<li data-id='" + d.ID + "' ondblclick='PointSetJS.ShowPointInfo(this)' onmousedown='PointSetJS.IconEventMouseDown(this)' style='background-image:url(/Content/IOT/img/" + d.ImagePath + "); left: " + d.MapXs + "; top: " + d.MapYs + ";'></li>";
                            $("#UL_Icon").append(html);
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

    var AddIcon = function () {
        //alert('23345');
        var SelectType = $("#SelScanTypePoint option:selected").attr('data-icon');
        //alert(SelectType);
       
        $("#UL_Icon").append("<li data-id='0' ondblclick='PointSetJS.AddPoint(this)' style='background-image:url(" + SelectType + ");' onmousedown='PointSetJS.IconEventMouseDown(this)'></li>");

    }

    var IconEventMouseDown = function (e) {

        _move = true;

        var Icon = e;

        $(document).mousemove(function (dom) {

            if (!_move) {
                return false;
            }

            var x = dom.pageX - OriginX - 10;
            var y = dom.pageY - OriginY - 10;

            DivWidth = $('.yujing-bg').width();
            DivHeight = $('.yujing-bg').height();

            if (x <= 0) {
                x = 0;
            }
            else if (x >= DivWidth) {
                x = DivWidth;
            }

            if (y <= 0) {
                y = 0;
            }
            else if (y >= DivHeight) {
                y = DivHeight;
            }

            xLeft = x / DivWidth;
            xTop = y / DivHeight;

            $("input[name='x']").val(xLeft * 100);
            $("input[name='y']").val(xTop * 100);

            $(Icon).css({
                'left': 100 * xLeft + '%',
                'top': 100 * xTop + '%'
            });

        }).mouseup(function () {
            _move = false;
            $(document).off('mousemove');
        })

        //$(e).mouseup(function () {
        //    $(e).off('mousemove');
        //});
        //return false;
    }

    //保存
    var SavePoint = function () {

        var PointInfo = {};
        PointInfo.Type = $("input[name='PointType']").val();

        PointInfo.ID = $("input[name='PointID']").val();
        PointInfo.PointName = $("input[name='PointName']").val();
        PointInfo.pName = $("input[name='pName']").val();
        //PointInfo.MapX = xLeft*100;
        //PointInfo.MapY = xTop*100;
        PointInfo.MapX = $("input[name='x']").val();
        PointInfo.MapY = $("input[name='y']").val();
        PointInfo.MapDataName = $("input[name='PointMapDataName']").val();
        PointInfo.MapDataName2 = $("input[name='PointMapDataName2']").val();
        PointInfo.Unit = $("input[name='PointUnit']").val();
        PointInfo.Unit2 = $("input[name='PointUnit2']").val();
        PointInfo.MaxNum = $("input[name='PointMaxNum']").val();
        PointInfo.MaxNum2 = $("input[name='PointMaxNum2']").val();
        PointInfo.MinNum = $("input[name='PointMinNum']").val();
        PointInfo.MinNum2 = $("input[name='PointMinNum2']").val();
        PointInfo.Height = $("input[name='PointHeight']").val();
        PointInfo.testHeight = $("input[name='testHeight']").val();
        PointInfo.SensorNumber = $("input[name='SensorNumber']").val();

        PointInfo.PositionId = $("select[name='PointPositionSelect']").val();
        PointInfo.DeviceId = $("select[name='PointDeviceSelect']").val();
        PointInfo.SensorType = $("select[name='PointSensorSelect']").val();
        //PointInfo.ScanTypeId = $("select[name='PointSelectScanType']").val();

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
        //$("input[name='PointMapX']").val('');
        //$("input[name='PointMapY']").val('');
        $("input[name='PointMapDataName']").val('');
        $("input[name='PointMapDataName2']").val('');
        $("input[name='PointUnit']").val('');
        $("input[name='PointUnit2']").val('');
        $("input[name='PointMaxNum']").val('');
        $("input[name='PointMaxNum2']").val('');
        $("input[name='PointMinNum']").val('');
        $("input[name='PointMinNum2']").val('');
        $("input[name='PointHeight']").val('');
        $("input[name='testHeight']").val('');
        $("input[name='SensorNumber']").val('');
       // $("select[name='PointPositionSelect'] option[value='" + $("select[name = 'PointSelectPosition']").val() + "']").prop('selected', true);
        //$("select[name='PointSelectScanType'] option[value='" + $("select[name = 'SelScanTypePoint']").val() + "']").prop('selected', true);
        $("input[name='PointType']").val('Add');
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
        SelectChange: SelectChange,
        SelectChange1: SelectChange1,
        ShowPointInfo: ShowPointInfo,
        SelectChange2:SelectChange2,
        DeletePoint: DeletePoint,
        TurnBack: TurnBack,
        ShowModal: ShowModal,
        AddPoint: AddPoint,
        IconEventMouseDown:IconEventMouseDown,
        AddIcon:AddIcon,
        field: {

        }

    }

}(jQuery);
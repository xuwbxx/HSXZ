var LayerSetJS = function ($) {

    var Param = {};

    var positionleft = '';
    var positiontop = '';
    var tempID = 0;
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
        LayerQuery();

    })


    var ParamInit = function () {

        //Param.PageIndex = 1;
        //Param.PageSize = 20;
        //Param.TotalPage = 0;
        //Param.TotalCount = 0;

        //Param.PositionID = 0;
        //Param.DeviceID = 0;
        Param.ScanTypeID = 0;
    }

    var LayerQuery = function () {

        //打开遮罩层
        OpenMaskLayout();
        //Param.ScanTypeID=
        $.post('LayerSetQuery', Param)
            .success(function (data, statusText, jqXRH) {
                if (data) {
                    if (data.Success) {
                        var html = '';
                        $("#UL_Icon li").remove();
                        $(data.Data).each(function (n, d) {
                            //   html += '<option value="' + d.ID + '">' + d.PositionName + '</option>';
                            //html = '<li><a style="top: ' + d.MapYs + '; left: ' + d.MapXs + ';" title="' + d.PointName + '" href="#">' + d.PointName + '</a></li>';
                            html = "<li data-id='" + d.ID + "' ondblclick='LayerSetJS.ShowLayerInfo(this)' onmousedown='LayerSetJS.IconEventMouseDown(this)' style='background-image:url(/Content/IOT/img/point1.png); left: " + d.MapXs + "; top: " + d.MapYs + ";'></li>";
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
    var DoLayerQuery = function () {
        //Param.PositionID = $("#PointSelectPosition").val();
        //Param.LayerID = $("#PointSelectLayer").val();
        ////   Param.DeviceID = $("#PointSelectDevice").val();
        Param.ScanTypeID = $("#SelLayerProj").val();
        LayerQuery();
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
    var ShowLayerInfo = function (dom) {
        var ID = $(dom).attr('data-id');
        var x = $(dom).offset().left;
        var y = $(dom).offset().top;

        if (ID == null || ID == '') {
            return;
        }
        //$("p[name='UserInfoTitle']").html('用户信息更改');
        $("input[name='TypeLayer']").val('Modify');

        DataInit();

        var Param = {};
        Param.LayerID = ID;

        OpenMaskPopUp();

        $.post('LayerInfoQuery', Param)
            .success(function (data, statusText, jqXRH) {
                if (data) {
                    if (data.Success) {

                        var LayerInfo = data.Data;
                        //信息绑定
                        $("input[name='LayerIdx']").val(LayerInfo.ID);
                        $("input[name='LayerName']").val(LayerInfo.LayerName);
                        if(xLeft=='')
                            $("input[name='x']").val(LayerInfo.MapX);
                        if(xTop=='')
                        $("input[name='y']").val(LayerInfo.MapY);

                        
                        //$("input[name='LayerMapX']").val(LayerInfo.MapX);
                        //$("input[name='LayerMapY']").val(LayerInfo.MapY);
                        $("input[name='DescriptionLayer']").val(LayerInfo.Description);
                        //下拉绑定
                        $("select[name='LayerSelect'] option[value='" + LayerInfo.ProjectId + "']").prop('selected', true);
                        $("select[name='TypeSelect'] option[value='" + LayerInfo.ScanTypeId + "']").prop('selected', true);
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

        $('#LayerSetPopUp').slideDown();

    }

    var DataInit = function () {

        //$("select[name='PointPositionSelect'] option:selected").each(function (index, dom) {
        //    $(dom).prop('selected', false);
        //});
        //$("select[name='PointDeviceSelect'] option:selected").each(function (index, dom) {
        //    $(dom).prop('selected', false);
        //});
        $("select[name='LayerSelect'] option:selected").each(function (index, dom) {
            $(dom).prop('selected', false);
        });
        var ScanType = $("select[name='SelLayerProj']").val();
        $("select[name='TypeSelect'] option[value='" + ScanType + "']").prop('selected', true);
    }


    //打开模态框
    var ShowModal = function () {

        OpenSaveModal();

        $("#ModalSubmit").bind('click', function () {
            SaveLayer();
        })
        $("#myModal").modal('show');
    }

    var SelectChange1 = function (type) {
        DoLayerQuery();
    }

    //var PositionSetQuery = function () {
    //    Param.LayerID = $("#PointSelectLayer").val();

    //    if (Param.LayerID == '0') {
    //        return;
    //    }

    //    $.post('PositionQuery', Param)
    //        .success(function (data, statusText, jqXRH) {
    //            if (data) {
    //                if (data.Success) {
    //                    var html = '<option value="0">请选择</option>';
    //                    $(data.Data).each(function (n, d) {
    //                        html += '<option value="' + d.ID + '">' + d.PositionName + '</option>';
    //                    })
    //                    $("#PointSelectPosition").html(html);
    //                }
    //            }
    //        })
    //        .fail(function (jqXHR, statusText, errorMessage) {
    //        })
    //        .always(function (data) {
    //            // 关闭等待遮罩
    //            //CloseMaskPopUp();
    //        });
    //}

    //var SelectChange = function () {

    //    // var option = '<option value="0">请选择</option>';

    //    //if ($("#select_AutoLayer").val() == '0') {
    //    //    $("#select_AutoPosition").val('0');
    //    //    $("#select_AutoPosition").html(option);
    //    //    $("#select_AutoDevice").val('0');
    //    //    $("#select_AutoDevice").html(option);
    //    //    $("#select_AutoPoint").val('0');
    //    //    $("#select_AutoPoint").html('<option value="0">全部</option>');
    //    //}


    //    //  if (type == 'layer') {
    //    PositionQuery();
    //    //  }


    //}

    //var PositionQuery = function (obj) {
    //    Param.PositionID = $("#PointSelectPosition").val();
    //    Param.ScanTypeID = $("#SelScanTypePoint").val();
    //    if (Param.PositionID == '0') {
    //        return;
    //    }

    //    $.post('PointSetQuery', Param)
    //        .success(function (data, statusText, jqXRH) {
    //            if (data) {
    //                if (data.Success) {
    //                    var html = '';
    //                    $("#UL_Icon li").remove();
    //                    $(data.Data).each(function (n, d) {
    //                        //   html += '<option value="' + d.ID + '">' + d.PositionName + '</option>';
    //                        html = "<li data-id='" + d.ID + "' ondblclick='PointSetJS.ShowPointInfo(this)' onmousedown='PointSetJS.IconEventMouseDown(this)' style='background-image:url(/Content/IOT/img/" + d.ImagePath + "); left: " + d.MapXs + "; top: " + d.MapYs + ";'></li>";
    //                        $("#UL_Icon").append(html);
    //                    })

    //                }
    //            }
    //        })
    //        .fail(function (jqXHR, statusText, errorMessage) {
    //        })
    //        .always(function (data) {
    //            // 关闭等待遮罩
    //            //CloseMaskPopUp();
    //        });

    //    $.post('PositionImgQuery', Param)
    //        .success(function (data, statusText, jqXRH) {
    //            if (data) {
    //                if (data.Success) {

    //                    var positionInfo = data.Data;
    //                    //信息绑定
    //                    $("#imgback").prop("src", '/Upload/' + positionInfo.BackGroundPath);
    //                }
    //            }
    //        })
    //        .fail(function (jqXHR, statusText, errorMessage) {
    //            //console.error(arguments);
    //        })
    //        .always(function (data) {
    //            // 关闭等待遮罩
    //            //   CloseMaskPopUp();
    //        });
    //}

    var AddIcon = function () {
        //alert('23345');
       // var SelectType = $("#SelScanTypePoint option:selected").attr('data-icon');
        //alert(SelectType);

        $("#UL_Icon").append("<li data-id='0' ondblclick='LayerSetJS.AddLayer(this)' style='background-image:url(/Content/IOT/img/point1.png);' onmousedown='LayerSetJS.IconEventMouseDown(this)'></li>");

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

            $("input[name='x']").val(xLeft*100);
            $("input[name='y']").val(xTop*100);

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
    var SaveLayer = function () {

        var LayerInfo = {};
        LayerInfo.Type = $("input[name='TypeLayer']").val();

        LayerInfo.ID = $("input[name='LayerIdx']").val();
        LayerInfo.LayerName = $("input[name='LayerName']").val();
        LayerInfo.MapX = $("input[name='x']").val();
        LayerInfo.MapY = $("input[name='y']").val();
        LayerInfo.Description = $("input[name='DescriptionLayer']").val();
        LayerInfo.ProjectID = $("select[name='LayerSelect']").val();
        LayerInfo.ScanTypeID = $("select[name='TypeSelect']").val();
        OpenMaskLayout();
        $.post('SaveLayer', LayerInfo)
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
        LayerQuery();
    }

    var TurnBack = function () {
        //$('.popup-max').css('display', 'none');
        $('.popup-close').click();
    }

    //打开用户信息弹出框
    var AddLayer = function () {

        $("input[name='LayerName']").val('');
        $("input[name='DescriptionLayer']").val('');
        //$("input[name='LayerMapX']").val('');
        //$("input[name='LayerMapY']").val('');
        $("input[name='TypeLayer']").val('Add');
        DataInit();

        popupOpen();
        $('#LayerSetPopUp').slideDown();
    }

    var DeleteLayer = function (dom) {

        var ID = $(dom).attr('data-id');
        if (ID == null || ID == '') {
            return;
        }

        var Param = {};
        Param.ID = ID;

        OpenDeleteModal();

        $("#ModalSubmit").bind('click', function () {

            DoDeleteLayer(Param);
        })

        $("#myModal").modal('show');
    }

    var DoDeleteLayer = function (dom) {

        $.post('DeleteLayer', dom)
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
        DoLayerQuery: DoLayerQuery,
        //SelectChange: SelectChange,
        SelectChange1: SelectChange1,
        ShowLayerInfo: ShowLayerInfo,
        DeleteLayer: DeleteLayer,
        TurnBack: TurnBack,
        ShowModal: ShowModal,
        AddLayer: AddLayer,
        IconEventMouseDown: IconEventMouseDown,
        AddIcon: AddIcon,
        field: {

        }

    }

}(jQuery);
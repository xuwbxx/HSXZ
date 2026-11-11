var PositionSetJS = function ($) {

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
        PositionQuery();

    })


    var ParamInit = function () {

        //Param.PageIndex = 1;
        //Param.PageSize = 20;
        //Param.TotalPage = 0;
        //Param.TotalCount = 0;

        //Param.PositionID = 0;
        //Param.DeviceID = 0;
        Param.LayerID = 0;
        Param.ID=0;
    }

    var PositionQuery = function () {

        //打开遮罩层
       // OpenMaskLayout();
        
        //Param.ScanTypeID=
        $.post('PositionSetQuery', Param)
            .success(function (data, statusText, jqXRH) {
                if (data) {
                    if (data.Success) {
                        var html = '';
                        $("#UL_Icon li").remove();
                        $(data.Data).each(function (n, d) {
                            //   html += '<option value="' + d.ID + '">' + d.PositionName + '</option>';
                            //html = '<li><a style="top: ' + d.MapYs + '; left: ' + d.MapXs + ';" title="' + d.PointName + '" href="#">' + d.PointName + '</a></li>';
                            html = "<li data-id='" + d.ID + "' ondblclick='PositionSetJS.ShowPositionInfo(this)' onmousedown='PositionSetJS.IconEventMouseDown(this)' style='background-image:url(/Content/IOT/img/dianji-4.png); left: " + d.MapXs + "; top: " + d.MapYs + ";'></li>";
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

               // CloseMaskLayout();
            });
        $.post('LayerInfoQuery', Param)
            .success(function (data, statusText, jqXRH) {
                if (data) {
                    if (data.Success) {

                        var positionInfo = data.Data;
                        //信息绑定
                        $("#imgback").prop("src", '/Upload/' + positionInfo.BackgroundPath);
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

    //查询列表数据
    var DoPositionQuery = function () {
        //Param.PositionID = $("#PointSelectPosition").val();
        //Param.LayerID = $("#PointSelectLayer").val();
        ////   Param.DeviceID = $("#PointSelectDevice").val();
        Param.LayerID = $("#SelLayerProj").val();
        Param.ID = $("#SelLayerProj").val();
        PositionQuery();
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
    var ShowPositionInfo = function (dom) {
        var ID = $(dom).attr('data-id');
        var x = $(dom).offset().left;
        var y = $(dom).offset().top;
        if (ID == null || ID == '') {
            return;
        }
        //$("p[name='UserInfoTitle']").html('用户信息更改');
        $("input[name='PositionType']").val('Modify');

        DataInit();

        var Param = {};
        Param.PositionID = ID;
        Param.ID = ID;

        OpenMaskPopUp();

        $.post('PositionInfoQuery', Param)
            .success(function (data, statusText, jqXRH) {
                if (data) {
                    if (data.Success) {

                        var LayerInfo = data.Data;
                        //信息绑定
                        $("input[name='PositionID']").val(LayerInfo.ID);
                        $("input[name='PositionName']").val(LayerInfo.PositionName);
                        $("input[name='PileDiameter']").val(LayerInfo.PileDiameter);
                        $("input[name='PileLength']").val(LayerInfo.PileLength);
                        $("input[name='PileNumber']").val(LayerInfo.PileNumber);
                        if (xLeft == '')
                            $("input[name='x']").val(LayerInfo.MapX);
                        if (xTop == '')
                            $("input[name='y']").val(LayerInfo.MapY);
                        //$("input[name='LayerMapX']").val(LayerInfo.MapX);
                        //$("input[name='LayerMapY']").val(LayerInfo.MapY);
                        //$("input[name='DescriptionLayer']").val(LayerInfo.Description);
                        //下拉绑定
                        $("select[name='PositionSelectLayer'] option[value='" + LayerInfo.LayerId + "']").prop('selected', true);
                        
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

        $('#PositionSetPopUp').slideDown();

    }

    var DataInit = function () {

        //$("select[name='PointPositionSelect'] option:selected").each(function (index, dom) {
        //    $(dom).prop('selected', false);
        //});
        //$("select[name='PointDeviceSelect'] option:selected").each(function (index, dom) {
        //    $(dom).prop('selected', false);
        //});
       
        var LayerId = $("select[name='SelLayerProj']").val();
        $("select[name='PositionSelectLayer'] option[value='" + LayerId + "']").prop('selected', true);
    }


    //打开模态框
    var ShowModal = function () {

        OpenSaveModal();

        $("#ModalSubmit").bind('click', function () {
            SavePosition();
        })
        $("#myModal").modal('show');
    }

    var SelectChange1 = function (type) {
        DoPositionQuery();
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


    var SelectChange = function () {

         var option = '<option value="0">请选择</option>';

         if ($("#SelLayerType").val() == '0') {
             $("#SelLayerProj").val('0');
             $("#SelLayerProj").html(option);
             $("#PositionSelectLayer").val('0');
             $("#PositionSelectLayer").html(option);

        }
       

          //if (type == 'layer') {
        LayerQuery();
          


    }

    //var SelectChange3 = function () {

    //    var option = '<option value="0">请选择</option>';

    //    if ($("#PositionSelectType").val() == '0') {
    //        $("#PositionSelectLayer").val('0');
    //        $("#PositionSelectLayer").html(option);

    //    }


        //if (type == 'layer') {
    //    LayerPopQuery();



    //}


    var LayerQuery = function (obj) {
       // Param.PositionID = $("#PointSelectPosition").val();
        Param.ScanTypeID = $("#SelLayerType").val();
       
        if (Param.ScanTypeID == '0') {
            return;
        }

        $.post('LayerSetQuery', Param)
           .success(function (data, statusText, jqXRH) {
               if (data) {
                   if (data.Success) {
                       var html = '<option value="0">请选择</option>';
                       $(data.Data).each(function (n, d) {
                           html += '<option value="' + d.ID + '">' + d.LayerName + '</option>';
                       })
                       $("#SelLayerProj").html(html);
                       $("#PositionSelectLayer").html(html);
                      // $("#PositionSelectLayer").html(html);
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

    var AddIcon = function () {
        //alert('23345');
        // var SelectType = $("#SelScanTypePoint option:selected").attr('data-icon');
        //alert(SelectType);

        $("#UL_Icon").append("<li data-id='0' ondblclick='PositionSetJS.AddPosition(this)' style='background-image:url(/Content/IOT/img/dianji-4.png);' onmousedown='PositionSetJS.IconEventMouseDown(this)'></li>");

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
    var SavePosition = function () {

        var LayerInfo = {};
        LayerInfo.Type = $("input[name='PositionType']").val();
        LayerInfo.PileDiameter = $("input[name='PileDiameter']").val();
        LayerInfo.PileLength = $("input[name='PileLength']").val();
        LayerInfo.PileNumber = $("input[name='PileNumber']").val();
        LayerInfo.ID = $("input[name='PositionID']").val();
        LayerInfo.PositionName = $("input[name='PositionName']").val();
        //LayerInfo.MapX = xLeft * 100;
        //LayerInfo.MapY = xTop * 100;
        LayerInfo.MapX = $("input[name='x']").val();
        LayerInfo.MapY = $("input[name='y']").val();
      //  LayerInfo.Description = $("input[name='DescriptionLayer']").val();
        LayerInfo.LayerId = $("select[name='PositionSelectLayer']").val();
       // LayerInfo.ScanTypeId = $("select[name='PositionSelectType']").val();
        OpenMaskLayout();
        $.post('SavePosition', LayerInfo)
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
        PositionQuery();
    }

    var TurnBack = function () {
        //$('.popup-max').css('display', 'none');
        $('.popup-close').click();
    }

    //打开用户信息弹出框
    var AddPosition = function () {

        $("input[name='PositionName']").val('');
        $("input[name='PileDiameter']").val();
        $("input[name='PileLength']").val();
        $("input[name='PileNumber']").val();
       // $("input[name='DescriptionLayer']").val('');
        //$("input[name='LayerMapX']").val('');
        //$("input[name='LayerMapY']").val('');
        $("input[name='PositionType']").val('Add');
        DataInit();

        popupOpen();
        $('#PositionSetPopUp').slideDown();
    }

    var DeletePosition = function (dom) {

        var ID = $(dom).attr('data-id');
        if (ID == null || ID == '') {
            return;
        }

        var Param = {};
        Param.ID = ID;

        OpenDeleteModal();

        $("#ModalSubmit").bind('click', function () {

            DoDeletePosition(Param);
        })

        $("#myModal").modal('show');
    }

    var DoDeletePosition = function (dom) {

        $.post('DeletePosition', dom)
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
        DoPositionQuery: DoPositionQuery,
        SelectChange: SelectChange,
        SelectChange1: SelectChange1,
       // SelectChange3: SelectChange3,
        ShowPositionInfo: ShowPositionInfo,
        DeletePosition: DeletePosition,
        TurnBack: TurnBack,
        ShowModal: ShowModal,
        AddPosition: AddPosition,
        IconEventMouseDown: IconEventMouseDown,
        AddIcon: AddIcon,
        field: {

        }

    }

}(jQuery);
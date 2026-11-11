var ConstructConfigPositionJS = function ($) {

    var Param = {};

    $(document).ready(function () {
        //参数
        ParamInit();

        //列表数据绑定
        PositionQuery();

    })


    var ParamInit = function () {

        Param.PageIndex = 1;
        Param.PageSize = 20;
        Param.TotalPage = 0;
        Param.TotalCount = 0;

        Param.ProjectID = 0;
        Param.LayerId = 0;
        Param.ScanTypeID = 0;
    }

    var PositionQuery = function () {

        //打开遮罩层
        OpenMaskLayout();

        $.post('PositionQuery', Param)
            .success(function (data, statusText, jqXRH) {
                if (data) {
                    if (data.Success) {
                        var Model = {};
                        Model.list = data.Data;
                        var html = template("PositionTemplate", Model);
                        $("#PositionHtmlTemplate").html(html);

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


    var SelectChange = function () {
        var option = '<option value="0">请选择</option>';

        if ($("#SelProjPosition").val() == '0') {
            $("#SelLayerPosition").val('0');
            $("#SelLayerPosition").html(option);

        }
        LayerQuery();
    }

    var LayerQuery = function (obj) {
        // Param.PositionID = $("#PointSelectPosition").val();
        Param.ScanTypeID = $("#SelProjPosition").val();

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
                       $("#SelLayerPosition").html(html);
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


    //查询列表数据
    var DoPositionQuery = function () {
        Param.ProjectID = $("#SelProjPosition").val();
        Param.LayerId = $("#SelLayerPosition").val();
     //   Param.ScanTypeID = $("#SelScanTypePosition").val();
        PositionQuery();
    }
    //分页
    var Pagination = function () {
        $("#PaginationPosition").pagination({
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
                DoPositionQuery();
            }
        });
    }

    //打开用户信息弹出窗体
    var ShowPositionInfo = function (dom) {
        var ID = $(dom).attr('data-id');
        if (ID == null || ID == '') {
            return;
        }
        //$("p[name='UserInfoTitle']").html('用户信息更改');
        $("input[name='PositionType']").val('Modify');

        DataInit();

        var Param = {};
        Param.ID = ID;

        OpenMaskPopUp();

        $.post('PositionInfoQuery', Param)
            .success(function (data, statusText, jqXRH) {
                if (data) {
                    if (data.Success) {

                        var PositionInfo = data.Data;
                        //信息绑定
                        $("input[name='PositionID']").val(PositionInfo.ID);
                        $("input[name='PositionName']").val(PositionInfo.PositionName);
                        $("input[name='MapX']").val(PositionInfo.MapX);
                        $("input[name='MapY']").val(PositionInfo.MapY);
                        //$("input[name='Width']").val(PositionInfo.width);
                        //$("input[name='Height']").val(PositionInfo.height);
                        $("input[name='PileDiameter']").val(PositionInfo.PileDiameter);
                        $("input[name='PileLength']").val(PositionInfo.PileLength);
                        $("input[name='PileNumber']").val(PositionInfo.PileNumber);

                        //下拉绑定
                        $("select[name='PositionSelectLayer'] option[value='" + PositionInfo.LayerId + "']").prop('selected', true);
                     //   $("select[name='PositionSelectScanType'] option[value='" + PositionInfo.ScanTypeId + "']").prop('selected', true);
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

        $('#PositionPopUp').slideDown();

    }

    var DataInit = function () {

        $("select[name='PositionSelectLayer'] option:selected").each(function (index, dom) {
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
            SavePosition();
        })
        $("#myModal").modal('show');
    }

    //保存
    var SavePosition = function () {

        var PositionInfo = {};
        PositionInfo.Type = $("input[name='PositionType']").val();

        PositionInfo.ID = $("input[name='PositionID']").val();
        PositionInfo.PositionName = $("input[name='PositionName']").val();
        PositionInfo.LayerId = $("select[name='PositionSelectLayer']").val();
       // PositionInfo.ScanTypeID = $("select[name='PositionSelectScanType']").val();

        PositionInfo.MapX = $("input[name='MapX']").val();
        PositionInfo.MapY = $("input[name='MapY']").val();
        //PositionInfo.Height = $("input[name='Height']").val();
        //PositionInfo.Width = $("input[name='Width']").val();
        PositionInfo.PileDiameter=$("input[name='PileDiameter']").val();
        PositionInfo.PileLength=$("input[name='PileLength']").val();
        PositionInfo.PileNumber=$("input[name='PileNumber']").val();
        OpenMaskLayout();

        $.post('SavePosition', PositionInfo)
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
        $("input[name='MapX']").val('');
        $("input[name='MapY']").val('');
        $("input[name='PileDiameter']").val();
        $("input[name='PileLength']").val();
        $("input[name='PileNumber']").val();
        //$("input[name='Width']").val('');
        //$("input[name='Height']").val('');

        $("input[name='PositionType']").val('Add');
        DataInit();

        popupOpen();
        $('#PositionPopUp').slideDown();
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
        ShowPositionInfo: ShowPositionInfo,
        DeletePosition: DeletePosition,
        SelectChange:SelectChange,
        TurnBack: TurnBack,
        ShowModal: ShowModal,
        AddPosition: AddPosition,
        field: {

        }

    }

}(jQuery);
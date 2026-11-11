var ConstructConfigLayerJS = function ($) {

    var Param = {};

    $(document).ready(function () {
        //参数
        ParamInit();

        //列表数据绑定
        LayerQuery();

    })


    var ParamInit = function () {

        Param.PageIndex = 1;
        Param.PageSize = 20;
        Param.TotalPage = 0;
        Param.TotalCount = 0;

        Param.ProjectID = 0;
    }

    var LayerQuery = function () {

        //打开遮罩层
        OpenMaskLayout();

        $.post('LayerQuery', Param)
            .success(function (data, statusText, jqXRH) {
                if (data) {
                    if (data.Success) {
                        var Model = {};
                        Model.list = data.Data;
                        var html = template("LayerTemplate", Model);
                        $("#LayerHtmlTemplate").html(html);

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
    var DoLayerQuery = function () {
        Param.ProjectID = $("#SelLayerProj").val();
        LayerQuery();
    }
    //分页
    var Pagination = function () {
        $("#PaginationLayer").pagination({
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
                DoLayerQuery();
            }
        });
    }

    //打开用户信息弹出窗体
    var ShowLayerInfo = function (dom) {
        var ID = $(dom).attr('data-id');
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
                        $("input[name='LayerMapX']").val(LayerInfo.MapX);
                        $("input[name='LayerMapY']").val(LayerInfo.MapY);
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

        $('#LayerPopUp').slideDown();

    }

    var DataInit = function () {

        $("select[name='LayerSelect'] option:selected").each(function (index, dom) {
            $(dom).prop('selected', false);
        });
        $("select[name='TypeSelect'] option:selected").each(function (index, dom) {
            $(dom).prop('selected', false);
        });

    }


    //打开模态框
    var ShowModal = function () {

        OpenSaveModal();

        $("#ModalSubmit").bind('click', function () {
            SaveLayer();
        })
        $("#myModal").modal('show');
    }

    //保存
    var SaveLayer = function () {

        var LayerInfo = {};
        LayerInfo.Type = $("input[name='TypeLayer']").val();

        LayerInfo.ID = $("input[name='LayerIdx']").val();
        LayerInfo.LayerName = $("input[name='LayerName']").val();
        LayerInfo.MapX = $("input[name='LayerMapX']").val();
        LayerInfo.MapY = $("input[name='LayerMapY']").val();
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
        $("input[name='LayerMapX']").val('');
        $("input[name='LayerMapY']").val('');
        $("input[name='TypeLayer']").val('Add');
        DataInit();

        popupOpen();
        $('#LayerPopUp').slideDown();
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
        ShowLayerInfo: ShowLayerInfo,
        DeleteLayer: DeleteLayer,
        TurnBack: TurnBack,
        ShowModal: ShowModal,
        AddLayer: AddLayer,
        field: {

        }

    }

}(jQuery);
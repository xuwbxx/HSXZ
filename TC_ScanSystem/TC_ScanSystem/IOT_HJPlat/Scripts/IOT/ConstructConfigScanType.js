var ConstructConfigScanTypeJS = function ($) {

    var Param = {};

    $(document).ready(function () {
        //参数
        ParamInit();

        //列表数据绑定
        ScanTypeQuery();

    })


    var ParamInit = function () {

        Param.PageIndex = 1;
        Param.PageSize = 20;
        Param.TotalPage = 0;
        Param.TotalCount = 0;

        Param.ProjectID = 0;
        Param.IsCommand = 0;
    }

    var ScanTypeQuery = function () {

        //打开遮罩层
        OpenMaskLayout();

        $.post('ScanTypeQuery', Param)
            .success(function (data, statusText, jqXRH) {
                if (data) {
                    if (data.Success) {
                        var Model = {};
                        Model.list = data.Data;
                        var html = template("ScanTypeTemplate", Model);
                        $("#ScanTypeHtmlTemplate").html(html);

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
    var DoScanTypeQuery = function () {
        Param.ProjectID = $("#SelectScanTypeProj").val();
        Param.IsCommand = $("#SelectScanTypeProtocols").val();
        ScanTypeQuery();
    }
    //分页
    var Pagination = function () {
        $("#PaginationScanType").pagination({
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
                DoScanTypeQuery();
            }
        });
    }

    var DataInit = function () {
        $("select[name='ProjectSelectScanType'] option:selected").each(function (index, dom) {
            $(dom).prop('selected', false);
        });

        $("select[name='ProtocolSelectScanType'] option:selected").each(function (index, dom) {
            $(dom).prop('selected', false);
        });
    }

    //打开用户信息弹出窗体
    var ShowScanTypeInfo = function (dom) {
        var ID = $(dom).attr('data-id');
        if (ID == null || ID == '') {
            return;
        }
        //$("p[name='UserInfoTitle']").html('用户信息更改');
        $("input[name='TypeScanType']").val('Modify');

        DataInit();

        var Param = {};
        Param.ScanTypeID = ID;

        OpenMaskPopUp();

        $.post('ScanTypeInfoQuery', Param)
            .success(function (data, statusText, jqXRH) {
                if (data) {
                    if (data.Success) {

                        var ScanTypeInfo = data.Data;
                        //信息绑定
                        $("input[name='ScanTypeIdx']").val(ScanTypeInfo.ID);
                        $("input[name='ScanTypeName']").val(ScanTypeInfo.TypeName);
                        $("input[name='DescriptionScanType']").val(ScanTypeInfo.Description);
                        $("input[name='ImagePath']").val(ScanTypeInfo.ImagePath);
                        //下拉绑定
                        $("select[name='ProjectSelectScanType'] option[value='" + ScanTypeInfo.ProjectId + "']").prop('selected', true);
                        $("select[name='ProtocolSelectScanType'] option[value='" + ScanTypeInfo.IsCommand + "']").prop('selected', true);
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

        $('#ScanTypePopUp').slideDown();

    }

    var TurnBack = function () {
        //$('.popup-max').css('display', 'none');
        $('.popup-close').click();
    }

    var ShowModal = function () {

        OpenSaveModal();

        $("#ModalSubmit").bind('click', function () {
            SaveScanType();
        })
        $("#myModal").modal('show');
    }

    //保存
    var SaveScanType = function () {

        var ScanTypeInfo = {};
        ScanTypeInfo.Type = $("input[name='TypeScanType']").val();

        ScanTypeInfo.ID = $("input[name='ScanTypeIdx']").val();
        ScanTypeInfo.TypeName = $("input[name='ScanTypeName']").val();
        ScanTypeInfo.ImagePath= $("input[name='ImagePath']").val();
        ScanTypeInfo.Description = $("input[name='DescriptionScanType']").val();
        ScanTypeInfo.ProjectId = $("select[name='ProjectSelectScanType']").val();
        ScanTypeInfo.IsCommand = $("select[name='ProtocolSelectScanType']").val();

        OpenMaskLayout();

        $.post('SaveScanType', ScanTypeInfo)
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
        ScanTypeQuery();
    }

    var DeleteScanType = function (dom) {

        var ID = $(dom).attr('data-id');
        if (ID == null || ID == '') {
            return;
        }

        var Param = {};
        Param.ID = ID;

        OpenDeleteModal();

        $("#ModalSubmit").bind('click', function () {

            DoDeleteScanType(Param);
        })

        $("#myModal").modal('show');
    }

    var DoDeleteScanType = function (dom) {

        $.post('DeleteScanType', dom)
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

    //打开用户信息弹出框
    var AddScanType = function () {

        $("input[name='ScanTypeName']").val('');
        $("input[name='DescriptionScanType']").val('');
        $("input[name='ImagePath']").val('');
        $("input[name='TypeScanType']").val('Add');
        DataInit();

        popupOpen();
        $('#ScanTypePopUp').slideDown();
    }

    return {
        HomeSiteInit: function () {
            Init();
        },
        DoScanTypeQuery: DoScanTypeQuery,
        ShowScanTypeInfo: ShowScanTypeInfo,
        TurnBack: TurnBack,
        ShowModal: ShowModal,
        DeleteScanType: DeleteScanType,
        AddScanType: AddScanType,
        field: {

        }

    }

}(jQuery);
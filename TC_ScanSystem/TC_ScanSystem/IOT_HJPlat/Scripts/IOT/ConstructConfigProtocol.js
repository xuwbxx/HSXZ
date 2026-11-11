var ConstructConfigProtocolJS = function ($) {

    var Param = {};

    $(document).ready(function () {
        //参数
        ParamInit();

        //列表数据绑定
        ProtocolQuery();

    })


    var ParamInit = function () {

        Param.PageIndex = 1;
        Param.PageSize = 20;
        Param.TotalPage = 0;
        Param.TotalCount = 0;

        Param.Protocol = "";
    }

    var ProtocolQuery = function () {

        //打开遮罩层
        OpenMaskLayout();

        $.post('ProtocolQuery', Param)
            .success(function (data, statusText, jqXRH) {
                if (data) {
                    if (data.Success) {
                        var Model = {};
                        Model.list = data.Data;
                        var html = template("ProtocolTemplate", Model);
                        $("#ProtocolHtmlTemplate").html(html);

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
    var DoProtocolQuery = function () {
        Param.Protocol = $("#ProtocolName").val();
        ProtocolQuery();
    }
    //分页
    var Pagination = function () {
        $("#ProtocolPagination").pagination({
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
                DoProtocolQuery();
            }
        });
    }

    //打开用户信息弹出窗体
    var ShowProtocolInfo = function (dom) {
        var ID = $(dom).attr('data-id');
        if (ID == null || ID == '') {
            return;
        }
        //$("p[name='UserInfoTitle']").html('用户信息更改');
        $("input[name='ProtocolType']").val('Modify');

        var Param = {};
        Param.ProtocolID = ID;

        OpenMaskPopUp();

        $.post('ProtocolInfoQuery', Param)
            .success(function (data, statusText, jqXRH) {
                if (data) {
                    if (data.Success) {

                        var ProtocolInfo = data.Data;
                        //信息绑定
                        $("input[name='ProtocolID']").val(ProtocolInfo.ID);
                        $("input[name='ProtocolName']").val(ProtocolInfo.Protocol);
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

        $('#ProtocolPopUp').slideDown();

    }



    //打开模态框
    var ShowModal = function () {

        OpenSaveModal();

        $("#ModalSubmit").bind('click', function () {
            SaveProtocol();
        })
        $("#myModal").modal('show');
    }

    //保存
    var SaveProtocol = function () {

        var ProtocolInfo = {};
        ProtocolInfo.Type = $("input[name='ProtocolType']").val();

        ProtocolInfo.ID = $("input[name='ProtocolID']").val();
        ProtocolInfo.Protocol = $("input[name='ProtocolName']").val();

        OpenMaskLayout();

        $.post('SaveProtocol', ProtocolInfo)
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
        ProtocolQuery();
    }

    var TurnBack = function () {
        //$('.popup-max').css('display', 'none');
        $('.popup-close').click();
    }

    //打开用户信息弹出框
    var AddProtocol = function () {

        $("input[name='ProtocolName']").val('');

        $("input[name='ProtocolType']").val('Add');

        popupOpen();
        $('#ProtocolPopUp').slideDown();
    }

    var DeleteProtocol = function (dom) {

        var ID = $(dom).attr('data-id');
        if (ID == null || ID == '') {
            return;
        }

        var Param = {};
        Param.ID = ID;

        OpenDeleteModal();

        $("#ModalSubmit").bind('click', function () {

            DoDeleteProtocol(Param);
        })

        $("#myModal").modal('show');
    }

    var DoDeleteProtocol = function (dom) {

        $.post('DeleteProtocol', dom)
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
        DoProtocolQuery: DoProtocolQuery,
        ShowProtocolInfo: ShowProtocolInfo,
        DeleteProtocol: DeleteProtocol,
        TurnBack: TurnBack,
        ShowModal: ShowModal,
        AddProtocol: AddProtocol,
        field: {

        }

    }

}(jQuery);
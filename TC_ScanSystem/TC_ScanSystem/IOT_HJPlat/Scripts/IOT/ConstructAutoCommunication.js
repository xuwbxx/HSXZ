var ConstructAutoCommunicationJS = function ($) {

    var Param = {};

    $(document).ready(function () {
        //参数
        ParamInit();

        //列表数据绑定
        CommunicationQuery();

    })


    var ParamInit = function () {

        Param.PageIndex = 1;
        Param.PageSize = 20;
        Param.TotalPage = 0;
        Param.TotalCount = 0;

        Param.ProjectID = 0;
    }

    var CommunicationQuery = function () {

        //打开遮罩层
        OpenMaskLayout();

        $.post('CommunicationQuery', Param)
            .success(function (data, statusText, jqXRH) {
                if (data) {
                    if (data.Success) {
                        var Model = {};
                        Model.list = data.Data;
                        var html = template("CommunicationTemplate", Model);
                        $("#CommunicationHtmlTemplate").html(html);

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
    var DoCommunicationQuery = function () {
        Param.ProjectID = $("#Select_Project").val();
        Param.LayerID = $("#Select_Layer").val();
        Param.ScanTypeID = $("#Select_ScanType").val();
        CommunicationQuery();
    }
    //分页
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
                DoCommunicationQuery();
            }
        });
    }

    //打开用户信息弹出窗体
    var ShowCommunicationInfo = function (dom) {
        var ID = $(dom).attr('data-id');
        if (ID == null || ID == '') {
            return;
        }
        //$("p[name='UserInfoTitle']").html('用户信息更改');
        $("input[name='Type']").val('Modify');

        DataInit();

        var Param = {};
        Param.ID = ID;

        OpenMaskPopUp();

        $.post('CommunicationInfoQuery', Param)
            .success(function (data, statusText, jqXRH) {
                if (data) {
                    if (data.Success) {

                        var CommunicationInfo = data.Data;
                        //信息绑定
                        $("input[name='CommunicationID']").val(CommunicationInfo.ID);
                        $("input[name='CommunicationName']").val(CommunicationInfo.CommnunicationName);
                        $("input[name='CommunicationCode']").val(CommunicationInfo.CommunicationCode);
                        $("input[name='IP']").val(CommunicationInfo.IP);
                        $("input[name='Port']").val(CommunicationInfo.Port);

                        //下拉绑定
                        $("select[name='ProjectSelect'] option[value='" + CommunicationInfo.ProjectId + "']").prop('selected', true);
                        $("select[name='CommunicationTypeSelect'] option[value='" + CommunicationInfo.CommunicationType + "']").prop('selected', true);
                        $("select[name='DataProtocolSelect'] option[value='" + CommunicationInfo.DataProtocol + "']").prop('selected', true);
                        $("select[name='DeviceTypeSelect'] option[value='" + CommunicationInfo.DeviceType + "']").prop('selected', true);
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

        $('#CommunicationPopUp').slideDown();

    }

    var DataInit = function () {

        $("select[name='ProjectSelect'] option:selected").each(function (index, dom) {
            $(dom).prop('selected', false);
        });
        $("select[name='CommunicationTypeSelect'] option:selected").each(function (index, dom) {
            $(dom).prop('selected', false);
        });
        $("select[name='DataProtocolSelect'] option:selected").each(function (index, dom) {
            $(dom).prop('selected', false);
        });
        $("select[name='DeviceTypeSelect'] option:selected").each(function (index, dom) {
            $(dom).prop('selected', false);
        });

    }


    //打开模态框
    var ShowModal = function () {

        OpenSaveModal();

        $("#ModalSubmit").bind('click', function () {
            SaveCommunication();
        })
        $("#myModal").modal('show');
    }

    //保存
    var SaveCommunication = function () {

        var CommunicationInfo = {};
        CommunicationInfo.Type = $("input[name='Type']").val();

        CommunicationInfo.ID = $("input[name='CommunicationID']").val();
        CommunicationInfo.CommnunicationName = $("input[name='CommunicationName']").val();
        CommunicationInfo.CommunicationCode = $("input[name='CommunicationCode']").val();
        CommunicationInfo.IP = $("input[name='IP']").val();
        CommunicationInfo.Port = $("input[name='Port']").val();

        CommunicationInfo.ProjectId = $("select[name='ProjectSelect']").val();
        CommunicationInfo.CommunicationType = $("select[name='CommunicationTypeSelect']").val();
        CommunicationInfo.DeviceType = $("select[name='DeviceTypeSelect']").val();
        CommunicationInfo.DataProtocol = $("select[name='DataProtocolSelect']").val();

        OpenMaskLayout();

        $.post('SaveCommunication', CommunicationInfo)
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
        CommunicationQuery();
    }

    var TurnBack = function () {
        //$('.popup-max').css('display', 'none');
        $('.popup-close').click();
    }

    //打开用户信息弹出框
    var AddCommunication = function () {

        $("input[name='CommunicationID']").val('');
        $("input[name='CommunicationName']").val('');
        $("input[name='CommunicationCode']").val('');
        $("input[name='IP']").val('');
        $("input[name='Port']").val('');

        $("input[name='Type']").val('Add');
        DataInit();

        popupOpen();
        $('#CommunicationPopUp').slideDown();
    }

    var DeleteCommunication = function (dom) {

        var ID = $(dom).attr('data-id');
        if (ID == null || ID == '') {
            return;
        }

        var Param = {};
        Param.ID = ID;

        OpenDeleteModal();

        $("#ModalSubmit").bind('click', function () {

            DoDeleteCommunication(Param);
        })

        $("#myModal").modal('show');
    }

    var DoDeleteCommunication = function (dom) {

        $.post('DeleteCommunication', dom)
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
        DoCommunicationQuery: DoCommunicationQuery,
        ShowCommunicationInfo: ShowCommunicationInfo,
        DeleteCommunication: DeleteCommunication,
        TurnBack: TurnBack,
        ShowModal: ShowModal,
        AddCommunication: AddCommunication,
        field: {

        }

    }

}(jQuery);
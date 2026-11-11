var ConstructAutoDeviceJS = function ($) {

    var Param = {};

    $(document).ready(function () {
        //参数
        ParamInit();

        //列表数据绑定
        DeviceQuery();

    })


    var ParamInit = function () {

        Param.PageIndex = 1;
        Param.PageSize = 20;
        Param.TotalPage = 0;
        Param.TotalCount = 0;

        Param.ProjectID = 0;
        Param.LayerID = 0;
        Param.CommunicationDeviceID = 0;
    }

    var DeviceQuery = function () {

        //打开遮罩层
        OpenMaskLayout();

        $.post('DeviceQuery', Param)
            .success(function (data, statusText, jqXRH) {
                if (data) {
                    if (data.Success) {
                        var Model = {};
                        Model.list = data.Data;
                        var html = template("DeviceTemplate", Model);
                        $("#DeviceHtmlTemplate").html(html);

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
    var DoDeviceQuery = function () {
        Param.ProjectID = $("#Select_Project").val();
        Param.LayerID = $("#Select_Layer").val();
        Param.CommunicationDeviceID = $("#Select_Communication").val();
        DeviceQuery();
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
                DoDeviceQuery();
            }
        });
    }

    //打开用户信息弹出窗体
    var ShowDeviceInfo = function (dom) {
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

        $.post('DeviceInfoQuery', Param)
            .success(function (data, statusText, jqXRH) {
                if (data) {
                    if (data.Success) {

                        var DeviceInfo = data.Data;
                        //信息绑定
                        $("input[name='DeviceID']").val(DeviceInfo.ID);
                        $("input[name='DeviceName']").val(DeviceInfo.DeviceName);
                        $("input[name='DeviceIden']").val(DeviceInfo.DeviceIden);

                        //下拉绑定
                        $("select[name='ProjectSelect'] option[value='" + DeviceInfo.ProjectId + "']").prop('selected', true);
                        $("select[name='DeviceProtocolSelect'] option[value='" + DeviceInfo.DeviceProtocol + "']").prop('selected', true);
                        $("select[name='LayerSelect'] option[value='" + DeviceInfo.LayerId + "']").prop('selected', true);
                        $("select[name='CommunicationSelect'] option[value='" + DeviceInfo.CommunicationDeviceId + "']").prop('selected', true);
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

        $('#DevicePopUp').slideDown();

    }

    var DataInit = function () {

        $("select[name='ProjectSelect'] option:selected").each(function (index, dom) {
            $(dom).prop('selected', false);
        });
        $("select[name='DeviceProtocolSelect'] option:selected").each(function (index, dom) {
            $(dom).prop('selected', false);
        });
        $("select[name='LayerSelect'] option:selected").each(function (index, dom) {
            $(dom).prop('selected', false);
        });
        $("select[name='CommunicationSelect'] option:selected").each(function (index, dom) {
            $(dom).prop('selected', false);
        });
    }


    //打开模态框
    var ShowModal = function () {

        OpenSaveModal();

        $("#ModalSubmit").bind('click', function () {
            SaveDevice();
        })
        $("#myModal").modal('show');
    }

    //保存
    var SaveDevice = function () {

        var DeviceInfo = {};
        DeviceInfo.Type = $("input[name='Type']").val();

        DeviceInfo.ID = $("input[name='DeviceID']").val();
        DeviceInfo.DeviceName = $("input[name='DeviceName']").val();
        DeviceInfo.DeviceIden = $("input[name='DeviceIden']").val();

        DeviceInfo.ProjectId = $("select[name='ProjectSelect']").val();
        DeviceInfo.DeviceProtocol = $("select[name='DeviceProtocolSelect']").val();
        DeviceInfo.LayerId = $("select[name='LayerSelect']").val();
        DeviceInfo.CommunicationDeviceId = $("select[name='CommunicationSelect']").val();

        OpenMaskLayout();

        $.post('SaveDevice', DeviceInfo)
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
        DeviceQuery();
    }

    var TurnBack = function () {
        //$('.popup-max').css('display', 'none');
        $('.popup-close').click();
    }

    //打开用户信息弹出框
    var AddDevice = function () {

        $("input[name='DeviceID']").val('');
        $("input[name='DeviceName']").val('');
        $("input[name='DeviceIden']").val('');

        $("input[name='Type']").val('Add');
        DataInit();

        popupOpen();
        $('#DevicePopUp').slideDown();
    }

    var DeleteDevice = function (dom) {

        var ID = $(dom).attr('data-id');
        if (ID == null || ID == '') {
            return;
        }

        var Param = {};
        Param.ID = ID;

        OpenDeleteModal();

        $("#ModalSubmit").bind('click', function () {

            DoDeleteDevice(Param);
        })

        $("#myModal").modal('show');
    }

    var DoDeleteDevice = function (dom) {

        $.post('DeleteDevice', dom)
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
        DoDeviceQuery: DoDeviceQuery,
        ShowDeviceInfo: ShowDeviceInfo,
        DeleteDevice: DeleteDevice,
        TurnBack: TurnBack,
        ShowModal: ShowModal,
        AddDevice: AddDevice,
        field: {

        }

    }

}(jQuery);
var ConstructAutoCommandJS = function ($) {

    var Param = {};

    $(document).ready(function () {
        //参数
        ParamInit();

        //列表数据绑定
      //  DeviceQuery();

    })


    var ParamInit = function () {

        //Param.PageIndex = 1;
        //Param.PageSize = 20;
        //Param.TotalPage = 0;
        //Param.TotalCount = 0;

        Param.DeviceId = 0;
    }

    //var DeviceQuery = function () {

    //    //打开遮罩层
    //    OpenMaskLayout();

    //    $.post('DeviceQuery', Param)
    //        .success(function (data, statusText, jqXRH) {
    //            if (data) {
    //                if (data.Success) {
    //                    var Model = {};
    //                    Model.list = data.Data;
    //                    var html = template("DeviceTemplate", Model);
    //                    $("#DeviceHtmlTemplate").html(html);

    //                    //分页
    //                    Param.TotalPage = data.TotalPage;
    //                    Param.TotalCount = data.TotalCount;
    //                    Param.PageIndex = data.PageIndex;
    //                }
    //            }
    //        })
    //        .fail(function (jqXHR, statusText, errorMessage) {
    //            //console.error(arguments);
    //        })
    //        .always(function (data) {
    //            // 关闭等待遮罩
    //            Pagination();

    //            CloseMaskLayout();
    //        });
    //}

    //查询列表数据
    //var DoDeviceQuery = function () {
    //    Param.ProjectID = $("#Select_Project").val();
    //    Param.LayerID = $("#Select_Layer").val();
    //    Param.CommunicationDeviceID = $("#Select_Communication").val();
    //    DeviceQuery();
    //}
    ////分页
    //var Pagination = function () {
    //    $("#Pagination").pagination({
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
    //            DoDeviceQuery();
    //        }
    //    });
    //}



    var SelectChange = function (type) {

        //var option = '<option value="0">请选择</option>';

        //    $("#select_AutoDevice").val('0');
        //    $("#select_AutoDevice").html(option);

        //if ($("#select_AutoDevice").val() == '0') {
        //}

         if (type == 'device') {
            ShowDeviceInfo();
        }


    }


    //打开用户信息弹出窗体
var ShowDeviceInfo = function (dom) {
        var Param = {};
        Param.ID = $("#select_AutoDevice").val();
        if(Param.ID == '0')
        {
            return;
        }
        //var ID = $(dom).attr('data-id');
        //if (ID == null || ID == '') {
        //    return;
        //}
        //$("p[name='UserInfoTitle']").html('用户信息更改');

        DataInit();

        

        $.post('CommandQuery', Param)
            .success(function (data, statusText, jqXRH) {
                if (data) {
                    if (data.Success) {

                        var DeviceInfo = data.Data;
                        //信息绑定
                        $("input[name='txtDate']").val(DeviceInfo.Date);
                        $("input[name='txtHour']").val(DeviceInfo.Hour);
                        $("input[name='txtMin']").val(DeviceInfo.Min);
                        $("input[name='txtBeginTime']").val(DeviceInfo.BeginTime);
                       // $("input[name='DeviceDate']").val(DeviceInfo.DeviceDate);
                        //$("input[name='AcquisitionBeginDate']").val(DeviceInfo.DeviceDate);
                        $('.form_datetime[data-link-field="dtp_input11"]').datetimepicker("setDate", new Date(DeviceInfo.DeviceDate));

                    }
                }
            })
            .fail(function (jqXHR, statusText, errorMessage) {
                //console.error(arguments);
            })
            .always(function (data) {
                // 关闭等待遮罩
               // CloseMaskPopUp();
            });

    }

    var DataInit = function () {

        $("select[name='select_AutoDevice'] option:selected").each(function (index, dom) {
            $(dom).prop('selected', false);
        });
    }


    //打开模态框
    var ShowModal = function () {

        OpenSaveModal();

        $("#ModalSubmit").bind('click', function () {
            SaveDevice();
           // SaveInterval();
        })
        $("#myModal").modal('show');
    }

    var SaveInterval = function () {

        var DeviceInfo = {};

        DeviceInfo.Date = $("input[name='txtDate']").val();
        DeviceInfo.Min = $("input[name='txtMin']").val();
        DeviceInfo.Hour = $("input[name='txtHour']").val();
        DeviceInfo.ID = $("#select_AutoDevice").val();

        OpenMaskLayout();

        $.post('SaveInterval', DeviceInfo)
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


    var SaveDeviceTime = function () {

        var DeviceInfo = {};

        DeviceInfo.DeviceTime = $("input[name='DeviceDate']").val();
        DeviceInfo.ID = $("#select_AutoDevice").val();

        OpenMaskLayout();

        $.post('SaveDeviceTime', DeviceInfo)
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

    var SaveBeginTime = function () {

        var DeviceInfo = {};

        DeviceInfo.BeginTime = $("input[name='txtBeginTime']").val();
        DeviceInfo.ID = $("#select_AutoDevice").val();

        OpenMaskLayout();

        $.post('SaveBeginTime', DeviceInfo)
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

    var Send = function () {

        var DeviceInfo = {};

        DeviceInfo.Command = $("input[name='txtCommand']").val();
        DeviceInfo.ID = $("#select_AutoDevice").val();

        OpenMaskLayout();

        $.post('Send', DeviceInfo)
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
        ShowDeviceInfo();
       // DeviceQuery();
    }

    var TurnBack = function () {
        //$('.popup-max').css('display', 'none');
        $('.popup-close').click();
    }

    return {
        HomeSiteInit: function () {
            Init();
        },
        // DoDeviceQuery: DoDeviceQuery,
        SelectChange:SelectChange,
        ShowDeviceInfo: ShowDeviceInfo,
        TurnBack: TurnBack,
        SaveInterval: SaveInterval,
        SaveDeviceTime: SaveDeviceTime,
        SaveBeginTime:SaveBeginTime,
        ShowModal: ShowModal,
        Send:Send,
       // AddDevice: AddDevice,
        field: {

        }

    }

}(jQuery);
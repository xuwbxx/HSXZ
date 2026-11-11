var TargetJS = function ($) {

    var Param = {};

    $(document).ready(function () {
        Init();
        ParamInit();
        Query();
    })

    var ParamInit = function () {
        //Param.PageIndex = 1;
        //Param.PageSize = 20;
        //Param.TotalPage = 0;
        //Param.TotalCount = 0;

        //Param.UserName = $("input[name='txt_UserName']").val();
        //Param.RealName = $("input[name='txt_RealName']").val();
    }

    var Init = function () {

    }

    //查询
    var Query = function () {

        OpenMaskLayout();

        $.post('DataQuery', Param)
                .success(function (data, statusText, jqXRH) {
                    if (data) {

                        if (data.Success) {
                            var Model = {};
                            Model.list = data.Data;
                            var html = template("TargetTemplate", Model);
                            $("#TargetHtmlTemplate").html(html);


                        }
                    }
                })
                .fail(function (jqXHR, statusText, errorMessage) {
                    //console.error(arguments);
                })
                .always(function (data) {
                   // Pagination();
                    CloseMaskLayout();
                });

    }


    //关闭窗体并刷新
    var CloseModalEvent = function () {
        TurnBack();
        Query();
    }

    //返回
    var TurnBack = function () {
        //$('.popup-max').css('display', 'none');
        $('.popup-close').click();
    }

    //打开编辑框
    var Edit = function (dom) {
        if (dom == null)
            return;
        var ID = $(dom).attr('data-id');

       // $("input[name='txt_UserNamePop']").prop('readonly', true);
        $("p[name='TargetTitle']").html('看板页指标填报');
        //$("input[name='Type']").val('Modify');

       // DataInit();

        var Param = {};
        Param.ID = ID;

        OpenMaskPopUp();

        $.post('TargetQuery', Param)
            .success(function (data, statusText, jqXRH) {
                if (data) {
                    if (data.Success) {

                        var Account = data.Data;
                        //信息绑定
                        $("input[name='TargetID']").val(Account.ID);
                        $("input[name='txt_Security']").val(Account.Security);
                        $("input[name='txt_Quality']").val(Account.Quality);
                        $("input[name='txt_Progress']").val(Account.Progress);
                        $("input[name='txt_Cost']").val(Account.Cost);
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

        $('#TargetPopUp').slideDown();
    }

    //打开模态框
    var Save = function () {

        OpenSaveModal();

        $("#ModalSubmit").bind('click', function () {
            SaveData();
        })
        $("#myModal").modal('show');
    }

    //保存
    var SaveData = function () {

        var Data = {};
        Data.ID = $("input[name='TargetID']").val();
        Data.Security = $("input[name='txt_Security']").val();
        Data.Quality = $("input[name='txt_Quality']").val();
        Data.Progress = $("input[name='txt_Progress']").val();
        Data.Cost = $("input[name='txt_Cost']").val();

        OpenMaskLayout();

        $.post('TargetSave', Data)
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
      //  DoQuery: DoQuery,
        Save: Save,
        Edit: Edit,
        TurnBack: TurnBack,
        field: {

        }
    }

}(jQuery);
var ConstructConfigAcquisitionJS = function ($) {

    var ShowModal = function () {

        OpenSaveModal();

        $("#ModalSubmit").bind('click', function () {
            Save();
        })
        $("#myModal").modal('show');
    }

    var Save = function () {
        //alert('123');
        var Param = {};
        Param.BeginDate = $("input[name='AcquisitionBeginDate']").val();
        Param.Interval = $("input[name='AcquisitionInterval']").val();

        //打开遮罩层
        OpenMaskLayout();

        $.post('ReadConfigSave', Param)
            .success(function (data, statusText, jqXRH) {
                if (data) {
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
                
                CloseMaskLayout();
            });

    }

    var CloseModalEvent = function () {

        $('.popup-close').click();
    }


    return {
        ShowModal: ShowModal,
        field: {

        }

    }

}(jQuery);
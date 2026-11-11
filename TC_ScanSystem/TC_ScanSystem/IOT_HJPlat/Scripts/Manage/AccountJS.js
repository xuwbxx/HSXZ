var AccountJS = function ($) {

    var Param = {};

    $(document).ready(function () {
        Init();
        ParamInit();
        Query();
    })

    var ParamInit = function () {
        Param.PageIndex = 1;
        Param.PageSize = 20;
        Param.TotalPage = 0;
        Param.TotalCount = 0;

        Param.UserName = $("input[name='txt_UserName']").val();
        Param.RealName = $("input[name='txt_RealName']").val();
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
                            var html = template("AccountHtmlTemplate", Model);
                            $("#AccountHtml").html(html);

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
                    Pagination();
                    CloseMaskLayout();
                });

    }

    var DoQuery = function () {

        Param.UserName = $("input[name='txt_UserName']").val();
        Param.RealName = $("input[name='txt_RealName']").val();
        Query();
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
                Query();
            }
        });
    }

    //打开删除提示框
    var Delete = function (dom) {
        if (dom == null)
        {
            return;
        }
        var ID = $(dom).attr('data-id');
        var Name = $(dom).attr('data-name');
        OpenDeleteModal(Name);

        $("#myModal").modal('show');

        $("#ModalSubmit").bind('click', function () {
            var DeleteParam = {};
            DeleteParam.ID = ID;
            DoDelete(DeleteParam);
        })

        $("#ModalClose").bind('click', function () {
            CloseModalEvent();
        })

    }

    //删除数据
    var DoDelete = function (dom) {

        $.post('DataDelete', dom)
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
                // 关闭等待遮罩
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

        $("input[name='txt_UserNamePop']").prop('readonly', true);
        $("p[name='AccountTitle']").html('用户信息更改');
        $("input[name='Type']").val('Modify');

        DataInit();

        var Param = {};
        Param.ID = ID;

        OpenMaskPopUp();

        $.post('AccountQuery', Param)
            .success(function (data, statusText, jqXRH) {
                if (data) {
                    if (data.Success) {

                        var Account = data.Data;
                        //信息绑定
                        $("input[name='AccountID']").val(Account.ID);
                        $("input[name='txt_UserNamePop']").val(Account.UserName);
                        $("input[name='txt_PasswordPop']").val(Account.Password);
                        $("input[name='txt_RealNamePop']").val(Account.RealName);
                        $("input[name='txt_MobilePop']").val(Account.Mobile);

                        //下拉绑定
                        $("select[name='select_DepartPop'] option[value='" + Account.DepartID + "']").prop('selected', true);
                        $("select[name='select_PositionPop'] option[value='" + Account.PositionID + "']").prop('selected', true);
                        $("select[name='select_StructorPop'] option[value='" + Account.StructorID + "']").prop('selected', true);
                        //多选绑定
                        $("ul[name='UL_RolePop'] input[type='checkbox']").each(function (index, dom) {
                            $(dom).prop('checked', false);
                            for (var i = 0; i < Account.Roles.split(',').length; i++) {
                                if ($(dom).val() == Account.Roles.split(',')[i]) {
                                    $(dom).prop('checked', true);
                                }
                            }
                        })
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

        $('#AccountPopUp').slideDown();
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
        Data.Type = $("input[name='Type']").val();
        Data.ID = $("input[name='AccountID']").val();
        Data.UserName = $("input[name='txt_UserNamePop']").val();
        Data.Password = $("input[name='txt_PasswordPop']").val();
        Data.RealName = $("input[name='txt_RealNamePop']").val();
        Data.Mobile = $("input[name='txt_MobilePop']").val();

        Data.StructorID = $("select[name='select_StructorPop']").val();
        Data.PositionID = $("select[name='select_PositionPop']").val();
        Data.DepartID = $("select[name='select_DepartPop']").val();

        var Roles = '';
        $("ul[name='UL_RolePop'] input[type='checkbox']:checked").each(function (index, dom) {
            Roles += $(dom).val() + ',';
        })
        Data.Roles = Roles;

        OpenMaskLayout();

        $.post('AccountSave', Data)
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

    //新增
    var Add = function () {
        $("input[name='Type']").val('Add');

        $("p[name='AccountTitle']").html('用户新增');
        $("input[name='txt_UserNamePop']").prop('readonly', false);
        $("input[name='txt_UserNamePop']").val('');
        $("input[name='txt_PasswordPop']").val('123456');
        $("input[name='txt_RealNamePop']").val('');
        $("input[name='txt_MobilePop']").val('');
        
        DataInit();

        popupOpen();
        $('#AccountPopUp').slideDown();
    }

    //数据初始化
    var DataInit = function () {
        $("select[name='select_DepartPop'] option:selected").each(function (index, dom) {
            $(dom).prop('selected', false);
        });
        $("select[name='select_PositionPop'] option:selected").each(function (index, dom) {
            $(dom).prop('selected', false);
        });
        $("select[name='select_StructorPop'] option:selected").each(function (index, dom) {
            $(dom).prop('selected', false);
        });
        $("ul[name='UL_RolePop'] input:checked").each(function (index, dom) {
            $(dom).prop('checked', false);
        })
    }

    return {
        HomeSiteInit: function () {
            Init();
        },
        DoQuery: DoQuery,
        Delete: Delete,
        Save: Save,
        Edit: Edit,
        Add: Add,
        TurnBack: TurnBack,
        field: {

        }
    }

}(jQuery);
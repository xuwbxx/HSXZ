var ProgressJS = function ($) {

    var Param = {};
    var treeData = {};

    $(document).ready(function () {
        //参数
        ParamInit();
        //列表数据绑定
        Query();

    })

    var ParamInit = function () {

        Param.PageIndex = 1;
        Param.PageSize = 50;
        Param.TotalPage = 0;
        Param.TotalCount = 0;

        Param.RoleName = '';
    }

    var Query = function () {

        //打开遮罩层
        OpenMaskLayout();

        $.post('DataQuery', Param)
            .success(function (data, statusText, jqXRH) {
                if (data) {
                    if (data.Success) {
                        var Model = {};
                        Model.list = data.Data;

                        //for (var i = 0, len = Model.list.length; i < len; i++) {
                        //    Model.list[i].MenuIDs = JSON.stringify(Model.list[i].MenuIDs);
                        //}

                        //treeData = data.Data.MenuTree;
                        //treeData = JSON.stringify(data.Data.MenuTree);

                        var html = template("ProgressTemplate", Model);
                        $("#ProgressHtmlTemplate").html(html);

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
    var DoQuery = function () {
    //    Param.FillinMonth = $("input[name='txt_FillinMonth']").val();
        Query();
    }

    //编辑
    var Edit = function (dom) {
        if (dom == null)
            return;
        var ID = $(dom).attr('data-id');

        $("input[name='ID']").val(ID);
        $("p[name='ProgressTitle']").html('产值计划编辑');
       // $("input[name='txt_FillinMonthPop']").prop('readonly', true);
        $("select[name='Select_Year']").attr("disabled", "disabled")
        $("select[name='Select_Month']").attr("disabled", "disabled");
        var Param = {};
        Param.ID = ID;

        OpenMaskPopUp();

        $.post('ProgressQuery', Param)
            .success(function (data, statusText, jqXRH) {
                if (data) {
                    if (data.Success) {

                        var Role = data.Data;
                        //信息绑定
                        $("input[name='ID']").val(Role.ID);
                        $("input[name='txt_PlanValue']").val(Role.PlanValue);
                        $("input[name='txt_ActualValue']").val(Role.ActualValue);
                        // $("input[input='txt_FillinMonthPop']").val(Role.FillinMonth);
                        $("select[name='Select_Year']").val(Role.Year);
                        $("select[name='Select_Month']").val(Role.Month);
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

        $('#ProgressPopUp').slideDown();
    }

    var Add = function () {
        $("p[name='ProgressTitle']").html('产值计划新增');
        $("input[name='ID']").val('0');
        $("input[name='txt_PlanValue']").val('');
        $("input[name='txt_ActualValue']").val('');
        $("select[name='Select_Year']").val('');
        $("select[name='Select_Month']").val('');
        $("select[name='Select_Year']").attr("disabled", false)
        $("select[name='Select_Month']").attr("disabled", false);
       // $("input[name='txt_FillinMonth']").;

        popupOpen();

        $('#ProgressPopUp').slideDown();
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
                DoQuery();
            }
        });
    }


    var Delete = function (dom) {
        if (dom == null) {
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

    ///获取编辑页数据
    function ReadDataFromPopup() {

        var nodeInfo = {};

        nodeInfo.ID = $("input[name='ID']").val();
        nodeInfo.PlanValue = $("input[name='txt_PlanValue']").val();
        nodeInfo.ActualValue = $("input[name='txt_ActualValue']").val();
        nodeInfo.tempYear = $("select[name='Select_Year']").val();;
        nodeInfo.tempMonth = $("select[name='Select_Month']").val();;
       // nodeInfo.FillinMonth = $("input[name='txt_FillinMonthPop']");
       
      

        return nodeInfo;
    }

    var Save = function () {

        var Data = ReadDataFromPopup();

        OpenSaveModal()
        $("#myModal").modal('show');

        $("#ModalSubmit").bind('click', function () {

            if (Data.ID > 0) {
                Data.Type = 'Modify';
            }
            else if (Data.ID == 0) {
                Data.Type = 'Add';
            }
            CallMethod('DataSave', Data);
        })
    }



    var CallMethod = function (methodUrl, methodParam) {
        var ret = {};
        //打开遮罩层
        OpenMaskLayout();

        $.post(methodUrl, methodParam)
            .success(function (data, statusText, jqXRH) {
                if (data) {
                    if (data.Success) {
                        OpenModalSuccessTips();
                        $("#myModal").modal('show');

                        $("#ModalClose").bind('click', function () {
                            CloseModalEvent();
                        })
                        ret = data;
                        DoQuery();
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
                return ret;
            });
    }

    //关闭窗体并刷新
    var CloseModalEvent = function () {

        TurnBack();
        Query();
    }

    var TurnBack = function () {
        $('.popup-close').click();
    }


    return {
        HomeSiteInit: function () {
            Init();
        },
        DoQuery: DoQuery,
        Edit: Edit,
        Add: Add,
        Delete: Delete,
        //DoDelete:DoDelete,
        Save: Save,
        TurnBack: TurnBack,
        field: {

        }

    }

}(jQuery);
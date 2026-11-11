var ProjectJS = function ($) {

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
        Param.PageSize = 20;
        Param.TotalPage = 0;
        Param.TotalCount = 0;

        Param.ProjectName = '';
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

                        var html = template("ProjectTemplate", Model);
                        $("#ProjectHtml").html(html);

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
        Param.ProjectName = $("input[name='txt_ProjectName']").val();
        Query();
    }

    //编辑
    var Edit = function (dom) {
        if (dom == null)
            return;
        var ID = $(dom).attr('data-id');

        $("input[name='ID']").val(ID);
        $("input[name='txt_ShortNamePop']").prop('readonly', true);
        $("input[name='txt_ProjectCodePop']").prop('readonly', true);
        $("p[name='ProjectTitle']").html('项目编辑');

        var Param = {};
        Param.ID = ID;

        OpenMaskPopUp();

        $.post('ProjectQuery', Param)
            .success(function (data, statusText, jqXRH) {
                if (data) {
                    if (data.Success) {

                        var Project = data.Data;
                        //信息绑定
                        $("input[name='ID']").val(Project.ID);
                        $("input[name='txt_ProjectNamePop']").val(Project.ProjectName);
                        $("input[name='txt_ShortNamePop']").val(Project.ShortName);
                        $("input[name='txt_ProjectCodePop']").val(Project.ProjectCode);

                        //下拉菜单绑定
                        var treeData = Project.trees;
                        if (treeData == null || treeData.length == 0) {
                            $('#treetable').html("<p>暂无菜单</p>");
                        }
                        else
                        {
                            TreeLoad(treeData);
                        }
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

        $('#ProjectPop').slideDown();
    }

    //var Add = function () {
    //    $("p[name='RoleTitle']").html('角色新增');
    //    $("input[name='ID']").val('0');
    //    $("input[name='txt_RoleNamePop']").val('');
    //    $("input[name='txt_RoleCodePop']").val('');
    //    $("input[name='txt_RoleNamePop']").prop('readonly', false);

    //    popupOpen();

    //    $('#RolePop').slideDown();
    //}


    var TreeLoad = function (treeJson) {

        $("#treetable").fancytree({
            //extensions: ["dnd5", "edit", "glyph", "table"],
            extensions: ["glyph", "table"],
            checkbox: true,
            selectMode: 3,
            glyph: glyph_opts,
            source: treeJson,
            //source: treeData,
            table: {
                //菜单缩进，默认16
                indentation: 25,
                //菜单名称table表列序号
                nodeColumnIdx: 1
            },
            icon: false,
            renderColumns: function (event, data) {
                var node = data.node,
                    $tdList = $(node.tr).find(">td");
                $tdList.eq(0).text(node.key);
            }
        });
        var tree = $("#treetable").fancytree("getTree");
        tree.reload(treeJson);
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

    var glyph_opts = {
        preset: "bootstrap3",
        map: {
            expanderClosed: "glyphicon glyphicon-plus",
            expanderOpen: "glyphicon glyphicon-minus"
        }
    };

    function IsMenuArrNode(node) {
        var ret = false;

        if (node.partsel || node.selected) {
            ret = true;
        }

        return ret;
    }

    function GetMenuArr(tree) {
        var menuArr = new Array();
        var nodes = new Array();

        nodes = tree.findAll(IsMenuArrNode);
        nodes.forEach(function (node) { menuArr.push(parseInt(node.key)) });

        return menuArr.sort(function (a, b) { return a - b });
    }


    //var Delete = function (dom) {

    //    if (dom == null) {
    //        return;
    //    }
    //    var ID = $(dom).attr('data-id');
    //    var Name = $(dom).attr('data-name');
    //    OpenDeleteModal(Name);


    //    $("#myModal").modal('show');

    //    $("#ModalSubmit").bind('click', function () {
    //        //打开遮罩层

    //        var tempPara = {};
    //        tempPara.ID = ID;

    //        CallMethod('DataDelete', tempPara);
    //    })

    //    $("#ModalClose").bind('click', function () {
    //        CloseModalEvent();
    //    })

    //}

    ///获取编辑页数据
    function ReadDataFromPopup() {

        var nodeInfo = {};
        var tree = $("#treetable").fancytree("getTree");

        nodeInfo.ID = $("input[name='ID']").val();
        nodeInfo.ProjectName = $("input[name='txt_ProjectNamePop']").val();
        nodeInfo.ShortName = $("input[name='txt_ShortNamePop']").val();
        nodeInfo.MenuIDs = GetMenuArr(tree);

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
                        DoRoleQuery();
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
        //Add: Add,
        //Delete: Delete,
        Save: Save,
        TurnBack: TurnBack,
        field: {

        }

    }

}(jQuery);
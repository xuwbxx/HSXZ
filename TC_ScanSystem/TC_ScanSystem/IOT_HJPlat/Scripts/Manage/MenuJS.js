var MenuJS = function ($) {
    $(document).ready(function () {
        GetMenuTreeView();
    })

    var glyph_opts = {
        preset: "bootstrap3",
        map: {
            expanderClosed: "glyphicon glyphicon-plus",
            expanderOpen: "glyphicon glyphicon-minus"
        }
    };

    var GetMenuTreeView = function () {
        var tree = {};
        OpenMaskPopUp();

        $.post('DataQuery', tree)
            .success(function (data, statusText, jqXRH) {
                if (data.Success) {
                    var menuList = data.Data;

                    if (menuList == null || menuList.length == 0) {
                        $('#treetable').html("<p>暂无菜单</p>")
                    }
                    else {

                        $("#treetable").fancytree({
                            //extensions: ["dnd5", "edit", "glyph", "table"],
                            extensions: ["glyph", "table"],
                            checkbox: false,
                            selectMode: 3,
                            glyph: glyph_opts,
                            source: menuList,
                            table: {
                                //菜单缩进，默认16
                                indentation: 25,
                                //菜单名称table表列序号
                                nodeColumnIdx: 1
                            },
                            icon: false,
                            renderNode: function (event, data) {
                                var node = data.node;
                                var $nodeSpan = $(node.span);

                                if (!$nodeSpan.data('rendered')) {

                                    var addBtn = $('<button type="button" class="glyphicon glyphicon-plus-sign" style="background:none; border:none; height:20px; margin-left:10px;"  onclick="MenuJS.ShowAddPopup(this);"></button>');
                                    var editBtn = $('<button type="button" class="glyphicon glyphicon-pencil" style="background:none; border:none; height:20px; margin-left:10px;" onclick="MenuJS.ShowEidtPopup()"></button>');
                                    var deleteBtn = $('<button type="button" class="glyphicon glyphicon-minus-sign" style="background:none; border:none; height:20px; margin-left:10px;" onclick="MenuJS.DeleteConfirm()"></button>');

                                    $nodeSpan.append(addBtn);
                                    $nodeSpan.append(editBtn);
                                    $nodeSpan.append(deleteBtn);

                                    addBtn.hide();
                                    editBtn.hide();
                                    deleteBtn.hide();

                                    $nodeSpan.hover(function () {
                                        // mouse over
                                        addBtn.show();
                                        editBtn.show();
                                        deleteBtn.show();

                                    }, function () {

                                        // mouse out
                                        addBtn.hide();
                                        editBtn.hide();
                                        deleteBtn.hide();
                                    })

                                    $nodeSpan.data('rendered', true);
                                }
                            },
                            renderColumns: function (event, data) {
                                var node = data.node,
                                    $tdList = $(node.tr).find(">td");
                                $tdList.eq(0).text(node.key);
                                $tdList.eq(2).text(node.data.Path);
                                //$tdList.eq(3).text(node.data.Depth);
                                //$tdList.eq(4).text(node.getIndexHier());
                                $tdList.eq(3).text(node.data.Order);
                            }
                        });
                        var tree = $("#treetable").fancytree("getTree");
                        tree.reload(menuList);
                        //tree.expandAll();

                    }

                }
            })
            .fail(function (jqXHR, statusText, errorMessage) {
                //console.error(arguments);
            })
            .always(function (data) {
                // 关闭等待遮罩
                CloseMaskPopUp();
                return tree;
            });

    }


    var ShowAddPopup = function (dom) {

        var node = {};

        if (dom.id && dom.id == "btnRoot") {
            node = $("#treetable").fancytree("getTree").rootNode;
        }
        else {
            node = $("#treetable").fancytree("getTree").focusNode;
        }
        var level = node.getLevel();
        if (level == 2) {
            OpenModalContentTips('失败', "目录层级不能大于两层！", 'fail');
            $("#myModal").modal('show');
        }
        else {
            var ConfigInfo = InitConfigData(node);
            $("p[name='MenuTitle']").html('新增');
            SetDataToPopup(ConfigInfo)

            popupOpen();

            $('#MenuPop').slideDown();
        }
    }

    var ShowEidtPopup = function () {

        var nodeInfo = {};
        var node = $("#treetable").fancytree("getTree").focusNode;

        nodeInfo = ReadDataFromNode(node);

        SetDataToPopup(nodeInfo);

        popupOpen();

        $('#MenuPop').slideDown();
    }

    var DeleteConfirm = function (dom) {

        var node = $("#treetable").fancytree("getTree").focusNode;
        OpenDeleteModal(node.title);

        $("#myModal").modal('show');

        $("#ModalSubmit").bind('click', function () {
            //打开遮罩层

            var tempPara = {};
            tempPara.ID = node.key;

            CallMethod('Delete', tempPara);
        })

        $("#ModalClose").bind('click', function () {
            CloseModalEvent();
        })

    }

    ///获取编辑页数据
    function ReadDataFromPopup() {

        var nodeInfo = {};

        nodeInfo.ID = $("input[name='ID']").val();
        nodeInfo.MenuName = $("input[name='txt_MenuNamePop']").val();
        nodeInfo.Path = $("input[name='txt_PathPop']").val();
        nodeInfo.Depth = $("input[name='txt_DepthPop']").val();
        nodeInfo.ParentID = $("input[name='ParentID']").val();
        nodeInfo.Order = $("input[name='txt_OrderPop']").val();
        nodeInfo.Class = $("input[name='txt_ClassPop']").val();
        nodeInfo.FunctionCode = $("input[name='txt_FunctionCodePop']").val();

        return nodeInfo;
    }

    var ReadDataFromNode = function (node) {

        var nodeInfo = {}

        nodeInfo.ID = node.key;
        nodeInfo.MenuName = node.title;
        nodeInfo.Path = node.data.Path;
        nodeInfo.FunctionCode = node.data.FunctionCode;
        nodeInfo.Class = node.data.Class;
        nodeInfo.Order = node.data.Order;
        //nodeInfo.ParentID = node.parent.key;
        if (node.parent.key === $("#treetable").fancytree("getTree").rootNode.key) {
            nodeInfo.ParentID = 0;
        }
        else {
            nodeInfo.ParentID = node.parent.key;
        }
        nodeInfo.Depth = node.getLevel();

        return nodeInfo;
    }

    function InitConfigData(parentNode) {

        var nodeInfo = {};

        nodeInfo.ID = 0;
        nodeInfo.MenuName = "";
        nodeInfo.Path = "";
        nodeInfo.Depth = parentNode.getLevel() + 1;
        //if (parentNode.key != $("#treetable").fancytree("getTree").rootNode.key) {
        //    nodeInfo.ParentID = parentNode.key;
        //}
        nodeInfo.ParentID = parentNode.key;
        nodeInfo.Class = "";
        nodeInfo.Order = "";
        nodeInfo.FunctionCode = "";

        return nodeInfo;
    }

    ///设置编辑页数据
    var SetDataToPopup = function (nodeInfo) {

        $("input[name='ID']").val(nodeInfo.ID);
        $("input[name='txt_MenuNamePop']").val(nodeInfo.MenuName);
        $("input[name='txt_PathPop']").val(nodeInfo.Path);
        //$("input[name='txtDepth']").val(nodeInfo.Depth);
        $("input[name='ParentID']").val(nodeInfo.ParentID);
        $("input[name='txt_ClassPop']").val(nodeInfo.Class);
        $("input[name='txt_OrderPop']").val(nodeInfo.Order);
        $("input[name='txt_FunctionCodePop']").val(nodeInfo.FunctionCode);

    }

    var SaveConfigInfo = function () {

        var ConfigInfoEntity = ReadDataFromPopup();

        OpenSaveModal()
        $("#myModal").modal('show');

        $("#ModalSubmit").bind('click', function () {

            if (ConfigInfoEntity.ID > 0) {
                CallMethod('Modify', ConfigInfoEntity);
            }
            else if (ConfigInfoEntity.ID == 0) {
                CallMethod('Add', ConfigInfoEntity);
            }
        })
    }

    var CallMethod = function (methodName, methodParam) {
        var ret = {};
        //打开遮罩层
        OpenMaskLayout();

        $.post(methodName, methodParam)
            .success(function (data, statusText, jqXRH) {
                if (data) {
                    if (data.Success) {
                        OpenModalSuccessTips();
                        $("#myModal").modal('show');

                        $("#ModalClose").bind('click', function () {
                            CloseModalEvent();
                        })
                        ret = data;
                        GetMenuTreeView();
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

    }

    var TurnBack = function () {

        $('.popup-close').click();
    }

    return {
        HomeSiteInit: function () {
        },
        GetMenuTreeView: GetMenuTreeView,
        ShowEidtPopup: ShowEidtPopup,
        ShowAddPopup: ShowAddPopup,
        DeleteConfirm: DeleteConfirm,
        SaveConfigInfo: SaveConfigInfo,
        TurnBack: TurnBack,
        field: {

        }
    }
}(jQuery);
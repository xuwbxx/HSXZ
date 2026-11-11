var SafetyHatJS = function ($) {

    var Param = {};

    $(document).ready(function () {

        Init();

        ParamInit();

        Query();

    })

    var ParamInit = function () {

        Param.PageIndex = 1;
        Param.PageSize = 10;
        Param.TotalPage = 0;
        Param.TotalCount = 0;


        Param.Begin = $("input[name='Begin']").val();
        Param.End = $("input[name='End']").val();
    }

    var Init = function () {


    }

    var Query = function () {


        OpenMaskLayout();

        $.post('DataQuery', Param)
                .success(function (data, statusText, jqXRH) {
                    if (data) {

                        if (data.Success) {
                            var Model = {};
                            Model.list = data.Data;
                            var html = template("SafetyHatHtmlTemplate", Model);
                            $("#SafetyHatHtml").html(html);

                            //分页
                            Param.TotalPage = data.TotalPage;
                            Param.TotalCount = data.TotalCount;
                            Param.PageIndex = data.PageIndex;

                            //Pic_HoverBind();
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

        Param.Begin = $("input[name='Begin']").val();
        Param.End = $("input[name='End']").val();
        Query();
    }

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



    return {
        HomeSiteInit: function () {
            Init();
        },
        DoQuery: DoQuery,
        field: {

        }
    }

}(jQuery);
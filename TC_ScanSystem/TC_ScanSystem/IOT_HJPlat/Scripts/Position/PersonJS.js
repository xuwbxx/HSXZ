var PersonJS = function ($) {

    var map;

    var CavePositionInfo;

    $(document).ready(function () {

        setTimeout(MapLoad, 1000);

    });

    var MapInit = function () {
        map = new BMap.Map("allmap");    // 创建Map实例
        var point = new BMap.Point(118.66872792882400, 32.05649817066350);//中心点
        map.centerAndZoom(point, 15);  // 初始化地图,设置中心点坐标和地图级别
        //map.enableScrollWheelZoom(true);     //开启鼠标滚轮缩放

        ToolBuild();
    }

    var MapLoad = function () {

        $("div#allmap").css('height', window.innerHeight - 180);

        MapInit();

        //CustomControlBuild();

        //读取数据
        OpenMaskLayout();

        var Param = {};

        $.post('DataQuery2', Param)
                .success(function (data, statusText, jqXRH) {
                    if (data) {
                        if (data.Success) {
                            CavePositionInfo = data.Data.Caves;
                            CaveMapBuild(data.Data.Caves);
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

    //工具栏
    var ToolBuild = function () {

        var top_left_control = new BMap.ScaleControl({ anchor: BMAP_ANCHOR_TOP_LEFT });// 左上角，添加比例尺
        var top_left_navigation = new BMap.NavigationControl();  //左上角，添加默认缩放平移控件
        map.addControl(top_left_control);
        map.addControl(top_left_navigation);
        //添加地图类型控件
        //map.addControl(new BMap.MapTypeControl({
        //    mapTypes: [
        //        BMAP_NORMAL_MAP,
        //        BMAP_HYBRID_MAP
        //    ]
        //}));
    }

    //生成自定义按键
    function ZoomControl() {
        // 默认停靠位置和偏移量
        this.defaultAnchor = BMAP_ANCHOR_TOP_RIGHT;
        this.defaultOffset = new BMap.Size(10, 10);
    }

    var CustomControlBuild = function () {

        // 通过JavaScript的prototype属性继承于BMap.Control
        ZoomControl.prototype = new BMap.Control();

        // 自定义控件必须实现自己的initialize方法,并且将控件的DOM元素返回
        // 在本方法中创建个div元素作为控件的容器,并将其添加到地图容器中
        ZoomControl.prototype.initialize = function (map) {
            // 创建一个DOM元素
            var input = document.createElement("input");
            // 添加文字说明
            input.type = "button";
            input.value = "返回工区";
            // 设置样式
            input.style.cursor = "pointer";
            input.style.border = "1px solid gray";
            input.style.backgroundColor = "white";
            // 绑定事件,点击一次放大两级
            input.onclick = function (e) {
                //map.setZoom(map.getZoom() + 2);
                //alert('111111111');
                MapInit();
                CaveMapBuild(CavePositionInfo);
            }
            // 添加DOM元素到地图中
            map.getContainer().appendChild(input);
            // 将DOM元素返回
            return input;
        }
        // 创建控件
        var myZoomCtrl = new ZoomControl();
        // 添加到地图当中
        map.addControl(myZoomCtrl);
    }


    //工点生成
    var CaveMapBuild = function (Caves) {
        if (Caves == null || Caves.length == 0) {
            return;
        }

        $(Caves).each(function (index, dom) {
            var PersonNum = dom.Persons.length;
            var CenterPoint = new BMap.Point(dom.CenterLon, dom.CenterLat);
            //画工区圆
            CircieBuild(CenterPoint, dom.CaveName, PersonNum);
        });
    }

    //画工区
    var CircieBuild = function (CirclePoint, AreaName, Number) {
        //画圈
        var circle = new BMap.Circle(CirclePoint, 500, { strokeWeight: 2, strokeOpacity: 0.3, fillColor: "#6a9ff6" }); //创建圆
        //生成主键
        $(CavePositionInfo).each(function (index, dom) {
            if (dom.CaveName == AreaName) {
                dom.guid = circle.guid;
            }
        })


        map.addOverlay(circle);
        //添加事件
        circle.addEventListener("click", OpenWorkCave);


        //标记工区名字
        var CircleLabel = new BMap.Label(AreaName, {
            position: CirclePoint,    // 指定文本标注所在的地理位置
            offset: new BMap.Size(-35, -15)    //设置文本偏移量
        });  // 创建文本标注对象
        CircleLabel.setStyle(CircleCaveLabelStyle);
        CircleLabel.addEventListener("click", OpenLabelWorkCave);
        map.addOverlay(CircleLabel);

        //标记工区人数
        var NumberLabel = new BMap.Label(Number + " 人", {
            position: CirclePoint,    // 指定文本标注所在的地理位置
            offset: new BMap.Size(-15, 5)    //设置文本偏移量
        });  // 创建文本标注对象
        NumberLabel.setStyle(CircleNumLabelStyle);
        NumberLabel.addEventListener("click", OpenLabelWorkCave);
        map.addOverlay(NumberLabel);
    }

    //label打开详情
    var OpenLabelWorkCave = function (e) {
        //alert('12345');
        var Label = e.target;
        //找到CaveInfo
        var CircleInfo;
        $(CavePositionInfo).each(function (index, dom) {
            if (dom.CenterLon == Label.point.lng && dom.CenterLat == Label.point.lat) {
                CircleInfo = dom;
            }
        })
        if (CircleInfo == null)
            return;

        //显示人员定位
        DisplayPersonPosition(CircleInfo);
    }


    //Circle打开工区详情
    var OpenWorkCave = function (e) {
        var circle = e.target;
        var CircleInfo;
        $(CavePositionInfo).each(function (index, dom) {
            if (dom.guid == circle.guid) {
                CircleInfo = dom;
            }
        })
        if (CircleInfo == null)
            return;

        //显示人员定位
        DisplayPersonPosition(CircleInfo);

    }

    var DisplayPersonPosition = function (CaveInfo) {
        //map.reset();
        map = new BMap.Map("allmap");    // 创建Map实例
        var point = new BMap.Point(CaveInfo.CenterLon, CaveInfo.CenterLat);//中心点
        map.centerAndZoom(point, 18);  // 初始化地图,设置中心点坐标和地图级别
        ToolBuild();

        //返回工区按钮
        CustomControlBuild();

        //画工区
        var rectangle = new BMap.Polygon([
            new BMap.Point(CaveInfo.LonMin, CaveInfo.LatMax),
            new BMap.Point(CaveInfo.LonMax, CaveInfo.LatMax),
            new BMap.Point(CaveInfo.LonMax, CaveInfo.LatMin),
            new BMap.Point(CaveInfo.LonMin, CaveInfo.LatMin)
        ], PolygonStyle);  //创建矩形
        map.addOverlay(rectangle);         //增加矩形

        //画工人
        if (CaveInfo.Persons.length == 0)
            return;

        $(CaveInfo.Persons).each(function (index, dom) {
            var PersonPoint = new BMap.Point(dom.Lon, dom.Lat);
            var myIcon = new BMap.Icon("/Content/IOT/img/map-people.png", new BMap.Size(32, 40)
            );
            //var content = dom.PersonName;
            //var content = "<a href='#'>" + dom.PersonName + "</a>";
            var content = "<ul><li>姓名：" + dom.PersonName + "</li><li>岗位：" + dom.UnitName + "</li></ul>";
            var name = dom.PersonName;
            addCustomMarker(PersonPoint, myIcon, content, name);
        });


    }

    //添加自定义图标
    var addCustomMarker = function (point, Icon, content, name) {
        var marker = new BMap.Marker(point, { icon: Icon });  // 创建标注

        var label = new BMap.Label(name, { offset: new BMap.Size(-10, -30) });
        label.setStyle(labelStyle);
        marker.setLabel(label);

        map.addOverlay(marker);
        addClickHandler(content, marker);
    }

    //弹出信息窗口
    function addClickHandler(content, marker) {
        marker.addEventListener("click", function (e) {
            openInfo(content, e)
        }
        );

    }
    function openInfo(content, e) {
        var p = e.target;
        var point = new BMap.Point(p.getPosition().lng, p.getPosition().lat);
        var infoWindow = new BMap.InfoWindow(content, PopStyle);  // 创建信息窗口对象 
        map.openInfoWindow(infoWindow, point); //开启信息窗口

        var num = map.getZoom();
    }

    var CircleCaveLabelStyle = {
        color: "white",
        fontSize: "16px",
        height: "20px",
        lineHeight: "20px",
        fontFamily: "微软雅黑",
        border: "0",
        fontWeight: "bold",
        background: 'none',
        cursor: "pointer"
    };
    var CircleNumLabelStyle = {
        color: "green",
        fontSize: "16px",
        height: "20px",
        lineHeight: "20px",
        fontFamily: "微软雅黑",
        border: "0",
        fontWeight: "bold",
        background: 'none',
        cursor: "pointer"
    };

    var PolygonStyle = {
        strokeColor: 'blue',//边框颜色
        fillColor: 'RGBA(0,0,255)',//填充颜色
        strokeWeight: 0.5,//边框粗细
        strokeOpacity: 0.5,//边框透明度
        fillOpacity: 0.1,//填充透明度
        strokeStyle: 'dash'
    };

    var labelStyle = {

        color: "#005db2 ", //字体颜色
        fontSize: "14px",//字体大小 　
        lineHeight: "20px",
        marginBottom: "4px",
        height: "26px",
        width: "50px",
        display: "block",
        textAlign: 'center',
        border: "1.5px solid #0b8aff ",
        borderRadius: "3px",
        backgroundColor: "0.05", //文本标注背景颜色　
        //border: "solid",
        //borderColor: "green",
        //fontWeight: "bold" //字体加粗

    };

    var PopStyle = {
        width: 250,     // 信息窗口宽度
        height: 80,     // 信息窗口高度
        title: "人员信息", // 信息窗口标题
        enableMessage: true//设置允许信息窗发送短息
    };

    return {
        HomeSiteInit: function () {
            //Init();
        },
        field: {

        }
    }

}(jQuery);
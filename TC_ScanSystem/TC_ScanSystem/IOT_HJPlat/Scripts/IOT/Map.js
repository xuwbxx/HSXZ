var Main_MapJS = function ($) {
    var Param = {};

    $(document).ready(function () {

        Init();

    })

    var Init = function () {

        var map = new BMap.Map("container", { mapType: BMAP_HYBRID_MAP });
        //var map = new BMap.Map("container");
        var point = new BMap.Point(118.672906,32.062485);
        var top_left_control = new BMap.ScaleControl({ anchor: BMAP_ANCHOR_TOP_LEFT });// 左上角，添加比例尺
        var top_left_navigation = new BMap.NavigationControl();  //左上角，添加默认缩放平移控件
        map.addControl(top_left_control);
        map.addControl(top_left_navigation);

        var mapType1 = new BMap.MapTypeControl(
		{
		    mapTypes: [BMAP_NORMAL_MAP, BMAP_HYBRID_MAP],
		    anchor: BMAP_ANCHOR_TOP_RIGHT
		}
	);
        map.addControl(mapType1);          //2D图，混合图

        map.centerAndZoom(point, 15);
        //map.addControl(new BMap.NavigationControl());
        //map.addControl(new BMap.MapTypeControl());
        //var marker = new BMap.Marker(point);
        //map.addOverlay(marker);
        var marker = new BMap.Marker(point);  // 创建标注
        map.addOverlay(marker);               // 将标注添加到地图中
        marker.setAnimation(BMAP_ANIMATION_BOUNCE); //跳动的动画


    }


    return {
        HomeSiteInit: function () {
            Init();
        },
        field: {

        }

    }

}(jQuery);
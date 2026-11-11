using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Echarts
{
    public class LineEchart : EchartBase
    {
        //折线图初始化(因为多处用到，在这里不可轻易改动！请在调用的地方自行改动)
        public LineEchart()
        {
            //title
            title = new title();

            //tooltip
            tooltip = new tooltip();
            tooltip.trigger = "item";

            //legend
            legend = new legend();
            legend.data = new List<string>();
            legend.orient = "horizontal";
            legend.left = "center";

            //toolbox
            toolbox = new toolbox();
            toolbox.show = true;
            toolbox.feature = new toolbox_feature();
            toolbox.feature.dataZoom = new toolbox_feature_dataZoom();
            toolbox.feature.dataView = new toolbox_feature_dataView();
            toolbox.feature.dataView.readOnly = false;
            toolbox.feature.magicType = new toolbox_feature_magicType();
            toolbox.feature.magicType.type = new List<string>();
            toolbox.feature.magicType.type.AddRange(new List<string> { "line", "bar" });
            toolbox.feature.restore = new toolbox_feature_restore();
            toolbox.feature.saveAsImage = new toolbox_feature_saveAsImage();

            //xAxis
            xAxis = new xAxis();
            xAxis.type = "category";
            xAxis.axisLine = new axisLine();
            xAxis.axisLine.symbol = new List<string>();
            xAxis.show = true;
            xAxis.axisLine.symbol.Add("none");
            xAxis.axisLine.symbol.Add("arrow");
            xAxis.axisLine.show = true;
            xAxis.boundaryGap = false;
            xAxis.data = new List<string>();
            xAxis.name = "X轴";
          //  xAxis.nameLocation = "end";

            //yAxis
            yAxis = new yAxis();
            yAxis.type = "value";
            yAxis.axisLabel = new yAxis_axisLabel();
            yAxis.show = true;
            yAxis.axisLine = new axisLine();
            yAxis.axisLine.symbol = new List<string>();
            yAxis.axisLine.symbol.Add("none");
            yAxis.axisLine.symbol.Add("arrow");
            yAxis.axisLine.show = true;
            yAxis.name = "Y轴";
            yAxis.nameLocation = "end";
            yAxis.nameGap = 15;

            //series
            series = new List<Echarts.series>();

            dataZoom = new dataZoom();
            dataZoom.type = "inside";

        }
    }
}

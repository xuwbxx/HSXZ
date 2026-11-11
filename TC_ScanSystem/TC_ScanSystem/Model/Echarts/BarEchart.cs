using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Echarts
{
    public class BarEchart:EchartBase
    {
        public BarEchart()
        {
            //title
            title = new title();

            //tooltip
            tooltip = new tooltip();
            tooltip.trigger = "axis";

            //legend
            legend = new legend();
            legend.data = new List<string>();

            //toolbox
            toolbox = new toolbox();
            toolbox.show = true;
            toolbox.feature = new toolbox_feature();
            toolbox.feature.dataZoom = new toolbox_feature_dataZoom();
            toolbox.feature.dataView = new toolbox_feature_dataView();
            toolbox.feature.dataView.readOnly = false;
            toolbox.feature.magicType = new toolbox_feature_magicType();
            toolbox.feature.magicType.type = new List<string>();
            toolbox.feature.magicType.type.AddRange(new List<string> { "bar" });
            toolbox.feature.restore = new toolbox_feature_restore();
            toolbox.feature.saveAsImage = new toolbox_feature_saveAsImage();

            //xAxis
            xAxis = new xAxis();
            xAxis.type = "category";
            xAxis.boundaryGap = true;
            xAxis.data = new List<string>();

            //yAxis
            yAxis = new yAxis();
            yAxis.type = "value";
            yAxis.axisLabel = new yAxis_axisLabel();

            //series
            series = new List<Echarts.series>();

        }
    }
}

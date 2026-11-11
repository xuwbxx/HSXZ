using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Echarts
{
    public class PieEchart : EchartBase
    {
        //折线图初始化(因为多处用到，在这里不可轻易改动！请在调用的地方自行改动)
        public PieEchart()
        {
            //title
            title = new title();

            //tooltip
            tooltip = new tooltip();
            tooltip.trigger = "item";
            tooltip.formatter = "{a}<br/>{b}:{c}({d}%)";

            //legend
            legend = new legend();
            legend.x = "left";
            legend.orient = "vertical";
            legend.data = new List<string>();

            //toolbox
            toolbox = new toolbox();
            toolbox.show = true;
            //toolbox.feature = new toolbox_feature();
            //toolbox.feature.mark = new toolbox_feature_mark();
            //toolbox.feature.mark.show = true;
            //toolbox.feature.dataView = new toolbox_feature_dataView();
            //toolbox.feature.dataView.show = true;

            //toolbox.feature.magicType = new toolbox_feature_magicType();
            //toolbox.feature.magicType.type = new List<string>();
            //toolbox.feature.magicType.type.AddRange(new List<string> { "pie", "funnel" });
            //toolbox.feature.magicType.show = true;
            //toolbox.feature.magicType.option = new magicTypeOption();
            //toolbox.feature.magicType.option.funnel = new magicTypeOptionFunnel();
            //toolbox.feature.magicType.option.funnel.x = "25%";
            //toolbox.feature.magicType.option.funnel.max = 1548;
            //toolbox.feature.magicType.option.funnel.width = "50%";
            //toolbox.feature.magicType.option.funnel.funnelAlign = "center";
            //toolbox.feature.restore = new toolbox_feature_restore();
            //toolbox.feature.restore.show = true;
            //toolbox.feature.saveAsImage = new toolbox_feature_saveAsImage();
            //toolbox.feature.saveAsImage.show = true;

            //xAxis
            //xAxis = new xAxis();
            //xAxis.type = "category";
            //xAxis.boundaryGap = false;
            //xAxis.data = new List<string>();

            //yAxis
            //yAxis = new yAxis();
            //yAxis.type = "value";
            //yAxis.axisLabel = new yAxis_axisLabel();

            //series
            series = new List<Echarts.series>();

            calculable = true;
        }
    }
}

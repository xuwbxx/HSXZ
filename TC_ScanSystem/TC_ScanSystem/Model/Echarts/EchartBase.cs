using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Echarts
{
    /// <summary>
    /// 注意：这里是Echarts基础项，这里不要添加构造函数，因为不是所有继承项都会用到这个构造函数中的属性
    /// 请在继承类中自行添加所需要的属性
    /// </summary>
    public class EchartBase
    {
        public title title { set; get; }

        public tooltip tooltip { set; get; }

        public legend legend { set; get; }

        public toolbox toolbox { set; get; }

        public xAxis xAxis { set; get; }

        public yAxis yAxis { set; get; }

        public bool calculable { set; get; }

        public grid grid { set; get; }

        public List<series> series { set; get; }

        public dataZoom dataZoom { set; get; }
    }

    #region title
    public class title
    {
        public string text { set; get; }

        public string subtext { set; get; }

        public string x { set; get; }

        public string align { set; get; }

    }
    #endregion

    #region tooltip
    public class tooltip
    {
        public string trigger { set; get; }

        public string formatter { set; get; }
    }
    #endregion

    #region legend
    public class legend
    {
        public string x { set; get; }
        public string orient { set; get; }
        public List<string> data { set; get; }

        public string left { set; get; }
    }
    #endregion

    #region toolbox
    public class toolbox
    {
        public bool show { set; get; }
        
        public toolbox_feature feature { set; get; }
    }

    public class toolbox_feature
    {
        /// <summary>
        /// 数据区域缩放。
        /// </summary>
        public toolbox_feature_dataZoom dataZoom { set; get; }

        public toolbox_feature_mark mark { set; get; }

        public toolbox_feature_dataView dataView { set; get; }

        public toolbox_feature_magicType magicType { set; get; }

        public toolbox_feature_restore restore { set; get; }

        public toolbox_feature_saveAsImage saveAsImage { set; get; }
    }

    public class toolbox_feature_mark
    {
        public bool show { set; get; }
    }

    public class toolbox_feature_dataZoom
    {
        /// <summary>
        /// 指定哪些 yAxis 被控制。如果缺省则控制所有的y轴。如果设置为 false 则不控制任何y轴。
        /// 如果设置成 3 则控制 axisIndex 为 3 的y轴。如果设置为 [0, 3] 则控制 axisIndex 为 0 和 3 的y轴。
        /// </summary>
        public string yAxisIndex { set; get; }
    }

    public class toolbox_feature_dataView
    {
        public bool show { set; get; }
        public bool readOnly { set; get; }
    }

    public class toolbox_feature_magicType
    {
        public bool show { set; get; }
        public List<string> type { set; get; }

        public magicTypeOption option { set; get; }
    }

    public class magicTypeOption
    {
        public magicTypeOptionFunnel funnel { set; get; }
    }

    public class magicTypeOptionFunnel
    {
        public string x { set; get; }

        public string width { set; get; }

        public string funnelAlign { set; get; }

        public decimal max { set; get; }
    }



    public class toolbox_feature_restore
    {
        public bool show { set; get; }
    }

    public class toolbox_feature_saveAsImage
    {
        public bool show { set; get; }
    }
    #endregion

    #region xAxis
    public class xAxis
    {
        public string type { set; get; }

        public bool show { set; get; }

        public bool boundaryGap { set; get; }

        public List<string> data { set; get; }

        public axisLine axisLine { set; get; }

        public string name { set; get; }

        //public string nameLocation { set; get; }
    }

    public class axisLine
    {
        public bool show { set; get; }
        public List<string> symbol { set; get; }
    }

    #endregion

    #region grid
    public class grid
    {
        public string left { set; get; }

        public string right { set; get; }

        public string bottom { set; get; }

        /// <summary>
        /// grid 区域是否包含坐标轴的刻度标签。
        /// </summary>
        public bool containLabel { set; get; }
    }
    #endregion

    #region yAxis
    public class yAxis
    {
        public string type { set; get; }

        public yAxis_axisLabel axisLabel { set; get; }

        public axisLine axisLine { set; get; }

        public string name { set; get; }

        public bool show { set; get; }

        public string nameLocation { set; get; }

        public int nameGap { set; get; }
    }

    public class yAxis_axisLabel
    {
        public string formatter { set; get; }
    }
    #endregion

    #region series
    public class series
    {
        public series()
        {
            data = new List<object>();
            markPoint = new series_markPoint();
           // markLine = new series_markLine();
        }
        public string name { set; get; }

        public string type { set; get; }

        public List<object> data { set; get; }

        public series_markPoint markPoint { set; get; }

        //public series_markLine markLine { set; get; }

        public List<string> radius { set; get; }

        public bool smooth { set; get; }

        public itemStyle itemStyle { set; get; }

        
    }

    public class emphasis
    {
        public emphasisLabel label { set; get; }
    }

    public class emphasisLabel
    {
        public bool show { set; get; }

        public string position { set; get; }

        public textStyle textStyle { set; get; }
    }

    public class textStyle
    {
        public string fontSize { set; get; }

        public string fontWeight { set; get; }
    }

    public class itemStyle
    {
        public itemStyleNormal normal { set; get; }

        public emphasis emphasis { set; get; }
    }



    public class itemStyleNormal
    {
        public string color { set; get; }

        public itemStyleNormalLabel label { set; get; }

        public itemStyleNormallabelLine labelLine { set; get; }

    }

    public class itemStyleNormalLabel
    {
        public bool show { set; get; }
    }

    public class itemStyleNormallabelLine
    {
        public bool show { set; get; }
    }

    public class series_markPoint
    {
        public series_markPoint()
        {
            series_markPoint_data max = new series_markPoint_data();
            max.type = "max";
            max.name = "最大值";
            series_markPoint_data min = new series_markPoint_data();
            min.type = "min";
            min.name = "最小值";

            data = new List<series_markPoint_data>();
            data.Add(max);
            data.Add(min);
            symbolSize=new List<int>();
            symbolSize.Add(50);
            symbolSize.Add(25);
            symbol = "circle";
        }
        public List<series_markPoint_data> data { set; get; }
        public List<int> symbolSize { set; get; }
        public string symbol { set; get; }
    }

    public class series_markPoint_data
    {
        public string type { set; get; }
        public string name { set; get; }
    }

    public class series_markLine
    {
        public series_markLine()
        {
            series_markLine_data aver = new series_markLine_data();
            aver.type = "average";
            aver.name = "平均值";

            data = new List<series_markLine_data>();
            data.Add(aver);
        }
        public List<series_markLine_data> data { set; get; }
    }

    public class series_markLine_data
    {
        public string type { set; get; }
        public string name { set; get; }
    }

    public class SerieData
    {
        public decimal value { set; get; }

        public string name { set; get; }
    }

    #endregion

    #region dataZoom

    public class dataZoom
    {
        public string type { set; get; }
    }

    #endregion
}

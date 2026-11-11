using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.HJPlat.Response
{
    public class GantryCraneStatusResponse
    {
        public GantryCraneStatusResponse()
        {
            success = false;
            Cart = new CraneCart();
            Hook = new CraneHook();
            Weight = new CraneWeight();
            Tilt = new CraneTilt();
            Wind = new CraneWind();
            Duration = new WorkDuration();
            Clamp = new CraneClamp();
            Dolly = new CraneDolly();
            Door = new CraneDoor();
            Leg = new CraneLeg();
        }

        public string Mac { set; get; }

        public bool success { set; get; }

        public CraneCart Cart { set; get; }

        public CraneHook Hook { set; get; }

        public CraneWeight Weight { set; get; }

        public CraneTilt Tilt { set; get; }

        public CraneWind Wind { set; get; }

        public WorkDuration Duration { set; get; }

        public CraneClamp Clamp { set; get; }

        public CraneDoor Door { set; get; }

        public CraneDolly Dolly { set; get; }

        public CraneLeg Leg { set; get; }

        public string CreateTime { get; set; }
    }

    /// <summary>
    /// 大车A
    /// </summary>
    public class CraneCart
    {
        /// <summary>
        /// S静止, L向左, R向右
        /// </summary>
        public string action { set; get; }

        /// <summary>
        /// 大车位置
        /// </summary>
        public decimal position { set; get; }

        /// <summary>
        /// 单位(米)
        /// </summary>
        public string unit { set; get; }

        /// <summary>
        /// 左限位状态 0:锁定 1:解除
        /// </summary>
        public string LeftLimitStatus { set; get; }


        /// <summary>
        /// 左限位状态 0:锁定 1:解除
        /// </summary>
        public string RightLimitStatus { set; get; }
    }

    /// <summary>
    /// 小车
    /// </summary>
    public class CraneDolly
    {
        /// <summary>
        /// 前限位状态 0:锁定 1:解除
        /// </summary>
        public string FrontLimitStatus { set; get; }

        /// <summary>
        /// 后限位状态 0:锁定 1:解除
        /// </summary>
        public string BackLimitStatus { set; get; }
    }

    /// <summary>
    /// 吊钩B,G
    /// </summary>
    public class CraneHook
    {
        /// <summary>
        /// S静止, U向上, D向下
        /// </summary>
        public string action { set; get; }

        /// <summary>
        /// 吊钩位置
        /// </summary>
        public decimal position { set; get; }

        /// <summary>
        /// 单位(米)
        /// </summary>
        public string unit { set; get; }

        /// <summary>
        /// 吊钩上下速度
        /// </summary>
        public decimal speed { set; get; }

        /// <summary>
        /// 速度单位（米/秒）
        /// </summary>
        public string speedunit { set; get; }
    }

    /// <summary>
    /// 载重D
    /// </summary>
    public class CraneWeight
    {
        public decimal weight { set; get; }

        /// <summary>
        /// 单位(吨)
        /// </summary>
        public string unit { set; get; }
    }

    /// <summary>
    /// 吊钩倾斜E
    /// </summary>
    public class CraneTilt
    {
        /// <summary>
        /// 方向(S是0度,L度数负,R度数正)
        /// </summary>
        public string direction { set; get; }

        /// <summary>
        /// 倾斜值
        /// </summary>
        public decimal Tilt { set; get; }

        /// <summary>
        /// 单位(°)
        /// </summary>
        public string unit { set; get; }

    }

    /// <summary>
    /// 风速F
    /// </summary>
    public class CraneWind
    {
        /// <summary>
        /// 米/秒
        /// </summary>
        public string unit { set; get; }

        /// <summary>
        /// 风速
        /// </summary>
        public decimal speed { set; get; }
    }

    /// <summary>
    /// 工作时间
    /// </summary>
    public class WorkDuration
    {
        /// <summary>
        /// 累计工作时间
        /// </summary>
        public decimal time { set; get; }

        /// <summary>
        /// 分钟
        /// </summary>
        public string unit { set; get; }
    }

    /// <summary>
    /// 夹轨器J
    /// </summary>
    public class CraneClamp
    {
        /// <summary>
        /// 0:锁定 1:解除
        /// </summary>
        public string action { set; get; }
    }


    public class CraneDoor
    {
        /// <summary>
        /// 门限位状态 0:锁定 1:解除
        /// </summary>
        public string action { set; get; }
    }

    public class CraneLeg
    {
        /// <summary>
        /// 支腿限位状态 0:锁定 1:解除
        /// </summary>
        public string action { set; get; }
    }



}

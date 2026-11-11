using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.HJPlat.Response
{
    public class MixingStationResponse
    {
        public int Idx { get; set; }
        public string TaskID { get; set; }
        public string Oper { get; set; }
        public string MixDuration { get; set; }
        public string ActualAmount { get; set; }
        public string DesignAmount { get; set; }
        public string ProduceTime { get; set; }
        public string MixMachineNo { get; set; }
        public string BuildPart { get; set; }
        public string WorkMixingRateNo { get; set; }
        public string IntensityClass { get; set; }
        public string SkeletonSoil { get; set; }
        public string SkeletonSoilRate { get; set; }
        public string BoneFlour { get; set; }
        public string BoneFlourRate { get; set; }
        public string Cement { get; set; }
        public string CementRate { get; set; }
        public string Flyash { get; set; }
        public string FlyashRate { get; set; }
        public string PowderOre { get; set; }
        public string PowderOreRate { get; set; }
        public string Water { get; set; }
        public string WaterRate { get; set; }
        public string Addition { get; set; }
        public string AdditionRate { get; set; }
        public string PowderLot { get; set; }
        public string PowderLotRate { get; set; }
    }
}

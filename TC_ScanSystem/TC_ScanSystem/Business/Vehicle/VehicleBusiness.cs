using DataEntity.Services;
using Model.HJPlat.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Vehicle
{
    public class VehicleBusiness
    {
        WorkService service = new WorkService();
        public List<VehiclePresentResponse> GetVehiclePresent(string areaCode)
        {
            List<VehiclePresentResponse> ret = new List<VehiclePresentResponse>();

            DateTime curDate = DateTime.Now;

            //var source = service.Repository_View_Scan_VehiclePresent.Query(a => a.EnterTime.Value > curDate.AddHours(-12));
            var source = service.Repository_View_Scan_VehiclePresent.Query(a => a.EnterTime.HasValue);

            if (!string.IsNullOrWhiteSpace(areaCode))
            {
                var cave = service.Repository_Base_Cave.Query(a => a.Code == areaCode).FirstOrDefault();
                if (cave != null)
                {
                    var retSource = source.Where(a => a.WorkCaveIdx == cave.ID).OrderByDescending(a => a.EnterTime).ToList();
                    foreach (var item in retSource)
                    {
                        VehiclePresentResponse temp = new VehiclePresentResponse();

                        temp.EnterPlate = item.EnterPlate;
                        temp.CaveName = item.CaveName;
                        temp.EnterTime = string.Format(item.EnterTime.ToString(), "yyyy-MM-dd HH:mm:ss");
                        temp.AreaName = item.AreaName;

                        ret.Add(temp);
                    }
                }
            }

            return ret;
        }
    }
}

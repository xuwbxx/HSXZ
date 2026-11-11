using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using FDScanSystem.DAO.Model;

namespace FDScanSystem.DAO.Scan
{
    public class ServerDAL
    {
        /// <summary>
        /// 获取需要初始化的COM设备列表
        /// </summary>
        /// <returns></returns>
        public static List<COMDeviceModel> ListComDevice()
        {
            Dictionary<string, COMDeviceModel> result = new Dictionary<string, COMDeviceModel>();
            using (IDbConnection conn = ConnectionFactory.Create())
            {
                COMDeviceModel lookup = null;
                string commandText = "SELECT * FROM MK_CommunicationDevice A INNER JOIN MK_Device B on A.ID = B.CommunicationDeviceId where DeviceProtocol=0 or DeviceProtocol=1";
                conn.Query<COMDeviceModel, ScanDevice, COMDeviceModel>(commandText, (comDevice, scanDevice) =>
                 {
                     if (comDevice != null)
                     {
                         if (!result.TryGetValue(comDevice.CommunicationCode, out lookup))
                         {
                             lookup = comDevice;
                             result.Add(lookup.CommunicationCode, lookup);
                         }
                         if (scanDevice != null)
                             lookup.Devices.Add(scanDevice);
                         return lookup;
                     }
                     return null;
                 }).ToList();

            }
            return result.Values.ToList();
        }


        public static COMDeviceModel GetComDevice(int deviceId)
        {
            using (IDbConnection conn = ConnectionFactory.Create())
            {

                string commandText = "SELECT * FROM MK_CommunicationDevice where ID = @id";
                return conn.Query<COMDeviceModel>(commandText, new { id = deviceId }).FirstOrDefault();

            }

        }

        public static ScanTypeModel ScanTypeModel(int ScanTypeId)
        {
            using (IDbConnection conn = ConnectionFactory.Create())
            {
                string commandText = "SELECT * FROM FD_Dict where ID = @ID";
                return conn.Query<ScanTypeModel>(commandText, new { ID = ScanTypeId }).FirstOrDefault();
            }
        }


        public static List<ScanPointModel> ListPoint(int deviceId)
        {

            using (IDbConnection conn = ConnectionFactory.Create())
            {
                string commandText = "SELECT a.*,b.TypeName as ScanTypeName FROM MK_Point a inner join MK_ScanType b on a.ScanTypeId=b.ID where a.DeviceId = @DeviceId";
                return conn.Query<ScanPointModel>(commandText, new { DeviceId = deviceId }).ToList();
            }

        }

        public static int IsExist(int PointId, DateTime CreateDate)
        {

            using (IDbConnection conn = ConnectionFactory.Create())
            {
                string commandText = "SELECT * FROM MK_DataValue where PointId = @PointId and CreateDate=@CreateDate";
                var p = conn.Query<DataValueModel>(commandText, new { PointId = PointId, CreateDate = CreateDate }).ToList();
                return p.Count();
            }
        }
    }
}

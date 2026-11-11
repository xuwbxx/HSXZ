using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FDScanSystem.DAO;
using FDScanSystem.Helper;
using Interactive.Utility;
using ZJFramework10.Extension;

namespace FDScanSystem.Server.Job
{
    public class NewDataReadJob : MoniterBase
    {
        protected NewDataReadJob() : base() { }
        static MoniterBase _myInstance;
        static object _lock = new object();
        //private string now = "2013-10-8 00:00:00";
        protected override void PostData()
        {
            AppContext.Log("执行自动定时读取模块数据....");
            // LogWriter.Info("执行自动定时读取模块数据....");

            var db = DataContextFactory.Create();
            var config = db.MK_AutoReadConfig.FirstOrDefault(u => u.ID == 1);
            if (config == null)
            {
                AppContext.Log("自动读取数据的配置未找到");
                // AppContext.Error("自动读取数据的配置未找到");
                // LogWriter.Info("自动读取数据的配置未找到"); return;
            }
          //  if (!IsPass(config)) { return; }
            var queue = db.view_ComAutoReadList.Where(u => u.DeviceProtocol == 1).ToList();
            if (queue == null) return;

            List<MK_Scan_Command> commands = new List<MK_Scan_Command>(queue.Count);
            MK_Scan_Command command = null;
            foreach (var item in queue)
            {
                //if (item.DeviceProtocol == 0)
                //{ continue; }
                command = new MK_Scan_Command();
                //if (item.DataProtocol == (int)AnalysisProtocolType.MCU)

                //command.Command = ConvertStr("68 04 00 04 08");
                string str = item.CRC.AsStr().Trim();
                command.Command = ConvertStr(str);

                //command.Command = "68 04 00 04 08";
                //else
                //    command.Command = "10";
                command.CommunicationDeviceID = item.CommunicationDeviceId;
                command.CommunicationIden = item.CommunicationCode;
                command.CreateDate = DateTime.Now;
                command.DeviceID = item.ID;
                command.DeviceIden = item.DeviceIdentity;
                // command.PointId = item.PointId;
                command.Result = string.Empty;
                command.IsSystem = 1;
                command.Status = (int)CommandStatus.New;
                //if (item.DataProtocol == (int)AnalysisProtocolType.HuaHeng
                //  && (string.IsNullOrEmpty(item.DeviceIden)
                //  || item.DeviceIden.Length < 20))
                //{
                //    continue;
                //}

                commands.Add(command);



                //if (Singleton<ServicesStatuesMachine>.Instance.IsOutofMaxRange())
                //{
                //    command = new MK_Scan_Command();
                //    command.Command = "MCU32 {0} Clear";
                //    command.CommunicationDeviceID = item.CommunicationDeviceId;
                //    command.CommunicationIden = item.CommunicationCode;
                //    command.CreateDate = DateTime.Now;
                //    command.DeviceID = item.ID;
                //    command.DeviceIden = item.DeviceIdentity;
                //    //  command.PointId = item.PointId;
                //    command.Result = string.Empty;
                //    command.IsSystem = 1;
                //    command.Status = (int)CommandStatus.New;
                //    commands.Add(command);
                //}
            }
            db.MK_Scan_Command.InsertAllOnSubmit(commands);
            db.SubmitChanges();

            Singleton<ServicesStatuesMachine>.Instance.Increment();
        }

        public string ConvertStr(string str)
        {
            string val = "";
            var temp = str.Split(' ');
            foreach (var s in temp)
            {

                val = val + System.Convert.ToInt32(s, 16).ToString() + " ";
            }
            return val.Trim();
        }

        private bool IsPass(MK_AutoReadConfig config)
        {
            TimeSpan subTime = DateTime.Now.Subtract(config.BeginDate.Value);
            return (((long)subTime.TotalMinutes) % config.Interval) == 1;
        }



        /// <summary>
        /// 获取单例实例
        /// </summary>
        /// <returns></returns>
        public static MoniterBase GetInstance()
        {
            if (_myInstance == null)
            {
                lock (_lock)
                {
                    if (_myInstance == null)
                    {
                        _myInstance = new NewDataReadJob();
                    }
                }
            }
            return _myInstance;
        }
    }
}

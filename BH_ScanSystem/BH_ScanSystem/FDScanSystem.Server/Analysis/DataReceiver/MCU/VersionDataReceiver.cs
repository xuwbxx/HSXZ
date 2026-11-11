using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FDScanSystem.DAO;
using FDScanSystem.Helper;
using FDScanSystem.Server;
using FDScanSystem.Server.Analysis.DataReceiver.MCU;
using FDScanSystem.Server.Device;

namespace FDScanSystem.Server.Job.DataReceiver
{
    public class VersionDataReceiver : MCUDataReceiverBase
    {


        protected override DataReceiveArgs SubHandle(Device.DeviceItem device, DataReceiveArgs args, string msg)
        {
            if (msg.IndexOf("Version", StringComparison.CurrentCultureIgnoreCase) > 0)
            {
                args.IsHandle = true;
                string version = msg.Substring(msg.IndexOf("=") + 1, 5);
                var connection = DeviceConnectionManager.GetInstance().GetWithoutStatus(device.Key);
                if (connection != null)
                {
                    if (version != null)
                    {
                        var deviceConnectionInstance = DeviceConnectionManager.GetInstance();
                        if (deviceConnectionInstance.ActiveLock())
                        {
                            try
                            {
                                var db = DataContextFactory.Create();
                                var deviceEntity = db.MK_Device.Where(u => u.DeviceIden == version).FirstOrDefault();
                                if (deviceEntity != null)
                                {
                                    deviceEntity.Status = 1;
                                    db.SubmitChanges();
                                    var scanDevice = device.ScanDevices.Where(u => u.DeviceIden.Equals(version)).FirstOrDefault();
                                    if (scanDevice != null && scanDevice.ID == deviceEntity.ID)
                                    {

                                    }
                                    deviceConnectionInstance.Active(device);

                                }

                                //var currentCommunication = db.XPD_Current_Communication.Where(u => u.IP == device.IP && u.Port == device.Port).FirstOrDefault();
                                //if (currentCommunication != null)
                                //{
                                //    currentCommunication.CommunicationIden = currentCommunication.CommunicationIden + "_" + version;
                                //    db.SubmitChanges();
                                //}
                            }
                            catch (Exception ex) { 
                              //  AppContext.Error(ex);
                                AppContext.Log(ex.Message);
                            }
                            finally
                            {
                                deviceConnectionInstance.ActiveUnlock();
                            }
                        }
                    }
                }
                // LogWriter.Info("成功处理了版本信息 " + version);
                AppContext.Log("成功处理了版本信息 " + version);
            }
            return args;
        }
    }
}

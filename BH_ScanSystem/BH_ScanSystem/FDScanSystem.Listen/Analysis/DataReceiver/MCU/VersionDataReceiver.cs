using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FDScanSystem.Listen.Analysis.DataReceiver.MCU;

using FDScanSystem.Listen.Device;
using FDScanSystem.DAO;

namespace FDScanSystem.Listen.Logic.DataReceiver
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
                                //db.
                                //var deviceEntity = db..Where(u => u.DeivceIden == version).FirstOrDefault();
                                //if (deviceEntity != null)
                                //{
                                //    //var communicationDevice = db..Where(u => u.DeviceType == 0 && u.ID == deviceEntity.CommunicationDeviceId).FirstOrDefault();
                                //    //if (communicationDevice != null)
                                //    //{
                                //    //    communicationDevice.IP = device.IP;
                                //    //    communicationDevice.Port = device.Port;
                                //    //    db.SubmitChanges();
                                //    //    device.DeviceId = communicationDevice.ID;
                                //    //    deviceConnectionInstance.Active(device);
                                //    //    //     AppContext.Log(string.Format("VersionDataReceiver更新关键字{0}", device.IP + ":" + device.Port + ":" + device.Type + ":" + device.SIMId));
                                //    //}
                                //}

                                //var currentCommunication = db.XPD_Current_Communication.Where(u => u.IP == device.IP && u.Port == device.Port).FirstOrDefault();
                                //if (currentCommunication != null)
                                //{
                                //    if (!string.IsNullOrEmpty(currentCommunication.CommunicationIden) && currentCommunication.CommunicationIden.Length > 200)
                                //    {

                                //    }
                                //    else
                                //    {
                                //        currentCommunication.CommunicationIden = currentCommunication.CommunicationIden + "_" + version;
                                //        db.SubmitChanges();
                                //    }
                                //}
                            }
                            catch (Exception ex) { AppContext.Error(ex); }
                            finally
                            {
                                deviceConnectionInstance.ActiveUnlock();
                            }
                        }
                    }
                }
                AppContext.Log("成功处理了版本信息 " + version);
            }
            return args;
        }
    }
}

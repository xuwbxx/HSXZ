using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Interactive.Utility;
using System.Threading;
using FDScanSystem.Listen.Device;
using FDScanSystem.DAO;
using System.Configuration;
using System.Net.Sockets;
using System.Net;
using ZJFramework10.Extension;

namespace FDScanSystem.Listen.Logic
{

    /// <summary>
    /// UDP监控类
    /// </summary>
    public class UDPWIndMoniter : WindMoniterBase
    {
        protected UDPWIndMoniter() : base() { }
        static WindMoniterBase _myInstance;
        static object _lock = new object();
        /// <summary>
        /// 用于UDP发送的网络服务类
        /// </summary>
        private UdpClient udpcSend;
        /// <summary>
        /// 用于UDP接收的网络服务类
        /// </summary>
        private UdpClient udpcRecv;
        /// <summary>
        /// 开关：在监听UDP报文阶段为true，否则为false
        /// </summary>
        bool IsUdpcRecvStart = false;
        /// 线程：不断监听UDP报文
        /// </summary>
        Thread thrRecv;
         /// <summary>
        /// 执行定时读取
        /// </summary>
        protected override void PostData()
        {
            string serverip = ConfigurationManager.AppSettings["serverip"];
            string serverport = ConfigurationManager.AppSettings["serverport"];
            string localip = ConfigurationManager.AppSettings["localip"];
            string localport = ConfigurationManager.AppSettings["localport"];
            IPEndPoint localIpep = new IPEndPoint(
                      IPAddress.Parse(localip), localport.AsInt()); // 本机IP和监听端口号
            if (!IsUdpcRecvStart) // 未监听的情况，开始监听
            {
                try
                {

                    //IPEndPoint localIpep = new IPEndPoint(
                    //    IPAddress.Parse(localip), localport.AsInt()); // 本机IP和监听端口号

                    udpcRecv = new UdpClient(localIpep);

                    thrRecv = new Thread(ReceiveMessage);
                    thrRecv.Start();

                    IsUdpcRecvStart = true;
                    AppContext.Log("UDP监听器已成功启动");


                    IPEndPoint localIpep1 = new IPEndPoint(
                    IPAddress.Parse(localip), 5550); // 本机IP，指定的端口号
                    udpcSend = new UdpClient(localIpep1);

                    Thread thrSend = new Thread(SendMessage);
                    thrSend.Start(string.Format("{0}|{1}|{2}|{3}|{4}||", localip, localport, serverip, serverport, 1));
                }
                catch (Exception ex)
                {
                    AppContext.Error(ex.Message);
                }


            }

            //IPEndPoint localIpep1 = new IPEndPoint(
            //IPAddress.Parse(localip), 5550); // 本机IP，指定的端口号
            //udpcSend = new UdpClient(localIpep1);

            //Thread thrSend = new Thread(SendMessage);
            //thrSend.Start(string.Format("{0}|{1}|{2}|{3}|{4}||", localip, localport, serverip, serverport, 1));

            CommandDispatcher.GetInstance().Tick(1);//读取命令后触发一次命令的Tick事件
        }


        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="obj"></param>
        private void SendMessage(object obj)
        {
            string message = (string)obj;
            byte[] sendbytes = Encoding.Unicode.GetBytes(message);
            string serverip = ConfigurationManager.AppSettings["serverip"];
            string serverport = ConfigurationManager.AppSettings["serverport"];
            IPEndPoint remoteIpep = new IPEndPoint(
                IPAddress.Parse(serverip), serverport.AsInt()); // 发送到的IP地址和端口号
            udpcSend.Send(sendbytes, sendbytes.Length, remoteIpep);
            AppContext.Log(message + "已发送");
            udpcSend.Close();

            //   ResetTextBox(txtSendMssg);
        }


        // 向TextBox中添加文本
        delegate void ShowMessageDelegate(string message);
        private void ShowMessage(string message)
        {
            //if (txtbox.InvokeRequired)
            //{
            //    ShowMessageDelegate showMessageDelegate = ShowMessage;
            //    txtbox.Invoke(showMessageDelegate, new object[] { txtbox, message });
            //}
            //else
            //{
            //    txtbox.Text += message + "\r\n";
            //}
            AppContext.Log(message);
        }


        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="obj"></param>
        private void ReceiveMessage(object obj)
        {
            IPEndPoint remoteIpep = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                try
                {
                    byte[] bytRecv = udpcRecv.Receive(ref remoteIpep);
                    string message = Encoding.GetEncoding("gb2312").GetString(
                        bytRecv, 0, bytRecv.Length);
                    

                   // ShowMessage(string.Format("{0}[{1}]", remoteIpep, message));


                    AppContext.Log(string.Format("接收到报文：{0}[{1}]", remoteIpep, message));

                    getString(message);

                    AppContext.Log("报文处理完成");

                }
                catch (Exception ex)
                {
                    ShowMessage(ex.Message);
                    break;
                }
            }
        }

        public void getString(string message)
        {
            string aa = message;
            var db = DataContextFactory.Create();
            if (!string.IsNullOrEmpty(aa))
            {
                string[] arr = aa.Split('|');
                if (arr.Length > 0)
                {


                    FD_Device device = db.FD_Device.FirstOrDefault(u => u.DeviceIdentity == arr[1]);
                    if (device == null)
                    {
                        return;
                    }

                    FD_ScanNode node = db.FD_ScanNode.FirstOrDefault(u => u.ID == device.NodeId);
                    if (node == null)
                    {
                        return;
                    }

                    FD_DynamicValue dyna = new FD_DynamicValue();
                    int Count = arr[3].AsInt();
                    List<FD_DynamicValue2> list = new List<FD_DynamicValue2>();


                    dyna.CreateDate = DateTime.Now;
                    string name = dyna.CreateDate.Value.ToString("yyyyMM");
                    string DBName = "FD_DynamicValue2" + name;
                    string DBName1 = "FD_DynamicValue" + name;
                    var Exist = db.ExecuteQuery<FD_DynamicValue2>(string.Format("Select Count(1) as ID From sysObjects Where Name ='{0}'", DBName)).FirstOrDefault();
                    if (Exist.ID == 0)
                    {
                        string sql = string.Format(@"CREATE TABLE [dbo].[{0}](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DeviceId] [int] NULL,
	[PointId] [int] NULL,
	[MAXValue] [float] NULL,
	[YXValue] [float] NULL,
	[AverageValue] [float] NULL,
	[CreateDate] [datetime] NULL,
	[DynamicId] [int] NULL,
 CONSTRAINT [PK_{1}] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]", DBName, DBName);
                        db.ExecuteCommand(sql);
                    }


                    var Exist1 = db.ExecuteQuery<FD_DynamicValue2>(string.Format("Select Count(1) as ID From sysObjects Where Name ='{0}'", DBName1)).FirstOrDefault();
                    if (Exist1.ID == 0)
                    {
                        string sql = string.Format(@"CREATE TABLE [dbo].[{0}](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DeviceId] [int] NULL,
	[ScanTime] [datetime] NULL,
	[CreateDate] [datetime] NULL,
	[YQLD] [float] NULL,
	[AlertJL] [float] NULL,
	[MAXWY] [float] NULL,
	[WYB] [float] NULL,
	[HCJSD] [float] NULL,
	[HCJSDYX] [float] NULL,
	[JP] [float] NULL,
	[PointId] [int] NULL,
 CONSTRAINT [PK_{1}] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]", DBName1, DBName1);
                        db.ExecuteCommand(sql);
                    }


                    for (int i = 0; i < Count; i++)
                    {
                        string channel = (i + 1).ToString();
                        FD_DynamicValue2 model = new FD_DynamicValue2();
                        var point = db.FD_ScanPoint.FirstOrDefault(u => u.Channel1 == channel && u.DeviceId == device.ID);
                        //model.DeviceId = arr[1].AsInt();

                        if (point != null)
                        {
                            model.PointId = point.ID;
                            model.DeviceId = device.ID;
                            dyna.DeviceId = device.ID;
                            dyna.PointId = point.ID;
                        }

                        list.Add(model);
                    }
                    try
                    {

                        if (Count > 0)
                        {
                            string s = arr[4];
                            string[] srr = s.Split(',');
                            int index = Count * 3;
                            dyna.ScanTime = dyna.CreateDate;
                            dyna.CreateDate = dyna.ScanTime;
                            dyna.YQLD = srr[index].AsDouble();
                            index++;
                            dyna.AlertJL = srr[index].AsDouble();
                            index++;
                            dyna.MAXWY = srr[index].AsDouble();
                            index++;
                            dyna.WYB = srr[index].AsDouble();
                            index++;
                            dyna.HCJSD = srr[index].AsDouble();
                            index++;
                            dyna.HCJSDYX = srr[index].AsDouble();
                            index++;
                            dyna.JP = srr[index].AsDouble();



                            //db.FD_DynamicValue.InsertOnSubmit(dyna);
                            //db.SubmitChanges();
                            db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')", DBName1, dyna.DeviceId, dyna.ScanTime, dyna.CreateDate, dyna.YQLD, dyna.AlertJL, dyna.MAXWY, dyna.WYB, dyna.HCJSD, dyna.HCJSDYX, dyna.JP, dyna.PointId));
                            for (int i = 0; i < Count; i++)
                            {
                                list[i].MAXValue = srr[i].AsDouble();
                                list[i].CreateDate = dyna.CreateDate;
                                list[i].DynamicId = dyna.ID;

                            }
                            for (int i = Count; i < Count * 2; i++)
                            {
                                list[i - Count].YXValue = srr[i].AsDouble();
                            }
                            for (int i = Count * 2; i < Count * 3; i++)
                            {
                                list[i - Count * 2].AverageValue = srr[i].AsDouble();
                            }
                            foreach (var model in list)
                            {
                                db.ExecuteCommand(string.Format("insert into {0} values ('{1}','{2}','{3}','{4}','{5}','{6}','{7}')", DBName, model.DeviceId, model.PointId, model.MAXValue, model.YXValue, model.AverageValue, model.CreateDate, model.DynamicId));
                            }
                            //db.FD_DynamicValue2.InsertAllOnSubmit(list);
                            //db.SubmitChanges();


                           // MessageBox.Show("导入成功");
                        }
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.Message);
                        AppContext.Error(ex.Message);
                    }

                }

            }

        }


        /// <summary>
        /// 获取单例实例
        /// </summary>
        /// <returns></returns>
        public static WindMoniterBase GetInstance()
        {
            if (_myInstance == null)
            {
                lock (_lock)
                {
                    if (_myInstance == null)
                    {
                        _myInstance = new UDPWIndMoniter();
                    }
                }
            }
            return _myInstance;
        }

    }
}

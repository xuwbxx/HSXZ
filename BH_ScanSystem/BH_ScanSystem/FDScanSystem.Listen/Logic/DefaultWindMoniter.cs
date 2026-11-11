using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Interactive.Utility;
using System.Threading;
using FDScanSystem.Listen.Device;
using FDScanSystem.DAO;



namespace FDScanSystem.Listen.Logic
{
    /// <summary>
    /// 默认监控类
    /// </summary>
    public class DefaultWindMoniter : WindMoniterBase
    {
        protected DefaultWindMoniter() : base() { }
        static WindMoniterBase _myInstance;
        static object _lock = new object();

        /// <summary>
        /// 执行定时读取
        /// </summary>
        protected override void PostData()
        {

            var db = DataContextFactory.Create();
            var commands = db.view_TCPCommand.Where(u => u.Status == (int)CommandStatus.New).ToList();

            foreach (var item in commands)
            {
                //  AppContext.Log(item.ToString());
                if (item.Status == (int)CommandStatus.New)/*
                                         0 - 新增命令
                                         1 - 重置命令
                                         2 - 已发送命令
                                         3 - 待发送命令
                                      */
                {
                    if (CommandDispatcher
                        .GetInstance()
                        .Dispatch(item))
                    {
                        item.Status = (int)CommandStatus.WaitSending;
                    }
                }
            }

            db.SubmitChanges();

            CommandDispatcher.GetInstance().Tick(1);//读取命令后触发一次命令的Tick事件
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
                        _myInstance = new DefaultWindMoniter();
                    }
                }
            }
            return _myInstance;
        }
    }
}

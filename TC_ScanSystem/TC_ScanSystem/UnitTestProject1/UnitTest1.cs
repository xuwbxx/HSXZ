using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tool;
using System.Collections.Generic;
using DataEntity.DataContext;
using DataEntity.Services;
using Model.Enum;
using Business.AlertService;
using ExternalService.IOT;
using Business.DataHandler;
using System.Configuration;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            try
            {
                //IntelligentVisionService service = new IntelligentVisionService();

                //DateTime start = DateTime.Now.AddDays(-60);
                //DateTime end = DateTime.Now.AddDays(-59);

                //service.GetVisionAlarmData(start, end);
                //string IntelligentVisionPicUrl = ConfigurationManager.AppSettings["IntelligentVisionPicUrl"].ToString();

                //string url = @"http://127.0.0.1:8081/jpgs/611/1/2019/4/15/16/636909431923197788_0L.jpg";

                //string o = IntelligentVisionPicUrl + url.Substring(url.IndexOf("jpgs"));

                DataCreateService service = new DataCreateService();

                service.CreatePersonInAndOutData();


            }
            catch (Exception ex)
            {

                throw;
            }
        }

        [TestMethod]
        public void Test2()
        {
            try
            {
                string str = "[{\"ID\":0,\"EmployeeName\":\"徐天洋\",\"JobName\":\"管理人员\",\"EventTime\":\"2019 - 03 - 15T20: 08:45\",\"AreaName\":\"\",\"DoorName\":\"现场入口\",\"Card\":\"102\",\"TypeName\":\"进         \",\"CaveName\":\"\",\"DoorID\":1,\"EmployeeID\":4,\"EventType\":0,\"CaveId\":1,\"StructorId\":0,\"StructorName\":\"\",\"PersonId\":\"4\",\"Picture\":null},{\"ID\":0,\"EmployeeName\":\"徐天洋\",\"JobName\":\"管理人员\",\"EventTime\":\"2019 - 03 - 15T20: 08:47\",\"AreaName\":\"\",\"DoorName\":\"现场入口\",\"Card\":\"102\",\"TypeName\":\"进         \",\"CaveName\":\"\",\"DoorID\":1,\"EmployeeID\":4,\"EventType\":0,\"CaveId\":1,\"StructorId\":0,\"StructorName\":\"\",\"PersonId\":\"4\",\"Picture\":null}]";
                List<Scan_PersonInAndOut> list = str.Deserialize<List<Scan_PersonInAndOut>>();

                WorkService service = new WorkService();

                Scan_PersonInAndOut se = new Scan_PersonInAndOut();
                se.AreaName = list[0].AreaName;
                se.Card = list[0].Card;
                se.CaveId = list[0].CaveId;
                se.CaveName = list[0].CaveName;


                service.Repository_Scan_PersonInAndOut.AddList(list);

                service.SaveChange();

            }
            catch (Exception ex)
            {

                throw;
            }
            

        }

        [TestMethod]
        public void SMSTest()
        {
            List<Scan_PersonInAndOut> list = new List<Scan_PersonInAndOut>();

            Scan_PersonInAndOut se1 = new Scan_PersonInAndOut();
            se1.EmployeeName = "张丛生";
            Scan_PersonInAndOut se2 = new Scan_PersonInAndOut();
            se2.EmployeeName = "陈家任";
            Scan_PersonInAndOut se3 = new Scan_PersonInAndOut();
            se3.EmployeeName = "程伟";

            list.Add(se1);
            list.Add(se2);
            list.Add(se3);

            DataCreateService handler = new DataCreateService();
            handler.CreatePositionData(list);
        }


        [TestMethod]
        public void LoginTest()
        {
            //< add key = "LoginKey" value = "8db2f07d0f5e48bb84cf6444" />

            //< add key = "LoginIV" value = "shgwy829" />

            string Key = "8db2f07d0f5e48bb84cf6444";
            string IV = "shgwy829";

            string username = "atide";
            string password = "123456";

            string code = username + "-" + password;

            string str = CryptographyUtils.TripleDESEncrypt(code, Key, IV);
        }

    }
}

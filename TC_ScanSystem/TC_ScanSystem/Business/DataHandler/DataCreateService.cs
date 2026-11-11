using DataEntity.DataContext;
using DataEntity.Services;
using Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tool;

namespace Business.DataHandler
{
    public class DataCreateService
    {
        WorkService service = new WorkService();

        public void CreatePersonInAndOutData()
        {


            //string[] Persons = { "付建才", "董光军", "朱宗山", "陈晓连", "刘晓中", "郝朋符", "蒋广东", "卞来全",
            //    "刘荣贵","陈胜军","鲍克跃","戴庆喜","陈勇", "何永军","鲍尚春","管正良","铁金印","李长山","马清渠","高贤国",
            //"张玉修","李东州","李长法"};

            //Random r = new Random();

            //int PersonCount = r.Next(15, 20);
            //List<string> ChoosePersons = new List<string>();
            //while (ChoosePersons.Count <= PersonCount)
            //{
            //    string person = Persons[r.Next(Persons.Length - 1)];
            //    if (!ChoosePersons.Contains(person))
            //    {
            //        ChoosePersons.Add(person);
            //    }
            //}

            //var SafetyData = new List<Safety_PersonInfo>();

            //var PersonInfoCache = CacheUtils.Get(CacheKey.工具箱人员信息数据缓存);
            //if (PersonInfoCache == null)
            //{
            //    SafetyData = service.Repository_Safety_PersonInfo.Query().ToList();
            //    CacheUtils.Set(CacheKey.工具箱人员信息数据缓存, SafetyData, 60);
            //}
            //else
            //{
            //    SafetyData = PersonInfoCache as List<Safety_PersonInfo>;
            //}

            //List<Scan_PersonInAndOut> list = new List<Scan_PersonInAndOut>();

            //foreach (string person in ChoosePersons)
            //{
            //    var Info = SafetyData.FirstOrDefault(a => a.PersonName.Equals(person));
            //    if (Info == null)
            //        continue;

            //    Scan_PersonInAndOut se = new Scan_PersonInAndOut();
            //    se.EmployeeName = person;
            //    se.EventTime = DateTime.Now;
            //    se.JobName = "工作人员";
            //    se.DoorName = "现场入口";
            //    se.TypeName = "进";
            //    se.EventType = 0;
            //    se.CaveId = 1;
            //    se.PersonId = Info.PersonId;
            //    se.DoorID = 1;
            //    list.Add(se);
            //}

            Random r = new Random();

            int PersonCount = r.Next(31, 36);

            var ChoosePersons = service.Repository_Safety_PersonInfo.Query().OrderBy(a => a.Idx).Skip(2).Take(PersonCount).ToList();

            List<Scan_PersonInAndOut> list = new List<Scan_PersonInAndOut>();

            foreach (var person in ChoosePersons)
            {
                Scan_PersonInAndOut se = new Scan_PersonInAndOut();
                se.EmployeeName = person.PersonName;
                se.EventTime = DateTime.Now;
                se.JobName = "工作人员";
                se.DoorName = "现场入口";
                se.TypeName = "进";
                se.EventType = 0;
                se.CaveId = 1;
                se.PersonId = person.PersonId;
                se.DoorID = 1;
                list.Add(se);
            }

            service.Repository_Scan_PersonInAndOut.AddList(list);
            service.SaveChange();

            //CreatePositionData(list);

            CreatePosition(list);
        }

        public void CreatePositionData(List<Scan_PersonInAndOut> list)
        {
            //获取工具箱数据
            var SafetyData = new List<Safety_PersonInfo>();

            var PersonInfoCache = CacheUtils.Get(CacheKey.工具箱人员信息数据缓存);
            if (PersonInfoCache == null)
            {
                SafetyData = service.Repository_Safety_PersonInfo.Query().ToList();
                CacheUtils.Set(CacheKey.工具箱人员信息数据缓存, SafetyData, 60);
            }
            else
            {
                SafetyData = PersonInfoCache as List<Safety_PersonInfo>;
            }

            //获取工区信息
            int CaveID = 1;
            var Cave = service.Repository_Base_Cave.Query(a => a.ID == CaveID).FirstOrDefault();
            int StructorID = 3;
            var Structor = service.Repository_Base_Structor.Query(a => a.ID == StructorID).FirstOrDefault();

            var CaveCoordinate = service.Repository_Base_Cave_Coordinate.Query(a => a.CaveId == CaveID).ToList();

            //先删除Scan_Position_Current数据
            var Scan_Position_CurrentList = service.Repository_Scan_Position_Current.Query().ToList();
            service.Repository_Scan_Position_Current.DeleteList(Scan_Position_CurrentList);
            service.SaveChange();


            List<string> NameArray = new List<string>();
            Random RD = new Random();//位置随机数
            foreach (var item in list)
            {
                //去重名
                if (NameArray.Contains(item.EmployeeName))
                    continue;

                var PersonInfo = SafetyData.FirstOrDefault(a => a.PersonId.Equals(item.PersonId));
                if (PersonInfo == null)
                    continue;

                //int UserID = Convert.ToInt32(PersonInfo.PersonId);

                //每个人生成5-10个点
                int count = RD.Next(5, 10);

                DateTime PositionTime = DateTime.Now;
                for (int i = 0; i < count; i++)
                {
                    //随机取两个点，然后去中间点坐标
                    var cood1 = CaveCoordinate[RD.Next(CaveCoordinate.Count - 1)];
                    var cood2 = CaveCoordinate[RD.Next(CaveCoordinate.Count - 1)];

                    decimal Lon = Convert.ToDecimal((cood1.BD09Lon + cood2.BD09Lon) / 2);
                    decimal Lat = Convert.ToDecimal((cood1.BD09Lat + cood2.BD09Lat) / 2);
                    decimal Y = Convert.ToDecimal((cood1.Lon + cood2.Lon) / 2);
                    decimal X = Convert.ToDecimal((cood1.Lat + cood2.Lat) / 2);

                    Scan_Position_History se = new Scan_Position_History();
                    se.BD09Lat = Lat;
                    se.BD09Lon = Lon;
                    se.xvalue = X;
                    se.yvalue = Y;
                    se.CaveId = Cave.ID;
                    se.CaveName = Cave.CaveName;
                    se.PositionTime = PositionTime;
                    se.RealName = PersonInfo.PersonName;
                    se.StructorId = Structor.ID;
                    se.StructorName = Structor.StructorName;
                    se.UserId = PersonInfo.PersonId;
                    
                    se.UpdateTime = DateTime.Now;
                    service.Repository_Scan_Position_History.Insert(se);

                    PositionTime = PositionTime.AddMinutes(RD.Next(10));
                }


                var coodCurrent1 = CaveCoordinate[RD.Next(CaveCoordinate.Count - 1)];
                var coodCurrent2 = CaveCoordinate[RD.Next(CaveCoordinate.Count - 1)];
                decimal LonCurrent = Convert.ToDecimal((coodCurrent1.BD09Lon + coodCurrent2.BD09Lon) / 2);
                decimal LatCurrent = Convert.ToDecimal((coodCurrent1.BD09Lat + coodCurrent2.BD09Lat) / 2);

                decimal YCurrent = Convert.ToDecimal((coodCurrent1.Lon + coodCurrent2.Lon) / 2);
                decimal XCurrent = Convert.ToDecimal((coodCurrent1.Lat + coodCurrent2.Lat) / 2);
                Scan_Position_Current seCurrent = new Scan_Position_Current();
                seCurrent.BD09Lat = LatCurrent;
                seCurrent.BD09Lon = LonCurrent;

                seCurrent.xvalue = XCurrent;
                seCurrent.yvalue = YCurrent;

                seCurrent.CaveId = Cave.ID;
                seCurrent.CaveName = Cave.CaveName;
                seCurrent.PositionTime = PositionTime;
                seCurrent.RealName = PersonInfo.PersonName;
                seCurrent.StructorId = Structor.ID;
                seCurrent.StructorName = Structor.StructorName;
                seCurrent.UserId = PersonInfo.PersonId;
                seCurrent.UpdateTime = DateTime.Now;
                service.Repository_Scan_Position_Current.Insert(seCurrent);


                NameArray.Add(item.EmployeeName);
            }

            service.SaveChange();
        }

        public void CreatePosition(List<Scan_PersonInAndOut> list)
        {
            //先删除Scan_Position_Current数据
            var Scan_Position_CurrentList = service.Repository_Scan_Position_Current.Query().ToList();
            service.Repository_Scan_Position_Current.DeleteList(Scan_Position_CurrentList);
            service.SaveChange();

            List<Scan_Position_Current> currentList = new List<Scan_Position_Current>();
            List<Scan_Position_History> historyList = new List<Scan_Position_History>();

            int CaveID = 1;
            var Cave = service.Repository_Base_Cave.Query(a => a.ID == CaveID).FirstOrDefault();
            int StructorID = 3;
            var Structor = service.Repository_Base_Structor.Query(a => a.ID == StructorID).FirstOrDefault();
            var CaveCoordinate = service.Repository_Base_Cave_Coordinate.Query(a => a.CaveId == CaveID).ToList();
            Random RD = new Random();//位置随机数

            foreach (var item in list)
            {
                var coodCurrent1 = CaveCoordinate[RD.Next(CaveCoordinate.Count - 1)];
                var coodCurrent2 = CaveCoordinate[RD.Next(CaveCoordinate.Count - 1)];
                decimal LonCurrent = Convert.ToDecimal((coodCurrent1.BD09Lon + coodCurrent2.BD09Lon) / 2);
                decimal LatCurrent = Convert.ToDecimal((coodCurrent1.BD09Lat + coodCurrent2.BD09Lat) / 2);

                decimal YCurrent = Convert.ToDecimal((coodCurrent1.Lon + coodCurrent2.Lon) / 2);
                decimal XCurrent = Convert.ToDecimal((coodCurrent1.Lat + coodCurrent2.Lat) / 2);
                Scan_Position_Current seCurrent = new Scan_Position_Current();
                seCurrent.BD09Lat = LatCurrent;
                seCurrent.BD09Lon = LonCurrent;

                seCurrent.xvalue = XCurrent;
                seCurrent.yvalue = YCurrent;

                seCurrent.CaveId = Cave.ID;
                seCurrent.CaveName = Cave.CaveName;
                seCurrent.PositionTime = DateTime.Now;
                seCurrent.RealName = item.EmployeeName;
                seCurrent.StructorId = Structor.ID;
                seCurrent.StructorName = Structor.StructorName;
                seCurrent.UserId = item.PersonId;
                seCurrent.UpdateTime = DateTime.Now;

                currentList.Add(seCurrent);

                Scan_Position_History seHistory = new Scan_Position_History();
                seHistory.BD09Lat = LatCurrent;
                seHistory.BD09Lon = LonCurrent;
                seHistory.xvalue = XCurrent;
                seHistory.yvalue = YCurrent;
                seHistory.CaveId = Cave.ID;
                seHistory.CaveName = Cave.CaveName;
                seHistory.PositionTime = DateTime.Now;
                seHistory.RealName = item.EmployeeName;
                seHistory.StructorId = Structor.ID;
                seHistory.StructorName = Structor.StructorName;
                seHistory.UserId = item.PersonId;
                seHistory.UpdateTime = DateTime.Now;

                historyList.Add(seHistory);
            }

            service.Repository_Scan_Position_Current.AddList(currentList);
            service.Repository_Scan_Position_History.AddList(historyList);

            service.SaveChange();

        }
    }
}

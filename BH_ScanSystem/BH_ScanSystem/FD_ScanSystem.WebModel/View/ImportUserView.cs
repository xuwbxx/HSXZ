using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FD_ScanSystem.WebModel.View
{
    public class ImportUserView
    {

        public int ID { get; set; }

        public string Number { get; set; }

        public string Name { get; set; }

        public string Company { get; set; }

        public string birthday { get; set; }

        public int Sex { get; set; }

        public string WorkerBeginDate { get; set; }

        public string Education { get; set; }

        public string Post { get; set; }

        public string Professional { get; set; }

        public int WorkAge { get; set; }

        public string WorkProfessional { get; set; }

        public string TestResult { get; set; }

        public string ComputerTestStatus { get; set; }

        public System.Nullable<double> ComputerTest { get; set; }

        public string EnglishTestStatus { get; set; }

        public System.Nullable<double> EnglishTest { get; set; }

        public string Remark { get; set; }

        public System.Nullable<int> GroupId { get; set; }

        public int TaskId { get; set; }

        public System.Nullable<int> LevelId { get; set; }

        public System.Nullable<System.DateTime> CreateDate { get; set; }

        public System.Nullable<System.DateTime> UpdateDate { get; set; }

        public string Updator { get; set; }


        public string SexName
        {
            get
            {
                if (Sex == 0) return "男";
                else return "女";
            }
        }
    }
}

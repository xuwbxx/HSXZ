using Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Manage.Request
{
    public class AccountRequest : BaseRequest
    {
        public int ID { set; get; }

        /// <summary>
        /// add  modify
        /// </summary>
        public string Type { set; get; }

        public string UserName { set; get; }

        public string RealName { set; get; }

        public string Mobile { set; get; }

        public string Password { set; get; }

        public int StructorID { set; get; }

        public int PositionID { set; get; }

        public int DepartID { set; get; }

        public string Roles { set; get; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FDScanSystem.DAO.Model
{
    public class VoteUser_Entity
    {
        public int UserId { set; get; }
        public int GroupId { set; get; }
        public int TaskId { set; get; }
        public string UserName { set; get; }
        public string GroupName { set; get; }
        public bool IsLeader { set; get; }
        public bool IsAllLeader { set; get; }
        public string LearStatus 
        {
            get
            {
                if (IsLeader == false)
                {
                    return "no";
                }
                return "yes";
            }
        }
        public string AllLearStatus
        {
            get
            {
                if (IsAllLeader == false)
                {
                    return "no";
                }
                return "yes";
            }
        }
    }
}

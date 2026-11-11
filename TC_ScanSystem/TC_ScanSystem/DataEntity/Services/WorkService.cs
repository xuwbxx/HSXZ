using DataEntity.DataContext;
using DataEntity.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataEntity.Services
{
    public class WorkService : IDisposable
    {

        public DbContext context;

        public WorkService(string ProjectCode = "HJ")
        {
            switch (ProjectCode)
            {
                case ("HJ"):
                    context = new HJ_ModelContext();
                    break;
                default:
                    context = null;
                    break;
            }
        }

        private GenericRepository<Base_Depart> repository_Base_Depart;
        public GenericRepository<Base_Depart> Repository_Base_Depart
        {
            get
            {
                if (this.repository_Base_Depart == null)
                {
                    this.repository_Base_Depart = new GenericRepository<Base_Depart>(context);
                }
                return repository_Base_Depart;
            }
        }

        private GenericRepository<Base_Menu> repository_Base_Menu;
        public GenericRepository<Base_Menu> Repository_Base_Menu
        {
            get
            {
                if (this.repository_Base_Menu == null)
                {
                    this.repository_Base_Menu = new GenericRepository<Base_Menu>(context);
                }
                return repository_Base_Menu;
            }
        }

        private GenericRepository<Base_Role> repository_Base_Role;
        public GenericRepository<Base_Role> Repository_Base_Role
        {
            get
            {
                if (this.repository_Base_Role == null)
                {
                    this.repository_Base_Role = new GenericRepository<Base_Role>(context);
                }
                return repository_Base_Role;
            }
        }

        private GenericRepository<Scan_Safty_Inspection> repository_Scan_Safty_Inspection;
        public GenericRepository<Scan_Safty_Inspection> Repository_Scan_Safty_Inspection
        {
            get
            {
                if (this.repository_Scan_Safty_Inspection == null)
                {
                    this.repository_Scan_Safty_Inspection = new GenericRepository<Scan_Safty_Inspection>(context);
                }
                return repository_Scan_Safty_Inspection;
            }
        }


        private GenericRepository<View_Scan_Door_InAndOutRecord> repository_View_Scan_Door_InAndOutRecord;
        public GenericRepository<View_Scan_Door_InAndOutRecord> Repository_View_Scan_Door_InAndOutRecord
        {
            get
            {
                if (this.repository_View_Scan_Door_InAndOutRecord == null)
                {
                    this.repository_View_Scan_Door_InAndOutRecord = new GenericRepository<View_Scan_Door_InAndOutRecord>(context);
                }
                return repository_View_Scan_Door_InAndOutRecord;
            }
        }

        private GenericRepository<Scan_Door_InAndOutRecord> repository_Scan_Door_InAndOutRecord;
        public GenericRepository<Scan_Door_InAndOutRecord> Repository_Scan_Door_InAndOutRecord
        {
            get
            {
                if (this.repository_Scan_Door_InAndOutRecord == null)
                {
                    this.repository_Scan_Door_InAndOutRecord = new GenericRepository<Scan_Door_InAndOutRecord>(context);
                }
                return repository_Scan_Door_InAndOutRecord;
            }
        }

        private GenericRepository<Scan_Inspect_ContentCategory> repository_Scan_Inspect_ContentCategory;
        public GenericRepository<Scan_Inspect_ContentCategory> Repository_Scan_Inspect_ContentCategory
        {
            get
            {
                if (this.repository_Scan_Inspect_ContentCategory == null)
                {
                    this.repository_Scan_Inspect_ContentCategory = new GenericRepository<Scan_Inspect_ContentCategory>(context);
                }
                return repository_Scan_Inspect_ContentCategory;
            }
        }

        private GenericRepository<Base_User> repository_Base_User;
        public GenericRepository<Base_User> Repository_Base_User
        {
            get
            {
                if (this.repository_Base_User == null)
                {
                    this.repository_Base_User = new GenericRepository<Base_User>(context);
                }
                return repository_Base_User;
            }
        }

        private GenericRepository<Board_Target> repository_Board_Target;
        public GenericRepository<Board_Target> Repository_Board_Target
        {
            get
            {
                if (this.repository_Board_Target == null)
                {
                    this.repository_Board_Target = new GenericRepository<Board_Target>(context);
                }
                return repository_Board_Target;
            }
        }

        private GenericRepository<Board_Progress> repository_Board_Progress;
        public GenericRepository<Board_Progress> Repository_Board_Progress
        {
            get
            {
                if (this.repository_Board_Progress == null)
                {
                    this.repository_Board_Progress = new GenericRepository<Board_Progress>(context);
                }
                return repository_Board_Progress;
            }
        }

        private GenericRepository<CCCC_MixingStation> repository_CCCC_MixingStation;
        public GenericRepository<CCCC_MixingStation> Repository_CCCC_MixingStation
        {
            get
            {
                if (this.repository_CCCC_MixingStation == null)
                {
                    this.repository_CCCC_MixingStation = new GenericRepository<CCCC_MixingStation>(context);
                }
                return repository_CCCC_MixingStation;
            }
        }

        private GenericRepository<CCCC_MixingStation_ByDate> repository_CCCC_MixingStation_ByDate;
        public GenericRepository<CCCC_MixingStation_ByDate> Repository_CCCC_MixingStation_ByDate
        {
            get
            {
                if (this.repository_CCCC_MixingStation_ByDate == null)
                {
                    this.repository_CCCC_MixingStation_ByDate = new GenericRepository<CCCC_MixingStation_ByDate>(context);
                }
                return repository_CCCC_MixingStation_ByDate;
            }
        }

        private GenericRepository<CCCC_MixingStation_ByMaterial> repository_CCCC_MixingStation_ByMaterial;
        public GenericRepository<CCCC_MixingStation_ByMaterial> Repository_CCCC_MixingStation_ByMaterial
        {
            get
            {
                if (this.repository_CCCC_MixingStation_ByMaterial == null)
                {
                    this.repository_CCCC_MixingStation_ByMaterial = new GenericRepository<CCCC_MixingStation_ByMaterial>(context);
                }
                return repository_CCCC_MixingStation_ByMaterial;
            }
        }


        private GenericRepository<CCCC_MixingStation_ByQuality> repository_CCCC_MixingStation_ByQuality;
        public GenericRepository<CCCC_MixingStation_ByQuality> Repository_CCCC_MixingStation_ByQuality
        {
            get
            {
                if (this.repository_CCCC_MixingStation_ByQuality == null)
                {
                    this.repository_CCCC_MixingStation_ByQuality = new GenericRepository<CCCC_MixingStation_ByQuality>(context);
                }
                return repository_CCCC_MixingStation_ByQuality;
            }
        }


        private GenericRepository<CCCC_MixingStation_ByStation> repository_CCCC_MixingStation_ByStation;
        public GenericRepository<CCCC_MixingStation_ByStation> Repository_CCCC_MixingStation_ByStation
        {
            get
            {
                if (this.repository_CCCC_MixingStation_ByStation == null)
                {
                    this.repository_CCCC_MixingStation_ByStation = new GenericRepository<CCCC_MixingStation_ByStation>(context);
                }
                return repository_CCCC_MixingStation_ByStation;
            }
        }


        private GenericRepository<Base_LED_SyncInfo> repository_Base_LED_SyncInfo;
        public GenericRepository<Base_LED_SyncInfo> Repository_Base_LED_SyncInfo
        {
            get
            {
                if (this.repository_Base_LED_SyncInfo == null)
                {
                    this.repository_Base_LED_SyncInfo = new GenericRepository<Base_LED_SyncInfo>(context);
                }
                return repository_Base_LED_SyncInfo;
            }
        }



        private GenericRepository<View_Base_LED_SyncInfo> repository_View_Base_LED_SyncInfo;
        public GenericRepository<View_Base_LED_SyncInfo> Repository_View_Base_LED_SyncInfo
        {
            get
            {
                if (this.repository_View_Base_LED_SyncInfo == null)
                {
                    this.repository_View_Base_LED_SyncInfo = new GenericRepository<View_Base_LED_SyncInfo>(context);
                }
                return repository_View_Base_LED_SyncInfo;
            }
        }

        private GenericRepository<Scan_Point> repository_Scan_Point;
        public GenericRepository<Scan_Point> Repository_Scan_Point
        {
            get
            {
                if (this.repository_Scan_Point == null)
                {
                    this.repository_Scan_Point = new GenericRepository<Scan_Point>(context);
                }
                return repository_Scan_Point;
            }
        }

        private GenericRepository<View_BaseUser> repository_View_BaseUser;
        public GenericRepository<View_BaseUser> Repository_View_BaseUser
        {
            get
            {
                if (this.repository_View_BaseUser == null)
                {
                    this.repository_View_BaseUser = new GenericRepository<View_BaseUser>(context);
                }
                return repository_View_BaseUser;
            }
        }

        private GenericRepository<View_EnvironmentData> repository_View_EnvironmentData;
        public GenericRepository<View_EnvironmentData> Repository_View_EnvironmentData
        {
            get
            {
                if (this.repository_View_EnvironmentData == null)
                {
                    this.repository_View_EnvironmentData = new GenericRepository<View_EnvironmentData>(context);
                }
                return repository_View_EnvironmentData;
            }
        }

        private GenericRepository<Scan_EnvironmentData> repository_Scan_EnvironmentData;
        public GenericRepository<Scan_EnvironmentData> Repository_Scan_EnvironmentData
        {
            get
            {
                if (this.repository_Scan_EnvironmentData == null)
                {
                    this.repository_Scan_EnvironmentData = new GenericRepository<Scan_EnvironmentData>(context);
                }
                return repository_Scan_EnvironmentData;
            }
        }

        private GenericRepository<Scan_PersonInAndOut> repository_Scan_PersonInAndOut;
        public GenericRepository<Scan_PersonInAndOut> Repository_Scan_PersonInAndOut
        {
            get
            {
                if (this.repository_Scan_PersonInAndOut == null)
                {
                    this.repository_Scan_PersonInAndOut = new GenericRepository<Scan_PersonInAndOut>(context);
                }
                return repository_Scan_PersonInAndOut;
            }
        }

        private GenericRepository<Alarm_SMSLog> repository_Alarm_SMSLog;
        public GenericRepository<Alarm_SMSLog> Repository_Alarm_SMSLog
        {
            get
            {
                if (this.repository_Alarm_SMSLog == null)
                {
                    this.repository_Alarm_SMSLog = new GenericRepository<Alarm_SMSLog>(context);
                }
                return repository_Alarm_SMSLog;
            }
        }

        private GenericRepository<Alarm_SMSType> repository_Alarm_SMSType;
        public GenericRepository<Alarm_SMSType> Repository_Alarm_SMSType
        {
            get
            {
                if (this.repository_Alarm_SMSType == null)
                {
                    this.repository_Alarm_SMSType = new GenericRepository<Alarm_SMSType>(context);
                }
                return repository_Alarm_SMSType;
            }
        }

        private GenericRepository<Alarm_SMSUser> repository_Alarm_SMSUser;
        public GenericRepository<Alarm_SMSUser> Repository_Alarm_SMSUser
        {
            get
            {
                if (this.repository_Alarm_SMSUser == null)
                {
                    this.repository_Alarm_SMSUser = new GenericRepository<Alarm_SMSUser>(context);
                }
                return repository_Alarm_SMSUser;
            }
        }

        private GenericRepository<View_Alarm_SMSUser> repository_View_Alarm_SMSUser;
        public GenericRepository<View_Alarm_SMSUser> Repository_View_Alarm_SMSUser
        {
            get
            {
                if (this.repository_View_Alarm_SMSUser == null)
                {
                    this.repository_View_Alarm_SMSUser = new GenericRepository<View_Alarm_SMSUser>(context);
                }
                return repository_View_Alarm_SMSUser;
            }
        }

        private GenericRepository<Base_UserRole> repository_Base_UserRole;
        public GenericRepository<Base_UserRole> Repository_Base_UserRole
        {
            get
            {
                if (this.repository_Base_UserRole == null)
                {
                    this.repository_Base_UserRole = new GenericRepository<Base_UserRole>(context);
                }
                return repository_Base_UserRole;
            }
        }

        private GenericRepository<Base_RoleMenu> repository_Base_RoleMenu;
        public GenericRepository<Base_RoleMenu> Repository_Base_RoleMenu
        {
            get
            {
                if (this.repository_Base_RoleMenu == null)
                {
                    this.repository_Base_RoleMenu = new GenericRepository<Base_RoleMenu>(context);
                }
                return repository_Base_RoleMenu;
            }
        }

        private GenericRepository<View_Base_RoleMenu> repository_View_Base_RoleMenu;
        public GenericRepository<View_Base_RoleMenu> Repository_View_Base_RoleMenu
        {
            get
            {
                if (this.repository_View_Base_RoleMenu == null)
                {
                    this.repository_View_Base_RoleMenu = new GenericRepository<View_Base_RoleMenu>(context);
                }
                return repository_View_Base_RoleMenu;
            }
        }

        private GenericRepository<Scan_Position_Current> repository_Scan_Position_Current;
        public GenericRepository<Scan_Position_Current> Repository_Scan_Position_Current
        {
            get
            {
                if (this.repository_Scan_Position_Current == null)
                {
                    this.repository_Scan_Position_Current = new GenericRepository<Scan_Position_Current>(context);
                }
                return repository_Scan_Position_Current;
            }
        }

        private GenericRepository<Scan_Position_History> repository_Scan_Position_History;
        public GenericRepository<Scan_Position_History> Repository_Scan_Position_History
        {
            get
            {
                if (this.repository_Scan_Position_History == null)
                {
                    this.repository_Scan_Position_History = new GenericRepository<Scan_Position_History>(context);
                }
                return repository_Scan_Position_History;
            }
        }

        private GenericRepository<Safety_PersonInfo> repository_Safety_PersonInfo;
        public GenericRepository<Safety_PersonInfo> Repository_Safety_PersonInfo
        {
            get
            {
                if (this.repository_Safety_PersonInfo == null)
                {
                    this.repository_Safety_PersonInfo = new GenericRepository<Safety_PersonInfo>(context);
                }
                return repository_Safety_PersonInfo;
            }
        }

        private GenericRepository<Base_Project> repository_Base_Project;
        public GenericRepository<Base_Project> Repository_Base_Project
        {
            get
            {
                if (this.repository_Base_Project == null)
                {
                    this.repository_Base_Project = new GenericRepository<Base_Project>(context);
                }
                return repository_Base_Project;
            }
        }

        private GenericRepository<Base_DownloadCenter> repository_DownloadCenter;
        public GenericRepository<Base_DownloadCenter> Repository_DownloadCenter
        {
            get
            {
                if (this.repository_DownloadCenter == null)
                {
                    this.repository_DownloadCenter = new GenericRepository<Base_DownloadCenter>(context);
                }
                return repository_DownloadCenter;
            }
        }

        private GenericRepository<Scan_JC_AutoReadConfig> repository_Scan_JC_AutoReadConfig;
        public GenericRepository<Scan_JC_AutoReadConfig> Repository_Scan_JC_AutoReadConfig
        {
            get
            {
                if (this.repository_Scan_JC_AutoReadConfig == null)
                {
                    this.repository_Scan_JC_AutoReadConfig = new GenericRepository<Scan_JC_AutoReadConfig>(context);
                }
                return repository_Scan_JC_AutoReadConfig;
            }
        }

        private GenericRepository<Scan_JC_Device> repository_Scan_JC_Device;
        public GenericRepository<Scan_JC_Device> Repository_Scan_JC_Device
        {
            get
            {
                if (this.repository_Scan_JC_Device == null)
                {
                    this.repository_Scan_JC_Device = new GenericRepository<Scan_JC_Device>(context);
                }
                return repository_Scan_JC_Device;
            }
        }

        private GenericRepository<Scan_JC_Layer> repository_Scan_JC_Layer;
        public GenericRepository<Scan_JC_Layer> Repository_Scan_JC_Layer
        {
            get
            {
                if (this.repository_Scan_JC_Layer == null)
                {
                    this.repository_Scan_JC_Layer = new GenericRepository<Scan_JC_Layer>(context);
                }
                return repository_Scan_JC_Layer;
            }
        }

        private GenericRepository<View_Scan_JC_Layer> repository_View_Scan_JC_Layer;
        public GenericRepository<View_Scan_JC_Layer> Repository_View_Scan_JC_Layer
        {
            get
            {
                if (this.repository_View_Scan_JC_Layer == null)
                {
                    this.repository_View_Scan_JC_Layer = new GenericRepository<View_Scan_JC_Layer>(context);
                }
                return repository_View_Scan_JC_Layer;
            }
        }

        private GenericRepository<Scan_JC_Point> repository_Scan_JC_Point;
        public GenericRepository<Scan_JC_Point> Repository_Scan_JC_Point
        {
            get
            {
                if (this.repository_Scan_JC_Point == null)
                {
                    this.repository_Scan_JC_Point = new GenericRepository<Scan_JC_Point>(context);
                }
                return repository_Scan_JC_Point;
            }
        }

        private GenericRepository<View_Scan_JC_Point> repository_View_Scan_JC_Point;
        public GenericRepository<View_Scan_JC_Point> Repository_View_Scan_JC_Point
        {
            get
            {
                if (this.repository_View_Scan_JC_Point == null)
                {
                    this.repository_View_Scan_JC_Point = new GenericRepository<View_Scan_JC_Point>(context);
                }
                return repository_View_Scan_JC_Point;
            }
        }

        private GenericRepository<Scan_JC_Position> repository_Scan_JC_Position;
        public GenericRepository<Scan_JC_Position> Repository_Scan_JC_Position
        {
            get
            {
                if (this.repository_Scan_JC_Position == null)
                {
                    this.repository_Scan_JC_Position = new GenericRepository<Scan_JC_Position>(context);
                }
                return repository_Scan_JC_Position;
            }
        }

        private GenericRepository<View_Scan_JC_Position> repository_View_Scan_JC_Position;
        public GenericRepository<View_Scan_JC_Position> Repository_View_Scan_JC_Position
        {
            get
            {
                if (this.repository_View_Scan_JC_Position == null)
                {
                    this.repository_View_Scan_JC_Position = new GenericRepository<View_Scan_JC_Position>(context);
                }
                return repository_View_Scan_JC_Position;
            }
        }

        private GenericRepository<Scan_JC_Protocol> repository_Scan_JC_Protocol;
        public GenericRepository<Scan_JC_Protocol> Repository_Scan_JC_Protocol
        {
            get
            {
                if (this.repository_Scan_JC_Protocol == null)
                {
                    this.repository_Scan_JC_Protocol = new GenericRepository<Scan_JC_Protocol>(context);
                }
                return repository_Scan_JC_Protocol;
            }
        }

        private GenericRepository<Scan_JC_ScanType> repository_Scan_JC_ScanType;
        public GenericRepository<Scan_JC_ScanType> Repository_Scan_JC_ScanType
        {
            get
            {
                if (this.repository_Scan_JC_ScanType == null)
                {
                    this.repository_Scan_JC_ScanType = new GenericRepository<Scan_JC_ScanType>(context);
                }
                return repository_Scan_JC_ScanType;
            }
        }

        private GenericRepository<View_Scan_JC_ScanType> repository_View_Scan_JC_ScanType;
        public GenericRepository<View_Scan_JC_ScanType> Repository_View_Scan_JC_ScanType
        {
            get
            {
                if (this.repository_View_Scan_JC_ScanType == null)
                {
                    this.repository_View_Scan_JC_ScanType = new GenericRepository<View_Scan_JC_ScanType>(context);
                }
                return repository_View_Scan_JC_ScanType;
            }
        }

        private GenericRepository<Base_Position> repository_Base_Position;
        public GenericRepository<Base_Position> Repository_Base_Position
        {
            get
            {
                if (this.repository_Base_Position == null)
                {
                    this.repository_Base_Position = new GenericRepository<Base_Position>(context);
                }
                return repository_Base_Position;
            }
        }

        private GenericRepository<Base_Structor> repository_Base_Structor;
        public GenericRepository<Base_Structor> Repository_Base_Structor
        {
            get
            {
                if (this.repository_Base_Structor == null)
                {
                    this.repository_Base_Structor = new GenericRepository<Base_Structor>(context);
                }
                return repository_Base_Structor;
            }
        }

        private GenericRepository<Scan_Special_Monitor> repository_Scan_Special_Monitor;
        public GenericRepository<Scan_Special_Monitor> Repository_Scan_Special_Monitor
        {
            get
            {
                if (this.repository_Scan_Special_Monitor == null)
                {
                    this.repository_Scan_Special_Monitor = new GenericRepository<Scan_Special_Monitor>(context);
                }
                return repository_Scan_Special_Monitor;
            }
        }

        private GenericRepository<Base_Equipment_Special> repository_Base_Equipment_Special;
        public GenericRepository<Base_Equipment_Special> Repository_Base_Equipment_Special
        {
            get
            {
                if (this.repository_Base_Equipment_Special == null)
                {
                    this.repository_Base_Equipment_Special = new GenericRepository<Base_Equipment_Special>(context);
                }
                return repository_Base_Equipment_Special;
            }
        }

        private GenericRepository<Base_Cave> repository_Base_Cave;
        public GenericRepository<Base_Cave> Repository_Base_Cave
        {
            get
            {
                if (this.repository_Base_Cave == null)
                {
                    this.repository_Base_Cave = new GenericRepository<Base_Cave>(context);
                }
                return repository_Base_Cave;
            }
        }

        private GenericRepository<View_Scan_VehiclePresent> repository_View_Scan_VehiclePresent;
        public GenericRepository<View_Scan_VehiclePresent> Repository_View_Scan_VehiclePresent
        {
            get
            {
                if (this.repository_View_Scan_VehiclePresent == null)
                {
                    this.repository_View_Scan_VehiclePresent = new GenericRepository<View_Scan_VehiclePresent>(context);
                }
                return repository_View_Scan_VehiclePresent;
            }
        }

        private GenericRepository<View_Scan_Position_Current> repository_View_Scan_Position_Current;
        public GenericRepository<View_Scan_Position_Current> Repository_View_Scan_Position_Current
        {
            get
            {
                if (this.repository_View_Scan_Position_Current == null)
                {
                    this.repository_View_Scan_Position_Current = new GenericRepository<View_Scan_Position_Current>(context);
                }
                return repository_View_Scan_Position_Current;
            }
        }

        private GenericRepository<View_PositionHistory> repository_View_PositionHistory;
        public GenericRepository<View_PositionHistory> Repository_View_PositionHistory
        {
            get
            {
                if (this.repository_View_PositionHistory == null)
                {
                    this.repository_View_PositionHistory = new GenericRepository<View_PositionHistory>(context);
                }
                return repository_View_PositionHistory;
            }
        }

        private GenericRepository<View_Base_ProjectMenu> repository_View_Base_ProjectMenu;
        public GenericRepository<View_Base_ProjectMenu> Repository_View_Base_ProjectMenu
        {
            get
            {
                if (this.repository_View_Base_ProjectMenu == null)
                {
                    this.repository_View_Base_ProjectMenu = new GenericRepository<View_Base_ProjectMenu>(context);
                }
                return repository_View_Base_ProjectMenu;
            }
        }

        private GenericRepository<Base_ProjectMenu> repository_Base_ProjectMenu;
        public GenericRepository<Base_ProjectMenu> Repository_Base_ProjectMenu
        {
            get
            {
                if (this.repository_Base_ProjectMenu == null)
                {
                    this.repository_Base_ProjectMenu = new GenericRepository<Base_ProjectMenu>(context);
                }
                return repository_Base_ProjectMenu;
            }
        }

        private GenericRepository<View_Scan_EnvironmentData> repository_View_Scan_EnvironmentData;
        public GenericRepository<View_Scan_EnvironmentData> Repository_View_Scan_EnvironmentData
        {
            get
            {
                if (this.repository_View_Scan_EnvironmentData == null)
                {
                    this.repository_View_Scan_EnvironmentData = new GenericRepository<View_Scan_EnvironmentData>(context);
                }
                return repository_View_Scan_EnvironmentData;
            }
        }

        private GenericRepository<View_Base_Cave_Coordinate> repository_View_Base_Cave_Coordinate;
        public GenericRepository<View_Base_Cave_Coordinate> Repository_View_Base_Cave_Coordinate
        {
            get
            {
                if (this.repository_View_Base_Cave_Coordinate == null)
                {
                    this.repository_View_Base_Cave_Coordinate = new GenericRepository<View_Base_Cave_Coordinate>(context);
                }
                return repository_View_Base_Cave_Coordinate;
            }
        }

        private GenericRepository<Base_Cave_Coordinate> repository_Base_Cave_Coordinate;
        public GenericRepository<Base_Cave_Coordinate> Repository_Base_Cave_Coordinate
        {
            get
            {
                if (this.repository_Base_Cave_Coordinate == null)
                {
                    this.repository_Base_Cave_Coordinate = new GenericRepository<Base_Cave_Coordinate>(context);
                }
                return repository_Base_Cave_Coordinate;
            }
        }


        private GenericRepository<View_Base_Equipment> repository_View_Base_Equipment;
        public GenericRepository<View_Base_Equipment> Repository_View_Base_Equipment
        {
            get
            {
                if (this.repository_View_Base_Equipment == null)
                {
                    this.repository_View_Base_Equipment = new GenericRepository<View_Base_Equipment>(context);
                }
                return repository_View_Base_Equipment;
            }
        }

        private GenericRepository<Scan_Alarm_History> repository_Scan_Alarm_History;
        public GenericRepository<Scan_Alarm_History> Repository_Scan_Alarm_History
        {
            get
            {
                if (this.repository_Scan_Alarm_History == null)
                {
                    this.repository_Scan_Alarm_History = new GenericRepository<Scan_Alarm_History>(context);
                }
                return repository_Scan_Alarm_History;
            }
        }

        private GenericRepository<Scan_Alarm_Type> repository_Scan_Alarm_Type;
        public GenericRepository<Scan_Alarm_Type> Repository_Scan_Alarm_Type
        {
            get
            {
                if (this.repository_Scan_Alarm_Type == null)
                {
                    this.repository_Scan_Alarm_Type = new GenericRepository<Scan_Alarm_Type>(context);
                }
                return repository_Scan_Alarm_Type;
            }
        }

        private GenericRepository<Scan_Monitor_EnvItem> repository_Scan_Monitor_EnvItem;
        public GenericRepository<Scan_Monitor_EnvItem> Repository_Scan_Monitor_EnvItem
        {
            get
            {
                if (this.repository_Scan_Monitor_EnvItem == null)
                {
                    this.repository_Scan_Monitor_EnvItem = new GenericRepository<Scan_Monitor_EnvItem>(context);
                }
                return repository_Scan_Monitor_EnvItem;
            }
        }

        private GenericRepository<View_Scan_Loc_UserData> repository_View_Scan_Loc_UserData;
        public GenericRepository<View_Scan_Loc_UserData> Repository_View_Scan_Loc_UserData
        {
            get
            {
                if (this.repository_View_Scan_Loc_UserData == null)
                {
                    this.repository_View_Scan_Loc_UserData = new GenericRepository<View_Scan_Loc_UserData>(context);
                }
                return repository_View_Scan_Loc_UserData;
            }
        }

        private GenericRepository<View_Scan_JC_AutoScan> repository_View_Scan_JC_AutoScan;
        public GenericRepository<View_Scan_JC_AutoScan> Repository_View_Scan_JC_AutoScan
        {
            get
            {
                if (this.repository_View_Scan_JC_AutoScan == null)
                {
                    this.repository_View_Scan_JC_AutoScan = new GenericRepository<View_Scan_JC_AutoScan>(context);
                }
                return repository_View_Scan_JC_AutoScan;
            }
        }




        //以下不要修改
        public void SaveChange()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


    }
}

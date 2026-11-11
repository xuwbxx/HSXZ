using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OfficeShop.Model;
using WisDragon.Framework.Data.DataAccess;
using OfficeShop.DAL;
using FDScanSystem.DAO.Model;


namespace OfficeShop.DAL
{



    public class Sys_Permission_DAL
    {
        #region 功能管理
        public static FD_Function_Entity FD_Function_View(System.Int64 ID)
        {
            FD_Function_Entity c = default(FD_Function_Entity);
            try
            {
                DataCommand cmd = DataCommandManager.GetDataCommand("FD_Function_View");
                cmd.SetParameterValue("@ID", ID);
                c = cmd.ExecuteEntity<FD_Function_Entity>();
            }
            catch (Exception ex)
            {
            }
            return c;
        }

        public static FD_Function_Entity FD_Function_Add(FD_Function_Entity entity)
        {
            try
            {
                DataCommand cmd = DataCommandManager.GetDataCommand("FD_Function_Add");
                cmd.SetParameterValue("@FUNCTIONNAME", entity.FunctionName);
                cmd.SetParameterValue("@DESCRIPTION", entity.DESCRIPTION);
                cmd.SetParameterValue("@LINKURL", entity.FunctionUrl);
                cmd.SetParameterValue("@PARENTID", entity.ParentId);
                cmd.SetParameterValue("@CREATEDATE", entity.CreateDate);
                cmd.SetParameterValue("@SORT", entity.Sort);
                cmd.SetParameterValue("@Code", entity.FunctionCode);
                cmd.SetParameterValue("@IsMenu", entity.IsMenu);
                cmd.ExecuteNonQuery();
                entity.ID = Convert.ToInt32(cmd.GetParameterValue("@PID"));
            }
            catch (Exception ex)
            {
            }

            return entity;
        }

        public static void FD_Function_Update(FD_Function_Entity entity)
        {
            try
            {
                DataCommand cmd = DataCommandManager.GetDataCommand("FD_Function_Update");
                cmd.SetParameterValue("@ID", entity.ID);
                cmd.SetParameterValue("@FUNCTIONNAME", entity.FunctionName);
                cmd.SetParameterValue("@DESCRIPTION", entity.DESCRIPTION);
                cmd.SetParameterValue("@LINKURL", entity.FunctionUrl);
                cmd.SetParameterValue("@PARENTID", entity.ParentId);
                cmd.SetParameterValue("@SORT", entity.Sort);
                cmd.SetParameterValue("@Code", entity.FunctionCode);
                cmd.SetParameterValue("@IsMenu", entity.IsMenu);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
        }

        public static void FD_Function_Delete(System.Decimal ID)
        {
            try
            {
                DataCommand cmd = DataCommandManager.GetDataCommand("FD_Function_Delete");
                cmd.SetParameterValue("@ID", ID);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 功能记录数
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static int FD_Function_RecordCount(Dictionary<string, object> parameters)
        {
            int RecordCount = 0;
            try
            {
                DataCommand cmd = DataCommandManager.GetDataCommand("FD_Function_RecordCount");
                Utility.BuilderCommandParameter(parameters, cmd);
                RecordCount = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
            }
            return RecordCount;
        }

        /// <summary>
        /// 功能分页列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static List<FD_Function_Entity> FD_Function_ListByPaging(Dictionary<string, object> parameters, int pageSize, int pageIndex, out int recordCount)
        {
            List<FD_Function_Entity> list = new List<FD_Function_Entity>();
            recordCount = 0;
            try
            {
                DataCommand cmd = DataCommandManager.GetDataCommand("FD_Function_ListByPaging");
                Utility.BuilderCommandParameter(parameters, cmd);
                cmd.SetParameterValue("@pageSize", pageSize);
                cmd.SetParameterValue("@pageIndex", pageIndex);
                list = cmd.ExecuteEntityList<FD_Function_Entity>();
                recordCount = Convert.ToInt32(cmd.GetParameterValue("@recordCount"));
            }
            catch (Exception ex)
            {
            }
            return list;
        }


        /// <summary>
        /// 功能分页列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static List<FD_Function_Entity> FD_Function_List()
        {
            List<FD_Function_Entity> list = new List<FD_Function_Entity>();
            //recordCount = 0;
            try
            {
                DataCommand cmd = DataCommandManager.GetDataCommand("FD_Function_List");
                //Utility.BuilderCommandParameter(parameters, cmd);
                //cmd.SetParameterValue("@pageSize", pageSize);
                //cmd.SetParameterValue("@pageIndex", pageIndex);
                list = cmd.ExecuteEntityList<FD_Function_Entity>();
                //recordCount = Convert.ToInt32(cmd.GetParameterValue("@recordCount"));
            }
            catch (Exception ex)
            {
            }
            return list;
        }

        /// <summary>
        /// 父功能列表
        /// </summary>
        /// <returns></returns>
        public static List<FD_Function_Entity> FD_Function_GetParentFunction()
        {
            List<FD_Function_Entity> list = new List<FD_Function_Entity>();
            try
            {
                DataCommand cmd = DataCommandManager.GetDataCommand("FD_Function_GetParentFunction");
                list = cmd.ExecuteEntityList<FD_Function_Entity>();
            }
            catch (Exception ex)
            {
            }
            return list;
        }

        /// <summary>
        /// 功能全列表
        /// </summary>
        /// <returns></returns>
        public static List<FD_Function_Entity> FD_Function_GetAllFunction()
        {
            List<FD_Function_Entity> list = new List<FD_Function_Entity>();
            try
            {
                DataCommand cmd = DataCommandManager.GetDataCommand("FD_Function_GetAllFunction");
                list = cmd.ExecuteEntityList<FD_Function_Entity>();
            }
            catch (Exception ex)
            {
            }
            return list;
        }

        /// <summary>
        /// 根据用户取功能列表
        /// </summary>
        /// <returns></returns>
        public static List<FD_Function_Entity> FD_Function_ListByUserId(System.Int64 UserId)
        {
            List<FD_Function_Entity> list = new List<FD_Function_Entity>();
            try
            {
                DataCommand cmd = DataCommandManager.GetDataCommand("FD_Function_ListByUserId");
                cmd.SetParameterValue("@USERID", UserId);
                list = cmd.ExecuteEntityList<FD_Function_Entity>();
            }
            catch (Exception ex)
            {
            }
            return list;
        }
        #endregion

        #region 角色管理

        public static FD_Role_Entity FD_Role_View(System.Decimal ID)
        {
            FD_Role_Entity c = default(FD_Role_Entity);
            try
            {
                DataCommand cmd = DataCommandManager.GetDataCommand("FD_Role_View");
                cmd.SetParameterValue("@ID", ID);
                c = cmd.ExecuteEntity<FD_Role_Entity>();
            }
            catch (Exception ex)
            {
            }
            return c;
        }

        public static FD_Role_Entity FD_Role_Add(FD_Role_Entity entity)
        {
            try
            {
                DataCommand cmd = DataCommandManager.GetDataCommand("FD_Role_Add");
                cmd.SetParameterValue("@ROLENAME", entity.ROLENAME);
                cmd.SetParameterValue("@DESCRIPTION", entity.DESCRIPTION);
                cmd.SetParameterValue("@CREATEDATE", entity.CREATEDATE);
                cmd.ExecuteNonQuery();
                entity.ID = Convert.ToInt32(cmd.GetParameterValue("@PID"));
            }
            catch (Exception ex)
            {
            }
            return entity;
        }

        public static void FD_Role_Update(FD_Role_Entity entity)
        {
            try
            {
                DataCommand cmd = DataCommandManager.GetDataCommand("FD_Role_Update");
                cmd.SetParameterValue("@ID", entity.ID);
                cmd.SetParameterValue("@ROLENAME", entity.ROLENAME);
                cmd.SetParameterValue("@DESCRIPTION", entity.DESCRIPTION);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
        }

        public static void FD_Role_Delete(System.Decimal ID)
        {
            try
            {
                DataCommand cmd = DataCommandManager.GetDataCommand("FD_Role_Delete");
                cmd.SetParameterValue("@ID", ID);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 角色记录数
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static int FD_Role_RecordCount(Dictionary<string, object> parameters)
        {
            int RecordCount = 0;
            try
            {
                DataCommand cmd = DataCommandManager.GetDataCommand("FD_Role_RecordCount");
                Utility.BuilderCommandParameter(parameters, cmd);
                RecordCount = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
            }
            return RecordCount;
        }

        /// <summary>
        /// 角色分页列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static List<FD_Role_Entity> FD_Role_ListByPaging(Dictionary<string, object> parameters, int pageSize, int pageIndex, out int recordCount)
        {
            List<FD_Role_Entity> list = new List<FD_Role_Entity>();
            recordCount = 0;
            try
            {
                DataCommand cmd = DataCommandManager.GetDataCommand("FD_Role_ListByPaging");
                Utility.BuilderCommandParameter(parameters, cmd);
                cmd.SetParameterValue("@pageSize", pageSize);
                cmd.SetParameterValue("@pageIndex", pageIndex);
                list = cmd.ExecuteEntityList<FD_Role_Entity>();
                recordCount = Convert.ToInt32(cmd.GetParameterValue("@recordCount"));
            }
            catch (Exception ex)
            {
            }
            return list;
        }

        /// <summary>
        /// 角色全列表
        /// </summary>
        /// <returns></returns>
        public static List<FD_Role_Entity> FD_Role_GetAllRole()
        {
            List<FD_Role_Entity> list = new List<FD_Role_Entity>();
            try
            {
                DataCommand cmd = DataCommandManager.GetDataCommand("FD_Role_GetAllRole");
                list = cmd.ExecuteEntityList<FD_Role_Entity>();
            }
            catch (Exception ex)
            {
            }
            return list;
        }

        /// <summary>
        /// 根据用户获取角色列表
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static List<FD_Role_Entity> FD_Role_ListByUserId(System.Int64 UserId)
        {
            List<FD_Role_Entity> list = new List<FD_Role_Entity>();
            try
            {
                DataCommand cmd = DataCommandManager.GetDataCommand("FD_Role_ListByUserId");
                cmd.SetParameterValue("@USERID", UserId);
                list = cmd.ExecuteEntityList<FD_Role_Entity>();
            }
            catch (Exception ex)
            {
            }
            return list;
        }
        #endregion

        #region 角色功能关系
        /// <summary>
        /// 根据角色删除功能
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public static void FD_RoleFunction_DeleteByRoleId(System.Decimal RoleId)
        {
            try
            {
                DataCommand cmd = DataCommandManager.GetDataCommand("FD_RoleFunction_DeleteByRoleId");
                cmd.SetParameterValue("@RoleId", RoleId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 新增角色功能关系
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static FD_Role_FUNCTION_Entity FD_RoleFunction_Add(FD_Role_FUNCTION_Entity entity)
        {
            try
            {
                DataCommand cmd = DataCommandManager.GetDataCommand("FD_RoleFunction_Add");
                cmd.SetParameterValue("@ROLEID", entity.RoleId);
                cmd.SetParameterValue("@FUNCTIONID", entity.FunctionId);
                cmd.ExecuteNonQuery();
                entity.ID = Convert.ToInt32(cmd.GetParameterValue("@PID"));
            }
            catch (Exception ex)
            {
            }

            return entity;
        }

        /// <summary>
        /// 根据角色获取功能
        /// </summary>
        /// <param name="RoleId"></param>
        public static List<FD_Role_FUNCTION_Entity> FD_RoleFunction_ListByRoleId(System.Decimal RoleId)
        {
            List<FD_Role_FUNCTION_Entity> list = new List<FD_Role_FUNCTION_Entity>();
            try
            {
                DataCommand cmd = DataCommandManager.GetDataCommand("FD_RoleFunction_ListByRoleId");
                cmd.SetParameterValue("@ROLEID", RoleId);
                list = cmd.ExecuteEntityList<FD_Role_FUNCTION_Entity>();
            }
            catch (Exception ex)
            {
            }
            return list;
        }

        /// <summary>
        /// 根据用户获取功能
        /// </summary>
        /// <param name="UserId"></param>
        public static List<FD_Role_FUNCTION_Entity> FD_RoleFunction_ListByUserId(System.Decimal UserId)
        {
            List<FD_Role_FUNCTION_Entity> list = new List<FD_Role_FUNCTION_Entity>();
            try
            {
                DataCommand cmd = DataCommandManager.GetDataCommand("FD_RoleFunction_ListByUserId");
                cmd.SetParameterValue("@USERID", UserId);
                list = cmd.ExecuteEntityList<FD_Role_FUNCTION_Entity>();
            }
            catch (Exception ex)
            {
            }
            return list;
        }
        #endregion

        #region 用户管理
        public static OS_Admin_User_Entity FD_USER_View(System.Int64 Idx)
        {
            OS_Admin_User_Entity c = default(OS_Admin_User_Entity);
            try
            {
                DataCommand cmd = DataCommandManager.GetDataCommand("FD_USER_View");
                cmd.SetParameterValue("@Idx", Idx);
                c = cmd.ExecuteEntity<OS_Admin_User_Entity>();
            }
            catch (Exception ex)
            {
            }
            return c;
        }

        public static OS_Admin_User_Entity FD_USER_View(System.String UserName)
        {
            OS_Admin_User_Entity c = default(OS_Admin_User_Entity);
            try
            {
                DataCommand cmd = DataCommandManager.GetDataCommand("FD_USER_ViewByUserName");
                cmd.SetParameterValue("@UserName", UserName);
                c = cmd.ExecuteEntity<OS_Admin_User_Entity>();
            }
            catch (Exception ex)
            {
            }
            return c;
        }



        public static void FD_USER_Delete(System.Decimal ID)
        {
            try
            {
                DataCommand cmd = DataCommandManager.GetDataCommand("FD_USER_Delete");
                cmd.SetParameterValue("@ID", ID);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 用户记录数
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static int FD_USER_RecordCount(Dictionary<string, object> parameters)
        {
            int RecordCount = 0;
            try
            {
                DataCommand cmd = DataCommandManager.GetDataCommand("FD_USER_RecordCount");
                Utility.BuilderCommandParameter(parameters, cmd);
                RecordCount = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
            }
            return RecordCount;
        }

        /// <summary>
        /// 用户分页列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public static List<OS_Admin_User_Entity> FD_User_ListByPaging(Dictionary<string, object> parameters, int pageSize, int pageIndex, out int recordCount)
        {
            List<OS_Admin_User_Entity> list = new List<OS_Admin_User_Entity>();
            recordCount = 0;
            try
            {
                DataCommand cmd = DataCommandManager.GetDataCommand("FD_USER_ListByPaging");
                Utility.BuilderCommandParameter(parameters, cmd);
                cmd.SetParameterValue("@pageSize", pageSize);
                cmd.SetParameterValue("@pageIndex", pageIndex);

                list = cmd.ExecuteEntityList<OS_Admin_User_Entity>();
                recordCount = Convert.ToInt32(cmd.GetParameterValue("@recordCount"));
            }
            catch (Exception ex)
            {
            }
            return list;
        }
        #endregion

        #region 用户角色关系
        /// <summary>
        /// 根据用户删除角色
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static void FD_USER_Role_DeleteByUserId(System.Decimal UserId)
        {
            try
            {
                DataCommand cmd = DataCommandManager.GetDataCommand("FD_USER_ROLE_DeleteByUserId");
                cmd.SetParameterValue("@USERID", UserId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 新增用户角色关系
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static FD_USER_ROLE_Entity FD_USER_Role_Add(FD_USER_ROLE_Entity entity)
        {
            try
            {
                DataCommand cmd = DataCommandManager.GetDataCommand("FD_USER_ROLE_Add");
                cmd.SetParameterValue("@USERID", entity.UserId);
                cmd.SetParameterValue("@ROLEID", entity.RoleId);
                cmd.ExecuteNonQuery();
                entity.ID = Convert.ToInt32(cmd.GetParameterValue("@PID"));
            }
            catch (Exception ex)
            {
            }

            return entity;
        }

        /// <summary>
        /// 根据用户获取角色
        /// </summary>
        /// <param name="RoleId"></param>
        public static List<FD_USER_ROLE_Entity> FD_USER_Role_ListByUserId(System.Decimal UserId)
        {
            List<FD_USER_ROLE_Entity> list = new List<FD_USER_ROLE_Entity>();
            try
            {
                DataCommand cmd = DataCommandManager.GetDataCommand("FD_USER_ROLE_ListByUserId");
                cmd.SetParameterValue("@USERID", UserId);
                list = cmd.ExecuteEntityList<FD_USER_ROLE_Entity>();
            }
            catch (Exception ex)
            {
            }
            return list;
        }


        public static UserContext GetAdminUserInfo(System.String UserName)
        {
            AdminUserContext user = null;
            OS_Admin_User_Entity userInfo = FD_USER_View(UserName);
            if (userInfo != null)
            {
                user = new AdminUserContext();
                // List<FD_Role_Entity> roleInfo = FD_Role_ListByUserId(userInfo.ID);
                List<FD_Function_Entity> funcInfo = FD_Function_ListByUserId(userInfo.ID);


                //user.IsLeader = userInfo.IsLeader;
                //user.DepartId = userInfo.DepartId;
                user.UserName = userInfo.UserName;

                //user.UserType = (UserType)userInfo.UserType;
                user.FunctionRights = funcInfo;
                user.UserName = userInfo.UserName;
                user.Remark = userInfo.Remark;
                user.UserID = userInfo.ID;
                user.RealName = userInfo.RealName;
                user.LoginTime = DateTime.Now;
                user.CompanyId = userInfo.CompanyId;
                user.DepartmentName = userInfo.DepartmentName;
                user.Sex = userInfo.Sex;
                user.Professional = userInfo.Professional;
                user.Mobile = userInfo.Phone;
                user.FunctionRights = funcInfo;
                //Sys_Permission_DAL.FD_USER_Update(userInfo);
            }
            return user;
        }

        public static void FD_RoleFunction_Update(System.Decimal RoleId, List<FD_Role_FUNCTION_Entity> list)
        {
            Sys_Permission_DAL.FD_RoleFunction_DeleteByRoleId(RoleId);
            for (int i = 0; i < list.Count; i++)
            {
                FD_RoleFunction_Add(list[i]);
            }
        }


        public static void FD_USER_Role_Update(System.Decimal UserId, List<FD_USER_ROLE_Entity> list)
        {
            Sys_Permission_DAL.FD_USER_Role_DeleteByUserId(UserId);
            for (int i = 0; i < list.Count; i++)
            {
                Sys_Permission_DAL.FD_USER_Role_Add(list[i]);
            }
        }

        /// <summary>
        /// 根据角色获取功能
        /// </summary>
        /// <param name="RoleId"></param>
        public static List<FD_Role_FUNCTION_Entity> FD_Role_Function_ListByRoleId(System.Decimal RoleId)
        {
            List<FD_Role_FUNCTION_Entity> list = new List<FD_Role_FUNCTION_Entity>();
            try
            {
                DataCommand cmd = DataCommandManager.GetDataCommand("FD_RoleFunction_ListByRoleId");
                cmd.SetParameterValue("@ROLEID", RoleId);
                list = cmd.ExecuteEntityList<FD_Role_FUNCTION_Entity>();
            }
            catch (Exception ex)
            {
            }
            return list;
        }

        #endregion
    }
}

//    #region 功能管理
//    public static FD_Function_Entity FD_Function_View(System.Int64 ID) {
//        return Sys_Permission_DAL.FD_Function_View(ID);
//    }

//    public static FD_Function_Entity FD_Function_Add(FD_Function_Entity entity) {
//        return Sys_Permission_DAL.FD_Function_Add(entity);
//    }

//    public static void FD_Function_Update(FD_Function_Entity entity) {
//        Sys_Permission_DAL.FD_Function_Update(entity);
//    }

//    public static void FD_Function_Delete(System.Decimal ID) {
//        Sys_Permission_DAL.FD_Function_Delete(ID);
//    }

//    //public static int FD_Function_RecordCount(Dictionary<string, object> parameters)
//    //{
//    //    return Sys_Permission_DAL.FD_Function_RecordCount(parameters);
//    //}

//    public static List<FD_Function_Entity> FD_Function_ListByPaging(Dictionary<string, object> parameters, int pageSize, int pageIndex, out int recordCount) {
//        return Sys_Permission_DAL.FD_Function_ListByPaging(parameters, pageSize, pageIndex, out recordCount);
//    }

//    /// <summary>
//    /// 父功能列表
//    /// </summary>
//    /// <returns></returns>
//    public static List<FD_Function_Entity> FD_Function_GetParentFunction() {
//        return Sys_Permission_DAL.FD_Function_GetParentFunction();
//    }

//    /// <summary>
//    /// 功能列表
//    /// </summary>
//    /// <returns></returns>
//    public static List<FD_Function_Entity> FD_Function_GetAllFunction() {
//        return Sys_Permission_DAL.FD_Function_GetAllFunction();
//    }

//    /// <summary>
//    /// 根据用户取功能列表
//    /// </summary>
//    /// <returns></returns>
//    public static List<FD_Function_Entity> FD_Function_ListByUserId(System.Int64 UserId) {
//        return Sys_Permission_DAL.FD_Function_ListByUserId(UserId);
//    }
//    #endregion

//    #region 角色管理

//    public static FD_Role_Entity FD_Role_View(System.Decimal ID) {
//        return Sys_Permission_DAL.FD_Role_View(ID);
//    }

//    public static FD_Role_Entity FD_Role_Add(FD_Role_Entity entity) {
//        return Sys_Permission_DAL.FD_Role_Add(entity);
//    }

//    public static void FD_Role_Update(FD_Role_Entity entity) {
//        Sys_Permission_DAL.FD_Role_Update(entity);
//    }

//    public static void FD_Role_Delete(System.Decimal ID) {
//        Sys_Permission_DAL.FD_Role_Delete(ID);
//    }

//    /// <summary>
//    /// 角色记录数
//    /// </summary>
//    /// <param name="parameters"></param>
//    /// <returns></returns>
//    //public static int FD_Role_RecordCount(Dictionary<string, object> parameters)
//    //{
//    //    return Sys_Permission_DAL.FD_Role_RecordCount(parameters);
//    //}

//    /// <summary>
//    /// 角色分页列表
//    /// </summary>
//    /// <param name="pageSize"></param>
//    /// <param name="pageIndex"></param>
//    /// <param name="total"></param>
//    /// <returns></returns>
//    public static List<FD_Role_Entity> FD_Role_ListByPaging(Dictionary<string, object> parameters, int pageSize, int pageIndex, out int recordCount) {
//        return Sys_Permission_DAL.FD_Role_ListByPaging(parameters, pageSize, pageIndex, out recordCount);
//    }

//    /// <summary>
//    /// 角色全列表
//    /// </summary>
//    /// <returns></returns>
//    public static List<FD_Role_Entity> FD_Role_GetAllRole() {
//        return Sys_Permission_DAL.FD_Role_GetAllRole();
//    }

//    /// <summary>
//    /// 根据用户获取角色
//    /// </summary>
//    /// <returns></returns>
//    public static List<FD_Role_Entity> FD_Role_ListByUserId(System.Int64 UserId) {
//        return Sys_Permission_DAL.FD_Role_ListByUserId(UserId);
//    }
//    #endregion

//    #region 角色功能关系


    ///// <summary>
    ///// 根据角色获取功能
    ///// </summary>
    ///// <param name="RoleId"></param>
    //public static List<FD_Role_FUNCTION_Entity> FD_RoleFunction_ListByRoleId(System.Decimal RoleId) {
    //    return Sys_Permission_DAL.FD_RoleFunction_ListByRoleId(RoleId);
    //}

//    /// <summary>
//    /// 根据用户获取功能
//    /// </summary>
//    /// <param name="UserId"></param>
//    public static List<FD_Role_FUNCTION_Entity> FD_RoleFunction_ListByUserId(System.Decimal UserId) {
//        return Sys_Permission_Logic.FD_RoleFunction_ListByUserId(UserId);
//    }
//    #endregion

//    #region 用户管理
//    public static TS_User_Entity FD_USER_View(System.Int64 ID) {
//        return Sys_Permission_DAL.FD_USER_View(ID);
//    }

//    public static TS_User_Entity FD_USER_View(System.String UserName) {
//        return Sys_Permission_DAL.FD_USER_View(UserName);
//    }

//    public static TS_User_Entity FD_USER_Add(TS_User_Entity entity) {
//        return Sys_Permission_DAL.FD_USER_Add(entity);
//    }

//    public static void FD_USER_Update(TS_User_Entity entity) {
//        Sys_Permission_DAL.FD_USER_Update(entity);
//    }

//    public static void FD_USER_Delete(System.Decimal ID) {
//        Sys_Permission_DAL.FD_USER_Delete(ID);
//    }

//    /// <summary>
//    /// 用户记录数
//    /// </summary>
//    /// <param name="parameters"></param>
//    /// <returns></returns>
//    //public static int FD_USER_RecordCount(Dictionary<string, object> parameters)
//    //{
//    //    return Sys_Permission_DAL.FD_USER_RecordCount(parameters);
//    //}

//    /// <summary>
//    /// 用户分页列表
//    /// </summary>
//    /// <param name="pageSize"></param>
//    /// <param name="pageIndex"></param>
//    /// <param name="total"></param>
//    /// <returns></returns>
//    public static List<TS_User_Entity> FD_User_ListByPaging(Dictionary<string, object> parameters, int pageSize, int pageIndex, out int recordCount) {
//        return Sys_Permission_DAL.FD_User_ListByPaging(parameters, pageSize, pageIndex, out  recordCount);
//    }
//    #endregion

//    #region 用户角色关系
//    /// <summary>
//    /// 更新用户角色
//    /// </summary>
//    /// <param name="YserId"></param>
//    /// <param name="list"></param>
//    /// <returns></returns>
//    public static void FD_USER_Role_Update(System.Decimal UserId, List<TS_USER_ROLE_Entity> list) {
//        Sys_Permission_DAL.FD_USER_Role_DeleteByUserId(UserId);
//        for (int i = 0; i < list.Count; i++) {
//            Sys_Permission_DAL.FD_USER_Role_Add(list[i]);
//        }
//    }

//    /// <summary>
//    /// 根据用户获取角色
//    /// </summary>
//    /// <param name="UserId"></param>
//    public static List<TS_USER_ROLE_Entity> FD_USER_Role_ListByUserId(System.Decimal UserId) {
//        return Sys_Permission_DAL.FD_USER_Role_ListByUserId(UserId);
//    }
//    #endregion

//    #region 获取用户登录信息

//    public static UserContext UserLogin(System.String Account, System.String Pwd) {
//        UserContext user = null;
//        TS_User_Entity userInfo = FD_USER_View(Account);
//        if (userInfo != null && string.Compare(userInfo.Password ?? "", Pwd, true) == 0) {
//            user = new UserContext();

//            user.RealName = userInfo.Name;
//            user.LoginTime = DateTime.Now;
//            user.UserName = Account;
//            user.UserName = userInfo.UserName;

//            //Sys_Permission_DAL.FD_USER_Update(userInfo);
//        }
//        return user;
//    }

//    #endregion
//}

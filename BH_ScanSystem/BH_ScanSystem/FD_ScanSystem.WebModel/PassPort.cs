using System;
using System.Collections.Generic;
using System.Data.Linq;
using OfficeShop.WebModel.Debug;
using OfficeShop.Model;
//using OfficeShop.DAL;
using System.Linq;
namespace OfficeShop.WebModel {
    public class PassPort {
        public static bool IsAuthenticated {
            get {
                // return CurrentUser == null ? false : true;

                return true ;
            }
        }

        public static UserContext CurrentUser {
            get {

                return UserStateProvider.GetUserContext();
            }
        }


        static IUserState m_userState;
        static readonly object m_userlock = new object();

        public static IUserState UserStateProvider {
            get {
                if (m_userState == null) {
                    lock (m_userlock) {
                        if (m_userState == null) {
                            m_userState = new ReleaseUserStateProvider();
                            //   m_userState = new DebugUserStateProvider();
                        }
                    }
                }
                return m_userState;
            }
        }


        public static UserContext AdminUserLogin(string uName, string pwd) {
            return UserStateProvider.AdminLogin(uName, pwd);
        }

        public static void UserLogout() {

            UserStateProvider.Logout();
        }



    }
}

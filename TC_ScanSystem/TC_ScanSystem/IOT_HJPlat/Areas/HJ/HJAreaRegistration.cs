using System.Web.Mvc;

namespace IOT_HJPlat.Areas.HJ
{
    public class HJAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "HJ";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "HJ_default",
                "HJ/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
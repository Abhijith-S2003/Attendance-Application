using System.Web;
using System.Web.Mvc;

namespace Attendance_Application
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
        public class NoCacheAttribute : ActionFilterAttribute
        {
            public override void OnResultExecuting(ResultExecutingContext filterContext)
            {
                var response = filterContext.HttpContext.Response;
                response.Cache.SetCacheability(HttpCacheability.NoCache);
                response.Cache.SetNoStore();
                base.OnResultExecuting(filterContext);
            }
        }
    }
}

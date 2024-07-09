using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Attendance_Application.Controllers
{
    [RoutePrefix("home")]
    public class HomeController : Controller
    {
        [Route("dashboard")]
        [Authorize]
        public ActionResult Index()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
            return View();
        }
    }
}
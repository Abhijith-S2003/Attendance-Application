using Attendance_Application.Responses;
using Attendance_Application.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Attendance_Application.Controllers
{
    public class LoginController : Controller
    {
        HttpUtilityClass _httpUtility;
        SuccessResponse _successResponse;
        public LoginController()
        {
            _httpUtility = new HttpUtilityClass();
            _successResponse = new SuccessResponse();
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult SignIn()
        {
            ViewBag.UserRegSuccess = TempData["UserRegSuccess"];
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> SignIn(string username, string password)
        {

            string URL = "https://localhost:7142/api/DB/Login";
            _httpUtility = new HttpUtilityClass(URL);
            var response = await _httpUtility.LoginAPICall(username, password);

            if (response.StatusCode == 200)
            {
                Session["username"] = username;
                FormsAuthentication.SetAuthCookie(username, false);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = "Invalid username or password.";
                ViewBag.UserNotFound = "The user you are looking for was not found, Please try again!";
                return View("Error");
            }
        }
        [HttpGet]
        public async Task<ActionResult> Logout()
        {
            Session.Abandon();
            Session.Contents.RemoveAll();
            FormsAuthentication.SignOut();  
            return RedirectToAction("SignIn");
        }
    }
}
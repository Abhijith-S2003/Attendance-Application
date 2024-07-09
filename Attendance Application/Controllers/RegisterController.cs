using Attendance_Application.Models;
using Attendance_Application.Responses;
using Attendance_Application.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Attendance_Application.Controllers
{
    public class RegisterController : Controller
    {
        HttpUtilityClass _httpUtility;
        SuccessResponse _successResponse;
        public RegisterController()
        {
            _httpUtility = new HttpUtilityClass();
            _successResponse = new SuccessResponse();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Register(RegisterDB register)
        {
            if(register != null)
            {
                if (register.iAgree)
                {
                    string URL = "https://localhost:7142/api/DB/Register";
                    _httpUtility = new HttpUtilityClass(URL);
                    var response = await _httpUtility.RegisterAPICall(register);
                    if(response.StatusCode==200)
                    {
                         
                        TempData["UserRegSuccess"] = "User registered succesfully";
                        return RedirectToAction("SignIn", "Login");
                    }
                    else
                    {
                        return View();
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Terms and conditions not agreed.";
                    ViewBag.UserNotFound = "New user registeration failed , Please try again!";
                    return View("Error");
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Unable to register user.";
                ViewBag.UserNotFound = "New user registeration failed , Please try again!";
                return View("Error");
            }
        }
    }
}
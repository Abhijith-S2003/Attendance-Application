using Attendance_Application.Models;
using Attendance_Application.Responses;
using Attendance_Application.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Attendance_Application.Controllers
{
    public class UserController : Controller
    {
        HttpUtilityClass _httpUtility;
        SuccessResponse _successResponse;
        public UserController()
        {
            _httpUtility = new HttpUtilityClass();
            _successResponse = new SuccessResponse();
        }
        [HttpGet]
        [Authorize]
        public  async Task<ActionResult> Index()
        {
            string URL = "https://localhost:7142/api/DB/AllDetails";
            _httpUtility = new HttpUtilityClass(URL);
            var response = await _httpUtility.AllUserDetailsAPICall();
            if(response.StatusCode == 200)
            {
                var allUserDetails = JsonConvert.DeserializeObject<List<RegisterDB>>(response.Message);
                return View(allUserDetails);
            }
            else
            {
                ViewBag.ErrorMessage = "Not Authorized to see this webpage";
                ViewBag.UserNotFound = "Unable to fetch user details, Please contact administrator!";
                return View("Error");
            }
            
        }
    }
}
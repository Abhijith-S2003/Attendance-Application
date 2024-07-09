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
    public class UserAttendanceController : Controller
    {
        HttpUtilityClass _httpUtility;
        SuccessResponse _successResponse;
        public UserAttendanceController()
        {
            _httpUtility = new HttpUtilityClass();
            _successResponse = new SuccessResponse();
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Attendance()
        {
            string URL = "https://localhost:7142/api/DB/AllAttDetails";
            _httpUtility = new HttpUtilityClass(URL);
            var response = await _httpUtility.AllAttendanceDetailsAPICall();
            if(response.StatusCode == 200)
            {
                var allAttendanceDetails = JsonConvert.DeserializeObject<List<AttendanceModel>>(response.Message);
                return View(allAttendanceDetails);
            }
            else
            {
                ViewBag.ErrorMessage = "Not Authorized to see this webpage";
                ViewBag.UserNotFound = "Unable to fetch attendance details, Please contact administrator!";
                return View("Error");
            }
        }
    }
 }

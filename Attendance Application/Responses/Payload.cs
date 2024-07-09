using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Attendance_Application.Responses
{
    public class Payload
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public bool Status { get; set; }
    }
    public class SuccessResponse : Payload
    {
        public SuccessResponse()
        {
            StatusCode = 200;
            Message = String.Empty;
            Status = true;
        }
    }
    public class ErrorResponse : Payload
    {
        public string FriendlyError { get; set; }
        public ErrorResponse()
        {
            StatusCode = 400;
            Message = string.Empty;
            FriendlyError = string.Empty;
            Status = false;
        }
    }
}
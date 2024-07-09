using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Web;
using System.Threading.Tasks;
using System.Text;
using Attendance_Application.Responses;
using Newtonsoft.Json;
using Attendance_Application.Models;

namespace Attendance_Application.Utility
{
    public class HttpUtilityClass
    {
        private readonly string url;
        HttpClient client = new HttpClient();
        public HttpUtilityClass()
        {

        }
        public HttpUtilityClass(string url)
        {
            this.url = url;
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<Payload> LoginAPICall(string username,string password)
        {

            var builder = new UriBuilder(url);
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["username"] = username;
            query["password"] = password;
            builder.Query = query.ToString();
            string finalUrl = builder.ToString();


            try
            {
                HttpResponseMessage res = await client.GetAsync(finalUrl);
                if (res.IsSuccessStatusCode)
                {
                    var response = res.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<SuccessResponse>(response);
                }
                else
                {
                    var response = res.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<ErrorResponse>(response);
                }
            }
            catch (Exception ex)
            {
                ErrorResponse error = new ErrorResponse();
                error.Message = ex.Message;
                return error;
            }
        }
        public async Task<Payload> RegisterAPICall(RegisterDB register)
        {
            try
            {
                string json = JsonConvert.SerializeObject(register);
                HttpResponseMessage res = await client.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
                if (res.IsSuccessStatusCode)
                {
                    var response = res.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<SuccessResponse>(response);
                }
                else
                {
                    var response = res.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<ErrorResponse>(response);
                }
            }
            catch (Exception ex)
            {
                ErrorResponse error = new ErrorResponse();
                error.Message = ex.Message;
                return error;
            }
        }
        public async Task<Payload> AllUserDetailsAPICall()
        {
            try
            {
                HttpResponseMessage res = await client.GetAsync(url);
                if (res.IsSuccessStatusCode)
                {
                    var response = res.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<SuccessResponse>(response);
                }
                else
                {
                    var response = res.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<ErrorResponse>(response);
                }
            }
            catch (Exception ex)
            {
                ErrorResponse error = new ErrorResponse();
                error.Message = ex.Message;
                return error;
            }
        }
        public async Task<Payload> AllAttendanceDetailsAPICall()
        {
            try
            {
                HttpResponseMessage res = await client.GetAsync(url);
                if(res.IsSuccessStatusCode)
                {
                    var response = res.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<SuccessResponse>(response);
                }
                else
                {
                    var response = res.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<ErrorResponse>(response);
                }
            }
            catch (Exception ex)
            {
                ErrorResponse error = new ErrorResponse();
                error.Message = ex.Message;
                return error;
            }
        }
    }
}
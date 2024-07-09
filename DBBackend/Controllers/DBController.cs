using DBBackend.Data;
using DBBackend.Model;
using DBBackend.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace DBBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DBController : ControllerBase
    {
        DBConnection DBConnection;
        public DBController(IConfiguration configuration)
        {
            DBConnection = new DBConnection(configuration);
        }
        [HttpGet]
        [Route("Login")]
        public async Task<Payload> Login(string username, string password)
        {
            try
            {
                if(!String.IsNullOrEmpty(username) && !String.IsNullOrEmpty(password))
                {
                    LoginTable loginData = DBConnection.GetLoginTableData(username, password);
                    if (!String.IsNullOrEmpty(loginData.UserName) && !String.IsNullOrEmpty(loginData.Password))
                    {
                        SuccessResponse success = new SuccessResponse
                        {
                            Message = "Login Success"
                        };
                        return success;
                    }
                    else
                    {
                        ErrorResponse error = new ErrorResponse
                        {
                            Message = "Login Failed"
                        };
                        return error;
                    }

                }
                else
                {
                    ErrorResponse error = new ErrorResponse
                    {
                        Message = "UserName or Password became NULL"
                    };
                    return error;
                }

            }
            catch (Exception ex)
            {
                ErrorResponse error = new ErrorResponse
                {
                    Message = "Exception Occured",
                    FriendlyError = ex.Message
                };
                return error;
            }
        }
        [HttpPost]
        [Route ("Register")]
        public async Task<Payload> Register(RegisterDB register)
        {
            if(!String.IsNullOrEmpty(register.UserName) && !String.IsNullOrEmpty(register.Email) && !String.IsNullOrEmpty(register.Password) && register.PhNo !=null)
            {
                try
                {
                    bool inserStatus = DBConnection.InsertRegisterDBData(register);
                    if (inserStatus) 
                    {
                        SuccessResponse success = new SuccessResponse
                        {
                            Message = "Registered Successfully"
                        };
                        return success;
                    }
                    else
                    {
                        ErrorResponse error = new ErrorResponse
                        {
                            Message = "Unable to Register"
                        };
                        return error;
                    }
                }
                catch (Exception ex)
                {
                    ErrorResponse error = new ErrorResponse
                    {
                        Message = "Unable to Register",
                        FriendlyError = ex.Message
                    };
                    return error;
                }
            }
            else
            {
                ErrorResponse error = new ErrorResponse
                {
                    Message = "Unable to Register"
                };
                return error;
            }
        }
        [HttpGet]
        [Route("AllDetails")]
        public async Task<Payload> AllDetails()
        {
            try
            {
               List<RegisterDB> getAllData =  DBConnection.GetAllRegisteredData();
                if (getAllData != null && getAllData.Count > 0)
                {
                    string serializeObject = JsonConvert.SerializeObject(getAllData);
                    SuccessResponse success = new SuccessResponse
                    {
                        Message = serializeObject
                    };
                    return success;
                }
                else
                {
                    ErrorResponse error = new ErrorResponse
                    {
                        Message = "Unable to Fetch Data"
                    };
                    return error;
                }
            }
            catch (Exception ex)
            {
                ErrorResponse error = new ErrorResponse
                {
                    Message = "Unable to Fetch Data",
                    FriendlyError = ex.Message
                };
                return error;
            }
        }
        [HttpPut]
        [Route("Update")]
        public async Task<Payload> Update(string? UserName=null,string? Password=null,string? Email=null , int ID=0,long PhNo = 0)
        {
            try
            {
                if (ID == 0 || ID == null)
                {
                    ErrorResponse error = new ErrorResponse
                    {
                        Message = "Update failed"
                    };
                    return error;
                }

                RegisterDB register = new RegisterDB
                {
                    ID = ID,
                    UserName = UserName,
                    Password = Password,
                    Email = Email,
                    PhNo = PhNo
                };

                bool updateStatus = DBConnection.UpdateRegisterDBData(register);
                if (updateStatus)
                {
                    SuccessResponse success = new SuccessResponse
                    {
                        Message = "Updated Successfully"
                    };
                    return success;
                }
                else
                {
                    ErrorResponse error = new ErrorResponse
                    {
                        Message = "Update failed"
                    };
                    return error;
                }
                
            }
            catch (Exception ex)
            {
                ErrorResponse error = new ErrorResponse
                {
                    Message = "Update failed ",
                    FriendlyError = ex.Message
                };
                return error;
            }
        }
        [HttpDelete]
        [Route("Delete")]
        public async Task<Payload> Delete(int ID=0 ,string UserName=null)
        {
            try
            {
                bool deleteStatus = DBConnection.DeleteRegisterDB(ID,UserName);
                if (deleteStatus)
                {
                    SuccessResponse success = new SuccessResponse
                    {
                        Message = "Deleted Successfully"
                    };
                    return success;
                }
                else
                {
                    ErrorResponse error = new ErrorResponse
                    {
                        Message = "Delete failed"
                    };
                    return error;
                }
            }
            catch (Exception ex)
            {
                ErrorResponse error = new ErrorResponse
                {
                    Message = "Delete failed ",
                    FriendlyError = ex.Message
                };
                return error;
            }
        }
        [HttpPost]
        [Route("InsertAtt")]
        public async Task<Payload> InsertAtt(AttendanceModel attendance)
        {
            try
            {
                bool insertStatus = DBConnection.InsertAttendanceDB(attendance);
                if (insertStatus)
                {
                    SuccessResponse success = new SuccessResponse
                    {
                        Message = "Inserted Succesfully"
                    };
                    return success;
                }
                else
                {
                    ErrorResponse error = new ErrorResponse
                    {
                        Message = "Insertion Failed"
                    };
                    return error;
                }
            }
            catch (Exception ex)
            {
                ErrorResponse error = new ErrorResponse
                {
                    Message = "Insertion failed ",
                    FriendlyError = ex.Message
                };
                return error;
            }
        }
        [HttpPut]
        [Route("UpdateAttendance")]
        public async Task<Payload> UpdateAttendance(AttendanceModel attendance)
        {
            try
            {
                bool insertStatus = DBConnection.UpdateAttendanceDB(attendance);
                if (insertStatus)
                {
                    SuccessResponse success = new SuccessResponse
                    {
                        Message = "Updated Succesfully"
                    };
                    return success;
                }
                else
                {
                    ErrorResponse error = new ErrorResponse
                    {
                        Message = "Update Failed"
                    };
                    return error;
                }
            }
            catch (Exception ex)
            {
                ErrorResponse error = new ErrorResponse
                {
                    Message = "Update failed ",
                    FriendlyError = ex.Message
                };
                return error;
            }
        }
        [HttpDelete]
        [Route("DeleteAtt")]
        public async Task<Payload> DeleteAtt(int Reg_Ref_ID)
        {
            try
            {
                bool insertStatus = DBConnection.DeleteAttendanceDB(Reg_Ref_ID);
                if (insertStatus)
                {
                    SuccessResponse success = new SuccessResponse
                    {
                        Message = "Delete Succesfully"
                    };
                    return success;
                }
                else
                {
                    ErrorResponse error = new ErrorResponse
                    {
                        Message = "Delete Failed"
                    };
                    return error;
                }
            }
            catch (Exception ex)
            {
                ErrorResponse error = new ErrorResponse
                {
                    Message = "Delete failed ",
                    FriendlyError = ex.Message
                };
                return error;
            }
        }
        [HttpGet]
        [Route ("AllAttDetails")]
        public async Task<Payload> AllAttDetails()
        {
            try
            {
                List<AttendanceModel> getAllAttData = DBConnection.GetAllAttendance();
                if (getAllAttData != null && getAllAttData.Count > 0)
                {
                    string serializeObject = JsonConvert.SerializeObject(getAllAttData);
                    SuccessResponse success = new SuccessResponse
                    {
                        Message = serializeObject
                    };
                    return success;
                }
                else
                {
                    ErrorResponse error = new ErrorResponse
                    {
                        Message = "Unable to Fetch Attendance Data"
                    };
                    return error;
                }
            }
            catch (Exception ex)
            {
                ErrorResponse error = new ErrorResponse
                {
                    Message = "Unable to Fetch Attendance Data",
                    FriendlyError = ex.Message
                };
                return error;
            }
        }
        [HttpGet]
        [Route("UserAttDetails")]
        public async Task<Payload> UserAttDetails(int Reg_Ref_ID)
        {
            try
            {
                List<AttendanceModel> getUserAtt = DBConnection.GetAllUserAttendance(Reg_Ref_ID);
                if(getUserAtt !=null && getUserAtt.Count > 0)
                {
                    string serializeObject = JsonConvert.SerializeObject(getUserAtt);
                    SuccessResponse success = new SuccessResponse
                    {
                        Message = serializeObject
                    };
                    return success;
                }
                else
                {
                    ErrorResponse error = new ErrorResponse
                    {
                        Message = "Unable to Fetch User Attendance Data"
                    };
                    return error;
                }
            }
            catch (Exception ex)
            {
                ErrorResponse error = new ErrorResponse
                {
                    Message = "Unable to Fetch User Attendance Data",
                    FriendlyError = ex.Message
                };
                return error;
            }
        }
    }
}

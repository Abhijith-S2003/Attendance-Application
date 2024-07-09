using DBBackend.Model;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using System.Runtime.CompilerServices;
using Microsoft.Win32;
using System.Diagnostics.Eventing.Reader;

namespace DBBackend.Data
{
    public class DBConnection
    {
        private string _connectionString;
        SqlConnection conn;
        public DBConnection(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default");
            conn = new SqlConnection(_connectionString);
            conn.Open();
        }
        public LoginTable GetLoginTableData(string Username, string Password)
        {
            LoginTable loginTable = new LoginTable();
            if (!String.IsNullOrEmpty(Username) && !String.IsNullOrEmpty(Password))
            {
                try
                {
                    string command = "SP_SELECT_LoginTable";
                    using (var cmd = new SqlCommand(command, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@USERNAME", Username);
                        cmd.Parameters.AddWithValue("@PASSWORD", Password);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (reader["UserName"] != DBNull.Value && reader["Password"] != DBNull.Value)
                                {
                                    loginTable.UserName = reader["UserName"].ToString();
                                    loginTable.Password = reader["Password"].ToString();
                                }
                            }
                        }
                        command = "SP_UPDATE_LoginTable";
                        using (var cmd2 = new SqlCommand(command, conn))
                        {
                            cmd2.CommandType = CommandType.StoredProcedure;
                            cmd2.Parameters.AddWithValue("@USERNAME", Username);
                            cmd2.Parameters.AddWithValue("@LASTLOGIN", DateTime.Now);
                            int noOfRowsAffected = cmd2.ExecuteNonQuery();
                            if (noOfRowsAffected > 0)
                            {
                                Console.WriteLine("LastLogin Updated");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }
            return loginTable;
        }
        public bool InsertRegisterDBData(RegisterDB register)
        {
            if (!String.IsNullOrEmpty(register.UserName) && !String.IsNullOrEmpty(register.Email) && !String.IsNullOrEmpty(register.Password) && register.PhNo != null)
            {
                try
                {
                    string command = "SP_INSERT_RegisterDB";
                    using (var cmd = new SqlCommand(command, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@USERNAME", register.UserName);
                        cmd.Parameters.AddWithValue("@EMAIL", register.Email);
                        cmd.Parameters.AddWithValue("@PASSWORD", register.Password);
                        cmd.Parameters.AddWithValue("@PHNO", register.PhNo);
                        int numberOfRowsAffected = cmd.ExecuteNonQuery();
                        if (numberOfRowsAffected > 0)
                        {
                            command = ("SELECT IDENT_CURRENT('dbo.RegisterDB')");
                            SqlCommand newCommand = new SqlCommand(command, conn);
                            int lastValue = Convert.ToInt32(newCommand.ExecuteScalar());
                            command = "SP_INSERT_LoginTable";
                            var cmd2 = new SqlCommand(command, conn);
                            cmd2.CommandType = CommandType.StoredProcedure;
                            cmd2.Parameters.AddWithValue("@USERNAME", register.UserName);
                            cmd2.Parameters.AddWithValue("@PASSWORD", register.Password);
                            cmd2.Parameters.AddWithValue("@LASTLOGIN", DateTime.Today);
                            cmd2.Parameters.AddWithValue("@Reg_Ref_ID", lastValue);
                            numberOfRowsAffected = cmd2.ExecuteNonQuery();
                            if (numberOfRowsAffected > 0)
                            {
                                return true;
                            }
                            else
                            {
                                command = "SP_DELETE_RegisterDB";
                                using (var cmd3 = new SqlCommand(command, conn))
                                {
                                    cmd3.CommandType = CommandType.StoredProcedure;
                                    cmd3.Parameters.AddWithValue("@USERNAME", register.UserName);
                                    int noOfRowsAffected = cmd3.ExecuteNonQuery();
                                    return false;
                                }
                            }

                        }
                        else
                        {
                            command = "SP_DELETE_RegisterDB";

                            using (var cmd3 = new SqlCommand(command, conn))
                            {
                                cmd3.CommandType = CommandType.StoredProcedure;
                                cmd3.Parameters.AddWithValue("@USERNAME", register.UserName);
                                int noOfRowsAffected = cmd3.ExecuteNonQuery();
                                return false;
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    if (!ex.Message.Contains("Unique", StringComparison.CurrentCultureIgnoreCase))
                    {
                        string command = "SP_DELETE_RegisterDB";
                        using (var cmd = new SqlCommand(command, conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@USERNAME", register.UserName);
                            int noOfRowsAffected = cmd.ExecuteNonQuery();
                        }
                    }
                    return false;

                }
            }
            else
            {
                return false;
            }
        }
        public List<RegisterDB> GetAllRegisteredData()
        {
            List<RegisterDB> getRegister = new List<RegisterDB>();
            try
            {
                string command = "SP_SELECT_RegisterDB";
                using (var cmd = new SqlCommand(command, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RegisterDB register = new RegisterDB();
                            register.ID = Convert.ToInt32(reader["ID"]);
                            register.UserName = reader["UserName"].ToString();
                            register.Email = reader["Email"].ToString();
                            register.PhNo = Convert.ToInt64(reader["PhNo"]);
                            //RegisterDB register = new RegisterDB
                            //{
                            //    ID = Convert.ToInt32(reader["ID"]),
                            //    UserName = reader["UserName"].ToString(),
                            //    Email = reader["Email"].ToString(),
                            //    PhNo = Convert.ToInt64(reader["PhNo"])
                            //};
                            getRegister.Add(register);
                        }
                    }

                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return getRegister;
        }
        public bool UpdateRegisterDBData(RegisterDB register)
        {
            if (register.ID != null)
            {
                try
                {
                    string command = "SP_UPDATE_RegisterDB";
                    using (var cmd = new SqlCommand(command, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if(!String.IsNullOrEmpty(register.UserName))
                        {
                            cmd.Parameters.AddWithValue("@USERNAME", register.UserName);
                        }
                        if (!String.IsNullOrEmpty(register.Email))
                        {
                            cmd.Parameters.AddWithValue("@EMAIL", register.Email);
                        }
                        if(!String.IsNullOrEmpty(register.Password))
                        {
                            cmd.Parameters.AddWithValue("@PASSWORD", register.Password);
                        }
                        if(register.PhNo != 0)
                        {
                            cmd.Parameters.AddWithValue("@PHNO", register.PhNo);
                        }
                        cmd.Parameters.AddWithValue("@ID", register.ID);
                        int numberOfRowsAffected = cmd.ExecuteNonQuery();
                        if (numberOfRowsAffected > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public bool DeleteRegisterDB(int ID=0,string UserName=null)
        {
            try
            {
                string command = "SP_DELETE_RegisterDB";
                using (var cmd = new SqlCommand(command, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if(ID != null && ID !=0)
                    {
                        cmd.Parameters.AddWithValue("@ID",ID);
                    }
                    if(!String.IsNullOrEmpty(UserName))
                    {
                        cmd.Parameters.AddWithValue("USERNAME",UserName);
                    }
                    int numberOfRowsAffected = cmd.ExecuteNonQuery();
                    if (numberOfRowsAffected > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public bool InsertAttendanceDB(AttendanceModel attendance)
        {
            try
            {
                string command = "SP_INSERT_Attendance";
                using (var cmd = new SqlCommand(command , conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (attendance.Reg_Ref_ID != null && attendance.LoginTime != null && attendance.LogoutTime != null)
                    {
                        cmd.Parameters.AddWithValue("@REG_REF_ID",attendance.Reg_Ref_ID);
                        cmd.Parameters.AddWithValue("@LOGINTIME",attendance.LoginTime);
                        cmd.Parameters.AddWithValue("@LOGOUTTIME",attendance.LogoutTime);
                        int numberOfRowsAffected = cmd.ExecuteNonQuery();
                        if (numberOfRowsAffected > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public bool UpdateAttendanceDB(AttendanceModel attendance)
        {
            try
            {
                string command = "SP_UPDATE_Attendance";
                using (var cmd = new SqlCommand(command, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (attendance.Reg_Ref_ID != null && attendance.LoginTime != null && attendance.LogoutTime != null)
                    {
                        cmd.Parameters.AddWithValue("@REG_REF_ID", attendance.Reg_Ref_ID);
                        cmd.Parameters.AddWithValue("@LOGINTIME", attendance.LoginTime);
                        cmd.Parameters.AddWithValue("@LOGOUTTIME", attendance.LogoutTime);
                        int numberOfRowsAffected = cmd.ExecuteNonQuery();
                        if (numberOfRowsAffected > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false ;
            }
        }
        public bool DeleteAttendanceDB(int Reg_Ref_ID)
        {
            try
            {
                string command = "SP_DELETE_Attendance";
                using (var cmd = new SqlCommand(command, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (Reg_Ref_ID != null)
                    {
                        cmd.Parameters.AddWithValue("@REG_REF_ID", Reg_Ref_ID);
                        int numberOfRowsAffected = cmd.ExecuteNonQuery();
                        if (numberOfRowsAffected > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
        public List<AttendanceModel> GetAllAttendance()
        {
            List<AttendanceModel> getRegister = new List<AttendanceModel>();
            try
            {
                string command = "SP_SELECTALL_Attendance";
                using (var cmd = new SqlCommand(command, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AttendanceModel model = new AttendanceModel();
                            model.Reg_Ref_ID = Convert.ToInt32(reader["REG_REF_ID"]);
                            model.LoginTime = Convert.ToDateTime(reader["LoginTime"]);
                            model.LogoutTime = Convert.ToDateTime(reader["LogoutTime"]);
                            model.THour = Convert.ToDouble(reader["THours"]);
                            getRegister.Add(model);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return getRegister;
        }
        public List<AttendanceModel> GetAllUserAttendance(int Reg_Ref_ID)
        {
            List<AttendanceModel> getUserRegister = new List<AttendanceModel>();
            try
            {
                string command = "SP_SELECTUSER_Attendance";
                using (var cmd = new SqlCommand(command, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@REG_REF_ID",Reg_Ref_ID);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AttendanceModel user = new AttendanceModel();
                            user.Reg_Ref_ID = Convert.ToInt32(reader["REG_REF_ID"]);
                            user.LoginTime = Convert.ToDateTime(reader["LoginTime"]);
                            user.LogoutTime = Convert.ToDateTime(reader["LogoutTime"]);
                            user.THour = Convert.ToDouble(reader["THours"]);
                            getUserRegister.Add(user);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return getUserRegister;
        }
        ~DBConnection()
        {
            conn.Close();
            conn.Dispose();
        }
    }
}

using MeetingRoom1.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace MeetingRoom1.Repository
{
    public class EmployeeRL : IEmployeeRL
    {
        string dbpath = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MeetingRoom;Integrated Security=True";

        SqlConnection sqlConnection;

        public EmployeeRL(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public List<MeetingRoomModel> meetingroomlist = new List<MeetingRoomModel>();
        public List<EmployeeModel> employeemodel = new List<EmployeeModel>();
        public List<RequestModel> requestmodel = new List<RequestModel>();
        SqlDataReader reader;

        public EmployeeModel AddEmployee(EmployeeModel employee)
        {
            try
            {
                sqlConnection = new SqlConnection(dbpath);
                SqlCommand command = new SqlCommand("Meeting_usp_Employee ", sqlConnection);
                command.CommandType = System.Data.CommandType.StoredProcedure;


                sqlConnection.Open();

                // command.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
                command.Parameters.AddWithValue("@Email", employee.Email);
                command.Parameters.AddWithValue("@Password", employee.Password);
                command.Parameters.AddWithValue("@BranchName", employee.BranchName);
                command.Parameters.AddWithValue("@RoleId", employee.Role);
                command.ExecuteNonQuery();
                return employee;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        public EmployeeModel UserLogin(LoginModel loginModel)
        {
            sqlConnection = new SqlConnection(dbpath);
            using (sqlConnection)
            {
                try
                {
                    SqlCommand command = new SqlCommand("Meeting_usp_EmployeeLogin ", sqlConnection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;


                    sqlConnection.Open();
                    command.Parameters.AddWithValue("@Email", loginModel.Email);
                    command.Parameters.AddWithValue("@Password", loginModel.Password);

                    var result = command.ExecuteScalar();
                    if (result != null)
                    {
                        string query = "SELECT * FROM tbl_EmployeeReg WHERE Email = '" + result + "'";
                        SqlCommand cmd = new SqlCommand(query, sqlConnection);
                        reader = cmd.ExecuteReader();
                        EmployeeModel employeeemodel = new EmployeeModel();
                        while (reader.Read()) {
                            employeeemodel.EmployeeId = Convert.ToInt32(reader["EmployeeId"]);
                            employeeemodel.Email = reader["Email"].ToString();
                            employeeemodel.Password = reader["Password"].ToString();
                            employeeemodel.BranchName = reader["BranchName"].ToString();
                            employeeemodel.Role = Convert.ToInt32(reader["RoleId"]);
                        }
                        return employeeemodel;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
                finally
                {
                    sqlConnection.Close();
                }

            }
        }


        //private string GenerateSecurityToken(string Email, string EmployeeId)
        //{
        //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:key"]));
        //    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        //    var claims = new[] {
        //        new Claim(ClaimTypes.Role,"Employee"),
        //        new Claim(ClaimTypes.Email,Email),
        //      //  new Claim("EmployeeId",EmployeeId.ToString())
        //        new Claim(ClaimTypes.NameIdentifier, EmployeeId.ToString())
        //    };

        //    var token = new JwtSecurityToken(Configuration["JWT:key"],
        //      Configuration["JWT:key"],
        //      claims,
        //      expires: DateTime.Now.AddMinutes(60),
        //      signingCredentials: credentials);
        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}


        public IEnumerable<MeetingRoomModel> GetAllMeetingRooms(string BranchName)
        {
          
            sqlConnection = new SqlConnection(dbpath);
            try
            {
                SqlCommand command = new SqlCommand("spGetAllMeetingRooms ", sqlConnection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                sqlConnection.Open();
                command.Parameters.AddWithValue("@BranchName",BranchName);


                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                       meetingroomlist.Add(new MeetingRoomModel
                       {
                           MeetingRoom_Id = Convert.ToInt32(reader["MeetingRoom_Id"]),
                           FloorNo = Convert.ToInt32(reader["FloorNo"]),
                           RoomNo = Convert.ToInt32(reader["RoomNo"]),
                           Desktops = Convert.ToInt32(reader["Desktops"]),
                           Projectors = Convert.ToInt32(reader["Projectors"]),
                           Capacity = Convert.ToInt32(reader["Capacity"]),
                           BranchName = reader["BranchName"].ToString(),
                       });
                    }
                    return meetingroomlist;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                sqlConnection.Close();
            }


        }

        public RequestModel AddRequest(RequestModel employee, int UserId, int MeetingRoom_Id)
        {
            try
            {
                sqlConnection = new SqlConnection(dbpath);
                SqlCommand command = new SqlCommand("MeetingRequest", sqlConnection);
                command.CommandType = System.Data.CommandType.StoredProcedure;


                sqlConnection.Open();

                command.Parameters.AddWithValue("@UserId", UserId);
                command.Parameters.AddWithValue("@MeetingRoom_Id", MeetingRoom_Id);
                command.Parameters.AddWithValue("@StartTime", employee.StartTime);
                command.Parameters.AddWithValue("@EndTime", employee.EndTime);
                command.Parameters.AddWithValue("@Mdate", employee.MDate);
                command.Parameters.AddWithValue("@Purpose", employee.Purpose);
                command.Parameters.AddWithValue("@RequestFor", employee.RequestFor);
                command.Parameters.AddWithValue("@NoOfEmps", employee.NoOfEmps);
                command.ExecuteNonQuery();
                return employee;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        public IEnumerable<EmployeeModel> GetAllEmployees()
        {

            sqlConnection = new SqlConnection(dbpath);
            try
            {
                SqlCommand command = new SqlCommand("spGetAllEmployees ", sqlConnection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                sqlConnection.Open();


                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        employeemodel.Add(new EmployeeModel
                        {
                            EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                            Email = reader["Email"].ToString(),
                            BranchName = reader["BranchName"].ToString(),
                           // Role = Convert.ToInt32(reader["RoleId"]),
                        });
                    }
                    return employeemodel;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                sqlConnection.Close();
            }


        }

        public IEnumerable<RequestModel> GetAllRequests()
        {

            sqlConnection = new SqlConnection(dbpath);
            try
            {
                SqlCommand command = new SqlCommand("spGetAllRequests ", sqlConnection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                sqlConnection.Open();


                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        requestmodel.Add(new RequestModel
                        {
                            UserId = Convert.ToInt32(reader["UserId"]),
                            Id = Convert.ToInt32(reader["MeetingRoom_Id"]),
                            StartTime = reader["StartTime"].ToString(),
                            EndTime = reader["EndTime"].ToString(),
                            MDate = reader["MDate"].ToString(),
                            Purpose = reader["Purpose"].ToString(),
                            RequestFor = reader["RequestFor"].ToString(),
                            NoOfEmps = Convert.ToInt32(reader["NoOfEmps"]),
                            Status = reader["ReqStatus"].ToString(),
                            Request_Id = Convert.ToInt32(reader["Request_Id"]),

                        });                
                    }
                    return requestmodel;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                sqlConnection.Close();
            }


        }

       

    }
    
}

using MeetingRoom1.Models;
using MeetingRoom1.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;


namespace MeetingRoom1.Controllers
{
    public class EmployeeController : Controller
    {

        IEmployeeRL employeeRL;


        public EmployeeController(IEmployeeRL employeeRL)
        {
            this.employeeRL = employeeRL;
        }
        public int MeetingRoom_Id;
        public int UserId;



        [HttpGet]
        public IActionResult ListOfMeetingRooms()
        {
            //List<MeetingRoomModel> meetingroomlist = new List<MeetingRoomModel>();
            var BranchName = HttpContext.Session.GetString("BranchName");

            var meetingroomlist = employeeRL.GetAllMeetingRooms(BranchName);
            //ViewBag.model = meetingroomlist;
            //int MeetingRoom_Id = ViewBag.model.MeetingRoom_Id;  

            return View(meetingroomlist);

        }

        [HttpGet]
        public IActionResult AddEmp()
        {


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddEmp([Bind] EmployeeModel employee)
        {
            if (ModelState.IsValid)
            {
                employeeRL.AddEmployee(employee);
                //return RedirectToAction("Employee/Login");
            }
            return View(employee);
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        // [Authorize(Roles = "Admin,Employee")]
        public IActionResult Login([Bind] LoginModel loginModel)
        {
            string message = string.Empty;
            if (ModelState.IsValid)
            {
                var result = employeeRL.UserLogin(loginModel);

                if (result != null)
                {
                    //  HttpContext.Session.SetInt32("user_id", loginModel.user_id);
                    HttpContext.Session.SetString("Email", result.Email);
                    HttpContext.Session.SetString("Password", result.Password);
                    HttpContext.Session.SetInt32("EmployeeId", result.EmployeeId);
                    HttpContext.Session.SetString("BranchName", result.BranchName);
                    HttpContext.Session.SetInt32("Role", result.Role);
                    if (result.Role.Equals(1))
                    {
                        return RedirectToAction("Dashboard");
                    }
                    else if (result.Role.Equals(2))
                    {
                        return RedirectToAction("AdminView");
                    }
                    else
                    {
                        return RedirectToAction("BranchManagerView");
                    }
                    //message = "Username and/or password is correct.";
                    //Console.WriteLine(message);
                    // return RedirectToAction("Dashboard");

                }
                else
                {
                    return RedirectToAction("Login");
                }


            }
            return null;
        }



        [HttpGet]
        public IActionResult AddRequest()
        {



            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddRequest([Bind] RequestModel employee, int id)
        {

            if (ModelState.IsValid)
            {

                int UserId = (int)HttpContext.Session.GetInt32("EmployeeId");
                int MeetingRoom_Id = id;
                employeeRL.AddRequest(employee, UserId, MeetingRoom_Id);
                // HttpContext.Session.SetInt32("MeetingRoom_Id", result.MeetingRoom_Id)
                return RedirectToAction("BranchManagerView");

            }
            return View(employee);
        }

        [HttpGet]
        public IActionResult Dashboard()
        {



            return View();
        }
        [HttpGet]
        public IActionResult AdminView()
        {
            return View();
        }


        public IActionResult ListOfEmployees()
        {
            var employeemodel = employeeRL.GetAllEmployees();

            return View(employeemodel);


            return View();
        }
        [HttpGet]
        public IActionResult BranchManagerView()
        {
            var requestmodel = employeeRL.GetAllRequests();

            return View(requestmodel);


        }
        //[HttpGet]
        //public IActionResult Status()
        //{
        //    return View();
        //}

       [HttpGet]
        public IActionResult Status(string name, int Request_Id)
        {
            string dbpath = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=MeetingRoom;Integrated Security=True";
            SqlConnection sqlConnection = new SqlConnection(dbpath);
            switch (name)
            {
                case "Accept":

                    using (sqlConnection)
                    {
                        try
                        {
                            string query = " Update tbl_MeetingRequest set ReqStatus= 'Accepted' where Request_Id= " + Request_Id;
                            SqlCommand cmd = new SqlCommand(query, sqlConnection);


                            sqlConnection.Open();
                           
                            int i = cmd.ExecuteNonQuery();

                            sqlConnection.Close();

                            if (i >= 1)

                            {
                                return RedirectToAction("BranchManagerView");

                               //return Json(new { success = true,result = "Accepted" });

                            }

                            return null;
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
                    return View();

                    break;
                    case "Reject":
                    using (sqlConnection)
                    {
                        try
                        {
                            string query = " Update tbl_MeetingRequest set ReqStatus= 'Rejected' where Request_Id= " + Request_Id;
                            SqlCommand cmd = new SqlCommand(query, sqlConnection);


                            sqlConnection.Open();
                            int i = cmd.ExecuteNonQuery();

                            sqlConnection.Close();

                            if (i >= 1)

                            {
                                return RedirectToAction("BranchManagerView");
                                //return Json(new { success = true, result = "Rejected" });

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
                    break;
                default:
                    // handle unexpected action value
                    break;
                   
            }
            return Json(new { success = false});


        }

    }
}


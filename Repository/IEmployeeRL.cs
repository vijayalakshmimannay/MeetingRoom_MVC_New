using MeetingRoom1.Models;
using System;
using System.Collections.Generic;

namespace MeetingRoom1.Repository
{
    public interface IEmployeeRL
    {
        public EmployeeModel AddEmployee(EmployeeModel employee);
        public EmployeeModel UserLogin(LoginModel loginModel);
        public IEnumerable<MeetingRoomModel> GetAllMeetingRooms(string BranchName);
        public RequestModel AddRequest(RequestModel employee, int UserId, int MeetingRoom_Id);
        public IEnumerable<EmployeeModel> GetAllEmployees();
        public IEnumerable<RequestModel> GetAllRequests();
        public IEnumerable<RequestModel> TodayMeetList(string MDate);
        public IEnumerable<RequestModel> GetAllMeetingRooms_Admiin();
    }
        
}

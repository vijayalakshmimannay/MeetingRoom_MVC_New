
using System;

namespace MeetingRoom1.Models
{
    public class RequestModel
    {
        public int UserId { get; set; }
        public int Id { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string MDate { get; set; }
        public string Purpose { get; set; }
        public string RequestFor { get; set; }
        public int NoOfEmps { get; set; }

        public string Status { get; set; }
        public int Request_Id { get; set; }
    }
   
}

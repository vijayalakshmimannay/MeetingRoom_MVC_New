namespace MeetingRoom1.Models
{
    public class MeetingRoomModel
    {
        public int MeetingRoom_Id { get; set; }
        public int FloorNo { get; set; }
        public int RoomNo { get; set; }
        public int Desktops { get; set; }
        public int Projectors { get; set; }
        public int Capacity { get; set; }

        public string BranchName { get; set; }

    }
}

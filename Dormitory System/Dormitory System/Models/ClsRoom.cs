namespace Dormitory_System.Models
{
    public class ClsRoom:ClsRoomType
    {
        public int roomid { get; set; }
        public int tenant { get; set; }
        public string roomname { get; set; }
        public int floor { get; set; }
        public int bed { get; set; }
        public int capacity { get; set; }
        public string note { get; set; }
      
    }
    public class Clsmainroom 
    {
        public List<ClsRoom> clsRooms { get; set; }
        public int Errcode { get; set; }
        public string Errmsg { get; set; }
    }

}

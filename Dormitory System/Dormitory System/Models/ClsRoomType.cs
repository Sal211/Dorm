namespace Dormitory_System.Models
{
    public class ClsRoomType
    {
        public int roomtypeid { get; set; }
        public string roomtype { get; set; }
        public string description { get; set; }
        public int createuid { get; set; }
        public DateTime createdate { get; set; }
        public string formatdate { get; set; }
    }
    public class Clsmainroomtype 
    { 
        public List<ClsRoomType> clsRoomTypes { get; set; }
        public int Errcode { get; set; }
        public string Errmsg { get; set; }
    }

}

namespace Dormitory_System.Models
{
    public class ClsBooking:ClsCustomer
    {
        public int tranid { get; set; }
        public int roomid { get; set; }
        public int roomtypeid { get; set; }
        public int studentid { get; set; }
        public string studentname { get; set; }
        public string roomname { get ; set; }
        public string roomtype { get; set; }
        public int ternant_type { get; set; }
        public bool bool_custype { get; set; }
        public int customertype { get; set; }
        public string custype { get; set; }
        public DateTime startdate { get; set; }
        public DateTime starttime { get; set; }
        public DateTime enddate { get; set; }
        public DateTime endtime { get; set; }
        public double tuitiondue { get; set; }
        public double tuitionpaid { get; set; }
        public int month { get; set; }
        public int createuid { get; set; }
        public string createdate { get; set; }
        public bool ispaid { get; set; }
        public float discount { get; set; }
        public string str_ispaid { get; set; }  
        public string format_startdate { get; set; }
        public string format_starttime { get; set; }
        public string format_enddate { get; set; }
        public string format_endtime { get; set; }
    }
    public class ClsmainBooking
    {
        public List<ClsBooking> clsBookings { get; set; }
        public int Errcode { get; set; }
        public string Errmsg { get; set; }
    }
}

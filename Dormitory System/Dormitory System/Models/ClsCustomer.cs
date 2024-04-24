using System.Numerics;

namespace Dormitory_System.Models
{
    public class ClsCustomer
    {
        public int contactid { get; set; }
        public string studentname { get; set; }
        public bool bool_custype { get; set; }
        public int customertype { get; set; }
        public string str_custype { get; set; }
        public int studentid { get; set; }  
        public string dob { get; set; }
        public DateTime datedob { get; set; }
        public string formatdob { get; set; }
        public string contactnumber { get; set; }
        public string address { get; set; }
        public string email { get; set; }
        public string guardianname { get; set; }
        public string guardianaddress { get; set; }
        public string guardiancontact { get; set; }
        public string guardianrelation { get; set; }
    }
    public class Clsmaincustomer
    {
        public List<ClsCustomer> clsCustomers { get; set; }
        public int Errcode { get; set; }
        public string Errmsg { get; set; }
    }
}

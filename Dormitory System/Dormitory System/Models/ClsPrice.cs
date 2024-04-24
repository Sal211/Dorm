namespace Dormitory_System.Models
{
    public class ClsPrice:ClsRoom
    {
        public int priceid { get; set; }
        public double fullprice_month { get; set; }
        public double fullprice_day { get; set; }
        public double fullper_month { get; set; }
        public double fullper_day { get; set; }
    }
    public class Clsmainprice
    {
        public List<ClsPrice> clsPrices { get; set; }
        public int Errcode { get; set; }
        public string Errmsg { get; set; }
    }

}

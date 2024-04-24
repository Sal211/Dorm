using Dormitory_System.Models;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Models;
using System.Data.SqlClient;
using System.Data;

namespace Dormitory_System.Controllers
{
    public class PriceController : Controller
    {
        public IActionResult ViewPrice()
        {
            return View();
        }
        public ReturnStatus PricePost(string query)
        {
            ClsConnection con = new ClsConnection();
            ReturnStatus err = new ReturnStatus();
            if (con._Errcode == 0)
            {
                try
                {
                    con._cmd = new SqlCommand(query, con._con);
                    if (con._cmd.ExecuteNonQuery() > 0)
                    {
                        err.Errcode = 1;
                    }
                    else
                    {
                        err.Errcode = 9999;
                    }
                }
                catch (Exception ex)
                {
                    err.Errcode = ex.HResult;
                    err.ErrMsg = ex.Message;
                }
            }
            else
            {
                err.Errcode = con._Errcode;
                err.ErrMsg = con._ErrMsg;
            }
            return err;
        }
        public IActionResult PostPrice(ClsPrice obj)
        {
            DateTime time = DateTime.Now;
            string query = "INSERT INTO Dorm_RoomPriceSetting VALUES('" + obj.roomid + "','" + obj.fullprice_month + "','" + obj.fullprice_day + "','" + obj.fullper_month + "','" + obj.fullper_day + "',0,'" + HttpContext.Session.GetString("id") + "','" + time.ToString("yyy-MM-dd hh:mm:ss") + "')";
            ReturnStatus err = PricePost(query);
            return Ok(err);
        }

        public IActionResult GetPrice()
        {
            ClsConnection con = new ClsConnection();
            Clsmainprice obj = new Clsmainprice();
            DataTable dt = new DataTable();
            ClsPrice price;
            List<ClsPrice> list = new List<ClsPrice>();
            if (con._Errcode == 0)
            {
                try
                {
                    string query = " EXEC Proc_GetPrice";
                    con._Ad = new SqlDataAdapter(query, con._con);
                    con._Ad.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        price = new ClsPrice();
                        price.priceid = int.Parse(row[0].ToString());
                        price.roomid = int.Parse(row[1].ToString());
                        price.fullprice_month = double.Parse(row[2].ToString());
                        price.fullprice_day = double.Parse(row[3].ToString());
                        price.fullper_month = double.Parse(row[4].ToString());
                        price.fullper_day = double.Parse(row[5].ToString());
                        price.createuid = int.Parse(row[7].ToString());
                        price.createdate = DateTime.Parse(row[8].ToString());
                        price.formatdate = price.createdate.ToString("dd-MMM-yyy hh:mm tt");
                        price.roomname = row[9].ToString();
                        price.roomtypeid = int.Parse(row[10].ToString());
                        price.roomtype = row[11].ToString();
                        list.Add(price);
                    }
                    obj.Errcode = 1;
                    obj.clsPrices = list;
                }
                catch (Exception ex)
                {
                    obj.Errmsg = ex.Message;
                }
            }
            else
            {
                obj.Errcode = con._Errcode;
                obj.Errmsg = con._ErrMsg;
            }
            return Ok(obj);
        }
        public IActionResult UpdatePrice(ClsPrice obj)
        {
            string query = "UPDATE Dorm_RoomPriceSetting SET Room_id = '" + obj.roomid + "',FullPrice_Month = '" + obj.fullprice_month + "',FullPrice_Day = '" + obj.fullprice_day + "',FullPer_Month = '" + obj.fullper_month + "',FullPer_Day = '" + obj.fullper_day + "'  WHERE price_id = '" + (int)obj.priceid + "'";
            ReturnStatus err = PricePost(query);
            return Ok(err);
        }
        public IActionResult DeletePrice(string id)
        {
            string query = "UPDATE Dorm_RoomPriceSetting SET Inactive = '" + 1 + "' WHERE price_id = '" + id + "'";
            ReturnStatus err = PricePost(query);
            return Ok(err);
        }
    
    }
}

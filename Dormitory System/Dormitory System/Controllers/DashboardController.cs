using Dormitory_System.Models;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Models;
using System.Data.SqlClient;
using System.Data;

namespace Dormitory_System.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult ViewDashboad()
        {
            return View();
        }
        public IActionResult GetBookingRecent()
        {
            ClsConnection con = new ClsConnection();
            DataTable dt = new DataTable();
            ClsmainBooking obj = new ClsmainBooking();
            ClsBooking customer;
            List<ClsBooking> list = new List<ClsBooking>();
            if (con._Errcode == 0)
            {
                try
                {
                    string query = "EXEC Get_Recentbooking";
                    con._Ad = new SqlDataAdapter(query, con._con);
                    con._Ad.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        customer = new ClsBooking();
                        customer.studentid = int.Parse(row[0].ToString());
                        customer.tuitiondue = double.Parse(row[2].ToString());
                        customer.ispaid = bool.Parse(row[3].ToString());
                        customer.roomname = row[1].ToString();
                        customer.str_ispaid = customer.ispaid ? "Paid" : "Unpaid";
                        list.Add(customer);
                    }
                    obj.Errcode = 1;
                    obj.clsBookings = list;
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
        public IActionResult GetDataDB_Tenant(int type,DateTime date)
        {
            ClsConnection con = new ClsConnection();
            ReturnStatus obj = new ReturnStatus();
            DataTable dt = new DataTable();
            ClsDashboard objDB;
            List<ClsDashboard> list = new List<ClsDashboard>();
            if (con._Errcode == 0)
            {
                try
                {
                    string query = " GetDBtenant @type ='"+type+"',@date = '"+date+"'";
                    con._Ad = new SqlDataAdapter(query, con._con);
                    con._Ad.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        objDB = new ClsDashboard();             
                        objDB.percent_tenant = float.Parse(row["percentage"].ToString());
                        objDB.roomType = row["Type_Name"].ToString();
                        list.Add(objDB);
                    }
                    dt = new DataTable();
                    query = "SELECT * FROM dbo.Return_CountTenant('"+type+"','"+date+"')";
                    con._Ad = new SqlDataAdapter(query, con._con);
                    con._Ad.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        objDB = new ClsDashboard();
                        objDB.tenant = int.Parse(row["tenant"].ToString());
                        list.Add(objDB);
                    }
                }
                catch (Exception ex)
                {
                    obj.ErrMsg = ex.Message;
                }
            }
            else
            {
                obj.Errcode = con._Errcode;
                obj.ErrMsg = con._ErrMsg;
            }

            return Json(list);
        }
    }
}

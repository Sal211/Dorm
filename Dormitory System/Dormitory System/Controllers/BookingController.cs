using Dormitory_System.Models;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Models;
using System.Data.SqlClient;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using System;

namespace Dormitory_System.Controllers
{
    public class BookingController : Controller
    {
        public IActionResult ViewBooking()
        {
            return View();
        }
        public ReturnStatus BookingPost(string query, Dictionary<string, object> DataBooking)
        {
            ClsConnection con = new ClsConnection();
            ReturnStatus err = new ReturnStatus();

            if (con._Errcode == 0)
            {
                SqlTransaction tran = con._con.BeginTransaction();

                try
                {
                    con._cmd = new SqlCommand(query, con._con);
                    con._cmd.Transaction = tran;

                    AddParameters(con._cmd, DataBooking);

                    if (con._cmd.ExecuteNonQuery() > 0)
                    {
                        err.Errcode = 1;
                    }
                    else
                    {
                        err.Errcode = 9999;
                    }

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    err.Errcode = ex.HResult;
                    err.ErrMsg = ex.Message;
                }
                finally
                {
                    tran.Dispose();
                }
            }
            else
            {
                err.Errcode = con._Errcode;
                err.ErrMsg = con._ErrMsg;
            }

            return err;
        }

        public void AddParameters(SqlCommand command, Dictionary<string, object> DataBooking)
        {
            foreach (var Booking in DataBooking)
            {
                command.Parameters.AddWithValue(Booking.Key, Booking.Value);
            }
        }

        public IActionResult PostBooking(ClsBooking obj)
        {
            DateTime time = DateTime.Now;

            string query = "INSERT INTO Dorm_BookingRoomTrans VALUES(@roomid,@stuid,@startdate,@starttime,@enddate,@endtime,@t_due,@t_paid,@month,@ispaid,@custype,@createuid,@createdate,@ternand_type,@discount)";

            Dictionary<string, object> DataBooking = new Dictionary<string, object>
            {
                { "@roomid", obj.roomid },
                { "@stuid", obj.studentid },
                { "@startdate", obj.startdate },
                { "@enddate", obj.enddate },
                { "@starttime", obj.starttime },
                { "@endtime", obj.endtime },
                { "@t_paid", 0 },
                { "@t_due", obj.tuitiondue },
                { "@month", obj.month },
                { "@ispaid", 0 },
                { "@custype", obj.custype },
                { "@createuid", HttpContext.Session.GetString("id") },
                { "@createdate", time },
                {"@ternand_type",obj.ternant_type },
                {"@discount", obj.discount }
            };

            ReturnStatus err = BookingPost(query, DataBooking);
            return Ok(err);
        }
        public ClsmainBooking  GetData_Booking(string query)
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
                    con._Ad = new SqlDataAdapter(query, con._con);
                    con._Ad.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        customer = new ClsBooking();
                        customer.tranid = int.Parse(row[0].ToString());
                        customer.roomid = int.Parse(row[1].ToString());
                        customer.studentid = int.Parse(row[2].ToString());
                        customer.startdate = DateTime.Parse(row[3].ToString());
                        customer.starttime = DateTime.Parse(row[4].ToString());
                        customer.enddate = DateTime.Parse(row[5].ToString());
                        customer.endtime = DateTime.Parse(row[6].ToString());
                        customer.tuitiondue = double.Parse(row[7].ToString());
                        customer.tuitionpaid = double.Parse(row[8].ToString());
                        customer.ispaid = bool.Parse(row[10].ToString());
                        bool custypes = bool.Parse(row[11].ToString());
                        customer.createuid = int.Parse(row[12].ToString());
                        DateTime createdates = DateTime.Parse(row[13].ToString());
                        bool ternant_type = bool.Parse(row[14].ToString());
                        customer.ternant_type = ternant_type ? 1 : 0;
                        customer.discount = float.Parse(row[15].ToString());                      
                        customer.studentname = row[16].ToString();
                        customer.roomname = row[17].ToString();
                        customer.roomtypeid = int.Parse(row[18].ToString());
                        customer.roomtype = row[19].ToString();

                        customer.createdate = createdates.ToString("dd-MMM-yyy");
                        customer.format_endtime = customer.endtime.ToString("hh:mm:ss tt");
                        customer.format_starttime = customer.starttime.ToString("hh:mm:ss tt");
                        customer.format_startdate = customer.enddate.ToString("dd-MMM-yyy");
                        customer.format_enddate = customer.startdate.ToString("dd-MMM-yyy");
                        customer.custype = custypes ? "SID" : "SIF";
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
            return obj;
        }
        
        public IActionResult GetBooking()
        {
            string query = "EXEC Proc_GetBooking";
            ClsmainBooking obj =  GetData_Booking(query);
            return Ok(obj);
        }
        public IActionResult UpdateBooking(ClsBooking obj)
        {
            string query = "EXEC Proc_updatebooking @roomid = @roomid,@startdate = @startdate,@enddate  = @enddate,@starttime  = @startime,@endtime  = @endtime,@tuitiondue  = @tuitiondue,@month  = @month,@discount  = @discount,@tranid  = @tranid";
            Dictionary<string, object> DataBooking = new Dictionary<string, object>
            {
                { "@roomid", obj.roomid },
                { "@startdate", obj.startdate },
                { "@enddate", obj.enddate },
                { "@startime", obj.starttime },
                { "@endtime", obj.endtime },
                { "@tuitiondue", obj.tuitiondue },
                { "@month", obj.month },
                { "@discount", obj.discount },
                { "@tenant_type", obj.ternant_type },
                { "@tranid", obj.tranid}
            };
            ReturnStatus err = BookingPost(query, DataBooking);
            return Ok(err);
        }
        public IActionResult DeleteBooking(string id)
        {
            string query = "Delete Dorm_BookingRoomTrans Where Tran_id = @tranid ";
            Dictionary<string, object> DataBooking = new Dictionary<string, object>
            {
                { "@tranid", id }          

            };
            ReturnStatus err = BookingPost(query, DataBooking);
            return Ok(err);
        }
    }
}

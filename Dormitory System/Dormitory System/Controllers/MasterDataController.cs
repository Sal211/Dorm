using Dormitory_System.Models;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Models;
using SchoolManagement.Models.Connection;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
namespace Dormitory_System.Controllers
{
    public class MasterDataController : Controller
    {
        public ReturnStatus MasterPost(string query)
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
        public IActionResult Getcboroomtype()
        {
            ClsConnection con = new ClsConnection();
            DataTable dt = new DataTable();
            Clsmainroomtype obj = new Clsmainroomtype();
            ClsRoomType roomtype;
            List<ClsRoomType> list = new List<ClsRoomType>();
            if (con._Errcode == 0)
            {
                try
                {
                    string query = "SELECT * FROM Dorm_RoomType WHERE Inactive = 0";
                    con._Ad = new SqlDataAdapter(query, con._con);
                    con._Ad.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        roomtype = new ClsRoomType();
                        roomtype.roomtypeid = int.Parse(row[0].ToString());
                        roomtype.roomtype = row[1].ToString();                     
                        list.Add(roomtype);
                    }
                    obj.Errcode = 1;
                    obj.clsRoomTypes = list;

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
   
        public IActionResult Getcboroom(ClsPrice objprice)       
         {
            ClsConnection con = new ClsConnection();
            DataTable dt = new DataTable();
            Clsmainroom obj = new Clsmainroom();
            ClsRoom room;
            string roomid = "";
            List<ClsRoom> list = new List<ClsRoom>();
            if (con._Errcode == 0)
            {
                try
                {
                    string query = "SELECT Room_id FROM Dorm_RoomPriceSetting WHERE Inactive = 0";
                    con._Ad = new SqlDataAdapter(query, con._con);
                    con._Ad.Fill(dt);
                    foreach(DataRow row in dt.Rows)
                    {
                        if(objprice.roomid != int.Parse(row[0].ToString()))
                        {
                            roomid += row[0].ToString() + ",";
                        }                    
                    }
                    roomid = roomid.TrimEnd(',');
                    dt = new DataTable();
                    query = "SELECT * FROM Dorm_Rooms WHERE Inactive = 0 and Room_Id NOT IN ("+roomid+") and RoomType_id = '"+ objprice.roomtypeid + "' ";
                    con._Ad = new SqlDataAdapter(query, con._con);
                    con._Ad.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        room = new ClsRoom();
                        room.roomid = int.Parse(row[0].ToString());
                        room.roomname = row[2].ToString();
                        list.Add(room);
                    }
                    obj.Errcode = 1;
                    obj.clsRooms = list;

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
        public IActionResult Getcboroom_booking(ClsBooking objbook)
        {
            ClsConnection con = new ClsConnection();
            DataTable dt = new DataTable();
            Clsmainroom obj = new Clsmainroom();
            ClsRoom room;
            List<ClsRoom> list = new List<ClsRoom>();
            if (con._Errcode == 0)
            {
                try
                {
                    string query = "EXEC Getdatafreecbo_booking @roomtypeid = '"+objbook.roomtypeid+"',@tenant_type = '"+objbook.ternant_type+"',@roomid = '"+objbook.roomid+"'";                   
                    dt = new DataTable();
                    con._Ad = new SqlDataAdapter(query, con._con);
                    con._Ad.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        room = new ClsRoom();
                        room.roomid = int.Parse(row[0].ToString());
                        room.roomname = row[2].ToString();
                        list.Add(room);
                    }
                    obj.Errcode = 1;
                    obj.clsRooms = list;

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
    
        public IActionResult FindCustomer(string stuid)
        {
            ClsConnection con = new ClsConnection();
            bool find = false;
            DataTable dt = new DataTable();
            ClsmainBooking obj = new ClsmainBooking();
            ClsBooking book;
            List<ClsBooking> list = new List<ClsBooking>();
            if (con._Errcode == 0)
            {
                try
                {
                    string query = "SELECT * FROM Students WHERE Student_ID = '"+ stuid + "'";
                    con._Ad = new SqlDataAdapter(query, con._con);
                    con._Ad.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        find = true;
                        book = new ClsBooking();
                        book.studentid = int.Parse(row[0].ToString());
                        book.bool_custype = bool.Parse(row[1].ToString());
                        book.customertype = book.bool_custype ? 1 : 0;
                        list.Add(book);
                    }
                 
                    obj.Errcode = (!find) ? 0 : 1;
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
        public IActionResult GetPrice(string roomid)
        {
            ClsConnection con = new ClsConnection();
            DataTable dt = new DataTable();
            Clsmainprice obj = new Clsmainprice();
            ClsPrice room;
            List<ClsPrice> list = new List<ClsPrice>();
            if (con._Errcode == 0)
            {
                try
                {
                    string query = "SELECT * FROM Dorm_RoomPriceSetting WHERE Inactive = 0 and  Room_id = '" + roomid + "' ";
                    con._Ad = new SqlDataAdapter(query, con._con);
                    con._Ad.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        room = new ClsPrice();
                        room.fullprice_month = double.Parse(row[2].ToString());
                        room.fullprice_day = double.Parse(row[3].ToString());
                        room.fullper_month = double.Parse(row[4].ToString());
                        room.fullper_day = double.Parse(row[5].ToString());
                        list.Add(room);
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
    }
}

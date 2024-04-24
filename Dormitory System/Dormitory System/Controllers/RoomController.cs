using Dormitory_System.Models;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Models;
using System.Data.SqlClient;
using System.Data;

namespace Dormitory_System.Controllers
{
    public class RoomController : Controller
    {
        public IActionResult ViewAvailableRoom()
        {
            return View();
        }
        public IActionResult ViewUsingRoom()
        {
            return View();
        }
        public IActionResult ViewRoom()
        {
            return View();
        }
        public  ReturnStatus RoomPost(string query,ClsRoom obj,bool type)
        {
            ClsConnection con = new ClsConnection();
            ReturnStatus err = new ReturnStatus();
            if (con._Errcode == 0)
            {
                try
                {
                    if(!type)
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
                    else
                    {
                        string find = "SELECT COUNT(*) FROM Dorm_Rooms WHERE Room_Name = '" + obj.roomname + "' and Inactive = 0 and Room_id <> '" + obj.roomid + "'";
                        con._cmd = new SqlCommand(find, con._con);
                        int count = (int)con._cmd.ExecuteScalar();
                        if (count < 1)
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
                        else
                        {
                            err.Errcode = 0;
                        }
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
        public IActionResult PostRoom(ClsRoom obj)
        {
            DateTime time = DateTime.Now;
            string query = "INSERT INTO Dorm_Rooms VALUES('" + obj.roomtypeid + "','" + obj.roomname + "','" + (int)obj.floor + "','" + (int)obj.capacity+ "','" + (int)obj.bed + "','" + obj.note + "',0,'" + HttpContext.Session.GetString("id") + "','" + time.ToString("yyy-MM-dd hh:mm:ss") + "',1)";
            ReturnStatus err = RoomPost(query,obj,true);
            return Ok(err);
        }
        public Clsmainroom GetDataroom(string query)
        {
            ClsConnection con = new ClsConnection();
            Clsmainroom obj = new Clsmainroom();
            DataTable dt = new DataTable();
            ClsRoom room;
            List<ClsRoom> list = new List<ClsRoom>();
            if (con._Errcode == 0)
            {
                try
                {                 
                    con._Ad = new SqlDataAdapter(query, con._con);
                    con._Ad.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        room = new ClsRoom();
                        room.roomid = int.Parse(row[0].ToString());
                        room.roomtypeid = int.Parse(row[1].ToString());
                        room.roomname = row["Room_Name"].ToString();
                        room.floor = int.Parse(row[3].ToString());
                        room.capacity = int.Parse(row[4].ToString());
                        room.bed = int.Parse(row[5].ToString());
                        room.note = row[6].ToString();
                        room.createuid = int.Parse(row[8].ToString());
                        room.createdate = DateTime.Parse(row[9].ToString());
                        room.formatdate = room.createdate.ToString("dd-MMM-yyy hh:mm tt");
                        room.roomtype = row[10].ToString();
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
            return obj;
        }
        public IActionResult GetRoom()
        {       
            string query = " EXEC Proc_GetRoom";
            Clsmainroom obj = GetDataroom(query);
            return Ok(obj);
        }
        public IActionResult DetailGetUsingRoom(int roomid)
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
                    string query = "EXEC Get_DetailStatusRoom @roomid = '"+roomid+"'";
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
            return Ok(obj);
        }
        public IActionResult GetUsingRoom(int type,int roomtypeid)
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
                    string query = "EXEC Get_StatusRoom @type = '"+ type + "',@roomtypeid = '"+ roomtypeid + "'";
                    con._Ad = new SqlDataAdapter(query, con._con);
                    con._Ad.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        room = new ClsRoom();
                        room.tenant = int.Parse(row[0].ToString());            
                        room.roomid = int.Parse(row[1].ToString());
                        room.capacity = int.Parse(row[2].ToString());
                        room.roomname = row[3].ToString();
                        room.bed = int.Parse(row[4].ToString());
                        room.floor = int.Parse(row[5].ToString());
                        room.note = row[6].ToString();
                        room.roomtypeid = int.Parse(row[7].ToString());
                        room.roomtype = row[8].ToString();
                        list.Add(room);
                    }
                    obj.Errcode = 0;
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
        public IActionResult GetAvailableRoom()
        {
            string query = " EXEC GetAvailableRoom";
            Clsmainroom obj = GetDataroom(query);
            return Ok(obj);
        }
        public IActionResult UpdateRoom(ClsRoom obj)
        {            
            string query = "UPDATE Dorm_Rooms SET RoomType_id = '"+obj.roomtypeid+ "',Room_Name = '"+obj.roomname+ "',Floor = '"+obj.floor+ "',Capacity = '"+obj.capacity+ "',Bed = '"+obj.bed+ "',Note = '"+obj.note+"' WHERE room_id = '" + (int)obj.roomid + "'";
            ReturnStatus err = RoomPost(query,obj,true); 
            return Ok(err);
        }
        public IActionResult DeleteRoom(int id)
        {
            ClsRoom obj = new ClsRoom();
            obj.roomid = id;
            string query = "UPDATE Dorm_Rooms SET Inactive = '" + 1 + "' WHERE room_id = '" + id + "'";
            ReturnStatus err = RoomPost(query,obj,false);
            return Ok(err);
        }
       
    }
}

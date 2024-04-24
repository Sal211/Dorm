using Dormitory_System.Models;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Models;
using System.Data.SqlClient;

using SchoolManagement.Models.Connection;
using System.Data;

namespace Dormitory_System.Controllers
{
    public class RoomTypeController : Controller
    {
        public IActionResult ViewRoomType()
        {
            return View();
        }
        public ReturnStatus RoomtypePost(string query,ClsRoomType obj,bool type)
        {
            ClsConnection con = new ClsConnection();
            ReturnStatus err = new ReturnStatus();
            if (con._Errcode == 0)
            {
                try
                {          
                    if (!type)
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
                        string find = "SELECT COUNT(*) FROM Dorm_RoomType WHERE Type_Name = '" + obj.roomtype + "' and Inactive = 0 and RoomType_id <> '"+obj.roomtypeid+"'";
                        con._cmd = new SqlCommand(find, con._con);
                        int count = (int)con._cmd.ExecuteScalar();
                        if(count < 1)
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

        public IActionResult PostRoomType(ClsRoomType obj)
        {
            DateTime time = DateTime.Now;            
            string query = "INSERT INTO Dorm_RoomType VALUES('" + obj.roomtype + "','" + obj.description + "',0,'" + HttpContext.Session.GetString("id") + "','" + time.ToString("yyy-MM-dd hh:mm:ss") + "')";
            ReturnStatus err = RoomtypePost(query,obj,true);              
            return Ok(err);  
        }

        public IActionResult GetRoomType()
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
                    con._Ad = new SqlDataAdapter(query,con._con);
                    con._Ad.Fill(dt);
                    foreach(DataRow row in dt.Rows)
                    {
                        roomtype = new ClsRoomType();
                        roomtype.roomtypeid = int.Parse(row[0].ToString());
                        roomtype.roomtype = row[1].ToString();
                        roomtype.description = row[2].ToString();
                        roomtype.createuid = int.Parse(row[4].ToString());
                        roomtype.createdate = DateTime.Parse(row[5].ToString());
                        roomtype.formatdate = roomtype.createdate.ToString("dd-MMM-yyy hh:mm tt");
                        list.Add(roomtype);
                    }
                    obj.Errcode = 0;
                    obj.clsRoomTypes = list;

                }catch(Exception ex)
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
        public IActionResult UpdateRoomType(ClsRoomType obj)
        {
            string query = "UPDATE Dorm_RoomType SET type_name = '" + obj.roomtype + "',description = '" + obj.description + "' WHERE roomtype_id = '"+ obj.roomtypeid+ "'";
            ReturnStatus err = RoomtypePost(query, obj, true);
            return Ok(err);
        }
        public IActionResult DeleteRoomType(int id)
        {
            ClsRoomType obj = new ClsRoomType();
            obj.roomtypeid = id;
            string query = "UPDATE Dorm_RoomType SET Inactive = '" + 1 + "' WHERE roomtype_id = '"+id+"'" ;
            ReturnStatus err = RoomtypePost(query, obj, false);
            return Ok(err);
        }
    }
}

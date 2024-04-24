 using Dormitory_System.Models;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Models;
using System.Data;
using System.Data.SqlClient;

namespace Dormitory_System.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Viewlogin()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetLogin(string userid,string password)
        {
            ClsConnection con = new ClsConnection();
            ReturnStatus err = new ReturnStatus();
            bool have = false;
             if(con._Errcode == 0)
            {
                try
                {
                    string query = "SELECT COUNT(*) FROM Login WHERE userid = '" + int.Parse(userid) + "' AND password = '" + password + "' ";
                    con._cmd = new SqlCommand(query, con._con);
                    int count = (int)con._cmd.ExecuteScalar();
                    if (count > 0)
                    {
                        have = true;
                        HttpContext.Session.SetString("id", userid.ToString());
                    }
                    if (have) err.Errcode = 1;
                }
                catch(Exception ex)
                {
                    err.Errcode = ex.HResult;
                    err.ErrMsg = ex.Message;
                 }
            }
            else
            {
                err.Errcode = con._Errcode;
                err.ErrMsg  = con._ErrMsg;
            }
            return Ok(err);
        }
    }
}

using Dormitory_System.Models;
using Microsoft.AspNetCore.Mvc;
using SchoolManagement.Models;
using System.Data.SqlClient;
using System.Data;

namespace Dormitory_System.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult ViewCustomer()
        {
            return View();
        }
        public ReturnStatus CustomerPost(string query)
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

        public IActionResult PostCustomer(ClsCustomer obj)
        {
            DateTime time = DateTime.Now;

            string query = "EXEC Proc_PostCustomer  @custtype = '"+obj.customertype+"',@sid = '"+obj.studentid+"' ,@studentname = '"+obj.studentname+"',@dob = '"+obj.dob+"',@contactnumber  = '"+obj.contactnumber+"',@email = '"+obj.email+"',@address = '"+obj.address+"',@guardianname  = '"+obj.guardianname+"' , @guar_address = '"+obj.guardianaddress+"',@guar_contact = '"+obj.guardiancontact+"',@guar_relation = '"+obj.guardianrelation+"'";
             ReturnStatus err = CustomerPost(query);
            return Ok(err);
        }

        public IActionResult GetCustomer()
        {
            ClsConnection con = new ClsConnection();
            DataTable dt = new DataTable();
            Clsmaincustomer obj = new Clsmaincustomer();
            ClsCustomer customer;
            List<ClsCustomer> list = new List<ClsCustomer>();
            if (con._Errcode == 0)
            {
                try
                {
                    string query = "EXEC Proc_GetCustomer";
                    con._Ad = new SqlDataAdapter(query, con._con);
                    con._Ad.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        customer = new ClsCustomer();
                        customer.contactid = int.Parse(row[0].ToString());
                        customer.studentid = int.Parse(row[1].ToString());
                        customer.guardianname = row[2].ToString();
                        customer.guardianaddress = row[3].ToString();
                        customer.guardiancontact = row[4].ToString();
                        customer.guardianrelation = row[5].ToString();
                        customer.bool_custype = bool.Parse(row[7].ToString());
                        customer.customertype = customer.bool_custype ? 1 : 0;

                        customer.studentname = row[8].ToString();
                        customer.contactnumber = row[9].ToString();
                        customer.email = row[10].ToString();
                        customer.address = row[11].ToString();
                        customer.datedob = DateTime.Parse(row[12].ToString());
                        customer.dob = customer.datedob.ToString("dd-MMM-yyy");
                        customer.formatdob = customer.datedob.ToString("yyy-MM-dd");
                        customer.str_custype = customer.customertype == 1 ? "SID" : "SIF";  
                        list.Add(customer);
                    }
                    obj.Errcode = 1;
                    obj.clsCustomers = list;
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
        public IActionResult UpdateCustomer(ClsCustomer obj)
        {
            string query = "EXEC Proc_UpdateCustomer  @custtype = '" + obj.customertype + "',@sid = '" + obj.studentid + "' ,@studentname = '" + obj.studentname + "',@dob = '" + obj.dob + "',@contactnumber  = '" + obj.contactnumber + "',@email = '" + obj.email + "',@address = '" + obj.address + "',@guardianname  = '" + obj.guardianname + "' , @guar_address = '" + obj.guardianaddress + "',@guar_contact = '" + obj.guardiancontact + "',@guar_relation = '" + obj.guardianrelation + "'";
            ReturnStatus err = CustomerPost(query);
            return Ok(err);
        }
        public IActionResult DeleteCustomer(string id)
        {
            string query = "EXEC Proc_delCustomer @id = '"+id+"' ";
            ReturnStatus err = CustomerPost(query);
            return Ok(err);
        }
    }

}

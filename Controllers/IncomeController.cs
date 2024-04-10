using PropertyTracker.Models.Account;
using PropertyTracker.Models.DBConnection;
using PropertyTracker.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PropertyTracker.Controllers
{
    public class IncomeController : Controller
    {
        // GET: Rent
        public ActionResult RentalList()
        {
            List<RentalPaymentInfoVM> list = new List<RentalPaymentInfoVM>();

            try
            {
                using (SqlConnection con = new SqlConnection(AdoNetDBContext.GetConnectionString()))
                {
                    con.Open();
                    string query = @"SELECT 
                                RPI.Id, 
                                RPI.PropertyId, 
                                RPI.TenantId, 
                                RPI.RentStatus, 
                                RPI.RentMonth, 
                                RPI.RentPaidAmount, 
                                RPI.BalanceAmount, 
                                RPI.RentPaidOn, 
                                T.Name AS TenantName, 
                                PM.PropertyName 
                            FROM RentalPaymentInfo RPI
                            INNER JOIN Tenants T ON RPI.TenantId = T.Id
                            INNER JOIN PropertyMaster PM ON RPI.PropertyId = PM.Id";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            RentalPaymentInfoVM rentalInfo = new RentalPaymentInfoVM();
                            rentalInfo.Id = Convert.ToInt32(reader["Id"]);
                            rentalInfo.PropertyId = Convert.ToInt32(reader["PropertyId"]);
                            rentalInfo.TenantId = Convert.ToInt32(reader["TenantId"]);
                            rentalInfo.RentStatus = reader["RentStatus"].ToString();
                            rentalInfo.RentMonth = reader["RentMonth"].ToString();
                            rentalInfo.RentPaidAmount = reader["RentPaidAmount"].ToString();
                            rentalInfo.BalanceAmount = reader["BalanceAmount"].ToString();
                            rentalInfo.RentPaidOn = reader["RentPaidOn"].ToString();
                            rentalInfo.TenantName = reader["TenantName"].ToString();
                            rentalInfo.PropertyName = reader["PropertyName"].ToString();
                            list.Add(rentalInfo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle the exception, log it, or display an error message
            }

            return View(list);
        }


        public ActionResult PayRent()
        {
            List<PropertyMaster> propertyList = new List<PropertyMaster>();
            List<Tenants> tenantList = new List<Tenants>();
            try
            {
                // Fetch property list from the database
               
                using (SqlConnection con = new SqlConnection(AdoNetDBContext.GetConnectionString()))
                {
                    con.Open();
                    string query = "SELECT Id, PropertyName FROM PropertyMaster";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            PropertyMaster property = new PropertyMaster();
                            property.Id = Convert.ToInt32(reader["Id"]);
                            property.PropertyName = reader["PropertyName"].ToString();
                            propertyList.Add(property);
                        }
                        reader.Close();
                    }
                }

                // Fetch tenant list from the database
                
                using (SqlConnection con = new SqlConnection(AdoNetDBContext.GetConnectionString()))
                {
                    con.Open();
                    string query = "SELECT Id, Name FROM Tenants";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Tenants tenant = new Tenants();
                            tenant.Id = Convert.ToInt32(reader["Id"]);
                            tenant.Name = reader["Name"].ToString();
                            tenantList.Add(tenant);
                        }
                        reader.Close();
                    }
                }

                

            }
            catch (Exception)
            {

            }
            ViewBag.PropertyList = new SelectList(propertyList, "Id", "PropertyName");
            ViewBag.TenantList = new SelectList(tenantList, "Id", "Name");
            return View();
        }

        public JsonResult GetTenantsByPropertyId(int propertyId)
        {
            List<Tenants> tenantList = new List<Tenants>();

            try
            {
                using (SqlConnection con = new SqlConnection(AdoNetDBContext.GetConnectionString()))
                {
                    con.Open();
                    string query = "SELECT Id, Name FROM Tenants WHERE PropertyId = @PropertyId";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@PropertyId", propertyId);
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Tenants tenant = new Tenants();
                            tenant.Id = Convert.ToInt32(reader["Id"]);
                            tenant.Name = reader["Name"].ToString();
                            tenantList.Add(tenant);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return Json(tenantList, JsonRequestBehavior.AllowGet);
        }


        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult PayRent(RentalPaymentInfo data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (SqlConnection con = new SqlConnection(AdoNetDBContext.GetConnectionString()))
                    {
                        con.Open();
                        string query = @"INSERT INTO RentalPaymentInfo (PropertyId, TenantId, RentStatus, RentMonth, RentPaidAmount, BalanceAmount, RentPaidOn) 
                                 VALUES (@PropertyId, @TenantId, @RentStatus, @RentMonth, @RentPaidAmount, @BalanceAmount, @RentPaidOn)";
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@PropertyId", data.PropertyId);
                            cmd.Parameters.AddWithValue("@TenantId", data.TenantId);
                            cmd.Parameters.AddWithValue("@RentStatus", data.RentStatus);
                            cmd.Parameters.AddWithValue("@RentMonth", data.RentMonth);
                            cmd.Parameters.AddWithValue("@RentPaidAmount", data.RentPaidAmount);
                            cmd.Parameters.AddWithValue("@BalanceAmount", data.BalanceAmount);
                            cmd.Parameters.AddWithValue("@RentPaidOn", DateTime.Now); 

                            cmd.ExecuteNonQuery();
                        }
                    }

                    return RedirectToAction("RentalList"); 
                }
            }
            catch (Exception ex)
            {
            }

            return View(data);
        }

    }
}
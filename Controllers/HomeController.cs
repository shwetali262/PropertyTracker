using PropertyTracker.Models.Account;
using PropertyTracker.Models.DBConnection;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace PropertyTracker.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult PropertyList()
        {
            List<PropertyMaster> list = new List<PropertyMaster>();

            try
            {
                using (SqlConnection con = new SqlConnection(AdoNetDBContext.GetConnectionString()))
                {
                    con.Open();
                    string query = "SELECT Id, PropertyName, PropertyLocation, IsOnRent, RentalAmount, InDate FROM PropertyMaster";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            PropertyMaster property = new PropertyMaster();
                            property.Id = Convert.ToInt32(reader["Id"]);
                            property.PropertyName = reader["PropertyName"].ToString();
                            property.PropertyLocation = reader["PropertyLocation"].ToString();
                            property.IsOnRent = Convert.ToBoolean(reader["IsOnRent"]);
                            property.RentalAmount = reader["RentalAmount"].ToString();
                            property.InDate = Convert.ToDateTime(reader["InDate"]);
                            list.Add(property);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return View(list);
        }

        public ActionResult AddNewProperty()
        {
            return View("AddNewProperty");
        }

        [HttpPost]
        public ActionResult AddNewProperty(PropertyMaster data)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection con = new SqlConnection(AdoNetDBContext.GetConnectionString()))
                {
                    con.Open();
                    string query = @"INSERT INTO PropertyMaster (PropertyName, PropertyLocation, IsOnRent, RentalAmount, InDate) 
                             VALUES (@PropertyName, @PropertyLocation, @IsOnRent, @RentalAmount, GETDATE())";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@PropertyName", data.PropertyName);
                        cmd.Parameters.AddWithValue("@PropertyLocation", data.PropertyLocation);
                        cmd.Parameters.AddWithValue("@IsOnRent", data.IsOnRent);
                        cmd.Parameters.AddWithValue("@RentalAmount", data.RentalAmount);

                        cmd.ExecuteNonQuery();
                    }
                }
                ViewBag.Message = "Property Added Successfully";
                return RedirectToAction("PropertyList");
            }
            return View(data);
        }

        public ActionResult EditProperty(int id)
        {
            try
            {
                PropertyMaster property = new PropertyMaster();

                using (SqlConnection con = new SqlConnection(AdoNetDBContext.GetConnectionString()))
                {
                    con.Open();
                    string query = "SELECT Id, PropertyName, PropertyLocation, IsOnRent, RentalAmount, InDate FROM PropertyMaster WHERE Id = @Id";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.Read())
                        {
                            property.Id = Convert.ToInt32(reader["Id"]);
                            property.PropertyName = reader["PropertyName"].ToString();
                            property.PropertyLocation = reader["PropertyLocation"].ToString();
                            property.IsOnRent = Convert.ToBoolean(reader["IsOnRent"]);
                            property.RentalAmount = reader["RentalAmount"].ToString();
                            property.InDate = Convert.ToDateTime(reader["InDate"]);
                        }
                        else
                        {
                            return HttpNotFound();
                        }
                    }
                }

                return View(property);
            }
            catch (Exception ex)
            {
                return HttpNotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProperty(PropertyMaster property)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (SqlConnection con = new SqlConnection(AdoNetDBContext.GetConnectionString()))
                    {
                        con.Open();
                        string query = @"UPDATE PropertyMaster 
                                 SET PropertyName = @PropertyName, 
                                     PropertyLocation = @PropertyLocation, 
                                     IsOnRent = @IsOnRent, 
                                     RentalAmount = @RentalAmount
                                 WHERE Id = @Id";
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@Id", property.Id);
                            cmd.Parameters.AddWithValue("@PropertyName", property.PropertyName);
                            cmd.Parameters.AddWithValue("@PropertyLocation", property.PropertyLocation);
                            cmd.Parameters.AddWithValue("@IsOnRent", property.IsOnRent);
                            cmd.Parameters.AddWithValue("@RentalAmount", property.RentalAmount);

                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                return RedirectToAction("PropertyList");
                            }
                            else
                            {
                                return HttpNotFound();
                            }
                        }
                    }
                }

                return View(property);
            }
            catch (Exception ex)
            {
                return HttpNotFound();
            }

        }
    }
}
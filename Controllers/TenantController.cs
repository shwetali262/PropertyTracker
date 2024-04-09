using PropertyTracker.Models.Account;
using PropertyTracker.Models.DBConnection;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PropertyTracker.Controllers
{
    public class TenantController : Controller
    {
        // GET: Tenant
        public ActionResult TenantList()
        {
            List<Tenants> list = new List<Tenants>();

            try
            {
                using (SqlConnection con = new SqlConnection(AdoNetDBContext.GetConnectionString()))
                {
                    con.Open();
                    string query = "SELECT Id, Name, Email, PhoneNo, Address, PropertyId, InDate FROM Tenants";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            Tenants tenant = new Tenants();
                            tenant.Id = Convert.ToInt32(reader["Id"]);
                            tenant.Name = reader["Name"].ToString();
                            tenant.Email = reader["Email"].ToString();
                            tenant.PhoneNo = reader["PhoneNo"].ToString();
                            tenant.Address = reader["Address"].ToString();
                            tenant.PropertyId = Convert.ToInt32(reader["PropertyId"]);
                            tenant.InDate = Convert.ToDateTime(reader["InDate"]);
                            list.Add(tenant);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return View(list);
        }

        public ActionResult AddTenant()
        {
            List<PropertyMaster> propertyList = new List<PropertyMaster>();

            try
            {
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
                    }
                }
            }
            catch (Exception ex)
            {
            }

            ViewBag.PropertyList = new SelectList(propertyList, "Id", "PropertyName");
            return View();
        }

        [HttpPost]
        public ActionResult AddTenant(Tenants data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (SqlConnection con = new SqlConnection(AdoNetDBContext.GetConnectionString()))
                    {
                        con.Open();
                        string query = @"INSERT INTO Tenants (Name, Email, PhoneNo, Address, PropertyId, InDate) 
                                 VALUES (@Name, @Email, @PhoneNo, @Address, @PropertyId, @InDate)";
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@Name", data.Name);
                            cmd.Parameters.AddWithValue("@Email", data.Email);
                            cmd.Parameters.AddWithValue("@PhoneNo", data.PhoneNo);
                            cmd.Parameters.AddWithValue("@Address", data.Address);
                            cmd.Parameters.AddWithValue("@PropertyId", data.PropertyId);
                            cmd.Parameters.AddWithValue("@InDate", DateTime.Now);
                            cmd.ExecuteNonQuery();
                        }
                    }
                    return RedirectToAction("TenantList");
                }
            }
            catch (Exception ex)
            {
            }
            return View(data);
        }


    }
}



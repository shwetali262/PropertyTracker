using PropertyTracker.Models.Account;
using PropertyTracker.Models.DBConnection;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PropertyTracker.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous] 
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginPostData data, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection con = new SqlConnection(AdoNetDBContext.GetConnectionString()))
                {
                    con.Open();
                    string query = "SELECT Id, Email FROM UserMaster WHERE Email = @Email AND Password = @Password";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@Email", data.UserName); 
                        cmd.Parameters.AddWithValue("@Password", data.Password);

                        SqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            int userId = (int)reader["Id"];
                            string email = reader["Email"].ToString();

                            FormsAuthentication.SetAuthCookie(email, false);

                            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                                return Redirect(returnUrl);
                            else
                                return RedirectToAction("PropertyList", "Home");
                        }
                        else
                        {
                            ViewBag.Message = "Invalid username or password.";
                        }
                    }
                }
            }

            return View();
        }

        public ActionResult RegisterUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterUser(RegisterPostData data)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (SqlConnection con = new SqlConnection(AdoNetDBContext.GetConnectionString()))
                    {
                        con.Open();
                        string query = "SELECT Id FROM UserMaster WHERE Email = @Email";
                        using (SqlCommand cmd = new SqlCommand("query", con))
                        {
                            cmd.CommandType = System.Data.CommandType.Text;
                            cmd.CommandText = query;
                            cmd.Parameters.AddWithValue("@Email", data.Email);
                            var userId = cmd.ExecuteScalar();
                            if (userId != null)
                            {
                                ViewBag.Message = "Email already exists.";
                            }
                            else
                            {
                                query = "INSERT INTO UserMaster (UserName, Email, Password) VALUES (@UserName, @Email, @Password)";
                                cmd.CommandText = query;
                                cmd.Parameters.AddWithValue("@Password", data.Password);
                                cmd.Parameters.AddWithValue("@UserName", data.UserName);
                                cmd.ExecuteNonQuery();
                                ViewBag.Message = "User Registered Successfully.";
                                return RedirectToAction("Login");
                            }
                        }
                    }
                    return RedirectToAction("Login");
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                return View();
            }

        }


    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace PropertyTracker.Models.DBConnection
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserMaster> UserMasters { get; set; }
    }

    public class AdoNetDBContext
    {
        public static string GetConnectionString()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["AppDbContext"].ConnectionString;
        }
    }
}
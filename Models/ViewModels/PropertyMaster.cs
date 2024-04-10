using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PropertyTracker.Models.Account
{
    public class PropertyMaster
    {
        public int Id { get; set; }
        public string PropertyName { get; set; }
        public string PropertyLocation { get; set; }
        public bool IsOnRent { get; set; }
        public string RentalAmount { get; set; }
        public DateTime InDate { get; set; }
    }
}
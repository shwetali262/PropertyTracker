﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PropertyTracker.Models.Account
{
    public class Tenants
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public string Address { get; set; }
        public int PropertyId { get; set; }
        public DateTime InDate { get; set; } 
    }
}
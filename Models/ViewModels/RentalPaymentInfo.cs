using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PropertyTracker.Models.ViewModels
{
    public class RentalPaymentInfo
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public int TenantId { get; set; }
        public string RentStatus { get; set; }
        public string RentMonth { get; set; }
        public string RentPaidAmount { get; set; }
        public string BalanceAmount { get; set; }
        public string RentPaidOn { get; set; }
         
    }

    public class RentalPaymentInfoVM : RentalPaymentInfo
    {
        public string TenantName { get; set; }
        public string PropertyName { get; set; }
    }
}
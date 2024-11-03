using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KoiFishCare.service.VnpayService.Models
{
    public class PaymentInformationModel
    {
        public int BookingID { get; set; }
        public decimal Amount { get; set; }
        public string? ServiceName { get; set; }
    }
}
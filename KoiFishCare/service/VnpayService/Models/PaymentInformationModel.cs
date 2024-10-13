using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoiFishCare.service.VnpayService.Models
{
    public class PaymentInformationModel
    {
        public double Amount { get; set; }
        public string? OrderDescription { get; set; }
        public string? Name { get; set; }
    }
}
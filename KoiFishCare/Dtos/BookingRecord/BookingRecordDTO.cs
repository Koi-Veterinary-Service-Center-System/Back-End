using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoiFishCare.Dtos.BookingRecord
{
    public class BookingRecordDTO
    {
        public int BookingRecordID { get; set; }

        public int BookingID { get; set; }

        public decimal ArisedMoney { get; set; }

        public decimal TotalAmount { get; set; }

        public string? Note { get; set; }

    }
}
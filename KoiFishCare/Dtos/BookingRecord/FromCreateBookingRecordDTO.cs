using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoiFishCare.Dtos.BookingRecord
{
    public class FromCreateBookingRecordDTO
    {
        public int BookingID { get; set; }

        public int ArisedQuantity { get; set; }

        public string? Note { get; set; }

    }
}
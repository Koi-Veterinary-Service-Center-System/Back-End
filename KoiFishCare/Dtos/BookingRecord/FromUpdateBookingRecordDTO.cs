using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoiFishCare.Dtos.BookingRecord
{
    public class FromUpdateBookingRecordDTO
    {
        public int ArisedQuantity { get; set; }

        public string? Note { get; set; }
    }
}
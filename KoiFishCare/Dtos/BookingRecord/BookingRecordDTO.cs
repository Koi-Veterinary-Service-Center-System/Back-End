using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoiFishCare.Dtos.BookingRecord
{
    public class BookingRecordDTO
    {
        public DateTime CreateAt { get; set; }

        public int BookingRecordID { get; set; }

        public int BookingID { get; set; }

        public decimal InitAmount { get; set; }

        public decimal UnitPrice { get; set; }

        public int InitQuantity { get; set; }

        public int ArisedQuantity { get; set; }

        public int TotalQuantity { get; set; }

        public decimal QuantityMoney { get; set; }

        public decimal ReceivableAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public string? Note { get; set; }

        public decimal? RefundMoney { get; set; }

        public decimal? RefundPercent { get; set; }
    }
}
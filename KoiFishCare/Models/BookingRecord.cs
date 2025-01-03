using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace KoiFishCare.Models
{
    [Table("BookingRecords")]
    public class BookingRecord
    {
        [Key]
        public int BookingRecordID { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }

        public int ArisedQuantity { get; set; } 

        [Column(TypeName = "decimal(18, 2)")]
        public decimal QuantityMoney { get; set; }

        public string? Note { get; set; }

        public DateTime CreateAt { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? RefundMoney { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal? RefundPercent { get; set; }

        [ForeignKey("BookingID")]
        public int BookingID { get; set; }
        public Booking Booking { get; set; } = null!;
    }
}
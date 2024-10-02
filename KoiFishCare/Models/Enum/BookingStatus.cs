using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KoiFishCare.Models.Enum
{
    public enum BookingStatus
    {
        Pending,
        Confirmed,
        Scheduled,
        Ongoing,
        Completed,
        Received_Money,
        Succeeded,
        Refunded,
        Cancelled,
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace KoiFishCare.Models.Enum
{
    public enum BookingStatus
    {
        [EnumMember(Value = "Pending")]
        Pending,
        [EnumMember(Value = "Scheduled")]
        Scheduled,
        [EnumMember(Value = "Ongoing")]
        Ongoing,
        [EnumMember(Value = "Completed")]
        Completed,
        [EnumMember(Value = "Received Money")]
        Received_Money,
        [EnumMember(Value = "Succeeded")]
        Succeeded,
        [EnumMember(Value = "Refunded")]
        Refunded,
        [EnumMember(Value = "Cancelled")]
        Cancelled,
    }
}
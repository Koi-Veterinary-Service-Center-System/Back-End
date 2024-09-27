using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Dtos.Service;
using KoiFishCare.Models;

namespace KoiFishCare.Mappers
{
    public static class ServiceMappers
    {
        public static ServiceDto ToServiceDto(this Service s)
        {
            return new ServiceDto
            {
                ServiceID = s.ServiceID,
                ServiceName = s.ServiceName,
                Description = s.Description,
                Price = s.Price,
                EstimatedDuration = s.EstimatedDuration
            };
        }

        public static Service ToServiceFromAddUpdateDto(this AddUpdateServiceDto s)
        {
            return new Service
            {
                ServiceName = s.ServiceName,
                Description = s.Description,
                Price = s.Price,
                EstimatedDuration = s.EstimatedDuration
            };
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.DTOs.Service;
using KoiFishCare.Models;

namespace KoiFishCare.Mappers
{
    public static class ServiceMappers
    {
        public static ServiceDTO ToServiceDto(this Service s)
        {
            return new ServiceDTO
            {
                ServiceID = s.ServiceID,
                ServiceName = s.ServiceName,
                Description = s.Description,
                Price = s.Price,
                QuantityPrice = s.QuantityPrice,
                EstimatedDuration = s.EstimatedDuration,
                ImageURL = s.ImageURL,
                IsAtHome = s.IsAtHome,
                IsDeleted = s.IsDeleted,
                IsOnline = s.IsOnline,
            };
        }

        public static Service ToServiceFromAddUpdateDto(this AddUpdateServiceDTO s)
        {
            return new Service
            {
                ServiceName = s.ServiceName,
                Description = s.Description,
                Price = s.Price,
                QuantityPrice = s.QuantityPrice,
                EstimatedDuration = s.EstimatedDuration,
                ImageURL = s.ImageURL,
                IsAtHome = s.IsAtHome,
                IsOnline = s.IsOnline
            };
        }
    }
}
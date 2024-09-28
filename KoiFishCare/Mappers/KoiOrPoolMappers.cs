using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Dtos.KoiOrPool;
using KoiFishCare.DTOs.KoiOrPool;
using KoiFishCare.Models;

namespace KoiFishCare.Mappers
{
    public static class KoiOrPoolMappers
    {
        public static KoiOrPool ToModelFromCreateUpdate(this CreateUpdateKoiOrPoolDto dto)
        {
            return new KoiOrPool
            {
                Name = dto.Name,
                IsPool = dto.IsPool,
                Description = dto.Description
            };
        }

        public static KoiOrPoolDTO ToDtoFromModel(this KoiOrPool model)
        {
            return new KoiOrPoolDTO
            {
                KoiOrPoolID = model.KoiOrPoolID,
                Name = model.Name,
                IsPool = model.IsPool,
                Description = model.Description,
                CustomerId = model.CustomerID
            };
        }
    }
}
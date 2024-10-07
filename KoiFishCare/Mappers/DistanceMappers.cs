using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Dtos.Distance;
using KoiFishCare.Models;

namespace KoiFishCare.Mappers
{
    public static class DistanceMappers
    {
        public static DistanceDTO ToDTOFromModel(this Distance distanceModel)
        {
            return new DistanceDTO
            {
                DistanceID = distanceModel.DistanceID,
                District = distanceModel.District,
                Area = distanceModel.Area,
                Price = distanceModel.Price
            };
        }

        public static Distance ToModelFromDTO(this AddDistanceDTO addDistanceDTO)
        {
            return new Distance
            {
                District = addDistanceDTO.District,
                Area = addDistanceDTO.Area,
                Price = addDistanceDTO.Price
            };
        }

    }
}
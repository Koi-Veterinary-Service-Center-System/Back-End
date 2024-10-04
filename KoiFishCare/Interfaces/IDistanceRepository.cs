using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Dtos.Distance;
using KoiFishCare.Models;

namespace KoiFishCare.Interfaces
{
    public interface IDistanceRepository
    {
        Task<List<Distance>?> GetAllDistanceAsync();
        Task<Distance?> GetDistanceById(int distanceID);
        Task<Distance> CreateDistance(Distance distance);
        Task<Distance?> UpdateDistanceById(int distanceID, AddDistanceDTO distanceDTO);
        Task<Distance?> DeleteDistance (int distanceID);
    }
}
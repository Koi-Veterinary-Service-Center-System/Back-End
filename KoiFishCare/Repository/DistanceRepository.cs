using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KoiFishCare.Data;
using KoiFishCare.Dtos.Distance;
using KoiFishCare.Interfaces;
using KoiFishCare.Models;
using Microsoft.EntityFrameworkCore;

namespace KoiFishCare.Repository
{
    public class DistanceRepository : IDistanceRepository
    {
        private readonly KoiFishVeterinaryServiceContext _context;

        public DistanceRepository(KoiFishVeterinaryServiceContext context)
        {
            _context = context;
        }

        public async Task<Distance> CreateDistance(Distance distance)
        {
            await _context.Distances.AddAsync(distance);
            await _context.SaveChangesAsync();
            return distance;
        }

        public async Task<Distance?> DeleteDistance(int distanceID)
        {
            var distance = await _context.Distances.FirstOrDefaultAsync(x => x.DistanceID == distanceID);
            if (distance == null)
            {
                return null;
            }
            _context.Distances.Remove(distance);
            await _context.SaveChangesAsync();
            return distance;
        }

        public async Task<List<Distance>?> GetAllDistanceAsync()
        {
            return await _context.Distances.ToListAsync();
        }

        public async Task<Distance?> GetDistanceById(int distanceID)
        {
            return await _context.Distances.FirstOrDefaultAsync(x => x.DistanceID == distanceID);

        }

        public async Task<Distance?> UpdateDistanceById(int distanceID, AddDistanceDTO distanceDTO)
        {
            var updateDistance = await _context.Distances.FirstOrDefaultAsync(x => x.DistanceID == distanceID);
            if (updateDistance == null)
            {
                return null;
            }

            updateDistance.District = distanceDTO.District;
            updateDistance.Area = distanceDTO.Area;
            updateDistance.Price = distanceDTO.Price;

            _context.Distances.Update(updateDistance);
            await _context.SaveChangesAsync();
            return updateDistance;
        }

    }
}
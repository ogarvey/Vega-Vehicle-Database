using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vega.Data.Models;
using vega.Interfaces;

namespace vega.Data.Persistence
{
    public class VehicleRepository : IVehicleRepository
    {
        public VehicleRepository(VegaDbContext context)
        {
            _context = context;
        }

        public VegaDbContext _context { get; set; }
        public async Task<Vehicle> GetVehicle(int id, bool includeRelated = true)
        {
            if (includeRelated)
                return await _context.Vehicles
                .Include(v => v.Features)
                    .ThenInclude(vf => vf.Feature)
                .Include(v => v.Model)
                    .ThenInclude(m => m.Make)
                .SingleOrDefaultAsync(v => v.Id == id);
            
            return await _context.Vehicles
                .SingleOrDefaultAsync(v => v.Id == id);
        }

        public void Add(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
        }

        public void Remove(Vehicle vehicle)
        {
            _context.Vehicles.Remove(vehicle);
        }
    }
}
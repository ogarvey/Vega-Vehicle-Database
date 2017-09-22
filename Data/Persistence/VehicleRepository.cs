using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vega.Data.Models;
using Vega.Interfaces;

namespace Vega.Data.Persistence
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

        public async Task<IEnumerable<Vehicle>> GetVehicles(VehicleQuery queryObj)
        {
            var query = _context.Vehicles
                .Include(v => v.Features)
                    .ThenInclude(vf => vf.Feature)
                .Include(v => v.Model)
                    .ThenInclude(m => m.Make)
                .AsQueryable();

            if (queryObj.MakeId.HasValue)
                query = query.Where(v => v.Model.MakeId == queryObj.MakeId.Value);

            if (queryObj.ModelId.HasValue)
                query = query.Where(v => v.Model.Id == queryObj.ModelId.Value);

            if (queryObj.IsRegistered.HasValue)
                query = query.Where( v => v.IsRegistered == queryObj.IsRegistered.Value);

            if(queryObj.SortBy == "make")
                query = (queryObj.IsSortAscending) ? query.OrderBy(v => v.Model.Make.Name) : query.OrderByDescending(v => v.Model.Make.Name);

            if(queryObj.SortBy == "model")
                query = (queryObj.IsSortAscending) ? query.OrderBy(v => v.Model.Name) : query.OrderByDescending(v => v.Model.Name);

            if(queryObj.SortBy == "contactName")
                query = (queryObj.IsSortAscending) ? query.OrderBy(v => v.ContactName) : query.OrderByDescending(v => v.ContactName);

            if(queryObj.SortBy == "id")
                query = (queryObj.IsSortAscending) ? query.OrderBy(v => v.Id) : query.OrderByDescending(v => v.Id);

            return await query.ToListAsync();
        }
    }
}
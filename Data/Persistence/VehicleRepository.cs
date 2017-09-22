using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

            var columnsMap = new Dictionary<string, Expression<Func<Vehicle, object>>>()
            {
                ["make"] = v => v.Model.Make.Name,
                ["model"] = v => v.Model.Name,
                ["contactName"] = v => v.ContactName,
                ["id"] = v => v.Id
            };

            query = ApplyOrdering(queryObj, query, columnsMap);
                
            return await query.ToListAsync();
        }

        private IQueryable<Vehicle> ApplyOrdering(VehicleQuery queryObj, IQueryable<Vehicle> query, Dictionary<string, Expression<Func<Vehicle, object>>> columnsMapping)
        {
            if (queryObj.IsSortAscending)
                query = query.OrderBy(columnsMapping[queryObj.SortBy]);
            else
                query = query.OrderByDescending(columnsMapping[queryObj.SortBy]);
                
            return query;
        }
    }
}
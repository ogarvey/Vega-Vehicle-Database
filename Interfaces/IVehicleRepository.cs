using System.Collections.Generic;
using System.Threading.Tasks;
using vega.Data.Models;

namespace vega.Interfaces
{
    public interface IVehicleRepository
    {
        Task<Vehicle> GetVehicle(int id, bool includeRelated = true);
        void Add(Vehicle vehicle);
        void Remove(Vehicle vehicle);
        Task<IEnumerable<Vehicle>> GetVehicles();
    }
}
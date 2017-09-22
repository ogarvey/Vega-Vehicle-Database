using Vega.Interfaces;

namespace Vega.Data.Models
{
    public class VehicleQuery : IQueryObject
    {
        public int? MakeId { get; set; }
        public int? ModelId { get; set; }
        public bool? IsRegistered { get; set; }
        public string SortBy { get; set; }
        public bool IsSortAscending { get; set; }
    }
}
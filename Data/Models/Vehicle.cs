using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Vega.Data.Models;

namespace Vega.Data.Models
{
    public class Vehicle
    {
        public Vehicle()
        {
            Features = new Collection<VehicleFeature>();
        }
        public int Id { get; set; }
        public int ModelId { get; set; }
        public Model Model { get; set; }
        public bool IsRegistered { get; set; }
        [Required]
        [StringLength(255)]
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        [Required]
        [StringLength(20)]
        public string ContactPhone { get; set; }
        public DateTime LastUpdate { get; set; }
        public ICollection<VehicleFeature> Features { get; set; }
    }
}
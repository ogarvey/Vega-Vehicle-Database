using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vega.Controllers.Resources;
using Vega.Data.Persistence;
using Vega.Data.Models;
using Vega.Interfaces;
using System.Collections.Generic;

namespace Vega.Controllers
{
    [Route("api/[controller]")]
    public class VehiclesController : Controller
    {
        public IMapper _mapper { get; set; }
        public IVehicleRepository _repo { get; set; }
        public IUnitOfWork _uow { get; set; }

        public VehiclesController(IMapper mapper
            , IUnitOfWork uow
            , IVehicleRepository repo)
        {
            _mapper = mapper;
            _uow = uow;
            _repo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> CreateVehicle([FromBody]SaveVehicleResource vehicleResource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vehicle = _mapper.Map<SaveVehicleResource, Vehicle>(vehicleResource);
            vehicle.LastUpdate = DateTime.Now;
            _repo.Add(vehicle);

            await _uow.CompleteAsync();

            vehicle = await _repo.GetVehicle(vehicle.Id);

            var result = _mapper.Map<Vehicle, VehicleResource>(vehicle);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicle(int id, [FromBody]SaveVehicleResource vehicleResource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vehicle = await _repo.GetVehicle(id);

            if(vehicle == null)
            {
                return NotFound($"No vehicle found with the id: {id}.");
            }

            _mapper.Map<SaveVehicleResource, Vehicle>(vehicleResource, vehicle);
            vehicle.LastUpdate = DateTime.Now;

            await _uow.CompleteAsync();
            vehicle = await _repo.GetVehicle(vehicle.Id);
            var result = _mapper.Map<Vehicle, VehicleResource>(vehicle);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var vehicle = await _repo.GetVehicle(id, false);

            if(vehicle == null)
            {
                return NotFound($"No vehicle found with the id: {id}.");
            }

            try
            {
                _repo.Remove(vehicle);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            await _uow.CompleteAsync();

            return Ok(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicle(int id)
        {

            var vehicle = await _repo.GetVehicle(id, true);

            if(vehicle == null)
            {
                return NotFound($"No vehicle found with the id: {id}.");
            }

            var vehicleResource = _mapper.
                Map<Vehicle, VehicleResource>(vehicle);

            return Ok(vehicleResource);
        }

        [HttpGet]
        public async Task<IEnumerable<VehicleResource>> GetVehicles(VehicleQueryResource filterResource)
        {
            var filter = _mapper.Map<VehicleQueryResource, VehicleQuery>(filterResource);

            var vehicles = await _repo.GetVehicles(filter);

            return _mapper.Map<IEnumerable<Vehicle>, IEnumerable<VehicleResource>>(vehicles);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CitiesWebAPI.DTOs;
using CitiesWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CitiesWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly DataContext _db;
        private readonly IMapper _mapper;
        public CitiesController(DataContext db, IMapper mapper)
        {
            _mapper = mapper;
            _db = db;
        }

        [Route("Cities")]
        public IActionResult GetCities(bool ShouldGetPointsOfInterest = false)
        {
            List<City> cities = _db.Cities.ToList();
            if (!ShouldGetPointsOfInterest)
            {
                return new ObjectResult(_mapper.Map<List<CityDTO>>(cities));
            }
            return new ObjectResult(_mapper.Map<List<CityDetailedDTO>>(_db.Cities.Include(x => x.PointOfInterests)));
        }

        [Route("City/{id}")]
        public IActionResult GetCity(int id, bool ShouldGetPointsOfInterest = false)
        {
            List<City> cities = _db.Cities.ToList();
            if (!cities.Exists(x => x.Id == id))
            {
                return NotFound();
            }
            if (!ShouldGetPointsOfInterest)
            {
                return new ObjectResult(cities.FindAll(x => x.Id == id).Select(x => new { x.Id, x.Name, x.Description }));
            }
            return new ObjectResult(_db.Cities.Where(x => x.Id == id).Include(x => x.PointOfInterests));
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult CreateCity(City city)
        {
            _db.Cities.Add(city);
            _db.SaveChanges();
            return CreatedAtAction("CreateCity", city);
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public IActionResult DeleteCity(int id)
        {
            var city = _db.Cities.FirstOrDefault(x => x.Id == id);
            if(city is null)
            {
                return NotFound();
            }
            _db.Cities.Remove(city);
            _db.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("Update")]
        public IActionResult Update(City city)
        {
            List<City> cities = _db.Cities.ToList();
            if (!cities.Exists(x => x.Id == city.Id))
            {
                return NotFound();
            }
            City oldCity = _db.Cities.FirstOrDefault(x => x.Id == city.Id);
            _db.Entry(oldCity).CurrentValues.SetValues(city);
            _db.SaveChanges();
            return Ok();
        }

        [HttpPatch]
        [Route("Update/{id}")]
        public IActionResult Patch(JsonPatchDocument<City> cityPatch, int id)
        {
            var oldCity = _db.Cities.FirstOrDefault(x => x.Id == id);
            if(oldCity is null)
            {
                return NotFound();
            }
            cityPatch.ApplyTo(oldCity);
            _db.Update(oldCity);
            _db.SaveChanges();

            return Ok();
        }
    }
}
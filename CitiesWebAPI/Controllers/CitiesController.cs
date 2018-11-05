using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitiesWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CitiesWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private List<City> _cities = new List<City>()
            {
                new City(){Id=1,Name="Odense", Description="Brånner!", PointsOfInterest=new List<PointOfInterest>{ new PointOfInterest(){ Id = 1, Name = "Brandts", Description = "Kunst!" }, new PointOfInterest(){ Id = 2, Name = "H. C. Andersens hus", Description = "Kultur!" } } },
                new City(){Id=2,Name="Aalborg", Description="prlør!", PointsOfInterest= new List<PointOfInterest>{ new PointOfInterest(){ Id=3, Name="Vandtårnet", Description="Der er fucking meget vand"}, new PointOfInterest(){ Id = 4, Name = "Friluftsbadet", Description = "Der er endnu mere vand" } } }
            };
        public CitiesController()
        {

        }

        [Route("Cities")]
        public IActionResult GetCities(bool ShouldGetPointsOfInterest = false)
        {
            List<City> cities = _cities;
            if (!ShouldGetPointsOfInterest)
            {
                return new ObjectResult(cities.Select(x => new { x.Id, x.Name, x.Description }));
            }
            return new ObjectResult(cities);
        }

        [Route("City/{id}")]
        public IActionResult GetCity(int id, bool ShouldGetPointsOfInterest = false)
        {
            if (!_cities.Exists(x => x.Id == id))
            {
                return NotFound();
            }
            if (!ShouldGetPointsOfInterest)
            {
                return new ObjectResult(_cities.FindAll(x => x.Id == id).Select(x => new { x.Id, x.Name, x.Description }));
            }
            return new ObjectResult(_cities.FirstOrDefault(x => x.Id == id));
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult CreateCity(City city)
        {
            _cities.Add(city);
            return CreatedAtAction("CreateCity", city);
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public IActionResult DeleteCity(int id)
        {
            _cities.RemoveAll(x => x.Id == id);
            return Ok();
        }

        [HttpPut]
        [Route("Update")]
        public IActionResult Update(City city)
        {
            if(!_cities.Exists(x => x.Id == city.Id))
            {
                return NotFound();
            }
            City oldCity = _cities.FirstOrDefault(x => x.Id == city.Id);
            oldCity = city;
            return Ok();
        }
    }
}
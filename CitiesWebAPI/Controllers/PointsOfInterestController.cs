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
    public class PointsOfInterestController : ControllerBase
    {
        private List<City> _cities = new List<City>()
            {
                new City(){Id=1,Name="Odense", Description="Brånner!", PointsOfInterest=new List<PointOfInterest>{ new PointOfInterest(){ Id = 1, Name = "Brandts", Description = "Kunst!" }, new PointOfInterest(){ Id = 2, Name = "H. C. Andersens hus", Description = "Kultur!" } } },
                new City(){Id=2,Name="Aalborg", Description="prlør!", PointsOfInterest= new List<PointOfInterest>{ new PointOfInterest(){ Id=3, Name="Vandtårnet", Description="Der er fucking meget vand"}, new PointOfInterest(){ Id = 4, Name = "Friluftsbadet", Description = "Der er endnu mere vand" } } }
            };

        [Route("PointsOfInterest/{id}")]
        public IActionResult GetPointsOfInterest(int id)
        {
            if(!_cities.Exists(x=> x.Id == id)) {
                return NotFound();
            }
            return new OkObjectResult(_cities.FirstOrDefault(x => x.Id == id).PointsOfInterest);
        }

        [Route("PointOfInterest/{cityId}/{poiId}")]
        public IActionResult GetPointOfInterest(int cityId, int poiId)
        {
            if (!_cities.Exists(x => x.Id == cityId) || !_cities.FirstOrDefault(x => x.Id == cityId).PointsOfInterest.Exists(x => x.Id == poiId))
            {
                return NotFound();
            }
            return new OkObjectResult(_cities.FirstOrDefault(x => x.Id == cityId).PointsOfInterest.FirstOrDefault(x => x.Id == poiId));
        }

        [HttpPost]
        [Route("Create/{id}")]
        public IActionResult CreatePoi(int id, PointOfInterest poi)
        {
            _cities.FirstOrDefault(x => x.Id == id).PointsOfInterest.Add(poi);
            return CreatedAtAction("CreatePoi", poi);
        }

        [HttpDelete]
        [Route("Delete/{cityId}/{poiId}")]
        public IActionResult DeleteCity(int cityId, int poiId)
        {
            _cities.FirstOrDefault(x => x.Id == cityId).PointsOfInterest.RemoveAll(x => x.Id == poiId);
            return Ok();
        }

        [HttpPut]
        [Route("Update/{id}")]
        public IActionResult Update(int id, PointOfInterest poi)
        {
            if (!_cities.Exists(x => x.Id == id))
            {
                return NotFound();
            }
            City city = _cities.FirstOrDefault(x => x.Id == id);
            PointOfInterest oldPoi = city.PointsOfInterest.FirstOrDefault(x => x.Id == poi.Id);
            oldPoi = poi;
            return Ok();
        }
    }
}
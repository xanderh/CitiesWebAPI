using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitiesWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CitiesWebAPI.Controllers
{
    [Route("api/poi")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {

        private readonly DataContext _db;
        public PointsOfInterestController(DataContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Route("pois/{id}")]
        public IActionResult GetPointsOfInterest(int id)
        {
            var cities = _db.Cities.ToList();
            if(!cities.Exists(x=> x.Id == id)) {
                return NotFound();
            }
            return new OkObjectResult(_db.PointsOfInterest.Where(x => x.CityId == id));
        }

        [HttpGet]
        [Route("poi/{poiId}")]
        public IActionResult GetPointOfInterest(int poiId)
        {
            var pois = _db.PointsOfInterest.ToList();
            if (!pois.Exists(x => x.Id == poiId))
            {
                return NotFound();
            }
            return new OkObjectResult(_db.PointsOfInterest.FirstOrDefault(x => x.Id == poiId));
        }

        [HttpGet]
        [Route("pois")]
        public IActionResult GetPointsOfInterest()
        {
            var pois = _db.PointsOfInterest.ToList();
            return new OkObjectResult(_db.PointsOfInterest.ToList());
        }

        [HttpPost]
        [Route("Create/{id}")]
        public IActionResult CreatePoi(int id, PointOfInterest poi)
        {
            poi.CityId = id;
            _db.PointsOfInterest.Add(poi);
            _db.SaveChanges();
            return CreatedAtAction("CreatePoi", poi);
        }

        [HttpDelete]
        [Route("Delete/{poiId}")]
        public IActionResult DeletePOI(int poiId)
        {
            var poi = _db.PointsOfInterest.FirstOrDefault(x => x.Id == poiId);
            if(poi is null)
            {
                return NotFound();
            }
            _db.PointsOfInterest.Remove(poi);
            _db.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route("Update")]
        public IActionResult Update(PointOfInterest poi)
        {
            var oldPoi = _db.PointsOfInterest.FirstOrDefault(x => x.Id == poi.Id);
            if (oldPoi is null)
            {
                return NotFound();
            }
            _db.Entry(oldPoi).CurrentValues.SetValues(poi);
            _db.SaveChanges();
            return Ok();
        }

        [HttpPatch]
        [Route("Update/{id}")]
        public IActionResult Patch(JsonPatchDocument<PointOfInterest> poiPatch, int id)
        {
            var oldPOI = _db.PointsOfInterest.FirstOrDefault(x => x.Id == id);
            if (oldPOI is null)
            {
                return NotFound();
            }
            poiPatch.ApplyTo(oldPOI);
            _db.Update(oldPOI);
            _db.SaveChanges();

            return Ok();
        }
    }
}
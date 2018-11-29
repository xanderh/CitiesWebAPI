using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CitiesWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CitiesWebAPI.Controllers
{
    [Route("api/poi")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly DataContext _db;
        public PointsOfInterestController(DataContext db, UnitOfWork unitOfWork)
        {
            _db = db;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("pois/{id}")]
        public IActionResult GetPointsOfInterest(int id)
        {
            var cities = _db.Cities.ToList();
            if (_unitOfWork.Cities.Get(id) is null)
            {
                return NotFound();
            }
            return new OkObjectResult(_unitOfWork.PointsOfInterest.GetPOIsByCity(id));
        }

        [HttpGet]
        [Route("poi/{poiId}")]
        public IActionResult GetPointOfInterest(int poiId)
        {
            var pois = _db.PointsOfInterest.ToList();
            if (_unitOfWork.PointsOfInterest.Get(poiId) is null)
            {
                return NotFound();
            }
            return new OkObjectResult(_unitOfWork.PointsOfInterest.Get(poiId));
        }

        [HttpGet]
        [Route("pois")]
        public IActionResult GetPointsOfInterest()
        {
            return new OkObjectResult(_unitOfWork.PointsOfInterest.GetAll());
        }

        [Authorize]
        [HttpPost]
        [Route("Create/{id}")]
        public IActionResult CreatePoi(int id, PointOfInterest poi)
        {
            poi.CityId = id;
            _unitOfWork.PointsOfInterest.Add(poi);
            try
            {

                _unitOfWork.Complete();
                return CreatedAtAction("CreatePoi", poi);
            }
            catch (Exception)
            {
                return Conflict();
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("Delete/{poiId}")]
        public IActionResult DeletePOI(int poiId)
        {
            var poi = _unitOfWork.PointsOfInterest.Get(poiId);
            if (poi is null)
            {
                return NotFound();
            }
            try
            {
                _unitOfWork.PointsOfInterest.Remove(poi);
                _unitOfWork.Complete();
                return Ok();
            }
            catch (Exception)
            {
                return Conflict();
            }
        }

        [Authorize]
        [HttpPut]
        [Route("Update")]
        public IActionResult Update(PointOfInterest poi)
        {
            PointOfInterest oldPoi = _unitOfWork.PointsOfInterest.Get(poi.Id);
            if (oldPoi is null)
            {
                return NotFound();
            }
            oldPoi.CityId = poi.CityId;
            oldPoi.Description = poi.Description;
            oldPoi.Name = poi.Name;
            try
            {
                _unitOfWork.Complete();
                return Ok();
            }
            catch (Exception)
            {
                return Conflict();
            }
        }

        [Authorize]
        [HttpPatch]
        [Route("Update/{id}")]
        public IActionResult Patch(JsonPatchDocument<PointOfInterest> poiPatch, int id)
        {
            var oldPOI = _unitOfWork.PointsOfInterest.Get(id);
            if (oldPOI is null)
            {
                return NotFound();
            }
            try
            {
                poiPatch.ApplyTo(oldPOI);
                _unitOfWork.Complete();
                return Ok();
            }
            catch (Exception)
            {
                return Conflict();
            }
        }
    }
}
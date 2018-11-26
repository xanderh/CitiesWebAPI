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
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;
        public CitiesController(IMapper mapper, UnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("Cities")]
        public IActionResult GetCities(bool getPOI = false)
        {
            if (!getPOI)
            {
                return new ObjectResult(_mapper.Map<List<CityDTO>>(_unitOfWork.Cities.GetCitiesWithoutPOIs()));
            }
            return new ObjectResult(_mapper.Map<List<CityDetailedDTO>>(_unitOfWork.Cities.GetCitiesWithPOIs()));
        }

        [HttpGet]
        [Route("City/{id}")]
        public IActionResult GetCity(int id, bool getPOI = false)
        {
            if (_unitOfWork.Cities.Get(id) is null)
            {
                return NotFound();
            }
            if (!getPOI)
            {
                return new ObjectResult(_mapper.Map<List<CityDTO>>(_unitOfWork.Cities.GetCityWithoutPOIs(id)));
            }
            return new ObjectResult(_mapper.Map<List<CityDetailedDTO>>(_unitOfWork.Cities.GetCityWithPOIs(id)));
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult CreateCity(City city)
        {
            _unitOfWork.Cities.Add(city);
            _unitOfWork.Complete();
            return CreatedAtAction("CreateCity", city);
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public IActionResult DeleteCity(int id)
        {
            var city = _unitOfWork.Cities.Get(id);
            if (city is null)
            {
                return NotFound();
            }
            try
            {
                _unitOfWork.Cities.Remove(city);
                _unitOfWork.Complete();
                return Ok();
            }
            catch (Exception)
            {
                return Conflict();
            }
        }

        [HttpPut]
        [Route("Update")]
        public IActionResult Update(City city)
        {
            if (_unitOfWork.Cities.Get(city.Id) is null)
            {
                return NotFound();
            }
            City oldCity = _unitOfWork.Cities.Get(city.Id);
            try
            {
                oldCity.Name = city.Name;
                oldCity.Description = city.Description;
                _unitOfWork.Complete();
                return Ok();
            }
            catch (Exception)
            {
                return Conflict();
            }
        }

        [HttpPatch]
        [Route("Update/{id}")]
        public IActionResult Patch(JsonPatchDocument<City> cityPatch, int id)
        {
            var oldCity = _unitOfWork.Cities.Get(id);
            if (oldCity is null)
            {
                return NotFound();
            }
            try
            {
                cityPatch.ApplyTo(oldCity);
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
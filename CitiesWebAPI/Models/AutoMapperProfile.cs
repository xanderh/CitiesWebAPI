using AutoMapper;
using CitiesWebAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CitiesWebAPI.Models
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<City, CityDTO>();
            CreateMap<CityDTO, City>();
            CreateMap<City, CityDetailedDTO>();
            CreateMap<CityDetailedDTO, City>();
        }
    }
}

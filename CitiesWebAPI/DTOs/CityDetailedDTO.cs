﻿using CitiesWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CitiesWebAPI.DTOs
{
    public class CityDetailedDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<PointOfInterest> PointOfInterests { get; set; }
    }
}

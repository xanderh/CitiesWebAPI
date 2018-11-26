using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CitiesWebAPI.Models
{
    public class PointOfInterest
    {
        public int Id { get; set; }
        [ConcurrencyCheck]
        public string Name { get; set; }
        [ConcurrencyCheck]
        public string Description { get; set; }
        [ConcurrencyCheck]
        public int CityId { get; set; }

    }
}

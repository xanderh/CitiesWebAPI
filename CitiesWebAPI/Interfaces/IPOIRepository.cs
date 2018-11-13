using CitiesWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CitiesWebAPI.Interfaces
{
    public interface IPOIRepository : IRepository<PointOfInterest>
    {
        IEnumerable<PointOfInterest> GetPOIsByCity(int id);
    }
}

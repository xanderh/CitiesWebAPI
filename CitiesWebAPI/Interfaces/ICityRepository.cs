using CitiesWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CitiesWebAPI.Interfaces
{
    public interface ICityRepository : IRepository<City>
    {
        IEnumerable<City> GetCitiesWithoutPOIs();
        IEnumerable<City> GetCitiesWithPOIs();
        City GetCityWithoutPOIs(int id);
        City GetCityWithPOIs(int id);

    }
}

using CitiesWebAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CitiesWebAPI.Models.Repositories
{
    public class CityRepository : Repository<City>, ICityRepository
    {
        public CityRepository(DataContext context)
            : base(context)
        {
        }

        public IEnumerable<City> GetCitiesWithoutPOIs()
        {
            return DataContext.Cities.ToList();
        }

        public IEnumerable<City> GetCitiesWithPOIs()
        {
            return DataContext.Cities.Include(c => c.PointOfInterests).ToList();
        }

        public City GetCityWithoutPOIs(int id)
        {
            return DataContext.Cities.FirstOrDefault(c => c.Id == id);
        }

        public City GetCityWithPOIs(int id)
        {
            return DataContext.Cities.Include(c => c.PointOfInterests).FirstOrDefault(c => c.Id == id);
        }

        public DataContext DataContext
        {
            get { return base.Context as DataContext; }
        }
    }
}

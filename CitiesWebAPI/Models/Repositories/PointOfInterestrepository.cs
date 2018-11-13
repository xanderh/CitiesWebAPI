using CitiesWebAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CitiesWebAPI.Models.Repositories
{
    public class PointOfInterestRepository : Repository<PointOfInterest>, IPOIRepository
    {
        public PointOfInterestRepository(DataContext dataContext)
            : base(dataContext)
        {
        }
        public IEnumerable<PointOfInterest> GetPOIsByCity(int id)
        {
            return DataContext.Cities.Include(c => c.PointOfInterests).FirstOrDefault(c => c.Id == id).PointOfInterests.ToList();
        }

        public DataContext DataContext
        {
            get { return base.Context as DataContext; }
        }
    }
}

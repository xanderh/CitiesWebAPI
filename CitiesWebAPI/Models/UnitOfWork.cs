using CitiesWebAPI.Interfaces;
using CitiesWebAPI.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CitiesWebAPI.Models
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;

        public UnitOfWork(DataContext context)
        {
            _context = context;
            Cities = new CityRepository(_context);
            PointsOfInterest = new PointOfInterestRepository(_context);
        }

        public ICityRepository Cities { get; private set; }

        public IPOIRepository PointsOfInterest { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

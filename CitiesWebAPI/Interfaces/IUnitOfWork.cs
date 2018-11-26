using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CitiesWebAPI.Interfaces
{
    interface IUnitOfWork : IDisposable
    {
        ICityRepository Cities { get; }
        IPOIRepository PointsOfInterest { get; }
        int Complete();
    }
}

using MouratoAirport.Data.Entities;
using MouratoAirport.Data;

namespace MouratoAirport.Data
{
    public class FlightRepository : GenericRepository<Flights>, IFlightRepository
    {
        public FlightRepository(DataContext context) : base(context)
        {
        }
    }
}

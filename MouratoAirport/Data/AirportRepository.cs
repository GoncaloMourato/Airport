using MouratoAirport.Data.Entities;

namespace MouratoAirport.Data
{
    public class AirportRepository : GenericRepository<Airpo>, IAirportRepository
    {
        public AirportRepository(DataContext context) : base(context)
        {

        }
    }
}

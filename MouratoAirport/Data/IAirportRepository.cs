using Microsoft.AspNetCore.Mvc.Rendering;
using MouratoAirport.Data.Entities;
using System.Collections.Generic;

namespace MouratoAirport.Data
{
    public interface IAirportRepository : IGenericRepository<Airpo>
    {
        IEnumerable<SelectListItem>GetComboAirports();
    }
}

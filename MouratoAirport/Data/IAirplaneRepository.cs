using MouratoAirport.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using MouratoAirport.Data;
using System.Collections.Generic;

namespace MouratoAirport.Data
{
    public interface IAirplaneRepository : IGenericRepository<Airplane>
    {
        IEnumerable<SelectListItem> GetComboAviaos();
    }
}

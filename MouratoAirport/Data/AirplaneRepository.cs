using MouratoAirport.Data.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;

namespace MouratoAirport.Data
{
    public class AirplaneRepository : GenericRepository<Airplane>, IAirplaneRepository
    {
        private readonly DataContext _context;

        public AirplaneRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem> GetComboAviaos()
        {
            var lista = _context.Airplane.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

            lista.Insert(0, new SelectListItem
            {
                Text = "Select an airplane ...",
                Value = "0"
            });

            return lista;
        }
    }
}

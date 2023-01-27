using Microsoft.AspNetCore.Mvc.Rendering;
using MouratoAirport.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace MouratoAirport.Data
{
    public class AirportRepository : GenericRepository<Airpo>, IAirportRepository
    {
        private readonly DataContext _context;

        public AirportRepository(DataContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<SelectListItem> GetComboAirports()
        {
            var list = _context.Airpo.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value = p.Name
            }).ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Select one ...)",
                Value = "0"
            });
            return list;
        }
    }
}

using MouratoAirport.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MouratoAirport.Helpers;
using MouratoAirport.Data;
using Microsoft.EntityFrameworkCore;

namespace Airport.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserHelper _userHelper;
        private readonly ITicketRepository _ticketRepository;
        private readonly IFlightRepository _flightRepository;

        public HomeController(ILogger<HomeController> logger,IUserHelper userHelper, ITicketRepository ticketRepository, IFlightRepository flightRepository)
        {
            _logger = logger;
            _userHelper = userHelper;
            _ticketRepository = ticketRepository;
            _flightRepository = flightRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> TicketSee(int id)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);
            var model = new TicketSeeViewModel
            {
                Date= ticket.Date,
                Name=ticket.Name,
                FlightsId=ticket.FlightsId,
                Flights = await _flightRepository.GetByIdAsync(ticket.FlightsId),
                Number=ticket.Number,
                Seat =ticket.Seat,
                TypeSeat =ticket.TypeSeat

            };

            return View(model);
        }

        public IActionResult Error404()
        {
            return View();
        }

        public async Task<IActionResult> History()
        {
            var user = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);

            if(user == null)
            {
                return View("Error");
            }

            var model = new HistoryTicketViewModel
            {
                Tickets = _ticketRepository.GetAll().Include(x => x.Flights).Where(p => p.UserId == user.Id).Where(p=>p.Flights.Date.Date >= DateTime.Today).ToList(),
                TicketsExpired = _ticketRepository.GetAll().Include(x => x.Flights).Where(p => p.UserId == user.Id).Where(p=> p.Flights.Date <= DateTime.Today).ToList()
            };

            return View(model);
        }

        public IActionResult Schedules()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MouratoAirport.Models;
using Microsoft.AspNetCore.Authorization;
using MouratoAirport.Data.Entities;
using MouratoAirport.Data;

namespace MouratoAirport.Controllers
{
    //[Authorize]
    public class FlightsController : Controller
    {
        private readonly IFlightRepository _flightsRepository;
        private readonly IAirplaneRepository _airplaneRepository;
        private readonly IAirportRepository _airportRepository;

        public FlightsController(IFlightRepository flightsRepository, IAirplaneRepository AirplaneRepository, IAirportRepository airportRepository)
        {
            _flightsRepository = flightsRepository;
            _airplaneRepository = AirplaneRepository;
            _airportRepository = airportRepository;
        }

        // GET: Voos
        public IActionResult Index()
        {
            var model = new IndexFlightsViewModel
            {
                Flights = _flightsRepository.GetAll().Include(p => p.Airplane).OrderBy(x => x.Date).ToList()
            };



            return View(model);

        }

        // GET: Aviaos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundObjectResult("FlyNotFound");
            }

            var lessee = await _flightsRepository.GetByIdAsync(id.Value);
            if (lessee == null)
            {
                return new NotFoundObjectResult("FlyNotFound");
            }

            return View(lessee);
        }
        // GET: Aviaos/Create
        public IActionResult Create()
        {
            Random rand = new Random();

            // Define a string de caracteres possíveis
            string possibleChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            // Define o tamanho do número aleatório
            int size = 6;

            // Cria uma string vazia para armazenar o número aleatório
            string numberflight = "";

            // Adiciona caracteres aleatórios à string
            for (int i = 0; i < size; i++)
            {
                numberflight += possibleChars[rand.Next(possibleChars.Length)];
            }

            var model = new FlightsViewModel
            {
                Airplanes = _airplaneRepository.GetComboAviaos(),
                Airports = _airportRepository.GetComboAirports(),
                RandomNumber = numberflight
            };
            return View(model);
        }

        // POST: Aviaos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FlightsViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Airplanes = _airplaneRepository.GetComboAviaos();
                model.Number = model.RandomNumber;
                await _flightsRepository.CreateAsync(model);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Aviaos/Edit/5
        public async Task<IActionResult> Edit(int? id, FlightsViewModel model)
        {
            if (id == null)
            {
                return new NotFoundObjectResult("FlyNotFound");
            }

            var flights = await _flightsRepository.GetByIdAsync(id.Value);
            if (flights == null)
            {
                return new NotFoundObjectResult("FlyNotFound");
            }

            var flight = new FlightsViewModel
            {
                Id = flights.Id,
                From = flights.From,
                To = flights.To,
                Date = flights.Date,
                AirplaneId = flights.AirplaneId,
                Airplane = flights.Airplane,
                Airplanes = _airplaneRepository.GetComboAviaos()

            };

            return View(flight);
        }

        // POST: Aviaos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Flights flights)
        {
            if (id != flights.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _flightsRepository.UpdateAsync(flights);
                }
                catch (DbUpdateConcurrencyException)
                {

                }
                return RedirectToAction(nameof(Index));
            }

            var voo = new FlightsViewModel
            {
                Id = flights.Id,
                From = flights.From,
                To = flights.To,
                Date = flights.Date,
                AirplaneId = flights.AirplaneId,
                Airplane = flights.Airplane,
                Airplanes = _airplaneRepository.GetComboAviaos()

            };

            return View(voo);
        }

        // GET: Aviaos/Delete/5

        public async Task<IActionResult> Delete(int id)
        {
            var flights = await _flightsRepository.GetByIdAsync(id);
            await _flightsRepository.DeleteAsync(flights);

            return RedirectToAction("Index");
        }

        public IActionResult FlyNotFound()
        {
            return View();
        }

    }
}

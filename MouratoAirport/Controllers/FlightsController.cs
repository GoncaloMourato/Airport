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

        public FlightsController(IFlightRepository voosRepository, IAirplaneRepository AirplaneRepository)
        {
            _flightsRepository = voosRepository;
            _airplaneRepository = AirplaneRepository;
        }

        // GET: Voos
        public IActionResult Index()
        {
            return View(_flightsRepository.GetAll().OrderBy(p => p.Airplane));
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
            var model = new FlightsViewModel
            {
                Airplanes = _airplaneRepository.GetComboAviaos()
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
                await _flightsRepository.CreateAsync(model);
                return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> Delete(int? id)
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

            return View(flights);
        }

        // POST: Aviaos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var flights = await _flightsRepository.GetByIdAsync(id);
            await _flightsRepository.DeleteAsync(flights);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult FlyNotFound()
        {
            return View();
        }

    }
}

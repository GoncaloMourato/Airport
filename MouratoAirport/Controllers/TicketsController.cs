using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using MouratoAirport.Data.Entities;
using MouratoAirport.Data;

namespace MouratoAirport.Controllers
{
    //[Authorize]
    public class TicketsController : Controller
    {
        private readonly ITicketRepository _ticketsRepository;

        public TicketsController(ITicketRepository ticketsRepository)
        {
            _ticketsRepository = ticketsRepository;
        }

        // GET: Bilhetes

        public IActionResult Seats()
        {
            return View();
        }

        public IActionResult BuyTicket()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View(_ticketsRepository.GetAll().OrderBy(p => p.Id));
        }

        // GET: Bilhetes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundObjectResult("TicketNotFound");
            }

            var lessee = await _ticketsRepository.GetByIdAsync(id.Value);
            if (lessee == null)
            {
                return new NotFoundObjectResult("TicketNotFound");
            }

            return View(lessee);
        }
        // GET: Aviaos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Aviaos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                await _ticketsRepository.CreateAsync(ticket);
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: Aviaos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new NotFoundObjectResult("TicketNotFound");
            }

            var ticket = await _ticketsRepository.GetByIdAsync(id.Value);
            if (ticket == null)
            {
                return new NotFoundObjectResult("TicketNotFound");
            }
            return View(ticket);
        }

        // POST: Aviaos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _ticketsRepository.UpdateAsync(ticket);
                }
                catch (DbUpdateConcurrencyException)
                {

                }
                return RedirectToAction(nameof(Index));
            }
            return View(ticket);
        }

        // GET: Aviaos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundObjectResult("TicketNotFound");
            }

            var ticket = await _ticketsRepository.GetByIdAsync(id.Value);
            if (ticket == null)
            {
                return new NotFoundObjectResult("TicketNotFound");
            }

            return View(ticket);
        }

        // POST: Aviaos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _ticketsRepository.GetByIdAsync(id);
            await _ticketsRepository.DeleteAsync(ticket);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult TicketNotFound()
        {
            return View();
        }

    }
}

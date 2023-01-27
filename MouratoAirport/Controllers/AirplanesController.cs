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
using MouratoAirport.Models;
using MouratoAirport.Helpers;
using Microsoft.AspNetCore.Identity;

namespace MouratoAirport.Controllers
{
    //[Authorize]
    [Authorize(Roles = "Admin")]
    public class AirplanesController : Controller
    {
        private readonly IAirplaneRepository _airplaneRepository;
        private readonly IImageHelper _imageHelper;
        private readonly IAirportRepository _airportRepository;

        public AirplanesController(IAirplaneRepository airplaneRepository,IImageHelper imageHelper,IAirportRepository airportRepository)
        {
            _airplaneRepository = airplaneRepository;
            _imageHelper = imageHelper;
            _airportRepository = airportRepository;
        }

        // GET: Aviaos
        public  IActionResult Index()
        {
            var model = new AirplaneAirpoViewModel
            {
                Airplane = _airplaneRepository.GetAll().OrderBy(p => p.Name).ToList(),
                Airpo = _airportRepository.GetAll().OrderBy(p => p.Name).ToList()
            };



            return View(model);


        }

        // GET: Aviaos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new NotFoundObjectResult("PlainNotFound");
            }

            var lessee = await _airplaneRepository.GetByIdAsync(id.Value);
            if (lessee == null)
            {
                return new NotFoundObjectResult("PlainNotFound");
            }

            return View(lessee);
        }
        // GET: Aviaos/Create
        //[Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Aviaos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AirplaneViewModel model1)
        {
            var imageId = string.Empty;

            if (model1.ImageFile != null && model1.ImageFile.Length > 0)
            {
                imageId = await _imageHelper.UploadImageAsync(model1.ImageFile, "Airplane");
            }
            var airplane = new Airplane
            {
                ImageUrl = imageId,
                Name = model1.Name,
                Model = model1.Model,
                Seat = model1.Seat
            };

            try
            {
                await _airplaneRepository.CreateAsync(airplane);
            }
            catch
            {
                
            }

            return RedirectToAction("Index");
        }

        // GET: Aviaos/Edit/5
        public async Task<IActionResult> Edit(int? id,string edit)
        {
            if(edit == "airplane")
            {
                if(id == null)
                {
                    return RedirectToAction("Index");
                }

                var airplane = await _airplaneRepository.GetByIdAsync(id.Value);

                if (airplane == null)
                {
                    return RedirectToAction("Index");
                }

                var model = new AirplaneViewModel
                {
                    Name = airplane.Name,
                    Model = airplane.Model,
                    Seat = airplane.Seat,
                    ImageUrl = airplane.ImageUrl,
                    Id= airplane.Id,
                    TypeEdit = "airplane"
                };

                return View(model);
            }
            else if (edit == "airport")
            {
                if (id == null)
                {
                    return RedirectToAction("Index");
                }

                var airport = await _airportRepository.GetByIdAsync(id.Value);

                if (airport == null)
                {
                    return RedirectToAction("Index");
                }

                var model = new AirplaneViewModel
                {
                NameAirport = airport.Name,
                CityAirport = airport.City,
                LocationAirport = airport.Location,
                Id = airport.Id,
                TypeEdit = "airport"
                };

                return View(model);
            }

            return View();
        }

        // POST: Aviaos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AirplaneViewModel model2)
        {

            if(model2.TypeEdit == "airplane")
            {
                if(id != 0)
                {
                    var imageId = string.Empty;

                    if (model2.ImageFile != null && model2.ImageFile.Length > 0)
                    {
                        imageId = await _imageHelper.UploadImageAsync(model2.ImageFile, "Airplane");
                    }


                    var airplane = await _airplaneRepository.GetByIdAsync(id);
                    airplane.Seat = model2.Seat;
                    airplane.Model = model2.Model;
                    airplane.Name = model2.Name;
                    airplane.ImageUrl = imageId;

                    try
                    {
                        await _airplaneRepository.UpdateAsync(airplane);
                    }
                    catch
                    {

                    }


                }


            }
            else if(model2.TypeEdit == "airport")
            {
                if (id != 0)
                {


                    var airport = await _airportRepository.GetByIdAsync(id);
                    airport.Name = model2.NameAirport;
                    airport.City = model2.CityAirport;
                    airport.Location = model2.LocationAirport;

                    try
                    {
                        await _airportRepository.UpdateAsync(airport);
                    }
                    catch
                    {

                    }

                }

            }

            return RedirectToAction("Index");

        }

        // GET: Aviaos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new NotFoundObjectResult("PlainNotFound");
            }

            var airplane = await _airplaneRepository.GetByIdAsync(id.Value);
            if (airplane == null)
            {
                return new NotFoundObjectResult("PlainNotFound");
            }

            return View(airplane);
        }





        public async Task<IActionResult> Remove(int id)
        {
            var airplane = await _airplaneRepository.GetByIdAsync(id);
            await _airplaneRepository.DeleteAsync(airplane);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> RemoveAirport(int id)
        {
            var airport = await _airportRepository.GetByIdAsync(id);
            await _airportRepository.DeleteAsync(airport);

            return RedirectToAction("Index");
        }





        // POST: Aviaos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var airplane = await _airplaneRepository.GetByIdAsync(id);
            await _airplaneRepository.DeleteAsync(airplane);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult PlainNotFound()
        {
            return View();
        }



        public async Task<IActionResult> CreateAirport(AirplaneViewModel model1)
        {

            var airport = new Airpo
            {
                Name = model1.NameAirport,
                City = model1.CityAirport,
                Location = model1.LocationAirport
            };

            try
            {
                await _airportRepository.CreateAsync(airport);
            }
            catch
            {

            }

            return RedirectToAction("Index");
        }
    }
}

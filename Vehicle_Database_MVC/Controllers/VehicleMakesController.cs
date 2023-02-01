using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vehicle_Database_MVC.Data;
using Vehicle_Database_MVC.Models;
using Vehicle_Database_MVC.Models.Domain;

namespace Vehicle_Database_MVC.Controllers
{
    public class VehicleMakesController : Controller
    {
        private readonly VehicleDbContext vehicleDbContext;

        public VehicleMakesController(VehicleDbContext vehicleDbContext)
        {
            this.vehicleDbContext = vehicleDbContext;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var makes = await vehicleDbContext.Makes.ToListAsync();
            return View(makes);
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddVehicleMakeViewModel addVehicleMake)
        {
            var vehicle = new VehicleMake()
            {
                VehicleName = addVehicleMake.VehicleName,
                VehicleAbrv = addVehicleMake.VehicleAbrv
            };
            await vehicleDbContext.Makes.AddAsync(vehicle);
            await vehicleDbContext.SaveChangesAsync();
            return RedirectToAction("Add");


        }
        [HttpGet]
        public async Task<IActionResult> View(int id)
        {
            var make = await vehicleDbContext.Makes.FirstOrDefaultAsync(x => x.Id == id);
            if (make != null)
            {

                var viewModel = new UpdateVehicleMakeViewModel()
                {
                    Id = make.Id,
                    VehicleName = make.VehicleName,
                    VehicleAbrv = make.VehicleAbrv
                };
                return await Task.Run(() => View("View", viewModel));
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> View(UpdateVehicleMakeViewModel model)
        {
            var vehicleMake = await vehicleDbContext.Makes.FindAsync(model.Id);
            if (vehicleMake != null)
            {
                vehicleMake.VehicleName = model.VehicleName;
                vehicleMake.VehicleAbrv = model.VehicleAbrv;
                await vehicleDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(UpdateVehicleMakeViewModel model)
        {
            var vehicleMake = await vehicleDbContext.Makes.FindAsync(model.Id);
            if (vehicleMake != null)
            {
                vehicleDbContext.Makes.Remove(vehicleMake);
                await vehicleDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

    }
}

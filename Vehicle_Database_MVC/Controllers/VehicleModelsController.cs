using Microsoft.AspNetCore.Mvc;
using Vehicle_Database_MVC.Data;
using Vehicle_Database_MVC.Models.Domain;
using Vehicle_Database_MVC.Models;
using Microsoft.EntityFrameworkCore;

namespace Vehicle_Database_MVC.Controllers
{
    public class VehicleModelsController : Controller
    {
        private readonly VehicleDbContext vehicleDbContext;

        public VehicleModelsController(VehicleDbContext vehicleDbContext)
        {
            this.vehicleDbContext = vehicleDbContext;
        }
        [HttpGet]
        public async Task<IActionResult> IndexVehicleModel()
        {
            var models = await vehicleDbContext.Models.ToListAsync();
            return View(models);
        }
        public IActionResult AddVehicleModel()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddVehicleModel(AddVehicleModelViewModel viewModel)
        {
            var vehicle = new VehicleModel()
            {
                VehicleName = viewModel.VehicleName,
                VehicleAbrv = viewModel.VehicleAbrv
            };
            await vehicleDbContext.Models.AddAsync(vehicle);
            await vehicleDbContext.SaveChangesAsync();
            return RedirectToAction("AddVehicleModel");
        }

        [HttpGet]
        public async Task<IActionResult> ViewVehicleModel(int id)
        {
            var model = await vehicleDbContext.Models.FirstOrDefaultAsync(x => x.Id == id);
            if (model != null)
            {

                var viewModel = new UpdateVehicleModelViewModel()
                {
                    Id = model.Id,
                    MakeId = model.MakeId,
                    VehicleName = model.VehicleName,
                    VehicleAbrv = model.VehicleAbrv
                };
                return await Task.Run(() => View("ViewVehicleModel", viewModel));
            }
            return RedirectToAction("IndexVehicleModel");
        }
        [HttpPost]
        public async Task<IActionResult> ViewVehicleModel(UpdateVehicleModelViewModel model)
        {
            var vehicleModel = await vehicleDbContext.Models.FindAsync(model.Id);
            if (vehicleModel != null)
            {
                vehicleModel.VehicleName = model.VehicleName;
                vehicleModel.VehicleAbrv = model.VehicleAbrv;
                await vehicleDbContext.SaveChangesAsync();
                return RedirectToAction("IndexVehicleModel");
            }
            return RedirectToAction("IndexVehicleModel");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(UpdateVehicleModelViewModel model)
        {
            var vehicleModel = await vehicleDbContext.Models.FindAsync(model.Id);
            if (vehicleModel != null)
            {
                vehicleDbContext.Models.Remove(vehicleModel);
                await vehicleDbContext.SaveChangesAsync();
                return RedirectToAction("IndexVehicleModel");
            }
            return RedirectToAction("IndexVehicleModel");
        }

    }
}

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
            var model = new CombinedViewModel();
            model.vehicleMakes = await vehicleDbContext.Makes.ToListAsync();
            model.vehicleModels = await vehicleDbContext.Models.ToListAsync();
            //var makes = await vehicleDbContext.Makes.ToListAsync();
            return View(model);
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(AddVehicleMakeViewModel addVehicleMake, AddVehicleModelViewModel addVehicleModel)
        {
            var vehicleMake = new VehicleMake()
            {
                VehicleName = addVehicleMake.VehicleName,
                VehicleAbrv = addVehicleMake.VehicleAbrv
            };
            var vehicleModel = new VehicleModel()
            {
                VehicleName = addVehicleModel.VehicleName,
                VehicleAbrv = addVehicleModel.VehicleAbrv
            };
            await vehicleDbContext.Makes.AddAsync(vehicleMake);
            await vehicleDbContext.Models.AddAsync(vehicleModel);
            await vehicleDbContext.SaveChangesAsync();
            return RedirectToAction("Add");


        }
        [HttpGet]
        public async Task<IActionResult> View(int vehicleMakeId)
        {
            var make = await vehicleDbContext.Makes.FirstOrDefaultAsync(x => x.Id == vehicleMakeId);
            if (make != null)
            {

                var viewMake = new UpdateVehicleMakeViewModel()
                {
                    Id = make.Id,
                    VehicleName = make.VehicleName,
                    VehicleAbrv = make.VehicleAbrv
                };

                return await Task.Run(() => View("View", viewMake));
            }
            return RedirectToAction("Index");
        }
    
        [HttpGet]
        public async Task<IActionResult> ViewModel(int vehicleModelId)
        {
            var model = await vehicleDbContext.Models.FirstOrDefaultAsync(x => x.Id == vehicleModelId);
            if (model != null)
            {
                var viewModel = new UpdateVehicleModelViewModel()
            {
                Id = model.Id,
                MakeId = model.Id,
                VehicleName = model.VehicleName,
                VehicleAbrv = model.VehicleAbrv
            };
            return await Task.Run(() => View("ViewModel", viewModel));
        }
        return RedirectToAction("Index");
    }

        

        [HttpPost]
            public async Task<IActionResult> View(UpdateVehicleMakeViewModel makeViewModel, UpdateVehicleModelViewModel modelViewModel)
        {
            var vehicleMake = await vehicleDbContext.Makes.FindAsync(makeViewModel.Id);
            var vehicleModel = await vehicleDbContext.Models.FindAsync(makeViewModel.Id);
               
            if (vehicleMake != null)
            {
                vehicleMake.VehicleName= makeViewModel.VehicleName;
                vehicleMake.VehicleAbrv= makeViewModel.VehicleAbrv;
                await vehicleDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            if (vehicleModel != null)
            {
                vehicleModel.VehicleName = modelViewModel.VehicleName;
                vehicleModel.VehicleAbrv = modelViewModel.VehicleAbrv;
                await vehicleDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Delete(UpdateVehicleMakeViewModel makeViewModel, UpdateVehicleModelViewModel modelViewModel)
        {
            var vehicleMake = await vehicleDbContext.Makes.FindAsync(makeViewModel.Id);
            var vehicleModel = await vehicleDbContext.Models.FindAsync(modelViewModel.Id);
            if (vehicleMake != null)
            {
                vehicleDbContext.Makes.Remove(vehicleMake);
                await vehicleDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            if (vehicleModel != null)
            {
                vehicleDbContext.Models.Remove(vehicleModel);
                await vehicleDbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

    }
}

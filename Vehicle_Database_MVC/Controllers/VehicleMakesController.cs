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
        public async Task<IActionResult> Index(
            string sortOrder,
            string searchString,
            string currentFilter,
            int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["AbrvSortParm"] = String.IsNullOrEmpty(sortOrder) ? "abrv_desc" : "";
            ViewData["IdSortParm"] = sortOrder == "Id" ? "Id_desc" : "Id";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;
            var vehicles = from v in vehicleDbContext.Makes
                           select v;

            if (!String.IsNullOrEmpty(searchString))
            {
                vehicles = vehicles.Where(v => v.VehicleName.Contains(searchString)
                                       || v.VehicleAbrv.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    vehicles = vehicles.OrderByDescending(v => v.VehicleName);
                    break;
                case "abrv_desc":
                    vehicles = vehicles.OrderByDescending(v => v.VehicleAbrv);
                    break;
                case "Id":
                    vehicles = vehicles.OrderBy(v => v.Id); 
                    break;
                case "Id_desc":
                    vehicles = vehicles.OrderByDescending(v => v.Id);
                    break;
                default:
                    vehicles = vehicles.OrderBy(v=>v.VehicleName); 
                    break;
            }
            int pageSize = 3;
                return View(await PaginatedList<VehicleMake>.CreateAsync(vehicles.AsNoTracking(), pageNumber ?? 1, pageSize));
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

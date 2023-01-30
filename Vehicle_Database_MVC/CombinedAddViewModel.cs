using Vehicle_Database_MVC.Models;
using Vehicle_Database_MVC.Models.Domain;

namespace Vehicle_Database_MVC
{
    public class CombinedAddViewModel
    {
        public IEnumerable<AddVehicleMakeViewModel> vehicleMakes { get; set; }
        public IEnumerable<AddVehicleModelViewModel> vehicleModels { get; set; }
    }
}

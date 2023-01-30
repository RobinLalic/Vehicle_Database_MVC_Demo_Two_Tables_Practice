using Vehicle_Database_MVC.Models.Domain;

namespace Vehicle_Database_MVC
{
    public class CombinedViewModel
    {

        public IEnumerable<VehicleMake> vehicleMakes{ get; set; }
        public IEnumerable<VehicleModel> vehicleModels   { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace Vehicle_Database_MVC.Models
{
    public class UpdateVehicleModelViewModel
    {
        public int Id { get; set; }

        [ForeignKey("VehicleMakeForeignKey")]
        public int MakeId { get; set; }
        public string VehicleName { get; set; }
        public string VehicleAbrv { get; set; }
    }
}

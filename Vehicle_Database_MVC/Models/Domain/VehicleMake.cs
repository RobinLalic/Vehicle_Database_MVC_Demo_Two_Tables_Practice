using System.ComponentModel.DataAnnotations;

namespace Vehicle_Database_MVC.Models.Domain
{
    public class VehicleMake
    {
        public int Id { get; set; }
        public string VehicleName { get; set; }
        public string VehicleAbrv { get; set; }
    }
}

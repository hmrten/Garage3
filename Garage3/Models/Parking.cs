using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Garage3.Models
{
    public class Parking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ParkingSlotId { get; set; }

        [Required]
        public int VehicleId { get; set; }

        [Required]
        public DateTime DateIn { get; set; }

        public DateTime? DateOut { get; set; }

        [ForeignKey("VehicleId")]
        public virtual Vehicle Vehicle { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Garage3.Models
{
    public class ParkingSlot
    {
        [Key]
        public int Id { get; set; }

        public int? ParkingId { get; set; }

        [ForeignKey("ParkingId")]
        public virtual Parking Parking { get; set; }
    }
}
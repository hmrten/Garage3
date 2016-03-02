using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Garage3.Models
{
    public class Vehicle
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(32), Index(IsUnique=true)]
        public string RegNr { get; set; }

        [Required]
        public int OwnerId { get; set; }

        [Required]
        public int VehicleTypeId { get; set; }

        [ForeignKey("VehicleTypeId")]
        public virtual VehicleType Type { get; set; }

        [ForeignKey("OwnerId")]
        public virtual Owner Owner { get; set; }
    }
}
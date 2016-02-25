using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Garage3.Models
{
    public class Owner
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(128)]
        public string Name { get; set; }

        public virtual ICollection<Vehicle> Vehicles { get; set; }

        public Owner()
        {
            Vehicles = new List<Vehicle>();
        }
    }
}
﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Garage3.Models
{
    public class VehicleType
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(64)]
        public string Name { get; set; }
    }
}
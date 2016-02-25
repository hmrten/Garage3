using Garage3.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace Garage3.DataAccess
{
    public class GarageContext : DbContext
    {
        public DbSet<Owner> Owners { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<ParkingSlot> ParkingSlots { get; set; }
        public DbSet<Parking> Parkings { get; set; }

        public GarageContext()
            : base("garage3")
        {

        }

        public void SeedTestData()
        {
            if (VehicleTypes.Count() == 0)
            {
                VehicleTypes.AddOrUpdate(t => t.Id,
                    new[] {
                    new VehicleType { Id = 1, Name = "Car" },
                    new VehicleType { Id = 2, Name = "Motorcycle" },
                    new VehicleType { Id = 3, Name = "Truck" },
                    new VehicleType { Id = 4, Name = "Bus" }
                });
            }

            if (ParkingSlots.Count() == 0)
            {
                for (int i = 0; i < 100; ++i)
                {
                    ParkingSlots.AddOrUpdate(new ParkingSlot { VehicleId = null });
                }
            }

            Owners.AddOrUpdate(o => o.Id,
                new[] {
                new Owner { Id = 1, Name = "Bob" },
                new Owner { Id = 2, Name = "John" },
                new Owner { Id = 3, Name = "Eve" }
            });

            SaveChanges();

            Vehicles.AddOrUpdate(v => v.RegNr,
                new[] {
                new Vehicle { Id = 1, OwnerId = 1, RegNr = "AAA111", VehicleTypeId = 1 },
                new Vehicle { Id = 1, OwnerId = 1, RegNr = "AAA222", VehicleTypeId = 1 },
                new Vehicle { Id = 1, OwnerId = 1, RegNr = "AAA333", VehicleTypeId = 1 },
                new Vehicle { Id = 2, OwnerId = 1, RegNr = "BBB222", VehicleTypeId = 2 },
                new Vehicle { Id = 2, OwnerId = 2, RegNr = "BBB333", VehicleTypeId = 2 },
                new Vehicle { Id = 3, OwnerId = 2, RegNr = "CCC333", VehicleTypeId = 3 },
                new Vehicle { Id = 3, OwnerId = 2, RegNr = "CCC444", VehicleTypeId = 3 },
                new Vehicle { Id = 4, OwnerId = 3, RegNr = "DDD444", VehicleTypeId = 4 },
                new Vehicle { Id = 4, OwnerId = 3, RegNr = "DDD555", VehicleTypeId = 4 }
            });

            SaveChanges();
        }
    }
}
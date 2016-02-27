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
                    ParkingSlots.AddOrUpdate(new ParkingSlot { ParkingId = null });
                }
            }

            Owners.AddOrUpdate(o => o.Id,
                new[] {
                new Owner { Id = 1, Name = "Bob" },
                new Owner { Id = 2, Name = "John" },
                new Owner { Id = 3, Name = "Eve" }
            });

            SaveChanges();

            Vehicles.AddOrUpdate(v => v.Id,
                new[] {
                new Vehicle { Id = 1, OwnerId = 1, RegNr = "AAA111", VehicleTypeId = 1 },
                new Vehicle { Id = 2, OwnerId = 1, RegNr = "AAA222", VehicleTypeId = 1 },
                new Vehicle { Id = 3, OwnerId = 1, RegNr = "AAA333", VehicleTypeId = 1 },
                new Vehicle { Id = 4, OwnerId = 1, RegNr = "BBB222", VehicleTypeId = 2 },
                new Vehicle { Id = 5, OwnerId = 2, RegNr = "BBB333", VehicleTypeId = 2 },
                new Vehicle { Id = 6, OwnerId = 2, RegNr = "CCC333", VehicleTypeId = 3 },
                new Vehicle { Id = 7, OwnerId = 2, RegNr = "CCC444", VehicleTypeId = 3 },
                new Vehicle { Id = 8, OwnerId = 3, RegNr = "DDD444", VehicleTypeId = 4 },
                new Vehicle { Id = 9, OwnerId = 3, RegNr = "DDD555", VehicleTypeId = 4 }
            });

            SaveChanges();

            if (Parkings.Count() == 0)
            {
                Parkings.AddOrUpdate(p => p.Id,
                    new[] {
                        new Parking { Id = 1, ParkingSlotId = 1, VehicleId = 1, DateIn = DateTime.Now, DateOut = null },
                        new Parking { Id = 2, ParkingSlotId = 2, VehicleId = 2, DateIn = DateTime.Now.AddDays(-1), DateOut = null },
                        new Parking { Id = 3, ParkingSlotId = 3, VehicleId = 3, DateIn = DateTime.Now.AddDays(-2), DateOut = DateTime.Now },
                        new Parking { Id = 4, ParkingSlotId = 4, VehicleId = 4, DateIn = DateTime.Now.AddDays(-4), DateOut = DateTime.Now.AddDays(-2) },
                        new Parking { Id = 5, ParkingSlotId = 4, VehicleId = 5, DateIn = DateTime.Now, DateOut = null },
                    });
                ParkingSlots.Find(1).ParkingId = 1;
				ParkingSlots.Find(2).ParkingId = 2;
				ParkingSlots.Find(4).ParkingId = 5;
                SaveChanges();
            }
        }
    }
}
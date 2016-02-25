namespace Garage3.Migrations
{
    using Garage3.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Garage3.DataAccess.GarageContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Garage3.DataAccess.GarageContext db)
        {
            if (db.VehicleTypes.Count() == 0)
            {
                db.VehicleTypes.AddOrUpdate(t => t.Id,
                    new[] {
                        new VehicleType { Id = 1, Name = "Car" },
                        new VehicleType { Id = 2, Name = "Motorcycle" },
                        new VehicleType { Id = 3, Name = "Truck" },
                        new VehicleType { Id = 4, Name = "Bus" }
                    });
            }

            db.Owners.AddOrUpdate(o => o.Id,
                new[] {
                    new Owner { Id = 1, Name = "Bob" },
                    new Owner { Id = 2, Name = "John" },
                    new Owner { Id = 3, Name = "Eve" }
                });

            db.SaveChanges();

            db.Vehicles.AddOrUpdate(v => v.RegNr,
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

            db.SaveChanges();
        }
    }
}

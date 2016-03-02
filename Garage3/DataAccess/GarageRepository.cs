﻿using Garage3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Garage3.DataAccess
{
    public class GarageRepository : IGarageRepository
    {
        private GarageContext ctx;
        private bool disposed;

        public GarageRepository(GarageContext ctx)
        {
            this.ctx = ctx;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed && disposing)
                ctx.Dispose();
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IEnumerable<Models.ParkingSlot> GetParkingSlots()
        {
            return ctx.ParkingSlots;
        }

        public IEnumerable<Models.Parking> GetParkings()
        {
            return ctx.Parkings;
        }

        public IEnumerable<Models.Owner> GetOwners()
        {
            return ctx.Owners;
        }

        public IEnumerable<Models.VehicleType> GetVehicleTypes()
        {
            return ctx.VehicleTypes;
        }

        public IEnumerable<Models.Vehicle> GetVehicles()
        {
            return ctx.Vehicles;
        }

        public IEnumerable<Models.Vehicle> GetVehiclesByType(int typeId)
        {
            return ctx.VehicleTypes.Find(typeId).Vehicles;
        }

        public IEnumerable<Models.Vehicle> GetVehiclesByOwner(int ownerId)
        {
            return ctx.Owners.Find(ownerId).Vehicles;
        }

        public IEnumerable<Vehicle> FindVehicleByRegNr(string regNr)
        {
            return ctx.Vehicles.Where(v => String.Compare(v.RegNr, regNr, true) == 0);
        }

        public bool RegNrIsParked(string regNr)
        {
            var v = from p in ctx.Parkings
                    where p.DateOut == null
                    select p.Vehicle.RegNr into reg
                    where reg.ToLower() == regNr.ToLower()
                    select reg;
            return v.Count() == 1;
        }

        public ParkResult Park(int slotId, string regNr, int? typeId, string ownerName)
        {
            // First check if a vehicle exists in the database
            var v = ctx.Vehicles.Where(x => String.Compare(x.RegNr, regNr, true) == 0).SingleOrDefault();
            if (v == null)
            {
                if (typeId == null && ownerName == null)
                    return ParkResult.NeedRegister;

                // TODO: this assumes names are unique, would probably need something better
                //       for a real application.
                var o = ctx.Owners.Where(x => String.Compare(x.Name, ownerName, true) == 0).SingleOrDefault();
                if (o == null)
                {
                    o = ctx.Owners.Add(new Owner { Name = ownerName });
                    ctx.SaveChanges();
                }

                // TODO: validate typeId
                v = ctx.Vehicles.Add(new Vehicle { RegNr = regNr, OwnerId = o.Id, VehicleTypeId = typeId.Value });
                ctx.SaveChanges();
            }

            if (RegNrIsParked(regNr))
                return ParkResult.AlreadyParked;

            // now we've set up an owner and a vehicle, so now it's time to park it.
            var p = ctx.Parkings.Add(new Parking { DateIn = DateTime.Now, DateOut = null, ParkingSlotId = slotId, VehicleId = v.Id });
            // Update the parking slot to point to the new ParkingId.
            var s = ctx.ParkingSlots.Find(slotId);
            s.ParkingId = p.Id;
            ctx.SaveChanges();

            return ParkResult.Success;
        }

        public string Unpark(int parkingId)
        {
            var p = ctx.Parkings.Find(parkingId);
            if (p == null)
                return null;
            p.DateOut = DateTime.Now;
            var s = ctx.ParkingSlots.Find(p.ParkingSlotId);
            s.ParkingId = null;
            ctx.SaveChanges();
            return p.Vehicle.RegNr;
        }
    }
}
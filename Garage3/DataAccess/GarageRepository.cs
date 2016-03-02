using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
            throw new NotImplementedException();
        }

        public IEnumerable<Models.Parking> GetParkings()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Models.Owner> GetOwners()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Models.VehicleType> GetVehicleTypes()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Models.Vehicle> GetVehicles()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Models.Vehicle> GetVehiclesByType(int typeId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Models.Vehicle> GetVehiclesByOwner(int ownerId)
        {
            throw new NotImplementedException();
        }

        public Models.Vehicle FindVehicleByRegNr(string regNr)
        {
            throw new NotImplementedException();
        }

        public bool RegNrIsParked(string regNr)
        {
            throw new NotImplementedException();
        }

        public System.Web.Mvc.HttpStatusCodeResult Park(int slotId, string regNr, int? typeId, string ownerName)
        {
            throw new NotImplementedException();
        }

        public System.Web.Mvc.HttpStatusCodeResult Unpark(int vehicleId)
        {
            throw new NotImplementedException();
        }
    }
}
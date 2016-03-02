using Garage3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Garage3.DataAccess
{
    interface IGarageRepository : IDisposable
    {
        IEnumerable<ParkingSlot> GetParkingSlots();
        IEnumerable<Parking> GetParkings();
        IEnumerable<Owner> GetOwners();
        IEnumerable<VehicleType> GetVehicleTypes();
        IEnumerable<Vehicle> GetVehicles();
        IEnumerable<Vehicle> GetVehiclesByType(int typeId);
        IEnumerable<Vehicle> GetVehiclesByOwner(int ownerId);
        Vehicle FindVehicleByRegNr(string regNr);
        bool RegNrIsParked(string regNr);
        HttpStatusCodeResult Park(int slotId, string regNr, int? typeId, string ownerName);
        HttpStatusCodeResult Unpark(int vehicleId);
    }
}

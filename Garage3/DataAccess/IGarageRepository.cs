using Garage3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Garage3.DataAccess
{
    public enum ParkResult
    {
        Success,
        AlreadyParked,
        NeedRegister
    }

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
        ParkResult Park(int slotId, string regNr, int? typeId, string ownerName);
        string Unpark(int parkingId);
    }
}

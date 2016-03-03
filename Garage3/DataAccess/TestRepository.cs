using Garage3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Garage3.DataAccess
{
    public class TestRepository : IGarageRepository
    {
        public List<Owner> Owners { get; set; }
        public List<VehicleType> VehicleTypes { get; set; }
        public List<Vehicle> Vehicles { get; set; }
        public List<ParkingSlot> ParkingSlots { get; set; }
        public List<Parking> Parkings { get; set; }

		public TestRepository()
		{
			Owners = new List<Owner>();
			VehicleTypes = new List<VehicleType>();
			Vehicles = new List<Vehicle>();
			ParkingSlots = new List<ParkingSlot>();
			Parkings = new List<Parking>();
		}

        public IEnumerable<Models.ParkingSlot> GetParkingSlots()
        {
            return ParkingSlots;
        }

        public IEnumerable<Models.Parking> GetParkings()
        {
            return Parkings;
        }

        public IEnumerable<Models.Owner> GetOwners()
        {
            return Owners;
        }

        public IEnumerable<Models.VehicleType> GetVehicleTypes()
        {
            return VehicleTypes;
        }

        public IEnumerable<Models.Vehicle> GetVehicles()
        {
            return Vehicles;
        }

        public IEnumerable<Models.Vehicle> GetVehiclesByType(int typeId)
        {
            return Vehicles.Where(v => v.VehicleTypeId == typeId);
        }

        public IEnumerable<Models.Vehicle> GetVehiclesByOwner(int ownerId)
        {
            return Vehicles.Where(v => v.Owner.Id == ownerId);
        }

        public IEnumerable<Models.Vehicle> FindVehicleByRegNr(string regNr)
        {
            return Vehicles.Where(v => String.Compare(v.RegNr, regNr, true) == 0);
        }

        public bool RegNrIsParked(string regNr)
        {
            var v = from p in Parkings
                    where p.DateOut == null
                    select p.Vehicle.RegNr into reg
                    where reg.ToLower() == regNr.ToLower()
                    select reg;
            return v.Count() == 1;
        }

        public ParkResult Park(int slotId, string regNr, int? typeId, string ownerName)
        {
            throw new NotImplementedException();
        }

        public string Unpark(int parkingId)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            
        }
    }
}
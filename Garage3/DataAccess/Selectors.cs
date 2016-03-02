using Garage3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Garage3.DataAccess
{
    public static class Selectors
    {
        // A couple helper projection functions for the Json serializer which
        // cannot deal with circular depdencies between models.
        // So we project them manually, which also serves as having
        // better control over the json object field names

        public static Func<Vehicle, object> VehicleSelector = v => new
        {
            id = v.Id,
            reg = v.RegNr,
            type = new { id = v.Type.Id, name = v.Type.Name },
            owner = new { id = v.OwnerId, name = v.Owner.Name }
        };

        public static Func<Owner, object> OwnerSelector = o => new
        {
            id = o.Id,
            name = o.Name,
        };

        public static Func<VehicleType, object> VehicleTypeSelector = t => new
        {
            id = t.Id,
            name = t.Name
        };

        public static Func<Parking, object> ParkingSelector = p => new
        {
            id = p.Id,
            slot_id = p.ParkingSlotId,
            date_in = p.DateIn,
            date_out = p.DateOut,
            vehicle = new
            {
                id = p.Vehicle.Id,
                reg = p.Vehicle.RegNr,
                type = new { id = p.Vehicle.Type.Id, name = p.Vehicle.Type.Name },
                owner = new { id = p.Vehicle.OwnerId, name = p.Vehicle.Owner.Name }
            }
        };
    }
}
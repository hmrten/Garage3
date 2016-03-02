using Garage3.DataAccess;
using Garage3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Garage3.Controllers
{
	// This is basically an API for angular to get data from

    public class VehicleController : Controller
    {
        private GarageContext db = new GarageContext();

        // A couple helper projection functions for the Json serializer which
        // cannot deal with circular depdencies between models.
        // So we project them manually, which also serves as having
        // better control over the json object field names

		private Func<Vehicle, object> VehicleSelector = v => new
		{
			id = v.Id,
			reg = v.RegNr,
			type = new { id = v.Type.Id, name = v.Type.Name },
			owner = new { id = v.OwnerId, name = v.Owner.Name }
		};

        private Func<Owner, object> OwnerSelector = o => new
        {
            id = o.Id,
            name = o.Name,
        };

        private Func<VehicleType, object> VehicleTypeSelector = t => new
        {
            id = t.Id,
            name = t.Name
        };

		// return an array of vehicles, optionally filtered by id
        public JsonResult List(int? id)
        {
            var q = db.Vehicles.AsQueryable();
            if (id != null)
                q = q.Where(v => v.Id == id);
            var vs = q.Select(VehicleSelector);
            return Json(vs.ToList(), JsonRequestBehavior.AllowGet);
        }

		// return an array of owners, optionally filtered by id
        public JsonResult Owners(int? id)
        {
            var q = db.Owners.AsQueryable();
            if (id != null)
                q = q.Where(o => o.Id == id);
            var os = q.Select(OwnerSelector);
            return Json(os.ToList(), JsonRequestBehavior.AllowGet);
        }

		// return an array of vehicle types, optionally filtered by id
		// can use this to populate drop down list
        public JsonResult Types(int? id)
        {
            var q = db.VehicleTypes.AsQueryable();
            if (id != null)
                q = q.Where(t => t.Id == id);
            var ts = q.Select(VehicleTypeSelector);
            return Json(ts.ToList(), JsonRequestBehavior.AllowGet);
        }

		// Lookup a vehicle by RegNr
        public JsonResult ByRegNr(string id)
        {
            var vs = db.Vehicles
                .Where(v => String.Compare(v.RegNr, id, true) == 0)
                .Select(VehicleSelector);
            return Json(vs.ToList(), JsonRequestBehavior.AllowGet);
        }

		// Get all vehicles with a specific type (by id)
        public JsonResult ByType(int id)
        {
            var vs = db.VehicleTypes
                .Find(id)
                .Vehicles
                .Select(VehicleSelector);
            return Json(vs.ToList(), JsonRequestBehavior.AllowGet);
        }

		// get all vehicles owner by ownerId
        public JsonResult ByOwner(int id)
        {
            var os = db.Owners
                .Where(o => o.Id == id)
                .SingleOrDefault()
                .Vehicles
                .Select(VehicleSelector);
            return Json(os.ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}
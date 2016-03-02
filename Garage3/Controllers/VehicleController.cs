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
        private IGarageRepository repo;

        public VehicleController()
        {
            repo = new GarageRepository(new GarageContext());
        }

		// return an array of vehicles, optionally filtered by id
        public JsonResult List(int? id)
        {
            var q = repo.GetVehicles();
            if (id != null)
                q = q.Where(v => v.Id == id);
            var vs = q.Select(Selectors.VehicleSelector);
            return Json(vs.ToList(), JsonRequestBehavior.AllowGet);
        }

		// return an array of owners, optionally filtered by id
        public JsonResult Owners(int? id)
        {
            var q = repo.GetOwners();
            if (id != null)
                q = q.Where(o => o.Id == id);
            var os = q.Select(Selectors.OwnerSelector);
            return Json(os.ToList(), JsonRequestBehavior.AllowGet);
        }

		// return an array of vehicle types, optionally filtered by id
		// can use this to populate drop down list
        public JsonResult Types(int? id)
        {
            var q = repo.GetVehicleTypes();
            if (id != null)
                q = q.Where(t => t.Id == id);
            var ts = q.Select(Selectors.VehicleTypeSelector);
            return Json(ts.ToList(), JsonRequestBehavior.AllowGet);
        }

		// Lookup a vehicle by RegNr
        public JsonResult ByRegNr(string id)
        {
            var vs = repo.FindVehicleByRegNr(id).Select(Selectors.VehicleSelector);
            return Json(vs.ToList(), JsonRequestBehavior.AllowGet);
        }

		// Get all vehicles with a specific type (by id)
        public JsonResult ByType(int id)
        {
            var vs = repo.GetVehiclesByType(id).Select(Selectors.VehicleSelector);
            return Json(vs.ToList(), JsonRequestBehavior.AllowGet);
        }

		// get all vehicles owner by ownerId
        public JsonResult ByOwner(int id)
        {
            var os = repo.GetVehiclesByOwner(id).Select(Selectors.VehicleSelector);
            return Json(os.ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}
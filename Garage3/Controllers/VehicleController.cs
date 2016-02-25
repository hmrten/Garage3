using Garage3.DataAccess;
using Garage3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Garage3.Controllers
{
    public class VehicleController : Controller
    {
        private GarageContext db = new GarageContext();

        // A couple helper projection functions for the Json serializer which
        // cannot deal with circular depdencies between models.
        // So we project them manually, which also serves as having
        // better control over the json object field names

        private Func<Vehicle, object> VehicleSelector = v => new
        {
            v_id = v.Id,
            v_reg = v.RegNr,
            vt_id = v.Type.Id,
            vt_name = v.Type.Name,
            o_id = v.OwnerId,
            o_name = v.Owner.Name,
        };

        private Func<Owner, object> OwnerSelector = o => new
        {
            o_id = o.Id,
            o_name = o.Name,
        };

        private Func<VehicleType, object> VehicleTypeSelector = t => new
        {
            vt_id = t.Id,
            vt_name = t.Name
        };

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Vehicles(int? id)
        {
            var q = db.Vehicles.AsQueryable();
            if (id != null)
                q = q.Where(v => v.Id == id);
            var vs = q.Select(VehicleSelector);
            return Json(vs.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Owners(int? id)
        {
            var q = db.Owners.AsQueryable();
            if (id != null)
                q = q.Where(o => o.Id == id);
            var os = q.Select(OwnerSelector);
            return Json(os.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Types(int? id)
        {
            var q = db.VehicleTypes.AsQueryable();
            if (id != null)
                q = q.Where(t => t.Id == id);
            var ts = q.Select(VehicleTypeSelector);
            return Json(ts.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ByType(int id)
        {
            var vs = db.Vehicles.Where(v => v.VehicleTypeId == id).Select(VehicleSelector);
            return Json(vs.ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}
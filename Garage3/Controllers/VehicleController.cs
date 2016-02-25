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
            //vehicles = o.Vehicles.Select(v => new { reg = v.RegNr, type = v.Type.Name })
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

        public ActionResult Vehicles()
        {
            var vs = db.Vehicles.Select(VehicleSelector);
            return Json(vs.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Owners()
        {
            var os = db.Owners.Select(OwnerSelector);
            return Json(os.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Types()
        {
            var ts = db.VehicleTypes.Select(VehicleTypeSelector);
            return Json(ts.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ByType(int id)
        {
            var vs = db.Vehicles.Where(v => v.VehicleTypeId == id).Select(VehicleSelector);
            return Json(vs.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ByOwner(int id)
        {
            var vs = db.Vehicles.Where(v => v.OwnerId == id).Select(VehicleSelector);
            return Json(vs.ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}
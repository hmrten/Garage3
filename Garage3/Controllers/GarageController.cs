using Garage3.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Garage3.Controllers
{
    public class GarageController : Controller
    {
        private GarageContext db = new GarageContext();

        public ActionResult Slots(int? id)
        {
            var q = db.ParkingSlots.AsQueryable();
            if (id != null)
                q = q.Where(s => s.Id == id);
            var slots = q.Select(s => new { slot_id = s.Id, v_id = s.VehicleId });
            return Json(q.ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}
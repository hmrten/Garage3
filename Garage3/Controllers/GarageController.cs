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
            return Json(slots.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Parkings(int? id)
        {
            var q = db.Parkings.AsQueryable();
            if (id != null)
                q = q.Where(p => p.Id == id);
            var ps = q.Select(p => new
            {
                id = p.Id,
                slot_id = p.ParkingSlotId,
                date_in = p.DateIn,
                date_out = p.DateOut,
                vehicle = new
                {
                    v_id = p.Vehicle.Id,
                    v_reg = p.Vehicle.RegNr,
                    vt_id = p.Vehicle.Type.Id,
                    vt_name = p.Vehicle.Type.Name,
                    o_id = p.Vehicle.OwnerId,
                    o_name = p.Vehicle.Owner.Name,
                }
            });
            return Json(ps.ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}
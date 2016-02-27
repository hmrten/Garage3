using Garage3.DataAccess;
using Garage3.Models;
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

		private Func<Parking, object> ParkingSelector = p => new
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

		public ActionResult Index()
		{
			ViewBag.Slots = db.ParkingSlots.OrderBy(p => p.Id);
			return View();
		}

		[HttpPost]
		public ActionResult Park(int slotId, string regNr, int? typeId, string ownerName)
		{
			var v = db.Vehicles.Where(x => String.Compare(x.RegNr, regNr, true) == 0).SingleOrDefault();
			if (v == null)
			{
				if (typeId == null && ownerName == null)
				{
					return HttpNotFound();
				}

				var o = db.Owners.Where(x => String.Compare(x.Name, ownerName, true) == 0).SingleOrDefault();
				if (o == null)
				{
					o = db.Owners.Add(new Owner { Name = ownerName });
					db.SaveChanges();
				}

				// TODO: validate typeId
				v = db.Vehicles.Add(new Vehicle { RegNr = regNr, OwnerId = o.Id, VehicleTypeId = typeId.Value });
				db.SaveChanges();
			}

			var s = db.ParkingSlots.Find(slotId);
			s.VehicleId = v.Id;

			db.Parkings.Add(new Parking { DateIn = DateTime.Now, DateOut = null, ParkingSlotId = slotId, VehicleId = v.Id });

			db.SaveChanges();

			return RedirectToAction("Index", new { owner = v.Owner.Name });
		}

        public ActionResult Slots(int? id)
        {
            var q = db.ParkingSlots.AsQueryable();
			if (id != null)
				q = q.Where(s => s.Id == id);
            var slots = q.Select(s => new { id = s.Id, v_id = s.VehicleId });
            return Json(slots.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Parkings(int? id)
        {
            var q = db.Parkings.AsQueryable();
            if (id != null)
                q = q.Where(p => p.Id == id);
			var ps = q.Select(ParkingSelector);
            return Json(ps.ToList(), JsonRequestBehavior.AllowGet);
        }

		public ActionResult ParkingBySlot(int id)
		{
			var p = db.Parkings.Where(x => x.ParkingSlotId == id).SingleOrDefault();
			if (p == null)
			{
				return HttpNotFound();
			}
			var data = ParkingSelector(p);
			return Json(data, JsonRequestBehavior.AllowGet);
		}
    }
}
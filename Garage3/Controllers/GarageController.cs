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
			return View();
		}

		public ActionResult Manage()
		{
			ViewBag.Slots = db.ParkingSlots.OrderBy(p => p.Id);
			return View();
		}

		public ActionResult History()
		{
			return View();
		}

        public ActionResult Api()
        {
            return View();
        }

		private bool IsParked(string regNr)
		{
			var v = from p in db.Parkings
					where p.DateOut == null
					select p.Vehicle.RegNr into reg
					where reg.ToLower() == regNr.ToLower()
					select reg;
			return v.Count() == 1;
		}

		[HttpPost]
		public ActionResult Park(int slotId, string regNr, int? typeId, string ownerName)
		{
			// First check if a vehicle exists in the database
			var v = db.Vehicles.Where(x => String.Compare(x.RegNr, regNr, true) == 0).SingleOrDefault();
			if (v == null)
			{
				// If not, lets try and register one
				// First time from the angular form typeId and ownerName will be null
				// so ow we should return with a 404 to indicate that
				// angular should show a form to register a new vehicle
				if (typeId == null && ownerName == null)
				{
					return HttpNotFound("no vehicle with reg nr: " + regNr + " found");
				}

				// otherwise, typeId and ownerName was filled in
				// which mean this request came from the full form
				// so now it's time to hook up a new vehicle and owner

				// Check if an owner already exists
				// TODO: this assumes names a re qunique, would probably need something better
				//       for a real application.
				var o = db.Owners.Where(x => String.Compare(x.Name, ownerName, true) == 0).SingleOrDefault();
				if (o == null)
				{
					// if no owner found with that name, time to add a new one
					o = db.Owners.Add(new Owner { Name = ownerName });
					db.SaveChanges();
				}

				// TODO: validate typeId
				// Add a new vehicle.
				v = db.Vehicles.Add(new Vehicle { RegNr = regNr, OwnerId = o.Id, VehicleTypeId = typeId.Value });
				db.SaveChanges();
			}

			if (IsParked(regNr))
			{
				return new HttpStatusCodeResult(400, regNr + " is already parked");
			}

			// now we've set up an owner and a vehicle, so now it's time to park it.

			var p = db.Parkings.Add(new Parking { DateIn = DateTime.Now, DateOut = null, ParkingSlotId = slotId, VehicleId = v.Id });

			// Update the parking slot to point to the new ParkingId.
			var s = db.ParkingSlots.Find(slotId);
			s.ParkingId = p.Id;

			db.SaveChanges();

			// all done, return to Index (angular will take care of updating the view)
			return RedirectToAction("Index");
		}

		// use a PUT method for unparking because all it involves
		// is updating ParkingSlots and Parkings with new values
		// (namely setting ParkingId to null in slots and setting DateOut in Parkings)
		[HttpPut]
		public ActionResult Unpark(int id)
		{
			var p = db.Parkings.Find(id);
			p.DateOut = DateTime.Now;
			var s = db.ParkingSlots.Find(p.ParkingSlotId);
			s.ParkingId = null;
			db.SaveChanges();
			return new HttpStatusCodeResult(200, "unparked " + p.Vehicle.RegNr);
		}

		// API: return an array of the slots in json format
        public ActionResult Slots(int? id)
        {
            var q = db.ParkingSlots.AsQueryable();
			if (id != null)
				q = q.Where(s => s.Id == id);
            var slots = q.Select(s => new { id = s.Id, p_id = s.ParkingId });
            return Json(slots.ToList(), JsonRequestBehavior.AllowGet);
        }

		// API: return an array of parkings in json format
        public ActionResult Parkings(int? id)
        {
            var q = db.Parkings.AsQueryable();
            if (id != null)
                q = q.Where(p => p.Id == id);
			var ps = q.Select(ParkingSelector);
            return Json(ps.ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}
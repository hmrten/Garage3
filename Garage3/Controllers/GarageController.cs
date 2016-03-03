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
        private IGarageRepository repo;

        public GarageController()
        {
            repo = new GarageRepository(new GarageContext());
        }

        public GarageController(IGarageRepository repo)
        {
            this.repo = repo;
        }

        public ViewResult Index()
		{
			return View();
		}

		public ViewResult Manage()
		{
            ViewBag.Slots = repo.GetParkingSlots();
			return View();
		}

        public ViewResult History()
		{
			return View();
		}

        public ViewResult Api()
        {
            return View();
        }

		[HttpPost]
        public HttpStatusCodeResult Park(int slotId, string regNr, int? typeId, string ownerName)
		{
            var res = repo.Park(slotId, regNr, typeId, ownerName);
            switch (res)
            {
                case ParkResult.Success:
                    return new HttpStatusCodeResult(200);
                case ParkResult.AlreadyParked:
                    return new HttpStatusCodeResult(400, regNr + " is already parked");
                case ParkResult.NeedRegister:
                    return HttpNotFound("no vehicle with reg nr: " + regNr + " found");
            }
            return new HttpStatusCodeResult(500, "this can't ever happen");
		}

		// use a PUT method for unparking because all it involves
		// is updating ParkingSlots and Parkings with new values
		// (namely setting ParkingId to null in slots and setting DateOut in Parkings)
		[HttpPut]
		public HttpStatusCodeResult Unpark(int id)
		{
            var reg = repo.Unpark(id);
			return new HttpStatusCodeResult(200, "unparked " + reg);
		}

		// API: return an array of the slots in json format
        public JsonResult Slots()
        {
            var slots = repo.GetParkingSlots().Select(Selectors.ParkingSlotSelector);
            return Json(slots.ToList(), JsonRequestBehavior.AllowGet);
        }

		// API: return an array of parkings in json format
        public JsonResult Parkings(int? id)
        {
            var q = repo.GetParkings();
            if (id != null)
                q = q.Where(p => p.Id == id);
			var ps = q.Select(Selectors.ParkingSelector);
            return Json(ps.ToList(), JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            repo.Dispose();
            base.Dispose(disposing);
        }
    }
}
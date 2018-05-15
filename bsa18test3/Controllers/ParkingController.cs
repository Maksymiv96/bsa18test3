using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using bsa18test3.Models;
using Microsoft.AspNetCore.Mvc;

namespace bsa18test3.Controllers
{
    [Produces("application/json")]
    //[Route("api/Parking")]
    public class ParkingController : Controller
    {

        LibraryContext db;

        public ParkingController(LibraryContext context)
        {
            db = context;
        }

        // GET: api/Parking
        [HttpGet]
        [Route("api/parking/money")]
        public ActionResult GetMoney()
        {
            return Content(Parking.gettotalearning());
        }

        // GET: api/Parking
        [HttpGet]
        [Route("api/parking/free")]
        public ActionResult GetFreeSpace()
        {
            return Content(Parking.freespaceparking());
        }

        [HttpGet]
        [Route("api/parking/total")]
        public ActionResult TotalCar()
        {
            return Content("{\"TotalCar\":" + db.Cars.Count() + "}");
        }

    }
}

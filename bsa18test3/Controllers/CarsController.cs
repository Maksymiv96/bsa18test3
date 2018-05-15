using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bsa18test3.Models;
using Microsoft.AspNetCore.Mvc;

namespace bsa18test3.Controllers
{
    [Route("api/[controller]")]
    public class CarsController : Controller
    {
        LibraryContext db;
        
        public CarsController(LibraryContext context)
        {
            this.db = context;
            if (!db.Cars.Any())
            {
                
                db.Cars.Add(new Car { Ident = "AA123AA", Balance = 23524, Type = ((CarType)1).ToString()});
                db.Cars.Add(new Car { Ident = "BC3492BC", Balance = 52423, Type = ((CarType)2).ToString() });
                db.Cars.Add(new Car { Ident = "AB43212", Balance = 35423, Type = ((CarType)3).ToString() });
                db.Cars.Add(new Car { Ident = "IA42131", Balance = 99999, Type = ((CarType)4).ToString() });

                db.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<Car> Get()
        {
            //Parking.Cars = db.Cars.ToList();
            return db.Cars.ToList();
        }

        // GET api/Cars/AA123AA
        [HttpGet("{id}")]
        public IActionResult GetByNum(string id)
        {
            Car Car = db.Cars.FirstOrDefault(x => x.Ident.ToLower() == id.ToLower());
            if (Car == null)
                return NotFound();
            return new ObjectResult(Car);
        }


        

        // POST api/Cars
        [HttpPost]
        public IActionResult Post([FromBody]Car Car)
        {
            if (Car == null)
            {
                return BadRequest();
            }
            if (db.Cars.Any(x => x.Ident.ToLower() == Car.Ident.ToLower()))
            {
                return  Content("This car is already on parking");
            }

            if (db.Cars.Count() >= Setting.Parkingspace) return Content("Parking is full, VVait a little and try a luck again");

            db.Cars.Add(Car);
            db.SaveChanges();
            return Ok(Car);
        }

        
        // DELETE api/Cars/aa123aa
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            Car Car = db.Cars.FirstOrDefault(x => x.Ident.ToLower() == id.ToLower());
            if (Car == null)
            {
                return NotFound();
            }
            if (Car.Balance < 0) return Content("This car is ovver, try refill your ballance");
            
            db.Cars.Remove(Car);
            db.SaveChanges();
            return Ok(Car);
        }
    }
}
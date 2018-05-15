using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Web;
using Microsoft.EntityFrameworkCore;
using bsa18test3.Models;

namespace bsa18test3.Controllers
{
    //[Produces("application/json")]
    //[Route("api/Transactions")]
    public class TransactionsController : Controller
    {
        LibraryContext _context;

        public TransactionsController(LibraryContext context)
        {
            _context = context;
        }

        // GET: api/Transactions
        [Route("api/Transactions")]
        [HttpGet]
        public IEnumerable<Transaction> GetTransactions()
        {
            var transaction = _context.Transactions.ToList();
            if (transaction == null)
                return null;
            List<Transaction> result = new List<Transaction>();
            foreach (var buf in transaction)
                if (timetosec(buf.DateTime) > (timetosec(DateTime.Now) - 60))
                {
                    result.Add(buf);
                }
            return result;
        }

        [Route("api/transactions/all")]
        [HttpGet]
        public IEnumerable<Transaction> GetAllTransaction()
        {
            return _context.Transactions;
        }



        // GET: api/Transactions/AA123AA
        [Route("api/Transactions/{id}")]
        [HttpGet("{id}")]
        public IActionResult GetCarTransaction([FromRoute] string ID)
        {
            var transaction = _context.Transactions.ToList().Where(m => m.IDcar.ToUpper() == ID.ToUpper());
            if (transaction == null)
                return NotFound();
            List<Transaction> result = new List<Transaction>();
            foreach (var buf in transaction)
                if (timetosec(buf.DateTime) > (timetosec(DateTime.Now) - 60))
                {
                    result.Add(buf);
                }
            return new ObjectResult(result);

        }






        // PUT: api/Transactions/5
        //[Route("api/Transactions/{ID}/{money}")]
        [Route("api/transactions/{ID}")]
        [HttpPut]
        //public async Task<IActionResult> Refill([FromBody] Car car)
        //public async Task<IActionResult> Refill(string ID, double money)
        //public IActionResult Refill(string ID, int money)
        public IActionResult Refill([FromRoute]string ID, [FromBody] int money)
        {
            if (!ModelState.IsValid)
            {
                //return BadRequest(ModelState);
                return null;
            }


           /* if (car == null)
            {
                return BadRequest();
            }*/
            if (!_context.Cars.Any(x => x.Ident == ID))
            {
                //return NotFound();
                return null;
            }
            //x => x.Ident == Car.Ident
            //int index = Parking.Cars.FindIndex(x => x.Ident.ToUpper().Equals(ID.ToUpper()));

            Car car = _context.Cars.FirstOrDefault(x => x.Ident.ToUpper().Equals(ID.ToUpper()));


            if (money <= Math.Abs(car.Balance))
            {
                Parking.Balance += Math.Abs(car.Balance);
                car.Balance += money;
            }
            else
            {
                Parking.Balance += money;
                car.Balance += money;

            }



            _context.Update(car);
            _context.SaveChanges();

            //return NoContent();
            return Ok(car);
        }

        
        [Route("api/Transactions/log")]
        [HttpGet]
        public IEnumerable<Log> GetTransactionsLog()
        {
            return Parking.readlog();
        }
       
        private bool TransactionExists(int id)
        {
            return _context.Transactions.Any(e => e.Id == id);
        }

        static int timetosec(DateTime time)
        {
            DateTime dt = TimeZoneInfo.ConvertTimeToUtc(time);
            DateTime dt2018 = new DateTime(2018, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan tsInterval = dt.Subtract(dt2018);
            return Convert.ToInt32(tsInterval.TotalSeconds);
        }

    }

}
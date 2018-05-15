using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;


namespace bsa18test3.Models
{
    public class library
    {
        public library()
        {
            Setting settings = Setting.Instance;

            Parking parking = Parking.Instance;
            var optionsBuilder = new DbContextOptionsBuilder<LibraryContext>();
            

                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=usersdbstore;Trusted_Connection=True;MultipleActiveResultSets=true");
            var context = new LibraryContext(optionsBuilder.Options);
            
            /*string path = "gebug.log";

            using (StreamWriter sw = File.AppendText(path))
            {
                
                foreach (Car car in context.Cars.ToList())
                {
                    if (car.Balance == 125) car.Balance = 130;
                    context.SaveChanges();
                    sw.WriteLine(car.Shovv());
                }
            }
            */
           


        }
    }

    enum CarType
    {
        Passenger = 1, Truck, Bus, Motorcycle
    }

     public class Transaction
     {
        public int Id { get; set; }
        DateTime _dateTime;
         string _iDcar;
         double _mOney;

         public DateTime DateTime { get => _dateTime; set => _dateTime = value; }
         public string IDcar { get => _iDcar; set => _iDcar = value; }
         public double MOney { get => _mOney; set => _mOney = value; }

        public Transaction()
        {

        }

         public Transaction(string ID, double charge)
         {
             DateTime = DateTime.Now;
             IDcar = ID;
             MOney = charge;
         }

         public string Shovv()
         {
             return ($"{DateTime} {IDcar} paid {MOney} uah");
         }
     }
     
     sealed class Setting
     {
         private static int _timeout = 3;
         private static Dictionary<string, int> _dictionary = new Dictionary<string, int>()
             {
                 { ((CarType)1).ToString(), 3},
                 { ((CarType)2).ToString(), 5},
                 { ((CarType)3).ToString(), 2},
                 { ((CarType)4).ToString(), 1},
             };
         private static int _parkingspace = 150;
         private static double _fine = 1.5;




         private static readonly Lazy<Setting> Lazy = new Lazy<Setting>(() => new Setting());
         public static Setting Instance => Lazy.Value;

         public static Setting GetInstance()
         {
             return Lazy.Value;
         }


         public static int Timeout { get => _timeout; set => _timeout = value; }
         //public static Dictionary<string, int> Dictionary { get => _dictionary; set => _dictionary = value; }
         public static void DictionarySet(string key, int value)
         {
             if (_dictionary.ContainsKey(key))
             {
                 _dictionary[key] = value;
             }
             else
             {
                 _dictionary.Add(key, value);
             }
         }

         public static int DictionaryGet(string key)
         {
             int result = 0;

             if (_dictionary.ContainsKey(key))
             {
                 result = _dictionary[key];
             }

             return result;
         }
         public static int Parkingspace { get => _parkingspace; set => _parkingspace = value; }
         public static double Fine { get => _fine; set => _fine = value; }

         

         private Setting()
         {
             
           }

           public void SetSetting()
           {
               Console.WriteLine("Initialization setting\ninput timeout (sec) (you can scip it, just press enter)");
               int timeout = Convert.ToInt32(Console.ReadLine());
               Dictionary<string, int> dictionary = new Dictionary<string, int>();
              
               for (int i = 1; i < 5; i++)
               {
                   Console.WriteLine($"Set price for {((CarType)i).ToString()}");
                   int tempprice = Convert.ToInt32(Console.ReadLine());
                   dictionary.Add(((CarType)i).ToString(), tempprice);
               }
               Console.WriteLine("Inpt size of parking (int)");
               int parkingspace = (Convert.ToInt32(Console.ReadLine()));
               Console.WriteLine("Set fine coef (double x,x)");
               Double fine = Convert.ToDouble(Console.ReadLine());
               _timeout = timeout;
               _dictionary = dictionary;
               _parkingspace = parkingspace;
               _fine = fine;

           }

           public void SetSetting(Dictionary<string, int> dict, int timeout = 3, int parkingspace = 150, double fine = 1.5)
           {
               _dictionary = dict;
               _timeout = timeout;
               _parkingspace = parkingspace;
               _fine = fine;

           }
       }

       class Parking
       {
           private static List<Transaction> transactions = new List<Transaction>();
          // private static List<Car> cars = new List<Car>();

           internal static List<Transaction> Transactions { get => transactions; set => transactions = value; }
           //internal static List<Car> Cars { get => cars; set => cars = value; }

           private static double balance = 0;
           private static double startbalance = 0;

           public static double Balance { get => balance; set => balance = value; }
           public static double Startbalance { get => startbalance; }

           private static TimerCallback tm = new TimerCallback(takecharge);
           private Timer _timer = new Timer(tm, null, 0, Setting.Timeout * 1000);

           private static TimerCallback tm2 = new TimerCallback(creatinglog);
           private Timer _timer2 = new Timer(tm2, null, 0, 60 * 1000);

           private Parking()
           {

           }

           private static readonly Lazy<Parking> Lazy = new Lazy<Parking>(() => new Parking());
           public static Parking Instance => Lazy.Value;

            
           private static void takecharge(object obj)
           {
            var optionsBuilder = new DbContextOptionsBuilder<LibraryContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=usersdbstore;Trusted_Connection=True;MultipleActiveResultSets=true");
            using (var context = new LibraryContext(optionsBuilder.Options))
            {
                foreach (Car car in context.Cars.ToList())
                {
                    if (car.Balance >= Setting.DictionaryGet(car.Type))
                    {
                        Balance += Setting.DictionaryGet(car.Type);
                        car.Balance -= Setting.DictionaryGet(car.Type);
                    }
                    else if (car.Balance > 0)
                    {
                        Balance += car.Balance;
                        car.Balance -= Setting.DictionaryGet(car.Type) * Setting.Fine;
                    }
                    else if (car.Balance <= 0)
                    {
                        car.Balance -= Setting.DictionaryGet(car.Type) * Setting.Fine;
                    }
                    Transactions.Add(new Transaction(car.Ident, Setting.DictionaryGet(car.Type)));

                    context.Transactions.Add(new Transaction(car.Ident, Setting.DictionaryGet(car.Type)));
                    context.Cars.Update(car);
                    context.SaveChanges();
                }
            }
           }


           public void shovvlog()
           {
               foreach (Transaction trans in Transactions)
               {
                   if (timetosec(trans.DateTime) > (timetosec(DateTime.Now) - 60))
                   {
                       Console.WriteLine(trans.Shovv());
                   }
               }
           }
           public void lastminuteearn()
           {
               double sum = 0;
               foreach (Transaction trans in Transactions)
               {
                   if (timetosec(trans.DateTime) > (timetosec(DateTime.Now) - 60))
                   {
                       sum += trans.MOney;
                   }

               }
               Console.WriteLine($"total earnd in last minute: {sum}");
           }

        
        private static void creatinglog(object obj)
        {
            double sum = 0;
            
            foreach (Transaction trans in Transactions.ToList())
            {
                if (timetosec(trans.DateTime) > (timetosec(DateTime.Now) - 60))
                {
                    sum += trans.MOney;
                }

            }
            string path = "transaction.log";

            Log lg = new Log();
            lg.Time = DateTime.Now; lg.Lastminuteearn = sum;

            string json = JsonConvert.SerializeObject(lg);

            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(json);
            }
        }

        static public List<Log> readlog()
        {
            string path = "transaction.log";
            List<Log>  result = new List<Log>();
            using (StreamReader fs = new StreamReader(path))
            {
                while (true)
                {
                    
                    string temp = fs.ReadLine();             
                    if (temp == null) break;
                    result.Add(JsonConvert.DeserializeObject<Log>(temp));
                }
            }

            return result;

        }

       

       static int timetosec(DateTime time)
       {
           DateTime dt = TimeZoneInfo.ConvertTimeToUtc(time);
           DateTime dt2018 = new DateTime(2018, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
           TimeSpan tsInterval = dt.Subtract(dt2018);
           return Convert.ToInt32(tsInterval.TotalSeconds);
       }

        public static string freespaceparking()
        {   
            var optionsBuilder = new DbContextOptionsBuilder<LibraryContext>();
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=usersdbstore;Trusted_Connection=True;MultipleActiveResultSets=true");
            using (var context = new LibraryContext(optionsBuilder.Options))
            {
                return ("{\"freespace\":" + (Setting.Parkingspace - context.Cars.Count())) + "}";
            }             
        }

       public static string gettotalearning()
       {

            return ("{\"totalearn\":" + (balance - startbalance)+"}");

       }
        

   }

    
    public class Log
    {
        public DateTime Time;
        public double Lastminuteearn;
        public Log() { }
    }


    public class Car
    {
        public int Id { get; set; }
        private string _ident = "";//probably can be car number
        private double _balance;
        private string _type;

        public string Ident { get => _ident; set => _ident = value; }
        public double Balance { get => _balance; set => _balance = value; }
        public string Type { get => _type; set => _type = value; }

       

        public string Shovv()
        {
            return ($"{_type} vvith ID {_ident} has balance {_balance}");
        }
    }
}



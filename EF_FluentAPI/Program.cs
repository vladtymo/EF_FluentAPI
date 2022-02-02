using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

namespace EF_FluentAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            AirlinesDbContext context = new AirlinesDbContext();

            // .Include() - eager loading
            var query = context.Airplanes.Include(a => a.Flights).ThenInclude(f => f.DepartureCity)
                                         .Include(a => a.Flights).ThenInclude(f => f.ArrivalCity);

            foreach (var aiplane in query)
            {
                Console.WriteLine(new string('_', 30));
                Console.WriteLine($"Airplane [{aiplane.Id}]: {aiplane.Model} has {aiplane.Flights.Count} flight");

                foreach (var f in aiplane.Flights)
                {
                    Console.WriteLine($"\tFlight #{f.Number} {f.Date.ToLongDateString()} / {f.DepartureCity?.Name}...{f.ArrivalCity?.Name}");
                }
            }
        }
    }
}

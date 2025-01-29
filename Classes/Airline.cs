using PRG2_T13_03.Classes.Flights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_T13_03.Classes
{
    class Airline
    {
        // Properties
        public string Name { get; set; } = "";
        public string Code { get; set; } = "";
        public Dictionary<string, Flight> Flights { get; set; } = new Dictionary<string, Flight>();


        // Methods
        public bool AddFlight(Flight flight)// PLACEHOLDER
        {
            return Flights.TryAdd(flight.FlightNumber, flight);
        }

        public double CalculateFees()// PLACEHOLDER
        {
            double fees = Flights.Sum(x => x.Value.CalculateFees());

            int noOfFlights = Flights.Count;

            // Apply 3% discount if more than 5 flights
            if (noOfFlights > 5) fees *= .97;

            //{
            //    foreach (KeyValuePair<string, Flight> kvp in Flights)
            //    {
            //        fees += kvp.Value.CalculateFees();
            //    }
            //    fees *= 0.97;
            //}

            // Calculate the other fees
            foreach (KeyValuePair<string, Flight> kvp in Flights)
            {
                Flight flight = kvp.Value;

                // For flights arriving before 11 am or after 9 pm
                int hour = flight.ExpectedTime.Hour;
                if (hour < 11 || hour > 21) fees -= 110;

                // For flights with these origins:
                // Dubai(DXB), Bangkok(BKK) or Tokyo(NRT)
                if (flight.Origin == "Dubai (DXB)" 
                    || flight.Origin == "Bangkok (BKK)"
                    || flight.Origin == "Tokyo (NRT)"
                    ) fees -= 25;

                // For flights not indicating any special request codes
                if (flight is NORMFlight) fees -= 50;
            }

            // For every 3 flights arriving/ departing, airlines will receive a discount
            fees -= ((int)(noOfFlights / 3)) * 350;

            return (fees < 0) ? 0 : fees; // Makes sure the fees don't go below 0
        }

        public bool RemoveFlight(Flight flight)
        {
            return Flights.Remove(flight.FlightNumber);
        }

        public override string ToString()
        {
            return $"Name: {Name}\tCode: {Code}";
        }


        // Constructors -> Done
        public Airline() { } // default
        public Airline(string name, string code) // parameterized
        {
            Name = name;
            Code = code;
        }
    }
}

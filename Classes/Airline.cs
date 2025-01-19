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
            double fees = 0;

            int noOfFlights = Flights.Count;

            // Initiate base fee first IF NO OF FLIGHTS > 5 to apply 3% discount if necessary
            if (noOfFlights > 5)
            {
                foreach (KeyValuePair<string, Flight> kvp in Flights)
                {
                    fees += kvp.Value.CalculateFees();
                }
                fees *= 0.97;
            }

            foreach (KeyValuePair<string, Flight> kvp in Flights)
            {
                Flight flight = kvp.Value;

                // Calculate and add to fees IF NO OF FLIGHTS <= 5
                // (to not overstate the actual value)
                if (noOfFlights <= 5) fees += flight.CalculateFees();

                // For flights arriving before 11 am or after 9 pm
                int hours = flight.ExpectedTime.TimeOfDay.Hours;
                if (hours < 11 || hours > 21) fees -= 110;

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
            fees -= ((int)noOfFlights % 3) * 350;

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

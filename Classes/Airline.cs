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
            bool specialRequestFound = false;
            foreach (KeyValuePair<string, Flight> kvp in Flights)
            {
                Flight flight = kvp.Value;

                // Calculate and add to fees
                fees += flight.CalculateFees();

                // For flights arriving before 11 am or after 9 pm
                int hours = flight.ExpectedTime.TimeOfDay.Hours;
                if (hours < 11 || hours > 21) fees -= 110;

                // For flights with these origins:
                // Dubai(DXB), Bangkok(BKK) or Tokyo(NRT)
                if (flight.Origin == "Dubai (DXB)" 
                    || flight.Origin == "Bangkok (BKK)"
                    || flight.Origin == "Tokyo (NRT)"
                    ) fees -= 25;

                // To find if there is any special request
                if (specialRequestFound) continue;
                if (flight is CFFTFlight
                    || flight is DDJBFlight
                    || flight is LWTTFlight
                    ) specialRequestFound = true;
            }

            int noOfFlights = Flights.Count;

            // For every 3 flights arriving/ departing, airlines will receive a discount
            fees -= ((int)noOfFlights % 3) * 350;

            // For not indicating any special request codes
            if (specialRequestFound) fees -= 50;

            // For more than 5 flights arriving/departing, airlines receive an additional discount off the total bill
            fees *= (noOfFlights > 5) ? 0.97 : 1;

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


        // Constructor
        public Airline() { }
        public Airline(string name, string code)
        {
            Name = name;
            Code = code;
        }
    }
}

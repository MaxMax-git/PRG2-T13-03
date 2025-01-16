using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_T13_03
{
    internal class Airline
    {
        public string Name { get; set; } = "";
        public string Code { get; set; } = "";
        public Dictionary<string, Flight> Flights { get; set; } = new Dictionary<string, Flight>();
        
        public bool AddFlight(Flight flight)// PLACEHOLDER
        {
            if (Flights.ContainsKey(flight.FlightNumber)) return false;
            Flights.Add(flight.FlightNumber, flight);
            return true;
        }

        public double CalculateFees()// PLACEHOLDER
        {
            return 0.0;
        }

        public bool RemoveFlight(Flight flight)
        {
            return Flights.Remove(flight.FlightNumber);
        }

        public override string ToString()
        {
            return $"Name: {Name}\tCode: {Code}";
        }

        public Airline() { }
        public Airline(string name, string code)
        {
            Name = name ;
            Code = code ;
        }
    }
}

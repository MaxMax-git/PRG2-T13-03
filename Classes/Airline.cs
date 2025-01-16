using PRG2_T13_03;
using PRG2_T13_03.Classes;
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


        // Constructor
        public Airline() { }
        public Airline(string name, string code)
        {
            Name = name;
            Code = code;
        }
    }
}

using PRG2_T13_03;
using PRG2_T13_03.Classes;
using PRG2_T13_03.Classes.Flights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_T13_03.Classes.Flights
{
    class LWTTFlight : Flight
    {
        // Properties
        public double RequestFee { get; set; } = 500;


        // Methods
        public override double CalculateFees()
        {
            return base.CalculateFees() + RequestFee;
        }

        public override string ToString()
        {
            return base.ToString() + $"\tRequest Fee: {RequestFee}";
        }


        // Constructor
        public LWTTFlight() { }
        public LWTTFlight(string flightNumber, string origin, string destination, DateTime expectedTime) :
            base(flightNumber, origin, destination, expectedTime)
        { }
    }
}

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
            // Boarding Gates Base Fee
            double fee = 300 + RequestFee;

            // Destination/Origin in Singapore
            fee += (Origin == "Singapore (SIN)") ? 800 : 0;
            fee += (Destination == "Singapore (SIN)") ? 500 : 0;

            return fee;
        }

        public override string ToString()
        {
            return base.ToString() + $"\tRequest Fee: {RequestFee}";
        }


        // Constructor
        public LWTTFlight() { }
        public LWTTFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status) :
            base(flightNumber, origin, destination, expectedTime, status)
        {
            //
        }
    }
}

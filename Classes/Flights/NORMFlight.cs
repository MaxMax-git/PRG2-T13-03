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
    class NORMFlight : Flight
    {
        // PROPERTIES -> Don't have. 


        // Methods
        public override double CalculateFees()
        {
            // Boarding Gates Base Fee
            double fee = 300;

            // Destination/Origin in Singapore
            fee += (Origin == "Singapore (SIN)") ? 800 : 0;
            fee += (Destination == "Singapore (SIN)") ? 500 : 0;
            
            return fee;
        }

        public override string ToString()
        {
            return base.ToString();
        }


        // Constructor
        public NORMFlight() { }
        public NORMFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status) :
            base(flightNumber, origin, destination, expectedTime, status)
        {
            //
        }
    }
}

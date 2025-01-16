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
    class CFFTFlight : Flight
    {
        // Properties
        public double RequestFee { get; set; } = 150;


        // Methods
        public override double CalculateFees()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return base.ToString();
        }


        // Constructor
        public CFFTFlight() { }
        public CFFTFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status) :
            base(flightNumber, origin, destination, expectedTime, status)
        { }
    }
}

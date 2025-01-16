using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_T13_03
{
    internal class LWTTFlight : Flight
    {
        public double RequestFee { get; set; }

        public override double CalculateFees()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public LWTTFlight() { }
        public LWTTFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status, double requestFee): 
            base(flightNumber, origin, destination, expectedTime, status)
        {
           RequestFee = requestFee;
        }
    }
}

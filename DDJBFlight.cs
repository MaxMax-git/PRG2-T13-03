﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_T13_03
{
    internal class DDJBFlight : Flight
    {
        public double RequestFee { get; set; } = 300;

        public override double CalculateFees()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public DDJBFlight() { }
        public DDJBFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status): 
            base(flightNumber, origin, destination, expectedTime, status) {}
    }
}

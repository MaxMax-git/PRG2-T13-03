using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_T13_03.Classes
{
    internal abstract class Flight
    {
        public string FlightNumber { get; set; } = "";
        public string Origin { get; set; } = "";
        public string Destination { get; set; } = "";
        public DateTime ExpectedTime { get; set; }
        public string Status { get; set; } = "";

        public abstract double CalculateFees();
        public override string ToString()
        {
            return $"Flight Number: {FlightNumber}\tOrigin: {Origin}\tDestination: {Destination}\tExpectedTime: {ExpectedTime}\tStatus: {Status}";
        }

        public Flight() { }
        public Flight(string flightNumber, string origin, string destination, DateTime expectedTime, string status)
        {
            FlightNumber = flightNumber;
            Origin = origin;
            Destination = destination;
            ExpectedTime = expectedTime;
            Status = status;
        }
    }
}

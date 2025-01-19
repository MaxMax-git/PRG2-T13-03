using PRG2_T13_03;
using PRG2_T13_03.Classes;
using PRG2_T13_03.Classes.Flights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_T13_03.Classes
{
    abstract class Flight
    {
        // Properties
        public string FlightNumber { get; set; } = "";
        public string Origin { get; set; } = "";
        public string Destination { get; set; } = "";
        public DateTime ExpectedTime { get; set; }
        public string Status { get; set; } = "On Time"; // Set to this UNLESS otherwise specified


        // Methods

        // Base implementation of calculate fees, will be used for flight subclasses
        public virtual double CalculateFees() // virtual -> override in its child classes
        {
            double fee = 300;

            // Destination/Origin in Singapore
            fee += (Origin == "Singapore (SIN)") ? 800 : 0;
            fee += (Destination == "Singapore (SIN)") ? 500 : 0;

            return fee;
        }
        public override string ToString()
        {
            return $"Flight Number: {FlightNumber}\tOrigin: {Origin}\tDestination: {Destination}\tExpectedTime: {ExpectedTime}\tStatus: {Status}";
        }


        // Constructor
        public Flight() { }
        public Flight(string flightNumber, string origin, string destination, DateTime expectedTime)
        {
            FlightNumber = flightNumber;
            Origin = origin;
            Destination = destination;
            ExpectedTime = expectedTime;
            //Status = status;
        }
    }
}

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
    class BoardingGate
    {
        // Properties
        public string GateName { get; set; } = "";
        public bool SupportsCFFT { get; set; }
        public bool SupportsDDJB { get; set; }
        public bool SupportsLWTT { get; set; }
        public Flight? Flight { get; set; }

        // Methods
        public double CalculateFees()
        {
            return Flight.CalculateFees();
        }

        public override string ToString()   
        {
            return $"GateName: {GateName}\tSupports CFFT: {SupportsCFFT}\tSupports DDJB: {SupportsDDJB}\tSupports LWTT: {SupportsLWTT}";
        }

        // Constructor
        public BoardingGate() // default constructor
        {
            Flight = new NORMFlight();
        }

        public BoardingGate(string gateName, bool supportsCFFT, bool supportDDJB, bool supportsLWTT) // parameterized consturctor
        {
            GateName = gateName;
            SupportsCFFT = supportsCFFT;
            SupportsDDJB = supportDDJB;
            SupportsLWTT = supportsLWTT;
        }
    }
}
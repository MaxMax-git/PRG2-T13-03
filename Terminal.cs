using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Terminal
{
    // Properties
    public string TerminalName { get; set; }
    public Dictionary<string, Airline> Airlines { get; set; }
    public Dictionary<string, Flight> Flights { get; set; }
    public Dictionary<string, BoardingGate> BoardingGates { get; set; }
    public Dictionary<string, double> GateFees { get; set; }


    // Constructor
    public Terminal() { }
    public Terminal()
    {
        //
    }

    // Methods
    public bool AddAirline(Airline airline)
    {
        //
    }

    public bool AddBoardingGate(BoardingGate boardingGate)
    {
        //
    }

    public Airline GetAirlineFromFlight(Flight flight)
    {
        //
    }

    public void PrintAirlineFees()
    {
        // 
    }

    public override string ToString()
    {
        //
    }
}


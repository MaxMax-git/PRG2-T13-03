using PRG2_T13_03.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;


class Terminal
{
    // Properties
    public string TerminalName { get; set; } = "";
    public Dictionary<string, Airline> Airlines { get; set; } = new Dictionary<string, Airline>();
    public Dictionary<string, Flight> Flights { get; set; } = new Dictionary<string, Flight>();
    public Dictionary<string, BoardingGate> BoardingGates { get; set; } = new Dictionary<string, BoardingGate>();
    public Dictionary<string, double> GateFees { get; set; } = new Dictionary<string, double>();


    // Constructor
    public Terminal() { }
    public Terminal(string terminalName)
    {
        TerminalName = terminalName;
    }

    // Methods
    public bool AddAirline(Airline airline)
    {
        return Airlines.TryAdd(airline.Code, airline);
    }

    public bool AddBoardingGate(BoardingGate boardingGate)
    {
        return BoardingGates.TryAdd(boardingGate.GateName, boardingGate);
    }

    public Airline GetAirlineFromFlight(Flight flight)
    {
        foreach (KeyValuePair<string, Airline> kvp in Airlines)
        {
            if (kvp.Value.Flights.ContainsKey(flight.FlightNumber))
            {
                return kvp.Value;
            }
        }

        // Throws an error is there is somehow a flight without a airline
        throw new Exception("Passed flight does not have a airline!");
    }

    public void PrintAirlineFees()
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        return $"Terminal Name: {TerminalName}";
    }
}


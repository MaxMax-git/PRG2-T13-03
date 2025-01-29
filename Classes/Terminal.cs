using PRG2_T13_03;
using PRG2_T13_03.Classes;
using PRG2_T13_03.Classes.Flights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_T13_03.Classes
{
    class Terminal
    {
        // Properties & dictionaries initialised at properties.
        public string TerminalName { get; set; } = "";
        public Dictionary<string, Airline> Airlines { get; set; } = new Dictionary<string, Airline>();
        public Dictionary<string, Flight> Flights { get; set; } = new Dictionary<string, Flight>();
        public Dictionary<string, BoardingGate> BoardingGates { get; set; } = new Dictionary<string, BoardingGate>();
        public Dictionary<string, double> GateFees { get; set; } = new Dictionary<string, double>();


        // Methods
        public bool AddAirline(Airline airline)
        {
            // If the airline already exists, return False
            // else, airline does not exists, add Airline to Dict & return True
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
            double totalBaseFees = 0;
            double totalDiscounts = 0;

            string format = "  {0,-25}: {1:C2}";
            foreach (Airline airline in Airlines.Values)
            {
                double baseFees = airline.Flights.Sum(x => (x.Value.CalculateFees()));
                double finalFees = airline.CalculateFees();
                double discounts = baseFees - finalFees;

                // Add to the total
                totalBaseFees += baseFees;
                totalDiscounts += discounts;

                Console.WriteLine($"Fees for {airline.Name} ({airline.Code}):");
                Console.WriteLine(format, "Subtotal of fees", baseFees);
                Console.WriteLine(format, "Subtotal of discounts", discounts);
                Console.WriteLine($"  Total final fees = {baseFees:C2} - {discounts:C2}");
                Console.WriteLine($"                   = {finalFees:C2}");
                Console.WriteLine();
            }

            format = "{0,-30}: {1:C2}";
            
            Console.WriteLine(format, "Final subtotal of fees", totalBaseFees);
            Console.WriteLine(format, "Final subtotal of discounts", totalDiscounts);
            Console.WriteLine(format, "Final total", totalBaseFees - totalDiscounts);
            Console.WriteLine("{0,-30}: {1:P1}", "Percentage Discounted", totalDiscounts/totalBaseFees);
        }

        public override string ToString()
        {
            return $"Terminal Name: {TerminalName}";
        }

        // Constructor, ->Done
        public Terminal() { } // default
        public Terminal(string terminalName) // parameterized
        {
            TerminalName = terminalName; 
        }
    }
}
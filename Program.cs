using PRG2_T13_03;
using PRG2_T13_03.Classes;
using PRG2_T13_03.Classes.Flights;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;

class Program
{
    // Settings
    private static string dataFolderName = "Data/";
    private static string airlineFileName = dataFolderName + "airlines.csv";
    private static string boardingGateFileName = dataFolderName + "boardinggates.csv";
    private static string flightsFileName = dataFolderName + "flights.csv";

    // Values
    private static Terminal t5 = new Terminal("Terminal5");

    private static Dictionary<string, Airline> airlineDict = new Dictionary<string, Airline>();
    private static Dictionary<string, BoardingGate> boardingGateDict = new Dictionary<string, BoardingGate>();
    private static Dictionary<string, Flight> flightDict = new Dictionary<string, Flight>();

    // Utility

    // ValidateInputFrom( ICollection<string> validInputs, string? msg, string? err )
    //  Prompts user for input, and checks if said input is within valid inputs
    //      Done through: validInputs.Contains()
    //
    //  Returns the user's input if valid
    //
    //  Optional message/error can be added
    //
    //  Basic usage:
    //      string input = ValidateInputFrom(new string[] { "yes", "no" }, "Do you like apples? ")
    //      Output: "Do you like apples? " -- user inputs yes or no
    //
    // Note:
    //  Inputs are trimmed and turned to lowercase
    //  Messages/Errors uses Console.WriteLine()
    private static string ValidateInputFrom(ICollection<string> validInputs, string msg = "Please select your option:", string err = "Invalid option.")
    {
        string? input = null;
        while (input == null || !validInputs.Contains(input))
        {
            Console.WriteLine(msg);
            input = Console.ReadLine();
            input = (input != null) ? input.Trim().ToLower() : input;
            if (input == null || !validInputs.Contains(input)) Console.WriteLine(err);
        }
        return input;
    }

    // PART 1 //

    // pt1. -> Load the airlines.csv file.
    private static void LoadAirlines()
    {
        // Header
        Console.WriteLine("Loading Airlines...");

        // Airlines loaded count
        int count = 0; 

        // use StreamReader to read the datafile
        using (StreamReader sr = new StreamReader(airlineFileName)) // reads the Airlines file.csv
        {
            sr.ReadLine(); // ignore the header, don't need it
            string? line; // initialise string line to use for ltr

            // Reads the file line by line, until end of file is reached
            while ((line = sr.ReadLine()) != null)
            {
                // details[0] = Airline Name
                // details[1] = Airline Code
                string[] details = line.Split(',');

                // Create the Airline objects based on the data loaded.
                Airline airline = new Airline(details[0], details[1]); // new Airline(name, code)

                // Add the Airlines objects into an Airline Dictionary, under Terminal
                if (t5.AddAirline(airline))
                {
                    airlineDict.Add(details[1], airline);
                    count++; // Increment by 1
                }
            }

            // Display the number of loaded Airlines
            Console.WriteLine($"{count} Airlines Loaded!");
        }
    }

    // pt1. -> Load the boardinggates.csv file
    private static void LoadBoardingGates()
    {
        // Header
        Console.WriteLine("Loading Boarding Gates...");

        // Boarding Gates loaded count
        int count = 0;

        // use StreamReader to read the datafile
        using (StreamReader sr = new StreamReader(boardingGateFileName)) // reads the Boarding Gates file.csv
        {
            sr.ReadLine(); // ignore the header
            string? line; // initialise string line to use for ltr

            // Reads the file line by line, until end of file is reached
            while ((line = sr.ReadLine()) != null)
            {
                // details[0] = Boarding Gate Name || details[1] = DDJB
                // details[2] = CFFT || details[3] = LWTT
                string[] details = line.Split(',');
                // For increased clarity 
                bool DDJB = Convert.ToBoolean(details[1]);
                bool CFFT = Convert.ToBoolean(details[2]);
                bool LWTT = Convert.ToBoolean(details[3]);

                // Create the Boarding Gate objects based on the data loaded
                BoardingGate boardingGate = new BoardingGate(details[0], CFFT, DDJB, LWTT); // new BoardingGate(name, CFFT, DDJB, LWTT, )

                // Add the Boarding Gate objects into a Boarding Gate Dictionary
                if (t5.AddBoardingGate(boardingGate))
                {
                    boardingGateDict.Add(details[0], boardingGate);
                    count++; // Increment by 1
                }
            }

            // Display the number of loaded Boarding Gates
            Console.WriteLine($"{count} Boarding Gates Loaded!");
        }
    }

    // PART 2 //

    // pt2. -> Load flights.csv file
    private static void LoadFlights()
    {
        // Header
        Console.WriteLine("Loading Flights...");

        // Flights loaded count
        int count = 0;

        // Constructor dictionary reference
        Dictionary<string, Func<string, string, string, DateTime, Flight>> flightConstructors =
             new Dictionary<string, Func<string, string, string, DateTime, Flight>>
         {
            { "CFFT", (a,b,c,d) => new CFFTFlight(a,b,c,d) },
            { "DDJB", (a,b,c,d) => new DDJBFlight(a,b,c,d) },
            { "LWTT", (a,b,c,d) => new LWTTFlight(a,b,c,d) },
            { "", (a,b,c,d) => new NORMFlight(a,b,c,d) },
         };

        // use StreamReader to read the datafile
        using (StreamReader sr = new StreamReader(flightsFileName)) // reads the Boarding Gates file.csv
        {
            sr.ReadLine(); // ignore the header
            string? line; // initialise string line to use for ltr

            // Reads the file line by line, until end of file is reached
            while ((line = sr.ReadLine()) != null)
            {
                // details[0] = Flight Number
                // details[1] = Origin
                // details[2] = Destination
                // details[3] = Expected Departure/Arrival
                // details[4] = Special Request Code
                string[] details = line.Split(',');

                // For increased clarity
                string airlineCode = details[0].Substring(0,2);
                DateTime expectedTime = Convert.ToDateTime(details[3]);
                string specialRequest = details[4].Trim(); // to account for whitespace

                // Check if airline even exists
                if (!t5.Airlines.ContainsKey(airlineCode)) continue;
                // Check if special request exists
                if (!flightConstructors.ContainsKey(specialRequest)) continue;

                // Create the flight
                Flight flight = flightConstructors[specialRequest](
                    details[0],
                    details[1],
                    details[2],
                    expectedTime);

                // Add the flight objects into their respective airlines
                if (t5.Airlines[airlineCode].AddFlight(flight)) 
                {
                    flightDict.Add(details[0], flight);
                    count++;
                };
            }

            // Display the number of loaded Boarding Gates
            Console.WriteLine($"{count} Flights Loaded!");
        }
    }

    // PART 3 //

    // Option dictionary reference
    private static Dictionary<string, Action> options = new Dictionary<string, Action>
    {
        {"1", () => ListAllFlights()},
         {"2", () => ListBoardingGates()},
        // {"3", () => AssignFlightToBoardingGate()},
        // {"4", () => CreateFlight()},
        // {"5", () => DisplayAirlineFlights()},
        // {"6", () => ModifyFlightDetails()},
        // {"7", () => DisplayFlightSchedule()},
        {"0", () => {
            Console.Write("Goodbye!");
            Environment.Exit(0);
        } }, // Closes the program
    };

    // pt3. -> Main Menu

    // ShowMenu()
    //  Shows the menu and prompts the user for their option
    private static string ShowMenu()
    {
        Console.WriteLine(
            "=============================================              \r\n" +
            "Welcome to Changi Airport Terminal 5                       \r\n" +
            "=============================================              \r\n" +
            "1. List All Flights                                        \r\n" +
            "2. List Boarding Gates                                     \r\n" +
            "3. Assign a Boarding Gate to a Flight                      \r\n" +
            "4. Create Flight                                           \r\n" +
            "5. Display Airline Flights                                 \r\n" +
            "6. Modify Flight Details                                   \r\n" +
            "7. Display Flight Schedule                                 \r\n" +
            "0. Exit                                                    \r\n");
        return ValidateInputFrom(options.Keys);
    }

    private static void ListAllFlights()
    {
        // Header
        Console.WriteLine(
            "=============================================\r\n" +
            "List of Flights for Changi Airport Terminal 5\r\n" +
            "=============================================");

        string format = "{0,-16}{1,-23}{2,-23}{3,-23}{4}";
        Console.WriteLine(format, "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time");

        foreach (KeyValuePair<string, Flight> kvp in flightDict)
        {
            Flight flight = kvp.Value;
            Console.WriteLine(format,
                flight.FlightNumber,
                airlineDict[flight.FlightNumber.Substring(0, 2)].Name, // Gets the name from the Airline object
                flight.Origin,
                flight.Destination,
                flight.ExpectedTime
            );
        }
        Console.WriteLine();
    }


    // PART 4 //
    // List all the boarding gates
    private static void ListBoardingGates()
    {
        // Header
        Console.WriteLine(
            "=============================================\r\n" +
            "List of Boarding Gates for Changi Airport Terminal 5\r\n" +
            "=============================================");

        string format = "{0,-16}{1,-23}{2,-23}{3}";
        Console.WriteLine(format, "Gate Name", "DDJB", "CFFT", "LWTT");

        foreach (KeyValuePair<string, BoardingGate> kvp in boardingGateDict)
        {
            BoardingGate boardingGate = kvp.Value;
            Console.WriteLine(format, 
                boardingGate.GateName,
                boardingGate.SupportsDDJB,
                boardingGate.SupportsCFFT,
                boardingGate.SupportsLWTT
            );
        }
        Console.WriteLine();
    }

    static void Main(string[] args)
    {
        // Load the Airlines
        LoadAirlines();

        // Load the Boarding Gates
        LoadBoardingGates();

        // Load flights
        LoadFlights();

        // Program start
        Console.Write("\n\n\n\n");
        while (true)
        {
            string option = ShowMenu();
            options[option]();
        }
    }
}
using PRG2_T13_03;
using PRG2_T13_03.Classes;
using PRG2_T13_03.Classes.Flights;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Net.Http.Headers;

// Add attribute boarding gate to flight class

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
    //  Inputs are trimmed
    //  Messages/Errors uses Console.WriteLine()
    private static string ValidateInputFrom(ICollection<string> validInputs, string msg = "Please select your option:", string err = "Invalid option.")
    {
        string? input = null;
        while (input == null || !validInputs.Contains(input))
        {
            Console.WriteLine(msg);
            input = Console.ReadLine();
            input = (input != null) ? input.Trim(): input;
            if (input == null || !validInputs.Contains(input)) Console.WriteLine(err);
        }
        return input;
    }   

    // GetSpecialRequestCode( Flight flight )
    //  Returns the special request code of the flight as a string
    //
    // Possible Values:
    //  CFFT, DDJB, LWTT, None
    private static string GetSpecialRequestCode(Flight flight)
    {
        return (flight is CFFTFlight) ? "CFFT" :
               (flight is DDJBFlight) ? "DDJB" :
               (flight is LWTTFlight) ? "LWTT" :
               "None";
    }

    // BoardingGateSupportsFlight( BoardingGate bg, Flight flight )
    //  Returns whether a boarding gate supports a flight depending on the flight's special request code
    private static bool BoardingGateSupportsFlight(BoardingGate bg, Flight flight)
    {
        return 
            (flight is CFFTFlight) ? bg.SupportsCFFT :
            (flight is DDJBFlight) ? bg.SupportsDDJB :
            (flight is LWTTFlight) ? bg.SupportsCFFT :
            true;
    }

    // GetFlightBoardingGate ( Flight myFlight )
    //  Returns the Boarding Gate name that fligt belongs to. If none, return "None"
    private static string GetFlightBoardingGate(Flight myFlight)
    {
        // Iterate thru the boarding gate dictionary.
        foreach (BoardingGate boardingGate in boardingGateDict.Values)
        {
            // If flight allocated to boardingGate matches myFlight
            if (boardingGate.Flight == myFlight)
            {
                return boardingGate.GateName; // returns name of BoardingGate
            }
        }
        return "None"; // if no boardingGate assigned to flight
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
        {"3", () => AssignFlightToBoardingGate()},
        //{"4", () => CreateFlight()},
        {"5", () => DisplayAirlineFlights()},
        //{"6", () => ModifyFlightDetails()},
        //{"7", () => DisplayFlightSchedule()},
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
        string format = "{0,-16}{1,-23}{2,-23}{3,-23}{4}";
        // Header
        Console.WriteLine(
            "=============================================\r\n" +
            "List of Flights for Changi Airport Terminal 5\r\n" +
            "=============================================");

        // Display the Flights with their basic Information.
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
        string format = "{0,-16}{1,-23}{2,-23}{3}";
        // Header
        Console.WriteLine(
            "=============================================\r\n" +
            "List of Boarding Gates for Changi Airport Terminal 5\r\n" +
            "=============================================");

        // Display the Boarding Gates & Special Request Codes
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

    // PART 5 //
    private static void AssignFlightToBoardingGate()
    {
        Console.WriteLine(
            "=============================================  \r\n" +
            "Assign a Boarding Gate to a Flight             \r\n" +
            "=============================================");
        Flight flight = flightDict[ValidateInputFrom(flightDict.Keys, "Enter Flight Number:", "Invalid Flight Number!")];
        BoardingGate boardingGate;
        while (true)
        {
            boardingGate = boardingGateDict[ValidateInputFrom(boardingGateDict.Keys, "Enter Boarding Gate Name:", "Invalid Boarding Gate Name!")];
            if (boardingGate.Flight != null)
            {
                Console.WriteLine($"Boarding Gate {boardingGate.GateName} is already assigned to flight {flight.FlightNumber}!");
            }
            else if (!BoardingGateSupportsFlight(boardingGate, flight))
            {
                Console.WriteLine($"Boarding Gate {boardingGate.GateName} is already assigned to flight {flight.FlightNumber}!");
            }
            else break;
        }
        
        Console.WriteLine($"Flight Number: {flight.FlightNumber}");
        Console.WriteLine($"Origin: {flight.Origin}");
        Console.WriteLine($"Destination: {flight.Destination}");
        Console.WriteLine($"Expected Time: {flight.ExpectedTime}");
        Console.WriteLine($"Special Request Code: {GetSpecialRequestCode(flight)}");
        Console.WriteLine($"Boarding Gate Name: {boardingGate.GateName}");
        Console.WriteLine($"Supports DDJB: {boardingGate.SupportsDDJB}");
        Console.WriteLine($"Supports CFFT: {boardingGate.SupportsCFFT}");
        Console.WriteLine($"Supports LWTT: {boardingGate.SupportsLWTT}");

        // Prompt to update flight status
        if (ValidateInputFrom(new string[] { "Y", "N", "y", "n" }, "Would you like to update the status of the flight? (Y/N)").ToLower() == "y")
        {
            Console.WriteLine("1. Delayed");
            Console.WriteLine("2. Boarding");
            Console.WriteLine("3. On Time");

            // Prompt to add status
            string status = ValidateInputFrom(new string[] { "1", "2", "3" }, "Please select the new status of the flight:");
            flight.Status = (status == "1") ? "Delayed" : (status == "2") ? "Boarding" : "On Time";
        }
        
        Console.WriteLine($"Flight {flight.FlightNumber} has been assigned to Boarding Gate {boardingGate.GateName}!");
    }

    // PART 7 & 8 //
    // ListAirLines() -> Shows all the Airline Name & Code.
    private static void ListAirlines()
    {
        string format = "{0,-16}{1}";
        // Header
        Console.WriteLine(
            "=============================================\r\n" +
            "List of Airlines for Changi Airport Terminal 5\r\n" +
            "=============================================");

        // Display all Airlines Available
        Console.WriteLine(format, "Airline Code", "Airline Name");
        foreach (KeyValuePair<string, Airline> kvp in airlineDict)
        {
            Airline airline = kvp.Value;
            Console.WriteLine(format,
                airline.Code,
                airline.Name
            );
        }
    }

    // PART 7 //
    private static void DisplayAirlineFlights()
    {
        string format = "{0,-16}{1,-23}{2,-23}{3,-23}{4}";

        // Values
        bool match = false;
        string airlineCode = "";
        string flightNo = "";
        Airline myAirline; // initialise to retrieve later for further use.
        Flight myFlight; // initialise to retrieve later for further use.

        // List all the Airlines available
        ListAirlines();

        // Prompt the user to enter the 2-Letter Airline Code (e.g. SQ or MH, etc.)
        while (true)
        {
            Console.Write("Enter the Airline Code: ");
            airlineCode = Console.ReadLine()!.ToUpper().Trim(); // changes lowercase inputs to uppercase & remove whitespace
            
            // Found Airline Code -> true
            // No found Airline Code -> false
            match = airlineDict.ContainsKey( airlineCode );

            if (match) { break; } // proceed if found Airline Code
            Console.WriteLine("Invalid Option."); // else prompt again.
        }

        // Retrieve the Airline object selected.
        myAirline = airlineDict[airlineCode];

        // Header
        Console.WriteLine(
            "=============================================\r\n" +
            "List of Flights for Changi Airport Terminal 5\r\n" +
            "=============================================");

        // Display the Flights with their basic Information.
        Console.WriteLine(format, "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time");
        foreach (KeyValuePair<string, Flight> kvp in flightDict)
        {
            Flight flight = kvp.Value;
            string flightAirline = flight.FlightNumber.Substring(0, 2);

            // Check for Matching Airline Code
            if (myAirline.Code == flightAirline)
            {
                Console.WriteLine(format,
                    flight.FlightNumber,
                    airlineDict[flightAirline].Name, // Gets the name from the Airline object
                    flight.Origin,
                    flight.Destination,
                    flight.ExpectedTime
                );
            }
        }

        // Prompt the user to select a Flight Number
        while (true)
        {
            Console.Write("Enter the Flight Number: ");
            flightNo = Console.ReadLine()!.ToUpper().Trim(); // UpperCase the input.

            // Found flight Code -> true
            // No found flight Code -> false
            match = flightDict.ContainsKey(flightNo);

            // Check for Valid flight code
            if (match)
            {
                // AND check whether flight code is from CHOSEN Airline
                if (flightNo.Substring(0, 2) == airlineCode) { break; }
            }
            Console.WriteLine("Invalid Option."); 
        }

        // Retrieve the Flight object selected
        myFlight = flightDict[flightNo];

        // Display the FULL Flight details.
        // Header
        Console.WriteLine(
            "=============================================\r\n" +
            $"Details of Flight {myFlight.FlightNumber}\r\n" +
            "=============================================");

        // Display selected flight details
        Console.WriteLine(
            $"Flight Number: {myFlight.FlightNumber}\r\n" +
            $"Airline Name: {myAirline}\r\n" +
            $"Origin: {myFlight.Origin}\r\n" + 
            $"Destination: {myFlight.Destination}\r\n" + 
            $"Expected Departure/ Arrival Time: {myFlight.ExpectedTime}\r\n" +
            $"Special Request Code: {GetSpecialRequestCode(myFlight)}\r\n" + 
            $"Boarding Gate: {GetFlightBoardingGate(myFlight)}" );
    } // end of this method, no indentatiom errors


    static void Main(string[] args)
    {
        // Load the Airlines
        LoadAirlines();

        // Load the Boarding Gates
        LoadBoardingGates();

        // Load flights
        LoadFlights();

        // Program start
        while (true)
        {
            Console.Write("\n\n\n\n");
            string option = ShowMenu();
            options[option]();
        }
    }
}
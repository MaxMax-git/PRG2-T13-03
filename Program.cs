//==========================================================
// Student Number	: S10267518J
// Student Name	    : Moh Journ Haydn
// Partner Name	    : Low Yu Wen Max
//==========================================================

using PRG2_T13_03;
using PRG2_T13_03.Classes;
using PRG2_T13_03.Classes.Flights;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.IO;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

// -- IMPORTANT NOTES (to be deleted after completion.) -- // 
// *USE THE DICTIONARIES FROM T5!! 
// *T5 NOT STATIC, USE IN THE PROGRAM. 

// SPECIFY THE ERROR FOR INPUT VALIDATION!!! -> More marks. :D

// ENSURE THAT COMMENTS ARE PROPERLY LABELLED. -> Marks will be awarded.
// -> Do not spam/ flood code with comments. 

// TODO: Add attribute boarding gate to flight class

class Program
{
    // Settings
    private static string dataFolderName = "Data/";
    private static string airlineFileName = dataFolderName + "airlines.csv";
    private static string boardingGateFileName = dataFolderName + "boardinggates.csv";
    private static string flightsFileName = dataFolderName + "flights.csv";

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
    private static string ValidateInputFrom(ICollection<string> validInputs, string msg = "Please select your option:", string err = "Invalid option.", bool uppercase = false)
    {
        string? input = null;
        while (string.IsNullOrEmpty(input) || !validInputs.Contains(input))
        {
            Console.WriteLine(msg);
            input = Console.ReadLine();
            input = (input != null) ? input.Trim(): input;
            if (uppercase) { input = input.ToUpper(); }
            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Input cannot be empty!");
            }
            else if (!validInputs.Contains(input))
            {
                Console.WriteLine(err);
            }
        }
        return input;
    }

    // GetSpecialRequestCode( Flight flight )
    //  Returns the special request code of the flight as a string
    //
    // Possible Values:
    //  CFFT, DDJB, LWTT, None
    private static string GetSpecialRequestCode(Flight flight) => flight switch
    {
        CFFTFlight => "CFFT",
        DDJBFlight => "DDJB",
        LWTTFlight => "LWTT",
        _ => "None",
    };

    // BoardingGateSupportsFlight( BoardingGate bg, Flight flight )
    //  Returns whether a boarding gate supports a flight depending on the flight's special request code
    private static bool BoardingGateSupportsFlight(BoardingGate bg, Flight flight) => flight switch 
    {
        CFFTFlight => bg.SupportsCFFT,
        DDJBFlight => bg.SupportsDDJB,
        LWTTFlight => bg.SupportsCFFT,
        _ => true,
    };

    // GetFlightBoardingGate ( Flight myFlight )
    //  Returns the Boarding Gate name that fligt belongs to. If none, return "None"
    private static string GetFlightBoardingGate(Dictionary<string, BoardingGate> boardingDict, Flight flight)
    {
        // Iterate thru the boarding gate dictionary.
        foreach (BoardingGate boardingGate in boardingDict.Values)
        {
            // If flight allocated to boardingGate matches myFlight
            if (boardingGate.Flight == flight)
            {
                return boardingGate.GateName; // returns name of BoardingGate
            }
        }
        return "Unassigned"; // if no boardingGate assigned to flight
    }

    // PART 1 //

    // pt1. -> Load the airlines.csv file.
    private static void LoadAirlines(Terminal t5)
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
                    count++; // Increment by 1
                }
            }

            // Display the number of loaded Airlines
            Console.WriteLine($"{count} Airlines Loaded!");
        }
    }

    // pt1. -> Load the boardinggates.csv file
    private static void LoadBoardingGates(Terminal t5)
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
                    count++; // Increment by 1
                }
            }

            // Display the number of loaded Boarding Gates
            Console.WriteLine($"{count} Boarding Gates Loaded!");
        }
    }

    // PART 2 //

    // pt2. -> Load flights.csv file
    private static void LoadFlights(Terminal t5)
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
                    details[0], // Flight No
                    details[1], // Flight Origin
                    details[2], // Destination
                    expectedTime); // Expected Time

                // Add the flight objects into their respective airlines
                if (t5.Airlines[airlineCode].AddFlight(flight)) 
                {
                    t5.Flights.TryAdd(details[0], flight);
                    count++;
                };
            }

            // Display the number of loaded Boarding Gates
            Console.WriteLine($"{count} Flights Loaded!");
        }
    }

    // PART 3 //

    // Option dictionary reference
    private static Dictionary<string, Action<Terminal>> options = new Dictionary<string, Action<Terminal>>
    {
        {"1", t => ListAllFlights(t)},
        {"2", t => ListAllBoardingGates(t.BoardingGates)},
        {"3", t => AssignFlightToBoardingGate(t)},
        //{"4", () => CreateFlight()},
        {"5", t => DisplayAirlineFlights(t)},
        {"6", t => ModifyFlightDetails(t)},
        //{"7", () => DisplayFlightSchedule()},
        {"0", t => {
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

    private static void ListAllFlights(Terminal t5)
    {
        string format = "{0,-16}{1,-23}{2,-23}{3,-23}{4}";
        // Header
        Console.WriteLine(
            "=============================================\r\n" +
            "List of Flights for Changi Airport Terminal 5\r\n" +
            "=============================================");

        // Display the Flights with their basic Information.
        Console.WriteLine(format, "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time");
        foreach (KeyValuePair<string, Flight> kvp in t5.Flights)
        {
            Flight flight = kvp.Value;
            Console.WriteLine(format,
                flight.FlightNumber,
                t5.Airlines[flight.FlightNumber.Substring(0, 2)].Name, // Gets the name from the Airline object
                flight.Origin,
                flight.Destination,
                flight.ExpectedTime
            );
        }
        Console.WriteLine();
    }


    // PART 4 //
    // List all the boarding gates
    private static void ListAllBoardingGates(Dictionary<string, BoardingGate> boardingDict)
    {
        string format = "{0,-16}{1,-23}{2,-23}{3,-23}{4}"; // format for string (string interpolation)
        // Header
        Console.WriteLine(
            "=============================================\r\n" +
            "List of Boarding Gates for Changi Airport Terminal 5\r\n" +
            "=============================================");

        // Display the Boarding Gates & Special Request Codes, Flight Number Assigned 
        Console.WriteLine(format, "Gate Name", "DDJB", "CFFT", "LWTT", "Assigned Flight Number");
        foreach (KeyValuePair<string, BoardingGate> kvp in boardingDict)
        {
            BoardingGate boardingGate = kvp.Value;
            Console.WriteLine(format, 
                boardingGate.GateName,
                boardingGate.SupportsDDJB,
                boardingGate.SupportsCFFT,
                boardingGate.SupportsLWTT,
                boardingGate.Flight?.FlightNumber ?? "--" // if flight can be null, and if null, output "--"
            );
        }
    }

    // PART 5 //
    private static void AssignFlightToBoardingGate(Terminal t5)
    {
        Console.WriteLine(
            "=============================================  \r\n" +
            "Assign a Boarding Gate to a Flight             \r\n" +
            "=============================================");

        // Prompts the user for the flight number, invalidating if not found
        // Afterwards, lookup for the flight through the terminal's flight dictionary
        Flight flight = t5.Flights[ValidateInputFrom(t5.Flights.Keys, "Enter Flight Number:", "Invalid Flight Number!")];
        BoardingGate boardingGate;
        while (true)
        {
            // Prompts the user for the boarding gate name, invalidating if not found
            // Afterwards, lookup for the boarding gate through the terminal's boarding gate dictionary
            boardingGate = t5.BoardingGates[ ValidateInputFrom(t5.BoardingGates.Keys, "Enter Boarding Gate Name:", "Invalid Boarding Gate Name!") ];
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
    private static void ListAirlines(Dictionary<string, Airline> airlineDict)
    {
        string format = "{0,-16}{1}";
        // Header
        Console.WriteLine(
            "=============================================\r\n" +
            "List of Airlines for Changi Airport Terminal 5\r\n" +
            "=============================================");

        // Display all Airlines Available
        Console.WriteLine(format, "Airline Code", "Airline Name");
        foreach (Airline airline in airlineDict.Values)
        {
            Console.WriteLine(format,
                airline.Code,
                airline.Name
            );
        }
    }

    // PART 7 & 8 //
    // PromptFlighNumberFromAirline(string message, string airlineCode)
    private static string PromptFlightNumberFromAirline(Terminal t5, string message, string airlineCode)
    {
        // Prompt the user to select a Flight Number
        while (true)
        {
            Console.Write("Enter the Flight Number: ");
            string flightNo = Console.ReadLine()!.ToUpper().Trim(); // UpperCase the input.

            // Found flight Code -> true
            // No found flight Code -> false
            bool match = t5.Flights.ContainsKey(flightNo);

            // Check for Valid flight code
            if (match)
            {
                // AND check whether flight code is from CHOSEN Airline
                if (flightNo.Substring(0, 2) == airlineCode) { 
                    return flightNo; 
                }
                else 
                {
                    Console.WriteLine($"Invalid, {flightNo} not from {t5.Airlines[airlineCode].Name}"); 
                }
            }
            else { Console.WriteLine("Invalid Flight Number. "); }
        }
    }

    // PART 7 & 8 //
    // DisplayFullFlightDetails()
    private static void DisplayFullFlightDetails(Terminal t5, Flight flight)
    {
        // Header
        Console.WriteLine(
            "=============================================\r\n" +
            $"Details of Flight {flight.FlightNumber}\r\n" +
            "=============================================");

        // Displays the full details of the selected flight.
        Console.WriteLine(
            $"Flight Number: {flight.FlightNumber}\r\n" +
            $"Airline Name: {t5.Airlines[flight.FlightNumber.Substring(0, 2)].Name}\r\n" + // Gets the name from the Airline object
            $"Origin: {flight.Origin}\r\n" +
            $"Destination: {flight.Destination}\r\n" +
            $"Expected Departure/ Arrival Time: {flight.ExpectedTime}\r\n" +
            $"Status: {flight.Status}\r\n" +
            $"Special Request Code: {GetSpecialRequestCode(flight)}\r\n" +
            $"Boarding Gate: {GetFlightBoardingGate(t5.BoardingGates, flight)}"
            );
    }

    // PART 7 //
    // DisplayAirlineFlights() 
    private static void DisplayAirlineFlights(Terminal t5)
    {
        string format = "{0,-16}{1,-23}{2,-23}{3,-23}{4}";

        // Values
        Airline myAirline; // initialise to retrieve later for further use.
        Flight myFlight; // initialise to retrieve later for further use.

        // List all the Airlines available
        ListAirlines(t5.Airlines);

        // Prompt the user to enter the 2-Letter Airline Code (e.g. SQ or MH, etc.)
        // string airlineCode = PromptAirLineCode(t5.Airlines, "Enter airline code: ");
        string airlineCode = ValidateInputFrom(t5.Airlines.Keys, "Enter Airline Code: ", "No matching airline code found.", true);

        // Retrieve the Airline object selected.
        myAirline = t5.Airlines[airlineCode];

        // Header
        Console.WriteLine(
            "=============================================\r\n" +
            $"List of Flights for {myAirline.Name}\r\n" +
            "=============================================");

        // Display the Flights with their basic Information.
        Console.WriteLine(format, "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time");
        foreach (Flight flight in t5.Flights.Values)
        {
            string flightAirline = flight.FlightNumber.Substring(0, 2);

            // Check for Matching Airline Code
            if (myAirline.Code == flightAirline)
            {
                Console.WriteLine(format,
                    flight.FlightNumber,
                    t5.Airlines[flightAirline].Name, // Gets the name from the Airline object
                    flight.Origin,
                    flight.Destination,
                    flight.ExpectedTime
                );
            }
        }

        // Prompt the user to select a Flight Number
        string flightNo = PromptFlightNumberFromAirline(t5, "Enter Flight Number: ", airlineCode);

        // Retrieve the Flight object selected
        myFlight = t5.Flights[flightNo];

        // Display the FULL details of selected Flight.
        DisplayFullFlightDetails(t5, myFlight);
        
    } // end of this method, no indentatiom errors

    // PART 8 //
    // ModifyFlightDetails() -> 
    private static void ModifyFlightDetails(Terminal t5)
    {
        string format = "{0,-16}{1,-23}{2,-23}{3,-23}{4}";
        string option = "";

        // List all the Airlines Available
        ListAirlines(t5.Airlines);

        // Prompt the user to enter the 2-Letter Airline Code (e.g. SQ or MH, etc.)
        string airlineCode = ValidateInputFrom(t5.Airlines.Keys, "Enter Airline Code: ", "No matching airline code found.", true);

        // Retrieve the Airline object selected.
        Airline myAirline = t5.Airlines[airlineCode];

        // Header
        Console.WriteLine(
            "=============================================\r\n" +
            $"List of Flights for {myAirline.Name}\r\n" +
            "=============================================");

        // Display the Flights with their basic Information.
        Console.WriteLine(format, "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time");
        foreach (Flight flight in t5.Flights.Values)
        {
            string flightAirline = flight.FlightNumber.Substring(0, 2);

            // Check for Matching Airline Code
            if (myAirline.Code == flightAirline)
            {
                Console.WriteLine(format,
                    flight.FlightNumber,
                    t5.Airlines[flightAirline].Name, // Gets the name from the Airline object
                    flight.Origin,
                    flight.Destination,
                    flight.ExpectedTime
                );
            }
        }

        // Prompt the user to select a Flight Number
        string flightNo = PromptFlightNumberFromAirline(t5, "Choose an existing Flight to modify or delete: ", airlineCode);

        // Option 1 -> Modify Flight
        // Option 2 -> Delete Flight
        option = Console.ReadLine()!;
    }
    
    static void Main(string[] args)
    {
        Terminal t5 = new Terminal("Terminal5");

        // Load the Airlines
        LoadAirlines(t5);

        // Load the Boarding Gates
        LoadBoardingGates(t5);

        // Load flights
        LoadFlights(t5);

        // Program start
        while (true)
        {
            Console.Write("\n\n\n\n");
            string option = ShowMenu();
            options[option](t5);
        }
    }
}
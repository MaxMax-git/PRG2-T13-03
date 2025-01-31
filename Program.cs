//==========================================================
// Student Number	: S10267518J
// Student Name	    : Moh Journ Haydn
// Partner Name	    : Low Yu Wen Max
//==========================================================

using Microsoft.VisualBasic.FileIO;
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
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

class Program
{
    // Settings
    private static string dataFolderName = "Data/";
    private static string airlineFileName = dataFolderName + "airlines.csv";
    private static string boardingGateFileName = dataFolderName + "boardinggates.csv";
    private static string flightsFileName = dataFolderName + "flights.csv";

    // Utility

    // ValidateInputFrom( ICollection<string> validInputs, string? msg, string? err, int casing )
    //  Prompts user for input, and checks if said input is within valid inputs
    //      Done through: validInputs.Contains()
    //
    //  Returns the user's input if valid
    //
    //  Optional message/error can be added
    //
    //  Optional int can be passed to determine casing of the input (to check against the valid inputs)
    //  0 - No change (default)
    //  More than 0 - Uppercase
    //  Less than 0 - Lowercase
    //
    //  Basic usage:
    //      string input = ValidateInputFrom(new string[] { "yes", "no" }, "Do you like apples? ")
    //      Output: "Do you like apples? " -- user inputs yes or no
    //
    // Note:
    //  Inputs are trimmed
    //  Messages/Errors uses Console.WriteLine()
    private static string ValidateInputFrom(ICollection<string> validInputs, string msg = "Please select your option:", string err = "Invalid option.", int casing = 0)
    {
        string? input = null;
        while (string.IsNullOrEmpty(input) || !validInputs.Contains(input))
        {
            Console.WriteLine(msg);
            input = Console.ReadLine() ?? "";
            input = input.Trim();
            if (casing > 0) input = input.ToUpper();
            else if (casing < 0) input = input.ToLower();
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
    private static string GetSpecialRequestCode(Flight flight, bool emptyStringIfNone = false) => flight switch
    {
        CFFTFlight => "CFFT",
        DDJBFlight => "DDJB",
        LWTTFlight => "LWTT",
        _ => (emptyStringIfNone) ? "" : "None",
    };

    // BoardingGateSupportsFlight( BoardingGate bg, Flight flight )
    //  Returns whether a boarding gate supports a flight depending on the flight's special request code
    private static bool BoardingGateSupportsFlight(BoardingGate bg, Flight flight) => flight switch 
    {
        CFFTFlight => bg.SupportsCFFT,
        DDJBFlight => bg.SupportsDDJB,
        LWTTFlight => bg.SupportsLWTT,
        _ => true,
    };

    // GetFlightBoardingGate ( Terminal t5, Flight flight )
    //  Returns the Boarding Gate name that fligt belongs to. If none, return "None"
    private static BoardingGate? GetFlightBoardingGate(Terminal t5, Flight flight)
    {
        // PAST IMPLEMENTATION
        //// Iterate thru the boarding gate dictionary.
        //foreach (BoardingGate boardingGate in t5.BoardingGates.Values)
        //{
        //    // If flight allocated to boardingGate matches myFlight
        //    if (boardingGate.Flight == flight)
        //    {
        //        return boardingGate; // returns name of BoardingGate
        //    }
        //}
        //return null; // if no boardingGate assigned to flight

        // NEW IMPLEMENTATION
        // Finds the first boarding gate which has a flight equal to the flight passed, else returns null
        return t5.BoardingGates.FirstOrDefault(x => (x.Value.Flight == flight)).Value;
    }

    // GetFlightBoardingGateName( Terminal t5, Flight, flight )
    private static string GetFlightBoardingGateName(Terminal t5, Flight flight)
    {
        BoardingGate? boardingGate = GetFlightBoardingGate(t5, flight);
        return (boardingGate != null) ? boardingGate.GateName : "Unassigned";
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
                string airlineCode = details[0][0..2];
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
        {"2", t => ListAllBoardingGates(t)},
        {"3", t => AssignFlightToBoardingGate(t)},
        {"4", t => CreateFlight(t)},
        {"5", t => DisplayAirlineFlightFullDetails(t)},
        {"6", t => ModifyFlightDetails(t)},
        {"7", t => DisplayFlightSchedule(t)},
        {"8", t => ProcessAllUnassignedFlights(t)},
        {"9", t => DisplayAirlineFees(t)},
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
            "8. Process All Unassigned Flights                          \r\n" +
            "9. Display Total Fee Per Airline For the Day               \r\n" +
            "0. Exit                                                    \r\n");

        // Prompts the user for an option,
        // reprompting if the value is not 0-7
        // (which are the keys of the option Dictionary)
        return ValidateInputFrom(options.Keys);
    }

    private static void ListAllFlights(Terminal t5, bool moreDetails = false)
    {
        string format = "{0,-15}{1,-20}{2,-20}{3,-20}{4,-35}";
        // Header
        Console.WriteLine(
            "=============================================\r\n" +
            "List of Flights for Changi Airport Terminal 5\r\n" +
            "=============================================");

        // Display the Flights with their basic Information.
        Console.Write(format, "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time");
        if (moreDetails) { Console.Write("{0,-25}{1}", "Special Request Code", "Boarding Gate"); }
        Console.WriteLine();
        foreach (KeyValuePair<string, Flight> kvp in t5.Flights)
        {
            Flight flight = kvp.Value;

            // Prints the flight with the format
            Console.Write(format,
                flight.FlightNumber,
                // Get the substring of the flight number
                // Gets the name from the Airline object
                t5.Airlines[flight.FlightNumber[0..2]].Name, 
                flight.Origin,
                flight.Destination,
                flight.ExpectedTime
            );
            if (moreDetails) // advanced -> add Flight's Special Request Code + Boarding Gate Name
            {
                Console.Write("{0,-25}{1}", 
                    GetSpecialRequestCode(flight),
                    GetFlightBoardingGateName(t5, flight)
                    );
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }


    // PART 4 //
    // ListAllBoardingGates() -> List info about boarding gates. (Special Request Code + Flight assigned)
    private static void ListAllBoardingGates(Terminal t5)
    {
        string format = "{0,-15}{1,-20}{2,-20}{3,-20}{4}"; // format for string (string interpolation)
        // Header
        Console.WriteLine(
            "=============================================\r\n" +
            "List of Boarding Gates for Changi Airport Terminal 5\r\n" +
            "=============================================");

        // Display the Boarding Gates & Special Request Codes, Flight Number Assigned 
        Console.WriteLine(format, "Gate Name", "DDJB", "CFFT", "LWTT", "Assigned Flight Number");
        foreach (BoardingGate boardingGate in t5.BoardingGates.Values)
        {
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
                Console.WriteLine($"Boarding Gate {boardingGate.GateName} does not support flight {flight.FlightNumber}!");
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
        
        boardingGate.Flight = flight;

        Console.WriteLine($"Flight {flight.FlightNumber} has been assigned to Boarding Gate {boardingGate.GateName}!");
    }

    // PART 6 //
    private static string PromptNewFlightNumber(Terminal t5)
    {
        string? flightNo = null;
        while (true)
        {
            Console.Write("Enter Flight Number: ");
            flightNo = Console.ReadLine() ?? "";
            flightNo = flightNo.Trim().ToUpper();

            // Warn if empty
            if (string.IsNullOrEmpty(flightNo)) { Console.WriteLine("Flight number cannot be empty!"); }

            // Warn if the flight number already exists
            else if (t5.Flights.ContainsKey(flightNo)) { Console.WriteLine("Another flight exists with the same flight number!"); }

            // Warn if the flight number is not the correct format
            else if (flightNo.Length != 6 || flightNo[2] != ' ') { Console.WriteLine("Flight number must follow the format! e.g. SQ 123"); }

            // Warn if flight number does not correspond to an existing airline
            else if (!t5.Airlines.ContainsKey(flightNo[0..2])) { Console.WriteLine("Flight number must belong to an existing airline! e.g. SQ 123"); }

            // Warn if flight number does not end off with 3 digits
            else if (!flightNo[3..].All(char.IsDigit)) { Console.WriteLine("Flight number must end off with 3 digits! e.g. SQ 123"); }
            else { return flightNo; }
        }
    }

    private static string PromptLocation(string locationString, bool specifyNew = false)
    {
        string? location = null;
        while (true)
        {
            Console.Write($"Enter{(specifyNew ? " new " : " ")}{locationString}: ");
            location = Console.ReadLine() ?? "";

            // Warn if empty
            if (string.IsNullOrEmpty(location)) Console.WriteLine($"{locationString} cannot be empty!");

            // Warn if origin not in correct format
            if (!Regex.IsMatch(location, @"^[A-Za-z\s]+ \([A-Za-z]{3}\)$")) Console.WriteLine($"{locationString} must follow the format! e.g. Singapore (SIN)");
            else return (location.ToUpper()[0] + location[1..^5].ToLower() + location[^5..].ToUpper()); // Proper capitalizations
        }
    }

    private static DateTime PromptExpectedTime(bool specifyNew = false)
    {
        DateTime flightDateTime;
        string myDateTimeInput = "";
        while (true)
        {
            try
            {
                // Prompt new Expected Departure/ Arrival Time (dd/mm/yyyy hh:mm)
                Console.Write($"Enter{(specifyNew ? " new " : " ")}Expected Departure/Arrival Time (dd/MM/yyyy HH:mm): ");
                myDateTimeInput = Console.ReadLine() ?? "";
                flightDateTime = Convert.ToDateTime(myDateTimeInput); ;
                return flightDateTime;
            }
            // check if input is Date Time
            catch (FormatException) { Console.WriteLine($"The string {myDateTimeInput} is not a valid Date Time."); }
            // checks for additional errors
            catch (Exception exception) { Console.WriteLine(exception.Message); }
        }
    }

    private static void CreateFlight(Terminal t5)
    {
        Console.WriteLine(
            "=============================================  \r\n" +
            "Create Flight for Changi Airport Terminal 5    \r\n" +
            "=============================================");

        int count = 0;

        while (true)
        {
            // Prompt for flight info
            string flightNo = PromptNewFlightNumber(t5);
            string origin = PromptLocation("Origin");
            string destination = PromptLocation("Destination");
            DateTime expectedTime = PromptExpectedTime();

            // Constructor dictionary reference
            Dictionary<string, Func<string, string, string, DateTime, Flight>> flightConstructors =
                new Dictionary<string, Func<string, string, string, DateTime, Flight>>
                {
                        { "CFFT", (a,b,c,d) => new CFFTFlight(a,b,c,d) },
                        { "DDJB", (a,b,c,d) => new DDJBFlight(a,b,c,d) },
                        { "LWTT", (a,b,c,d) => new LWTTFlight(a,b,c,d) },
                        { "NONE", (a,b,c,d) => new NORMFlight(a,b,c,d) },
                };

            string flightSRQ = "NONE";

            // Prompt whether to add a special request
            if (
            ValidateInputFrom(
                new string[] { "Y", "N" },
                $"Do you want a special request for your flight? (Y/N): ",
                casing: 1
            ) == "Y")
            {
                // Prompt new Special Request Code
                flightSRQ = ValidateInputFrom(new string[] { "CFFT", "DDJB", "LWTT", "NONE" },
                                        "Enter new Special Request Code (CFFT/DDJB/LWTT/None): ",
                                        "Error. No matching Special Request Code found.", 1);
            }

            // Initialise flight
            Flight flight = flightConstructors[flightSRQ](
                    flightNo,
                    origin,
                    destination,
                    expectedTime);

            Airline airline = t5.Airlines[flightNo[0..2]];

            // Add the flight to both flight dictionary and their respective airline
            if (airline.AddFlight(flight))
            {
                t5.Flights.TryAdd(flightNo, flight);
            }

            // Append to flights.csv
            using (StreamWriter sw = new StreamWriter(flightsFileName, true))
            {
                sw.WriteLine($"{flightNo},{origin},{destination},{expectedTime.ToString("hh:mm tt")},{GetSpecialRequestCode(flight, true)}");
            }

            // Increment flight counter
            count++;

            Console.WriteLine("New flight added to the terminal");
            Console.WriteLine(
                $"Flight Number: {flight.FlightNumber}\r\n" +
                $"Airline Name: {airline.Name}\r\n" + // Gets the name from the Airline object
                $"Origin: {flight.Origin}\r\n" +
                $"Destination: {flight.Destination}\r\n" +
                $"Expected Departure/Arrival Time: {flight.ExpectedTime}\r\n" +
                $"Status: {flight.Status}\r\n" +
                $"Special Request Code: {GetSpecialRequestCode(flight)}\r\n" +
                $"Boarding Gate: {GetFlightBoardingGateName(t5, flight)}");

            // Prompt for another flight, else break out of loop
            if (
            ValidateInputFrom(
                new string[] { "Y", "N" },
                "Would you like to add another flight? (Y/N): ",
                casing: 1
            ) == "N") break;
        }

        Console.WriteLine($"{count} flight{((count>1)?"s":"")} succesfully added!");
    }

    // PART 7 & 8 //
    // ListAirLines() -> Shows all the Airline Name & Code.
    private static void ListAirlines(Dictionary<string, Airline> airlineDict)
    {
        string format = "{0,-16}{1}"; // format of table
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
    // ListAirlineFlights() -> Shows all flight detail from an airline.
    private static void ListAirlineFlights(Airline airline)
    {
        string format = "{0,-16}{1,-23}{2,-23}{3,-23}{4}"; // format of table

        // Header
        Console.WriteLine(
            "=============================================\r\n" +
            $"List of Flights for {airline.Name}\r\n" +
            "=============================================");

        // Display the Flights with their basic Information.
        Console.WriteLine(format, "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time");
        foreach (Flight flight in airline.Flights.Values) // for flight in selected airline
        {
            Console.WriteLine(format,
                flight.FlightNumber,
                airline.Name, // name of Airline
                flight.Origin,
                flight.Destination,
                flight.ExpectedTime
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
                // AND check whether flight code is from CHOSEN Airline\
                if (t5.Airlines[airlineCode].Flights.ContainsKey(flightNo)) 
                {
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
    // DisplayFullFlightDetails() -> Shows the FULL detail of a Flight
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
            $"Airline Name: {t5.GetAirlineFromFlight(flight).Name}\r\n" + // Gets the name from the Airline object
            $"Origin: {flight.Origin}\r\n" +
            $"Destination: {flight.Destination}\r\n" +
            $"Expected Departure/ Arrival Time: {flight.ExpectedTime}\r\n" +
            $"Status: {flight.Status}\r\n" +
            $"Special Request Code: {GetSpecialRequestCode(flight)}\r\n" +
            $"Boarding Gate: {GetFlightBoardingGateName(t5, flight)}"
            );
    }

    // PART 7 //
    // DisplayAirlineFlights() 
    private static void DisplayAirlineFlightFullDetails(Terminal t5)
    {
        // Values
        Airline myAirline; // initialise to retrieve later for further use.
        Flight myFlight; // initialise to retrieve later for further use.

        // List all the Airlines available
        ListAirlines(t5.Airlines);

        // Prompt the user to enter the 2-Letter Airline Code (e.g. SQ or MH, etc.)
        // string airlineCode = PromptAirLineCode(t5.Airlines, "Enter airline code: ");
        string airlineCode = ValidateInputFrom(t5.Airlines.Keys, "Enter Airline Code: ", "No matching airline code found.", 1);

        // Retrieve the Airline object selected.
        myAirline = t5.Airlines[airlineCode];

        // Show all flight (basic detail) from selected airline.
        ListAirlineFlights(myAirline);

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
        string option = "";
        Airline myAirline; // initialised for retrieving & using the Airline object later.
        Flight myFlight;

        // List all the Airlines Available
        ListAirlines(t5.Airlines);

        // Prompt the user to enter the 2-Letter Airline Code (e.g. SQ or MH, etc.)
        string airlineCode = ValidateInputFrom(t5.Airlines.Keys, "Enter Airline Code: ", "No matching airline code found.", 1);

        // Retrieve the Airline object selected.
        myAirline = t5.Airlines[airlineCode];

        // List all Flights (basic info) from selected Airline.
        ListAirlineFlights(myAirline);

        // Prompt the user to select a Flight Number
        string flightNo = PromptFlightNumberFromAirline(t5, "Choose an existing Flight to modify or delete: ", airlineCode);

        // Retrieve the Flight object
        myFlight = t5.Flights[flightNo];

        // Prompt input to Modify/ Delete flight.
        Console.WriteLine("1. Modify Flight");
        Console.WriteLine("2. Delete Flight");
        
        option = ValidateInputFrom(new string[] { "1", "2" }); // option can ONLY either by 1 / 2

        if (option == "1") // if modify flight
        {
            // Display Options to modify flight
            Console.Write(
                "1. Modify Basic Information\r\n" +
                "2. Modify Status\r\n" +
                "3. Modify Special Request Code\r\n" +
                "4. Modify Boarding Gate\r\n"
                );

            // Prompt user to enter options to modify flight
            option = ValidateInputFrom(new string[] { "1", "2", "3", "4" });

            if (option == "1") // Change Basic Flight Info
            {
                // Prompt new flight Origin
                Console.Write("Enter new Origin: ");
                string flightOrigin = Console.ReadLine()!;

                // Prompt new Destination
                Console.Write("Enter new Destination: ");
                string flightDestination = Console.ReadLine()!;

                // Get the new Flight's Expected Departure/ Arrival Time
                DateTime flightDateTime;
                string myDateTimeInput = "";
                while (true)
                {
                    try
                    {
                        // Prompt new Expected Departure/ Arrival Time (dd/mm/yyyy hh:mm)
                        Console.Write("Enter new Expected Departure/ Arrival Time (dd/MM/yyyy HH:mm): ");
                        myDateTimeInput = Console.ReadLine()!;
                        flightDateTime = Convert.ToDateTime(myDateTimeInput); ;
                        break;
                    }
                    // check if input is Date Time
                    catch (FormatException) { Console.WriteLine($"The string {myDateTimeInput} is not a valid Date Time."); }
                    // checks for additional errors
                    catch (Exception exception) { Console.WriteLine(exception.Message); }
                }

                // Change the basic info
                myFlight.Origin = flightOrigin;
                myFlight.Destination = flightDestination;
                myFlight.ExpectedTime = flightDateTime;
            }
            else if (option == "2") // Change Flight Status
            {
                // Prompt new Flight Status
                string flightStatus = ValidateInputFrom(
                                        new string[] { "DELAYED", "BOARDING", "ON TIME", "SCHEDULED" },
                                        "Enter new Status: ",
                                        "Error. Invalid Status entered.", 1);

                // Uppercase the first letter of input and the rest is in lower case
                flightStatus = flightStatus.ToUpper()[0] + flightStatus[1..].ToLower();

                // Update new flight status
                myFlight.Status = flightStatus;
            }
            else if (option == "3") // Modify Special Request Code
            {
                // Values
                string flightSRQ = "";
                Flight updated;
                bool match = true;

                // Constructor dictionary reference
                Dictionary<string, Func<string, string, string, DateTime, Flight>> flightConstructors =
                    new Dictionary<string, Func<string, string, string, DateTime, Flight>>
                    {
                        { "CFFT", (a,b,c,d) => new CFFTFlight(a,b,c,d) },
                        { "DDJB", (a,b,c,d) => new DDJBFlight(a,b,c,d) },
                        { "LWTT", (a,b,c,d) => new LWTTFlight(a,b,c,d) },
                        { "NONE", (a,b,c,d) => new NORMFlight(a,b,c,d) },
                    };

                // Check whether it already belongs to a boarding gate
                while (true)
                {
                    // e.g. SQ 115 -> DDJB   
                    // Prompt new Special Request Code
                    flightSRQ = ValidateInputFrom(new string[] { "CFFT", "DDJB", "LWTT", "NONE" },
                                            "Enter new Special Request Code (CFFT/DDJB/LWTT/None): ",
                                            "Error. No matching Special Request Code found.", 1);

                    // Flight class with UPDATED Special Request Code
                    updated = flightConstructors[flightSRQ](
                                        myFlight.FlightNumber,
                                        myFlight.Origin,
                                        myFlight.Destination,
                                        myFlight.ExpectedTime);

                    foreach (BoardingGate bg in t5.BoardingGates.Values)
                    {
                        // Check for matching flight
                        if (bg.Flight != null && bg.Flight.FlightNumber == myFlight.FlightNumber)
                        {
                            match = BoardingGateSupportsFlight(bg, updated);
                            // Check if it voilates the boarding gate domains
                            if (!match)
                            {
                                // Display error Message
                                Console.WriteLine($"Boarding Gate ({bg.GateName}) does not support Special Request Code ({flightSRQ})");
                            }
                            else { bg.Flight = updated; } // updates flight belonging to boarding gate
                            break;
                        }
                    }
                    if (match) { break; } // if no gates found or does not violate boarding gate SRQ
                }

                myFlight = updated;
                // update the dictionaries 
                t5.Flights[myFlight.FlightNumber] = myFlight;
                myAirline.Flights[myFlight.FlightNumber] = myFlight;
            }
            else if (option == "4") // Modify Flight Boarding Gate
            {
                bool match = false;
                BoardingGate? bgOld = null;
                BoardingGate bgNew;
                
                foreach (BoardingGate boardingGate in t5.BoardingGates.Values)
                {
                    // Checks if flight belongs to a Boarding Gate 
                    match = (boardingGate.Flight != null) && (boardingGate.Flight == myFlight);
                    if (match) // if found, display Found message
                    {
                        bgOld = boardingGate;
                        Console.WriteLine($"Flight {myFlight.FlightNumber} currently belongs to Boarding Gate ({boardingGate.GateName})");
                        break;
                    }
                }
                // Display Not-Found message
                if (!match) { Console.WriteLine($"Flight {myFlight.FlightNumber} currently does not belong to a Boarding Gate."); }

                while (true)
                {
                    // Ask user to enter a valid Boarding Gate
                    string boardingGateCode = ValidateInputFrom(
                                    t5.BoardingGates.Keys,
                                    "Enter a new Boarding Gate: ",
                                    "Boarding Gate not found.", 1);

                    bgNew = t5.BoardingGates[boardingGateCode];
                    // Checks if selected Boarding Gate has flight.
                    if (bgNew.Flight != null)
                    {
                        // Display Error message
                        Console.WriteLine($"Error. Boarding Gate ({bgNew.GateName}) is occupied by Flight {bgNew.Flight!.FlightNumber}. ");
                    }
                    // Checks if boarding gate suppports flight's Special Request Code
                    else if (!BoardingGateSupportsFlight(bgNew, myFlight))
                    {
                        // Display Error Message
                        Console.WriteLine($"Boarding Gate ({bgNew.GateName}) does not suppport Special Request Code ({GetSpecialRequestCode(myFlight)}).");
                    }
                    else { break; } // if satisfy the above conditions
                }
                if (match && bgOld != null) // remove flight if it was in BG, to assign to its new BG
                { 
                    bgOld.Flight = null;
                    Console.WriteLine($"Flight {myFlight.FlightNumber} is removed from Boarding Gate {bgOld.GateName}.");
                }

                // Display success message
                bgNew.Flight = myFlight; // assign flight to selected Boarding Gate
                Console.WriteLine($"Flight {myFlight.FlightNumber} is now successfully assigned to Boarding Gate ({bgNew.GateName}). ");
            }
            Console.WriteLine("Flight Updated!");
            DisplayFullFlightDetails(t5, myFlight);
        }
        else if (option == "2") // if delete flight
        {
            // Display warning message to delete flight
            option = ValidateInputFrom(
                                new string[] { "Y", "N" },
                                $"Are you sure you want to delete Flight {myFlight.FlightNumber} (Y/N): ",
                                casing: 1);

            if (option == "Y") // if user wants to delete flight
            {
                // Remove flight from Airline
                myAirline.RemoveFlight(myFlight);

                // Remove flight from Boarding Gate (if assigned)
                foreach (BoardingGate boardingGate in t5.BoardingGates.Values)
                {
                    if (boardingGate.Flight == myFlight)
                    {
                        boardingGate.Flight = null; // remove flight from boarding gate
                    }
                }

                // Remove flights from t5.Flights dictionary
                t5.Flights.Remove(myFlight.FlightNumber); // logic works cuz myFlight is retreived from t5.Flights[flightNo]

                // Display success message
                Console.WriteLine($"Flight {myFlight.FlightNumber} is successfully deleted.");
            }
            else if (option == "N") // if user does not want to delete flight
            {
                Console.WriteLine($"Flight {myFlight.FlightNumber} is not deleted.");
            }
        }
    }

    // PART 9 // 
    private static void DisplayFlightSchedule(Terminal t5)
    {
        string format = "{0,-15}{1,-20}{2,-20}{3,-20}{4,-35}{5,-15}{6,-25}{7}";
        // Header
        Console.WriteLine(
            "=============================================  \r\n" +
            "Flight Schedule for Changi Airport Terminal 5  \r\n" +
            "=============================================");
        
        List<Flight> schedule = new List<Flight>(t5.Flights.Values);
        schedule.Sort();

        // Display the Flights with their basic Information.
        Console.WriteLine(format, "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time", "Status", "Special Request Code", "Boarding Gate");
        foreach (Flight flight in schedule)
        {
            // Prints the flight with the format
            Console.WriteLine(format,
                flight.FlightNumber,
                // Get the substring of the flight number
                // Gets the name from the Airline object
                t5.GetAirlineFromFlight(flight).Name,
                flight.Origin,
                flight.Destination,
                flight.ExpectedTime,
                flight.Status,
                GetSpecialRequestCode(flight), 
                GetFlightBoardingGateName(t5, flight)
            );
        }
        Console.WriteLine();
    }

    // ADVANCED FEATURE (a) // -> Max
    // ProcessAllUnassignedFlights()
    private static void ProcessAllUnassignedFlights(Terminal t5)
    {
        int countNotAssigned = 0;
        int countAssigned = 0;  
        int countProcessed = 0;
        // Queue to store Flight objects if no Boarding Gate is assigned.
        Queue<Flight> flightQueue = new Queue<Flight>();

        // List of flights that are assigned a boarding gate.
        List<Flight> flightBoarding = new List<Flight>(); 

        // Check for flights that are assigned a boarding gate
        foreach (BoardingGate boardingGate in t5.BoardingGates.Values)
        {
            if (boardingGate.Flight != null) // if Boarding Gate has Flight
            {
                flightBoarding.Add(boardingGate.Flight);
            }
            else { countNotAssigned++; } // if Boarding Gate has no Flight
        }

        // Check for flights that are not assigned a boarding gate
        foreach (Flight myflight in t5.Flights.Values)
        {
            bool match = false;
            foreach (Flight assignedFlight in flightBoarding)
            {
                // if flight is assigned
                if (myflight == assignedFlight)
                {
                    match = true;
                    break;
                }
            }
            if (!match)
            {
                // flight not found -> not assigned -> Add to queue
                flightQueue.Enqueue(myflight);
            }
        }

        // Display the total number of Flights that do not have any Boarding Gate assigned yet
        Console.WriteLine($"Number of Flights that do not have a Boarding Gate assigned: {flightQueue.Count}");

        // Display the total number of Boarding Gates that do not have a Flight Number assigned yet
        Console.WriteLine($"Number of Boarding Gates without an assigned Flight Number: {countNotAssigned}\n");

        // While loop -> to iterate thru every flight in queue.
        while ( flightQueue.Count > 0)
        {
            // Deqeueue the first flight in the queue
            Flight flightDequeue = flightQueue.Dequeue();

            // Check if the Flight has a Special Request Code
            if (GetSpecialRequestCode(flightDequeue) != "None")
            {
                // Search for an unassigned Boarding Gate that matches the Special Request Code
                foreach (BoardingGate boardingGate in t5.BoardingGates.Values)
                {
                    if (BoardingGateSupportsFlight(boardingGate, flightDequeue) && boardingGate.Flight == null)
                    {
                        boardingGate.Flight = flightDequeue; // assign boarding gate to flight
                        countProcessed++;
                        break;
                    }
                }
            }
            // Else -> Flight has NO Special Request Code
            else
            {
                // Search for an unassigned Boarding Gate that has no Special Request Code
                foreach (BoardingGate boardingGate in t5.BoardingGates.Values)
                {
                    if (BoardingGateSupportsFlight(boardingGate, flightDequeue) && boardingGate.Flight == null)
                    {
                        boardingGate.Flight = flightDequeue; // assign boarding gate to flight
                        countProcessed++;
                        break;
                    }
                }
            }
        }
        // Display flight's 5 basic info + Special Request Code & Boarding Gate Name
        ListAllFlights(t5, true);

        // Calculate total number of Boarding Gates assigned a Flight.
        foreach (BoardingGate boardingGate in t5.BoardingGates.Values)
        {
            if (boardingGate.Flight != null) // if boarding gate got flight
            {
                countAssigned++;
            }
        }

        // Display the total number of Flights and Boarding Gates processed and assigned
        Console.WriteLine($"Number of Flights & Boarding Gates processed & assigned: {countProcessed}");

        // Display the total number of Flights and Boarding Gates that were processed automatically
        //  over those that were already assigned as a PERCENTAGE
        double percentage = (double)countProcessed / countAssigned * 100;
        Console.WriteLine($"Percentage of automatically processed Flights & Boarding Gates over those already assigned: {percentage:f2}%");

    }

    // ADVANCED FEATURE (b) // -> Haydn
    private static void DisplayAirlineFees(Terminal t5)
    {
        // Find an unassigned flight, then display error message if there is one
        Flight? unassignedFlight = t5.Flights
            .FirstOrDefault(
            x => (GetFlightBoardingGate(t5, x.Value) == null)
            ).Value;
        if (unassignedFlight != null)
        {
            Console.WriteLine(
                   "Not all flights have been assigned boarding gates! \r\n" +
                   "Please ensure that all flights have been assigned a boarding gate before running this feature!");
            return;
        }

        Console.WriteLine(
           "=============================================  \r\n" +
           "Total Fee Per Airline For the Day  \r\n" +
           "=============================================");

        t5.PrintAirlineFees();
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
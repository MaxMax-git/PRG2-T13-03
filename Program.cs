using PRG2_T13_03;
using PRG2_T13_03.Classes;
using PRG2_T13_03.Classes.Flights;
using System;
using System.Net.Http.Headers;

class Program
{
    // Values
    static Terminal t5 = new Terminal("Terminal5");


    // pt1. -> Load the airlines.csv file.
    private static void LoadAirlines()
    {
        // Header
        Console.WriteLine("Loading Airlines...");

        // Airlines loaded count
        int count = 0; 

        // use StreamReader to read the datafile
        using (StreamReader sr = new StreamReader("Data/airlines.csv")) // reads the Airlines file.csv
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
                    count++; // increment by 1
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
        using (StreamReader sr = new StreamReader("Data/boardinggates.csv")) // reads the Boarding Gates file.csv
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
                    count++; // increment by 1
                }
            }

            // Display the number of loaded Boarding Gates
            Console.WriteLine($"{count} Boarding Gates Loaded!");
        }
    }

    static void Main(string[] args)
    {
        // Load the Airlines
        LoadAirlines();

        // Load the Boarding Gates
        LoadBoardingGates();

    }
}
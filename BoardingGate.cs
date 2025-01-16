using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class BoardingGate
{
    // Properties
    public string GateName { get; set; }
    public bool SupportsCFFT { get; set; }
    public bool SupportsDDJB { get; set; }
    public bool SupportsLWTT {  get; set; }
    public Flight flight { get; set; }

    // Constructor
    public BoardingGate() { } // default constructor
    public BoardingGate() // parameterized consturctor
    {
        //
    }

    // Methods
    public double CalculateFees()
    {
        //
    }

    public override string ToString()
    {
        //
    }






}
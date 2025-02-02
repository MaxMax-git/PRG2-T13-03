using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_T13_03.Classes
{
    public class WeatherForecast
    {
        public int code { get; set; }
        public Data data { get; set; }
        public string errorMsg { get; set; }
    }

    public class Data
    {
        public Record[] records { get; set; }
    }

    public class Record
    {
        public string date { get; set; }
        public DateTime updatedTimestamp { get; set; }
        public General general { get; set; }
        public Period[] periods { get; set; }
        public DateTime timestamp { get; set; }
    }

    public class General
    {
        public Temperature temperature { get; set; }
        public Relativehumidity relativeHumidity { get; set; }
        public Forecast forecast { get; set; }
        public Validperiod validPeriod { get; set; }
        public Wind wind { get; set; }
    }

    public class Temperature
    {
        public int low { get; set; }
        public int high { get; set; }
        public string unit { get; set; }
    }

    public class Relativehumidity
    {
        public int low { get; set; }
        public int high { get; set; }
        public string unit { get; set; }
    }

    public class Forecast
    {
        public string code { get; set; }
        public string text { get; set; }
    }

    public class Validperiod
    {
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string text { get; set; }
    }

    public class Wind
    {
        public Speed speed { get; set; }
        public string direction { get; set; }
    }

    public class Speed
    {
        public int low { get; set; }
        public int high { get; set; }
    }

    public class Period
    {
        public Timeperiod timePeriod { get; set; }
        public Regions regions { get; set; }
    }

    public class Timeperiod
    {
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string text { get; set; }
    }

    public class Regions
    {
        public Weather west { get; set; }
        public Weather east { get; set; }
        public Weather central { get; set; }
        public Weather south { get; set; }
        public Weather north { get; set; }
    }

    public class Weather
    {
        public string code { get; set; }
        public string text { get; set; }
    }
}

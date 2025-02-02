<a id="readme-top"></a>

# Flight Information Display System (FIDS) - Terminal 5
![.NET Version](https://img.shields.io/badge/.NET-6.0-blueviolet)
![License](https://img.shields.io/badge/License-MIT-green)
A C# console application for managing flight data, boarding gate assignments, and terminal fees at Changi Airport Terminal 5.

# Background 
Singapore has begun construction on Terminal 5 at Changi Airport, and you have been tasked with developing
a Flight Information Display System (FIDS) that will provide real-time flight information to passengers.
The system will use electronic display boards and monitors throughout Terminal 5 to display and provide 
vital information regarding arriving and departing flights, including Flight Numbers, Airline Names, 
City of Origin or Destination, Expected Departure/Arrival Time, and Flight Status. 

[![Typing SVG](https://readme-typing-svg.demolab.com?font=Fira+Code&pause=1000&width=435&lines=Flight+Information+Display+System)](https://git.io/typing-svg)

---

## Table of Contents
- [Features](#features)
- [Class Diagram](#class-diagram)
- [Installation](#installation)
- [Usage](#usage)
- [Data Files](#data-files)
- [Team Contributions](#team-contributions)
- [License](#license)

## Features

### Core Functionality
| Feature | Description |
|---------|-------------|
| 📁: Load Data Files | Load the Airlines, Boarding Gates & Flight files. |
| 📰: List Info | List All Boarding Gates/ Flights with their basic or full info. |
| ✈️: Flight Management | Create/modify/delete flights with special requests. |
| ⏲️: Process Unassigned Flights | Process all Unassigned Flights to Boarding Gates in bulk. |
| 🚪: Boarding Gate Assignment | Manual and bulk assignment with conflict checking. |
| :bar_chart:: Financial Reports | Daily fee calculations with promotional discounts. |
| :clock3:: Chronological Display | Flights sorted by departure/arrival time. |
| ✔️: Input Validation | Handle all invalid inputs by the user. |

### Technical Implementation
- Object-Oriented Design (8 classes)
- CSV data parsing (`airlines.csv`, `flights.csv`, `boardinggates.csv`)
- IComparable<T> interface for flight sorting
- Exception handling and input validation

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## Class Diagram
- Terminal.cs
- BoardingGate.cs
- Airline.cs
- Flight.cs (Parent Class)
  - NORMFlight
  - LWTTFlight
  - DDJBFlight
  - CFFTFlight
- WeatherAPI.cs

 <p align="right">(<a href="#readme-top">back to top</a>)</p>
 
## Installation

### Prerequisites
- [.NET 6.0 SDK](https://dotnet.microsoft.com/download)

### Setup
```bash
# Clone repository
git clone https://github.com/[your-username]/[your-repo-name].git

# Navigate to project directory
cd [your-repo-name]/src

# Restore dependencies
dotnet restore

# IMPORTANT: Ensure that required CSV data files are in the DATA Folder.
# CSV Files Need: airlines.csv , boardinggates.csv, flights.csv, weathersToCancel.csv, weathersToDelay.csv
# If missing, download or copy them into the "Data" folder

# Run application
dotnet run
```

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## Usage
### Main Menu Overview

When you run the application, you’ll see the following menu:  
*(Options are selected by entering the corresponding number.)*

```text
=============================================
Welcome to Changi Airport Terminal 5
=============================================
1. List All Flights
2. List Boarding Gates
3. Assign a Boarding Gate to a Flight
4. Create Flight
5. Display Airline Flights
6. Modify Flight Details
7. Display Flight Schedule
8. Process All Unassigned Flights
9. Display Total Fee Per Airline For the Day
10. Display 24-hour Weather Forecast
11. Bulk Reschedule Flights
12. Cancel Flight
13. Bulk Cancel Flights
0. Exit

Please select your option:
```

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## Data Files
There a total of 5 CSV Data Files needed to run this program. They are found in the "Data" Folder.
- airlines.csv
- boardinggates.csv
- flights.csv
- weathersToCancel.csv
- weathersToDelay.csv

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## Team Contributions
Contributions are what make the open source community such an amazing place to learn, inspire, and create. Any contributions you make are **greatly appreciated**.

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".
Don't forget to give the project a star! Thanks again!

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### Contributors
<a href="https://github.com/MaxMax-git/PRG2-T13-03/graphs/contributors">
  <img src="https://contrib.rocks/image?repo=MaxMax-git/PRG2-T13-03"alt="Contributors" />
</a>

### Authors
Low Yu Wen Max (S10266723)

Moh Journ Haydn (S10267518J)

<p align="right">(<a href="#readme-top">back to top</a>)</p>

## License

Distributed under the MIT License. See `LICENSE.txt` for more information.

<p align="right">(<a href="#readme-top">back to top</a>)</p>



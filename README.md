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
- [Installation & Usage](#installation--usage)
- [Data Files](#data-files)
- [Team Contributions](#team-contributions)
- [Grading Criteria](#grading-criteria)
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

## Class Diagram
- Terminal.cs
- BoardingGate.cs
- Airline.cs
- Flight.cs
  - NORMFlight
  - LWTTFlight
  - DDJBFlight
  - CFFTFlight
 

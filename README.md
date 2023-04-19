# VRPTW

A naive solution to the Vehicle Routing Problem with Time Windows written in C#. It works by generating all the possible route plans, 
filtering out duplicates and rejecting the invalid ones, then selecting the best one.

## Setup

Modify the parameters in `Program.cs` to suit your needs and run the app.

## Data

The app was tested using the R101 instance of the Solomon benchmark problems, [sourced from here](https://github.com/CervEdin/solomon-vrptw-benchmarks).

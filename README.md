# Load Balancer Middleware for .NET Web API
This project implements a custom load balancing middleware for a .NET Web API. The middleware can distribute incoming HTTP requests to multiple server instances using different load balancing algorithms.

## Features
Load Balancing Algorithms: Supports equally distributed, emphasis on first receiver, emphasis on second receiver, and random distribution.
Middleware Integration: Easily integrates with the .NET middleware pipeline.
Configuration: Load balancer settings are configurable through appsettings.json.
Request History: Keeps track of request history.

## Project Structure
LoadBalancer.Balancer: Contains the core logic for load balancing.
LoadBalancer.Balancer.Extension: Provides an extension method for adding the load balancing middleware to the application pipeline.
LoadBalancer.Models: Defines the models used in the load balancer.
Getting Started

## Prerequisites
.NET 8.0 SDK or later

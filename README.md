# Introduction

[![Build Status](https://dev.azure.com/dimenicsbe/Utilities/_apis/build/status/dimenics.i18n?repoName=dimenics%2Fi18n&branchName=master)](https://dev.azure.com/dimenicsbe/Utilities/_build/latest?definitionId=134&repoName=dimenics%2Fi18n&branchName=master) ![Code coverage](https://img.shields.io/azure-devops/coverage/dimenicsbe/Utilities/134/master) ![CodeQL](https://github.com/dimenics/i18n/workflows/CodeQL/badge.svg)

Dime.i18n aims to make internationalization in .NET just a little bit easier with a set of helper classes and methods.

## Getting Started

- You must have Visual Studio 2019 Community or higher.
- The dotnet cli is also highly recommended.

## About this project

This repository is split it in several projects. Each one has a specific focus:

- Dime.i18n.Date is there to convert `DateTime` objects from UTC to the local (i.e. user-defined) time zone to UTC and vice versa.
- Dime.i18n.Countries provides some capabilities to retrieve a list of the world's countries.

## Build and Test

- Run dotnet restore
- Run dotnet build
- Run dotnet test

## Installation

Use the package manager NuGet to install Dime.i18n:

`dotnet add package Dime.i18n.{SubProjectName}`

## Usage

``` csharp
using System.Globalization;

[HttpGet]
public IEnumerable<Customer> Get()
{
     string timezone = CurrentUser.GetTimeZone(); // Fetch time zone from HTTP Context
     IEnumerable<Customer> customers = MyDbContext.Customers.ToList(); // Dates are stored in UTC

     UtcDateTimeConverter converter = new UtcDateTimeConverter(timezone);

     // Use custom extension called Tap to iterate through each item without changing the return type and object
     // The code inside the tap method is not necessarily a good practice but it shows the power of this library
     return customers.Tap(x => x.Date = converter.ConvertToLocalTime(x.Date));
}

[HttpGet]
public IEnumerable<Customer> GetWithExtension()
{
     string timezone = CurrentUser.GetTimeZone(); // Fetch time zone from HTTP Context
     IEnumerable<Customer> customers = MyDbContext.Customers.ToList(); // Dates in database should be stored in UTC

     // Use custom extension called Tap to iterate through each item without changing the return type and object
     // The code inside the tap method is not necessarily a good practice but it shows the power of this library
     return customers.Tap(x => x.Date = x.Date.ToLocal("America/New_York"));
}
```

## Contributing

![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg?style=flat-square)

Pull requests are welcome. Please check out the contribution and code of conduct guidelines.

## License

[![License](http://img.shields.io/:license-mit-blue.svg?style=flat-square)](http://badges.mit-license.org)

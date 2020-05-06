# Introduction 

![Build Status](https://dev.azure.com/dimenicsbe/Utilities/_apis/build/status/dimenics.dime-linq?branchName=master) [![Dime.Linq package in Dime.Scheduler feed in Azure Artifacts](https://feeds.dev.azure.com/dimenicsbe/_apis/public/Packaging/Feeds/a7b896fd-9cd8-4291-afe1-f223483d87f0/Packages/a4ea1a44-b4ee-49dd-ba2f-eff013a1c9ce/Badge)](https://dev.azure.com/dimenicsbe/Utilities/_packaging?_a=package&feed=a7b896fd-9cd8-4291-afe1-f223483d87f0&package=a4ea1a44-b4ee-49dd-ba2f-eff013a1c9ce&preferRelease=true)

Dime.Internationalization aims to make internationalization in .NET just a little bit easier with a set of helper classes and methods.

## Getting Started

- You must have Visual Studio 2019 Community or higher.
- The dotnet cli is also highly recommended.

## About this project

This repository is split it in several projects. Each one has a very specific focus:

- Dime.Internationalization.Date is there to convert `DateTime` objects from UTC to the local (i.e. user-defined) time zone to UTC and vice versa.
- Dime.Internationalization.Countries provides some capabilities to retrieve a list of the world's countries.

## Build and Test

- Run dotnet restore
- Run dotnet build
- Run dotnet test

## Installation

Use the package manager NuGet to install Dime.Internationalization:

`dotnet add package Dime.Internationalization.{SubProjectName}`

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
```

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

# License

[![License](http://img.shields.io/:license-mit-blue.svg?style=flat-square)](http://badges.mit-license.org)
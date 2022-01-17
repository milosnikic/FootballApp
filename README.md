# FootballApp
## _Application that makes your football experience fantastic_

FootballApp is a web application for creating football matches, finding new friends, organizing football matches, tracking your statistics et.c,

## Some of features:
- Creating football matches
- Organizing people inside groups
- Adding new friends
- Tracking personal and other users statistics
- Communicating with indivdual/group of friends
- Tracking your upcoming matches
- Tracking history of your previous matches

## Tech

FootballApp technologies used:

- [Dotnet Core] - Free. Cross-platform. Open source!
- [MSSQL Server] - SQL Server, ideal for development and production for desktop, web, and small server applications.
- [Angular2] - One framework. Mobile & desktop.
- [XUnit] - xUnit is a free, open source, community-focused unit testing tool for the .NET Framework
- [Bootstrap] - Build fast, responsive sites with Bootstrap

## Installation

Dillinger requires [Node.js](https://nodejs.org/) v10+, [.NET Core](https://dotnet.microsoft.com/en-us/download) 2.1.x, [Angular](https://angular.io/) 8 and [MSSQL Server] to run.

Install the dependencies and clone the repository.

```sh
git clone <repository_link>
cd FootballApp
```

After that create global.json file in root of the folder specifying the exact version of installed .net core skd.
```json
{
    "sdk": {
      "version": "2.1.803"
    }
}
```

After that you need to install npm packages.
```sh
cd FootballApp-SPA
npm install
```

After the packages are installed, you should change the connection string in appsettings.Development.json
```json
"ConnectionStrings": {
    "DefaultConnection": "Server=SERVER_NAME;Database=FootballApp;User Id=sa; Password=Test123*"
  },
```
Insted of SERVER_NAME put your server name.

Alternatively if you are running MSSQL Server in Docker container, change your appsettings.Development.json as follows:

```json
"ConnectionStrings": {
    "DefaultConnection": "Server=127.0.0.1, 1433;Database=FootballApp;User Id=sa; Password=Test123*"
  },
```
And after that being applied, you can start backend application.
```sh
cd FootballApp.API
dotnet run
```

And frontend application as well
```sh
cd FootballApp-SPA
npm start
```


## Docker

Support for docker will be added.


[//]: # (These are reference links used in the body of this note and get stripped out when the markdown processor does its job. There is no need to format nicely because it shouldn't be seen. Thanks SO - http://stackoverflow.com/questions/4823468/store-comments-in-markdown-syntax)
   [Bootstrap]: <https://getbootstrap.com/>
   [MSSQL Server]: <https://www.microsoft.com/en-us/sql-server/sql-server-downloads>
   [Dotnet Core]: <https://dotnet.microsoft.com/en-us/download>
   [Angular2]: <https://angular.io/>
   [XUnit]: <https://xunit.net/>

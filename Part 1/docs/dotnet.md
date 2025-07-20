# Technology Stack
The following outlines the technology stack to be used.
Whenever possible, use the context7 MCP to look up applicable reference documentation.

## Project Name
Becore creating solution and project files, ask for the project name which will be used to name the .sln and .csproj files as defined in the Project Structure section.

## Dotnet
- **C#**
- **dotnet framework 10**
- **xUnit**
- **Entity Framework**

### Commands
#### The following path should be used for dotnet commands
- **dotnet path**: `/mnt/c/Program Files/dotnet/dotnet.exe`
- **Database scaffolding**: `dotnet ef dbcontext scaffold` with connection string. Before this command is run, ask for the connection string parameters which are "host","port","database","username","password". These should be used to build the connection string. Append "(TrustServerCertificate=true)" to the connection string at the end.

### Project Structure
- **[Project Name].sln** - Dotnet solution. All projects should be added to this.
- **[Project Name].Shared.csproj** - Shared class library. All projects should reference this project. All shared code will go here. Some examples would be Entity Framework models, Helpers, Utilities, Domain Models.
- **[Project Name].Integration.Tests** - Class library. All integration tests should go here
- **[Project Name].Unit.Tests** - Class library. All unit tests should go here
- **[Project Name].ConsoleRunner.csproj** - Console Application. This will be a Command line interface project designed so users can interact with the system. 
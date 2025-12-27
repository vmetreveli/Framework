---
description: 'Guidelines for building C# applications'
applyTo: '**/*.cs'
---

# C# Development

## C# Instructions
- Always use the latest version C#, currently C# 13 features.
- Write clear and concise comments for each function except code that is evident.

## General Instructions
- Make only high confidence suggestions when reviewing code changes.
- Write code with good maintainability practices, comments should explain why and business logic, not what the code does.
- Handle edge cases and write clear exception handling.

## Formatting

- Prefer file-scoped namespace declarations and single-line using directives.
- Insert a newline before the opening curly brace of any code block (e.g., after `if`, `for`, `while`, `foreach`, `using`, `try`, etc.).
- Ensure that the final return statement of a method is on its own line.
- Use pattern matching and switch expressions wherever possible.
- Use `nameof` instead of string literals when referring to member names.

## .NET Good Practices

- Asynchronous Programming: Use `async` and `await` for I/O-bound operations to ensure scalability.
- Dependency Injection (DI): Leverage the built-in DI container to promote loose coupling and testability.
- LINQ: Use Language-Integrated Query for expressive and readable data manipulation.
- Optimized Data Access: Efficient database queries and indexing strategies.
- Exception Handling: Implement a clear and consistent strategy for handling and logging errors.

## Testing

- Always include test cases for critical paths of the application.
- Add comprehensive tests following naming conventions.
- Do not emit "Act", "Arrange" or "Assert" comments.
- Demonstrate how to mock dependencies for effective testing.
- Use xUnit for testing and NSubstitute for mocking.
- Copy existing style in nearby files for test method names and capitalization.

## System Design Considerations

- We have designed our application with Clean Architecture
- We have also brought concepts from CQRS to break Application Services (a.k.a. Use Case Services) into smaller operation level classes in the form of commands and queries
- Command will be a complex operation which will change the system state.
- Query will be a read operation, which doesn't change any state of the system.
- For queries:
  - Handler will orchestrate required steps for a given operation.
  - Handler uses repository to get data from DB.
  - Handler may utilize infrastructure services to get data from 3rd party API.
- For Commands:
  - A write operation that changes the system state.
  - Handler will orchestrate required steps for a given operation.
  - Handler uses repository to write data to DB.
  - Handler may utilize infrastructure services to get data from 3rd party API.

# Project Layers

* API
  * `Controllers` folder for controllers
* Application
  * `Features` folder for the features of the service (a place for commands and queries)
  * `HttpClients` folder for [RestEase] interfaces
  * `Options` folder for various configuration classes (to use with `appsettings`)
  * `Repositories` folder for repository interfaces
  * `Resources` folder for localization
  * `Services` folder for service interfaces and some implementations as well (under tenant specific folders). Services are useful when you want to [have tenant specific implementations](https://github.com/SpaceBank/Space.Service.Common.Factory), or just abstract things out. Application layer is a place for services that contain business logic
* Domain
  * `Constants` folder for some hardcoded strings, etc.
  * `Entities` folder for EF Core entities
  * `Enums` folder for enums that are used in entities, or throughout various layers/places in the project
  * `Exceptions` folder for [custom exceptions]
* Infrastructure
  * `Services` folder for service implementations (under tenant specific folders if necessary). Infrastructure layer is place for helper services (e.g. `DatetimeService`), services that are wrappers for external communication (e.g. a service that uses RestEase interface and adds some request/response handling on top of that) or other kind of services that do not contain business logic
  * `Workers` folder for [background services]
* Persistence
  * `Configurations` folder for EF Core entities configurations (how entity tables are represented in DB)
  * `Migrations` folder for migrations generated automatically by [EF Core migrations](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli)
  * `Repositories` folder for repository implementations

 **Important notes:** 

***Application layer should be the only layer that contains business logic***

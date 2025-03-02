# DrinkingPassion API Development Guide

## Build/Test Commands
- Build solution: `dotnet build DrinkingPassion.sln`
- Run API: `dotnet run --project API/API.csproj`
- Run tests: `dotnet test`
- Run specific test class: `dotnet test --filter "FullyQualifiedName~Tests.Core.Extensions.StringExtensions.SlugifyReturnsCorrectString"`
- Run specific test method: `dotnet test --filter "Name=SentenceReturnsCorrectSlug"`
- Format code: `dotnet format`

## Database
- Docker: `docker-compose up -d` (runs PostgreSQL and Adminer)
- Migration: `dotnet ef migrations add <Name> --project Infrastructure --startup-project API`
- Update database: `dotnet ef database update --project Infrastructure --startup-project API`

## Code Style Guidelines
- **Naming**: PascalCase for classes, properties, methods; camelCase for variables
- **DTO pattern**: Use DTOs for data transfer between layers
- **Repository pattern**: Use specifications for query encapsulation
- **Error handling**: Use middleware for global exception handling
- **Validation**: Use attributes for validation on DTOs
- **Folder structure**: Group by feature within appropriate layer (Core/Infrastructure/API)
- **Tests**: Follow Arrange/Act/Assert pattern for tests
- **Imports**: Group and order: System > External > Project
- **Async/await**: Use async/await for all I/O operations
- **Entity config**: Use configuration classes for entity type configs
- **Documentation**: XML comments for public APIs
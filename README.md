## Basic info
DrinkingPassion is an application whose main goal is to help hobbyist bartenders follow their passion. The app lets you, besides browsing and filtering cocktail recipes, create and manage your positions. Give it a try!  
The app has a Polish interface because I wanted to spread it among some of my friends who don't speak English very well.  

## Technology Stack

The app consists of two parts: a REST API and a client application.

### API
- **.NET 9**: Modern C# features with nullable reference types
- **PostgreSQL**: Relational database with Entity Framework Core
- **FluentValidation**: Robust request validation
- **Repository Pattern**: With specification pattern for queries
- **JWT Authentication**: For secure API access
- **Azure Services**: Key Vault for secrets, Blob Storage for images

### Client
- **Blazor WebAssembly**: For rich interactive UI
- **Web API Integration**: For data fetching and state management
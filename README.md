## Basic info
DrinkingPassion is an application whose main goal is to help hobbyist bartenders follow their passion. The app lets you, besides browsing and filtering cocktail recipes, create and manage your positions. Give it a try!  
The app has a Polish interface because I wanted to spread it among some of my friends who don't speak English very well.  

## Technology stack
The app consists of two parts: a REST API and a client application.

### API
The API project is written in .NET 7. As a database provider, a PostgreSQL was chosen (free DB on Heroku up to 10k rows was the main reason). I use some of Azure's functionalities, such as Azure Key Vault - to safely store secrets, and Azure Blob Storage - to store uploaded images.

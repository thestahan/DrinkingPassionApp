# DTO Naming Conventions

The project uses consistent naming conventions to clearly distinguish between DTOs used for commands (writes) and queries (reads).

## Interfaces

All DTOs implement one of these marker interfaces:
- `ICommandDto`: For DTOs used in command operations (create, update, delete)
- `IQueryDto`: For DTOs used in query operations (read/get)

## Naming Patterns

### Command DTOs 
- `{Entity}ToAddDto`: For creating new entities
- `{Entity}ToUpdateDto`: For updating existing entities
- `{Entity}ToDeleteDto`: For deletion operations (if needed)

### Query DTOs
- `{Entity}ToReturnDto`: For basic entity data in listings
- `{Entity}DetailsToReturnDto`: For detailed entity information 
- `{Entity}BasicInfoDto`: For minimal entity information

## Organization
DTOs are organized by domain entity in subdirectories to maintain domain cohesion.

## Examples

Command DTOs:
- `ProductToAddDto`
- `CocktailInfoToUpdateDto`
- `UserRegisterDto`

Query DTOs:
- `ProductToReturnDto`
- `CocktailDetailsToReturnDto`
- `UserDetailsDto`
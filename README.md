# NetCoreWebApi

## Professional CSharp Resources

- [ProfessionalCSharp7 Published By Apress](https://github.com/ProfessionalCSharp/ProfessionalCSharp7)
- [ProfessionalCSharp6 Published By Apress](https://github.com/ProfessionalCSharp/ProfessionalCSharp6)
- [C# 8.0 and .NET Core 3.0, by Mark J. Price,4th Edition](https://github.com/PacktPublishing/CSharp-8.0-and-.NET-Core-3.0-Modern-Cross-Platform-Development-Fourth-Edition)
- [C# 8.0 and .NET Core 3.1, by Mark J. Price Repo](https://github.com/markjprice/cs8dotnetcore3)
- [C# 8 and .NET Core 3.0 New Features](https://github.com/PacktPublishing/C-8-and-.NET-Core-3.0-New-Features)
- [ASP.NET Core 3.1 2020 Blog Series](https://wakeupandcode.com/aspnetcore/#aspnetcore2020)
- [Routing in ASP.NET Core 3.1](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/routing?view=aspnetcore-3.1)
- [Microsoft DotNet Architecture](https://github.com/dotnet-architecture/eShopOnWeb)


## EntityFramework with SQLite

### EF for SqlLite 

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```
### share code between multiple projects/assemblies

- Separating things by project ensures decisions about dependency direction are enforced by the compiler, helping avoid careless mistakes. Separating into projects isn't solely about individually deploying or reusing assemblies.
- Verify that the shared library is a Class Library project targeting .NET Standard 2.1. Check out the official docs to learn more about how to pick a version of .NET Standard for your class libraries.



## BookServiceApi Client

- https://docs.microsoft.com/en-us/dotnet/architecture/microservices/
- https://github.com/dotnet-architecture/eShopOnContainers

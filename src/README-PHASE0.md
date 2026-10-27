# Phase 0 – what to copy where

Replace/add these files in your solution (same relative paths):

- Root: Directory.Build.props, global.json, .editorconfig, .gitignore
- MyApp.Domain/MyApp.Domain.csproj (only if yours has extra settings; otherwise just remove duplicated properties)
- MyApp.Application/** (rename folder MyApplication -> MyApp.Application first; delete the old files)
- MyApp.Infrastructure/MyApp.Infrastructure.csproj and DependencyInjection.cs
- MyApp.Api/** (csproj, appsettings*.json, Program.cs, Controllers)

Untouched: all Domain .cs files (just delete Class1.cs), Persistence/* in Infrastructure.

Then:
  dotnet new sln -n MyApp            (if you have no .sln)
  dotnet sln add MyApp.Domain MyApp.Application MyApp.Infrastructure MyApp.Api
  dotnet tool install --global dotnet-ef
  dotnet build
  dotnet ef migrations add InitialCreate -p MyApp.Infrastructure -s MyApp.Api -o Persistence/Migrations
  dotnet ef database update -p MyApp.Infrastructure -s MyApp.Api

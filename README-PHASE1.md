# Phase 1: apply

1. Copy src\, tests\, .github\ and docs\ from this zip into D:\MySolution (merge folders).
   All files are new, nothing existing is replaced.

2. From D:\MySolution, create the test project and wire it up:

   dotnet new xunit -n MyApp.Domain.Tests -o tests\MyApp.Domain.Tests
   del tests\MyApp.Domain.Tests\UnitTest1.cs
   dotnet sln add tests\MyApp.Domain.Tests\MyApp.Domain.Tests.csproj
   dotnet add tests\MyApp.Domain.Tests\MyApp.Domain.Tests.csproj reference src\MyApp.Domain\MyApp.Domain.csproj
   dotnet add tests\MyApp.Domain.Tests\MyApp.Domain.Tests.csproj package Shouldly

   If `dotnet new xunit` already created a file with different content in the folder,
   keep the generated .csproj and only add the .cs files from this zip.

3. dotnet build
   dotnet test

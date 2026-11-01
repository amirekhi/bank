# Auth step: how to apply

1. Copy every file in this zip over your solution (same relative paths).
   DepositMoneyCommand.cs and WithdrawMoneyCommand.cs are unchanged and not included.
2. Packages:
   dotnet add MyApp.Api package Microsoft.AspNetCore.Authentication.JwtBearer --version 9.0.0
   dotnet add MyApp.Infrastructure package Microsoft.IdentityModel.JsonWebTokens
   dotnet add MyApp.Infrastructure package Microsoft.Extensions.Identity.Core --version 9.0.0
3. Secrets (run inside MyApp.Api):
   dotnet user-secrets init
   dotnet user-secrets set "Jwt:SigningKey" "<generated key>"
   dotnet user-secrets set "Seed:Admin:Email" "admin@myapp.local"
   dotnet user-secrets set "Seed:Admin:Password" "<at least 12 characters>"
4. Recreate the database (dev data is wiped):
   delete MyApp.Infrastructure/Persistence/Migrations
   dotnet ef database drop -f -p MyApp.Infrastructure -s MyApp.Api
   dotnet ef migrations add InitialCreate -p MyApp.Infrastructure -s MyApp.Api -o Persistence/Migrations
   dotnet ef database update -p MyApp.Infrastructure -s MyApp.Api
5. dotnet run --project MyApp.Api --launch-profile https

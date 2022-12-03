# Zapiski

## Navodila
### Vaja 04

Kot osnovo za začetek vzamemo rešitev vaje 03.


1. V rešitev namestimo knjižnico `Microsoft.AspNetCore.Identity.EntityFrameworkCore` in `Microsoft.AspNetCore.Identity.UI` aktualne verzije. Uporabimo NuGet package manager (Package manager, .NET cli ali PackageReference v .csproj).
```bash
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore --version 6.0.10
dotnet add package Microsoft.AspNetCore.Identity.UI --version 6.0.10
dotnet add package Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore --version 6.0.10
```
2. V Models dodamo nov razred `ApplicationUser.cs`, ki deduje razred `IdentityUser` (
iz knjižnice `Microsoft.AspNetCore.Identity`). Razredu dodamo atribute `FirstName`, `LastName`, `City`. 
```csharp
using Microsoft.AspNetCore.Identity;

namespace web.Models;

public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? City { get; set; }

}
```
3. V datoteki `SchoolContext.cs` dodamo `using Microsoft.AspNetCore.Identity.EntityFrameworkCore;` in spremenimo dedovanje (`: IdentityDbContext<ApplicationUser>`) in dodamo vrstico `base.OnModelCreating(modelBuilder);` v `OnModelCreating()`.
4. Ustvarimo migracije in jih izvedemo na bazo.
```bash
dotnet ef migrations add AppUser
# izbrišemo obstoječo bazo s podatki in nato ustvarimo novo
dotnet ef database drop
dotnet ef database update
```
5. Zaženemo generator kode za Identity:
```bash
dotnet-aspnet-codegenerator identity -dc web.Data.SchoolContext --generateLayout

#ali (samo določene view-e)
dotnet-aspnet-codegenerator identity -dc web.Data.SchoolContext -fi "Account.Register;Account.Login;Account.Logout;Account.RegisterConfirmation" --generateLayout
```

6. Zamenjaj v Program.cs:
```csharp
// uvozi
using web.Models;

// nastavi spremenljivko connectionString za .useSqlServer(connectionString)
var connectionString = builder.Configuration.GetConnectionString("SchoolContext");

// odstrani stari .AddDbContext
//builder.Services.AddDbContext<SchoolContext>(options =>
//            options.UseSqlServer(builder.Configuration.GetConnectionString("SchoolContext")));

// prilagodi RequireConfirmedAccount = false in .AddRoles<IdentityRole>()
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<SchoolContext>();

// dodatj app.MapRazorPages(); (npr. za app.useAuthentication())
app.MapRazorPages();
```

7. Dodamo `<partial name="_LoginPartial" />` v `/Views/Shared/_Layout.cshtml` (na koncu znotraj elementa `<div class="navbar-collapse..."></div>`).
8. Dodamo avtorizacijo na posamezne metode v StudentsController.cs in nad celoten CourseController. `[Authorize]` in `using Microsoft.AspNetCore.Authorization;`
9. Razširimo Course.cs z lastnostmi `Owner`, `DateCreated`, `DateEdited`.
10. Posodobimo bazo.
```bash
dotnet ef migrations add ExtendCourse
dotnet ef database update
```
11. Ob kreiranju novega predmeta v `Course.OwnerId` zapišemo id trenutno prijavljenega uporabnika.
12. V tabelo `AspNetRoles` dodaj 3 različne vloge (Administrator, Manager, Staff). Vloge dodeliš uporabniku z zapisi v tabeli AspNetUserRoles (zapis: id vloge | id uporabnika).
13. Omeji dostop do operacij v CourseController.cs s pomočjo `[Authorize(Roles = "Administrator, Manager, Staff")]`.
14. Dopolni `DbInitializer.cs` za vnos podatkov o uporabniku, vlogah in povezavi med uporabnikom ter vlogo.
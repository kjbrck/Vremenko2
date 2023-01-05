using web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using web.Models;
using web.Services.GetDataService;
using Newtonsoft.Json;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("SchoolContext");

// Add services to the container.
builder.Services.AddControllersWithViews();
//builder.Services.AddDbContext<SchoolContext>(options =>
//            options.UseSqlServer(builder.Configuration.GetConnectionString("SchoolContext")));
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<SchoolContext>();


builder.Services.AddDbContext<SchoolContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddHostedService<TimedHostedService>();

var app = builder.Build();

//test!!
//app.MapGet("/test", () => "Hello World!");
var handler = new Handler();
app.MapGet("/pridobipostaje", handler.GetStations);

app.MapGet("/odstraniuserja/{id}", async (string id) => handler.DeleteUser($"{id}"));

app.MapGet("/posodobipostajo/{id}/{newname}", async (string id, string newname) => handler.UpdateStation($"{id}", $"{newname}"));

app.MapGet("/vnesipostajo/{id}/{name}/{lat}/{lon}/{alt}", async (string id, string name, float lat, float lon, float alt) => handler.AddStation($"{id}", $"{name}", lat, lon, alt));

CreateDbIfNotExists(app);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.MapRazorPages();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

static void CreateDbIfNotExists(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<SchoolContext>();
                    //context.Database.EnsureCreated();
                    DbInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred creating the DB.");
                }
            }
        }

class Handler{
    public string GetStations()
    {
        //return "[\n\t{\n\t\"MeteoId:\": \"metropola\"\n\t}\n]";
        int count = 0;
        for(int i = 0; i < GetDataService.ws.Length; i++){
            if(GetDataService.ws[i] != null)
                count++;
        }
        WeatherStation[] ws2 = new WeatherStation[count];
        for(int i = 0; i < ws2.Length; i++){
            ws2[i] = GetDataService.ws[i];
        }
        var finalJson = JsonConvert.SerializeObject(ws2);
        return finalJson;
    }

    public string DeleteUser(string id){//DELETE USER
        //pokliči metodo, k bo odstranla vnos iz baze
        string query = "DELETE FROM dbo.AspNetUsers where Email = '" + id + "';";
            using(SqlConnection cn=new SqlConnection("Server=uni-db.database.windows.net;Database=University;User Id=university-sa;Password=yourStrong(!)Password;"))
                {
                    using(SqlCommand command = new SqlCommand(query, cn))
                    {
                        cn.Open();
                        command.ExecuteReader();
                        cn.Close();
                        
                    }
                }
        return id;
    }

    public string UpdateStation(string id, string newname){//UPDARE USER
        //pokliči metodo, k bo posodobila vnos v bazi
        return id;
    }

    public string AddStation(string id, string name, float lat, float lon, float alt){//ADD USER
        //pokliči metodo, k bo dodala vnos v bazo
        return id;
    }
}

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using web.Models;
using web.Controllers;
using web.Services.GetDataService;

namespace web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
        
    }

    public IActionResult Index()
    {
        GetDataService.getWeatherStationData();
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    /*
    public static async void ZazeniAsync()
    {
        var timer = new PeriodicTimer(TimeSpan.FromSeconds(1));

        while (await timer.WaitForNextTickAsync())
        {
            XMLtoEntity.Read("https://meteo.arso.gov.si/uploads/probase/www/observ/surface/text/sl/observationAms_si_latest.xml");
        }
    }*/
}

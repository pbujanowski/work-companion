using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WorkCompanion.Identity.Web.Models;

namespace WorkCompanion.Identity.Web.Controllers;

/// <summary>
/// Handles home page and general application views
/// </summary>
public class HomeController : Controller
{
    /// <summary>
    /// Displays the home page
    /// </summary>
    /// <returns>The home page view</returns>
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// Displays the privacy policy page
    /// </summary>
    /// <returns>The privacy policy view</returns>
    public IActionResult Privacy()
    {
        return View();
    }

    /// <summary>
    /// Displays the error page
    /// </summary>
    /// <returns>The error view with request information</returns>
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(
            new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }
        );
    }
}

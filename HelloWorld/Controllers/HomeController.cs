using Microsoft.AspNetCore.Mvc;
using HelloWorld.Models;
using OpenFeature;
using OpenFeature.Model;

namespace HelloWorld.Controllers;

public class HomeController(IFeatureClient featureClient)
    : Controller
{
    public async Task<IActionResult> Index(Languages language)
    {
        ViewData["LocalizationFeatureEnabled"] = await featureClient.GetBooleanValue("localization", false);
        ViewData["Greeting"] = Greeting(language);
        ViewData["Language"] = language;
        return View();
    }

    string Greeting(Languages language)
    {
        return language switch
        {
            Languages.French => "Bonjour",
            Languages.Spanish => "Hola",
            _ => "Hello"
        };
    }
}
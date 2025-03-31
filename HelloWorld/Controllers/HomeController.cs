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
        ViewData["LocalizationFeatureEnabled"] = await EvaluateFeatureToggle("localization"); 
        ViewData["Greeting"] = Greeting(language);
        ViewData["Language"] = language;
        return View();
    }

    async Task<bool> EvaluateFeatureToggle(string flagKey)
    {
        var evaluationContext = EvaluationContext.Builder()
            .Set("licenseType", "free")
            .Build();
        
        return await featureClient.GetBooleanValueAsync(flagKey, false, evaluationContext);
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
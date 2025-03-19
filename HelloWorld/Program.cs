using Octopus.OpenFeature.Provider;
using OpenFeature;
using OpenFeature.Contrib.Providers.EnvVar;
using OpenFeature.Model;

var builder = WebApplication.CreateBuilder(args);

// Add OpenFeature feature toggle client 
var octopusFeatureTogglesClientId = builder.Configuration["FeatureToggles:ClientId"] ?? "";

if (builder.Environment.IsDevelopment())
{
    // If running locally, use the environment variable provider
    await OpenFeature.Api.Instance.SetProviderAsync(new EnvVarProvider("FeatureToggle_"));
}
else
{
    // if running in another environment, use the Octopus provider
    await OpenFeature.Api.Instance.SetProviderAsync(
        new OctopusFeatureProvider(new OctopusFeatureConfiguration(octopusFeatureTogglesClientId)));
}

builder.Services.AddScoped(GetFeatureClient);

// Add MVC controllers 
builder.Services.AddControllersWithViews();

var app = builder.Build();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
return;

static IFeatureClient GetFeatureClient(IServiceProvider serviceProvider)
{
    var client = OpenFeature.Api.Instance.GetClient();
    client.SetContext(EvaluationContext.Builder().Set("segment", "alpha").Build());
    return client;
}
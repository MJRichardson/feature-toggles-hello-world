using Octopus.OpenFeature.Provider;
using OpenFeature;

var builder = WebApplication.CreateBuilder(args);

// Add OpenFeature feature toggle client 
var octopusFeatureTogglesClientId = builder.Configuration["FeatureToggles:ClientId"] ?? "";
await OpenFeature.Api.Instance.SetProviderAsync(
    new OctopusFeatureProvider(new OctopusFeatureConfiguration(octopusFeatureTogglesClientId)));
builder.Services.AddScoped<IFeatureClient>(sp => OpenFeature.Api.Instance.GetClient());

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
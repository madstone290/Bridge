using Blazored.LocalStorage;
using Bridge.Infrastructure.Extensions;
using Bridge.Shared.Constants;
using Bridge.WebApp.Api.ApiClients;
using Bridge.WebApp.Api.ApiClients.Identity;
using Bridge.WebApp.Constants;
using Bridge.WebApp.Services;
using Bridge.WebApp.Services.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using MudBlazor.Services;
using ILocalStorageService = Bridge.WebApp.Services.ILocalStorageService;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("Secrets/encryption_service_config.json");
builder.Services.AddOptionsEx<EncryptionService.Config>(builder.Configuration.GetSection("EncryptionService"));

// Blazor and Razor services
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// MudBlazor servicead
builder.Services.AddMudServices(options =>
{
    options.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopCenter;
    options.SnackbarConfiguration.PreventDuplicates = false;
    options.SnackbarConfiguration.NewestOnTop = false;
    options.SnackbarConfiguration.ShowCloseIcon = true;
    options.SnackbarConfiguration.MaxDisplayedSnackbars = 10;
    options.SnackbarConfiguration.VisibleStateDuration = 5000;
    options.SnackbarConfiguration.ShowTransitionDuration = 0;
    options.SnackbarConfiguration.HideTransitionDuration = 1000;

    options.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(PolicyConstants.AdminOrProvider, policyBuilder =>
    {
        policyBuilder.RequireClaim(ClaimTypeConstants.UserType, ClaimConstants.Admin, ClaimConstants.Provider);
    });
});


builder.Services.AddSingleton<HttpClient>((sp) =>
{
    var apiAddress = builder.Environment.IsProduction()
        ? new Uri(builder.Configuration["ApiUrls:Production"])
        : new Uri(builder.Configuration["ApiUrls:Development"]);

    return new HttpClient()
    {
        BaseAddress = apiAddress
    };
});

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<ILocalStorageService, EncryptionLocationStorageService>();
builder.Services.AddScoped<IEncryptionService, EncryptionService>();
builder.Services.AddScoped<ICookieService, CookieService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => (AuthService)sp.GetRequiredService<IAuthService>());

builder.Services.AddScoped<UserApiClient>();
builder.Services.AddScoped<PlaceApiClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

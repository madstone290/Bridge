using Blazored.LocalStorage;
using Bridge.Infrastructure.Extensions;
using Bridge.Shared.Constants;
using Bridge.WebApp.Api.ApiClients;
using Bridge.WebApp.Api.ApiClients.Admin;
using Bridge.WebApp.Api.ApiClients.Identity;
using Bridge.WebApp.Extensions;
using Bridge.WebApp.Pages.Admin.ViewModels;
using Bridge.WebApp.Pages.Admin.ViewModels.Implement;
using Bridge.WebApp.Pages.Common.ViewModels;
using Bridge.WebApp.Pages.Common.ViewModels.Implement;
using Bridge.WebApp.Pages.Home.ViewModels;
using Bridge.WebApp.Pages.Home.ViewModels.Implement;
using Bridge.WebApp.Services;
using Bridge.WebApp.Services.DynamicMap;
using Bridge.WebApp.Services.DynamicMap.Naver;
using Bridge.WebApp.Services.GeoLocation;
using Bridge.WebApp.Services.Identity;
using Bridge.WebApp.Services.ReverseGeocode;
using Bridge.WebApp.Services.ReverseGeocode.Naver;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using MudBlazor.Services;
using ILocalStorageService = Bridge.WebApp.Services.ILocalStorageService;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.HandleArgs(args);

builder.Configuration.AddJsonFile("Secrets/encryption_service_config.json");
builder.Configuration.AddJsonFile("Secrets/naver_api_config.json");
builder.Services.AddOptionsEx<EncryptionService.Config>(builder.Configuration.GetSection("EncryptionService"));
builder.Services.AddOptionsEx<NaverReverseGeocodeApi.Config>(builder.Configuration.GetSection("NaverApi"));

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
    options.AddPolicy(PolicyConstants.Admin, policyBuilder =>
    {
        policyBuilder.RequireClaim(ClaimTypeConstants.UserType, ClaimConstants.Admin);
    });
    options.AddPolicy(PolicyConstants.AdminOrProvider, policyBuilder =>
    {
        policyBuilder.RequireClaim(ClaimTypeConstants.UserType, ClaimConstants.Admin, ClaimConstants.Provider);
    });
});


builder.Services.AddSingleton<HttpClient>((sp) =>
{
    var urlString = builder.Environment.IsProduction()
        ? builder.Configuration["API_URL"]
        : builder.Configuration["ApiUrls:Development"];
    var apiUrl = new Uri(urlString);
    var clientHandler = new HttpClientHandler
    {
        AllowAutoRedirect = false
    };
    return new HttpClient(clientHandler)
    {
        BaseAddress = apiUrl
    };
});

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<ICommonJsService, CommonJsService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IExcelService, ExcelService>();
builder.Services.AddScoped<IResultValidationService, SnackbarResultValidationService>();
builder.Services.AddScoped<NaverReverseGeocodeApi>();
builder.Services.AddScoped<IReverseGeocodeService, NaverReverseGeocodeService>();
builder.Services.AddScoped<IDynamicMapService, NaverMapService>();
builder.Services.AddScoped<IHtmlGeoService, HtmlGeoService>();
builder.Services.AddScoped<ILocalStorageService, EncryptionLocationStorageService>();
builder.Services.AddScoped<IEncryptionService, EncryptionService>();
builder.Services.AddScoped<ICookieService, CookieService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => (AuthService)sp.GetRequiredService<IAuthService>());

builder.Services.AddScoped<UserApiClient>();
builder.Services.AddScoped<AdminPlaceApiClient>();
builder.Services.AddScoped<AdminProductApiClient>();
builder.Services.AddScoped<AdminRestroomApiClient>();

builder.Services.AddScoped<ProductApiClient>();
builder.Services.AddScoped<PlaceApiClient>();

builder.Services.AddTransient<IPlaceFormViewModel, PlaceFormViewModel>();
builder.Services.AddTransient<IProductFormViewModel, ProductFormViewModel>();
builder.Services.AddTransient<IOpeningTimeViewModel, OpeningTimeViewModel>();

builder.Services.AddTransient<IIndexViewModel, IndexViewModel>();
builder.Services.AddTransient<ILocationSelectViewModel, LocationSelectViewModel>();
builder.Services.AddTransient<IPlaceAddViewModel, PlaceAddViewModel>();
builder.Services.AddTransient<IPlaceDetailViewModel, PlaceDetailViewModel>();

builder.Services.AddTransient<IPlaceListViewModel, PlaceListViewModel>();
builder.Services.AddTransient<IPlaceViewModel, PlaceViewModel>();
builder.Services.AddTransient<IPlaceProductListViewModel, PlaceProductListViewModel>();
builder.Services.AddTransient<IRestroomFormViewModel, RestroomFormViewModel>();


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

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
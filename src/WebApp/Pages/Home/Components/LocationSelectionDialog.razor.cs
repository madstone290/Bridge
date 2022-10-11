using Bridge.WebApp.Services.Maps;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Bridge.WebApp.Pages.Home.Components
{
    public partial class LocationSelectionDialog : IAsyncDisposable
    {
        private const string MapId = "map";

        [CascadingParameter]
        public MudDialogInstance MudDialog { get; set; } = null!;

        [Parameter]
        public double? Latitude { get; set; }

        [Parameter]
        public double? Longitude { get; set; }

        [Parameter]
        public string? Address { get; set; }

        [Inject]
        public IDynamicMapService MapService { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {

            MapService.LocationChanged = new(this, OnLocationChanged);
            MapService.AddressChanged = new(this, OnAddressChanged);

            var mapOptions = new NaverMapService.MapOptions()
            {
                MapId = MapId,
                CenterX = Longitude,
                CenterY = Latitude
            };

            await MapService.InitAsync(mapOptions);
        }
        

        private void Cancel_ClickAsync()
        {
            MudDialog.Cancel();
        }

        private void Ok_ClickAsync()
        {
            if (Latitude.HasValue && Longitude.HasValue)
            {
                MudDialog.Close(DialogResult.Ok(
                    new
                    {
                        Latitude = Latitude.Value,
                        Longitude = Longitude.Value,
                        Address = Address,
                    }));
            }
                
            else
                MudDialog.Cancel();

        }

        private void OnLocationChanged(MapPoint point)
        {
            Latitude = point.Y;
            Longitude = point.X;
        }

        private void OnAddressChanged(string address)
        {
            Address = address;
        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            await MapService.CloseAsync();
        }
    }
}

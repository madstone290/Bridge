using Bridge.WebApp.Api.ApiClients;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Data;

namespace Bridge.WebApp.Pages
{
    public partial class Index
    {
        private string? _searchText;
        private List<string> _placeList = new()
        {
            "1번",
            "2번"
        };

        public double CenterEasting { get; set; } = 10000;
        public double CenterNorthing { get; set; } = 10000;

        [Inject]
        public PlaceApiClient PlaceApiClient { get; set; } = null!;

        public async Task AutoComplete_OnKeyUpAsync(KeyboardEventArgs args)
        {
            if (args.Key == "Enter")
            {
                await SearchPlacesAsync();
            }
        }

        private async Task SearchPlacesAsync()
        {
            _placeList.Clear();

            if (_searchText == null)
            {
                return;
            }

            _placeList.Add(_searchText + "1");
            _placeList.Add(_searchText + "2");
            _placeList.Add(_searchText + "3");
            _placeList.Add(_searchText + "4");
            _placeList.Add(_searchText + "5");
            StateHasChanged();
            //var places = await PlaceApiClient.GetPlacesByNameAndRegion(_searchText, CenterEasting - 3000, CenterEasting + 3000, CenterNorthing - 3000, CenterNorthing + 3000);
            //_placeList.AddRange(places.Select(x=> x.Name));
        }
    }
}

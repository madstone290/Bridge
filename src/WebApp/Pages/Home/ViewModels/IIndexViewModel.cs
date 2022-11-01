using Bridge.WebApp.Pages.Home.Records;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Bridge.WebApp.Pages.Home.ViewModels
{
    public interface IIndexViewModel : IAsyncDisposable
    {
        IHandleEvent Receiver { get; set; }

        Action ForceRender { get; set; }

        string MapElementId { get; }

        bool Searched { get; }

        string SearchText { get; set; }

        LatLon? CurrentLocation { get; set; }

        string? CurrentAddress { get; set; }

        object? SelectedListItem { get; set; }

        PlaceRecord? SelectedPlace { get; set; }

        IEnumerable<PlaceRecord> Places { get; set; }

        EventCallback SearchCompleted { get; set; }

        Task InitAsync();

        Task SearchPlacesAsync();

        Task ShowLocationSelectionAsync();

        Task Handle_KeyUp(KeyboardEventArgs args);

        Task Handle_PlaceSelected(PlaceRecord place);

    }
}

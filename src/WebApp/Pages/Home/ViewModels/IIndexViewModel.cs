using Bridge.WebApp.Pages.Home.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Bridge.WebApp.Pages.Home.ViewModels
{
    public interface IIndexViewModel : IAsyncDisposable
    {
        IHandleEvent Receiver { set; }

        string MapElementId { get; }

        string ListElementId { get; }

        bool Searched { get; }

        string SearchText { get; set; }

        LatLon? CurrentLocation { get; set; }

        string? CurrentAddress { get; set; }

        object? SelectedListItem { get; set; }

        Place? SelectedPlace { get; set; }

        IEnumerable<Place> Places { get; }

        EventCallback SearchCompleted { set; }

        Task InitAsync();

        Task SearchPlacesAsync();

        Task ShowLocationSelectionAsync();

        Task Handle_KeyUp(KeyboardEventArgs args);

        Task Handle_PlaceSelected(Place place);

    }
}

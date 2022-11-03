using Bridge.WebApp.Pages.Home.Models;

namespace Bridge.WebApp.Pages.Home.ViewModels
{
    public interface ILocationSelectViewModel : IModalViewModel, IAsyncDisposable, IEventReceiver
    {
        string MapElementId { get; }

        LatLon? CurrentLocation { get; set; }

        string? CurrentAddress { get; set; }

        Task Initialize();

        Task OnCancelClick();

        Task OnOkClick();

    }
}

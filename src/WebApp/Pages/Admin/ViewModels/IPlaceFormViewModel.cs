using Bridge.WebApp.Pages.Admin.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace Bridge.WebApp.Pages.Admin.ViewModels
{
    public interface IPlaceFormViewModel : IFormViewModel<PlaceModel>, IModalViewModel
    {
        bool IsPlaceValid { get; set; }

        long PlaceId { get; set; }

        PlaceModel Place { get; }

        Task Initialize();

        Task OnImageFileChange(InputFileChangeEventArgs args);

        Task OnCancelClick();

        Task OnSaveClick();
    }
}

using Bridge.WebApp.Pages.Admin.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace Bridge.WebApp.Pages.Admin.ViewModels
{
    public interface IPlaceFormViewModel : IFormValidation<Place>, IModal
    {
        bool IsPlaceValid { get; set; }

        Guid PlaceId { get; set; }

        Place Place { get; }

        Task Initialize();

        Task OnImageFileChange(InputFileChangeEventArgs args);

        Task OnDeleteFileClick();

        Task OnCancelClick();

        Task OnSaveClick();
    }
}

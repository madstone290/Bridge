using Bridge.WebApp.Pages.Admin.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace Bridge.WebApp.Pages.Admin.ViewModels
{
    public interface IProductFormViewModel : IFormValidation<Product>, IModal
    {
        long PlaceId { get; set; }
        long ProductId { get; set; }
        Product Product { get; }
        bool IsProductValid { get; set; }

        Task OnImageFileChanged(InputFileChangeEventArgs args);

        Task Initialize();

        Task OnCancelClick();

        Task OnSaveClick();
    }
}

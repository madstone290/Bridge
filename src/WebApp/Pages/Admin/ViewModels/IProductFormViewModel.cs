using Bridge.WebApp.Pages.Admin.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace Bridge.WebApp.Pages.Admin.ViewModels
{
    public interface IProductFormViewModel : IFormViewModel<ProductModel>, IModalViewModel
    {
        long PlaceId { get; set; }
        long ProductId { get; set; }
        ProductModel Product { get; }
        bool IsProductValid { get; set; }

        Task OnImageFileChanged(InputFileChangeEventArgs args);

        Task Initialize();

        Task OnCancelClick();

        Task OnSaveClick();
    }
}

using Bridge.WebApp.Pages.Admin.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace Bridge.WebApp.Pages.Common.ViewModels
{
    public interface IProductFormViewModel : IFormValidation<Product>, IModal
    {
        Guid PlaceId { get; set; }
        Guid ProductId { get; set; }
        Product Product { get; }
        bool IsProductValid { get; set; }

        Task OnImageFileChanged(InputFileChangeEventArgs args);

        Task OnDeleteFileClick();

        Task Initialize();

        Task OnCancelClick();

        Task OnSaveClick();
    }
}

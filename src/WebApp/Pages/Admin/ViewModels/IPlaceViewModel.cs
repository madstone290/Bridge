using Bridge.WebApp.Pages.Admin.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace Bridge.WebApp.Pages.Admin.ViewModels
{
    public interface IPlaceViewModel : IValidation<Place>
    {
        Task Initialize();

        Guid PlaceId { get; set; }

        bool BaseInfoReadOnly { get; }

        bool IsBaseInfoValid { get; set; }

        Place Place { get; }

        Task OnUploadFileChange(InputFileChangeEventArgs args);

        Task OnDeleteFileClick();

        Task OnEditBaseInfoClick();

        Task OnCancelBaseInfoClick();

        Task OnSaveBaseInfoClick();

        bool OpeningTimeReadOnly { get; }

        bool IsOpeningTimeValid { get; set; }

        Task OnEditOpeningTimeClick();

        Task OnCancelOpeningTimeClick();

        Task OnSaveOpeningTimeClick();

        string SearchText { get; set; }

        int TotalCount { get; set; }

        int PageCount { get; set; }

        int PageNumber { get; set; }

        int RowsPerPage { get; set; }

        IEnumerable<Product> Products { get; }

        Task OnCreateProductClick();

        Task OnUpdateProductClick(Product product);

        Task OnDiscardProductClick(Product product);

        Task OnPageNumberChanged(int pageNumber);

        Task OnRowsPerPageChanged(int rowsPerPage);

    }
}

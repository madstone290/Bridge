using Bridge.WebApp.Pages.Admin.Models;
using Bridge.WebApp.Pages.Admin.Records;
using Microsoft.AspNetCore.Components.Forms;
using System.Linq.Expressions;

namespace Bridge.WebApp.Pages.Admin.ViewModels
{
    public interface IPlaceViewModel
    {
        Task Initialize();

        long PlaceId { get; set; }

        bool BaseInfoReadOnly { get; }

        bool IsBaseInfoValid { get; set; }

        PlaceFormModel Place { get; }

        Func<TProperty, Task<IEnumerable<string>>> GetValidation<TProperty>(Expression<Func<PlaceFormModel, TProperty>> expression);

        Task OnUploadFileChange(InputFileChangeEventArgs args);

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

        IEnumerable<ProductRecord> Products { get; }

        Task OnCreateProductClick();

        Task OnUpdateProductClick(ProductRecord product);

        Task OnDiscardProductClick(ProductRecord product);

        Task OnPageNumberChanged(int pageNumber);

        Task OnRowsPerPageChanged(int rowsPerPage);

    }
}

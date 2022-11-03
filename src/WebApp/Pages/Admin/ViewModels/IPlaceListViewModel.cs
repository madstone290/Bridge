using Bridge.Domain.Places.Entities;
using Bridge.WebApp.Pages.Admin.Models;
using Microsoft.AspNetCore.Components.Web;

namespace Bridge.WebApp.Pages.Admin.ViewModels
{
    public interface IPlaceListViewModel
    {
        PlaceType? PlaceType { get; set; }

        string SearchText { get; set; }

        int TotalCount { get; set; }

        int PageCount { get; set; }

        int PageNumber { get; set; }

        int RowsPerPage { get; set; }

        IEnumerable<PlaceModel> Places { get; }

        string GetPlaceTypeText(PlaceType? placeType);

        Task OnSearchTextKeyUp(KeyboardEventArgs args);

        Task OnLoadClick();

        Task OnCreateClick();

        Task OnCreateRestroomClick();

        Task OnRestroomExcelDownloadClick();

        Task OnRestroomExcelUploadClick();

        Task OnShowOpeningTimeClick(PlaceModel place);

        Task OnEditPlaceClick(PlaceModel place);

        Task OnManagePlaceClick(PlaceModel place);

        Task OnManageProductClick(PlaceModel place);

        Task OnClosePlaceClick(PlaceModel place);

        Task OnPageNumberChanged(int pageNumber);

        Task OnRowsPerPageChanged(int rowsPerPage);



    }
}

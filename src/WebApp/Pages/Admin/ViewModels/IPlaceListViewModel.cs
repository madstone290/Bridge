using Bridge.Domain.Places.Enums;
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

        IEnumerable<Place> Places { get; }

        string GetPlaceTypeText(PlaceType? placeType);

        Task OnSearchTextKeyUp(KeyboardEventArgs args);

        Task OnLoadClick();

        Task OnCreateClick();

        Task OnCreateRestroomClick();

        Task OnRestroomExcelDownloadClick();

        Task OnRestroomExcelUploadClick();

        Task OnShowOpeningTimeClick(Place place);

        Task OnEditPlaceClick(Place place);

        Task OnManagePlaceClick(Place place);

        Task OnManageProductClick(Place place);

        Task OnClosePlaceClick(Place place);

        Task OnPageNumberChanged(int pageNumber);

        Task OnRowsPerPageChanged(int rowsPerPage);



    }
}

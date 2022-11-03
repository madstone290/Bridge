using Bridge.Application.Places.ReadModels;
using Bridge.WebApp.Pages.Admin.Records;

namespace Bridge.WebApp.Pages.Admin.ViewModels
{
    public interface IPlaceProductListViewModel
    {
        long PlaceId { get; set; }
        
        PlaceReadModel Place { get; }

        string SearchText { get; set; }

        IEnumerable<ProductRecord> Products { get; }

        bool FilterProduct(ProductRecord product);

        Task Initialize();

        Task OnLoadClick();

        Task OnCreateClick();

    }
}

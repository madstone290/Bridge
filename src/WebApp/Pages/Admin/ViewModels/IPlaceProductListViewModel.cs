using Bridge.Application.Places.ReadModels;
using Bridge.WebApp.Pages.Admin.Models;

namespace Bridge.WebApp.Pages.Admin.ViewModels
{
    public interface IPlaceProductListViewModel
    {
        long PlaceId { get; set; }
        
        PlaceReadModel Place { get; }

        string SearchText { get; set; }

        IEnumerable<ProductModel> Products { get; }

        bool FilterProduct(ProductModel product);

        Task Initialize();

        Task OnLoadClick();

        Task OnCreateClick();

    }
}

using Bridge.WebApp.Pages.Home.Models;
using System.ComponentModel;

namespace Bridge.WebApp.Pages.Home.ViewModels
{
    public interface IPlaceDetailViewModel
    {
        event PropertyChangedEventHandler? PropertyChanged;

        Place Place { get; set; }

        IEnumerable<Product> Products { get; }

        Task LoadProducts();
    }
}

using Bridge.WebApp.Pages.Common.Models;
using System.ComponentModel;

namespace Bridge.WebApp.Pages.Home.ViewModels
{
    public interface IPlaceDetailViewModel
    {
        event PropertyChangedEventHandler? PropertyChanged;

        bool IsPlaceOwner { get; }

        Place Place { get; set; }

        IEnumerable<Product> Products { get; }

        /// <summary>
        /// 사용자 정보 조회
        /// </summary>
        /// <returns></returns>
        Task LoadUserAsync();

        Task LoadProducts();

        Task OnAddProductClick();
    }
}

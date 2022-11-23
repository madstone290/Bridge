using Bridge.WebApp.Pages.Home.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Bridge.WebApp.Pages.Home.ViewModels
{
    public interface IIndexViewModel : IAsyncDisposable
    {
        IPlaceDetailViewModel PlaceDetailVM { get; set; }

        ResultTab SelectedTab { get; set; }

        /// <summary>
        /// 네이버 맵 서비스 콜백등록을 위한 이벤트 수신자
        /// </summary>
        IHandleEvent Receiver { set; }

        string MapElementId { get; }
        
        bool ProductSearched { get; }

        bool PlaceSearched { get; }

        string SearchText { get; set; }

        LatLon? CurrentLocation { get; set; }

        string? CurrentAddress { get; set; }

        string ProductListElementId { get; }

        Product? SelectedProduct { get; set; }

        IEnumerable<Product> Products { get; }

        /// <summary>
        /// 리스트에서 제품이 선택된 경우
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        Task OnProductSelected(Product product);

        string PlaceListElementId { get; }

        Place? SelectedPlace { get; set; }

        IEnumerable<Place> Places { get; }

        /// <summary>
        /// 리스트에서 장소가 선택된 경우
        /// </summary>
        /// <param name="place"></param>
        /// <returns></returns>
        Task OnPlaceSelected(Place place);

        EventCallback SearchCompleted { set; }

        /// <summary>
        /// 뷰모델 초기화
        /// </summary>
        /// <returns></returns>
        Task InitAsync();

        /// <summary>
        /// 장소 추가 클릭
        /// </summary>
        /// <returns></returns>
        Task OnAddPlaceClick();

        /// <summary>
        /// 검색 클릭
        /// </summary>
        /// <returns></returns>
        Task OnSearchClick();

        /// <summary>
        /// 초기화 클릭
        /// </summary>
        /// <returns></returns>
        Task OnClearClick();

        /// <summary>
        /// 검색필드에서 키가 입력된 경우
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        Task OnSearchFieldKeyUp(KeyboardEventArgs args);

        /// <summary>
        /// 탭이 변경된 경우
        /// </summary>
        /// <param name="tab"></param>
        /// <returns></returns>
        Task OnSelectedTabChanged(ResultTab tab);

    }
}

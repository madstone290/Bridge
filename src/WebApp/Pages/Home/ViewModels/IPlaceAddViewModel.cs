using Bridge.WebApp.Pages.Home.Models;
using MudBlazor;

namespace Bridge.WebApp.Pages.Home.ViewModels
{
    public interface IPlaceAddViewModel : IValidation<Place>
    {
        MudDialogInstance MudDialog { get; set; }

        /// <summary>
        /// 장소 데이터 유효성
        /// </summary>
        bool IsPlaceValid { get; set; }

        /// <summary>
        /// 추가할 장소
        /// </summary>
        Place Place { get; set; }

        Task OnCancelClick();

        Task OnSaveClick();

    }
}

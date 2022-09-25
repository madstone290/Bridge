using Bridge.WebApp.Api.ApiClients;
using Bridge.WebApp.Models;
using Microsoft.AspNetCore.Components;

namespace Bridge.WebApp.Pages.Admin
{
    public partial class PlaceList
    {
        /// <summary>
        /// 장소 목록
        /// </summary>
        private readonly List<PlaceModel> _places = new();

        /// <summary>
        /// 검색어
        /// </summary>
        private string _searchString = string.Empty;

        [Inject]
        public PlaceApiClient PlaceApiClient { get; set; } = null!;

        /// <summary>
        /// 입력된 검색어로 장소를 검색한다.
        /// </summary>
        /// <param name="place"></param>
        /// <returns></returns>
        private bool Search(PlaceModel place)
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;
            return place.Name.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true ||
                place.Address.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true ||
                place.CategoriesString.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true;
        }

        protected override async Task OnInitializedAsync()
        {
            LoadPlaceList();

            await Task.Delay(300);
            _places.Add(new PlaceModel()
            {
                Name = "test1",
                Categories = new List<Domain.Places.Entities.PlaceCategory>() { Domain.Places.Entities.PlaceCategory.Cafeteria, Domain.Places.Entities.PlaceCategory.Restaurant },
                OpeningTimes = new List<Application.Places.Dtos.OpeningTimeDto>()
                {
                    new Application.Places.Dtos.OpeningTimeDto()
                    {
                        Day = DayOfWeek.Monday,
                        OpenTime = TimeSpan.FromHours(9),
                        CloseTime= TimeSpan.FromHours(18),
                    },
                    new Application.Places.Dtos.OpeningTimeDto()
                    {
                        Day = DayOfWeek.Tuesday,
                        OpenTime = TimeSpan.FromHours(10),
                        CloseTime= TimeSpan.FromHours(15),
                    }
                }
            });
        }

        private void LoadPlaceList()
        {
            
        }

        private void Create_Click()
        {
            NavManager.NavigateTo(PageRoutes.Admin.PlaceCreate);
        }

        private void Load_Click()
        {
            LoadPlaceList();
        }

        private void ToggleShowOpeningTime_Click(PlaceModel place)
        {
            place.ShowOpeningTimes = !place.ShowOpeningTimes;
        }
    }
}

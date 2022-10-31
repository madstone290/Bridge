using Bridge.Application.Places.ReadModels;
using Bridge.Shared.Extensions;
using Bridge.WebApp.Api.ApiClients.Admin;
using Bridge.WebApp.Pages.Admin.Records;
using Microsoft.AspNetCore.Components;

namespace Bridge.WebApp.Pages.Admin
{
    public partial class PlaceProductList
    {
        /// <summary>
        /// 장소 목록
        /// </summary>
        private readonly List<ProductRecord> _products = new();

        private readonly PlaceReadModel _place = new();

        /// <summary>
        /// 검색어
        /// </summary>
        private string _searchString = string.Empty;

        /// <summary>
        /// 장소 아이디
        /// </summary>
        [Parameter]
        public long PlaceId { get; set; }

        [Inject]
        public AdminPlaceApiClient PlaceApiClient { get; set; } = null!;

        [Inject]
        public AdminProductApiClient ProductApiClient { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await Load_ClickAsync();
        }

        /// <summary>
        /// 입력된 검색어로 제품을 검색한다.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        private bool Search(ProductRecord product)
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;
            return product.Name.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true ||
                product.Type.ToString().Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true ||
                product.Price?.ToString().Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true;
        }

        private void Create_Click()
        {
            NavManager.NavigateTo(PageRoutes.Admin.PlaceProductCreate.AddRouteParam("PlaceId", PlaceId));
        }

        private async Task Load_ClickAsync()
        {
            var placeTask = PlaceApiClient.GetPlaceById(PlaceId);
            var productTask = ProductApiClient.GetProductList(PlaceId);

            await Task.WhenAll(placeTask, productTask);

            var placeResult = placeTask.Result;
            var productResult = productTask.Result;

            if (!ValidationService.Validate(placeTask.Result) || !ValidationService.Validate(productResult))
                return;

            var placeDto = placeResult.Data!;
            _place.Name = placeDto.Name;
            _place.Address = placeDto.Address;

            var productListDto = productResult.Data!;
            _products.Clear();
            _products.AddRange(productListDto.OrderByDescending(x=> x.CreationDateTime).Select(x => ProductRecord.Create(x)));
        }

    }
}

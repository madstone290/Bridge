using Bridge.WebApp.Api.ApiClients;
using Bridge.WebApp.Extensions;
using Bridge.WebApp.Pages.Admin.Models;
using Microsoft.AspNetCore.Components;

namespace Bridge.WebApp.Pages.Admin
{
    public partial class PlaceProductList
    {
        /// <summary>
        /// 장소 목록
        /// </summary>
        private readonly List<ProductModel> _products = new();

        /// <summary>
        /// 검색어
        /// </summary>
        private string _searchString = string.Empty;

        /// <summary>
        /// 장소 아이디
        /// </summary>
        [Parameter]
        [SupplyParameterFromQuery]
        public long PlaceId { get; set; }

        [Inject]
        public ProductApiClient ProductApiClient { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await Load_ClickAsync();
        }

        /// <summary>
        /// 입력된 검색어로 제품을 검색한다.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        private bool Search(ProductModel product)
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;
            return product.Name.Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true ||
                product.Type.ToString().Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true ||
                product.Price?.ToString().Contains(_searchString, StringComparison.OrdinalIgnoreCase) == true;
        }

        private void Create_Click()
        {
            //NavManager.NavigateTo(PageRoutes.Admin.ProductCreate);
        }

        private async Task Load_ClickAsync()
        {
            var response = await ProductApiClient.GetProductList(PlaceId);
            if (!Snackbar.CheckSuccess(response))
                return;

            _products.Clear();
            _products.AddRange(response.Data!.Select(x => ProductModel.Create(x)));
        }

    }
}

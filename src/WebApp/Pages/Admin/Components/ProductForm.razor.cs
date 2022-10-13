using Bridge.Application.Products.Commands;
using Bridge.Shared.Extensions;
using Bridge.WebApp.Api.ApiClients;
using Bridge.WebApp.Extensions;
using Bridge.WebApp.Pages.Admin.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Bridge.WebApp.Pages.Admin.Components
{
    public partial class ProductForm
    {
        private MudForm? _form;
        private bool _isFormValid;
        private readonly ProductFormModel _product = new();
        private readonly ProductFormModel.Validator _validator = new();

        /// <summary>
        /// 폼 모드
        /// </summary>
        [Parameter] 
        public FormMode FormMode { get; set; }

        /// <summary>
        /// 제품 아이디
        /// </summary>
        [Parameter] 
        public long ProductId { get; set; }

        /// <summary>
        /// 장소 아이디
        /// </summary>
        [Parameter]
        public long PlaceId { get; set; }

        [Inject] 
        public ProductApiClient ProductApiClient { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            if(FormMode == FormMode.Create)
            {
                _product.PlaceId = PlaceId;
            }
            else if(FormMode == FormMode.Update)
            {
                var productResponse = await ProductApiClient.GetProductById(ProductId);
                if (!Snackbar.CheckSuccess(productResponse))
                    return;

                var productDto = productResponse.Data!;

                _product.Id = productDto.Id;
                _product.Type = productDto.Type;
                _product.Name = productDto.Name;
                _product.Price = productDto.Price;
                _product.Categories = productDto.Categories;
            }
        }

        void Cancel_Click()
        {
            NavManager.NavigateTo(PageRoutes.Admin.PlaceProductList);
        }

        async Task Save_Click()
        {
            if (_form == null)
                return;

            await _form.Validate();

            if (_isFormValid)
            {
                if (FormMode == FormMode.Create)
                {
                    var command = new CreateProductCommand()
                    {
                        
                        Name = _product.Name,
                        PlaceId = _product.PlaceId,
                        Price = _product.Price,
                        Categories = _product.Categories.ToList(),
                    };
                    var result = await ProductApiClient.CreateProduct(command);

                    if (Snackbar.CheckSuccess(result))
                        NavManager.NavigateTo(PageRoutes.Admin.PlaceProductList.AddRouteParam("PlaceId", PlaceId));
                }
                else
                {
                    var command = new UpdateProductCommand()
                    {
                        ProductId = _product.Id,
                        Name = _product.Name,
                        Price = _product.Price,
                        Categories = _product.Categories.ToList(),
                    };
                    var result = await ProductApiClient.UpdateProduct(command);

                    if (Snackbar.CheckSuccess(result))
                        NavManager.NavigateTo(PageRoutes.Admin.PlaceProductList.AddRouteParam("PlaceId", PlaceId));
                }

            }
        }
    }
}


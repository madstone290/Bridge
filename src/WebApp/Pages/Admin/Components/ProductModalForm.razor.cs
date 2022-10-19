using Bridge.Application.Products.Commands;
using Bridge.Shared.Extensions;
using Bridge.WebApp.Api.ApiClients.Admin;
using Bridge.WebApp.Pages.Admin.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Bridge.WebApp.Pages.Admin.Components
{
    public partial class ProductModalForm
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

        [CascadingParameter]
        public MudDialogInstance MudDialog { get; set; } = null!;

        [Inject] 
        public AdminProductApiClient ProductApiClient { get; set; } = null!;

        /// <summary>
        /// 제품리스트 Uri
        /// </summary>
        public string PlaceProductListUri => PageRoutes.Admin.PlaceProductList.AddRouteParam("PlaceId", PlaceId);

        protected override async Task OnInitializedAsync()
        {
            if(FormMode == FormMode.Create)
            {
                _product.PlaceId = PlaceId;
            }
            else if(FormMode == FormMode.Update)
            {
                var productResponse = await ProductApiClient.GetProductById(ProductId);
                if (!ValidationService.Validate(productResponse))
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
            MudDialog.Cancel();
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

                    if (ValidationService.Validate(result))
                    {
                        MudDialog.Close();
                    }
                }
                else
                {
                    var command = new UpdateProductCommand()
                    {
                        Id = _product.Id,
                        Name = _product.Name,
                        Price = _product.Price,
                        Categories = _product.Categories.ToList(),
                    };
                    var result = await ProductApiClient.UpdateProduct(command);

                    if (ValidationService.Validate(result))
                    {
                        MudDialog.Close();
                    }
                }

            }
        }
    }
}


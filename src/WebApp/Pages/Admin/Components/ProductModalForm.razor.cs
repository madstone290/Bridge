using Bridge.Application.Products.Commands;
using Bridge.Shared.Extensions;
using Bridge.WebApp.Api.ApiClients.Admin;
using Bridge.WebApp.Pages.Admin.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace Bridge.WebApp.Pages.Admin.Components
{
    public partial class ProductModalForm
    {
        private MudForm? _form;
        private bool _isFormValid;
        private readonly ProductModel _product = new();
        private readonly ProductModel.Validator _validator = new();

        private string? _imgSrc;

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
            if (FormMode == FormMode.Create)
            {
                _product.PlaceId = PlaceId;
            }
            else if (FormMode == FormMode.Update)
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
            if (!_isFormValid)
                return;

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
                    MudDialog.Close();
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
                    MudDialog.Close();
            }
        }

        private async void UploadFiles(InputFileChangeEventArgs e)
        {
            var file = e.File;
            var sizeLimit = 50000;
            if (sizeLimit < file.Size)
            {
                Snackbar.Add("50Kb가 넘는 이미지는 사용할 수 없습니다");
                return;
            }

            var format = file.ContentType;
            var buffer = new byte[file.Size];
            using var stream = file.OpenReadStream(file.Size);
            await stream.ReadAsync(buffer);

            var base64 = Convert.ToBase64String(buffer);
            _imgSrc = $"data:{format};base64,{base64}";
            StateHasChanged();
        }
    }
}


using Bridge.Application.Products.Commands;
using Bridge.WebApp.Api.ApiClients.Admin;
using Bridge.WebApp.Pages.Common.Models;
using Bridge.WebApp.Services;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace Bridge.WebApp.Pages.Common.ViewModels.Implement
{
    public class ProductFormViewModel : IProductFormViewModel
    {
        private readonly Product.Validator _validator = new();

        private readonly AdminProductApiClient _productApiClient;
        private readonly IResultValidationService _validationService;
        private readonly ISnackbar _snackbar;

        public ProductFormViewModel(AdminProductApiClient productApiClient, IResultValidationService validationService, ISnackbar snackbar)
        {
            _productApiClient = productApiClient;
            _validationService = validationService;
            _snackbar = snackbar;
        }

        public Guid ProductId { get; set; }
        public Guid PlaceId { get; set; }
        public Product Product { get; private set; } = new();
        public MudDialogInstance MudDialog { get; set; } = null!;
        public FormMode FormMode { get; set; }
        public bool IsProductValid { get; set; }

        private async Task<bool> CreateAsync()
        {
            var command = new CreateProductCommand()
            {

                Name = Product.Name,
                PlaceId = Product.PlaceId,
                Price = Product.Price,
                Categories = Product.Categories.ToList(),
            };
            var result = await _productApiClient.CreateProduct(command);

            return _validationService.Validate(result);
        }

        private async Task<bool> UpdateAsync()
        {
            var command = new UpdateProductCommand()
            {
                Id = Product.Id,
                Name = Product.Name,
                Price = Product.Price,
                Categories = Product.Categories.ToList(),
            };
            var result = await _productApiClient.UpdateProduct(command);
            return _validationService.Validate(result);
        }

        public Func<TProperty, Task<IEnumerable<string>>> GetValidation<TProperty>(System.Linq.Expressions.Expression<Func<Product, TProperty>> expression)
        {
            return _validator.PropertyValidation(expression);
        }

        public async Task Initialize()
        {
            if (FormMode == FormMode.Create)
            {
                Product.PlaceId = PlaceId;
            }
            else if (FormMode == FormMode.Update)
            {
                var productResponse = await _productApiClient.GetProductById(ProductId);
                if (!_validationService.Validate(productResponse))
                    return;

                var productDto = productResponse.Data!;

                Product.Id = productDto.Id;
                Product.Type = productDto.Type;
                Product.Name = productDto.Name;
                Product.Price = productDto.Price;
                Product.Categories = productDto.Categories;
            }
        }

        public Task OnCancelClick()
        {
            MudDialog.Cancel();
            return Task.CompletedTask;
        }

        public async Task OnImageFileChanged(InputFileChangeEventArgs args)
        {
            var file = args.File;
            var sizeLimit = 50000;
            if (sizeLimit < file.Size)
            {
                _snackbar.Add("50Kb가 넘는 이미지는 사용할 수 없습니다");
                return;
            }

            var format = file.ContentType;
            var buffer = new byte[file.Size];
            using var stream = file.OpenReadStream(file.Size);
            await stream.ReadAsync(buffer);

            var base64 = Convert.ToBase64String(buffer);
            Product.ImageSrc = $"data:{format};base64,{base64}";
        }

        public async Task OnDeleteFileClick()
        {
            Product.ImageSrc = null;
            await Task.CompletedTask;
        }

        public async Task OnSaveClick()
        {
            if (!IsProductValid)
                return;

            bool success;
            if (FormMode == FormMode.Create)
            {
                success = await CreateAsync();
            }
            else
            {
                success = await UpdateAsync();
            }

            if (success)
                MudDialog.Close();
        }
    }
}

using Bridge.Application.Products.Commands;
using Bridge.Application.Products.Queries;
using Bridge.Application.Products.ReadModels;
using Bridge.Domain.Products.Entities;
using Bridge.IntegrationTests.Config;
using Bridge.Shared;
using Bridge.Shared.Extensions;
using FluentAssertions;
using System.Net.Http.Json;

namespace Bridge.IntegrationTests
{
    public class ProductsControllerTests : IClassFixture<ApiTestFactory>
    {
        private readonly TestClient _client;
        private readonly ApiClient _apiClient;

        public ProductsControllerTests(ApiTestFactory apiTestFactory)
        {
            _client = apiTestFactory.Client;
            _apiClient = apiTestFactory.ApiClient;
        }

    }
}

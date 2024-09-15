using LTshowcase.Pages.Products.Services;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace LTshowcase.Tests;

[CollectionDefinition(nameof(SliceFixture))]
public class SliceFixtureCollection : ICollectionFixture<SliceFixture> { }

public class SliceFixture
{
    private readonly TestApplicationFactory _factory;
    private readonly IServiceScopeFactory _scopeFactory;

    public SliceFixture()
    {
        _factory = new TestApplicationFactory();
        _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();        
    }

    private class TestApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IProductClient));
                
                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddScoped<IProductClient, TestProductClient>();
            });
        }
    }

    public T ExecuteScoped<T>(Func<IProductRepository, IMediator, T> action)
    {
        using var scope = _scopeFactory.CreateScope();
        var client = scope.ServiceProvider.GetService<IProductClient>() as IProductRepository ?? new TestProductClient();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var result = action(client, mediator);

        return result;
    }

    public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = _scopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        return await mediator.Send(request);
    }

    //introducing a test client/repo to break the dependency on an External API for testing handlers and allow for simpler test setup
    public interface IProductRepository
    {
        void Add(Product product);
        void Add(IEnumerable<Product> products);
    }
    public class TestProductClient : IProductClient, IProductRepository
    {
        private readonly List<Product> _products = [];

        public Task<Product?> GetById(int id, CancellationToken token)
        {
            return Task.FromResult(_products.Find(p => p.Id == id));
        }

        public Task<SearchResult?> Search(SearchQuery query, CancellationToken token)
        {
            var results = _products.Where(p => p.Title.Contains(query.SearchTerm) || query.SearchTerm == string.Empty);
            var searchResult = new SearchResult
            {
                SearchTerm = query.SearchTerm,
                Products = results,
                Total = results.Count()
            };
            return Task.FromResult(searchResult ?? default);
        }

        public void Add(Product product) => _products.Add(product);
        public void Add(IEnumerable<Product> products) => _products.AddRange(products);        
    }
}

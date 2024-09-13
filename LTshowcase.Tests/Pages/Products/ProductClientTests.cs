using FluentAssertions;
using LTshowcase.Pages.Products.Services;

namespace LTshowcase.Tests.Pages.Products;

public class ProductClientTests
{
    private readonly IProductClient _productService = new ExternalApiProductClient(new HttpClient());

    [Fact]
    public async Task Should_find_product_by_id()
    {
        var query = new SearchQuery();
        var allProducts = await _productService.Search(query, default);
        var firstProduct = allProducts.Products.First();
        var result = await _productService.GetById(firstProduct.Id, default);

        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Should_not_find_product_by_id()
    {
        var result = await _productService.GetById(0, default);

        result.Should().NotBeNull();
        result.Id.Should().Be(0);
        result.Title.Should().BeEmpty();
    }

    [Fact]
    public async Task Should_return_all_products_for_default_search()
    {
        var query = new SearchQuery();
        var result = await _productService.Search(query, default);

        result.Should().NotBeNull();

        var totalProducts = result.Products.Count();
        totalProducts.Should().BeGreaterThan(50);
        result.Total.Should().Be(totalProducts);
    }

    [Fact]
    public async Task Should_find_product_by_search_term()
    {
        var query = new SearchQuery { SearchTerm = "laptop" };
        var result = await _productService.Search(query, default);

        result.Should().NotBeNull();
        
        var totalProducts = result.Products.Count();
        totalProducts.Should().BeGreaterThan(1);
        result.Total.Should().Be(totalProducts);
    }

    [Fact]
    public async Task Should_not_find_product_by_search_term()
    {
        var query = new SearchQuery { SearchTerm = "should not find a matching product" };
        var result = await _productService.Search(query, default);

        result.Should().NotBeNull();

        var totalProducts = result.Products.Count();
        totalProducts.Should().Be(0);
        result.Total.Should().Be(totalProducts);
    }
}
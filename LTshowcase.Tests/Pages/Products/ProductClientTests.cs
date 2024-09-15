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

    [Theory]
    [InlineData(1, 1)]
    [InlineData(1, 30)]
    [InlineData(1, 1000)]
    [InlineData(2, 30)]
    public async Task Should_return_products_by_page_according_to_page_size(int page, int pageSize)
    {
        var query = new SearchQuery { CurrentPage = page, PageSize = pageSize};
        var result = await _productService.Search(query, default);

        result.Should().NotBeNull();

        result.CurrentPage.Should().Be(page);

        var totalProducts = result.Products.Count();
        if (pageSize < result.Total)
            totalProducts.Should().Be(pageSize);
        else
            totalProducts.Should().Be(result.Total);
        
        result.Total.Should().BeGreaterThan(50);
    }

    [Fact]
    public async Task Should_find_product_by_search_term()
    {
        var query = new SearchQuery { SearchTerm = "laptop", PageSize = 1000 };
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
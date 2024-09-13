using FluentAssertions;
using LTshowcase.Pages.Products.Services;

namespace LTshowcase.Tests.Pages.Products;

[Collection(nameof(SliceFixture))]
public class IndexTests(SliceFixture sliceFixture)
{
    private readonly SliceFixture _sliceFixture = sliceFixture;

    [Fact]
    public async Task Should_return_all_products_for_default_search()
    {
        var productsAdded = 10;
        var result = await _sliceFixture.ExecuteScoped(async (repo, mediator) =>
        {
            var products = Enumerable.Repeat(new Product(), productsAdded);
            repo.Add(products);

            var query = new SearchQuery();
            return await mediator.Send(query, default);
        });

        result.Should().NotBeNull();

        var totalProducts = result.Products.Count();
        totalProducts.Should().Be(productsAdded);
        result.Total.Should().Be(totalProducts);
    }

    [Theory]
    [InlineData(1, "laptop", "laptop")]
    [InlineData(1, "match by Id", "1")]
    public async Task Should_find_product_by_search_term(int id, string title, string searchTerm)
    {
        Product[] productsAdded = [];
        var result = await _sliceFixture.ExecuteScoped(async (repo, mediator) =>
        {
            var matchingProduct = new Product { Id = id, Title = title };
            var nonMatchingProduct = new Product { Id = id + 1, Title = "won't match" };
            productsAdded = [ matchingProduct, nonMatchingProduct ];
            repo.Add(productsAdded);

            var query = new SearchQuery { SearchTerm = searchTerm };
            return await mediator.Send(query, default);
        });

        result.Should().NotBeNull();

        var totalProducts = result.Products.Count();
        totalProducts.Should().Be(1);
        totalProducts.Should().NotBe(productsAdded.Length);
        result.Total.Should().Be(totalProducts);
    }
}
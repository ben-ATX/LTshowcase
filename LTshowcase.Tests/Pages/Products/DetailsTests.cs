using FluentAssertions;
using LTshowcase.Pages.Products.Services;
using static LTshowcase.Pages.Products.Details;

namespace LTshowcase.Tests.Pages.Products;

[Collection(nameof(SliceFixture))]
public class DetailsTests(SliceFixture sliceFixture)
{
    private readonly SliceFixture _sliceFixture = sliceFixture;
    
    [Fact]
    public async Task Should_find_product_by_id()
    {
        var result = await _sliceFixture.ExecuteScoped(async (client, mediator) =>
        {
            var product = new Product { Id = 1 };
            client.Add(product);

            var query = new Query { Id = product.Id };
            return await mediator.Send(query);
        });

        result.Should().NotBeNull();
        result.Id.Should().Be(1);
    }

    [Fact]
    public async Task Should_not_find_product_by_id()
    {
        var result = await _sliceFixture.ExecuteScoped(async (client, mediator) =>
        {
            var query = new Query { Id = 1 };
            return await mediator.Send(query);
        });

        result.Should().NotBeNull();
        result.Id.Should().Be(0);
        result.Title.Should().BeEmpty();
    }
}

using LTshowcase.Pages.Products.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LTshowcase.Pages.Products;
public class Details(IMediator _mediator) : PageModel
{
    public Product Data { get; private set; } = new();

    public async Task OnGetAsync(int id)
    {
        var query = new Query { Id = id };
        Data = await _mediator.Send(query);
    }

    public record Query : IRequest<Product>
    {
        public int Id { get; set; }
    }

    public class Handler : IRequestHandler<Query, Product>
    {
        private readonly IProductClient _products;

        public Handler(IProductClient products) => _products = products;
        public async Task<Product> Handle(Query query, CancellationToken token) =>
            await _products.GetById(query.Id, token) ?? new();
    }
}

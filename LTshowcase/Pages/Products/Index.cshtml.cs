using LTshowcase.Pages.Products.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LTshowcase.Pages.Products;
public class Index(IMediator _mediator) : PageModel
{
    public SearchResult Data { get; private set; } = new() { Products = [] };

    [BindProperty(SupportsGet = true)]
    public SearchQuery Query { get; set; } = new SearchQuery();

    public async Task OnGetAsync() => Data = await _mediator.Send(Query);

    public class Handler : IRequestHandler<SearchQuery, SearchResult>
    {
        private readonly IProductClient _products;

        public Handler(IProductClient products) => _products = products;

        public async Task<SearchResult> Handle(SearchQuery query, CancellationToken token)
        {
            var isSearchTermProductId = int.TryParse(query.SearchTerm, out var productId);
            if (isSearchTermProductId)
            {
                var product = await _products.GetById(productId, token);
                var result = new SearchResult();

                if (product?.Id > 0)
                {
                    result.Products = [product];
                    result.Total = 1;
                }

                return result;
            }

            return await _products.Search(query, token) ?? new();
        }
    }
}

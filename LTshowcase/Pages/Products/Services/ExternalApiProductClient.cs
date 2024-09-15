using MediatR;
using System.Text.Json;

namespace LTshowcase.Pages.Products.Services;
public interface IProductClient
{
    Task<Product?> GetById(int id, CancellationToken token);

    Task<SearchResult?> Search(SearchQuery query, CancellationToken token);
}
public class ExternalApiProductClient : IProductClient
{
    private readonly HttpClient _httpClient;
    public ExternalApiProductClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://dummyjson.com/products/");
    }

    public async Task<Product?> GetById(int id, CancellationToken token) =>
        await Get<Product>(id.ToString(), token);

    public async Task<SearchResult?> Search(SearchQuery query, CancellationToken token)
    {
        var skip = (query.CurrentPage - 1) * query.PageSize;
        var result = await Get<SearchResult>($"search?q={query.SearchTerm}&skip={skip}&limit={query.PageSize}", token);

        result.CurrentPage = (int)Math.Ceiling(result.Skip / (double)query.PageSize) + 1;
        result.TotalPages = (int)Math.Ceiling(Math.Max(result.Total, 1) / (double)query.PageSize);// result.CalculateTotalPages(query.PageSize);

        return result;
    }
    private async Task<T?> Get<T>(string requestUri, CancellationToken token) where T : class
    {
        var response = await _httpClient.GetAsync(requestUri, token);

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<T>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return result;
    }
}

public record Product
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public decimal Price { get; set; }
    public string Thumbnail { get; set; } = "";
    public IEnumerable<string> Images { get; set; } = [];
}

public record SearchQuery : IRequest<SearchResult>
{
    public string SearchTerm { get; set; } = "";

    public int CurrentPage { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}

public record SearchResult
{
    public string SearchTerm { get; set; } = "";
    public int Skip { get; set; }
    public int Limit { get; set; }
    public int Total { get; set; }
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; } = 1;

    public IEnumerable<Product> Products { get; set; } = [];
}
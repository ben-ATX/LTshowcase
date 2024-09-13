using LTshowcase.Pages.Products.Services;

var builder = WebApplication.CreateBuilder(args);

RegisterServices(builder);

var app = builder.Build();

ConfigureApplication(app);

app.Run();

static void RegisterServices(WebApplicationBuilder builder)
{
    builder.Services.AddRazorPages();
    builder.Services.AddHttpClient<IProductClient, ExternalApiProductClient>();
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
}

static void ConfigureApplication(WebApplication app)
{
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error");
    }
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthorization();

    app.MapRazorPages();

    app.MapGet("/", () => Results.Redirect("/Products/Index"));
}
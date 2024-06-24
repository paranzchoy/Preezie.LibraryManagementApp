using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Preezie.LibraryManagementApp.Data;
using Preezie.LibraryManagementApp.IntegrationTests.Api.Shared;

namespace Preezie.LibraryManagementApp.IntegrationTests.Api.Books;


[CollectionDefinition("BookApiTests")]
public class BookApiCollection : ICollectionFixture<BookApiTestFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}

public class BookApiTestFixture
{
    private readonly HttpClient _httpClientAuthorized = new HttpClient();
    private readonly HttpClient _httpClientNotAuthorized = new HttpClient();
    private readonly ApplicationDbContext _context = default!;

    public BookApiTestFixture()
    {
        var waf = new BookApiWebApplicationFactory();
        waf.InitializeAsync().Wait();

        _httpClientAuthorized = waf.WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
            });
        }).CreateClient();

        _httpClientNotAuthorized = waf.CreateDefaultClient();

        var scope = waf.Services.GetRequiredService<IServiceScopeFactory>()?.CreateScope();
        if (scope != null)
        {
            _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        }
    }

    public HttpClient HttpClientAuthorized => _httpClientAuthorized;
    public HttpClient HttpClientNotAuthorized => _httpClientNotAuthorized;
    public ApplicationDbContext DbContext => _context;

}

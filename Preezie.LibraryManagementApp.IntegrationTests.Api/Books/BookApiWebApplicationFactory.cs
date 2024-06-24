using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Preezie.LibraryManagementApp.Data;
using Preezie.LibraryManagementApp;
using Testcontainers.MsSql;

namespace Preezie.LibraryManagementApp.IntegrationTests.Api.Books;

public class BookApiWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MsSqlContainer _dbContainer;

    public BookApiWebApplicationFactory()
    {
        _dbContainer = new MsSqlBuilder().Build();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services
                .SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
        
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            //services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                var connectionString = _dbContainer.GetConnectionString();
                options.UseSqlServer(connectionString);
            });
        });
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync().ConfigureAwait(true);

        using (var scope = Services.CreateScope())
        {
            var scopedServices = scope.ServiceProvider;
            var cntx = scopedServices.GetRequiredService<ApplicationDbContext>();

            await cntx.Database.EnsureCreatedAsync();
        }
    }

    public new Task DisposeAsync()
    {
        return _dbContainer.StopAsync();
    }
}

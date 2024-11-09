using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using FakeAuth;


namespace Calebs.WebAPI.Tests;

internal class WebAPIApplication : WebApplicationFactory<Program>
{
    private readonly string _environment;

    public WebAPIApplication(string environment = "Development")
    {
        _environment = environment;
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment(_environment);

        // Add mock/test services to the builder here
        builder.ConfigureServices(services =>
        {
            // nothing to change
        });

        return base.CreateHost(builder);
    }
}

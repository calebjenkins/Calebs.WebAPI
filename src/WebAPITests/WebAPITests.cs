using Microsoft.AspNetCore.Mvc.Testing;
using Calebs.WebAPI;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Xml.Linq;


namespace Calebs.WebAPI.Tests;

public class WebAPITests
{
    private WebAPIApplication _server;
    private HttpClient _client;
    private const string _token = "fakeToken123xyz";

    public WebAPITests()
    {
        _server = new WebAPIApplication();
        _client = _server.CreateClient();
    }

    [Fact]
    public async Task Hello_Should_Return_HelloWorld()
    {
        var response = await _client.GetAsync("/hello/");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var value = await response.Content.ReadAsStringAsync();
        value.Should().Be("hello world");
    }
    [Theory]
    [InlineData ("Joe")]
    [InlineData ("Bill")]
    [InlineData ("Fred the great!")]
    [InlineData ("Elizabeth")]
    public async Task Hello_NAME_Should_return_name(string Name)
    {
        var response = await _client.GetAsync($"/hello/{ Name }/");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var value = await response.Content.ReadAsStringAsync();
        value.Should().Be($"hello {Name}");
    }
    
    [Fact]
    public async Task Secure_No_Token_Should_Deny()
    {
        var response = await _client.GetAsync($"/secure");
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task FakeToken_Should_return_token()
    {
        var response = await _client.GetAsync($"/FakeToken");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var value = await response.Content.ReadAsStringAsync();
        value.Should().Be(_token);
    }

    [Fact]
    public async Task Secure_with_Token_should_return_token()
    {
        var newClient = _server.CreateClient();
        newClient.DefaultRequestHeaders.Add("bearer", _token);
        var response = await newClient.GetAsync($"/Secure");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var value = await response.Content.ReadAsStringAsync();
        value.Should().Contain("Success!");
    }


}

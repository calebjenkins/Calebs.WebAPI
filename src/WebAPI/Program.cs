using FakeAuth; // Using FakeAuth so I can send a 403 without setting up all the needed auth infrastructure
using Calebs.Extensions.Console;
using Calebs.Extensions;

//namespace Calebs.WebAPI;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication().AddFakeAuth();

var app = builder.Build();

app.MapGet("/hello", () => { return "hello world"; })
    .WithOpenApi().AllowAnonymous();

app.MapGet("/hello/{name}", (string name) => { return $"hello {name}"; })
        .WithOpenApi().AllowAnonymous();

app.MapGet("/echo", Echo_Result);
app.MapPost("/echo", Echo_Result);
app.MapDelete("/echo", Echo_Result);
app.MapPatch("/echo", Echo_Result);
app.MapPut("/echo", Echo_Result);

const string TOKEN = "fakeToken123xyz";
app.MapGet("FakeToken", () => TOKEN).AllowAnonymous();
app.MapPost("FakeToken", () => TOKEN).AllowAnonymous();

app.MapGet("/Secure", Check_Secure);

var url = app.Urls.FirstOrDefault();
var ver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;


ConsoleColor.Red.WriteLine(" -==::Caleb's Web API::==-");
ConsoleColor.Blue.Write($"version: {ver}");
ConsoleColor.Blue.WriteLine(" --- more info at https://github.com/calebjenkins/calebs.webapi/ ");
Console.WriteLine("");
Console.WriteLine("Available Endpoints:");
Console.WriteLine($" - GET {url}/hello");
Console.WriteLine($" - GET {url}/hello/{{name}}");
Console.WriteLine($" - GET POST {url}/FakeToken");
Console.WriteLine($" - GET {url}/Secure");
Console.WriteLine($" - GET POST DELETE PATCH PUT {url}/echo");
Console.WriteLine("");

ConsoleColor.Red.Write("Use");
ConsoleColor.Blue.Write(" --urls=http://localhost:PORT");
ConsoleColor.Red.WriteLine(" to specifiy local port");

Console.WriteLine("");

app.Run();

Console.WriteLine("Exiting Web App");

IResult Check_Secure(HttpContext context)
{
    if (context.Request.Headers.Keys.Contains("bearer")) // && context.Request.Headers["bearer"] == TOKEN)
    {
        return Results.Ok(new { Results = "Success!" });
    }

    return Results.Forbid();
}

IResult Echo_Result(HttpContext context)
{
    var header = context.Request.Headers;
    var cookies = context.Request.Cookies;
    var query = context.Request.Query;
    var verb = context.Request.Method;
    var bodyTask = StreamToString(context.Request.Body);
    bodyTask.Wait();

    var body = bodyTask.Result;

    var results = Results.Ok(new { Verb = verb, Query = query, Headers = header, Cookies = cookies, Body = body });
    return results;
}

async Task<string> StreamToString(Stream stream)
{
    var reader = new StreamReader(stream);
    var result = await reader.ReadToEndAsync();
    return result;
}

public partial class Program
{ } // needed for test visability


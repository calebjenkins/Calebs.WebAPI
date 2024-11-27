using System.Reflection;
using CommandLine;
using FakeAuth; 
using Calebs.Extensions.Console;
using Calebs.WebAPI;


// Let's get started! 
var builder = WebApplication.CreateBuilder(args);

// Using FakeAuth so we can send 403s without all the auth hassel
builder.Services.AddAuthentication().AddFakeAuth();

var app = builder.Build();

// Using CommandLine Parcer nuget package
var parser = new Parser((x) =>
{
    x.IgnoreUnknownArguments = true;
    x.CaseSensitive = false;
});
var opt = parser.ParseArguments<Options>(args).Value;

if (opt != null && opt.ProcessModels)
{
    
}



// Hello Endpoints
app.MapGet("/hello", () => { return "hello world"; })
    .WithOpenApi().AllowAnonymous();

app.MapGet("/hello/{name}", (string name) => { return $"hello {name}"; })
        .WithOpenApi().AllowAnonymous();

// Echo Endpoints
app.MapGet("/echo", Echo_Result);
app.MapPost("/echo", Echo_Result);
app.MapDelete("/echo", Echo_Result);
app.MapPatch("/echo", Echo_Result);
app.MapPut("/echo", Echo_Result);

// Secure Endpoints
const string TOKEN = "fakeToken123xyz";
app.MapGet("FakeToken", () => TOKEN).AllowAnonymous();
app.MapPost("FakeToken", () => TOKEN).AllowAnonymous();

app.MapGet("/Secure", Check_Secure);

// Set Up Model/Data end points
if(args.Contains("--models"))
{
    app.MapGet("/test", () => { return "testing 123"; })
        .WithOpenApi().AllowAnonymous();
}


// *********************
// **  Splash Screen  **
// *********************

// This is since we are deploying as a dotnet tool. 
// So we want to provide some context when this is run.

var url = app.Urls.FirstOrDefault();
var ver = Assembly.GetExecutingAssembly().GetName().Version;

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

// Run this thing. 
app.Run();

// bye!
Console.WriteLine("Exiting Web App");


// Secure Endpoint
IResult Check_Secure(HttpContext context)
{
    if (context.Request.Headers.Keys.Contains("bearer")) // && context.Request.Headers["bearer"] == TOKEN)
    {
        return Results.Ok(new { Results = "Success!" });
    }

    return Results.Forbid();
}

// Echo Endpoint
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

// streams are weird.. 
async Task<string> StreamToString(Stream stream)
{
    var reader = new StreamReader(stream);
    var result = await reader.ReadToEndAsync();
    return result;
}

// needed for test visability
public partial class Program { }


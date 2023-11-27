using FakeAuth; // Using FakeAuth so I can send a 403 without setting up all the needed auth infrastructure


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication().AddFakeAuth();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();



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


app.Run();

IResult Check_Secure(HttpContext context)
{
    if (context.Request.Headers.Keys.Contains("bearer") && context.Request.Headers["bearer"] == TOKEN)
    {
        return Results.Ok(new { Results = "Suceess!" });
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

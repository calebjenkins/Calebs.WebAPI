using Microsoft.AspNetCore.Mvc;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.MapGet("/hello", () =>
{
    return "hello world";
});
//.WithOpenApi();

app.MapGet("/hello/{name}", (string name)=>
{
    return $"hello {name}";
});


app.Map("/echo/", () =>
{

});

app.Run();


using NotesApp.Api.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

const string ClientCors = "client";

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSingleton<INoteService, InMemoryNoteService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(ClientCors, policy => policy
        .WithOrigins("http://localhost:5173")
        .AllowAnyHeader()
        .AllowAnyMethod());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseCors(ClientCors);
app.MapControllers();

app.Run();

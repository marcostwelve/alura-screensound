using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScreenSound.API.Endpoints;
using ScreenSound.Banco;
using ScreenSound.Modelos;
using ScreenSound.Shared.Modelos.Modelos;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ScreenSoundContext>((opt) =>
{
    opt.UseSqlServer(builder.Configuration["ConnectionStrings:ScreenSoundDb"]).UseLazyLoadingProxies();
});

builder.Services.AddTransient<DAL<Artista>>();
builder.Services.AddTransient<DAL<Musica>>();
builder.Services.AddTransient<DAL<Genero>>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => 
options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
builder.Services.AddCors();
var app = builder.Build();

app.UseCors(options =>
{
    options.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();

});

//builder.Services.AddCors(
//        options => options.AddPolicy(
//            "wasm",
//            policy => policy.WithOrigins([builder.Configuration["BackendUrl"] ?? "https://localhost:7021",
//            builder.Configuration["ForntendUrl"] ?? "https://localhost:7021"])
//            .AllowAnyMethod()
//            .SetIsOriginAllowed(pol => true)
//            .AllowAnyHeader()
//            .AllowCredentials()));


//app.UseCors("wasm");

app.UseStaticFiles();

app.AddEndPointsArtistas();
app.AddEndPointsMusicas();
app.AddEndPointsGeneros();

app.UseSwagger();
app.UseSwaggerUI();



app.Run();

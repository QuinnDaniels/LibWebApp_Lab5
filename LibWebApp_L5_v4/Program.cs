using LibWebApp_L5_v4.Components;


using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Globalization;
using CsvHelper;
using LibWebApp_L5_v4.Services;




//using LibWebApp_L5_v4.Data;
//using LibWebApp_L5_v4.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<ILibraryService, LibraryService>();

builder.Services.AddSingleton<ILibServ, LibServ>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

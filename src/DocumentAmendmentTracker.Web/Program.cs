using DocumentAmendmentTracker.Web.Components;
using DocumentAmendmentTracker.Web.Data;
using DocumentAmendmentTracker.Web.Options;
using DocumentAmendmentTracker.Web.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddRadzenComponents();

builder.Services.Configure<FileStorageOptions>(
    builder.Configuration.GetSection(FileStorageOptions.SectionName));

builder.Services.AddDbContextFactory<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AppDbContext")));

builder.Services.AddScoped<IFileStorageService, FileStorageService>();
builder.Services.AddScoped<IRecordService, RecordService>();

// Raise the SignalR message size limit so attachment uploads up to FileStorage:MaxFileSizeBytes
// can stream over the Blazor Server circuit (default is 32KB).
var maxFileSizeBytes = builder.Configuration.GetValue<long>("FileStorage:MaxFileSizeBytes", 50 * 1024 * 1024);
builder.Services.Configure<HubOptions>(options =>
{
    options.MaximumReceiveMessageSize = maxFileSizeBytes;
});

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

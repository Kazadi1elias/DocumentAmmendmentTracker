using CurrieTechnologies.Razor.SweetAlert2;
using DocumentAmendmentTracker.Web.Data;
using DocumentAmendmentTracker.Web.Options;
using DocumentAmendmentTracker.Web.Services;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
    .AddNegotiate();
builder.Services.AddAuthorization(options =>
{
    // Every page requires a signed-in Windows/AD user unless explicitly marked [AllowAnonymous].
    options.FallbackPolicy = options.DefaultPolicy;
});

builder.Services.AddRadzenComponents();
builder.Services.AddSweetAlert2();

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
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

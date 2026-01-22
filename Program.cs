using Azure;
using Azure.AI.ContentSafety;
using DemoContentSafety.Components;
using DemoContentSafety.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Azure Content Safety Client
var endpoint = builder.Configuration["AzureContentSafety:Endpoint"] ?? string.Empty;
var key = builder.Configuration["AzureContentSafety:Key"] ?? string.Empty;

if (!string.IsNullOrEmpty(endpoint) && !string.IsNullOrEmpty(key))
{
    builder.Services.AddSingleton(new ContentSafetyClient(
        new Uri(endpoint),
        new AzureKeyCredential(key)
    ));
    builder.Services.AddScoped<ITeamNameSafetyService, TeamNameSafetyService>();
}
else
{
    // Fallback: Content Safety未設定の場合は NoOp サービスを使用
    builder.Services.AddScoped<ITeamNameSafetyService, NoOpTeamNameSafetyService>();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

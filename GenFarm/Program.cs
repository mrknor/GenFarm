using GenFarm;
using GenFarm.Services;

var builder = WebApplication.CreateBuilder(args);

// Load configuration
var apiSettings = builder.Configuration.GetSection("ApiSettings").Get<ApiSettings>();


// Add services to the container
builder.Services.AddControllersWithViews();

// Register the IHttpClientFactory
builder.Services.AddHttpClient();

// Register your services with configuration
builder.Services.AddSingleton<GenFarm.Infrastructure.MessageQueue>();
builder.Services.AddSingleton<GenFarm.Infrastructure.TaskOrchestrator>();
builder.Services.AddSingleton<GenFarm.Services.SEOKeywordAgent>();
builder.Services.AddSingleton<GenFarm.Services.HeaderGenerationAgent>(provider =>
    new HeaderGenerationAgent(provider.GetRequiredService<IHttpClientFactory>(), apiSettings.ApiKey));
builder.Services.AddSingleton<GenFarm.Services.BodyGenerationAgent>(provider =>
    new BodyGenerationAgent(provider.GetRequiredService<IHttpClientFactory>(), apiSettings.ApiKey));
builder.Services.AddSingleton<GenFarm.Services.EditorAgent>();
builder.Services.AddSingleton<GenFarm.Services.SEOOptimizationAgent>();
builder.Services.AddSingleton<GenFarm.Services.DeploymentAgent>();

var app = builder.Build();

// Manually resolve the MessageQueue to force its initialization
var messageQueue = app.Services.GetRequiredService<GenFarm.Infrastructure.MessageQueue>();
messageQueue.PurgeQueue();
var orchestrator = app.Services.GetRequiredService<GenFarm.Infrastructure.TaskOrchestrator>();
orchestrator.StartOrchestration();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); // Ensure controllers are mapped
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using SupervisorioSIMMAQ_NXA.Data;
using SupervisorioSIMMAQ_NXA.Options;
using SupervisorioSIMMAQ_NXA.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<MqttOptions>(builder.Configuration.GetSection("Mqtt"));
builder.Services.Configure<AiDiagnosticOptions>(builder.Configuration.GetSection("AiDiagnostic"));

builder.Services.AddDbContext<SupervisorioDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<MqttConsumerService>();
builder.Services.AddHostedService<IndustrialDiagnosticService>();
builder.Services.AddScoped<TagValueService>();
builder.Services.AddHttpClient<AiDiagnosticClient>();
builder.Services.AddSingleton<MachineComponentMap>();
builder.Services.AddSingleton<AssistantConversationMemory>();
builder.Services.AddScoped<IndustrialAgentOrchestrator>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SupervisorioDbContext>();
    await DatabaseSeeder.SeedAsync(dbContext);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var frontEndPath = Path.Combine(app.Environment.ContentRootPath, "FrontEnd");
if (Directory.Exists(frontEndPath))
{
    app.UseDefaultFiles(new DefaultFilesOptions
    {
        FileProvider = new PhysicalFileProvider(frontEndPath)
    });
    app.UseStaticFiles(new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(frontEndPath)
    });
}

app.UseCors("Frontend");

app.UseAuthorization();

app.MapControllers();

app.Run();

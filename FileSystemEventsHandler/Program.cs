using FileMonitor;
using FileMonitor.Implementations;
using FileMonitor.Interfaces;
using log4net;
using log4net.Config;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


BasicConfigurator.Configure();

builder.Services.AddSingleton<ILog>(_ => LogManager.GetLogger(nameof(FileMonitor)));
builder.Services.AddSingleton<InputValidator>();
builder.Services.AddSingleton<ILogsCacheManager, LogsCacheManager>();
builder.Services.AddSingleton<IPrintEventLogs, PrintEventLogs>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

using FileMonitor;
using FileMonitor.Interfaces;
using FileMonitor.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// TODO fix DI - inject maxNumberOfFoldersToMonitor, create FileSystemMonitorFactory and inject here
builder.Services.AddSingleton<IFileSystemWatcherMonitor>(_ => new FileSystemWatcherMonitor(Consts.DefualtMaxNumberOfFoldersToMonitor));


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

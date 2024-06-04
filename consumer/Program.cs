using FlightInfoConsumer.Services;
using FlightInfoConsumer.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContextFactory<FlightInfoDbContext>(options =>
{
    var source = builder.Configuration.GetSection("SQLite:Source");
    options.UseSqlite("Data Source=" + source.Value);
});

builder.Services.AddHostedService<ConsumerService>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
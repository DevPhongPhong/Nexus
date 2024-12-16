using EIM.Attributes.FilterPipelines.Authorizations;
using Microsoft.EntityFrameworkCore;
using Nexus;
using Nexus.Services;

var builder = WebApplication.CreateBuilder(args);
var locationDbConnectionString = builder.Configuration.GetConnectionString("Nexus");

builder.Services.AddDbContext<NexusDbContext>(options =>
    options.UseMySql(locationDbConnectionString,
        ServerVersion.AutoDetect(locationDbConnectionString)));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IRentalShopService, RentalShopService>();
builder.Services.AddScoped<NexusAuthorizationFilter>();

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

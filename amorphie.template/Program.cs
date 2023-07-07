using amorphie.core.Extension;
using amorphie.core.HealthCheck;
using amorphie.core.Swagger;
using amorphie.template.data;
using amorphie.template.HealthCheck;
using amorphie.template.Validator;
using FluentValidation;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Npgsql.Replication;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDaprClient();
builder.Services.AddHealthChecks().AddBBTHealthCheck();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options=>
{
    options.OperationFilter<AddSwaggerParameterFilter>();
});

builder.Services.AddValidatorsFromAssemblyContaining<StudentValidator>(includeInternalTypes: true);
builder.Services.AddAutoMapper(typeof(Program).Assembly);


builder.Services.AddDbContext<TemplateDbContext>
    // (options => options.UseInMemoryDatabase("TemplateDbContext"));
    (options => options.UseNpgsql("Host=localhost:5432;Database=TemplateDb;Username=postgres;Password=postgres", b => b.MigrationsAssembly("amorphie.template.data")));


var app = builder.Build();


using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<TemplateDbContext>();

db.Database.Migrate();
DbInitializer.Initialize(db);


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.AddRoutes();

app.MapHealthChecks("/healthz", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});
app.MapMetrics();

app.Run();


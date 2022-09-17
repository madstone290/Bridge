using Bridge.Application;
using Bridge.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Mvc.Formatters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    var noContentFormatter = options.OutputFormatters.OfType<HttpNoContentOutputFormatter>().FirstOrDefault();
    if (noContentFormatter != null)
    {
        noContentFormatter.TreatNullValueAsNoContent = false;
    }
});

// Swagger API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(typeof(ApplicationReference).Assembly);

builder.Services.AddInfrastructureDependency(builder.Configuration);

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

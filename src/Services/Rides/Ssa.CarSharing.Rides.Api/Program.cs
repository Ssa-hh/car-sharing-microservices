using Carter;
using Ssa.CarSharing.Common.Presentation.Middlewares;
using Ssa.CarSharing.Rides.Api;
using Ssa.CarSharing.Rides.Application;
using Ssa.CarSharing.Rides.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Aspire
builder.AddServiceDefaults();
builder.AddDefaultMassTransit(typeof(Program).Assembly);


builder.Services.AddApplication();

builder.AddInfrastructure();
builder.AddApi();

// TODO: remove it or refine it
var allowFrontendOrigins = "_allowFrontendOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowFrontendOrigins,
                      policy =>
                      {
                          policy.WithOrigins("*")
                          .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI();
}

// TODO: remove it or refine it
app.UseCors(allowFrontendOrigins);

app.UseHttpsRedirection();

app.UseMiddleware<CustomExceptionHandler>();

app.UseAuthorization();

app.MapCarter();

app.Run();

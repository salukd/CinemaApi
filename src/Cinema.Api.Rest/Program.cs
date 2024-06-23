using Cinema.Api;
using Cinema.Application;
using Cinema.Application.ExpiredReservationsWorker;
using Cinema.Infrastructure;
using Cinema.Infrastructure.Common.Persistence;
using Coravel;
using Hellang.Middleware.ProblemDetails;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddPresentation()
    .AddWorker()
    .AddApplicationServices()
    .AddInfrastructureServices(configuration);


var app = builder.Build();

app.Services.UseScheduler(scheduler =>
{
    scheduler
        .Schedule<RemoveExpiredReservations>()
        .EverySeconds(5)
        .PreventOverlapping("RemoveExpiredReservations");
});

app.UseProblemDetails();

app.UseExceptionHandler(options => { });

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();



SampleData.Initialize(app);

app.Run();

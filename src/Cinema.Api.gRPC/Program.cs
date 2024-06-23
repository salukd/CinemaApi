using Cinema.Application;
using Cinema.Application.ExpiredReservationsWorker;
using Cinema.gRPC.Api;
using Cinema.gRPC.Api.Services;
using Cinema.Infrastructure;
using Cinema.Infrastructure.Common.Persistence;
using Coravel;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<ExceptionHandlingInterceptor>();
});


builder.Services.AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddWorker();

var app = builder.Build();

app.Services.UseScheduler(scheduler =>
{
    scheduler
        .Schedule<RemoveExpiredReservations>()
        .EverySeconds(5)
        .PreventOverlapping("RemoveExpiredReservations");
});

app.MapGrpcService<TicketService>();
app.MapGrpcService<ShowtimeService>();

app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

SampleData.Initialize(app);

app.Run();
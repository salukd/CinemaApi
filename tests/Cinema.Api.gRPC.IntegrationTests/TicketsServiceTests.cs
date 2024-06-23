using Cinema.gRPC.Api;
using FluentAssertions;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.Testing;
using Empty = Google.Protobuf.WellKnownTypes.Empty;

namespace Cinema.Api.gRPC.IntegrationTests;

[Collection("ApplicationFactory")]
public class TicketsServiceTests 
{
    private readonly WebApplicationFactory<IApiMarker> _factory;

    public TicketsServiceTests(ApplicationFactory factory)
    {
        _factory = factory;
    }
    
    [Fact]
    public async Task CreateReservation_CreatesAndReturnsReservation()
    {
        // Arrange
        var client = CreateClient();

        var request = new CreateReservationRequest
        {
            ShowtimeId = 1,
            Row = 5,
            Seats = { 1, 2, 3 }
        };

        // Act
        var response = await client.CreateReservationAsync(request);

        // Assert
        response.Should().NotBeNull();
        response.Id.Should().NotBeNullOrEmpty();
        response.AuditoriumId.Should().Be(1);
        response.SeatsCount.Should().Be(3);
        response.MovieTitle.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task PurchaseTicket_PurchasesSuccessfully()
    {
        // Arrange
        var client = CreateClient();

        var createRequest = new CreateReservationRequest
        {
            ShowtimeId = 1,
            Row = 6,
            Seats = { 4,5,6 }
        };

        var createResponse = await client.CreateReservationAsync(createRequest);

        var purchaseRequest = new PurchaseTicketRequest
        {
            ReservationId = createResponse.Id
        };

        // Act
        var purchaseResponse = await client.PurchaseTicketAsync(purchaseRequest);

        // Assert
        purchaseResponse.Should().BeOfType<Empty>();
    }
    
    private TicketsApi.TicketsApiClient CreateClient()
    {
        var client = _factory.CreateDefaultClient();
        var channel = GrpcChannel.ForAddress(client.BaseAddress, new GrpcChannelOptions { HttpClient = client });
        return new TicketsApi.TicketsApiClient(channel);
    }
}
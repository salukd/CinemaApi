using Cinema.gRPC.Api;
using FluentAssertions;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.Testing;
using Empty = Google.Protobuf.WellKnownTypes.Empty;

namespace Cinema.Api.gRPC.IntegrationTests;


[Collection("ApplicationFactory")]
public class ShowtimeServiceTests 
{
    private readonly WebApplicationFactory<IApiMarker> _factory;

    public ShowtimeServiceTests(ApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetById_ReturnsCorrectShowtime()
    {
        // Arrange
        var client = CreateClient();
       
    
        // Act
        var response = await client.GetByIdAsync(new IdRequest { Id = 1 });
    
        // Assert
        response.Should().NotBeNull();
        response.Id.Should().Be(1);
    }

    [Fact]
    public async Task Search_ReturnsCorrectShowtimes()
    {
        // Arrange
        var client = CreateClient();

        // Act
        var response = await client.SearchAsync(new SearchRequest { Text = "Incep" });

        // Assert
        response.Showtimes.Should().NotBeEmpty();
        response.Showtimes.First().MovieTitle.Should().Contain("Incep");
    }

    [Fact]
    public async Task GetAll_ReturnsAllShowtimes()
    {
        // Arrange
        var client = CreateClient();
       
    
        // Act
        var response = await client.GetAllAsync(new Empty());
    
        // Assert
        response.Showtimes.Should().NotBeEmpty();
    }

    [Fact]
    public async Task CreateShowtime_CreatesAndReturnsShowtime()
    {
        // Arrange
        var client = CreateClient();
    
        // Act
        var response = await client.CreateShowtimeAsync(new CreateShowtimeRequest
        {
            AuditoriumId = 1,
            MovieId = 1,
            SessionDate = Timestamp.FromDateTime(System.DateTime.UtcNow)
        });
    
        // Assert
        response.Should().NotBeNull();
        response.Id.Should().BeGreaterThan(0);
        response.MovieTitle.Should().Be("Inception");
    }

    private Showtime.ShowtimeClient CreateClient()
    {
        var client = _factory.CreateDefaultClient();
        var channel = GrpcChannel.ForAddress(client.BaseAddress, new GrpcChannelOptions { HttpClient = client });
        return new Showtime.ShowtimeClient(channel);
    }
}
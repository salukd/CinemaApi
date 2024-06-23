using Cinema.gRPC.Api;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Cinema.Api.gRPC.IntegrationTests;

public class ApplicationFactory : WebApplicationFactory<IApiMarker>
{
    
}

[CollectionDefinition("ApplicationFactory")]
public sealed class ApplicationFactoryCollection : ICollectionFixture<ApplicationFactory>
{
    
}
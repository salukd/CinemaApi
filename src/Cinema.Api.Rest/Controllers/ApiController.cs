using Asp.Versioning;


namespace Cinema.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class ApiController : ControllerBase
{
    protected readonly ISender Mediator;

    public ApiController(ISender mediator)
    {
        Mediator = mediator;
    }


}
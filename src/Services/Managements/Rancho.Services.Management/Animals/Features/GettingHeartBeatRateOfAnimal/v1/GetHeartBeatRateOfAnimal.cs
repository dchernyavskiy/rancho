using Ardalis.ApiEndpoints;
using Asp.Versioning;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Rancho.Services.Management.Animals.Features.GettingAnimalById.v1;
using Rancho.Services.Management.Hubs;

namespace Rancho.Services.Management.Animals.Features.GettingHeartBeatRateOfAnimal.v1;

public record GetHeartBeatRateOfAnimal(int Beat, Guid AnimalId) : IRequest;

public class GetHeartBeatRateOfAnimalHandler : IRequestHandler<GetHeartBeatRateOfAnimal>
{
    public async Task<Unit> Handle(GetHeartBeatRateOfAnimal request, CancellationToken cancellationToken)
    {
        return Unit.Value;
    }
}

public class GetHeartBeatRateOfAnimalEndpoint : EndpointBaseAsync.WithRequest<GetHeartBeatRateOfAnimal>.WithoutResult
{
    private readonly IHubContext<HeartHub> _context;

    public GetHeartBeatRateOfAnimalEndpoint(IHubContext<HeartHub> context)
    {
        _context = context;
    }

    [HttpPost(AnimalConfigs.AnimalsPrefixUri + "/post-heart-beat", Name = "GetHeartBeatRateOfAnimal")]
    [ProducesResponseType(typeof(GetAnimalByIdResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(StatusCodeProblemDetails), StatusCodes.Status400BadRequest)]
    [ApiVersion(1.0)]
    public override async Task HandleAsync(
        GetHeartBeatRateOfAnimal request,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        await _context.Clients.Group(request.AnimalId.ToString())
            .SendAsync("ReceiveMessage", new HearBeatMessage(request.Beat), cancellationToken: cancellationToken);
        //await _context.Clients.All.SendAsync("ReceiveMessage", new Message() {Content = request.Content}, cancellationToken: cancellationToken);
    }
}

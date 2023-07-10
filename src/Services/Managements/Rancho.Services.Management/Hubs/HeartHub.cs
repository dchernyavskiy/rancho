using MediatR;
using Microsoft.AspNetCore.SignalR;
using Rancho.Services.Management.Animals.Features.GettingHeartBeatRateOfAnimal.v1;

namespace Rancho.Services.Management.Hubs;

public class HeartHub : Hub
{
    private readonly IMediator _mediator;
    private readonly IHubContext<HeartHub> _context;

    public HeartHub(IMediator mediator, IHubContext<HeartHub> context)
    {
        _mediator = mediator;
        _context = context;
    }

    public async Task JoinGroup(Guid animalId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, animalId.ToString());
    }

    public async Task SendMessage(HearBeatMessage message)
    {
        //await _mediator.Send(new GetHeartBeatRateOfAnimal(message.Content));
        await SendToAllClients(message);
    }

    public async Task SendToAllClients(HearBeatMessage message)
    {
        await _context.Clients.All.SendAsync("ReceiveMessage", message);
    }
}

public record HearBeatMessage(int Beat);

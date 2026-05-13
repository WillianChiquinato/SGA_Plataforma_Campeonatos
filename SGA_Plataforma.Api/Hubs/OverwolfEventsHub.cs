using Microsoft.AspNetCore.SignalR;

namespace SGA_Plataforma.Api.Hubs;

public sealed class OverwolfEventsHub : Hub
{
    public const string IngestionGroup = "ingestion:all";

    public static string MatchGroup(int matchId) => $"match:{matchId}";

    public Task JoinMatch(int matchId)
    {
        return Groups.AddToGroupAsync(Context.ConnectionId, MatchGroup(matchId));
    }

    public Task LeaveMatch(int matchId)
    {
        return Groups.RemoveFromGroupAsync(Context.ConnectionId, MatchGroup(matchId));
    }

    public Task JoinIngestionFeed()
    {
        return Groups.AddToGroupAsync(Context.ConnectionId, IngestionGroup);
    }

    public Task LeaveIngestionFeed()
    {
        return Groups.RemoveFromGroupAsync(Context.ConnectionId, IngestionGroup);
    }
}
using Marten.Events;

namespace Helpdesk.Api.Incidents;

public record IncidentProjection(
    // marten stuff
    Guid Id,
    long Version,
    long Sequence,
    // our stuff
    IncidentStatus Status,
    Guid? CustomerId
)
{
    public static IncidentProjection Create(IEvent<IncidentLogged> e) =>
        new(default!, e.Version, e.Sequence, IncidentStatus.Pending, e.Data.CustomerId);
    public static IncidentProjection Create(IEvent<IncidentClosed> e) =>
        new(default!, e.Version, e.Sequence, IncidentStatus.Closed, null);
    public bool ShouldDelete(IncidentClosed _) => true;
}

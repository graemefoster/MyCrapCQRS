namespace EventSourcing
{
    internal interface IReplayEvents
    {
        void ApplyChanges(IEvent @event);
    }
}
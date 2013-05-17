using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace EventSourcing
{
    internal static class EventStore
    {
        private static readonly JsonSerializer Serializer;
        private static readonly Dictionary<Guid, IList<IEvent>> Events = new Dictionary<Guid, IList<IEvent>>();
        private static IReplayEvents _projection;

        static EventStore()
        {
            Serializer = new JsonSerializer() { Formatting = Formatting.Indented};
        }

        public static void Write(IEvent @event)
        {
            Serializer.Serialize(Console.Out, @event, @event.GetType());
            Console.WriteLine();
            if (Events.ContainsKey(@event.Id))
            {
                Events[@event.Id].Add(@event);
            }
            else
            {
                var events = new List<IEvent> {@event};
                Events.Add(@event.Id, events);
            }
            _projection.ApplyChanges(@event);
        }

        public static T Get<T>(Guid id) where T : IReplayEvents, new()
        {
            var newobj = new T();
            foreach (IEvent @event in Events[id])
            {
                newobj.ApplyChanges(@event);
            }
            return newobj;
        }

        public static IEvent Create(IEvent @event)
        {
            Write(@event);
            return @event;
        }

        public static void RegisterProjection(IReplayEvents projection)
        {
            _projection = projection;
        }
    }
}
//Base class type that allows for the creation of event datas

namespace Events
{
    public class EventData
    {
        public readonly EventIdentifiers EventIdentifiers;

        public EventData(EventIdentifiers identifiers)
        {
            EventIdentifiers = identifiers;
        }
    }
}

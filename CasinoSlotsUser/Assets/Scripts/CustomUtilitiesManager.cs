    using Events;

    public static class CustomUtilitiesManager
    {
        /// <summary>
        /// Allows for event data to be casted and outputted if it is of an expected type.
        /// </summary>
        /// <typeparam name="T">The type of custom event data.</typeparam>
        /// <param name="eventData">The event data being checked.</param>
        /// <param name="outEvent">The casted and outputted event, which sets the expected type.</param>
        /// <param name="throwError">If the method should throw an exception error upon failure.</param>
        /// <returns>If the given event was of the expected type.</returns>
        public static bool IsEventOfType<T>(this EventData eventData, out T outEvent, bool throwError = true)
            where T : class
        {
            if (typeof(T) == eventData.GetType())
            {
                outEvent = eventData as T;
                return true;
            }
            else if (throwError)
            {
                throw new System.InvalidCastException(string.Concat("Error: EventData class with EventType.",
                    eventData.EventIdentifiers, " was received but is not of class ", typeof(T).FullName, "."));
            }

            outEvent = null;
            return false;
        }
    }

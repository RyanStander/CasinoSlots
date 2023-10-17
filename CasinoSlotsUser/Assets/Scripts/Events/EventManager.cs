using System;
using System.Collections.Generic;
using UnityEngine;

//Manages and controls events that listeners can use to share and exchange data without being linked together
namespace Events
{
    public class EventManager : MonoBehaviour
    {
        //------------------------------------------------------------------------------------------------------------------------
        //                                                  Singleton
        //------------------------------------------------------------------------------------------------------------------------
        //Getter and setter for the current manager, static so that there is only one manager at any given time
        public static EventManager currentManager { get; set; } = null;

        //Awake function ensures that only one copy exists in the scene at a given time
        private void Awake()
        {
            if (currentManager == null)
            {
                currentManager = this; //Sets the active manager to this instance of it
            }
            else
            {
                Destroy(gameObject);
            }
        }

        //------------------------------------------------------------------------------------------------------------------------
        //                                                 Event Queue Methods
        //------------------------------------------------------------------------------------------------------------------------
        public delegate void EventHandler(EventData eventData); //Defines the method signature
        private Dictionary<EventIdentifiers, EventHandler> subscriberDictionary = new Dictionary<EventIdentifiers, EventHandler>(); //Holds a dictionary of all the subscribers
        private Queue<EventData> eventQueue = new Queue<EventData>(); //Queues up the events that are coming in

        //Subscribes a listener from a specific event so that they will grab it when fired
        public void Subscribe(EventIdentifiers eventIdentifiers, EventHandler eventHandler)
        {
            //If key is not already present, then add a new key and value pair (event, handler)
            if (!subscriberDictionary.ContainsKey(eventIdentifiers))
            {
                EventHandler handler = null;
                subscriberDictionary.Add(eventIdentifiers, handler);
            }
            //Adds the specific handler to the assosiated key
            subscriberDictionary[eventIdentifiers] += eventHandler;
        }

        //Unsubscribes a listener from a specific event so that they will not grab it when fired
        public void Unsubscribe(EventIdentifiers eventIdentifiers, EventHandler eventHandler)
        {
            //If key is already present, then remove the key and value pair (event, handler)
            if (subscriberDictionary.ContainsKey(eventIdentifiers))
            {
                //If event handler is not present, following code will get ignored
                subscriberDictionary[eventIdentifiers] -= eventHandler;
            }
            else
            {
                //Throw an error (Log file)
                Console.WriteLine("Warning: Event type " + eventIdentifiers.ToString()+" doesn't exist in the event manager's subscriber dictionary");

                //Unity Player
                Debug.Log("Warning: Event type " + eventIdentifiers.ToString() + " doesn't exist in the event manager's subscriber dictionary");
            }
        }

        //Adds given event to the queue
        public void AddEvent(EventData eventData)
        {
            //Error handling
            if (!Enum.IsDefined(typeof(EventIdentifiers),eventData.EventIdentifiers))
            {
                throw new ArgumentOutOfRangeException(eventData.EventIdentifiers.ToString(),"Event type is invalid");
            }

            //Add the event to the queue so that it may be proccesed
            eventQueue.Enqueue(eventData);
        }

        //Iterates through the event queue, invokes the event, and then removes the event.
        public void PublishEvents()
        {
            //Forloop for all current queued up events
            for (int i = eventQueue.Count - 1; i >=0;i--)
            {
                //Peeks the frontward item of the queue
                EventData data = eventQueue.Peek();

                //Check if the dictionary already contains this event type
                if (subscriberDictionary.ContainsKey(data.EventIdentifiers))
                {
                    //Invoke/Fire off the event for all of its listeners
                    subscriberDictionary[data.EventIdentifiers]?.Invoke(data);
                }
                else
                {
                    //Throw an error (Log file)
                    Console.WriteLine("Warning: Event type " + data.ToString() + " doesn't exist in the event manager's subscriber dictionary");

                    //Unity Player
                    Debug.Log("Warning: Event type " + data.ToString() + " doesn't exist in the event manager's subscriber dictionary");
                }
                //Remove the event from the queue now that it has been resolved
                eventQueue.Dequeue();
            }
        }
        public void Update()
        {
            PublishEvents();
        }
    }
}

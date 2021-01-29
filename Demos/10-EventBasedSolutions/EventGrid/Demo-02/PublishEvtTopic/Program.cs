﻿using System;
using System.Collections.Generic;
using Microsoft.Azure.EventGrid;
using Microsoft.Azure.EventGrid.Models;
using Newtonsoft.Json;

namespace PublishTopic
{
    class Program
    {
        static void Main(string[] args)
        {
            var topic = "foodorder-topic";
            var region = "westeurope";

            // TODO: Enter value for topic-key from create script
            string topicKey = "eG3vxTHsHg95AWWr964DEmebNM8o0XvOlkCIq0afBjA=";
            string topicEndpoint = $"https://{topic}.{region}-1.eventgrid.azure.net/api/events";
            string topicHostname = new Uri (topicEndpoint).Host;

            TopicCredentials topicCredentials = new TopicCredentials (topicKey);
            EventGridClient client = new EventGridClient (topicCredentials);
            var evts = GetEventsList ();
            client.PublishEventsAsync (topicHostname, evts).GetAwaiter ().GetResult ();
            Console.Write ("Published events to Event Grid topic.");
        }

        static IList<EventGridEvent> GetEventsList () {
            List<EventGridEvent> eventsList = new List<EventGridEvent> ();

            for (int i = 0; i < 10; i++) {
                eventsList.Add (new EventGridEvent () {
                    Id = Guid.NewGuid ().ToString (),
                        EventType = "Contoso.Items.ItemReceived",
                        Data = new ContosoItemReceivedEventData () {
                            ItemSku = "Contoso Item SKU #1"
                        },
                        EventTime = DateTime.Now,
                        Subject = "Door1",
                        DataVersion = "2.0"
                });
            }

            return eventsList;
        }
    }

    class ContosoItemReceivedEventData {
        [JsonProperty (PropertyName = "itemSku")]
        public string ItemSku { get; set; }
    }
}

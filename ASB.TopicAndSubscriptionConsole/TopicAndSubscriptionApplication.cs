using System;
using System.Collections.Generic;
using ASB.Constants;
using ASB.Models;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace ASB.TopicAndSubscriptionConsole
{
    class TopicAndSubscriptionApplication
    {
        private static MessagingFactory _factory;
        private static NamespaceManager _namespaceManager;
        private static TopicClient _orderTopicClient;

        static void Main(string[] args)
        {
            CreateManagerAndFactory();
            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.WriteLine("Creating Topics and Subscriptions");
            CreateTopicAndSubscriptions();
            Console.WriteLine("Done!");

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Press any key to send a message");
            Console.ReadLine();

            _orderTopicClient = _factory.CreateTopicClient(Mother.OrderTopic);
            Console.WriteLine("Sending orders...");

            SendOrder(new Order
            {
                Name = "Loyal Customer",
                Value = 19.99m,
                Region = "USA",
                Items = 1,
                HasLoyaltyCard = true
            });

            SendOrder(new Order
            {
                Name = "Large Order",
                Value = 49.99m,
                Region = "USA",
                Items = 50,
                HasLoyaltyCard = false
            });

            SendOrder(new Order
            {
                Name = "High Value Order",
                Value = 749.95m,
                Region = "USA",
                Items = 45,
                HasLoyaltyCard = false
            });

            SendOrder(new Order
            {
                Name = "Loyalty Europe Order",
                Value = 49.95m,
                Region = "EU",
                Items = 3,
                HasLoyaltyCard = true
            });

            SendOrder(new Order
            {
                Name = "UK Order",
                Value = 49.95m,
                Region = "UK",
                Items = 3,
                HasLoyaltyCard = true
            });

            _orderTopicClient.Close();
        }

        private static void SendOrder(Order order)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Sending {order.Name}");
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new List<JsonConverter> {new StringEnumConverter()}
            };

            var json = JsonConvert.SerializeObject(order, settings);
            var message = new BrokeredMessage(json) {ContentType = @"application/json"};
            message.Properties.Add("Loyalty", order.HasLoyaltyCard);

        }

        private static void CreateTopicAndSubscriptions()
        {
            if (_namespaceManager.TopicExists(Mother.OrderTopic))
            {
                _namespaceManager.DeleteTopic(Mother.OrderTopic);
            }

            _namespaceManager.CreateTopic(Mother.OrderTopic);
            _namespaceManager.CreateSubscription(Mother.OrderTopic, "all-orders");

            _namespaceManager.CreateSubscription(Mother.OrderTopic, "usa-orders", new SqlFilter("Region = 'USA'"));
            _namespaceManager.CreateSubscription(Mother.OrderTopic, "eu-orders", new SqlFilter("Region = 'EU'"));

            _namespaceManager.CreateSubscription(Mother.OrderTopic, "large-orders", new SqlFilter("Items > 30"));
            _namespaceManager.CreateSubscription(Mother.OrderTopic, "large-value", new SqlFilter("Value > 500"));

            _namespaceManager.CreateSubscription(Mother.OrderTopic, "loyalty",
                new SqlFilter("Loyalty = true AND Region = 'USA'"));

            _namespaceManager.CreateSubscription(Mother.OrderTopic, "uk-orders", new CorrelationFilter("UK"));
        }

        private static void CreateManagerAndFactory()
        {
            _namespaceManager = NamespaceManager.CreateFromConnectionString(Mother.ConnectionString);
            _factory = MessagingFactory.CreateFromConnectionString(Mother.ConnectionString);
        }
    }
}
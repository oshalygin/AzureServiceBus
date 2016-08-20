using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;

namespace ASB
{
    public class Communication : ICommunication
    {
        public const string ConnectionString =
            @"Endpoint=sb://asboleg.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=4ac/HxC9hMAeDE3Y1MxmPuByCYM9Z+S8WkRKhnWZINM=";

        public const string QueuePath = "firstqueue";

        public void AddTenGenericMessages()
        {
            var queueClient = QueueClient.CreateFromConnectionString(ConnectionString, QueuePath);
            for (var i = 0; i < 10; i++)
            {
                var brokeredMessage = new BrokeredMessage($"Message #{i}");
                queueClient.Send(brokeredMessage);
                Console.WriteLine($"Sent - {brokeredMessage.GetBody<string>()}");
            }
            queueClient.Close();
        }

        public void RetrievAllMessagesFromTheQueue()
        {
            Console.WriteLine("started");
            var queueClient = QueueClient.CreateFromConnectionString(ConnectionString, QueuePath);
            for (var i = 0; i < 5; i++)
            {
                Console.WriteLine("processing");
                Task.WaitAll(queueClient.ReceiveAsync().ContinueWith(ProcessMessageAsync));

            }
            queueClient.Close();
            Console.WriteLine("done");
        }


        private async void ProcessMessageAsync(Task<BrokeredMessage> brokeredMessageTask)
        {
            Console.WriteLine("in here");
            var message = brokeredMessageTask.Result;
            Console.WriteLine($"{message.MessageId} - {message.GetBody<string>()}");
            await message.CompleteAsync();
        }

        private async void ProcessBatch(Task<IEnumerable<BrokeredMessage>> brokeredMessageTask)
        {
            await brokeredMessageTask.ContinueWith(result =>
                {
                    var messages = result.Result;
                    Console.WriteLine(messages.Count());
                    foreach (var message in messages)
                    {
                        Console.WriteLine($"{message.MessageId} - {message.GetBody<string>()}");
                        message.Complete();
                    }
                }
            );
        }
    }
}
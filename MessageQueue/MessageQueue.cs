using System;
using System.Threading.Tasks;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace MessageQueue
{
    public class MessageQueue : IMessageQueue
    {
        private readonly QueueClient _client;
        private const string SasKeyValue = @"RootManageSharedAccessKey";
        private const string SasKeyName = @"+6fwr+TjP4vD+yeuLi6iwMQQZqTWJ+RHjeikPQgAWEQ=";
        //@"Endpoint=sb://oshalygin.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=+6fwr+TjP4vD+yeuLi6iwMQQZqTWJ+RHjeikPQgAWEQ=";


        private const string ConnectionString =
            @"Endpoint=sb://oshalygin.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=+6fwr+TjP4vD+yeuLi6iwMQQZqTWJ+RHjeikPQgAWEQ=";

        private const string QueueName = @"samplequeue";

        public MessageQueue()
        {
            _client = QueueClient.CreateFromConnectionString(ConnectionString, QueueName);
        }

        public void SendBrokeredMessage(string message)
        {
            var brokeredMessage = new BrokeredMessage(message);
            _client.Send(brokeredMessage);
        }

        public BrokeredMessage RetrieveBrokeredMessage()
        {
            BrokeredMessage brokeredMessage = null;
            _client.OnMessage(message =>
            {
                Console.WriteLine(message.GetBody<string>());
                brokeredMessage = message;
            });
            return brokeredMessage;
        }

        public async Task Queue()
        {
            var credentials = TokenProvider.CreateSharedAccessSignatureTokenProvider(SasKeyName, SasKeyValue);
            var namespaceClient = new NamespaceManager(ServiceBusEnvironment.CreateServiceUri("sb", "oshalygin", string.Empty));
        }
    }
}
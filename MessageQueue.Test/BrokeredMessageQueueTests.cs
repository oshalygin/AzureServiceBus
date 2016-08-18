using System;
using NUnit.Framework;

namespace MessageQueue.Test
{
    [TestFixture]
    public class BrokeredMessageQueueTests
    {
        private MessageQueue _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new MessageQueue();
        }

        [Test]
        public void Should_Send_Message_To_Queue()
        {
            const string message = "Some other message";
            _sut.SendBrokeredMessage(message);
        }

        [Test]
        public void Should_Retrieve_Brokered_Message_From_Queue()
        {
            var brokeredMessage = _sut.RetrieveBrokeredMessage();
            Console.WriteLine(brokeredMessage.MessageId);
            //Console.WriteLine(brokeredMessage.GetBody<string>());
        }
    }
}
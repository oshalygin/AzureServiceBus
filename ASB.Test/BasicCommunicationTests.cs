using System.Threading.Tasks;
using NUnit.Framework;

namespace ASB.Test
{
    [TestFixture]
    public class BasicCommunicationTests
    {
        private ICommunication _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new Communication();
        }

        [Test]
        public void Should_Send_Generic_Messages_To_Queue()
        {
            _sut.AddTenGenericMessages();
        }

        [Test]
        public void Should_Read_All_Messages_From_Queue()
        {
           _sut.RetrievAllMessagesFromTheQueue();
        }
    }
}
using System;
using Microsoft.ServiceBus.Messaging;

namespace ASB.SenderConsole
{
    public class Program
    {

        private static ICommunication _sut;
        static void Main(string[] args)
        {
            _sut = new Communication();
            _sut.RetrievAllMessagesFromTheQueue();
            Console.ReadLine();
        }
    }
}
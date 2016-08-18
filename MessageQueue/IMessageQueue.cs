namespace MessageQueue
{
    public interface IMessageQueue
    {
        void SendBrokeredMessage(string message);
    }
}
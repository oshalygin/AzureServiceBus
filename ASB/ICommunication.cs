using System.Threading.Tasks;

namespace ASB
{
    public interface ICommunication
    {
        void AddTenGenericMessages();
        void RetrievAllMessagesFromTheQueue();
    }
}       
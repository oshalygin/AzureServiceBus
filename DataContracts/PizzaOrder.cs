using System.Runtime.Serialization;

namespace DataContracts
{
    [DataContract]
    public class PizzaOrder
    {
        [DataMember]
        public string CustomerName { get; set; }
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public string Size { get; set; }
    }
}
namespace ASB.Constants
{
    public static class Mother
    {
        public static string ConnectionString =
            @"Endpoint=sb://asboleg.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=4ac/HxC9hMAeDE3Y1MxmPuByCYM9Z+S8WkRKhnWZINM=";

        public static string QueuePath = "first-queue";
        public static string ForwardingPath = "forwarded-queue";
        public static string TopicPath = "chattopic";
        public static string OrderTopic = "order-topic";
    }
}
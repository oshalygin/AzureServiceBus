using System;

namespace ASB.ManagementConsole
{
    class ManagementProgram
    {
        static void Main(string[] args)
        {
            var helper = new ManagementHelper();
            bool done = false;
            do
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(">");
                var command = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.Magenta;
                var commands = command.Split(' ');

                try
                {
                    if (commands.Length > 0)
                    {
                        switch (commands[0])
                        {
                            case "createqueue":
                            case "cq":
                                if (commands.Length > 1)
                                {
                                    helper.CreateQueue(commands[1]);
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("Queue path wasnt specified");
                                }
                                break;
                            case "listqueues":
                            case "ls":
                                helper.ListQueues();
                                break;
                            case "getQueues":
                            case "get":
                                if (commands.Length > 1)
                                {
                                    helper.GetQueue(commands[1]);
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("Queue path wasnt specified");
                                }
                                break;
                            case "deleteQueue":
                                if (commands.Length > 1)
                                {
                                    helper.DeleteQueue(commands[1]);
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("Queue path wasnt specified");
                                }
                                break;
                            case "createTopic":
                            case "ct":
                                if (commands.Length > 1)
                                {
                                    helper.CreateTopic(commands[1]);
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("Topic path wasnt specified");
                                }
                                break;
                            case "sub":
                                if (commands.Length > 2)
                                {
                                    helper.CreateSubscription(commands[1], commands[2]);
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("Topic path or sub path wasnt specified");
                                }
                                break;
                            case "exit":
                                done = true;
                                break;
                            case "all":
                                helper.ListTopicsAndSubscriptions();
                                break;
                        }
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            } while (!done);
        }
    }
}
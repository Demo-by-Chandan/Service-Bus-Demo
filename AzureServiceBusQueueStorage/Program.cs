using System;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;

namespace AzureServiceBusQueueStorage
{
    internal class Program
    {
        private const string connectionString = "<connectionstring>";
        private const string queueName = "<queue name>";
        private const int maxMessage = 10;

        private static void Main(string[] args)
        {
            var res = "y";
            do
            {
                QueueClient client = new QueueClient(connectionString, queueName);
                client.CreateIfNotExists();
                for (int i = 0; i < maxMessage; i++)
                {
                    client.SendMessage($"Message from Batch2 {DateTime.Now}");
                }

                QueueProperties prop = client.GetProperties();
                int? cahcedMessage = prop.ApproximateMessagesCount;

                Console.WriteLine("Reading message from queue without removing them");

                PeekedMessage[] peeked = client.PeekMessages((int)cahcedMessage);

                Console.ReadLine();
                foreach (PeekedMessage item in peeked)
                {
                    Console.WriteLine($"Message read from the queue {System.Text.Encoding.ASCII.GetString(item.Body)}");

                    prop = client.GetProperties();
                    int? queueLength = prop.ApproximateMessagesCount;
                    Console.WriteLine($"Current Length of the queue is {queueLength}");
                }

                Console.ReadLine();

                Console.WriteLine("Reading message fromt he queue");
                QueueMessage[] messages = client.ReceiveMessages((int)cahcedMessage);
                foreach (QueueMessage item in messages)
                {
                    Console.WriteLine($"Message read from the queue {System.Text.Encoding.ASCII.GetString(item.Body)}");
                    client.DeleteMessage(item.MessageId, item.PopReceipt);

                    prop = client.GetProperties();
                    int? queueLength = prop.ApproximateMessagesCount;
                    Console.WriteLine($"Current Length of the queue is {queueLength}");
                }

                Console.WriteLine("Want to try more? (y/n)");
                res = Console.ReadLine();
            } while (res.ToUpper() == "Y");
            Console.ReadLine();
        }
    }
}
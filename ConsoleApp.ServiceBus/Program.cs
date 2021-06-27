using Microsoft.Azure.ServiceBus;
using System;
using System.Text;

namespace ConsoleApp.ServiceBus
{
    internal class Program
    {
        private const string ServiceBusConnectionString = "<connection string>";
        private const string TopicName = "<topic name>";
        private const int numberOfMessagesToSend = 100;

        private static ITopicClient topicClient;

        private static void Main(string[] args)
        {
            topicClient = new TopicClient(ServiceBusConnectionString, TopicName);

            Console.WriteLine("Press ENTER key to exit after sending all the messages.");

            // Send messages.
            try
            {
                for (var i = 0; i < numberOfMessagesToSend; i++)
                {
                    // Create a new message to send to the topic.
                    string messageBody = $"Message {i} {DateTime.Now}";
                    var message = new Message(Encoding.UTF8.GetBytes(messageBody));

                    // Write the body of the message to the console.
                    Console.WriteLine($"Sending message: {messageBody}");

                    // Send the message to the topic.
                    topicClient.SendAsync(message);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
            Console.ReadKey();

            topicClient.CloseAsync();
            Console.ReadLine();
        }
    }
}
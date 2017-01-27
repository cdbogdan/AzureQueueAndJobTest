using System;
using Domain;
using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Queue; // Namespace for Queue storage types
using Newtonsoft.Json;

// useful link: https://docs.microsoft.com/en-us/azure/storage/storage-dotnet-how-to-use-queues

namespace AzureQueueAndTable
{
    class Program
    {

        // write a message to an azure queue

        static void Main(string[] args)
        {
            // get the storage account
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // get the queue
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference("firstqueue");

            // Create the queue if it doesn't already exist
            queue.CreateIfNotExists();

            // Create a message and add it to the queue.
            while (true)
            {
                var jsonMessage = JsonConvert.SerializeObject(new Item()
                {
                    Name = "bogdan",
                    Age = 1,
                });

                var message = new CloudQueueMessage(jsonMessage);
                queue.AddMessage(message);

                Console.WriteLine("Message sent to queue");
                Console.ReadKey();
            }
        }
    }
}

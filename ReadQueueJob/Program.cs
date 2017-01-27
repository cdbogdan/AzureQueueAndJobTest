using System;
using Microsoft.Azure;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

// web job care proceseaza mesajele dintr-o coada si le pune intr-un storage account table

namespace ReadQueueJob
{
    // To learn more about Microsoft Azure WebJobs SDK, please see https://go.microsoft.com/fwlink/?LinkID=320976
    // https://docs.microsoft.com/en-us/azure/app-service-web/websites-dotnet-webjobs-sdk-storage-queues-how-to
    class Program
    {
        static void Main()
        {
            // get the storage account
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            // Retrieve a reference to the table.
            CloudTable table = tableClient.GetTableReference("firstTable");

            // Create the table if it doesn't exist.
            table.CreateIfNotExists();
            Functions.FirstTable = table;


            // The following code ensures that the WebJob will be running continuously
            JobHostConfiguration config = new JobHostConfiguration();
            config.Queues.BatchSize = 8; // cate mesaje sa proceseze in paralel; The maximum number of queue messages that are picked up simultaneously to be executed in parallel (default is 16).
            config.Queues.MaxDequeueCount = 4; // de cate ori sa reincerce pana sa il trimita in poison; The maximum number of retries before a queue message is sent to a poison queue (default is 5).
            config.Queues.MaxPollingInterval = TimeSpan.FromSeconds(30); // la cat timp intreaba coada; default 1 minute; The maximum wait time before polling again when a queue is empty (default is 1 minute).

            var host = new JobHost();
            host.RunAndBlock();
        }
    }
}

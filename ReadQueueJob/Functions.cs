using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using Domain;

namespace ReadQueueJob
{
    public class Functions
    {
        public static CloudTable FirstTable;

        // aceasta functie stie sa faca mesajul in coada invizibil pentru 30 de secunde si cand se termina ii da delete
        // in caz ca apare ceva erroare aici, atunci mesajul se va face vizibil din nou in coada - false, de fapt mesajul se muta in firstqueue-poison - te speli pe cap cu el dupa aceea

        // This function will get triggered/executed when a new message is written on an Azure Queue called firstqueue.
        public static void ProcessQueueMessage([QueueTrigger("firstqueue")] string message, TextWriter log)
        {
            //throw new NullReferenceException(); // daca pusca aici, atunci mesajul citit va fi inserat in firstqueue-poison dupa ce se reincearca de 5 ori by default

            log.WriteLine(message);

            // parse the new message and prepare it for table insertion
            var item = JsonConvert.DeserializeObject<Item>(message);
            item.PartitionKey = "part1";
            item.RowKey = Guid.NewGuid().ToString();
            
            
            // Create the TableOperation object that inserts the customer entity.
            TableOperation insertOperation = TableOperation.Insert(item);
            // Execute the insert operation.
            FirstTable.Execute(insertOperation);
        }
    }
}

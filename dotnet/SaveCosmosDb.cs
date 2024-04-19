using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace dotnet
{
    public static class SaveCosmosDb
    {
        [FunctionName("SaveCosmosDb")]
        public static async Task RunAsync([QueueTrigger("datas", Connection = "AzureWebJobsStorage")] string item, 
            [CosmosDB(
                databaseName: "Employee", 
                containerName: "datas", 
                CreateIfNotExists = true, 
                Id = "/id", 
                PartitionKey = "/company", 
                Connection = "CosmosDbConnectionString")]
            IAsyncCollector<Item> collector,
            ILogger log)
        {
            log.LogInformation("Started importing");
            
            await collector.AddAsync(JsonSerializer.Deserialize<Item>(item));
            
            log.LogInformation("Finished working");

        }
    }
}
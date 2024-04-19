using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace dotnet
{
    public class ImportEmployee
    {
        [FunctionName("ImportEmployee")]
        public async Task Run(
            [BlobTrigger("data/{name}", Connection = "AzureWebJobsStorage")]Stream myBlob, string name, 
            [Queue("datas", Connection = "AzureWebJobsStorage")] IAsyncCollector<Item> queues,
            ILogger log)
        {
            log.LogInformation("Start running importing file: {name}", name);
            
            var isHeader = true;
            using var sr = new StreamReader(myBlob); //Import stream inside to the memory to read it as a string
            string line;
            while ((line = await sr.ReadLineAsync()) != null) //loop reading line by line and convert to json
            {
                if (isHeader)
                {
                    isHeader = false;
                    continue;
                }
                
                string[] values = line.Split(','); //split line by ',' delimeter
                var item = new Item //map to class to easier add to Queue
                {
                    id = values[0],
                    firstName = values[1],
                    lastName = values[2],
                    email = values[3],
                    gender = values[4],
                    jobTitle = values[5],
                    company = values[6],
                    departament = values[7]
                };
                
                await queues.AddAsync(item);
            }
            log.LogInformation("imported file: {name}", name);
        }
        
    }
}

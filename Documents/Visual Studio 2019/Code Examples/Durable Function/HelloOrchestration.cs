using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Company.Function
{
    public static class HelloOrchestration
    {
        [FunctionName("HelloOrchestration")]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var outputs = new List<string>();

            // Replace "hello" with the name of your Durable Activity Function.
            outputs.Add(await context.CallActivityAsync<string>("HelloOrchestration_Hello", "Joburg"));
            outputs.Add(await context.CallActivityAsync<string>("HelloOrchestration_Avuxeni", "Limpopo"));
            outputs.Add(await context.CallActivityAsync<string>("HelloOrchestration_Dumelang", "North West"));

            // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
            return outputs;
        }

        [FunctionName("HelloOrchestration_Hello")]
        public static string SayHello([ActivityTrigger] string name, ILogger log)
        {
            log.LogInformation($"Saying hello to {name}.");
            return $"Hello {name}!";
        }

        [FunctionName("HelloOrchestration_Avuxeni")]
        public static string SayAvuxeni([ActivityTrigger] string name, ILogger log)
        {
            log.LogInformation($"Avuxeni {name}.");
            return $"Avuxeni {name}!";
        }


        [FunctionName("HelloOrchestration_Dumelang")]
        public static string SayDumelang([ActivityTrigger] string name, ILogger log)
        {
            log.LogInformation($"Dumela {name}.");
            return $"Demela {name}!";
        }
        

        [FunctionName("HelloOrchestration_HttpStart")]
        public static async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("HelloOrchestration", null);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}
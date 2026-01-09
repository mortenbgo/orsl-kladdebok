using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Functions.Worker.Http;
namespace function;

public class EnkelTriggerForDeploy
{
    private readonly ILogger<EnkelTriggerForDeploy> _logger;

    public EnkelTriggerForDeploy(ILogger<EnkelTriggerForDeploy> logger)
    {
        _logger = logger;
    }

    [Function("EnkelTriggerForDeploy")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
        await response.WriteStringAsync("Welcome to Azure Functions!");
        return response;
    }
}

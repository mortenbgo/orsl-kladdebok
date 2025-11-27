using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace function;

public class EnkelTriggerForDeploy
{
    private readonly ILogger<EnkelTriggerForDeploy> _logger;

    public EnkelTriggerForDeploy(ILogger<EnkelTriggerForDeploy> logger)
    {
        _logger = logger;
    }

    [Function("EnkelTriggerForDeploy")]
    public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("Welcome to Azure Functions!");
    }
}

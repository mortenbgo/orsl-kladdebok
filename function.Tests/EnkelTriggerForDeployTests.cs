using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Moq;

namespace function.Tests;

public class EnkelTriggerForDeployTests
{
    [Fact]
    public async Task Run_ReturnsOkWithWelcomeMessage()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<EnkelTriggerForDeploy>>();
        var function = new EnkelTriggerForDeploy(loggerMock.Object);

        var context = new Mock<FunctionContext>();
        var request = new Mock<HttpRequestData>(context.Object);
        
        var memoryStream = new MemoryStream();
        var response = new Mock<HttpResponseData>(context.Object);
        response.SetupProperty(r => r.StatusCode);
        response.SetupProperty(r => r.Body, memoryStream);
        response.Setup(r => r.Headers).Returns(new HttpHeadersCollection());

        request.Setup(r => r.CreateResponse()).Returns(response.Object);

        // Act
        var result = await function.Run(request.Object);

        // Assert
        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        
        memoryStream.Position = 0;
        using var reader = new StreamReader(memoryStream);
        var body = await reader.ReadToEndAsync();
        Assert.Equal("Welcome to Azure Functions!", body);
    }

    [Fact]
    public async Task Run_LogsInformationMessage()
    {
        // Arrange
        var loggerMock = new Mock<ILogger<EnkelTriggerForDeploy>>();
        var function = new EnkelTriggerForDeploy(loggerMock.Object);

        var context = new Mock<FunctionContext>();
        var request = new Mock<HttpRequestData>(context.Object);
        
        var memoryStream = new MemoryStream();
        var response = new Mock<HttpResponseData>(context.Object);
        response.SetupProperty(r => r.StatusCode);
        response.SetupProperty(r => r.Body, memoryStream);
        response.Setup(r => r.Headers).Returns(new HttpHeadersCollection());

        request.Setup(r => r.CreateResponse()).Returns(response.Object);

        // Act
        await function.Run(request.Object);

        // Assert
        loggerMock.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("HTTP trigger function processed a request")),
                It.IsAny<Exception?>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}

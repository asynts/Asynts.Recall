using Moq;
using Microsoft.Extensions.Logging;

namespace Asynts.Recall.Backend.Services.Test;

public class Tests
{
    private PageParserService? pageParserService;

    [SetUp]
    public void Setup()
    {
        var mockLogger = new Mock<ILogger<PageParserService>>();

        pageParserService = new PageParserService(mockLogger.Object);
    }
}

namespace Asynts.Recall.Backend.Services.Test;

public class Tests
{
    private PageParserService? pageParserService;

    [SetUp]
    public void Setup()
    {
        pageParserService = new PageParserService();
    }

    [Test]
    public void TestParse()
    {
        var pageData = pageParserService!.Parse("""
--- Metadata
{
    "id": "37cf9645-ce5d-43cd-b5fc-3cd386860d32",
    "title": "This is an example page.",
    "tags": [ "example/foo/", "hello/" ]
}
--- Comment
This is not indexed by the system and can be used to comment on the file itself.
--- Summary
This is the summary that is shown in the search results.
--- Details
This can be much longer.
This is on another line.
""");

        Assert.That(pageData.Uuid, Is.EqualTo("37cf9645-ce5d-43cd-b5fc-3cd386860d32"));
        Assert.That(pageData.Title, Is.EqualTo("This is an example page."));
        Assert.That(pageData.Tags, Is.EquivalentTo(new[] { "example/foo/", "hello/" }));
        Assert.That(pageData.Summary, Is.EqualTo("This is the summary that is shown in the search results.\n"));
        Assert.That(pageData.Details, Is.EqualTo("This can be much longer.\nThis is on another line.\n"));
    }
}

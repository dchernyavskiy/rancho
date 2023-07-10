using FluentAssertions;
using Microsoft.Extensions.Logging;
using Rancho.Services.Feeding.Shared.Data;
using Rancho.Services.Feeding.TestShared.Fakes.Feeds.Features;
using Tests.Shared.Fixtures;
using Tests.Shared.XunitCategories;
using Xunit.Abstractions;

namespace Rancho.Services.Feeding.IntegrationTests.Feeds.Features.RegisteringFeedTests.v1;

public class RegisterFeedTests : FeedingServiceIntegrationTestBase
{
    private readonly ILogger<RegisterFeedTests> _logger;

    public RegisterFeedTests(
        SharedFixtureWithEfCore<Feeding.Api.Program, FeedingDbContext> sharedFixture,
        ITestOutputHelper outputHelper
    ) : base(sharedFixture, outputHelper)
    {
        _logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<RegisterFeedTests>();
    }

    [Fact]
    [CategoryTrait(TestCategory.Integration)]
    public async Task ShouldCreateRecordInDb()
    {
        var registerFeed = new FakeRegisterFeed().Generate();

        var registerFeedResponse = await SharedFixture.SendAsync(registerFeed);

        registerFeedResponse.Should().NotBeNull();
        registerFeedResponse.Feed.Should().NotBeNull();
        registerFeedResponse.Feed.Id.Should().NotBe(Guid.Empty);
    }
}

using MessagingSample.UnitTests;

namespace MessagingSample.Tests;

[CollectionDefinition(SharedCollectionDefinition.Name)]
public sealed class SharedCollectionDefinition : ICollectionFixture<DatabaseContainerFixture>
{
    public const string Name = "Shared";
}
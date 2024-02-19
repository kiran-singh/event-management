using AutoFixture;
using AutoFixture.Xunit2;
using EventManagement.Domain;
using EventManagement.Repositories;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Moq;

namespace Repositories.Tests;

public class BaseRepositoryTests : IClassFixture<MongoTestFixture>
{
    private static readonly Random Random = new();
    private readonly IMongoCollection<Event> _eventsCollection;
    private readonly BaseRepository<Event> _repository;
    private readonly IList<Event> _events;

    public BaseRepositoryTests(MongoTestFixture testFixture)
    {
        var fixture = new Fixture();
        
        _eventsCollection = testFixture.Events;
        _eventsCollection.DeleteMany(FilterDefinition<Event>.Empty);
        
        _events = fixture.CreateMany<Event>(Random.Next(10, 20)).ToList();
        _eventsCollection.InsertMany(_events);

        _repository = new BaseRepository<Event>(new MongoContext(testFixture.Database),
            new Mock<ILogger<BaseRepository<Event>>>().Object);
    }

    [AutoData]
    [Theory]
    public async Task AddOrUpdateAsync_NewEntity_AddedToCollection(Event evt)
    {
        // Arrange
        (await _eventsCollection.Find(x => x.Id == evt.Id)
            .FirstOrDefaultAsync()).Should().BeNull();

        // Act
        var result = await _repository.AddOrUpdateAsync(evt);

        // Assert
        result.Should().BeTrue();
        var inserted = await _eventsCollection.Find(x => x.Id == evt.Id).FirstAsync();
        inserted.Should().BeEquivalentTo(evt, opt => opt.DateTimeClose());
    }

    [AutoData]
    [Theory]
    public async Task AddOrUpdateAsync_ExistingEntity_UpdatedInCollection(Guid categoryId, string name)
    {
        // Arrange
        var evt = _events[Random.Next(0, 10)];
        evt.CategoryId = categoryId;
        evt.Name = name;

        // Act
        var result = await _repository.AddOrUpdateAsync(evt);

        // Assert
        result.Should().BeTrue();
        var updated = await _eventsCollection.Find(x => x.Id == evt.Id).FirstAsync();
        updated.CategoryId.Should().Be(categoryId);
        updated.Name.Should().Be(name);
    }

    [Fact]
    public async Task DeleteAsync_ExistingEntity_DeletedFromCollection()
    {
        // Arrange
        var evt = _events[Random.Next(0, 10)];

        // Act
        await _repository.DeleteAsync(evt.Id);

        // Assert
        (await _eventsCollection.Find(x => x.Id == evt.Id)
            .FirstOrDefaultAsync()).Should().BeNull();
    }

    [Fact]
    public async Task GetByIdAsync_ExistingEntity_ReturnedFromCollection()
    {
        // Arrange
        var expected = _events[Random.Next(0, 10)];

        // Act
        var actual = await _repository.GetByIdAsync(expected.Id);

        // Assert
        actual.Should().BeEquivalentTo(expected, opt => opt.DateTimeClose());
    }
    
    [Fact]
    public async Task GetByIdAsync_NonExistentId_ReturnsNull()
    {
        // Arrange // Act
        var actual = await _repository.GetByIdAsync(Guid.NewGuid());

        // Assert
        actual.Should().BeNull();
    }

    [Fact]
    public async Task ListAllAsync_Returns_AllEntitiesInCollection()
    {
        // Arrange // Act
        var actual = await _repository.ListAllAsync();

        // Assert
        actual.Should().BeEquivalentTo(_events, opt => opt.DateTimeClose());
    }
}
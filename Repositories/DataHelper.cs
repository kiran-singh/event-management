using EventManagement.Domain;

namespace EventManagement.Repositories;

public static class DataHelper
{
    public static void AddSeedData(IMongoContext mongoContext)
    {
        var categoryCollection = mongoContext.ReturnCollectionIfEmpty<Category>();
        categoryCollection?.InsertMany(new[] { Concerts, Conferences, Musicals, Plays, });

        var eventCollection = mongoContext.ReturnCollectionIfEmpty<Event>();
        eventCollection?.InsertMany(Events);
    }
    
    private static readonly Category Concerts = new() { Id = Guid.Parse("{B0788D2F-8003-43C1-92A4-EDC76A7C5DDE}"), Name = "Concerts" };
    private static readonly Category Conferences = new() { Id = Guid.Parse("{6313179F-7837-473A-A4D5-A5571B43E6A6}"), Name = "Conferences" };
    private static readonly Category Musicals = new() { Id = Guid.Parse("{BF3F3002-7E53-441E-8B76-F6280BE284AA}"), Name = "Musicals" };
    private static readonly Category Plays = new() { Id = Guid.Parse("{FE98F549-E790-4E9F-AA16-18C2292A2EE9}"), Name = "Plays" };

    private static readonly List<Event> Events = new()
    {
        new()
        {
            Id = Guid.NewGuid(),
            Artist = "John Egbert",
            CategoryId = Concerts.Id,
            Created = DateTime.Today,
            Date = new DateTime(2024, 6, 11, 20, 59, 16, 613, DateTimeKind.Local).AddTicks(8011),
            Description =
                "Join John for his farwell tour across 15 continents. John really needs no introduction since he has already mesmerized the world with his banjo.",
            ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/banjo.jpg",
            Name = "John Egbert Live",
            Price = 65
        },
        new()
        {
            Id = Guid.NewGuid(),
            Artist = "Michael Johnson",
            CategoryId = Concerts.Id,
            Created = DateTime.Today,
            Date = new DateTime(2024, 9, 11, 20, 59, 16, 613, DateTimeKind.Local).AddTicks(8041),
            Description =
                "Michael Johnson doesn't need an introduction. His 25 concert across the globe last year were seen by thousands. Can we add you to the list?",
            ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/michael.jpg",
            Name = "The State of Affairs: Michael Live!",
            Price = 85
        },
        new()
        {
            Id = Guid.NewGuid(),
            Artist = "DJ 'The Mike'",
            CategoryId = Concerts.Id,
            Created = DateTime.Today,
            Date = new DateTime(2024, 4, 11, 20, 59, 16, 613, DateTimeKind.Local).AddTicks(8048),
            Description = "DJs from all over the world will compete in this epic battle for eternal fame.",
            ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/dj.jpg",
            Name = "Clash of the DJs",
            Price = 85
        },
        new()
        {
            Id = Guid.NewGuid(),
            Artist = "Manuel Santinonisi",
            CategoryId = Concerts.Id,
            Created = DateTime.Today,
            Date = new DateTime(2024, 4, 11, 20, 59, 16, 613, DateTimeKind.Local).AddTicks(8053),
            Description = "Get on the hype of Spanish Guitar concerts with Manuel.",
            ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/guitar.jpg",
            Name = "Spanish guitar hits with Manuel",
            Price = 25
        },
        new()
        {
            Id = Guid.NewGuid(),
            Artist = "Many",
            CategoryId = Plays.Id,
            Created = DateTime.Today,
            Date = new DateTime(2024, 10, 11, 20, 59, 16, 613, DateTimeKind.Local).AddTicks(8058),
            Description = "The best tech conference in the world",
            ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/conf.jpg",
            Name = "Techorama 2021",
            Price = 400
        },
        new()
        {
            Id = Guid.NewGuid(),
            Artist = "Nick Sailor",
            CategoryId = Conferences.Id,
            Created = DateTime.Today,
            Date = new DateTime(2024, 8, 11, 20, 59, 16, 613, DateTimeKind.Local).AddTicks(8064),
            Description =
                "The critics are over the moon and so will you after you've watched this sing and dance extravaganza written by Nick Sailor, the man from 'My dad and sister'.",
            ImageUrl = "https://gillcleerenpluralsight.blob.core.windows.net/files/GloboTicket/musical.jpg",
            Name = "To the Moon and Back",
            Price = 135
        }
    };
}
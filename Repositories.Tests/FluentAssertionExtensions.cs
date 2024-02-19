using FluentAssertions;
using FluentAssertions.Equivalency;

namespace Repositories.Tests;

public static class FluentAssertionExtensions
{
    public static EquivalencyAssertionOptions<T> DateTimeClose<T>(this EquivalencyAssertionOptions<T> opt)
        => opt.Using<DateTime>(ctx =>
                ctx.Subject.Should().BeCloseTo(ctx.Expectation, TimeSpan.FromSeconds(1)))
            .WhenTypeIs<DateTime>();
}
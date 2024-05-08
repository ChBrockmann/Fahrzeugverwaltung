using AutoFixture;
using MapsterMapper;
using Model.Mapping;

namespace MappingTests;

public class TestBase
{
    protected readonly Fixture Fixture;
    protected readonly IMapper Mapper;
    
    public TestBase()
    {
        Mapper = new Mapper(MappingConfiguration.Get());
        Fixture = new Fixture();

        Random random = new();
        Fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(x => Fixture.Behaviors.Remove(x));
        Fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        Fixture.Register(() => new DateOnly(random.Next(1990, DateTime.Now.Year), random.Next(1, 12), random.Next(1, 28)));
    }
}
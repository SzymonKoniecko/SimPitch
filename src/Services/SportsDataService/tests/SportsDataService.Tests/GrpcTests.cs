using Grpc.Core.Testing;
using SportsDataService.Domain.Entities;
using SportsDataService.Tests.Consts;
namespace SportsDataService.Tests;

public class GrpcTests : IClassFixture<GrpcTestFixture>
{
    private readonly GrpcTestFixture _fixture;

    public GrpcTests(GrpcTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void Test1()
    {
        Assert.Fail();
    }
    public class CountryGrpcTests : GrpcTests
    {
        private Country country;
        public CountryGrpcTests(GrpcTestFixture fixture) : base(fixture)
        {

        }

        [Fact]
        public void Test2()
        {
            _fixture.InsertTestCountryAsync(country);
        }
    }
}

using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;

namespace HogwartsAPI.IntegrationTests
{
    public class WandControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private HttpClient _client;
        public WandControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }
        [Theory]
        [InlineData("pageSize=6&pageNumber=1")]
        [InlineData("pageSize=1&pageNumber=2")]
        [InlineData("pageSize=20&pageNumber=1")]
        [InlineData("pageSize=20&pageNumber=2")]
        public async Task GetAll_WithQueryParameters_ReturnsOk(string queryParams)
        {
            //act

            var response = await _client.GetAsync("/api/wand?" + queryParams);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

        }
        [Theory]
        [InlineData("pageSize=a&pageNumber=1")]
        [InlineData("pageSize=a&pageNumber=a")]
        public async Task GetAll_WithInvalidQueryParameters_ReturnsBadRequest(string queryParams)
        {
            //act

            var response = await _client.GetAsync("/api/wand?" + queryParams);

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);

        }
    }
}

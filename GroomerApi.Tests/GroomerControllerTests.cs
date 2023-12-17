using FluentAssertions;
using GroomerApi.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace GroomerApi.Tests
{
    public class GroomerControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private HttpClient _client;
        public GroomerControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services
                            .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<GroomerDbContext>));

                        services.Remove(dbContextOptions);

                        services
                            .AddEntityFrameworkSqlServer()
                            .AddDbContext<GroomerDbContext>((sp,options) => 
                            options
                            .UseInMemoryDatabase("GroomerDb")
                            .UseInternalServiceProvider(sp)
                            );

                    });
                })
                .CreateClient();
        }
        [Theory]
        [InlineData("searchPhrase=k&pageSize=5&pageNumber=1&sortBy=FirstName")]
        [InlineData("searchPhrase=k&pageSize=10&pageNumber=3&sortBy=LastName")]
        [InlineData("searchPhrase=k&pageSize=15&pageNumber=3&sortBy=FirstName")]
        public async Task GetAll_WithQueryParameters_ReturnsOkResult(string queryParams)
        {

            //act

            var response = await _client.GetAsync("/api/user?" + queryParams);

            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);


        }
        [Theory]
        [InlineData("searchPhrase=k&pageSize=1&pageNumber=1&sortBy=FirstName")]
        [InlineData("searchPhrase=k&pageSize=11&pageNumber=3&sortBy=LastName")]
        [InlineData("searchPhrase=k&pageSize=15&sortBy=FirstName")]
        [InlineData("searchPhrase=&pageSize=5&pageNumber=1&sortBy=null")]
        [InlineData("pageSize=10&pageNumber=3&sortBy=LastName")]
        [InlineData("searchPhrase=k&pageSize=5&pageNumber=3")]
        [InlineData(null)]
        [InlineData("")]
        public async Task GetAll_WithInvalidQueryParameters_ReturnsBadRequest(string queryParams)
        {

            //act

            var response = await _client.GetAsync("/api/user?" + queryParams);

            //assert

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
    }
}

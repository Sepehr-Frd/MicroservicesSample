using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using RedditMockup.Common.Dtos;
using RestSharp;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace RedditMockup.IntegrationTest;

public class AccountControllerTest : IClassFixture<WebApplicationFactory<Program>>
{

    // [Field(s)]

    private const string BaseAddress = "/api/Account";

    private const int DefaultTimeout = 2000;

    private readonly HttpClient _client;

    

    // [Constructor]

    public AccountControllerTest(WebApplicationFactory<Program> factory) =>
        _client = factory.WithWebHostBuilder(builder =>
                builder.UseEnvironment("Testing"))
            .CreateClient();

    

    // [Theory Method(s)]

    [Theory]
    [MemberData(nameof(GenerateLoginData))]
    public async Task Login_ReturnExpectedResult(LoginDto loginDto, bool expected)
    {
        // [Arrange]

        var client = new RestClient(_client);

        var request = new RestRequest($"{BaseAddress}/Login")
        {
            Timeout = DefaultTimeout
        };

        request.AddJsonBody(loginDto);

        

        // [Act]

        var response = await client.ExecutePostAsync<CustomResponse>(request);

        

        // [Assert]

        response.Data?.IsSuccess.Should().Be(expected);

        
    }

    [Theory]
    [MemberData(nameof(GenerateIntegrationData))]
    public async Task LoginGetAllLogout_ReturnExpectedResult(LoginDto loginDto, TestResultCode expected)
    {
        // [Arrange]

        var client = new RestClient(_client);

        

        // [Act]

        // [Login]

        var loginRequest = new RestRequest($"{BaseAddress}/Login")
        {
            Timeout = DefaultTimeout
        };

        loginRequest.AddJsonBody(loginDto);

        var loginResponse = await client.ExecutePostAsync<CustomResponse>(loginRequest);

        

        // [GetAll]

        var getAllRequest = new RestRequest("/api/User")
        {
            Timeout = DefaultTimeout
        };

        var getAllResponse = await client.ExecuteGetAsync<CustomResponse>(getAllRequest);

        

        // [Logout]

        var logoutRequest = new RestRequest($"{BaseAddress}/Logout")
        {
            Timeout = DefaultTimeout
        };

        var logoutResponse = await client.ExecutePostAsync<CustomResponse>(logoutRequest);

        

        

        // [Assert]

        switch (expected)
        {
            case TestResultCode.AllFailed:

                loginResponse.Data?.IsSuccess.Should().BeFalse();

                getAllResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

                logoutResponse.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

                break;

            case TestResultCode.AllSuccessful:

                loginResponse.Data?.IsSuccess.Should().BeTrue();

                getAllResponse.StatusCode.Should().Be(HttpStatusCode.OK);

                logoutResponse.StatusCode.Should().Be(HttpStatusCode.OK);

                break;

            case TestResultCode.Unauthorized:

                loginResponse.Data?.IsSuccess.Should().BeTrue();

                getAllResponse.StatusCode.Should().Be(HttpStatusCode.Forbidden);

                logoutResponse.StatusCode.Should().Be(HttpStatusCode.OK);

                break;

            default:
                Assert.True(false);
                break;
        }

        
    }

    

    // [Data Method(s)]

    public static IEnumerable<object[]> GenerateLoginData()
    {
        return new List<object[]>
        {
            new object[]
            {
                new LoginDto
                {
                    Username = "sepehr_frd",
                    Password = "sfr1376",
                    RememberMe = true
                },
                true
            },

            new object[]
            {
                new LoginDto
                {
                    Username = "sepehr_frd",
                    Password = "asdasdasdasd",
                    RememberMe = false
                },
                false
            },

            new object[]
            {
                new LoginDto
                {
                    Username = "sepehr_d",
                    Password = "sfr1376",
                    RememberMe = false
                },
                false
            },

            new object[]
            {
                new LoginDto
                {
                    Username = "223",
                    Password = "sd2",
                    RememberMe = false
                },
                false
            }
        };
    }

    public static IEnumerable<object[]> GenerateIntegrationData()
    {
        return new List<object[]>
        {
            new object[]
            {
                new LoginDto
                {
                    Username = "abbas_booazaar",
                    Password = "abbasabbas",
                    RememberMe = true
                },
                TestResultCode.Unauthorized
            },

            new object[]
            {
                new LoginDto
                {
                    Username = "sepehr_frd",
                    Password = "sfr1376",
                    RememberMe = true
                },
                TestResultCode.AllSuccessful
            },

            new object[]
            {
                new LoginDto
                {
                    Username = "sepehr_frd",
                    Password = "sfr1231123376",
                    RememberMe = true
                },
                TestResultCode.AllFailed
            },

            new object[]
            {
                new LoginDto
                {
                    Username = "sepeasdfahr_frd",
                    Password = "sfr1376",
                    RememberMe = true
                },
                TestResultCode.AllFailed
            }
        };
    }

    

    public enum TestResultCode
    {
        AllFailed,
        AllSuccessful,
        Unauthorized
    }
}
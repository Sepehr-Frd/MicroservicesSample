using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using RedditMockup.Common.Dtos;
using System;
using Xunit;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace RedditMockup.IntegrationTest;

public class AnswerControllerTest : IClassFixture<WebApplicationFactory<Program>>
{
    // |------------------------------------|Fields|-------------------------------------------->

    private const string BaseAddress = "/api/Answer";

    private const string LoginAddress = "/api/Account/Login";

    private const string ValidTitle = "How to do sth";

    private const string ValidDescription = "Can anybody help me with my problem?";

    private readonly HttpClient _client;

    // <--------------------------------------------------------------------------------|

    public enum TestResultCode
    {
        Ok,
        NotFound,
        Unauthorized
    }

    // |------------------------------------|Constructor|-------------------------------------------->

    public AnswerControllerTest(WebApplicationFactory<Program> factory)
    {
        using var customFactory = factory.WithWebHostBuilder(builder => builder.UseEnvironment("Testing"));

        _client = customFactory.CreateClient();
    }

    // <--------------------------------------------------------------------------------|

    // |------------------------------------|Methods|-------------------------------------------->

    private async Task AuthenticateAsync()
    {
        var loginDto = new LoginDto
        {
            Username = "sepehr_frd",
            Password = "sfr1376",
            RememberMe = true
        };

        var serializedLoginDto = JsonSerializer.Serialize(loginDto);

        var stringContent = new StringContent(serializedLoginDto, Encoding.UTF8, "application/json");

        await _client.PostAsync(LoginAddress, stringContent);
    }

    // <--------------------------------------------------------------------------------|


    // |------------------------------------|Theory Methods|-------------------------------------------->
    [Theory]
    [MemberData(nameof(GenerateCreateData))]
    public async Task Create_ReturnExpectedResult(AnswerDto dto, TestResultCode testResultCode)
    {
        // [Arrange]

        var serializedLoginDto = JsonSerializer.Serialize(dto);

        var stringContent = new StringContent(serializedLoginDto, Encoding.UTF8, "application/json");

        

        // [Act]

        if (testResultCode != TestResultCode.Unauthorized) await AuthenticateAsync();

        var response = await _client.PostAsync(BaseAddress, stringContent);

        if (testResultCode == TestResultCode.Unauthorized)
        {
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            return;
        }

        var streamResponse = await response.Content.ReadAsStreamAsync();

        var apiResponse = await JsonSerializer.DeserializeAsync<CustomResponse>(streamResponse);

        

        // [Assert]

        switch (testResultCode)
        {
            case TestResultCode.Ok:

                response.StatusCode.Should().Be(HttpStatusCode.OK);

                apiResponse?.IsSuccess.Should().BeTrue();

                break;

            case TestResultCode.NotFound:

                response.StatusCode.Should().Be(HttpStatusCode.OK);

                apiResponse?.IsSuccess.Should().BeFalse();

                break;

            default:

                Assert.Null("Error");

                break;
        }

        
    }

    [Fact]
    public async Task GetAll_ReturnCustomResponseOfListOfAnswerDto()
    {
        // [Act]

        var response = await _client.GetAsync(BaseAddress);

        var streamResponse = await response.Content.ReadAsStringAsync();

        var apiResponse = await Task.Factory.StartNew(() =>
            JsonConvert.DeserializeObject<CustomResponse<List<AnswerDto>>>(streamResponse));

        

        // [Assert]

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        apiResponse?.Data?.Should().BeOfType<List<AnswerDto>>();

        
    }

    [Theory]
    [MemberData(nameof(GenerateGetByIdData))]
    public async Task GetById_ReturnExpectedResult(int id, bool isAuthenticated, HttpStatusCode httpStatusCode)
    {
        if (isAuthenticated)
        {
            // [Arrange]

            await AuthenticateAsync();

            

            // [Act]

            var response = await _client.GetAsync(BaseAddress + "/id" + $"?id={id}");

            var streamResponse = await response.Content.ReadAsStreamAsync();

            var apiResponse = await JsonSerializer.DeserializeAsync<CustomResponse>(streamResponse);

            

            // [Assert]

            response.StatusCode.Should().Be(httpStatusCode);

            if (id > 150)
                apiResponse?.IsSuccess.Should().BeFalse();
            else
                apiResponse?.IsSuccess.Should().BeTrue();

            
        }
        else
        {
            // [Act]

            var response = await _client.GetAsync(BaseAddress + "/id" + $"?id={id}");

            

            // [Assert]

            response.StatusCode.Should().Be(httpStatusCode);

            
        }
    }

    [Theory]
    [MemberData(nameof(GenerateUpdateData))]
    public async Task Update_ReturnExpectedResult(int answerId, AnswerDto dto, TestResultCode testResultCode)
    {
        // [Arrange]

        var serializedLoginDto = JsonSerializer.Serialize(dto);

        var stringContent = new StringContent(serializedLoginDto, Encoding.UTF8, "application/json");

        if (testResultCode != TestResultCode.Unauthorized)
        {
            await AuthenticateAsync();
        }

        

        // [Act]

        var response = await _client.PutAsync($"{BaseAddress}?id={answerId}", stringContent);

        if (testResultCode == TestResultCode.Unauthorized)
        {
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            return;
        }

        var streamResponse = await response.Content.ReadAsStreamAsync();

        var apiResponse = await JsonSerializer.DeserializeAsync<CustomResponse>(streamResponse);

        

        // [Assert]

        switch (testResultCode)
        {
            case TestResultCode.Ok:

                response.StatusCode.Should().Be(HttpStatusCode.OK);

                apiResponse?.IsSuccess.Should().BeTrue();

                break;

            case TestResultCode.NotFound:

                response.StatusCode.Should().Be(HttpStatusCode.OK);

                apiResponse?.IsSuccess.Should().BeFalse();

                break;

            default:

                Assert.Null("Error");

                break;
        }

        

    }

    [Fact]
    public async Task GetVotes_ReturnCustomResponseOfListOfVoteDto()
    {
        // [Act]

        var response = await _client.GetAsync(BaseAddress + "/AnswerVotes");

        var streamResponse = await response.Content.ReadAsStringAsync();

        var apiResponse = await Task.Factory.StartNew(() =>
            JsonConvert.DeserializeObject<CustomResponse<List<VoteDto>>>(streamResponse));

        

        // [Assert]

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        apiResponse?.Data?.Should().BeOfType<List<VoteDto>>();

        
    }

    [Theory]
    [MemberData(nameof(GenerateSubmitVoteData))]
    public async Task SubmitVote_ReturnExpectedResult(int answerId, bool kind, TestResultCode testResultCode)
    {
        // [Arrange]

        if (testResultCode != TestResultCode.Unauthorized)
        {
            await AuthenticateAsync();
        }

        

        // [Act]

        var response = await _client.PostAsync($"{BaseAddress}/SubmitVote?answerId={answerId}&kind={kind}", null);

        if (testResultCode == TestResultCode.Unauthorized)
        {
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

            return;
        }

        var streamResponse = await response.Content.ReadAsStreamAsync();

        var apiResponse = await JsonSerializer.DeserializeAsync<CustomResponse>(streamResponse);

        

        // [Assert]

        switch (testResultCode)
        {
            case TestResultCode.Ok:

                response.StatusCode.Should().Be(HttpStatusCode.OK);

                apiResponse?.IsSuccess.Should().BeTrue();

                break;

            case TestResultCode.NotFound:

                response.StatusCode.Should().Be(HttpStatusCode.OK);

                apiResponse?.IsSuccess.Should().BeFalse();

                break;

            default:

                Assert.Null("Error");

                break;
        }

        


    }

    [Theory]
    [MemberData(nameof(GenerateDeleteData))]
    public async Task Delete_ReturnExpectedResult(int answerId, TestResultCode testResultCode)
    {

        // [Arrange]

        if (testResultCode != TestResultCode.Unauthorized)
        {
            await AuthenticateAsync();
        }

        

        // [Act]

        var response = await _client.DeleteAsync($"{BaseAddress}?id={answerId}");

        if (testResultCode == TestResultCode.Unauthorized)
        {
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            return;
        }

        var streamResponse = await response.Content.ReadAsStreamAsync();

        var apiResponse = await JsonSerializer.DeserializeAsync<CustomResponse>(streamResponse);

        

        // [Assert]

        switch (testResultCode)
        {
            case TestResultCode.Ok:

                response.StatusCode.Should().Be(HttpStatusCode.OK);

                apiResponse?.IsSuccess.Should().BeTrue();

                break;

            case TestResultCode.NotFound:

                response.StatusCode.Should().Be(HttpStatusCode.OK);

                apiResponse?.IsSuccess.Should().BeFalse();

                break;

            default:

                Assert.Null("Error");

                break;

        }

        
    }

    // <--------------------------------------------------------------------------------|
    

    // |------------------------------------|Data Methods|-------------------------------------------->
    public static IEnumerable<object[]> GenerateCreateData()
    {
        return new List<object[]>
        {
            new object[]
            {
                new AnswerDto
                {
                    QuestionGuid = Guid.NewGuid(),
                    Title = ValidTitle,
                    Description = ValidDescription
                },
                TestResultCode.Ok
            },

            new object[]
            {
                new AnswerDto
                {
                    QuestionGuid = Guid.NewGuid(),
                    Title = ValidTitle,
                    Description = ValidDescription
                },
                TestResultCode.NotFound
            },

            new object[]
            {
                new AnswerDto
                {
                    QuestionGuid = Guid.NewGuid(),
                    Title = ValidTitle,
                    Description = ValidDescription
                },
                TestResultCode.Unauthorized
            }
        };
    }

    public static IEnumerable<object[]> GenerateGetByIdData()
    {
        return new List<object[]>
        {
            new object[]
            {
                1,
                true,
                HttpStatusCode.OK
            },
            new object[]
            {
                20,
                true,
                HttpStatusCode.OK
            },
            new object[]
            {
                2,
                false,
                HttpStatusCode.Unauthorized
            },
            new object[]
            {
                20,
                false,
                HttpStatusCode.Unauthorized
            }
        };
    }

    public static IEnumerable<object[]> GenerateSubmitVoteData()
    {
        return new List<object[]>
        {
            new object[]
            {
                5,
                true,
                TestResultCode.Ok
            },

            new object[]
            {
                5,
                false,
                TestResultCode.Unauthorized
            },

            new object[]
            {
                200,
                true,
                TestResultCode.NotFound
            }
        };
    }

    public static IEnumerable<object[]> GenerateUpdateData()
    {



        return new List<object[]>
        {
            new object[]
            {
                5,
                new AnswerDto
                {
                    QuestionGuid = Guid.NewGuid(),
                    Title = ValidTitle,
                    Description = ValidDescription
                },
                TestResultCode.Ok
            },

            new object[]
            {
                5,
                new AnswerDto
                {
                    QuestionGuid = Guid.NewGuid(),
                    Title = ValidTitle,
                    Description = ValidDescription
                },
                TestResultCode.Ok
            },

            new object[]
            {
                5,
                new AnswerDto
                {
                    QuestionGuid = Guid.NewGuid(),
                    Title = ValidTitle,
                    Description = ValidDescription
                },
                TestResultCode.Unauthorized
            },

            new object[]
            {
                5,
                new AnswerDto
                {
                    QuestionGuid = Guid.NewGuid(),
                    Title = ValidTitle,
                    Description = ValidDescription
                },
                TestResultCode.NotFound
            },

            new object[]
            {
                450,
                new AnswerDto
                {
                    QuestionGuid = Guid.NewGuid(),
                    Title = ValidTitle,
                    Description = ValidDescription
                },
                TestResultCode.NotFound
            },

            new object[]
            {
                450,
                new AnswerDto
                {
                    QuestionGuid = Guid.NewGuid(),
                    Title = ValidTitle,
                    Description = ValidDescription
                },
                TestResultCode.NotFound
            }
        };
    }

    public static IEnumerable<object[]> GenerateDeleteData()
    {
        return new List<object[]>
        {
            new object[]
            {
                5,
                TestResultCode.Ok
            },

            new object[]
            {
                200,
                TestResultCode.NotFound
            },

            new object[]
            {
                5,
                TestResultCode.Unauthorized
            }
        };
    }
    
    // <--------------------------------------------------------------------------------|
}
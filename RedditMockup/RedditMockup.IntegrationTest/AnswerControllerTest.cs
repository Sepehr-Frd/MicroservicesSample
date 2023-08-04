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
    #region [Field(s)]

    private const string BaseAddress = "/api/Answer";

    private const string LoginAddress = "/api/Account/Login";

    private const string ValidTitle = "How to do sth";

    private const string ValidDescription = "Can anybody help me with my problem?";

    private readonly HttpClient _client;

    #endregion

    public enum TestResultCode
    {
        Ok,
        NotFound,
        Unauthorized
    }

    #region [Constructor]

    public AnswerControllerTest(WebApplicationFactory<Program> factory)
    {
        using var customFactory = factory.WithWebHostBuilder(builder => builder.UseEnvironment("Testing"));

        _client = customFactory.CreateClient();
    }

    #endregion

    #region [Method(s)]

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

    #endregion

    #region [Theory Method(s)]

    [Theory]
    [MemberData(nameof(GenerateCreateData))]
    public async Task Create_ReturnExpectedResult(AnswerDto dto, TestResultCode testResultCode)
    {
        #region [Arrange]

        var serializedLoginDto = JsonSerializer.Serialize(dto);

        var stringContent = new StringContent(serializedLoginDto, Encoding.UTF8, "application/json");

        #endregion

        #region [Act]

        if (testResultCode != TestResultCode.Unauthorized) await AuthenticateAsync();

        var response = await _client.PostAsync(BaseAddress, stringContent);

        if (testResultCode == TestResultCode.Unauthorized)
        {
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            return;
        }

        var streamResponse = await response.Content.ReadAsStreamAsync();

        var apiResponse = await JsonSerializer.DeserializeAsync<CustomResponse>(streamResponse);

        #endregion

        #region [Assert]

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

        #endregion
    }

    [Fact]
    public async Task GetAll_ReturnCustomResponseOfListOfAnswerDto()
    {
        #region [Act]

        var response = await _client.GetAsync(BaseAddress);

        var streamResponse = await response.Content.ReadAsStringAsync();

        var apiResponse = await Task.Factory.StartNew(() =>
            JsonConvert.DeserializeObject<CustomResponse<List<AnswerDto>>>(streamResponse));

        #endregion

        #region [Assert]

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        apiResponse?.Data?.Should().BeOfType<List<AnswerDto>>();

        #endregion
    }

    [Theory]
    [MemberData(nameof(GenerateGetByIdData))]
    public async Task GetById_ReturnExpectedResult(int id, bool isAuthenticated, HttpStatusCode httpStatusCode)
    {
        if (isAuthenticated)
        {
            #region [Arrange]

            await AuthenticateAsync();

            #endregion

            #region [Act]

            var response = await _client.GetAsync(BaseAddress + "/id" + $"?id={id}");

            var streamResponse = await response.Content.ReadAsStreamAsync();

            var apiResponse = await JsonSerializer.DeserializeAsync<CustomResponse>(streamResponse);

            #endregion

            #region [Assert]

            response.StatusCode.Should().Be(httpStatusCode);

            if (id > 150)
                apiResponse?.IsSuccess.Should().BeFalse();
            else
                apiResponse?.IsSuccess.Should().BeTrue();

            #endregion
        }
        else
        {
            #region [Act]

            var response = await _client.GetAsync(BaseAddress + "/id" + $"?id={id}");

            #endregion

            #region [Assert]

            response.StatusCode.Should().Be(httpStatusCode);

            #endregion
        }
    }

    [Theory]
    [MemberData(nameof(GenerateUpdateData))]
    public async Task Update_ReturnExpectedResult(int answerId, AnswerDto dto, TestResultCode testResultCode)
    {
        #region [Arrange]

        var serializedLoginDto = JsonSerializer.Serialize(dto);

        var stringContent = new StringContent(serializedLoginDto, Encoding.UTF8, "application/json");

        if (testResultCode != TestResultCode.Unauthorized)
        {
            await AuthenticateAsync();
        }

        #endregion

        #region [Act]

        var response = await _client.PutAsync($"{BaseAddress}?id={answerId}", stringContent);

        if (testResultCode == TestResultCode.Unauthorized)
        {
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            return;
        }

        var streamResponse = await response.Content.ReadAsStreamAsync();

        var apiResponse = await JsonSerializer.DeserializeAsync<CustomResponse>(streamResponse);

        #endregion

        #region [Assert]

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

        #endregion

    }

    [Fact]
    public async Task GetVotes_ReturnCustomResponseOfListOfVoteDto()
    {
        #region [Act]

        var response = await _client.GetAsync(BaseAddress + "/AnswerVotes");

        var streamResponse = await response.Content.ReadAsStringAsync();

        var apiResponse = await Task.Factory.StartNew(() =>
            JsonConvert.DeserializeObject<CustomResponse<List<VoteDto>>>(streamResponse));

        #endregion

        #region [Assert]

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        apiResponse?.Data?.Should().BeOfType<List<VoteDto>>();

        #endregion
    }

    [Theory]
    [MemberData(nameof(GenerateSubmitVoteData))]
    public async Task SubmitVote_ReturnExpectedResult(int answerId, bool kind, TestResultCode testResultCode)
    {
        #region [Arrange]


        if (testResultCode != TestResultCode.Unauthorized)
        {
            await AuthenticateAsync();
        }

        #endregion

        #region [Act]

        var response = await _client.PostAsync($"{BaseAddress}/SubmitVote?answerId={answerId}&kind={kind}", null);

        if (testResultCode == TestResultCode.Unauthorized)
        {
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            return;
        }

        var streamResponse = await response.Content.ReadAsStreamAsync();

        var apiResponse = await JsonSerializer.DeserializeAsync<CustomResponse>(streamResponse);

        #endregion

        #region [Assert]

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


        #endregion


    }

    [Theory]
    [MemberData(nameof(GenerateDeleteData))]
    public async Task Delete_ReturnExpectedResult(int answerId, TestResultCode testResultCode)
    {

        #region [Arrange]

        if (testResultCode != TestResultCode.Unauthorized)
        {
            await AuthenticateAsync();
        }

        #endregion

        #region [Act]

        var response = await _client.DeleteAsync($"{BaseAddress}?id={answerId}");

        if (testResultCode == TestResultCode.Unauthorized)
        {
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
            return;
        }

        var streamResponse = await response.Content.ReadAsStreamAsync();

        var apiResponse = await JsonSerializer.DeserializeAsync<CustomResponse>(streamResponse);

        #endregion

        #region [Assert]

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

        #endregion
    }

    #endregion

    #region [Data Method(s)]

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

    #endregion
}

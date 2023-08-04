using RedditMockup.Common.Dtos;
using RedditMockup.Model.Entities;
using Profile = AutoMapper.Profile;

namespace RedditMockup.Common.Profiles;

public class VoteProfile : Profile
{
    public VoteProfile()
    {
        CreateMap<VoteDto, AnswerVote>()
            .ReverseMap();

        CreateMap<VoteDto, QuestionVote>()
            .ReverseMap();
    }
}
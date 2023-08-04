using RedditMockup.Common.Dtos;
using RedditMockup.Model.Entities;
using Profile = AutoMapper.Profile;

namespace RedditMockup.Common.Profiles;

public class QuestionProfile : Profile
{
    public QuestionProfile()
    {
        CreateMap<Question, QuestionDto>()
            .ForMember(questionDto => questionDto.UserGuid,
                option => option.MapFrom(question => question.User!.Guid));

        CreateMap<QuestionDto, Question>();
    }
}
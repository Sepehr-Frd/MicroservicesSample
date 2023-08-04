using RedditMockup.Model.Entities;
using Profile = AutoMapper.Profile;

namespace RedditMockup.Common.Profiles;

public class GrpcProfile : Profile
{
    public GrpcProfile() =>
        CreateMap<Question, GrpcQuestionModel>();
    
}

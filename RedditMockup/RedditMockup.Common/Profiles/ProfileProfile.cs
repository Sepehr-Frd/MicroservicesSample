using AutoMapper;
using RedditMockup.Common.Dtos;

namespace RedditMockup.Common.Profiles;

public class ProfileProfile : Profile
{
    public ProfileProfile() =>
        CreateMap<Model.Entities.Profile, ProfileDto>()
            .ReverseMap();

}
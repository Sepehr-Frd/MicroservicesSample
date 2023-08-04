using RedditMockup.Common.Dtos;
using RedditMockup.Model.Entities;
using Profile = AutoMapper.Profile;

namespace RedditMockup.Common.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(userDto => userDto.FirstName,
                option => option.MapFrom(user => user.Person!.FirstName))
            .ForMember(userDto => userDto.LastName,
                option => option.MapFrom(user => user.Person!.LastName));

        CreateMap<UserDto, User>();
    }
}
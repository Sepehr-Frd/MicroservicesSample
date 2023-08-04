using DataSelector.Common.Dtos;
using DataSelector.Model.Models;
using AutoMapper;
using RedditMockup;

namespace DataSelector.Common.Profiles;

public class QuestionProfile : Profile
{
    public QuestionProfile()
    {
        CreateMap<QuestionDocument, QuestionResponseDto>()
            .ReverseMap();

        CreateMap<QuestionPublishedDto, QuestionDocument>();

        CreateMap<GrpcQuestionModel, QuestionResponseDto>();
    }


}


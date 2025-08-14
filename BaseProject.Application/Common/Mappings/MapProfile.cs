using AutoMapper;
using BaseProject.Application.DTOs.User;
using BaseProject.Domain.Entities;
using BaseProject.Shared.DTOs.Common;

namespace BaseProject.Application.Common.Mappings;

public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<User, UserSignInRequestDto>().ReverseMap();
        CreateMap<User, UserSignInResponseDto>().ReverseMap();
        CreateMap<User, UserSignUpRequestDto>().ReverseMap();
        CreateMap<User, UserSignUpResponseDto>().ReverseMap();
        CreateMap<User, UserProfileResponseDto>().ReverseMap();

        CreateMap(typeof(PaginatedList<>), typeof(PaginatedList<>)).ConvertUsing(typeof(PaginationConverter<,>));
    }
}

public class PaginationConverter<TSource, TDestination> : ITypeConverter<PaginatedList<TSource>, PaginatedList<TDestination>>
{
    private readonly IMapper _mapper;

    public PaginationConverter(IMapper mapper)
    {
        _mapper = mapper;
    }

    public PaginatedList<TDestination> Convert(PaginatedList<TSource> source, PaginatedList<TDestination> destination, ResolutionContext context)
    {
        var mappedItems = _mapper.Map<List<TDestination>>(source.Items);
        return new PaginatedList<TDestination>(mappedItems, source.TotalCount, source.CurrentPage, source.PageSize);
    }
}

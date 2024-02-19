using AutoMapper;
using EventManagement.API.Models;
using EventManagement.API.ViewModels;
using EventManagement.Domain;

namespace EventManagement.API.Profiles;

public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<CreateCategory, Category>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));

        CreateMap<Category, CategoryViewModel>();
    }
}
using AutoMapper;
using EventManagement.API.Models;
using EventManagement.API.ViewModels;
using EventManagement.Domain;

namespace EventManagement.API.Profiles;

public class EventProfile : Profile
{
    public EventProfile()
    {
        CreateMap<CreateEvent, Event>()
            .ForMember(dest => dest.Created, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()));
        
        CreateMap<Event, EventDetailsViewModel>();
        
        CreateMap<Event, EventListViewModel>();

        CreateMap<UpdateEvent, Event>()
            .ForMember(dest => dest.Created, opt => opt.Ignore());
    }
}
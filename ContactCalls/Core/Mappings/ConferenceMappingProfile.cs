using AutoMapper;
using ContactCalls.Domain.Entities;
using ContactCalls.Dtos;

namespace ContactCalls.Core.Mappings;

public class ConferenceMappingProfile : Profile
{
    public ConferenceMappingProfile()
    {
        CreateMap<Conference, ConferenceDto>();
        
        CreateMap<ConferenceParticipant, ConferenceParticipantDto>()
            .ForMember(dest => dest.PhoneNumber, 
                opt => opt.MapFrom(src => src.Phone.Number))
            .ForMember(dest => dest.ContactName, 
                opt => opt.MapFrom(src => $"{src.Phone.Contact.LastName} {src.Phone.Contact.FirstName}"));
    }
}
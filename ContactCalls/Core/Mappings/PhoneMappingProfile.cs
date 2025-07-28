using AutoMapper;
using ContactCalls.Domain.Entities;
using ContactCalls.Dtos;

namespace ContactCalls.Core.Mappings;

public class PhoneMappingProfile : Profile
{
    public PhoneMappingProfile()
    {
        CreateMap<Phone, PhoneDto>()
            .ForMember(dest => dest.ContactName, 
                opt => opt.MapFrom(src => $"{src.Contact.LastName} {src.Contact.FirstName}"));
                
        CreateMap<CreatePhoneDto, Phone>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Contact, opt => opt.Ignore())
            .ForMember(dest => dest.OutgoingCalls, opt => opt.Ignore())
            .ForMember(dest => dest.IncomingCalls, opt => opt.Ignore())
            .ForMember(dest => dest.ConferenceParticipations, opt => opt.Ignore());
            
        CreateMap<UpdatePhoneDto, Phone>()
            .ForMember(dest => dest.ContactId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Contact, opt => opt.Ignore())
            .ForMember(dest => dest.OutgoingCalls, opt => opt.Ignore())
            .ForMember(dest => dest.IncomingCalls, opt => opt.Ignore())
            .ForMember(dest => dest.ConferenceParticipations, opt => opt.Ignore());
    }
}

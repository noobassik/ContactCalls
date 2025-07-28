using AutoMapper;
using ContactCalls.Domain.Entities;
using ContactCalls.Dtos;

namespace ContactCalls.Core.Mappings;

public class ContactMappingProfile : Profile
{
    public ContactMappingProfile()
    {
        CreateMap<Contact, ContactDto>()
            .ForMember(dest => dest.Phones, opt => opt.MapFrom(src => src.Phones));
            
        CreateMap<CreateContactDto, Contact>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Profile, opt => opt.Ignore())
            .ForMember(dest => dest.Phones, opt => opt.Ignore());
            
        CreateMap<UpdateContactDto, Contact>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.Profile, opt => opt.Ignore())
            .ForMember(dest => dest.Phones, opt => opt.Ignore());
            
        CreateMap<ContactProfile, ContactProfileDto>();
        
        CreateMap<CreateContactProfileDto, ContactProfile>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ContactId, opt => opt.Ignore())
            .ForMember(dest => dest.Contact, opt => opt.Ignore());
    }
}


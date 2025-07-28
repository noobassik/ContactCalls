using AutoMapper;
using ContactCalls.Domain.Entities;
using ContactCalls.Dtos;

namespace ContactCalls.Core.Mappings;

public class CallMappingProfile : Profile
{
	public CallMappingProfile()
	{
		CreateMap<Call, CallDto>()
			.ForMember(dest => dest.FromPhoneNumber,
				opt => opt.MapFrom(src => src.FromPhone.Number))
			.ForMember(dest => dest.ToPhoneNumber,
				opt => opt.MapFrom(src => src.ToPhone.Number))
			.ForMember(dest => dest.FromContactName,
				opt => opt.MapFrom(src => $"{src.FromPhone.Contact.LastName} {src.FromPhone.Contact.FirstName}"))
			.ForMember(dest => dest.ToContactName,
				opt => opt.MapFrom(src => $"{src.ToPhone.Contact.LastName} {src.ToPhone.Contact.FirstName}"));

		CreateMap<CreateCallDto, Call>()
			.ForMember(dest => dest.Id, opt => opt.Ignore())
			.ForMember(dest => dest.FromPhone, opt => opt.Ignore())
			.ForMember(dest => dest.ToPhone, opt => opt.Ignore());

		CreateMap<UpdateCallDto, Call>()
			.ForMember(dest => dest.FromPhone, opt => opt.Ignore())
			.ForMember(dest => dest.ToPhone, opt => opt.Ignore());

		CreateMap<Call, BillingItemDto>()
			.ForMember(dest => dest.CallId, opt => opt.MapFrom(src => src.Id))
			.ForMember(dest => dest.CallDate, opt => opt.MapFrom(src => src.StartTime))
			.ForMember(dest => dest.FromPhoneNumber, opt => opt.MapFrom(src => src.FromPhone.Number))
			.ForMember(dest => dest.ToPhoneNumber, opt => opt.MapFrom(src => src.ToPhone.Number));
	}
}
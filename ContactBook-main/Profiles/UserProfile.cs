using AutoMapper;
namespace ContactBookApi.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<DTOs.UserRequestDTOs.RegRequestDTO, Models.Contact>();

            CreateMap<Models.Contact, DTOs.UserResponseDTOs.LoginResponseDTO>()
                   .ForMember(dest => dest.Name, option => option.MapFrom(src =>
                            $"{src.FirstName} {src.LastName}"));

            CreateMap<Models.Contact, DTOs.UserResponseDTOs.GetUserDTO>()
                .ForMember(dest => dest.Name, option => option.MapFrom(src =>
                            $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.Address,
                            option => option.MapFrom(src => $"{src.StreetAddress}, {src.City}, {src.State}."))
                .ForMember(dest => dest.DateOfBirth,
                            option => option.MapFrom(src => src.DateOfBirth.ToShortDateString()));
        }
    }
}

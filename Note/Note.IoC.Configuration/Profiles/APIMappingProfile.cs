namespace Note.IoC.Configuration.Profiles
{
    using AutoMapper;
    using DCR = API.DataContracts.Responses;
    using DC = API.DataContracts;
    using RP = Repository.Data.Entities;

    public class APIMappingProfile : Profile
    {
        public APIMappingProfile()
        {
            CreateMap<RP.User, DC.User>();

            //Mapping First name and last name as name.
            CreateMap<RP.User, DCR.UserCreationResponse>()
                .ForMember(dest => dest.Token, opt => opt.MapFrom(src =>
                 "N/A"))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src =>
                    $"{src.FirstName} {src.LastName}"));

            CreateMap<RP.NewPatient, DC.NewPatient>();
        }
    }
}

namespace Note.IoC.Configuration.Profiles
{
    using AutoMapper;
    using RP = Repository.Data.Entities;

    public class ServicesMappingProfile : Profile
    {
        public ServicesMappingProfile()
        {
            //business and repository mappings
           // CreateMap<RP.NewPatient, S.NewPatient>();
           // CreateMap<RP.User, S.User>();
           // CreateMap<S.User, RP.User>();
        }
    }
}

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


            #region Operatory Notes
            CreateMap<RP.operatory_notes, DC.OperatoryNotes>()
                .AfterMap((rpnotes, dcnotes) => dcnotes.Patient = new DC.Patient
                {
                    Id  = rpnotes.patient_id,
                    Name = $"{rpnotes.patientFirstName} {rpnotes.patientLastName}"
                })
                  .AfterMap((rpnotes, dcnotes) => dcnotes.Provider = new DC.Provider
                  {
                      Id = rpnotes.provider_id,
                      Name = $"{rpnotes.first_name} {rpnotes.last_name}"
                  })
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src =>
                $"{src.note_id}"))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src =>
                $"{src.Date_entered}"))
               .ForMember(dest => dest.NoteClass, opt => opt.MapFrom(src =>
               $"{src.note_class}"))
               .ForMember(dest => dest.NoteType, opt => opt.MapFrom(src =>
              $"{src.note_type}"))
              .ForMember(dest => dest.NoteTypeId, opt => opt.MapFrom(src =>
             $"{src.note_type_id}"))
             .ForMember(dest => dest.Description, opt => opt.MapFrom(src =>
            $"{src.description}"))
             .ForMember(dest => dest.Note, opt => opt.MapFrom(src =>
            $"{src.note}"))
            .ForMember(dest => dest.Color, opt => opt.MapFrom(src =>
           $"{src.color}"))
           .ForMember(dest => dest.PostProcStatus, opt => opt.MapFrom(src =>
            $"{src.post_proc_status}"))
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src =>
             $"{src.date_modified}"))
           .ForMember(dest => dest.ModifiedBy, opt => opt.MapFrom(src =>
            $"{src.modified_by}"))
           .ForMember(dest => dest.LockedEod, opt => opt.MapFrom(src =>
            $"{src.locked_eod}"))
          .ForMember(dest => dest.Status, opt => opt.MapFrom(src =>
           $"{src.status}"))
          .ForMember(dest => dest.ToothData, opt => opt.MapFrom(src =>
           $"{src.tooth_data}"))
           .ForMember(dest => dest.ClaimID, opt => opt.MapFrom(src =>
             $"{src.claim_id}"))
           .ForMember(dest => dest.StatementYn, opt => opt.MapFrom(src =>
            $"{src.statement_yn}"))
          .ForMember(dest => dest.RespPartyId, opt => opt.MapFrom(src =>
           $"{src.resp_party_id}"))
          .ForMember(dest => dest.Tooth, opt => opt.MapFrom(src =>
           $"{src.tooth}"))
          .ForMember(dest => dest.TranNum, opt => opt.MapFrom(src =>
           $"{src.tran_num}"))
           .ForMember(dest => dest.ArchiveName, opt => opt.MapFrom(src =>
            $"{src.archive_name}"))
            .ForMember(dest => dest.ArchivePath, opt => opt.MapFrom(src =>
             $"{src.archive_path}"))
            .ForMember(dest => dest.ServiceCode, opt => opt.MapFrom(src =>
             $"{src.service_code}"))
           .ForMember(dest => dest.ClinicID, opt => opt.MapFrom(src =>
            $"{src.practice_id}"))
          .ForMember(dest => dest.Freshness, opt => opt.MapFrom(src =>
           $"{src.freshness}"))
         .ForMember(dest => dest.SurfaceDetail, opt => opt.MapFrom(src =>
          $"{src.surface_detail}"))
          .ForMember(dest => dest.Surface, opt => opt.MapFrom(src =>
           $"{src.surface}"));


            #endregion Operatory Notes

        }
    }
}
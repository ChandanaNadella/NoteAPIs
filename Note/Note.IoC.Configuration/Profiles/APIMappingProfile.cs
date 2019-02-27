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
            //Mapping First name and last name as name.   
            CreateMap<DC.OperatoryNotesUpdateDto, RP.operatory_notes>();


          //  CreateMap<DC.OperatoryNotesUpdateDto, RP.operatory_notes>()
           
          // .ForMember(dest => dest.note_id, opt => opt.MapFrom(src =>
          //      $"{src.Id}"))
          //       .ForMember(dest => dest.patient_id, opt => opt.MapFrom(src =>
          //      $"{src.PatientId}"))
          //     .ForMember(dest => dest.user_id, opt => opt.MapFrom(src =>
          //     $"{src.UserId}"))
          //     .ForMember(dest => dest.note_class, opt => opt.MapFrom(src =>
          //    $"{src.NoteClass}"))
          //    .ForMember(dest => dest.note_type, opt => opt.MapFrom(src =>
          //   $"{src.NoteType}"))
          //   .ForMember(dest => dest.note, opt => opt.MapFrom(src =>
          //  $"{src.Note}"))
          //  .ForMember(dest => dest.color, opt => opt.MapFrom(src =>
          // $"{src.Color}"))
          // .ForMember(dest => dest.post_proc_status, opt => opt.MapFrom(src =>
          //  $"{src.PostProcStatus}"))
          //  .ForMember(dest => dest.locked_eod, opt => opt.MapFrom(src =>
          //   $"{src.LockedEod}"))
          // .ForMember(dest => dest.status, opt => opt.MapFrom(src =>
          //  $"{src.Status}"))
          // .ForMember(dest => dest.tooth_data, opt => opt.MapFrom(src =>
          //  $"{src.ToothData}"))
          //.ForMember(dest => dest.claim_id, opt => opt.MapFrom(src =>
          // $"{src.ClaimID}"))
          //.ForMember(dest => dest.statement_yn, opt => opt.MapFrom(src =>
          // $"{src.StatementYn}"))
          // .ForMember(dest => dest.resp_party_id, opt => opt.MapFrom(src =>
          //   $"{src.RespPartyId}"))
          // .ForMember(dest => dest.tooth, opt => opt.MapFrom(src =>
          //  $"{src.Tooth}"))
          //.ForMember(dest => dest.tran_num, opt => opt.MapFrom(src =>
          // $"{src.TranNum}"))
          //.ForMember(dest => dest.archive_name, opt => opt.MapFrom(src =>
          // $"{src.ArchiveName}"))
          //.ForMember(dest => dest.archive_path, opt => opt.MapFrom(src =>
          // $"{src.ArchivePath}"))
          // .ForMember(dest => dest.service_code, opt => opt.MapFrom(src =>
          //  $"{src.ServiceCode}"))
          //  .ForMember(dest => dest.practice_id, opt => opt.MapFrom(src =>
          //   $"{src.ClinicID}"))
          //  .ForMember(dest => dest.surface_detail, opt => opt.MapFrom(src =>
          //   $"{src.SurfaceDetail}"))
          // .ForMember(dest => dest.surface, opt => opt.MapFrom(src =>
          //  $"{src.Surface}"));


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
                $"{src.date_entered}"))
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
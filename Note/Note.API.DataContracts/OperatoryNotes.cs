namespace Note.API.DataContracts
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class OperatoryNotes
{
        public OperatoryNotes()
        {
            Patient = new Patient();
            Provider = new Provider();
        }
   
    public int Id { set; get; }

    public Patient Patient { set; get; }

    public Provider Provider { set; get; }

    public DateTime CreatedDate { set; get; }

    public char NoteClass { set; get; }

    public string NoteType { set; get; }

    public int NoteTypeId { set; get; }

    public string Description { set; get; }

    public string Note { set; get; }

    public int Color { set; get; }

    public char PostProcStatus { set; get; }

    public String ModifiedDate { set; get; }

    public string ModifiedBy { set; get; }

    public int LockedEod { set; get; }

    public char Status { set; get; }

    public string ToothData { set; get; }

    //DEFAULT Value = -1
    public int ClaimID { set; get; }

    //DEFAULT Value = N
    public char StatementYn { set; get; }

    public string RespPartyId { set; get; }
    public string Tooth { set; get; }

    public int TranNum { set; get; }

    public string ArchiveName { set; get; }

    public string ArchivePath { set; get; }

    public string ServiceCode { set; get; }

    public short ClinicID{ set; get; }

    public DateTime Freshness { set; get; }

    public string SurfaceDetail { set; get; }

    public string Surface { set; get; }
}
}
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
    [Key]
    public int Id { set; get; }

    public Patient Patient { set; get; }

    public Provider Provider { set; get; }

    public DateTime CreatedDate { set; get; }


    [MaxLength(1)]
    public char NoteClass { set; get; }

    [MaxLength(1)]
    public string NoteType { set; get; }

    public int NoteTypeId { set; get; }

    [MaxLength(40)]
    public string Description { set; get; }

    [MaxLength(4000)]
    public string Note { set; get; }

    public int Color { set; get; }

    [MaxLength(1)]
    public char PostProcStatus { set; get; }

    public String ModifiedDate { set; get; }

    [MaxLength(3)]
    public string ModifiedBy { set; get; }

    public int LockedEod { set; get; }

    [MaxLength(1)]
    public char Status { set; get; }

    [MaxLength(55)]
    public string ToothData { set; get; }

    //DEFAULT Value = -1
    public int ClaimID { set; get; }

    [MaxLength(1)]
    //DEFAULT Value = N
    public char StatementYn { set; get; }

    [MaxLength(5)]
    public string RespPartyId { set; get; }

    [MaxLength(10)]
    public string Tooth { set; get; }

    public int TranNum { set; get; }

    [MaxLength(40)]
    public string ArchiveName { set; get; }

    [MaxLength(4000)]
    public string ArchivePath { set; get; }

    [MaxLength(5)]
    public string ServiceCode { set; get; }

    public string PracticeId { set; get; }

    public DateTime Freshness { set; get; }

    [MaxLength(23)]
    public string SurfaceDetail { set; get; }

    [MaxLength(8)]
    public string Surface { set; get; }
}
}
namespace Note.API.DataContracts
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class NewPatient
    {
        [DataType(DataType.Text)]
        public int Id { get; set; }

        [DataType(DataType.Text)]
        public int ClinicId { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [DataType(DataType.Text)]
        public int Year { get; set; }

        [DataType(DataType.Text)]
        public int Month { get; set; }

        [DataType(DataType.Text)]
        public int Week { get; set; }

        [DataType(DataType.Text)]
        public int Count { get; set; }

        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [DataType(DataType.Text)]
        public int ProviderType { get; set; }

        [DataType(DataType.Text)]
        public bool IsCompleted { get; set; }
    }
}

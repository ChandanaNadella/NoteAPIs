namespace Note.Services.Contracts
{
    using System;
    using static Note.Services.NewPatientService;

    public interface INewPatientService
    {
        /// <summary>
        /// Gets the details of a new patient by Clinic Id.
        /// Aviral, Bpktech.
        /// </summary>
        /// <param name="clinicId">Clinic Id</param>
        /// <returns>Clinic new patient details.</returns>
        Tuple<DashboardDetails, bool, string> GetNewPatientDetailByClinicIdAsync(string clinicId);
        Tuple<DashboardDetails, bool, string> GetNewPatientDetailByCurrentMonthAsync();
    }
}

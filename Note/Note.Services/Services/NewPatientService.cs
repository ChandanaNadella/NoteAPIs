namespace Note.Services
{
    using AutoMapper;
    using Note.Repository.Data;
    using Note.Services.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public class NewPatientService : INewPatientService
    {
        private NoteDataContext _context = null;
        private readonly IMapper _mapper;

        public NewPatientService(NoteDataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the details of a new patient by Clinic Id.
        /// Aviral, Bpktech.
        /// </summary>
        /// <param name="clinicId">Clinic Id</param>
        /// <returns>Clinic new patient details.</returns>
        public Tuple<DashboardDetails, bool, string> GetNewPatientDetailByClinicIdAsync(string clinicId)
        {
            Tuple<DashboardDetails, bool, string> responseData;
            try
            {
                DateTime currentDate = DateTime.Now;
                DateTime prevMonthDate = DateTime.Now.AddMonths(-1);
                int year = currentDate.Year;
                int month = currentDate.Month;

                // using System.Globalization;
                var calendar = CultureInfo.CurrentCulture.Calendar;
                var firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
                var weekPeriods =
                Enumerable.Range(1, calendar.GetDaysInMonth(year, month))
                          .Select(d =>
                          {
                              var date = new DateTime(year, month, d);
                              var weekNumInYear = calendar.GetWeekOfYear(date, CalendarWeekRule.FirstDay, firstDayOfWeek);
                              return new { date, weekNumInYear };
                          })
                          .GroupBy(x => x.weekNumInYear)
                          .Select(x => new { DateFrom = x.First().date, To = x.Last().date })
                          .ToList();

                List<Chart> lstDashBoard = new List<Chart>();
                Chart objProduction = new Chart();
                objProduction.VisitCount = new List<int>();

                Chart objNewPatient = new Chart();
                objNewPatient.Id = 1;
                objNewPatient.Title = "New Patients";
                objNewPatient.Date = currentDate.ToString("MM-dd-yyyy");
                objNewPatient.VisitCount = new List<int>();
                foreach (var week in weekPeriods)
                {
                    int count = _context.NewPatient.Where(np => np.Date >= week.DateFrom && np.Date <= week.To && np.ClinicId.Equals(clinicId)).Sum(x => x.Count);
                    objNewPatient.VisitCount.Add(count);
                    objProduction.VisitCount.Add(count);
                }

                objNewPatient.Details = new List<Details>();
                Details objDetail = new Details()
                {
                    Title = "Scheduled",
                    Value = _context.NewPatient.Where(np => np.Date.Year == year && np.Date.Month == month && np.ClinicId.Equals(clinicId)).Sum(x => x.Count).ToString()
                };
                objNewPatient.Details.Add(objDetail);
                objDetail = new Details()
                {
                    Title = "Completed",
                    Value = _context.NewPatient.Where(np => np.Date.Year == year && np.Date.Month == month && np.IsCompleted && np.ClinicId.Equals(clinicId)).Sum(x => x.Count).ToString()
                };
                objNewPatient.Details.Add(objDetail);
                objDetail = new Details()
                {
                    Title = "Broken",
                    Value = _context.NewPatient.Where(np => np.Date.Year == year && np.Date.Month == month && !np.IsCompleted && np.ClinicId.Equals(clinicId)).Sum(x => x.Count).ToString(),
                    LastMonth = _context.NewPatient.Where(np => np.Date.Year == prevMonthDate.Year && np.Date.Month == prevMonthDate.Month && !np.IsCompleted && np.ClinicId.Equals(clinicId)).Sum(x => x.Count).ToString()
                };
                objNewPatient.Details.Add(objDetail);

                lstDashBoard.Add(objNewPatient);

                //objProduction.Id = 2;
                //objProduction.Title = "Production";
                //objProduction.Date = currentDate.ToString("MM-dd-yyyy");
                //objProduction.Details = new List<Details>();
                //objDetail = new Details()
                //{
                //    Title = "Scheduled",
                //    Value = _context.NewPatient.Where(np => np.Date.Year == year && np.Date.Month == month && np.ClinicId.Equals(clinicId)).Sum(x => x.Amount).ToString("0.00")
                //};
                //objProduction.Details.Add(objDetail);
                //objDetail = new Details()
                //{
                //    Title = "Completed",
                //    Value = _context.NewPatient.Where(np => np.Date.Year == year && np.Date.Month == month && np.IsCompleted && np.ClinicId.Equals(clinicId)).Sum(x => x.Amount).ToString("0.00")
                //};
                //objProduction.Details.Add(objDetail);
                //objDetail = new Details()
                //{
                //    Title = "Broken",
                //    Value = _context.NewPatient.Where(np => np.Date.Year == year && np.Date.Month == month && !np.IsCompleted && np.ClinicId.Equals(clinicId)).Sum(x => x.Amount).ToString("0.00"),
                //    LastMonth = _context.NewPatient.Where(np => np.Date.Year == prevMonthDate.Year && np.Date.Month == prevMonthDate.Month && !np.IsCompleted && np.ClinicId.Equals(clinicId)).Sum(x => x.Amount).ToString("0.00")
                //};
                //objProduction.Details.Add(objDetail);

                //lstDashBoard.Add(objProduction);

                Carousel objCarousel = new Carousel();
                objCarousel.Id = 1;
                objCarousel.Title = "New Patients";
                objCarousel.Details = new List<Details>();
                Details objNewPatientDetail = new Details()
                {
                    Title = "Scheduled",
                    Value = _context.NewPatient.Where(np => np.Date.Year == year && np.Date.Month == month && np.ClinicId.Equals(clinicId)).Sum(x => x.Amount).ToString("0.00"),
                    LastMonth = _context.NewPatient.Where(np => np.Date.Year == prevMonthDate.Year && np.Date.Month == prevMonthDate.Month && np.ClinicId.Equals(clinicId)).Sum(x => x.Amount).ToString("0.00")
                };
                objCarousel.Details.Add(objNewPatientDetail);

                objNewPatientDetail = new Details()
                {
                    Title = "Completed",
                    Value = _context.NewPatient.Where(np => np.Date.Year == year && np.Date.Month == month && np.IsCompleted && np.ClinicId.Equals(clinicId)).Sum(x => x.Amount).ToString("0.00"),
                    LastMonth = _context.NewPatient.Where(np => np.Date.Year == prevMonthDate.Year && np.Date.Month == prevMonthDate.Month && np.IsCompleted && np.ClinicId.Equals(clinicId)).Sum(x => x.Amount).ToString("0.00")
                };
                objCarousel.Details.Add(objNewPatientDetail);

                objNewPatientDetail = new Details()
                {
                    Title = "Broken",
                    Value = _context.NewPatient.Where(np => np.Date.Year == year && np.Date.Month == month && !np.IsCompleted && np.ClinicId.Equals(clinicId)).Sum(x => x.Amount).ToString("0.00"),
                    LastMonth = _context.NewPatient.Where(np => np.Date.Year == prevMonthDate.Year && np.Date.Month == prevMonthDate.Month && !np.IsCompleted && np.ClinicId.Equals(clinicId)).Sum(x => x.Amount).ToString("0.00")
                };
                objCarousel.Details.Add(objNewPatientDetail);

                objNewPatientDetail = new Details()
                {
                    Title = "Hygienist Patients",
                    Value = _context.NewPatient.Where(np => np.Date.Year == year && np.Date.Month == month && np.ProviderType == 25 && np.ClinicId.Equals(clinicId)).Sum(x => x.Amount).ToString("0.00"),
                    LastMonth = _context.NewPatient.Where(np => np.Date.Year == prevMonthDate.Year && np.Date.Month == prevMonthDate.Month && np.ProviderType == 25).Sum(x => x.Amount).ToString("0.00")
                };
                objCarousel.Details.Add(objNewPatientDetail);

                objNewPatientDetail = new Details()
                {
                    Title = "Dentist Patients",
                    Value = _context.NewPatient.Where(np => np.Date.Year == year && np.Date.Month == month && np.ProviderType == 27 && np.ClinicId.Equals(clinicId)).Sum(x => x.Amount).ToString("0.00"),
                    LastMonth = _context.NewPatient.Where(np => np.Date.Year == prevMonthDate.Year && np.Date.Month == prevMonthDate.Month && np.ProviderType == 27 && np.ClinicId.Equals(clinicId)).Sum(x => x.Amount).ToString("0.00")
                };
                objCarousel.Details.Add(objNewPatientDetail);

                DashboardDetails objDashBoardDeatils = new DashboardDetails();
                objDashBoardDeatils.DashBoard = lstDashBoard;
                objDashBoardDeatils.Carousel = objCarousel;

                var result = _mapper.Map<DashboardDetails>(objDashBoardDeatils);
                responseData = Tuple.Create(result, true, "Data Found");
            }
            catch (Exception ex)
            {
                responseData = Tuple.Create<DashboardDetails, bool, string>(null, false, ex.Message);
            }

            return responseData;
        }

        public Tuple<DashboardDetails, bool, string> GetNewPatientDetailByCurrentMonthAsync()
        {
            Tuple<DashboardDetails, bool, string> responseData;
            try
            {
                DateTime currentDate = DateTime.Now;
                DateTime prevMonthDate = DateTime.Now.AddMonths(-1);
                int year = currentDate.Year;
                int month = currentDate.Month;

                // using System.Globalization;
                var calendar = CultureInfo.CurrentCulture.Calendar;
                var firstDayOfWeek = CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek;
                var weekPeriods =
                Enumerable.Range(1, calendar.GetDaysInMonth(year, month))
                          .Select(d =>
                          {
                              var date = new DateTime(year, month, d);
                              var weekNumInYear = calendar.GetWeekOfYear(date, CalendarWeekRule.FirstDay, firstDayOfWeek);
                              return new { date, weekNumInYear };
                          })
                          .GroupBy(x => x.weekNumInYear)
                          .Select(x => new { DateFrom = x.First().date, To = x.Last().date })
                          .ToList();

                List<Chart> lstDashBoard = new List<Chart>();
                Chart objProduction = new Chart();
                objProduction.VisitCount = new List<int>();

                Chart objNewPatient = new Chart();
                objNewPatient.Id = 1;
                objNewPatient.Title = "New Patients";
                objNewPatient.Date = currentDate.ToString("MM-dd-yyyy");
                objNewPatient.VisitCount = new List<int>();
                foreach (var week in weekPeriods)
                {
                    int count = _context.NewPatient.Where(np => np.Date >= week.DateFrom && np.Date <= week.To).Sum(x => x.Count);
                    objNewPatient.VisitCount.Add(count);
                    objProduction.VisitCount.Add(count);
                }

                objNewPatient.Details = new List<Details>();
                Details objDetail = new Details()
                {
                    Title = "Scheduled",
                    Value = _context.NewPatient.Where(np => np.Date.Year == year && np.Date.Month == month).Sum(x => x.Count).ToString()
                };
                objNewPatient.Details.Add(objDetail);
                objDetail = new Details()
                {
                    Title = "Completed",
                    Value = _context.NewPatient.Where(np => np.Date.Year == year && np.Date.Month == month && np.IsCompleted).Sum(x => x.Count).ToString()
                };
                objNewPatient.Details.Add(objDetail);
                objDetail = new Details()
                {
                    Title = "Broken",
                    Value = _context.NewPatient.Where(np => np.Date.Year == year && np.Date.Month == month && !np.IsCompleted).Sum(x => x.Count).ToString(),
                    LastMonth = _context.NewPatient.Where(np => np.Date.Year == prevMonthDate.Year && np.Date.Month == prevMonthDate.Month && !np.IsCompleted).Sum(x => x.Count).ToString()
                };
                objNewPatient.Details.Add(objDetail);

                lstDashBoard.Add(objNewPatient);

                //objProduction.Id = 2;
                //objProduction.Title = "Production";
                //objProduction.Date = currentDate.ToString("MM-dd-yyyy");
                //objProduction.Details = new List<Details>();
                //objDetail = new Details()
                //{
                //    Title = "Scheduled",
                //    Value = _context.NewPatient.Where(np => np.Date.Year == year && np.Date.Month == month).Sum(x => x.Amount).ToString("0.00")
                //};
                //objProduction.Details.Add(objDetail);
                //objDetail = new Details()
                //{
                //    Title = "Completed",
                //    Value = _context.NewPatient.Where(np => np.Date.Year == year && np.Date.Month == month && np.IsCompleted).Sum(x => x.Amount).ToString("0.00")
                //};
                //objProduction.Details.Add(objDetail);
                //objDetail = new Details()
                //{
                //    Title = "Broken",
                //    Value = _context.NewPatient.Where(np => np.Date.Year == year && np.Date.Month == month && !np.IsCompleted).Sum(x => x.Amount).ToString("0.00"),
                //    LastMonth = _context.NewPatient.Where(np => np.Date.Year == prevMonthDate.Year && np.Date.Month == prevMonthDate.Month && !np.IsCompleted).Sum(x => x.Amount).ToString("0.00")
                //};
                //objProduction.Details.Add(objDetail);

                //lstDashBoard.Add(objProduction);

                Carousel objCarousel = new Carousel();
                objCarousel.Id = 1;
                objCarousel.Title = "New Patients";
                objCarousel.Details = new List<Details>();
                Details objNewPatientDetail = new Details()
                {
                    Title = "Scheduled",
                    Value = _context.NewPatient.Where(np => np.Date.Year == year && np.Date.Month == month).Sum(x => x.Amount).ToString("0.00"),
                    LastMonth = _context.NewPatient.Where(np => np.Date.Year == prevMonthDate.Year && np.Date.Month == prevMonthDate.Month).Sum(x => x.Amount).ToString("0.00")
                };
                objCarousel.Details.Add(objNewPatientDetail);

                objNewPatientDetail = new Details()
                {
                    Title = "Completed",
                    Value = _context.NewPatient.Where(np => np.Date.Year == year && np.Date.Month == month && np.IsCompleted).Sum(x => x.Amount).ToString("0.00"),
                    LastMonth = _context.NewPatient.Where(np => np.Date.Year == prevMonthDate.Year && np.Date.Month == prevMonthDate.Month && np.IsCompleted).Sum(x => x.Amount).ToString("0.00")
                };
                objCarousel.Details.Add(objNewPatientDetail);

                objNewPatientDetail = new Details()
                {
                    Title = "Broken",
                    Value = _context.NewPatient.Where(np => np.Date.Year == year && np.Date.Month == month && !np.IsCompleted).Sum(x => x.Amount).ToString("0.00"),
                    LastMonth = _context.NewPatient.Where(np => np.Date.Year == prevMonthDate.Year && np.Date.Month == prevMonthDate.Month && !np.IsCompleted).Sum(x => x.Amount).ToString("0.00")
                };
                objCarousel.Details.Add(objNewPatientDetail);

                objNewPatientDetail = new Details()
                {
                    Title = "Higinest Patients",
                    Value = _context.NewPatient.Where(np => np.Date.Year == year && np.Date.Month == month && np.ProviderType == 25).Sum(x => x.Amount).ToString("0.00"),
                    LastMonth = _context.NewPatient.Where(np => np.Date.Year == prevMonthDate.Year && np.Date.Month == prevMonthDate.Month && np.ProviderType == 25).Sum(x => x.Amount).ToString("0.00")
                };
                objCarousel.Details.Add(objNewPatientDetail);

                objNewPatientDetail = new Details()
                {
                    Title = "Dentist Patients",
                    Value = _context.NewPatient.Where(np => np.Date.Year == year && np.Date.Month == month && np.ProviderType == 27).Sum(x => x.Amount).ToString("0.00"),
                    LastMonth = _context.NewPatient.Where(np => np.Date.Year == prevMonthDate.Year && np.Date.Month == prevMonthDate.Month && np.ProviderType == 27).Sum(x => x.Amount).ToString("0.00")
                };
                objCarousel.Details.Add(objNewPatientDetail);

                DashboardDetails objDashBoardDeatils = new DashboardDetails();
                objDashBoardDeatils.DashBoard = lstDashBoard;
                objDashBoardDeatils.Carousel = objCarousel;

                var result = _mapper.Map<DashboardDetails>(objDashBoardDeatils);
                responseData = Tuple.Create(result, true, "Data Found");
            }
            catch (Exception ex)
            {
                responseData = Tuple.Create<DashboardDetails, bool, string>(null, false, ex.Message);
            }

            return responseData;
        }

        public class DashboardDetails
        {
            public List<Chart> DashBoard { set; get; }
            public Carousel Carousel { set; get; }
        }

        public class Chart
        {
            public int Id { set; get; }
            public string Title { set; get; }
            public string Date { set; get; }
            public List<int> VisitCount { set; get; }
            public List<Details> Details { set; get; }

        }

        public class Carousel
        {
            public int Id { set; get; }
            public string Title { set; get; }
            public List<Details> Details { set; get; }
        }

        public class Details
        {
            public string Title { set; get; }
            public string Value { set; get; }
            public string LastMonth { set; get; }
        }

        public class VisitCount
        {
            public int Count { set; get; }
        }
    }
}

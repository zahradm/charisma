using AppointmentDomain;

namespace AppointmentTest;

public class ClinicBuilder
{
    private Clinic _clinic;
    private Doctor _doctor;
    private Patient _patient;
    private List<(DayOfWeek, TimeSpan, TimeSpan)> _doctorSchedule;
    private DateTime _appointmentStartTime;
    private int _appointmentDuration;
    private DoctorService _doctorService;
    private PatientService _patientService;
    private AvailabilityChecker _availabilityChecker;
    private AppointmentValidator _appointmentValidator;
    private AppointmentService _appointmentService;

    public ClinicBuilder()
    {
        // Initialize services
        _doctorService = new DoctorService();
        _patientService = new PatientService();
        _availabilityChecker = new AvailabilityChecker(_doctorService, new List<Appointment>());
        _appointmentValidator = new AppointmentValidator(new StandardWorkingHoursPolicy());
        _appointmentService = new AppointmentService(_appointmentValidator, _availabilityChecker);

        // Initialize default objects
        _clinic = new Clinic(_doctorService, _patientService, _appointmentService);
        _doctor = new Doctor("Ali", "Ahmadi", "09123456789", "General");
        _patient = new Patient("Sara", "Rahimi", "09234567890", 30, "Tamin");
        _doctorSchedule = new List<(DayOfWeek, TimeSpan, TimeSpan)>
        {
            (DayOfWeek.Monday, TimeSpan.FromHours(9), TimeSpan.FromHours(12))
        };
        _appointmentStartTime = DateTime.Now.Date.AddDays((int)DayOfWeek.Monday - (int)DateTime.Now.DayOfWeek).AddHours(10);
        _appointmentDuration = 10;

        SetupDefaults();
    }

    private void SetupDefaults()
    {
        _clinic.AddDoctor(_doctor, _doctorSchedule);
        _clinic.AddPatient(_patient);
    }

    // Fluent interface to customize doctor schedule
    public ClinicBuilder WithDoctorSchedule(List<(DayOfWeek, TimeSpan, TimeSpan)> schedule)
    {
        _doctorSchedule = schedule;
        _clinic = new Clinic(_doctorService, _patientService, _appointmentService);
        SetupDefaults();
        return this;
    }

    // Fluent interface to customize appointment start time
    public ClinicBuilder WithAppointmentStartTime(DateTime startTime)
    {
        _appointmentStartTime = startTime;
        return this;
    }

    // Fluent interface to customize appointment duration
    public ClinicBuilder WithAppointmentDuration(int duration)
    {
        _appointmentDuration = duration;
        return this;
    }

    // Builds the clinic
    public Clinic Build() => _clinic;

    // Returns the appointment details to be used for scheduling
    public (Doctor, Patient, DateTime, int) BuildAppointmentDetails()
    {
        return (_doctor, _patient, _appointmentStartTime, _appointmentDuration);
    }
}

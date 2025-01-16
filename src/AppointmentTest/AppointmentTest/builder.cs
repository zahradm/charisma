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

    public ClinicBuilder()
    {
        _clinic = new Clinic(new StandardWorkingHoursPolicy());
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

    public ClinicBuilder WithDoctorSchedule(List<(DayOfWeek, TimeSpan, TimeSpan)> schedule)
    {
        _doctorSchedule = schedule;
        _clinic = new Clinic(new StandardWorkingHoursPolicy());
        SetupDefaults();
        return this;
    }

    public ClinicBuilder WithAppointmentStartTime(DateTime startTime)
    {
        _appointmentStartTime = startTime;
        return this;
    }

    public ClinicBuilder WithAppointmentDuration(int duration)
    {
        _appointmentDuration = duration;
        return this;
    }

    public Clinic Build() => _clinic;

    public (Doctor, Patient, DateTime, int) BuildAppointmentDetails()
    {
        return (_doctor, _patient, _appointmentStartTime, _appointmentDuration);
    }
}
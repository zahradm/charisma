namespace AppointmentDomain;

public class Clinic
{
    private readonly DoctorService _doctorService;
    private readonly PatientService _patientService;
    private readonly AppointmentService _appointmentService;
    private List<Appointment> _appointments = new List<Appointment>();

    public Clinic(DoctorService doctorService, PatientService patientService, AppointmentService appointmentService)
    {
        _doctorService = doctorService;
        _patientService = patientService;
        _appointmentService = appointmentService;
    }

    public void AddDoctor(Doctor doctor, List<(DayOfWeek Day, TimeSpan Start, TimeSpan End)> schedule)
    {
        _doctorService.AddDoctor(doctor, schedule);
    }

    public void AddPatient(Patient patient)
    {
        _patientService.AddPatient(patient);
    }
    public Appointment CreateNewAppointment(Patient patient, Doctor doctor, DateTime startTime, int duration)
    {
        var appointment = _appointmentService.CreateNewAppointment(patient, doctor, startTime, duration);
        _appointments.Add(appointment);
        Console.WriteLine($"{_patientService}");
        return appointment;
    }

}

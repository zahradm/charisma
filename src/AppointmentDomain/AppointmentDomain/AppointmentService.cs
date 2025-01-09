using AppointmentDomain.Exceptions;
namespace AppointmentDomain;

public class AppointmentService
{
    private readonly List<Appointment> _appointments;
    private readonly AppointmentValidator _validator;
    private readonly AvailabilityChecker _availabilityChecker;

    public AppointmentService(AppointmentValidator validator, AvailabilityChecker availabilityChecker)
    {
        _appointments = new List<Appointment>();
        _validator = validator;
        _availabilityChecker = availabilityChecker;
    }

    public Appointment CreateNewAppointment(Patient patient, Doctor doctor, DateTime startTime, int duration)
    {
        // Validate if the doctor and patient are available.
        if (!_availabilityChecker.IsDoctorAvailable(doctor, startTime, duration))
            throw new DoctorUnavailableException($"Doctor {doctor.LastName} is unavailable at the requested time.");

        if (!_availabilityChecker.IsPatientAvailable(patient, startTime, duration))
            throw new AppointmentOverlapException($"Patient {patient.FirstName} {patient.LastName} has an overlapping appointment.");

        // Validate appointment time (working hours, duration, etc.)
        _validator.ValidateAppointmentTime(doctor, startTime, duration);

        // Create the appointment and add it to the list
        var endTime = startTime.AddMinutes(duration);
        var appointment = new Appointment(doctor, patient, startTime, endTime);
        _appointments.Add(appointment);

        return appointment;
    }

    public List<Appointment> GetAppointments()
    {
        return _appointments;
    }
}

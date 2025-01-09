namespace AppointmentDomain;

public class AvailabilityChecker
{
    private readonly DoctorService _doctorService;
    private readonly List<Appointment> _appointments;

    public AvailabilityChecker(DoctorService doctorService, List<Appointment> appointments)
    {
        _doctorService = doctorService;
        _appointments = appointments;
    }

    public bool IsDoctorAvailable(Doctor doctor, DateTime startTime, int duration)
    {
        var endTime = startTime.AddMinutes(duration);
        var schedule = _doctorService.GetSchedule(doctor)
            .FirstOrDefault(s => s.Day == startTime.DayOfWeek &&
                                 startTime.TimeOfDay >= s.Start &&
                                 endTime.TimeOfDay <= s.End);

        if (schedule == default)
            return false;

        int maxOverlaps = doctor.Type == "General" ? 2 : 3;
        int overlappingAppointments = _appointments.Count(a =>
            a.Doctor == doctor &&
            a.StartTime < endTime &&
            a.EndTime > startTime);

        return overlappingAppointments < maxOverlaps;
    }

    public bool IsPatientAvailable(Patient patient, DateTime startTime, int duration)
    {
        var endTime = startTime.AddMinutes(duration);

        // Check if the patient already has overlapping appointments
        bool hasOverlap = _appointments.Any(a =>
            a.Patient == patient && 
            a.StartTime < endTime && 
            a.EndTime > startTime);

        return !hasOverlap;
    }
}

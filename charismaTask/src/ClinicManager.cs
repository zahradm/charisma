namespace CharismaTask.Src
{
   public class ClinicManager
{
    private List<Doctor> Doctors { get; set; } = new List<Doctor>();
    private List<Patient> Patients { get; set; } = new List<Patient>();
    private List<Appointment> Appointments { get; set; } = new List<Appointment>();

    // Schedule an appointment
    public bool ScheduleAppointment(Patient patient, Doctor doctor, DateTime startTime, int duration)
    {
        // Check if the appointment falls within valid hours and duration
        if (!IsValidAppointmentTime(doctor, startTime, duration))
        {
            Console.WriteLine("Invalid appointment time or duration.");
            return false;
        }

        // Check doctor and patient availability
        if (!IsDoctorAvailable(doctor, startTime, duration) || !IsPatientAvailable(patient, startTime, duration))
        {
            Console.WriteLine("Doctor or patient is not available.");
            return false;
        }

        // Create and add the appointment
        Appointment appointment = new Appointment(
            doctor,
            patient,
            startTime,
            startTime.AddMinutes(duration)
        );
        doctor.Appointments.Add(appointment);
        patient.Appointments.Add(appointment);
        Appointments.Add(appointment);

        Console.WriteLine("Appointment scheduled successfully.");
        return true;
    }

    // Helper: Validate appointment time and duration
    private bool IsValidAppointmentTime(Doctor doctor, DateTime startTime, int duration)
    {
        if (startTime.DayOfWeek == DayOfWeek.Thursday || startTime.DayOfWeek == DayOfWeek.Friday)
        {
            return false; // Doctor is not available on these days
        }

        if (startTime.TimeOfDay < TimeSpan.FromHours(9) || startTime.TimeOfDay > TimeSpan.FromHours(18))
        {
            return false; // Outside valid hours
        }

        if ((doctor.Type == "General" && (duration < 5 || duration > 15)) ||
            (doctor.Type == "Specialist" && (duration < 10 || duration > 30)))
        {
            return false; // Invalid duration
        }

        return true;
    }

    // Helper: Check if doctor is available
    private bool IsDoctorAvailable(Doctor doctor, DateTime startTime, int duration)
    {
        var endTime = startTime.AddMinutes(duration);

        // Check doctor's weekly schedule
        if (!doctor.IsScheduled(startTime))
        {
            return false;
        }

        // Check overlapping appointments
        int maxOverlaps = doctor.Type == "General" ? 2 : 3;
        int overlappingAppointments = doctor.Appointments.Count(a =>
            a.StartTime < endTime && a.EndTime > startTime);

        return overlappingAppointments < maxOverlaps;
    }

    // Helper: Check if patient is available
    private bool IsPatientAvailable(Patient patient, DateTime startTime, int duration)
    {
        var endTime = startTime.AddMinutes(duration);

        // Check for overlapping appointments
        if (patient.Appointments.Any(a => a.StartTime < endTime && a.EndTime > startTime))
        {
            return false;
        }

        // Check daily appointment limit
        int dailyAppointments = patient.Appointments.Count(a => a.StartTime.Date == startTime.Date);
        return dailyAppointments < 2;
    }
}

}

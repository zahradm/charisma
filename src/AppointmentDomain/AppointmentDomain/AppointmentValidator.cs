using AppointmentDomain.Exceptions;
using AppointmentDomain.Interfaces;
namespace AppointmentDomain;

public class AppointmentValidator
{
    private readonly IWorkingHoursPolicy _workingHoursPolicy;

    public AppointmentValidator(IWorkingHoursPolicy workingHoursPolicy)
    {
        _workingHoursPolicy = workingHoursPolicy;
    }

    public void ValidateAppointmentTime(Doctor doctor, DateTime startTime, int duration)
    {
        if (!_workingHoursPolicy.IsWithinWorkingHours(startTime, duration))
            throw new InvalidAppointmentTimeException("Invalid working hours.");

        if ((doctor.Type == "General" && (duration < 5 || duration > 15)) ||
            (doctor.Type == "Specialist" && (duration < 10 || duration > 30)))
            throw new InvalidAppointmentTimeException("Invalid duration for the selected doctor type.");
    }
}

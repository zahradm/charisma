namespace AppointmentDomain.Interfaces
{
    public interface IWorkingHoursPolicy
    {
        bool IsWithinWorkingHours(DateTime startTime, int duration);
    }
}


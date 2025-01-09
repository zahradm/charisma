using AppointmentDomain.Interfaces;
namespace AppointmentDomain;

public class StandardWorkingHoursPolicy : IWorkingHoursPolicy
{
    public bool IsWithinWorkingHours(DateTime startTime, int duration)
    {
        if (startTime.DayOfWeek == DayOfWeek.Thursday || startTime.DayOfWeek == DayOfWeek.Friday)
            return false;

        if (startTime.TimeOfDay < TimeSpan.FromHours(9) || startTime.TimeOfDay > TimeSpan.FromHours(18))
            return false;

        return true;
    }
}

public class AllOpenWorkingHoursPolicy : IWorkingHoursPolicy
{
    public bool IsWithinWorkingHours(DateTime startTime, int duration)
    {
            
        return true;
    }
}
using AppointmentDomain.Exceptions;
namespace AppointmentDomain;

public class DoctorService
{
    private List<Doctor> Doctors { get; set; } = new List<Doctor>();
    private Dictionary<Doctor, List<(DayOfWeek Day, TimeSpan Start, TimeSpan End)>> DoctorSchedules { get; set; } = new();

    public void AddDoctor(Doctor doctor, List<(DayOfWeek Day, TimeSpan Start, TimeSpan End)> schedule)
    {
        Doctors.Add(doctor);
        DoctorSchedules[doctor] = schedule;
    }

    public List<(DayOfWeek Day, TimeSpan Start, TimeSpan End)> GetSchedule(Doctor doctor)
    {
        if (!DoctorSchedules.ContainsKey(doctor))
            throw new DoctorNotFoundException($"Doctor {doctor.LastName} not found in schedule.");
        return DoctorSchedules[doctor];
    }
}

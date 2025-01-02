using Xunit;

namespace AppointmentTest;

public class ClinicTests
{
    [Fact]
    public void CreateNewAppointment_SuccessfullySchedulesAppointment()
    {
        
        var builder = new ClinicBuilder();
        var clinic = builder.Build();
        var (doctor, patient, startTime, duration) = builder.BuildAppointmentDetails();
        
        var appointment = clinic.CreateNewAppointment(patient, doctor, startTime, duration);
        
        Assert.NotNull(appointment);
        Assert.Equal(doctor, appointment.Doctor);
        Assert.Equal(patient, appointment.Patient);
        Assert.Equal(startTime, appointment.StartTime);
        Assert.Equal(startTime.AddMinutes(duration), appointment.EndTime);
    }

    [Fact]
    public void CreateNewAppointment_FailsWhenDoctorUnavailable()
    {
        
        var builder = new ClinicBuilder()
            .WithDoctorSchedule(new List<(DayOfWeek, TimeSpan, TimeSpan)>
            {
                (DayOfWeek.Tuesday, TimeSpan.FromHours(9), TimeSpan.FromHours(12))
            });
        var clinic = builder.Build();
        var (doctor, patient, startTime, duration) = builder.BuildAppointmentDetails();

        
        var exception = Assert.Throws<Exception>(() => clinic.CreateNewAppointment(patient, doctor, startTime, duration));
        Assert.Equal("Unable to schedule the appointment.", exception.Message);
    }

    [Fact]
    public void CreateNewAppointment_FailsWhenPatientUnavailable()
    {
        
        var builder = new ClinicBuilder();
        var clinic = builder.Build();
        var (doctor, patient, startTime1, duration) = builder.BuildAppointmentDetails();

        clinic.CreateNewAppointment(patient, doctor, startTime1, duration);

        var startTime2 = startTime1.AddMinutes(5);

        
        var exception = Assert.Throws<Exception>(() => clinic.CreateNewAppointment(patient, doctor, startTime2, duration));
        Assert.Equal("Unable to schedule the appointment.", exception.Message);
    }
}

using AppointmentDomain;
using AppointmentDomain.Exceptions;
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

        
        var exception = Assert.Throws<DoctorUnavailableException>(() => clinic.CreateNewAppointment(patient, doctor, startTime, duration));
        Assert.Equal($"Doctor {doctor.LastName} is unavailable.", exception.Message);
    }

    [Fact]
    public void CreateNewAppointment_FailsWhenPatientUnavailable()
    {
        var builder = new ClinicBuilder();
        var clinic = builder.Build();
        var (doctor, patient, startTime1, duration) = builder.BuildAppointmentDetails();

        // First appointment creation
        clinic.CreateNewAppointment(patient, doctor, startTime1, duration);

        // Try to create another appointment that overlaps the first one
        var startTime2 = startTime1.AddMinutes(5);
        var exception = Assert.Throws<AppointmentOverlapException>(() => 
            clinic.CreateNewAppointment(patient, doctor, startTime2, duration)
        );

        // Assert that the correct exception message is thrown
        Assert.Equal($"Patient {patient.FirstName} {patient.LastName} has an overlapping appointment.", exception.Message);
    }

}

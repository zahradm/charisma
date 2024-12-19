using System;
using System.Collections.Generic;
using Xunit;
using AppointmentDomain;

namespace AppointmentTest;

public class Tests
{
    public class ClinicTests
    {
        [Fact]
        public void CreateNewAppointment_SuccessfullySchedulesAppointment()
        {
            // Arrange
            var clinic = new Clinic();
            var doctor = new Doctor("Ali", "Ahmadi", "09123456789", "General");
            var patient = new Patient("Sara", "Rahimi", "09234567890", 30, "Tamin");
            
            // Add doctor and schedule
            clinic.AddDoctor(doctor, new List<(DayOfWeek, TimeSpan, TimeSpan)>
            {
                (DayOfWeek.Monday, TimeSpan.FromHours(9), TimeSpan.FromHours(12))
            });

            // Add patient
            clinic.AddPatient(patient);

            // Appointment details
            var startTime = DateTime.Now.Date.AddDays((int)DayOfWeek.Monday - (int)DateTime.Now.DayOfWeek).AddHours(10);
            var duration = 10;

            // Act
            var appointment = clinic.CreateNewAppointment(patient, doctor, startTime, duration);

            // Assert
            Assert.NotNull(appointment);
            Assert.Equal(doctor, appointment.Doctor);
            Assert.Equal(patient, appointment.Patient);
            Assert.Equal(startTime, appointment.StartTime);
            Assert.Equal(startTime.AddMinutes(duration), appointment.EndTime);
        }

        [Fact]
        public void CreateNewAppointment_FailsWhenDoctorUnavailable()
        {
            // Arrange
            var clinic = new Clinic();
            var doctor = new Doctor("Ali", "Ahmadi", "09123456789", "General");
            var patient = new Patient("Sara", "Rahimi", "09234567890", 30, "Tamin");

            clinic.AddDoctor(doctor, new List<(DayOfWeek, TimeSpan, TimeSpan)>
            {
                (DayOfWeek.Tuesday, TimeSpan.FromHours(9), TimeSpan.FromHours(12))
            });
            clinic.AddPatient(patient);

            // Appointment details outside the schedule
            var startTime = DateTime.Now.Date.AddDays((int)DayOfWeek.Monday - (int)DateTime.Now.DayOfWeek).AddHours(10);
            var duration = 10;

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => clinic.CreateNewAppointment(patient, doctor, startTime, duration));
            Assert.Equal("Unable to schedule the appointment.", exception.Message);
        }

        [Fact]
        public void CreateNewAppointment_FailsWhenPatientUnavailable()
        {
            // Arrange
            var clinic = new Clinic();
            var doctor = new Doctor("Ali", "Ahmadi", "09123456789", "General");
            var patient = new Patient("Sara", "Rahimi", "09234567890", 30, "Tamin");

            clinic.AddDoctor(doctor, new List<(DayOfWeek, TimeSpan, TimeSpan)>
            {
                (DayOfWeek.Monday, TimeSpan.FromHours(9), TimeSpan.FromHours(12))
            });
            clinic.AddPatient(patient);

            // First appointment
            var startTime1 = DateTime.Now.Date.AddDays((int)DayOfWeek.Monday - (int)DateTime.Now.DayOfWeek).AddHours(10);
            clinic.CreateNewAppointment(patient, doctor, startTime1, 10);

            // Second appointment overlapping with the first
            var startTime2 = startTime1.AddMinutes(5);

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => clinic.CreateNewAppointment(patient, doctor, startTime2, 10));
            Assert.Equal("Unable to schedule the appointment.", exception.Message);
        }
    }
}
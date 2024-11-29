using System;
using System.Collections.Generic;

namespace CharismaTask.Src
{
    public class Person
    {
     
        // Properties
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        // Constructor
        public Person(string firstName, string lastName, string phoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
        }
    }

    public class Doctor : Person
{
    
    public string Type { get; set; } // "General" or "Specialist"

    public Doctor(string firstName, string lastName, string phoneNumber, string type)
            : base(firstName, lastName, phoneNumber)
        {
            Type = type;
        }
    public List<Appointment> Appointments { get; set; } = new List<Appointment>();
    public List<Schedule> WeeklySchedule { get; set; } = new List<Schedule>();

    // Check if doctor is scheduled for the given time
    public bool IsScheduled(DateTime appointmentStartTime)
    {
        var schedule = WeeklySchedule.FirstOrDefault(s => s.WeekDay == appointmentStartTime.DayOfWeek);
        return schedule != null &&
               appointmentStartTime.TimeOfDay >= schedule.StartTime &&
               appointmentStartTime.TimeOfDay <= schedule.EndTime;
    }
}

// Define a schedule entry
public class Schedule
{
    public DayOfWeek WeekDay { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
}


    public class Patient : Person
    {
        public int Age { get; set; }
        public string InsuranceName { get; set; }
        public List<Appointment> Appointments { get; set; } = new List<Appointment>();

        public Patient(string firstName, string lastName, string phoneNumber, int age, string insuranceName)
            : base(firstName, lastName, phoneNumber)
        {
            Age = age;
            InsuranceName = insuranceName;
        }

        public bool CanScheduleAppointment(DateTime appointmentDate)
        {
            int count = 0;
            foreach (var appointment in Appointments)
            {
                if (appointment.StartTime.Date == appointmentDate.Date)
                    count++;
            }
            return count < 2; 
        }
    }

}

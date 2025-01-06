namespace AppointmentDomain
{
    public class Clinic
    {
        private List<Doctor> Doctors { get; set; } = new List<Doctor>();
        private List<Patient> Patients { get; set; } = new List<Patient>();
        private List<Appointment> Appointments { get; set; } = new List<Appointment>();
        private Dictionary<Doctor, List<(DayOfWeek Day, TimeSpan Start, TimeSpan End)>> DoctorSchedules { get; set; } = new();

        public void AddDoctor(Doctor doctor, List<(DayOfWeek Day, TimeSpan Start, TimeSpan End)> schedule)
        {
            Doctors.Add(doctor);
            DoctorSchedules[doctor] = schedule;
        }

        public void AddPatient(Patient patient)
        {
            Patients.Add(patient);
        }

        public Appointment CreateNewAppointment(Patient patient, Doctor doctor, DateTime startTime, int duration)
        {   
            if (!ScheduleAppointment(patient, doctor, startTime, duration))
                throw new AppointmentSchedulingException("Unable to schedule the appointment.");
            
            var endTime = startTime.AddMinutes(duration);
            var appointment = new Appointment(doctor, patient, startTime, endTime);
            Appointments.Add(appointment);
            return appointment;
        }

        private bool ScheduleAppointment(Patient patient, Doctor doctor, DateTime startTime, int duration)
        {
            if (!IsValidAppointmentTime(doctor, startTime, duration))
                return false;

            if (!IsDoctorAvailable(doctor, startTime, duration))
                return false;

            if (!IsPatientAvailable(patient, startTime, duration))
                return false;

            return true;
        }

        private bool IsValidAppointmentTime(Doctor doctor, DateTime startTime, int duration)
        {
            if (startTime.DayOfWeek == DayOfWeek.Thursday || startTime.DayOfWeek == DayOfWeek.Friday)
                throw new InvalidAppointmentTimeException("Appointments cannot be scheduled on Thursday or Friday.");
            
            if (startTime.TimeOfDay < TimeSpan.FromHours(9) || startTime.TimeOfDay > TimeSpan.FromHours(18))
                throw new InvalidAppointmentTimeException("Appointments must be scheduled between 9:00 AM and 6:00 PM.");
            
            if ((doctor.Type == "General" && (duration < 5 || duration > 15)) ||
                (doctor.Type == "Specialist" && (duration < 10 || duration > 30)))
                throw new InvalidAppointmentTimeException("Invalid duration for the selected doctor type.");

            return true;
        }

        private bool IsDoctorAvailable(Doctor doctor, DateTime startTime, int duration)
        {
            var endTime = startTime.AddMinutes(duration);
            if (!DoctorSchedules.ContainsKey(doctor))
                throw new DoctorNotFoundException($"Doctor {doctor.LastName} not found in schedule.");

            var schedule = DoctorSchedules[doctor]
                .FirstOrDefault(s => s.Day == startTime.DayOfWeek && 
                                     startTime.TimeOfDay >= s.Start &&
                                     endTime.TimeOfDay <= s.End);

            if (schedule == default)
                throw new DoctorUnavailableException($"Doctor {doctor.LastName} is unavailable at the requested time.");

            int maxOverlaps = doctor.Type == "General" ? 2 : 3;
            int overlappingAppointments = Appointments.Count(a =>
                a.Doctor == doctor && 
                a.StartTime < endTime && 
                a.EndTime > startTime);

            if (overlappingAppointments >= maxOverlaps)
                throw new DoctorOverbookedException($"Doctor {doctor.LastName} is overbooked at the requested time.");

            return true;
        }

        private bool IsPatientAvailable(Patient patient, DateTime startTime, int duration)
        {
            var endTime = startTime.AddMinutes(duration);
            int dailyAppointments = Appointments.Count(a =>
                a.Patient == patient && a.StartTime.Date == startTime.Date);

            if (dailyAppointments >= 2)
                throw new PatientLimitReachedException($"Patient {patient.FirstName} {patient.LastName} has already reached the maximum number of appointments for the day.");

            bool hasOverlap = Appointments.Any(a =>
                a.Patient == patient && a.StartTime < endTime && a.EndTime > startTime);

            if (hasOverlap)
                throw new AppointmentOverlapException($"Patient {patient.FirstName} {patient.LastName} has an overlapping appointment.");

            return true;
        }
    }

    // Custom Exception Classes
    public class AppointmentSchedulingException : Exception
    {
        public AppointmentSchedulingException(string message) : base(message) { }
    }

    public class InvalidAppointmentTimeException : Exception
    {
        public InvalidAppointmentTimeException(string message) : base(message) { }
    }

    public class DoctorNotFoundException : Exception
    {
        public DoctorNotFoundException(string message) : base(message) { }
    }

    public class DoctorUnavailableException : Exception
    {
        public DoctorUnavailableException(string message) : base(message) { }
    }

    public class DoctorOverbookedException : Exception
    {
        public DoctorOverbookedException(string message) : base(message) { }
    }

    public class PatientLimitReachedException : Exception
    {
        public PatientLimitReachedException(string message) : base(message) { }
    }

    public class AppointmentOverlapException : Exception
    {
        public AppointmentOverlapException(string message) : base(message) { }
    }
}

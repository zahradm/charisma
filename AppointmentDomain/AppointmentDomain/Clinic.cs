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
                throw new Exception("Unable to schedule the appointment.");
            
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
            //Console.WriteLine(!(startTime.DayOfWeek == DayOfWeek.Thursday || startTime.DayOfWeek == DayOfWeek.Friday));
            if (startTime.DayOfWeek == DayOfWeek.Thursday || startTime.DayOfWeek == DayOfWeek.Friday)
                return false;
            
            Console.WriteLine(!(startTime.TimeOfDay < TimeSpan.FromHours(9) || startTime.TimeOfDay > TimeSpan.FromHours(18)));
            if (startTime.TimeOfDay < TimeSpan.FromHours(9) || startTime.TimeOfDay > TimeSpan.FromHours(18))
                return false;
            Console.WriteLine("test");
            if ((doctor.Type == "General" && (duration < 5 || duration > 15)) ||
                (doctor.Type == "Specialist" && (duration < 10 || duration > 30)))
                return false;

            return true;
        }

        private bool IsDoctorAvailable(Doctor doctor, DateTime startTime, int duration)
        {
            Console.WriteLine($"Checking doctor availability: {doctor.LastName}, {startTime}, Duration: {duration}");

            var endTime = startTime.AddMinutes(duration);
            if (!DoctorSchedules.ContainsKey(doctor))
                return false;

            var schedule = DoctorSchedules[doctor]
                .FirstOrDefault(s => s.Day == startTime.DayOfWeek && 
                                     startTime.TimeOfDay >= s.Start &&
                                     endTime.TimeOfDay <= s.End);

            if (schedule == default)
                return false;

            int maxOverlaps = doctor.Type == "General" ? 2 : 3;
            int overlappingAppointments = Appointments.Count(a =>
                a.Doctor == doctor && 
                a.StartTime < endTime && 
                a.EndTime > startTime);

            return overlappingAppointments < maxOverlaps;
        }

        private bool IsPatientAvailable(Patient patient, DateTime startTime, int duration)
        {
            var endTime = startTime.AddMinutes(duration);
            int dailyAppointments = Appointments.Count(a =>
                a.Patient == patient && a.StartTime.Date == startTime.Date);

            if (dailyAppointments >= 2)
                return false;

            bool hasOverlap = Appointments.Any(a =>
                a.Patient == patient && a.StartTime < endTime && a.EndTime > startTime);

            return !hasOverlap;
        }
    }
}

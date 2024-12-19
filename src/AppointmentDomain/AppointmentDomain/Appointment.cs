namespace AppointmentDomain
{
    public class Appointment
    {
        public Doctor Doctor { get; private set; }
        public Patient Patient { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }

        public Appointment(Doctor doctor, Patient patient, DateTime startTime, DateTime endTime)
        {
            Doctor = doctor;
            Patient = patient;
            StartTime = startTime;
            EndTime = endTime;
        }
    }
}
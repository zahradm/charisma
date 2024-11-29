namespace CharismaTask.Src
{
    public class Appointment
    {
        public Doctor Doctor { get; set; }
        public Patient Patient { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Money ConsultationFee { get; set; }

        public Appointment(Doctor doctor, Patient patient, DateTime startTime, DateTime endTime, Money consultationFee)
        {
            Doctor = doctor;
            Patient = patient;
            StartTime = startTime;
            EndTime = endTime;
            ConsultationFee = consultationFee;
        }
    }

    public record Money(string Symbol, decimal Value)
    {

    }
}

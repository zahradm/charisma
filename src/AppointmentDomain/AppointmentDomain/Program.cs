namespace AppointmentDomain;

class Program
{
    static void Main(string[] args)
    {
        var clinic = new Clinic(new StandardWorkingHoursPolicy());
        var doctor = new Doctor("Ali", "Ahmadi", "09123456789", "General");
        var patient = new Patient("Sara", "Rahimi", "09234567890", 30, "Tamin");

        clinic.AddDoctor(doctor, new List<(DayOfWeek, TimeSpan, TimeSpan)>
        {
            (DayOfWeek.Monday, TimeSpan.FromHours(9), TimeSpan.FromHours(12))
        });

        clinic.AddPatient(patient);
        var appointmentDate = new DateTime(2024, 12, 23, 10, 0, 0);
        try
        {    
            Console.WriteLine($"Date {DateTime.Now.AddDays(3).AddHours(15)} for Doctor {doctor.LastName}.");

            var appointment = clinic.CreateNewAppointment(patient, doctor, appointmentDate, 10);
            Console.WriteLine($"Appointment scheduled for {appointment.Patient.FirstName} {appointment.Patient.LastName} with Dr. {appointment.Doctor.LastName}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
namespace AppointmentDomain;

class Program
{
    static void Main(string[] args)
    {
        // Create instances of the services
        var doctorService = new DoctorService();
        var patientService = new PatientService();
        var availabilityChecker = new AvailabilityChecker(doctorService, new List<Appointment>());
        var appointmentValidator = new AppointmentValidator(new StandardWorkingHoursPolicy());
        var appointmentService = new AppointmentService(appointmentValidator, availabilityChecker);

        // Create the clinic instance using dependency injection
        var clinic = new Clinic(doctorService, patientService, appointmentService);

        // Create a doctor and a patient
        var doctor = new Doctor("Ali", "Ahmadi", "09123456789", "General");
        var patient = new Patient("Sara", "Rahimi", "09234567890", 30, "Tamin");

        // Add doctor and patient to the clinic
        clinic.AddDoctor(doctor, new List<(DayOfWeek, TimeSpan, TimeSpan)>
        {
            (DayOfWeek.Monday, TimeSpan.FromHours(9), TimeSpan.FromHours(12))
        });

        clinic.AddPatient(patient);

        // Define the appointment date
        var appointmentDate = new DateTime(2024, 12, 23, 10, 0, 0);

        try
        {
            // Attempt to schedule the appointment
            Console.WriteLine($"Trying to schedule an appointment on {appointmentDate} for Doctor {doctor.LastName}.");
            var appointment = clinic.CreateNewAppointment(patient, doctor, appointmentDate, 10);
            Console.WriteLine(
                $"Appointment scheduled for {appointment.Patient.FirstName} {appointment.Patient.LastName} with Dr. {appointment.Doctor.LastName} at {appointment.StartTime}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
namespace AppointmentDomain;

public class PatientService
{
    private List<Patient> Patients { get; set; } = new List<Patient>();

    public void AddPatient(Patient patient)
    {
        Patients.Add(patient);
    }
}

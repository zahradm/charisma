namespace AppointmentDomain;

public class Patient : Person
{
    public int Age { get; private set; }
    public string InsuranceName { get; private set; }

    public Patient(string firstName, string lastName, string phoneNumber, int age, string insuranceName)
        : base(firstName, lastName, phoneNumber)
    {
        Age = age;
        InsuranceName = insuranceName;
    }
}
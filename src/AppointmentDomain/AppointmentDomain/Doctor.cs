namespace AppointmentDomain;

public class Doctor : Person
{
    public string Type { get; private set; } // "General" or "Specialist"

    public Doctor(string firstName, string lastName, string phoneNumber, string type)
        : base(firstName, lastName, phoneNumber)
    {
        Type = type;
    }
}
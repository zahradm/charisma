namespace AppointmentDomain.Exceptions
{
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
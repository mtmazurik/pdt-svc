namespace pdt.svc.services.exceptions
{
    public class bogusException : Exception      // read more on user-defined exceptions here: https://docs.microsoft.com/en-us/dotnet/standard/exceptions/how-to-create-user-defined-exceptions
    {
        public bogusException(string message) : base(message) { }
    }
}

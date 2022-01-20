namespace pdt.svc.services.exceptions
{
    public class searchException : Exception      // read more on user-defined exceptions here: https://docs.microsoft.com/en-us/dotnet/standard/exceptions/how-to-create-user-defined-exceptions
    {
        public searchException(string message, Exception inner) : base(message, inner) { }
    }
}

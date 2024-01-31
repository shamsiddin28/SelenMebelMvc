namespace SelenMebel.Service.Exceptions;

public class SelenMebelException : Exception
{
    public int StatusCode { get; set; }

    public SelenMebelException(int code, string message) : base(message)
    {
        StatusCode = code;
    }
}

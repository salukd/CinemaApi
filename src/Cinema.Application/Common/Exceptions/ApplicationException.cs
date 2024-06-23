namespace Cinema.Application.Common.Exceptions;

public class ApplicationException: Exception
{
    public string Code { get; }
    public string Description { get; }

    public ApplicationException(string code, string description = "")
        : base("Application exception have occurred.")
    {
        Code = code;
        Description = description;
    }
    
    
}
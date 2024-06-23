namespace Cinema.Application.Common.Exceptions;

public class ConflictException : Exception
{
    public string Code { get; }
    public string Description { get; }

    public ConflictException(string code, string description = "")
        : base("Conflict exception have occurred.")
    {
        Code = code;
        Description = description;
    }
}
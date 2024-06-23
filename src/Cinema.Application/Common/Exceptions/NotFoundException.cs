namespace Cinema.Application.Common.Exceptions;

public class NotFoundException : Exception
{
    public string Code { get; }
    public string Description { get; }

    public NotFoundException(string code, string description = "")
        : base("Not found exception have occurred.")
    {
        Code = code;
        Description = description;
    }
}
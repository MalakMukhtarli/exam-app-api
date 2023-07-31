namespace ExamApp.API.Exceptions;

public class DuplicateConflictException : ApplicationException
{
    public DuplicateConflictException() : base()
    {
    }

    public DuplicateConflictException(string message)
        : base(message)
    {
    }
}
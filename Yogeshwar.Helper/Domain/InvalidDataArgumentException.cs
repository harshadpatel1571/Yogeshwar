namespace Yogeshwar.Helper.Domain;

internal class InvalidDataArgumentException : Exception
{
    public InvalidDataArgumentException(string message) : base(message)
    {
    }
}
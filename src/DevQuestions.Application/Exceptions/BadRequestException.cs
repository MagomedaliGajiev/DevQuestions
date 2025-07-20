using System.Text.Json;
using Shared;

namespace DevQuestions.Application.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(Error[] errors)
        : base(JsonSerializer.Serialize(errors))
    {
    }
}

using System.Net;
using BuildingBlocks.Core.Exception.Types;

namespace Rancho.Services.Identity.Identity.Exceptions;

public class PasswordIsInvalidException : AppException
{
    public PasswordIsInvalidException(string message)
        : base(message, HttpStatusCode.Forbidden) { }
}

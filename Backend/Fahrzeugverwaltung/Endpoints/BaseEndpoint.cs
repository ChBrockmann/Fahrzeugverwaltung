using Model.User;

namespace Fahrzeugverwaltung.Endpoints;

public class BaseEndpoint<TRequest, TResponse> : Endpoint<TRequest, TResponse> where TRequest : notnull
{
    public UserModel UserFromContext => HttpContext.Items["User"] as UserModel ?? throw new InvalidOperationException("User not found");
}
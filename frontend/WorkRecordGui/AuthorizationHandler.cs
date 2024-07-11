using System.Net.Http.Headers;
using WorkRecordGui;

public class AuthorizationHandler : DelegatingHandler
{
    private readonly Session _session;

    public AuthorizationHandler(Session session)
    {
        _session = session;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _session.Token);
        return base.SendAsync(request, cancellationToken);
    }
}